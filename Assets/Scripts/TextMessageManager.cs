using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextMessageManager : IGameService
{
    public Action<TextMessage> OnNewTextMessage;
    public bool NewMessage { private set; get; }


    public void SendNewMessage(TextMessage msg)
    {
        OnNewTextMessage?.Invoke(msg);
        NewMessage = true;
    }

    public void PhoneOpened()
    {
        NewMessage = false;
    }
}
