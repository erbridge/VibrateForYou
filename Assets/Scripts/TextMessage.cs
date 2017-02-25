using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextMessage : MonoBehaviour {
    public Text myText;
    public Image TextBackground;
    public MessageProgress state = MessageProgress.notSent;
  //  public Color[] ColorStates = new Color[] { Color.blue, Color.blue, Color.blue};
    //public float AnimationTime = 0.5f;
    public Image ReadBar;
    public Image SentCircle;
	// Use this for initializatio.n
	void Start () {
		
	}
    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateMessageState(MessageProgress newState)
    {
        state = newState;
        switch(newState)
        {
            case (MessageProgress.Sent):
                StartSentAnimation();
                break;
            case (MessageProgress.Seen):
                StartRecievedAnimation();
                break;
            default:
                break;
        }
    }

    void StartSentAnimation()
    {
        SentCircle.GetComponent<Animator>().SetTrigger("Delivered");
    }

    void StartRecievedAnimation()
    {
        StartCoroutine(ReadBar.GetComponent<ReadBarAnimator>().ReadAnimation());
    }

    public void UpdateText(string newText)
    {
        if(myText == null)
        {
            return;
        }
        myText.text = newText;
    }
}
