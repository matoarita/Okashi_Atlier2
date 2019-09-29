using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl1_status : SingletonMonoBehaviour<Girl1_status>
{

    //女の子の好み値。この値と、選択したアイテムの数値を比較し、近いほど得点があがる。
    public int girl1_Quality;

    public int girl1_Sweat;
    public int girl1_Sour;
    public int girl1_Bitter;

    public int girl1_Crispy;
    public int girl1_Fluffy;
    public int girl1_Smooth;
    public int girl1_Hardness;
    public int girl1_Chewy;
    public int girl1_Jiggly;

    public int girl1_Powdery;
    public int girl1_Oily;
    public int girl1_Watery;



    public string girl1_Subtype1;
    public string girl1_Subtype2;
    public int girl1_Subtype1_p;
    public int girl1_Subtype2_p;

    public string girl1_Topping1; 
    public string girl1_Topping2;

    public int girl1_Love_exp; //女の子の好感度のこと。ゲーム中に、お菓子をあげることで変動する。総量。
    public int girl1_Getlove_exp; //お菓子の判定の際、取得した好感度。Love_expにこの値を加算していく。

    public bool girl_comment_flag; //女の子が感想をいうときに、宴をON/OFFにするフラグ
    public bool girl_comment_endflag; //感想を全て言い終えたフラグ


    //採点結果　宴と共有する用のパラメータ。採点は、GirlEat_Judgeで行っている。
    public int girl_final_kettei_item;
    public int itemLike_score_final;

    public int quality_score_final;
    public int sweat_score_final;
    public int bitter_score_final;
    public int sour_score_final;

    public int crispy_score_final;
    public int fluffy_score_final;
    public int smooth_score_final;
    public int hardness_score_final;
    public int jiggly_score_final;
    public int chewy_score_final;

    public int subtype1_score_final;
    public int subtype2_score_final;

    public int total_score_final;

    // Use this for initialization

    void Start () {

        DontDestroyOnLoad(this);

        girl_comment_flag = false;
        girl_comment_endflag = false;

        //アイテムそれぞれに、女の子の好き度を表す、基礎パラメータがある。エクセルのアイテムデータベースにgirl1_likeで登録している。


        //女の子の好み。初期化。甘さ・苦さ・酸味は近いものほど高得点。
        girl1_Sweat = 50;
        girl1_Sour = 20;
        girl1_Bitter = 30;

        //食感は、100に近いほど高得点。クッキーならサクサク感が強いほど得点が高い。また、ガールの最低限の好みに達していなければ、得点は低い。
        girl1_Crispy = 10;
        girl1_Fluffy = 10;
        girl1_Smooth = 10;
        girl1_Hardness = 10;
        girl1_Chewy = 10;
        girl1_Jiggly = 10;


        //女の子が好きなお菓子のタイプ。クッキー好きとか、ケーキ好きとか。_pは、どれぐらい好きかを数値化。
        girl1_Subtype1 = "Cookie";
        girl1_Subtype1_p = 10;

        girl1_Subtype2 = "Cake";
        girl1_Subtype2_p = 20;

        //女の子が好きなトッピングの値。アイテムのトッピング欄に、該当するアイテムNoがあった場合、得点がプラスされる。
        girl1_Topping1 = "nuts";
        girl1_Topping2 = "grape";

        girl1_Love_exp = 0;
        girl1_Getlove_exp = 0;
        
    }

    // Update is called once per frame
    void Update () {
		
	}


}
