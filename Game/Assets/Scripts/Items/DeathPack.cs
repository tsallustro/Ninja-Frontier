using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamNinja;

namespace TeamNinja
{
    public class DeathPack : BasicItem
    {
        public override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && canPickUp)
            {
                other.gameObject.GetComponentInParent<Health>().DecreaseHealth();
                gameObject.SetActive(false);
                //basic_item_pick_up.Play();
            }
        }

        protected override void DisableHitbox()
        {
            base.DisableHitbox();
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        }
    }
}
