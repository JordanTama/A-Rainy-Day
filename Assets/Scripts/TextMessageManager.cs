using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TextMessageManager : IGameService
{
    public List<TextMessage> AllSentMessages = new List<TextMessage>();
    public Action<TextMessage> OnNewTextMessage;
    public bool NewMessage { private set; get; } = false;

    public TextMessageManager()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "Yo. If you want to move tiles around, click and drag them in the direction you want to move them." });
        SendNewMessage(new TextMessage() { Sender = MessageSender.PLAYER, MessageText = "Thank you." });
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {

    }

    public void SendNewMessage(TextMessage msg)
    {
        OnNewTextMessage?.Invoke(msg);
        AllSentMessages.Add(msg);
        NewMessage = true;
    }

    public void PhoneOpened()
    {
        NewMessage = false;
    }
}
