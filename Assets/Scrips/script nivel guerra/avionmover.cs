using UnityEngine;

public class avionmover : MonoBehaviour
{
    [Header("Velocidad de Movimiento")]
    public float speed = 2f;

    [Header("Límites para Repetir (Loop)")]
    public float leftBoundary = -20f;  // Punto donde desaparece
    public float rightBoundary = 20f;  // Punto donde reaparece

    [Header("Debug")]
    public bool mostrarDebug = true;
    
    void Update()
    {
        // Mueve el avión hacia la izquierda
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Comprueba si necesita resetear la posición
        if (transform.position.x < leftBoundary)
        {
            if (mostrarDebug)
            {
                Debug.Log($"Reset posición - Actual: {transform.position.x}, Límite: {leftBoundary}, Nuevo: {rightBoundary}");
            }
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        // Teletransporta al límite derecho manteniendo Y y Z
        Vector3 newPosition = new Vector3(rightBoundary, transform.position.y, transform.position.z);
        transform.position = newPosition;
        
        if (mostrarDebug)
        {
            Debug.Log($"Posición reseteada a: {newPosition}");
        }
    }

    // Método para validar en el Inspector
    void OnValidate()
    {
        // Asegura que rightBoundary sea mayor que leftBoundary
        if (rightBoundary <= leftBoundary)
        {
            rightBoundary = leftBoundary + 1f;
            Debug.LogWarning("rightBoundary debe ser mayor que leftBoundary. Ajustado automáticamente.");
        }
    }

    // Dibuja gizmos en el editor para visualizar los límites
    void OnDrawGizmosSelected()
    {
        if (!mostrarDebug) return;

        Gizmos.color = Color.red;
        // Línea para leftBoundary
        Vector3 leftStart = new Vector3(leftBoundary, transform.position.y - 5f, transform.position.z);
        Vector3 leftEnd = new Vector3(leftBoundary, transform.position.y + 5f, transform.position.z);
        Gizmos.DrawLine(leftStart, leftEnd);

        Gizmos.color = Color.green;
        // Línea para rightBoundary
        Vector3 rightStart = new Vector3(rightBoundary, transform.position.y - 5f, transform.position.z);
        Vector3 rightEnd = new Vector3(rightBoundary, transform.position.y + 5f, transform.position.z);
        Gizmos.DrawLine(rightStart, rightEnd);
    }
}
