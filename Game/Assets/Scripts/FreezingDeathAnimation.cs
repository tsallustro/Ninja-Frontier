using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class FreezingDeathAnimation : MonoBehaviour
    {
        [SerializeField] private Animator GirlNinja;
        [SerializeField] private float time;

        // Update is called once per frame
        void Update()
        {
            if(GirlNinja.GetBool("StartAnimation"))
            {
                StartCoroutine(PlayAni());
            }
        }

        IEnumerator PlayAni()
        {
            yield return new WaitForSecondsRealtime(time);
            GirlNinja.enabled = false;
        }
    }
}
