using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem_kettei : MonoBehaviour {

    public bool kettei1;
    public bool onclick;

	// Use this for initialization
	void Start ()
    {
        kettei1 = false;
        onclick = false;
        this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick() //Yesが選択された時
    { // 必ず public にする
        //Debug.Log("clicked");
        onclick = true; //押された～というフラグ

        kettei1 = true;
    }

    public void OnClick2() //Noが選択された時
    { // 必ず public にする
        //Debug.Log("clicked");
        onclick = true;

        kettei1 = false;
    }
}
