using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yes : MonoBehaviour {

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    // Use this for initialization
    void Start () {

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick_Yes() //Yesが選択された時
    { // 必ず public にする
        //Debug.Log("clicked");
        selectitem_kettei.onclick = true;

        selectitem_kettei.kettei1 = true;
    }

}
