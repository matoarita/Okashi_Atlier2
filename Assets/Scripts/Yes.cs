﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Yes : MonoBehaviour {

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト
    private SoundController sc;
    private keyManager keymanager;

    // Use this for initialization
    void Start () {

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //キー入力受付コントローラーの取得
        keymanager = keyManager.Instance.GetComponent<keyManager>();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        { 

            //Debug.Log("Enter");
            selectitem_kettei.onclick = true;

            selectitem_kettei.kettei1 = true;
            sc.PlaySe(46);
        }
    }

    public void OnClick_Yes() //Yesが選択された時
    { // 必ず public にする
        //Debug.Log("clicked");
        selectitem_kettei.onclick = true;

        selectitem_kettei.kettei1 = true;
    }

    public void OnClick_Yes2() //Yesが選択された時 納品パネル
    { // 必ず public にする
        //Debug.Log("clicked");
        selectitem_kettei.NouhinOnClick();
    }

}
