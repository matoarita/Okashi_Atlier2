using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Shop : MonoBehaviour {

    private GameObject canvas;

    private Girl1_status girl1_status;

    private Shop_Main shop_Main;

    private PlayerItemList pitemlist;

    private int j;
    private int recipi_id;

    // Use this for initialization
    void Start () {

        //キャンバスの取得
        canvas = GameObject.FindWithTag("Canvas");

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //ショップオブジェクトの取得
        shop_Main = GameObject.FindWithTag("Shop_Main").GetComponent<Shop_Main>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TouchCharacterFace() //ヒントをだしてくれるときもある。
    {
        switch (GameMgr.scenario_flag)
        {

            case 160: //ラスク作り中。まずかったときにヒントをだす。

                if (girl1_status.girl_Mazui_flag)
                {
                    //バゲットのレシピもゲット
                    //レシピの追加
                    recipi_id = Find_eventitemdatabase("bugget_recipi");
                    pitemlist.add_eventPlayerItem(recipi_id, 1); //ラスクのレシピを追加

                    shop_Main.hukidasi_sub.SetActive(false);

                    GameMgr.scenario_ON = true;
                    GameMgr.shop_hint = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                }
                break;

            case 165: //ヒント出した後。

                if (girl1_status.girl_Mazui_flag)
                {
                    shop_Main.hukidasi_sub.SetActive(false);

                    GameMgr.scenario_ON = true;
                    GameMgr.shop_hint = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                }
                break;

            default:
                break;
        }
        
    }

    //アイテム名を入力すると、該当するeventitem_IDを返す処理
    public int Find_eventitemdatabase(string compo_itemname)
    {
        j = 0;
        while (j < pitemlist.eventitemlist.Count)
        {
            if (compo_itemname == pitemlist.eventitemlist[j].event_itemName)
            {
                return j;
            }
            j++;
        }

        return 9999; //該当するIDがない場合
    }
}
