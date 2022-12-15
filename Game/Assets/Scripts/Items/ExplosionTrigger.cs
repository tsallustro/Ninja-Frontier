using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class ExplosionTrigger : MonoBehaviour
    {


        // called on the barrel and mine
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponentInParent<Health>().DamageTakenEvent.Invoke();
            }
            else if (other.transform.CompareTag("Destructable"))
            {
                other.transform.GetComponent<DestructableObject>().ShatterObject(transform.position);
            }
            else if(other.transform.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Health>().DeathEvent.Invoke();
            }
        }
    }
}
