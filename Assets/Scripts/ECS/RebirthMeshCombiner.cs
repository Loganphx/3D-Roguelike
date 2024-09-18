#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using File = System.IO.File;

namespace RebirthStudios.Editor
{
    public class RebirthMeshCombiner : EditorWindow
    {
      
        [MenuItem("Tools/Rebirth Studios/RebirthMeshCombiner")]
        public static void ShowWindow()
        {
            //GetWindow<TestOdin3>().CreateMatrixData();
            GetWindow<RebirthMeshCombiner>().Show();
        }
    
        public void OnGUI()
        {
            if (GUILayout.Button("Combine Meshes"))
            {
                CombineMeshes();
            }
        }

        private (UnityEngine.Mesh mesh, Dictionary<string, Material> materials) GenerateLODMesh(GameObject selectedGameObject, int lodNumber)
        {
            var lods = new List<string>
            {
                "_LOD0",
                "_LOD1",
                "_LOD2",
            };
            var collisions = new List<string>
            {
                "_Collision",
                "_Collision_1",
                "_Collision_2",
                "_Collision_3",
                "_Collision_4",
                "_Collision_5",
                "_Collision_6",
                "_Collision_7",
                "_Collision_8",
                "_Collision_9",
                "_collision",
                "_collision_1",
                "_collision_2",
                "_collision_3",
                "_collision_4",
                "_collision_5",
                "_collision_6",
                "_collision_7",
                "_collision_8",
                "_collision_9",
            };
            lods.Remove("_LOD" + lodNumber);
            
             var meshFilters = selectedGameObject.GetComponentsInChildren<MeshFilter>().Where(x => !x.transform.CompareTag("Exclude") && !x.transform.parent.CompareTag("Exclude")).ToList();

             for (int j = 0; j < meshFilters.Count; j++)
             {
                 var meshFilter = meshFilters[j];
                 if (!meshFilter.gameObject.activeSelf)
                 {
                     meshFilters.RemoveAt(j);
                     j--;
                     continue;
                 }
                 
                 foreach (var lod in lods)
                 {
                     if (meshFilter.name.EndsWith(lod))
                     {
                         meshFilters.RemoveAt(j);
                         j--;
                         break;
                     }

                 } 
             }
             
             for (int j = 0; j < meshFilters.Count; j++)
             {
                 var meshFilter = meshFilters[j];
                 foreach (var lod in collisions)
                 {
                     if (meshFilter.name.EndsWith(lod))
                     {
                         meshFilters.RemoveAt(j);
                         j--;
                         break;
                     }
                 }
             }

             foreach (var meshFilter in meshFilters)
             {
                 //Debug.Log($"LOD{lodNumber} - {meshFilter.transform}");
             }
             

            CombineInstance[] combines = new CombineInstance[meshFilters.Count];

            Dictionary<string, Material> materials = new Dictionary<string, Material>();
            foreach (var meshFilter in meshFilters)
            {
                var meshRenderer = meshFilter.GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    Debug.LogError(meshFilter + " has no meshRenderer", meshFilter);
                    continue;
                }

                if (meshRenderer.sharedMaterial == null)
                {
                    Debug.LogError(meshFilter + " has no meshRenderer", meshFilter);
                    continue;
                }
                materials.TryAdd(meshRenderer.sharedMaterial.name, meshRenderer.sharedMaterial);
            }
            
            int i = 0;
            while (i < meshFilters.Count)
            {
                combines[i].mesh = meshFilters[i].sharedMesh;
                combines[i].transform = meshFilters[i].transform.localToWorldMatrix;
                var material = meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial;
                var subMeshIndex = materials.Keys.ToList().IndexOf(material.name);
                //Debug.Log($"SubMeshIndex(LOD{lodNumber}): {meshFilters[i].transform}, {subMeshIndex}");
                combines[i].subMeshIndex = subMeshIndex;
                //meshFilters[i].gameObject.SetActive(false);
                i++;
            }

            List<UnityEngine.Mesh> meshes = new List<UnityEngine.Mesh>();
            for (int index = 0; index < materials.Count; index++)
            {
                List<CombineInstance> combineInstances = new List<CombineInstance>();
                for (int j = 0; j < combines.Length; j++)
                {
                    if (combines[j].subMeshIndex == index)
                    {
                        combines[j].subMeshIndex = 0;
                        combineInstances.Add(combines[j]);
                    }   
                }
                
                UnityEngine.Mesh mesh = new UnityEngine.Mesh();
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                mesh.CombineMeshes(combineInstances.ToArray(), true);
                Debug.Log($"LOD{lodNumber} - Mesh Vert: {mesh.vertices.Length}");
                meshes.Add(mesh);
                
            }
            
            List<CombineInstance> finalCombineInstances = new List<CombineInstance>();
            foreach (var mesh in meshes)
            {
                finalCombineInstances.Add(new CombineInstance()
                {
                    mesh = mesh,
                    transform = selectedGameObject.transform.worldToLocalMatrix
                });
            }

            UnityEngine.Mesh finalMesh;
            if (File.Exists($"{Application.dataPath}/" +$"Assets/CombinedMeshes/{selectedGameObject.name}_LOD{lodNumber}.asset"))
            {
                finalMesh = AssetDatabase.LoadAssetAtPath<UnityEngine.Mesh>($"Assets/CombinedMeshes/{selectedGameObject.name}_LOD{lodNumber}.asset");
                if (finalMesh == null)
                {
                    finalMesh = new UnityEngine.Mesh();
                }
            }
            else
            {
                finalMesh = new UnityEngine.Mesh();
                AssetDatabase.CreateAsset(finalMesh, $"Assets/CombinedMeshes/{selectedGameObject.name}_LOD{lodNumber}.asset");
            }

            finalMesh.indexFormat = IndexFormat.UInt32;

            finalMesh.CombineMeshes(finalCombineInstances.ToArray(), false);
            AssetDatabase.SaveAssetIfDirty(finalMesh);

            return (finalMesh, materials);
        }
        
        public void CombineMeshes()
        {
            var selectedGameObjects = Selection.activeGameObject;
            if(selectedGameObjects == null) return;
            
            var lod0Mesh = GenerateLODMesh(selectedGameObjects, 0);
            var lod1Mesh = GenerateLODMesh(selectedGameObjects, 1);
            var lod2Mesh = GenerateLODMesh(selectedGameObjects, 2);

            var meshes = new UnityEngine.Mesh[]
            {
                lod0Mesh.mesh,
                lod1Mesh.mesh,
                lod2Mesh.mesh
            };
            
            var newMesh = new GameObject(selectedGameObjects.name + "(Combined)");
            //newMesh.AddComponent<MeshFilter>();
            var lod0 = new GameObject("LOD0", typeof(MeshFilter), typeof(MeshRenderer));
            var lod1 = new GameObject("LOD1", typeof(MeshFilter), typeof(MeshRenderer));
            var lod2 = new GameObject("LOD2", typeof(MeshFilter), typeof(MeshRenderer));
            var lodGroup = newMesh.AddComponent<LODGroup>();
            lodGroup.SetLODs(new LOD[4]
            {
                new LOD(.25f, new[] {lod0.GetComponent<Renderer>()}),
                new LOD(.125f, new[] {lod1.GetComponent<Renderer>()}),
                new LOD(.01f, new[] {lod2.GetComponent<Renderer>()}),
                new LOD(.00f, new Renderer[0]),
            });
            lodGroup.RecalculateBounds();
            
            lod0.transform.SetParent(newMesh.transform);
            lod0.transform.GetComponent<MeshFilter>().sharedMesh = lod0Mesh.mesh;
            lod0.GetComponent<MeshRenderer>().materials = lod0Mesh.materials.Values.ToArray();
            
            lod1.transform.SetParent(newMesh.transform);
            lod1.transform.GetComponent<MeshFilter>().sharedMesh = lod1Mesh.mesh;
            lod1.GetComponent<MeshRenderer>().materials = lod1Mesh.materials.Values.ToArray();
            
            lod2.transform.SetParent(newMesh.transform);
            lod2.transform.GetComponent<MeshFilter>().sharedMesh = lod2Mesh.mesh;
            lod2.GetComponent<MeshRenderer>().materials = lod2Mesh.materials.Values.ToArray();
            
            newMesh.transform.gameObject.SetActive(true);
            newMesh.transform.position = selectedGameObjects.transform.position;
            newMesh.transform.rotation = selectedGameObjects.transform.rotation;
            MeshFilter[] excludedGameObjects = selectedGameObjects.GetComponentsInChildren<MeshFilter>().Where(x => !x.gameObject.name.EndsWith("_LOD1") && 
                !x.gameObject.name.EndsWith("_LOD2") && !x.gameObject.name.EndsWith("_collision") && !x.gameObject.name.EndsWith("_Collision") && !x.gameObject.name.EndsWith("_Collision_1") &&
                !x.gameObject.name.EndsWith("_Collision_2") && !x.gameObject.name.EndsWith("_Collision_3") && !x.gameObject.name.EndsWith("_Collision_4") 
                && !x.gameObject.name.EndsWith("_Collision_5") && !x.gameObject.name.EndsWith("_Collision_6") 
                && !x.gameObject.name.EndsWith("_Collision_7") && !x.gameObject.name.EndsWith("_Collision_8") 
                && x.transform.parent.CompareTag("Exclude") && x.transform.CompareTag("Exclude")).ToArray();
            foreach (var copyMesh in excludedGameObjects)
            {
                Debug.Log($"Copying Mesh: {copyMesh.transform.parent.gameObject}");
                var prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(copyMesh.transform.parent.gameObject);
                if(prefab == null) continue;
                Debug.Log(prefab);
                var prefabObject = PrefabUtility.InstantiatePrefab(prefab, newMesh.transform) as
                    GameObject;
                Debug.Log(prefabObject);
                prefabObject.transform.position = copyMesh.transform.position;
                prefabObject.transform.rotation = copyMesh.transform.rotation;
                prefabObject.transform.localScale = copyMesh.transform.localScale;
    
            }
            newMesh.transform.parent = selectedGameObjects.transform.parent;
            newMesh.transform.SetSiblingIndex(selectedGameObjects.transform.childCount+1);
        }
    }
}
#endif