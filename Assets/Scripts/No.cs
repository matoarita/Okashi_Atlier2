using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class No : MonoBehaviour {

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト
    private SoundController sc;

    // Use this for initialization
    void Start () {

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.C))
        {
            //Debug.Log("今、右クリックをした");
            selectitem_kettei.onclick = true;

            selectitem_kettei.kettei1 = false;
            selectitem_kettei.kettei3 = false;

            sc.PlaySe(18);
        }
    }

    public void OnClick_No() //Noが選択された時
    { // 必ず public にする
        //Debug.Log("clicked");
        selectitem_kettei.onclick = true;

        selectitem_kettei.kettei1 = false;
        selectitem_kettei.kettei3 = false;
    }

    public void OnClick_No2() //Noが選択された時 納品パネル
    { // 必ず public にする
        //Debug.Log("clicked");
        selectitem_kettei.NouhinOnClick2();
    }
}
