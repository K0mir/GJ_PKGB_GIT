using UnityEngine;

public class avionmover : MonoBehaviour
{
    // La velocidad a la que se moverá el avión
    [Header("Velocidad de Movimiento")]
    public float speed = 2f;

    // Los límites de la pantalla
    [Header("Límites para Repetir (Loop)")]
    public float leftBoundary = -20f;  // Punto donde el avión desaparece (ej. -20)
    public float rightBoundary = 20f; // Punto donde el avión reaparece (ej. 20)

    void Update()
    {
        // 1. Mueve el avión hacia la izquierda (negativo en X)
        // Usamos Time.deltaTime para que el movimiento sea suave y no dependa de los FPS
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 2. Comprueba si el avión se ha salido del límite izquierdo
        if (transform.position.x < leftBoundary)
        {
            // 3. Si se salió, lo teletransporta al límite derecho
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        // Mantiene la altura (Y) y profundidad (Z) originales, pero cambia la X
        transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
    }
}
