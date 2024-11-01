using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text LoggedInUserText; 

    void Start()
    {
        
        string loggedInUser = PlayerPrefs.GetString("username", "Guest");
        UpdateLoggedInUserText(loggedInUser);
    }

    private void UpdateLoggedInUserText(string username)
    {
        
        LoggedInUserText.text = "Logged in as: " + username;
    }

    public void PLAY()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ACCOUNT()
    {
        SceneManager.LoadScene("Accounts");
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
