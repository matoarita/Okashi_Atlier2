using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExtremePanel : MonoBehaviour {

    public int extreme_itemID;
    public int extreme_itemtype;

    public int extreme_kaisu;

    private BGM sceneBGM;

    private Girl1_status girl1_status;

    private GameObject image_effect;
    private GameObject canvas;
    private GameObject compoBG_A;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private Exp_Controller exp_Controller;

    private OkashiParamKeisanMethod Okashi_keisan;

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    private SlotChangeName slotchangename;

    private Texture2D texture2d;

    private Image item_Icon;
    private Text item_Name;
    private Text extreme_Param;

    private Text extreme_itemName;

    private string[] _slotHyouji2 = new string[10]; //日本語に変換後の表記を格納する。フルネーム用 

    private Slider _hpslider; //お菓子のHPバーを取得
    public bool Life_anim_on;
    private float Starthp;
    private float _deg;
    private float _moneydeg;

    private Text CullentOkashi_money;
    private float Okashi_moneyparam; //計算時はfloatだが、最終的にintになおして計算する
    public int Okashi_moneypram_int;

    private Button extreme_Button;
    private Button recipi_Button;
    private GameObject sell_Button;
    private GameObject present_Button;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject compoundselect_onoff_obj;

    private GameObject text_area;
    private Text _text;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private bool myscene_loaded;

    private GameObject Extreme_Failed_effect_Prefab;
    private GameObject Extreme_Failed_effect;

    private SoundController sc;

    //時間
    private float timeOut;

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //お菓子パラム計算用メソッドの取得
        Okashi_keisan = GameObject.FindWithTag("OkashiParamKeisanMethod").GetComponent<OkashiParamKeisanMethod>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //windowテキストエリアの取得
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //コンポBGパネルの取得
        compoBG_A = canvas.transform.Find("Compound_BGPanel_A").gameObject;

        //確率パネルの取得
        kakuritsuPanel_obj = canvas.transform.Find("KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = GameObject.FindWithTag("Canvas").transform.Find("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        //エフェクト取得
        Extreme_Failed_effect_Prefab = (GameObject)Resources.Load("Prefabs/Particle_Extreme_Failed");

        //スロットをもとに、正式名称を計算するメソッド
        slotchangename = GameObject.FindWithTag("SlotChangeName").gameObject.GetComponent<SlotChangeName>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        item_Icon = this.transform.Find("Extreme_Image").gameObject.GetComponent<Image>(); //画像アイコン

        extreme_Param = this.transform.Find("ExtremeKaisu/Text/ExtremeKaisuParam").gameObject.GetComponent<Text>(); //エクストリーム残り回数
        extreme_Param.text = "-";
        
        extreme_itemName = this.transform.Find("ExtremeItemText").gameObject.GetComponent<Text>();
        extreme_itemName.text = "";

        //ボタンの取得
        extreme_Button = this.transform.Find("ExtremeButton").gameObject.GetComponent<Button>(); //エクストリームボタン
        recipi_Button = this.transform.Find("RecipiButton").gameObject.GetComponent<Button>(); //レシピボタン
        sell_Button = this.transform.Find("SellButton").gameObject; //売るボタン
        present_Button = this.transform.Find("PresentButton").gameObject; //売るボタン 

        image_effect = this.transform.Find("Extreme_Image_effect").gameObject;
        image_effect.SetActive(false);

        item_Icon.color = new Color(1, 1, 1, 0);

        extreme_itemID = 9999;

        //お菓子HPバーの取得
        _hpslider = this.transform.Find("Life_Bar").GetComponent<Slider>();
        _hpslider.value = 0;

        //現在のお菓子の価格テキストを取得
        CullentOkashi_money = this.transform.Find("SellButton/CullentOkashiMoney/GordParam").gameObject.GetComponent<Text>();
        CullentOkashi_money.text = "-";
        Okashi_moneyparam = 0;

        _deg = 1.0f; //1秒間あたりの減少量

        Life_anim_on = false;

        myscene_loaded = false;
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。
    }
	
	// Update is called once per frame
	void Update () {

        //別シーンから、再度読み込まれたときに、すでにお菓子を作成済みだった場合は、初期化する。
        if (myscene_loaded == true)
        {
            extreme_itemID = exp_Controller._temp_extreme_id; // 空の場合は、9999でリセット
            

            if (extreme_itemID != 9999)
            {
                extreme_itemtype = exp_Controller._temp_extreme_itemtype;
                Starthp = exp_Controller._temp_Starthp;
                Life_anim_on = exp_Controller._temp_life_anim_on;
                Okashi_moneyparam = exp_Controller._temp_extreme_money;
                _moneydeg = exp_Controller._temp_moneydeg;

                Extreme_Hyouji();
                //Debug.Log(exp_Controller._temp_extreme_id + " extreme_itemID");
                //Debug.Log(exp_Controller._temp_extreme_itemtype + " exp_Controller._temp_extreme_itemtype");
                //Debug.Log(exp_Controller._temp_Starthp + " exp_Controller._temp_Starthp");
            }

            myscene_loaded = false;
        }

        if( Life_anim_on == true) //お菓子が完成したら、だんだんとHPが減っていく。０になると、お菓子が壊れる。
        {

            if (timeOut <= 0.0)
            {
                timeOut = 1.0f; //１秒ずつ減少

                Starthp -= _deg; //_degの分ずつ、減少していく。
                exp_Controller._temp_Starthp = Starthp;

                _hpslider.value = Starthp; //それをバーにも反映。
                
                Okashi_moneyparam -= _moneydeg;
                exp_Controller._temp_extreme_money = Okashi_moneyparam;
                Okashi_moneypram_int = (int)Mathf.Ceil(Okashi_moneyparam);                
                CullentOkashi_money.text = Okashi_moneypram_int.ToString();

                if (Starthp <= 0) //0になったら、お菓子が壊れる。
                {
                    //所持品削除
                    switch (extreme_itemtype)
                    {
                        case 0: //プレイヤーアイテムリストから選択している。

                            pitemlist.deletePlayerItem(extreme_itemID, 1);
                            break;

                        case 1: //オリジナルアイテムリストから選択している。

                            pitemlist.deleteOriginalItem(extreme_itemID, 1);
                            break;

                        default:
                            break;
                    }

                    deleteExtreme_Item();

                    Life_anim_on = false;
                    exp_Controller._temp_life_anim_on = false;

                    //エフェクト生成
                    Extreme_Failed_effect = Instantiate(Extreme_Failed_effect_Prefab);
                    Extreme_Failed_effect.GetComponent<Canvas>().worldCamera = Camera.main;

                    //音を鳴らす
                    sc.PlaySe(20);
                }
            }

            //時間減少
            timeOut -= Time.deltaTime;
        }
	}

    public void SetExtremeItem( int item_id, int itemtype )
    {

        extreme_itemID = item_id;
        extreme_itemtype = itemtype;

        //シーン移動用に保存
        exp_Controller._temp_extreme_id = extreme_itemID;
        exp_Controller._temp_extreme_itemtype = extreme_itemtype;

        Extreme_Hyouji();
    }

    void Extreme_Hyouji()
    {
        if (extreme_itemtype == 0) //デフォルトアイテムの場合
        {
            texture2d = database.items[extreme_itemID].itemIcon;
            extreme_kaisu = database.items[extreme_itemID].ExtremeKaisu;
            extreme_itemName.text = database.items[extreme_itemID].itemNameHyouji;
        }
        else if (extreme_itemtype == 1) //オリジナルアイテムの場合
        {
            texture2d = pitemlist.player_originalitemlist[extreme_itemID].itemIcon;
            extreme_kaisu = pitemlist.player_originalitemlist[extreme_itemID].ExtremeKaisu;

            //スロットの正式名称計算
            slotchangename.slotChangeName(extreme_itemtype, extreme_itemID, "yellow");

            _slotHyouji2[0] = slotchangename._slotHyouji[0];
            _slotHyouji2[1] = slotchangename._slotHyouji[1];
            _slotHyouji2[2] = slotchangename._slotHyouji[2];
            _slotHyouji2[3] = slotchangename._slotHyouji[3];
            _slotHyouji2[4] = slotchangename._slotHyouji[4];
            _slotHyouji2[5] = slotchangename._slotHyouji[5];
            _slotHyouji2[6] = slotchangename._slotHyouji[6];
            _slotHyouji2[7] = slotchangename._slotHyouji[7];
            _slotHyouji2[8] = slotchangename._slotHyouji[8];
            _slotHyouji2[9] = slotchangename._slotHyouji[9];

            //スロット名+アイテム名の表示
            extreme_itemName.text = _slotHyouji2[0] + _slotHyouji2[1] + _slotHyouji2[2] + _slotHyouji2[3] + _slotHyouji2[4] + _slotHyouji2[5] + _slotHyouji2[6] + _slotHyouji2[7] + _slotHyouji2[8] + _slotHyouji2[9] + pitemlist.player_originalitemlist[extreme_itemID].itemNameHyouji;
        }


        item_Icon.color = new Color(1, 1, 1, 1);
        item_Icon.sprite = Sprite.Create(texture2d,
                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                   Vector2.zero);

        //エクストリーム残り回数の表示更新。
        extreme_Param.text = extreme_kaisu.ToString();

        //エフェクトの表示
        image_effect.SetActive(true);

        //売るボタンを表示
        //sell_Button.SetActive(true);

        //あげるボタンを表示
        //present_Button.SetActive(true);
    }

    public void OnClick_ExtremeButton()
    {

        extreme_Button.interactable = false;
        recipi_Button.interactable = false;

        //Compound_Mainのトッピング時と処理が同じ
        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        if (extreme_itemID != 9999)
        {
            compound_Main.compound_status = 6; //調合選択画面に移動 元々4にしてた

            //チュートリアルモードがONのときの処理。ボタンを押した、フラグをたてる。
            if (GameMgr.tutorial_ON == true)
            {
                if (GameMgr.tutorial_Num == 200)
                {
                    GameMgr.tutorial_Progress = true;
                    GameMgr.tutorial_Num = 210;
                }

            }

        }
        else //何もまだ作られていない場合は、新規調合
        {
            card_view.DeleteCard_DrawView();

            if (PlayerStatus.First_recipi_on == false)
            {
                _text.text = compound_Main.originai_text;
                compound_Main.compound_status = 3;

                pitemlistController.extremepanel_on = false;
            }
            else
            {
                _text.text = "何の調合をする？";
                compound_Main.compound_status = 6;

                pitemlistController.extremepanel_on = false;
            }

            //チュートリアルモードがONのときの処理。ボタンを押した、フラグをたてる。
            if (GameMgr.tutorial_ON == true)
            {
                if (GameMgr.tutorial_Num == 10)
                {
                    GameMgr.tutorial_Progress = true;
                    GameMgr.tutorial_Num = 20;
                }
                else if (GameMgr.tutorial_Num == 140)
                {
                    GameMgr.tutorial_Progress = true;
                    GameMgr.tutorial_Num = 150;
                }
            }
        }

    }

    //CompoundMainから読みこむ用
    public void extreme_Compo_Setup()
    {
        //以下、エクストリーム用に再度パラメータを設定
        pitemlistController.extremepanel_on = true;

        if (extreme_itemtype == 0) //デフォルトアイテムの場合
        {
            pitemlistController.final_base_kettei_item = database.items[extreme_itemID].itemID;
        }
        else if (extreme_itemtype == 1) //オリジナルアイテムの場合
        {
            pitemlistController.final_base_kettei_item = pitemlist.player_originalitemlist[extreme_itemID].itemID;
        }

        pitemlistController.base_kettei_item = extreme_itemID;
        pitemlistController._base_toggle_type = extreme_itemtype;

        pitemlistController.final_base_kettei_kosu = 1;

        pitemlistController.kettei1_bunki = 10; //トッピング材料から選び始める。
        pitemlistController.reset_and_DrawView_Topping();

        card_view.SelectCard_DrawView(pitemlistController._base_toggle_type, pitemlistController.base_kettei_item);
        card_view.OKCard_DrawView();

        itemselect_cancel.update_ListSelect_Flag = 10; //ベースアイテムを選択できないようにする。
        itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理
    }

    public void OnClick_RecipiBook()
    {
        extreme_Button.interactable = false;
        recipi_Button.interactable = false;
        //sell_Button.GetComponent<Button>().interactable = false;
        //present_Button.GetComponent<Button>().interactable = false;

        card_view.DeleteCard_DrawView();

        _text.text = "レシピから作るよ。何を作る？";
        compound_Main.compound_status = 1;
    }

    public void OnClick_PresentButton()
    {
        extreme_Button.interactable = false;
        recipi_Button.interactable = false;
        //sell_Button.GetComponent<Button>().interactable = false;
        //present_Button.GetComponent<Button>().interactable = false;

        card_view.DeleteCard_DrawView();

        if (extreme_itemID != 9999)
        {
            _text.text = "今、作ったお菓子をあげますか？";
            compound_Main.compound_status = 10;
        }
        else //まだ作ってないときは
        {
            _text.text = "まだお菓子を作っていない。";
        }
    }

    public void OnClick_SellButton()
    {
        extreme_Button.interactable = false;
        recipi_Button.interactable = false;
        //sell_Button.GetComponent<Button>().interactable = false;
        //present_Button.GetComponent<Button>().interactable = false;

        card_view.DeleteCard_DrawView();

        if (extreme_itemID != 9999)
        {
            Okashi_moneypram_int = (int)Mathf.Ceil(Okashi_moneyparam);
            _text.text = "作ったお菓子をショップへ卸しますか？" + "\n" + "現在の価格: " + Okashi_moneypram_int.ToString() + "G です。";
            compound_Main.compound_status = 30;
        }
        else //まだ作ってないときは
        {
            _text.text = "まだお菓子を作っていない。";
        }
    }

    public void Sell_Okashi()
    {
        Okashi_moneypram_int = (int)Mathf.Ceil(Okashi_moneyparam);

        _text.text = "お菓子を売った！" + "\n" + Okashi_moneypram_int.ToString() + "G で売れた！";

        //お金の取得
        moneyStatus_Controller.GetMoney(Okashi_moneypram_int);

        //効果音
        sc.PlaySe(31);

        //持ち物から減らす。
        if (extreme_itemtype == 0) //デフォルトアイテムの場合
        {
            pitemlist.deletePlayerItem(extreme_itemID, 1);
        }
        else if (extreme_itemtype == 1) //オリジナルアイテムの場合
        {
            pitemlist.deleteOriginalItem(extreme_itemID, 1);
        }

        //エクストリームパネルからも削除
        deleteExtreme_Item();

        compound_Main.compound_status = 0;
        compound_Main.compound_select = 0;
    }


    public void deleteExtreme_Item() //削除
    {
        card_view.DeleteCard_DrawView();

        item_Icon.color = new Color(1, 1, 1, 0);

        extreme_Param.text = "-";
        extreme_itemName.text = "";
        CullentOkashi_money.text = "-";
        
        extreme_itemID = 9999;
        _hpslider.value = 0;
        Starthp = 0;

        //sell_Button.SetActive(false);
        //present_Button.SetActive(false);

        Life_anim_on = false;
        image_effect.SetActive(false);

        exp_Controller._temp_extreme_id = 9999;
    }


    public void extremeButtonInteractOn()
    {
        extreme_Button.interactable = true;
        recipi_Button.interactable = true;
        //sell_Button.GetComponent<Button>().interactable = true;
        //present_Button.GetComponent<Button>().interactable = true;
    }

    public void extremeButtonInteractOFF()
    {
        extreme_Button.interactable = false;
        recipi_Button.interactable = false;
        //sell_Button.GetComponent<Button>().interactable = false;
        //present_Button.GetComponent<Button>().interactable = false;
    }


    public void SetDegOkashiLife( int Life )
    {
        _hpslider.value = Life;
        Starthp = Life; //floatで計算し、valueに反映する。

        exp_Controller._temp_Starthp = Starthp;

        timeOut = 1.0f;
        //Life_anim_on = true;
        exp_Controller._temp_life_anim_on = true;

        //お菓子の現在の価値もセット
        Okashi_moneyparam = Okashi_keisan.Sell_Okashi(extreme_itemID, extreme_itemtype);
        CullentOkashi_money.text = Okashi_moneyparam.ToString();

        //減少量も決定
        _moneydeg = Okashi_moneyparam / Starthp;
        exp_Controller._temp_moneydeg = _moneydeg;
        //Debug.Log("_moneydeg: " + _moneydeg);
    }

    //お菓子のHP減少を一時的にストップ。調合アニメ開始時などで使用
    public void LifeAnimeOnFalse()
    {
        Life_anim_on = false;
    }

    public void LifeAnimeOnTrue()
    {
        if (extreme_itemID != 9999)
        {
            Life_anim_on = true;
        }
        else
        {

        }
    }

    //別シーンからこのシーンが読み込まれたときに、読み込む
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {
            //Debug.Log(scene.name + " scene loaded");
            myscene_loaded = true;

        }
    }
}
