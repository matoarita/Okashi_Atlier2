using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    private List<GameObject> _listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count;

    private List<GameObject> _effect_list = new List<GameObject>(); //

    private GameObject catlist_iconanim;
    private GameObject catlist_iconanim_prefab;

    private GameObject finalcheck_panel;
    private GameObject namechange_panel;
    private GameObject esaChange_panel;
    private GameObject catzairyocheck_panel;

    private GameObject yes_no_panel_comeback;
    private GameObject yes_no_panel_fire;
    private GameObject black_img;

    private GameObject charapanel_obj;
    private GameObject effparticle_Prefab1;

    private CatGetContent _toggle_catID;
    private Image _Img;
    private Sprite texture2d;

    private int Nokori_time, hour_count;

    private bool closebutton;
    private int i, mapid;

    //オブジェクトと結びつける
    private InputField inputField_catname;
    private Text namechange_origintext;
    private string namechange_text;

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
        yes_no_panel_fire = this.transform.Find("Yes_no_Panel_Fire").gameObject;
        yes_no_panel_fire.SetActive(false);

        //Yes no を判別する用のオブジェクトの取得
        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        finalcheck_panel = this.transform.Find("FinalCheckPanel").gameObject;
        finalcheck_panel.SetActive(false);

        finalcheck_panel.transform.Find("CatDataPanel/CatIconBaseImg/CatIcon").gameObject.SetActive(true);
        finalcheck_panel.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim").gameObject.SetActive(false);
        charapanel_obj = finalcheck_panel.transform.Find("CatDataPanel/CatIconBaseImg").gameObject;

        namechange_panel = this.transform.Find("NameChangePanel").gameObject;
        namechange_panel.SetActive(false);

        esaChange_panel = this.transform.Find("EsaChangePanel").gameObject;
        esaChange_panel.SetActive(false);

        catzairyocheck_panel = this.transform.Find("CatZairyoCheckPanel").gameObject;
        catzairyocheck_panel.SetActive(false);

        inputField_catname = namechange_panel.transform.Find("CatDataPanel/InputField").GetComponent<InputField>();
        namechange_origintext = namechange_panel.transform.Find("CatDataPanel/NameText").GetComponent<Text>();

        black_img = this.transform.Find("BlackImage").gameObject;
        black_img.SetActive(false);

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Comp/CatGetView/Viewport/Content").gameObject;
        textPrefab = (GameObject)Resources.Load("Prefabs/CatGetContent");

        effparticle_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_KiraExplode_3");

        catlist_iconanim_prefab = (GameObject)Resources.Load("Prefabs/CatIconAnim");
        
    }

    private void OnEnable()
    {
        SetInit();

        reset_and_DrawView();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameMgr.CatStartPanel_HyoujiKoushinFlag) //LVアップしたときに、すぐに表示更新する
        {
            GameMgr.CatStartPanel_HyoujiKoushinFlag = false;

            catStatus_Redraw();
        }
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
        _Img = _listitem[list_count].transform.Find("CatIconBaseImg/CatIcon").GetComponent<Image>(); //アイテムの画像データ

        catlist_iconanim = Instantiate(catlist_iconanim_prefab, _listitem[list_count].transform.Find("CatIconBaseImg/Catlist_iconanim").transform);
        catlist_iconanim.name = "CatIconAnim";
        catlist_iconanim.transform.DOScale(0.8f, 0.0f);

        _toggle_catID = _listitem[list_count].GetComponent<CatGetContent>();

        _toggle_catID.toggle_listid = i; //リスト番号を、リストビューのトグル自体にも記録させておく。 
        _toggle_catID.toggle_catid = catDataBase.catdata_list[i].catID; //固有ID string

        catStatus_ContentDraw(list_count);
        
        //画像を変更
        texture2d = catDataBase.SetSprite(i, 0);
        _Img.sprite = texture2d;

        //アニメアイコン変更
        _listitem[list_count].transform.Find("CatIconBaseImg/Catlist_iconanim/CatIconAnim/" + catDataBase.SetCatAnimObj(i, 0)).gameObject.SetActive(true);

        //静止画かアニメアイコンどちらを使う
        catlist_iconanim.SetActive(false); //anim
        _listitem[list_count].transform.Find("CatIconBaseImg/CatIcon").gameObject.SetActive(true); //Img

        ++list_count;
    }

    void catStatus_Redraw()
    {
        for (i = 0; i < _listitem.Count; i++)
        {
            catStatus_ContentDraw(i);           
        }
    }

    void catStatus_ContentDraw(int _num)
    {
        _listitem[_num].transform.Find("CatStatusText").GetComponent<Text>().text = catDataBase.CatStatusTextLibrary(_num);
        _listitem[_num].transform.Find("CatName").GetComponent<Text>().text = catDataBase.catdata_list[_num].catnameHyouji;
        _listitem[_num].transform.Find("EsaPanel/EsaText").GetComponent<Text>().text = catDataBase.catdata_list[_num].catCost.ToString();
        _listitem[_num].transform.Find("CatLv_text").GetComponent<Text>().text = catDataBase.catdata_list[_num].catLv.ToString();       
        _listitem[_num].transform.Find("CatTansakuSp_CounterText").GetComponent<Text>().text = catDataBase.catdata_list[_num].cat_GetMateriaTimeCounter.ToString();
        _listitem[_num].transform.Find("CatTansakuCounter").GetComponent<Slider>().maxValue = catDataBase.catdata_list[_num].catTansaku_Speed;
        _listitem[_num].transform.Find("CatTansakuCounter").GetComponent<Slider>().value = catDataBase.catdata_list[_num].cat_GetMateriaTimeCounter;

        SetMinuteHour_Tansaku(_num);
    }

    void SetMinuteHour_Tansaku(int _num)
    {
        //表記を時間と分に変更
        hour_count = 0;
        Nokori_time = catDataBase.catdata_list[_num].cat_GetMateriaTimeCounter;

        while(Nokori_time >= 60)
        {
            Nokori_time -= 60;
            hour_count++;
        }

        _listitem[_num].transform.Find("CatTansakuSp_Hour").GetComponent<Text>().text = hour_count.ToString();
        _listitem[_num].transform.Find("CatTansakuSp_Minute").GetComponent<Text>().text = Nokori_time.ToString();
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

        CatIconImage_Hyouji(finalcheck_panel);
        FinalCheck_CatDataKoushin(finalcheck_panel, GameMgr.Select_cat_num);

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

                //それまでとってきた材料リストは一度削除
                catDataBase.CatDeleteZairyoList(GameMgr.Select_cat_num);

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
    public void FinalCheck_CatDataKoushin(GameObject _obj, int _catid)
    {
        //CatIconAnim_Hyouji(_obj);

        //ねこの画像
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIcon").GetComponent<Image>().sprite = catDataBase.SetSprite(_catid, 0);

        //ねこの画像アニメ
        foreach(Transform child in _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim").gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim/" + catDataBase.SetCatAnimObj(_catid, 0)).gameObject.SetActive(true);

        //ねこの名前
        _obj.transform.Find("CatDataPanel/NameText").GetComponent<Text>().text = catDataBase.catdata_list[_catid].catnameHyouji;
    }

    void CatIconAnim_Hyouji(GameObject _obj) //FinalCheckのときにアニメアイコンを表示　画像は非表示
    {
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIcon").gameObject.SetActive(false);
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim").gameObject.SetActive(true);
    }

    void CatIconImage_Hyouji(GameObject _obj) //FinalCheckのときに画像を表示
    {
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIcon").gameObject.SetActive(true);
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim").gameObject.SetActive(false);
    }

    public void CatGet_FinalOK(int _catid, int _map_listid)
    {
        //選んだ猫を採取地へいかせる　決定
        
        catDataBase.SetCatGotoMap(_catid, matplace_database.matplace_lists[_map_listid].placeName);
        GameMgr.catGetMat_PlayFlag = true;

        //それまでとってきた材料リストは一度削除
        catDataBase.CatDeleteZairyoList(_catid);

        //** 演出系 **//        
        CatIconAnim_Hyouji(finalcheck_panel); //アイコンを変える
        FinalCheck_CatDataKoushin(finalcheck_panel, _catid);

        sc.PlaySe(catDataBase.SetVoice(_catid, 0)); //ねこごとに鳴き声変わる

        sc.PlaySe(25);

        _effect_list.Clear();
        _effect_list.Add(Instantiate(effparticle_Prefab1, finalcheck_panel.transform));
        AnimPoyon(); //ぽよんアニメ
        //

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

        //エフェクトも削除
        Destroy(_effect_list[0].gameObject);
        _effect_list.Clear(); 

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

    public void OnNameChangePanel()
    {
        sc.PlaySe(34);
        namechange_panel.SetActive(true);
        text_area_compound.SetActive(false);

        CatIconImage_Hyouji(namechange_panel);
        FinalCheck_CatDataKoushin(namechange_panel, GameMgr.Select_cat_num);      
    }

    public void Input_CatName()
    {
        namechange_origintext.text = inputField_catname.text;
        namechange_text = inputField_catname.text;
    }

    public void CatName_OK()
    {
        catDataBase.catdata_list[GameMgr.Select_cat_num].catnameHyouji = namechange_text;
        catStatus_Redraw();
        namechange_panel.SetActive(false);

        text_area_compound.SetActive(true);
        _textcomp.text = "名前を変更したよ！";
    }

    public void CatName_Cancel()
    {
        namechange_panel.SetActive(false);

        text_area_compound.SetActive(true);
    }

    public void OnCatEsaPanel()
    {
        esaChange_panel.SetActive(true);

        _textcomp.text = "エサを変更するよ！" + "\n" + "高いエサほど、いっぱい材料をとってきてくれるよ！";
    }

    public void OnCatFirePanel()
    {
        yes_no_panel_fire.SetActive(true);
        finalcheck_panel.SetActive(true);

        CatIconImage_Hyouji(finalcheck_panel);
        FinalCheck_CatDataKoushin(finalcheck_panel, GameMgr.Select_cat_num);

        _textcomp.text = "解雇すると、猫は出てっちゃうよ..。" + "\n" + "もう二度と会えないけど、ほんとにいいの？";

        StartCoroutine("Fire_kakunin");
    }

    IEnumerator Fire_kakunin()
    {
        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。
        while (yes_selectitem_kettei.onclick != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false;

        finalcheck_panel.SetActive(false);
        yes_no_panel_fire.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true: //決定が押された

                catDataBase.Sayonara_Cat(GameMgr.Select_cat_num);
                _textcomp.text = GameMgr.Select_cat_nameHyouji + "は、悲しい顔で去っていった..。";

                reset_and_DrawView(); //リストが移動するので、一度全部書き直し
                break;

            case false: //キャンセルが押された

                _textcomp.text = "ねこに材料をとってきてもらおう！" + "\n" + "好きなねこを選んでね。";
                break;
        }

    }

    public void OnEsaSelectButton(int _status)
    {
        switch(_status)
        {
            case 1:

                catDataBase.catdata_list[GameMgr.Select_cat_num].catCost = 50;
                catDataBase.catdata_list[GameMgr.Select_cat_num].catCostLV = 1;

                catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_Speed = (int)(catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_DefaultSpeed * 2.0f);
                catDataBase.catdata_list[GameMgr.Select_cat_num].cat_GetMateriaTimeCounter = catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_Speed;
                break;

            case 2:

                catDataBase.catdata_list[GameMgr.Select_cat_num].catCost = 250;
                catDataBase.catdata_list[GameMgr.Select_cat_num].catCostLV = 2;

                catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_Speed = (int)(catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_DefaultSpeed * 1.0f);
                catDataBase.catdata_list[GameMgr.Select_cat_num].cat_GetMateriaTimeCounter = catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_Speed;
                break;

            case 3:

                catDataBase.catdata_list[GameMgr.Select_cat_num].catCost = 500;
                catDataBase.catdata_list[GameMgr.Select_cat_num].catCostLV = 3;

                catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_Speed = (int)(catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_DefaultSpeed * 0.75f);
                catDataBase.catdata_list[GameMgr.Select_cat_num].cat_GetMateriaTimeCounter = catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_Speed;
                break;

            case 4:

                catDataBase.catdata_list[GameMgr.Select_cat_num].catCost = 1000;
                catDataBase.catdata_list[GameMgr.Select_cat_num].catCostLV = 4;

                catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_Speed = (int)(catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_DefaultSpeed * 0.5f);
                catDataBase.catdata_list[GameMgr.Select_cat_num].cat_GetMateriaTimeCounter = catDataBase.catdata_list[GameMgr.Select_cat_num].catTansaku_Speed;
                break;

            case 9:

                //変更なし
                break;
        }

        if (_status != 9)
        {
            _textcomp.text = "エサ代を変更した！"; //効果が反映されるのを次の日にしないと、直前でエサ代を安くすませるワザを使われてしまう。
        }
        catStatus_Redraw();
        esaChange_panel.SetActive(false);
    }

    public void OnZairyoCheckPanel()
    {
        catzairyocheck_panel.SetActive(true);
        catzairyocheck_panel.GetComponent<CatZairyoCheckPanel>().OpenPanel(GameMgr.Select_cat_num);
    }

    public void CloseZairyoCheckButton()
    {
        catzairyocheck_panel.SetActive(false);
    }

    void AnimPoyon()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        charapanel_obj.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(charapanel_obj.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.0f));


        //移動のアニメ
        sequence.Append(charapanel_obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.75f)
            .SetEase(Ease.OutElastic));

        sequence.Join(charapanel_obj.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    public void Debug_CatRandomAdd()
    {

        catDataBase.SetInit_CustomCatData("", Random.Range(0, 2), Random.Range(0, 4), 250, Random.Range(200, 800), Random.Range(2, 6), 1, 0);
        reset_and_DrawView();
    }

    
}
