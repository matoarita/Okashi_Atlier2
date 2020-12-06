using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Contest_Main : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private SoundController sc;
    private Girl1_status girl1_status;

    private GameObject placename_panel;
    private GameObject black_effect;
    private GameObject canvas;

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    private ItemDataBase database;

    private Debug_Panel_Init debug_panel_init;
    private Exp_Controller exp_Controller;

    private GameObject contest_judge_obj;
    private Contest_Judge contest_judge;

    public int contest_status;
    private PlayerItemList pitemlist;

    private GameObject text_area;
    private Text _text;

    private GameObject contest_select;
    private GameObject conteston_toggle_judge;

    private int kettei_itemID;
    private int kettei_itemType;

    private BGM sceneBGM;

    private string itemName;
    private string item_subType;
    private int compNum;

    private int i, count;
    private bool judge_flag;
    private int judge_Type, DB_list_Type;

    // Use this for initialization
    void Start () {

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの取得
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //女の子の好みのお菓子セットの取得
        girlLikeSet_database = GirlLikeSetDataBase.Instance.GetComponent<GirlLikeSetDataBase>();

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //黒半透明パネルの取得
        black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

        //場所名前パネル
        placename_panel = canvas.transform.Find("PlaceNamePanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //お菓子の判定処理オブジェクトの取得
        contest_judge_obj = GameObject.FindWithTag("Contest_Judge");
        contest_judge = contest_judge_obj.GetComponent<Contest_Judge>();

        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        _text.text = "コンテスト会場だ。";

        contest_select = canvas.transform.Find("Contest_Select").gameObject;
        conteston_toggle_judge = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_Judge").gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        //コンテスト会場きたときのイベント
        if (!GameMgr.ContestEvent_stage[0])
        {
            GameMgr.ContestEvent_stage[0] = true;
            GameMgr.scenario_ON = true;

            sceneBGM.MuteBGM();

            GameMgr.contest_event_num = 0;
            GameMgr.contest_event_flag = true;

        }

        //コンテスト終了後、エンディングへ
        if(GameMgr.ending_on)
        {
            GameMgr.scenario_ON = true;
            GameMgr.ending_on = false;
            FadeManager.Instance.LoadScene("100_Ending", 0.3f);
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            text_area.SetActive(false);
            placename_panel.SetActive(false);
            contest_select.SetActive(false);
            black_effect.SetActive(false);

            contest_status = 0;
        }
        else
        {
            switch (contest_status)
            {
                case 0:

                    text_area.SetActive(true);
                    placename_panel.SetActive(true);
                    contest_select.SetActive(true);
                    sceneBGM.MuteOFFBGM();

                    contest_status = 100;
                    break;

                case 100: //退避

                    break;

                default:

                    break;
            }
        }
    }

    public void OnContest_Judge_Toggle()
    {
        if(conteston_toggle_judge.GetComponent<Toggle>().isOn == true)
        {
            conteston_toggle_judge.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。          

            Contest_Judge();
        }
    }


    public void Contest_Judge()
    {
        Debug.Log("コンテスト判定ON");

        //判定するお菓子を決定
        
        if (exp_Controller._temp_extreme_id != 9999)
        {
            kettei_itemID = exp_Controller._temp_extreme_id;
            kettei_itemType = exp_Controller._temp_extreme_itemtype;
        }
        else //エクストリームパネルにお菓子が入っていない時。デバッグ用。
        {
            //お試し　店売りねこクッキー
            kettei_itemID = 200;
            kettei_itemType = 0;
        }

        //提出されたお菓子の固有アイテム名・タイプサブを出し、判定用DBから一致するものを探す。
        if (kettei_itemType == 0)
        {
            itemName = database.items[kettei_itemID].itemName;
            item_subType = database.items[kettei_itemID].itemType_sub.ToString();

            //表示用アイテム名
            GameMgr.contest_okashiSlotName = "";
            GameMgr.contest_okashiName = database.items[kettei_itemID].itemNameHyouji;
        }
        else if (kettei_itemType == 1)
        {
            itemName = pitemlist.player_originalitemlist[kettei_itemID].itemName;
            item_subType = pitemlist.player_originalitemlist[kettei_itemID].itemType_sub.ToString();

            //表示用アイテム名
            GameMgr.contest_okashiSlotName = pitemlist.player_originalitemlist[kettei_itemID].item_SlotName;
            GameMgr.contest_okashiName =  pitemlist.player_originalitemlist[kettei_itemID].itemNameHyouji;
        }

        


        judge_flag = false;

        //***お菓子の判定処理　***
        //左二つが判定するお菓子、3番目が判定用のセット番号(girlLikeCompoのcompIDか、girlLikeSetの番号を直接指定。）
        //4番目がコンテスト判定タイプ　0=審査員3人か、1=審査員1人として計算
        //3番目の番号は、girlLikeSetのcomp_Num番号。
        //***

        judge_Type = 0; //0=審査員3人。1=審査員1人

        if (judge_Type == 0) //1000～が審査員3人個別に判定セット。1500～が審査員まとめて一つバージョンの評価セット
        {
            DB_list_Type = 1000;
        }
        else if (judge_Type == 1)
        {
            DB_list_Type = 1500; //現在は使用していない。
        }

        i = 0;
        while (i < girlLikeSet_database.girllikeset.Count)
        {
            if (girlLikeSet_database.girllikeset[i].girlLike_compNum >= DB_list_Type) 
            {
                if (girlLikeSet_database.girllikeset[i].girlLike_itemName != "Non") //固有名がはいってる場合は、固有名をみる。
                {
                    //固有のアイテム名と一致するかどうかを判定。
                    if (girlLikeSet_database.girllikeset[i].girlLike_itemName == itemName)
                    {
                        //一致した場合の番号を入れる。
                        compNum = girlLikeSet_database.girllikeset[i].girlLike_compNum;
                        judge_flag = true;
                        Debug.Log("判定番号: " + compNum);
                        break;
                    }
                }
                else//固有名が入ってない場合は、サブタイプをみる。
                {
                    if (girlLikeSet_database.girllikeset[i].girlLike_itemSubtype == item_subType && girlLikeSet_database.girllikeset[i].girlLike_itemSubtype != "Non")
                    {
                        compNum = girlLikeSet_database.girllikeset[i].girlLike_compNum;
                        judge_flag = true;
                        Debug.Log("判定番号: " + compNum);
                        break;
                    }
                }
            }
            i++;
        }

        if (!judge_flag)
        {
            //例外処理。もし、審査員DB上に登録されていないお菓子を渡した場合。通常はないはずだが、登録し忘れなどの場合。
            for (i = 0; i < GameMgr.contest_Score.Length; i++)
            {
                GameMgr.contest_Score[i] = -9999;
            }

            GameMgr.contest_TotalScore = -9999;
        }
        else
        {
            switch(judge_Type)
            {
                case 0:

                    //0
                    //審査員個別に判定バージョン　個別の場合、3人それぞれの評価値を設定する必要があり。
                    contest_judge.Contest_Judge_method(kettei_itemID, kettei_itemType, compNum, 0);
                    break;

                case 1:

                    //1
                    //審査員まとめて一つの点数バージョン
                    contest_judge.Contest_Judge_method(kettei_itemID, kettei_itemType, compNum, 1);
                    break;
            }                       
        }
    }
}
