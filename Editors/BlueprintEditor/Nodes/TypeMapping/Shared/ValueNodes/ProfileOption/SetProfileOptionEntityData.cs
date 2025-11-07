using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using FrostyEditor;
using FrostySdk.Ebx;
using FrostySdk.IO;
using FrostySdk.Managers;

namespace BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.TypeMapping.Shared.ValueNodes.ProfileOption
{
	public class SetProfileOptionNode : EntityNode
	{
		public override string ObjectType => "SetProfileOptionEntityData";

		public override string ToolTip => "Sets and saves information passed to it in the given ProfileOption asset files";

        public override void OnCreation()
		{
			base.OnCreation();

			AddInput("BoolValue", ConnectionType.Property, Realm);
			AddInput("IntValue", ConnectionType.Property, Realm);
			AddInput("FloatValue", ConnectionType.Property, Realm);
			AddInput("StringValue", ConnectionType.Property, Realm);
			AddInput("OptionData", ConnectionType.Property, Realm);
			
			AddInput("Apply", ConnectionType.Event, Realm);
			AddInput("Save", ConnectionType.Event, Realm);
			AddInput("ResetToDefault", ConnectionType.Event, Realm);
			AddInput("0x68a11699", ConnectionType.Event, Realm);
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
