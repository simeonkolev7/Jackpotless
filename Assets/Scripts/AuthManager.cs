using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Firebase.Extensions;
using TMPro; // Ensure this is included for TextMesh Pro

public class AuthManager : MonoBehaviour
{
    public TMP_InputField EmailInput; // Use TMP_InputField for TextMesh Pro
    public TMP_InputField PasswordInput; // Use TMP_InputField for TextMesh Pro
    public TMP_Text FeedbackText; // Changed to TMP_Text for TextMesh Pro feedback

    private FirebaseAuth auth;
    private DatabaseReference databaseReference;

    void Start()
    {
        // Initialize Firebase Auth and Database
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Set focus on the EmailInput field
        EmailInput.Select(); // Set focus to the EmailInput field
        EmailInput.ActivateInputField(); // Activate the input field to allow typing
    }

    // Method to Register a New User
    public void RegisterUser()
    {
        string email = EmailInput.text;
        string password = PasswordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                DisplayFeedback("Registration was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                DisplayFeedback("Registration error: " + task.Exception?.Flatten().Message);
                return;
            }

            FirebaseUser newUser = auth.CurrentUser;  // Get the current user instead of task.Result
            DisplayFeedback("Registration successful! Welcome, " + newUser.Email);
            SaveUserData(newUser.UserId, 0, 0); // Initialize high score and win count to 0
        });
    }

    // Method to Log In an Existing User
    public void LoginUser()
    {
        string email = EmailInput.text;
        string password = PasswordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                DisplayFeedback("Login was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                DisplayFeedback("Login error: " + task.Exception?.Flatten().Message);
                return;
            }

            FirebaseUser loggedInUser = auth.CurrentUser;  // Get the current user instead of task.Result
            DisplayFeedback("Login successful! Welcome back, " + loggedInUser.Email);
            LoadUserData(loggedInUser.UserId);
        });
    }

    // Method to Save User Data to Firebase Database
    private void SaveUserData(string userId, int highScore, int wins)
    {
        UserData userData = new UserData(highScore, wins);
        string json = JsonUtility.ToJson(userData);
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    // Method to Load User Data from Firebase Database
    private void LoadUserData(string userId)
    {
        databaseReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                DisplayFeedback("Error loading data.");
                return;
            }
            if (task.IsCompleted && task.Result.Exists)
            {
                UserData userData = JsonUtility.FromJson<UserData>(task.Result.GetRawJsonValue());
                DisplayFeedback("High Score: " + userData.highScore + ", Wins: " + userData.wins);
            }
            else
            {
                DisplayFeedback("No data found for user.");
            }
        });
    }

    // Helper method to display feedback to the user
    private void DisplayFeedback(string message)
    {
        FeedbackText.text = message; // Update feedback text using TMP_Text
    }
}


