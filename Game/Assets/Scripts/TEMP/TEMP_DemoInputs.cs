using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class TEMP_DemoInputs : MonoBehaviour
    {
        [SerializeField] NewMovement nm;
        [SerializeField] TEMP_LookCamera tlc;

        private TEMP_DemoControls controls;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void Awake()
        {
            controls = new();
            nm.Initialize(controls.Input.Move, controls.Input.Jump, controls.Input.Sprint, controls.Input.Crouch); ;
            tlc.Initialize(controls.Input.Look);
        }
    }
}
