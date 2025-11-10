using UnityEngine;

public class bala : MonoBehaviour
{
    [Header("Velocidad de Movimiento")]
    public float speed = 2f;  // ← Se llama "speed"

    [Header("Límites para Repetir (Loop)")]
    public float leftBoundary = -20f;
    public float rightBoundary = 20f;

    void Update()
    {
        // Mueve la bala hacia la DERECHA
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Comprueba si la bala se ha salido del límite DERECHO
        if (transform.position.x > rightBoundary)
        {
            // Si se salió, la teletransporta al límite IZQUIERDO
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        // Mantiene la altura (Y) y profundidad (Z) originales, pero cambia la X
        transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
    }
}
