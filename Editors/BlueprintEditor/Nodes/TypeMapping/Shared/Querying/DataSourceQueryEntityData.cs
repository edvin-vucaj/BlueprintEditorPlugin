using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Querying
{
    public class DataSourceQueryEntityData : EntityNode
    {
        public override string ObjectType => "DataSourceQueryEntityData";

        public override string ToolTip => "This node queries different kinds of data from asset files and outputs them as dynamic data types";

        public override void OnCreation()
        {
            base.OnCreation();

            AddInput("InData", ConnectionType.Property, Realm);
        }
    }
}
