/*
 * -----------------------------------------------------------------------------
 * Script Name: AnimateHandInput
 * Author: [Tu Nombre]
 * Created: [Fecha de Creación]
 * Description:
 *     Este script controla las animaciones de las manos (izquierda y derecha) 
 *     en un entorno XR. Conecta las entradas del controlador XR (grip y trigger) 
 *     con los parámetros correspondientes en los componentes Animator 
 *     de las manos. Esto permite que las manos del avatar reflejen las acciones 
 *     del jugador en tiempo real.
 * 
 * Features:
 *     - Sincroniza los valores de grip y trigger de los controladores XR con 
 *       las animaciones de las manos.
 *     - Funciona con controladores basados en Action (ActionBasedController).
 *     - Admite suscripción a eventos de entrada de forma modular.
 * 
 * Dependencies:
 *     - UnityEngine.XR.Interaction.Toolkit
 *     - UnityEngine.InputSystem
 * 
 * Usage:
 *     - Asegúrate de que las manos del personaje tienen un Animator con 
 *       parámetros "Grip" y "Trigger".
 *     - Asigna los controladores izquierdo y derecho desde el 
 *       CharacterControllManager.
 * 
 * -----------------------------------------------------------------------------
 */

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Inputs
{
    /// <summary>
    /// Controla las animaciones de las manos en función de las entradas del 
    /// controlador XR (grip y trigger).
    /// </summary>
    public class AnimateHandInput : MonoBehaviour
    {
        [Header("Left Hand")]
        [Tooltip("Referencia al Animator de la mano izquierda del personaje.")]
        public Animator leftHandAnimator;

        [Header("Right Hand")]
        [Tooltip("Referencia al Animator de la mano derecha del personaje.")]
        public Animator rightHandAnimator;

        // Controladores de XR para la mano izquierda y derecha.
        private ActionBasedController leftController;
        private ActionBasedController rightController;

        /// <summary>
        /// Enum para identificar la mano correspondiente (izquierda o derecha).
        /// </summary>
        public enum Hand { Left, Right };

        /// <summary>
        /// Inicializa referencias y suscribe eventos a los controladores XR.
        /// </summary>
        void Start()
        {
            // Asigna referencias a los controladores desde el CharacterControlManager.
            leftController = CharacterControllManager.instance.leftController;
            rightController = CharacterControllManager.instance.rightController;

            if (leftController != null && rightController != null)
            {
                // Suscribir eventos para el controlador derecho (grip y trigger).
                OnSuscribedEvents(rightController.selectActionValue.action, OnGripHand, Hand.Right);
                OnSuscribedEvents(rightController.activateActionValue.action, OnTriggerHand, Hand.Right);

                // Suscribir eventos para el controlador izquierdo (grip y trigger).
                OnSuscribedEvents(leftController.selectActionValue.action, OnGripHand, Hand.Left);
                OnSuscribedEvents(leftController.activateActionValue.action, OnTriggerHand, Hand.Left);
            }
            else
            {
                Debug.LogError("InputActionManager no asignado en AnimateHandInput.");
            }
        }

        /// <summary>
        /// Maneja el evento de grip para sincronizar con el Animator correspondiente.
        /// </summary>
        /// <param name="context">Contexto de entrada del controlador.</param>
        /// <param name="hand">Mano afectada (izquierda o derecha).</param>
        public void OnGripHand(InputAction.CallbackContext context, Hand hand)
        {
            float handGrabValue = context.ReadValue<float>(); // Lee el valor del grip.

            // Actualiza el valor del parámetro "Grip" en el Animator.
            if (hand == Hand.Left)
                leftHandAnimator.SetFloat("Grip", handGrabValue);
            else if (hand == Hand.Right)
                rightHandAnimator.SetFloat("Grip", handGrabValue);
        }

        /// <summary>
        /// Maneja el evento de trigger para sincronizar con el Animator correspondiente.
        /// </summary>
        /// <param name="context">Contexto de entrada del controlador.</param>
        /// <param name="hand">Mano afectada (izquierda o derecha).</param>
        public void OnTriggerHand(InputAction.CallbackContext context, Hand hand)
        {
            float handTriggerValue = context.ReadValue<float>(); // Lee el valor del trigger.

            // Actualiza el valor del parámetro "Trigger" en el Animator.
            if (hand == Hand.Left)
                leftHandAnimator.SetFloat("Trigger", handTriggerValue);
            else if (hand == Hand.Right)
                rightHandAnimator.SetFloat("Trigger", handTriggerValue);
        }

        /// <summary>
        /// Suscribe eventos de entrada a un callback con la mano especificada.
        /// </summary>
        /// <param name="action">Acción de entrada a suscribir.</param>
        /// <param name="callback">Callback que se ejecutará en cada evento.</param>
        /// <param name="hand">Mano asociada al evento.</param>
        public void OnSuscribedEvents(InputAction action, Action<InputAction.CallbackContext, Hand> callback, Hand hand)
        {
            action.started += context => callback(context, hand);
            action.performed += context => callback(context, hand);
            action.canceled += context => callback(context, hand);
        }
    }
}
