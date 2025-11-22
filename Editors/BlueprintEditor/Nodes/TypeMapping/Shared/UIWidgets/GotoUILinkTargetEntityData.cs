using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.UIWidgets
{
    public class GotoUILinkTargetEntityData : EntityNode
    {
        public override string ObjectType => "GotoUILinkTargetEntityData";

        public override string ToolTip => "This node is used to listen to certain events that trigger UI screen's logic";

        public override void OnCreation()
        {
            base.OnCreation();

            AddInput("0x4ac5c127", ConnectionType.Event, Realm);
            AddInput("Return", ConnectionType.Event, Realm);
        }
    }
}
