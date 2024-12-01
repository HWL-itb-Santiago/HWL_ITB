using System;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit.UI
{
    /// <summary>
    /// Component to make a Canvas follow the player and always face them smoothly.
    /// </summary>
    /// <remarks>
    /// This component ensures the Canvas remains visible to the player by keeping
    /// it at a configurable distance and height, and smoothly rotating it to face the player.
    /// </remarks>
    [AddComponentMenu("XR/UI/Canvas Follow and Look At Player")]
    [HelpURL("https://docs.unity3d.com/ScriptReference/Transform.html")]
    public class CanvasFollowAndLookAtPlayer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("Reference to the player's camera (usually the main camera).")]
        private Transform m_PlayerCamera;

        /// <summary>
        /// Reference to the player's camera.
        /// </summary>
        public Transform PlayerCamera
        {
            get => m_PlayerCamera;
            set => m_PlayerCamera = value;
        }

        [Header("Follow Settings")]
        [SerializeField, Tooltip("Distance the Canvas should maintain from the player.")]
        private float m_FollowDistance;

        /// <summary>
        /// Distance the Canvas should maintain from the player.
        /// </summary>
        public float FollowDistance
        {
            get => m_FollowDistance;
            set => m_FollowDistance = Mathf.Max(0, value);
        }

        [SerializeField, Tooltip("Height offset relative to the player's camera.")]
        private float m_FollowHeight;

        /// <summary>
        /// Height offset relative to the player's camera.
        /// </summary>
        public float FollowHeight
        {
            get => m_FollowHeight;
            set => m_FollowHeight = value;
        }

        [SerializeField, Tooltip("Speed at which the Canvas follows the player's position.")]
        private float m_FollowSpeed;

        /// <summary>
        /// Speed at which the Canvas follows the player's position.
        /// </summary>
        public float FollowSpeed
        {
            get => m_FollowSpeed;
            set => m_FollowSpeed = Mathf.Max(0, value);
        }

        [Header("Rotation Settings")]
        [SerializeField, Tooltip("Define if the Canvas should only rotate on the Y axis.")]
        private bool m_RotateOnlyY = true;

        /// <summary>
        /// Whether the Canvas should only rotate on the Y axis.
        /// </summary>
        public bool RotateOnlyY
        {
            get => m_RotateOnlyY;
            set => m_RotateOnlyY = value;
        }

        [SerializeField, Tooltip("Speed at which the Canvas rotates to face the player.")]
        private float m_RotationSpeed;

        /// <summary>
        /// Speed at which the Canvas rotates to face the player.
        /// </summary>
        public float RotationSpeed
        {
            get => m_RotationSpeed;
            set => m_RotationSpeed = Mathf.Max(0, value);
        }

        [Header("General Settings")]
        [SerializeField, Tooltip("Enable or disable position following.")]
        private bool m_EnablePositionFollowing = true;

        /// <summary>
        /// Enable or disable position following.
        /// </summary>
        public bool EnablePositionFollowing
        {
            get => m_EnablePositionFollowing;
            set => m_EnablePositionFollowing = value;
        }

        [SerializeField, Tooltip("Enable or disable rotation to face player.")]
        private bool m_EnableRotationFollowing = true;

        /// <summary>
        /// Enable or disable rotation to face the player.
        /// </summary>
        public bool EnableRotationFollowing
        {
            get => m_EnableRotationFollowing;
            set => m_EnableRotationFollowing = value;
        }

        /// <summary>
        /// Called when the component is enabled.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (m_PlayerCamera == null)
            {
                m_PlayerCamera = Camera.main?.transform;
                if (m_PlayerCamera == null)
                {
                    Debug.LogWarning("Player Camera is not assigned, and no Main Camera is found.");
                }
            }
        }

        /// <summary>
        /// Called every frame to update the Canvas position and rotation.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (m_PlayerCamera == null) return;

            if (m_EnablePositionFollowing)
                UpdatePosition();

            if (m_EnableRotationFollowing)
                UpdateRotation();
        }

        /// <summary>
        /// Updates the Canvas position smoothly to maintain a comfortable distance and height.
        /// </summary>
        protected virtual void UpdatePosition()
        {
            Vector3 targetPosition = m_PlayerCamera.position + m_PlayerCamera.forward * m_FollowDistance;
            targetPosition.y = m_PlayerCamera.position.y + m_FollowHeight;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * m_FollowSpeed);
        }

        /// <summary>
        /// Updates the Canvas rotation to face the player smoothly.
        /// </summary>
        protected virtual void UpdateRotation()
        {
            Vector3 directionToPlayer = m_PlayerCamera.position - transform.position;

            if (m_RotateOnlyY)
                directionToPlayer.y = 0;

            if (directionToPlayer.sqrMagnitude < Mathf.Epsilon) return;

            Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * m_RotationSpeed);
        }
    }
}
