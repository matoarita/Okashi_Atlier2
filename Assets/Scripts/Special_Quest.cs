using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//スペシャルお菓子　吹き出しクエストのON/OFFを管理

public class Special_Quest : SingletonMonoBehaviour<Special_Quest>
{
    private GameObject canvas;

    private Girl1_status girl1_status;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private Text questname;

    public int[] special_score_record; //そのときの、点数も記録。一つの列に3個点数を保持。
    public int spquest_set_num;

    public string OkashiQuest_Name;
    public string OkashiQuest_Number;
    public int OkashiQuest_AllCount;
    public int OkashiQuest_Count;
    public Sprite OkashiQuest_sprite;

    private Dictionary<int, int> QuestDict;
    private int _questnum;

    private int i;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        special_score_record = new int[GameMgr.GirlLoveEvent_stage1.Length];

        for (i = 0; i < special_score_record.GetLength(0); i++)
        {
            special_score_record[i] = 0;
        }

        //クエストナンバーの紐づけ
        InitQuestNumDict();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSpecialOkashi(int _num, int _status)
    {
      
        spquest_set_num = _num;
        GameMgr.OkashiQuest_Num = _num; //現在のクエストナンバーを設定　GirlLoveEvent_numと数値は一緒

        if (_status == 0)
        {
            girl1_status.OkashiNew_Status = 0;
            GameMgr.QuestClearflag = false;
        }
        else //ロードから再開された場合の処理
        {

        }

        GameMgr.QuestClearAnim_Flag = false; //クエスト前に一度falseでリセット

        switch (_num)
        {
            case 0: //オリジナルクッキーを食べたい

                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。               
                girl1_status.OkashiQuest_ID = 1000;
                //GameMgr.stageclear_love = 20; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "1-1";
                OkashiQuest_AllCount = 3;
                OkashiQuest_Count = 1;
                //GameMgr.OkashiQuest_cullentcount = 1; //現在のクエストが、全体の中で何番目かを決定。デバッグでハートレベル更新する用。
                girl1_status.ResetHukidashi();

                break;

            case 1: //ぶどうクッキー

                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。               
                girl1_status.OkashiQuest_ID = 1010;
                //GameMgr.stageclear_love = 50; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "1-2";
                OkashiQuest_AllCount = 3;
                OkashiQuest_Count = 2;
                //GameMgr.OkashiQuest_cullentcount = 2;
                girl1_status.ResetHukidashi();

                break;

            case 2: //かわいいクッキー

                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。               
                girl1_status.OkashiQuest_ID = 1020;
                //GameMgr.stageclear_love = 60; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "1-3";
                OkashiQuest_AllCount = 3;
                OkashiQuest_Count = 3;
                //GameMgr.OkashiQuest_cullentcount = 3;
                girl1_status.ResetHukidashi();
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 10: //ラスク食べたい

                girl1_status.OkashiQuest_ID = 1100;
                //GameMgr.stageclear_love = 50; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "2-1";
                OkashiQuest_AllCount = 2;
                OkashiQuest_Count = 1;
                //GameMgr.OkashiQuest_cullentcount = 4;
                girl1_status.ResetHukidashi();

                break;

            case 11: //すっぱいラスク食べたい

                girl1_status.OkashiQuest_ID = 1110;
                //GameMgr.stageclear_love = 50; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "2-2";
                OkashiQuest_AllCount = 2;
                OkashiQuest_Count = 2;
                //GameMgr.OkashiQuest_cullentcount = 5;
                girl1_status.ResetHukidashi();
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 12: //幻の青色紅茶食べたい＜10ラスクからの分岐２＞

                girl1_status.OkashiQuest_ID = 1120;
                //GameMgr.stageclear_love = 50; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "2-2";
                OkashiQuest_AllCount = 2;
                OkashiQuest_Count = 2;
                //GameMgr.OkashiQuest_cullentcount = 5;
                girl1_status.ResetHukidashi();
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 13: //キラキララスク食べたい＜10ラスクからの分岐１＞

                girl1_status.OkashiQuest_ID = 1130;
                //GameMgr.stageclear_love = 50; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "2-2";
                OkashiQuest_AllCount = 2;
                OkashiQuest_Count = 2;
                //GameMgr.OkashiQuest_cullentcount = 5;
                girl1_status.ResetHukidashi();
                //GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 20: //クレープ食べたい

                girl1_status.OkashiQuest_ID = 1200;
                //GameMgr.stageclear_love = 60; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "3-1";
                OkashiQuest_AllCount = 2;
                OkashiQuest_Count = 1;
                //GameMgr.OkashiQuest_cullentcount = 6;
                girl1_status.ResetHukidashi();

                break;

            case 21: //豪華なクレープ食べたい

                girl1_status.OkashiQuest_ID = 1210;
                //GameMgr.stageclear_love = 100; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "3-2";
                OkashiQuest_AllCount = 2;
                OkashiQuest_Count = 2;
                //GameMgr.OkashiQuest_cullentcount = 7;
                girl1_status.ResetHukidashi();
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 22: //クレープ＜20クレープからの分岐１＞

                girl1_status.OkashiQuest_ID = 1220;
                //GameMgr.stageclear_love = 60; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "Secret";
                OkashiQuest_AllCount = 2;
                OkashiQuest_Count = 2;
                //GameMgr.OkashiQuest_cullentcount = 6;
                girl1_status.ResetHukidashi();

                break;

            /*case 22: //アイスクリームを食べたい

                girl1_status.OkashiQuest_ID = 1220;
                GameMgr.stageclear_love = 100; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "3-3";
                OkashiQuest_AllCount = 3;
                OkashiQuest_Count = 3;
                girl1_status.ResetHukidashi();
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;*/

            case 30: //シュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1300;
                //GameMgr.stageclear_love = 60; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "4-1";
                OkashiQuest_AllCount = 1;
                OkashiQuest_Count = 1;
                //GameMgr.OkashiQuest_cullentcount = 8;
                girl1_status.ResetHukidashi();
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 40: //ドーナツ食べたい

                girl1_status.OkashiQuest_ID = 1400;
                //GameMgr.stageclear_love = 80; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "5-1";
                OkashiQuest_AllCount = 1;
                OkashiQuest_Count = 1;
                //GameMgr.OkashiQuest_cullentcount = 9;
                girl1_status.ResetHukidashi();
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 50: //ステージ１ラスト　コンテスト開始

                girl1_status.OkashiQuest_ID = 1500;
                //GameMgr.stageclear_love = 1000; //クエスト以外のお菓子で、ハートをこの量集めたら、クリアできる。
                QuestNameFind();
                OkashiQuest_Number = "Contest";
                OkashiQuest_AllCount = 1;
                OkashiQuest_Count = 1;
                //GameMgr.OkashiQuest_cullentcount = 10;
                girl1_status.ResetHukidashi();
                break;

            default:
                break;
        } 
    }

    //GirlLikeCompoのクエストのIDを入れると、GirlloveEventNumに変換して、SPクエストを指定する。
    public void SetSpecialOkashiDict(int _questID, int _status)
    {
        _questnum = QuestDict[_questID];
        GameMgr.GirlLoveEvent_num = _questnum;

        SetSpecialOkashi(_questnum, _status);
    }

    //エクセルDBのset_compID（クエスト番号）と、GirlLoveEvent_numの紐づけ。ゲーム中クエストの総数でもある。
    void InitQuestNumDict()
    {
        QuestDict = new Dictionary<int, int>();

        QuestDict.Add(1000, 0);
        QuestDict.Add(1010, 1);
        QuestDict.Add(1020, 2);
        QuestDict.Add(1100, 10);
        QuestDict.Add(1110, 11);
        QuestDict.Add(1120, 12);
        QuestDict.Add(1130, 13);
        QuestDict.Add(1200, 20);
        QuestDict.Add(1210, 21);
        QuestDict.Add(1300, 30);
        QuestDict.Add(1400, 40);
        QuestDict.Add(1500, 50);
    }   

    void QuestNameFind()
    {
        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
        {
            if (girlLikeCompo_database.girllike_composet[i].set_ID == girl1_status.OkashiQuest_ID)
            {
                girlLikeCompo_database.girllike_composet[i].clearFlag = true; //クリアした

                OkashiQuest_Name = girlLikeCompo_database.girllike_composet[i].spquest_name1;
                OkashiQuest_sprite = girlLikeCompo_database.girllike_composet[i].itemIcon_sprite;
                GameMgr.NextQuestID = girlLikeCompo_database.girllike_composet[i].next_ID;
                girl1_status.OkashiQuest_Name = OkashiQuest_Name;
            }
        }
    }

    public void RedrawQeustName()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //メイン画面に表示する、現在のクエスト
        questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();

        //現在のクエストネーム更新。
        questname.text = OkashiQuest_Name;
    }

    
}
