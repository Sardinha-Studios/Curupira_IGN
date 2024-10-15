using System;
using System.Collections.Generic;
using InsaneScatterbrain.DataStructures;
using InsaneScatterbrain.ScriptGraph;
using UnityEngine;

[ScriptNode("Filter Invalid Connections (Columns & Rows)", "Custom/Samples/Overworld"), Serializable]
public class FilterInvalidConnectionsNode : ProcessorNode
{
    [InPort("Connections", typeof(Pair<Vector2Int>[]), true), SerializeReference]
    private InPort connectionsIn = null;
    
    [InPort("Bounds", typeof(Vector2Int), true), SerializeReference]
    private InPort boundsIn = null;
    
    [InPort("Num. Rows", typeof(int), true), SerializeReference]
    private InPort numRowsIn = null;
    
    [InPort("Num. Columns", typeof(int), true), SerializeReference]
    private InPort numColumnsIn = null;
    
    
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
        var pointRows = instanceProvider.Get<Dictionary<Vector2Int, int>>();
        var pointColumns = instanceProvider.Get<Dictionary<Vector2Int, int>>();
        
        var originalConnections = connectionsIn.Get<Pair<Vector2Int>[]>();
        var bounds = boundsIn.Get<Vector2Int>();
        var numRows = numRowsIn.Get<int>();
        var numColumns = numColumnsIn.Get<int>();
        
        connections.AddRange(originalConnections);
        
        var rowHeight = bounds.y / numRows;
        var columnWidth = bounds.x / numColumns;

        // We first need to figure out which row and column each point is in.
        // Get all the points from the connections and store them with their respective row and column.
        foreach (var connection in originalConnections)
        {
            var pointA = connection.First;
            var pointB = connection.Second;

            if (!pointRows.ContainsKey(pointA))
            {
                pointRows.Add(pointA, pointA.y / rowHeight);
            }
            
            if (!pointRows.ContainsKey(pointB))
            {
                pointRows.Add(pointB, pointB.y / rowHeight);
            }
            
            
            if (!pointColumns.ContainsKey(pointA))
            {
                pointColumns.Add(pointA, pointA.x / columnWidth);
            }
            
            if (!pointColumns.ContainsKey(pointB))
            {
                pointColumns.Add(pointB, pointB.x / columnWidth);
            }
        }
        
        // Filter any connections that are considered invalid. Connections are invalid when:
        // - The start and end point are in the same row. (Horizontal connections)
        // - The start and end points are more than a rows apart on any row. So there's no way to skip a row.
        // - The start and end points are more than a columns apart on rows that are not the start or end row.
        foreach (var connection in originalConnections)
        {
            var pointA = connection.First;
            var pointB = connection.Second;

            // Get the row number for each point.
            var rowA = pointRows[pointA];
            var rowB = pointRows[pointB];

            // And the column.
            var columnA = pointColumns[pointA];
            var columnB = pointColumns[pointB];
            
            // Calculate the difference between the rows and columns between the two points.
            var rowDifference = Mathf.Abs(rowA - rowB);
            var columnDifference = Mathf.Abs(columnA - columnB);
            
            // Check if either of the points are in the start or end row.
            var isConnectingStartOrEndRow = 
                rowA == 0 || rowA == numRows - 1 || 
                rowB == 0 || rowB == numRows - 1;
            
            // If the difference is exactly one it means that the points are exactly one row apart, which is what we want.
            if (rowDifference != 1)
            {
                // So otherwise we remove the connection.
                connections.Remove(connection);
            }
            
            // The points can be either in the same column or one column apart, with the exception of the start and end row.
            if (columnDifference > 1 && !isConnectingStartOrEndRow)
            {
                // So otherwise we remove the connection.
                connections.Remove(connection);
            }
        }
        
        connectionsArray = connections.ToArray();
        
        connectionsOut.Set(() => connectionsArray);
    }
}