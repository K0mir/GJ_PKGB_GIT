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
            
            var gameOver = FindFirstObjectByType<GameOverCI>();
            if (gameOver != null)
            {
                gameOver.GameOver();
            }
            else
            {
                Debug.LogError("❌ No se encontró el GameOverCI en la escena actual. Asegúrate de que el GameManager persista entre escenas o esté presente en esta.");
            }
        }
    }
}
