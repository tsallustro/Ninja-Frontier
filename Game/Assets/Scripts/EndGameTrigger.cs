using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using UnityEngine;

namespace TeamNinja
{
    public class EndGameTrigger : GoalTrigger
    {
        [SerializeField] private Animator GirlNinja;
        [SerializeField] private Animator BadNinja;
        [SerializeField] private Animator Ninja;

        [SerializeField] private GameObject Player;
        [SerializeField] private GameObject inputManager;
        [SerializeField] private NewMovement newMovement;
        [SerializeField] private CombatControl combatControl;

        [SerializeField] private Vector3 finalLocation;

        [SerializeField] private GameObject MainCamera;
        [SerializeField] private GameObject SideCamera;
        [SerializeField] private GameObject ZoomedOutCamera;
        [SerializeField] private GameObject CloseUpCamera;
        [SerializeField] private GameObject CloseUpCamera2;
        [SerializeField] private GameObject BlackOutCamera;

        [SerializeField] private float walkingSpeed;

        [SerializeField] private AudioSource bgMusic;
        [SerializeField] private AudioSource bgMusic2;
        [SerializeField] private AudioSource badEndingSound;
        [SerializeField] private AudioSource badEndingSound2;
        [SerializeField] private AudioSource GameOverMusic;
        [SerializeField] private AudioSource GameOverMusic2;

        [SerializeField] private AudioSource goodEndingSound;

        [SerializeField] private GameObject GameOverText;
        [SerializeField] private GameObject GameOverText2;

        [SerializeField] private GameObject AliveNinja;
        [SerializeField] private GameObject DeadNinja;



        private bool sideCamOnPlayer;
        private bool zoomedOutCam;
        private bool closeupCam;
        private bool closeupCam2;
        private bool blackout;

        private int creditsSceneNum;

        private bool badEnding;

        [SerializeField] private GameObject badEndingScene;
        [SerializeField] private GameObject goodEndingScene;

        [SerializeField] private float firstTransition;
        [SerializeField] private float secondTransition;
        [SerializeField] private float thirdTransition;
        [SerializeField] private float fourthTransition;
        [SerializeField] private float fifthTransition;

        void Start()
        {
            sideCamOnPlayer = false;
            closeupCam = false;
            zoomedOutCam = false;
            blackout = false;
            badEnding = false;
            firstTransition = 4;
            secondTransition = 1;
            thirdTransition = 4;
            fourthTransition = 1.5f;
            fifthTransition = 2f;
            creditsSceneNum = 7;
        }

        void OnTriggerEnter(Collider player)
        {

            Save();
            if (player.gameObject.transform.CompareTag("Player"))
            {
                StartCoroutine(PlayFinalCutscene());
            }
        }

        IEnumerator PlayFinalCutscene()
        {
            for (int i =0 ; i<4 && !badEnding; i++)
            {
                badEnding = (int)MEDAL.BRONZE == FileSaver.Instance.GetActiveLevelMedals()[i];
            }
            if (badEnding)
            {
                badEndingScene.SetActive(true);
                Player.transform.position = finalLocation;
                Player.transform.rotation = Quaternion.Euler(Vector3.zero);
                bgMusic.Stop();
                bgMusic2.Play();
                newMovement.enabled = false;
                MainCamera.SetActive(false);
                SideCamera.SetActive(true);
                combatControl.OnDestroy();
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                inputManager.SetActive(false);
                sideCamOnPlayer = true;
                yield return new WaitForSecondsRealtime(firstTransition);
                sideCamOnPlayer = false;
                zoomedOutCam = true;
                Ninja.SetBool("IsSprinting", false);
                Ninja.SetBool("IsMoving", false);
                yield return new WaitForSecondsRealtime(secondTransition);
                zoomedOutCam = false;
                closeupCam = true;
                badEndingSound.Play();
                yield return new WaitForSecondsRealtime(thirdTransition);
                closeupCam = false;
                blackout = true;
                badEndingSound2.Play();
                yield return new WaitForSecondsRealtime(fourthTransition);
                blackout = false;
                zoomedOutCam = true;
                AliveNinja.SetActive(false);
                DeadNinja.SetActive(true);
                BadNinja.SetBool("StartAnimation", true);
                Ninja.SetBool("StartAnimation", true);
                GameOverText.SetActive(true);
                bgMusic2.Stop();
                GameOverMusic.Play();
                yield return new WaitForSecondsRealtime(fifthTransition);
            }
            else
            {
                goodEndingScene.SetActive(true);
                Player.transform.position = finalLocation;
                Player.transform.rotation = Quaternion.Euler(Vector3.zero);
                bgMusic.Stop();
                bgMusic2.Play();
                newMovement.enabled = false;
                MainCamera.SetActive(false);
                SideCamera.SetActive(true);
                combatControl.OnDestroy();
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                inputManager.SetActive(false);
                sideCamOnPlayer = true;
                print(Player.GetComponent<Rigidbody>().velocity);
                yield return new WaitForSecondsRealtime(firstTransition);
                print(Player.GetComponent<Rigidbody>().velocity);
                sideCamOnPlayer = false;
                zoomedOutCam = true;
                Ninja.SetBool("IsSprinting", false);
                Ninja.SetBool("IsMoving", false);
                yield return new WaitForSecondsRealtime(secondTransition);
                zoomedOutCam = false;
                closeupCam2 = true;
                goodEndingSound.Play();
                yield return new WaitForSecondsRealtime(thirdTransition);
                zoomedOutCam = true;
                GirlNinja.SetBool("StartGood", true);
                Ninja.SetBool("StartGood", true);
                GameOverText2.SetActive(true);
                bgMusic2.Stop();
                GameOverMusic2.Play();
                yield return new WaitForSecondsRealtime(fifthTransition);
            }
            GameStateManager.Instance.LoadLevel(creditsSceneNum);
        }

        void FixedUpdate()
        {
            if(sideCamOnPlayer)
            {
                Player.transform.position += new Vector3(0,0,walkingSpeed);
            }
            else if(zoomedOutCam)
            {
                SideCamera.SetActive(false);
                CloseUpCamera.SetActive(false);
                CloseUpCamera2.SetActive(false);
                BlackOutCamera.SetActive(false);
                ZoomedOutCamera.SetActive(true);
            }
            else if(closeupCam)
            {
                ZoomedOutCamera.SetActive(false);
                CloseUpCamera.SetActive(true);
            }
            else if(closeupCam2)
            {
                ZoomedOutCamera.SetActive(false);
                CloseUpCamera2.SetActive(true);
            }
            else if(blackout)
            {
                CloseUpCamera.SetActive(false);
                BlackOutCamera.SetActive(true);
            }
        }

      
    }
}
