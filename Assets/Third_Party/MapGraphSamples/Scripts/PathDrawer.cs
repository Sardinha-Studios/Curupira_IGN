using System.Collections.Generic;
using InsaneScatterbrain.DataStructures;
using InsaneScatterbrain.ScriptGraph;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    [SerializeField] private ScriptGraphRunner runner = null;
    [SerializeField] private LineRenderer lineRendererPrefab = null;

    private void Start()
    {
        runner.OnProcessed += OnProcessed;
    }

    private void OnProcessed(IReadOnlyDictionary<string, object> result)
    {
        var paths = (Pair<Vector2Int>[]) result["Paths"];

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var path in paths)
        {
            var start = (Vector2) path.First;
            var end = (Vector2) path.Second;
            
            // Create a line renderer.
            var lineRenderer = Instantiate(lineRendererPrefab, transform);
            lineRenderer.positionCount = 2;

            var distanceFromPoints = 1.25f;
            
            var direction = (end - start).normalized;
            start += direction * distanceFromPoints;
            end -= direction * distanceFromPoints;
            
            lineRenderer.SetPosition(0, new Vector3(start.x, start.y, 0));
            lineRenderer.SetPosition(1, new Vector3(end.x, end.y, 0f));
            
            // Add the children to the parent.
            lineRenderer.transform.SetParent(transform);
        }
    }
}