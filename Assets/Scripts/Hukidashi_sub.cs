using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hukidashi_sub : MonoBehaviour {

    private Text _text;
    private int hukidashi_status;

    private float TimeOut;

	// Use this for initialization
	void Start () {

        _text = this.transform.Find("hukidashi_Text").GetComponent<Text>();

        TimeOut = 0.1f;

        hukidashi_status = 0;

    }
	
	// Update is called once per frame
	void Update () {

        TimeOut -= Time.deltaTime;

        if(TimeOut <= 0.0f)
        {
            TimeOut = 0.1f;

            switch(hukidashi_status)
            {
                case 0:

                    hukidashi_status = 1;
                    _text.text = ".";
                    break;

                case 1:

                    hukidashi_status = 2;
                    _text.text = ". .";
                    break;

                case 2:

                    hukidashi_status = 0;
                    _text.text = ". . .";
                    break;
            }
        }
    }
}
