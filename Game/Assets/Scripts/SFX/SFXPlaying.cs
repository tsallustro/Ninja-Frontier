using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class SFXPlaying : MonoBehaviour
    {
        [SerializeField] public AudioSource walking;
        [SerializeField] public AudioSource end_Goal;

        public void Play_Walking()
        {
            walking.Play();
            Debug.Log("Walking sound");
        }

        public void Stop_Walking()
        {
            walking.Stop();
            Debug.Log("Walking sound stopped");

        }

        public void Play_End_Goal()
        {
            end_Goal.Play();
        }
    }
}
