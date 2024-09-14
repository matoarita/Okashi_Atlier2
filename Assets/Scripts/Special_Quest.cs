using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//スペシャルお菓子　吹き出しクエストのON/OFFを管理

public class Special_Quest : SingletonMonoBehaviour<Special_Quest>
{
    private GameObject canvas;

    private Girl1_status girl1_status;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    private ItemDataBase database;

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

    private int i, _ID;
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

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

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

    //EventDataBaseから設定
    public void SetSpecialOkashi(int _num, int _status)
    {
        //クエスト総数の更新
        InitQuestCount();

        spquest_set_num = _num;
        GameMgr.GirlLoveEvent_num = _num; //現在のクエストナンバーを設定

        //ステージの判定　10桁目をみてステージ数を自動で検出 1の位の数もみて、現在のクエスト番号を見る。
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
            GameMgr.QuestClearflag = false;
        }
        else if (_status == 2) //エクストラ
        {
            GameMgr.QuestClearflag = false;
        }
        else //ロードから再開された場合の処理
        {

        }

        GameMgr.QuestClearAnim_Flag = false; //クエスト前に一度falseでリセット

        
        if (GameMgr.Story_Mode == 0)
        {
            //Stage1_Normal(spquest_set_num);
            Stage2_Main(spquest_set_num);
            //全てのクエストで、クエストクリア時にクエストボタンを登場させる。
            GameMgr.QuestClearAnim_Flag = true;
        }
        else
        {
            Stage1_Extra(spquest_set_num); //エクストラ
            GameMgr.QuestClearAnim_Flag = true;　//全てのクエストで、クエストボタンなしで次へ。
        }

       
        //クエストネームの設定
        RedrawQuestName(); //ネーム更新

        //◆ボタン表示用
        OkashiQuest_AllCount = QuestCountDict[GameMgr.stage_quest_num];

        GameMgr.stage_quest_num_sub = OkashiQuest_Count;

        if (GameMgr.GirlLoveEvent_num != 50)
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


    //ステージ設定
    //「OkashiQuest_ID」は、ここでのみ設定している。
    void Stage1_Normal(int _spquest_setnum)
    {
        switch (_spquest_setnum)
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

                break;

            case 10: //ラスク食べたい

                girl1_status.OkashiQuest_ID = 1100;
                OkashiQuest_Count = 1;

                break;

            case 11: //すっぱいラスク食べたい

                girl1_status.OkashiQuest_ID = 1110;
                OkashiQuest_Count = 2;

                break;

            case 12: //幻の青色紅茶食べたい＜13ラスクからの分岐＞

                girl1_status.OkashiQuest_ID = 1120;
                OkashiQuest_Count = 3;

                break;

            case 13: //キラキララスク食べたい＜10ラスクからの分岐１＞

                girl1_status.OkashiQuest_ID = 1130;
                OkashiQuest_Count = 2;

                break;

            case 20: //クレープ食べたい

                girl1_status.OkashiQuest_ID = 1200;
                OkashiQuest_Count = 1;

                break;

            case 21: //オレンジクレープ食べたい

                girl1_status.OkashiQuest_ID = 1210;
                OkashiQuest_Count = 2;

                break;

            case 22: //アイス食べたい

                girl1_status.OkashiQuest_ID = 1240;
                OkashiQuest_Count = 3;

                break;

            case 23: //ジェム・ボンボン 宝石のような見た目のお菓子を食べたい

                girl1_status.OkashiQuest_ID = 1220;
                OkashiQuest_Count = 4;

                break;

            case 24: //豪華なベリークレープ食べたい

                girl1_status.OkashiQuest_ID = 1230;
                OkashiQuest_Count = 5;

                break;


            /*case 29: //クレープ＜20クレープからの分岐１＞ 200点クレープ

                girl1_status.OkashiQuest_ID = 1290;
                OkashiQuest_Count = 2;

                break;*/


            case 30: //シュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1300;
                OkashiQuest_Count = 1;

                break;

            case 31: //ラズベリーシュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1310;
                OkashiQuest_Count = 2;


                break;

            case 32: //カフェオーレシュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1320;
                OkashiQuest_Count = 3;


                break;

            case 33: //ティラミス食べたい

                girl1_status.OkashiQuest_ID = 1330;
                OkashiQuest_Count = 4;


                break;

            case 34: //150点以上のシュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1340;
                OkashiQuest_Count = 5;

                break;

            case 40: //ドーナツ食べたい

                girl1_status.OkashiQuest_ID = 1400;
                OkashiQuest_Count = 1;

                break;

            case 50: //ステージ１ラスト　コンテスト開始

                girl1_status.OkashiQuest_ID = 1500;

                OkashiQuest_Count = 1;
                break;

            default:
                break;
        }
    }

    void Stage1_Extra(int _spquest_setnum)
    {
        switch (_spquest_setnum)
        {
            case 0: //茶色クッキー　（ハートを100）

                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。               
                girl1_status.OkashiQuest_ID = 10000;
                OkashiQuest_Count = 1;

                break;

            case 1: //ヒカリにお菓子一個覚えさせる

                girl1_status.OkashiQuest_ID = 10010;
                OkashiQuest_Count = 2;

                break;

            case 2: //未使用

                girl1_status.OkashiQuest_ID = 10020;
                OkashiQuest_Count = 3;

                break;

            case 3: //ピンクのクッキー

                girl1_status.OkashiQuest_ID = 10030;
                OkashiQuest_Count = 3;

                break;

            case 4: //スーパークッキー

                girl1_status.OkashiQuest_ID = 10040;
                OkashiQuest_Count = 4;

                break;

            case 10: //茶色クッキー

                girl1_status.OkashiQuest_ID = 10100;
                OkashiQuest_Count = 1;

                break;

            case 11: //お茶会用のお茶

                girl1_status.OkashiQuest_ID = 10110;
                OkashiQuest_Count = 2;

                break;

            case 12: //豪華なベリークレープ

                girl1_status.OkashiQuest_ID = 10120;
                OkashiQuest_Count = 3;

                break;

            case 13: //カミナリのすっぱいクレープ

                girl1_status.OkashiQuest_ID = 10130;
                OkashiQuest_Count = 4;

                break;

            case 14: //300点以上のいちごのクレープ

                girl1_status.OkashiQuest_ID = 10140;
                OkashiQuest_Count = 5;

                break;

            case 20: //大人でムーディーなお菓子

                girl1_status.OkashiQuest_ID = 10200;
                OkashiQuest_Count = 1;

                break;

            case 21: //未使用　（ハート3000以上）

                girl1_status.OkashiQuest_ID = 10210;
                OkashiQuest_Count = 2;

                break;

            case 22: //未使用　（ヒカリが10種類のお菓子を作れる）

                girl1_status.OkashiQuest_ID = 10220;
                OkashiQuest_Count = 3;

                break;

            case 23: //200点超える夢のようなパンケーキ

                girl1_status.OkashiQuest_ID = 10230;
                OkashiQuest_Count = 2;

                break;

            case 24: //ミントとプリンセストータで対決！

                //Debug.Log("Extra 24: Check");
                girl1_status.OkashiQuest_ID = 10240;
                OkashiQuest_Count = 3;

                //３と４は飛ばす GirlEatJudgeのほうで、条件分岐    
                //debug_panelでも更新してる。

                break;

            case 50: //ステージ１ラスト　コンテスト開始

                girl1_status.OkashiQuest_ID = 10500;
                OkashiQuest_Count = 1;
                break;

            default:
                break;
        }
    }

    void Stage2_Main(int _spquest_setnum)
    {
        switch (_spquest_setnum)
        {
            case 0: //２の最初　やはりオリジナルクッキーを食べたい

                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。               
                girl1_status.OkashiQuest_ID = 100000;
                OkashiQuest_Count = 1;
                GameMgr.EatOkashi_DecideFlag = 0; //0=食べたいお菓子がランダムでなくなり、メインクエストに固定する
                GameMgr.SPquestPanelOff = false; //false = Spクエストパネルを表示する

                break;

            case 1: //さくらクッキー

                girl1_status.OkashiQuest_ID = 100010;
                OkashiQuest_Count = 2;
                GameMgr.EatOkashi_DecideFlag = 0;
                GameMgr.SPquestPanelOff = false;

                break;

            case 2: //かわいいクッキー

                girl1_status.OkashiQuest_ID = 100020;
                OkashiQuest_Count = 3;
                GameMgr.EatOkashi_DecideFlag = 0;
                GameMgr.SPquestPanelOff = false;

                break;

            case 3: //ここから自由に探索パート開始 エデンのてがかりを探そう

                girl1_status.OkashiQuest_ID = 100030;
                OkashiQuest_Count = 4;
                GameMgr.EatOkashi_DecideFlag = 1; //0=食べたいお菓子がランダムでなくなり、メインクエストに固定する                
                //GameMgr.SPquestPanelOff = true;

                break;

            case 4: //コンテストに出場しよう

                girl1_status.OkashiQuest_ID = 100040;
                OkashiQuest_Count = 5;
                GameMgr.EatOkashi_DecideFlag = 1;
                //GameMgr.SPquestPanelOff = true;

                break;

            case 10: //ラスク食べたい

                girl1_status.OkashiQuest_ID = 1100;
                OkashiQuest_Count = 1;

                break;

            case 11: //すっぱいラスク食べたい

                girl1_status.OkashiQuest_ID = 1110;
                OkashiQuest_Count = 2;

                break;

            case 12: //幻の青色紅茶食べたい＜13ラスクからの分岐＞

                girl1_status.OkashiQuest_ID = 1120;
                OkashiQuest_Count = 3;

                break;

            case 13: //キラキララスク食べたい＜10ラスクからの分岐１＞

                girl1_status.OkashiQuest_ID = 1130;
                OkashiQuest_Count = 2;

                break;

            case 20: //クレープ食べたい

                girl1_status.OkashiQuest_ID = 1200;
                OkashiQuest_Count = 1;

                break;

            case 21: //オレンジクレープ食べたい

                girl1_status.OkashiQuest_ID = 1210;
                OkashiQuest_Count = 2;

                break;

            case 22: //アイス食べたい

                girl1_status.OkashiQuest_ID = 1240;
                OkashiQuest_Count = 3;

                break;

            case 23: //ジェム・ボンボン 宝石のような見た目のお菓子を食べたい

                girl1_status.OkashiQuest_ID = 1220;
                OkashiQuest_Count = 4;

                break;

            case 24: //豪華なベリークレープ食べたい

                girl1_status.OkashiQuest_ID = 1230;
                OkashiQuest_Count = 5;

                break;


            /*case 29: //クレープ＜20クレープからの分岐１＞ 200点クレープ

                girl1_status.OkashiQuest_ID = 1290;
                OkashiQuest_Count = 2;

                break;*/


            case 30: //シュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1300;
                OkashiQuest_Count = 1;

                break;

            case 31: //ラズベリーシュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1310;
                OkashiQuest_Count = 2;


                break;

            case 32: //カフェオーレシュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1320;
                OkashiQuest_Count = 3;


                break;

            case 33: //ティラミス食べたい

                girl1_status.OkashiQuest_ID = 1330;
                OkashiQuest_Count = 4;


                break;

            case 34: //150点以上のシュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1340;
                OkashiQuest_Count = 5;

                break;

            case 40: //ドーナツ食べたい

                girl1_status.OkashiQuest_ID = 1400;
                OkashiQuest_Count = 1;

                break;

            case 50: //ステージ１ラスト　コンテスト開始

                girl1_status.OkashiQuest_ID = 1500;

                OkashiQuest_Count = 1;
                break;

            default:
                break;
        }

        if(_spquest_setnum >= 3)
        {
            GameMgr.OutEntrance_ON = true;
        }
    }

    //GirlLikeSetCompoエクセルDBのset_compID（クエスト番号）と、GirlLoveEvent_numの紐づけ。ゲーム中メインクエストの総数でもある。
    void InitQuestNumDict()
    {
        QuestDict = new Dictionary<int, int>();

        //１のやつ
        //本編のモード
        QuestDict.Add(1000, 0);
        QuestDict.Add(1010, 1);
        QuestDict.Add(1020, 2);        
        QuestDict.Add(1100, 10);
        QuestDict.Add(1110, 11);
        QuestDict.Add(1120, 12);
        QuestDict.Add(1130, 13);
        QuestDict.Add(1200, 20);
        QuestDict.Add(1210, 21);
        QuestDict.Add(1220, 23);
        QuestDict.Add(1230, 24);
        QuestDict.Add(1240, 22);
        QuestDict.Add(1300, 30);
        QuestDict.Add(1310, 31);
        QuestDict.Add(1320, 32);
        QuestDict.Add(1330, 33);
        QuestDict.Add(1340, 34);
        QuestDict.Add(1400, 40);
        QuestDict.Add(1500, 50);

        //エクストラモード用　左の数字は、上のやつと被らないように。
        QuestDict.Add(10000, 0);
        QuestDict.Add(10010, 1);
        QuestDict.Add(10020, 2);
        QuestDict.Add(10030, 3);
        QuestDict.Add(10040, 4);
        QuestDict.Add(10100, 10);
        QuestDict.Add(10110, 11);
        QuestDict.Add(10120, 12);
        QuestDict.Add(10130, 13);
        QuestDict.Add(10140, 14);
        QuestDict.Add(10200, 20);
        QuestDict.Add(10210, 21);
        QuestDict.Add(10220, 22);
        QuestDict.Add(10230, 23);
        QuestDict.Add(10240, 24);
        QuestDict.Add(10500, 50);
        //

        //２から
        QuestDict.Add(100000, 0);
        QuestDict.Add(100010, 1);
        QuestDict.Add(100020, 2);
        QuestDict.Add(100030, 3);
        QuestDict.Add(100040, 4);
        QuestDict.Add(100100, 10);
        QuestDict.Add(100110, 11);
        QuestDict.Add(100120, 12);
        QuestDict.Add(100130, 13);
        QuestDict.Add(100140, 14);
        //QuestDict.Add(100500, 50);
    }

    void InitQuestCount() //ステージごとの、クエストの総数　1なら、クエスト3個など。クエストの進行度を表示する◆ボタン用に使う。
    {
        QuestCountDict = new Dictionary<int, int>();

        /*if (GameMgr.Story_Mode == 0)
        {
            QuestCountDict.Add(1, 3);
            QuestCountDict.Add(2, 2);
            QuestCountDict.Add(3, 1);
            QuestCountDict.Add(4, 1);
            QuestCountDict.Add(5, 1);
            QuestCountDict.Add(6, 1);
        }
        else
        {
            QuestCountDict.Add(1, 4);
            QuestCountDict.Add(2, 5);
            QuestCountDict.Add(3, 3);
            QuestCountDict.Add(6, 1);
        }*/

        QuestCountDict.Add(1, 5);
        QuestCountDict.Add(2, 5);
    }

    //GirlLikeCompoのクエストのIDを入れると、GirlloveEventNumに変換して、SPクエストを指定する。GirlEatJudgeから読み出し。
    public void SetSpecialOkashiDict(int _questID, int _status)
    {
        _questnum = QuestDict[_questID];
        GameMgr.GirlLoveEvent_num = _questnum;

        SetSpecialOkashi(_questnum, _status);
    }

    

    void QuestNameFind()
    {
        if (GameMgr.EatOkashi_DecideFlag == 0)
        {
            for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
            {
                if (girlLikeCompo_database.girllike_composet[i].set_ID == girl1_status.OkashiQuest_ID)
                {
                    girlLikeCompo_database.girllike_composet[i].clearFlag = true; //クリアした

                    OkashiQuest_Name = girlLikeCompo_database.girllike_composet[i].spquest_name1;                   
                    GameMgr.NextQuestID = girlLikeCompo_database.girllike_composet[i].next_ID;
                    //girl1_status.OkashiQuest_Name = OkashiQuest_Name;
                }
            }
        }
        else
        {
            _ID = GameMgr.NowEatOkashiID;

            if (database.items[_ID].itemType_sub.ToString() == "Coffee" || database.items[_ID].itemType_sub.ToString() == "Coffee_Mat" ||
                   database.items[_ID].itemType_sub.ToString() == "Juice" || database.items[_ID].itemType_sub.ToString() == "Tea" ||
                   database.items[_ID].itemType_sub.ToString() == "Tea_Mat" || database.items[_ID].itemType_sub.ToString() == "Tea_Potion" || 
                   database.items[_ID].itemType_sub.ToString() == "Soda")
            {
                OkashiQuest_Name = GameMgr.NowEatOkashiName + "がのみたい！";
            }
            else
            {
                OkashiQuest_Name = GameMgr.NowEatOkashiName + "が食べたい！";
            }
            
        }

        //クエスト開始時のタイトル名検索
        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
        {
            if (girlLikeCompo_database.girllike_composet[i].set_ID == girl1_status.OkashiQuest_ID)
            {
                GameMgr.MainQuestTitleName = girlLikeCompo_database.girllike_composet[i].spquest_name1;
                OkashiQuest_sprite = girlLikeCompo_database.girllike_composet[i].itemIcon_sprite;
            }
        }
    }

    //Girl1_statusからも読み出し
    public void RedrawQuestName()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //メイン画面に表示する、現在のクエスト
        questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();

        QuestNameFind();

        //現在のクエストネーム更新。
        questname.text = OkashiQuest_Name;
    }
    
}
