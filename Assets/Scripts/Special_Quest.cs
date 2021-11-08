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

    private Dictionary<int, int> QuestCountDict;

    private int i;
    private int _keta;
    private int _stage_count;

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

        InitQuestCount();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSpecialOkashi(int _num, int _status)
    {
      
        spquest_set_num = _num;
        GameMgr.OkashiQuest_Num = _num; //現在のクエストナンバーを設定　GirlLoveEvent_numと数値は一緒

        //ステージの判定　10桁目をみてステージ数を自動で検出
        _stage_count = 1;
        _keta = _num;
        while (_keta >= 0)
        {
            _keta = _keta - 10;
            
            if (_keta < 0)
            {
                break;              
            }
            _stage_count++;
        }
        GameMgr.stage_quest_num = _stage_count;

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
                OkashiQuest_Count = 1;
                

                break;

            case 1: //ぶどうクッキー
             
                girl1_status.OkashiQuest_ID = 1010;
                OkashiQuest_Count = 2;

                break;

            case 2: //かわいいクッキー
             
                girl1_status.OkashiQuest_ID = 1020;
                OkashiQuest_Count = 3;
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 10: //ラスク食べたい

                girl1_status.OkashiQuest_ID = 1100;
                OkashiQuest_Count = 1;

                break;

            case 11: //すっぱいラスク食べたい

                girl1_status.OkashiQuest_ID = 1110;
                OkashiQuest_Count = 2;
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 12: //幻の青色紅茶食べたい＜10ラスクからの分岐２＞

                girl1_status.OkashiQuest_ID = 1120;
                OkashiQuest_Count = 2;
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 13: //キラキララスク食べたい＜10ラスクからの分岐１＞

                girl1_status.OkashiQuest_ID = 1130;
                OkashiQuest_Count = 2;
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 20: //クレープ食べたい

                girl1_status.OkashiQuest_ID = 1200;
                OkashiQuest_Count = 1;

                break;

            case 21: //オレンジクレープ食べたい

                girl1_status.OkashiQuest_ID = 1210;
                OkashiQuest_Count = 2;

                break;

            case 24: //ミントアイスクレープ食べたい

                girl1_status.OkashiQuest_ID = 1240;
                OkashiQuest_Count = 3;
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 22: //ジェム・ボンボン 宝石のような見た目のお菓子を食べたい

                girl1_status.OkashiQuest_ID = 1220;
                OkashiQuest_Count = 4;

                break;

            case 23: //豪華なベリークレープ食べたい

                girl1_status.OkashiQuest_ID = 1230;
                OkashiQuest_Count = 5;
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;
            

            /*case 29: //クレープ＜20クレープからの分岐１＞ 200点クレープ

                girl1_status.OkashiQuest_ID = 1290;
                OkashiQuest_Count = 2;

                break;*/

            /*case 22: //アイスクリームを食べたい

                girl1_status.OkashiQuest_ID = 1220;
                OkashiQuest_Count = 3;
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;*/

            case 30: //シュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1300;
                OkashiQuest_Count = 1;
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 40: //ドーナツ食べたい

                girl1_status.OkashiQuest_ID = 1400;
                OkashiQuest_Count = 1;
                GameMgr.QuestClearAnim_Flag = true; //そのクエストの最後は、ボタンを登場させる。

                break;

            case 50: //ステージ１ラスト　コンテスト開始

                girl1_status.OkashiQuest_ID = 1500;
                
                OkashiQuest_Count = 1;
                break;

            default:
                break;
        }

        //クエストボタンを登場させる。
        GameMgr.QuestClearAnim_Flag = true;

        //クエストネームの設定
        QuestNameFind();

        //◆ボタン表示用
        OkashiQuest_AllCount = QuestCountDict[GameMgr.stage_quest_num];

        GameMgr.stage_quest_num_sub = OkashiQuest_Count;

        if (GameMgr.OkashiQuest_Num != 50)
        {
            OkashiQuest_Number = GameMgr.stage_quest_num.ToString() + "-" + OkashiQuest_Count.ToString(); //表示用のステージ番号
        }
        else
        {
            OkashiQuest_Number = "Contest";
        }

        //吹き出しは消す。
        girl1_status.ResetHukidashiNoSound();
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
        QuestDict.Add(1220, 22);
        QuestDict.Add(1230, 23);
        QuestDict.Add(1240, 24);
        QuestDict.Add(1300, 30);
        QuestDict.Add(1400, 40);
        QuestDict.Add(1500, 50);
    }

    void InitQuestCount() //ステージごとの、クエストの総数　1なら、クエスト3個など。クエストの進行度を表示する◆ボタン用に使う。
    {
        QuestCountDict = new Dictionary<int, int>();

        QuestCountDict.Add(1, 3);
        QuestCountDict.Add(2, 2);
        QuestCountDict.Add(3, 5);
        QuestCountDict.Add(4, 1);
        QuestCountDict.Add(5, 1);
    }

    //GirlLikeCompoのクエストのIDを入れると、GirlloveEventNumに変換して、SPクエストを指定する。
    public void SetSpecialOkashiDict(int _questID, int _status)
    {
        _questnum = QuestDict[_questID];
        GameMgr.GirlLoveEvent_num = _questnum;

        SetSpecialOkashi(_questnum, _status);
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
