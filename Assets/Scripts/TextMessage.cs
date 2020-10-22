using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageSender
{
    ANNA,
    JESSICA,
    DAD,
    OTHER,
}

[CreateAssetMenu(fileName ="Text Message")]
public class TextMessage : ScriptableObject
{
    public TextMessage(MessageSender sender, string text)
    {
        this.Sender = sender;
        this.MessageText = text;
    }

    public TextMessage() { }

    public MessageSender Sender;
    public string MessageText;
}
