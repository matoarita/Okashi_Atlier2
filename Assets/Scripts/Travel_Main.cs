using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Travel_Main : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject canvas;

    // Use this for initialization
    void Start () {

        //Debug.Log("Travel_Start scene loaded");

        //キャンバスの取得
        canvas = GameObject.FindWithTag("Canvas");

        //所持アイテム画面を開く。初期設定で最初はOFF。
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();
        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        playeritemlist_onoff.SetActive(false);

        //初期メッセージ
        _text.text = "さて.. 行けるとこは、と。";

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
