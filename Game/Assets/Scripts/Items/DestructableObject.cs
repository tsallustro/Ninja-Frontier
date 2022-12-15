using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace TeamNinja
{
    public class DestructableObject : MonoBehaviour
    {
        [SerializeField] private GameObject BrokenVersion;

        public void ShatterObject(Vector3 HitPosition)
        {
            GameObject DestroyedObject = Instantiate(BrokenVersion, transform.position, transform.rotation) as GameObject;
            if(DestroyedObject.TryGetComponent<AudioSource>(out AudioSource DestructionSound))
            {
                DestructionSound.Play();
            }
            foreach (Transform child in DestroyedObject.transform)
                if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigid))
                {
                    childRigid.AddExplosionForce(1000f, HitPosition, 4f);
                }
            Destroy(gameObject);
        }
    }
}
