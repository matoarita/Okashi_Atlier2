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
        //どんぐり追加
        pitemlist.addPlayerItem("emeralDongri", 5);
        pitemlist.addPlayerItem("sapphireDongri", 5);

        //基本アイテムのみ追加。
        /*pitemlist.addPlayerItem("komugiko", 5);
        pitemlist.addPlayerItem("butter", 5);
        pitemlist.addPlayerItem("suger", 5);
        pitemlist.addPlayerItem("orange", 5);
        pitemlist.addPlayerItem("nuts", 5);

        pitemlist.addPlayerItem("neko_cookie", 5);*/

        //pitemlist.addOriginalItem("neko_cookie", 0, 0, 20, 5, 0.95f, 99, 99, 50, 30, 30, 0, 0, 0, 0, 0, 50, 50, 50, 20, 50, 50, "Orange", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 5, 3, 1);

        pitemlistController.AddItemList();

    }

    void Allitem_Add()
    {
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

        pitemlist.addOriginalItem("neko_cookie", 0, 0, 20, 5, 0.95f, 60, 60, 50, 10, 200, 0, 0, 0, 0, 0, 50, 50, 50, 50, 120, 20, 50, 50, "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 5, 3, 1, 0);

        pitemlistController.AddItemList();
    }
}
