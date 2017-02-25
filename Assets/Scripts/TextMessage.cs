using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextMessage : MonoBehaviour {
    public Text myText;
    public Image TextBackground;
    public MessageProgress state = MessageProgress.notSent;
    public Color[] ColorStates = new Color[] { Color.blue, Color.blue, Color.blue};
    public Image OutlineImage;
    public float AnimationTime = 0.5f;
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
                StartCoroutine(TextSentAnimation());
                break;
            case (MessageProgress.Seen):
                StartCoroutine(TextRecievedAnimation());
                break;
            default:
                break;
        }
    }

    IEnumerator TextSentAnimation()
    {
        float timer = 0;
        Color startingColor = TextBackground.color;
      //  Color outlineStartingColor = OutlineImage.color;
        while (timer < AnimationTime)
        {
            TextBackground.color = Color.Lerp(startingColor, ColorStates[1], timer / AnimationTime);
      //      OutlineImage.fillAmount = timer / AnimationTime;
      //      OutlineImage.color = Color.Lerp(outlineStartingColor, ColorStates[2], timer / AnimationTime);
            yield return null;
            timer += Time.smoothDeltaTime;
        }
        
    }

    IEnumerator TextRecievedAnimation()
    {
        float timer = 0;
        Color startingColor = TextBackground.color;
        //  Color outlineStartingColor = OutlineImage.color;
        while (timer < AnimationTime)
        {
            TextBackground.color = Color.Lerp(startingColor, ColorStates[2], timer / AnimationTime);
            //      OutlineImage.fillAmount = timer / AnimationTime;
            //      OutlineImage.color = Color.Lerp(outlineStartingColor, ColorStates[2], timer / AnimationTime);
            yield return null;
            timer += Time.smoothDeltaTime;
        }
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
