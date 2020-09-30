using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMessageUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    public void SetText(string text)
    {
        messageText.text = text;
    }
}
