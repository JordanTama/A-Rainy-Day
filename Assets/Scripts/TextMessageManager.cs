using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TextMessageManager : IGameService
{
    public Dictionary<string, TextMessage[]> LevelTextMessages = new Dictionary<string, TextMessage[]>
    {
        {"Level1-1", new TextMessage[] { 
            new TextMessage(MessageSender.ANNA, "Hey Jess, you're coming back today right?"), 
            new TextMessage(MessageSender.JESSICA, "Yeah, I’m about to get on the train now"), 
            new TextMessage(MessageSender.JESSICA, "How's work?"), 
            new TextMessage(MessageSender.ANNA, "Yeah not bad, everything has just been sliding into place"), 
        } },
        {"Level1-2", new TextMessage[] {
            new TextMessage(MessageSender.JESSICA, "You’re visiting your dad today right?"),
            new TextMessage(MessageSender.ANNA, "Yeah, surprised you remembered"),
            new TextMessage(MessageSender.JESSICA, "RUDE"),
            new TextMessage(MessageSender.JESSICA, "I only need to remember the important stuff"),
            new TextMessage(MessageSender.ANNA, "Yeah? Where did we go on our sixth date?"),
            new TextMessage(MessageSender.JESSICA, "Hiking"),
            new TextMessage(MessageSender.ANNA, "Really??? That was supposed to be an easy one as well."),
        } },
        {"Level1-3", new TextMessage[] {
            new TextMessage(MessageSender.JESSICA, "Whaaat, when did we go hiking then?"),
            new TextMessage(MessageSender.ANNA, "Sometime afterwards, like the eighth or ninth date?"),
            new TextMessage(MessageSender.JESSICA, "Oh right, I just got mixed up"),
            new TextMessage(MessageSender.ANNA, "?"),
            new TextMessage(MessageSender.JESSICA, "If you rotate things around you can see that 6 becomes 9."),
            new TextMessage(MessageSender.JESSICA, "A simple mistake ;)"),
            new TextMessage(MessageSender.ANNA, "OMG that’s not a valid excuse"),
        } },
        {"Level1-4", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "BTW, how was your concert? I know you’ve been practicing really hard for it"),
            new TextMessage(MessageSender.JESSICA, "It didn’t go that great, I messed up a lot."),
            new TextMessage(MessageSender.JESSICA, "But I’m okay really! It’s just that, I’ve been thinking If I’m really suited to music"),
            new TextMessage(MessageSender.ANNA, "Don’t say that. Maybe smaller jams and recording in studios is just more your style"),
            new TextMessage(MessageSender.JESSICA, "Yeah I guess so, I always thought that holding concerts was the end goal but maybe it’s not always that set in stone."),
        } },
        {"Level1-5", new TextMessage[] {
            new TextMessage(MessageSender.JESSICA, "You know, sometimes I feel like I always have you listen to stuff about me. Must get boring for you right?"),
            new TextMessage(MessageSender.ANNA, "Not at all, It should only be normal that I enjoy hearing things about someone I like"),
            new TextMessage(MessageSender.JESSICA, "But you know, I also wanna hear about you more, we haven’t texted much in the past week since I went to Kyoto for that gig."),
            new TextMessage(MessageSender.ANNA, "Nothing exciting really happens. Just the typical 9 to 5. I did fix a bug on my website I’m really happy about though."),
            new TextMessage(MessageSender.ANNA, "Ah sorry, I’m getting into geeky talk again."),
            new TextMessage(MessageSender.JESSICA, "No I get it. How many times have I told you that you don’t have to be sorry."),
            new TextMessage(MessageSender.JESSICA, "You did something well and you're proud of it :) Keep it up 👍"),
        } },
        {"Level1-6", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "Ah"),
            new TextMessage(MessageSender.JESSICA, "You were about to say sorry again weren’t you XD Old habits die hard I guess"),
            new TextMessage(MessageSender.JESSICA, "Well the trains getting to a place that has notoriously bad reception so is there anything you want me to get along the way?"),
            new TextMessage(MessageSender.ANNA, "Same. Not really, we can get some stuff for dinner tonight once we meet up at Uranohoshi"),
            new TextMessage(MessageSender.JESSICA, "Sounds good, stay safe <3"),
            new TextMessage(MessageSender.JESSICA, "See you then <3"),
        } },
    };

    //public List<TextMessage> AllSentMessages = new List<TextMessage>();
    //public Action<TextMessage> OnNewTextMessage;
    //public Action OnLightFlash;
    //public Action<bool> OnPhoneShow;
    public bool NewMessage { private set; get; } = false;

    public TextMessageManager()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        //SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "Yo. If you want to move tiles around, click and drag them in the direction you want to move them." });
        //SendNewMessage(new TextMessage() { Sender = MessageSender.ANNA, MessageText = "Thank you." });
        //SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "By the way, red buildings can't be moved." });
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        
    }

    public void SendNewMessage(TextMessage msg)
    {
        //OnNewTextMessage?.Invoke(msg);
        //AllSentMessages.Add(msg);
    }
}
