/*
 * -----------------------------------------------------------------------------
 * Script Name: CharacterControllManager
 * Author: Santiago Vergara Rodriguez
 * Created: 21/11/2024
 * Description:
 *     Este script gestiona los modos de interacción en un entorno de VR. Permite 
 *     alternar entre el modo Teleport y el modo Distance Ray, activando o 
 *     desactivando los componentes relevantes para cada modo. Incluye un sistema 
 *     de notificación para informar a otros scripts sobre cambios en el modo actual.
 * 
 * Features:
 *     - Alterna entre modos de interacción (Teleport y Distance Ray).
 *     - Notifica cambios en el modo mediante eventos.
 *     - Controla la activación y desactivación de componentes según el modo seleccionado.
 * 
 * Dependencies:
 *     - UnityEngine.XR.Interaction.Toolkit
 *     - UnityEngine.InputSystem
 *     - TMPro (TextMeshPro para mostrar estados en tiempo real).
 * 
 * -----------------------------------------------------------------------------
 */

using System;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// Gestiona los modos de interacción en VR, alternando entre Teleport y Distance Ray.
    /// Controla la activación/desactivación de componentes según el modo actual y 
    /// notifica cambios a través de eventos.
    /// </summary>
    public class CharacterControllManager : MonoBehaviour
    {
        [Header("UI Reference")]
        [Tooltip("Referencia al texto que muestra los estados de interacción.")]
        public TMP_Text m_Text;

        [Header("Reference to Input Action Manager")]
        [Tooltip("Gestor de acciones de entrada configuradas en Input System.")]
        public InputActionManager inputActionManager;

        [Header("Reference to Distance Ray Interactor Controllers")]
        [Tooltip("Interactor de rayos para la mano izquierda.")]
        public XRRayInteractor distanceLeftInteractorRay;

        [Tooltip("Interactor de rayos para la mano derecha.")]
        public XRRayInteractor distanceRightInteractorRay;

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

        [Header("Reference to Player Controllers")]
        [Tooltip("Controlador basado en acciones para la mano izquierda.")]
        public ActionBasedController leftController;

        [Tooltip("Controlador basado en acciones para la mano derecha.")]
        public ActionBasedController rightController;

        // Estados internos de los modos de interacción.
        private bool isTeleport;       // Indica si el modo Teleport está activo.
        private bool isDistanceRay;   // Indica si el modo Distance Ray está activo.

        // Singleton para acceso global.
        public static CharacterControllManager instance;

        /// <summary>
        /// Evento para notificar a otros scripts cuando el modo cambia.
        /// </summary>
        public event Action<bool, bool> OnModeChanged;

        /// <summary>
        /// Configura el Singleton y establece los estados iniciales.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            isTeleport = false;
            isDistanceRay = false;
        }

        /// <summary>
        /// Inicializa los modos y suscribe eventos de entrada al inicio.
        /// </summary>
        void Start()
        {
            UpdateInteractionMode();
            SubscribeToInputActions();
        }

        /// <summary>
        /// Actualiza el texto UI para mostrar el estado actual de los modos.
        /// </summary>
        void Update()
        {
            Debug.Log($"Teleport Mode: {isTeleport}, Distance Ray: {isDistanceRay}");
            m_Text.text = $"Teleport: {isTeleport}, DistanceRay: {isDistanceRay}";
        }

        /// <summary>
        /// Suscribe los eventos de entrada para alternar entre modos.
        /// </summary>
        private void SubscribeToInputActions()
        {
            if (rightController != null)
                SubscribeToEvents(rightController.activateAction.action, UpdateDistanceRayMode);
            if (leftController != null)
                SubscribeToEvents(leftController.activateAction.action, UpdateTeleportMode);
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

        /// <summary>
        /// Actualiza el estado del modo Teleport en función de la entrada del usuario.
        /// </summary>
        /// <param name="context">Contexto de la entrada.</param>
        private void UpdateTeleportMode(InputAction.CallbackContext context)
        {
            isTeleport = context.performed;
            UpdateInteractionMode();
        }

        /// <summary>
        /// Actualiza el estado del modo Distance Ray en función de la entrada del usuario.
        /// </summary>
        /// <param name="context">Contexto de la entrada.</param>
        private void UpdateDistanceRayMode(InputAction.CallbackContext context)
        {
            isDistanceRay = context.performed;
            UpdateInteractionMode();
        }

        /// <summary>
        /// Actualiza los componentes activos según los estados de los modos.
        /// </summary>
        private void UpdateInteractionMode()
        {
            bool teleportActive = isTeleport && !isDistanceRay;
            bool distanceRayActive = isDistanceRay && !isTeleport;

            // Activar/desactivar componentes.
            SetActiveComponents(teleportActive, distanceRayActive);

            // Notificar a otros scripts sobre el cambio de modo.
            OnModeChanged?.Invoke(teleportActive, distanceRayActive);
        }

        /// <summary>
        /// Activa o desactiva los componentes según los modos seleccionados.
        /// </summary>
        /// <param name="teleportActive">Indica si el modo Teleport está activo.</param>
        /// <param name="distanceRayActive">Indica si el modo Distance Ray está activo.</param>
        private void SetActiveComponents(bool teleportActive, bool distanceRayActive)
        {
            Teleport.gameObject.SetActive(teleportActive);
            isSnapTurn.gameObject.SetActive(teleportActive);

            Continuous.gameObject.SetActive(!teleportActive);
            ContinuousTurn.gameObject.SetActive(!teleportActive);

            distanceLeftInteractorRay.gameObject.SetActive(distanceRayActive);
            distanceRightInteractorRay.gameObject.SetActive(distanceRayActive);
        }
    }
}
