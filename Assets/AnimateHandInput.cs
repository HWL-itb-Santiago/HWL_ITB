using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Inputs
{
    public class AnimateHandInput : MonoBehaviour
    {
        public Animator handAnimator;
        public InputActionManager inputActionManager;
        public InputActionReference handActionGrab;
        public InputActionReference handActionTrigger;
        // Start is called before the first frame update
        void Start()
        {
            if (inputActionManager != null)
            {
                // Suscribirse a los eventos de las acciones
                if (handActionGrab != null)
                {
                    handActionGrab.action.performed += OnGripHand;
                    handActionGrab.action.canceled += OnGripHand;
                }
                if (handActionTrigger != null)
                {
                    handActionTrigger.action.performed += OnTriggerHand;
                    handActionTrigger.action.canceled += OnTriggerHand;
                }
            }
            else
            {
                Debug.LogError("InputActionManager no asignado en AnimateHandInput.");
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
        private void FixedUpdate()
        {
        }
        public void OnGripHand(InputAction.CallbackContext context)
        {
            float handGrabValue = context.ReadValue<float>();
            Debug.Log("Grab Value" + handGrabValue);
            handAnimator.SetFloat("Grip", handGrabValue);
        }

        public void OnTriggerHand(InputAction.CallbackContext context)
        {
            float handTriggerValue = context.ReadValue<float>();
            Debug.Log("Trigger Value" + handTriggerValue);
            handAnimator.SetFloat("Trigger", handTriggerValue);
        }

    }
}

