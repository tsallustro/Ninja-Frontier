using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace TeamNinja
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance { get; private set; }

        [SerializeField] private GameObject[] ItemDrops;

        public void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void DropItem(Vector3 enemyPosition)
        {
            print("Item Drop");
            int randomVal = UnityEngine.Random.Range(0, ItemDrops.Length);
            Instantiate(ItemDrops[randomVal], enemyPosition + Vector3.up, Quaternion.identity);
            print(randomVal);
        }
    }
}
