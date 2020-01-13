using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プレイヤーアイテムリストスクロールビュー描画するときの初期設定。位置などもここで決める。

public class PlayerItemListView_Init : SingletonMonoBehaviour<PlayerItemListView_Init>
{

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init;

    private GameObject canvas;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerItemList_ScrollView_Init()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        pitemlist_scrollview_init = (GameObject)Resources.Load("Prefabs/PlayeritemList_ScrollView");
        playeritemlist_onoff = Instantiate(pitemlist_scrollview_init, canvas.transform);
        playeritemlist_onoff.transform.localScale = new Vector3(0.85f, 0.85f, 1.0f);
        playeritemlist_onoff.transform.localPosition = new Vector3(-220,90, 0);
        playeritemlist_onoff.name = "PlayeritemList_ScrollView";
    }
}
