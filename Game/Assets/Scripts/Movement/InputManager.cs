using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

namespace TeamNinja
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private NewMovement newMove;
        [SerializeField] private LookControl lookControl;
        [SerializeField] private CombatControl combatControl;
        [SerializeField] private PausePressedListener pauseListener;
        [SerializeField] private ShurikenSwitchListener switchListener;
        private Controls controls;

        private void Awake()
        {
            controls = new Controls();
            newMove.Initialize(controls.Movement.MoveTowards, controls.Movement.Jump, controls.Movement.Sprint, controls.Movement.Crouch);
            lookControl.Initialize(controls.Movement.LookTowards, controls.Movement.SwitchCamera);
            combatControl.Initialize(controls.Movement.PrimaryAttack, controls.Movement.SecondaryAttack);
            pauseListener.Initialize(controls.Movement.Pause);
            switchListener.Initialize(controls.Movement.SwitchShuriken);


        }

        

    }
}
