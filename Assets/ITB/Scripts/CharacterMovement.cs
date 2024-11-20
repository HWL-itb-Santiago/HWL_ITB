
using TMPro;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("References to the Move Provider")]
        public ContinuousMoveProviderBase moveCharacter;
        private float minSpeed;
        private ActionBasedController controller;

        [Header("Character Movement Modifications")]
        [Tooltip("Maximun Speed Velocity of the character")]
        public float maxSpeed;
        public float actualSpeed;


        private void Awake()
        {
        }
        // Start is called before the first frame update
        void Start()
        {
            controller = CharacterControllManager.instance.leftController;
            minSpeed = moveCharacter.moveSpeed;
            actualSpeed = minSpeed;
            CharacterControllManager.instance.OnSuscribedEvents(controller.scaleToggleAction.action, Run);
        }

        private void Update()
        {
            
        }
        private void FixedUpdate()
        {
        }

        public void Run(InputAction.CallbackContext context)
        {
            if (moveCharacter != null)
            {
                float amount = context.ReadValue<float>();
                float newSpeed = minSpeed + (amount * maxSpeed);
                actualSpeed = Mathf.Clamp(Mathf.Lerp(minSpeed, newSpeed, 1f), minSpeed, maxSpeed);
            }
            moveCharacter.moveSpeed = actualSpeed;
        }
    }

}
