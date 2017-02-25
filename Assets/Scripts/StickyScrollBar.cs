using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class StickyScrollBar : MonoBehaviour {
    bool ScrollBarStuck = true;
    float LastContentSize = 0;
    ScrollRect rect;
	// Use this for initialization
	void Start () {
        rect = GetComponent<ScrollRect>();

	}

    public void UpdateValue()
    {
        if(LastContentSize == rect.content.rect.height)
        {
            ScrollBarStuck = rect.verticalScrollbar.value <= 0;
        }
        else
        {
            LastContentSize = rect.content.rect.height;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(ScrollBarStuck)
        {
            rect.verticalNormalizedPosition = 0;
            rect.verticalScrollbar.value = 0;
        }
		
	}
    
}
