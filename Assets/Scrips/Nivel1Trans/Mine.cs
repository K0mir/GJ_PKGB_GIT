using UnityEngine;

public class Mine : MonoBehaviour
{
    public Animator animator;
    public AudioSource explosionSound;
    public float gameOverDelay = 0.3f;
    
    private GameOverCI gameOverManager;
    private bool hasExploded = false;

    void Start()
    {
        gameOverManager = GameOverCI.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasExploded && collision.CompareTag("Player"))
        {
            hasExploded = true;
            Explode();
        }
    }

    void Explode()
    {
        // Activar animación
        if (animator != null)
            animator.SetTrigger("Explode");

        // Reproducir sonido de explosión
        if (explosionSound != null)
            explosionSound.Play();
        else
            Debug.LogWarning("AudioSource no asignado en la mina");

        // Game Over después del delay
        Invoke("TriggerGameOver", gameOverDelay);
    }

    void TriggerGameOver()
    {
        if (GameOverCI.instance != null)
        {
            GameOverCI.instance.GameOver();
        }
    }

    public void ResetMine()
    {
        hasExploded = false;

        if (animator != null)
        {
            animator.ResetTrigger("Explode");
            animator.Play("Idle", 0, 0f);
        }

        CancelInvoke("TriggerGameOver");
    }
}