using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TeamNinja
{
    public class PausePressedListener : MonoBehaviour
    {
        static bool wasInitialized = false;
        InputAction localPauseControl;
        public void Initialize(InputAction pause)
        {
            if (!wasInitialized)
            {
                localPauseControl = pause;
                localPauseControl.Enable();
                localPauseControl.performed += OnPausePressed;
                wasInitialized = true;

            }
        }

        private void OnPausePressed(InputAction.CallbackContext context)
        {
            Debug.Log("PAUSE EVENT REGISTERED");
            Pauser.TogglePause();
            if (Pauser.GetPauseState()) GameUIManager.ShowPauseMenu();
            else GameUIManager.HidePauseAndSettings();

        }
    }
}
