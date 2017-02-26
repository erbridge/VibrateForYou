using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateOptions : MonoBehaviour {
    public static PopulateOptions instance = null;
    public GameObject[] Options;
    public Text[] TextFields;
	// Use this for initialization
	void Start () {
        instance = this;
	}
	public void Populate(KeyValuePair<string,string>[] text)
    {
        int i;
      /*print(text.Length);
        for(i = 0; i < text.Length; ++i)
        {
            print(text[i].Key + " " + text[i].Value);
        }*/
        for(i = 0; text != null && i < text.Length; ++i)
        {
            TextFields[i].GetComponent<Text>().text = text[i].Key;
            Options[i].GetComponent<Button>().interactable = true;
            // Options[i].GetComponent<LayoutElement>().ignoreLayout = false;
        }
        for (; i < Options.Length; ++i)
        {
            Options[i].GetComponent<Button>().interactable = false;
            // Options[i].GetComponent<LayoutElement>().ignoreLayout = true;
        }

    }
	// Update is called once per frame
	void Update () {
		
	}
}
