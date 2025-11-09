using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class DialogManager : MonoBehaviour
{
  [Header("UI Referencias")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public GameObject dialoguePanel;

    [Header("Configuración de Indicadores")]
    public bool showProgressIndicator = true;
    
    [Header("Configuración de Cinemática")]
    public bool playCinematicAfterDialogue = false;
    public CinematicManager cinematicManager;
    
    private Queue<Dialog.DialogueLine> dialogueLines;
    private bool isTyping = false;
    private Dialog.DialogueLine currentLine;
    private bool canSkip = true;
    private float skipCooldown = 0.3f;
    private Dictionary<string, Sprite> characterPortraits = new Dictionary<string, Sprite>();
    private DialogTriggers currentDialogTrigger;
    
    // Referencia al indicador de progreso (ahora busca cualquier tipo)
    private MonoBehaviour progressIndicator;

    void Start()
    {
        dialogueLines = new Queue<Dialog.DialogueLine>();
        dialoguePanel.SetActive(false);
        
        // Buscar cualquier tipo de indicador de progreso
        FindProgressIndicator();
        
        // Buscar CinematicManager si no está asignado
        if (cinematicManager == null)
        {
            cinematicManager = FindFirstObjectByType<CinematicManager>();
        }
    }

    void FindProgressIndicator()
    {
        // Buscar ProgressIndicator (Screen Space)
        progressIndicator = FindFirstObjectByType<AdvancedProgressIndicator>();
        
        // Si no encuentra, buscar SimpleOverlayIndicator
        if (progressIndicator == null)
        {
            progressIndicator = FindFirstObjectByType<AdvancedProgressIndicator>();
        }
        
        // Si no encuentra, buscar AdvancedProgressIndicator (si existe)
        if (progressIndicator == null)
        {
            progressIndicator = FindFirstObjectByType<AdvancedProgressIndicator>();
        }
        
        // Si no encuentra ninguno, crear uno automáticamente
        if (progressIndicator == null)
        {
            CreateAutoProgressIndicator();
        }
        
        if (progressIndicator != null)
        {
            Debug.Log("Indicador de progreso encontrado: " + progressIndicator.GetType().Name);
        }
        else
        {
            Debug.LogWarning("No se pudo encontrar o crear un indicador de progreso");
        }
    }

    void CreateAutoProgressIndicator()
    {
        GameObject indicatorObj = new GameObject("AutoProgressIndicator");
        progressIndicator = indicatorObj.AddComponent<AdvancedProgressIndicator>();
        Debug.Log("Indicador automático creado");
    }

    public void StartDialogue(Dialog dialogue, DialogTriggers trigger = null)
    {
        // No iniciar diálogo si hay una cinemática en curso
        if (cinematicManager != null && cinematicManager.IsCinematicPlaying())
        {
            Debug.LogWarning("No se puede iniciar diálogo durante una cinemática");
            return;
        }

        StopAllCoroutines();
        dialogueLines.Clear();
        characterPortraits.Clear();

        currentDialogTrigger = trigger;

        // Almacenar retratos
        foreach (var line in dialogue.dialogueLines)
        {
            if (!characterPortraits.ContainsKey(line.speakerName))
            {
                characterPortraits[line.speakerName] = line.speakerPortrait;
            }
        }

        dialoguePanel.SetActive(true);

        foreach (Dialog.DialogueLine line in dialogue.dialogueLines)
        {
            dialogueLines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (!canSkip) return;

        // Si se está escribiendo, termina de mostrar la línea actual
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentLine.sentence;
            isTyping = false;
            StartCoroutine(SkipCooldown());
            return;
        }

        // Si ya no hay más líneas, cerrar el diálogo y mostrar indicador
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Mostrar la siguiente línea
        currentLine = dialogueLines.Dequeue();
        UpdateDialogueUI(currentLine);
        StartCoroutine(TypeSentence(currentLine.sentence));
        StartCoroutine(SkipCooldown());
    }

    void UpdateDialogueUI(Dialog.DialogueLine line)
    {
        nameText.text = line.speakerName;
        
        if (line.speakerPortrait != null)
        {
            portraitImage.sprite = line.speakerPortrait;
        }
        else if (characterPortraits.ContainsKey(line.speakerName))
        {
            portraitImage.sprite = characterPortraits[line.speakerName];
        }
        
        portraitImage.gameObject.SetActive(portraitImage.sprite != null);
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    IEnumerator SkipCooldown()
    {
        canSkip = false;
        yield return new WaitForSeconds(skipCooldown);
        canSkip = true;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueLines.Clear();
        isTyping = false;
        canSkip = true;
        characterPortraits.Clear();

        // MOSTRAR INDICADOR DE PROGRESO (MANERA CORRECTA)
        if (showProgressIndicator && progressIndicator != null)
        {
            ShowProgressIndicator();
        }
        else if (showProgressIndicator)
        {
            Debug.LogWarning("showProgressIndicator está activado pero no hay indicador disponible");
        }

        // REPRODUCIR CINEMÁTICA SI ESTÁ CONFIGURADO
        if (playCinematicAfterDialogue && cinematicManager != null)
        {
            Debug.Log("Iniciando cinemática después del diálogo");
            cinematicManager.PlayCinematic();
        }

        // Notificar al trigger que el diálogo terminó
        if (currentDialogTrigger != null)
        {
            currentDialogTrigger.OnDialogueEnd();
            currentDialogTrigger = null;
        }
        
        Debug.Log("Diálogo terminado");
    }

    void ShowProgressIndicator()
    {
        // Usar reflexión para llamar al método correcto según el tipo de indicador
        var method = progressIndicator.GetType().GetMethod("ShowProgressIndicator");
        if (method != null)
        {
            method.Invoke(progressIndicator, null);
            Debug.Log("Indicador mostrado: " + progressIndicator.GetType().Name);
        }
        else
        {
            Debug.LogError("El indicador no tiene el método ShowProgressIndicator: " + progressIndicator.GetType().Name);
        }
    }
    
    // Método público para configurar la cinemática desde otros scripts
    public void SetCinematicAfterDialogue(bool playCinematic, CinematicManager manager = null)
    {
        playCinematicAfterDialogue = playCinematic;
        if (manager != null)
        {
            cinematicManager = manager;
        }
    }
}
