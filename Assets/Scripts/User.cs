using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class User : MonoBehaviour 
{
    public string username;
    public string password;

    public User(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}