using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TeamNinja
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnPoint;
        [SerializeField] private UnityEvent respawnEventStart;
        [SerializeField] private UnityEvent respawnEventEnd;
        [SerializeField] Transform objectToRespawn;
        [SerializeField] float timeBeforeRespawn = 0.1f;
        [SerializeField] float yPositionOffset = 5f;
        [SerializeField] private AudioSource backgroundMusic;
        [SerializeField] float respawnPlaneLevel = -5f;
        private bool respawning = false;

        public void Update()
        {
            if(objectToRespawn.transform.position.y <= respawnPlaneLevel)
            {
                RespawnEventStart();
            }
        }

        public void RespawnEventStart()
        {
            respawnEventStart.Invoke();
        }

        public void RespawnEventEnd()
        {
            respawnEventEnd.Invoke();
        }

        public void Respawn()
        {
            if (!respawning)
            {
                respawning = true;

                StartCoroutine(waitToRespawn());
            }
        }

        private IEnumerator waitToRespawn()
        {
            if(backgroundMusic != null) backgroundMusic.Stop();

            yield return new WaitForSecondsRealtime(timeBeforeRespawn);
            
            objectToRespawn.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y, respawnPoint.position.z + yPositionOffset);

            respawning = false;

            RespawnEventEnd();
            if(backgroundMusic != null) backgroundMusic.Play();
            yield return null;
        }

    }
}
