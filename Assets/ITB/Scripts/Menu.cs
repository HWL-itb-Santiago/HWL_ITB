using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Inputs
{
    /// <summary>
    /// Gestiona la apertura y cierre de un men� basado en el tiempo que el jugador
    /// permanece apuntando u "hovereando" sobre un objeto interactivo.
    /// </summary>
    public class Menu : MonoBehaviour
    {
        [Header("Time Settings")]
        [Tooltip("Cantidad de tiempo (en segundos) que el jugador debe mantener el puntero sobre el objeto para abrir/cerrar el men�.")]
        public float timeToActivate = 2.0f; // Tiempo predeterminado en segundos.

        // Variable que controla el tiempo acumulado mientras el puntero est� sobre el objeto.
        private float currentTime;

        [Header("Menu Object")]
        [Tooltip("El men� que se activar� o desactivar�.")]
        [SerializeField]
        private GameObject menu;

        // Referencia a la Coroutine que controla el tiempo de activaci�n.
        private Coroutine timeCoroutine;

        /// <summary>
        /// Inicializa variables al inicio.
        /// </summary>
        private void Start()
        {
            currentTime = 0f; // Aseg�rate de que el contador comience en 0.
        }

        /// <summary>
        /// Evento llamado cuando el jugador comienza a "hoverear" sobre el objeto.
        /// Inicia la Coroutine que cuenta el tiempo para activar el men�.
        /// </summary>
        public void OnHover()
        {
            // Inicia la Coroutine solo si no hay otra en ejecuci�n.
            if (timeCoroutine == null)
            {
                timeCoroutine = StartCoroutine(nameof(OnMenu));
            }
        }

        /// <summary>
        /// Evento llamado cuando el jugador deja de "hoverear" sobre el objeto.
        /// Detiene la Coroutine y reinicia el contador de tiempo.
        /// </summary>
        public void OffHover()
        {
            if (timeCoroutine != null)
            {
                StopCoroutine(timeCoroutine);
                timeCoroutine = null; // Limpia la referencia para evitar m�ltiples Coroutines.
            }
            currentTime = 0f; // Reinicia el tiempo acumulado.
        }

        /// <summary>
        /// Coroutine que gestiona el tiempo necesario para activar/desactivar el men�.
        /// </summary>
        /// <returns>Una enumeraci�n que controla el flujo del tiempo.</returns>
        private IEnumerator OnMenu()
        {
            // Mientras el tiempo acumulado sea menor al tiempo requerido para activar el men�.
            while (currentTime < timeToActivate)
            {
                // Incrementa el tiempo acumulado seg�n el tiempo de cada frame.
                currentTime += Time.deltaTime;

                // Opci�n de depuraci�n para verificar el tiempo acumulado.
                Debug.Log($"Tiempo actual: {currentTime:F2}s");

                // Espera al siguiente frame.
                yield return null;
            }

            // Alterna la visibilidad del men� (activo/inactivo).
            menu.SetActive(!menu.activeSelf);

            // Reinicia el contador de tiempo.
            currentTime = 0f;

            // Limpia la referencia de la Coroutine activa.
            timeCoroutine = null;
        }
    }
}
