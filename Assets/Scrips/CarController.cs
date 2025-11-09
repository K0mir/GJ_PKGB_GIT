using UnityEngine;
using UnityEngine.InputSystem;
public class CarController : MonoBehaviour
{
    [Header("Velocidad")]
    public float speed = 15f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("💥 ¡Sparky fue atropellado!");
            // Aquí puedes cargar una escena de “Game Over”
            // SceneManager.LoadScene("GameOver");
            FindFirstObjectByType<GameOverCI>().GameOver();
        }
    }
}
