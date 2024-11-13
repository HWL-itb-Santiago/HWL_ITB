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
                leftController.selectActionValue.action.performed += OnLeftGripHand;
                leftController.selectActionValue.action.canceled += OnLeftGripHand;
                leftController.activateActionValue.action.performed += OnLeftTriggerHand;
                leftController.activateActionValue.action.canceled += OnLeftTriggerHand;

                rightController.selectActionValue.action.performed += OnRightGripHand;
                rightController.selectActionValue.action.canceled += OnRightGripHand;
                rightController.activateActionValue.action.performed += OnRightTriggerHand;
                rightController.activateActionValue.action.canceled += OnRightTriggerHand;
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
        public void OnLeftGripHand(InputAction.CallbackContext context)
        {
            float handGrabValue = context.ReadValue<float>();
            Debug.Log("Grab Value" + handGrabValue);
            leftHandAnimator.SetFloat("Grip", handGrabValue);
        }

        public void OnLeftTriggerHand(InputAction.CallbackContext context)
        {
            float handTriggerValue = context.ReadValue<float>();
            Debug.Log("Trigger Value" + handTriggerValue);
            leftHandAnimator.SetFloat("Trigger", handTriggerValue);
        }

        public void OnRightGripHand(InputAction.CallbackContext context)
        {
            float handGrabValue = context.ReadValue<float>();
            Debug.Log("Grab Value" + handGrabValue);
            rightHandAnimator.SetFloat("Grip", handGrabValue);
        }


        public void OnRightTriggerHand(InputAction.CallbackContext context)
        {
            float handTriggerValue = context.ReadValue<float>();
            Debug.Log("Trigger Value" + handTriggerValue);
            rightHandAnimator.SetFloat("Trigger", handTriggerValue);
        }
    }
}

