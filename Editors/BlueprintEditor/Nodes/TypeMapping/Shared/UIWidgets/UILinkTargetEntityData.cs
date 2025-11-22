using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.UIWidgets
{
    public class UILinkTargetEntityData : EntityNode
    {
        public override string ObjectType => "UILinkTargetEntityData";

        public override string ToolTip => "This node outputs events listened from GoToUILink entities";

        public override void OnCreation()
        {
            base.OnCreation();

            AddOutput("0x9f56abcd", ConnectionType.Event, Realm);
            AddOutput("0xe9588f4f", ConnectionType.Event, Realm);
            AddOutput("0x624f5545", ConnectionType.Event, Realm);
        }
    }
}
