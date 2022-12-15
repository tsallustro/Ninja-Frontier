using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class ExplodingShuriken : BasicItem
    {
        public override void UseItem(Vector3 Position, Quaternion Rotation)
        {
            base.UseItem(Position, Rotation);
            gameObject.GetComponent<AudioSource>().Play();
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 30, ForceMode.Impulse);
            rb.AddTorque(transform.up * 100, ForceMode.Impulse);
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if(other.transform.tag == "Destructable")
            {
                other.transform.GetComponent<DestructableObject>().ShatterObject(transform.position);
                Destroy(gameObject);
            }
        }

        protected override void DisableHitbox()
        {
            base.DisableHitbox();
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
        }
    }
}
