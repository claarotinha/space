using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class SpaceBossAutoSetup
{
    static SpaceBossAutoSetup()
    {
        EditorApplication.delayCall += TryCreateScene;
    }

    static void TryCreateScene()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            return;
        const string scenePath = "Assets/SpaceBoss/_Boot.unity";
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) != null) return;
        if (EditorSceneManager.GetActiveScene().isDirty) return;
        SpaceBossBuilder.Build();
        EditorSceneManager.OpenScene(scenePath);
        Debug.Log("Space Boss: _Boot criada automaticamente. Aperte Play.");
    }
}
