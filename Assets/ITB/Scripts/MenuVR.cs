using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UI;

namespace UnityEngine.XR.Interaction.Toolkit.Inputs
{
    /// <summary>
    /// Manages the interaction of a VR menu by detecting hover events on interactive objects.
    /// Allows toggling a menu and handling UI button interactions based on hover duration.
    /// </summary>
    public class MenuVR : MonoBehaviour
    {
        // Opacity settings for buttons
        [Header("Button Opacity Settings")]
        [Tooltip("Normal opacity of buttons when not being hovered.")]
        [SerializeField]
        private float m_NormalOpacityButton = 1f;

        /// <summary>
        /// Gets or sets the default opacity of UI buttons. Values are clamped between 0 and 1.
        /// </summary>
        public float NormalOpacityButton
        {
            get => m_NormalOpacityButton;
            set => m_NormalOpacityButton = Mathf.Clamp01(value);
        }

        [Tooltip("Target opacity for buttons when being hovered.")]
        [SerializeField]
        private float m_HoverOpacityButton = 0.5f;

        /// <summary>
        /// Gets or sets the opacity of UI buttons when hovered. Values are clamped between 0 and 1.
        /// </summary>
        public float HoverOpacityButton
        {
            get => m_HoverOpacityButton;
            set => m_HoverOpacityButton = Mathf.Clamp01(value);
        }

        // Time settings
        [Header("Time Settings")]
        [Tooltip("Time (in seconds) required to toggle the menu when hovering over the object.")]
        [SerializeField]
        private float m_TimeToActivateMenu = 2f;

        /// <summary>
        /// Gets or sets the duration required to toggle the menu. Must be non-negative.
        /// </summary>
        public float TimeToActivateMenu
        {
            get => m_TimeToActivateMenu;
            set => m_TimeToActivateMenu = Mathf.Max(0, value);
        }

        [Tooltip("Time (in seconds) required to trigger button hover effects.")]
        [SerializeField]
        private float m_TimeToActivateButton = 1f;

        /// <summary>
        /// Gets or sets the duration required to activate button hover effects. Must be non-negative.
        /// </summary>
        public float TimeToActivateButton
        {
            get => m_TimeToActivateButton;
            set => m_TimeToActivateButton = Mathf.Max(0, value);
        }

        // UI references
        [Header("UI References")]
        [Tooltip("The menu GameObject to be toggled.")]
        [SerializeField]
        private GameObject m_Menu;

        // Internal state
        private bool m_IsMenuOpen = false;

        /// <summary>
        /// Gets or sets the state of the menu (open or closed).
        /// </summary>
        public bool IsMenuOpen
        {
            get => m_IsMenuOpen;
            set => m_IsMenuOpen = value;
        }

        // Internal variables for managing time and coroutines
        private float m_CurrentTime;
        private Coroutine m_ActiveCoroutine;

        /// <summary>
        /// Initializes variables and ensures timers start at zero.
        /// </summary>
        private void Start()
        {
            m_CurrentTime = 0f; // Reset the timer at the start.
        }

        /// <summary>
        /// Called when the player begins hovering over an interactive object.
        /// Starts a coroutine to handle menu or button interaction logic.
        /// </summary>
        /// <param name="obj">The GameObject being hovered over.</param>
        public void OnHover(GameObject obj)
        {
            if (m_ActiveCoroutine == null && obj.CompareTag("Menu"))
            {
                // Start the menu activation coroutine if hovering over a menu object.
                m_ActiveCoroutine = StartCoroutine(HandleMenuActivation());
            }
            else if (m_ActiveCoroutine == null && obj.CompareTag("ButtonUI"))
            {
                // Start the button hover coroutine if hovering over a UI button.
                Button button = obj.GetComponent<Button>();
                if (button != null)
                {
                    m_ActiveCoroutine = StartCoroutine(HandleButtonHover(button));
                }
            }
        }

        /// <summary>
        /// Called when the player stops hovering over an interactive object.
        /// Stops the active coroutine and resets the button opacity if necessary.
        /// </summary>
        /// <param name="obj">The GameObject the player stopped hovering over.</param>
        public void OffHover(GameObject obj)
        {
            // Stop the active coroutine and reset variables.
            if (m_ActiveCoroutine != null)
            {
                StopCoroutine(m_ActiveCoroutine);
                m_ActiveCoroutine = null;
            }

            // Reset the button opacity when no longer hovering over it.
            if (obj.CompareTag("ButtonUI"))
            {
                Button button = obj.GetComponent<Button>();
                if (button != null)
                {
                    SetButtonOpacity(button, m_NormalOpacityButton);
                }
            }

            m_CurrentTime = 0f; // Reset the timer.
        }
        /// <summary>
        /// Handles a timed action with interpolation, allowing for a custom callback to handle specific updates.
        /// </summary>
        /// <param name="duration">The total duration of the action in seconds.</param>
        /// <param name="onUpdate">A callback invoked every frame to update a value based on the elapsed time (0 to 1).</param>
        /// <param name="onComplete">An optional callback invoked when the action is completed.</param>
        /// <returns>An IEnumerator to be used in a coroutine.</returns>
        private IEnumerator HandleTimedAction(float duration, System.Action<float> onUpdate, System.Action onComplete = null)
        {
            m_CurrentTime = 0f;

            while (m_CurrentTime < duration)
            {
                m_CurrentTime += Time.deltaTime;
                float t = Mathf.Clamp01(m_CurrentTime / duration);

                // Invoke the update callback with the interpolation value.
                onUpdate?.Invoke(t);

                yield return null;
            }

            // Invoke the complete callback when finished.
            onComplete?.Invoke();
            ResetState();
        }

        /// <summary>
        /// Coroutine that waits for the required time to toggle the menu.
        /// </summary>
        private IEnumerator HandleMenuActivation()
        {
            yield return HandleTimedAction(
                m_TimeToActivateMenu,
                t => { /* No interpolation needed here, just wait. */ },
                ToggleMenu
            );
        }

        /// <summary>
        /// Coroutine that progressively changes the opacity of a button during hover.
        /// Invokes the button's onClick event when the required time is reached.
        /// </summary>
        private IEnumerator HandleButtonHover(Button button)
        {
            yield return HandleTimedAction(
                m_TimeToActivateButton,
                t =>
                {
                    // Interpolate button opacity during hover.
                    float opacity = Mathf.Lerp(m_NormalOpacityButton, m_HoverOpacityButton, t);
                    SetButtonOpacity(button, opacity);
                },
                () => button.onClick.Invoke() // Trigger the button's click event when done.
            );
        }


        /// <summary>
        /// Toggles the menu's active state (open/closed).
        /// </summary>
        private void ToggleMenu()
        {
            if (m_Menu == null) return;

            m_Menu.SetActive(!m_Menu.activeSelf); // Toggle the menu's active state.
            m_IsMenuOpen = m_Menu.activeSelf; // Update the state variable.
        }

        /// <summary>
        /// Sets the opacity of a UI button image.
        /// </summary>
        /// <param name="button">The button to modify.</param>
        /// <param name="targetOpacity">The target opacity value (between 0 and 1).</param>
        private void SetButtonOpacity(Button button, float targetOpacity)
        {
            if (button == null || button.image == null) return;

            // Update the button's image color to reflect the target opacity.
            Color currentColor = button.image.color;
            button.image.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetOpacity);
        }

        /// <summary>
        /// Resets the internal timer and coroutine state variables.
        /// </summary>
        private void ResetState()
        {
            m_CurrentTime = 0f; // Reset the timer.
            m_ActiveCoroutine = null; // Clear the active coroutine reference.
        }
    }
}
