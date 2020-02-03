using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExtremePanel : MonoBehaviour {

    public int extreme_itemID;
    public int extreme_itemtype;

    private int extreme_kaisu;

    private GameObject image_effect;
    private GameObject canvas;
    private GameObject black_panel_A;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private Exp_Controller exp_Controller;

    private OkashiParamKeisanMethod Okashi_keisan;

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    private Texture2D texture2d;

    private Image item_Icon;
    private Text item_Name;
    private Text extreme_Param;

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

        //黒半透明パネルの取得
        black_panel_A = canvas.transform.Find("Black_Panel_A").gameObject;

        //確率パネルの取得
        kakuritsuPanel_obj = canvas.transform.Find("KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = GameObject.FindWithTag("Canvas").transform.Find("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        item_Icon = this.transform.Find("Extreme_Image").gameObject.GetComponent<Image>(); //画像アイコン

        extreme_Param = this.transform.Find("ExtremeKaisu/Text/ExtremeKaisuParam").gameObject.GetComponent<Text>(); //エクストリーム残り回数
        extreme_Param.text = "-";

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
        }
        else if (extreme_itemtype == 1) //オリジナルアイテムの場合
        {
            texture2d = pitemlist.player_originalitemlist[extreme_itemID].itemIcon;
            extreme_kaisu = pitemlist.player_originalitemlist[extreme_itemID].ExtremeKaisu;
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
        sell_Button.SetActive(true);

        //あげるボタンを表示
        present_Button.SetActive(true);
    }

    public void OnClick_ExtremeButton()
    {
        if (extreme_itemID != 9999 && extreme_kaisu <= 0)
        { //残り回数０のときは、もうエクストリーム出来ない
            _text.text = "エクストリーム回数が限度に達したよ！" + "\n" + "これ以上はもう調合出来ないよ。" + "\n" + "あげるか、売るかしてね。";
        }
        else
        {
            extreme_Button.interactable = false;
            recipi_Button.interactable = false;

            //Compound_Mainのトッピング時と処理が同じ
            pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

            if (extreme_itemID != 9999)
            {
                
                compoundselect_onoff_obj = canvas.transform.Find("CompoundSelect_ScrollView").gameObject;

                //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
                yes = pitemlistController_obj.transform.Find("Yes").gameObject;
                yes_text = yes.GetComponentInChildren<Text>();
                no = pitemlistController_obj.transform.Find("No").gameObject;
                no_text = no.GetComponentInChildren<Text>();              

                compoundselect_onoff_obj.SetActive(false);

                compound_Main.compound_status = 4; //調合シーンに入っています、というフラグ
                compound_Main.compound_select = 2; //トッピング調合を選択

                kakuritsuPanel_obj.SetActive(true);
                pitemlistController_obj.SetActive(true); //プレイヤーアイテム画面を表示。
                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                black_panel_A.SetActive(true);

                yes.SetActive(false);
                no.SetActive(true);

                _text.text = "エクストリーム調合をするよ！ 一個目の材料を選んでね。";


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
            else //何もまだ作られていない場合は、新規調合
            {
                card_view.All_DeleteCard();

                _text.text = "新しくお菓子を作るよ！" + "\n" + "好きな材料を" + "<color=#0000FF>" + "２つ" + "</color>" + "か" + "<color=#0000FF>" + "３つ" + "</color>" + "選んでね。";
                compound_Main.compound_status = 3;

                pitemlistController.extremepanel_on = false;
            }
        }
    }


    public void OnClick_RecipiBook()
    {
        extreme_Button.interactable = false;
        recipi_Button.interactable = false;
        sell_Button.GetComponent<Button>().interactable = false;
        present_Button.GetComponent<Button>().interactable = false;

        card_view.All_DeleteCard();

        _text.text = "レシピから作るよ。何を作る？";
        compound_Main.compound_status = 1;
    }

    public void OnClick_PresentButton()
    {
        extreme_Button.interactable = false;
        recipi_Button.interactable = false;
        sell_Button.GetComponent<Button>().interactable = false;
        present_Button.GetComponent<Button>().interactable = false;

        card_view.All_DeleteCard();

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
        sell_Button.GetComponent<Button>().interactable = false;
        present_Button.GetComponent<Button>().interactable = false;

        card_view.All_DeleteCard();

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

        deleteExtreme_Item();

        compound_Main.compound_status = 0;
        compound_Main.compound_select = 0;
    }


    public void deleteExtreme_Item() //削除
    {
        card_view.DeleteCard_DrawView();

        item_Icon.color = new Color(1, 1, 1, 0);

        extreme_Param.text = "-";
        CullentOkashi_money.text = "-";
        
        extreme_itemID = 9999;
        _hpslider.value = 0;
        Starthp = 0;

        sell_Button.SetActive(false);
        present_Button.SetActive(false);

        Life_anim_on = false;
        image_effect.SetActive(false);
    }


    public void extremeButtonInteractOn()
    {
        extreme_Button.interactable = true;
        recipi_Button.interactable = true;
        sell_Button.GetComponent<Button>().interactable = true;
        present_Button.GetComponent<Button>().interactable = true;
    }


    public void SetDegOkashiLife( int Life )
    {
        _hpslider.value = Life;
        Starthp = Life; //floatで計算し、valueに反映する。

        exp_Controller._temp_Starthp = Starthp;

        timeOut = 1.0f;
        Life_anim_on = true;
        exp_Controller._temp_life_anim_on = true;

        //お菓子の現在の価値もセット
        Okashi_moneyparam = Okashi_keisan.Sell_Okashi(extreme_itemID, extreme_itemtype);
        CullentOkashi_money.text = Okashi_moneyparam.ToString();

        //減少量も決定
        _moneydeg = Okashi_moneyparam / Starthp;
        //Debug.Log("_moneydeg: " + _moneydeg);
    }

    //お菓子のHP減少を一時的にストップ。調合アニメ開始時などで使用
    public void LifeAnimeOnFalse()
    {
        Life_anim_on = false;
    }

    public void LifeAnimeOnTrue()
    {
        Life_anim_on = true;
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
