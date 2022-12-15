using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
namespace TeamNinja
{
    public class ShurikenSwitchListener : MonoBehaviour
    {
        [SerializeField] UnityEvent OnSwitch;
        static bool wasInitialized = false;
        InputAction localcontrol;
        public void Initialize(InputAction switchAction)
        {
            if (!wasInitialized)
            {
                localcontrol = switchAction;
                localcontrol.Enable();
                localcontrol.performed += OnSwitchPressed;
                wasInitialized = true;

            }
        }

        private void OnSwitchPressed(InputAction.CallbackContext context)
        {
            Debug.Log("SHURIKEN SWITCH REGISTERED");
           
            OnSwitch.Invoke();

        }
    }
}
