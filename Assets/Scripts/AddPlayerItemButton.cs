using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddPlayerItemButton : MonoBehaviour {

    private ItemDataBase database;

    private PlayerItemList pitemlist;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private int i, rand, _randID;
    private int count, j;

    // Use this for initialization
    void Start()
    {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //プレイヤー所持アイテムリストの取得
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
    }

    public void OnClickAddSkillButton2()
    {

        //基本アイテムのみ追加。
        for (i = 0; i <= database.sheet_topendID[1]; i++)
        {
            /*if( database.items[i].itemName == "egg")
            {
                pitemlist.addPlayerItem(i, 5);
            }*/

            if (database.items[i].itemName == "komugiko")
            {
                pitemlist.addPlayerItem(i, 5);
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
            if (database.items[i].itemName == "nuts")
            {
                pitemlist.addPlayerItem(i, 5);
            }
            
        }

        count = database.sheet_topendID[2];

        j = database.sheet_topendID[3] - database.sheet_topendID[2];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == "neko_cookie")
            {
                pitemlist.addPlayerItem(count, 5);
            }
            ++count;
        }

        //pitemlist.addOriginalItem("neko_cookie", 0, 0, 20, 5, 0.95f, 99, 99, 50, 30, 30, 0, 0, 0, 0, 0, 50, 50, 50, 20, 50, 50, "Orange", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 5, 3, 1);

        pitemlistController.AddItemList();

    }

    void Allitem_Add()
    {
        /*rand = Random.Range(0, database.items[database.sheet_topendID[1]].itemID);
            _randID = database.items[rand].itemID;*/

        //デバッグ用　すべてのアイテムを追加する。
        for (i = 0; i <= database.sheet_topendID[1]; i++)
        {
            if (database.items[i].itemType_sub.ToString() == "Pate" || database.items[i].itemType_sub.ToString() == "Cookie_base")
            {
                //生地タイプ、クッキーベースタイプ、アパレイユを無視する。
            }
            else
            {
                pitemlist.addPlayerItem(i, 5);
            }

            //アパレイユ追加。
            /*if (database.items[i].itemName == "appaleil")
            {
                pitemlist.addPlayerItem(i, 5);
            }*/
        }

        //お菓子タイプ
        count = database.sheet_topendID[2];

        j = database.sheet_topendID[3] - database.sheet_topendID[2];

        for (i = 0; i <= j; i++)
        {
            pitemlist.addPlayerItem(count, 5);
            ++count;
        }

        //ポーションタイプ
        count = database.sheet_topendID[4];

        j = database.sheet_topendID[5] - database.sheet_topendID[4];

        for (i = 0; i <= j; i++)
        {
            pitemlist.addPlayerItem(count, 5);
            ++count;
        }

        //その他タイプ
        count = database.sheet_topendID[6];

        j = database.sheet_topendID[7] - database.sheet_topendID[6];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemType_sub.ToString() != "Equip")
            {
                pitemlist.addPlayerItem(count, 5);
            }
            ++count;
        }

        pitemlist.addOriginalItem("neko_cookie", 0, 0, 20, 5, 0.95f, 99, 99, 50, 30, 30, 0, 0, 0, 0, 0, 50, 50, 50, 0, 20, 50, 50, "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 5, 3, 1, 0);

        pitemlistController.AddItemList();
    }
}
