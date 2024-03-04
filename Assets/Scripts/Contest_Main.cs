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
    private GameObject scene_black_effect;
    private GameObject canvas;

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

    private int contest_status;
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

        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 100;

        //さらにどのコンテストに現在出場しているかを指定
        GameMgr.ContestSelectNum = 0; //コンテストのシーン番号
        GameMgr.Contest_ON = false; //コンテストだけど、１のころのやつなのでfalseでだいじょうぶ

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

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //黒半透明パネルの取得
        black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

        //シーン全てをブラックに消すパネル
        scene_black_effect = canvas.transform.Find("Scene_Black").gameObject;

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

        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        _text.text = "コンテスト会場だ。";

        contest_select = canvas.transform.Find("Contest_Select").gameObject;
        conteston_toggle_judge = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_Judge").gameObject;

        contest_status = 100;

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }
	
	// Update is called once per frame
	void Update () {

        //コンテスト会場きたときのイベント
        if (!GameMgr.contest_eventStart_flag)
        {
            //採点処理
            contest_judge.Contest_Judge_Start();

            GameMgr.contest_eventStart_flag = true;
            GameMgr.scenario_ON = true;

            sceneBGM.MuteBGM();

            GameMgr.contest_event_num = 0;
            GameMgr.contest_event_flag = true;

        }

        //コンテスト終了後、エンディングへ
        if(GameMgr.ending_on)
        {
            scene_black_effect.SetActive(true);
            //GameMgr.scenario_ON = true;
            GameMgr.ending_on = false;
            FadeManager.Instance.LoadScene("100_Ending", 0.3f);
        }

        //バッドEDの場合、スタッフロールなし
        if (GameMgr.ending_on2)
        {
            scene_black_effect.SetActive(true);
            //GameMgr.scenario_ON = true;
            GameMgr.ending_on2 = false;

            if (GameMgr.RESULTPANEL_ON)
            {
                FadeManager.Instance.LoadScene("110_TotalResult", 0.3f);
            }
            else
            {
                FadeManager.Instance.LoadScene("120_AutoSave", 0.3f);
            }
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            text_area.SetActive(false);
            placename_panel.SetActive(false);
            contest_select.SetActive(false);
            black_effect.SetActive(false);
            scene_black_effect.SetActive(false);

            //contest_status = 0;
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

            contest_judge.Contest_Judge_Start();
        }
    }

    //別シーンからこのシーンが読み込まれたときに、読み込む
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameMgr.Scene_LoadedOn_End = true;
    }

    //シーンがアンロードされたタイミングで呼び出しされる
    void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
        GameMgr.Scene_LoadedOn_End = false;
    }
}
