using UnityEngine;
using UnityEngine.InputSystem;
public class AutoDialogTrigger : MonoBehaviour
{
    [Header("Configuración de Diálogo")]
    public Dialog dialogue;
    public float activationDelay = 0.1f;
    
    [Header("Modo de Activación")]
    public bool useAutoActivation = true; // ← Puedes elegir el modo
    public bool showInteractionIndicator = false; // ← Opcional para modo automático
    
    private DialogManager dialogueManager;
    private bool hasActivated = false;
    private bool isPlayerInRange = false;

    void Start()
    {
        FindDialogManager();
    }

    void FindDialogManager()
    {
        // Buscar DialogManager
        dialogueManager = FindFirstObjectByType<DialogManager>();
        
        if (dialogueManager == null)
        {
            Debug.LogError("❌ DialogManager NO encontrado en la escena!");
            Debug.LogError("Asegúrate de tener un GameObject con el componente DialogManager");
        }
        else
        {
            Debug.Log("✅ DialogManager encontrado: " + dialogueManager.gameObject.name);
        }

        if (dialogue == null)
        {
            Debug.LogError("❌ Diálogo no asignado en: " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated && dialogueManager != null && dialogue != null)
        {
            isPlayerInRange = true;
            
            if (useAutoActivation)
            {
                // MODO AUTOMÁTICO: Activar después del delay
                Invoke(nameof(TriggerDialogue), activationDelay);
            }
            // Si no es automático, solo marca que el jugador está en rango
            // El Input System se encargará del resto a través de OnInteract
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            // Cancelar activación automática si el jugador sale
            if (useAutoActivation && !hasActivated)
            {
                CancelInvoke(nameof(TriggerDialogue));
            }
        }
    }

    // ← MÉTODO PARA ACTIVACIÓN AUTOMÁTICA
    private void TriggerDialogue()
    {
        if (!hasActivated && isPlayerInRange && dialogueManager != null && dialogue != null)
        {
            hasActivated = true;
            dialogueManager.StartDialogue(dialogue);
            Debug.Log("Diálogo automático activado: " + gameObject.name);
        }
    }

    // ← MÉTODO PARA ACTIVACIÓN MANUAL (desde Input System)
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Solo funciona si NO es auto-activación y el jugador está en rango
        if (context.started && isPlayerInRange && !hasActivated && !useAutoActivation && dialogueManager != null && dialogue != null)
        {
            TriggerDialogue();
        }
    }

    // ← MÉTODO PARA SKIP (compatible con tu Input System existente)
    public void OnSkipDialogue(InputAction.CallbackContext context)
    {
        if (context.started && dialogueManager != null)
        {
            dialogueManager.DisplayNextLine();
        }
    }

    public void ResetTrigger()
    {
        hasActivated = false;
        isPlayerInRange = false;
        CancelInvoke(nameof(TriggerDialogue));
    }

    // Método público para cambiar el modo en tiempo de ejecución
    public void SetAutoActivation(bool autoActivate)
    {
        useAutoActivation = autoActivate;
    }
}
