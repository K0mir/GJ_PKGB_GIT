using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
public class CinematicManager : MonoBehaviour
{
  [Header("Referencias de Cinemática")]
    public PlayableDirector cinematicDirector;
    
    [Header("Configuración")]
    public bool disableNPCControlDuringCinematic = true;
    public GameObject npcObject; // Tu NPC aquí
    
    private bool isCinematicPlaying = false;
    private MonoBehaviour[] npcComponents;

    void Start()
    {
        if (cinematicDirector != null)
        {
            cinematicDirector.stopped += OnCinematicFinished;
            cinematicDirector.playOnAwake = false;
            
            // Pausar inmediatamente al inicio
            cinematicDirector.Pause();
            cinematicDirector.time = 0f;
        }
        
        // Guardar referencia a los componentes del NPC
        if (npcObject != null)
        {
            npcComponents = npcObject.GetComponents<MonoBehaviour>();
        }
    }

    public void PlayCinematic()
    {
        if (isCinematicPlaying || cinematicDirector == null) return;
        
        StartCoroutine(StartCinematicCoroutine());
    }

    private IEnumerator StartCinematicCoroutine()
    {
        isCinematicPlaying = true;
        
        Debug.Log("Iniciando cinemática...");
        
        // Desactivar scripts del NPC pero NO el GameObject
        if (disableNPCControlDuringCinematic && npcObject != null)
        {
            foreach (MonoBehaviour component in npcComponents)
            {
                if (component != null && component != this && component.enabled)
                {
                    component.enabled = false;
                    Debug.Log("Desactivado: " + component.GetType().Name);
                }
            }
        }
        
        // Pequeña pausa para asegurar que todo está listo
        yield return new WaitForSeconds(0.1f);
        
        // Asegurar que la timeline empiece desde 0
        cinematicDirector.time = 0f;
        cinematicDirector.initialTime = 0f;
        
        // Reproducir la cinemática
        cinematicDirector.Play();
        
        Debug.Log("Cinemática en reproducción - Timeline time: " + cinematicDirector.time);
    }

    private void OnCinematicFinished(PlayableDirector director)
    {
        if (director == cinematicDirector)
        {
            FinishCinematic();
        }
    }

    private void FinishCinematic()
    {
        Debug.Log("Cinemática terminada");
        
        // Reactivar scripts del NPC
        if (disableNPCControlDuringCinematic && npcObject != null)
        {
            foreach (MonoBehaviour component in npcComponents)
            {
                if (component != null && component != this)
                {
                    component.enabled = true;
                }
            }
        }
        
        // Pausar y resetear la timeline
        cinematicDirector.Stop();
        cinematicDirector.time = 0f;
        cinematicDirector.Pause();
        
        isCinematicPlaying = false;
        
        Debug.Log("Cinematic Manager listo para siguiente cinemática");
    }

    public bool IsCinematicPlaying()
    {
        return isCinematicPlaying;
    }

    void OnDestroy()
    {
        if (cinematicDirector != null)
        {
            cinematicDirector.stopped -= OnCinematicFinished;
        }
    }
}