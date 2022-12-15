using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TeamNinja;

namespace TeamNinja
{
    public class GameStateManager : MonoBehaviour 
    {

        public static GameStateManager Instance {get; private set; }

        // keeps track if the application just started
        public bool firstBootup = true;

        public int currentLevel = 0;
        public int persistenObjectSceneNum = 0;
        public int loadingSceneNum = 1;
        public int mainMenuSceneNum = 2;
        public int tutorialSceneNum = 3;
        public int levelOneSceneNum = 4;
        public int levelTwoSceneNum = 5;
        public int levelThreeSceneNum = 6;
        public int creditsSceneNum = 7;

        public float sensitivity = 50f;
        
        private void Awake() 
        {


            // If there is an instance, and it's not me, delete myself.
            
            if (Instance != null && Instance != this) 
            { 
               Destroy(this); 
            } 
            else 
            { 
              Instance = this;
              DontDestroyOnLoad(gameObject);
            }

            currentLevel = SceneManager.GetActiveScene().buildIndex;

            if (Application.isEditor)
            {
                firstBootup = false;
            }
            else
            {
                LoadLevel(mainMenuSceneNum);
            }

        }


        public void LoadLevel(int sceneIndex)
        {
            currentLevel = sceneIndex;
            Pauser.AbsoluteUnpause();
            StartCoroutine(LoadingLoadingScene());

        }

        private IEnumerator LoadingLoadingScene()
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(loadingSceneNum);

            while (!op.isDone)
            {
                yield return null;
            }
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }

        public void LoadEditorScene(string editorScenePath)
        {
            StartCoroutine(LoadingEditorScene(editorScenePath));
        }

        private IEnumerator LoadingEditorScene(string editorScenePath)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(editorScenePath, LoadSceneMode.Additive);

            while(!op.isDone)
            {
                yield return null;
            }

            yield return new WaitForFixedUpdate();
            currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(editorScenePath));
        }


    }

}
