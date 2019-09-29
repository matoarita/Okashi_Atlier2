﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddPlayerItemButton : MonoBehaviour {

    //private GameObject Itemdatabase_object; //ItemDataBaseゲームオブジェクトを取得
    private ItemDataBase database;

    //private GameObject playeritemlist_obj;
    private PlayerItemList pitemlist;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private int i, rand, _randID;
    private int count, j;

    // Use this for initialization
    void Start()
    {

        //アイテムデータベースの取得
        //Itemdatabase_object = GameObject.FindWithTag("ItemDataBase");
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //プレイヤー所持アイテムリストの取得
        //playeritemlist_obj = GameObject.FindWithTag("PlayerItemList");
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickAddSkillButton()
    {
        Allitem_Add();

        //Debug.Log(pitemlist.playeritemlist.Count);
        //Debug.Log(pitemlistController.itemSelected.Count);
    }

    void Allitem_Add()
    {
        /*rand = Random.Range(0, database.items[database.sheet_topendID[1]].itemID);
            _randID = database.items[rand].itemID;*/

        //デバッグ用　すべてのアイテムを追加する。
        for (i = 0; i <= database.sheet_topendID[1]; i++)
        {
            pitemlist.addPlayerItem(i, 5);

        }

        count = database.sheet_topendID[2];

        j = database.sheet_topendID[3] - database.sheet_topendID[2];

        for (i = 0; i <= j; i++)
        {
            pitemlist.addPlayerItem(count, 5);
            ++count;
        }

        count = database.sheet_topendID[4];

        j = database.sheet_topendID[5] - database.sheet_topendID[4];

        for (i = 0; i <= j; i++)
        {
            pitemlist.addPlayerItem(count, 5);
            ++count;
        }
                                 
        pitemlist.addOriginalItem("neko_cookie", 0, 0, 20, 99, 99, 50, 30, 30, 0, 0, 0, 0, 0, 50, 50, 50, 20, 9999, 9999, "Orange", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 5);

        pitemlistController.AddItemList();
    }
}
