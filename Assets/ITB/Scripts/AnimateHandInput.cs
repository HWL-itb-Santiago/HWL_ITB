using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Inputs
{
    public class AnimateHandInput : MonoBehaviour
    {
        [Header("Left Hand")]

        [Tooltip("Reference to the LeftHand of the character")]
        public Animator leftHandAnimator;

        [Header("Right Hand")]

        [Tooltip("Reference to the RightHand of the character")]
        public Animator rightHandAnimator;


        // XRController leftController;
        private ActionBasedController leftController;

        // XRController rightController;
        private ActionBasedController rightController;

        public enum Hand {Left, Right};

        private void Awake()
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            // Asignacion referencia left controller+
            leftController = CharacterControllManager.instance.leftController;
            rightController = CharacterControllManager.instance.rightController;
            //Suscripcion de los eventos
            if (leftController != null && rightController != null)
            {
                OnSuscribedEvents(rightController.selectActionValue.action, OnGripHand, Hand.Right);
                OnSuscribedEvents(rightController.activateActionValue.action, OnTriggerHand, Hand.Right);

                OnSuscribedEvents(leftController.selectActionValue.action, OnGripHand, Hand.Left);
                OnSuscribedEvents(leftController.activateActionValue.action, OnTriggerHand, Hand.Left);
            }
            else
            {
                Debug.LogError("InputActionManager no asignado en AnimateHandInput.");
            }
        }

        private void OnSuscribedEvents(InputAction action, Action<InputAction.CallbackContext, Hand> callback, Hand hand)
        {
            action.started += context => callback(context, hand);
            action.performed += context => callback(context, hand);
            action.canceled += context => callback(context, hand);
        }
        // Update is called once per frame
        void Update()
        {
        }
        private void FixedUpdate()
        {
        }
        public void OnGripHand(InputAction.CallbackContext context, Hand hand)
        {
            float handGrabValue = context.ReadValue<float>();
            if (hand == Hand.Left)
                leftHandAnimator.SetFloat("Grip", handGrabValue);
            else if (hand == Hand.Right)
                rightHandAnimator.SetFloat("Grip", handGrabValue);
        }

        public void OnTriggerHand(InputAction.CallbackContext context, Hand hand)
        {
            float handTriggerValue = context.ReadValue<float>();
            if (hand == Hand.Left)
                leftHandAnimator.SetFloat("Trigger", handTriggerValue);
            else if (hand == Hand.Right)
                rightHandAnimator.SetFloat("Trigger", handTriggerValue);
        }
    }
}

