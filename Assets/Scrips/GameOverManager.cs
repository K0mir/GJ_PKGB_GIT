using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCI : MonoBehaviour
{
    [Header("Canvas de Game Over")]
    private GameObject gameOverCanvas; // Se asignará automáticamente

    private bool isGameOver = false;
    public static GameOverCI instance;

    void Awake()
    {
        // Evitar duplicados si ya existe un GameOverCI persistente
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Escuchar cambios de escena
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Buscar canvas en la escena actual al iniciar
        FindCanvasInScene();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Se llama cada vez que se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCanvasInScene();
    }

    // 🔹 Busca el canvas de la escena actual, incluyendo objetos desactivados
    private void FindCanvasInScene()
    {
        // SOLUCIÓN: Usar el método correcto según la versión de Unity
#if UNITY_2023_1_OR_NEWER
        // Para Unity 2023.1 o superior
        Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
        // Para versiones anteriores de Unity
        Canvas[] allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();
#endif

        gameOverCanvas = null;

        foreach (Canvas c in allCanvases)
        {
            if (c.gameObject.CompareTag("GameOverUI"))
            {
                gameOverCanvas = c.gameObject;
                Debug.Log($"✅ GameOverCanvas encontrado automáticamente en la escena: {SceneManager.GetActiveScene().name}");
                break;
            }
        }

        if (gameOverCanvas == null)
        {
            Debug.LogWarning($"⚠️ No se encontró ningún objeto con el tag 'GameOverUI' en la escena {SceneManager.GetActiveScene().name}");
        }
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer();
        }
    }

    // 🔹 Llamar cuando el jugador muere
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);
        else
            Debug.LogError("❌ gameOverCanvas no está asignado en esta escena.");

        Time.timeScale = 0f;
    }

    // 🔹 Reaparecer al jugador y reiniciar minas
    void RespawnPlayer()
    {
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        Time.timeScale = 1f;
        isGameOver = false;

        ResetAllMines();

        if (PlayerCheckpoint.Instance != null)
            PlayerCheckpoint.Instance.Respawn();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🔹 Reiniciar todas las minas de la escena
    void ResetAllMines()
    {
    #if UNITY_2023_1_OR_NEWER
        Mine[] allMines = FindObjectsByType<Mine>(FindObjectsSortMode.None);
    #else
        Mine[] allMines = FindObjectsOfType<Mine>();
    #endif
        
        foreach (Mine mine in allMines)
        {
            if (mine != null)
            {
                // Asegurar que la mina esté activa antes de resetear
                mine.gameObject.SetActive(true);
                mine.ResetMine();
            }
        }

        Debug.Log($"Minas reseteadas: {allMines.Length}");

    }

    // 🔹 Método público para reiniciar desde botones
    public void RestartGame()
    {
        RespawnPlayer();
    }
}