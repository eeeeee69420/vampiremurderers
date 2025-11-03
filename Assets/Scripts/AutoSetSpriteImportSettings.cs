using UnityEngine;
using UnityEditor;

public class AutoSetSpriteImportSettings : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        var importer = (TextureImporter)assetImporter;

        // Only apply to Sprites (2D and UI)
        if (importer.textureType == TextureImporterType.Sprite)
        {
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.filterMode = FilterMode.Point; // optional, good for pixel art
            importer.mipmapEnabled = false;         // optional
        }
    }
}