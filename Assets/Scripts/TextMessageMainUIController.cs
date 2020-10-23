using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextMessageMainUIController : MonoBehaviour
{

    [SerializeField] private GameObject receiveMessageUI;
    [SerializeField] private GameObject sendMessageUI;
    [SerializeField] private GameObject messageParent;
    [SerializeField] private CanvasGroup cGroup;
    [SerializeField] private float msgTimeMultiplier = 0.1f;

    private TextMessageManager msgMan;
    private TextMessage[] messageBuffer;
    private int currentMsg = 0;
    private float msgTime;
    private float timer = 0;
    private Dictionary<MessageSender, GameObject> msgMap;
    private bool waitingForMessage;
    private bool readyToMessage = false;

    // Start is called before the first frame update
    void Start()
    {
        msgMap = new Dictionary<MessageSender, GameObject>
        {
            {MessageSender.ANNA, sendMessageUI },
            {MessageSender.JESSICA, receiveMessageUI },
            {MessageSender.DAD, receiveMessageUI },
            {MessageSender.OTHER, receiveMessageUI },
        };

        ServiceLocator.Current.Get<GameLoopManager>().OnLevelReady += OnLevelReady;
        msgMan = ServiceLocator.Current.Get<TextMessageManager>();

        if (msgMan.LevelTextMessages.ContainsKey(SceneManager.GetActiveScene().name))
            messageBuffer = msgMan.LevelTextMessages[SceneManager.GetActiveScene().name];
        else
            return;

        msgTime = messageBuffer[currentMsg].MessageText.Length * msgTimeMultiplier;
    }

    private void OnLevelReady()
    {
        if(messageBuffer.Length > 0)
            readyToMessage = true;
    }

    private void Update()
    {
        if (!readyToMessage)
            return;

        if (currentMsg >= messageBuffer.Length)
        {
            float x = 0;
            DOTween.To(() => x, y => x = y, 0, 3).OnComplete(() => HideMessages());
            readyToMessage = false;
            return;
        }

        timer += Time.deltaTime;

        if(timer >= msgTime && !waitingForMessage)
        {
            SpawnMessage(messageBuffer[currentMsg]);
            waitingForMessage = true;
        }
    }

    private void HideMessages()
    {
        cGroup.DOFade(0, 3f);
    }

    private void SpawnMessage(TextMessage textMessage)
    {
        var msg = Instantiate(msgMap[textMessage.Sender], messageParent.transform).GetComponent<TextMessageUIController>();
        msg.Init(this, msgTime, textMessage.MessageText);
    }

    public void SentMessage()
    {
        waitingForMessage = false;
        timer = 0;
        msgTime = messageBuffer[currentMsg].MessageText.Length * msgTimeMultiplier;
        currentMsg++;
    }

    private void OnDestroy()
    {
        ServiceLocator.Current.Get<GameLoopManager>().OnLevelReady -= OnLevelReady;
    }
}
