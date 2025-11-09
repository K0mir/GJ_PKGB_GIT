using UnityEngine;

public class Mine : MonoBehaviour
{
    public Animator animator;
    public float delayBeforeGameOver = 0.5f; // Cambiado de 1.2f a 0.5f
    public AudioSource explosionSound;
    public GameOverCI gameOverManager;

    private bool hasExploded = false;

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
        animator.SetTrigger("Explode");

        if (explosionSound != null)
            explosionSound.Play();

        Invoke("TriggerGameOver", delayBeforeGameOver);
    }

    void TriggerGameOver()
    {
        if (gameOverManager != null)
        {
            gameOverManager.GameOver();
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