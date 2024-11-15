// AnimateHandInput.cs
// Este script controla la animación de las manos (izquierda y derecha) en respuesta a los eventos de entrada del controlador XR.
// Conecta los componentes Animator de cada mano a los eventos de entrada (grip y trigger) proporcionados por los controladores de XR.

using System;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Inputs
{
    public class AnimateHandInput : MonoBehaviour
    {
        [Header("Left Hand")]

        [Tooltip("Referencia al Animator de la mano izquierda del personaje")]
        public Animator leftHandAnimator;

        [Header("Right Hand")]

        [Tooltip("Referencia al Animator de la mano derecha del personaje")]
        public Animator rightHandAnimator;

        // Controladores de XR para la mano izquierda y derecha.
        private ActionBasedController leftController;
        private ActionBasedController rightController;

        // Enum para identificar la mano correspondiente (izquierda o derecha).
        public enum Hand { Left, Right };

        // Start se llama antes de la primera actualización de fotograma.
        void Start()
        {
            // Asignación de referencias a los controladores de cada mano desde el CharacterControlManager.
            leftController = CharacterControllManager.instance.leftController;
            rightController = CharacterControllManager.instance.rightController;

            // Suscripción de los eventos de entrada a las acciones correspondientes, si los controladores están configurados.
            if (leftController != null && rightController != null)
            {
                // Suscribir los eventos de entrada para el controlador derecho (grip y trigger).
                OnSuscribedEvents(rightController.selectActionValue.action, OnGripHand, Hand.Right);
                OnSuscribedEvents(rightController.activateActionValue.action, OnTriggerHand, Hand.Right);

                // Suscribir los eventos de entrada para el controlador izquierdo (grip y trigger).
                OnSuscribedEvents(leftController.selectActionValue.action, OnGripHand, Hand.Left);
                OnSuscribedEvents(leftController.activateActionValue.action, OnTriggerHand, Hand.Left);
            }
            else
            {
                Debug.LogError("InputActionManager no asignado en AnimateHandInput.");
            }
        }

        // Método para manejar el evento de grip. Recibe el contexto de entrada y la mano (izquierda o derecha).
        public void OnGripHand(InputAction.CallbackContext context, Hand hand)
        {
            float handGrabValue = context.ReadValue<float>(); // Lee el valor del grip.

            // Asigna el valor del grip al Animator correspondiente según la mano.
            if (hand == Hand.Left)
                leftHandAnimator.SetFloat("Grip", handGrabValue);
            else if (hand == Hand.Right)
                rightHandAnimator.SetFloat("Grip", handGrabValue);
        }

        // Método para manejar el evento de trigger. Recibe el contexto de entrada y la mano (izquierda o derecha).
        public void OnTriggerHand(InputAction.CallbackContext context, Hand hand)
        {
            float handTriggerValue = context.ReadValue<float>(); // Lee el valor del trigger.

            // Asigna el valor del trigger al Animator correspondiente según la mano.
            if (hand == Hand.Left)
                leftHandAnimator.SetFloat("Trigger", handTriggerValue);
            else if (hand == Hand.Right)
                rightHandAnimator.SetFloat("Trigger", handTriggerValue);
        }

        public void OnSuscribedEvents(InputAction action, Action<InputAction.CallbackContext, Hand> callback, Hand hand)
        {
            // Suscribe cada fase de la acción de entrada al callback con la mano especificada.
            action.started += context => callback(context, hand);
            action.performed += context => callback(context, hand);
            action.canceled += context => callback(context, hand);
        }
    }
}
