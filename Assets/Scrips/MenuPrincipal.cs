using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Cargar la escena del juego
    public void PlayGame()
    {
        SceneManager.LoadScene("Nivel1Casa"); // <-- Cambia "Nivel1" por el nombre real de tu escena del juego
    }

    // Salir del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
