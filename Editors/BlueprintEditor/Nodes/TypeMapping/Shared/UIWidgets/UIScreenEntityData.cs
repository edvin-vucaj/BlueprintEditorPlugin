using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.UIWidgets
{
    public class UIScreenEntityData : EntityNode
    {
        public override string ObjectType => "UIScreenEntityData";

        public override string ToolTip => "This node is used to create UI screen entities and implement their logic";

        public override void OnCreation()
        {
            base.OnCreation();

            AddInput("Activate", ConnectionType.Event, Realm);
            AddInput("0x321be0dd", ConnectionType.Event, Realm);
            AddInput("0x192e67c8", ConnectionType.Event, Realm);
            AddInput("0x61f26ebc", ConnectionType.Event, Realm);
        }
    }
}
