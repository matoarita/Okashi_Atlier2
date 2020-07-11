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

    private Debug_Panel_Init debug_panel_init;
    private Exp_Controller exp_Controller;

    private GameObject contest_judge_obj;
    private Contest_Judge contest_judge;

    public int contest_status;

    private GameObject text_area;
    private Text _text;

    private GameObject contest_select;
    private GameObject conteston_toggle_judge;

    private int kettei_itemID;
    private int kettei_itemType;

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

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

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

        //お菓子の判定処理オブジェクトの取得
        contest_judge_obj = GameObject.FindWithTag("Contest_Judge");
        contest_judge = contest_judge_obj.GetComponent<Contest_Judge>();

        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        _text.text = "コンテスト会場だ。";
        //text_area.SetActive(false);

        contest_select = canvas.transform.Find("Contest_Select").gameObject;
        conteston_toggle_judge = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_Judge").gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            text_area.SetActive(false);
            placename_panel.SetActive(false);
            black_effect.SetActive(false);

            contest_status = 0;
        }
        else
        { }
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
        Debug.Log("コンテストON");

        //判定するお菓子を決定
        
        if (exp_Controller._temp_extreme_id != 9999)
        {
            kettei_itemID = exp_Controller._temp_extreme_id;
            kettei_itemType = exp_Controller._temp_extreme_itemtype;
        }
        else //エクストリームパネルにお菓子が入っていない時
        {
            //お試し　店売りねこクッキー
            kettei_itemID = 200;
            kettei_itemType = 0;
        }

        //***お菓子の判定処理　***
        //一回戦はExtremePanelに入ったお菓子を判定する。決勝戦は、ランダムで課題を一つ、予定
        //左二つが判定するお菓子、3番目が判定用のセット番号(girlLikeCompoのcompIDか、girlLikeSetの番号を直接指定。）
        //4番目がコンテスト判定タイプ　0=審査員3人か、1=ランダムで自由にお菓子を一つ作る判定 に分岐予定。
        //0だと、3番目の番号は、girlLikeCompoのcompID。　1だと、girlLikeSetのcomp_Num番号を直接選べばOK
        //***

        //一回戦
        //contest_judge.Contest_Judge_method(kettei_itemID, kettei_itemType, girl1_status.OkashiQuest_ID, 0);

        //決勝戦　自由課題バージョン　アマクサが80点以上の予定で、それを超えないとダメ
        contest_judge.Contest_Judge_method(kettei_itemID, kettei_itemType, 1510, 1);
    }
}
