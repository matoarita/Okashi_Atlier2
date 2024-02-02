using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//シーンの最初にプレファブから生成するオブジェクト　全シーン共通で置いておく

public class SceneInitSetting : SingletonMonoBehaviour<SceneInitSetting>
{
    private GameObject canvas;

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    private GameObject yes_no_panel_obj;
    private GameObject yes_no_panel_Prefab;

    private GameObject playeritemlist_onoff;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject recipilist_onoff;
    private GameObject magicskill_list_onoff;

    private bool playerlist_check_on;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void InitSetting()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

        //シーン最初にYes,noパネルも生成する。基本、はいかいいえは、シーンで一個共通して使う
        //yes_no_panel_Prefab = (GameObject)Resources.Load("Prefabs/Yes_no_Panel");
        //yes_no_panel_obj = Instantiate(yes_no_panel_Prefab, canvas.transform);

        
    }

    public void PlayerItemListController_Init()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムリストがすでに生成されているかをチェック　Shop_Main+CompoundMainControllerが混在する場合などで、重複する可能性がありなので、重複回避も実施。
        playerlist_check_on = false;
        foreach (Transform child in canvas.transform)
        {
            if(child.name == "PlayeritemList_ScrollView")
            {
                playerlist_check_on = true;
            }
        }

        if(!playerlist_check_on)
        {
            //シーン最初にプレイヤー所持アイテムリストパネルの生成
            pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
            pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();

            playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            playeritemlist_onoff.SetActive(false);

            //シーン最初にレシピリストパネルの生成
            pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().RecipiList_ScrollView_Init();
            recipilist_onoff = canvas.transform.Find("RecipiList_ScrollView").gameObject;
            recipilist_onoff.SetActive(false);

            //シーン最初にスキルリストパネルの生成
            pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().MagicSkillList_ScrollView_Init();
            magicskill_list_onoff = canvas.transform.Find("MagicSkillList_Panel/MagicSkillList_ScrollView").gameObject;
            magicskill_list_onoff.SetActive(false);

            //シーン最初にカウンターも生成する。
            updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
            updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

            //シーン最初にYes,noパネルも生成する。基本、はいかいいえは、シーンで一個共通して使う
            //yes_no_panel_Prefab = (GameObject)Resources.Load("Prefabs/Yes_no_Panel");
            //yes_no_panel_obj = Instantiate(yes_no_panel_Prefab, canvas.transform);
        }
        
    }

}
