using UnityEngine;
using UnityEngine.InputSystem;
public class FinalDialogueTrigger : MonoBehaviour
{
     [Header("Diálogo Final")]
    public Dialog finalDialogue;
    
    [Header("Configuración")]
    public bool autoActivate = true;
    public float activationDelay = 0.2f;
    
    private DialogManager dialogueManager;
    private FadeManager fadeManager;
    private bool hasTriggered = false;
    private bool isPlayerInRange = false;

    void Start()
    {
        // Buscar los managers necesarios
        dialogueManager = FindFirstObjectByType<DialogManager>();
        fadeManager = FadeManager.Instance; // Usar la instancia singleton
        
        // Configurar el diálogo para activar el final
        if (dialogueManager != null && finalDialogue != null)
        {
            // Buscar el método SetTriggerEndingAfterDialogue en DialogManager
            // Si no existe, usaremos un método alternativo
            ConfigureDialogManagerForEnding();
        }
        else
        {
            Debug.LogError("Faltan referencias en FinalDialogueTrigger: " + gameObject.name);
        }
    }

    void ConfigureDialogManagerForEnding()
    {
        // Método 1: Si existe el método SetTriggerEndingAfterDialogue
        var method = dialogueManager.GetType().GetMethod("SetTriggerEndingAfterDialogue");
        if (method != null)
        {
            method.Invoke(dialogueManager, new object[] { true, fadeManager });
        }
        else
        {
            // Método 2: Configurar manualmente
            ConfigureManually();
        }
    }

    void ConfigureManually()
    {
        // Agregar componente temporal para manejar el final
        dialogueManager.gameObject.AddComponent<EndingHandler>().Initialize(fadeManager);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered && dialogueManager != null && finalDialogue != null)
        {
            isPlayerInRange = true;
            
            if (autoActivate)
            {
                Invoke(nameof(TriggerFinalDialogue), activationDelay);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            if (autoActivate && !hasTriggered)
            {
                CancelInvoke(nameof(TriggerFinalDialogue));
            }
        }
    }

    private void TriggerFinalDialogue()
    {
        if (!hasTriggered && isPlayerInRange && dialogueManager != null && finalDialogue != null)
        {
            hasTriggered = true;
            Debug.Log("🔚 Iniciando diálogo final...");
            dialogueManager.StartDialogue(finalDialogue);
        }
    }

    // Para activación manual (desde Input System)
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && isPlayerInRange && !hasTriggered && !autoActivate && 
            dialogueManager != null && finalDialogue != null)
        {
            TriggerFinalDialogue();
        }
    }
}

// Clase helper para manejar el final
public class EndingHandler : MonoBehaviour
{
    private FadeManager fadeManager;
    
    public void Initialize(FadeManager manager)
    {
        fadeManager = manager;
    }
    
    // Este método será llamado desde DialogManager cuando termine el diálogo
    public void OnDialogueEnd()
    {
        if (fadeManager != null)
        {
            fadeManager.FadeToBlackAndLoadEnding();
        }
        Destroy(this); // Limpiar después de usar
    }
}
