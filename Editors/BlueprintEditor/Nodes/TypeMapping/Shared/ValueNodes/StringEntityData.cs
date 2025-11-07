using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using Frosty.Core.Controls;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.ValueNodes
{
    public class StringEntityData : EntityNode
    {
        public override string ObjectType => "StringEntityData";

        public override string ToolTip => "This node outputs the string from its own \"DefaultString\" property value";

        public override void OnCreation()
        {
            base.OnCreation();

            AddOutput("String", ConnectionType.Property, Realm);
        }

        public override void BuildFooter()
        {
            Footer = $"Default string: {TryGetProperty("DefaultString")}";
        }
    }
}