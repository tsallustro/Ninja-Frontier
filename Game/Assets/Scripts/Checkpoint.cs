using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] Level2Respawner l2r;
        [SerializeField] public Checkpoint previousCheckpoint;

        private void OnTriggerEnter(Collider other)
        {
            l2r.setCheckpoint(this);
            this.GetComponent<MeshRenderer>().enabled = false; //Poof
        }

    }
}
