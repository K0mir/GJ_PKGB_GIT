using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    public static PlayerCheckpoint Instance;

    private Vector3 currentCheckpoint;

    private void Awake()
    {
        // Singleton para que pueda ser accedido desde otros scripts
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Inicializar con la posición inicial del jugador
        currentCheckpoint = transform.position;
    }

    // Guardar un checkpoint
    public void SetCheckpoint(Vector3 position)
    {
        currentCheckpoint = position;
    }

    // Reaparecer en el último checkpoint
    public void Respawn()
    {
        transform.position = currentCheckpoint;
    }
}
