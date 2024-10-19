using InsaneScatterbrain.MapGraph.Editor;
using InsaneScatterbrain.ScriptGraph;
using InsaneScatterbrain.ScriptGraph.Editor;
using UnityEngine;

[ScriptNodeView(typeof(FilterTooManyConnectionsNode))]
public class FilterTooManyConnectionsNodeView : ScriptNodeView
{
    public FilterTooManyConnectionsNodeView(IScriptNode node, ScriptGraphView graphView) : base(node, graphView)
    {
        this.AddPreview<FilterTooManyConnectionsNode>(GetPreviewTexture);
    }

    private Texture2D GetPreviewTexture(FilterTooManyConnectionsNode node) =>
        ConnectionsNodeView.GetConnectionsTexture(node.Connections);
}