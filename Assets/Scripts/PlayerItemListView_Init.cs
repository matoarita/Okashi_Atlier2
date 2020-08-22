using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プレイヤーアイテムリストスクロールビュー描画するときの初期設定。位置などもここで決める。

public class PlayerItemListView_Init : SingletonMonoBehaviour<PlayerItemListView_Init>
{
    private ItemDataBase database;

    private ItemCompoundDataBase databaseCompo;

    private PlayerItemList pitemlist;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;
    private GameObject recipilist_scrollview_init;

    private GameObject canvas;

    private int i;

    // Use this for initialization
    void Start () {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();
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

        playeritemlist_onoff.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
        playeritemlist_onoff.transform.localPosition = new Vector3(-240,80, 0);
        playeritemlist_onoff.name = "PlayeritemList_ScrollView";

    }

    public void RecipiList_ScrollView_Init()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        recipilist_scrollview_init = (GameObject)Resources.Load("Prefabs/RecipiList_ScrollView");
        recipilist_onoff = Instantiate(recipilist_scrollview_init, canvas.transform);

        recipilist_onoff.transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);
        recipilist_onoff.transform.localPosition = new Vector3(0, 70, 0);
        recipilist_onoff.name = "RecipiList_ScrollView";
    }

}
