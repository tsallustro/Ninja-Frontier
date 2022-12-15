using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class LookToward : MonoBehaviour
    {
        private bool isLooking = false;
        [SerializeField] Transform target;
        [SerializeField] float speed;
        [SerializeField] Transform turretHorizontal;
        [SerializeField] Transform turretVertical;
        [SerializeField] Transform currentPosition;
        [SerializeField] bool isTurret;

        public IEnumerator Looking()
        {
            while(isLooking)
            {
                if (isTurret)
                    RotateTurret();
                else
                    RotateEnemy();

                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }

        private void RotateTurret()
        {
            Vector3 dir = target.position - currentPosition.position;

            Quaternion rotation = Quaternion.LookRotation(dir);

            turretHorizontal.rotation = Quaternion.Lerp(turretHorizontal.rotation, rotation, speed * Time.fixedDeltaTime);
            turretHorizontal.localEulerAngles = new Vector3(0, turretHorizontal.localEulerAngles.y, 0);


            turretVertical.rotation = Quaternion.Lerp(turretVertical.rotation, rotation, speed * Time.fixedDeltaTime);
            turretVertical.localEulerAngles = new Vector3(turretVertical.localEulerAngles.x, 0, 0);


        }
        private void RotateEnemy()
        {
            Vector3 dir = target.position - currentPosition.position;

            Quaternion rotation = Quaternion.LookRotation(dir);

            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotation, speed * Time.fixedDeltaTime);
            gameObject.transform.localEulerAngles = new Vector3(0, gameObject.transform.localEulerAngles.y, 0);

        }
        public void SetLooking(bool isLooking)
        {
            // dont need to set this.isLooking if they are already the same 
            if (this.isLooking != isLooking)
            {
                this.isLooking = isLooking;


                if(isLooking)
                {
                    StartCoroutine(Looking());
                }
            }
        }

    }
}
