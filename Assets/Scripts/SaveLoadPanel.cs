using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

//プレイヤーアイテムリストのスクロールビューのコントローラー。
//調合シーンや、アイテムを女の子にあげたときの処理は、「itemSelectToggle」スクリプトに記述してます。

public class SaveLoadPanel : MonoBehaviour {

    private GameObject canvas;

    private Girl1_status girl1_status; //女の子ステータス

    private SaveController save_controller;
    private TimeController time_controller;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数

    public List<GameObject> _listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text _text_title;
    private Text _text_playtime;
    private SaveloadListSelectToggle _toggle_itemID;

    private GameObject _modetitle_obj1;
    private GameObject _modetitle_obj2;

    private Sprite icon_texture1;
    private Sprite icon_texture2;
    private Sprite icon_texture3;
    private Sprite icon_texture4;
    private Image _iconImg;

    private int count;
    private int i, n;
    private int _lv;
    private int _girllove_lv, _girlgokigen_status;
    

    // Use this for initialization
    void Start()
    {
        
    }

    void InitSetUp()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Panel/ScrollView/Viewport/Content").gameObject;
        textPrefab = (GameObject)Resources.Load("Prefabs/SaveloadListSelectToggle");           
        
        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();
        yes_selectitem_kettei.onclick = false;

        _modetitle_obj1 = this.transform.Find("Panel/titleText1").gameObject;
        _modetitle_obj2 = this.transform.Find("Panel/titleText2").gameObject;

        icon_texture1 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_04_Little_fine");
        icon_texture2 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_01_Normal");
        icon_texture3 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_02_Joukigen");
        icon_texture4 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_06_Tereru");

        i = 0;
        _listitem.Clear();
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    void OnEnable()
    {

        //ウィンドウがアクティヴになった瞬間だけ読み出される
        //Debug.Log("OnEnable");   

        InitSetUp();

        SetModeText(GameMgr.SaveLoadPanel_mode);
        save_controller.PlayerDataSaveCheck();
        reset_and_DrawView();
    }
    
    public void SetModeText(int _mstatus)
    {
        switch(_mstatus)
        {
            case 0:
                _modetitle_obj1.SetActive(true);
                _modetitle_obj2.SetActive(false);
                break;

            case 1:
                _modetitle_obj1.SetActive(false);
                _modetitle_obj2.SetActive(true);
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
        _listitem.Clear();

        //一画面で16個までセーブ可能
        for (i = 0; i < GameMgr.System_SaveSlot_Count; i++)
        {
            list_hyouji();
        }

    }
    

    //リストにアイテム名（デフォルトアイテム）を表示する処理
    void list_hyouji()
    {
        //Debug.Log(i);
        _listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。

        _iconImg = _listitem[list_count].transform.Find("SaveONButton/ImageFaceBG/ImageFaceIcon").GetComponent<Image>(); //アイテムの画像データ
        _iconImg.sprite = icon_texture2; //デフォルトセット

        _toggle_itemID = _listitem[list_count].GetComponent<SaveloadListSelectToggle>();
        _toggle_itemID._toggleID = i; //アイテムIDを、リストビューのトグル自体にも記録させておく。 

        _text_title = _listitem[list_count].transform.Find("SaveONButton/Title").GetComponent<Text>(); //
        _text_playtime = _listitem[list_count].transform.Find("SaveONButton/PlayTime").GetComponent<Text>(); //



        //以下、セーブデータの有無とセーブかロードかで、表示を4パターン変更する。
        switch(GameMgr.SaveLoadPanel_mode)
        {
            case 0: //セーブ

                if (!GameMgr.System_savepanel_slot[i]) //セーブデータがない場合
                {
                    _listitem[list_count].transform.Find("NodataButton").gameObject.SetActive(true);
                    _listitem[list_count].transform.Find("SaveONButton").gameObject.SetActive(false);
                }
                else
                {
                    //ある場合
                    _listitem[list_count].transform.Find("NodataButton").gameObject.SetActive(false);
                    _listitem[list_count].transform.Find("SaveONButton").gameObject.SetActive(true);

                    _text_title.text = "ヒカリのセーブ " + (i + 1).ToString();
                    PlayTimeHyouji();
                    GirlLoveIconHyouji();
                }

                break;

            case 1: //ロード

                if (!GameMgr.System_savepanel_slot[i]) //セーブデータがない場合
                {
                    _listitem[list_count].transform.Find("NodataButton").gameObject.SetActive(true);
                    _listitem[list_count].transform.Find("SaveONButton").gameObject.SetActive(false);

                    //さらにインタラクトもオフにする。
                    _listitem[list_count].transform.Find("NodataButton").GetComponent<Button>().interactable = false;
                }
                else
                {
                    //ある場合
                    _listitem[list_count].transform.Find("NodataButton").gameObject.SetActive(false);
                    _listitem[list_count].transform.Find("SaveONButton").gameObject.SetActive(true);

                    _text_title.text = "ヒカリのセーブ " + (i + 1).ToString();
                    PlayTimeHyouji();
                    GirlLoveIconHyouji();
                }

                break;
        }
        


        

        ++list_count;
    }

    void PlayTimeHyouji()
    {
        time_controller.SetPlayTime(GameMgr.System_savepanel_playtime[i]);
        _text_playtime.text = GameMgr.System_tempTime_Hour.ToString("00") + ":" +
            GameMgr.System_tempTime_Minute.ToString("00") + ":" +
            GameMgr.System_tempTime_Second.ToString("00"); //何分何秒にもどす
    }

    void GirlLoveIconHyouji()
    {
        _girllove_lv = GameMgr.System_savepanel_girllovelv[i];
        _girlgokigen_status = girl1_status.CheckGokigenReturn(_girllove_lv);

        //画像を変更 girl1_statusのゴキゲン状態とそろえる
        if (_girlgokigen_status < 4) //LV4まではしょぼん
        {
            _iconImg.sprite = icon_texture1;
        }
        else if (_girlgokigen_status >= 4 && _girlgokigen_status < 6) //LV35まで通常元気
        {
            _iconImg.sprite = icon_texture2;
        }
        else if (_girlgokigen_status >= 6 && _girlgokigen_status < 7) //LV35上機嫌
        {
            _iconImg.sprite = icon_texture3;
        }
        else if (_girlgokigen_status >= 7) //LV40~照れ
        {
            _iconImg.sprite = icon_texture4;
        }
    }

    public void OnNoButton()
    {
        //調合シーンでここを通るときは、特に影響がない
        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;

        this.gameObject.SetActive(false);
    }

    public void ReDraw()
    {
        save_controller.PlayerDataSaveCheck();
        reset_and_DrawView();
    }

    //一時的に全てのアイテムを触れなくする。
    public void Offinteract()
    {
        for(i=0; i < _listitem.Count; i++)
        {
            _listitem[i].transform.Find("SaveONButton").GetComponent<Button>().interactable = false;
        }
    }

    //全てのアイテムをONにする
    public void Oninteract()
    {
        for (i = 0; i < _listitem.Count; i++)
        {
            _listitem[i].transform.Find("SaveONButton").GetComponent<Button>().interactable = true;
        }
    }

}
