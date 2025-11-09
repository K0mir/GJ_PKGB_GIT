using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogTriggers : MonoBehaviour
{
public Dialog dialogue;
    public string interactionText = "Habla con Franck"; // Texto personalizable
    public GameObject interactionIndicator; // Referencia al objeto del indicador
    
    private bool isPlayerInRange = false;
    private DialogManager dialogueManager;
    private TextMeshProUGUI indicatorText;

    private bool canSkip = true;
    private float skipCooldown = 0.15f;
    
    // NUEVA VARIABLE: Controla si el diálogo ya fue completado
    private bool dialogueCompleted = false;

    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogManager>();
        
        // Configurar el indicador si existe
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
        if (other.CompareTag("Player") && !dialogueCompleted) // SOLO si no se completó
        {
            isPlayerInRange = true;
            ShowInteractionIndicator();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            HideInteractionIndicator();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && isPlayerInRange && !dialogueCompleted) // SOLO si no se completó
        {
            HideInteractionIndicator(); // Ocultar al interactuar
            dialogueManager.StartDialogue(dialogue, this); // Asegúrate de pasar 'this'
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
        if (interactionIndicator != null && !dialogueCompleted) // SOLO si no se completó
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

    // Método para que el DialogManager pueda ocultar el indicador cuando termine el diálogo
    public void OnDialogueEnd()
    {
        // MARCA EL DIÁLOGO COMO COMPLETADO
        dialogueCompleted = true;
        
        // Asegurarse de ocultar el indicador
        HideInteractionIndicator();
        
        Debug.Log("Diálogo completado - El indicador no aparecerá más");
    }
}
