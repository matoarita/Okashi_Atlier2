using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GetMatPlace_Panel : MonoBehaviour {

    private ItemMatPlaceDataBase matplace_database;
    private ItemDataBase database;
    private PlayerItemList pitemlist;

    private GameObject canvas;    
    private Sprite map_icon;

    private BGM sceneBGM;
    private SoundController sc;
    private Map_Ambience map_ambience;

    private EventDataBase eventdatabase;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private Exp_Controller exp_Controller;
    private SaveController save_controller;

    private Girl1_status girl1_status;
    private GirlEat_Judge girlEat_judge;

    private TimeController time_controller;
    private GameObject TimePanel_obj;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private GameObject content;
    private GameObject GetMatStatusButton_obj;

    private GameObject getmatplace_panel;
    private GameObject getmatplace_view;
    private GameObject getmatResult_panel_obj;
    private GetMatResult_Panel getmatResult_panel;
    private GameObject slot_view;
    private GameObject slot_tansaku_button_obj;
    private GameObject slot_tansaku_button;
    private Image slot_view_image;

    private GameObject moveanim_panel;
    private GameObject moveanim_panel_image;
    private GameObject moveanim_panel_image_text;

    private GameObject matplace_toggle_obj;
    public List<GameObject> matplace_toggle = new List<GameObject>();

    private GameObject text_area;
    private Text _text;
    private GameObject text_kaigyo_button;
    private GameObject text_kaigyo_buttonPanel;
    private string _temp_tx;
    private bool text_kaigyo_active;
    private bool MapList_ReadEnd;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject yes_no_panel; //通常時のYes, noボタン

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject get_material_obj;
    private GetMaterial get_material;

    private GameObject map_imageBG;
    private Sprite texture2d;
    private Sprite texture2d_map;

    private GameObject BG_obj;
    private GameObject BG_Imagepanel;
    private GameObject MapSelect_BGpanel;
    private List<GameObject> MapSelect_Imagepanel_obj = new List<GameObject>();

    private GameObject map_bg_effect;

    private int mapid;
    private int _place_num;
    public int slot_view_status;

    private int i, j, count;
    private int select_num;
    private float _return_blackTime;

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

    //public Dictionary<string, int> result_items;

    private bool subevent_on;
    private bool event_end_flag;

    public int next_flag; //先へ進める場合のイベントナンバー
    private bool next_on; //先へ進んだ場合、ON

    private GameObject NextButton_obj;
    private GameObject OpenTreasureButton_obj;

    private GameObject HeroineLifePanel;
    private Text HeroineLifeText;
    private int HeroineLife;

    private GameObject TreasureGetitem_obj;

    private GameObject Fadeout_Black_obj;

    // Use this for initialization
    void Start()
    {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");       

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //イベントデータベースの取得
        eventdatabase = EventDataBase.Instance.GetComponent<EventDataBase>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();
        text_kaigyo_button = canvas.transform.Find("MessageWindow/KaigyoButton").gameObject;
        text_kaigyo_buttonPanel = canvas.transform.Find("MessageWindow/KaigyoButtonPanel").gameObject;

        //Yes no パネルの取得
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //時間管理オブジェクトの取得
        TimePanel_obj = this.transform.Find("Comp/TimePanelMaterial").gameObject;

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子  

        //GirlEat_Judgeの取得
        girlEat_judge = GirlEat_Judge.Instance.GetComponent<GirlEat_Judge>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //ヒロインライフパネル
        GetMatStatusButton_obj = this.transform.Find("Comp/GetMatStatusPanel").gameObject;
        HeroineLifePanel = GetMatStatusButton_obj.transform.Find("HeroineLife").gameObject;
        HeroineLifeText = HeroineLifePanel.transform.Find("HPguage/HPparam").GetComponent<Text>();
        

        TreasureGetitem_obj = this.transform.Find("Comp/Slot_View/Image/TreasureGetImage").gameObject;

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


        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = this.transform.Find("Comp/MoneyStatus_Matpanel").gameObject;
        moneyStatus_Controller = MoneyStatus_Controller.Instance.GetComponent<MoneyStatus_Controller>();

        //材料採取地パネルの取得
        getmatplace_panel = this.transform.Find("Comp").gameObject;
        getmatplace_view = this.transform.Find("Comp/GetMatPlace_View").gameObject;
        getmatResult_panel_obj = this.transform.Find("GetMatResult_Panel/Comp").gameObject;
        getmatResult_panel = this.transform.Find("GetMatResult_Panel").GetComponent<GetMatResult_Panel>();
        
        MapSelect_BGpanel = getmatplace_panel.transform.Find("MapSelectBGPanel").gameObject;
        MapSelect_Imagepanel_obj.Clear();

        //マップ背景エフェクト
        map_bg_effect = this.transform.Find("MapBG_Effect").gameObject;

        //フェードアウトブラックのオブジェクトを取得
        Fadeout_Black_obj = GameObject.FindWithTag("FadeOutBlack");

        content = getmatplace_view.transform.Find("Viewport/Content").gameObject;
        matplace_toggle_obj = (GameObject)Resources.Load("Prefabs/MatPlace_toggle1");

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        matplace_toggle.Clear();
        count = 0;
        MapList_ReadEnd = false;

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        i = 0;
        while (i < matplace_database.matplace_lists.Count)
        {
            switch (GameMgr.Scene_Name)
            {
                case "Compound":

                    if (matplace_database.matplace_lists[i].matplaceID >= 0)
                    {
                        mapicon_Draw();

                        if (matplace_database.matplace_lists[i].read_end == 1)
                        {
                            MapList_ReadEnd = true;
                            break;
                        }
                    }

                    break;

                /*case "Or_Compound":

                    if (matplace_database.matplace_lists[i].matplaceID >= 100)
                    {
                        mapicon_Draw();

                        if (matplace_database.matplace_lists[i].read_end == 1)
                        {
                            MapList_ReadEnd = true;
                            break;
                        }
                    }
                    break;*/

                default:

                    if (matplace_database.matplace_lists[i].matplaceID >= 100)
                    {
                        mapicon_Draw();

                        if (matplace_database.matplace_lists[i].read_end == 1)
                        {
                            MapList_ReadEnd = true;
                            break;
                        }
                    }
                    break;
            }

            if(MapList_ReadEnd)
            {
                break;
            }
            i++;
        }       

        //採取地画面の取得
        slot_view = this.transform.Find("Comp/Slot_View").gameObject;
        slot_view_image = this.transform.Find("Comp/Slot_View/Image").gameObject.GetComponent<Image>();
        slot_tansaku_button_obj = slot_view.transform.Find("Tansaku_panel").gameObject;
        slot_tansaku_button = slot_tansaku_button_obj.transform.Find("TansakuActionList/Viewport/Content").gameObject;

        NextButton_obj = slot_tansaku_button_obj.transform.Find("TansakuActionList/Viewport/Content/Next_tansaku").gameObject;
        OpenTreasureButton_obj = slot_tansaku_button_obj.transform.Find("TansakuActionList/Viewport/Content/Open_treasure").gameObject;

        //妹立ち絵の取得
        sister_stand_img1 = this.transform.Find("Comp/Slot_View/SisterPanel/GirlTachie_Panel").gameObject;
        sister_stand_img1.SetActive(false);
 

        slot_view_status = 0;

        next_on = false;
        subevent_on = false;

    }

    void mapicon_Draw()
    {
        //Debug.Log(child.name);           
        matplace_toggle.Add(Instantiate(matplace_toggle_obj, content.transform));
        map_icon = matplace_database.matplace_lists[i].mapIcon_sprite;
        matplace_toggle[count].transform.Find("Background").GetComponent<Image>().sprite = map_icon;       
        matplace_toggle[count].transform.Find("Background").GetComponentInChildren<Text>().text = matplace_database.matplace_lists[i].placeNameHyouji;
        matplace_toggle[count].GetComponent<matplaceSelectToggle>().place_flag = matplace_database.matplace_lists[i].placeFlag;
        matplace_toggle[count].GetComponent<matplaceSelectToggle>().place_default_flag = matplace_database.matplace_lists[i].placeDefaultFlag;
        matplace_toggle[count].GetComponent<matplaceSelectToggle>().placeNum = i; //トグルにリスト配列番号を割り振っておく。
        
        count++;
    }

    void mapicon_Active()
    {
        //表示フラグにそって、採取地の表示/非表示の決定 read_endflagに注意　設定し忘れてると、表示されなかったりするよ
        for (i = 0; i < matplace_toggle.Count; i++)
        {
            if (matplace_toggle[i].GetComponent<matplaceSelectToggle>().place_default_flag == 9999) //ゲーム中の解禁フラグにかかわらず、必ずON/OFFできるフラグ 9999の場合、ゲーム中のフラグを優先する。
            {
                if (matplace_toggle[i].GetComponent<matplaceSelectToggle>().place_flag == 1)
                {
                    matplace_toggle[i].SetActive(true);
                }
                else
                {
                    matplace_toggle[i].SetActive(false);
                }
            }
            else if(matplace_toggle[i].GetComponent<matplaceSelectToggle>().place_default_flag == 0)
            {
                matplace_toggle[i].SetActive(false);
            }
            else if (matplace_toggle[i].GetComponent<matplaceSelectToggle>().place_default_flag == 1)
            {
                matplace_toggle[i].SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        
    }

    //外から読み込む。初期化用。
    public void SetInit()
    {
        //スロットビューは最初Off
        slot_view.SetActive(false);

        //最初は、採取地選択画面をonに。
        getmatplace_view.SetActive(true);

        //採取地用ステータスパネル表示
        StatusPanelON();

        //画面のアニメ
        OpenAnim();

        //時刻によって、背景の絵の天気を変える。
        /*if (GameMgr.Story_Mode != 0)
        {
            //まずリセット
            MapSelect_Imagepanel_obj.Clear();
            MapSelect_Imagepanel_obj.Add(MapSelect_BGpanel.transform.Find("Map_01/SelectMapBG1").gameObject);
            MapSelect_Imagepanel_obj.Add(MapSelect_BGpanel.transform.Find("Map_01/SelectMapBG2").gameObject);
            MapSelect_Imagepanel_obj.Add(MapSelect_BGpanel.transform.Find("Map_01/SelectMapBG3").gameObject);

            MapSelect_Imagepanel_obj[0].SetActive(true);
            MapSelect_Imagepanel_obj[1].SetActive(false);
            MapSelect_Imagepanel_obj[2].SetActive(false);

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

                    MapSelect_Imagepanel_obj[1].SetActive(true);
                    break;

                case 6: //夜

                    MapSelect_Imagepanel_obj[2].SetActive(true);

                    break;
            }
        }*/

        move_anim_on = false;
        modoru_anim_on = false;
        treasure_anim_on = false;

        mapicon_Active();
        

        //妹の体力（HP)を表示
        HeroineLifeText.text = PlayerStatus.player_girl_lifepoint.ToString();
        //HeroineLifeText.text = PlayerStatus.girl1_Love_exp.ToString();

        //採取地行く前なら、場所番号などはリセット　採取地シーンに入った場合は、ここは無視する
        GameMgr.Select_place_num = 0;
    }

    void OpenAnim()
    {
        //まず、初期値。
        getmatplace_view.GetComponent<CanvasGroup>().alpha = 0;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(getmatplace_view.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。

        sequence.Append(getmatplace_view.transform.DOLocalMove(new Vector3(50f, 0f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。
        sequence.Join(getmatplace_view.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    // Update is called once per frame
    void Update () {

        //matplace_database.ReSetMapFlagString("Or_Hiroba1", 1); //デバッグ用　フラグが狂ったときに

        if (move_anim_on == true)
        {
            //移動中のウェイトアニメ
            MoveAnim();
        }

        if (move_anim_end == true)
        {
            move_anim_end = false;
            slot_view_status = 0;

            //シーン移動のマップは、そのままシーン移動
            switch (GameMgr.Select_place_name)
            {
                case "Hiroba":

                    FadeManager.Instance.LoadScene("Hiroba2", GameMgr.SceneFadeTime);
                    break;

                case "Shop":

                    FadeManager.Instance.LoadScene("Shop", GameMgr.SceneFadeTime);
                    break;

                case "Bar":

                    FadeManager.Instance.LoadScene("Bar", GameMgr.SceneFadeTime);
                    break;

                case "Emerald_Shop":

                    FadeManager.Instance.LoadScene("Emerald_Shop", GameMgr.SceneFadeTime);
                    break;

                case "Farm":

                    FadeManager.Instance.LoadScene("Farm", GameMgr.SceneFadeTime);
                    break;

                case "Grt_StartCompound":

                    FadeManager.Instance.LoadScene("Compound", GameMgr.SceneFadeTime);
                    break;

                case "Grt_Station":

                    GameMgr.SceneSelectNum = 0;
                    FadeManager.Instance.LoadScene("Station", GameMgr.SceneFadeTime);
                    break;

                //以下、オランジーナ関連
                case "Orangina_Compound":

                    FadeManager.Instance.LoadScene("Or_Compound", GameMgr.SceneFadeTime);
                    break;

                case "Or_Comp_Entrance": //玄関から外にでる。

                    sc.EnterSound_01();
                    GameMgr.SceneSelectNum = 0;
                    FadeManager.Instance.LoadScene("Or_Compound_Enterance", GameMgr.SceneFadeTime);
                    break;

                case "Or_Shop_A1":

                    GameMgr.SceneSelectNum = 0; //ショップや酒場・コンテストなどは行く前に、どのエリアのお店なのか番号を指定する　名前は移動先シーンのメインcsのStartで決定
                    FadeManager.Instance.LoadScene("Or_Shop", GameMgr.SceneFadeTime);
                    break;

                case "Or_Shop_B1":

                    GameMgr.SceneSelectNum = 10; //
                    FadeManager.Instance.LoadScene("Or_Shop", GameMgr.SceneFadeTime);
                    break;

                case "Or_Shop_C1":

                    GameMgr.SceneSelectNum = 20; //
                    FadeManager.Instance.LoadScene("Or_Shop", GameMgr.SceneFadeTime);
                    break;

                case "Or_Shop_D1":

                    GameMgr.SceneSelectNum = 30; //
                    FadeManager.Instance.LoadScene("Or_Shop", GameMgr.SceneFadeTime);
                    break;

                case "Or_Bar_A1":

                    GameMgr.SceneSelectNum = 0;
                    FadeManager.Instance.LoadScene("Or_Bar", GameMgr.SceneFadeTime);
                    break;

                case "Or_Bar_B1":

                    GameMgr.SceneSelectNum = 10;
                    FadeManager.Instance.LoadScene("Or_Bar", GameMgr.SceneFadeTime);
                    break;

                case "Or_Bar_C1":

                    GameMgr.SceneSelectNum = 20;
                    FadeManager.Instance.LoadScene("Or_Bar", GameMgr.SceneFadeTime);
                    break;

                case "Or_Bar_D1":

                    GameMgr.SceneSelectNum = 30;
                    FadeManager.Instance.LoadScene("Or_Bar", GameMgr.SceneFadeTime);
                    break;

                case "Or_Emerald_Shop":

                    GameMgr.Scene_Name = "Or_Emerald_Shop";
                    FadeManager.Instance.LoadScene("Or_Emerald_Shop", GameMgr.SceneFadeTime);
                    break;

                case "Or_Hiroba1":

                    GameMgr.SceneSelectNum = 0;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                case "Or_Station":

                    GameMgr.SceneSelectNum = 100;
                    FadeManager.Instance.LoadScene("Station", GameMgr.SceneFadeTime);
                    break;

                case "Or_Contest_A1":

                    GameMgr.SceneSelectNum = 0;
                    FadeManager.Instance.LoadScene("Or_Contest_Reception", GameMgr.SceneFadeTime);
                    break;

                case "Or_Contest_B1":

                    GameMgr.SceneSelectNum = 10;
                    FadeManager.Instance.LoadScene("Or_Contest_Reception", GameMgr.SceneFadeTime);
                    break;

                case "Or_Contest_C1":

                    GameMgr.SceneSelectNum = 20;
                    FadeManager.Instance.LoadScene("Or_Contest_Reception", GameMgr.SceneFadeTime);
                    break;

                case "Or_Contest_D1":

                    GameMgr.SceneSelectNum = 30;
                    FadeManager.Instance.LoadScene("Or_Contest_Reception", GameMgr.SceneFadeTime);
                    break;


                default:

                    //採取地表示
                    //新しいシーンに移動してから、SlotView以下をはしらせる
                    FadeManager.Instance.LoadScene("GetMaterial", GameMgr.SceneFadeTime);
                    //Slot_View();
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

            GameMgr.Getmat_return_home = true;
            GameMgr.Scene_back_home = true; //玄関音鳴らすフラグ
            FadeManager.Instance.LoadScene("Or_Compound", GameMgr.SceneFadeTime);   

        }
    }

    //リザルトパネル表示の一連の処理　EventDataBaseかUtage_scenarioからも読み出し
    public void ResultPanelOn()
    {
        GameMgr.ResultOFF = true; //リザルト表示中
        getmatResult_panel_obj.SetActive(true);
        getmatResult_panel.reset_and_DrawView();
        getmatResult_panel.OnStartAnim();
        StartCoroutine("ResultOn");
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
                if (PlayerStatus.player_girl_lifepoint <= 0 && matplace_database.matplace_lists[_place_num].placeType == 1) //0以下かつダンジョンタイプに行こうとする場合
                {
                    if (GameMgr.outgirl_Nowprogress)
                    {
                        _text.text = "今日はもうからだが疲れているな..。" + "\n" + "（寝て、" + GameMgr.ColorYellow + "体力を回復" + "</color>" + "しよう。）";
                    }
                    else
                    {
                        _text.text = "にいちゃん。からだがグタグタでもう動けねぇ～・・。" + "\n" + "（寝て、" + GameMgr.ColorYellow + "体力を回復" + "</color>" + "しよう。）";
                    }

                    All_Off();
                }
                else
                {
                    //時間が20時をこえないかチェック
                    if (GameMgr.TimeUSE_FLAG)
                    {
                        if (GameMgr.Story_Mode == 0)
                        {
                            //_yosokutime = PlayerStatus.player_time + matplace_database.matplace_lists[_place_num].placeDay; //行きの時間だけ計算
                            _yosokutime = time_controller.YosokuMinuteToHour(matplace_database.matplace_lists[_place_num].placeDay);
                            if (_yosokutime >= GameMgr.EndDay_hour) //20時をこえるかどうか。
                            {
                                //20時を超えるので、妹に止められる。
                                if (GameMgr.outgirl_Nowprogress)
                                {
                                    _text.text = "時間が遅くなりそうだ..。今日はやめておこう。";
                                }
                                else
                                {
                                    _text.text = "にいちゃん。今日は遅いから、明日いこ～。";
                                }
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
                            //エクストラモードだと、19時以降は、採取地にはもう移動できない。
                            if (GameMgr.BG_cullent_weather == 6)
                            {
                                if (matplace_database.matplace_lists[_place_num].placeType == 0)
                                {
                                    KakuninPlace();
                                    break;
                                }
                                else
                                {
                                    //20時を超えるので、妹に止められる。
                                    if (GameMgr.outgirl_Nowprogress)
                                    {
                                        _text.text = "時間が遅くなりそうだ..。今日はやめておこう。";
                                    }
                                    else
                                    {
                                        _text.text = "にいちゃん。今日は遅いから、明日いこ～。";
                                    }
                                    All_Off();
                                }
                            }
                            else
                            {
                                KakuninPlace();
                                break;
                            }
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
        if (matplace_database.matplace_lists[_place_num].placeType == 0)
        {
            _text.text = matplace_database.matplace_lists[_place_num].placeNameHyouji + "へ行きますか？";
        }
        else
        {
            _text.text = matplace_database.matplace_lists[_place_num].placeNameHyouji + "へ行きますか？" + "\n" + "移動費用：" + GameMgr.ColorYellow + matplace_database.matplace_lists[i].placeCost.ToString() + GameMgr.MoneyCurrency + "</color>"
                + "  " + "体力消費：" + GameMgr.ColorPink + matplace_database.matplace_lists[_place_num].placeHP + "</color>";
        }

        
        GameMgr.Select_place_num = _place_num;
        GameMgr.Select_place_name = matplace_database.matplace_lists[_place_num].placeName;
        GameMgr.Select_place_day = matplace_database.matplace_lists[_place_num].placeDay;

        Debug.Log("mapID: " + matplace_database.matplace_lists[_place_num].matplaceID + " " + GameMgr.Select_place_name + "が選択されました。");

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
                mat_cost = matplace_database.matplace_lists[GameMgr.Select_place_num].placeCost;
                if (PlayerStatus.player_money < mat_cost)
                {
                    if (GameMgr.outgirl_Nowprogress)
                    {
                        _text.text = "お金が足りない・・。";
                    }
                    else
                    {
                        _text.text = "にいちゃん。お金が足りないよ～・・。";
                    }

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

                    subevent_on = false;
                    event_end_flag = false;

                    next_on = false;

                    girl1_status.hukidasiOff();

                    //音量フェードアウト
                    sceneBGM.FadeOutBGM();

                    //日数の経過。場所ごとに、移動までの日数が変わる。
                    time_controller.SetMinuteToHour(GameMgr.Select_place_day);
                    time_controller.TimeKoushin(0);

                    //時間の項目リセット
                    time_controller.ResetTimeFlag();

                    //お金の消費
                    moneyStatus_Controller.UseMoney(mat_cost);

                    //リザルトアイテムをリセット
                    InitializeResultItemDicts();

                    //腹も減る
                    if (GameMgr.Story_Mode != 0)
                    {
                        PlayerStatus.player_girl_manpuku -= 10;
                    }

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








    //
    //採取地シーンで動く
    //
    //GetMaterial_Mainから読み込み
    public void Slot_ViewON()
    {
        getmatplace_panel.SetActive(true);
        map_bg_effect.SetActive(true);

        //材料ランダムで３つ手に入るオブジェクトの取得
        get_material_obj = GameObject.FindWithTag("GetMaterial");
        get_material = get_material_obj.GetComponent<GetMaterial>();

        switch (slot_view_status)
        {
            case 0: //初期化

                moveanim_panel.GetComponent<CanvasGroup>().DOFade(0, 0.5f); //背景黒フェード           

                if (next_on) //先へ進む場合、背景も黒フェードを消す
                {                    
                    text_area.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
                }

                //this.transform.Find("Comp/Map_ImageBG_FadeBlack").GetComponent<CanvasGroup>().DOFade(1, 0.0f);
                getmatplace_view.SetActive(false);
                TreasureGetitem_obj.SetActive(false);
                slot_view.SetActive(true);
                GetMatStatusButton_obj.SetActive(true);
                yes_no_panel.SetActive(false);
                slot_tansaku_button_obj.SetActive(true);
                OpenTreasureButton_obj.SetActive(false);
                NextButton_obj.SetActive(false);

                /*if (!next_on)//先へ進まない場合は、リセットしない。
                {
                    InitializeResultItemDicts();
                    get_material.SetInit();
                } */

                slot_view_status = 1;
                GameMgr.Scene_Status = 22;
                //GameMgr.compound_status = 22;

                //音量フェードイン
                sceneBGM.MuteOFFBGM();
                sceneBGM.FadeInBGM();

                Debug.Log(GameMgr.Select_place_name + "へ移動");
                for(i=0; i< matplace_database.matplace_lists.Count; i++) //強制移動などの際に、select_place_numが更新されないので、ここでも更新
                {
                    if(matplace_database.matplace_lists[i].placeName == GameMgr.Select_place_name)
                    {
                        GameMgr.Select_place_num = i;
                    }
                }
                

                //背景のセッティング
                SetMapBG(GameMgr.Select_place_name);

                //ステータスパネルをON
                StatusPanelON();

                MapSetting_01();
                MapSetting_02();

                if (!subevent_on) //サブイベントなどが発生しなければ、そのままフェードをONにして探索開始
                {
                    //this.transform.Find("Comp/Map_ImageBG_FadeBlack").GetComponent<CanvasGroup>().DOFade(0, 0.5f); //背景黒フェード
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

                if (text_kaigyo_active)
                {
                    text_kaigyo_button.SetActive(true);
                    text_kaigyo_buttonPanel.SetActive(true);
                }

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

                moveanim_panel.GetComponent<CanvasGroup>().DOFade(1, 0.3f); //背景黒フェード
                moveanim_panel.GetComponent<GraphicRaycaster>().enabled = true;

                StatusPanelOFF();

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

                moveanim_panel.GetComponent<GraphicRaycaster>().enabled = false;

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

                _return_blackTime = 0.5f;
                timeOut = _return_blackTime;
                modoru_anim_status = 1;
                modoru_anim_end = false;

                yes_no_panel.SetActive(false);

                moveanim_panel.GetComponent<CanvasGroup>().DOFade(1, 0.5f); //背景黒フェード
                moveanim_panel.GetComponent<GraphicRaycaster>().enabled = true;

                StatusPanelOFF();

                //背景のSEを止める。
                map_ambience.FadeOut();

                _text.text = "帰還中 .";
                moveanim_panel_image_text.GetComponent<Text>().text = "帰還中 .";
                break;

            case 1: // 状態2

                if (timeOut <= 0.0)
                {
                    timeOut = _return_blackTime;
                    modoru_anim_status = 2;

                    _text.text = "帰還中 . .";
                    moveanim_panel_image_text.GetComponent<Text>().text = "帰還中 . .";
                }
                break;

            case 2:

                if (timeOut <= 0.0)
                {
                    timeOut = _return_blackTime;
                    modoru_anim_status = 3;


                }
                break;

            case 3: //アニメ終了。判定する

                modoru_anim_on = false;
                modoru_anim_end = true;
             
                //modoru_anim_status = 0;
                moveanim_panel.GetComponent<GraphicRaycaster>().enabled = false;

                break;

            default:
                break;
        }

        //時間減少
        timeOut -= Time.deltaTime;
    }

    IEnumerator MapEventOn(int _status)
    {
        Debug.Log("マップ　イベントON");
        //イベントを再生。再生終了したら、イベントパネルをオフにし、探索ボタンもONにする。
        slot_tansaku_button_obj.SetActive(false);
        text_area.SetActive(false);
        subevent_on = true;
        //getmatplace_panel.SetActive(false);

        StatusPanelOFF();

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;

        StatusPanelON();

        text_area.SetActive(true);
        slot_tansaku_button_obj.SetActive(true);
        //getmatplace_panel.SetActive(true);　//Compオブジェクトをon

        subevent_on = false;

        switch (GameMgr.MapSubEvent_Flag)
        {
            case 0:
               
                break;

            case 10: //ねこのお墓をみつけた

                GameMgr.MapEvent_01[1] = true; //ししゃもクッキーイベント終了のフラグ

                matplace_database.matPlaceKaikin("CatGrave"); //ねこのお墓解禁
                GameMgr.Select_place_name = "CatGrave";
                _text.text = "ねこのお墓がある。";

                PlayerStatus.girl1_Love_exp += 50;//ハートが50あがっている。

                //イベントCG解禁
                GameMgr.SetEventCollectionFlag("event9", true);

                //イベントチェック ねこのお墓関連のイベントフラグ
                if (!GameMgr.MapEvent_08[0])
                {
                    GameMgr.MapEvent_08[0] = true;
                }

                break;
        }

        if(_status == 0)
        {
            slot_view_status = 1; //通常の材料集めシーンに切り替え
        }
        else //マップでサブイベントも読み込んだ場合
        {
            slot_view_status = 0; //イベント後にマップを切り替える場合
            Fadeout_Black_obj.GetComponent<FadeOutBlack>().FadeOut(); //FadeOutBlackは、家の背景を隠す用のブラック。フェードアウトでOFFにする。
                                                                      //採取地表示
            Slot_ViewON();
        }

    }

    void StatusPanelOFF()
    {
        MoneyStatus_Panel_obj.SetActive(false);
        GetMatStatusButton_obj.SetActive(false);
        if (GameMgr.TimeUSE_FLAG)
        {
            TimePanel_obj.SetActive(false);
        }
    }

    void StatusPanelON()
    {
        MoneyStatus_Panel_obj.SetActive(true);
        GetMatStatusButton_obj.SetActive(true);
        if (GameMgr.TimeUSE_FLAG)
        {
            TimePanel_obj.SetActive(true);
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
        BG_obj = GameObject.FindWithTag("BG");
        map_imageBG = BG_obj.transform.Find("BGPlace_01/BG_sprite_morning").gameObject;

        mapid = matplace_database.SearchMapString(_place_name);

        texture2d = matplace_database.matplace_lists[mapid].center_bg;
        texture2d_map = matplace_database.matplace_lists[mapid].back_bg;

        slot_view_image.sprite = texture2d;
        map_imageBG.GetComponent<SpriteRenderer>().sprite = texture2d_map;
    }


    //リザルト画面をOFF
    public void GetMatResultPanelOff()
    {
        InitializeResultItemDicts(); //取得したアイテム表示用のディクショナリーの初期化

        sc.PlaySe(30);
        GameMgr.ResultOFF = false;

        GameMgr.check_GetMat_flag = true; //採取地帰ってきたよのフラグ
        GameMgr.check_GirlLoveEvent_flag = false; //再度イベントチェック
        //Debug.Log("イベントチェックON");
    }

    //リザルト画面を表示中。オフにしたあとの処理。
    IEnumerator ResultOn()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。
        while (GameMgr.ResultOFF)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        getmatResult_panel_obj.SetActive(false);

        switch (GameMgr.Scene_Category_Num)
        {
            case 10: //アトリエの場合の処理　ヒカリが採取から家に帰ってきたとき

                //調合シーンメインオブジェクトの取得
                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                if (GameMgr.outgirl_returnhome_homeru)
                {
                    GameMgr.outgirl_returnhome_homeru = false;

                    //ほめるかどうかをまた宴できく。
                    eventdatabase.eventOutGirlHomeru();
                }
                else
                {
                    if (GameMgr.girl_returnhome_flag) //兄が家にかえってきたタイミングで、妹がすでに家にいた場合。おかえり～という。
                    {
                        if (GameMgr.girl_returnhome_num == 0)
                        {
                            GameMgr.girlloveevent_bunki = 1;
                            GameMgr.GirlLoveSubEvent_num = 153;
                            GameMgr.GirlLoveSubEvent_stage1[153] = true; //イベント初発生の分をフラグっておく。
                            GameMgr.girl_returnhome_endflag = true;

                            StartCoroutine("HikariOkaeri");
                            compound_Main.ReadGirlLoveEvent_Fire();
                        }
                        else
                        {
                            GameMgr.girl_returnhome_endflag2 = false;
                            GameMgr.girl_returnhome_flag = false;
                        }

                    }
                    else
                    {
                        //メインシーンのデフォルトに戻る。
                        time_controller.TimeCheck_flag = true; //寝るかどうかの判定する   
                        time_controller.TimeKoushin(0);
                        girl1_status.hukidasiOn();
                    }

                    slot_view_status = 0;
                }
                break;

            default: //その他のシーンの場合

                slot_view_status = 0;
                break;
        }
    
             
    }

    //Compound_mainから読み出し
    public void OnHikariOkaeri_Fire()
    {
        StartCoroutine("HikariOkaeri");
    }

    IEnumerator HikariOkaeri()
    {
        while (GameMgr.girl_returnhome_endflag)
        {
            yield return null;
        }
        GameMgr.girl_returnhome_endflag = false;

        //妹がとってきたアイテムの一覧
        eventdatabase.OutGirlGetItems();
        ResultPanelOn();
        GameMgr.girl_returnhome_endflag2 = true;
        GameMgr.girl_returnhome_num = 1;

        while (GameMgr.girl_returnhome_endflag2)
        {
            yield return null;
        }
        GameMgr.girl_returnhome_endflag2 = false;

        Debug.Log("時間更新＆チェック");
        //メインシーンのデフォルトに戻る。
        time_controller.TimeCheck_flag = true; //寝るかどうかの判定する   
        time_controller.TimeReturnHomeSleep_Status = true;
        time_controller.TimeKoushin(0);
        girl1_status.hukidasiOn();

        GameMgr.ReadGirlLoveTimeEvent_reading_now = false;
    }

    //外出から帰ってきた時のイベントからも読み出し　初期化。
    public void InitializeResultItemDicts()
    {
        GameMgr.GetMat_ResultList.Clear();

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコア(所持数)と、追加得点用のスコアをつける
        /*for (i = 0; i < database.items.Count; i++)
        {
            result_items.Add(database.items[i].itemName, 0);
        }*/
    }

    public void OnTansaku() //探索ボタンをおした
    {
        get_material.GetRandomMaterials(GameMgr.Select_place_num);
    }

    public void OnModoru() //街へ戻るをおした
    {
        slot_view_status = 2;

        _text.text = "家に戻る？";

        if(text_kaigyo_button.activeSelf)
        {
            text_kaigyo_active = true; //改行ボタンはtrueになっていた場合。一時的にオフにするが、あとでオンに直す用のフラグ。
            text_kaigyo_button.SetActive(false);
            text_kaigyo_buttonPanel.SetActive(false);
        }
        else
        {
            text_kaigyo_active = false;
        }

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
        //妹の体力がないと、先へ進めない。井戸や近くの森は、ハートがなくても採れる。
        if (PlayerStatus.player_girl_lifepoint < 3)　//PlayerStatus.player_girl_lifepoint
        {
            _text.text = "にいちゃん。やっぱりこわいよう..。" + "\n" + "（体力が足りないようだ。）";
        }
        else
        {
            OpenTreasureButton_obj.SetActive(false);
            slot_tansaku_button_obj.SetActive(false);

            //ハートを３つ消費
            //PlayerStatus.girl1_Love_exp -= 3;
            PlayerStatus.player_girl_lifepoint -= 3;
            HeroineLifeText.text = PlayerStatus.player_girl_lifepoint.ToString();

            treasure_anim_status = 0;
            treasure_anim_on = true;
        }
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
        get_material.GetTreasureBox(GameMgr.Select_place_name);
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

                GameMgr.Select_place_name = "BirdSanctuali"; //移動後の場所を指定
                GameMgr.Select_place_num = matplace_database.SearchMapString(GameMgr.Select_place_name); //次回より、「外へでる」ですぐ行けるよう、フラグ解放
                break;

            default:

                break;
        }
    }   

    void MapSetting_01()
    {
        switch (GameMgr.Select_place_name)
        {

            case "Forest":

                //森のBGM
                sceneBGM.OnGetMat_ForestBGM();
                //GameMgr.matbgm_change_flag = true;

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_Forest").gameObject.SetActive(true);

                if (GameMgr.outgirl_Nowprogress) //妹が一緒にいない場合
                {
                    _text.text = "森のいい香りだ。ここはとても落ち着く..。";
                }
                else
                {

                    //イベントチェック
                    if (!GameMgr.MapEvent_01[0])
                    {
                        GameMgr.MapEvent_01[0] = true;

                        _text.text = "すげぇ～～！森だー！";

                        slot_view_status = 3; //イベント読み込み中用に退避

                        GameMgr.map_ev_ID = 10;
                        GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                        StartCoroutine(MapEventOn(0));
                    }
                    else
                    {
                        _text.text = "にいちゃん、いっぱいとろうね！";

                        //サブイベントチェック
                        //Debug.Log("ししゃもクッキー所持数: " + pitemlist.ReturnItemKosu("shishamo_cookie"));
                        if (!event_end_flag) //来るたびの初回のみイベントチェック
                        {
                            event_end_flag = true;

                            if (!GameMgr.MapEvent_01[1]) //ししゃもクッキーをもっている　かつ　お菓子パネルにセットされてる
                            {
                                if (pitemlist.player_extremepanel_itemlist.Count > 0 &&
                                    pitemlist.player_extremepanel_itemlist[0].itemName == "shishamo_cookie")
                                {
                                    GameMgr.map_ev_ID = 11;
                                    GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                                    sceneBGM.MuteBGM(); //宴のBGMを使う
                                    //this.transform.Find("Comp/Map_ImageBG_FadeBlack").GetComponent<CanvasGroup>().DOFade(1, 0.0f); //背景黒フェード
                                    //getmatplace_panel.SetActive(false); //Comp自体もOFFにして、宴をクリックで進むように。
                                    Fadeout_Black_obj.GetComponent<FadeOutBlack>().NowIn(); //家の風景が見えないように、さらに黒をいれる。

                                    StartCoroutine(MapEventOn(1)); //1をいれると、イベント終わりに、再度slotview_status=0で、更新しなおす。
                                }
                            }
                        }
                    }
                }

                break;

            case "Lavender_field":

                //森のBGM
                sceneBGM.OnGetMat_LavenderFieldBGM();

                //背景のSEを鳴らす。
                map_ambience.MuteOFF();
                map_ambience.OnLavenderField();

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_Lavender").gameObject.SetActive(true);

                if (GameMgr.outgirl_Nowprogress) //妹が一緒にいない場合
                {
                    _text.text = "ラベンダー畑だ。湖が青く美しい。";
                }
                else
                {
                    //イベントチェック
                    if (!GameMgr.MapEvent_05[0])
                    {
                        GameMgr.MapEvent_05[0] = true;

                        _text.text = "ラベンダー畑だ～！いい香り～。";

                        slot_view_status = 3; //イベント読み込み中用に退避

                        GameMgr.map_ev_ID = 60;
                        GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                        StartCoroutine(MapEventOn(0));
                    }
                    else
                    {
                        _text.text = "にいちゃん！　お花のいい香り～.. ゴロゴロ。";
                    }
                }

                break;

            case "BerryFarm":

                //ベリーファームのBGM
                sceneBGM.OnGetMat_BerryFarmBGM();

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_Lavender").gameObject.SetActive(true);

                if (GameMgr.outgirl_Nowprogress) //妹が一緒にいない場合
                {
                    _text.text = "ベリーがそこかしこになっている。たくさん採るぞ！";
                }
                else
                {
                    _text.text = "にいちゃん、色んな実がなってるよ～！！";

                    //イベントチェック
                    if (!GameMgr.MapEvent_07[0])
                    {
                        GameMgr.MapEvent_07[0] = true;

                        //今のとこ、特にイベントなし
                    }
                }

                break;

            case "StrawberryGarden":

                //ストロベリーガーデンのBGM
                sceneBGM.OnGetMat_StrawberryGardenBGM();

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_StrawberryGarden").gameObject.SetActive(true);

                if (GameMgr.outgirl_Nowprogress) //妹が一緒にいない場合
                {
                    _text.text = "いちごが盛だくさんだ。いっぱい採っておこう！";
                }
                else
                {
                    //イベントチェック
                    if (!GameMgr.MapEvent_03[0])
                    {
                        GameMgr.MapEvent_03[0] = true;

                        _text.text = "いいにお～い。にいちゃん、いちごいっぱい～！";

                        slot_view_status = 3; //イベント読み込み中用に退避

                        GameMgr.map_ev_ID = 40;
                        GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                        StartCoroutine(MapEventOn(0));
                    }
                    else
                    {
                        _text.text = "にいちゃん、いちごがたくさん～！";
                    }
                }

                break;

            case "HimawariHill":

                //ひまわり畑のBGM
                sceneBGM.OnGetMat_HimawariHillBGM();

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_Himawari").gameObject.SetActive(true);

                if (GameMgr.outgirl_Nowprogress) //妹が一緒にいない場合
                {
                    _text.text = "ひまわり畑だ。どこか懐かしいにおいを感じる。";
                }
                else
                {
                    //イベントチェック
                    if (!GameMgr.MapEvent_04[0])
                    {
                        GameMgr.MapEvent_04[0] = true;

                        _text.text = "にいちゃん。まっ黄色～～！すごいきれい～。";

                        slot_view_status = 3; //イベント読み込み中用に退避

                        GameMgr.map_ev_ID = 50;
                        GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                        StartCoroutine(MapEventOn(0));
                    }
                    else
                    {
                        _text.text = "にいちゃん、種とりはまかせてね！";
                    }
                }

                break;

            case "BirdSanctuali":

                //バードサンクチュアリのBGM
                sceneBGM.OnGetMat_BirdSanctualiBGM();

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_BirdSanctuali").gameObject.SetActive(true);

                if (GameMgr.outgirl_Nowprogress) //妹が一緒にいない場合
                {
                    _text.text = "鳥たちが、静かに水をのんでいる。";
                }
                else
                {
                    //イベントチェック
                    if (!GameMgr.MapEvent_06[0])
                    {
                        GameMgr.MapEvent_06[0] = true;

                        _text.text = "にいちゃん。とりさんがいっぱいいるよ～！！";

                        slot_view_status = 3; //イベント読み込み中用に退避

                        GameMgr.map_ev_ID = 20;
                        GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                        //次回以降、バードサンクチュアリにいけるようになる。
                        matplace_database.matPlaceKaikin("BirdSanctuali");

                        StartCoroutine(MapEventOn(0));
                    }
                    else
                    {
                        _text.text = "にいちゃん。とりさんとあそぼ！！";
                    }
                }

                break;

            case "CatGrave":

                //ねこのおはかのBGM
                sceneBGM.OnGetMat_CatGraveBGM();

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_Forest").gameObject.SetActive(true);

                if (GameMgr.outgirl_Nowprogress) //妹が一緒にいない場合
                {
                    _text.text = "白猫のお墓だ。";
                }
                else
                {
                    _text.text = "ねこのお墓がある。";

                    //イベントチェック
                    if (!GameMgr.MapEvent_08[0])
                    {
                        GameMgr.MapEvent_08[0] = true;
                    }
                }

                break;

            case "IceCreamForest":

                //アイスの実の森のBGM
                sceneBGM.OnGetMat_HimawariHillBGM();

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_Himawari").gameObject.SetActive(true);

                _text.text = "にいちゃん！　アイスの実がいっぱいなってる..！";

                //イベントチェック
                /*if (!GameMgr.MapEvent_07[0])
                {
                    GameMgr.MapEvent_07[0] = true;

                    _text.text = "兄ちゃん。まっ黄色～～！すごいきれい～。";

                    slot_view_status = 3; //イベント読み込み中用に退避

                    GameMgr.map_ev_ID = 50;
                    GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                    StartCoroutine(MapEventOn(0));
                }
                else
                {
                    _text.text = "兄ちゃん、種とりは任せてね！";
                }*/

                break;

            case "Ido":

                //井戸のBGM
                sceneBGM.OnGetMat_IdoBGM();

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_Ido").gameObject.SetActive(true);

                if (GameMgr.outgirl_Nowprogress) //妹が一緒にいない場合
                {
                    _text.text = "村の井戸だ。キリキリに澄んだ水が、たっぷり貯まっている。";
                }
                else
                {
                    //イベントチェック
                    if (!GameMgr.MapEvent_02[0])
                    {
                        GameMgr.MapEvent_02[0] = true;

                        _text.text = "いっぱい水を汲もう。にいちゃん。";

                        slot_view_status = 3; //イベント読み込み中用に退避                           


                        GameMgr.map_ev_ID = 30;
                        GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                        StartCoroutine(MapEventOn(0));
                    }
                    else
                    {
                        _text.text = "にいちゃん、今日も水汲み？ヒカリも手伝うー！";
                    }
                }
                break;

            default:
                break;
        }
    }

    //主にオランジーナ採取地関係
    void MapSetting_02()
    {
        switch (GameMgr.Select_place_name)
        {

            case "Sakura_Forest":

                //森のBGM
                sceneBGM.OnGetMat_ForestBGM();

                //背景エフェクト
                map_bg_effect.transform.Find("MapBG_Effect_Forest").gameObject.SetActive(true);

                if (GameMgr.outgirl_Nowprogress) //妹が一緒にいない場合
                {
                    _text.text = "森のいい香りだ。ここはとても落ち着く..。";
                }
                else
                {

                    //イベントチェック
                    if (!GameMgr.MapEvent_01[0])
                    {
                        GameMgr.MapEvent_01[0] = true;

                        _text.text = "すげぇ～～！森だー！";

                        slot_view_status = 3; //イベント読み込み中用に退避

                        GameMgr.map_ev_ID = 10;
                        GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                        StartCoroutine(MapEventOn(0));
                    }
                    else
                    {
                        _text.text = "にいちゃん、いっぱいとろうね！";

                        //サブイベントチェック
                        //Debug.Log("ししゃもクッキー所持数: " + pitemlist.ReturnItemKosu("shishamo_cookie"));
                        if (!event_end_flag) //来るたびの初回のみイベントチェック
                        {
                            event_end_flag = true;

                            if (!GameMgr.MapEvent_01[1]) //ししゃもクッキーをもっている　かつ　お菓子パネルにセットされてる
                            {
                                if (pitemlist.player_extremepanel_itemlist.Count > 0 &&
                                    pitemlist.player_extremepanel_itemlist[0].itemName == "shishamo_cookie")
                                {
                                    GameMgr.map_ev_ID = 11;
                                    GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                                    sceneBGM.MuteBGM(); //宴のBGMを使う
                                    //this.transform.Find("Comp/Map_ImageBG_FadeBlack").GetComponent<CanvasGroup>().DOFade(1, 0.0f); //背景黒フェード
                                    //getmatplace_panel.SetActive(false); //Comp自体もOFFにして、宴をクリックで進むように。
                                    Fadeout_Black_obj.GetComponent<FadeOutBlack>().NowIn(); //家の風景が見えないように、さらに黒をいれる。

                                    StartCoroutine(MapEventOn(1)); //1をいれると、イベント終わりに、再度slotview_status=0で、更新しなおす。
                                }
                            }
                        }
                    }
                }

                break;
        }
    }
}
