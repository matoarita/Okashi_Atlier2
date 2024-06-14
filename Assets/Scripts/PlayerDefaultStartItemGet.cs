using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultStartItemGet : SingletonMonoBehaviour<PlayerDefaultStartItemGet>
{

    //ゲームの最初にプレイヤーのアイテムを追加したりするスクリプト
    //

    private int i, ev_id;

    private PlayerItemList pitemlist;
    private ItemMatPlaceDataBase matplace_database;
    private ItemDataBase database;

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //タイミングも重要　Compound_Mainから読み出し
    public void DefaultStartPitem()
    {
        //ゲーム「はじめから」で始まった場合の、最初の一回だけする処理       
        if (GameMgr.gamestart_recipi_get != true)
        {
            GameMgr.gamestart_recipi_get = true; //フラグをONに。  

            //ゲームの一番最初に絶対手に入れるレシピ
            ev_id = pitemlist.Find_eventitemdatabase("najya_start_recipi");
            pitemlist.add_eventPlayerItem(ev_id, 1); //ナジャの基本のレシピを追加

            ev_id = pitemlist.Find_eventitemdatabase("ev01_neko_cookie_recipi");
            pitemlist.add_eventPlayerItem(ev_id, 1); //クッキーのレシピを追加

            ev_id = pitemlist.Find_eventitemdatabase("rusk_recipi");
            pitemlist.add_eventPlayerItem(ev_id, 1); //ラスクのレシピを追加
            pitemlist.EventReadOn("rusk_recipi");

            //すでにレシピ100%フラグなど達成してた場合は、引き継がれる要素
            /*if (GameMgr.GirlLoveSubEvent_stage1[101])
            {
                ev_id = pitemlist.Find_eventitemdatabase("silver_neko_cookie_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //銀のねこクッキーのレシピ
                pitemlist.EventReadOn("silver_neko_cookie_recipi");
            }
            if (GameMgr.GirlLoveSubEvent_stage1[102])
            {
                ev_id = pitemlist.Find_eventitemdatabase("gold_neko_cookie_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //金のねこクッキーのレシピ
                pitemlist.EventReadOn("gold_neko_cookie_recipi");
            }*/

            if (GameMgr.Story_Mode != 0) //エクストラモードの初期設定
            {
                ev_id = pitemlist.Find_eventitemdatabase("crepe_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //クレープのレシピを追加
                pitemlist.EventReadOn("crepe_recipi");

                ev_id = pitemlist.Find_eventitemdatabase("rusk_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //ラスクのレシピを追加
                pitemlist.EventReadOn("rusk_recipi");

                ev_id = pitemlist.Find_eventitemdatabase("donuts_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //ドーナツのレシピを追加
                pitemlist.EventReadOn("donuts_recipi");

                //
                matplace_database.matPlaceKaikin("Farm"); //牧場解禁
                matplace_database.matPlaceKaikin("BerryFarm"); //ベリーファーム解禁
                matplace_database.matPlaceKaikin("Lavender_field"); //ラベンダー畑解禁
                matplace_database.matPlaceKaikin("StrawberryGarden"); //ストロベリーガーデン解禁
                matplace_database.matPlaceKaikin("HimawariHill"); //ひまわり畑解禁
                matplace_database.matPlaceKaikin("Hiroba"); //広場解禁

                //装備品は最初からもっている。
                //pitemlist.addPlayerItemString("milkpan", 1);
                //pitemlist.addPlayerItemString("pan_knife", 1);
                //pitemlist.addPlayerItemString("siboribukuro", 1);
                //pitemlist.addPlayerItemString("whisk", 1);
                //pitemlist.addPlayerItemString("wind_mixer", 1);
                //pitemlist.addPlayerItemString("oil_extracter", 1);
                //pitemlist.addPlayerItemString("juice_mixer", 1);
                //pitemlist.addPlayerItemString("egg_splitter", 1);
                //pitemlist.addPlayerItemString("ice_box", 1);
                pitemlist.addPlayerItemString("flyer", 1);
            }

            //二週目以降、自動で出てくる。
            /*if (GameMgr.ending_count >= 1)
            {
                matplace_database.matPlaceKaikin("Bar"); //酒場解禁
            }*/

            //Debug.Log("プレイヤーステータス　アイテム初期化　実行");
            //初期に所持するアイテム

            pitemlist.addPlayerItemString("komugiko", 10);
            pitemlist.addPlayerItemString("butter", 5);
            pitemlist.addPlayerItemString("suger", 5);
            pitemlist.addPlayerItemString("orange", 3);
            //pitemlist.addPlayerItemString("grape", 2);
            //pitemlist.addPlayerItemString("stone_oven", 1);

            //最初から表示するマップフラグ関係
            MapFlag();
        }
    }

    void MapFlag()
    {
        //GameMgr.NPCHiroba_eventList[2500] = true; //夏エリア解放
        //GameMgr.NPCHiroba_eventList[2501] = true; //秋エリア解放
        //GameMgr.NPCHiroba_eventList[2502] = true; //冬エリア解放
        //GameMgr.NPCHiroba_eventList[2503] = true; //城エリア解放
    }

    //オブジェクト・アクセ以外のアイテム以外を全て追加する
    public void AddAllItem_NoAcce()
    {
        //デバッグ用　すべてのアイテムを追加する。
        for (i = 0; i < database.items.Count; i++)
        {
            if (database.items[i].itemType_sub.ToString() == "Equip" || database.items[i].itemType_sub.ToString() == "Object")
            {
                //生地タイプ、クッキーベースタイプ、アパレイユ、アクセサリー装備品を無視する。
            }
            else
            {
                pitemlist.addPlayerItem(database.items[i].itemName, 5);
            }
            //Debug.Log(database.items[i].itemName);
        }
    }
}
