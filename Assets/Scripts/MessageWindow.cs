using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageWindow : MonoBehaviour {

    public bool window_clickon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void WindowClickDown()
    {
        window_clickon = true;
    }

    public void WindowClickUp()
    {
        window_clickon = false;
    }

    public void Window_Exit()
    {
        window_clickon = false;
    }
}
