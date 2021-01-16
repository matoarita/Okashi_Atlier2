using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GetMatPlace_Panel : MonoBehaviour {

    private ItemMatPlaceDataBase matplace_database;
    private ItemDataBase database;

    private GameObject canvas;
    private Texture2D texture2d;
    private Sprite map_icon;

    private BGM sceneBGM;
    private SoundController sc;
    private Map_Ambience map_ambience;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private Girl1_status girl1_status;

    private TimeController time_controller;
    private GameObject TimePanel_obj1;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private GameObject content;

    private GameObject getmatplace_view;
    private GameObject getmatResult_panel_obj;
    private GetMatResult_Panel getmatResult_panel;
    private GameObject slot_view;
    private GameObject slot_tansaku_button_obj;
    private GameObject slot_tansaku_button;
    private Image slot_view_image;

    private List<GameObject> mapevent_panel = new List<GameObject>();
    private GameObject event_panel;

    private GameObject moveanim_panel;
    private GameObject moveanim_panel_image;
    private GameObject moveanim_panel_image_text;

    private GameObject matplace_toggle_obj;
    public List<GameObject> matplace_toggle = new List<GameObject>();

    private GameObject text_area;
    private Text _text;
    private string _temp_tx;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject yes_no_panel; //通常時のYes, noボタン

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject get_material_obj;
    private GetMaterial get_material;

    private GameObject map_imageBG;
    private Texture2D texture2d_map;

    private GameObject map_bg_effect;

    private int mapid;

    private int select_place_num;
    private string select_place_name;
    private int select_place_day;
    private int _place_num;

    private bool Slot_view_on;
    public int slot_view_status;

    private int i, j;
    private int select_num;

    private bool move_anim_on;
    private bool move_anim_end;
    private int move_anim_status;
    private float timeOut;

    private bool modoru_anim_on;
    private bool modoru_anim_end;
    private int modoru_anim_status;

    private bool treasure_anim_on;
    private int treasure_anim_status;

    private int _yosokutime;
    private int mat_cost;

    private GameObject sister_stand_img1;

    public Dictionary<int, int> result_items;
    private bool result_off;

    public int next_flag; //先へ進める場合のイベントナンバー
    private bool next_on; //先へ進んだ場合、ON

    private GameObject NextButton_obj;
    private GameObject OpenTreasureButton_obj;

    private GameObject HeroineLifePanel;
    private Text HeroineLifeText;
    private int HeroineLife;

    // Use this for initialization
    void Start()
    {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //調合シーンメインオブジェクトの取得
        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //Yes no パネルの取得
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //時間管理オブジェクトの取得
        TimePanel_obj1 = canvas.transform.Find("MainUIPanel/TimePanel").gameObject;
        time_controller = canvas.transform.Find("MainUIPanel/TimePanel").GetComponent<TimeController>();
        
        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子  

        //ヒロインライフパネル
        HeroineLifePanel = this.transform.Find("Comp/HeroineLife").gameObject;
        HeroineLifeText = HeroineLifePanel.transform.Find("HPguage/HPparam").GetComponent<Text>();
        

        //移動中アニメーション用パネルの取得
        moveanim_panel = this.transform.Find("Comp/MoveAnimPanel").gameObject;
        moveanim_panel_image = this.transform.Find("Comp/MoveAnimPanel/moveImage").gameObject;
        moveanim_panel_image_text = this.transform.Find("Comp/MoveAnimPanel/moveImage/Text").gameObject;

        //Yes no を判別する用のオブジェクトの取得
        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //アイテムセレクトキャンセルオブジェクトの取得
        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        //材料ランダムで３つ手に入るオブジェクトの取得
        get_material_obj = GameObject.FindWithTag("GetMaterial");
        get_material = get_material_obj.GetComponent<GetMaterial>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = canvas.transform.Find("MainUIPanel/MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        getmatplace_view = this.transform.Find("Comp/GetMatPlace_View").gameObject;
        getmatResult_panel_obj = canvas.transform.Find("GetMatResult_Panel/Comp").gameObject;
        getmatResult_panel = canvas.transform.Find("GetMatResult_Panel").GetComponent<GetMatResult_Panel>();

        //マップ背景エフェクト
        map_bg_effect = GameObject.FindWithTag("MapBG_Effect");

        content = getmatplace_view.transform.Find("Viewport/Content").gameObject;
        matplace_toggle_obj = (GameObject)Resources.Load("Prefabs/MatPlace_toggle1");

        matplace_toggle.Clear();

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        for (i = 0; i < matplace_database.matplace_lists.Count; i++)
        {
            //Debug.Log(child.name);           
            matplace_toggle.Add(Instantiate(matplace_toggle_obj, content.transform));
            map_icon = matplace_database.matplace_lists[i].mapIcon_sprite;
            matplace_toggle[i].transform.Find("Background").GetComponent<Image>().sprite = map_icon;
            matplace_toggle[i].GetComponentInChildren<Text>().text = matplace_database.matplace_lists[i].placeNameHyouji;
            matplace_toggle[i].GetComponent<matplaceSelectToggle>().placeNum = i; //トグルにIDを割り振っておく。
        }

        //採取地画面の取得
        slot_view = this.transform.Find("Comp/Slot_View").gameObject;
        slot_view_image = this.transform.Find("Comp/Slot_View/Image").gameObject.GetComponent<Image>();
        slot_tansaku_button_obj = slot_view.transform.Find("Tansaku_panel").gameObject;
        slot_tansaku_button = slot_tansaku_button_obj.transform.Find("TansakuActionList/Viewport/Content").gameObject;

        NextButton_obj = slot_tansaku_button_obj.transform.Find("TansakuActionList/Viewport/Content/Next_tansaku").gameObject;
        OpenTreasureButton_obj = slot_tansaku_button_obj.transform.Find("TansakuActionList/Viewport/Content/Open_treasure").gameObject;

        i = 0;
        foreach (Transform child in slot_view.transform.Find("EventPanel/").transform)
        {
            //Debug.Log(child.name);           
            mapevent_panel.Add(child.gameObject);
            mapevent_panel[i].SetActive(false);
            i++;
        }
        event_panel = slot_view.transform.Find("EventPanel/").gameObject;


        map_imageBG = this.transform.Find("Comp/Map_ImageBG").gameObject;
        map_imageBG.SetActive(false);

        //妹立ち絵の取得
        sister_stand_img1 = this.transform.Find("Comp/Slot_View/SisterPanel/Girl_Tachie").gameObject;
        sister_stand_img1.SetActive(false);

        select_place_num = 0;

        Slot_view_on = false;
        slot_view_status = 0;

        next_on = false;

        InitializeResultItemDicts(); //取得したアイテム表示用のディクショナリー
    }

    private void OnEnable()
    {
        
    }

    //外から読み込む。初期化用。
    public void SetInit()
    {
        //スロットビューは最初Off
        slot_view.SetActive(false);

        //背景マップも最初はOff
        map_imageBG.SetActive(false);

        //最初は、採取地選択画面をonに。
        getmatplace_view.SetActive(true);

        moveanim_panel.SetActive(false);

        move_anim_on = false;
        modoru_anim_on = false;
        treasure_anim_on = false;

        //表示フラグにそって、採取地の表示/非表示の決定
        for (i = 0; i < matplace_toggle.Count; i++)
        {
            if (matplace_database.matplace_lists[i].placeFlag == 1)
            {
                matplace_toggle[i].SetActive(true);
            }
            else
            {
                matplace_toggle[i].SetActive(false);
            }
        }

        //妹の体力（HP)を表示
        //HeroineLifeText.text = PlayerStatus.player_girl_lifepoint.ToString();
        HeroineLifeText.text = PlayerStatus.girl1_Love_exp.ToString();
    }

    // Update is called once per frame
    void Update () {

        if (move_anim_on == true)
        {
            //移動中のウェイトアニメ
            MoveAnim();
        }

        if (move_anim_end == true)
        {
            move_anim_end = false;
            Slot_view_on = true;
            slot_view_status = 0;
        }

        if ( Slot_view_on == true )
        {
            //シーン移動のマップは、そのままシーン移動
            switch (select_place_name)
            {
                case "Hiroba":

                    Slot_view_on = false;
                    FadeManager.Instance.LoadScene("Hiroba2", 0.3f);
                    break;

                case "Shop":

                    Slot_view_on = false;
                    FadeManager.Instance.LoadScene("Shop", 0.3f);
                    break;

                case "Emerald_Shop":

                    Slot_view_on = false;
                    FadeManager.Instance.LoadScene("Emerald_Shop", 0.3f);
                    break;

                case "Farm":

                    Slot_view_on = false;
                    FadeManager.Instance.LoadScene("Farm", 0.3f);
                    break;

                default:
                    
                    //採取地表示
                    Slot_View();
                    break;
            }                    
        }

        if (treasure_anim_on == true)
        {
            //宝箱開け中のウェイトアニメ
            TreasureAnim();
        }

        if (modoru_anim_on == true)
        {
            //移動中のウェイトアニメ
            ModoruAnim();
        }

        if (modoru_anim_end == true)
        {
            //帰還できたらリセット
            modoru_anim_end = false;
            compound_Main.compound_status = 0;

            foreach (Transform child in slot_tansaku_button.transform) // 
            {
                child.GetComponent<Button>().interactable = true;
            }
            

            foreach (Transform map_bg_child in map_bg_effect.transform) // map_bg_effect以下のオブジェクトをoff
            {
                map_bg_child.gameObject.SetActive(false);
            }

            slot_view.SetActive(false);

            StatusPanelON();
            this.transform.Find("Comp/Map_ImageBG_FadeBlack").GetComponent<CanvasGroup>().DOFade(0, 0.0f); //背景黒フェードをOFF

            //girl1_status.hukidasiOn();

            //音量フェードイン
            sceneBGM.FadeInBGM();
            

            //ガチャン ドア開く音鳴らす。
            sc.PlaySe(38);
            sc.PlaySe(50);

            //立ち絵もオフ
            sister_stand_img1.SetActive(false);

            //日数の経過。帰りも同じ時間かかる。
            PlayerStatus.player_time += select_place_day;
            time_controller.TimeKoushin();

            //ハートゲージを更新。
            compound_Main.HeartGuageTextKoushin();

            _text.text = "家に戻ってきた。どうしようかなぁ？";

            //リザルトパネルを表示
            result_off = false;
            getmatResult_panel_obj.SetActive(true);
            getmatResult_panel.reset_and_DrawView();
            getmatResult_panel.OnStartAnim();
            StartCoroutine("ResultOn");
        }
    }

    public void OnClick_Place(int place_num)
    {
        i = 0;
        _place_num = place_num;
        while (i < matplace_toggle.Count)
        {
            if (matplace_toggle[i].GetComponent<Toggle>().isOn == true)
            {
                select_num = i;

                //妹の体力が足りてるかチェック
                if (PlayerStatus.girl1_Love_exp <= 0 && matplace_database.matplace_lists[_place_num].placeType == 1) //0以下かつダンジョンタイプに行こうとする場合
                {
                    _text.text = "にいちゃん。怖くて外にでれないよ～・・。" + "\n" + "まずは、" + GameMgr.ColorYellow  + "ヒカリのハートをあげて" + "</color>" + "ね！";

                    All_Off();
                }
                else
                {
                    //時間が20時をこえないかチェック
                    if (GameMgr.TimeUSE_FLAG)
                    {
                        _yosokutime = PlayerStatus.player_time + (matplace_database.matplace_lists[_place_num].placeDay); //行きの時間だけ計算
                        if (_yosokutime >= time_controller.max_time * 6)
                        {
                            //20時を超えるので、妹に止められる。
                            _text.text = "兄ちゃん。今日は遅いから、明日いこ～。";
                            All_Off();
                        }
                        else
                        {
                            KakuninPlace();
                            break;
                        }
                    }
                    else
                    {
                        KakuninPlace();
                        break;
                    }
                }

            }
            i++;
        }
    }

    void KakuninPlace()
    {
        if (matplace_database.matplace_lists[i].placeCost == 0)
        {
            _text.text = matplace_database.matplace_lists[_place_num].placeNameHyouji + "へ行きますか？";
        }
        else
        {
            _text.text = matplace_database.matplace_lists[_place_num].placeNameHyouji + "へ行きますか？" + "\n" + "探索費用：" + GameMgr.ColorYellow + matplace_database.matplace_lists[i].placeCost.ToString() + GameMgr.MoneyCurrency + "</color>";
        }

        select_place_num = _place_num;
        select_place_name = matplace_database.matplace_lists[_place_num].placeName;
        select_place_day = matplace_database.matplace_lists[_place_num].placeDay;

        Select_Pause();
    }

    void Select_Pause()
    {
        itemselect_cancel.kettei_on_waiting = true; //今、トグルをおして、選択中の状態

        for(j = 0; j < matplace_toggle.Count; j++ )
        {
            matplace_toggle[j].GetComponent<Toggle>().interactable = false;
        }

        yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);

        StartCoroutine("Place_kakunin");
    }


    IEnumerator Place_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //お金が足りてるかチェック
                mat_cost = matplace_database.matplace_lists[select_place_num].placeCost;
                if (PlayerStatus.player_money < mat_cost)
                {
                    _text.text = "にいちゃん。お金が足りないよ～・・。";

                    All_Off();
                }
                else
                {
                    //Debug.Log("ok");
                    //解除

                    itemselect_cancel.kettei_on_waiting = false;

                    yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                    //採取地確定したので、採取地の番号に従って、ランダムで３つアイテム取得＋金額を消費するメソッドへいく。

                    move_anim_on = true;
                    move_anim_status = 0;

                    next_on = false;

                    girl1_status.hukidasiOff();

                    //音量フェードアウト
                    sceneBGM.FadeOutBGM();

                    //日数の経過。場所ごとに、移動までの日数が変わる。
                    PlayerStatus.player_time += select_place_day;
                    time_controller.TimeKoushin();

                    //お金の消費
                    moneyStatus_Controller.UseMoney(mat_cost);

                    Random.InitState(GameMgr.Game_timeCount); //シード値をバラバラに変える。ゲーム内タイマーで変える。
                }

                break;


            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                All_Off();

                _text.text = "行き先を選んでね。";
                break;
        }

    }

    void Slot_View()
    {
        switch (slot_view_status)
        {
            case 0: //初期化

                moveanim_panel.GetComponent<FadeImage>().FadeImageOff();
                moveanim_panel_image.SetActive(false);
                moveanim_panel_image_text.SetActive(false);
                moveanim_panel_image.GetComponent<CanvasGroup>().DOFade(1, 0.0f);
                this.transform.Find("Comp/Map_ImageBG_FadeBlack").GetComponent<CanvasGroup>().DOFade(0, 0.5f); //背景黒フェード

                if (next_on) //先へ進む場合、背景も黒フェードを消す
                {                    
                    text_area.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
                }

                getmatplace_view.SetActive(false);
                slot_view.SetActive(true);
                yes_no_panel.SetActive(false);
                map_imageBG.SetActive(true);
                slot_tansaku_button_obj.SetActive(true);

                if (!next_on)//先へ進むの場合は、リセットしない。
                {
                    InitializeResultItemDicts();
                    get_material.SetInit();
                } 

                slot_view_status = 1;
                compound_Main.compound_status = 21;
                
                //音量フェードイン
                sceneBGM.FadeInBGM();

                //背景のセッティング
                SetMapBG(select_place_name);

                //ステータスパネルをON
                StatusPanelON();

                switch (select_place_name)
                {

                    case "Forest":
                                             
                        //森のBGM
                        sceneBGM.OnGetMat_ForestBGM();
                        compound_Main.bgm_change_flag = true;

                        //背景エフェクト
                        map_bg_effect.transform.Find("MapBG_Effect_Forest").gameObject.SetActive(true);

                        //イベントチェック
                        if (!GameMgr.MapEvent_01[0])
                        {
                            GameMgr.MapEvent_01[0] = true;

                            _text.text = "すげぇ～～！森だー！";

                            slot_view_status = 3; //イベント読み込み中用に退避

                            //初森へきたイベントを再生。再生終了したら、イベントパネルをオフにし、探索ボタンもONにする。
                            slot_tansaku_button_obj.SetActive(false);

                            //各イベントの再生用オブジェクト。このパネルをONにすると、イベントが再生される。
                            event_panel.transform.Find("MapEv_FirstForest").gameObject.SetActive(true);
                            text_area.SetActive(false);

                            GameMgr.map_ev_ID = 10;
                            GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                            StartCoroutine("MapEventOn");
                        }
                        else
                        {
                            _text.text = "兄ちゃん、いっぱいとろうね！";
                        }

                        break;

                    case "Lavender_field":

                        //森のBGM
                        sceneBGM.OnGetMat_LavenderFieldBGM();
                        compound_Main.bgm_change_flag = true;

                        //背景のSEを鳴らす。
                        map_ambience.OnLavenderField();

                        //背景エフェクト
                        map_bg_effect.transform.Find("MapBG_Effect_Lavender").gameObject.SetActive(true);

                        //イベントチェック
                        if (!GameMgr.MapEvent_05[0])
                        {
                            GameMgr.MapEvent_05[0] = true;

                            _text.text = "ラベンダー畑だ～！いい香り～。";

                            slot_view_status = 3; //イベント読み込み中用に退避

                            //初森へきたイベントを再生。再生終了したら、イベントパネルをオフにし、探索ボタンもONにする。
                            slot_tansaku_button_obj.SetActive(false);

                            //各イベントの再生用オブジェクト。このパネルをONにすると、イベントが再生される。
                            event_panel.transform.Find("MapEv_FirstLavender").gameObject.SetActive(true);
                            text_area.SetActive(false);

                            GameMgr.map_ev_ID = 60;
                            GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                            StartCoroutine("MapEventOn");
                        }
                        else
                        {
                            _text.text = "兄ちゃん、ちょっとゴロゴロしよ～！";
                        }

                        break;

                    case "StrawberryGarden":

                        //ストロベリーガーデンのBGM
                        sceneBGM.OnGetMat_StrawberryGardenBGM();
                        compound_Main.bgm_change_flag = true;

                        //背景エフェクト
                        map_bg_effect.transform.Find("MapBG_Effect_StrawberryGarden").gameObject.SetActive(true);

                        //イベントチェック
                        if (!GameMgr.MapEvent_03[0])
                        {
                            GameMgr.MapEvent_03[0] = true;

                            _text.text = "いいにお～い。兄ちゃん、いちごいっぱい～！";

                            slot_view_status = 3; //イベント読み込み中用に退避

                            //イベントを再生。再生終了したら、イベントパネルをオフにし、探索ボタンもONにする。
                            slot_tansaku_button_obj.SetActive(false);

                            //各イベントの再生用オブジェクト。このパネルをONにすると、イベントが再生される。
                            event_panel.transform.Find("MapEv_FirstHimawari").gameObject.SetActive(true);
                            text_area.SetActive(false);

                            GameMgr.map_ev_ID = 40;
                            GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                            StartCoroutine("MapEventOn");
                        }
                        else
                        {
                            _text.text = "兄ちゃん、いちごたくさんー！";
                        }

                        break;

                    case "HimawariHill":

                        //ひまわり畑のBGM
                        sceneBGM.OnGetMat_HimawariHillBGM();
                        compound_Main.bgm_change_flag = true;

                        //背景エフェクト
                        map_bg_effect.transform.Find("MapBG_Effect_Himawari").gameObject.SetActive(true);

                        //イベントチェック
                        if (!GameMgr.MapEvent_04[0])
                        {
                            GameMgr.MapEvent_04[0] = true;

                            _text.text = "兄ちゃん。まっ黄色～～！すごいきれい～。";

                            slot_view_status = 3; //イベント読み込み中用に退避

                            //イベントを再生。再生終了したら、イベントパネルをオフにし、探索ボタンもONにする。
                            slot_tansaku_button_obj.SetActive(false);

                            //各イベントの再生用オブジェクト。このパネルをONにすると、イベントが再生される。
                            event_panel.transform.Find("MapEv_FirstHimawari").gameObject.SetActive(true);
                            text_area.SetActive(false);

                            GameMgr.map_ev_ID = 50;
                            GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                            StartCoroutine("MapEventOn");
                        }
                        else
                        {
                            _text.text = "兄ちゃん、種とりは任せてね！";
                        }

                        break;

                    case "BirdSanctuali":

                        //バードサンクチュアリのBGM
                        sceneBGM.OnGetMat_BirdSanctualiBGM();
                        compound_Main.bgm_change_flag = true;

                        //背景エフェクト
                        map_bg_effect.transform.Find("MapBG_Effect_Himawari").gameObject.SetActive(true);

                        //イベントチェック
                        if (!GameMgr.MapEvent_06[0])
                        {
                            GameMgr.MapEvent_06[0] = true;

                            _text.text = "兄ちゃん。とりさんがいっぱいいるよ～！！";

                            slot_view_status = 3; //イベント読み込み中用に退避

                            //イベントを再生。再生終了したら、イベントパネルをオフにし、探索ボタンもONにする。
                            slot_tansaku_button_obj.SetActive(false);

                            //各イベントの再生用オブジェクト。このパネルをONにすると、イベントが再生される。
                            event_panel.transform.Find("MapEv_FirstBirdSanctuali").gameObject.SetActive(true);
                            text_area.SetActive(false);

                            GameMgr.map_ev_ID = 20;
                            GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                            //次回以降、バードサンクチュアリにいけるようになる。
                            matplace_database.matPlaceKaikin("BirdSanctuali");

                            StartCoroutine("MapEventOn");
                        }
                        else
                        {
                            _text.text = "兄ちゃん。とりさんとあそぼ！！";
                        }

                        break;

                    case "Ido":
                        
                        //井戸のBGM
                        sceneBGM.OnGetMat_IdoBGM();
                        compound_Main.bgm_change_flag = true;
                        
                        //イベントチェック
                        if (!GameMgr.MapEvent_02[0])
                        {
                            GameMgr.MapEvent_02[0] = true;

                            _text.text = "いっぱい水を汲もう。兄ちゃん。";

                            slot_view_status = 3; //イベント読み込み中用に退避

                            slot_tansaku_button_obj.SetActive(false);

                            //各イベントの再生用オブジェクト。このパネルをONにすると、イベントが再生される。
                            event_panel.transform.Find("MapEv_FirstIdo").gameObject.SetActive(true);
                            text_area.SetActive(false);

                            GameMgr.map_ev_ID = 2;
                            GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                            StartCoroutine("MapEventOn");
                        }
                        else
                        {
                            _text.text = "兄ちゃん、今日も水汲み？私も手伝うー！";
                        }
                        break;

                    default:
                        break;
                }                

                break;

            case 1: //探索するかどうかの入力まち

                break;

            case 2: //戻るかどうかの入力まち
                break;

            case 3: //イベントよみこみ中
                break;

            default:
                break;
        }
        
    }

    IEnumerator modoru_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //Debug.Log("ok");
                //解除

                //全ての処理が完了し、家に戻るときに、以下の処理でリセット
                All_Off();

                slot_tansaku_button_obj.SetActive(false);

                modoru_anim_on = true;

                //音量フェードアウト
                sceneBGM.FadeOutBGM();


                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");
                slot_view_status = 1;

                foreach (Transform child in slot_tansaku_button.transform) // 
                {
                    child.GetComponent<Button>().interactable = true;
                }

                yes_no_panel.SetActive(false);

                _text.text = "";

                yes_selectitem_kettei.onclick = false;
                break;
        }

    }

    void All_Off()
    {
        matplace_toggle[select_num].GetComponent<Toggle>().isOn = false;

        //再度、セレクトできるようにする
        for (i = 0; i < matplace_toggle.Count; i++)
        {
            
            matplace_toggle[i].GetComponent<Toggle>().interactable = true;
            
            //Debug.Log(matplace_toggle[i].GetComponent<Toggle>().interactable);
        }

        itemselect_cancel.kettei_on_waiting = false;

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        yes_no_panel.transform.Find("Yes").gameObject.SetActive(false);
    }    

    void MoveAnim()
    {
        switch (move_anim_status)
        {
            case 0: //初期化 状態１

                timeOut = 1.0f;
                move_anim_status = 1;
                move_anim_end = false;

                yes_no_panel.SetActive(false);                

                moveanim_panel.SetActive(true);
                moveanim_panel.GetComponent<FadeImage>().SetOn();
                moveanim_panel_image.SetActive(true);
                moveanim_panel_image_text.SetActive(true);

                StatusPanelOFF();
                this.transform.Find("Comp/Map_ImageBG_FadeBlack").GetComponent<CanvasGroup>().DOFade(1, 0.5f); //背景黒フェード

                if (next_on) //先へ進む場合
                {
                    
                }

                _text.text = "移動中 .";
                moveanim_panel_image_text.GetComponent<Text>().text = "移動中 .";
                break;

            case 1: // 状態2

                if (timeOut <= 0.0)
                {
                    //一度不要なものはリセット
                    foreach (Transform map_bg_child in map_bg_effect.transform) // map_bg_effect以下のオブジェクトをoff
                    {
                        map_bg_child.gameObject.SetActive(false);
                    }

                    timeOut = 1.0f;
                    move_anim_status = 2;

                    _text.text = "移動中 . .";
                    moveanim_panel_image_text.GetComponent<Text>().text = "移動中 . .";
                }
                break;

            case 2:

                if (timeOut <= 0.0)
                {
                    if (next_on)
                    {
                        timeOut = 2.0f; //先へ進んでるときは、少し長く
                        move_anim_status = 3;

                        _text.text = "移動中 . . .";
                        moveanim_panel_image_text.GetComponent<Text>().text = "移動中 . . .";
                    }
                    else
                    {
                        timeOut = 1.0f;
                        move_anim_status = 10;
                    }
                                      
                }
                break;

            case 3:

                
                moveanim_panel_image.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
                text_area.GetComponent<CanvasGroup>().DOFade(0, 0.5f);

                if (timeOut <= 0.0)
                {
                    timeOut = 1.0f;
                    move_anim_status = 10;

                }
                break;

            case 10: //アニメ終了。判定する

                move_anim_on = false;
                move_anim_end = true;                
                move_anim_status = 0;

                break;

            default:
                break;
        }

        //時間減少
        timeOut -= Time.deltaTime;
    }

    void ModoruAnim()
    {
        switch (modoru_anim_status)
        {
            case 0: //初期化 状態１

                timeOut = 1.0f;
                modoru_anim_status = 1;
                modoru_anim_end = false;

                yes_no_panel.SetActive(false);

                Slot_view_on = false;                

                moveanim_panel.GetComponent<FadeImage>().FadeImageOn();
                moveanim_panel_image.SetActive(true);
                moveanim_panel_image_text.SetActive(true);

                StatusPanelOFF();
                this.transform.Find("Comp/Map_ImageBG_FadeBlack").GetComponent<CanvasGroup>().DOFade(1, 0.5f); //背景黒フェード

                //背景のSEを止める。
                map_ambience.FadeOut();

                _text.text = "帰還中 .";
                moveanim_panel_image_text.GetComponent<Text>().text = "帰還中 .";
                break;

            case 1: // 状態2

                if (timeOut <= 0.0)
                {
                    timeOut = 1.0f;
                    modoru_anim_status = 2;

                    _text.text = "帰還中 . .";
                    moveanim_panel_image_text.GetComponent<Text>().text = "帰還中 . .";
                }
                break;

            case 2:

                if (timeOut <= 0.0)
                {
                    timeOut = 1.0f;
                    modoru_anim_status = 3;


                }
                break;

            case 3: //アニメ終了。判定する

                modoru_anim_on = false;
                modoru_anim_end = true;

                moveanim_panel.SetActive(false);
                moveanim_panel_image.SetActive(false);
                moveanim_panel_image_text.SetActive(false);                
                modoru_anim_status = 0;

                break;

            default:
                break;
        }

        //時間減少
        timeOut -= Time.deltaTime;
    }

    IEnumerator MapEventOn()
    {
        StatusPanelOFF();

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;

        StatusPanelON();

        text_area.SetActive(true);
        slot_tansaku_button_obj.SetActive(true);
        for(i=0; i<mapevent_panel.Count; i++)
        {
            mapevent_panel[i].SetActive(false);
        }
        

        slot_view_status = 1; //通常の材料集めシーンに切り替え
    }

    void StatusPanelOFF()
    {
        MoneyStatus_Panel_obj.SetActive(false);
        HeroineLifePanel.SetActive(false);
        if (GameMgr.TimeUSE_FLAG)
        {
            TimePanel_obj1.SetActive(false);
        }
    }

    void StatusPanelON()
    {
        MoneyStatus_Panel_obj.SetActive(true);
        HeroineLifePanel.SetActive(true);
        if (GameMgr.TimeUSE_FLAG)
        {
            TimePanel_obj1.SetActive(true);
        }
    }

    public void SisterOn1()
    {
        sister_stand_img1.SetActive(true);
    }

    public void SisterOff1()
    {
        sister_stand_img1.SetActive(false);
    }

    void SetMapBG(string _place_name)
    {
        mapid = matplace_database.SearchMapString(_place_name);

        texture2d = matplace_database.matplace_lists[mapid].center_bg;
        texture2d_map = matplace_database.matplace_lists[mapid].back_bg;

        // texture2dを使い、Spriteを作って、反映させる
        slot_view_image.sprite = Sprite.Create(texture2d,
                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                   Vector2.zero);

        map_imageBG.GetComponent<Image>().sprite = Sprite.Create(texture2d_map,
                                   new Rect(0, 0, texture2d_map.width, texture2d_map.height),
                                   Vector2.zero);
    }


    //リザルト画面を表示
    public void GetMatResultPanelOff()
    {
        sc.PlaySe(30);
        result_off = true;
    }

    IEnumerator ResultOn()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (result_off != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        result_off = false;
        getmatResult_panel_obj.SetActive(false);

        //メインシーンのデフォルトに戻る。
        time_controller.TimeCheck_flag = true; //寝るかどうかの判定する   
        time_controller.TimeKoushin();
        girl1_status.hukidasiOn();

        slot_view_status = 0;
    }


    void InitializeResultItemDicts()
    {
        result_items = new Dictionary<int, int>();

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコア(所持数)と、追加得点用のスコアをつける
        for (i = 0; i < database.items.Count; i++)
        {
            result_items.Add(database.items[i].itemID, 0);
        }
    }

    public void OnTansaku() //探索ボタンをおした
    {
        get_material.GetRandomMaterials(select_place_num);
    }

    public void OnModoru() //街へ戻るをおした
    {
        slot_view_status = 2;

        _text.text = "家に戻る？";

        foreach (Transform child in slot_tansaku_button.transform) // 
        {
            child.GetComponent<Button>().interactable = false;
        }

        yes_no_panel.SetActive(true);

        StartCoroutine("modoru_kakunin");
    }

    //
    //宝箱をあける（怪しい場所を散策するなども、このメソッド）
    //

    public void OnOpenTreasure() //「あける」ボタンをおした
    {
        OpenTreasureButton_obj.SetActive(false);
        slot_tansaku_button_obj.SetActive(false);

        //ハートを３つ消費
        PlayerStatus.girl1_Love_exp -= 3;
        HeroineLifeText.text = PlayerStatus.girl1_Love_exp.ToString();

        treasure_anim_status = 0;
        treasure_anim_on = true;
    }

    void TreasureAnim()
    {
        switch (treasure_anim_status)
        {
            case 0: //初期化 状態１
               
                timeOut = 1.0f;
                treasure_anim_status = 1;

                switch (get_material.Treasure_Status)
                {
                    case 0: //宝箱

                        sc.PlaySe(85); //開錠中ガチャガチャ
                        _text.text = "解錠中 .";
                        _temp_tx = "解錠中 . .";
                        break;

                    case 1: //あやしい場所を探索

                        sc.PlaySe(24); //シャカシャカ
                        _text.text = "探索中 .";
                        _temp_tx = "探索中 . .";
                        break;


                    default:
                        
                        break;
                }
                
                break;

            case 1: // 状態2

                if (timeOut <= 0.0)
                {
                    timeOut = 1.0f;
                    treasure_anim_status = 2;

                    _text.text = _temp_tx;
                }
                break;

            case 2:

                if (timeOut <= 0.0)
                {
                    timeOut = 1.0f;
                    treasure_anim_status = 3;

                }
                break;

            case 3: //アニメ終了。判定する

                treasure_anim_on = false;
                treasure_anim_status = 0;

                TreasureAnimEnd();
                break;

            default:
                break;
        }

        //時間減少
        timeOut -= Time.deltaTime;
    }

    void TreasureAnimEnd()
    {
        slot_tansaku_button_obj.SetActive(true);
        get_material.GetTreasureBox(select_place_name);
    }

    //
    //探索関係
    //

    //さらに奥へ進む場合の処理
    public void OnNext() //「先へ進む」ボタンをおした
    {
        //音を鳴らす
        sc.PlaySe(24);       

        move_anim_on = true;
        move_anim_status = 0;

        slot_tansaku_button_obj.SetActive(false);
        NextButton_obj.SetActive(false);

        next_on = true;
        

        switch (next_flag)
        {
            case 100: //バードサンクチュアリを発見

                select_place_name = "BirdSanctuali"; //移動後の場所を指定
                select_place_num = matplace_database.SearchMapString(select_place_name); //次回より、「外へでる」ですぐ行けるよう、フラグ解放
                break;

            default:

                break;
        }
    }   
}
