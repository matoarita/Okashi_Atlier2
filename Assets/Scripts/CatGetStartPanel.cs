using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatGetStartPanel : MonoBehaviour
{
    private GameObject canvas;

    private SoundController sc;

    private CatDataBase catDataBase;
    private ItemMatPlaceDataBase matplace_database;

    private GameObject text_area_compound;
    private Text _textcomp;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject getmatplace_panel;
    private GetMatPlace_Panel getmatplace;

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数

    public List<GameObject> _listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count;

    private GameObject finalcheck_panel;
    private GameObject yes_no_panel_comeback;
    private GameObject black_img;

    private CatGetContent _toggle_catID;
    private Image _Img;
    private Sprite texture2d;

    private bool closebutton;
    private int i, mapid;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetInit()
    {
        //ねこデータベースの取得
        catDataBase = CatDataBase.Instance.GetComponent<CatDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //材料採取地パネルの取得
        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel/Comp").gameObject;
        getmatplace = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();

        //windowテキストエリアの取得
        text_area_compound = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/MessageWindowComp").gameObject;
        _textcomp = text_area_compound.GetComponentInChildren<Text>();

        //Yes no パネルの取得
        yes_no_panel_comeback = this.transform.Find("Yes_no_Panel_ComeBack").gameObject;
        yes_no_panel_comeback.SetActive(false);

        //Yes no を判別する用のオブジェクトの取得
        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        finalcheck_panel = this.transform.Find("FinalCheckPanel").gameObject;
        finalcheck_panel.SetActive(false);

        black_img = this.transform.Find("BlackImage").gameObject;
        black_img.SetActive(false);

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Comp/CatGetView/Viewport/Content").gameObject;
        textPrefab = (GameObject)Resources.Load("Prefabs/CatGetContent");
    
    }

    private void OnEnable()
    {
        SetInit();

        reset_and_DrawView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // リストビューの描画部分。重要。
    public void reset_and_DrawView()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _listitem.Clear();

        for (i = 0; i < catDataBase.catdata_list.Count; i++)
        {
            catlist_hyouji();
        }
    }

    //リストにアイテム名（デフォルトアイテム）を表示する処理
    void catlist_hyouji()
    {
        //Debug.Log(i);
        _listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。        
        _Img = _listitem[list_count].transform.Find("CatIcon").GetComponent<Image>(); //アイテムの画像データ

        _toggle_catID = _listitem[list_count].GetComponent<CatGetContent>();

        _toggle_catID.toggle_listid = i; //リスト番号を、リストビューのトグル自体にも記録させておく。 
        _toggle_catID.toggle_catid = catDataBase.catdata_list[i].catID; //固有ID string

        _listitem[list_count].transform.Find("CatName").GetComponent<Text>().text = catDataBase.catdata_list[i].catnameHyouji; //ねこの名前
        _listitem[list_count].transform.Find("EsaPanel/EsaText").GetComponent<Text>().text = catDataBase.catdata_list[i].catCost.ToString(); //ねこのエサ代
        _listitem[list_count].transform.Find("CatStatusText").GetComponent<Text>().text = catDataBase.CatStatusTextLibrary(i);

        //画像を変更
        texture2d = catDataBase.SetSprite(i);
        _Img.sprite = texture2d;


        ++list_count;
    }

    void catStatus_Redraw()
    {
        for (i = 0; i < _listitem.Count; i++)
        {
            _listitem[i].transform.Find("CatStatusText").GetComponent<Text>().text = catDataBase.CatStatusTextLibrary(i);
        }
    }

    public void OnCatGet_MapSelect()
    {
        GameMgr.compound_select = 41;

        _textcomp.text = "どこの採取地に行かせる？";

        GameMgr.OnCatGetMaterial_modeON = true;
        getmatplace.SetInit();
        getmatplace_panel.SetActive(true);
    }

    public void OnCatGet_PlayCancel() //採取にでかけてるねこをキャンセルするかきく
    {
        mapid = matplace_database.SearchMapString(catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_MapName);

        _textcomp.text = GameMgr.Select_cat_nameHyouji + "　採取中: "  + matplace_database.matplace_lists[mapid].placeNameHyouji + "\n" + "この子を採取から戻す？";

        yes_no_panel_comeback.SetActive(true);
        finalcheck_panel.SetActive(true);
        FinalCheck_CatDataKoushin(GameMgr.Select_cat_num);

        StartCoroutine("Modosu_kakunin");
    }

    IEnumerator Modosu_kakunin()
    {
        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。
        while (yes_selectitem_kettei.onclick != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false;

        finalcheck_panel.SetActive(false);
        yes_no_panel_comeback.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true: //決定が押された

                catDataBase.catdata_list[GameMgr.Select_cat_num].catStatus = 0;
                _textcomp.text = GameMgr.Select_cat_nameHyouji + "を連れ戻した！";

                catStatus_Redraw();               
                break;

            case false: //キャンセルが押された

                _textcomp.text = "ねこに材料をとってきてもらおう！" + "\n" + "好きなねこを選んでね。";
                break;
        }

    }

    public void OnCancelButton()
    {
        GameMgr.compound_status = 6; //調合シーンに入っています、というフラグ
        GameMgr.compound_select = 6;

        GameMgr.OnCatGetMaterial_modeON = false;
        this.gameObject.SetActive(false);
    }

    //GetMatPlace_Panelからよみだし　finalcheckパネルの猫データを更新
    public void FinalCheck_CatDataKoushin(int _catid)
    {
        //ねこの画像
        finalcheck_panel.transform.Find("CatDataPanel/CatIcon").GetComponent<Image>().sprite = catDataBase.SetSprite(_catid);

        //ねこの名前
        finalcheck_panel.transform.Find("CatDataPanel/NameText").GetComponent<Text>().text = catDataBase.catdata_list[_catid].catnameHyouji;
    }

    public void CatGet_FinalOK(int _catid, int _map_listid)
    {
        //選んだ猫を採取地へいかせる　決定
        catDataBase.SetCatGotoMap(_catid, matplace_database.matplace_lists[_map_listid].placeName);
        GameMgr.catGetMat_PlayFlag = true;

        sc.PlaySe(239); //ねこごとに鳴き声変わる

        closebutton = false;
        StartCoroutine("WaitInteract");
        StartCoroutine("WaitCloseButtonON");
    }

    IEnumerator WaitCloseButtonON()
    {
        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。
        while (closebutton != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        closebutton = false; //オンクリックのフラグはオフにしておく。

        //閉じる　ねこ選択画面に戻る
        _textcomp.text = "採取中..";

        getmatplace.CatGet_AllOFF();
        finalcheck_panel.SetActive(false);
    }

    IEnumerator WaitInteract()
    {
        yield return new WaitForSeconds(0.5f); //1秒待つ

        this.transform.Find("FinalCheckPanel/CloseButton").gameObject.SetActive(true);
    }

    public void OnCloseButton()
    {
        closebutton = true;
    }

    
}
