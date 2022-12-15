#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

namespace TeamNinja
{
    [InitializeOnLoad]
    public class PersistentObjects : MonoBehaviour
    {
        public static string currentScenePath;

        static PersistentObjects()
        {
            currentScenePath = SceneManager.GetActiveScene().path;

            SceneManager.sceneLoaded += EditorSceneManager_sceneLoaded;

            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/PersistentObjects.unity");
        }

        private static void EditorSceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {

            SceneManager.sceneLoaded -= EditorSceneManager_sceneLoaded;

            GameStateManager.Instance.LoadEditorScene(currentScenePath);

        }
    }
}
#endif