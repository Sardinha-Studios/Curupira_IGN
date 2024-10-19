using InsaneScatterbrain.MapGraph.Editor;
using InsaneScatterbrain.ScriptGraph;
using InsaneScatterbrain.ScriptGraph.Editor;
using UnityEngine;

[ScriptNodeView(typeof(RecreateSecondRowConnectionsConnectionsNode))]
public class RecreateSecondRowConnectionsConnectionsNodeView : ScriptNodeView
{
    public RecreateSecondRowConnectionsConnectionsNodeView(IScriptNode node, ScriptGraphView graphView) : base(node, graphView)
    {
        this.AddPreview<RecreateSecondRowConnectionsConnectionsNode>(GetPreviewTexture);
    }

    private Texture2D GetPreviewTexture(RecreateSecondRowConnectionsConnectionsNode node) =>
        ConnectionsNodeView.GetConnectionsTexture(node.Connections);
}