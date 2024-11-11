using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

    public class AnimateHandInput : MonoBehaviour
    {
        public Animator handAnimator;
        public InputActionReference handActionGrab;
        public InputActionReference handActionTrigger;
        // Start is called before the first frame update
        void OnEnable()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        private void FixedUpdate()
        {
        }

        public void OnActivateValue(InputAction.CallbackContext context)
        {
            float handGrabValue = context.ReadValue<float>();
            handAnimator.SetFloat("Grip", handGrabValue);
        }

        public void OnActivate(InputAction.CallbackContext context)
        {
            float handTriggerValue = context.ReadValue<float>();
            handAnimator.SetFloat("Trigger", handTriggerValue);
        }
    }

