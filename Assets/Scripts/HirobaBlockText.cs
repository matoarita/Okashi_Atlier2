using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HirobaBlockText : MonoBehaviour {

    private GameObject ScrollView;
    private GameObject BlockText_obj;
    private Text BlockText;

    // Use this for initialization
    void Start () {

    }

    void InitSetting()
    {
        ScrollView = this.transform.parent.parent.parent.gameObject; //自分が今どこのViewにいるか
        BlockText_obj = this.transform.Find("Background/BlockText").gameObject;
        BlockText = BlockText_obj.GetComponent<Text>();

        BlockCheck();
        NPCEventCheck(); //NPCイベントも発生しているかチェック
        //AreaGoCheck(); //エリア進める→を表示するチェック
    }

    void BlockCheck()
    {
        switch(ScrollView.name)
        {
            case "MainList_ScrollView_04":

                if(this.gameObject.name == "NPC4_SelectToggle")
                {
                    //ハートレベルで通れない箇所のチェック
                    if (PlayerStatus.girl1_Love_lv < 10)
                    {
                        BlockText_obj.SetActive(true);
                        BlockText.text = "ハートLVが" + "\n" + "10" + "必要";
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
                            this.gameObject.SetActive(true);
                        }
                        else
                        {
                            this.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }
                }
                break;

            case "MainList_ScrollView_200":

                if (this.gameObject.name == "NPC2_SelectToggle")
                {
                        if (!GameMgr.NPCHiroba_eventList[101]) //ぬね　友達になれなかった
                        {
                            this.gameObject.SetActive(true);
                        }
                        else
                        {
                            this.gameObject.SetActive(false);
                        }
                }
                break;
        }
    }

    void AreaGoCheck()
    {
        switch (ScrollView.name)
        {
            case "MainList_ScrollView_01":

                if (this.gameObject.name == "NPC2_SelectToggle")
                {

                    if (GameMgr.NPCHiroba_eventList[2500]) //夏エリア解放
                    {
                        this.gameObject.SetActive(true);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }

                }
                if (this.gameObject.name == "NPC3_SelectToggle")
                {

                    if (GameMgr.NPCHiroba_eventList[2501]) //秋エリア解放
                    {
                        this.gameObject.SetActive(true);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }

                }
                if (this.gameObject.name == "NPC5_SelectToggle")
                {

                    if (GameMgr.NPCHiroba_eventList[2502]) //冬エリア解放
                    {
                        this.gameObject.SetActive(true);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }

                }
                if (this.gameObject.name == "NPC7_SelectToggle")
                {

                    if (GameMgr.NPCHiroba_eventList[2503]) //城エリア解放
                    {
                        this.gameObject.SetActive(true);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }

                }
                break;
        }
    }

    private void OnEnable()
    {
        InitSetting();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
