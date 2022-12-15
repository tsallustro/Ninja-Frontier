using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class DeathManager : MonoBehaviour
    {

        [SerializeField] private GameObject deathCanvas;


        private void Start()
        {
            deathCanvas.GetComponent<Canvas>().enabled = false;
        }

        public void ShowDeathCanvas()
        {
            print("Death");
            deathCanvas.GetComponent<Canvas>().enabled = true;
        }

        public void HideDeathCanvas()
        {
            deathCanvas.GetComponent<Canvas>().enabled = false;
        }
    }
}
