using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Travel_Main : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    // Use this for initialization
    void Start () {

        Debug.Log("Travel_Start scene loaded");

        //所持アイテム画面を開く。初期設定で最初はOFF。
        //playeritemlist_onoff = GameObject.FindWithTag("PlayeritemList_ScrollView");
        //pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        //playeritemlist_onoff.SetActive(false);

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //初期メッセージ
        _text.text = "さて.. 行けるとこは、と。";

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
