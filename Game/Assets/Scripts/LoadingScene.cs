using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TeamNinja;

namespace TeamNinja
{
    public class LoadingScene : MonoBehaviour
    {

        [SerializeField]
        private GameObject slider;

        [SerializeField]
        private GameObject[] logos;

        [SerializeField]
        private GameObject[] teamImages;

        [SerializeField]
        private float transitionTime;

        [SerializeField]
        private AudioSource backgroundMusic;

        private AsyncOperation operation;

        public void Start()
        {
            for(int i = 0; i < logos.Length; i++)
            {
                logos[i].SetActive(false);
            }
            for (int i = 0; i < teamImages.Length; i++)
            {
                teamImages[i].SetActive(false);
            }

            slider.GetComponent<Slider>().value = 0;
            slider.SetActive(false);
  

            this.GetComponent<Canvas>().enabled = false;


            if(GameStateManager.Instance.firstBootup)
            {
                GameStateManager.Instance.currentLevel = GameStateManager.Instance.mainMenuSceneNum;
                ShowInitialLoadingScreen(GameStateManager.Instance.mainMenuSceneNum);
                GameStateManager.Instance.firstBootup = false;
            }
            else
            {
                LoadLevel(GameStateManager.Instance.currentLevel);
            }
        }

        public void ShowInitialLoadingScreen(int sceneIndex)
        {
            StartCoroutine(ShowingSplashScreen(logos));
            StartCoroutine(LoadLevelAsynch(sceneIndex, logos, true, false));
        }

        public void LoadLevel(int sceneIndex)
        {
            StartCoroutine(ShowingSplashScreen(teamImages));
            StartCoroutine(LoadLevelAsynch(sceneIndex, teamImages, false, true));
        }

        IEnumerator LoadLevelAsynch(int sceneIndex, GameObject[] images, bool waitForImages, bool showProgressBar)
        {

            if(waitForImages)
                yield return new WaitForSecondsRealtime(transitionTime * images.Length);

            operation = SceneManager.LoadSceneAsync(sceneIndex);

            if (showProgressBar)
            {
                slider.SetActive(true);
                while (!operation.isDone)
                {
                    float progress = Mathf.Clamp01(operation.progress / .9f);

                    slider.GetComponent<Slider>().value = progress;

                    yield return null;
                }
            }
        }

        IEnumerator ShowingSplashScreen(GameObject[] images)
        {
            backgroundMusic.Play();
            this.GetComponent<Canvas>().enabled = true;

            yield return StartCoroutine(CyclingImages(images));

            backgroundMusic.Stop();
        }

        IEnumerator CyclingImages(GameObject[] images)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].SetActive(true);
                StartCoroutine(images[i].GetComponent<LerpImageColor>().LerpingColor(Color.clear, Color.white, transitionTime / 4));
                yield return new WaitForSeconds(transitionTime/2);
                StartCoroutine(images[i].GetComponent<LerpImageColor>().LerpingColor(Color.white, Color.clear, transitionTime / 4));
                yield return new WaitForSeconds(transitionTime / 2);
                images[i].SetActive(false);
                yield return null;
            }
        }
    }
}
