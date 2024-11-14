using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    public class CharacterMovement : MonoBehaviour
    {
        public DynamicMoveProvider dynamicMoveCharacter;
        private float minSpeed;
        [Header("Character Movement Modifications")]
        [Tooltip("Maximun Speed Velocity of the character")]
        public float maxSpeed;
        public float actualSpeed;

        public TMPro.TMP_Text text;

        // Start is called before the first frame update
        void Start()
        {
            minSpeed = dynamicMoveCharacter.moveSpeed;
            actualSpeed = minSpeed;
            CharacterControllManager.instance.leftController.scaleToggleAction.action.performed += Run;
            CharacterControllManager.instance.leftController.scaleToggleAction.action.canceled += Run;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            dynamicMoveCharacter.moveSpeed = actualSpeed;
            text.text = dynamicMoveCharacter.moveSpeed.ToString();
        }

        public void Run(InputAction.CallbackContext context)
        {
            if (dynamicMoveCharacter != null)
            {
                float amount = context.ReadValue<float>();
                float newSpeed = minSpeed + (amount * maxSpeed);
                actualSpeed = Mathf.Clamp(Mathf.Lerp(minSpeed, newSpeed, 1f), minSpeed, maxSpeed);
            }
        }
    }

}
