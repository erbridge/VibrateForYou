using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadBarAnimator : MonoBehaviour
{
	public float ElapsedTime = 1f;
	public AnimationCurve TweenCurve;
    public RectTransform TextTransform;

    // Use this for initialization
    void Start()
    {
		
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }

    public IEnumerator ReadAnimation()
    {
    	float t = 0f;
        var myRect = GetComponent<RectTransform>();
        float widthTo = TextTransform.rect.width + myRect.anchoredPosition.x;
        Debug.Log(widthTo);

    	while (t < ElapsedTime)
    	{
    		float s = TweenCurve.Evaluate(t/ElapsedTime);
            var r = myRect.sizeDelta;
            r.x = Mathf.Lerp(0f, widthTo, s);
            myRect.sizeDelta = r;
            yield return null;
            t += Time.deltaTime;
    	}
    }
}
