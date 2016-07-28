using UnityEngine;
using System.Collections;
using UnityEditor;

public class TextureImportExt : AssetPostprocessor
{
    void OnPostprocessTexture(Texture2D tx)
    {
        var textureImporter = assetImporter as TextureImporter;
        textureImporter.textureType = TextureImporterType.Advanced;
        textureImporter.npotScale = TextureImporterNPOTScale.None;
        textureImporter.mipmapEnabled = false;
        textureImporter.maxTextureSize = 2048;


    }
}
