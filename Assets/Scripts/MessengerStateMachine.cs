using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum MessengerStates { ViewMode, DialogChoices, WritingText, BackSpace};
public class MessengerStateMachine : MonoBehaviour {
    public Button ExitButton;
    public Button SendButton;
    public delegate void StateFunction();
    StateFunction OnEnter;
    StateFunction OnUpdate;
    StateFunction OnExit = null;
    public static MessengerStateMachine instance;
    MessengerStates currentState = MessengerStates.ViewMode;
    MessengerStates previousState = MessengerStates.ViewMode;

    int selectedOption;
    KeyValuePair<string, string>[] currentOptions = null;
   
    // Use this for initialization
    void Start() {
        instance = this;
        FindObjectOfType<Parser.Parser>().eventOnNewStatement += NewMessage;
        FindObjectOfType<Parser.Parser>().eventOnChangeChoices += SetOptions;
        StartCoroutine(WaitForFixed());
    }

    IEnumerator WaitForFixed()
    {
        yield return new WaitForFixedUpdate();
        FindObjectOfType<Parser.Parser>().Init("Jaimie", "you've matched!");
    }

    void ChangeState(MessengerStates state)
    {
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
       /* if (currentState == MessengerStates.WritingText)
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
        }*/
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
    public void ChatBarClicked()
    {
        if(currentState == MessengerStates.ViewMode)
        {
            ChangeState(MessengerStates.DialogChoices);
        }
    }
    public void ExitChatButtonClicked()
    {
        if(currentState == MessengerStates.WritingText)
        {
            ChangeState(MessengerStates.DialogChoices);
        }
        else if(currentState == MessengerStates.DialogChoices)
        {
            ChangeState(MessengerStates.ViewMode);
        }
    }
    public void OptionPressed(int optionNumber)
    {
        if(currentState == MessengerStates.DialogChoices)
        {
            selectedOption = optionNumber;
            ChatFillin.inst.PopulateText(currentOptions[selectedOption].Key);
            ChangeState(MessengerStates.WritingText);
            
        }
    }
    public void SendButtonPressed()
    {
        if(currentState == MessengerStates.WritingText )
        {
            ChatContent.inst.SpawnMessage(currentOptions[selectedOption].Key, MessageSender.Player);
            FindObjectOfType<Parser.Parser>().SetPage(currentOptions[selectedOption].Value);
            //Send Message
            ChangeState(MessengerStates.ViewMode);
        }
    }
    //View Mode States
    void EnterStateViewMode()
    {
       
    }
    void UpdateStateViewMode()
    {

    }
    void ExitStateViewMode()
    {
      
    }
    //DialogChoices
    void EnterDialogChoices()
    {
        FindObjectOfType<ExpandKeyboard>().OnMoveKeyboard(true);
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
        FindObjectOfType<ExpandKeyboard>().OnMoveKeyboard(false);
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

	// Update is called once per frame
	void Update () {
        if(OnUpdate != null)
        {
            OnUpdate();
        }
        
	}

    
}
