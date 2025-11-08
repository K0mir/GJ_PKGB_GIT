using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private bool parallaxVertical = false;
    [Range(0.01f, 1f)]
    public float parallaxSpeed = 0.5f;
    
    private Vector3 camStartPos;
    private Material[] mat;
    private float[] backSpeed;
    private float farthestBack;

    void Start()
    {
        if (cam == null) cam = Camera.main.transform;
        camStartPos = cam.position;
        
        InitializeBackgrounds();
    }

    void InitializeBackgrounds()
    {
        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        
        for (int i = 0; i < backCount; i++)
        {
            GameObject bg = transform.GetChild(i).gameObject;
            mat[i] = bg.GetComponent<Renderer>().material;
        }
        
        CalculateLayerSpeeds();
    }

    void CalculateLayerSpeeds()
    {
        farthestBack = 0;
        
        // Encontrar la capa más lejana
        for (int i = 0; i < mat.Length; i++)
        {
            float depth = Mathf.Abs(transform.GetChild(i).position.z - cam.position.z);
            farthestBack = Mathf.Max(farthestBack, depth);
        }

        // Calcular velocidades (capas más lejanas = más lentas)
        for (int i = 0; i < mat.Length; i++)
        {
            float depth = Mathf.Abs(transform.GetChild(i).position.z - cam.position.z);
            backSpeed[i] = 1 - (depth / farthestBack);
        }
    }

    private void LateUpdate()
    {
        Vector3 distance = cam.position - camStartPos;
        
        for (int i = 0; i < mat.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            Vector2 offset = new Vector2(distance.x, parallaxVertical ? distance.y : 0) * speed;
            mat[i].SetTextureOffset("_MainTex", offset);
        }
    }
}
