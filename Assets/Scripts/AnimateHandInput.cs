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
        [Tooltip("Reference to the RightHand of the character")]
        public Animator rightHandAnimator;

        [Tooltip("Reference to the XR Controller of LeftHand")]
        public ActionBasedController leftController;

        [Tooltip("Reference to the XR Controller of RightHand")]
        public ActionBasedController rightController;


        // Start is called before the first frame update
        void Start()
        {
            if (leftController != null && rightController != null)
            {
                leftController.selectAction.action.performed += OnLeftGripHand;
                leftController.selectAction.action.canceled += OnLeftGripHand;
                leftController.activateAction.action.performed += OnLeftTriggerHand;
                leftController.activateAction.action.canceled += OnLeftTriggerHand;

                rightController.selectAction.action.performed += OnRightGripHand;
                rightController.selectAction.action.canceled += OnRightGripHand;
                rightController.activateAction.action.performed += OnRightTriggerHand;
                rightController.activateAction.action.canceled += OnRightTriggerHand;
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

