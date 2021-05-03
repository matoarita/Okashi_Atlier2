using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_OFF : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(GameMgr.DEBUG_MODE)
        {
            On_Myself();
        }
        else
        {
            Off_Myself();
        }

	}

    public void Off_Myself()
    {
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.GetComponent<Button>().interactable = false;
    }

    public void On_Myself()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        this.GetComponent<Button>().interactable = true;
    }
}
