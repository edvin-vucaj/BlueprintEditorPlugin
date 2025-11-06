using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using FrostySdk.Ebx;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Comparison
{
    public class CompareStringEntityData : EntityNode
    {
        public override string ObjectType => "CompareStringEntityData";

        public override string ToolTip => "This object compares two input strings and outputs events based on whether the strings are the same or not";

        public override void OnCreation()
        {
            base.OnCreation();

            AddInput("A", ConnectionType.Property, Realm);
            AddInput("B", ConnectionType.Property, Realm);
            AddInput("In", ConnectionType.Event, Realm);

            AddOutput("A!=B", ConnectionType.Event, Realm);
            AddOutput("A=B", ConnectionType.Event, Realm);
        }

        public override void BuildFooter()
        {
            ClearFooter();

            if ((bool)TryGetProperty("TriggerOnPropertyChange"))
            {
                if (Footer == null)
                    Footer = "Triggers on property changed";
                else
                    Footer += "\nTriggers on property changed";
            }

            if ((bool)TryGetProperty("TriggerOnStart"))
            {
                if (Footer == null)
                    Footer = "Triggers on start";
                else
                    Footer += "\nTriggers on start";
            }

            if ((bool)TryGetProperty("AlwaysSend"))
            {
                if (Footer == null)
                    Footer = "Always sends outputs";
                else
                    Footer += "\nAlways sends outputs";
            }

            if (TryGetProperty("A") != null && (CString)TryGetProperty("A") != "")
            {
                if (Footer == null)
                    Footer = $"A = \"{TryGetProperty("A")}\"";
                else
                    Footer += $"\n\nA = \"{TryGetProperty("A")}\"";
            }

            if (TryGetProperty("B") != null && (CString)TryGetProperty("B") != "")
            {
                if (Footer == null)
                    Footer = $"B = \"{TryGetProperty("B")}\"";
                else if (Footer.Contains("A ="))
                    Footer += $"\nB = \"{TryGetProperty("B")}\"";
                else
                    Footer += $"\n\nB = \"{TryGetProperty("B")}\"";
            }
        }
    }
}
