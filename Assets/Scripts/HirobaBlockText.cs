using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HirobaBlockText : MonoBehaviour {

    private ContestStartListDataBase conteststartList_database;

    private GameObject ScrollView;
    private GameObject BlockText_obj;
    private Text BlockText;

    // Use this for initialization
    void Start () {

    }

    void InitSetting()
    {
        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        ScrollView = this.transform.parent.parent.parent.gameObject; //自分が今どこのViewにいるか
        BlockText_obj = this.transform.Find("Background/BlockText").gameObject;
        BlockText = BlockText_obj.GetComponent<Text>();

        MapDefaultFlag();
        CheckArea();
    }

    private void OnEnable()
    {
        InitSetting();
    }

    // Update is called once per frame
    void Update()
    {
        CheckArea();
    }

    void CheckArea()
    {
        BlockCheck(); //ハートレベル〇〇必要などのテキストの表示／非表示
        NPCEventCheck(); //NPCイベントも発生しているかチェック
        AreaGoCheck(); //エリア進める→を表示するチェック
    }

    void BlockCheck()
    {
        switch(ScrollView.name)
        {
            case "MainList_ScrollView_04":

                if(this.gameObject.name == "NPC4_SelectToggle")
                {
                    //ハートレベルで通れない箇所のチェック
                    if (PlayerStatus.girl1_Love_lv < GameMgr.System_HeartBlockLv_01)
                    {
                        BlockText_obj.SetActive(true);
                        BlockText.text = "ハートLVが" + "\n" + GameMgr.System_HeartBlockLv_01.ToString() + "必要";
                    }
                    else
                    {
                        BlockText_obj.SetActive(false);
                    }
                }
                break;
        }
    }

    void NPCEventCheck()
    {
        switch (ScrollView.name)
        {
            case "MainList_ScrollView_02":

                if (this.gameObject.name == "NPC4_SelectToggle")
                {
                    if (!GameMgr.outgirl_Nowprogress)
                    {
                        if (!GameMgr.NPCHiroba_HikarieventList[100]) //散歩道　ヒカリイベント
                        {
                            this.transform.Find("Background").gameObject.SetActive(true);
                        }
                        else
                        {
                            this.transform.Find("Background").gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        this.transform.Find("Background").gameObject.SetActive(false);
                    }
                }
                break;

            case "MainList_ScrollView_200":

                if (this.gameObject.name == "NPC2_SelectToggle")
                {
                        if (!GameMgr.NPCHiroba_eventList[101]) //ぬね　友達になれなかった
                        {
                            this.transform.Find("Background").gameObject.SetActive(true);
                        }
                        else
                        {
                            this.transform.Find("Background").gameObject.SetActive(false);
                        }
                }
                break;
        }
    }

    void AreaGoCheck()
    {
        switch (ScrollView.name)
        {
            case "MainList_ScrollView_51":

                if (this.gameObject.name == "NPC5_SelectToggle")
                {
                    if (GameMgr.System_DebugAreaKaikin_ON)
                    {
                        this.transform.Find("Background").gameObject.SetActive(true);
                    }
                    else
                    {
                        if (GameMgr.NPCHiroba_eventList[2502]) //冬エリア解放
                        {
                            this.transform.Find("Background").gameObject.SetActive(true);
                        }
                        else
                        {
                            this.transform.Find("Background").gameObject.SetActive(false);
                        }
                    }

                }
                /*if (this.gameObject.name == "NPC7_SelectToggle")
                {
                    if (GameMgr.System_DebugAreaKaikin_ON)
                    {
                        this.transform.Find("Background").gameObject.SetActive(true);
                    }
                    else
                    {
                        if (GameMgr.NPCHiroba_eventList[2503]) //城エリア解放
                        {
                            this.transform.Find("Background").gameObject.SetActive(true);
                        }
                        else
                        {
                            this.transform.Find("Background").gameObject.SetActive(false);
                        }
                    }

                }*/

                break;

            case "MainList_ScrollView_52":

                if (this.gameObject.name == "NPC1_SelectToggle")
                {
                    if (GameMgr.System_DebugAreaKaikin_ON)
                    {
                        this.transform.Find("Background").gameObject.SetActive(true);
                    }
                    else
                    {
                        if (GameMgr.NPCHiroba_eventList[2501]) //秋エリア解放
                        {
                            this.transform.Find("Background").gameObject.SetActive(true);
                        }
                        else
                        {
                            this.transform.Find("Background").gameObject.SetActive(false);
                        }
                    }

                }

                if (this.gameObject.name == "NPC5_SelectToggle")
                {

                    if (GameMgr.System_DebugAreaKaikin_ON)
                    {
                        this.transform.Find("Background").gameObject.SetActive(true);
                    }
                    else
                    {
                        if (GameMgr.NPCHiroba_eventList[2500]) //夏エリア解放
                        {
                            this.transform.Find("Background").gameObject.SetActive(true);
                        }
                        else
                        {
                            this.transform.Find("Background").gameObject.SetActive(false);
                        }

                    }
                }
                
                break;

            case "MainList_ScrollView_04":

                if (this.gameObject.name == "NPC5_SelectToggle")
                {
                    if (GameMgr.System_DebugAreaKaikin_ON)
                    {
                        this.transform.Find("Background").gameObject.SetActive(true);
                    }
                    else
                    {
                        //光パティシエ先生のはじめて会うイベントが発生してれば通れる
                        if (GameMgr.NPCMagic_eventList[0])
                        {
                            this.transform.Find("Background").gameObject.SetActive(true);
                        }
                        else
                        {
                            this.transform.Find("Background").gameObject.SetActive(false);
                        }
                    }
                }
                break;
        }
    }

    void MapDefaultFlag()
    {
        GameMgr.NPCHiroba_eventList[2500] = true; //夏エリア解放
        GameMgr.NPCHiroba_eventList[2501] = true; //秋エリア解放
        GameMgr.NPCHiroba_eventList[2502] = true; //冬エリア解放
        GameMgr.NPCHiroba_eventList[2503] = true; //城エリア解放
    }
}
