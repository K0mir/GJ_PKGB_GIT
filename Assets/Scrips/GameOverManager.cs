using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverCI : MonoBehaviour
{
   public GameObject gameOverCanvas; // Asigna el Canvas desde el Inspector

    private bool isGameOver = false;

    public void GameOver()
    {
        if (isGameOver) return; // Evita repetir la animación
        isGameOver = true;

        // Mostrar la pantalla Game Over
        gameOverCanvas.SetActive(true);

        // Detener el tiempo del juego
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        // Volver al estado normal
        Time.timeScale = 1f;

        // Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
