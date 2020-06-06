using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//スペシャルお菓子　吹き出しクエストのON/OFFを管理

public class Special_Quest : SingletonMonoBehaviour<Special_Quest>
{

    private Girl1_status girl1_status;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    public int special_kaisu; //一回のクエストで、3回まで挑戦できる。
    public int special_kaisu_max;
    public int[,] special_score_record; //そのときの、点数も記録。一つの列に3個点数を保持。
    public int spquest_set_num;

    private int i;

    // Use this for initialization
    void Start () {

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        special_kaisu = 0;
        special_kaisu_max = 1;
        special_score_record = new int[30, special_kaisu_max];

        for (i = 0; i < special_score_record.GetLength(0); i++)
        {
            special_score_record[i, 0] = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSpecialOkashi(int _num)
    {
        special_kaisu = 0; //0回にリセット
        spquest_set_num = _num;
        GameMgr.OkashiQuest_Num = _num;

        switch (_num)
        {
            case 0: //オリジナルクッキーを食べたい

                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                girl1_status.OkashiNew_Status = 0;
                girl1_status.OkashiQuest_ID = 1000;
                girl1_status.ResetHukidashi();
                
                break;

            case 1: //ラスク食べたい

                //ランダムで選ばられるセットのON/OFF
                //クッキー系をOFF
                /*girlLikeCompo_database.SetGirlSetFlag(0, 0);
                girlLikeCompo_database.SetGirlSetFlag(1, 0);            

                //ON
                girlLikeCompo_database.SetGirlSetFlag(20, 1); //ぶどうクッキーON
                girlLikeCompo_database.SetGirlSetFlag(30, 1); //ラスクON*/
                //** ここまで **//

                girl1_status.OkashiNew_Status = 0;
                girl1_status.OkashiQuest_ID = 1100;
                girl1_status.ResetHukidashi();

                break;

            case 2: //クレープ食べたい

                girl1_status.OkashiNew_Status = 0;
                girl1_status.OkashiQuest_ID = 1200;
                girl1_status.ResetHukidashi();
                break;

            case 3: //シュークリーム食べたい

                girl1_status.OkashiNew_Status = 0;
                girl1_status.OkashiQuest_ID = 1300;
                girl1_status.ResetHukidashi();
                break;

            case 4: //なんか食べたい

                girl1_status.OkashiNew_Status = 0;
                girl1_status.OkashiQuest_ID = 1400;
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
}
