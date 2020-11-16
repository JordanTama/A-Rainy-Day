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
            new TextMessage(MessageSender.JESSICA, "But I’m okay really! It’s just that, I’ve been thinking if I’m really suited to music"),
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
            new TextMessage(MessageSender.JESSICA, "You did something well and you're proud of it :) Keep it up"),
        } },
        {"Level1-6", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "Ah"),
            new TextMessage(MessageSender.JESSICA, "You were about to say sorry again weren’t you XD Old habits die hard I guess"),
            new TextMessage(MessageSender.JESSICA, "Well the trains getting to a place that has notoriously bad reception so is there anything you want me to get along the way?"),
            new TextMessage(MessageSender.ANNA, "Same. Not really, we can get some stuff for dinner tonight once we meet up at Uranohoshi"),
            new TextMessage(MessageSender.JESSICA, "Sounds good, stay safe <3"),
            new TextMessage(MessageSender.JESSICA, "See you then <3"),
        } },
        {"Level2-1", new TextMessage[] {
            new TextMessage(MessageSender.DAD, "You doing okay sweetheart?"),
            new TextMessage(MessageSender.ANNA, "No dad, I can’t deal with it anymore."),
            new TextMessage(MessageSender.ANNA, "I just get sh*t on for everything I do. No one understands me"),
            new TextMessage(MessageSender.DAD, "You’ll be alright, ignore what they say and do what makes you happy"),
            new TextMessage(MessageSender.ANNA, "How am I supposed to do that If I go to school?"),
            new TextMessage(MessageSender.DAD, "Step by step. I’m not saying it’s going to be fixed in an instant. There’s gates you have to pass through and each time you do, it’s going to get a little better."),
        } },
        {"Level2-2", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "Easier said than done, there’s no way you would say that if you were in my position."),
            new TextMessage(MessageSender.DAD, "I never said it would be easy. Do your own thing and befriend the people that respect that"),
            new TextMessage(MessageSender.ANNA, "I just can’t relate to other people, everyone just thinks and act so differently"),
            new TextMessage(MessageSender.ANNA, "Like all they do is gossip, talk about clothes and relationships. I haven’t been able to really connect with someone this whole time."),
            new TextMessage(MessageSender.DAD, "Everyones different just like you, but a lot of people are going to act in a way that lets them fit in. You don’t do that and that makes you special."),
        } },
        {"Level2-3", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "Yeah? What do I do if everyone hates me?"),
            new TextMessage(MessageSender.ANNA, "There’s so much crap being spread around about me by the popular girls"),
            new TextMessage(MessageSender.DAD, "Stay true to yourself and people will catch on that they are lies"),
            new TextMessage(MessageSender.DAD, "Things aren’t always going to fix themselves"),
            new TextMessage(MessageSender.ANNA, "You think I haven’t tried?"),
            new TextMessage(MessageSender.DAD, "I’m not saying that. I’m sure there’s things you haven’t tried though, just keep tapping into new things and I’m sure you’ll meet someone."),
        } },
        {"Level2-4", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "That was so cheesy, you reciting some quote from a movie or something?"),
            new TextMessage(MessageSender.DAD, "I’ve just been through more than you. I’m almost 50 now you know?"),
            new TextMessage(MessageSender.ANNA, "What were you like in school?"),
            new TextMessage(MessageSender.DAD, "I did quite well. Topped everyone in grades, was the captain of the rugby team and got into every party I wanted :)"),
            new TextMessage(MessageSender.ANNA, "WTF. So you're just spitting bullshit about things that worked in your imaginary world of what I’m going through???"),
            new TextMessage(MessageSender.DAD, "I never said I know your situation, but those things didn’t make my life perfect either."),
        } },
        {"Level2-5", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "What do you mean it wasn’t perfect? You had everything"),
            new TextMessage(MessageSender.DAD, "It came with its own challenges. Just cause I was at parties didn’t mean I was very close to anyone."),
            new TextMessage(MessageSender.DAD, "There was always a distance between me and others because they didn’t see me as one of them"),
            new TextMessage(MessageSender.ANNA, "How did you deal with that?"),
            new TextMessage(MessageSender.DAD, "I’m not sure really, but I just kept doing what I thought was right and stayed open to meeting new people."),
        } },
        {"Level2-6", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "Thanks dad, I’ll keep trying at school"),
            new TextMessage(MessageSender.DAD, "No problem, I know you’ll get through it. Is your mum home yet?"),
            new TextMessage(MessageSender.ANNA, "Nah not yet, she’s going out with friends tonight."),
            new TextMessage(MessageSender.DAD, "Are you working on your website then?"),
            new TextMessage(MessageSender.ANNA, "haha yeah"),
            new TextMessage(MessageSender.DAD, "Show it to me sweetheart?"),
            new TextMessage(MessageSender.ANNA, "I’ll send you the link when I’m done :)"),
        } },
        {"Level3-1", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "Do you think it’s the right time to tell my dad?"),
            new TextMessage(MessageSender.JESSICA, "I’m not sure, you know him best."),
            new TextMessage(MessageSender.ANNA, "He’s really conservative… maybe I could just hide it."),
            new TextMessage(MessageSender.JESSICA, "C'mon, I’m sure it’ll be okay. Can’t really hide it forever."),
            new TextMessage(MessageSender.JESSICA, " I’m sure once you get past this, your relationship with him will reach new heights you never expected."),
            new TextMessage(MessageSender.ANNA, "mmm, I hope you’re right."),
        } }, 
        {"Level3-2", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "Dad, can we talk?"),
            new TextMessage(MessageSender.ANNA, "Could you talk to me, even if it’s the last time?"),
            new TextMessage(MessageSender.DAD, "It won’t be the last time, you're still my daughter no matter what."),
            new TextMessage(MessageSender.ANNA, "Thanks."),
            new TextMessage(MessageSender.ANNA, "I just want to let you know that I’m lost as well."),
            new TextMessage(MessageSender.ANNA, "This experience is new for me as it is for you."),
        } },        
        {"Level3-3", new TextMessage[] {
            new TextMessage(MessageSender.DAD, "What I’m thinking is that you have gone through a rough period of your life and now that you found someone you can connect to, you are misinterpreting your emotions."),
            new TextMessage(MessageSender.ANNA, "Dad... I don’t think that’s right."),
            new TextMessage(MessageSender.DAD, "You might be going into university soon but you're still a teenager. Have you ever felt this type of emotion for another girl?"),
            new TextMessage(MessageSender.ANNA, "No"),
            new TextMessage(MessageSender.DAD, "Anna. She’s just a friend. You will find a man right for you later down the line."),
        } },
        {"Level3-4", new TextMessage[] {
            new TextMessage(MessageSender.ANNA, "That’s not it, that's not how I feel"),
            new TextMessage(MessageSender.DAD, "Anna, I know it’s hard to accept but just hear me out"),
            new TextMessage(MessageSender.ANNA, "No, I know this is real. I don’t want to leave this relationship behind and end up regretting it later"),
            new TextMessage(MessageSender.ANNA, "She is one, she is the person you said I would meet."),
            new TextMessage(MessageSender.DAD, "Come on, think this through some more"),
            new TextMessage(MessageSender.ANNA, "Like you? Didn’t you regret what happened between you and mum? Didn’t you wish you could patch things up with her before she died?"),
        } },
        {"Level3-5", new TextMessage[] {
            new TextMessage(MessageSender.DAD, "That’s different, relationships get more complicated when your older"),
            new TextMessage(MessageSender.ANNA, "What happens? Does everyone just give up on themselves?"),
            new TextMessage(MessageSender.ANNA, "This is my way of staying true to myself, this is what makes me happy"),
            new TextMessage(MessageSender.DAD, "Sweetheart, I love you and if that’s how you really feel then I won’t stop you"),
            new TextMessage(MessageSender.ANNA, "Hearing that means the world."),
        } },
        {"Level3-6", new TextMessage[] {
            new TextMessage(MessageSender.DAD, "I’m not sure how I can help, but we’ll get through this."),
            new TextMessage(MessageSender.ANNA, "Dad, I love you so much. I don’t know if this is the right time but do you want to meet her this weekend? She will be around town."),
            new TextMessage(MessageSender.ANNA, "Dad?"),
            new TextMessage(MessageSender.ANNA, "Are you listening?"),
            new TextMessage(MessageSender.ANNA, "Dad?"),
        } },
    };

    //public List<TextMessage> AllSentMessages = new List<TextMessage>();
    //public Action<TextMessage> OnNewTextMessage;
    //public Action OnLightFlash;
    //public Action<bool> OnPhoneShow;
    public bool NewMessage { private set; get; } = false;

    public TextMessageManager()
    {
        //SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "Yo. If you want to move tiles around, click and drag them in the direction you want to move them." });
        //SendNewMessage(new TextMessage() { Sender = MessageSender.ANNA, MessageText = "Thank you." });
        //SendNewMessage(new TextMessage() { Sender = MessageSender.DAD, MessageText = "By the way, red buildings can't be moved." });
    }
}
