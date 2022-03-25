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
    private GameObject particle_effect;
    private GameObject canvas;
    private GameObject compoBG_A;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private Exp_Controller exp_Controller;

    private OkashiParamKeisanMethod Okashi_keisan;

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    private SlotChangeName slotchangename;

    private Sprite texture2d;

    private Image item_Icon;
    private Text extreme_Param;

    private Text extreme_itemName;

    private Slider _hpslider; //お菓子のHPバーを取得
    public bool Life_anim_on;
    private float Starthp;
    private float _deg;
    private float _moneydeg;

    private Text CullentOkashi_money;
    private float Okashi_moneyparam; //計算時はfloatだが、最終的にintになおして計算する
    public int Okashi_moneypram_int;

    private Button extreme_Button;

    private GameObject card_view_obj;
    private CardView card_view;

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
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //コンポBGパネルの取得
        compoBG_A = canvas.transform.Find("Compound_BGPanel_A").gameObject;

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = GameObject.FindWithTag("Canvas").transform.Find("MainUIPanel/Comp/MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        //エフェクト取得
        Extreme_Failed_effect_Prefab = (GameObject)Resources.Load("Prefabs/Particle_Extreme_Failed");

        //スロットをもとに、正式名称を計算するメソッド
        slotchangename = GameObject.FindWithTag("SlotChangeName").gameObject.GetComponent<SlotChangeName>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        item_Icon = this.transform.Find("Comp/Extreme_Image").gameObject.GetComponent<Image>(); //画像アイコン

        extreme_Param = this.transform.Find("Comp/ExtremeKaisu/Text/ExtremeKaisuParam").gameObject.GetComponent<Text>(); //エクストリーム残り回数
        extreme_Param.text = "-";
        
        extreme_itemName = this.transform.Find("Comp/ExtremeItemText").gameObject.GetComponent<Text>();
        extreme_itemName.text = "";

        //ボタンの取得
        extreme_Button = this.transform.Find("Comp/ExtremeButton").gameObject.GetComponent<Button>(); //エクストリームボタン

        image_effect = this.transform.Find("Comp/Extreme_Image_effect").gameObject;
        image_effect.SetActive(false);

        particle_effect = this.transform.Find("Comp/Particle_Kirakira_3").gameObject;
        particle_effect.SetActive(false);

        item_Icon.color = new Color(1, 1, 1, 0);

        extreme_itemID = 9999;

        //お菓子HPバーの取得
        _hpslider = this.transform.Find("Comp/Life_Bar").GetComponent<Slider>();
        _hpslider.value = 0;

        //現在のお菓子の価格テキストを取得
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
            SetInitParamExtreme();            

            myscene_loaded = false;
        }

        if(pitemlistController_obj == null)
        { //Compound_Mainのトッピング時と処理が同じ
            pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();
        }
        
        /*
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

                if (Starthp <= 0) //0になったら、お菓子が壊れる。
                {
                    //所持品削除
                    switch (extreme_itemtype)
                    {
                        case 0: //プレイヤーアイテムリストから選択している。

                            pitemlist.deletePlayerItem(database.items[extreme_itemID].itemName, 1);
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
        }*/
	}

    public void SetInitParamExtreme()
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
        }
        else
        {
            deleteExtreme_Item();
        }
    }

    public void SetExtremeItem( int item_id, int itemtype )
    {

        extreme_itemID = item_id;
        extreme_itemtype = itemtype;

        //ゲーム保存用　下のtempと意味合い的には一緒。一緒にしてOKだが、面倒なのでとりあえず分け。
        GameMgr.sys_extreme_itemID = extreme_itemID;
        GameMgr.sys_extreme_itemType = extreme_itemtype;

        //シーン移動用に保存
        exp_Controller._temp_extreme_id = extreme_itemID; //オリジナルリストの最後の番号のこと
        exp_Controller._temp_extreme_itemtype = extreme_itemtype;
        exp_Controller._temp_extremeSetting = true; //セットされていますよ～、ということ。この状態で、オリジナルリストの最後の番号のアイテムが削除されたら、パネルのデータも削除する。

        if(itemtype == 0)
        {
            //お菓子のHPをセット
            SetDegOkashiLife(database.items[extreme_itemID].itemHP);
        } else
        {
            //お菓子のHPをセット
            SetDegOkashiLife(pitemlist.player_originalitemlist[extreme_itemID].itemHP);
        }

        Extreme_Hyouji();
    }

    void Extreme_Hyouji()
    {
        if (extreme_itemtype == 0) //デフォルトアイテムの場合
        {
            texture2d = database.items[extreme_itemID].itemIcon_sprite;
            //extreme_kaisu = database.items[extreme_itemID].ExtremeKaisu;
            extreme_itemName.text = database.items[extreme_itemID].itemNameHyouji;
        }
        else if (extreme_itemtype == 1) //オリジナルアイテムの場合
        {
            texture2d = pitemlist.player_originalitemlist[extreme_itemID].itemIcon_sprite;
            //extreme_kaisu = pitemlist.player_originalitemlist[extreme_itemID].ExtremeKaisu;

            //スロット名+アイテム名の表示
            extreme_itemName.text = GameMgr.ColorYellow + pitemlist.player_originalitemlist[extreme_itemID].item_SlotName + "</color>" + pitemlist.player_originalitemlist[extreme_itemID].itemNameHyouji;
            //extreme_itemName.text = pitemlist.player_originalitemlist[extreme_itemID].itemNameHyouji;
        }


        item_Icon.color = new Color(1, 1, 1, 1);
        item_Icon.sprite = texture2d;
                                   

        //エクストリーム残り回数の表示更新。
        extreme_kaisu = PlayerStatus.player_extreme_kaisu;
        extreme_Param.text = extreme_kaisu.ToString();

        //エフェクトの表示
        image_effect.SetActive(true);
        particle_effect.SetActive(true);
    }

    public void OnClick_ExtremeButton()
    {

        extreme_Button.interactable = false;

        if (extreme_itemID != 9999)
        {
            GameMgr.compound_status = 6; //調合選択画面に移動 元々4にしてた

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

            _text.text = "何の調合をする？";
            GameMgr.compound_status = 6;

            pitemlistController.extremepanel_on = false;

            //チュートリアルモードがONのときの処理。ボタンを押した、フラグをたてる。
            if (GameMgr.tutorial_ON == true)
            {
                if (GameMgr.tutorial_Num == 10)
                {
                    GameMgr.tutorial_Progress = true;
                    GameMgr.tutorial_Num = 15;
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
        card_view.OKCard_DrawView(pitemlistController.final_base_kettei_kosu);

        itemselect_cancel.update_ListSelect_Flag = 10; //ベースアイテムを選択できないようにする。
        itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理
    }

    public void OnClick_RecipiBook()
    {
        extreme_Button.interactable = false;

        card_view.DeleteCard_DrawView();

        _text.text = "レシピから作るよ。何を作る？";
        GameMgr.compound_status = 1;
    }

    public void OnClick_PresentButton()
    {
        extreme_Button.interactable = false;

        card_view.DeleteCard_DrawView();

        if (extreme_itemID != 9999)
        {
            _text.text = "今、作ったお菓子をあげますか？";
            GameMgr.compound_status = 10;
        }
        else //まだ作ってないときは
        {
            _text.text = "まだお菓子を作っていない。";
        }
    }

    public void OnClick_SellButton()
    {
        extreme_Button.interactable = false;

        card_view.DeleteCard_DrawView();

        if (extreme_itemID != 9999)
        {
            Okashi_moneypram_int = (int)Mathf.Ceil(Okashi_moneyparam);
            _text.text = "作ったお菓子をショップへ卸しますか？" + "\n" + "現在の価格: " + Okashi_moneypram_int.ToString() + "G です。";
            GameMgr.compound_status = 30;
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
            pitemlist.deletePlayerItem(database.items[extreme_itemID].itemName, 1);
        }
        else if (extreme_itemtype == 1) //オリジナルアイテムの場合
        {
            pitemlist.deleteOriginalItem(extreme_itemID, 1);
        }

        //エクストリームパネルからも削除
        deleteExtreme_Item();

        GameMgr.compound_status = 0;
        GameMgr.compound_select = 0;
    }


    public void deleteExtreme_Item() //削除
    {
        card_view.DeleteCard_DrawView();

        item_Icon.color = new Color(1, 1, 1, 0);

        extreme_Param.text = "-";
        extreme_itemName.text = "";
        
        extreme_itemID = 9999;
        _hpslider.value = 0;
        Starthp = 0;

        Life_anim_on = false;
        image_effect.SetActive(false);
        particle_effect.SetActive(false);

        exp_Controller._temp_extreme_id = 9999;
        exp_Controller._temp_extremeSetting = false;

        GameMgr.sys_extreme_itemID = 9999;
        GameMgr.sys_extreme_itemType = 0;
    }


    public void extremeButtonInteractOn()
    {
        extreme_Button.interactable = true;
    }

    public void extremeButtonInteractOFF()
    {
        extreme_Button.interactable = false;
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
