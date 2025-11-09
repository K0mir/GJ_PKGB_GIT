using UnityEngine;
using UnityEngine.InputSystem;
public class DialogoNarrativo : MonoBehaviour
{
    [Header("CONFIGURACIÓN")]
    public string[] textosNarrativos; // Textos que se mostrarán
    public float tiempoEntreLineas = 3f; // Tiempo entre cada línea
    public bool activarAlInicio = true; // ¿Comenzar automáticamente?
    
    [Header("MODO DE VISUALIZACIÓN")]
    public bool usarModoNarrativo = true; // TRUE: Narración, FALSE: Diálogo normal
    
    [Header("CONFIGURACIÓN NARRATIVA")]
    public Color colorTextoNarrativo = Color.white;
    public int tamañoFuenteNarrativo = 28;
    
    [Header("CONFIGURACIÓN DIÁLOGO")]
    public string nombrePersonaje = "NARRADOR"; // Solo se usa en modo diálogo
    public Sprite retratoPersonaje; // Solo se usa en modo diálogo
    
    [Header("REFERENCIAS")]
    public DialogManager2 dialogManager;
    
    private bool jugadorEnRango = false;
    private bool narracionActiva = false;
    private int indiceActual = 0;

    void Start()
    {
        if (dialogManager == null)
        {
            // CAMBIA ESTO:
            // dialogManager = FindObjectOfType<DialogManager>();
            
            // POR ESTO:
            dialogManager = FindFirstObjectByType<DialogManager2>();
        }
        
        if (activarAlInicio)
        {
            IniciarNarracion();
        }
    }

    public void IniciarNarracion()
    {
        if (textosNarrativos.Length == 0 || narracionActiva) return;
        
        narracionActiva = true;
        indiceActual = 0;
        MostrarSiguienteLinea();
    }

    void MostrarSiguienteLinea()
    {
        if (indiceActual >= textosNarrativos.Length)
        {
            TerminarNarracion();
            return;
        }

        // Crear diálogo según el modo elegido
        Dialog dialogo = usarModoNarrativo ? 
            CreateNarrativeDialog(textosNarrativos[indiceActual]) : 
            CreateNormalDialog(textosNarrativos[indiceActual]);
        
        // Mostrar a través del DialogManager
        if (dialogManager != null)
        {
            if (usarModoNarrativo)
            {
                dialogManager.StartNarrativeDialogue(dialogo, this, colorTextoNarrativo, tamañoFuenteNarrativo);
            }
            else
            {
                dialogManager.StartDialogue(dialogo);
            }
        }
        
        indiceActual++;
    }

    // Crear diálogo en modo NARRATIVO (sin nombre/retrato)
    Dialog CreateNarrativeDialog(string texto)
    {
        Dialog dialogo = new Dialog();
        dialogo.dialogueLines = new Dialog.DialogueLine[1];
        
        dialogo.dialogueLines[0] = new Dialog.DialogueLine();
        dialogo.dialogueLines[0].speakerName = ""; // Sin nombre
        dialogo.dialogueLines[0].speakerPortrait = null; // Sin retrato
        dialogo.dialogueLines[0].sentence = texto;
        
        return dialogo;
    }

    // Crear diálogo en modo NORMAL (con nombre/retrato)
    Dialog CreateNormalDialog(string texto)
    {
        Dialog dialogo = new Dialog();
        dialogo.dialogueLines = new Dialog.DialogueLine[1];
        
        dialogo.dialogueLines[0] = new Dialog.DialogueLine();
        dialogo.dialogueLines[0].speakerName = nombrePersonaje;
        dialogo.dialogueLines[0].speakerPortrait = retratoPersonaje;
        dialogo.dialogueLines[0].sentence = texto;
        
        return dialogo;
    }

    public void OnNarrativeLineCompleted()
    {
        if (narracionActiva)
        {
            Invoke(nameof(MostrarSiguienteLinea), tiempoEntreLineas);
        }
    }

    void TerminarNarracion()
    {
        narracionActiva = false;
        Debug.Log("Narración terminada - Modo: " + (usarModoNarrativo ? "Narrativo" : "Diálogo"));
    }

    // Para activar por trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activarAlInicio)
        {
            jugadorEnRango = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
        }
    }

    // Para activar por tecla
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && jugadorEnRango && !narracionActiva)
        {
            IniciarNarracion();
        }
    }

    // Método público para cambiar modo en tiempo de ejecución
    public void CambiarModoVisualizacion(bool usarNarrativo)
    {
        usarModoNarrativo = usarNarrativo;
        Debug.Log("Modo cambiado a: " + (usarNarrativo ? "Narrativo" : "Diálogo"));
    }
}
