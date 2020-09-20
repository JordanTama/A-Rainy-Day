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
        SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "By the way, red buildings can't be moved." });
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        if(arg1.name == "Level1-3")
        {
            SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "If you're having a hard time try changing your perspective." });
        }
        else if (arg1.name == "Level1-5")
        {
            SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "Sometimes buildings are connected. Makes things a bit harder." });
        }
        else if (arg1.name == "Level2-1")
        {
            SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "To get to the next area, you need to walk a certain path." });
        }
        else if (arg1.name == "Level2-3")
        {
            SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "Sometimes you might not be able to go a certain way, try clicking on something nearby." });
        }
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
