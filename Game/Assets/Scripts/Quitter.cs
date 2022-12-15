using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TeamNinja
{
    public class Quitter : MonoBehaviour
    {
        private static Quitter Instance;
        [SerializeField] private float transitionTime;

        void Awake()
        {
            Instance = this;
        }
        public static void QuitApplication()
        {
            Debug.Log("QUITTING APPLICATION...");
            Debug.Log("INSTANCE: " + (Instance == null));
            Instance.StartCoroutine(Instance.Quitting());
        }


        IEnumerator Quitting()
        {
            Debug.Log("STARTING QUIT...");
            yield return new WaitForSeconds(transitionTime);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Debug.Log("QUIT FINISHED");
            Application.Quit();
        }

        public static void QuitToMainMenu()
        {
            GameStateManager.Instance.LoadLevel(GameStateManager.Instance.mainMenuSceneNum);
        }
    }
}
