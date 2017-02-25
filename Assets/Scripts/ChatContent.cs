using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MessageProgress {notSent, Sent, Seen};
public enum MessageSender {Player, NPC};

[System.Serializable]
public struct SentMessage
{
    public string message;
    public float currentTime;
    public MessageProgress state;
    public TextMessage Reference;
    public MessageSender sender;
}

[System.Serializable]
public class ChatContent : ScriptableObject
{
    [System.NonSerialized]
    public static ChatContent inst;
    public List<SentMessage> ChatLog;
    //Different Offsets for the owner & the other person
    public float OwnerXOffset = 190.3f;
    public float PartnerXOffset = 162.4f;
    //------
    public float currentHeight = 20;
    public float padding = 10;
    public static int LineSizeEstimate = 36;
   // public static float CharWidthEstimate 
    public static float TextLineHeight = 100f;
    [System.NonSerialized]
    public ChatContentGameObj myGameObj;
    // Use this for initialization
    void Awake()
    {
        inst = this;
        ChatLog = new List<SentMessage>();
        FindObjectOfType<Parser.Parser>().eventOnReceived += MessagesSent;
        FindObjectOfType<Parser.Parser>().eventOnRead += MessagesSeen;
        FindObjectOfType<Parser.Parser>().eventOnTyping += TypeForSeconds;
       // myGameObj.Prnt("Chat content is awake");
    }

    public void SpawnTempMessage(string Input, GameObject MessagePrefab)
    {

    }

    public void SpawnMessage (string Input, MessageSender sender)
    {
        //Estimate the chat window size
        int numberOfCharacters = Input.ToCharArray().Length;
        int numberOfLines;
        if(numberOfCharacters % LineSizeEstimate == 0)
        {
            numberOfLines = numberOfCharacters / LineSizeEstimate;
        }
        else
        {
            numberOfLines = (numberOfCharacters / LineSizeEstimate) + 1;
        }

        //Put the new text such that it doesn't collide with any existing text
        GameObject chatPrefab = myGameObj.GetPrefabByOwner(sender);
        GameObject newChatMess = Instantiate(chatPrefab, myGameObj.transform) as GameObject;//Change this later
        newChatMess.transform.localScale = new Vector3(1,1,1);
        //Set values new chat
        Vector2 newDelta = newChatMess.GetComponent<RectTransform>().sizeDelta;
        newChatMess.GetComponent<RectTransform>().sizeDelta = new Vector2(newDelta.x, (TextLineHeight * numberOfLines) + 30);
        //Assume that the prefab has this component
        newChatMess.GetComponent<TextMessage>().UpdateText(Input);
        //Make the content window taller (If needed)
        currentHeight += (TextLineHeight * numberOfLines) + 40;
        Vector2 windowDelta = myGameObj.GetComponent<RectTransform>().sizeDelta;
        windowDelta.y = currentHeight;
        //myGameObj.GetComponent<RectTransform>().sizeDelta = windowDelta;
        //Add to the list
        SentMessage newListPart;
        newListPart.message = Input;
        newListPart.state = newChatMess.GetComponent<TextMessage>().state;
        newListPart.Reference = newChatMess.GetComponent<TextMessage>();
        newListPart.currentTime = Time.realtimeSinceStartup;
        newListPart.sender = sender;
        //Resize Chatlog to account for new stuff
        ChatLog.Add(newListPart);

        var sound = Resources.Load("SFX/Vibrate") as AudioClip;
        SFX.PlayAt(sound, Camera.main.transform.position, 0.3f);
        sound = Resources.Load("SFX/Message_Recieve") as AudioClip;
        SFX.PlayAt(sound, Camera.main.transform.position, 1f);
    }

    public void MessagesSeen()
    {
        //Find first message that's been sent & not seen
        myGameObj.StartCoroutine(SeeNewMessages());
    }

    IEnumerator SeeNewMessages()
    {
        foreach (SentMessage m in ChatLog)
        {
            if (m.Reference.state == MessageProgress.Sent)
            {
                m.Reference.UpdateMessageState(MessageProgress.Seen);
                //m.UpdateState(MessageProgress.Sent);
                //myGameObj.Prnt("" + m.state);
                yield return null;
            }
        }
    }

    public void MessagesSent()
    {
        myGameObj.StartCoroutine(SendMessage());
    }

    IEnumerator SendMessage()
    {
        foreach(SentMessage m in ChatLog)
        {
            if (m.Reference.state == MessageProgress.notSent)
            {
                m.Reference.UpdateMessageState(MessageProgress.Sent);
                //m.UpdateState(MessageProgress.Sent);
                //myGameObj.Prnt("" + m.state);
                yield return null;
            }
        }
        
    }
}
