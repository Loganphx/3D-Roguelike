using System.IO;
using UnityEditor;
using UnityEngine;

public class ScreenshotPrefabPreview : EditorWindow
{
    [MenuItem("Tools/Rebirth Studios/ScreenshotPrefabPreview")]
    public static void ShowWindow()
    {
        //GetWindow<TestOdin3>().CreateMatrixData();
        GetWindow<ScreenshotPrefabPreview>().Show();
    }
    
    public void OnGUI()
    {
        if (GUILayout.Button("Screenshot Preview"))
        {
            Start();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var prefabs = Resources.LoadAll<GameObject>("Prefabs/Models");
        foreach (var prefab in prefabs)
        {
            if(File.Exists(Application.dataPath + $"/Resources/Sprites/Items/{prefab.name.Replace("model_", "")}.png"))
                continue;
            
            Debug.Log(prefab.name);
            var texture = AssetPreview.GetAssetPreview(prefab);
            if (texture == null)
            {
                // texture = AssetPreview.GetMiniThumbnail(prefab);
                Debug.LogError($"Couldn't load texture for {prefab.name}", prefab);
                continue;
            }
            var bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + $"/Resources/Sprites/Items/{prefab.name.Replace("model_", "")}.png", bytes);
        }
    }

}
