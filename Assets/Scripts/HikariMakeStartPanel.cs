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

    private ItemDataBase database;
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

    private Text makeokasi_kosu;
    private Text timecost_kosu;

    private GameObject select_obj_1;
    private GameObject select_obj_2;

    private Sprite charaIcon_sprite_1;
    private Sprite charaIcon_sprite_2;
    private GameObject chara_Icon;

    private int i;

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

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //合成計算オブジェクトの取得
        compound_keisan = Compound_Keisan.Instance.GetComponent<Compound_Keisan>();

        charaIcon_sprite_1 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_02");
        charaIcon_sprite_2 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_07");
        chara_Icon = this.transform.Find("Comp2/CharaPanel/Image").gameObject;
        chara_Icon.GetComponent<Image>().sprite = charaIcon_sprite_1;

        cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

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

        makeokasi_kosu = this.transform.Find("Comp2/KosuPanel/Image/Kosu_param").GetComponent<Text>();
        makeokasi_kosu.text = GameMgr.hikari_make_okashiKosu.ToString();

        timecost_kosu = this.transform.Find("Comp2/TimePanel/Image/Time_param").GetComponent<Text>();
        timecost_kosu.text = (GameMgr.hikari_make_okashiTimeCost / 60.0f).ToString("F1");

        select_obj_1 = this.transform.Find("Comp2/Select_command/Scroll View/Viewport/Content/Select_2").gameObject;
        select_obj_2 = this.transform.Find("Comp2/Select_command/Scroll View/Viewport/Content/Select_3").gameObject;
        SelectHyouji_OnOFF();
        CharaIconChange();

        //一度contentの中身を削除
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }
        

        if(GameMgr.hikari_make_okashiFlag) //なんらかのお菓子を登録中
        {
            Emptypanel.SetActive(false);
            Start_ZairyoItemIconHyouji(1);
            HikariMakeResultCard_DrawView();
        }
        else
        {
            Emptypanel.SetActive(true);
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

    public void TakeResultCard_DrawView()
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

        _cardImage_obj2[0].transform.Find("CompoundResultButton").gameObject.SetActive(false);

        //店売りかオリジナルか、アイテムID        
        _cardImage.check_counter = pitemlist.player_yosokuitemlist.Count - 1;
        _cardImage.SetInitYosoku();

        _cardImage_obj2[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
        _cardImage_obj2[0].transform.localPosition = new Vector3(0, 85, 0);

        _cardImage_obj2[0].GetComponent<SetImage>().CardParamOFF_2();
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
        TakeResultCard_DrawView();

        StartCoroutine("Select2_finalcheck");
    }

    public void OnSelect_3()
    {
        yes_no_panel.SetActive(true);
        black_Image.SetActive(true);
        text_area.SetActive(true);

        _text.text = "お菓子作りをやめる？" + "\n" + "（途中まで作ったお菓子は、受け取れるよ！）";
        TakeResultCard_DrawView();

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

        black_Image.SetActive(false);
        yes_no_panel.SetActive(false);
        text_area.SetActive(false);
        yes_selectitem_kettei.onclick = false;

        DeleteCard_DrawView2();        

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //うけとる処理
                compound_keisan.HikariMakeGetItem();
                GameMgr.hikari_make_okashiKosu = 0;
                makeokasi_kosu.text = GameMgr.hikari_make_okashiKosu.ToString();

                break;

            case false:

                Debug.Log("cancel");

                break;

        }

        SelectHyouji_OnOFF();
        CharaIconChange();
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

                //お菓子作りやめる
                DeleteCard_DrawView();

                //一度contentの中身を削除
                foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
                {
                    Destroy(child.gameObject);
                }

                Emptypanel.SetActive(true);
                GameMgr.hikari_make_okashiFlag = false;

                //うけとる処理
                compound_keisan.HikariMakeGetItem();
                GameMgr.hikari_make_okashiKosu = 0;
                makeokasi_kosu.text = GameMgr.hikari_make_okashiKosu.ToString();

                break;

            case false:

                Debug.Log("cancel");

                break;

        }

        SelectHyouji_OnOFF();
        CharaIconChange();
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
}
