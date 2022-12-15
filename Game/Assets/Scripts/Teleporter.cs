using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] GameObject teleportExit;
        [SerializeField] GameObject player;

        private void OnTriggerEnter(Collider other)
        {
            player.transform.position = teleportExit.transform.position;
            player.transform.rotation = teleportExit.transform.rotation;
        }
    }
}
