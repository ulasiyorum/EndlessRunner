using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Messaging;
using System;

public class Notification : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseMessaging.TokenReceived += TokenReceived;
        FirebaseMessaging.MessageReceived += MessageReceived;
        
    }

    private void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Message Received: " + e.Message);
    }

    private void TokenReceived(object sender, TokenReceivedEventArgs e)
    {
        Debug.Log("Token Received");
    }

}
