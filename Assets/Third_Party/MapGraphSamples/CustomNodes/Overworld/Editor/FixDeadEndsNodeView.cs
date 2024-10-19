using InsaneScatterbrain.MapGraph.Editor;
using InsaneScatterbrain.ScriptGraph;
using InsaneScatterbrain.ScriptGraph.Editor;
using UnityEngine;

[ScriptNodeView(typeof(FixDeadEndsNode))]
public class FixDeadEndsNodeView : ScriptNodeView
{
    public FixDeadEndsNodeView(IScriptNode node, ScriptGraphView graphView) : base(node, graphView)
    {
        this.AddPreview<FixDeadEndsNode>(GetPreviewTexture);
    }

    private Texture2D GetPreviewTexture(FixDeadEndsNode node) =>
        ConnectionsNodeView.GetConnectionsTexture(node.Connections);
}