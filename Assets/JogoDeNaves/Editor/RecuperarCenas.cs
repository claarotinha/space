using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class SpaceBossAutoSetup
{
    private const string ScenePath = "Assets/JogoDeNaves/_Boot.unity";

    static SpaceBossAutoSetup()
    {
        EditorApplication.delayCall += EnsureScene;
    }

    [MenuItem("Ferramentas/Jogo de Naves/Abrir ou recriar _Boot")]
    public static void OpenOrRecreate()
    {
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) == null)
            SpaceBossBuilder.Build();
        EditorSceneManager.OpenScene(ScenePath);
    }

    private static void EnsureScene()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            return;
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null)
            return;
        if (EditorSceneManager.GetActiveScene().isDirty)
            return;
        OpenOrRecreate();
    }
}
