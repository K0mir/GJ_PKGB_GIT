using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioNivel : MonoBehaviour
{
    [Header("Nombre de la siguiente escena")]
    public string Transicion1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cargar la siguiente escena
            SceneManager.LoadScene(1);
        }
    }
}
