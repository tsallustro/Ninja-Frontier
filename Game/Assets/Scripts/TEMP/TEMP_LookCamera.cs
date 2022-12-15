using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TeamNinja
{
    public class TEMP_LookCamera : MonoBehaviour
    {
        [SerializeField] Transform character;

        private InputAction lookAction;

        private float xRot = 0f;
        private float yRot = 0f;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Initialize(InputAction lookAction)
        {
            this.lookAction = lookAction;
            lookAction.Enable();
        }

        // Update is called once per frame
        void Update()
        {
            yRot += lookAction.ReadValue<Vector2>().x;

            xRot -= lookAction.ReadValue<Vector2>().y;
            xRot = Mathf.Clamp(xRot, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRot, yRot, transform.rotation.eulerAngles.z);
            character.rotation = Quaternion.Euler(0f, yRot, 0f);
        }
    }
}
