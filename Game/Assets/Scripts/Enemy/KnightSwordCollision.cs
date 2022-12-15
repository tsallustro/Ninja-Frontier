using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class KnightSwordCollision : MonoBehaviour
    {

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                collider.gameObject.GetComponentInParent<Health>().DamageTakenEvent.Invoke();
            }
        }
    }
}
