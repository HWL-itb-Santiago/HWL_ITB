using UnityEngine;

public class CanvasLookAtPlayer : MonoBehaviour
{
    [Header("Player Camera Reference")]
    [Tooltip("Referencia a la c�mara del jugador (normalmente la c�mara principal).")]
    public Transform playerCamera;

    [Header("Settings")]
    [Tooltip("Define si el Canvas debe rotar solo en el eje Y.")]
    public bool rotateOnlyY = true;

    [Tooltip("Velocidad de la rotaci�n del Canvas hacia el jugador.")]
    public float rotationSpeed = 5f;

    private void Start()
    {
        // Si no se asign� una c�mara, usa la principal.
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (playerCamera == null) return;

        // Calcula la direcci�n hacia la c�mara del jugador.
        Vector3 directionToPlayer = playerCamera.position - transform.position;

        // Si solo queremos rotar en el eje Y, eliminamos las componentes X y Z.
        if (rotateOnlyY)
        {
            directionToPlayer.y = 0;
        }

        // Calcula la rotaci�n hacia el jugador.
        Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);

        // Interpola suavemente hacia la rotaci�n objetivo.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}

