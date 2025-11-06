using System.Collections.ObjectModel;
using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.Ports;
using BlueprintEditorPlugin.Models.Nodes.Ports;
using Frosty.Core.Controls;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Logic.Flow
{
    public class EventSwitchNode : EntityNode
    {
        public override string ObjectType => "EventSwitchEntityData";
        public override string ToolTip => "A switch which changes what event it outputs depending on the current Out event";

        public override void OnCreation()
        {
            base.OnCreation();
            
            uint inCount = (uint)TryGetProperty("OutEvents");

            for (uint i = 0; i < inCount; i++)
            {
                AddOutput($"Out{i}", ConnectionType.Event, Realm);
            }
        }
        
        public override void OnObjectModified(object sender, ItemModifiedEventArgs args)
        {
            base.OnObjectModified(sender, args);

            if (args.Item.Name == "OutEvents")
            {
                uint oldCount = (uint)args.OldValue;
                uint outCount = (uint)TryGetProperty("OutEvents");

                if (outCount == 0)
                {
                    ClearOutputs();
                    return;
                }
                
                if (oldCount < outCount)
                {
                    // Add new inputs
                    for (uint i = 0; i < outCount; i++)
                    {   
                        AddOutput($"Out{i}", ConnectionType.Event, Realm);
                    }
                }
                else
                {
                    for (uint i = oldCount - 1; i >= outCount; i--)
                    {
                        RemoveOutput($"Out{i}", ConnectionType.Event);
                    }
                }
            }
        }
        public override void BuildFooter()
        {
            ClearFooter();

            if ((bool)TryGetProperty("AutoIncrement"))
            {
                Footer = "Auto increments";
            }
        }
        public EventSwitchNode()
        {
            Inputs = new ObservableCollection<IPort>
            {
                new EventInput("In", this),
                new EventInput("NextOut", this),
                new EventInput("Reset", this)
            };
        }
    }
}