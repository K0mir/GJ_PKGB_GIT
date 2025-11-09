using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Guardar la posición del checkpoint
            PlayerCheckpoint.Instance.SetCheckpoint(transform.position);
        }
    }
}
