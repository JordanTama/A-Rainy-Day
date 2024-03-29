﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhoneUIController : MonoBehaviour
{
    [SerializeField] private GameObject receiveMessageUI;
    [SerializeField] private GameObject sendMessageUI;
    [SerializeField] private GameObject messageParent;
    [SerializeField] private Image notificationLight;

    public TextMessage[] MessageBuffer;

    private RectTransform myRect;
    private Dictionary<MessageSender, GameObject> msgMap;
    private bool isShowing = false;
    private TextMessageManager messageManager;
    private bool isLightFlashing = false;

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

        myRect = GetComponent<RectTransform>();
        messageManager = ServiceLocator.Current.Get<TextMessageManager>();
        //messageManager.OnNewTextMessage += ShowNewMessage;

        foreach (KeyValuePair<string, TextMessage[]> kvp in messageManager.LevelTextMessages)
        {
            foreach (var msg in kvp.Value)
            {
                ShowNewMessage(msg);
            }
            if (kvp.Key == SceneManager.GetActiveScene().name)
                break;
        }
    }

    public void ShowNewMessage(TextMessage textMessage)
    {
        var msg = Instantiate(msgMap[textMessage.Sender], messageParent.transform).GetComponent<TextMessageUIController>();
        msg.Init(textMessage.MessageText);
    }

    private void OnDestroy()
    {
        //ServiceLocator.Current.Get<TextMessageManager>().OnNewTextMessage -= ShowNewMessage;
    }

    private void Update()
    {
        if (notificationLight == null)
            return;

        if (messageManager.NewMessage)
        {
            notificationLight.color = Color.Lerp(new Color(0,0,0,0), new Color(1,0,0,1), Mathf.Abs(Mathf.Sin(Time.time * 2.5f)));

            if (notificationLight.color.r >= 0.5f && !isLightFlashing)
            {
                isLightFlashing = true;
                //messageManager.OnLightFlash?.Invoke();
            }

            if (notificationLight.color.r <= 0.1f && isLightFlashing)
            {
                isLightFlashing = false;
            }
        }
        
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (isShowing)
    //    {
    //        myRect.DOAnchorPosY(-500, 0.5f).OnComplete(() => isShowing = false);
    //    }
    //    else
    //    {
    //        myRect.DOAnchorPosY(0, 0.5f).OnComplete(() => isShowing = true);
    //        notificationLight.color = new Color(0, 0, 0, 0);
    //    }
        
    //    //messageManager.OnPhoneShow?.Invoke(isShowing);
    //}
}
