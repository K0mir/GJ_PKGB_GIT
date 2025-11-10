using UnityEngine;

public class botonReinicio : MonoBehaviour
{
    public void Restart()
    {
        if (GameOverCI.instance != null)
        {
            GameOverCI.instance.RestartGame();
            Debug.Log("Reinicio iniciado desde botón");
        }
        else
        {
            Debug.LogWarning("No se encontró GameOverCI en la escena. Asegúrate de que el objeto persistente esté en la escena.");
            // Opcional: cargar la escena directamente como fallback
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
