using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    public class CharacterMovement : MonoBehaviour
    {
        public DynamicMoveProvider dynamicMoveCharacter;
        private float actualSpeed;
        [Header("Character Movement Modifications")]
        [Tooltip("Maximun Speed Velocity of the character")]
        public float maxSpeed;

        // Start is called before the first frame update
        void Start()
        {
            actualSpeed = dynamicMoveCharacter.moveSpeed;
            CharacterControllManager.instance.leftController.selectActionValue.action.performed += Run;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {

        }

        public void Run(InputAction.CallbackContext context)
        {
            if (dynamicMoveCharacter != null)
            {
                float amount = context.ReadValue<float>();
                dynamicMoveCharacter.moveSpeed = Mathf.Lerp(actualSpeed, maxSpeed, 0.2f);
                actualSpeed = dynamicMoveCharacter.moveSpeed;
            }
        }
    }

}
