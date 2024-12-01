using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// Clase que gestiona el tipo de movimiento del jugador, permitiendo cambiar entre modos de Teletransporte y Movimiento Continuo.
    /// </summary>
    public class TypeOfMovement : MonoBehaviour
    {
        [Header("Reference to Distance Ray Interactor Controllers")]
        [Tooltip("Interactor de rayos para la mano izquierda.")]
        public XRRayInteractor distanceLeftInteractorRay;

        [Tooltip("Interactor de rayos para la mano derecha.")]
        public XRRayInteractor distanceRightInteractorRay;

        [Header("Reference to Teleport Controller")]
        [Tooltip("Controlador que gestiona el teletransporte.")]
        public ActionBasedController teleport;

        [Header("Reference to Movement Provider")]
        [Tooltip("Proveedor de movimiento continuo del personaje.")]
        public ContinuousMoveProviderBase continuous;

        [Header("Reference to SnapTurn and ContinuousTurn Providers")]
        [Tooltip("Proveedor de giro basado en Snap Turn.")]
        public SnapTurnProviderBase snapTurnProvider;

        [Tooltip("Proveedor de giro continuo.")]
        public ContinuousTurnProviderBase continuousTurnProvider;

        /// <summary>
        /// Referencia a la acción de entrada de selección de teletransporte.
        /// </summary>
        public InputActionReference teleportSelect;
        public InputActionReference move;

        private bool teleportActive = false;

        /// <summary>
        /// Inicializa los componentes y se suscribe al evento de cambio de modo de movimiento.
        /// </summary>
        void Start()
        {
            // Se suscribe al evento OnModeChanged que permite cambiar entre los modos de movimiento.
            CharacterControllManager.Instance.OnModeChanged += ChangedTypeOfMovement;
        }

        /// <summary>
        /// Método llamado para actualizar el estado de los componentes de movimiento.
        /// Este método no es necesario en este ejemplo, pero se ha dejado para mantener la estructura.
        /// </summary>
        void Update()
        {
        }

        /// <summary>
        /// Cambia el tipo de movimiento entre teletransporte y movimiento continuo basado en el parámetro isTeleport.
        /// </summary>
        /// <param name="isTeleport">Indica si se debe activar el modo teletransporte (true) o el movimiento continuo (false).</param>
        public void ChangedTypeOfMovement(bool isTeleport)
        {
            teleportActive = isTeleport;
            SetActiveComponents();
        }

        /// <summary>
        /// Activa o desactiva los componentes según el tipo de movimiento seleccionado (teletransporte o movimiento continuo).
        /// </summary>
        private void SetActiveComponents()
        {
            // Si el modo de teletransporte está activo, se suscribe a los eventos de entrada.
            if (teleportActive)
            {
                CharacterControllManager.Instance.SubscribeToAction(teleportSelect.action, SetTeleport);
                CharacterControllManager.Instance.UnsubscribeFromAction(move.action, SetContinuousMove);
            }
            else
            {
                CharacterControllManager.Instance.UnsubscribeFromAction(teleportSelect.action, SetTeleport);
                CharacterControllManager.Instance.SubscribeToAction(move.action, SetContinuousMove);
            }

            // Activa o desactiva los componentes de giro y movimiento según el modo seleccionado.
            snapTurnProvider.gameObject.SetActive(teleportActive);
            continuousTurnProvider.gameObject.SetActive(!teleportActive);
            continuous.gameObject.SetActive(!teleportActive);
        }



        /// <summary>
        /// Maneja la entrada del teletransporte. Cuando se activa el teletransporte, se activa el componente correspondiente.
        /// </summary>
        /// <param name="context">El contexto de la acción de entrada.</param>
        private void SetTeleport(InputAction.CallbackContext context)
        {
            // Se lee el valor del eje Y del Vector2, que indica si el teletransporte debe estar activo o no.
            float active = context.ReadValue<Vector2>().y;
            bool setActive = active > 0;

            // Se inicia la corutina para activar o desactivar el teletransporte con un pequeño retraso.
            StartCoroutine(DelayedActivation(teleport.gameObject, setActive, 0.1f));
        }

        private void SetContinuousMove(InputAction.CallbackContext context)
        {
            // Se lee el valor del eje Y del Vector2, que indica si el teletransporte debe estar activo o no.
            float active = context.ReadValue<Vector2>().y;
            bool setActive = active != 0;

            // Se inicia la corutina para activar o desactivar el teletransporte con un pequeño retraso.
            StartCoroutine(DelayedActivation(continuous.gameObject, setActive, 0.1f));
        }
        /// <summary>
        /// Activa o desactiva el componente Teleport después de un pequeño retraso.
        /// </summary>
        /// <param name="active">Indica si el componente debe activarse o desactivarse.</param>
        /// <param name="delay">El retraso en segundos antes de activar o desactivar el componente.</param>
        private IEnumerator DelayedActivation(GameObject obj, bool active, float delay)
        { 
            // Se espera el tiempo especificado antes de realizar la acción.
            yield return new WaitForSeconds(delay);

            // Activa o desactiva el objeto que contiene el componente de Teleport.
            obj.SetActive(active);
            distanceLeftInteractorRay.gameObject.SetActive(!active);
            distanceRightInteractorRay.gameObject.SetActive(!active);
        }
    }
}
