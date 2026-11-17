
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ControlMenu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void VerCreditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Salir()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
