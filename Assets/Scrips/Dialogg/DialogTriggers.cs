using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogTriggers : MonoBehaviour
{
public Dialog dialogue;
    public string interactionText = "Habla con Franck";
    public GameObject interactionIndicator;
    
    [Header("Configuración de Activación")]
    public bool autoActivate = true; // ← NUEVA VARIABLE: Activar al pasar
    public float activationDelay = 0.2f; // ← Pequeño delay para evitar activaciones accidentales
    
    private bool isPlayerInRange = false;
    private DialogManager dialogueManager;
    private TextMeshProUGUI indicatorText;
    private bool canSkip = true;
    private float skipCooldown = 0.15f;
    private bool dialogueCompleted = false;
    private bool hasActivated = false; // ← Para controlar que no se active múltiples veces

    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogManager>();
        
        if (interactionIndicator != null)
        {
            indicatorText = interactionIndicator.GetComponentInChildren<TextMeshProUGUI>();
            if (indicatorText != null)
            {
                indicatorText.text = interactionText;
            }
            interactionIndicator.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !dialogueCompleted)
        {
            isPlayerInRange = true;
            
            if (autoActivate && !hasActivated) // ← ACTIVACIÓN AUTOMÁTICA
            {
                Invoke(nameof(AutoActivateDialogue), activationDelay);
            }
            else // ← Mostrar indicador solo si no es auto-activación
            {
                ShowInteractionIndicator();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            HideInteractionIndicator();
            
            // Cancelar activación automática si el jugador sale rápido
            if (autoActivate && !hasActivated)
            {
                CancelInvoke(nameof(AutoActivateDialogue));
            }
        }
    }

    // ← NUEVO MÉTODO: Activación automática
    private void AutoActivateDialogue()
    {
        if (isPlayerInRange && !dialogueCompleted && !hasActivated)
        {
            hasActivated = true;
            dialogueManager.StartDialogue(dialogue, this);
            HideInteractionIndicator(); // Ocultar indicador si estaba visible
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Solo funciona si NO es auto-activación
        if (context.started && isPlayerInRange && !dialogueCompleted && !autoActivate)
        {
            HideInteractionIndicator();
            dialogueManager.StartDialogue(dialogue, this);
        }
    }

    public void OnSkipDialogue(InputAction.CallbackContext context)
    {
        if (context.started && canSkip)
        {
            canSkip = false;
            dialogueManager.DisplayNextLine();
            Invoke(nameof(ResetSkip), skipCooldown);
        }
    }

    private void ShowInteractionIndicator()
    {
        if (interactionIndicator != null && !dialogueCompleted && !autoActivate)
        {
            interactionIndicator.SetActive(true);
        }
    }

    private void HideInteractionIndicator()
    {
        if (interactionIndicator != null)
        {
            interactionIndicator.SetActive(false);
        }
    }

    private void ResetSkip()
    {
        canSkip = true;
    }

    public void OnDialogueEnd()
    {
        dialogueCompleted = true;
        hasActivated = false; // ← Resetear para posible reutilización
        HideInteractionIndicator();
        Debug.Log("Diálogo completado");
    }

    // ← NUEVO MÉTODO: Para reutilizar el trigger
    public void ResetTrigger()
    {
        dialogueCompleted = false;
        hasActivated = false;
        CancelInvoke(nameof(AutoActivateDialogue));
    }
}
