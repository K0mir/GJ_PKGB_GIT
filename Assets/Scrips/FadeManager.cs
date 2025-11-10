using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class FadeManager : MonoBehaviour
{
    [Header("Configuración de Fade")]
    public Image fadeImage;
    public float fadeDuration = 2.0f;
    
    private static FadeManager instance;
    
    public static FadeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<FadeManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("FadeManager");
                    instance = obj.AddComponent<FadeManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Crear la imagen de fade si no existe
        if (fadeImage == null)
        {
            CreateFadeImage();
        }
    }

    void Start()
    {
        // Iniciar con pantalla en negro y hacer fade in
        StartCoroutine(StartGameFade());
    }

    void CreateFadeImage()
    {
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // Muy alto para estar sobre todo
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform);
        
        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = Color.black;
        fadeImage.rectTransform.anchorMin = Vector2.zero;
        fadeImage.rectTransform.anchorMax = Vector2.one;
        fadeImage.rectTransform.offsetMin = Vector2.zero;
        fadeImage.rectTransform.offsetMax = Vector2.zero;
        
        DontDestroyOnLoad(canvasObj);
    }

    IEnumerator StartGameFade()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = Color.black;
        
        yield return new WaitForSeconds(0.5f);
        
        // Fade in al iniciar el juego
        yield return StartCoroutine(FadeFromTo(1f, 0f, fadeDuration));
        
        fadeImage.gameObject.SetActive(false);
    }

    public void FadeToBlackAndLoadEnding()
    {
        StartCoroutine(FadeToEnding());
    }

    private IEnumerator FadeToEnding()
    {
        // Activar la imagen de fade
        fadeImage.gameObject.SetActive(true);
        
        // Fade a negro
        yield return StartCoroutine(FadeFromTo(0f, 1f, fadeDuration));
        
        // Esperar un momento en negro
        yield return new WaitForSeconds(1f);
        
        // Mostrar la pantalla final
        ShowEndingScreen();
    }

    private IEnumerator FadeFromTo(float from, float to, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        
        fadeImage.color = new Color(0, 0, 0, to);
    }

    private void ShowEndingScreen()
    {
        // Crear la pantalla de final
        GameObject canvasObj = new GameObject("EndingCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10000;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Fondo
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvasObj.transform);
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.3f, 1f);
        
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        // Texto de final
        GameObject textObj = new GameObject("EndingText");
        textObj.transform.SetParent(canvasObj.transform);
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = "FIN DEL JUEGO\n\n¡Gracias por jugar!";
        text.fontSize = 48;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.1f, 0.4f);
        textRect.anchorMax = new Vector2(0.9f, 0.6f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        // Botón de salir (opcional)
        CreateExitButton(canvasObj);
    }

    private void CreateExitButton(GameObject parent)
    {
        GameObject buttonObj = new GameObject("ExitButton");
        buttonObj.transform.SetParent(parent.transform);
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.2f, 0.8f, 1f);
        
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(ExitGame);
        
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.4f, 0.2f);
        buttonRect.anchorMax = new Vector2(0.6f, 0.3f);
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;
        
        // Texto del botón
        GameObject buttonTextObj = new GameObject("ButtonText");
        buttonTextObj.transform.SetParent(buttonObj.transform);
        TextMeshProUGUI buttonText = buttonTextObj.AddComponent<TextMeshProUGUI>();
        buttonText.text = "SALIR";
        buttonText.fontSize = 24;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.white;
        
        RectTransform buttonTextRect = buttonTextObj.GetComponent<RectTransform>();
        buttonTextRect.anchorMin = Vector2.zero;
        buttonTextRect.anchorMax = Vector2.one;
        buttonTextRect.offsetMin = Vector2.zero;
        buttonTextRect.offsetMax = Vector2.zero;
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
