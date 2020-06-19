using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetMatPlace_Panel : MonoBehaviour {

    private ItemMatPlaceDataBase matplace_database;

    private GameObject canvas;
    private Texture2D texture2d;
    private Sprite map_icon;

    private BGM sceneBGM;
    private SoundController sc;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private Girl1_status girl1_status;

    private TimeController time_controller;

    private GameObject content;

    private GameObject getmatplace_view;
    private GameObject slot_view;
    private GameObject slot_tansaku_button;
    private GameObject slot_yes, slot_no;
    private Image slot_view_image;

    private List<GameObject> mapevent_panel = new List<GameObject>();

    private GameObject moveanim_panel;
    private GameObject moveanim_panel_image;
    private GameObject moveanim_panel_image_text;

    private GameObject matplace_toggle_obj;
    public List<GameObject> matplace_toggle = new List<GameObject>();

    private GameObject text_area;
    private Text _text;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject yes_no_panel; //通常時のYes, noボタン

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject get_material_obj;
    private GetMaterial get_material;

    private GameObject map_imageBG;
    private Texture2D texture2d_map;

    private int select_place_num;
    private string select_place_name;
    private int select_place_day;

    private bool Slot_view_on;
    private int slot_view_status;

    private int i, j;

    private bool move_anim_on;
    private bool move_anim_end;
    private int move_anim_status;
    private float timeOut;

    private bool modoru_anim_on;
    private bool modoru_anim_end;
    private int modoru_anim_status;

    private int _yosokutime;

    //SEを鳴らす
    public AudioClip sound1;
    AudioSource audioSource;

    private GameObject sister_stand_img1;

    // Use this for initialization
    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //調合シーンメインオブジェクトの取得
        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //Yes no パネルの取得
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //時間管理オブジェクトの取得
        time_controller = canvas.transform.Find("TimePanel").GetComponent<TimeController>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子       

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

        getmatplace_view = this.transform.Find("Comp/GetMatPlace_View").gameObject;

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
        slot_tansaku_button = slot_view.transform.Find("Tansaku_panel").gameObject;
        slot_yes = slot_view.transform.Find("Tansaku_panel/Yes_tansaku").gameObject;
        slot_no = slot_view.transform.Find("Tansaku_panel/No_tansaku").gameObject;

        i = 0;
        foreach (Transform child in slot_view.transform.Find("EventPanel/").transform)
        {
            //Debug.Log(child.name);           
            mapevent_panel.Add(child.gameObject);
            mapevent_panel[i].SetActive(false);
            i++;
        }


        map_imageBG = this.transform.Find("Comp/Map_ImageBG").gameObject;
        map_imageBG.SetActive(false);

        //妹立ち絵の取得
        sister_stand_img1 = this.transform.Find("Comp/Slot_View/SisterPanel/Girl_Tachie").gameObject;
        sister_stand_img1.SetActive(false);

        select_place_num = 0;

        Slot_view_on = false;
        slot_view_status = 0;

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

            slot_yes.GetComponent<Button>().interactable = true;
            slot_no.GetComponent<Button>().interactable = true;
            
            slot_view_status = 0;
            slot_view.SetActive(false);

            girl1_status.hukidasiOn();

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

            _text.text = "家に戻ってきた。どうしようかなぁ？";
        }
    }

    public void OnClick_Place(int place_num)
    {
        i = 0;
        while (i < matplace_toggle.Count)
        {
            if (matplace_toggle[i].GetComponent<Toggle>().isOn == true)
            {
                //時間が20時をこえないかチェック
                _yosokutime = PlayerStatus.player_time + (matplace_database.matplace_lists[place_num].placeDay * 2);
                if (_yosokutime >= time_controller.max_time * 6)
                {
                    //20時を超えるので、妹に止められる。
                    _text.text = "兄ちゃん。今日は遅いから、明日いこ～。";
                    All_Off();
                }
                else
                {
                    if (matplace_database.matplace_lists[i].placeCost == 0)
                    {
                        _text.text = matplace_database.matplace_lists[place_num].placeNameHyouji + "へ行きますか？";
                    }
                    else
                    {
                        _text.text = matplace_database.matplace_lists[place_num].placeNameHyouji + "へ行きますか？" + "\n" + "探索費用：" + matplace_database.matplace_lists[i].placeCost.ToString() + "G";
                    }

                    select_place_num = place_num;
                    select_place_name = matplace_database.matplace_lists[place_num].placeName;
                    select_place_day = matplace_database.matplace_lists[place_num].placeDay;

                    Select_Pause();
                    break;
                }
            }
            i++;
        }
    }

    void Select_Pause()
    {
        itemselect_cancel.kettei_on_waiting = true; //今、トグルをおして、選択中の状態

        for(j = 0; j < matplace_toggle.Count; j++ )
        {
            matplace_toggle[i].GetComponent<Toggle>().interactable = false;
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

                //Debug.Log("ok");
                //解除

                itemselect_cancel.kettei_on_waiting = false;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                //採取地確定したので、採取地の番号に従って、ランダムで３つアイテム取得＋金額を消費するメソッドへいく。

                move_anim_on = true;
                move_anim_status = 0;

                girl1_status.hukidasiOff();

                //音量フェードアウト
                sceneBGM.FadeOutBGM();

                //日数の経過。場所ごとに、移動までの日数が変わる。
                PlayerStatus.player_time += select_place_day;
                time_controller.TimeKoushin();

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

                getmatplace_view.SetActive(false);
                slot_view.SetActive(true);
                yes_no_panel.SetActive(false);
                map_imageBG.SetActive(true);

                slot_view_status = 1;
                compound_Main.compound_status = 21;
                get_material.SetInit();

                //音量フェードイン
                sceneBGM.FadeInBGM();

                switch (select_place_name)
                {

                    case "Forest":

                        SetMapBG(select_place_name);
                        
                        //森のBGM
                        sceneBGM.OnGetMat_ForestBGM();
                        compound_Main.bgm_change_flag = true;

                        if (GameMgr.MapEvent_01 == false)
                        {
                            _text.text = "すげぇ～～！森だー！";
                        }
                        else
                        {
                            _text.text = "兄ちゃん、いっぱいとろうね！";
                        }

                        //イベントチェック
                        if (GameMgr.MapEvent_01 == false)
                        {
                            GameMgr.MapEvent_01 = true;

                            slot_view_status = 3; //イベント読み込み中用に退避

                            //初森へきたイベントを再生。再生終了したら、イベントパネルをオフにし、探索ボタンもONにする。
                            slot_tansaku_button.SetActive(false);

                            mapevent_panel[0].SetActive(true);
                            text_area.SetActive(false);

                            GameMgr.map_ev_ID = 1;
                            GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                            StartCoroutine("MapEventOn");
                        }

                        /*
                        if (GameMgr.MapEvent_03 == false)
                        {
                            GameMgr.MapEvent_03 = true;

                            slot_view_status = 3; //イベント読み込み中用に退避

                            //初森へきたイベントを再生。再生終了したら、イベントパネルをオフにし、探索ボタンもONにする。
                            slot_tansaku_button.SetActive(false);

                            mapevent_panel[0].SetActive(true);
                            text_area.SetActive(false);

                            GameMgr.map_ev_ID = 3;
                            GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                            StartCoroutine("MapEventOn");
                        }
                        */


                        break;

                    case "Ido":

                        SetMapBG(select_place_name);
                        
                        //井戸のBGM
                        sceneBGM.OnGetMat_IdoBGM();
                        compound_Main.bgm_change_flag = true;

                        if (GameMgr.MapEvent_02 == false)
                        {
                            _text.text = "いっぱい水を汲もう。兄ちゃん。";
                        }
                        else
                        {
                            _text.text = "兄ちゃん、今日も水汲み？私も手伝うー！";
                        }

                        //イベントチェック
                        if (GameMgr.MapEvent_02 == false)
                        {
                            GameMgr.MapEvent_02 = true;

                            slot_view_status = 3; //イベント読み込み中用に退避
                           
                            slot_tansaku_button.SetActive(false);

                            mapevent_panel[1].SetActive(true);
                            text_area.SetActive(false);

                            GameMgr.map_ev_ID = 2;
                            GameMgr.map_event_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

                            StartCoroutine("MapEventOn");
                        }
                        break;

                    default:
                        break;
                }
                
                break;

            case 1: //入力まち

                if (yes_selectitem_kettei.onclick == true) //Yes, No ボタンが押された
                {
                    if (yes_selectitem_kettei.kettei1 == true) //探索ボタンをおした
                    {

                        get_material.GetRandomMaterials(select_place_num);

                        yes_selectitem_kettei.onclick = false;
                    }
                    else //街へ戻るをおした
                    {
                        slot_view_status = 2;

                        yes_selectitem_kettei.onclick = false;

                        _text.text = "家に戻る？";

                        slot_yes.GetComponent<Button>().interactable = false;
                        slot_no.GetComponent<Button>().interactable = false;

                        yes_no_panel.SetActive(true);

                        StartCoroutine("modoru_kakunin");
                       
                    }
                }
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

                modoru_anim_on = true;

                //音量フェードアウト
                sceneBGM.FadeOutBGM();


                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");
                slot_view_status = 1;

                slot_yes.GetComponent<Button>().interactable = true;
                slot_no.GetComponent<Button>().interactable = true;
                yes_no_panel.SetActive(false);

                _text.text = "";

                yes_selectitem_kettei.onclick = false;
                break;
        }

    }

    void All_Off()
    {
        //再度、セレクトできるようにする
        for (i = 0; i < matplace_toggle.Count; i++)
        {
            matplace_toggle[i].GetComponent<Toggle>().interactable = true;
            matplace_toggle[i].GetComponent<Toggle>().isOn = false;
            //Debug.Log(matplace_toggle[i].GetComponent<Toggle>().interactable);
        }

        itemselect_cancel.kettei_on_waiting = false;

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        yes_no_panel.transform.Find("Yes").gameObject.SetActive(false);
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

                _text.text = "移動中 .";
                moveanim_panel_image_text.GetComponent<Text>().text = "移動中 .";
                break;

            case 1: // 状態2

                if (timeOut <= 0.0)
                {
                    timeOut = 1.0f;
                    move_anim_status = 2;

                    _text.text = "移動中 . .";
                    moveanim_panel_image_text.GetComponent<Text>().text = "移動中 . .";
                }
                break;

            case 2:

                if (timeOut <= 0.0)
                {
                    timeOut = 1.0f;
                    move_anim_status = 3;

                    
                }
                break;

            case 3: //アニメ終了。判定する

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
        //Debug.Log("eventRecipi_end() on");
        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;

        text_area.SetActive(true);
        slot_tansaku_button.SetActive(true);
        for(i=0; i<mapevent_panel.Count; i++)
        {
            mapevent_panel[i].SetActive(false);
        }
        

        slot_view_status = 1; //通常の材料集めシーンに切り替え
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
        switch(_place_name)
        {
            case "Hiroba":

                texture2d = Resources.Load<Texture2D>("Utage_Scenario/Texture/Bg/ID003_Western-Castle_noon-1024x576");
                texture2d_map = Resources.Load<Texture2D>("Utage_Scenario/Texture/Bg/110618_");
                break;

            case "Forest":

                texture2d = Resources.Load<Texture2D>("Utage_Scenario/Texture/Bg/MatPlace/1_forest_a_600_300"); //真ん中枠
                texture2d_map = Resources.Load<Texture2D>("Utage_Scenario/Texture/Bg/110618_"); //背景

                break;

            case "Ido":

                texture2d = Resources.Load<Texture2D>("Utage_Scenario/Texture/Bg/MatPlace/2_ido_a_600_300");
                texture2d_map = Resources.Load<Texture2D>("Utage_Scenario/Texture/Bg/nexfan_01_800.600");

                break;
        }
        // texture2dを使い、Spriteを作って、反映させる
        slot_view_image.sprite = Sprite.Create(texture2d,
                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                   Vector2.zero);

        map_imageBG.GetComponent<Image>().sprite = Sprite.Create(texture2d_map,
                                   new Rect(0, 0, texture2d_map.width, texture2d_map.height),
                                   Vector2.zero);
    }
}
