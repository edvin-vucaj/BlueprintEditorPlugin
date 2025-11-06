using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.Ports;
using BlueprintEditorPlugin.Models.Nodes.Ports;
using Frosty.Core.Controls;
using FrostySdk.Ebx;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Logic.Flow
{
    public class SelectEventNode : EntityNode
    {
        public override string ObjectType => "SelectEventEntityData";
        public override string ToolTip => "This node lets you to select between a list of output events";

        public override void OnCreation()
        {
            base.OnCreation();

            AddInput("In", ConnectionType.Event, Realm);
            
            int evntCount = 0;
            foreach (CString evnt in (dynamic)TryGetProperty("Events"))
            {
                if (evnt == "")
                    AddOutput($"Out{evntCount++}", ConnectionType.Event, Realm);

                if (evnt.IsNull())
                    continue;

                AddOutput(evnt.ToString(), ConnectionType.Event, Realm);
            }
        }

        public override void OnObjectModified(object sender, ItemModifiedEventArgs args)
        {
            base.OnObjectModified(sender, args);

            // An event was edited
            if (args.Item.Parent.Name == "Events")
            {
                string oldName;
                EntityOutput output;
                if ((CString)args.OldValue == "")
                {
                    oldName = "";
                    output = GetOutput($"Out{args.ModifiedArgs.Index}", ConnectionType.Event);
                }
                else
                {
                    oldName = args.OldValue.ToString();
                    output = GetOutput(oldName, ConnectionType.Event);
                }

                // Update names to the new value
                string newName = args.NewValue.ToString();
                output.Name = newName;

                RefreshCache();
            }
            // The list itself was edited
            else if (args.Item.Name == "Events")
            {
                switch (args.ModifiedArgs.Type)
                {
                    case ItemModifiedTypes.Insert:
                    case ItemModifiedTypes.Add:
                    {
                        string tmp = $"Out{args.ModifiedArgs.Index}";
                        AddOutput(tmp, ConnectionType.Event, Realm);
                    } break;
                    case ItemModifiedTypes.Remove:
                    {
                        List<IPort> outputs = Outputs.ToList();
                        IPort output = outputs[args.ModifiedArgs.Index];
                        RemoveOutput((EntityOutput)output);

                    } break;
                    case ItemModifiedTypes.Clear:
                    {
                        List<IPort> outputs = Outputs.ToList();
                        
                        for (var i = 0; i < outputs.Count; i++)
                        {
                            IPort output = outputs[i];
                            RemoveOutput((EntityOutput)output);
                        }
                    } break;
                    case ItemModifiedTypes.Assign:
                    {
                        Inputs.Clear();
                        Outputs.Clear();
                        
                        foreach (CString evnt in (dynamic)TryGetProperty("Events"))
                        {
                            if (evnt.IsNull())
                                continue;
                
                            AddOutput(evnt.ToString(), ConnectionType.Event, Realm);
                        }
                        
                        RefreshCache();
                    } break;
                }
            }
        }

        public SelectEventNode()
        {
            Inputs = new ObservableCollection<IPort>
            {
                new EventInput("In", this),
                new PropertyInput("Selected", this)
            };
        }
    }
}
