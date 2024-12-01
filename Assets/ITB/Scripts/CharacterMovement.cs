using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    /// Gestiona el movimiento de un personaje controlado por VR, permitiendo modificar la velocidad de movimiento en tiempo real 
    /// basado en la entrada del usuario.
    /// </summary>
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Movement Provider References")]
        [Tooltip("Componente que gestiona el movimiento continuo del personaje.")]
        public ContinuousMoveProviderBase moveCharacter;

        [Header("Movement Parameters")]
        [Tooltip("Velocidad m�xima permitida para el movimiento del personaje.")]
        public float maxSpeed;

        [Tooltip("Velocidad actual del personaje, calculada din�micamente.")]
        public float actualSpeed;

        // Velocidad m�nima permitida, derivada del proveedor de movimiento.
        private float minSpeed;

        // Controlador para la entrada del usuario.
        private ActionBasedController controller;

        /// <summary>
        /// Se invoca al iniciar el script. Inicializa las variables y configura los eventos de entrada.
        /// </summary>
        private void Start()
        {
            InitializeControllerReferences();
            InitializeMovementParameters();
            SubscribeToControllerEvents();
        }

        /// <summary>
        /// Inicializa la referencia al controlador y otros par�metros necesarios.
        /// </summary>
        private void InitializeControllerReferences()
        {
            // Asignar el controlador izquierdo desde el CharacterControllManager.
            controller = CharacterControllManager.Instance.leftController;

            // Verifica que la referencia al controlador no sea nula.
            if (controller == null)
            {
                Debug.LogError("Controlador izquierdo no asignado en CharacterControllManager.");
            }
        }

        /// <summary>
        /// Configura las velocidades m�nimas y actuales basadas en el proveedor de movimiento.
        /// </summary>
        private void InitializeMovementParameters()
        {
            // Se asegura de que el proveedor de movimiento est� configurado correctamente.
            if (moveCharacter != null)
            {
                minSpeed = moveCharacter.moveSpeed;
                actualSpeed = minSpeed; // Inicializa con la velocidad m�nima.
            }
            else
            {
                Debug.LogError("El proveedor de movimiento no est� asignado.");
            }
        }

        /// <summary>
        /// Suscribe los m�todos a los eventos de acci�n del controlador.
        /// </summary>
        private void SubscribeToControllerEvents()
        {
            // Se suscribe al evento 'scaleToggleAction' para ajustar la velocidad de movimiento.
            if (controller != null && controller.scaleToggleAction != null)
            {
                SubscribeToEvents(controller.scaleToggleAction.action, OnMovementInputReceived);
            }
        }

        /// <summary>
        /// M�todo que ajusta la velocidad del personaje basado en la entrada del usuario.
        /// </summary>
        /// <param name="context">El contexto de la acci�n de entrada.</param>
        private void OnMovementInputReceived(InputAction.CallbackContext context)
        {
            // Asegura que el proveedor de movimiento est� asignado antes de procesar la entrada.
            if (moveCharacter == null)
            {
                return;
            }

            // Lee el valor de la entrada del usuario, en el rango [0, 1].
            float inputAmount = context.ReadValue<float>();

            // Calcula la nueva velocidad del personaje basada en la entrada.
            float newSpeed = CalculateSpeedFromInput(inputAmount);

            // Actualiza la velocidad real del personaje.
            UpdateMovementSpeed(newSpeed);

            // Activa la retroalimentaci�n h�ptica del controlador.
            TriggerHapticFeedback();
        }

        /// <summary>
        /// Calcula la velocidad de movimiento basada en la entrada del usuario.
        /// </summary>
        /// <param name="inputAmount">Valor de entrada del usuario en el rango [0, 1].</param>
        /// <returns>La nueva velocidad calculada.</returns>
        private float CalculateSpeedFromInput(float inputAmount)
        {
            // Calcula la nueva velocidad considerando el valor de entrada y la velocidad m�xima configurada.
            float newSpeed = minSpeed + (inputAmount * maxSpeed);

            // Limita la velocidad dentro del rango permitido.
            return Mathf.Clamp(newSpeed, minSpeed, maxSpeed);
        }

        /// <summary>
        /// Actualiza la velocidad de movimiento en el proveedor de movimiento.
        /// </summary>
        /// <param name="newSpeed">La nueva velocidad que se establecer�.</param>
        private void UpdateMovementSpeed(float newSpeed)
        {
            // Actualiza la velocidad del proveedor de movimiento.
            moveCharacter.moveSpeed = newSpeed;

            // Actualiza la velocidad actual mostrada.
            actualSpeed = newSpeed;
        }

        /// <summary>
        /// Activa la retroalimentaci�n h�ptica en el controlador.
        /// </summary>
        private void TriggerHapticFeedback()
        {
            // Habilita la acci�n h�ptica del dispositivo de control.
            if (controller != null && controller.hapticDeviceAction != null)
            {
                controller.hapticDeviceAction.action.Enable();
            }
        }

        /// <summary>
        /// Suscribe un conjunto de eventos de entrada (started, performed, canceled) a un callback espec�fico.
        /// </summary>
        /// <param name="action">La acci�n de entrada a la que se suscribe.</param>
        /// <param name="callback">El callback que se ejecutar� cuando el evento ocurra.</param>
        public void SubscribeToEvents(InputAction action, Action<InputAction.CallbackContext> callback)
        {
            action.started += context => callback(context);
            action.performed += context => callback(context);
            action.canceled += context => callback(context);
        }
    }
}
