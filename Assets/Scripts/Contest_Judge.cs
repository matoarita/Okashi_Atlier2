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

    private ItemCardEffectDataBase itemCardEffect_database;

    private Girl1_status girl1_status;

    private GameObject text_area;
    private Text _windowtext;

    private int i, count, sum;
    private int random;

    private int kettei_item1; //女の子にあげるアイテムの、アイテムリスト番号。
    private int _toggle_type1; //店売りか、オリジナルのアイテムなのかの判定用

    private int itemID;
    private string itemName;
    private string item_subType;
    private string item_subTypeB;
    private int compNum;
    private int _baseSetjudge_num;
    private int _baseMagic;

    private string[] _baseMS;
    private int[] _baseMSvalue;

    private int kettei_itemID;
    private int kettei_itemType;

    private string contest_Name;

    private bool judge_flag;
    private int judge_Type;

    private float contest_bairitsu_hosei;

    public int[] total_score;
    private float _temp_score;
    private int[] before_tastescore;
    private int[] before_sweatscore;
    private int[] before_bitterscore;
    private int[] before_sourscore;
    private int[] before_beautyscore;
    private string _basemagicslot_Name;

    private int rnd, rnd2;
    private int set_id;

    private string _shokukan_kansou;
    private string _beauty_kansou;
    private string _spscore_kansou;

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

        //魔法エフェクトの計算データベース
        itemCardEffect_database = ItemCardEffectDataBase.Instance.GetComponent<ItemCardEffectDataBase>();

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
        before_sweatscore = new int[girl1_status.youso_count];
        before_bitterscore = new int[girl1_status.youso_count];
        before_sourscore = new int[girl1_status.youso_count];
        before_beautyscore = new int[girl1_status.youso_count];
        _baseMS = new string[database.items[0].item_MagicSlot.Length];
        _baseMSvalue = new int[database.items[0].item_MagicSlotValue.Length];
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
            itemID = database.items[kettei_itemID].itemID;
            itemName = database.items[kettei_itemID].itemName;
            item_subType = database.items[kettei_itemID].itemType_sub.ToString();
            item_subTypeB = database.items[kettei_itemID].itemType_subB;
            _baseSetjudge_num = database.items[kettei_itemID].SetJudge_Num;
            _baseMagic = database.items[kettei_itemID].Magic;

            for (i = 0; i < database.items[kettei_itemID].item_MagicSlot.Length; i++)
            {
                _baseMS[i] = database.items[kettei_itemID].item_MagicSlot[i].ToString();
                _baseMSvalue[i] = database.items[kettei_itemID].item_MagicSlotValue[i];
            }

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
            itemID = pitemlist.player_originalitemlist[kettei_itemID].itemID;
            itemName = pitemlist.player_originalitemlist[kettei_itemID].itemName;
            item_subType = pitemlist.player_originalitemlist[kettei_itemID].itemType_sub.ToString();
            item_subTypeB = pitemlist.player_originalitemlist[kettei_itemID].itemType_subB;
            _baseSetjudge_num = pitemlist.player_originalitemlist[kettei_itemID].SetJudge_Num;
            _baseMagic = pitemlist.player_originalitemlist[kettei_itemID].Magic;

            for (i = 0; i < pitemlist.player_originalitemlist[kettei_itemID].item_MagicSlot.Length; i++)
            {
                _baseMS[i] = pitemlist.player_originalitemlist[kettei_itemID].item_MagicSlot[i].ToString();
                _baseMSvalue[i] = pitemlist.player_originalitemlist[kettei_itemID].item_MagicSlotValue[i];
            }

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
            itemID = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemID;
            itemName = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemName;
            item_subType = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemType_sub.ToString();
            item_subTypeB = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemType_subB;
            _baseSetjudge_num = pitemlist.player_extremepanel_itemlist[kettei_itemID].SetJudge_Num;
            _baseMagic = pitemlist.player_extremepanel_itemlist[kettei_itemID].Magic;

            for (i = 0; i < pitemlist.player_extremepanel_itemlist[kettei_itemID].item_MagicSlot.Length; i++)
            {
                _baseMS[i] = pitemlist.player_extremepanel_itemlist[kettei_itemID].item_MagicSlot[i].ToString();
                _baseMSvalue[i] = pitemlist.player_extremepanel_itemlist[kettei_itemID].item_MagicSlotValue[i];
                Debug.Log("_baseMS[i]: " + _baseMS[i] + " " + "パラメータ: " + _baseMSvalue[i]);
            }

            //表示用アイテム名
            GameMgr.contest_okashiSlotName = pitemlist.player_extremepanel_itemlist[kettei_itemID].item_SlotName;
            GameMgr.contest_okashiName = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemName;
            GameMgr.contest_okashiNameHyouji = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemNameHyouji;
            GameMgr.contest_okashiSubType = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemType_sub.ToString();
            GameMgr.contest_okashiID = pitemlist.player_extremepanel_itemlist[kettei_itemID].itemID;            

            GameMgr.contest_okashi_ItemData = pitemlist.player_extremepanel_itemlist[kettei_itemID];
            Debug.Log("コンテストお菓子　itemType:2 セッティングOK");
        }

        //おかしにかかっている演出魔法を見る
        _basemagicslot_Name = "Non";
        itemCardEffect_database.MagicEffect_SlotKeisan(_baseMS, _baseMSvalue, itemID, 0);
        //おかしにかかってる演出魔法
        _basemagicslot_Name = itemCardEffect_database._basemagicslot_Name;

        Debug.Log("提出したお菓子: " + GameMgr.contest_okashiNameHyouji);
        Debug.Log("かかっている演出魔法: " + _basemagicslot_Name);

        //***お菓子の判定処理　***
        //左二つが判定するお菓子
        //3番目の番号は、girlLikeSetのcomp_Num番号。
        //GameMgr.ContestSelectNumは、コンテストのシーン番号。Contest_DB_list_Type以上のcomp_Numを判定として使用。
        //***

        judge_flag = false;
        GameMgr.contest_Disqualification = false;
        GameMgr.contest_Disqualification2 = false;
        GameMgr.contest_last_Disqualification = false;
        //judge_Type = 0; //基本審査員3人で対応。judge_Typeは、どのコンテストかを指定する。

        if (GameMgr.Contest_JudgeType == 0) //1のときは、女の子の好み判定を使用する　自由課題など)
        {
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
        }
        else if(GameMgr.Contest_JudgeType == 1)
        {
            judge_flag = true;
            Debug.Log("判定番号: " + _baseSetjudge_num);

            //コンテストによって、ジャンルの指定がある場合は、ここでジャッジする。
            Contest_Score_JudgeHoseiLibrary(10);
        }

        if (!judge_flag)
        {
            //もし、審査員DB上に登録されていないお菓子を渡した場合。課題のお菓子でないので失格。
            //あるいは、女の子好みを使用する場合、Contest_Score_JudgeHoseiLibraryのstatus=10で課題のお菓子を指定し、その指定にないものは失格。
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                GameMgr.contest_Score[i] = 0;
            }

            GameMgr.contest_TotalScore = 0;
            GameMgr.contest_Disqualification = true;
            GameMgr.contest_last_Disqualification = true;
            _windowtext.text = "課題のお菓子ではないので、失格！";
            Debug.Log("課題のお菓子ではないので、失格！");
        }
        else
        {
            if (GameMgr.Contest_JudgeType == 0) //1のときは、女の子の好み判定を使用する　自由課題など)
            {
                //審査員判定
                Contest_Judge_method(kettei_itemID, kettei_itemType, compNum, 0);
            }
            else if (GameMgr.Contest_JudgeType == 1)
            {
                //審査員判定
                Contest_Judge_method(kettei_itemID, kettei_itemType, _baseSetjudge_num, 1);
            }
        }
    }

    //選んだアイテムを審査委員が判定するメソッド
    public void Contest_Judge_method(int value1, int value2, int judge_num, int _mstatus) //judge_typeは、コンテストを指定
    {
        if (_mstatus == 0) //ContestSetの判定値を使う
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
        }
        else if (_mstatus == 1) //女の子の判定値を使う
        {
            //一度、決定したアイテムのリスト番号と、タイプを取得
            kettei_item1 = value1;
            _toggle_type1 = value2;

            set_ID.Clear();
            set_ID.Add(_baseSetjudge_num);
            set_ID.Add(_baseSetjudge_num);
            set_ID.Add(_baseSetjudge_num);

            //さきほどのset_IDをもとに、好みの値を決定する。このとき、コンテストごとの審査員の好みの判定補正もかける。
            for (count = 0; count < set_ID.Count; count++)
            {
                girl1_status.InitializeStageGirlHungrySet(set_ID[count], count, 1); //compNum, セットする配列番号　の順　 3番目の数字は、コンテストで女の子好みを使用する場合の設定
            }

            // 各判定用パラメータに、さらにコンテストごとに補正をかける。
            GameMgr.contest_SPJudgeCommentNum = 0; //コンテストコメント番号リセット
            Contest_Score_JudgeHoseiLibrary(0);

            Set_Count = set_ID.Count;
        }


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
                "審査員３　点数：" + total_score[2] + "点" + "\n" + 
                "総合得点：" + GameMgr.contest_TotalScore + "点";
        }
        else
        {
            //現在はこっちは使用せず。判定のほうで減点するようにしている。
            sum = 0;
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = Random.Range(3, 20);
                GameMgr.contest_Score[i] = total_score[i];
                sum += total_score[i];
            }

            GameMgr.contest_TotalScore = sum / GameMgr.contest_Score.Length;
            if (GameMgr.contest_TotalScore < 0)
            {
                GameMgr.contest_TotalScore = 0;
            }

            _windowtext.text = "特殊点に届かなかった..。不合格！";
            GameMgr.contest_Disqualification2 = true; 
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
            GameMgr.contest_Sweat_Score[count] = girlEat_judge.sweat_score;
            GameMgr.contest_Bitter_Score[count] = girlEat_judge.bitter_score;
            GameMgr.contest_Sour_Score[count] = girlEat_judge.sour_score;
            GameMgr.contest_Beauty_Score[count] = girlEat_judge.beauty_score;           
            GameMgr.contest_Sweat_Comment[count] = girlEat_judge._contest_sweat_kansou;
            GameMgr.contest_Bitter_Comment[count] = girlEat_judge._contest_bitter_kansou;
            GameMgr.contest_Sour_Comment[count] = girlEat_judge._contest_sour_kansou;
            GameMgr.contest_Sp_Score1[count] = girlEat_judge.spscore1_score;
            GameMgr.contest_Sp_Score2[count] = girlEat_judge.spscore2_score;
            GameMgr.contest_Sp_Score3[count] = girlEat_judge.spscore3_score;
            GameMgr.contest_Sp_Score4[count] = girlEat_judge.spscore4_score;
            GameMgr.contest_Sp_Score5[count] = girlEat_judge.spscore5_score;
            GameMgr.contest_Sp_Score6[count] = girlEat_judge.spscore6_score;
            GameMgr.contest_Sp_Score7[count] = girlEat_judge.spscore7_score;
            GameMgr.contest_Sp_Score8[count] = girlEat_judge.spscore8_score;
            GameMgr.contest_Sp_Score9[count] = girlEat_judge.spscore9_score;
            GameMgr.contest_Sp_Score10[count] = girlEat_judge.spscore10_score;

            count++;
            
        }

        //コンテストSPスコア判定する場合　どの値の点数をみるか
        Contest_SPScoreJudgeCheck(GameMgr.contest_SPJudgeCommentNum);

        //
        //各コンテスト審査員ごとの判定分け　補正がけ
        //
        Contest_Score_JudgeHoseiLibrary(1);
        girlEat_judge.ContestDebugTextLog();

        //じいさんの食感感想 メモに表示用
        Contest_ShokukanHintHyouji(GameMgr.contest_Taste_Score[2], GameMgr.contest_shokukan_mes);

        //アントワネットの見た目感想 メモに表示用
        Contest_BeuatyHintHyouji(GameMgr.contest_Beauty_Score[1], "");

        //SPスコアの感想　メモに表示
        Contest_SPScoreHintHyouji(GameMgr.contest_SPScoreJudge, GameMgr.Contest_Spscore_text);


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

    //審査員の個別の判定補正＋判定値にSPスコア関連補正　各コンテスト個別に設定する
    void Contest_Score_JudgeHoseiLibrary(int _status)
    {
        switch (GameMgr.Contest_Name)
        {
            //1の頃のコンテスト
            case "First_Contest":

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {

                }
                else if (_status == 1)
                {
                    //審査員３　じいさんだけ、食感の補正　食感がよいほど、得点が上がりやすくなる。その代わり見た目の点数が一切入らない。
                    Contest_ShokukanHosei_1();

                    //200点を上限に100点に正規化する。
                    ScoreNormalized(200);
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;


            //２～
            case "Or_Contest_001":　//プラトンアカデミー

                switch(GameMgr.ContestRoundNum) //各回戦ごとの調整
                {
                    case 1: //焼き菓子のみ　クッキー　ラスク　マフィン　フィナンシェ

                        if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                        {
                            if (item_subType == "Cookie" || item_subType == "Cookie_Hard"
                                || item_subType == "Rusk" || item_subType == "Maffin" || item_subType == "Financier"
                                || item_subType == "Cannoli" || item_subType == "Biscotti")
                            {
                                judge_flag = true;
                            }
                            else
                            {
                                judge_flag = false;
                            }
                        }
                        break;

                    case 2:

                        break;

                    case 3:

                        break;
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //クッキー系は点数が下がる
                    Contest_CookieHosei();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();                    

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(130); //
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_002":　//サマードリームスフェスティバル

                switch (GameMgr.ContestRoundNum) //各回戦ごとの調整
                {
                    case 1:

                        break;

                    case 2:

                        break;

                    case 3:

                        break;
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //クッキー系は点数が下がる
                    Contest_CookieHosei();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();                    

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(200); //
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_003":　//アルクアンシェル

                switch (GameMgr.ContestRoundNum) //各回戦ごとの調整
                {
                    case 1:

                        break;

                    case 2:

                        break;

                    case 3:

                        break;
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //クッキー系は点数が下がる
                    Contest_CookieHosei();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(200); //
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_004":　//パティスリ・デュモンド

                switch (GameMgr.ContestRoundNum) //各回戦ごとの調整
                {
                    case 1:

                        break;

                    case 2:

                        break;

                    case 3:

                        break;
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //クッキー系は点数が下がる
                    Contest_CookieHosei();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(200); //
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;


            case "Or_Contest_010":　//クッキー初級コンテスト

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if(item_subType == "Cookie" || item_subType == "Cookie_Hard"
                        || item_subTypeB == "a_GlowCookie" || item_subTypeB == "a_GlowCookie_Hard")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(120); //90%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_020":　//オランジーナ・パティスリーアワード ケーキか　クリームブリュレ

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Cake" || item_subType == "CheeseCake" || item_subType == "PanCake" || item_subType == "Castella" || item_subType == "Maffin"
                        || item_subTypeB == "a_CreamBrulee" || item_subTypeB == "a_CookieCake")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //75%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_030":　//ベオルヴ家のディナー　見た目を高くしないと通らない 飲み物系は×

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    judge_flag = true;

                    for ( i=0; i < GameMgr.OkashiFoodOrDrink_list.Count; i++)
                    {
                        if (item_subType == GameMgr.OkashiFoodOrDrink_list[i])
                        {
                            judge_flag = false;
                        }
                    }
                    
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();

                    //見た目の審査基準が高め
                    girl1_status.girl1_Beauty[1] = 100;
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(130); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                
                break;

            case "Or_Contest_050":　//ラスク

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Rusk")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(120); //85%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_060":　//ルミエール・エピファニア　光りのお菓子で採点される

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subTypeB == "a_GlowCake" || item_subTypeB == "a_GlowCookie" || item_subTypeB == "a_GlowCookie_Hard"
                        || item_subTypeB == "a_GlowCheeseCake" || item_subTypeB == "a_GlowJelly" || item_subTypeB == "a_GlowCandy"
                        || item_subTypeB == "a_GlowRusk" || item_subTypeB == "a_GlowJuice")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        //上記タイプのおかしでなくても、光りの演出魔法がかかっていれば、採点は通る
                        if (_basemagicslot_Name == GameMgr.System_MagicSlotName02 || _basemagicslot_Name == GameMgr.System_MagicSlotName07)
                        {
                            judge_flag = true;
                        }
                        else
                        {
                            judge_flag = false;
                        }
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(130); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_070":　//ルミエール・カンデラ　光りのお菓子で採点される　キラキラ感で補正がはいる

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subTypeB == "a_GlowCake" || item_subTypeB == "a_GlowCookie" || item_subTypeB == "a_GlowCookie_Hard"
                        || item_subTypeB == "a_GlowCheeseCake" || item_subTypeB == "a_GlowJelly" || item_subTypeB == "a_GlowCandy"
                        || item_subTypeB == "a_GlowRusk" || item_subTypeB == "a_GlowJuice")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        //上記タイプのおかしでなくても、光りの演出魔法がかかっていれば、採点は通る
                        if (_basemagicslot_Name == GameMgr.System_MagicSlotName02 || _basemagicslot_Name == GameMgr.System_MagicSlotName07)
                        {
                            judge_flag = true;
                        }
                        else
                        {
                            judge_flag = false;
                        }
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score9[i] = 10; //キラキラ感の値が最低3は必要　上記の_status=10をクリアしてても、ここで弾かれる可能性あり
                    }
                    GameMgr.contest_SPJudgeCommentNum = 9; //コンテストコメント番号

                    Debug.Log("判定値追加： キラキラ感 " + 10);
                    Debug.Log("### ###");
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");

                    //SpScoreの値によって全体の点数に補正
                    SpScoreHosei_1(GameMgr.contest_SPScoreJudge);
                }

                break;

            case "Or_Contest_080":　//ガレットデロワ　ケーキ限定

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Cake" || item_subType == "Cake_Mat" || item_subType == "CheeseCake"
                        || item_subTypeB == "a_CookieCake")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(170); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_090":　//ディオ・ショコラ・チャンピオンシップ　チョコレート限定

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Chocolate")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(200); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_100":　//フィナンシェ

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //75%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_110":　//春の大祭典

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(170); //75%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_200":　//ひんやりお菓子

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //75%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_210":　//フライングソーダ

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Soda")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    //Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_220":　//ボンボヤージュ

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //75%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;

            case "Or_Contest_230":　//おみやげおかし クッキーかラスクを除く　子ども向けのおかし

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Cookie" || item_subType == "Cookie_Hard" || item_subType == "Cookie_Mat" || item_subType == "Rusk")
                    {
                        judge_flag = false;
                    }
                    else
                    { }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score6[i] = 1; //子供の値が最低1は必要
                    }
                    GameMgr.contest_SPJudgeCommentNum = 6; //コンテストコメント番号

                    Debug.Log("判定値追加： 子供 " + 1);
                    Debug.Log("### ###");
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");

                    //SpScoreの値によって全体の点数に補正
                    SpScoreHosei_1(GameMgr.contest_SPScoreJudge);
                }

                break;

            case "Or_Contest_250":　//遥かなる蒼

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Chocolate")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score2[i] = 10; //海らしさの値が最低10は必要
                    }
                    GameMgr.contest_SPJudgeCommentNum = 2; //コンテストコメント番号

                    Debug.Log("判定値追加： 海らしさ " + 10);
                    Debug.Log("### ###");
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(170); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");

                    //SpScoreの値によって全体の点数に補正
                    SpScoreHosei_1(GameMgr.contest_SPScoreJudge);
                }

                break;

            case "Or_Contest_260":　//マジックパティスリーアワード

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    //魔法のお菓子属性がついてるか、演出まほうがついてたらOK
                    if (_basemagicslot_Name != "Non" || _baseMagic != 0)
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(170); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_270":　//プラムおかし技術コンテスト　自由課題

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //クッキー系は点数が下がる
                    Contest_CookieHosei();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(200); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_280":　//チョコレート初級コンテスト

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Chocolate")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_290":　//パティシエの森　自由課題　クッキーやラスクだと点数上がりにくい

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //クッキー系は点数が下がる
                    Contest_CookieHosei();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(120); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_400":　//クレープ・ドゥ・シャノワール

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Crepe" || item_subType == "Crepe_Mat")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(120); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_410":　//アデュルティ・ガトー

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score5[i] = 5; //大人の値が最低3は必要
                    }
                    GameMgr.contest_SPJudgeCommentNum = 5; //コンテストコメント番号

                    Debug.Log("判定値追加： 大人 " + 5);
                    Debug.Log("### ###");
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //チョコの補正　黒以外は減点
                    Contest_ChocolateHosei();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(120); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");

                    //SpScoreの値によって全体の点数に補正
                    SpScoreHosei_2(GameMgr.contest_SPScoreJudge);
                }
                    
                break;

            case "Or_Contest_420":　//メルヘンランド♪カップ

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score7[i] = 5; //メルヘンの値が最低3は必要
                    }
                    GameMgr.contest_SPJudgeCommentNum = 7; //コンテストコメント番号

                    Debug.Log("判定値追加： メルヘン " + 5);
                    Debug.Log("### ###");
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(120); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");

                    //SpScoreの値によって全体の点数に補正
                    SpScoreHosei_1(GameMgr.contest_SPScoreJudge);                   
                }

                break;

            case "Or_Contest_430":　//キラキラ・ボンボンズ

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score6[i] = 5; //子供の値が最低3は必要
                    }
                    GameMgr.contest_SPJudgeCommentNum = 6; //コンテストコメント番号

                    Debug.Log("判定値追加： 子供 " + 5);
                    Debug.Log("### ###");
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(120); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");

                    //SpScoreの値によって全体の点数に補正
                    SpScoreHosei_1(GameMgr.contest_SPScoreJudge);
                }

                break;

            case "Or_Contest_440":　//英国ティータイムコンテスト

                if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subType == "Tea")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    //Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(170); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_450":　//ピエスモンテ 彫刻おかし限定 芸術点が高ければ高得点　低かったときかなり減点＋食感あまり点数評価されない

                /*if (_status == 10) //女の子の好みを使用する場合、お菓子タイプの判定をここで行う _status=10がないときは、判定をしていないので、どのお菓子でも通る。
                {
                    if (item_subTypeB == "a_IceCreamTwister" || item_subTypeB == "a_IceCandyTwister" || item_subTypeB == "a_ChocolateTwister"
                        || item_subTypeB == "a_CookieCake"
                        || itemName == "fantasian" || itemName == "fantasian_in_nightdream" || itemName == "princess_tota" || itemName == "slimejelly_freezed"
                        || itemName == "potate_jewerybox"
                        || itemName == "bush_de_noel" || itemName == "bush_de_noel_buttefly")
                    {
                        judge_flag = true;
                    }
                    else
                    {
                        judge_flag = false;
                    }
                }*/

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    //Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score8[i] = 20; //芸術性の値が最低20は必要 足りない場合、-数値*5倍 + -30 最大の減点が-130点
                    }
                    GameMgr.contest_SPJudgeCommentNum = 8; //コンテストコメント番号

                    Debug.Log("判定値追加： 芸術性 " + 20);
                    Debug.Log("### ###");
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員全員　食感の値下げる。
                    Contest_ShokukanHosei_20();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    
                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(100); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_460":　//コンチェルティーノ・イン・ブルー　独自の判定使用　なので、ここは補正値のみ

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    //Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score8[i] = 20; //芸術性の値が最低20は必要 足りない場合、-数値*5倍 + -30 最大の減点が-130点
                    }
                    GameMgr.contest_SPJudgeCommentNum = 8; //コンテストコメント番号

                    Debug.Log("判定値追加： 芸術性 " + 20);
                    Debug.Log("### ###");
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(200); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_470":　//ビジョウ・パティスリー・カップ


                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score9[i] = 10; //キラキラ感の値が最低3は必要
                    }
                    GameMgr.contest_SPJudgeCommentNum = 9; //コンテストコメント番号

                    Debug.Log("判定値追加： キラキラ感 " + 10);
                    Debug.Log("### ###");
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(180); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");

                    //SpScoreの値によって全体の点数に補正
                    SpScoreHosei_1(GameMgr.contest_SPScoreJudge);
                }

                break;

            //480　デザインコンテスト　未実装

            case "Or_Contest_490":　//秋のお菓子コンテスト　自由課題　中級

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }

                break;

            case "Or_Contest_630":　//フェド・フルラージュ

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();

                    for (i = 0; i < set_ID.Count; i++)
                    {
                        girl1_status.girl1_SP_Score7[i] = 5; //メルヘンの値が最低3は必要
                    }
                    GameMgr.contest_SPJudgeCommentNum = 7; //コンテストコメント番号

                    Debug.Log("判定値追加： メルヘン " + 5);
                    Debug.Log("### ###");
                }
                else if (_status == 1) //審査員の判定に補正
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(170); //50%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");

                    //SpScoreの値によって全体の点数に補正
                    SpScoreHosei_1(GameMgr.contest_SPScoreJudge);
                }

                break;

            default:

                if (_status == 0) //コンテストの判定に補正入れる場合は0
                {
                    //じいさんの見た目判定を0に。
                    Contest_KyotuHosei_1();
                }
                else if (_status == 1)
                {
                    //特定のおかし補正
                    Contest_KoyuOkashiHosei_1();

                    //審査員２　アントワネット王妃　見た目の補正
                    Contest_BeautyHosei_1();
                    Contest_ShokukanHosei_10();

                    //審査員３　じいさんだけ、食感の補正
                    Contest_ShokukanHosei_1();

                    //入れた数値を上限に100点に正規化する。
                    ScoreNormalized(150); //75%
                    Debug.Log("各点数にコンテスト補正で下げる：" + contest_bairitsu_hosei);
                    Debug.Log("### ###");
                }
                break;
        }
    }

    //コンテスト共通で適用する補正　じいさんの見た目判定をなくす。
    void Contest_KyotuHosei_1()
    {
        //じいさんの見た目判定を0に。
        girl1_status.girl1_Beauty[2] = 0;
    }

    //とくていのお菓子に反応して点数を補正する
    void Contest_KoyuOkashiHosei_1()
    {
        SetBeforeScore();

        //生地や素材系アイテム、パンなどお菓子でないものは点数が下がる
        if (item_subTypeB == "a_CookieSource" || item_subTypeB == "a_Crepe_Mat" || item_subTypeB == "a_CreampuffSimple"
            || item_subTypeB == "a_Cake_Mat" || item_subTypeB == "a_Bread" || item_subTypeB == "a_Bread_Sliced")
        {
            Hosei_ScoreKeisan(0.75f, 0.75f, 0.75f, 0.75f); //食感, 甘さ, 苦さ ,酸味　に補正値
        }

        // 補正前に、一回before_tastescore[2]は計算してtotal_scoreに加点されてるので、ここで引き算
        AfterHosei_TotalScoreKeisan();

        Debug.Log("審査員全員　シンプルなお菓子系だったので、食感点数0.75と甘さ関係0.75に補正");
        Debug.Log("審査員全員　食感補正前：" + before_tastescore[0] + "点");
        Debug.Log("審査員全員　食感補正後：" + GameMgr.contest_Taste_Score[0] + "点");
    }

    //クッキー系お菓子に対して点数を下方調整　ただし魔法のお菓子なら大丈夫
    void Contest_CookieHosei()
    {
        SetBeforeScore();
        

        //生地や素材系アイテムは点数が下がる
        if (item_subTypeB == "a_Cookie" || item_subTypeB == "a_Cookie_Hard" || item_subTypeB == "a_Rusk")
        {
            Hosei_ScoreKeisan(0.75f, 0.75f, 0.75f, 0.75f);          
        }

        // 補正前に、一回before_tastescore[2]は計算してtotal_scoreに加点されてるので、ここで引き算
        AfterHosei_TotalScoreKeisan();       


        Debug.Log("審査員全員　クッキーかラスク系だったので、食感点数0.75と甘さ関係0.75に補正");
        Debug.Log("審査員全員　食感補正前：" + before_tastescore[0] + "点");
        Debug.Log("審査員全員　食感補正後：" + GameMgr.contest_Taste_Score[0] + "点");
    }

    //チョコ系お菓子に対して点数を下方調整　ただし魔法のお菓子なら大丈夫
    void Contest_ChocolateHosei()
    {
        SetBeforeScore();

        //チョコ黒以外は点数が下がる
        if (item_subType == "Chocolate")
        {
            if (itemName == "chocolate_black" || itemName == "chocolate_black_twister" || itemName == "chocolate_black_type_of_bar"
                        || itemName == "chocolate_black_type_of_heart" || itemName == "chocolate_black_crown")
            { }
            else
            {
                Hosei_ScoreKeisan(0.75f, 0.75f, 0.75f, 0.75f);
            }
        }


        // 補正前に、一回before_tastescore[2]は計算してtotal_scoreに加点されてるので、ここで引き算
        AfterHosei_TotalScoreKeisan();

        Debug.Log("審査員全員　チョコ黒系以外だったので、食感点数0.75と甘さ関係0.75に補正");
        Debug.Log("審査員全員　食感補正前：" + before_tastescore[0] + "点");
        Debug.Log("審査員全員　食感補正後：" + GameMgr.contest_Taste_Score[0] + "点");
    }

    void SetBeforeScore()
    {
        before_tastescore[0] = GameMgr.contest_Taste_Score[0];
        before_tastescore[1] = GameMgr.contest_Taste_Score[1];
        before_tastescore[2] = GameMgr.contest_Taste_Score[2];

        before_sweatscore[0] = GameMgr.contest_Sweat_Score[0];
        before_sweatscore[1] = GameMgr.contest_Sweat_Score[1];
        before_sweatscore[2] = GameMgr.contest_Sweat_Score[2];

        before_bitterscore[0] = GameMgr.contest_Bitter_Score[0];
        before_bitterscore[1] = GameMgr.contest_Bitter_Score[1];
        before_bitterscore[2] = GameMgr.contest_Bitter_Score[2];

        before_sourscore[0] = GameMgr.contest_Sour_Score[0];
        before_sourscore[1] = GameMgr.contest_Sour_Score[1];
        before_sourscore[2] = GameMgr.contest_Sour_Score[2];
    }

    void Hosei_ScoreKeisan(float _deg1, float _deg2, float _deg3, float _deg4)
    {
        GameMgr.contest_Taste_Score[0] = (int)(GameMgr.contest_Taste_Score[0] * _deg1);
        GameMgr.contest_Taste_Score[1] = (int)(GameMgr.contest_Taste_Score[1] * _deg1);
        GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * _deg1);

        GameMgr.contest_Sweat_Score[0] = (int)(GameMgr.contest_Sweat_Score[0] * _deg2);
        GameMgr.contest_Sweat_Score[1] = (int)(GameMgr.contest_Sweat_Score[1] * _deg2);
        GameMgr.contest_Sweat_Score[2] = (int)(GameMgr.contest_Sweat_Score[2] * _deg2);

        GameMgr.contest_Bitter_Score[0] = (int)(GameMgr.contest_Bitter_Score[0] * _deg3);
        GameMgr.contest_Bitter_Score[1] = (int)(GameMgr.contest_Bitter_Score[1] * _deg3);
        GameMgr.contest_Bitter_Score[2] = (int)(GameMgr.contest_Bitter_Score[2] * _deg3);

        GameMgr.contest_Sour_Score[0] = (int)(GameMgr.contest_Sour_Score[0] * _deg4);
        GameMgr.contest_Sour_Score[1] = (int)(GameMgr.contest_Sour_Score[1] * _deg4);
        GameMgr.contest_Sour_Score[2] = (int)(GameMgr.contest_Sour_Score[2] * _deg4);
    }

    void AfterHosei_TotalScoreKeisan()
    {
        total_score[0] = total_score[0] + (GameMgr.contest_Taste_Score[0] - before_tastescore[0]);
        total_score[1] = total_score[1] + (GameMgr.contest_Taste_Score[1] - before_tastescore[1]);
        total_score[2] = total_score[2] + (GameMgr.contest_Taste_Score[2] - before_tastescore[2]);

        total_score[0] = total_score[0] + (GameMgr.contest_Sweat_Score[0] - before_sweatscore[0]); //補正がない場合は、単純に１を引いて、１を足す計算なので問題なし
        total_score[1] = total_score[1] + (GameMgr.contest_Sweat_Score[1] - before_sweatscore[1]);
        total_score[2] = total_score[2] + (GameMgr.contest_Sweat_Score[2] - before_sweatscore[2]);

        total_score[0] = total_score[0] + (GameMgr.contest_Bitter_Score[0] - before_bitterscore[0]);
        total_score[1] = total_score[1] + (GameMgr.contest_Bitter_Score[1] - before_bitterscore[1]);
        total_score[2] = total_score[2] + (GameMgr.contest_Bitter_Score[2] - before_bitterscore[2]);

        total_score[0] = total_score[0] + (GameMgr.contest_Sour_Score[0] - before_sourscore[0]);
        total_score[1] = total_score[1] + (GameMgr.contest_Sour_Score[1] - before_sourscore[1]);
        total_score[2] = total_score[2] + (GameMgr.contest_Sour_Score[2] - before_sourscore[2]);
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
        else if (GameMgr.contest_Taste_Score[2] >= 150 && GameMgr.contest_Taste_Score[2] < 300)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 1.8f);
        }
        else if (GameMgr.contest_Taste_Score[2] >= 300 && GameMgr.contest_Taste_Score[2] < 500)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 2.1f);
        }
        else if (GameMgr.contest_Taste_Score[2] >= 500 && GameMgr.contest_Taste_Score[2] < 750)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 2.5f);
        }
        else if (GameMgr.contest_Taste_Score[2] >= 750)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 3.0f);
        }
        else if (GameMgr.contest_Taste_Score[2] < 0)
        {
            GameMgr.contest_Taste_Score[2] = (int)(GameMgr.contest_Taste_Score[2] * 0.7f);
        }        

        total_score[2] = total_score[2] + (GameMgr.contest_Taste_Score[2] - before_tastescore[2]); //補正前に、一回before_tastescore[2]は計算してtotal_scoreに加点されてるので、ここで引き算

        Debug.Log("審査員３　じいさんは食感のみ、得点にバフがかかる。下の食感の値が最終の食感点数");
        Debug.Log("審査員３　食感補正前：" + before_tastescore[2] + "点");
        Debug.Log("審査員３　食感補正後：" + GameMgr.contest_Taste_Score[2] + "点");
    }

    void Contest_ShokukanHosei_10() //アントワネット王妃は、見た目の比重を大きくするため、食感の点数は影響を下げる。
    {
        before_tastescore[1] = GameMgr.contest_Taste_Score[1];        

        total_score[1] = total_score[1] + (int)(GameMgr.contest_Taste_Score[1] * 0.7f) - before_tastescore[1]; 
        //補正前に、一回before_tastescore[1]は計算してtotal_scoreに加点されてるので、ここで引き算

        Debug.Log("審査員２　王妃は食感の点数は少し下がる。下の食感の値が最終の食感点数");
        Debug.Log("審査員２　食感補正前：" + before_tastescore[1] + "点");
        Debug.Log("審査員２　食感補正後：" + GameMgr.contest_Taste_Score[1] + "点");
    }

    void Contest_ShokukanHosei_20() //審査員全員　見た目の比重を大きくするため、食感の点数は影響を下げる。
    {
        before_tastescore[0] = GameMgr.contest_Taste_Score[0];
        before_tastescore[1] = GameMgr.contest_Taste_Score[1];
        before_tastescore[2] = GameMgr.contest_Taste_Score[2];

        total_score[0] = total_score[0] + (int)(GameMgr.contest_Taste_Score[0] * 0.7f) - before_tastescore[0];
        total_score[1] = total_score[1] + (int)(GameMgr.contest_Taste_Score[1] * 0.7f) - before_tastescore[1];
        total_score[2] = total_score[2] + (int)(GameMgr.contest_Taste_Score[2] * 0.7f) - before_tastescore[2];
        //補正前に、一回before_tastescore[1]は計算してtotal_scoreに加点されてるので、ここで引き算

        Debug.Log("審査員全員　食感の点数少し下がる。下の食感の値が最終の食感点数");
        Debug.Log("審査員全員　食感補正前：" + before_tastescore[0] + "点");
        Debug.Log("審査員全員　食感補正後：" + GameMgr.contest_Taste_Score[0] + "点");
    }

    void Contest_BeautyHosei_1() //contest_Beauty_Scoreは、judge_beautyからベースを単純に引いた点数
    {
        //before_beautyscore[0] = GameMgr.contest_Beauty_Score[0];
        before_beautyscore[1] = GameMgr.contest_Beauty_Score[1];

        if (GameMgr.contest_Beauty_Score[1] > 0 && GameMgr.contest_Beauty_Score[1] < 30) //とりあえず基準値は満たした
        {
            GameMgr.contest_Beauty_Score[1] = 30;
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 30 && GameMgr.contest_Beauty_Score[1] < 45)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 1.1f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 45 && GameMgr.contest_Beauty_Score[1] < 60)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 1.2f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 60 && GameMgr.contest_Beauty_Score[1] < 70)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 1.3f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 70 && GameMgr.contest_Beauty_Score[1] < 80)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 1.5f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 80 && GameMgr.contest_Beauty_Score[1] < 90)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 1.8f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 90 && GameMgr.contest_Beauty_Score[1] < 110)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 2.0f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 110 && GameMgr.contest_Beauty_Score[1] < 130)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 2.1f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 130 && GameMgr.contest_Beauty_Score[1] < 150)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 2.25f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 150 && GameMgr.contest_Beauty_Score[1] < 220)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 2.35f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 220 && GameMgr.contest_Beauty_Score[1] < 270)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 2.5f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 270 && GameMgr.contest_Beauty_Score[1] < 320)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 2.75f);
        }
        else if (GameMgr.contest_Beauty_Score[1] >= 320)
        {
            GameMgr.contest_Beauty_Score[1] = (int)(GameMgr.contest_Beauty_Score[1] * 3.0f);
        }
        else if (GameMgr.contest_Beauty_Score[1] <= 0)
        {
            GameMgr.contest_Beauty_Score[1] = 0; //基準値に達していない
        }

        //GameMgr.contest_Beauty_Score[0] = GameMgr.contest_Beauty_Score[1]; //アントワネット補正後、タカノの見た目点数にも補正
        //total_score[0] = total_score[0] + (GameMgr.contest_Beauty_Score[0] - before_beautyscore[0]);

        total_score[1] = total_score[1] + (GameMgr.contest_Beauty_Score[1] - before_beautyscore[1]); //補正前に、一回before_beautyscore[1]は計算してtotal_scoreに加点されてるので、ここで引き算

        Debug.Log("審査員２　アントワネット王妃は、見た目で得点にバフがかかる。下の食感の値が最終の食感点数");
        Debug.Log("審査員２　見た目補正前：" + before_beautyscore[1] + "点");
        Debug.Log("審査員２　見た目補正後：" + GameMgr.contest_Taste_Score[1] + "点");
    }

    void Contest_BeautyHosei_2() //じいさんの補正　見た目の点数が0になる。
    {
        GameMgr.contest_Beauty_Score[2] = 0;

        Debug.Log("審査員３　じいさん　見た目点数を0に。");
    }

    //SpScoreの点数補正　各審査員のSP点数は同一なので、Score[0]をもってくればOK
    void SpScoreHosei_1(int _spscore)
    {
        if (_spscore >= 0 && _spscore < 5) //少し上がる
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] * 1.0f);
            }
        }
        else if (_spscore >= 5 && _spscore < 20) //ふつう
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] * 1.1f);
            }
        }
        else if (_spscore >= 20 && _spscore < 40) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 1.3f));
            }
        }
        else if (_spscore >= 40 && _spscore < 60) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 1.5f));
            }
        }
        else if (_spscore >= 60 && _spscore < 80) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 1.75f));
            }
        }
        else if (_spscore >= 80 && _spscore < 100) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 1.8f));
            }
        }
        else if (_spscore >= 100) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 1.85f));
            }
        }
        else if (_spscore < 0) //足りてないと0.75
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] * 0.75f);
            }
        }
    }

    //SpScoreの点数補正　各審査員のSP点数は同一なので、Score[0]をもってくればOK
    void SpScoreHosei_2(int _spscore)
    {
        if (_spscore >= 0 && _spscore < 5) //少し上がる
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] * 0.5f);
            }
        }
        else if (_spscore >= 5 && _spscore < 20) //ふつう
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] * 0.5f);
            }
        }
        else if (_spscore >= 20 && _spscore < 40) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 1.0f));
            }
        }
        else if (_spscore >= 40 && _spscore < 60) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 1.25f));
            }
        }
        else if (_spscore >= 60 && _spscore < 80) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 1.5f));
            }
        }
        else if (_spscore >= 80 && _spscore < 100) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 2.0f));
            }
        }
        else if (_spscore >= 100) //SpScoreに補正して加算
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] + (_spscore * 2.5f));
            }
        }
        else if (_spscore < 0) //足りてないと0.75
        {
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                total_score[i] = (int)(total_score[i] * 0.35f);
            }
        }
    }


    void Contest_ShokukanHintHyouji(int shokukan_score, string shokukan_mes)
    {
        //食感に関するヒント
        if (shokukan_score < 20) //
        {
            _shokukan_kansou = GameMgr.ColorRedDeep + "食感 F: " + shokukan_mes + "が全然足りない..。" + "</color>";
        }
        else if (shokukan_score >= 20 && shokukan_score < 40) //
        {
            _shokukan_kansou = GameMgr.ColorRedDeep + "食感 C: " + shokukan_mes + "がもっとほしい" + "</color>";
        }
        else if (shokukan_score >= 40 && shokukan_score < GameMgr.low_score) //
        {
            _shokukan_kansou = "食感 B: " + "まあまあの" + shokukan_mes;
        }
        else if (shokukan_score >= GameMgr.low_score && shokukan_score < GameMgr.high_score) //
        {
            _shokukan_kansou = "食感 B+: " + "ほどほどに良い" + shokukan_mes;
        }
        else if (shokukan_score >= GameMgr.high_score && shokukan_score < 200) //
        {
            _shokukan_kansou = "食感 A: " + "良い" + shokukan_mes;
        }
        else if (shokukan_score >= 200 && shokukan_score < 350) //
        {
            _shokukan_kansou = GameMgr.ColorPink + "食感 A+: " + "絶妙な" + shokukan_mes + "</color>";
        }
        else if (shokukan_score >= 350) //
        {
            _shokukan_kansou = GameMgr.ColorGold + "食感 S: " + "神の" + shokukan_mes + "！！" + "</color>";
        }

        GameMgr.contest_lasthint_text = _shokukan_kansou + "\n" + GameMgr.contest_lasthint_text;
    }

    void Contest_BeuatyHintHyouji(int beauty_score, string beauty_mes)
    {
        //見た目に関するヒント
        if (beauty_score < 0) //
        {
            _beauty_kansou = GameMgr.ColorRedDeep + "見た目 C: " + "見た目が美しくない..。" + "</color>";
        }
        else if (beauty_score >= 0 && beauty_score < 50) //
        {
            _beauty_kansou = "見た目 B: " + "もう少し見栄えがすると良いですわ。";
        }
        else if (beauty_score >= 50 && beauty_score < 100) //
        {
            _beauty_kansou = "見た目 A: " + "見た目かなり美しいですわ！";
        }
        else if (beauty_score >= 100 && beauty_score < 200) //
        {
            _beauty_kansou = GameMgr.ColorPink + "見た目 A+: " + "最高の美しさで感動しました！！" + "</color>";
        }
        else if (beauty_score >= 200) //
        {
            _beauty_kansou = GameMgr.ColorGold + "見た目 S: " + "神のように美しい！！" + "</color>";
        }


        GameMgr.contest_lasthint_text = GameMgr.contest_lasthint_text + "\n" + _beauty_kansou;
    }

    void Contest_SPScoreJudgeCheck(int _num) //
    {
        switch(_num)
        {
            case 1: //各SPスコアの値　そのコンテストでの判定用点数

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score1[0];
                break;

            case 2:

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score2[0];
                break;

            case 3:

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score3[0];
                break;

            case 4:

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score4[0];
                break;

            case 5:

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score5[0];
                break;

            case 6:

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score6[0];
                break;

            case 7:

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score7[0];
                break;

            case 8:

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score8[0];
                break;

            case 9:

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score9[0];
                break;

            case 10:

                GameMgr.contest_SPScoreJudge = GameMgr.contest_Sp_Score10[0];
                break;

            default:

                GameMgr.contest_SPScoreJudge = 0;
                break;
        }
    }

    void Contest_SPScoreHintHyouji(int sp_score, string _sptext)
    {
        //SPScoreに関するヒント
        if (GameMgr.contest_SPJudgeCommentNum == 0) //SP判定なし
        { }
        else
        {
            if (sp_score < 0) //
            {
                _spscore_kansou = GameMgr.ColorRedDeep + _sptext + " D: " + _sptext + "が全然足りない..。" + "</color>";
            }
            else if (sp_score >= 0 && sp_score < 40) //
            {
                _spscore_kansou = _sptext + " B: " + _sptext + "もう少し欲しいですわ。";
            }
            else if (sp_score >= 40 && sp_score < 60) //
            {
                _spscore_kansou = _sptext + " A: " + _sptext + "が出てますね。";
            }
            else if (sp_score >= 60 && sp_score < 100) //
            {
                _spscore_kansou = GameMgr.ColorPink + _sptext + " A+: " + _sptext + "がよく出ていい感じ！！" + "</color>";
            }
            else if (sp_score >= 100) //
            {
                _spscore_kansou = GameMgr.ColorGold + _sptext + " S: " + _sptext + "がパーフェクトです！！" + "</color>";
            }

            GameMgr.contest_lasthint_text = GameMgr.contest_lasthint_text + "\n" + _spscore_kansou;
        }        
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
        contest_bairitsu_hosei = 100.0f / _max;

        //デバッグパネルの取得
        debug_panel = canvas.transform.Find("Debug_Panel(Clone)").GetComponent<Debug_Panel>();
        debug_taste_resultText = canvas.transform.Find("Debug_Panel(Clone)/Hyouji/OkashiTaste_Scroll View/Viewport/Content/Text").GetComponent<Text>();       
        debug_taste_resultText.text += "\n" + "\n" + "\n" + "\n" + "コンテスト倍率補正: " + contest_bairitsu_hosei.ToString("f2");
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
