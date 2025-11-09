using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Prefabs de autos")]
    public GameObject[] carPrefabs;

    [System.Serializable]
    public class Lane
    {
        public float xPos;          // Posición X del carril
        public bool goesDown = true; // Si true: baja, si false: sube
    }

    [Header("Configuración de carriles")]
    public Lane[] lanes; // Lista de carriles con direcciones

    [Header("Posiciones de aparición")]
    public float topSpawnY = 6f;     // Punto superior
    public float bottomSpawnY = -6f; // Punto inferior

    [Header("Intervalo de aparición")]
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
    {
        if (carPrefabs.Length == 0 || lanes.Length == 0) return;

        // Seleccionar carril aleatorio
        Lane lane = lanes[Random.Range(0, lanes.Length)];
        GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        Vector3 spawnPos;
        Quaternion rotation;

        if (lane.goesDown)
        {
            // Autos que bajan
            spawnPos = new Vector3(lane.xPos, topSpawnY, 0);
            rotation = Quaternion.identity;
        }
        else
        {
            // Autos que suben
            spawnPos = new Vector3(lane.xPos, bottomSpawnY, 0);
            rotation = Quaternion.Euler(0, 0, 180);
        }

        // Instanciar auto
        GameObject car = Instantiate(carPrefab, spawnPos, rotation);

        // Aplicar movimiento
        Rigidbody2D rb = car.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speed = Random.Range(3f, 6f);
            rb.linearVelocity = lane.goesDown ? Vector2.down * speed : Vector2.up * speed;
        }

        Destroy(car, 2f);
    }
    
}
