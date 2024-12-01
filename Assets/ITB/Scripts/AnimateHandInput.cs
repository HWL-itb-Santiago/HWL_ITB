using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Inputs
{
    /// <summary>
    /// Controla las animaciones de las manos en función de las entradas del 
    /// controlador XR (grip y trigger).
    /// </summary>
    [AddComponentMenu("XR/Hand Animation Input")]
    public class AnimateHandInput : MonoBehaviour
    {
        [Header("Hand Animators")]
        [Tooltip("Referencia al Animator de la mano izquierda del personaje.")]
        [SerializeField] private Animator leftHandAnimator;

        [Tooltip("Referencia al Animator de la mano derecha del personaje.")]
        [SerializeField] private Animator rightHandAnimator;

        private ActionBasedController leftController;
        private ActionBasedController rightController;

        /// <summary>
        /// Enum para identificar la mano correspondiente (izquierda o derecha).
        /// </summary>
        public enum Hand { Left, Right };

        /// <summary>
        /// Inicializa las referencias a los controladores y anima las manos en función de las entradas del controlador.
        /// </summary>
        private void Start()
        {
            InitializeControllers();
            SubscribeToInputEvents();
        }

        /// <summary>
        /// Inicializa las referencias a los controladores izquierdo y derecho.
        /// </summary>
        private void InitializeControllers()
        {
            leftController = CharacterControllManager.Instance?.leftController;
            rightController = CharacterControllManager.Instance?.rightController;

            if (leftController == null || rightController == null)
            {
                Debug.LogError("Controladores XR no asignados en AnimateHandInput.");
                return;
            }
        }

        /// <summary>
        /// Suscribe eventos de entrada a las acciones correspondientes de grip y trigger de cada mano.
        /// </summary>
        private void SubscribeToInputEvents()
        {
            if (leftController != null)
            {
                SubscribeToHandActions(leftController, Hand.Left);
            }

            if (rightController != null)
            {
                SubscribeToHandActions(rightController, Hand.Right);
            }
        }

        /// <summary>
        /// Suscribe las acciones de entrada del controlador para las manos.
        /// </summary>
        /// <param name="controller">Controlador XR a suscribir.</param>
        /// <param name="hand">Mano asociada al controlador.</param>
        private void SubscribeToHandActions(ActionBasedController controller, Hand hand)
        {
            SubscribeToAction(controller.selectActionValue.action, OnGripHand, hand);
            SubscribeToAction(controller.activateActionValue.action, OnTriggerHand, hand);
        }

        /// <summary>
        /// Suscribe eventos de acción de entrada a un callback para una mano específica.
        /// </summary>
        /// <param name="action">Acción de entrada a suscribir.</param>
        /// <param name="callback">Método que será invocado cuando la acción se ejecute.</param>
        /// <param name="hand">Mano asociada a la acción.</param>
        private void SubscribeToAction(InputAction action, Action<InputAction.CallbackContext, Hand> callback, Hand hand)
        {
            if (action != null)
            {
                action.started += context => callback(context, hand);
                action.performed += context => callback(context, hand);
                action.canceled += context => callback(context, hand);
            }
        }

        /// <summary>
        /// Maneja el evento de grip para sincronizar con el Animator correspondiente.
        /// </summary>
        /// <param name="context">Contexto de la entrada del controlador.</param>
        /// <param name="hand">Mano afectada (izquierda o derecha).</param>
        private void OnGripHand(InputAction.CallbackContext context, Hand hand)
        {
            float value = context.ReadValue<float>();
            GetAnimator(hand)?.SetFloat("Grip", value);
        }

        /// <summary>
        /// Maneja el evento de trigger para sincronizar con el Animator correspondiente.
        /// </summary>
        /// <param name="context">Contexto de la entrada del controlador.</param>
        /// <param name="hand">Mano afectada (izquierda o derecha).</param>
        private void OnTriggerHand(InputAction.CallbackContext context, Hand hand)
        {
            float value = context.ReadValue<float>();
            GetAnimator(hand)?.SetFloat("Trigger", value);
        }

        /// <summary>
        /// Obtiene el Animator correspondiente a la mano especificada.
        /// </summary>
        /// <param name="hand">Mano (izquierda o derecha).</param>
        /// <returns>El Animator asociado a la mano.</returns>
        private Animator GetAnimator(Hand hand)
        {
            return hand == Hand.Left ? leftHandAnimator : rightHandAnimator;
        }
    }
}
