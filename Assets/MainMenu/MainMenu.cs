using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PLAY()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void QUIT()
    {
        Debug.Log("Quit Game!");


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
