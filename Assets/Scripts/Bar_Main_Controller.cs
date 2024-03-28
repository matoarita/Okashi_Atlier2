using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Bar_Main_Controller : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private ItemShopDataBase shop_database;
    private ItemMatPlaceDataBase matplace_database;

    private SoundController sc;
    private Girl1_status girl1_status;

    private SceneInitSetting sceneinit_setting;

    private BGM sceneBGM;

    private GameObject text_area;
    private Text _text;
    private string shopdefault_text;

    private Debug_Panel_Init debug_panel_init;

    private GameObject placename_panel;

    private GameObject quest_Judge_CanvasPanel;

    private GameObject shopitemlist_onoff;
    private GameObject shopquestlist_obj;

    private GameObject money_status_obj;
    private GameObject ninki_status_obj;

    public GameObject hukidasi_sub;
    private GameObject hukidasi_sub_Prefab;

    private GameObject character;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject backshopfirst_obj;

    private GameObject black_effect;

    private GameObject canvas;
    private GameObject shop_select;
    private GameObject shopon_toggle_talk;
    private GameObject shopon_toggle_quest;
    private GameObject shopon_toggle_uwasa;
    private GameObject shopon_toggle_present;
    private GameObject shopon_toggle_back;

    private bool check_event;
    private bool check_lvevent;
    private bool lvevent_loading;

    //public int bar_status;
    //public int bar_scene; //どのシーンを選択しているかを判別

    private bool hukidasi_oneshot; //吹き出しの作成は一つのみ

    private int i;

    private List<bool> shopuwasa_List = new List<bool>();
    private List<int> random_uwasa_select = new List<int>();
    private int uwasalist_count;
    private int rnd;
    private int count;
    private bool StartRead;

    // Use this for initialization
    void Start () {

        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 30;

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

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //吹き出しプレファブの取得
        hukidasi_sub_Prefab = (GameObject)Resources.Load("Prefabs/Emo_Hukidashi_Anim");

        //黒半透明パネルの取得
        black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

        character = GameObject.FindWithTag("Character");
        character.GetComponent<FadeCharacter>().SetOff();

        hukidasi_oneshot = false;

        shop_select = canvas.transform.Find("Bar_Select").gameObject;
        shopon_toggle_talk = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Talk").gameObject;
        shopon_toggle_quest = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Quest").gameObject;
        shopon_toggle_uwasa = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Uwasa").gameObject;
        shopon_toggle_present = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Present").gameObject;
        shopon_toggle_present.SetActive(false);
        shopon_toggle_back = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Back").gameObject;
        backshopfirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        backshopfirst_obj.SetActive(false);
        //shopon_toggle_quest.SetActive(false);

        //自分の持ってるお金などのステータス
        money_status_obj = canvas.transform.Find("MoneyStatus_panel").gameObject;
        money_status_obj.SetActive(false);

        //自分の持ってるお金などのステータス
        ninki_status_obj = canvas.transform.Find("NinkiStatus_panel").gameObject;
        if (GameMgr.Story_Mode == 0)
        {
            ninki_status_obj.SetActive(false);
        }
        else
        {
            ninki_status_obj.SetActive(true);
        }

        //場所名前パネル
        placename_panel = canvas.transform.Find("PlaceNamePanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        playeritemlist_onoff.SetActive(false);

        //ショップリスト画面。初期設定で最初はOFF。
        shopitemlist_onoff = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
        shopitemlist_onoff.SetActive(false);

        //クエストリスト画面。初期設定で最初はOFF。
        quest_Judge_CanvasPanel = canvas.transform.Find("Quest_Judge_CanvasPanel").gameObject;
        shopquestlist_obj = quest_Judge_CanvasPanel.transform.Find("ShopQuestList_ScrollView").gameObject;
        shopquestlist_obj.SetActive(false);

        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //初期メッセージ
        shopdefault_text = "いらっしゃい～。";
        _text.text = shopdefault_text;
        text_area.SetActive(false);

        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;

        check_event = false; //強制で発生するイベントのフラグ
        check_lvevent = false; //レベルに応じて発生するイベントのフラグ
        lvevent_loading = false;

        if (GameMgr.Story_Mode == 1)
        {
            //あるクエスト以降、フィオナにお菓子わたせる。
            if (GameMgr.GirlLoveEvent_num >= 11)
            {
                shopon_toggle_present.SetActive(true);
            }
        }

        //入店のタイミングでのみ、クエスト更新
        shopquestlist_obj.GetComponent<ShopQuestListController>().SetQuestInit = true;

        //入店の音
        sc.PlaySe(51);

        StartRead = false;
        GameMgr.Scene_LoadedOn_End = true; //シーン読み込み完了

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateBarScene()
    {
        if (!StartRead) //シーン最初だけ読み込む
        {
            StartRead = true;
            sceneBGM.PlaySub();
        }

        //強制的に発生するイベントをチェック。はじめてショップへきた時など

        if (!GameMgr.BarEvent_stage[0]) //はじめて酒場へきた。
        {
            GameMgr.BarEvent_stage[0] = true;

            GameMgr.scenario_ON = true;

            GameMgr.bar_event_num = 0;
            GameMgr.bar_event_flag = true;

            check_event = true;

            StartCoroutine("Scenario_loading");

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            GameMgr.CompoundEvent_num = 5;
            GameMgr.CompoundEvent_flag = true;
        }


        if (!check_event)
        {
            /*
            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {

                case 2: //かわいい材料を探しに来た。

                    if (!GameMgr.ShopEvent_stage[5])
                    {
                        GameMgr.ShopEvent_stage[5] = true;
                        GameMgr.scenario_ON = true;

                        GameMgr.shop_event_num = 2;
                        GameMgr.shop_event_flag = true;

                        StartCoroutine("Scenario_loading");
                    }

                    break;

            }
            */
        }

        if (GameMgr.Reset_SceneStatus)
        {
            GameMgr.Reset_SceneStatus = false;
            GameMgr.Scene_Status = 0;
        }


        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            //playeritemlist_onoff.SetActive(false);
            shopitemlist_onoff.SetActive(false);
            shopquestlist_obj.SetActive(false);
            backshopfirst_obj.SetActive(false);
            shop_select.SetActive(false);
            text_area.SetActive(false);
            money_status_obj.SetActive(false);
            placename_panel.SetActive(false);
            black_effect.SetActive(false);

            if (GameMgr.Story_Mode == 1)
            {
                ninki_status_obj.SetActive(false);
            }
        }
        else
        {
            if (!check_lvevent) //ショップの品数が増えるなど、パティシエレベルや好感度に応じたイベントの発生フラグをチェック
            {
                Debug.Log("チェック　パティシエレベルor好感度レベルイベント");
                CheckBarLvEvent();

                if (lvevent_loading) { }
                else
                {
                    //すべてのイベントをチェックし終わって、何もなければfalseになっており、lveventのチェックをtrueにして終了する。はず。
                    check_lvevent = true;
                }
            }
            else
            {
                //Debug.Log("shop_status" + shop_status);
                switch (GameMgr.Scene_Status)
                {
                    case 0:

                        character.GetComponent<FadeCharacter>().SetOn();
                        shopitemlist_onoff.SetActive(false);
                        shopquestlist_obj.SetActive(false);
                        playeritemlist_onoff.SetActive(false);
                        backshopfirst_obj.SetActive(false);
                        backshopfirst_obj.GetComponent<Button>().interactable = true;
                        shop_select.SetActive(true);
                        text_area.SetActive(true);
                        money_status_obj.SetActive(true);
                        placename_panel.SetActive(true);
                        black_effect.SetActive(false);

                        if (GameMgr.Story_Mode == 1)
                        {
                            ninki_status_obj.SetActive(true);
                        }

                        GameMgr.Scene_Select = 0;
                        GameMgr.Scene_Status = 100;

                        if (trans == 1) //カメラが寄っていたら、デフォに戻す。
                        {
                            //カメラ寄る。
                            trans--; //transが1を超えたときに、ズームするように設定されている。

                            //intパラメーターの値を設定する.
                            maincam_animator.SetInteger("trans", trans);
                        }
                        else if (trans == 10) //カメラが寄っていたら、デフォに戻す。
                        {
                            //カメラ寄る。
                            trans = 0; //transが1を超えたときに、ズームするように設定されている。

                            //intパラメーターの値を設定する.
                            maincam_animator.SetInteger("trans", trans);
                        }

                        break;

                    case 1: //ショップのアイテム選択中
                        break;

                    case 2:
                        break;

                    case 3: //クエスト選択中
                        break;

                    case 4: //うわさ話聞き中
                        break;

                    case 100: //退避
                        break;

                    default:
                        break;


                }
            }
        }
    }


    public void OnCheck_1() //
    {

    }

    public void OnCheck_2() //話す
    {
        if (shopon_toggle_talk.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_talk.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 2; //眺めるを押したときのフラグ
            GameMgr.Scene_Select = 2;

            //_text.text = "なぁに？お話する？";

            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.talk_flag = true;
            GameMgr.talk_number = 100;

            StartCoroutine("UtageEndWait");
        }
    }

    public void OnCheck_3() //依頼
    {
        if (shopon_toggle_quest.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_quest.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            shopquestlist_obj.SetActive(true); //ショップリスト画面を表示。
            backshopfirst_obj.SetActive(true);
            shop_select.SetActive(false);
            placename_panel.SetActive(false);
            //money_status_obj.SetActive(false);

            GameMgr.Scene_Status = 3; //クエストを押したときのフラグ
            GameMgr.Scene_Select = 3;

            _text.text = "いまは、こんな依頼があるわよ。どれをうける？";

            //カメラ寄る。
            trans++; //transが1を超えたときに、ズームするように設定されている。

            //intパラメーターの値を設定する.
            maincam_animator.SetInteger("trans", trans);

        }
    }

    public void OnCheck_4() //うわさ話　一回100Gとかで、ランダムで有用な情報をきける。
    {
        if (shopon_toggle_uwasa.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_uwasa.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 4; //クエストを押したときのフラグ
            GameMgr.Scene_Select = 4;

            //_text.text = "これはうわさ話なんだけど..聞く？　一回100Gいただくわ。";

            //カメラ寄る。
            trans = 10; //transが1を超えたときに、ズームするように設定されている。

            //intパラメーターの値を設定する.
            maincam_animator.SetInteger("trans", trans);

            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.uwasa_flag = true;

            //一度読んだうわさ話は出ない。
            InitUwasaList();

            StartCoroutine("UtageEndWait");
        }
    }

    public void OnCheck_5() //
    {

    }

    public void OnCheck_6() //アイテムをあげる
    {
        if (shopon_toggle_present.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_present.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 6; //クエストを押したときのフラグ
            GameMgr.Scene_Select = 6;

            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.talk_flag = true;
            GameMgr.talk_number = 500;
            GameMgr.utage_charaHyouji_flag = true;

            //アイテムを使用するときのフラグ
            GameMgr.event_pitem_use_select = true;
            GameMgr.bar_event_ON = true;

            //下は、使うときだけtrueにすればOK
            GameMgr.KoyuJudge_ON = true;//固有のセット判定を使う場合は、使うを宣言するフラグと、そのときのGirlLikeSetの番号も入れる。
            GameMgr.KoyuJudge_num = GameMgr.Bar_Okashi_num01;//GirlLikeSetの番号を直接指定
            GameMgr.NPC_Dislike_UseON = true; //判定時、そのお菓子の種類が合ってるかどうかのチェックもする

            StartCoroutine("UtageEndWait");

        }
    }

    public void OnCheck_Back() //ショップからでて、広場に戻る
    {
        if (shopon_toggle_back.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_back.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。


            switch (GameMgr.Scene_Name)
            {
                case "Or_Bar_A1": //春エリア

                    GameMgr.SceneSelectNum = 11;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                case "Or_Bar_B1": //夏エリア

                    GameMgr.SceneSelectNum = 11;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                case "Or_Bar_C1": //秋エリア

                    GameMgr.SceneSelectNum = 11;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                case "Or_Bar_D1": //冬エリア

                    GameMgr.SceneSelectNum = 11;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                default:


                    break;
            }

        }
    }


    //ショップの品数が増えるなど、パティシエレベルや好感度に応じたイベントの発生フラグをチェック
    void CheckBarLvEvent()
    {

    }

    IEnumerator UtageEndWait()
    {
        GameMgr.compound_select = 1000; //シナリオイベント読み中の状態
        GameMgr.compound_status = 1000;

        while (GameMgr.scenario_ON)
        {
            yield return null;
        }

        GameMgr.compound_select = 0; //何もしていない状態
        GameMgr.compound_status = 0;

        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;
    }

    IEnumerator Scenario_loading()
    {
        //Debug.Log("シナリオ開始");
        check_lvevent = true;

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        //Debug.Log("シナリオ終了");
        GameMgr.scenario_read_endflag = false;
        GameMgr.scenario_ON = false;

        check_event = false;
        check_lvevent = false;
        lvevent_loading = false;
        GameMgr.Scene_Status = 0;

    }

    //
    //ショップうわさ関係
    //
    void InitUwasaList()
    {
        shopuwasa_List.Clear();
        uwasalist_count = 5;
        count = 0;

        //***  うわさリスト選択 ***//

        if (GameMgr.Story_Mode == 0)
        {
            //クッキー作り開始　初期値
            for (i = 0; i < uwasalist_count; i++) //頭から５個ずつ
            {
                shopuwasa_List.Add(GameMgr.ShopUwasa_stage1[i]);
            }

            //ラスク
            if (GameMgr.GirlLoveEvent_stage1[10])
            {
                count++;
                for (i = 0; i < uwasalist_count; i++) //頭から５個ずつ
                {
                    shopuwasa_List.Add(GameMgr.ShopUwasa_stage1[i + (5 * count)]);
                }
            }
            //クレープ
            if (GameMgr.GirlLoveEvent_stage1[20])
            {
                count++;
                for (i = 0; i < uwasalist_count; i++) //頭から５個ずつ
                {
                    shopuwasa_List.Add(GameMgr.ShopUwasa_stage1[i + (5 * count)]);
                }
            }
            //シュークリーム
            if (GameMgr.GirlLoveEvent_stage1[30])
            {
                count++;
                for (i = 0; i < uwasalist_count; i++) //頭から５個ずつ
                {
                    shopuwasa_List.Add(GameMgr.ShopUwasa_stage1[i + (5 * count)]);
                }
            }
            //ドーナツ
            if (GameMgr.GirlLoveEvent_stage1[40])
            {
                count++;
                for (i = 0; i < uwasalist_count; i++) //頭から５個ずつ
                {
                    shopuwasa_List.Add(GameMgr.ShopUwasa_stage1[i + (5 * count)]);
                }
            }
            //コンテスト
            if (GameMgr.GirlLoveEvent_stage1[50])
            {
                count++;
                for (i = 0; i < uwasalist_count; i++) //頭から５個ずつ
                {
                    shopuwasa_List.Add(GameMgr.ShopUwasa_stage1[i + (5 * count)]);
                }
            }
        }
        else
        {
            //エクストラモードは全てでてる。
            for (i = 0; i < uwasalist_count * 6; i++) //頭から５個ずつ
            {
                shopuwasa_List.Add(GameMgr.ShopUwasa_stage1[i]);
            }
        }

        //ランダムで噂を選ぶメソッド
        uwasa_randomselect();

        if (random_uwasa_select.Count > 0)
        {
        }
        else //もしきける話を全て聞いていた場合
        {
            Debug.Log("うわさ番号　リセット");
            //きける話を全てリセットして、もっかい抽選
            for (i = 0; i < shopuwasa_List.Count; i++)
            {
                shopuwasa_List[i] = false; //すべてのうわさの聞いたフラグをリセット

            }

            for (i = 0; i < GameMgr.ShopUwasa_stage1.Length; i++)
            {
                GameMgr.ShopUwasa_stage1[i] = false;
            }


            uwasa_randomselect();
            //GameMgr.uwasa_number = 9999; //うわさ話はすべて聞いたというフラグ
        }

        rnd = Random.Range(0, random_uwasa_select.Count);
        GameMgr.uwasa_number = random_uwasa_select[rnd];

        Debug.Log("選ばれたうわさ番号: " + GameMgr.uwasa_number);

    }

    void uwasa_randomselect()
    {
        random_uwasa_select.Clear();

        for (i = 0; i < shopuwasa_List.Count; i++)
        {
            if (!shopuwasa_List[i]) //まだうわさをきいてないやつだけをランダムで選ばれるようにする。
            {
                random_uwasa_select.Add(i);
            }
            //Debug.Log("shopuwasa_List: " + shopuwasa_List[i]);
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
