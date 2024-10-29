using Firebase.Database;
using UnityEngine;
using TMPro; // Make sure to include this for TextMesh Pro

public class DatabaseManager : MonoBehaviour
{
    public TMP_InputField EmailInput; // Changed to match the AuthManager's Username concept
    public TMP_InputField PasswordInput;

    private DatabaseReference reference;
    private string userID;

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void CreateUser()
    {
        // Create a new user object using the email from EmailInput
        User newUser = new User(EmailInput.text, PasswordInput.text); // Using EmailInput for username

        // Convert the User object to JSON
        string json = JsonUtility.ToJson(newUser);

        // Save the user data in the database under a unique ID
        reference.Child("users").Child(userID).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User created successfully in Firebase.");
            }
            else
            {
                Debug.LogError("Failed to create user: " + task.Exception);
            }
        });
    }
}
