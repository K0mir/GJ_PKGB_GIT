using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioNivel : MonoBehaviour
{
    [Header("Nombre de la siguiente escena")]
    public string nombreSiguienteEscena; // Nombre exacto de la escena siguiente

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(nombreSiguienteEscena))
            {
                SceneManager.LoadScene(nombreSiguienteEscena);
            }
            else
            {
                Debug.LogWarning("⚠️ No se asignó el nombre de la siguiente escena en el Inspector.");
            }
        }
    }
}
