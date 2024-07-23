using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetMaterial_Main : MonoBehaviour
{

    private GameObject text_area;
    private Text _text;

    private SceneInitSetting sceneinit_setting;


    private ItemMatPlaceDataBase matplace_database;

    private GameObject getmatplace_panel;
    private GetMatPlace_Panel getmatplace;
    private GameObject mapselectBG_panel;

    private PlayerItemList pitemlist;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private SoundController sc;

    private Debug_Panel_Init debug_panel_init;

    private GameObject canvas;

    private GameObject yes_no_panel; //通常時のYes, noボタン

    private BGM sceneBGM;
    private Map_Ambience map_ambience;

    private int ev_id;

    private int i, rndnum;

    private bool StartRead;

    private string default_scenetext;

    private GameObject BGImagePanel;
    private List<GameObject> BGImg_List = new List<GameObject>();
    private List<GameObject> BGImg_List_mago = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 130;

        //Debug.Log("Hiroba scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();


        //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;

        //材料採取地パネルの取得
        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel/Comp").gameObject;
        getmatplace = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();
        getmatplace_panel.SetActive(false);
        mapselectBG_panel = getmatplace_panel.transform.Find("MapSelectBGPanel").gameObject;
        mapselectBG_panel.SetActive(false);

        //初期化
        //getmatplace.SetInit(); //SetInitは、採取地選択画面専用　採取地きてからはしてはいけない

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();
        GameMgr.matbgm_change_flag = false; //BGMをmainListControllerの宴のほうで変えたかどうかのフラグ。変えてた場合、trueで、宴終了後に元のBGMに切り替える。

        //ウィンドウキャラ名設定
        GameMgr.Window_CharaName = GameMgr.mainGirl_Name;

        /*
        //
        //背景と場所名の設定 最初にこれを行う
        //
        BGImagePanel = GameObject.FindWithTag("BG");
        BGImg_List.Clear();
        BGImg_List_mago.Clear();
        i = 0;
        foreach (Transform child in BGImagePanel.transform)　//子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            BGImg_List.Add(child.gameObject);
            BGImg_List[i].SetActive(false);
            i++;
        }

        switch (GameMgr.SceneSelectNum)
        {
            case 0: //デフォルト　アトリエ前

                SettingBGPanel(0); //Map〇〇のリスト番号を指定

                default_scenetext = "アトリエ前だ";
                break;

        }

        //天気対応
        if (GameMgr.WEATHER_TIMEMODE_ON)
        {
            if (GameMgr.Story_Mode != 0)
            {

                switch (GameMgr.BG_cullent_weather) //TimeControllerで変更
                {
                    case 1:

                        break;

                    case 2: //深夜→朝

                        break;

                    case 3: //朝

                        break;

                    case 4: //昼

                        break;

                    case 5: //夕方

                        BGImg_List_mago[1].gameObject.SetActive(true);
                        break;

                    case 6: //夜

                        BGImg_List_mago[2].gameObject.SetActive(true);
                        break;
                }
            }
        }*/
        //** 場所名設定ここまで **//

        //移動時に調合シーンステータスを0に。
        GameMgr.compound_status = 0;
        GameMgr.compound_select = 0;

        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;

        StartRead = false;

        text_scenario();
        text_area.GetComponent<MessageWindow>().DrawIcon(); //顔アイコンの有無　再設定

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    void SettingBGPanel(int _num)
    {
        BGImg_List[_num].gameObject.SetActive(true);

        i = 0;
        foreach (Transform child in BGImg_List[_num].transform) //子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            BGImg_List_mago.Add(child.gameObject);
            BGImg_List_mago[i].SetActive(false);
            i++;
        }
        BGImg_List_mago[0].gameObject.SetActive(true); //朝の画像一番上オブジェクトをON
    }

    void Update()
    {
        if (!StartRead) //シーン最初だけ読み込む
        {
            StartRead = true;
            //GameMgr.Select_place_num = 7; //デバッグ用
            //GameMgr.Select_place_name = "Forest"; //デバッグ用
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            text_area.SetActive(false);
            //placename_panel.SetActive(false);

        }
        else
        {
            switch (GameMgr.Scene_Status)
            {
                case 0:

                    text_area.SetActive(true);
                    //placename_panel.SetActive(true);
                    getmatplace_panel.SetActive(false);
                    yes_no_panel.SetActive(false);

                    text_scenario();

                    sceneBGM.MuteOFFBGM();

                    //GameMgr.Scene_Status = 100;
                    //GameMgr.Scene_Select = 0;

                    GameMgr.Scene_Select = 20;
                    getmatplace.Slot_ViewON();

                    break;

                case 20: //材料採取地を選択中

                    GameMgr.Scene_Status = 21; //
                    GameMgr.Scene_Select = 20; //

                    //UI OFF
                    text_area.SetActive(true);

                    break;

                case 21: //採取地選択中

                    break;

                case 22: //材料採取地に到着。探索中

                    break;

                case 100: //退避

                    break;

                default:

                    break;
            }
        }
    }


    void InitSetting()
    {
        if (playeritemlist_onoff == null)
        {
            //プレイヤー所持アイテムリストパネルの取得
            playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

            //レシピリストパネルの取得
            recipilist_onoff = canvas.transform.Find("RecipiList_ScrollView").gameObject;
            recipilistController = recipilist_onoff.GetComponent<RecipiListController>();
        }
    }

    void text_scenario()
    {
        _text.text = default_scenetext;
    }


    
    void EventReadingStart()
    {
        StartCoroutine("EventReading");
    }

    IEnumerator EventReading()
    {
        GameMgr.hiroba_event_flag = true;
        GameMgr.compound_select = 1000; //シナリオイベント読み中の状態
        GameMgr.compound_status = 1000;

        //Debug.Log("広場イベント　読み中");

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;
        GameMgr.compound_select = 0; //何もしていない状態
        GameMgr.compound_status = 0;

        //読み終わったら、またウィンドウなどを元に戻す。
        text_area.SetActive(true);

        //音を戻す。
        if (GameMgr.matbgm_change_flag)
        {
            GameMgr.matbgm_change_flag = false;
            sceneBGM.FadeInBGM(0.5f);
        }

        text_scenario(); //テキストの更新
    }

    void CanvasOff()
    {
        text_area.SetActive(false);
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
