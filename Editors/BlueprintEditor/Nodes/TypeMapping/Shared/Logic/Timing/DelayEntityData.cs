using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using Frosty.Core.Controls;
using FrostySdk.Ebx;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Logic.Timing
{
    public class DelayEntityData : EntityNode
    {
        public override string ObjectType => "DelayEntityData";

        public override void OnCreation()
        {
            base.OnCreation();

            AddInput("Delay", ConnectionType.Property, Realm);
            AddInput("In", ConnectionType.Event, Realm);
            AddInput("Reset", ConnectionType.Event, Realm);
            AddOutput("Out", ConnectionType.Event, Realm);
        }
        
        public override void BuildFooter()
        {
            ClearFooter();

            if ((bool)TryGetProperty("AutoStart"))
            {
                if (Footer == null)
                    Footer = "Auto starts";
                else
                    Footer += "\nAuto starts";
            }
            
            if ((bool)TryGetProperty("RunOnce"))
            {
                if (Footer == null)
                    Footer = "Runs once";
                else
                    Footer += "\nRuns once";
            }

            if ((bool)TryGetProperty("RemoveDuplicateEvents"))
            {
                if (Footer == null)
                    Footer = "Remove duplicate events";
                else
                    Footer += "\nRemove duplicate events";
            }

            if (Footer != null)
            {
                AddFooter($"\nDelay: {TryGetProperty("Delay")}");
            }
            else
            {
                AddFooter($"Delay: {TryGetProperty("Delay")}");
            }

            if (TryGetProperty("TimeDeltaType") != null)
            {
                string timeDelta = (CString)TryGetProperty("TimeDeltaType").ToString().Substring(14);
                AddFooter($"TimeDeltaType: {timeDelta}");
            }
        }
    }
}