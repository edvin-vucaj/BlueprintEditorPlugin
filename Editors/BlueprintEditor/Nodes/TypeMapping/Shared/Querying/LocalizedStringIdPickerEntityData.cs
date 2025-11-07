using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Querying
{
    internal class LocalizedStringIdPickerEntityData : EntityNode
    {
        public override string ObjectType => "LocalizedStringIdPickerEntityData";

        public override string ToolTip => "This node outputs a DynamicStringId based on the Sid given. The format of Sid is \"ID_*\"";

        public override void OnCreation()
        {
            base.OnCreation();

            AddOutput("StringId", ConnectionType.Property, Realm);
        }
    }
}
