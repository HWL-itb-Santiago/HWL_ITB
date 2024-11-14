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
            leftController = CharacterControllManager.instance.leftController;
            rightController = CharacterControllManager.instance.rightController;
            if (leftController != null && rightController != null)
            {
                OnSuscribedEvents(rightController, Hand.Right);
                OnSuscribedEvents(leftController, Hand.Left);
            }
            else
            {
                Debug.LogError("InputActionManager no asignado en AnimateHandInput.");
            }
        }

        private void OnSuscribedEvents(ActionBasedController controller, Hand hand)
        {
            controller.selectActionValue.action.started -= callback => OnGripHand(callback, hand);
            controller.selectActionValue.action.performed += callback => OnGripHand(callback, hand);
            controller.selectActionValue.action.canceled += callback => OnGripHand(callback, hand);

            controller.activateActionValue.action.started -= callback => OnTriggerHand(callback, hand);
            controller.activateActionValue.action.performed += callback => OnTriggerHand(callback, hand);
            controller.activateActionValue.action.canceled += callback => OnTriggerHand(callback, hand);
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

