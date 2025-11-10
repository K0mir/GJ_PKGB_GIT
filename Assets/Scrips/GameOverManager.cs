using UnityEngine;

public class GameOverCI : MonoBehaviour
{
    public GameObject gameOverCanvas; // Asigna el Canvas desde el Inspector
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    void RespawnPlayer()
    {
        // Ocultar Game Over y volver al tiempo normal
        gameOverCanvas.SetActive(false);
        Time.timeScale = 1f;
        isGameOver = false;

        // Resetear todas las minas antes de reaparecer
        ResetAllMines();

        // Reaparecer en el último checkpoint
        if (PlayerCheckpoint.Instance != null)
        {
            PlayerCheckpoint.Instance.Respawn();
        }
        else
        {
            // Fallback: recargar la escena si no hay sistema de checkpoints
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }

    void ResetAllMines()
    {
        // Encontrar y resetear todas las minas en la escena (versión actualizada)
        Mine[] allMines = FindObjectsByType<Mine>(FindObjectsSortMode.None);
        foreach (Mine mine in allMines)
        {
            mine.ResetMine();
        }

        Debug.Log($"Minas reseteadas: {allMines.Length}");
    }

    // Método público para reiniciar desde otros scripts
    public void RestartGame()
    {
        RespawnPlayer();
    }
}