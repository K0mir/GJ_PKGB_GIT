using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class AdvancedProgressIndicator : MonoBehaviour
{
 [Header("CONFIGURACIÓN DEL INDICADOR")]
    public string message = "Avanza para seguir el nivel";
    public float displayDuration = 5f;
    public bool showOnce = true;
    
    [Header("POSICIÓN EN PANTALLA")]
    [Range(0, 1)] public float screenX = 0.5f; // 0 = izquierda, 1 = derecha
    [Range(0, 1)] public float screenY = 0.2f; // 0 = abajo, 1 = arriba
    
    [Header("APARIENCIA")]
    public Color textColor = Color.yellow;
    public int fontSize = 36;
    public bool showBackground = true;
    public Color backgroundColor = new Color(0, 0, 0, 0.8f);
    
    private GameObject indicatorCanvas;
    private TextMeshProUGUI indicatorText;
    private CanvasGroup canvasGroup;
    private bool hasBeenShown = false;
    private bool isShowing = false;

    void Start()
    {
        CreateOverlayIndicator();
        HideIndicatorImmediate();
    }

    void CreateOverlayIndicator()
    {
        // 1. CREAR CANVAS EN SCREEN SPACE - OVERLAY
        indicatorCanvas = new GameObject("OverlayProgressIndicator");
        Canvas canvas = indicatorCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // Orden muy alto para que esté encima de todo
        
        // Canvas Scaler para escalado responsive
        CanvasScaler scaler = indicatorCanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        
        // Graphic Raycaster (opcional)
        indicatorCanvas.AddComponent<GraphicRaycaster>();
        
        // 2. CANVAS GROUP PARA FADE
        canvasGroup = indicatorCanvas.AddComponent<CanvasGroup>();
        
        // 3. PANEL DE FONDO (OPCIONAL)
        if (showBackground)
        {
            GameObject background = new GameObject("Background");
            background.transform.SetParent(indicatorCanvas.transform);
            
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = backgroundColor;
            
            // Configurar RectTransform para el fondo
            RectTransform bgRect = background.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(screenX - 0.15f, screenY - 0.05f);
            bgRect.anchorMax = new Vector2(screenX + 0.15f, screenY + 0.05f);
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
        }
        
        // 4. CREAR TEXTO
        GameObject textObj = new GameObject("ProgressText");
        textObj.transform.SetParent(indicatorCanvas.transform);
        
        indicatorText = textObj.AddComponent<TextMeshProUGUI>();
        indicatorText.text = message;
        indicatorText.fontSize = fontSize;
        indicatorText.alignment = TextAlignmentOptions.Center;
        indicatorText.color = textColor;
        indicatorText.fontStyle = FontStyles.Bold;
        
        // 5. CONFIGURAR POSICIÓN DEL TEXTO
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(screenX, screenY);
        textRect.anchorMax = new Vector2(screenX, screenY);
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = new Vector2(400, 60);
        
        // 6. OUTLINE PARA MEJOR VISIBILIDAD
        try
        {
            var outline = textObj.AddComponent<Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(2, 2);
        }
        catch
        {
            // Si falla el outline, usar shadow alternativa
            indicatorText.color = new Color(1f, 0.9f, 0f, 1f); // Amarillo más fuerte
        }
        
        Debug.Log("Indicador Overlay creado correctamente");
    }

    public void ShowProgressIndicator()
    {
        if (hasBeenShown && showOnce) return;
        
        StartCoroutine(ShowIndicatorCoroutine());
    }

    private IEnumerator ShowIndicatorCoroutine()
    {
        if (isShowing) yield break;
        
        isShowing = true;
        hasBeenShown = true;
        
        Debug.Log("Mostrando indicador overlay...");
        
        // Fade in
        indicatorCanvas.SetActive(true);
        canvasGroup.alpha = 0f;
        
        float timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / 0.5f);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        
        // Esperar el tiempo de display
        yield return new WaitForSeconds(displayDuration);
        
        // Fade out
        timer = 0f;
        while (timer < 0.8f)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / 0.8f);
            yield return null;
        }
        
        HideIndicatorImmediate();
        isShowing = false;
        
        Debug.Log("Indicador overlay ocultado");
    }

    private void HideIndicatorImmediate()
    {
        if (indicatorCanvas != null)
        {
            indicatorCanvas.SetActive(false);
            canvasGroup.alpha = 0f;
        }
    }

    public void ResetIndicator()
    {
        StopAllCoroutines();
        HideIndicatorImmediate();
        hasBeenShown = false;
        isShowing = false;
    }

    // Método para cambiar posición en tiempo de ejecución
    public void SetPosition(float x, float y)
    {
        screenX = x;
        screenY = y;
        
        // Actualizar posición del texto si existe
        if (indicatorText != null)
        {
            RectTransform textRect = indicatorText.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(screenX, screenY);
            textRect.anchorMax = new Vector2(screenX, screenY);
            textRect.anchoredPosition = Vector2.zero;
        }
    }
}
