using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl1_status : SingletonMonoBehaviour<Girl1_status>
{
    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    private float timeLeft;
    private int timeCount;
    private float timeOut;

    //女の子の好み値。この値と、選択したアイテムの数値を比較し、近いほど得点があがる。
    public int girl1_Quality;

    public int girl1_Rich;
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

    public int rich_score_final;
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

    private int i, count;
    private int index;

    //ランダムで変化する、女の子が今食べたいお菓子のテーブル
    public List<string> girl1_hungryInfo = new List<string>();
    public List<int> girl1_hungryScore = new List<int>();

    // Use this for initialization

    void Start () {

        DontDestroyOnLoad(this);

        girl_comment_flag = false;
        girl_comment_endflag = false;

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //秒計算。　
        timeLeft = 1.0f;
        timeCount = 0; //1秒タイマー

        //この時間ごとに、女の子は、お菓子を欲しがり始める。
        timeOut = 5.0f; 

        //アイテムそれぞれに、女の子の好き度を表す、基礎パラメータがある。エクセルのアイテムデータベースにgirl1_likeで登録している。

        //品質値に対する評価。
        girl1_Quality = 50;

        //女の子の好み。初期化。甘さ・苦さ・酸味は近いものほど高得点。
        girl1_Rich = 30;
        girl1_Sweat = 50;
        girl1_Sour = 20;
        girl1_Bitter = 30;

        //食感は、100に近いほど高得点。クッキーならサクサク感が強いほど得点が高い。また、ガールの最低限の好みに達していなければ、得点は低い。
        girl1_Crispy = 10;
        girl1_Fluffy = 10;
        girl1_Smooth = 10;
        girl1_Hardness = 10;

        //未使用
        girl1_Chewy = 0;
        girl1_Jiggly = 0;


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

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();
    }

    // Update is called once per frame
    void Update () {

        //一定時間たつと、女の子はお腹がへって、お菓子を欲しがる。
        timeLeft -= Time.deltaTime;
        timeOut -= Time.deltaTime;

        if (timeLeft <= 0.0)
        {
            timeLeft = 1.0f;
            timeCount++;

            //ここに処理
            //Debug.Log("経過時間: " + timeCount + " 秒");
        }

        if (timeOut <= 0.0)
        {
            timeOut = 5.0f;

            // Do anything
            Debug.Log("お腹が空いたよ～～");

            //女の子の食べたいものが、ランダムで切り替わるテーブル            
            index = Random.Range(1, girl1_hungryScore.Count);
            Debug.Log("ランダムで食べたいもの一つ決定: " + index + " " + slotnamedatabase.slotname_lists[index].slot_Hyouki_1);
            Debug.Log(slotnamedatabase.slotname_lists[index].slot_Hyouki_1 + " のお菓子が食べたいよ～");

            //まず全ての値を0に初期化
            for(i = 0; i < girl1_hungryScore.Count; i++ )
            {
                girl1_hungryScore[i] = 0;
            }            

            //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
            girl1_hungryScore[index]++;

            //表示用吹き出しを生成
        }
    }

    void InitializeItemSlotDicts()
    {

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            girl1_hungryInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            girl1_hungryScore.Add(0);
        }
    }
}
