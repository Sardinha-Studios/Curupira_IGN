using InsaneScatterbrain.MapGraph.Editor;
using InsaneScatterbrain.ScriptGraph;
using InsaneScatterbrain.ScriptGraph.Editor;
using UnityEngine;

[ScriptNodeView(typeof(FilterInvalidConnectionsNode))]
public class FilterInvalidConnectionsNodeView : ScriptNodeView
{
    public FilterInvalidConnectionsNodeView(IScriptNode node, ScriptGraphView graphView) : base(node, graphView)
    {
        this.AddPreview<FilterInvalidConnectionsNode>(GetPreviewTexture);
    }

    private Texture2D GetPreviewTexture(FilterInvalidConnectionsNode node) =>
        ConnectionsNodeView.GetConnectionsTexture(node.Connections);
}