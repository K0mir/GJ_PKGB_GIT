using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogManager2 : MonoBehaviour
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
    
    [Header("ESTILOS POR DEFECTO")]
    public Color colorTextoNormal = Color.white;
    public int tamañoFuenteNormal = 24;
    public Color colorTextoNarrativo = Color.yellow;
    public int tamañoFuenteNarrativo = 28;
    
    private Queue<Dialog.DialogueLine> dialogueLines;
    private bool isTyping = false;
    private Dialog.DialogueLine currentLine;
    private bool canSkip = true;
    private float skipCooldown = 0.3f;
    private Dictionary<string, Sprite> characterPortraits = new Dictionary<string, Sprite>();
    private DialogTriggers currentDialogTrigger;
    private DialogoNarrativo currentNarrative;
    
    // Referencias UI para modificar estilo
    private RectTransform panelRect;
    private Image panelBackground;
    
    // Referencia al indicador de progreso
    private MonoBehaviour progressIndicator;

    void Start()
    {
        dialogueLines = new Queue<Dialog.DialogueLine>();
        dialoguePanel.SetActive(false);
        
        // Obtener referencias UI
        panelRect = dialoguePanel.GetComponent<RectTransform>();
        panelBackground = dialoguePanel.GetComponent<Image>();
        
        // Buscar indicador de progreso
        FindProgressIndicator();
        
        // Buscar CinematicManager si no está asignado
        if (cinematicManager == null)
        {
            // CAMBIA ESTO:
            // cinematicManager = FindObjectOfType<CinematicManager>();
            
            // POR ESTO:
            cinematicManager = FindFirstObjectByType<CinematicManager>();
        }
    }

    void FindProgressIndicator()
    {
        // Buscar cualquier componente que tenga el método ShowProgressIndicator
        MonoBehaviour[] allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        
        foreach (MonoBehaviour behaviour in allBehaviours)
        {
            var method = behaviour.GetType().GetMethod("ShowProgressIndicator");
            if (method != null)
            {
                progressIndicator = behaviour;
                Debug.Log("Indicador de progreso encontrado: " + progressIndicator.GetType().Name);
                return;
            }
        }
        
        Debug.LogWarning("No se pudo encontrar un indicador de progreso");
    }

    // MÉTODO PARA DIÁLOGOS NARRATIVOS
    public void StartNarrativeDialogue(Dialog dialogue, DialogoNarrativo narrative = null, Color? colorTexto = null, int? tamañoFuente = null)
    {
        currentNarrative = narrative;
        ApplyNarrativeStyle(colorTexto ?? colorTextoNarrativo, tamañoFuente ?? tamañoFuenteNarrativo);
        StartDialogue(dialogue);
    }

    // MÉTODO PARA DIÁLOGOS NORMALES
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

    void ApplyNarrativeStyle(Color colorTexto, int tamañoFuente)
    {
        // Ocultar nombre y retrato
        nameText.gameObject.SetActive(false);
        portraitImage.gameObject.SetActive(false);
        
        // Configurar estilo de texto narrativo
        dialogueText.color = colorTexto;
        dialogueText.fontSize = tamañoFuente;
        dialogueText.alignment = TextAlignmentOptions.Top;
        
        // Configurar posición del panel (arriba del todo)
        if (panelRect != null)
        {
            panelRect.anchorMin = new Vector2(0.1f, 0.7f);
            panelRect.anchorMax = new Vector2(0.9f, 0.95f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
        }
        
        // Fondo semi-transparente para mejor lectura
        if (panelBackground != null)
        {
            panelBackground.color = new Color(0, 0, 0, 0.8f);
        }
    }

    void ResetDialogueStyle()
    {
        // Restaurar elementos normales
        nameText.gameObject.SetActive(true);
        portraitImage.gameObject.SetActive(true);
        
        // Restaurar estilo de texto
        dialogueText.color = colorTextoNormal;
        dialogueText.fontSize = tamañoFuenteNormal;
        dialogueText.alignment = TextAlignmentOptions.TopLeft;
        
        // Restaurar posición del panel
        if (panelRect != null)
        {
            panelRect.anchorMin = new Vector2(0.2f, 0.1f);
            panelRect.anchorMax = new Vector2(0.8f, 0.4f);
        }
        
        // Restaurar fondo
        if (panelBackground != null)
        {
            panelBackground.color = new Color(0, 0, 0, 0.7f);
        }
        
        currentNarrative = null;
    }

    public void DisplayNextLine()
    {
        if (!canSkip) return;

        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentLine.sentence;
            isTyping = false;
            StartCoroutine(SkipCooldown());
            return;
        }

        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = dialogueLines.Dequeue();
        UpdateDialogueUI(currentLine);
        StartCoroutine(TypeSentence(currentLine.sentence));
        StartCoroutine(SkipCooldown());
    }

    void UpdateDialogueUI(Dialog.DialogueLine line)
    {
        // Si es narración, ocultar nombre y retrato
        if (currentNarrative != null || string.IsNullOrEmpty(line.speakerName))
        {
            nameText.gameObject.SetActive(false);
            portraitImage.gameObject.SetActive(false);
        }
        else
        {
            nameText.gameObject.SetActive(true);
            nameText.text = line.speakerName;
            
            if (line.speakerPortrait != null)
            {
                portraitImage.gameObject.SetActive(true);
                portraitImage.sprite = line.speakerPortrait;
            }
            else if (characterPortraits.ContainsKey(line.speakerName))
            {
                portraitImage.gameObject.SetActive(true);
                portraitImage.sprite = characterPortraits[line.speakerName];
            }
            else
            {
                portraitImage.gameObject.SetActive(false);
            }
        }
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

        // Notificar a la narración que la línea terminó
        if (currentNarrative != null)
        {
            currentNarrative.OnNarrativeLineCompleted();
        }
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

        // MOSTRAR INDICADOR DE PROGRESO
        if (showProgressIndicator && progressIndicator != null)
        {
            ShowProgressIndicator();
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

        // Notificar a la narración que terminó
        if (currentNarrative != null)
        {
            currentNarrative.OnNarrativeLineCompleted();
            currentNarrative = null;
        }

        // Restaurar estilo normal
        ResetDialogueStyle();
        
        Debug.Log("Diálogo terminado");
    }

     void ShowProgressIndicator()
    {
        if (progressIndicator != null)
        {
            var method = progressIndicator.GetType().GetMethod("ShowProgressIndicator");
            if (method != null)
            {
                method.Invoke(progressIndicator, null);
                Debug.Log("Indicador mostrado: " + progressIndicator.GetType().Name);
            }
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
