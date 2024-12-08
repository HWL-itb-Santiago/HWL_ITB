using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace UnityEngine.XR.Interaction.Toolkit.UI
{
    /// <summary>
    /// Component to make a Canvas follow the player and always face them smoothly.
    /// </summary>
    /// <remarks>
    /// This component ensures the Canvas remains visible to the player by keeping
    /// it at a configurable distance and height, and smoothly rotating it to face the player.
    /// </remarks>
    [AddComponentMenu("XR/UI/Follow and Look At Player")]
    [HelpURL("https://docs.unity3d.com/ScriptReference/Transform.html")]
    public class FollowAndLookAtPlayer : MonoBehaviour
    {
        private bool IsFloating = true;
        private Vector3 targetPosition;
        private float InitialPosition;

        public float m_InitialPosition
        {
            get => InitialPosition;
            set => InitialPosition = value;
        }
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
        [SerializeField]
        private float m_FloatingSpeed;
        public float FloatingSpeed
        {
            get => m_FloatingSpeed;
            set => m_FloatingSpeed = Mathf.Max(0, value);
        }
        [SerializeField]
        private float m_FloatingDistance;
        public float FloatingDistance
        {
            get => m_FloatingDistance;
            set => m_FloatingDistance = value;
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

            if (m_FollowSpeed <= 0)
            {
                Debug.LogWarning("FollowSpeed must be greater than 0. Setting to default (1).");
                m_FollowSpeed = 1f;
            }

            if (m_RotationSpeed <= 0)
            {
                Debug.LogWarning("RotationSpeed must be greater than 0. Setting to default (1).");
                m_RotationSpeed = 1f;
            }
        }


        protected virtual void Start()
        {

            FloatingAnimation();
        }
        /// <summary>
        /// Called every frame to update the position and rotation.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (m_PlayerCamera == null) return;

            // Solo actualiza posición si está habilitada y la distancia es significativa
            if (m_EnablePositionFollowing && NeedsPositionUpdate())
                UpdatePosition();

            // Solo actualiza rotación si está habilitada y hay un cambio necesario
            if (m_EnableRotationFollowing && NeedsRotationUpdate())
                UpdateRotation();

        }

        /// <summary>
        /// Verifica si la posición necesita actualizarse.
        /// </summary>
        private bool NeedsPositionUpdate()
        {
            Vector3 targetPos = m_PlayerCamera.position + m_PlayerCamera.forward * m_FollowDistance;
            targetPos.y = m_PlayerCamera.position.y + m_FollowHeight;

            return Vector3.Distance(transform.position, targetPos) > Mathf.Epsilon;
        }

        /// <summary>
        /// Verifica si la rotación necesita actualizarse.
        /// </summary>
        private bool NeedsRotationUpdate()
        {
            Vector3 directionToPlayer = m_PlayerCamera.position - transform.position;
            if (m_RotateOnlyY)
                directionToPlayer.y = 0;

            if (directionToPlayer.sqrMagnitude < Mathf.Epsilon)
                return false;

            Quaternion targetRot = Quaternion.LookRotation(-directionToPlayer.normalized);
            return Quaternion.Angle(transform.rotation, targetRot) > Mathf.Epsilon;
        }

        /// <summary>
        /// Updates the Canvas position smoothly to maintain a comfortable distance and height.
        /// </summary>
        protected virtual void UpdatePosition()
        {
            targetPosition = m_PlayerCamera.position + m_PlayerCamera.forward * m_FollowDistance;
            targetPosition.y = m_PlayerCamera.position.y + m_FollowHeight;

            transform.DOMove(targetPosition, 1 / m_FollowSpeed).SetEase(Ease.InOutSine);
            //transform.DOMoveX(targetPosition.x, 1 / m_FollowSpeed).SetEase(Ease.InOutSine);
            //transform.DOMoveZ(targetPosition.z, 1 / m_FollowSpeed).SetEase(Ease.InOutSine);

            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * m_FollowSpeed);
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
            transform.DORotateQuaternion(targetRotation, 1 / m_RotationSpeed).SetEase(Ease.InOutSine);
        }


        public void FloatingAnimation()
        {
            Vector3 localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

            // Configura la animación flotante
            transform.DOScale(localScale * m_FloatingDistance, 1 / m_FloatingSpeed)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
