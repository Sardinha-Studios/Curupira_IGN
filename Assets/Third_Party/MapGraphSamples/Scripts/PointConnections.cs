using System.Collections.Generic;
using InsaneScatterbrain.DataStructures;
using UnityEngine;

public static class PointConnections
{
    /// <summary>
    /// Get connections for the given point and split them into incoming and outgoing connections.
    /// </summary>
    /// <param name="point">The point that we want to get the connections for.</param>
    /// <param name="connections">The connections that we want to filter.</param>
    /// <param name="incomingConnections">The incoming connections.</param>
    /// <param name="outgoingConnections">The outgoing connections.</param>
    public static void FilterByDirection(
        Vector2Int point,
        HashSet<Pair<Vector2Int>> connections, 
        ref List<Pair<Vector2Int>> incomingConnections, 
        ref List<Pair<Vector2Int>> outgoingConnections)
    {
        // Make sure the lists used to return the incoming and outgoing connections are empty.
        incomingConnections.Clear();
        outgoingConnections.Clear();
        
        foreach (var connection in connections)
        {
            // Get the other point in the connection.
            var otherPoint = connection.First == point ? connection.Second : connection.First;
            
            if (otherPoint.y > point.y)
            {
                // If it's above the current point, it's an outgoing connection.
                outgoingConnections.Add(connection);
            }
            else
            {
                // Otherwise, it's an incoming connection.
                incomingConnections.Add(connection);
            }
        }
    }
}