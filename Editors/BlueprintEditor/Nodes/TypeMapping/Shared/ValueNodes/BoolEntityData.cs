using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using Frosty.Core.Controls;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.ValueNodes
{
    public class BoolNode : EntityNode
    {
        public override string ObjectType => "BoolEntityData";

        public override string ToolTip => "This nodes outputs a Boolean value";

        public override void OnCreation()
        {
            base.OnCreation();

            AddInput("SetTrue", ConnectionType.Event, Realm);
            AddInput("SetFalse", ConnectionType.Event, Realm);
            AddInput("Toggle", ConnectionType.Event, Realm);

            AddOutput("Value", ConnectionType.Property, Realm);
        }

        public override void BuildFooter()
        {
            Footer = $"Default: {TryGetProperty("DefaultValue")}";
        }
    }
}