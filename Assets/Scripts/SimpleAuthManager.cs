using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SimpleAuthManager : MonoBehaviour
{
    public GameObject LoginPanel;
    public GameObject RegisterUI;
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public TMP_Text WarningText;
    public TMP_InputField RegisterUsernameInput;
    public TMP_InputField RegisterEmailInput;
    public TMP_InputField RegisterPasswordInput;
    public TMP_InputField ConfirmPasswordInput;
    public TMP_Text RegisterWarningText;

    void Start()
    {
        ShowLoginPanel();
    }

    public void ShowRegisterPanel()
    {
        LoginPanel.SetActive(false);
        RegisterUI.SetActive(true);
    }

    public void ShowLoginPanel()
    {
        RegisterUI.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RegisterUser()
    {
        string username = RegisterUsernameInput.text;
        string email = RegisterEmailInput.text;
        string password = RegisterPasswordInput.text;
        string confirmPassword = ConfirmPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            DisplayRegisterWarning("Please fill in all fields.");
            return;
        }

        if (password != confirmPassword)
        {
            DisplayRegisterWarning("Passwords do not match.");
            return;
        }

        string hashedPassword = password.GetHashCode().ToString();

        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("email", email);
        PlayerPrefs.SetString("password", hashedPassword);

        DisplayRegisterWarning("Registration successful!");
        ShowLoginPanel();
    }

    public void LoginUser()
    {
        string email = EmailInput.text;
        string password = PasswordInput.text;

        string storedEmail = PlayerPrefs.GetString("email");
        string storedPassword = PlayerPrefs.GetString("password");
        string hashedPassword = password.GetHashCode().ToString();

        if (storedEmail == email && storedPassword == hashedPassword)
        {
            DisplayWarning("Login successful! Welcome back.");
            LoadMainMenu();
        }
        else
        {
            DisplayWarning("Login failed. Incorrect email or password.");
        }
    }

    private void DisplayWarning(string message)
    {
        WarningText.text = message;
    }

    private void DisplayRegisterWarning(string message)
    {
        RegisterWarningText.text = message;
    }

    
    private void OnApplicationQuit()
    {
        
        PlayerPrefs.DeleteKey("username"); 
        PlayerPrefs.DeleteKey("email");    
        PlayerPrefs.DeleteKey("password");  
        PlayerPrefs.Save(); 
    }
}






