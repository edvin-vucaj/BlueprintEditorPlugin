using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.Ports;
using BlueprintEditorPlugin.Models.Nodes.Ports;
using Frosty.Core.Controls;
using FrostySdk.Ebx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Selection
{
    public class SelectPropertyEntityData_Base : EntityNode
    {
        public override string ObjectType => "SelectPropertyEntityData_Base";

        public override void OnCreation()
        {
            base.OnCreation();

            // Reads input count when object is created (added) on the graph or
            // when the graph is loaded
            readInputsCount();
        }

        public override void OnObjectModified(object sender, ItemModifiedEventArgs args)
        {
            base.OnObjectModified(sender, args);

            // An input property was edited
            if (args.Item.Parent.Name == "Inputs")
            {
                int modifiedIndex = args.ModifiedArgs.Index;

                string oldName;
                EntityInput input;
                if ((CString)args.OldValue == "")
                {
                    input = GetInput($"In{modifiedIndex}", ConnectionType.Property);
                }
                else
                {
                    oldName = args.OldValue.ToString();
                    input = GetInput(oldName, ConnectionType.Property);
                }

                // Update names to the new value
                string newName;
                if (args.NewValue.ToString() != "")
                    newName = args.NewValue.ToString();
                else
                    newName = $"In{modifiedIndex}";

                input.Name = newName;

                RefreshCache();
            }
            // The Inputs list was edited
            else if (args.Item.Name == "Inputs")
            {
                int modifiedIndex = args.ModifiedArgs.Index;
                switch (args.ModifiedArgs.Type)
                {
                    case ItemModifiedTypes.Insert:
                    case ItemModifiedTypes.Add:
                    {
                        string tmp = $"In{modifiedIndex}";
                        AddInput(tmp, ConnectionType.Property, Realm);

                        //FrostyPropertyGridItemData argument = args.Item.FindChild($"[{modifiedIndex}]");
                        //argument.Value = (CString)tmp;
                    }
                    break;
                    case ItemModifiedTypes.Remove:
                    {
                        List<IPort> inputs = Inputs.ToList();
                        IPort input = null;
                        try
                        {
                            input = inputs[modifiedIndex];
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                        }
                        if (input != null)
                            RemoveInput((EntityInput)input);
                    }
                    break;
                    case ItemModifiedTypes.Clear:
                    {
                        List<IPort> inputs = Inputs.ToList();

                        for (var i = 0; i < inputs.Count; i++)
                        {
                            IPort input = inputs[i];
                            RemoveInput((EntityInput)input);
                        }
                    }
                    break;
                }
            }
        }

        public void readInputsCount()
        {
            int input_index = 0;
            foreach (CString input in (dynamic)TryGetProperty("Inputs"))
            {
                // If input is empty, add the port with name: In0, In1, In2, ...
                if (input == "")
                    AddInput($"In{input_index++}", ConnectionType.Property, Realm);

                // have to check this one because it throws null exception if not
                if (input.IsNull())
                    continue;

                // Add input with the given name
                AddInput(input.ToString(), ConnectionType.Property, Realm);
            }
        }
    }

    public class SelectPropertyEntityData : SelectPropertyEntityData_Base
    {
        public override string ObjectType => "SelectPropertyEntityData";
        public override void OnCreation()
        {
            base.OnCreation();

            // Source field Ids
            AddOutput("Out", ConnectionType.Property, Realm);
            AddOutput("SelectedIndex", ConnectionType.Property, Realm);
        }
    }
    public class SelectBoolEntityData : SelectPropertyEntityData
    {
        public override string ObjectType => "SelectBoolEntityData";
        public override string ToolTip => "This node selects one out of all given input Boolean properties";
    }
    public class SelectIntEntityData : SelectPropertyEntityData
    {
        public override string ObjectType => "SelectIntEntityData";
        public override string ToolTip => "This node selects one out of all given input Int32 properties";

        public override void OnCreation()
        {
            base.OnCreation();

            // Source events
            AddOutput("OnSelected", ConnectionType.Event, Realm);
        }
    }

    public class SelectFloatEntityData : SelectPropertyEntityData
    {
        public override string ObjectType => "SelectFloatEntityData";

        public override string ToolTip => "This node selects one out of all given input Float32 properties";

        public override void OnCreation()
        {
            base.OnCreation();

            // Source events
            AddOutput("OnSelected", ConnectionType.Event, Realm);
        }
    }
    public class SelectStringEntityData : SelectPropertyEntityData_Base
    {
        public override string ObjectType => "SelectStringEntityData";
        public override string ToolTip => "This node selects one out of all given input CString properties";

        public override void OnCreation()
        {
            base.OnCreation();

            // Source field Ids
            AddOutput("Out", ConnectionType.Property, Realm);

            // Source events
            AddOutput("OnSelected", ConnectionType.Event, Realm);
        }
    }

    public class SelectVec2EntityData : SelectPropertyEntityData_Base
    {
        public override string ObjectType => "SelectVec2EntityData";
        public override string ToolTip => "This node selects one out of all given input Vec2 properties";

        public override void OnCreation()
        {
            base.OnCreation();

            // Source field Ids
            AddOutput("Out", ConnectionType.Property, Realm);
        }
    }

    public class SelectVec3EntityData : SelectPropertyEntityData
    {
        public override string ObjectType => "SelectVec3EntityData";
        public override string ToolTip => "This node selects one out of all given input Vec3 properties";
    }

    public class SelectVec4EntityData : SelectVec2EntityData
    {
        public override string ObjectType => "SelectVec4EntityData";
        public override string ToolTip => "This node selects one out of all given input Vec4 properties";
    }

    public class SelectTransformEntityData : SelectStringEntityData
    {
        public override string ObjectType => "SelectTransformEntityData";
        public override string ToolTip => "This node selects one out of all given input Transform properties";
    }

    public class SelectObjectEntityData : SelectPropertyEntityData_Base
    {
        public override string ObjectType => "SelectObjectEntityData";
        public override string ToolTip => "This node selects one out of all given input Object properties";

        public override void OnCreation()
        {
            base.OnCreation();

            // Source field Ids
            AddOutput("Out", ConnectionType.Property, Realm);
        }
    }
}
