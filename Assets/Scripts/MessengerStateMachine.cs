using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum MessengerStates { ViewMode, DialogChoices, WritingText, BackSpace, TitleScreen};
public class MessengerStateMachine : MonoBehaviour {
    public GameObject TitleScreen;
    public Button ExitButton;
    public Button SendButton;
    public delegate void StateFunction();
    StateFunction OnEnter;
    StateFunction OnUpdate;
    StateFunction OnExit = null;
    public static MessengerStateMachine instance;
    MessengerStates currentState = MessengerStates.TitleScreen;
    MessengerStates previousState = MessengerStates.TitleScreen;
    float FadeTime = 0.25f;


    int selectedOption;
    KeyValuePair<string, string>[] currentOptions = null;
   
    // Use this for initialization
    void Start() {
        instance = this;
        OnExit = ExitTitleScreen;
       // ChangeState(MessengerStates.TitleScreen);
        FindObjectOfType<Parser.Parser>().eventOnNewStatement += NewMessage;
        FindObjectOfType<Parser.Parser>().eventOnChangeChoices += SetOptions;
    }

    void ChangeState(MessengerStates state)
    {
        print("Change state: " + state);
        if(state == currentState)
        {
            return;
        }
          //print("Changing State");
        if (OnExit != null)
        {
            OnExit();
        }

        switch (state)
        {
            case (MessengerStates.ViewMode):
                OnEnter = EnterStateViewMode;
                OnUpdate = UpdateStateViewMode;
                OnExit = ExitStateViewMode;
                break;
            case (MessengerStates.DialogChoices):
                OnEnter = EnterDialogChoices;
                OnUpdate = UpdateDialogChoices;
                OnExit = ExitDialogChoices;
                break;
            case (MessengerStates.WritingText):
                OnEnter = EnterWritingText;
                OnUpdate = UpdateWritingText;
                OnExit = ExitWritingText;
                break;
            case (MessengerStates.TitleScreen):
                OnEnter = EnterTitleScreen;
                OnUpdate = UpdateTitleScreen;
                OnExit = ExitTitleScreen;
                break;
        }
        OnEnter();
        currentState = state;
    }

    void SetOptions(List<KeyValuePair<string, string>> input)
    {
        NewPage(input.ToArray());
    }
    public void NewPage(KeyValuePair<string, string>[] options)
    {
        if (currentState == MessengerStates.WritingText)
        {
            bool foundMatch = false;
            for(int i = 0; i < options.Length && i < currentOptions.Length; ++i)
            {
                if(options[selectedOption].Key == currentOptions[i].Key)
                {
                    foundMatch = true;
                    selectedOption = i;
                }
            }
            if(!foundMatch)
            {
                //Back Space up through text
                currentOptions = options;
                ChangeState(MessengerStates.DialogChoices);
                return;
            }
        }
        if (currentState == MessengerStates.DialogChoices)
        {
            FindObjectOfType<PopulateOptions>().Populate(options);
        }
        currentOptions = options;
    }
    public void NewMessage(string input)
    {
        ChatContent.inst.SpawnMessage(input, MessageSender.NPC);
    }
    //Interface functions
    public void TitleScreenClicked()
    {
        StartCoroutine(TitleScreenFadeOut());
    }

    IEnumerator TitleScreenFadeOut()
    {
        float timer = 0;
        CanvasGroup group = TitleScreen.GetComponent<CanvasGroup>();
        TitleScreen.GetComponent<Button>().interactable = false;
        while (timer < FadeTime)
        {
            timer += Time.deltaTime;
            group.alpha = 1 - (timer / FadeTime);
            yield return null;
        }
        ChangeState(MessengerStates.ViewMode);
    }
    IEnumerator TitleScreenFadeIn()
    {
        TitleScreen.SetActive(true);
        float timer = 0;
        CanvasGroup group = TitleScreen.GetComponent<CanvasGroup>();
        while (timer < FadeTime)
        {
            yield return null;
            timer += Time.deltaTime;
            group.alpha = timer / FadeTime;
        }

        ChangeState(MessengerStates.TitleScreen);
    }

    public void ResetButtonClicked()
    {
        print("Reset Clicked");
        StartCoroutine(TitleScreenFadeIn());
    }
    public void ChatBarClicked()
    {
        if(currentState == MessengerStates.ViewMode)
        {
           
            ChangeState(MessengerStates.DialogChoices);
        }
    }
    public void ExitChatButtonClicked()
    {
        print("Exit button click");
        if(currentState == MessengerStates.WritingText)
        {
            ChangeState(MessengerStates.ViewMode);
        }
        else if(currentState == MessengerStates.DialogChoices)
        {
            ChangeState(MessengerStates.ViewMode);
        }
    }
    public void OptionPressed(int optionNumber)
    {
        if(currentState == MessengerStates.DialogChoices || currentState == MessengerStates.WritingText)
        {
            selectedOption = optionNumber;
            ChatFillin.inst.PopulateText(currentOptions[selectedOption].Key);
            ChangeState(MessengerStates.WritingText);
            
            // var clickSound = Resources.Load("SFX/Keyboard_Click_0"+UnityEngine.Random.Range(0, 4)) as AudioClip;
            // SFX.PlayAt(clickSound, Camera.main.transform.position, 1f);
        }
    }
    public void SendButtonPressed()
    {
        if(currentState == MessengerStates.WritingText )
        {
            ChatContent.inst.SpawnMessage(currentOptions[selectedOption].Key, MessageSender.Player);
            FindObjectOfType<Parser.Parser>().SetPage(currentOptions[selectedOption].Value);
            currentOptions = null;
            //Send Message
            ChangeState(MessengerStates.DialogChoices);
            var sound = Resources.Load("SFX/Message_Send") as AudioClip;
            SFX.PlayAt(sound, Camera.main.transform.position, 1f);
        }
    }
    //View Mode States
    void EnterStateViewMode()
    {
        print(FindObjectOfType<ExpandKeyboard>().isUp); ;
        if(FindObjectOfType<ExpandKeyboard>().isUp)
        {
            FindObjectOfType<ExpandKeyboard>().OnMoveKeyboard(false);
        }
    }
    void UpdateStateViewMode()
    {

    }
    void ExitStateViewMode()
    {
        if (!FindObjectOfType<ExpandKeyboard>().isUp)
        {
            FindObjectOfType<ExpandKeyboard>().OnMoveKeyboard(true);
        }
    }
    //DialogChoices
    void EnterDialogChoices()
    {
        
        ExitButton.interactable = true;
        //Ask for dialog choices
        //Populate dialog choices
        PopulateOptions.instance.Populate(currentOptions);
    }
    void UpdateDialogChoices()
    {
        //Check for Update for Choices
    }
    void ExitDialogChoices()
    {
        
        ExitButton.interactable = false;
        //Clear dialog choices
    }
    //WritingText
    void EnterWritingText()
    {
        //Activate Exit chat button
        ExitButton.interactable = true;
        SendButton.interactable = true;
    }
    void UpdateWritingText()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Jump to the end of writing text
            ChatFillin.inst.skipToEnd();
        }

        //At end of writing Text, activate send message button
        if (ChatFillin.inst.isFinished)
        {
            SendButton.interactable = true;
        }
    }
    void ExitWritingText()
    {
        //deactivate Exit chat button
        ExitButton.interactable = false;
        SendButton.interactable = false;
        ChatFillin.inst.ResetText();
    }
    //Title Screen
    void EnterTitleScreen()
    {
        ChatContent.inst.ResetChatLog();
        TitleScreen.GetComponent<Button>().interactable = true;
    }
    void UpdateTitleScreen()
    {

    }
    void ExitTitleScreen()
    {
        FindObjectOfType<Parser.Parser>().Init("Main", "you've matched!");
        TitleScreen.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
        if(OnUpdate != null)
        {
            OnUpdate();
        }
        
	}

    
}
