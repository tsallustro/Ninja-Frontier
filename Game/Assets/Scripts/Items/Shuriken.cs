using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamNinja;

namespace TeamNinja
{
    public class Shuriken : BasicItem
    {
        public override void UseItem(Vector3 Position, Quaternion Rotation)
        {
            base.UseItem(Position, Rotation);
            gameObject.GetComponent<AudioSource>().Play();
            Body.AddForce(transform.forward * 30, ForceMode.Impulse);
            Body.AddTorque(transform.up * 100, ForceMode.Impulse);
        }

        protected override void DisableHitbox()
        {
            base.DisableHitbox();
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
        }
    }
}
