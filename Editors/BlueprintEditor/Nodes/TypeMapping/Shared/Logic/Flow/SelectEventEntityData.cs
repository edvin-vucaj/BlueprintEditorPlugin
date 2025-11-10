using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
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
                int modifiedIndex = args.ModifiedArgs.Index;

                string oldName;
                EntityOutput output;
                if ((CString)args.OldValue == "")
                {
                    output = GetOutput($"Out{modifiedIndex}", ConnectionType.Event);
                }
                else
                {
                    oldName = args.OldValue.ToString();
                    output = GetOutput(oldName, ConnectionType.Event);
                }

                // Update names to the new value
                string newName;
                if (args.NewValue.ToString() != "")
                    newName = args.NewValue.ToString();
                else
                    newName = $"Out{modifiedIndex}";

                output.Name = newName;

                RefreshCache();
            }
            // The list itself was edited
            else if (args.Item.Name == "Events")
            {
                int modifiedIndex = args.ModifiedArgs.Index;
                switch (args.ModifiedArgs.Type)
                {
                    case ItemModifiedTypes.Insert:
                    case ItemModifiedTypes.Add:
                    {
                        string tmp = $"Out{modifiedIndex}";
                        AddOutput(tmp, ConnectionType.Event, Realm);

                        FrostyPropertyGridItemData argument = args.Item.FindChild($"[{modifiedIndex}]");
                        argument.Value = (CString)tmp;
                    } break;
                    case ItemModifiedTypes.Remove:
                    {
                        List<IPort> outputs = Outputs.ToList();
                        IPort output = null;
                        try
                        {
                            output = outputs[modifiedIndex];
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                        }
                        if (output != null)
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
