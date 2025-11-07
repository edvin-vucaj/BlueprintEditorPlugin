using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using FrostyEditor;
using FrostySdk.Ebx;
using FrostySdk.IO;
using FrostySdk.Managers;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.ValueNodes.ProfileOption
{
	public class GetProfileOptionNode : EntityNode
	{
		public override string ObjectType => "GetProfileOptionEntityData";

		public override string ToolTip => "Gets information from ProfileOption asset files";

		public override void OnCreation()
		{
			base.OnCreation();

			// Target field Ids
			AddInput("OptionData", ConnectionType.Property, Realm);

			// Target events
			AddInput("Update", ConnectionType.Event, Realm);

			// Source field Ids
			AddOutput("BoolValue", ConnectionType.Property, Realm);
			AddOutput("IntValue", ConnectionType.Property, Realm);
			AddOutput("FloatValue", ConnectionType.Property, Realm);
			AddOutput("StringValue", ConnectionType.Property, Realm);
			AddOutput("OptionData", ConnectionType.Property, Realm);

			// Source events
			AddOutput("OnLoaded", ConnectionType.Event, Realm);
			AddOutput("OnApply", ConnectionType.Event, Realm);
			AddOutput("OnSaved", ConnectionType.Event, Realm);

        }

        public override void BuildFooter()
		{
			PointerRef pointerRef = (PointerRef)TryGetProperty("OptionData");
			if (pointerRef != PointerRefType.External)
				return;

			EbxAssetEntry assetEntry = App.AssetManager.GetEbxEntry(pointerRef.External.FileGuid);
			Footer = $"Option: {assetEntry.Filename}";
		}
	}
}
