using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddPlayerItemButton : MonoBehaviour {

    private GameObject canvas;

    private ItemDataBase database;

    private PlayerItemList pitemlist;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private int i, rand, _randID;
    private int count, j;

    // Use this for initialization
    void Start()
    {
        

    }

    void InitSetting()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
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
        InitSetting();

        //どんぐり追加
        pitemlist.addPlayerItem("emeralDongri", 5);
        pitemlist.addPlayerItem("sapphireDongri", 5);       

        pitemlistController.AddItemList();
    }

    public void OnClickAddSkillButton3()
    {
        InitSetting();

        //ALLPotionのみ追加
        pitemlist.addPlayerItem("ALL_potion", 5);

        pitemlistController.AddItemList();
    }

    public void OnClickDeleteItem()
    {
        InitSetting();

        //デバッグ用　材料アイテムを減らす。
        for (i = 0; i < database.items.Count; i++)
        {
            if (database.items[i].itemType_sub.ToString() == "Pate" || database.items[i].itemType_sub.ToString() == "Cookie_base" ||
                database.items[i].itemType_sub.ToString() == "Machine" || database.items[i].itemType_sub.ToString() == "Record" ||
                database.items[i].itemType_sub.ToString() == "Equip" || database.items[i].itemType_sub.ToString() == "Object" ||
                database.items[i].itemType_sub.ToString() == "Donguri" || database.items[i].itemType_sub.ToString() == "Rare" ||
                database.items[i].itemType_sub.ToString() == "Valuable" || database.items[i].itemType_sub.ToString() == "EventItem")
            {
                //生地タイプ、クッキーベースタイプ、アパレイユ、アクセサリー装備品を無視する。
            }
            else
            {
                pitemlist.deletePlayerItem(database.items[i].itemName, 5);
            }
            //Debug.Log(database.items[i].itemName);
           
        }

        pitemlistController.AddItemList();
    }

    public void OnClickDeleteItemALL()
    {
        InitSetting();

        //デバッグ用　すべてのアイテムを減らす。
        for (i = 0; i < database.items.Count; i++)
        {
            if (database.items[i].itemType_sub.ToString() == "Pate" || database.items[i].itemType_sub.ToString() == "Cookie_base" ||
                database.items[i].itemType_sub.ToString() == "Equip" || database.items[i].itemType_sub.ToString() == "Object")
            {
                //生地タイプ、クッキーベースタイプ、アパレイユ、アクセサリー装備品を無視する。これはallアイテム追加のときに増えないので、ここでも減らさない。
            }else
            {
                pitemlist.deletePlayerItem(database.items[i].itemName, 5);
            }
            
        }

        pitemlistController.AddItemList();
    }

    public void OnClickStarAdd()
    {
        //デバッグ用　スターを追加

        PlayerStatus.player_ninki_param += 5;
    }

    public void OnClickStarDeg()
    {
        //デバッグ用　スターを追加

        PlayerStatus.player_ninki_param -= 5;
    }

    void Allitem_Add()
    {
        InitSetting();

        /*rand = Random.Range(0, database.items[database.sheet_topendID[1]].itemID);
            _randID = database.items[rand].itemID;*/

        //デバッグ用　すべてのアイテムを追加する。
        for (i = 0; i < database.items.Count; i++)
        {
            if (database.items[i].itemType_sub.ToString() == "Pate" || database.items[i].itemType_sub.ToString() == "Cookie_base" || 
                database.items[i].itemType_sub.ToString() == "Equip" || database.items[i].itemType_sub.ToString() == "Object")
            {
                //生地タイプ、クッキーベースタイプ、アパレイユ、アクセサリー装備品を無視する。
            }
            else
            {
                pitemlist.addPlayerItem(database.items[i].itemName, 5);
            }
            //Debug.Log(database.items[i].itemName);
        }

        /*pitemlist.addOriginalItem("neko_cookie", 0, 0, 20, 5, 0.95f, 60, 60, 50, 10, 200, 0, 0, 0, 0, 0, 50, 50, 50, 50, 120, 
            0,             
            20, 50, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 5, 3, 1, 0);*/

        pitemlistController.AddItemList();
    }


}
