using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TeamNinja
{
    public class LookControl : MonoBehaviour
    {
        [SerializeField] GameObject character;
        [SerializeField] Camera playercamera;
        [SerializeField] Camera thirdPersonCamera;
        private InputAction lookAction;
        private InputAction cameraSwitchAction;

        private float lookAngle = 0f;
        private float minAngle = -90f;
        private float maxAngle = 90f;

        public void Initialize(InputAction lookAction, InputAction cameraSwitchAction)
        {
            this.lookAction = lookAction;
            this.cameraSwitchAction = cameraSwitchAction;
            lookAction.Enable();
            playercamera.enabled = true;
            thirdPersonCamera.enabled = false;
            cameraSwitchAction.Enable();
            cameraSwitchAction.performed += SwitchCamera;
        }
        private void OnDestroy()
        {
            cameraSwitchAction.performed -= SwitchCamera;
        }

        private void Update()
        {
            Vector2 input = lookAction.ReadValue<Vector2>();
            if (playercamera.enabled)
            {
                lookAngle -= input.y * GameStateManager.Instance.sensitivity * Time.fixedDeltaTime;
                lookAngle = Mathf.Clamp(lookAngle, minAngle, maxAngle);
                playercamera.transform.localRotation = Quaternion.Euler(lookAngle, 0f, playercamera.transform.rotation.eulerAngles.z);
                character.transform.Rotate(0f, input.x * GameStateManager.Instance.sensitivity * Time.fixedDeltaTime, 0f);
            }
            if (thirdPersonCamera.enabled)
            {
                float targetAngle = thirdPersonCamera.transform.eulerAngles.y;
                character.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            }
        }

        private void SwitchCamera(InputAction.CallbackContext obj)
        {
            playercamera.enabled = !playercamera.enabled;
            thirdPersonCamera.enabled = !thirdPersonCamera.enabled;
        }
    }
}