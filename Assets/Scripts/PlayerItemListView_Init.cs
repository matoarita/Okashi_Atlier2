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

    private bool GameStart_Init_flag;

    // Use this for initialization
    void Start () {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        GameStart_Init_flag = false;
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
        playeritemlist_onoff.transform.localPosition = new Vector3(-220,80, 0);
        playeritemlist_onoff.name = "PlayeritemList_ScrollView";

        if (GameStart_Init_flag != true)
        {
            //初期アイテムの設定
            pitem_init();

            GameStart_Init_flag = true; //ゲーム開始最初だけ、初期アイテムを追加する。
           
        }

    }

    public void RecipiList_ScrollView_Init()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        recipilist_scrollview_init = (GameObject)Resources.Load("Prefabs/RecipiList_ScrollView");
        recipilist_onoff = Instantiate(recipilist_scrollview_init, canvas.transform);

        recipilist_onoff.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        recipilist_onoff.transform.localPosition = new Vector3(0, 70, 0);
        recipilist_onoff.name = "RecipiList_ScrollView";
    }

    void pitem_init()
    {
        //Debug.Log("プレイヤーステータス　アイテム初期化　実行");
        //初期に所持するアイテム
        //基本アイテムのみ追加。
        for (i = 0; i <= database.sheet_topendID[1]; i++)
        {
            /*if( database.items[i].itemName == "egg")
            {
                pitemlist.addPlayerItem(i, 5);
            }*/
            
            if (database.items[i].itemName == "komugiko")
            {
                pitemlist.addPlayerItem(i, 10);
            }
            if (database.items[i].itemName == "butter")
            {
                pitemlist.addPlayerItem(i, 5);
            }
            if (database.items[i].itemName == "suger")
            {
                pitemlist.addPlayerItem(i, 5);
            }
            if (database.items[i].itemName == "orange")
            {
                pitemlist.addPlayerItem(i, 5);
            }
            if (database.items[i].itemName == "grape")
            {
                pitemlist.addPlayerItem(i, 2);
            }
        }

    }
}
