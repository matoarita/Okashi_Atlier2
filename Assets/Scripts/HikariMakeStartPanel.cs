using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HikariMakeStartPanel : MonoBehaviour {

    private GameObject canvas;
    private GameObject _comp, _comp2;

    private GameObject updown_counter;
    private Compound_Keisan compound_keisan;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject card_view_obj;
    private CardView card_view;
    private Transform resulttransform;

    private SoundController sc;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private PlayerItemList pitemlist;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject yes_no_panel;
    private GameObject black_Image;
    private GameObject text_area;
    private Text _text;

    private int itemID_1;
    private int itemID_2;
    private int itemID_3;

    private GameObject Emptypanel;

    private GameObject hikarimakecheck_Prefab; //調合最終チェック用のアイテムプレファブ
    private GameObject resultitem_Hyouji;
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    private List<GameObject> _listitem = new List<GameObject>();
    private int list_count;
    private Sprite texture2d;

    private List<GameObject> _cardImage_obj = new List<GameObject>(); //カード表示用のゲームオブジェクト
    private List<GameObject> _cardImage_obj2 = new List<GameObject>(); //

    private SetImage _cardImage; //カードの描画処理は、SetImageスクリプト
    private GameObject cardPrefab;

    private GameObject effect_Particle_KiraExplode;
    private GameObject effect_Particle_KiraExplode_2;

    private Text makeokasi_kosu;
    private Text timecost_kosu;

    private GameObject select_obj_1;
    private GameObject select_obj_2;

    private Sprite charaIcon_sprite_1;
    private Sprite charaIcon_sprite_2;
    private Sprite charaIcon_sprite_3;
    private GameObject chara_Icon;

    private Slider _slider;
    private GameObject makeTimePanel;
    private Text _slider_text;
    private Text hikari_success_text;
    private Text hikari_failed_text;

    private Text _hukidashi;

    private int i;
    private int _getexp;
    private int _nowexp, _nowlv;
    private string _itemType_subtext;

    // Use this for initialization
    void Start () {

        InitStart();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitStart()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //合成計算オブジェクトの取得
        compound_keisan = Compound_Keisan.Instance.GetComponent<Compound_Keisan>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        charaIcon_sprite_1 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_02");
        charaIcon_sprite_2 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_07");
        charaIcon_sprite_3 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_08");
        chara_Icon = this.transform.Find("Comp2/CharaPanel/Image").gameObject;
        chara_Icon.GetComponent<Image>().sprite = charaIcon_sprite_1;

        cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        effect_Particle_KiraExplode = this.transform.Find("Particle_KiraExplode").gameObject;
        effect_Particle_KiraExplode.SetActive(false);
        effect_Particle_KiraExplode_2 = this.transform.Find("Particle_KiraExplode_2").gameObject;
        effect_Particle_KiraExplode_2.SetActive(false);

        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        yes_no_panel = this.transform.Find("Yes_no_Panel_Finalcheck").gameObject;
        yes_no_panel.SetActive(false);
        black_Image = this.transform.Find("BlackImage_HikariMake").gameObject;
        black_Image.SetActive(false);

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        _comp = this.transform.Find("Comp").gameObject;
        _comp2 = this.transform.Find("Comp2").gameObject;

        //スクロールビュー内の、コンテンツ要素を取得
        Emptypanel = this.transform.Find("Comp/MatPanel/Image/Result_item/emptypanel").gameObject;
        content = this.transform.Find("Comp/MatPanel/Image/Scroll View/Viewport/Content").gameObject;
        hikarimakecheck_Prefab = (GameObject)Resources.Load("Prefabs/hikarimakecheck_item");
        resultitem_Hyouji = this.transform.Find("Comp/MatPanel/Image/Result_item").gameObject;

        makeTimePanel = this.transform.Find("Comp/MatPanel/Image/MakeTimePanel").gameObject;
        _slider = this.transform.Find("Comp/MatPanel/Image/MakeTimePanel/Slider").GetComponent<Slider>();
        _slider.maxValue = GameMgr.hikari_make_okashiTimeCost;
        _slider.value = GameMgr.hikari_make_okashiTimeCost - GameMgr.hikari_make_okashiTimeCounter;
        _slider_text = this.transform.Find("Comp/MatPanel/Image/MakeTimePanel/TimeTextPanel/TimeText").GetComponent<Text>();
        _slider_text.text = ((GameMgr.hikari_make_okashiTimeCost - GameMgr.hikari_make_okashiTimeCounter) / 60.0f).ToString("F1");

        hikari_success_text = this.transform.Find("Comp/MatPanel/Image/MakeTimePanel/SuccessTextPanel/SuccessText").GetComponent<Text>();
        hikari_failed_text = this.transform.Find("Comp/MatPanel/Image/MakeTimePanel/SuccessTextPanel/FailedText").GetComponent<Text>();
        hikari_success_text.text = GameMgr.hikari_make_success_count.ToString();
        hikari_failed_text.text = GameMgr.hikari_make_failed_count.ToString();

        _hukidashi = this.transform.Find("hukidashiPanel/Image/Text").GetComponent<Text>();

        makeokasi_kosu = this.transform.Find("Comp2/KosuPanel/Image/Kosu_param").GetComponent<Text>();
        makeokasi_kosu.text = GameMgr.hikari_make_okashiKosu.ToString();

        timecost_kosu = this.transform.Find("Comp2/TimePanel/Image/Time_param").GetComponent<Text>();
        timecost_kosu.text = (GameMgr.hikari_make_okashiTimeCost / 60.0f).ToString("F1");

        select_obj_1 = this.transform.Find("Comp2/Select_command/Scroll View/Viewport/Content/Select_2").gameObject;
        select_obj_2 = this.transform.Find("Comp2/Select_command/Scroll View/Viewport/Content/Select_3").gameObject;
        SelectHyouji_OnOFF();
        CharaIconChange();
        paramHyoujiKoushin();

        //一度contentの中身を削除
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }
        

        if(GameMgr.hikari_make_okashiFlag) //なんらかのお菓子を登録中
        {
            Emptypanel.SetActive(false);
            makeTimePanel.SetActive(true);
            Start_ZairyoItemIconHyouji(1);
            HikariMakeResultCard_DrawView();

            _hukidashi.text = "ヒカリ、" + "\n" + "ただいま作りちゅう～♪";
        }
        else
        {
            Emptypanel.SetActive(true);
            makeTimePanel.SetActive(false);

            if (GameMgr.hikari_make_Allfailed)
            {
                chara_Icon.GetComponent<Image>().sprite = charaIcon_sprite_3;
                _hukidashi.text = "にいちゃん..。" + "\n" + "しっぱいしちゃった～・・。";
                makeTimePanel.SetActive(true);
            }
            else
            {
                _hukidashi.text = "にいちゃん！" + "\n" + "ヒカリ、何作ろうかなぁ～？";
            }
        }
    }

    private void OnEnable()
    {
        InitStart();
        StartAnim();
    }

    void StartAnim()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        _comp.GetComponent<CanvasGroup>().alpha = 0;
        _comp2.GetComponent<CanvasGroup>().alpha = 0;

        //sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
        sequence.Append(_comp.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。
        sequence.Join(_comp2.transform.DOLocalMove(new Vector3(50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。
                             //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/
        sequence.Append(_comp.transform.DOLocalMove(new Vector3(50f, 0f, 0), 1.0f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。
        sequence.Join(_comp2.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 1.0f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。

        sequence.Join(_comp.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
        sequence.Join(_comp2.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    void Start_ZairyoItemIconHyouji(int _status)
    {
        list_count = 0;
        _listitem.Clear();

        if (GameMgr.hikari_kettei_itemName[0] != "") //該当するものがなかった場合、例外処理で内部の処理を飛ばす。最悪、ゲームは止まらない。
        {
            itemID_1 = database.SearchItemIDString(GameMgr.hikari_kettei_itemName[0]);
            itemID_2 = database.SearchItemIDString(GameMgr.hikari_kettei_itemName[1]);
            if (GameMgr.hikari_kettei_item[2] != 9999) //3個表示のとき
            {
                itemID_3 = database.SearchItemIDString(GameMgr.hikari_kettei_itemName[2]);
            }

            //一個目
            _listitem.Add(Instantiate(hikarimakecheck_Prefab, content.transform));

            _listitem[list_count].transform.Find("NameText").GetComponent<Text>().text = database.items[itemID_1].itemNameHyouji; //アイテム名
            texture2d = database.items[itemID_1].itemIcon_sprite;


            _listitem[list_count].transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
            _listitem[list_count].transform.Find("KosuText").GetComponent<Text>().text = GameMgr.hikari_kettei_kosu[0].ToString(); //個数

            list_count++;

            //二個目
            _listitem.Add(Instantiate(hikarimakecheck_Prefab, content.transform));

            _listitem[list_count].transform.Find("NameText").GetComponent<Text>().text = database.items[itemID_2].itemNameHyouji; //アイテム名
            texture2d = database.items[itemID_2].itemIcon_sprite;


            _listitem[list_count].transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
            _listitem[list_count].transform.Find("KosuText").GetComponent<Text>().text = GameMgr.hikari_kettei_kosu[1].ToString(); //個数

            if (GameMgr.hikari_kettei_item[2] != 9999) //3個表示のとき
            {
                list_count++;

                //三個目
                _listitem.Add(Instantiate(hikarimakecheck_Prefab, content.transform));

                _listitem[list_count].transform.Find("NameText").GetComponent<Text>().text = database.items[itemID_3].itemNameHyouji; //アイテム名
                texture2d = database.items[itemID_3].itemIcon_sprite;


                _listitem[list_count].transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
                _listitem[list_count].transform.Find("KosuText").GetComponent<Text>().text = GameMgr.hikari_kettei_kosu[2].ToString(); //個数

            }
        }
    }

    //この画面専用でのカード表示　cardViewとは別で処理
    public void HikariMakeResultCard_DrawView()
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        _cardImage_obj.Add(Instantiate(cardPrefab, resultitem_Hyouji.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();
        _cardImage.anim_status = 99;

        _cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(false);

        //店売りかオリジナルか、アイテムID        
        _cardImage.check_counter = pitemlist.player_yosokuitemlist.Count - 1;
        _cardImage.SetInitYosoku();

        _cardImage_obj[0].transform.localScale = new Vector3(0.75f, 0.75f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        _cardImage_obj[0].GetComponent<SetImage>().CardParamOFF_2();
    }

    //この画面専用でのカード表示　cardViewとは別で処理
    public void TakeResultCard_DrawView(int _mstatus)
    {
        for (i = 0; i < _cardImage_obj2.Count; i++)
        {
            Destroy(_cardImage_obj2[i]);
        }

        _cardImage_obj2.Clear();

        _cardImage_obj2.Add(Instantiate(cardPrefab, this.transform));
        _cardImage = _cardImage_obj2[0].GetComponent<SetImage>();
        _cardImage.anim_status = 99;

        _cardImage_obj2[0].GetComponent<Canvas>().sortingOrder = 1000;
        
        //店売りかオリジナルか、アイテムID        
        _cardImage.check_counter = pitemlist.player_yosokuitemlist.Count - 1;
        _cardImage.SetInitYosoku();
       
        _cardImage_obj2[0].GetComponent<SetImage>().CardParamOFF_2();

        if (_mstatus == 0)
        {
            _cardImage_obj2[0].transform.Find("CompoundResultButton").gameObject.SetActive(false);
            _cardImage_obj2[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj2[0].transform.localPosition = new Vector3(0, 85, 0);
        }
        else
        {
            _cardImage_obj2[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);
            _cardImage_obj2[0].transform.localScale = new Vector3(0.0f, 0.0f, 1);
            _cardImage_obj2[0].transform.localPosition = new Vector3(0, 85, 0);
            Result_animOn(0); //スケールが小さいから大きくなるアニメーションをON
        }
    }

    //全てのカードを削除する。
    void DeleteCard_DrawView()
    {
        for (i = 0; i < _cardImage_obj.Count; i++) //最後から削除していく。
        {
            Destroy(_cardImage_obj[(_cardImage_obj.Count - 1) - i]);
        }

        _cardImage_obj.Clear();

    }
   
    void DeleteCard_DrawView2()
    {
        for (i = 0; i < _cardImage_obj2.Count; i++) //最後から削除していく。
        {
            Destroy(_cardImage_obj2[(_cardImage_obj2.Count - 1) - i]);
        }

        _cardImage_obj2.Clear();

    }

    public void OnSelect_1()
    {
        DeleteCard_DrawView();

        GameMgr.compound_status = 7;
        this.gameObject.SetActive(false);
    }

    public void OnSelect_2()
    {       
        yes_no_panel.SetActive(true);
        black_Image.SetActive(true);
        text_area.SetActive(true);

        _text.text = database.items[GameMgr.hikari_make_okashiID].itemNameHyouji + "が　" + 
            GameMgr.ColorYellow + GameMgr.hikari_make_okashiKosu.ToString() + "</color>" + "個　できてるよ。" + "うけとる？";
        TakeResultCard_DrawView(0);

        StartCoroutine("Select2_finalcheck");
    }

    public void OnSelect_3()
    {
        yes_no_panel.SetActive(true);
        black_Image.SetActive(true);
        text_area.SetActive(true);

        _text.text = "お菓子作りをやめる？" + "\n" + "（途中まで作ったお菓子は、受け取れるよ！）";
        TakeResultCard_DrawView(0);

        StartCoroutine("Select3_finalcheck");
        
    }

    public void OnSelect_4()
    {
        DeleteCard_DrawView();

        GameMgr.compound_status = 6; //何も選択していない状態にもどる。
        GameMgr.compound_select = 0;

        this.gameObject.SetActive(false);
    }

    void SelectHyouji_OnOFF()
    {
        if (GameMgr.hikari_make_okashiKosu < 1)
        {
            select_obj_1.GetComponent<Button>().interactable = false;
        }
        else
        {
            select_obj_1.GetComponent<Button>().interactable = true;
        }

        if(GameMgr.hikari_make_okashiFlag)
        {
            select_obj_2.GetComponent<Button>().interactable = true;
        }
        else
        {
            select_obj_2.GetComponent<Button>().interactable = false;
        }
    }

    IEnumerator Select2_finalcheck()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false;

        yes_no_panel.SetActive(false);                

        DeleteCard_DrawView2();        

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //うけとる処理

                //カード表示
                sc.PlaySe(17);
                TakeResultCard_DrawView(1);
                effect_Particle_KiraExplode.SetActive(true); //エフェクト
                effect_Particle_KiraExplode_2.SetActive(true);

                compound_keisan.HikariMakeGetItem();
               

                //ヒカリのお菓子経験値の処理
                _getexp = 10 * GameMgr.hikari_make_okashiKosu;
                hikariOkashi_ExpTable(database.items[GameMgr.hikari_make_okashiID].itemType_sub.ToString(), 0);

                //ハートも少し上がる。
                //PlayerStatus.girl1_Love_exp += _getexp;

                if (GameMgr.hikari_make_doubleItemCreated == 0)
                {
                    if (databaseCompo.compoitems[GameMgr.hikari_make_okashi_compID].hikari_make_count == 0)
                    {
                        sc.PlaySe(19);
                        _text.text = database.items[GameMgr.hikari_make_okashiID].itemNameHyouji + "を　" +
                    GameMgr.ColorYellow + GameMgr.hikari_make_okashiKosu.ToString() + "</color>" + "個　うけとった！"
                    + "\n" + "ヒカリは　" + GameMgr.ColorYellow + database.items[GameMgr.hikari_make_okashiID].itemNameHyouji + "</color>" + "　を　おぼえた！"
                    + "\n" + _itemType_subtext + "経験値: " + _getexp + "アップ！　"
                    + _itemType_subtext + "LV: " + _nowlv;
                    }
                    else
                    {
                        _text.text = database.items[GameMgr.hikari_make_okashiID].itemNameHyouji + "を　" +
                    GameMgr.ColorYellow + GameMgr.hikari_make_okashiKosu.ToString() + "</color>" + "個　うけとった！"
                    + "\n" + _itemType_subtext + "経験値: " + _getexp + "アップ！　"
                    + _itemType_subtext + "LV: " + _nowlv;
                    }
                }
                else //卵白卵黄などの例外処理
                {
                    _text.text = database.items[GameMgr.hikari_make_okashiID].itemNameHyouji + "を　" +
                    GameMgr.ColorYellow + GameMgr.hikari_make_okashiKosu.ToString() + "</color>" + "個　うけとった！";
                }

                //個数リセット
                GameMgr.hikari_make_okashiKosu = 0;

                //ヒカリがそのお菓子作った回数をカウント
                databaseCompo.compoitems[GameMgr.hikari_make_okashi_compID].hikari_make_count++;

                break;

            case false:

                Debug.Log("cancel");

                black_Image.SetActive(false);
                text_area.SetActive(false);
                break;

        }

        SelectHyouji_OnOFF();
        paramHyoujiKoushin();
        CharaIconChange();
    }

    //TimeControllerなどから読み込み。失敗しても経験ははいる。
    public void hikarimake_GetExp(int _exp)
    {
        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        _getexp = _exp;
        hikariOkashi_ExpTable(database.items[GameMgr.hikari_make_okashiID].itemType_sub.ToString(), 1);
    }

    void hikariOkashi_ExpTable(string _itemType_sub, int _status)
    {
        switch (_itemType_sub)
        {
            case "Appaleil":
                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                PlayerStatus.player_girl_appaleil_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_appaleil_exp;
                _nowlv = PlayerStatus.player_girl_appaleil_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_appaleil_exp = _nowexp;
                PlayerStatus.player_girl_appaleil_lv = _nowlv;
                _itemType_subtext = "生地";
                break;
            case "Water":
                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                PlayerStatus.player_girl_appaleil_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_appaleil_exp;
                _nowlv = PlayerStatus.player_girl_appaleil_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_appaleil_exp = _nowexp;
                PlayerStatus.player_girl_appaleil_lv = _nowlv;
                _itemType_subtext = "生地";
                break;
            case "Cream":
                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                PlayerStatus.player_girl_cream_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cream_exp;
                _nowlv = PlayerStatus.player_girl_cream_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cream_exp = _nowexp;
                PlayerStatus.player_girl_cream_lv = _nowlv;
                _itemType_subtext = "クリーム";
                break;
            case "Biscotti":
                PlayerStatus.player_girl_rusk_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_rusk_exp;
                _nowlv = PlayerStatus.player_girl_rusk_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_rusk_exp = _nowexp;
                PlayerStatus.player_girl_rusk_lv = _nowlv;
                _itemType_subtext = "ラスク";
                break;
            case "Bread":
                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                PlayerStatus.player_girl_rusk_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_rusk_exp;
                _nowlv = PlayerStatus.player_girl_rusk_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_rusk_exp = _nowexp;
                PlayerStatus.player_girl_rusk_lv = _nowlv;
                _itemType_subtext = "ラスク";
                break;
            case "Bread_Sliced":
                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                PlayerStatus.player_girl_rusk_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_rusk_exp;
                _nowlv = PlayerStatus.player_girl_rusk_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_rusk_exp = _nowexp;
                PlayerStatus.player_girl_rusk_lv = _nowlv;
                _itemType_subtext = "ラスク";
                break;
            case "Cookie":
                PlayerStatus.player_girl_cookie_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cookie_exp;
                _nowlv = PlayerStatus.player_girl_cookie_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cookie_exp = _nowexp;
                PlayerStatus.player_girl_cookie_lv = _nowlv;
                _itemType_subtext = "クッキー";
                break;
            case "Cookie_Mat":
                PlayerStatus.player_girl_cookie_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cookie_exp;
                _nowlv = PlayerStatus.player_girl_cookie_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cookie_exp = _nowexp;
                PlayerStatus.player_girl_cookie_lv = _nowlv;
                _itemType_subtext = "クッキー";
                break;
            case "Cookie_Hard":
                PlayerStatus.player_girl_cookie_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cookie_exp;
                _nowlv = PlayerStatus.player_girl_cookie_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cookie_exp = _nowexp;
                PlayerStatus.player_girl_cookie_lv = _nowlv;
                _itemType_subtext = "クッキー";
                break;
            case "Chocolate":
                PlayerStatus.player_girl_chocolate_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_chocolate_exp;
                _nowlv = PlayerStatus.player_girl_chocolate_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_chocolate_exp = _nowexp;
                PlayerStatus.player_girl_chocolate_lv = _nowlv;
                _itemType_subtext = "チョコレート";
                break;
            case "Chocolate_Mat":
                PlayerStatus.player_girl_chocolate_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_chocolate_exp;
                _nowlv = PlayerStatus.player_girl_chocolate_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_chocolate_exp = _nowexp;
                PlayerStatus.player_girl_chocolate_lv = _nowlv;
                _itemType_subtext = "チョコレート";
                break;
            case "Cake":
                PlayerStatus.player_girl_cake_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cake_exp;
                _nowlv = PlayerStatus.player_girl_cake_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_cake_exp = _nowexp;
                PlayerStatus.player_girl_cake_lv = _nowlv;
                _itemType_subtext = "ケーキ";
                break;
            case "Cake_Mat":
                if (_status == 0)
                {
                    _getexp = 3 * GameMgr.hikari_make_okashiKosu;
                }
                PlayerStatus.player_girl_cake_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cake_exp;
                _nowlv = PlayerStatus.player_girl_cake_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_cake_exp = _nowexp;
                PlayerStatus.player_girl_cake_lv = _nowlv;
                _itemType_subtext = "ケーキ";
                break;
            case "Castella":
                PlayerStatus.player_girl_cake_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cake_exp;
                _nowlv = PlayerStatus.player_girl_cake_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cake_exp = _nowexp;
                PlayerStatus.player_girl_cake_lv = _nowlv;
                _itemType_subtext = "ケーキ";
                break;
            case "Cannoli":
                PlayerStatus.player_girl_crepe_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_crepe_exp;
                _nowlv = PlayerStatus.player_girl_crepe_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_crepe_exp = _nowexp;
                PlayerStatus.player_girl_crepe_lv = _nowlv;
                _itemType_subtext = "クレープ";
                break;
            case "Candy":
                PlayerStatus.player_girl_candy_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_candy_exp;
                _nowlv = PlayerStatus.player_girl_candy_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_candy_exp = _nowexp;
                PlayerStatus.player_girl_candy_lv = _nowlv;
                _itemType_subtext = "キャンディ";
                break;
            case "Crepe":
                PlayerStatus.player_girl_crepe_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_crepe_exp;
                _nowlv = PlayerStatus.player_girl_crepe_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_crepe_exp = _nowexp;
                PlayerStatus.player_girl_crepe_lv = _nowlv;
                _itemType_subtext = "クレープ";
                break;
            case "Crepe_Mat":
                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                PlayerStatus.player_girl_crepe_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_crepe_exp;
                _nowlv = PlayerStatus.player_girl_crepe_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_crepe_exp = _nowexp;
                PlayerStatus.player_girl_crepe_lv = _nowlv;
                _itemType_subtext = "クレープ";
                break;
            case "Creampuff":
                PlayerStatus.player_girl_creampuff_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_creampuff_exp;
                _nowlv = PlayerStatus.player_girl_creampuff_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_creampuff_exp = _nowexp;
                PlayerStatus.player_girl_creampuff_lv = _nowlv;
                _itemType_subtext = "シュークリーム";
                break;
            case "Coffee":
                PlayerStatus.player_girl_tea_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_tea_exp;
                _nowlv = PlayerStatus.player_girl_tea_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_tea_exp = _nowexp;
                PlayerStatus.player_girl_tea_lv = _nowlv;
                _itemType_subtext = "ティー";
                break;
            case "Coffee_Mat":
                PlayerStatus.player_girl_tea_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_tea_exp;
                _nowlv = PlayerStatus.player_girl_tea_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_tea_exp = _nowexp;
                PlayerStatus.player_girl_tea_lv = _nowlv;
                _itemType_subtext = "ティー";
                break;
            case "Donuts":
                PlayerStatus.player_girl_donuts_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_donuts_exp;
                _nowlv = PlayerStatus.player_girl_donuts_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_donuts_exp = _nowexp;
                PlayerStatus.player_girl_donuts_lv = _nowlv;
                _itemType_subtext = "ドーナツ";
                break;
            case "Financier":
                PlayerStatus.player_girl_cake_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cake_exp;
                _nowlv = PlayerStatus.player_girl_cake_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cake_exp = _nowexp;
                PlayerStatus.player_girl_cake_lv = _nowlv;
                _itemType_subtext = "ケーキ";
                break;
            case "IceCream":
                PlayerStatus.player_girl_icecream_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_icecream_exp;
                _nowlv = PlayerStatus.player_girl_icecream_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_icecream_exp = _nowexp;
                PlayerStatus.player_girl_icecream_lv = _nowlv;
                _itemType_subtext = "アイス";
                break;
            case "Juice":
                PlayerStatus.player_girl_juice_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_juice_exp;
                _nowlv = PlayerStatus.player_girl_juice_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_juice_exp = _nowexp;
                PlayerStatus.player_girl_juice_lv = _nowlv;
                _itemType_subtext = "ジュース";
                break;
            case "Jelly":
                PlayerStatus.player_girl_jelly_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_jelly_exp;
                _nowlv = PlayerStatus.player_girl_jelly_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_jelly_exp = _nowexp;
                PlayerStatus.player_girl_jelly_lv = _nowlv;
                _itemType_subtext = "ゼリー";
                break;
            case "Maffin":
                PlayerStatus.player_girl_cake_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cake_exp;
                _nowlv = PlayerStatus.player_girl_cake_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cake_exp = _nowexp;
                PlayerStatus.player_girl_cake_lv = _nowlv;
                _itemType_subtext = "ケーキ";
                break;
            case "PanCake":
                PlayerStatus.player_girl_cake_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cake_exp;
                _nowlv = PlayerStatus.player_girl_cake_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cake_exp = _nowexp;
                PlayerStatus.player_girl_cake_lv = _nowlv;
                _itemType_subtext = "ケーキ";
                break;
            case "Parfe":
                PlayerStatus.player_girl_icecream_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_icecream_exp;
                _nowlv = PlayerStatus.player_girl_icecream_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_icecream_exp = _nowexp;
                PlayerStatus.player_girl_icecream_lv = _nowlv;
                _itemType_subtext = "アイス";
                break;
            case "Pie":
                PlayerStatus.player_girl_cake_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cake_exp;
                _nowlv = PlayerStatus.player_girl_cake_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cake_exp = _nowexp;
                PlayerStatus.player_girl_cake_lv = _nowlv;
                _itemType_subtext = "ケーキ";
                break;
            case "Rusk":
                PlayerStatus.player_girl_rusk_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_rusk_exp;
                _nowlv = PlayerStatus.player_girl_rusk_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_rusk_exp = _nowexp;
                PlayerStatus.player_girl_rusk_lv = _nowlv;
                _itemType_subtext = "ラスク";
                break;
            case "SumireSuger":
                PlayerStatus.player_girl_rareokashi_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_rareokashi_exp;
                _nowlv = PlayerStatus.player_girl_rareokashi_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_rareokashi_exp = _nowexp;
                PlayerStatus.player_girl_rareokashi_lv = _nowlv;
                _itemType_subtext = "レアお菓子";
                break;
            case "Tea":
                PlayerStatus.player_girl_tea_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_tea_exp;
                _nowlv = PlayerStatus.player_girl_tea_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_tea_exp = _nowexp;
                PlayerStatus.player_girl_tea_lv = _nowlv;
                _itemType_subtext = "ティー";
                break;
            case "Tea_Mat":
                PlayerStatus.player_girl_tea_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_tea_exp;
                _nowlv = PlayerStatus.player_girl_tea_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_tea_exp = _nowexp;
                PlayerStatus.player_girl_tea_lv = _nowlv;
                _itemType_subtext = "ティー";
                break;
            case "Tea_Potion":
                PlayerStatus.player_girl_tea_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_tea_exp;
                _nowlv = PlayerStatus.player_girl_tea_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_tea_exp = _nowexp;
                PlayerStatus.player_girl_tea_lv = _nowlv;
                _itemType_subtext = "ティー";
                break;
            default: //一致しないものは何もしない
                break;
        }

        
    }

    void Check_OkashilvUP()
    {
        if (_nowlv >= 9) //9がカンスト
        { }
        else
        {
            if (_nowexp >= GameMgr.Hikariokashi_Exptable[_nowlv])
            {
                _nowexp = 0;
                _nowlv++;
            }
        }
    }

    //少し難しめのお菓子の経験テーブル
    void Check_OkashilvUP2()
    {
        if (_nowlv >= 9) //9がカンスト
        { }
        else
        {
            if (_nowexp >= GameMgr.Hikariokashi_Exptable2[_nowlv])
            {
                _nowexp = 0;
                _nowlv++;
            }
        }
    }


    //SetImageから読み出し
    public void ResultHikariMakeCardView_andOFF()
    {
        black_Image.SetActive(false);
        text_area.SetActive(false);
        effect_Particle_KiraExplode.SetActive(false);
        effect_Particle_KiraExplode_2.SetActive(false);
        DeleteCard_DrawView2();
    }

    IEnumerator Select3_finalcheck()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_Image.SetActive(false);
        yes_no_panel.SetActive(false);
        text_area.SetActive(false);
        yes_selectitem_kettei.onclick = false;

        DeleteCard_DrawView2();

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                GetYosokuItem();                               

                break;

            case false:

                Debug.Log("cancel");

                break;

        }

        SelectHyouji_OnOFF();
        paramHyoujiKoushin();
        CharaIconChange();
    }

    public void GetYosokuItem()
    {
        //お菓子作りやめる
        DeleteCard_DrawView();

        //一度contentの中身を削除
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        Emptypanel.SetActive(true);
        makeTimePanel.SetActive(false);
        GameMgr.hikari_make_okashiFlag = false;
        GameMgr.hikari_makeokashi_startflag = false;

        _hukidashi.text = "にいちゃん！" + "\n" + "ヒカリ、何作ろうかなぁ～？";

        //うけとる処理
        if (GameMgr.hikari_make_okashiKosu >= 1)
        {
            compound_keisan.HikariMakeGetItem();
            GameMgr.hikari_make_okashiKosu = 0;
        }
        else
        {
            GameMgr.hikari_make_okashiKosu = 0;
        }
    }

    void CharaIconChange()
    {
        if(GameMgr.hikari_make_okashiKosu >= 1)
        {
            chara_Icon.GetComponent<Image>().sprite = charaIcon_sprite_2;
        }
        else
        {
            chara_Icon.GetComponent<Image>().sprite = charaIcon_sprite_1;
        }
    }

    void paramHyoujiKoushin()
    {
        makeokasi_kosu.text = GameMgr.hikari_make_okashiKosu.ToString();
        if (!GameMgr.hikari_make_okashiFlag)
        {
            timecost_kosu.text = "-";
        }
        else
        {
            timecost_kosu.text = (GameMgr.hikari_make_okashiTimeCost / 60.0f).ToString("F1");
        }
    }

    //ボインとはじくようなアニメ
    void Result_animOn(int _card_num)
    {
        resulttransform = _cardImage_obj2[_card_num].transform;
        //resultPos = resulttransform.localPosition;
        //resultScale = resulttransform.localScale;

        {
            Sequence sequence = DOTween.Sequence();

            //まず、初期値。
            _cardImage_obj2[0].GetComponent<CanvasGroup>().alpha = 0;
            sequence.Append(resulttransform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.0f));
            //sequence.Join(resulttransform.DOLocalMove(new Vector3(0, 0, 0), 0.0f)
            //); //
            //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

            //移動のアニメ
            sequence.Append(resulttransform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.75f)
                .SetEase(Ease.OutElastic));
            /*sequence.Join(resulttransform.DOLocalMove(new Vector3(0f, 80f, 0), 0.75f)
                .SetRelative()
                .SetEase(Ease.OutExpo)); //元の位置に戻る。*/
            sequence.Join(_cardImage_obj2[0].GetComponent<CanvasGroup>().DOFade(1, 0.2f));
        }

    }
}
