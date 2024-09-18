// // Decompiled with JetBrains decompiler
// // Type: MapGenerator
// // Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// // MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// // Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll
//
// using System;
// using UnityEngine;
//
// #nullable disable
// public class MapGenerator : MonoBehaviour
// {
//   public MapGenerator.DrawMode drawMode;
//   public TerrainData terrainData;
//   public NoiseData noiseData;
//   public TextureData textureData;
//   public Material terrainMaterial;
//   [Range(0.0f, 6f)]
//   public int levelOfDetail;
//   private static int seed = 105;
//   public bool autoUpdate;
//   private float[,] falloffMap;
//   private MapDisplay display;
//   public static int mapChunkSize = 241;
//   public static int worldScale = 12;
//   public static MapGenerator Instance;
//   public static float[,] staticNoiseMap;
//   public float[,] heightMap;
//
//   private void Awake()
//   {
//     MapGenerator.Instance = this;
//     this.textureData.ApplyToMaterial(this.terrainMaterial);
//     this.textureData.UpdateMeshHeights(this.terrainMaterial, this.terrainData.minHeight, this.terrainData.maxHeight);
//   }
//
//   private void OnValuesUpdated()
//   {
//     if (Application.isPlaying)
//       return;
//     this.GenerateMap();
//     this.DrawMapInEditor();
//   }
//
//   private void OnTextureValuesUpdated() => this.textureData.ApplyToMaterial(this.terrainMaterial);
//
//   public void GenerateMap(int seed = 105)
//   {
//     this.heightMap = this.GeneratePerlinNoiseMap(seed);
//     this.GeneratePerlinNoiseMap(seed);
//     this.textureData.UpdateMeshHeights(this.terrainMaterial, this.terrainData.minHeight, this.terrainData.maxHeight);
//     this.display = UnityEngine.Object.FindObjectOfType<MapDisplay>();
//     if (this.drawMode == MapGenerator.DrawMode.NoiseMap)
//       this.display.DrawTexture(TextureGenerator.TextureFromHeightMap(this.heightMap));
//     else if (this.drawMode == MapGenerator.DrawMode.Mesh)
//       this.display.DrawMesh(MeshGenerator.GenerateTerrainMesh(this.heightMap, this.terrainData.heightMultiplier, this.terrainData.heightCurve, this.levelOfDetail));
//     else if (this.drawMode == MapGenerator.DrawMode.FalloffMap)
//       this.display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(MapGenerator.mapChunkSize)));
//     else if (this.drawMode == MapGenerator.DrawMode.ColorMap)
//       this.display.DrawTexture(TextureGenerator.ColorTextureFromHeightMap(this.heightMap, this.textureData));
//     this.textureData.ApplyToMaterial(this.terrainMaterial);
//     this.textureData.UpdateMeshHeights(this.terrainMaterial, this.terrainData.minHeight, this.terrainData.maxHeight);
//   }
//
//   public float[,] GeneratePerlinNoiseMap(NoiseData noiseData, int seed, bool useFalloffMap)
//   {
//     float[,] noiseMap = Noise.GenerateNoiseMap(MapGenerator.mapChunkSize, MapGenerator.mapChunkSize, seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, noiseData.blend, noiseData.blendStrength, noiseData.offset);
//     if (useFalloffMap)
//     {
//       if (this.falloffMap == null)
//         this.falloffMap = FalloffGenerator.GenerateFalloffMap(MapGenerator.mapChunkSize);
//       for (int index1 = 0; index1 < MapGenerator.mapChunkSize; ++index1)
//       {
//         for (int index2 = 0; index2 < MapGenerator.mapChunkSize; ++index2)
//         {
//           if (this.terrainData.useFalloff)
//             noiseMap[index2, index1] = Mathf.Clamp(noiseMap[index2, index1] - this.falloffMap[index2, index1], 0.0f, 1f);
//         }
//       }
//     }
//     return noiseMap;
//   }
//
//   public float[,] GeneratePerlinNoiseMap(int seed)
//   {
//     float[,] noiseMap = Noise.GenerateNoiseMap(MapGenerator.mapChunkSize, MapGenerator.mapChunkSize, seed, this.noiseData.noiseScale, this.noiseData.octaves, this.noiseData.persistance, this.noiseData.lacunarity, this.noiseData.blend, this.noiseData.blendStrength, this.noiseData.offset);
//     if (this.terrainData.useFalloff)
//     {
//       if (this.falloffMap == null)
//         this.falloffMap = FalloffGenerator.GenerateFalloffMap(MapGenerator.mapChunkSize);
//       for (int index1 = 0; index1 < MapGenerator.mapChunkSize; ++index1)
//       {
//         for (int index2 = 0; index2 < MapGenerator.mapChunkSize; ++index2)
//         {
//           if (this.terrainData.useFalloff)
//             noiseMap[index2, index1] = Mathf.Clamp(noiseMap[index2, index1] - this.falloffMap[index2, index1], 0.0f, 1f);
//         }
//       }
//     }
//     return noiseMap;
//   }
//
//   public void DrawMapInEditor()
//   {
//     float[,] perlinNoiseMap = this.GeneratePerlinNoiseMap(0);
//     MapDisplay objectOfType = UnityEngine.Object.FindObjectOfType<MapDisplay>();
//     if (this.drawMode == MapGenerator.DrawMode.NoiseMap)
//       objectOfType.DrawTexture(TextureGenerator.TextureFromHeightMap(perlinNoiseMap));
//     else if (this.drawMode == MapGenerator.DrawMode.Mesh)
//     {
//       objectOfType.DrawMesh(MeshGenerator.GenerateTerrainMesh(perlinNoiseMap, this.terrainData.heightMultiplier, this.terrainData.heightCurve, this.levelOfDetail));
//     }
//     else
//     {
//       if (this.drawMode != MapGenerator.DrawMode.FalloffMap)
//         return;
//       objectOfType.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(MapGenerator.mapChunkSize)));
//     }
//   }
//
//   private void OnValidate()
//   {
//     if ((UnityEngine.Object) this.terrainData != (UnityEngine.Object) null)
//     {
//       this.terrainData.OnValuesUpdate -= new Action(this.OnValuesUpdated);
//       this.terrainData.OnValuesUpdate += new Action(this.OnValuesUpdated);
//     }
//     if ((UnityEngine.Object) this.noiseData != (UnityEngine.Object) null)
//     {
//       this.noiseData.OnValuesUpdate -= new Action(this.OnValuesUpdated);
//       this.noiseData.OnValuesUpdate += new Action(this.OnValuesUpdated);
//     }
//     if (!((UnityEngine.Object) this.textureData != (UnityEngine.Object) null))
//       return;
//     this.textureData.OnValuesUpdated -= new Action(this.OnTextureValuesUpdated);
//     this.textureData.OnValuesUpdated += new Action(this.OnTextureValuesUpdated);
//   }
//
//   public enum DrawMode
//   {
//     NoiseMap,
//     Mesh,
//     FalloffMap,
//     ColorMap,
//   }
// }
//
