using System;
using System.Collections.Generic;
using System.Linq;
using InsaneScatterbrain.DataStructures;
using InsaneScatterbrain.Extensions;
using InsaneScatterbrain.ScriptGraph;
using UnityEngine;

[ScriptNode("Recreate 2nd Row Connections", "Custom/Samples/Overworld"), Serializable]
public class RecreateSecondRowConnectionsConnectionsNode : ProcessorNode
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
        
        var connections = instanceProvider.Get<List<Pair<Vector2Int>>>();
        var pointsByRow = new Dictionary<int, HashSet<Vector2Int>>();
        var connectionsByPoint = new Dictionary<Vector2Int, HashSet<Pair<Vector2Int>>>();
        
        var originalConnections = connectionsIn.Get<Pair<Vector2Int>[]>();
        
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
        
        // Remove all the outgoing connections from the points in the second row.
        // They're going to be recreated to make sure that they all have two outgoing connections.
        foreach (var point in pointsByRow[1])
        {
            var pointConnections = connectionsByPoint[point];
            
            PointConnections.FilterByDirection(point, pointConnections, ref incomingConnections, ref outgoingConnections);
            
            foreach (var connection in outgoingConnections)
            {
                connections.Remove(connection);
            }
        }
        
        // Now we create new connections between the points on the second row.
        // There should only be two and we want two outgoing connections for both of them.
        // Connect the first point to two left-most points on the row above it and connect the second point to the two
        // right-most points on the row above it.
        
        var points = instanceProvider.Get<List<Vector2Int>>();
        var sortedPoints = instanceProvider.Get<List<Vector2Int>>();
        
        // The connections for each row are not in order. We need to sort them by x position and remove duplicates.
        foreach (var row in pointsByRow)
        {
            points.Clear();
            sortedPoints.Clear();
            
            points.AddRange(row.Value);
            
            // Sort all the points on the row from left to right.
            while (points.Count > 0)
            {
                var leftMostPoint = Vector2Int.zero;
                var leftMostX = float.MaxValue;
            
                // Find the left-most point remaining.
                foreach (var point in points)
                {
                    if (point.x >= leftMostX) continue;
            
                    leftMostPoint = point;
                    leftMostX = point.x;
                }
            
                sortedPoints.Add(leftMostPoint);
                points.Remove(leftMostPoint);
            }
            
            // Replace the unsorted points with the sorted points.
            row.Value.Clear();
            row.Value.AddRange(sortedPoints);
        }
        
        // Loop through the first two points of the third row.
        for (var i = 0; i < 2; i++)
        {
            // Create a connection between this point and the first point on the second row
            var point = pointsByRow[2].ToList()[i];
            var otherPoint = pointsByRow[1].ToList()[0];
            
            var connection = new Pair<Vector2Int>(point, otherPoint);
            connections.Add(connection);
        }
        
        // Now loop through the last two points of the third row.
        for (var i = pointsByRow[2].Count - 1; i > pointsByRow[2].Count - 3; i--)
        {
            // Create a connection between this point and the last point on the second row
            var point = pointsByRow[2].ToList()[i];
            var otherPoint = pointsByRow[1].ToList()[1];
            
            var connection = new Pair<Vector2Int>(point, otherPoint);
            connections.Add(connection);
        }
        
        connectionsArray = connections.ToArray();
        
        connectionsOut.Set(() => connectionsArray);
    }
}