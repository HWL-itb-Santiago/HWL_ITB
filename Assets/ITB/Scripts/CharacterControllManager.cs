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

        [Header("Reference to Player Controllers")]
        [Tooltip("Controlador basado en acciones para la mano izquierda.")]
        public ActionBasedController leftController;

        [Tooltip("Controlador basado en acciones para la mano derecha.")]
        public ActionBasedController rightController;

        [Header("Reference to Distance Ray Controllers")]
        public XRRayInteractor distanceRayRightController;
        public XRRayInteractor distanceRayLeftController;

        [Header("Reference to Direct Ray Controllers")]
        public XRDirectInteractor directRayRightController;
        public XRDirectInteractor directRayLeftController;

        [Header("Reference to Player Teleport Action")]
        public InputActionReference leftControllerTeleport;
        // Estados internos de los modos de interacción.
        private bool isTeleport = false;       // Indica si el modo Teleport está activo.

        // Singleton para acceso global.
        public static CharacterControllManager Instance;

        /// <summary>
        /// Evento para notificar a otros scripts cuando el modo cambia.
        /// </summary>
        public event Action<bool> OnModeChanged;

        /// <summary>
        /// Configura el Singleton y establece los estados iniciales.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            isTeleport = false;
        }

        /// <summary>
        /// Actualiza los componentes activos según los estados de los modos.
        /// </summary>
        public void UpdateTeleportMode()
        {
            isTeleport = true;
            bool teleportActive = isTeleport;


            // Notificar a otros scripts sobre el cambio de modo.
            OnModeChanged(teleportActive);
        }

        public void UpdateContinuousMode()
        {
            isTeleport = false;
            bool teleportActive = isTeleport;

            // Notificar a otros scripts sobre el cambio de modo.
            OnModeChanged(teleportActive);
        }

        /// <summary>
        /// Suscribe un conjunto de eventos de entrada (started, performed, canceled) a un callback específico.
        /// </summary>
        /// <param name="action">Acción de entrada a suscribir.</param>
        /// <param name="callback">Callback que se ejecutará en cada evento.</param>
        public void SubscribeToAction(InputAction action, Action<InputAction.CallbackContext> callback)
        {
            // Se suscribe a los eventos de la acción de entrada.
            action.started += callback;
            action.performed += callback;
            action.canceled += callback;
        }

        /// <summary>
        /// Elimina la suscripción de eventos de entrada para la acción proporcionada.
        /// </summary>
        /// <param name="action">Acción de entrada de la cual se desea eliminar la suscripción.</param>
        /// <param name="callback">Callback que se debe desuscribir.</param>
        public void UnsubscribeFromAction(InputAction action, Action<InputAction.CallbackContext> callback)
        {
            // Se elimina la suscripción de los eventos de la acción de entrada.
            action.started -= callback;
            action.performed -= callback;
            action.canceled -= callback;
        }
    }
}
