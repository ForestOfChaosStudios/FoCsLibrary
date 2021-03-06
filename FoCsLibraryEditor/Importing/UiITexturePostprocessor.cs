﻿using UnityEditor;

namespace ForestOfChaosLib.Editor.AssetPostProcessors
{
	public class UiITexturePostprocessor: AssetPostprocessor
	{
		protected void OnPreprocessTexture()
		{
			var textureImporter = (TextureImporter)assetImporter;

			if(textureImporter.assetPath.Contains("UI"))
			{
				textureImporter.textureType = TextureImporterType.Sprite;
				//textureImporter.spritePixelsPerUnit = 512;
			}
			else if(textureImporter.assetPath.Contains("Sprite"))
			{
				textureImporter.textureType = TextureImporterType.Sprite;
				//textureImporter.spritePixelsPerUnit = 512;
			}

			//else if(textureImporter.assetPath.Contains("Editor Resources"))
			//{
			//	textureImporter.textureType    = TextureImporterType.GUI;
			//	textureImporter.maxTextureSize = 128;
			//}
		}
	}
}
