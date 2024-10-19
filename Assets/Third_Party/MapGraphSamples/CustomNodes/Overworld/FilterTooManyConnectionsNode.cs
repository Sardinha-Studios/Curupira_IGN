using System;
using System.Collections.Generic;
using InsaneScatterbrain.DataStructures;
using InsaneScatterbrain.Extensions;
using InsaneScatterbrain.ScriptGraph;
using InsaneScatterbrain.Services;
using UnityEngine;

[ScriptNode("Filter Invalid Connections (Too Many)", "Custom/Samples/Overworld"), Serializable]
public class FilterTooManyConnectionsNode : ProcessorNode
{
    [InPort("Connections", typeof(Pair<Vector2Int>[]), true), SerializeReference]
    private InPort connectionsIn = null;
    
    [InPort("Bounds", typeof(Vector2Int), true), SerializeReference]
    private InPort boundsIn = null;
    
    [InPort("Num. Rows", typeof(int), true), SerializeReference]
    private InPort numRowsIn = null;
    
    
    [OutPort("Connections", typeof(Pair<Vector2Int>[])), SerializeReference]
    private OutPort connectionsOut = null;
    
    private Pair<Vector2Int>[] connectionsArray;
    private List<Pair<Vector2Int>> potentialConnectionsToRemove;
    private List<Pair<Vector2Int>> otherPointIncomingConnections;
    private List<Pair<Vector2Int>> otherPointOutgoingConnections;
    private Dictionary<Vector2Int, HashSet<Pair<Vector2Int>>> connectionsByPoint;
    private List<Pair<Vector2Int>> connections;
    
#if UNITY_EDITOR
    public Pair<Vector2Int>[] Connections => connectionsArray;
#endif
    
    protected override void OnProcess()
    {
        var instanceProvider = Get<IInstanceProvider>();
        
        connections = instanceProvider.Get<List<Pair<Vector2Int>>>();
        var pointsByRow = instanceProvider.Get<Dictionary<int, HashSet<Vector2Int>>>();
        connectionsByPoint = instanceProvider.Get<Dictionary<Vector2Int, HashSet<Pair<Vector2Int>>>>();
        
        var originalConnections = connectionsIn.Get<Pair<Vector2Int>[]>();
        var bounds = boundsIn.Get<Vector2Int>();
        var numRows = numRowsIn.Get<int>();
        
        connections.AddRange(originalConnections);
        
        var rowHeight = bounds.y / numRows;
        
        // Get all the points from the connections and store them with their respective row and column.
        foreach (var connection in originalConnections)
        {
            var pointA = connection.First;
            var pointB = connection.Second;
            
            if (!pointsByRow.ContainsKey(pointA.y / rowHeight))
            {
                pointsByRow.Add(pointA.y / rowHeight, instanceProvider.Get<HashSet<Vector2Int>>());
            }
            
            if (!pointsByRow.ContainsKey(pointB.y / rowHeight))
            {
                pointsByRow.Add(pointB.y / rowHeight, instanceProvider.Get<HashSet<Vector2Int>>());
            }
            
            pointsByRow[pointA.y / rowHeight].Add(pointA);
            pointsByRow[pointB.y / rowHeight].Add(pointB);
            
            // Also store the connection with each point it's connected to.
            if (!connectionsByPoint.ContainsKey(pointA))
            {
                connectionsByPoint.Add(pointA, instanceProvider.Get<HashSet<Pair<Vector2Int>>>());
            }
            
            if (!connectionsByPoint.ContainsKey(pointB))
            {
                connectionsByPoint.Add(pointB, instanceProvider.Get<HashSet<Pair<Vector2Int>>>());
            }
            
            connectionsByPoint[pointA].Add(connection);
            connectionsByPoint[pointB].Add(connection);
        }
        
        var incomingConnections = instanceProvider.Get<List<Pair<Vector2Int>>>();
        var outgoingConnections = instanceProvider.Get<List<Pair<Vector2Int>>>();
        
        potentialConnectionsToRemove = instanceProvider.Get<List<Pair<Vector2Int>>>();
        
        otherPointIncomingConnections = instanceProvider.Get<List<Pair<Vector2Int>>>();
        otherPointOutgoingConnections = instanceProvider.Get<List<Pair<Vector2Int>>>();
        
        var pointsList = instanceProvider.Get<List<Vector2Int>>();
        
        // Go through all the rows checking for points that have too many connections.
        // They should only have 2 incoming and 2 outgoing connections.
        for (var row = 1; row < numRows - 1; row++)
        {
            var points = pointsByRow[row];
            
            pointsList.Clear();
            pointsList.AddRange(points);
            pointsList.Shuffle(Get<Rng>());
            
            // Go through each of a row's points and check if there are connections that need to be removed.
            foreach (var point in pointsList)
            {
                potentialConnectionsToRemove.Clear();

                // Get the point's connections split into incoming and outgoing connections.
                PointConnections.FilterByDirection(point, connectionsByPoint[point], ref incomingConnections, ref outgoingConnections);

                // If the point has more than 2 outgoing connections, we need to remove some.
                if (outgoingConnections.Count > 2)
                {
                    RemoveExceedingConnections(point, outgoingConnections, ConnectionType.Outgoing);
                }

                // Do the same for the incoming connections.
                if (incomingConnections.Count > 2)
                {
                    RemoveExceedingConnections(point, incomingConnections, ConnectionType.Incoming);
                }
            }
        }
        
        connectionsArray = connections.ToArray();
        
        connectionsOut.Set(() => connectionsArray);
    }

    private void RemoveExceedingConnections(Vector2Int point, List<Pair<Vector2Int>> pointConnections, ConnectionType connectionType)
    {
        var rng = Get<Rng>();
        
        potentialConnectionsToRemove.Clear();
                
        // Go through all the point's connections and check which ones are save to remove.
        foreach (var connection in pointConnections)
        {
            // We get the other point in the connection, so we can check its connections.
            var otherPoint = connection.First == point ? connection.Second : connection.First;
                    
            // We get the other point's connections split into incoming and outgoing connections.
            PointConnections.FilterByDirection(otherPoint, connectionsByPoint[otherPoint], ref otherPointIncomingConnections, ref otherPointOutgoingConnections);
            
            // If we're checking the incoming connections, that means for the other point these are outgoing connections.
            var otherConnections = connectionType == ConnectionType.Incoming ? otherPointOutgoingConnections : otherPointIncomingConnections;

            // If the other point has less than 2 connections, we can't safely remove a connection,
            // as that point would be left without any connections, creating a dead end.
            if (otherConnections.Count < 2) continue;
                    
            potentialConnectionsToRemove.Add(connection);
        }
                
        // Shuffle the list of potential connections to remove, so they get removed in a more random pattern.
        potentialConnectionsToRemove.Shuffle(rng);
                
        // Keep removing connections until the number of connections is no longer exceeding the maximum number of connections of 2.
        while (pointConnections.Count > 2)
        {
            if (potentialConnectionsToRemove.Count == 0)
            {
                // This should never happen, but if it does we want to know about it.
                Debug.LogError("No more potential connections to remove.");
                break;
            }
            
            // Grab the first connection from the list of potential connections to remove.
            var connectionToRemove = potentialConnectionsToRemove[0];
            
            // And remove it from all the lists.
            potentialConnectionsToRemove.RemoveAt(0);
            pointConnections.Remove(connectionToRemove);
            connections.Remove(connectionToRemove);
        }
    }
    
    private enum ConnectionType
    {
        Incoming,
        Outgoing
    }
}