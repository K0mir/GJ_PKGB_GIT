using UnityEngine;
using UnityEngine.Playables;
using TMPro;
public class ObjectTimelineTrigger : MonoBehaviour
{
    [Header("Configuración Timeline")]
    public PlayableDirector timeline; // Arrastra tu Timeline aquí
    
    [Header("Configuración del Trigger")]
    public bool playOnce = true; // ¿Solo una vez?
    
    private bool hasBeenActivated = false;

    // Cuando Sparky entre en el trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar que es el jugador y que no se ha activado antes
        if (other.CompareTag("Player") && !hasBeenActivated)
        {
            // Activar el Timeline
            if (timeline != null)
            {
                timeline.Play();
                hasBeenActivated = playOnce;
                
                Debug.Log("Timeline activado por: " + other.name);
            }
            else
            {
                Debug.LogError("No hay Timeline asignado en: " + gameObject.name);
            }
        }
    }
}
