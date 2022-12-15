using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class Spike : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<Health>().DeathEvent.Invoke();
            }
        }

    }
}
