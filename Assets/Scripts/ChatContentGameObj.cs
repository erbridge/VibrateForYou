using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatContentGameObj : MonoBehaviour {
    ChatContent Owner;
    [HideInInspector]
    public Rect myTransform;
    //Chat prefab must have TextMessage.cs on it
    public GameObject ChatPrefab;
    public GameObject NPCChatPrefab;
    public GameObject TypingPrefab;
  //  public GameObject Spacer;
    // Use this for initialization
    void Start () {
        Owner =  new ChatContent();
        Owner.myGameObj = this;
       // myTransform = GetComponent<Rect>();
	}

    public GameObject GetPrefabByOwner(MessageSender sender)
    {
        switch (sender)
        {
            case (MessageSender.Player):
                return ChatPrefab;
            case (MessageSender.NPC):
                return NPCChatPrefab;
            default:
                return null;
        }

    }

    // Update is called once per frame
    void Update () {
        /*
        if (Input.GetKeyDown(KeyCode.A))//Test Code
        {
            Owner.SpawnMessage("This is a test message hopefully this goes on for two lines and I can see if that part works! e if that part works!e if that part works!", MessageSender.Player);
        }
        if (Input.GetKeyDown(KeyCode.S))//Test Code
        {
            Owner.SpawnMessage("The Shorter line of text", MessageSender.NPC);
        }
        if (Input.GetKeyDown(KeyCode.D))//Test Code
        {
            Owner.SpawnMessage("An alternate line of text that supposed to be two lines", MessageSender.Player);
        }
        if (Input.GetKeyDown(KeyCode.Z))//Test Code
        {
            Owner.MessagesSent();
        }
        if (Input.GetKeyDown(KeyCode.X))//Test Code
        {
            Owner.MessagesSeen();
        }*/
    }

    public void Prnt(string input)
    {
        print(input);
    }
}
