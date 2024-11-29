using UnityEngine;

public class CanvasLookAtPlayer : MonoBehaviour
{
    [Header("Player Camera Reference")]
    [Tooltip("Referencia a la cámara del jugador (normalmente la cámara principal).")]
    public Transform playerCamera;

    [Header("Settings")]
    [Tooltip("Define si el Canvas debe rotar solo en el eje Y.")]
    public bool rotateOnlyY = true;

    [Tooltip("Velocidad de la rotación del Canvas hacia el jugador.")]
    public float rotationSpeed = 5f;

    private void Start()
    {
        // Si no se asignó una cámara, usa la principal.
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (playerCamera == null) return;

        // Calcula la dirección hacia la cámara del jugador.
        Vector3 directionToPlayer = playerCamera.position - transform.position;

        // Si solo queremos rotar en el eje Y, eliminamos las componentes X y Z.
        if (rotateOnlyY)
        {
            directionToPlayer.y = 0;
        }

        // Calcula la rotación hacia el jugador.
        Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);

        // Interpola suavemente hacia la rotación objetivo.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}

