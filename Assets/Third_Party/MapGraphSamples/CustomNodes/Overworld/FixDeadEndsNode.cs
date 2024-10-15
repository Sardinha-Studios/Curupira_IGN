using System;
using System.Collections.Generic;
using InsaneScatterbrain.DataStructures;
using InsaneScatterbrain.ScriptGraph;
using UnityEngine;

[ScriptNode("Fix Dead Ends", "Custom/Samples/Overworld"), Serializable]
public class FixDeadEndsNode : ProcessorNode
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
    
#if UNITY_EDITOR
    public Pair<Vector2Int>[] Connections => connectionsArray;
#endif

    protected override void OnProcess()
    {
        var instanceProvider = Get<IInstanceProvider>();
        
        var originalConnections = connectionsIn.Get<Pair<Vector2Int>[]>();

        var connections = instanceProvider.Get<List<Pair<Vector2Int>>>();
        var pointsByRow = instanceProvider.Get<Dictionary<int, HashSet<Vector2Int>>>();
        var connectionsByPoint = instanceProvider.Get<Dictionary<Vector2Int, HashSet<Pair<Vector2Int>>>>();
        
        connections.AddRange(originalConnections);
        
        var bounds = boundsIn.Get<Vector2Int>();
        var numRows = numRowsIn.Get<int>();
        
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
        
        // Go through all the rows, skipping the first and last row.
        for (var row = 1; row < numRows - 1; row++)
        {
            var points = pointsByRow[row];
            
            // Go through each of a row's points and check if they're dead ends.
            foreach (var point in points)
            {
                PointConnections.FilterByDirection(point, connectionsByPoint[point], ref incomingConnections, ref outgoingConnections);
                
                // If there's no outgoing connections, it means there's no way to get to the next row.
                // There if there's no connections at all, we create one between the current point and the closest point on the row above.
                if (outgoingConnections.Count == 0)
                {
                    // Keep track of the closest point.
                    var closestPoint = Vector2Int.zero;
                    var closestDistance = float.MaxValue;
                    
                    foreach (var otherPoint in pointsByRow[row + 1])
                    {
                        var distance = Vector2Int.Distance(point, otherPoint);

                        // If the current point is not closer than the closest point, we move on to the next one.
                        if (distance >= closestDistance) continue;
                        
                        closestPoint = otherPoint;
                        closestDistance = distance;
                    }
                    
                    connections.Add(new Pair<Vector2Int>(point, closestPoint));
                }
                
                // Do the same thing for incoming connections.
                if (incomingConnections.Count == 0)
                {
                    var closestPoint = Vector2Int.zero;
                    var closestDistance = float.MaxValue;
                    
                    foreach (var otherPoint in pointsByRow[row - 1])
                    {
                        var distance = Vector2Int.Distance(point, otherPoint);

                        if (distance >= closestDistance) continue;
                        
                        closestPoint = otherPoint;
                        closestDistance = distance;
                    }
                    
                    connections.Add(new Pair<Vector2Int>(point, closestPoint));
                }
            }
        }
        
        connectionsArray = connections.ToArray();
        
        connectionsOut.Set(() => connectionsArray);
    }
}