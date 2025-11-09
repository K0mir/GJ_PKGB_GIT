using UnityEngine;

public class MineKill : MonoBehaviour
{
    public float delayBeforeGameOver = 0.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(" ¡Sparky fue atropellado!");
            Invoke("TriggerGameOver", delayBeforeGameOver);
        }
    }

    void TriggerGameOver()
    {
        FindFirstObjectByType<GameOverCI>().GameOver();
    }
}