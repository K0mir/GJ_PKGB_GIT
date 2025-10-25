using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float startPosX;
    private float length;
    public GameObject cam;
    public float parallaxEffect; // entre 0 y 1 (menor = más lejos)

    void Start()
    {
        startPosX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPosX + dist, transform.position.y, transform.position.z);

        // Esto hace que el fondo se repita infinitamente
        if (temp > startPosX + length)
            startPosX += length;
        else if (temp < startPosX - length)
            startPosX -= length;
    }
}
