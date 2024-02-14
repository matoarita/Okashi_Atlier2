using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

public class Contest_Main_OrA1 : MonoBehaviour {

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
    private int contest_scene;
    private PlayerItemList pitemlist;

    private GameObject text_area;
    private Text _text;

    private GameObject contest_select;
    private GameObject conteston_toggle_judge;
    private GameObject conteston_toggle_compo;

    private int kettei_itemID;
    private int kettei_itemType;

    private BGM sceneBGM;

    private string itemName;
    private string item_subType;
    private int compNum;

    private int i, count;
    private bool judge_flag;
    private int judge_Type, DB_list_Type;

    //Live2Dモデルの取得    
    private GameObject _model_root_obj;
    private GameObject _model_move;
    private GameObject _model_obj;
    private CubismRenderController cubism_rendercontroller;
    private Animator live2d_animator;
    private bool character_ON;

    // Use this for initialization
    void Start () {

        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 100; 

        //さらにどのコンテストに現在出場しているかを指定
        GameMgr.ContestSelectNum = 1000; //コンテストのシーン番号　//大会の場合、1回戦　2回戦　決勝戦とかをシーン番号でさらに分けてよさげ。
        GameMgr.Contest_ON = true;

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
        placename_panel.SetActive(false);

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        sceneBGM.PlaySub();

        //お菓子の判定処理オブジェクトの取得
        contest_judge_obj = GameObject.FindWithTag("Contest_Judge");
        contest_judge = contest_judge_obj.GetComponent<Contest_Judge>();

        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //Live2Dモデルの取得
        character_ON = false;
        for (i = 0; i < SceneManager.sceneCount; i++)
        {
            //読み込まれているシーンを取得し、その名前をログに表示
            string sceneName = SceneManager.GetSceneAt(i).name;
            //Debug.Log(sceneName);

            GameObject[] rootObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();

            foreach (var obj in rootObjects)
            {
                //Debug.LogFormat("RootObject = {0}", obj.name);
                if (obj.name == "CharacterRoot")
                {
                    //Debug.Log("character_On: ヒカリちゃん　シーン内に存在する");
                    character_ON = true;

                    //Live2Dモデルの取得
                    _model_root_obj = GameObject.FindWithTag("CharacterRoot").gameObject;
                    _model_move = _model_root_obj.transform.Find("CharacterMove").gameObject;
                    _model_obj = _model_root_obj.transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;
                    cubism_rendercontroller = _model_obj.GetComponent<CubismRenderController>();
                    live2d_animator = _model_obj.GetComponent<Animator>();
                    live2d_animator.SetLayerWeight(3, 0.0f); //メインでは、最初宴用表情はオフにしておく。
                }
                else
                {

                }
            }
        }
        

        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        _text.text = "至高のチョコレート（Aランク）" + "\n"+ "テーマ：「風」をテーマにした美しいチョコレート";

        contest_select = canvas.transform.Find("Contest_Select").gameObject;
        conteston_toggle_compo = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_Compo").gameObject;

        contest_status = 100;
        

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }
	
	// Update is called once per frame
	void Update () {

        //コンテスト会場きたときのイベント
        /*if (!GameMgr.contest_eventStart_flag)
        {
            GameMgr.contest_eventStart_flag = true;
            GameMgr.scenario_ON = true;

            sceneBGM.MuteBGM();

            GameMgr.contest_event_num = 0;
            GameMgr.contest_event_flag = true;

        }*/

        //コンテスト終了後、エンディングへ
        if(GameMgr.ending_on)
        {
            scene_black_effect.SetActive(true);
            //GameMgr.scenario_ON = true;
            GameMgr.ending_on = false;
            FadeManager.Instance.LoadScene("100_Ending", 0.3f);
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            text_area.SetActive(false);
            //placename_panel.SetActive(false);
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
                    //placename_panel.SetActive(true);
                    contest_select.SetActive(true);
                    sceneBGM.MuteOFFBGM();

                    GameMgr.compound_select = 0; //何もしていない状態
                    GameMgr.compound_status = 0;

                    contest_status = 100;
                    contest_scene = 0;

                    GameMgr.Status_zero_readOK = true;
                    break;

                case 100: //退避

                    break;

                case 500: //調合用

                    //調合終了まち
                    if (GameMgr.CompoundSceneStartON == false)
                    {
                        GameMgr.compound_select = 0; //何もしていない状態
                        GameMgr.compound_status = 0;

                        contest_status = 0;
                        contest_scene = 0;
                    }
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
    

    public void OnCheck_Compound() //調合シーンに入る
    {
        if (conteston_toggle_compo.GetComponent<Toggle>().isOn == true)
        {
            conteston_toggle_compo.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            contest_status = 500; //
            contest_scene = 500;
            text_area.SetActive(false);

            GameMgr.compound_status = 6;
            GameMgr.CompoundSceneStartON = true; //調合シーンに入っています、というフラグ開始。処理をCompoundMainControllerオブジェに移す。
        }
    }

    public void OnDebugContestStart()
    {
        GameMgr.contest_eventStart_flag = true;
        GameMgr.scenario_ON = true;

        sceneBGM.MuteBGM();

        GameMgr.contest_event_num = 0;
        GameMgr.contest_event_flag = true;

        GameMgr.ending_count = 1;
        canvas.transform.Find("DebugContestStart").gameObject.SetActive(false);
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
