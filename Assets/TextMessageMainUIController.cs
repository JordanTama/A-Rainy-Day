using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextMessageMainUIController : MonoBehaviour
{

    [SerializeField] private GameObject receiveMessageUI;
    [SerializeField] private GameObject sendMessageUI;
    [SerializeField] private GameObject messageParent;
    [SerializeField] private float msgTimeMultiplier = 0.1f;

    private TextMessageManager msgMan;
    private TextMessage[] messageBuffer;
    private int currentMsg = 0;
    private float msgTime;
    private float timer = 0;
    private Dictionary<MessageSender, GameObject> msgMap;

    // Start is called before the first frame update
    void Start()
    {
        msgMap = new Dictionary<MessageSender, GameObject>
        {
            {MessageSender.ANNA, sendMessageUI },
            {MessageSender.JESSICA, receiveMessageUI },
        };

        msgMan = ServiceLocator.Current.Get<TextMessageManager>();
        messageBuffer = msgMan.LevelTextMessages[SceneManager.GetActiveScene().name];
        msgTime = messageBuffer[0].MessageText.Length * msgTimeMultiplier;
    }

    private void Update()
    {
        if (currentMsg > messageBuffer.Length)
            return;

        timer += Time.deltaTime;

        if(timer >= msgTime)
        {
            SpawnMessage(messageBuffer[currentMsg]);
            currentMsg++;
            timer = 0;
            msgTime = messageBuffer[currentMsg].MessageText.Length * msgTimeMultiplier;
        }
    }

    private void SpawnMessage(TextMessage textMessage)
    {
        var msg = Instantiate(msgMap[textMessage.Sender], messageParent.transform).GetComponent<TextMessageUIController>();
        msg.SetText(textMessage.MessageText);
    }
}
