using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_ImageOnOff : MonoBehaviour {

    private Button myButton;
    private Image myImage;
    private Text myText;
    
	// Use this for initialization
	void Start () {

        myButton = GetComponent<Button>();
        myImage = this.transform.Find("Image").gameObject.GetComponent<Image>();
        myText = this.transform.Find("Text").gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if( myButton.IsInteractable() == true )
        {
            myImage.color = new Color(1, 1, 1, 1);
            //myText.color = new Color(1, 1, 1, 1);
        }
        else
        {
            myImage.color = new Color(1, 1, 1, 0.5f);
            //myText.color = new Color(1, 1, 1, 0.5f);
        }

    }

}
