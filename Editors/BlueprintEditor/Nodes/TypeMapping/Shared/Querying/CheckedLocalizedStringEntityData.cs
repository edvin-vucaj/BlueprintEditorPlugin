using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.Ports;
using BlueprintEditorPlugin.Models.Nodes.Ports;
using Frosty.Core.Controls;
using FrostySdk;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Querying
{
    public class CheckedLocalizedStringEntityData : EntityNode
    {
        public override string ObjectType => "CheckedLocalizedStringEntityData";

        public override string ToolTip => "This node takes arguments of different data types and outputs a LocalizedString. Can also be used to create LocalizedStrings without input arguments";

        public override void OnCreation()
        {
            base.OnCreation();

            AddInput("DynamicStringId", ConnectionType.Property, Realm);
            AddOutput("LocalizedString", ConnectionType.Property, Realm);
            
            // Get argument hashes
            dynamic argumentHashes = TryGetProperty("ArgumentHashes");
            // Loop through them and add them as inputs, converting the hash to a string name
            foreach (UInt32 argumentHash in argumentHashes)
            {
                string name = Utils.GetString((int)argumentHash);
                AddInput(name, ConnectionType.Property, Realm);
            }
        }

        public override void OnObjectModified(object sender, ItemModifiedEventArgs args)
        {
            base.OnObjectModified(sender, args);

            int modifiedIndex = args.ModifiedArgs.Index;
            
            // An argument hash was edited
            if (args.Item.Parent.Name == "ArgumentHashes")
            {
                EntityInput input;

                // Get the old argument hash and its port
                UInt32 oldArgHash = (UInt32)args.OldValue;

                // Used for catching null exception
                if (oldArgHash == 0)
                {
                    string newPortName = $"Arg{modifiedIndex}";
                    input = GetInput(newPortName, ConnectionType.Property);
                }
                else
                    input = GetInput((int)oldArgHash, ConnectionType.Property);
                
                // set the new argument port name based on the new hash
                UInt32 newArgHash = (UInt32)args.NewValue;
                input.Name = Utils.GetString((int)newArgHash);

                RefreshCache();
            }
            // The list of ArgumentHashes was edited
            else if (args.Item.Name == "ArgumentHashes")
            {
                switch (args.ModifiedArgs.Type)
                {
                    case ItemModifiedTypes.Add:
                    {
                        // Set the argument hash string, consisting of "Arg" + the modified index
                        string argStr = $"Arg{modifiedIndex}";
                        // Get the int hash of the string
                        int argHash = Utils.HashString(argStr);

                        // Add the new input port
                        AddInput(argStr, ConnectionType.Property, Realm);

                        // Add the int hash of the string to the node's property
                        FrostyPropertyGridItemData argItem = args.Item.FindChild($"[{modifiedIndex}]");
                        argItem.Value = (UInt32)argHash;

                        RefreshCache();
                    } break;

                    case ItemModifiedTypes.Remove:
                    {
                        List<IPort> inputs = Inputs.ToList();
                        IPort input = null;

                        try
                        {
                            input = inputs[modifiedIndex + 1];
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                        }

                        if (input != null)
                            RemoveInput((EntityInput)input);

                        RefreshCache();
                    } break;

                    case ItemModifiedTypes.Clear:
                    {
                        List<IPort> inputs = Inputs.ToList();

                        for (var i = 0; i < inputs.Count; i++)
                        {
                            IPort input = inputs[i];

                            if (input.Name == "DynamicStringId")
                                continue;

                            RemoveInput((EntityInput)input);
                        }

                        RefreshCache();
                    } break;
                }
            }
        }
    }
}
