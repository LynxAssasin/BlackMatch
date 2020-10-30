using UnityEngine;
using UnityEditor;
using Game.Settings;

public class CreateScriptableObjects : MonoBehaviour
{

    [MenuItem("Assets/CreateSettings/LevelsSettings")]
    public static void CreateLevelSettings()
    {
        LevelsSettings asset = ScriptableObject.CreateInstance<LevelsSettings>();
        CreateAsset(asset, "Assets/LevelSettings.asset");
    }

    [MenuItem("Assets/CreateSettings/ElementsSettings")]
    public static void CreateElementsSettings()
    {
        ElementsSettings asset = ScriptableObject.CreateInstance<ElementsSettings>();
        CreateAsset(asset, "Assets/ElementsSettings.asset");
    }

    [MenuItem("Assets/CreateSettings/GameModesSettings")]
    public static void CreateGameModesSettings()
    {
        GameModesSettings asset = ScriptableObject.CreateInstance<GameModesSettings>();
        CreateAsset(asset, "Assets/GameModesSettings.asset");
    }

    private static void CreateAsset(Object obj, string path)
    {
        AssetDatabase.CreateAsset(obj, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = obj;
    }
}
