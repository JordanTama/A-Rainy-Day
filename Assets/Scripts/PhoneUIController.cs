using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhoneUIController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject receiveMessageUI;
    [SerializeField] private GameObject sendMessageUI;
    [SerializeField] private GameObject messageParent;
    [SerializeField] private Image notificationLight;

    private RectTransform myRect;
    private Dictionary<MessageSender, GameObject> msgMap;
    private bool isShowing = false;
    private TextMessageManager messageManager;

    // Start is called before the first frame update
    void Start()
    {
        msgMap = new Dictionary<MessageSender, GameObject>
        {
            {MessageSender.PLAYER, sendMessageUI },
            {MessageSender.DAD, receiveMessageUI },
        };
        myRect = GetComponent<RectTransform>();
        messageManager = ServiceLocator.Current.Get<TextMessageManager>();
        messageManager.OnNewTextMessage += ShowNewMessage;

        foreach (var m in messageManager.AllSentMessages)
        {
            ShowNewMessage(m);
        }
    }

    public void ShowNewMessage(TextMessage textMessage)
    {
        var msg = Instantiate(msgMap[textMessage.Sender], messageParent.transform).GetComponent<TextMessageUIController>();
        msg.SetText(textMessage.MessageText);
    }

    private void OnDestroy()
    {
        ServiceLocator.Current.Get<TextMessageManager>().OnNewTextMessage -= ShowNewMessage;
    }

    private void Update()
    {
        if (notificationLight == null)
            return;

        if (messageManager.NewMessage)
        {
            notificationLight.color = Color.Lerp(new Color(0,0,0,0), new Color(1,0,0,1), Mathf.Abs(Mathf.Sin(Time.time * 5)));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isShowing)
        {
            myRect.DOAnchorPosY(-500, 0.5f).OnComplete(() => isShowing = false);
        }
        else
        {
            myRect.DOAnchorPosY(0, 0.5f).OnComplete(() => isShowing = true);
            messageManager.PhoneOpened();
            notificationLight.color = new Color(0, 0, 0, 0);
        }
    }
}
