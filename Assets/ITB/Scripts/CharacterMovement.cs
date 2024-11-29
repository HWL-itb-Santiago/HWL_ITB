/*
 * -----------------------------------------------------------------------------
 * Script Name: CharacterMovement
 * Author: Santiago Vergara Rodriguez
 * Created: 21/11/2024
 * Description:
 *     Este script gestiona el movimiento del personaje en un entorno de VR. 
 *     Permite ajustar din�micamente la velocidad del personaje bas�ndose en 
 *     las entradas del controlador. Utiliza un ContinuousMoveProviderBase 
 *     para controlar el movimiento continuo, modificando la velocidad en tiempo 
 *     real seg�n los valores de entrada.
 * 
 * Features:
 *     - Ajuste din�mico de velocidad basado en las entradas del usuario.
 *     - Soporte para velocidades m�nimas y m�ximas configurables.
 *     - Implementaci�n de retroalimentaci�n h�ptica en el controlador.
 *     - Totalmente integrable con el sistema CharacterControllManager.
 * 
 * Dependencies:
 *     - UnityEngine.XR.Interaction.Toolkit
 *     - UnityEngine.InputSystem
 *     - CharacterControllManager (script externo que proporciona referencias de controladores).
 * 
 * -----------------------------------------------------------------------------
 */
using System;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    /// Gestiona el movimiento de un personaje controlado mediante un sistema de VR, 
    /// permitiendo modificar la velocidad de movimiento en tiempo real basado en la entrada del usuario.
    /// </summary>
    public class CharacterMovement : MonoBehaviour
    {
        [Header("References to the Move Provider")]
        [Tooltip("Referencia al componente que gestiona el movimiento continuo del personaje.")]
        public ContinuousMoveProviderBase moveCharacter;

        // Velocidad m�nima permitida del personaje, obtenida del proveedor de movimiento.
        private float minSpeed;

        // Referencia al controlador que recibe las entradas del usuario.
        private ActionBasedController controller;

        [Header("Character Movement Modifications")]
        [Tooltip("Velocidad m�xima permitida para el movimiento del personaje.")]
        public float maxSpeed;

        [Tooltip("Velocidad actual del personaje, calculada din�micamente.")]
        public float actualSpeed;

        /// <summary>
        /// M�todo invocado al cargar el script.
        /// </summary>
        private void Awake()
        {
            // No se inicializan valores aqu� por ahora.
        }

        /// <summary>
        /// M�todo invocado al inicio del ciclo de vida del script. Se inicializan variables 
        /// y se suscriben eventos a las acciones de entrada del controlador.
        /// </summary>
        void Start()
        {
            // Obtiene la referencia al controlador izquierdo desde el CharacterControlManager.
            controller = CharacterControllManager.instance.leftController;

            // Configura las velocidades iniciales basadas en el proveedor de movimiento.
            minSpeed = moveCharacter.moveSpeed;
            actualSpeed = minSpeed;

            // Suscribe el m�todo Run a la acci�n de entrada scaleToggleAction del controlador.
            SubscribeToEvents(controller.scaleToggleAction.action, Run);
        }

        /// <summary>
        /// M�todo que ajusta din�micamente la velocidad del personaje basado en la entrada del usuario.
        /// </summary>
        /// <param name="context">Contexto que contiene la informaci�n del evento de entrada.</param>
        public void Run(InputAction.CallbackContext context)
        {
            // Verifica que el proveedor de movimiento est� asignado.
            if (moveCharacter != null)
            {
                // Lee el valor de entrada del usuario, que deber�a estar en un rango [0, 1].
                float amount = context.ReadValue<float>();

                // Calcula la nueva velocidad basada en la entrada del usuario y la velocidad m�xima configurada.
                float newSpeed = minSpeed + (amount * maxSpeed);

                // Asegura que la velocidad calculada se encuentra dentro del rango permitido.
                actualSpeed = Mathf.Clamp(Mathf.Lerp(minSpeed, newSpeed, 1f), minSpeed, maxSpeed);

                // Actualiza la velocidad del proveedor de movimiento.
                moveCharacter.moveSpeed = actualSpeed;

                // Habilita una acci�n h�ptica asociada al controlador para proporcionar retroalimentaci�n al usuario.
                controller.hapticDeviceAction.action.Enable();
            }
        }

        public void SubscribeToEvents(InputAction action, Action<InputAction.CallbackContext> callback)
        {
            action.started += context => callback(context);
            action.performed += context => callback(context);
            action.canceled += context => callback(context);
        }
    }
}
