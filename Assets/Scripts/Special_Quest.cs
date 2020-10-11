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

    public int special_kaisu; //一回のクエストで、3回まで挑戦できる。現在は、回数制限は未使用。
    public int special_kaisu_max;
    public int[,] special_score_record; //そのときの、点数も記録。一つの列に3個点数を保持。
    public int spquest_set_num;

    private int i;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        special_kaisu = 0;
        special_kaisu_max = 1;
        special_score_record = new int[GameMgr.GirlLoveEvent_stage1.Length, special_kaisu_max];

        for (i = 0; i < special_score_record.GetLength(0); i++)
        {
            special_score_record[i, 0] = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSpecialOkashi(int _num, int _status)
    {
      
        spquest_set_num = _num;
        GameMgr.OkashiQuest_Num = _num;
        
        if(_status == 0)
        {
            special_kaisu = 0; //0回にリセット
            girl1_status.OkashiNew_Status = 0;
            GameMgr.QuestClearflag = false;
        }
        else //ロードから再開された場合の処理
        {

        }

        switch (_num)
        {
            case 0: //オリジナルクッキーを食べたい

                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。               
                girl1_status.OkashiQuest_ID = 1000;
                QuestNameFind();
                girl1_status.ResetHukidashi();
                
                break;

            case 10: //ラスク食べたい

                girl1_status.OkashiQuest_ID = 1100;
                QuestNameFind();
                girl1_status.ResetHukidashi();

                break;

            case 20: //クレープ食べたい

                girl1_status.OkashiQuest_ID = 1200;
                QuestNameFind();
                girl1_status.ResetHukidashi();
                break;

            case 30: //シュークリーム食べたい

                girl1_status.OkashiQuest_ID = 1300;
                QuestNameFind();
                girl1_status.ResetHukidashi();
                break;

            case 40: //ドーナツ食べたい

                girl1_status.OkashiQuest_ID = 1400;
                QuestNameFind();
                girl1_status.ResetHukidashi();
                break;

            case 50: //ステージ１ラスト　コンテスト開始

                girl1_status.OkashiQuest_ID = 1500;
                QuestNameFind();
                girl1_status.ResetHukidashi();
                break;

            default:
                break;
        }
        
    }

    public void SetNextRandomOkashi(int _num)
    {
        switch (_num)
        {
            case 0:

                break;

            case 1: //ぶどうクッキーを食べた

                //OFF
                girlLikeCompo_database.SetGirlSetFlag(20, 0); //ぶどうクッキー
                girlLikeCompo_database.SetGirlSetFlag(30, 0); //ラスク

                //ON
                girlLikeCompo_database.SetGirlSetFlag(40, 1); //たまごの入ったクッキー
                break;

            default:
                break;
        }
    }

    void QuestNameFind()
    {
        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
        {
            if (girlLikeCompo_database.girllike_composet[i].set_ID == girl1_status.OkashiQuest_ID)
            {
                girlLikeCompo_database.girllike_composet[i].clearFlag = true; //クリアした

                girl1_status.OkashiQuest_Name = girlLikeCompo_database.girllike_composet[i].spquest_name1;
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
        questname.text = girl1_status.OkashiQuest_Name;
    }
}
