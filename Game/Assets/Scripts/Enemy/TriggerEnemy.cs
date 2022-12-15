using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class TriggerEnemy : MonoBehaviour
    {

        [SerializeField] LookToward lookToward;
        [SerializeField] Shoot shoot;
        [SerializeField] Animator turret;


        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                turret.SetBool("playerFound", true);
                lookToward.SetLooking(true);
                shoot.StartShooting();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                turret.SetBool("playerFound", false);
                lookToward.SetLooking(false);
                shoot.StopShooting();
            }
        }
    }
}
