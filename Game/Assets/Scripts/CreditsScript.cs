using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TeamNinja
{
    public class CreditsScript : MonoBehaviour
    {
        [SerializeField] private GameObject creditScript;
        [SerializeField] private float scrollSpeed;
        [SerializeField] private Transform endPosition;
        private int mainMenuScene;

        void Start()
        {
            mainMenuScene = 2;
            StartCoroutine(LerpingPosition());
        }

        private IEnumerator LerpingPosition()
        {
            while(creditScript.transform.position.y <= endPosition.position.y)
            {
                creditScript.transform.position += new Vector3(0,scrollSpeed,0);
                yield return new WaitForEndOfFrame();
            }
            SceneManager.LoadScene(mainMenuScene);
            Debug.Log("finished");
        }
    }
}
