using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class Level2Respawner : MonoBehaviour
    {
        [SerializeField] Checkpoint curCheckpoint;
        [SerializeField] GameObject player;
        [SerializeField] float minYValue = 0f;

        void Update()
        {
            if(player.transform.position.y < minYValue)
            {
                player.transform.position = curCheckpoint.transform.position;
            }
        }

        public void setCheckpoint(Checkpoint checkpoint)
        {
            curCheckpoint = checkpoint;
            minYValue = checkpoint.previousCheckpoint.transform.position.y;
        }
    }
}
