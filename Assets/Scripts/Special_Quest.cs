using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//スペシャルお菓子　吹き出しクエストのON/OFFを管理

public class Special_Quest : SingletonMonoBehaviour<Special_Quest>
{

    private Girl1_status girl1_status;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    // Use this for initialization
    void Start () {

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSpecialOkashi(int _num)
    {
        switch(_num)
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
                girl1_status.OkashiQuest_ID = 1010;
                girl1_status.ResetHukidashi();

                break;

            case 2:

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
