using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ChatFillin : MonoBehaviour {
    public static ChatFillin inst;
    string StartingText;
    Text myText;
    public float TimeToWait = 0.005f;
    int currentChar = 0;
    public bool isFinished = false;
    IEnumerator typingAnimation;
	// Use this for initialization
	void Start () {
        myText = GetComponent<Text>();
        inst = this;
        StartingText = myText.text;
	}
	
    public void ResetText()
    {
        myText.text = StartingText;
    }

    public void PopulateText(string input)
    {
        currentChar = 0;
        isFinished = false;
        typingAnimation = typeText(input);
        StartCoroutine(typingAnimation);
    }

    public void skipToEnd()
    {
        currentChar = int.MaxValue;
    }

    IEnumerator typeText(string input)
    {
        while(currentChar < input.Length)
        {
            ++currentChar;
            myText.text = input.Substring(0, currentChar);
            yield return new WaitForSeconds(TimeToWait);
        }
        myText.text = input;
        CleanUp();
    }

    void CleanUp()
    {
        isFinished = true;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
