using UnityEngine;

public class CanonSimple : MonoBehaviour
{
   public GameObject balaPrefab;
    public float intervalo = 2f;
    public float velocidad = 5f;
    
    private float tiempoSiguienteDisparo;

    void Start()
    {
        tiempoSiguienteDisparo = Time.time + 1f;
    }

    void Update()
    {
        if (Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();
            tiempoSiguienteDisparo = Time.time + intervalo;
        }
    }

    void Disparar()
    {
        if (balaPrefab == null) return;
        
        GameObject bala = Instantiate(balaPrefab, transform.position, Quaternion.identity);
        
        // Usa "bala" minúscula para coincidir con tu script
        bala scriptBala = bala.GetComponent<bala>();
        if (scriptBala != null)
        {
            scriptBala.speed = velocidad; // ← "speed" como en tu código original
        }
    }
}
