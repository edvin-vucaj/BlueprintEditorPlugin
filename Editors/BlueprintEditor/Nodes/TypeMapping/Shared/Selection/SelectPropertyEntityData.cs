using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Selection
{
    public class SelectPropertyEntityData : EntityNode
    {
        public override void OnCreation()
        {
            base.OnCreation();

            // Source field Ids
            AddInput("Out", ConnectionType.Property, Realm);
            AddInput("SelectedIndex", ConnectionType.Property, Realm);

            // Source events
            AddOutput("OnSelected", ConnectionType.Event, Realm);
        }
    }
}
