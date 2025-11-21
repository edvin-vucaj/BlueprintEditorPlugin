using BlueprintEditorPlugin.Editors.BlueprintEditor.Connections;
using FrostyEditor;
using FrostySdk;
using FrostySdk.Ebx;
using FrostySdk.IO;
using FrostySdk.Managers;
using System;

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
            ClearFooter();

			// ConnectedOptionDataType property
			UInt32 connectedOptionDataType = (UInt32)TryGetProperty("ConnectedOptionDataType");

            if (connectedOptionDataType != 0)
			{
				Footer = $"ConnectedOptionDataType: {Utils.GetString((int)connectedOptionDataType)}";
			}

			// OptionData property
            PointerRef pointerRef = (PointerRef)TryGetProperty("OptionData");

			if (pointerRef.Type != PointerRefType.External)
				return;

			EbxAssetEntry assetEntry = App.AssetManager.GetEbxEntry(pointerRef.External.FileGuid);
			if (Footer == null)
				Footer = $"Option: {assetEntry.Filename}";
			else
				Footer += $"\nOption: {assetEntry.Filename}";

			// UpdateContinuously property
			bool updateContinuously;

            if (TryGetProperty("UpdateContinuously") != null)
			{
                updateContinuously = (bool)TryGetProperty("UpdateContinuously");

				if (updateContinuously && Footer == null)
					Footer = "Update continuously: True";
				else if (updateContinuously && Footer != null)
                    Footer += "\nUpdate continuously: True";
            }
		}
	}
}
