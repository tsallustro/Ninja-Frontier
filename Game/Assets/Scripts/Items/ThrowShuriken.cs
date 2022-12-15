using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class ThrowShuriken : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Shuriken;
        [SerializeField] private List<GameObject> ExpShuriken;
        private int indexOfShuriken = 0;
        private int overload;
        [SerializeField] private int numberOfHandShuriken;

        public void Start()
        {
            indexOfShuriken = 0;
        }

        public void AddShurikenToHand(GameObject obj)
        {
            if (obj.CompareTag("Shuriken") && indexOfShuriken < numberOfHandShuriken)
            {
                Shuriken[indexOfShuriken].SetActive(true);
                indexOfShuriken++;
            }
            else if (obj.CompareTag("ExplosiveShuriken") && indexOfShuriken < numberOfHandShuriken)
            {
                ExpShuriken[indexOfShuriken].SetActive(true);
                indexOfShuriken++;
            }
        }

        public void UseShuriken()
        {
            if (indexOfShuriken > 0)
            {
                indexOfShuriken--;
            }
            if (indexOfShuriken >= 0)
            {
                Shuriken[indexOfShuriken].SetActive(false);
                ExpShuriken[indexOfShuriken].SetActive(false);
            }
        }
    }
}
