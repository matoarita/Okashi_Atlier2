using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Emo_Hukidashi : MonoBehaviour {

    private Touch_Controll2 touch_controll2;

	// Use this for initialization
	void Start () {

        touch_controll2 = GameObject.FindWithTag("Character").transform.Find("TouchFace").GetComponent<Touch_Controll2>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Touch_OnHukidashi()
    {
        Debug.Log("吹き出し　タッチ");
        touch_controll2.OnTouchFace();
    }
}
