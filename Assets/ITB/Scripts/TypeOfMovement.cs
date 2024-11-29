using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace UnityEngine.XR.Interaction.Toolkit
{

    public class TypeOfMovement : MonoBehaviour
    {
        [Header("Reference to Teleport Controller")]
        [Tooltip("Controlador que gestiona el teletransporte.")]
        public ActionBasedController Teleport;

        [Header("Reference to Movement Provider")]
        [Tooltip("Proveedor de movimiento continuo del personaje.")]
        public ContinuousMoveProviderBase Continuous;

        [Header("Reference to SnapTurn and ContinuousTurn Providers")]
        [Tooltip("Proveedor de giro basado en Snap Turn.")]
        public SnapTurnProviderBase isSnapTurn;

        [Tooltip("Proveedor de giro continuo.")]
        public ContinuousTurnProviderBase ContinuousTurn;

        public InputActionReference teleportSelect;

        private bool teleportActive = false;
        // Start is called before the first frame update
        void Start()
        {
            CharacterControllManager.instance.OnModeChanged += ChangedTypeOfMovement;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangedTypeOfMovement(bool isTeleport)
        {
            teleportActive = isTeleport;
            SetActiveComponents();
        }

        /// <summary>
        /// Activa o desactiva los componentes según los modos seleccionados.
        /// </summary>
        /// <param name="teleportActive">Indica si el modo Teleport está activo.</param>
        /// <param name="distanceRayActive">Indica si el modo Distance Ray está activo.</param>
        private void SetActiveComponents()
        {
            if (teleportActive)
                SubscribeToEvents(teleportSelect, SetTeleport);
            else
                UnsubscribeToEvents(teleportSelect, SetTeleport);
            isSnapTurn.gameObject.SetActive(teleportActive);

            Continuous.gameObject.SetActive(!teleportActive);
            ContinuousTurn.gameObject.SetActive(!teleportActive);

            //distanceLeftInteractorRay.gameObject.SetActive(distanceRayActive);
            //distanceRightInteractorRay.gameObject.SetActive(distanceRayActive);
        }


        /// <summary>
        /// Suscribe un conjunto de eventos de entrada (started, performed, canceled) a un callback.
        /// </summary>
        /// <param name="action">Acción de entrada a suscribir.</param>
        /// <param name="callback">Callback que se ejecutará en cada evento.</param>
        public void SubscribeToEvents(InputAction action, Action<InputAction.CallbackContext> callback)
        {
            action.started += context => callback(context);
            action.performed += context => callback(context);
            action.canceled += context => callback(context);
        }

        public void UnsubscribeToEvents(InputAction action, Action<InputAction.CallbackContext> callback)
        {
            action.started -= context => callback(context);
            action.performed -= context => callback(context);
            action.canceled -= context => callback(context);
        }

        private void SetTeleport(InputAction.CallbackContext context)
        {
            float active = context.ReadValue<Vector2>().y;
            bool setActive = active > 0;
            StartCoroutine(DelayedSetActive(setActive, 0.1f));
        }

        private IEnumerator DelayedSetActive(bool active, float delay)
        {
            yield return new WaitForSeconds(delay);
            Teleport.gameObject.SetActive(active);
        }

    }
}
