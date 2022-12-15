using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector3 destination;
        [SerializeField] private float speed = 1f;
        private Vector3 startingLocation;
        private Boolean up;
        private Boolean down;

        public void Start()
        {
            startingLocation = this.gameObject.transform.position;
            up = false;
            down = false;
            
        }
        public void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                up = true;
                down = false;
            }
        }

        public void OnTriggerExit(Collider col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                down = true;
                up = false;
            }
        }

        public void OnTriggerStay(Collider col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (up)
                {
                    this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, destination, speed * Time.deltaTime);
                }
                else if (down)
                {
                    this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, startingLocation, speed * Time.deltaTime);
                }
            }
        }
    }
}
