using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] GameObject[] ammo;
        [SerializeField] Transform initialBulletPosition;           
        [SerializeField] int clipSize = 5;
        private int currentBulletCount = 0;
        [SerializeField] float timeBetweenBullets = .1f;
        [SerializeField] float reloadTime = 1;
        [SerializeField] ParticleSystem smoke;
        private bool isShooting = false;

        public void StartShooting()
        {
            if(!isShooting)
            {
                isShooting = true;
                StartCoroutine(Shooting());
            }
        }
        public void StopShooting()
        {
            isShooting = false;
        }


        public IEnumerator Shooting()
        {
            while(isShooting)
            {
                currentBulletCount = 0;
                while (currentBulletCount < clipSize && isShooting)
                {
                    ammo[currentBulletCount].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    ammo[currentBulletCount].GetComponent<BasicItem>().UseItem(initialBulletPosition.transform.position, initialBulletPosition.transform.rotation);
                    smoke.Play();

                    currentBulletCount++;
                    yield return new WaitForSeconds(timeBetweenBullets);
                }
                yield return new WaitForSeconds(reloadTime);
            }
        }
    }
}
