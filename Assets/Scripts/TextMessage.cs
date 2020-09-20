using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageSender
{
    PLAYER,
    DAD,
}

[CreateAssetMenu(fileName ="Text Message")]
public class TextMessage : ScriptableObject
{
    public MessageSender Sender;
    public string MessageText;
}
