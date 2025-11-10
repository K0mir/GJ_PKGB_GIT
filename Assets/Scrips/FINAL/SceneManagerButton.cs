using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerButton : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("Nombre de la escena a cargar")]
    public string sceneName;
    
    [Tooltip("Índice de la escena a cargar (sobrescribe el nombre si no es -1)")]
    public int sceneIndex = -1;
    
    public void LoadTargetScene()
    {
        if (sceneIndex >= 0)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("No se ha especificado ninguna escena para cargar");
        }
    }
}
