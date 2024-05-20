using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Contest_Judge : MonoBehaviour {

    private GameObject canvas;

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private SoundController sc;
    private BGM sceneBGM;

    private Debug_Panel debug_panel;
    private Text debug_taste_resultText;

    private GirlEat_Judge girlEat_judge;

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    //コンテストの判定セット
    private ContestSetDataBase contestSet_database;

    private Girl1_status girl1_status;

    private GameObject text_area;
    private Text _windowtext;

    private int i, count, sum;
    private int random;

    private int kettei_item1; //女の子にあげるアイテムの、アイテムリスト番号。
    private int _toggle_type1; //店売りか、オリジナルのアイテムなのかの判定用

    private string itemName;
    private string item_subType;
    private string item_subTypeB;
    private int compNum;

    private int kettei_itemID;
    private int kettei_itemType;

    private string contest_Name;

    private bool judge_flag;
    private int judge_Type;


    public int[] total_score;
    private float _temp_score;
    private int[] before_tastescore;

    private int rnd, rnd2;
    private int set_id;

    //女の子の好み組み合わせセットのデータ
    private int _compID;
    private int set1_ID;
    private int set2_ID;
    private int set3_ID;
    private int Set_Count;

    private List<int> set_ID = new List<int>();

    // Use this for initialization
    void Start () {

        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");       

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //コンテストの判定セットの取得
        contestSet_database = ContestSetDataBase.Instance.GetComponent<ContestSetDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _windowtext = text_area.GetComponentInChildren<Text>();

        //女の子、お菓子の判定処理オブジェクトの取得
        girlEat_judge = GirlEat_Judge.Instance.GetComponent<GirlEat_Judge>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //要素数の初期化
        total_score = new int[girl1_status.youso_count];
        before_tastescore = new int[girl1_status.youso_count];

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //判定前に作ったお菓子のセッティング
    public void Contest_Judge_Start()
    {
        Debug.Log("コンテスト判定ON");
        Debug.Log("コンテスト場所: " + GameMgr.ContestSelectNum);

        //判定するお菓子を決定

        if (pitemlist.player_extremepanel_itemlist.Count > 0)
        {
            kettei_itemID = 0;
            kettei_itemType = 2;
        }
        else //エクストリームパネルにお菓子が入っていない時。デバッグ用。
        {
            //お試し　店売りねこクッキー
            kettei_itemID = database.SearchItemIDString("neko_cookie");
            kettei_itemType = 0;
        }

        //提出されたお菓子の固有アイテム名・タイプサブを出し、判定用DBから一致するものを探す。
        if (kettei_itemType == 0)
        {
            itemName = database.items[kettei_itemID].itemName;
            item_subType = database.items[kettei_itemID].itemType_sub.ToString();
            item_subTypeB = database.items[kettei_itemID].itemType_subB;

            //表示用アイテム名
            GameMgr.contest_okashiSlotName = "";
            GameMgr.contest_okashiName = database.items[kettei_itemID].itemName;
            GameMgr.contest_okashiNameHyouji = database.items[kettei_itemID].itemNameHyouji;
            GameMgr.contest_okashiSubType = database.items[kettei_itemID].itemType_sub.ToString();
            GameMgr.contest_okashiID = database.items[kettei_itemID].itemID;

            GameMgr.contest_okashi_ItemData = database.items[kettei_itemID];
            Debug.Log("コンテストお菓子　itemType:0 セッティングOK");
        }
        else if (kettei_itemType == 1)
        {
            itemName = pitemlist.player_originalitemlist[kettei_itemID].itemName;
            item_subType = pitemlist.player_originalitemlist[kettei_itemID].itemType_sub.ToString();
            item_subTypeB = pitemlist.player_originalitemlist[kettei_itemID].itemType_subB;

            //表示用アイテム名
            GameMgr.contest_okashiSlotName = pitemlist.player_originalitemlist[kettei_itemID].item_SlotName;
            GameMgr.contest_okashiName = pitemlist.player_originalitemlist[kettei_itemID].itemName;
            GameMgr.contest_okashiNameHyouji = pitemlist.player_originalitemlist[kettei_itemID].itemNameHyouji;
            GameMgr.contest_okashiSubType = pitemlist.player_originalitemlist[kettei_itemID].itemType_sub.ToString();
            GameMgr.contest_okashiID = pitemlist.player_originalitemlist[kettei_itemID].itemID;

            GameMgr.contest_okashi_ItemData = pitemlist.player_originalitemlist[kettei_itemID];
            Debug.Log("コンテストお菓子　itemType:1 セッティングOK");
        }
        else if (kettei_itemType == 2)
        {
            itemName = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemName;
            item_subType = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemType_sub.ToString();
            item_subTypeB = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemType_subB;

            //表示用アイテム名
            GameMgr.contest_okashiSlotName = pitemlist.player_extremepanel_itemlist[kettei_itemID].item_SlotName;
            GameMgr.contest_okashiName = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemName;
            GameMgr.contest_okashiNameHyouji = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemNameHyouji;
            GameMgr.contest_okashiSubType = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemType_sub.ToString();
            GameMgr.contest_okashiID = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemID;

            GameMgr.contest_okashi_ItemData = pitemlist.player_extremepanel_itemlist[kettei_itemID];
            Debug.Log("コンテストお菓子　itemType:2 セッティングOK");
        }

        Debug.Log("提出したお菓子: " + GameMgr.contest_okashiNameHyouji);


        //***お菓子の判定処理　***
        //左二つが判定するお菓子
        //3番目の番号は、girlLikeSetのcomp_Num番号。
        //GameMgr.ContestSelectNumは、コンテストのシーン番号。Contest_DB_list_Type以上のcomp_Numを判定として使用。
        //***

        judge_flag = false;
        GameMgr.contest_Disqualification = false;
        //judge_Type = 0; //基本審査員3人で対応。judge_Typeは、どのコンテストかを指定する。

        i = 0;
        while (i < contestSet_database.contest_set.Count)
        {
            if (contestSet_database.contest_set[i].girlLike_compNum >= GameMgr.Contest_DB_list_Type)
            {
                if (contestSet_database.contest_set[i].girlLike_itemName != "Non") //固有名がはいってる場合は、固有名をみる。
                {
                    //固有のアイテム名と一致するかどうかを判定。
                    if (contestSet_database.contest_set[i].girlLike_itemName == itemName)
                    {
                        //一致した場合の番号を入れる。
                        compNum = contestSet_database.contest_set[i].girlLike_compNum;
                        judge_flag = true;
                        Debug.Log("判定番号: " + compNum);
                        break;
                    }
                }
                else//固有名が入ってない場合は、サブタイプをみる。
                {
                    if (contestSet_database.contest_set[i].girlLike_itemSubtype == item_subType && contestSet_database.contest_set[i].girlLike_itemSubtype != "Non")
                    {
                        compNum = contestSet_database.contest_set[i].girlLike_compNum;
                        judge_flag = true;
                        Debug.Log("判定番号: " + compNum);
                        break;
                    }
                    else if (contestSet_database.contest_set[i].girlLike_itemSubtype == item_subTypeB && contestSet_database.contest_set[i].girlLike_itemSubtype != "Non")
                    {
                        compNum = contestSet_database.contest_set[i].girlLike_compNum;
                        judge_flag = true;
                        Debug.Log("判定番号: " + compNum);
                        break;
                    }
                }

                //~そのシートの検索EndPointまで検索する。Excel上にフラグがある。
                if (contestSet_database.contest_set[i].girlLike__search_endflag == 1)
                {
                    judge_flag = false;
                    break;
                }
            }

            i++;
        }

        if (!judge_flag)
        {
            //もし、審査員DB上に登録されていないお菓子を渡した場合。課題のお菓子でないので失格。
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                GameMgr.contest_Score[i] = 0;
            }

            GameMgr.contest_TotalScore = 0;
            GameMgr.contest_Disqualification = true;
            _windowtext.text = "課題のお菓子ではないので、失格！";
            Debug.Log("課題のお菓子ではないので、失格！");
        }
        else
        {
            //審査員判定
            Contest_Judge_method(kettei_itemID, kettei_itemType, compNum);
        }
    }

    //選んだアイテムを審査委員が判定するメソッド
    public void Contest_Judge_method(int value1, int value2, int judge_num) //judge_typeは、コンテストを指定
    {
        //一度、決定したアイテムのリスト番号と、タイプを取得
        kettei_item1 = value1;
        _toggle_type1 = value2;

        //** 判定用に、コンテストの好み値(GirlLikeSet)をセッティング
        set1_ID = judge_num; //審査員１の好み
        set2_ID = judge_num + 1; //審査員２の好み
        set3_ID = judge_num + 2; //審査員３の好み

        set_ID.Clear();

        //set_idにリストの番号をセット
        if (set1_ID != 9999)
        {
            set_ID.Add(set1_ID);
        }
        if (set2_ID != 9999)
        {
            set_ID.Add(set2_ID);
        }
        if (set3_ID != 9999)
        {
            set_ID.Add(set3_ID);
        }

        //さきほどのset_IDをもとに、好みの値を決定する。このとき、コンテストごとの審査員の好みの判定補正もかける。
        for (count = 0; count < set_ID.Count; count++)
        {

            girl1_status.InitializeStageContestJudgeSet(set_ID[count], count); //compNum, セットする配列番号　の順　セットの番号は現状３つまで設定可
                                                                               //Debug.Log("set_ID: " + count + " : " + set_ID[count]);
        }

        Set_Count = set_ID.Count;


        //**

        //お菓子の味判定処理
        //
        judge_result_contest(); //判定し、トータルのスコアが算出される。

        if (!GameMgr.Contest_Clear_Failed)
        {
            sum = 0;
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                GameMgr.contest_Score[i] = total_score[i];
                sum += total_score[i];
            }

            GameMgr.contest_TotalScore = sum / GameMgr.contest_Score.Length;
            if (GameMgr.contest_TotalScore < 0)
            {
                GameMgr.contest_TotalScore = 0;
            }
            Debug.Log("総合得点：" + GameMgr.contest_TotalScore + "点");

            _windowtext.text = "審査員１　点数：" + total_score[0] + "点" + "\n" +
                "審査員２　点数：" + total_score[1] + "点" + "\n" +
                "審査員３　点数：" + total_score[2] + "点";
        }
        else
        {
            _windowtext.text = "特殊点に届かなかった..。不合格！";
        }

        //先に算出しておいて、あとで、審査員一人一人のコメント＋点数を演出して出す。宴へ戻る。
    }


    void judge_result_contest()
    {

        count = 0;

        while (count < Set_Count) //セットの組み合わせ=審査員の数だけ判定。まずかった場合は、単純にスコアが下がる補正がかかるようにフラグをたてる。
        {
            //パラメータ初期化し、判定処理
            set_id = count;
            

            //次に味の判定処理。判定後、採点の数値がかえってくる。

            Debug.Log("#####  審査員: " + set_id + "#####");
            total_score[count] = girlEat_judge.Judge_Score_Return(kettei_item1, _toggle_type1, 1, count); //点数の判定。3番目の0~1の数字は、女の子のお菓子の判定か、コンテストでの判定かのタイプ分け

            if(total_score[count] < 0)
            {
                total_score[count] = 0;
            }
            GameMgr.contest_Taste_Score[count] = girlEat_judge.shokukan_score;
            GameMgr.contest_Beauty_Score[count] = girlEat_judge.beauty_score;           
            GameMgr.contest_Sweat_Comment[count] = girlEat_judge._contest_sweat_kansou;
            GameMgr.contest_Bitter_Comment[count] = girlEat_judge._contest_bitter_kansou;
            GameMgr.contest_Sour_Comment[count] = girlEat_judge._contest_sour_kansou;

            count++;
            
        }

        //
        //各コンテスト審査員ごとの判定分け　補正がけ
        //
        switch(GameMgr.Contest_Name)
        {
            case "First_Contest":

                //審査員３　じいさんだけ、食感の補正　食感がよいほど、得点が上がりやすくなる。その代わり見た目の点数が一切入らない。
                Contest_ShokukanHosei_1();

                //200点を上限に100点に正規化する。
                ScoreNormalized(200);
                Debug.Log("各点数にコンテスト補正で下げる：" + "*0.5");
                Debug.Log("### ###");
                break;

                
            case "Or_Contest_010":　//クッキー初級コンテスト

                //審査員３　じいさんだけ、食感の補正
                Contest_ShokukanHosei_1();               

                //入れた数値を上限に100点に正規化する。
                ScoreNormalized(150);
                Debug.Log("各点数にコンテスト補正で下げる：" + "*0.5");
                Debug.Log("### ###");
                break;


            default:

                //審査員３　じいさんだけ、食感の補正
                Contest_ShokukanHosei_1();

                //入れた数値を上限に100点に正規化する。
                ScoreNormalized(200);
                Debug.Log("各点数にコンテスト補正で下げる：" + "*0.5");
                Debug.Log("### ###");
                break;
        }

        

        //さらに提出が遅れた場合減点
        if (GameMgr.contest_LimitTimeOver_DegScore_flag)
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = total_score[i] - Mathf.Abs(PlayerStatus.player_contest_LimitTime)*2; //遅れた時間分だけ減点
            }
            Debug.Log("提出時間が遅れたので、減点: -" + Mathf.Abs(PlayerStatus.player_contest_LimitTime) * 2);
        }

        Debug.Log("審査員１　点数：" + total_score[0] + "点");
        Debug.Log("審査員２　点数：" + total_score[1] + "点");
        Debug.Log("審査員３　点数：" + total_score[2] + "点");

        Debug.Log("### ###");
        //Debug.Log("審査員２　見た目：" + GameMgr.contest_Beauty_Score[1] + "点");
        

    }

    void Contest_ShokukanHosei_1()
    {
        before_tastescore[2] = GameMgr.contest_Taste_Score[2];
        if (GameMgr.contest_Taste_Score[2] >= 0 && GameMgr.contest_Taste_Score[2] < 30)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 1.0f);
        }
        else if (GameMgr.contest_Taste_Score[2] >= 30 && GameMgr.contest_Taste_Score[2] < 80)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 1.2f);
        }
        else if (GameMgr.contest_Taste_Score[2] >= 80 && GameMgr.contest_Taste_Score[2] < 150)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 1.5f);
        }
        else if (GameMgr.contest_Taste_Score[2] >= 150 && GameMgr.contest_Taste_Score[2] < 180)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 3.0f);
        }
        else if (GameMgr.contest_Taste_Score[2] >= 180)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 4.0f);
        }
        else if (GameMgr.contest_Taste_Score[2] < 0)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 0.7f);
        }

        total_score[2] = total_score[2] + (GameMgr.contest_Taste_Score[2] - before_tastescore[2]);

        Debug.Log("審査員３　じいさんは食感のみ、得点にバフがかかる。下の食感の値が最終の食感点数");
        Debug.Log("審査員３　食感補正前：" + before_tastescore[2] + "点");
        Debug.Log("審査員３　食感補正後：" + GameMgr.contest_Taste_Score[2] + "点");
    }

    //点数を、入れた値を上限にして100点に正規化する。
    void ScoreNormalized(int _max)
    {
        //200を入れた場合、点数を200点を上限にし、100点に正規化する処理　つまり、ヒカリの点数の２分の一になるということ
        for (i = 0; i < GameMgr.contest_Score.Length; i++)
        {
            _temp_score = SujiMap(total_score[i], 0, _max, 0, 100);
            total_score[i] = (int)_temp_score;
        }
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
