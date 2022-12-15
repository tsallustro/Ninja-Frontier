using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace TeamNinja
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] protected AudioSource basic_item_pick_up;
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private List<GameObject> HandShuriken;
        [SerializeField] private List<GameObject> HandExpShuriken;
        [SerializeField] private int numShuriken;
        [SerializeField] UnityEvent<Dictionary<string, int>> QuantityChangeEvent;
        [SerializeField] private GameObject shurikenPrefab;
        [SerializeField] private GameObject expShurikenPrefab;
        Dictionary<string, int> masterItemList = new();
        private int CurrentItem = 1;

        private void Start()
        {
            
            masterItemList.Add("Shuriken", 0);
            masterItemList.Add("ExplosiveShuriken", 0);
        }
        public void UseItem()
        {
            if (CurrentItem == 1 && masterItemList["Shuriken"] > 0)
            {
                masterItemList["Shuriken"]--;
                Instantiate(shurikenPrefab).GetComponent<Shuriken>().UseItem(mainCamera.transform.position, mainCamera.transform.rotation);
                QuantityChangeEvent.Invoke(masterItemList);
                if(masterItemList["Shuriken"] < numShuriken)
                    HandShuriken[masterItemList["Shuriken"]].SetActive(false);

            }
            else if (CurrentItem == -1 && masterItemList["ExplosiveShuriken"] > 0)
            {
                masterItemList["ExplosiveShuriken"]--;
                Instantiate(expShurikenPrefab).GetComponent<ExplodingShuriken>().UseItem(mainCamera.transform.position, mainCamera.transform.rotation);
                QuantityChangeEvent.Invoke(masterItemList);
                if (masterItemList["ExplosiveShuriken"] < numShuriken)
                    HandExpShuriken[masterItemList["ExplosiveShuriken"]].SetActive(false);
            }


        }

        public void SwitchItem()
        {
            if (CurrentItem == 1)
                foreach (GameObject element in HandShuriken)
                    element.SetActive(false);

            if (CurrentItem == -1)
                foreach (GameObject element in HandExpShuriken)
                    element.SetActive(false);

            CurrentItem = -CurrentItem;
            PopulateHand();


        }

        public void AddItem(BasicItem ItemToAdd)
        {
            basic_item_pick_up.Play();
            masterItemList[ItemToAdd.tag]++;
            if(masterItemList[ItemToAdd.tag] <= 4)
            {
                if (CurrentItem == 1 && ItemToAdd.CompareTag("Shuriken"))
                    HandShuriken[masterItemList["Shuriken"] - 1].SetActive(true);
                if (CurrentItem == -1 && ItemToAdd.CompareTag("ExplosiveShuriken"))
                    HandExpShuriken[masterItemList["ExplosiveShuriken"] - 1].SetActive(true);
            }


            QuantityChangeEvent.Invoke(masterItemList);

        }

        private void PopulateHand()
        {
            int index = 0;
            if (CurrentItem == 1)
            {
                while (index < masterItemList["Shuriken"] && index < numShuriken)
                {
                    HandShuriken[index].SetActive(true);
                    index++;
                }
            }
            else if (CurrentItem == -1)
            {
                while (index < masterItemList["ExplosiveShuriken"] && index < numShuriken)
                {
                    HandExpShuriken[index].SetActive(true);
                    index++;
                }
            }



        }
    }
}
