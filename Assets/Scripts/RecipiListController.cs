using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RecipiListController : MonoBehaviour {

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    public List<GameObject> _recipi_listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text _text;
    private recipiitemSelectToggle _toggle_itemID;

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。

    private PlayerItemList pitemlist;

    public List<GameObject> _cardImage_obj = new List<GameObject>(); //カード表示用のゲームオブジェクト
    private GameObject canvas;
    private GameObject cardPrefab;

    private Sprite texture2d;
    private Image _Img;
    private Image _TextBGImg;
    private GameObject _HighStar;
    private GameObject _HighStar_2;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private string item_name;
    private int item_kosu;

    private Girl1_status girl1_status;

    private GameObject comp_text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _comp_text; //同じく、Scene「Compund」用。

    private Text recipititle;

    private GameObject black_panel;

    private int max;
    private int count;
    private int i, j;

    public int _count1;

    public int final_kettei_recipiitemID_1;

    public int kettei_recipiitem1; //最終的に確定したアイテムのID（アイテムデータベースのIDを同一）
    public int kettei_recipiitem2;
    public int kettei_recipiitem3;

    public int kettei_toggle_type1;
    public int kettei_toggle_type2;
    public int kettei_toggle_type3;

    public int result_recipiitem;
    public int result_recipicompID; //最終的に確定したときの、コンポDBのアイテムID

    public int final_kettei_recipikosu1;
    public int final_kettei_recipikosu2;
    public int final_kettei_recipikosu3;
    public int final_select_kosu;

    public bool final_recipiselect_flag;    

    private GameObject yes_button;
    private GameObject no_button;

    public List<GameObject> category_toggle = new List<GameObject>();
    private int category_status;

    // Use this for initialization
    void Awake () {

        InitSetting();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitSetting()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = GameObject.FindWithTag("RecipiListContent");
        textPrefab = (GameObject)Resources.Load("Prefabs/recipiitemSelectToggle");

        //カードのプレファブコンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

        i = 0;

        foreach (Transform child in this.transform.Find("CategoryViewRecipi/Viewport/Content/").transform)
        {
            //Debug.Log(child.name);           
            category_toggle.Add(child.gameObject);
        }
        category_status = 0;

        recipititle = this.transform.Find("Scroll_view_title/Scroll_view_title_text").GetComponent<Text>();
    }

    void OnEnable()
    {
        //ウィンドウがアクティヴになった瞬間だけ読み出される
        //Debug.Log("OnEnable");  

        yes_button = this.transform.Find("Yes").gameObject;
        no_button = this.transform.Find("No").gameObject;
        black_panel = this.transform.Find("BlackImage").gameObject;
        yes_button.SetActive(false);
        no_button.SetActive(false);
        black_panel.SetActive(false);

        final_recipiselect_flag = false;
        for (i = 0; i < category_toggle.Count; i++)
        {
            category_toggle[i].GetComponent<Toggle>().isOn = false;
            category_toggle[i].SetActive(true);
        }

        //初期位置
        //this.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
        //this.transform.localPosition = new Vector3(-105, 80, 0);

        switch (GameMgr.compound_select)
        {
            case 1:

                //初期位置
                this.transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
                this.transform.localPosition = new Vector3(-105, 75, 0);

                category_toggle[0].SetActive(false);
                category_toggle[1].GetComponent<Toggle>().isOn = true;
                
                reset_and_DrawView_Okashi();
                recipititle.text = "お菓子手帳";
                break;

            case 60:

                //初期位置
                this.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
                this.transform.localPosition = new Vector3(0, 75, 0);

                no_button.SetActive(true);

                category_toggle[1].SetActive(false);
                category_toggle[0].GetComponent<Toggle>().isOn = true;
                
                reset_and_DrawView();
                recipititle.text = "レシピブック";
                break;

            case 3000: //オマケで開く場合

                //初期位置
                this.transform.localScale = new Vector3(0.85f, 0.85f, 1.0f);
                this.transform.localPosition = new Vector3(0, 40, 0);

                category_toggle[0].SetActive(false);
                category_toggle[1].GetComponent<Toggle>().isOn = true;

                reset_and_DrawView_Okashi();
                recipititle.text = "お菓子手帳";
                break;
        }

        

        OpenAnim();
    }

    //アニメーション
    void OpenAnim()
    {
        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。

        sequence.Append(this.transform.DOLocalMove(new Vector3(50f, 0f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。
        sequence.Join(this.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }


    public void RecipiList_DrawView()
    {
        if (category_toggle[0].GetComponent<Toggle>().isOn == true)
        {
            category_status = 0;
            reset_and_DrawView();
        }
    }

    public void RecipiList_DrawView2()
    {
        if (category_toggle[1].GetComponent<Toggle>().isOn == true)
        {
            category_status = 1;
            reset_and_DrawView_Okashi();
        }
    }

    public void RecipiList_Draw()
    {
        //アイテムリストを表示中に、アイテムを追加した場合、リアルタイムに表示を更新する
        switch (category_status)
        {
            case 0:

                reset_and_DrawView();
                break;

            case 1:

                reset_and_DrawView_Okashi();
                break;

        }
    }


    // リストビューの描画部分。重要。
    void reset_and_DrawView()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _recipi_listitem.Clear();

        
        if (GameMgr.tutorial_ON == true)
        {
            //チュートリアル中は、イベントレシピは見れないようにする。  
        }
        else
        {
            //イベント用レシピのフラグをチェック。レシピリストから、さらに読めるものを表示。章クリア用のメモなど。
            for (i = 0; i < pitemlist.eventitemlist.Count; i++)
            {
                if (pitemlist.eventitemlist[i].ev_itemKosu > 0 && pitemlist.eventitemlist[i].ev_ListOn == 1) //イベントレシピを所持してる　かつ　リスト表示がONのものを表示
                {
                    //Debug.Log(i);
                    drawEvRecipi();

                }
            }
        }
    }

    //集めたお菓子のブック
    void reset_and_DrawView_Okashi()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _recipi_listitem.Clear();


        //調合DBのフラグをチェック
        for (i = 0; i < databaseCompo.compoitems.Count; i++)
        {
            //調合DBのフラグが1（調合したことがある）かつ、ゲーム中に登場するフラグがONのやつを、表示。そのときに、格納されてる配列番号=iをtoggleに保持する。
            if (databaseCompo.compoitems[i].cmpitem_flag == 1 && databaseCompo.compoitems[i].recipi_count == 1)
            {
                //Debug.Log(i);
                drawNormalRecipi();

            }
            else if (databaseCompo.compoitems[i].cmpitem_flag == 0 && databaseCompo.compoitems[i].recipi_count == 1)
            {
                //Debug.Log(i);
                drawEmptyRecipi();

            }

            //Debug.Log(databaseCompo.compoitems[i].cmpitem_Name + ": databaseCompo.compoitems[i].cmpitem_flag: " + databaseCompo.compoitems[i].cmpitem_flag);
        }
    }

    void drawEvRecipi()
    {
        _recipi_listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _recipi_listitem[list_count].GetComponentInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。
        _Img = _recipi_listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ
        _TextBGImg = _recipi_listitem[list_count].transform.Find("Background/TextBG").GetComponent<Image>();
        _TextBGImg.color = new Color(239f / 250f, 184f / 255f, 255f / 255f);
        _HighStar = _recipi_listitem[list_count].transform.Find("Background/HighScoreStar").gameObject;
        _HighStar_2 = _recipi_listitem[list_count].transform.Find("Background/HighScoreStar_2").gameObject;

        _toggle_itemID = _recipi_listitem[list_count].GetComponent<recipiitemSelectToggle>();
        _toggle_itemID.recipi_toggleEventitem_ID = i; //イベントアイテムIDを、リストビューのトグル自体にも記録させておく。
        _toggle_itemID.recipi_toggleEventType = 0; //イベントアイテムタイプなので、0


        j = 0;

        //調合DBの生成アイテムはローマ字表記なので、データベースから、日本語表記をひっぱってくる。
        item_name = pitemlist.eventitemlist[i].event_itemNameHyouji;

        _toggle_itemID.recipi_itemNameHyouji = item_name;

        _text.text = item_name;
        //_text.color = new Color(153f / 255f, 89f / 255f, 201f / 255f); //9959C980

        //画像を変更
        texture2d = Resources.Load<Sprite>("Sprites/" + pitemlist.eventitemlist[i].event_fileName);
        _Img.sprite = texture2d;

        _HighStar.SetActive(false);
        _HighStar_2.SetActive(false);

        ++list_count;
    }

    void drawNormalRecipi()
    {
        _recipi_listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _recipi_listitem[list_count].GetComponentInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。
        _Img = _recipi_listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ
        _TextBGImg = _recipi_listitem[list_count].transform.Find("Background/TextBG").GetComponent<Image>();
        _TextBGImg.color = new Color(255f / 255f, 250f / 255f, 184f / 255f);
        _HighStar = _recipi_listitem[list_count].transform.Find("Background/HighScoreStar").gameObject;
        _HighStar_2 = _recipi_listitem[list_count].transform.Find("Background/HighScoreStar_2").gameObject;

        _toggle_itemID = _recipi_listitem[list_count].GetComponent<recipiitemSelectToggle>();
        _toggle_itemID.recipi_toggleCompoitem_ID = i; //コンポアイテムIDを、リストビューのトグル自体にも記録させておく。
        _toggle_itemID.recipi_toggleEventType = 1; //コンポ調合アイテムタイプなので、1

        _toggle_itemID._empty_flag = true;
        _recipi_listitem[list_count].GetComponent<Toggle>().interactable = true;
        

        j = 0;

        //調合DBの生成アイテムはローマ字表記なので、アイテムデータベースから、日本語表記をひっぱってくる。
        while (j < database.items.Count)
        {
            if (database.items[j].itemName == databaseCompo.compoitems[i].cmpitemID_result)
            {
                item_name = database.items[j].itemNameHyouji;
                texture2d = database.items[j].itemIcon_sprite;
                _toggle_itemID.recipi_itemID = j; //アイテムデータベース上の、アイテムID（コンポデータベースではない。）

                if (database.items[j].HighScore_flag == 1)
                {
                    _HighStar.SetActive(true);
                    _HighStar_2.SetActive(false);
                }
                else if (database.items[j].HighScore_flag == 2)
                {
                    _HighStar.SetActive(true);
                    _HighStar_2.SetActive(true);
                } else
                {
                    _HighStar.SetActive(false);
                    _HighStar_2.SetActive(false);
                }

                break;
            }
            ++j;
        }

        _toggle_itemID.recipi_itemNameHyouji = item_name;

        _text.text = item_name;

        //画像を変更              
        _Img.sprite = texture2d;

        ++list_count;
    }

    //空　？のやつ
    void drawEmptyRecipi()
    {
        _recipi_listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _recipi_listitem[list_count].GetComponentInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。
        _Img = _recipi_listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ
        _TextBGImg = _recipi_listitem[list_count].transform.Find("Background/TextBG").GetComponent<Image>();
        _TextBGImg.color = new Color(255f / 255f, 250f / 255f, 184f / 255f);
        _HighStar = _recipi_listitem[list_count].transform.Find("Background/HighScoreStar").gameObject;
        _HighStar_2 = _recipi_listitem[list_count].transform.Find("Background/HighScoreStar_2").gameObject;

        _toggle_itemID = _recipi_listitem[list_count].GetComponent<recipiitemSelectToggle>();
        _toggle_itemID.recipi_toggleCompoitem_ID = i; //コンポアイテムIDを、リストビューのトグル自体にも記録させておく。
        _toggle_itemID.recipi_toggleEventType = 1; //コンポ調合アイテムタイプなので、1

        _toggle_itemID._empty_flag = false;
        _recipi_listitem[list_count].GetComponent<Toggle>().interactable = false;


        j = 0;

        //調合DBの生成アイテムはローマ字表記なので、アイテムデータベースから、日本語表記をひっぱってくる。
        while (j < database.items.Count)
        {
            if (database.items[j].itemName == databaseCompo.compoitems[i].cmpitemID_result)
            {
                item_name = database.items[j].itemNameHyouji;
                texture2d = database.items[j].itemIcon_sprite;                

                _toggle_itemID.recipi_itemID = j; //アイテムデータベース上の、アイテムID（コンポデータベースではない。）

                if (database.items[j].HighScore_flag == 1)
                {
                    _HighStar.SetActive(true);
                    _HighStar_2.SetActive(false);
                }
                else if (database.items[j].HighScore_flag == 2)
                {
                    _HighStar.SetActive(true);
                    _HighStar_2.SetActive(true);
                }
                else
                {
                    _HighStar.SetActive(false);
                    _HighStar_2.SetActive(false);
                }

                break;
            }
            ++j;
        }

        _toggle_itemID.recipi_itemNameHyouji = item_name;

        _text.text = "???";

        //画像を変更              
        _Img.sprite = texture2d;
        //空の場合は、真っ黒にする。
        _Img.color = Color.black;

        ++list_count;
    }

    //集めたお菓子のブック　の描画更新のみ　処理を軽くするため
    public void Redraw_OkashiBook()
    {
        //調合DBのフラグをチェック
        for (i = 0; i < _recipi_listitem.Count; i++)
        {
            //調合DBのフラグが1（調合したことがある）かつ、ゲーム中に登場するフラグがONのやつを、表示。そのときに、格納されてる配列番号=iをtoggleに保持する。
            if (_recipi_listitem[i].GetComponent<recipiitemSelectToggle>()._empty_flag)
            {
                //Debug.Log(i);
                _recipi_listitem[i].GetComponent<Toggle>().interactable = true;

            }
            else if (!_recipi_listitem[i].GetComponent<recipiitemSelectToggle>()._empty_flag)
            {
                //Debug.Log(i);
                _recipi_listitem[i].GetComponent<Toggle>().interactable = false;

            }
        }
    }

    //デバッグ用　本全て解放　本を所持したことにする。
    public void Debug_R1_button()
    {
        //イベント用レシピのフラグをチェック。レシピリストから、さらに読めるものを表示。章クリア用のメモなど。
        for (i = 0; i < pitemlist.eventitemlist.Count; i++)
        {
            pitemlist.eventitemlist[i].ev_itemKosu = 1;
        }

        RecipiList_DrawView();
    }

    //デバッグ用　全てのレシピ全て解放
    public void Debug_R2_button()
    {
        //調合DBのフラグをチェック
        for (i = 0; i < databaseCompo.compoitems.Count; i++)
        {
            if (databaseCompo.compoitems[i].cmpitem_flag != 9999)
            {
                databaseCompo.compoitems[i].cmpitem_flag = 1;
            }
        }

        RecipiList_DrawView2();
    }
}
