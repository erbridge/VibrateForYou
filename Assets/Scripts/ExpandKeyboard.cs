using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExpandKeyboard : Selectable
{
    public RectTransform Viewport;
    public Image Keyboard;
    bool PointerIsOver = false;
    bool Clicked = false;
    bool isAnimating = false;
    public bool isUp = false;
    float MaxKeyboardSize = 200;
    float timer = 0;
    float AnimationTime = 0.25f;
    int Direction = 1;
    IEnumerator KeyboardAnimation = null;

    // override public void OnPointerEnter(PointerEventData data)
    // {
    //     PointerIsOver = true;
    // }

    // override public void OnPointerExit(PointerEventData data)
    // {
    //     PointerIsOver = false;
    // }

    void setViewPortHeight(float height)
    {
        Viewport.offsetMin = new Vector2(Viewport.offsetMin.x, height);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        MessengerStateMachine.instance.ChatBarClicked();
    }
    public void OnMoveKeyboard(bool Expand)
    {
        isUp = Expand;
        //print("Keyboard moved up: " + isUp);
        if (Expand)
        {
            
            Keyboard.GetComponent<Animator>().SetTrigger("BringUpKeyboard");
            setViewPortHeight(900);
        /*
            Direction = 1;
            if (!isAnimating)
            {
                isAnimating = true;
                KeyboardAnimation = ChangeChatWindow();
                timer = 0;
                StartCoroutine(KeyboardAnimation);
            }*/
        }
        else
        {
            Keyboard.GetComponent<Animator>().SetTrigger("CollapseKeyboard");
            setViewPortHeight(0);
        /*
            Direction = -1;
            if (!isAnimating)
            {
                isAnimating = true;
                KeyboardAnimation = ChangeChatWindow();
                timer = AnimationTime;
                StartCoroutine(KeyboardAnimation);
            }*/
        }
    }

    void CleanUp()
    {
        KeyboardAnimation = null;
        isAnimating = false;
    }

    IEnumerator ChangeChatWindow()
    {
        do{
            timer += Time.smoothDeltaTime * Direction;
            timer = Mathf.Clamp(timer, 0, AnimationTime);
            Keyboard.GetComponent<LayoutElement>().preferredHeight = MaxKeyboardSize * (timer / AnimationTime);
            yield return null;
        } while (timer > 0 && timer < AnimationTime) ;
        CleanUp();

    }

    public void Update()
    {
      //  print("Update is working");
    }
}
