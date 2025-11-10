using UnityEngine;

public class Nivel1GameOver : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("💥 ¡Sparky se a chocado!");
            // Aquí puedes cargar una escena de “Game Over”
            // SceneManager.LoadScene("GameOver");
            FindFirstObjectByType<GameOverCI>().GameOver();
        }
    }
}
