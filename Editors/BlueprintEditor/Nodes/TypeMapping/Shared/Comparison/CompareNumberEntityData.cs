using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.Comparison
{
	public abstract class CompareNumberNode : EntityNode
	{
		public override void OnCreation()
		{
			base.OnCreation();

			AddInput("A", ConnectionType.Property, Realm);
			AddInput("B", ConnectionType.Property, Realm);
			AddInput("In", ConnectionType.Event, Realm);

			AddOutput("A=B", ConnectionType.Event, Realm);
			AddOutput("A!=B", ConnectionType.Event, Realm);
			AddOutput("A>=B", ConnectionType.Event, Realm);
			AddOutput("A<=B", ConnectionType.Event, Realm);
			
			AddOutput("A>B", ConnectionType.Event, Realm);
			AddOutput("A<B", ConnectionType.Event, Realm);
		}
	}

	public class CompareIntNode : CompareNumberNode
	{
		public override string ObjectType => "CompareIntEntityData";

        public override string ToolTip => "This node sends output events based on the arithmetic relations of two integer inputs and the In event";

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

            if (TryGetProperty("AlwaysSend") != null && (bool)TryGetProperty("AlwaysSend"))
            {
                if (Footer == null)
                    Footer = "Always sends outputs";
                else
                    Footer += "\nAlways sends outputs";
            }

            if (TryGetProperty("A") != null && (int)TryGetProperty("A") != 0)
            {
                if (Footer == null)
                    Footer = $"A = {(int)TryGetProperty("A")}";
                else
                    Footer += $"\n\nA = {(int)TryGetProperty("A")}";
            }

            if (TryGetProperty("B") != null && (int)TryGetProperty("B") != 0)
            {
                if (Footer == null)
                    Footer = $"B = {(int)TryGetProperty("B")}";
                else if (Footer.Contains("A ="))
                    Footer += $"\nB = {(int)TryGetProperty("B")}";
                else
                    Footer += $"\n\nB = {(int)TryGetProperty("B")}";
            }
        }
    }
	
	public class CompareFloatNode : CompareNumberNode
	{
		public override string ObjectType => "CompareFloatEntityData";

        public override string ToolTip => "This node sends output events based on the arithmetic relations of two float inputs and the In event";

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

            if (TryGetProperty("AlwaysSend") != null && (bool)TryGetProperty("AlwaysSend"))
            {
                if (Footer == null)
                    Footer = "Always sends outputs";
                else
                    Footer += "\nAlways sends outputs";
            }

            if (TryGetProperty("A") != null && (float)TryGetProperty("A") != 0.0)
            {
                if (Footer == null)
                    Footer = $"A = {(float)TryGetProperty("A")}";
                else
                    Footer += $"\n\nA = {(float)TryGetProperty("A")}";
            }

            if (TryGetProperty("B") != null && (float)TryGetProperty("B") != 0.0)
            {
                if (Footer == null)
                    Footer = $"B = {(float)TryGetProperty("B")}";
                else if (Footer.Contains("A ="))
                    Footer += $"\nB = {(float)TryGetProperty("B")}";
                else
                    Footer += $"\n\nB = {(float)TryGetProperty("B")}";
            }
        }
    }
}
