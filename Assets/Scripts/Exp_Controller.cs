using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

//
//アイテムの更新・経験値の増減処理を行うコントローラー　＋　調合の処理を担うメソッド
//

public class Exp_Controller : SingletonMonoBehaviour<Exp_Controller>
{
    private GameObject canvas;

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private HikariMakeStartPanel Hikarimake_StartPanel;

    private TimeController time_controller;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private string _ex_text;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject shopitemlistController_obj;
    private ShopItemListController shopitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;

    private GameObject recipimemoController_obj;
    private GameObject recipiMemoButton;
    private GameObject memoResult_obj;

    private GameObject recipi_archivement_obj;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject GirlEat_scene_obj;
    private GirlEat_Main girlEat_scene;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    private Girl1_status girl1_status;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private KaeruCoin_Controller kaeruCoin_Controller;

    private ExpTable exp_table;
    private SoundController sc;

    private PlayerItemList pitemlist;

    private Compound_Keisan compound_keisan;

    private GameObject compoBG_A;
    private GameObject HikariMakeImage;

    private GameObject yes_no_panel;

    private GameObject _model_obj;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;
    private GameObject character_move;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private ItemRoastDataBase databaseRoast;
    private ItemShopDataBase shop_database;
    private SlotNameDataBase slotnamedatabase;
    private SlotChangeName slotchangename;

    private GameObject extremePanel_obj;
    private ExtremePanel extremePanel;

    private GameObject hukidashiitem;
    private Text _hukidashitext;
  
    private int toggle_type1;
    private int dongri_type;
    private int kettei_item1;

    private int result_item;
    public int result_ID; //SetImageなどからも読む可能性あり
    private int new_item;

    private int result_kosu;
    public int set_kaisu; //オリジナル調合時、その組み合わせで何個作るか、の個数

    private int _getexp;

    //プレイヤーが選んだ個数の組み合わせセット。Compound_Check.csから書き出す。
    public List<int> result_kosuset = new List<int>();

    //成功確率(外部スクリプトから保存・読み込み用）
    public float _temp_srate_1;
    public float _temp_srate_2;
    public float _temp_srate_3;
    //**ここまで**//

    public float _success_rate;
    public float _rate_final;
    public int _success_judge_flag; // 0=必ず成功, 1=計算する, 2=必ず失敗
    private int dice; //確率計算用サイコロ

    //private string[] _slot = new string[10];
    private string[] _slotHyouji1 = new string[10]; //日本語に変換後の表記を格納する。スロット覧用

    private int i, sw, count;

    public int Comp_method_bunki; //トッピング調合メソッドの分岐フラグ

    public bool compound_success; //調合の成功か失敗

    public bool NewRecipiFlag;  //新しいレシピをひらめいたフラグをON
    public int NewRecipi_compoID;   //そのときの、調合DBのID
    private int _releaseID;
    public int DoubleItemCreated; //一つの調合から、2つ以上のアイテムが生まれる場合のフラグ

    public bool NewRecipiflag_check;
    public bool extreme_on; //エクストリーム調合から、新しいアイテムを閃いた場合は、ON

    public bool result_ok; // 調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。現在、未使用。
    public bool recipiresult_ok; //レシピ調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool topping_result_ok; //トッピング調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool roast_result_ok; //「焼く」完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。

    public bool ResultSuccess; //成功か失敗かのフラグ

    public bool girleat_ok; // 女の子にアイテムをあげた時の完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    //public bool shop_buy_ok; //購入完了のフラグ。これがたっていたら、購入の処理を行い、フラグをオフに。
    public bool qbox_ok; // クエスト納品時の完了フラグ。

    //アニメーション用
    private int compo_anim_status;
    private bool compo_anim_on;
    private bool compo_anim_end;
    private float timeOut;

    private GameObject Compo_Magic_effect_Prefab1;
    private GameObject Compo_Magic_effect_Prefab2;
    private GameObject Compo_Magic_effect_Prefab3;
    private GameObject Compo_Magic_effect_Prefab4;
    private GameObject Compo_Magic_effect_Prefab5;
    private GameObject Compo_Magic_effect_Prefab6;
    private GameObject Compo_Magic_effect_Prefab_kiraexplode;   
    private List<GameObject> _listEffect = new List<GameObject>();

    private GameObject HikariMake_effect_Particle_KiraExplode;

    private GameObject ResultBGimage;
    private GameObject BlackImage;

    private GameObject CompleteImage;

    //エクストリームパネルで制作したお菓子の一時保存用パラメータ。シーン移動しても、削除されない。
    public int _temp_extreme_id;
    public int _temp_extreme_itemtype;
    public bool _temp_extremeSetting;
    public float _temp_extreme_money;
    public float _temp_moneydeg;
    public bool _temp_life_anim_on;
    public float _temp_Starthp;

    private string renkin_hyouji;

    private int _id1, _id2;



    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //「焼く」データベースの取得
        databaseRoast = ItemRoastDataBase.Instance.GetComponent<ItemRoastDataBase>();

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //合成計算オブジェクトの取得
        compound_keisan = Compound_Keisan.Instance.GetComponent<Compound_Keisan>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //レベルアップチェック用オブジェクトの取得
        exp_table = ExpTable.Instance.GetComponent<ExpTable>();

        //音声ファイルの取得。SCを使わずに鳴らす場合はこっち。

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //エフェクトプレファブの取得
        Compo_Magic_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_Compo1");
        Compo_Magic_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Particle_Compo2");
        Compo_Magic_effect_Prefab3 = (GameObject)Resources.Load("Prefabs/Particle_Compo3");
        Compo_Magic_effect_Prefab4 = (GameObject)Resources.Load("Prefabs/Particle_Compo4");
        Compo_Magic_effect_Prefab5 = (GameObject)Resources.Load("Prefabs/Particle_Compo5");
        Compo_Magic_effect_Prefab6 = (GameObject)Resources.Load("Prefabs/Particle_Compo6");
        Compo_Magic_effect_Prefab_kiraexplode = (GameObject)Resources.Load("Prefabs/Particle_KiraExplode");
        

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                InitObject();

                break;

            default:
                break;
        }

        result_ok = false;
        recipiresult_ok = false;
        girleat_ok = false;

        ResultSuccess = false;

        //blend_flag = false;

        compound_success = false;
        NewRecipiFlag = false;

        extreme_on = false;
        NewRecipiflag_check = false;

        i = 0;
        sw = 0;
        new_item = 0;

        Comp_method_bunki = 0;
        DoubleItemCreated = 0;

        compo_anim_status = 0;
        compo_anim_on = false;
        compo_anim_end = false;        

        _temp_extreme_id = 9999;
        _temp_extremeSetting = false;
    }

    private void InitObject()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        Hikarimake_StartPanel = canvas.transform.Find("Compound_BGPanel_A/HikariMakeStartPanel").GetComponent<HikariMakeStartPanel>();

        //時間管理オブジェクトの取得
        time_controller = canvas.transform.Find("MainUIPanel/Comp/TimePanel").GetComponent<TimeController>();

        extremePanel_obj = canvas.transform.Find("MainUIPanel/ExtremePanel").gameObject;
        extremePanel = extremePanel_obj.GetComponent<ExtremePanel>();

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        //確率パネルの取得
        kakuritsuPanel_obj = canvas.transform.Find("Compound_BGPanel_A/FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

        //レシピメモボタンを取得
        recipimemoController_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiMemo_ScrollView").gameObject;
        recipiMemoButton = canvas.transform.Find("Compound_BGPanel_A/RecipiMemoButton").gameObject;
        memoResult_obj = canvas.transform.Find("Compound_BGPanel_A/Memo_Result").gameObject;

        //レシピ達成率を取得
        recipi_archivement_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiCompoImage/Panel").gameObject;       

        //黒半透明パネルの取得
        BlackImage = canvas.transform.Find("Compound_BGPanel_A/BlackImage").gameObject; //魔法エフェクト用の半透明で幕

        //完成時パネルの取得
        CompleteImage = canvas.transform.Find("Compound_BGPanel_A/CompletePanel").gameObject; //調合成功時のイメージパネル

        //コンポBGパネルの取得
        compoBG_A = canvas.transform.Find("Compound_BGPanel_A").gameObject;
        ResultBGimage = compoBG_A.transform.Find("ResultBG").gameObject;
        ResultBGimage.SetActive(false);

        //YesNoパネル
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;

        //スロット名前変換用オブジェクトの取得
        slotchangename = GameObject.FindWithTag("SlotChangeName").gameObject.GetComponent<SlotChangeName>();

        //Live2Dモデルの取得
        _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;
        live2d_animator = _model_obj.GetComponent<Animator>();

        character_move = GameObject.FindWithTag("CharacterRoot").transform.Find("CharacterMove").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {

            //シーン読み込みのたびに、一度リセットされてしまうので、アップデートで一度初期化
            if (compound_Main_obj == null)
            {
                InitObject();
            }

            //調合中ウェイト+アニメ
            if (compo_anim_on == true)
            {
                compound_Main.compo_ON = true;
                GameMgr.check_GirlLoveSubEvent_flag = false;

                //アニメスタート
                Compo_Magic_Animation();
            }

        }
    }        


    //
    //オリジナル調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void ResultOK()
    {
        InitObject();

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();       

        //リザルトアイテムを代入
        result_item = pitemlistController.result_item;

        //コンポ調合データベースのIDを代入
        result_ID = pitemlistController.result_compID;

        Comp_method_bunki = 0;
        

        //ウェイトアニメーション開始
        pitemlistController_obj.SetActive(false);
        compo_anim_on = true; //アニメスタート

        StartCoroutine("Original_Compo_anim");

    }

    IEnumerator Original_Compo_anim()
    {
        while (compo_anim_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        compo_anim_on = false;
        compo_anim_end = false;
        compo_anim_status = 0;

        //調合判定
        //チュートリアルモードのときは100%成功
        if (GameMgr.tutorial_ON == true)
        {
            compound_success = true;
        }
        else
        {
            CompoundSuccess_judge();
        }

        //調合成功
        if (compound_success == true)
        {
            //個数の決定
            if (set_kaisu == 0) //例外処理。ロードしたてのときは、回数0のまま、仕上げから新規作成される際、0になることがある。
            {
                set_kaisu = 1;
            }
            result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * set_kaisu; //セット数は、updowncounterの数値がセットされる。スクリプトは、Compound_Checkから参照。


            //調合処理
            Compo_1();



            //チュートリアルのときは、一時的にOFF
            if (GameMgr.tutorial_ON == true)
            {
                if (GameMgr.tutorial_Num == 60)
                {
                    card_view.SetinteractiveOFF();
                }

                if (GameMgr.tutorial_Num == 250)
                {
                    card_view.SetinteractiveOFF();
                }

            }

            //完成アイテムの、レシピフラグをONにする。
            _releaseID = databaseCompo.SearchCompoIDString(databaseCompo.compoitems[result_ID].release_recipi);
            databaseCompo.compoitems[_releaseID].cmpitem_flag = 1;
            Debug.Log("レシピ上書きFlag=1: " + databaseCompo.compoitems[_releaseID].cmpitem_Name);

            //作ったことがあるかどうかをチェック
            if (databaseCompo.compoitems[result_ID].comp_count == 0)
            {
                //作った回数をカウント
                databaseCompo.compoitems[result_ID].comp_count++;               

                //レシピ達成率を更新
                databaseCompo.RecipiCount_database();

                _getexp = databaseCompo.compoitems[result_ID].renkin_Bexp;
                PlayerStatus.player_renkin_exp += _getexp; //調合完成のアイテムに対応した経験値がもらえる。

                //NewRecipiFlag = true;
                NewRecipi_compoID = result_ID;

                _ex_text = "<color=#FF78B4>" + "新しいレシピ" + "</color>" + "を閃いた！" + "\n";
            }
            //すでに作っていたことがある場合
            else if (databaseCompo.compoitems[result_ID].comp_count > 0)
            {
                //作った回数をカウント
                databaseCompo.compoitems[result_ID].comp_count++;
                
                _getexp = databaseCompo.compoitems[result_ID].renkin_Bexp / databaseCompo.compoitems[result_ID].comp_count;
                PlayerStatus.player_renkin_exp += _getexp; //すでに作ったことがある場合、取得量は少なくなる

                _ex_text = "";
            }

            //はじめて、アイテムを制作した場合は、フラグをONに。
            if (!GameMgr.tutorial_ON)
            {
                if (PlayerStatus.First_recipi_on != true)
                {
                    PlayerStatus.First_recipi_on = true;
                }
            }


            if (extreme_on) //トッピング調合から、新規作成に分岐した場合
            {
                if (!PlayerStatus.First_extreme_on) //仕上げを一度もやったことがなかったら、フラグをON
                {
                    PlayerStatus.First_extreme_on = true;
                }
            }

            //テキストの表示
            if (DoubleItemCreated == 0)
            {
                renkin_exp_up();
            }
            else //2つ同時にできたとき
            {
                renkin_exp_up2();
            }

            //完成エフェクト
            ResultEffect_OK();
            CompleteAnim(); //完成背景切り替え＋アニメ

            //調合完了＋成功
            GameMgr.ResultComplete_flag = 1;
            ResultSuccess = true;
            
        }
        else //調合失敗
        {

            _text.text = "調合失敗..！ ";

            //ゴミアイテムを検索。
            i = 0;

            while (i < database.items.Count)
            {

                if (database.items[i].itemName == "gomi_1")
                {
                    result_item = i; //プレイヤーコントローラーの変数に、アイテムIDを代入
                    break;
                }
                ++i;
            }

            Debug.Log(database.items[result_item].itemNameHyouji + "調合失敗..！");

            result_kosu = 1;
            NewRecipiFlag = false;

            //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
            pitemlist.addPlayerItem(database.items[result_item].itemName, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList(1);
            extremePanel.deleteExtreme_Item();

            card_view.ResultCard_DrawView(0, result_item);

            //テキストの表示
            Failed_Text();

            //完成エフェクト
            ResultEffect_NG();

            //調合完了＋失敗
            GameMgr.ResultComplete_flag = 2;
            ResultSuccess = false;
        }

        result_ok = false;

        //日数の経過
        time_controller.SetMinuteToHour(databaseCompo.compoitems[result_ID].cost_Time);
        time_controller.Weather_Change(0.0f);
        time_controller.HikarimakeTimeCheck(databaseCompo.compoitems[result_ID].cost_Time); //ヒカリのお菓子作り時間を計算

        _ex_text = "";

        //メインテキストも更新
        compound_Main.StartMessage();        

        //経験値の増減後、レベルアップしたかどうかをチェック
        //exp_table.Check_LevelUp();

        //時間の項目リセット
        time_controller.ResetTimeFlag();

        //作った直後のサブイベントをチェック
        GameMgr.check_CompoAfter_flag = true;
    }

    void Compo_1()
    {
        //①調合処理 オリジナルアイテムとして生成
        compound_keisan.Topping_Compound_Method(0);
                
        if (DoubleItemCreated == 0)
        {
            result_item = pitemlist.player_check_itemlist.Count - 1;
            GameMgr.Okashi_makeID = pitemlist.player_check_itemlist[result_item].itemID;
            renkin_hyouji = pitemlist.player_check_itemlist[result_item].itemNameHyouji;

            //制作したアイテムが材料、もしくはポーション類ならエクストリームパネルに設定はしない。
            if (pitemlist.player_check_itemlist[result_item].itemType.ToString() == "Mat" || 
                pitemlist.player_check_itemlist[result_item].itemType.ToString() == "Potion")
            {
            }
            else
            {
                //お菓子パネルに、作ったやつをセット。
                extremePanel.SetExtremeItem(result_item, 2);

                //仕上げ回数をリセット
                PlayerStatus.player_extreme_kaisu = PlayerStatus.player_extreme_kaisu_Max;

            }

            new_item = result_item;

            card_view.ResultCard_DrawView(3, new_item);
        }
        else //例外処理。卵白と卵黄が同時にできる場合など。
        {
            if (databaseCompo.compoitems[result_ID].cmpitem_Name == "egg_split")
            {
                _id1 = database.SearchItemIDString("egg_white");
                _id2 = database.SearchItemIDString("egg_yellow");
                card_view.ResultCard_DrawView2(0, _id1, _id2);
            }
            if (databaseCompo.compoitems[result_ID].cmpitem_Name == "egg_split_premiaum")
            {
                _id1 = database.SearchItemIDString("egg_premiaum_white");
                _id2 = database.SearchItemIDString("egg_premiaum_yellow");
                card_view.ResultCard_DrawView2(0, _id1, _id2);
            }
        }
    }

    //使ってない
    void Compo_2()
    {
        //②店売りアイテムとして生成し、実際にアイテムを追加。
        
        //リザルトアイテムを代入
        result_item = recipilistController.result_recipiitem;

        compound_keisan.Delete_playerItemList(1);
        renkin_hyouji = database.items[result_item].itemNameHyouji;
        pitemlist.addPlayerItem(database.items[result_item].itemName, result_kosu);

        if (database.items[result_item].itemType.ToString() == "Mat" || database.items[result_item].itemType.ToString() == "Potion")
        {
        }
        else
        {
            //右側パネルに、作ったやつを表示する。
            extremePanel.SetExtremeItem(result_item, 0);

        }

        card_view.RecipiResultCard_DrawView(0, result_item);
    }



    //
    //レシピ調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void Recipi_ResultOK()
    {
        InitObject();

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        recipilistController_obj = GameObject.FindWithTag("RecipiList_ScrollView");
        recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

       
        //コンポ調合データベースのIDを代入
        result_ID = recipilistController.result_recipicompID;

        //個数の決定
        result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * recipilistController.final_select_kosu;

        extreme_on = false; //念のため、エクストリーム調合で新規作成される場合のフラグもオフにしておく。

        Comp_method_bunki = 2;

        //ウェイトアニメーション開始
        recipilistController_obj.SetActive(false);
        compo_anim_on = true; //アニメスタート

        StartCoroutine("Recipi_Compo_anim");

    }

    IEnumerator Recipi_Compo_anim()
    {
        while (compo_anim_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        compo_anim_on = false;
        compo_anim_end = false;
        compo_anim_status = 0;

        //調合判定
        //チュートリアルモードのときは100%成功
        if (GameMgr.tutorial_ON == true)
        {
            compound_success = true;
        }
        else
        {
            CompoundSuccess_judge();
        }

        //調合成功
        if (compound_success == true)
        {

            //調合処理
            Compo_1();
            
            //チュートリアルのときは、一時的にOFF
            if (GameMgr.tutorial_ON == true)
            {
                if (GameMgr.tutorial_Num == 170)
                {
                    card_view.SetinteractiveOFF();
                }
            }

            //作ったことがあるかどうかをチェック
            if (databaseCompo.compoitems[result_ID].comp_count == 0)
            {
                //作った回数をカウント
                databaseCompo.compoitems[result_ID].comp_count++;

                _getexp = databaseCompo.compoitems[result_ID].renkin_Bexp;
                PlayerStatus.player_renkin_exp += _getexp; //調合完成のアイテムに対応した経験値がもらえる。
            }
            else if (databaseCompo.compoitems[result_ID].comp_count > 0)
            {
                //作った回数をカウント
                databaseCompo.compoitems[result_ID].comp_count++;

                _getexp = databaseCompo.compoitems[result_ID].renkin_Bexp / databaseCompo.compoitems[result_ID].comp_count;
                PlayerStatus.player_renkin_exp += _getexp; //レシピ調合の場合も同様。すでに作ったことがある場合、取得量は少なくなる

            }

            //はじめて、アイテムを制作した場合は、フラグをONに。
            if (!GameMgr.tutorial_ON)
            {
                if (PlayerStatus.First_recipi_on != true)
                {
                    PlayerStatus.First_recipi_on = true;
                }
            }

            //テキストの表示            
            renkin_default_exp_up();

            //完成エフェクト
            ResultEffect_OK();
            CompleteAnim(); //完成背景切り替え＋アニメ

            //調合完了＋成功
            GameMgr.ResultComplete_flag = 1;
            ResultSuccess = true;
            
        }
        else //失敗した
        {

            _text.text = "調合失敗..！ ";

            //ゴミアイテムを検索。
            i = 0;

            while (i < database.items.Count)
            {

                if (database.items[i].itemName == "gomi_1")
                {
                    result_item = i; //プレイヤーコントローラーの変数に、アイテムIDを代入
                    break;
                }
                ++i;
            }

            Debug.Log(database.items[result_item].itemNameHyouji + "調合失敗..！");

            result_kosu = 1;

            //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
            pitemlist.addPlayerItem(database.items[result_item].itemName, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList(1);
            extremePanel.deleteExtreme_Item();

            card_view.ResultCard_DrawView(0, result_item);

            //テキストの表示
            Failed_Text();

            //完成エフェクト
            ResultEffect_NG();

            //調合完了＋失敗
            GameMgr.ResultComplete_flag = 2;
            ResultSuccess = false;
        }

        recipiresult_ok = false;

        //メインテキストも更新
        compound_Main.StartMessage();

        //日数の経過
        time_controller.SetMinuteToHour(databaseCompo.compoitems[result_ID].cost_Time);
        time_controller.Weather_Change(0.0f);
        time_controller.HikarimakeTimeCheck(databaseCompo.compoitems[result_ID].cost_Time); //ヒカリのお菓子作り時間を計算

        //経験値の増減後、レベルアップしたかどうかをチェック
        //exp_table.Check_LevelUp();

        //時間の項目リセット
        time_controller.ResetTimeFlag();

        //作った直後のサブイベントをチェック
        GameMgr.check_CompoAfter_flag = true;
    }



    //
    //トッピング調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void Topping_Result_OK()
    {
        InitObject();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        Comp_method_bunki = 3; //トッピング調合の処理。

        //ウェイトアニメーション開始
        pitemlistController_obj.SetActive(false);
        compo_anim_on = true; //アニメスタート

        StartCoroutine("Topping_Compo_anim");

    }

    IEnumerator Topping_Compo_anim()
    {
        while (compo_anim_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        compo_anim_on = false;
        compo_anim_end = false;
        compo_anim_status = 0;

        //調合判定。エクストリーム調合の確率も含め計算する。

        //チュートリアルモードのときは100%成功
        /*if (GameMgr.tutorial_ON == true)
        {
            compound_success = true;
        }
        else
        {
            CompoundSuccess_judge();
        }*/

        //エクストリーム調合は必ず成功　トッピングなので。
        compound_success = true;


        if (compound_success == true)
        {
            Debug.Log("Topping_Compound_Sucess!!");

            //トッピング調合完了なので、リザルトアイテムのパラメータ計算と、プレイヤーアイテムリストに追加処理
            compound_keisan.Topping_Compound_Method(0);

            new_item = pitemlist.player_check_itemlist.Count - 1;

            //仕上げ回数を減らす
            PlayerStatus.player_extreme_kaisu--;

            //新しいアイテムを閃くかチェック
            if (NewRecipiflag_check != true)
            {
                _getexp = compound_keisan._getExp;
                PlayerStatus.player_renkin_exp += _getexp; //エクストリーム経験値。確率が低いものほど、経験値が大きくなる。

                _ex_text = "";
            }

            //新しいアイテムを閃くと、そのレシピを解禁
            else
            {
                //調合データベースのIDを代入
                result_ID = pitemlistController.result_compID;


                //閃き済みかどうかをチェック。
                if (databaseCompo.compoitems[result_ID].cmpitem_flag != 1)
                {
                    //作った回数をカウント
                    databaseCompo.compoitems[result_ID].comp_count++;

                    //完成アイテムの、レシピフラグをONにする。
                    _releaseID = databaseCompo.SearchCompoIDString(databaseCompo.compoitems[result_ID].release_recipi);
                    databaseCompo.compoitems[_releaseID].cmpitem_flag = 1;

                    //レシピ達成率を更新
                    databaseCompo.RecipiCount_database();

                    _getexp = databaseCompo.compoitems[result_ID].renkin_Bexp;
                    PlayerStatus.player_renkin_exp += _getexp; //エクストリームで新しく閃いた場合の経験値

                    //NewRecipiFlag = true;
                    NewRecipi_compoID = result_ID;

                    _ex_text = "<color=#FF78B4>" + "新しいレシピ" + "</color>" + "を閃いた！" + "\n";

                    //はじめて、アイテムを制作した場合は、フラグをONに。
                    if (PlayerStatus.First_recipi_on != true)
                    {
                        PlayerStatus.First_recipi_on = true;
                    }
                }

                //すでに閃いていた場合
                else
                {
                    //作った回数をカウント
                    databaseCompo.compoitems[result_ID].comp_count++;

                    _getexp = databaseCompo.compoitems[result_ID].renkin_Bexp / databaseCompo.compoitems[result_ID].comp_count;
                    PlayerStatus.player_renkin_exp += _getexp; //エクストリームで新しく閃いた場合の経験値

                    _ex_text = "";
                }


                extreme_on = false;
            }


            //カードで表示
            card_view.ResultCard_DrawView(3, new_item);

            //チュートリアルのときは、一時的にOFF
            if (GameMgr.tutorial_ON == true)
            {
                if (GameMgr.tutorial_Num == 250)
                {
                    card_view.SetinteractiveOFF();
                }
            }

            if (!PlayerStatus.First_extreme_on) //仕上げを一度もやったことがなかったら、フラグをON
            {
                PlayerStatus.First_extreme_on = true;
            }

            result_kosu = 1;

            //右側パネルに、作ったやつを表示する。
            extremePanel.SetExtremeItem(0, 2);
            
            //テキストの表示
            renkin_exp_up();

            //完成エフェクト
            ResultEffect_OK();
            CompleteAnim(); //完成背景切り替え＋アニメ

            //調合完了＋成功
            GameMgr.ResultComplete_flag = 1;
            ResultSuccess = true;
            
        }
        else //失敗の場合
        {
            _text.text = "調合失敗..！ ";

            //ゴミアイテムを検索。
            i = 0;

            while (i < database.items.Count)
            {

                if (database.items[i].itemName == "gomi_1")
                {
                    result_item = i; //プレイヤーコントローラーの変数に、アイテムIDを代入
                    break;
                }
                ++i;
            }

            Debug.Log(database.items[result_item].itemNameHyouji + "調合失敗..！");

            result_kosu = 1;
            NewRecipiFlag = false;

            //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
            pitemlist.addPlayerItem(database.items[result_item].itemName, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList(1);
            extremePanel.deleteExtreme_Item();

            card_view.ResultCard_DrawView(0, result_item);

            //テキストの表示
            Failed_Text();

            //完成エフェクト
            ResultEffect_NG();

            //調合完了＋失敗
            GameMgr.ResultComplete_flag = 2;
            ResultSuccess = false;
        }

        topping_result_ok = false;

        //テキスト表示後、閃いた～をリセットしておく
        _ex_text = "";

        //メインテキストも更新
        compound_Main.StartMessage();

        //日数の経過
        time_controller.SetMinuteToHour(3);
        time_controller.Weather_Change(0.0f);
        time_controller.HikarimakeTimeCheck(3); //ヒカリのお菓子作り時間を計算

        //経験値の増減後、レベルアップしたかどうかをチェック
        //exp_table.Check_LevelUp();

        //時間の項目リセット
        time_controller.ResetTimeFlag();

        //作った直後のサブイベントをチェック
        GameMgr.check_CompoAfter_flag = true;
    }


    //
    //ヒカリが作る完了の場合
    //
    public void HikariMakeOK()
    {
        InitObject();

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        HikariMakeImage = compoBG_A.transform.Find("HikariMakeImage").gameObject;
        HikariMake_effect_Particle_KiraExplode = HikariMakeImage.transform.Find("Particle_KiraExplode").gameObject;

        //一個以上作ってた場合、先にそれは入手する。
        if (GameMgr.hikari_make_okashiKosu >= 1)
        {
            Hikarimake_StartPanel.GetYosokuItem();
        }

        //リザルトアイテムを代入
        result_item = pitemlistController.result_item;

        //コンポ調合データベースのIDを代入
        result_ID = pitemlistController.result_compID;

        Comp_method_bunki = 0;

        
        //調合の予測処理 予測用オリジナルアイテムを生成　パラメータも予測して表示する（アイテム消費はしない）
        compound_keisan.Topping_Compound_Method(2);

        //使用する材料と個数を別に保存する。すぐにはアイテムの使用はせず、時間イベントに合わせて、処理を行う。
        GameMgr.hikari_kettei_item[0] = pitemlistController.kettei_item1;
        GameMgr.hikari_kettei_item[1] = pitemlistController.kettei_item2;
        GameMgr.hikari_kettei_item[2] = pitemlistController.kettei_item3;
        GameMgr.hikari_kettei_toggleType[0] = pitemlistController._toggle_type1;
        GameMgr.hikari_kettei_toggleType[1] = pitemlistController._toggle_type2;
        GameMgr.hikari_kettei_toggleType[2] = pitemlistController._toggle_type3;
        GameMgr.hikari_kettei_kosu[0] = pitemlistController.final_kettei_kosu1;
        GameMgr.hikari_kettei_kosu[1] = pitemlistController.final_kettei_kosu2;
        GameMgr.hikari_kettei_kosu[2] = pitemlistController.final_kettei_kosu3;        
        GameMgr.hikari_make_okashiFlag = true; //現在制作中。このフラグをもとに、キャンセルできるようにもする。
        GameMgr.hikari_make_okashiID = result_item;
        GameMgr.hikari_make_okashi_compID = result_ID;
        GameMgr.hikari_make_success_rate = _success_rate;

        GameMgr.hikari_make_success_count = 0;
        GameMgr.hikari_make_failed_count = 0;

        GameMgr.hikari_make_okashiTimeCounter = 0;
        GameMgr.hikari_make_doubleItemCreated = DoubleItemCreated;
        GameMgr.hikari_make_okashiKosu = 0;

        GameMgr.hikari_makeokashi_startcounter = 9999; //作り始めのフラグ。10なら、10秒たったら、TimeControllerでfalseにする。カウンターは今使ってない。
        GameMgr.hikari_makeokashi_startflag = true;

        GameMgr.hikari_make_Allfailed = false;

        //制作にかかる時間(compoDBのコストタイムで兄ちゃんと共通）とタイマーをセット cost_time=1が5分なので、*5。さらに、ヒカリの場合時間が2倍かかり、お菓子LVによってさらに遅くなる。
        GameMgr.hikari_make_okashiTimeCost = (int)(databaseCompo.compoitems[result_ID].cost_Time * 5f * 2 * GameMgr.hikari_make_okashiTime_costbuf);
        //Debug.Log("GameMgr.hikari_make_okashiTime_costbuf: " + GameMgr.hikari_make_okashiTime_costbuf);

        if (GameMgr.hikari_kettei_toggleType[0] == 0)
        {
            GameMgr.hikari_kettei_itemName[0] = database.items[GameMgr.hikari_kettei_item[0]].itemName;
        }
        else if (GameMgr.hikari_kettei_toggleType[0] == 1)
        {
            GameMgr.hikari_kettei_itemName[0] = pitemlist.player_originalitemlist[GameMgr.hikari_kettei_item[0]].itemName;
        }
        else if (GameMgr.hikari_kettei_toggleType[0] == 2)
        {
            GameMgr.hikari_kettei_itemName[0] = pitemlist.player_extremepanel_itemlist[GameMgr.hikari_kettei_item[0]].itemName;
        }

        if (GameMgr.hikari_kettei_toggleType[1] == 0)
        {
            GameMgr.hikari_kettei_itemName[1] = database.items[GameMgr.hikari_kettei_item[1]].itemName;
        }
        else if (GameMgr.hikari_kettei_toggleType[1] == 1)
        {
            GameMgr.hikari_kettei_itemName[1] = pitemlist.player_originalitemlist[GameMgr.hikari_kettei_item[1]].itemName;
        }
        else if (GameMgr.hikari_kettei_toggleType[1] == 2)
        {
            GameMgr.hikari_kettei_itemName[1] = pitemlist.player_extremepanel_itemlist[GameMgr.hikari_kettei_item[1]].itemName;
        }

        if (GameMgr.hikari_kettei_item[2] != 9999)
        {
            if (GameMgr.hikari_kettei_toggleType[2] == 0)
            {
                GameMgr.hikari_kettei_itemName[2] = database.items[GameMgr.hikari_kettei_item[2]].itemName;
            }
            else if (GameMgr.hikari_kettei_toggleType[2] == 1)
            {
                GameMgr.hikari_kettei_itemName[2] = pitemlist.player_originalitemlist[GameMgr.hikari_kettei_item[2]].itemName;
            }
            else if (GameMgr.hikari_kettei_toggleType[2] == 2)
            {
                GameMgr.hikari_kettei_itemName[2] = pitemlist.player_extremepanel_itemlist[GameMgr.hikari_kettei_item[2]].itemName;
            }
        }

        //
        result_item = pitemlist.player_yosokuitemlist.Count - 1;
        //GameMgr.Okashi_makeID = pitemlist.player_originalitemlist[result_item].itemID;
        renkin_hyouji = pitemlist.player_yosokuitemlist[result_item].itemNameHyouji;

        new_item = result_item;


        //リザルトアニメーション開始
        pitemlistController_obj.SetActive(false);
        //compo_anim_on = true; //アニメスタート
        //StartCoroutine("Original_Compo_anim");

        //メモは全てオフに
        recipiMemoButton.SetActive(false);
        recipimemoController_obj.SetActive(false);
        memoResult_obj.SetActive(false);
        recipi_archivement_obj.SetActive(false);
        kakuritsuPanel_obj.SetActive(false);
        yes_no_panel.SetActive(false);

        //半透明の黒をON
        BlackImage.GetComponent<CanvasGroup>().alpha = 1;

        //カード表示
        NewRecipiFlag = false; //ヒカリが作る場合、強制的に新レシピ解放フラグをOFFに。
        card_view.ResultCardYosoku_DrawView(1, new_item);

        //エフェクトON
        HikariMake_effect_Particle_KiraExplode.SetActive(true);

        //音も鳴らす
        sc.PlaySe(25);

        //テキストの表示
        _text.text = 
            pitemlist.player_yosokuitemlist[new_item].itemNameHyouji +
            " を登録しました！" + "\n" + "にいちゃん！　ヒカリ、がんばって作る～！";
    }




    //
    //「ショップで購入」の場合、ここでアイテムリストの更新行う。
    //
    public void Shop_ResultOK()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        shopitemlistController_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
        shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = canvas.transform.Find("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        kettei_item1 = shopitemlistController.shop_kettei_item1;
        //Debug.Log("決定したアイテムID: " + kettei_item1 + " リスト番号: " + shopitemlistController.shop_count);

        toggle_type1 = shopitemlistController.shop_itemType;
        dongri_type = shopitemlistController.shop_dongriType;

        result_kosu = shopitemlistController.shop_final_itemkosu_1; //買った個数

        //通常アイテム
        if (toggle_type1 == 0)
        {
            //プレイヤーアイテムリストに追加。
            pitemlist.addPlayerItem(database.items[kettei_item1].itemName, result_kosu);
        }
        else if (toggle_type1 == 1) //shop_itemType=1のものは、レシピのこと。買うことで、あとでアトリエに戻ったときに、本を読み、いくつかのレシピを解禁するフラグになる。
        {
            //イベントプレイヤーアイテムリストに追加。レシピのフラグなど。
            pitemlist.add_eventPlayerItem(kettei_item1, result_kosu);
            pitemlist.eventitemlist_Sansho(); //デバッグ用

        }
        else if (toggle_type1 == 5) //shop_itemType = 5 のものは、エメラルどんぐりで買うアイテムで特殊。レアアイテム・コスチュームなどのアイテム系。
        {
            //イベントプレイヤーアイテムリストに追加。レシピのフラグなど。
            pitemlist.add_EmeraldPlayerItem(kettei_item1, result_kosu);
            pitemlist.emeralditemlist_Sansho(); //デバッグ用。コメントアウトしても大丈夫。
            
        }
        else if (toggle_type1 == 2 || toggle_type1 == 6) //2は機材。shop_itemType = 6 は、装備品や飾りなどの特殊アイテム。買うことでパラメータを上昇させたり、フラグをたてる。
        {
            //かごの大きさ計算やバフの計算は「Buf_Power_keisan.cs」
            //プレイヤーアイテムリストに追加。
            pitemlist.addPlayerItem(database.items[kettei_item1].itemName, result_kosu);
            
        }
        else //トッピングなど
        {
            //プレイヤーアイテムリストに追加。
            pitemlist.addPlayerItem(database.items[kettei_item1].itemName, result_kosu);
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                //所持金をへらす
                moneyStatus_Controller.UseMoney(shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_costprice * result_kosu);

                //ショップの在庫をへらす。
                shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko -= result_kosu;
                break;

            case "Farm":

                //所持金をへらす
                moneyStatus_Controller.UseMoney(shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_costprice * result_kosu);

                //ショップの在庫をへらす。
                shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko -= result_kosu;
                break;

            case "Emerald_Shop":

                kaeruCoin_Controller = canvas.transform.Find("KaeruCoin_Panel").GetComponent<KaeruCoin_Controller>();

                switch(dongri_type)
                {
                    case 0:

                        //エメラルどんぐり数をへらす
                        kaeruCoin_Controller.UseCoin(shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_costprice * result_kosu);
                        break;

                    case 1:

                        //サファイアどんぐり数をへらす
                        kaeruCoin_Controller.UseCoin2(shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_costprice * result_kosu);
                        break;
                }
                
                //ショップの在庫をへらす。
                shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_itemzaiko -= result_kosu;
                break;

            default:
                break;
        }

        //効果音
        sc.PlaySe(32);

        _text.text = "購入しました！他にはなにか買う？";

        shopitemlistController.ReDraw(); //リスト描画の更新

    }

    //
    //「ショップで売る」の場合、ここでアイテムリストの更新行う。
    //
    public void Shop_SellOK()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = GameObject.FindWithTag("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        kettei_item1 = pitemlistController.kettei_item1;
        toggle_type1 = pitemlistController._toggle_type1;
        result_kosu = pitemlistController.final_kettei_kosu1; //買った個数

        //通常アイテム
        if (toggle_type1 == 0)
        {
            //プレイヤーアイテムリストに追加。
            pitemlist.deletePlayerItem(database.items[kettei_item1].itemName, result_kosu);
        }
        else if (toggle_type1 == 1) //プレイヤーオリジナルアイテム
        {
            //イベントプレイヤーアイテムリストに追加。レシピのフラグなど。
            pitemlist.deleteOriginalItem(kettei_item1, result_kosu);

        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                //所持金を増やす
                moneyStatus_Controller.GetMoney(database.items[pitemlistController.final_kettei_item1].sell_price * pitemlistController.final_kettei_kosu1);

                break;

            default:
                break;
        }

        //効果音
        sc.PlaySe(32);

        _text.text = "売りました！他にはなにか売る？";

        pitemlistController.reset_and_DrawView(); //リスト描画の更新

    }

    //
    //調合中のアニメ
    //
    void Compo_Magic_Animation()
    {

        switch (compo_anim_status)
        {
            case 0: //初期化 状態１

                //メモは全てオフに
                recipiMemoButton.SetActive(false);
                recipimemoController_obj.SetActive(false);
                memoResult_obj.SetActive(false);
                recipi_archivement_obj.SetActive(false);
                kakuritsuPanel_obj.SetActive(false);
                yes_no_panel.SetActive(false);

                //半透明の黒をON
                Sequence sequence = DOTween.Sequence();

                //まず、初期値。
                BlackImage.GetComponent<CanvasGroup>().alpha = 0;
                sequence.Append(BlackImage.GetComponent<CanvasGroup>().DOFade(1, 0.5f));

                //ヒカリちゃんを右にずらす
                character_move.transform.DOMoveX(8f, 1f)
                    .SetEase(Ease.InOutSine);

                //エフェクト生成＋アニメ開始
                _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab1));

                //音を鳴らす
                sc.PlaySe(10);

                //一時的にお菓子のHP減少をストップ
                extremePanel.LifeAnimeOnFalse();

                //背景変更
                //compoBG_A.SetActive(true);

                timeOut = 2.0f;
                compo_anim_status = 1;

                _text.text = "ガシャ .";
                break;

            case 1: // 状態2

                if (timeOut <= 0.0)
                {
                    timeOut = 1.0f;
                    compo_anim_status = 2;

                    _text.text = "ガシャ　ガシャ . .";
                }
                break;

            case 2:

                if (timeOut <= 0.0)
                {
                    //新しいアイテムができるときは、さらに追加のキラキラ演出
                    if (NewRecipiFlag)
                    {
                        timeOut = 1.0f;
                        compo_anim_status = 3;

                        //エフェクト生成＋アニメ開始
                        _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab6));

                        //色を変更 難しい..。
                        //ParticleSystem.MainModule main = _listEffect[0].gameObject.GetComponent<ParticleSystem>().main;
                        //main.startColor = Color.red;
                        //_listEffect[0].gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
                        //_listEffect[0].gameObject.GetComponents<Renderer>().material.SetColor("_TintColor", Color.red);

                        //音を鳴らす
                        sc.PlaySe(89);

                        _text.text = "ガシャ　ガシャ . . . ";
                    }
                    else
                    {
                        timeOut = 0.5f;
                        compo_anim_status = 5;
                    }


                }
                break;

            case 3:

                if (timeOut <= 0.0)
                {
                    timeOut = 2.0f;
                    compo_anim_status = 4;

                    _text.text = "ガシャ　ガシャ　ガシャ . . . . ";
                }
                break;

            case 4:

                if (timeOut <= 0.0)
                {
                    timeOut = 0.5f;
                    compo_anim_status = 5;
                }
                break;

            case 5: //アニメ終了。判定する


                //カードビューのカードアニメもストップ
                card_view.cardcompo_anim_on = false;
                card_view.DeleteCard_DrawView();

                //音を止める
                sc.StopSe();

                //チュートリアルモードがONのときの処理。ボタンを押した、フラグをたてる。
                if (GameMgr.tutorial_ON == true)
                {
                    if (GameMgr.tutorial_Num == 55)
                    {
                        GameMgr.tutorial_Progress = true;
                        GameMgr.tutorial_Num = 60;
                    }
                    if (GameMgr.tutorial_Num == 165)
                    {
                        GameMgr.tutorial_Progress = true;
                        GameMgr.tutorial_Num = 170;
                    }
                    if (GameMgr.tutorial_Num == 245)
                    {
                        GameMgr.tutorial_Progress = true;
                        GameMgr.tutorial_Num = 250;
                    }
                }

                //Debug.Log("アニメ終了");
                compo_anim_end = true;
               
                break;

            default:
                break;
        }        

        //時間減少
        timeOut -= Time.deltaTime;
    }

    void CompleteAnim()
    {
        //完成～出来たー！という変化をつけるために、背景を変え、ヒカリちゃんを登場させる。
        CompleteImage.SetActive(true);

        //アニメーション
        //まず、初期値。
        Sequence sequence2 = DOTween.Sequence();
        CompleteImage.transform.Find("Image").GetComponent<CanvasGroup>().alpha = 0;
        sequence2.Append(CompleteImage.transform.Find("Image").DOScale(new Vector3(0.3f, 0.3f, 1.0f), 0.0f)
            ); //

        //移動のアニメ
        sequence2.Append(CompleteImage.transform.Find("Image").DOScale(new Vector3(0.5f, 0.5f, 1.0f), 0.75f)
        //.SetEase(Ease.OutElastic)); //はねる動き
        .SetEase(Ease.OutExpo)); //スケール小からフェードイン
        sequence2.Join(CompleteImage.transform.Find("Image").GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    public void EffectListClear()
    {
        //初期化
        for (i = 0; i < _listEffect.Count; i++)
        {
            Destroy(_listEffect[i]);
        }
        _listEffect.Clear();
    }

    void ResultEffect_OK()
    {
        //初期化しておく
        for (i = 0; i < _listEffect.Count; i++)
        {
            Destroy(_listEffect[i]);
        }
        _listEffect.Clear();


        //リザルト時のエフェクト生成＋アニメ開始
        _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab2));
        _listEffect[0].GetComponent<Canvas>().worldCamera = Camera.main;
        _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab3));
        _listEffect[1].GetComponent<Canvas>().worldCamera = Camera.main;
        _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab5));
        _listEffect[2].GetComponent<Canvas>().worldCamera = Camera.main;
        _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab_kiraexplode));
        _listEffect[3].GetComponent<Canvas>().worldCamera = Camera.main;

        //音を鳴らす
        sc.PlaySe(4);        
        sc.PlaySe(27);
        sc.PlaySe(78);

        //ResultBGimage.SetActive(true);
    }

    void ResultEffect_NG()
    {
        //初期化しておく
        for (i = 0; i < _listEffect.Count; i++)
        {
            Destroy(_listEffect[i]);
        }
        _listEffect.Clear();


        //リザルト時のエフェクト生成＋アニメ開始
        _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab4));
        _listEffect[0].GetComponent<Canvas>().worldCamera = Camera.main;

        //音を鳴らす
        sc.PlaySe(20);
    }

    void renkin_default_exp_up()
    {
        if (_getexp != 0)
        {
            _text.text = "やったね！ " +
                renkin_hyouji +
                " が" + result_kosu + "個 できたよ！";
                //+ "\n" + _ex_text +"パティシエ経験値 " + _getexp + "上がった！";
        }
        else
        {
            _text.text = "やったね！ " +
                renkin_hyouji +
                " が" + result_kosu + "個 できたよ！";
                //+ "\n" + _ex_text +"パティシエ経験値は上がらなかった。";
        }

        Debug.Log(renkin_hyouji + "が出来ました！");

    }

    void renkin_exp_up()
    {

        if (_getexp != 0)
        {
            _text.text = "やったね！ " +
            //GameMgr.ColorYellow + pitemlist.player_originalitemlist[new_item].item_SlotName + "</color>" 
            pitemlist.player_check_itemlist[new_item].itemNameHyouji +
            " が" + result_kosu + "個 できたよ！";
            //+ "\n" + _ex_text + "パティシエ経験値 " + _getexp + "上がった！";
        }
        else
        {
            _text.text = "やったね！ " +
            //GameMgr.ColorYellow + pitemlist.player_originalitemlist[new_item].item_SlotName + "</color>" + 
            pitemlist.player_check_itemlist[new_item].itemNameHyouji +
            " が" + result_kosu + "個 できたよ！";
            //+ "\n" + _ex_text +"パティシエ経験値は上がらなかった。"; ;
        }

        Debug.Log(pitemlist.player_check_itemlist[new_item].itemNameHyouji + "が出来ました！");

    }

    void renkin_exp_up2()
    {

        if (_getexp != 0)
        {
            _text.text = "やったね！ " +
            database.items[_id1].itemNameHyouji + " と " + database.items[_id2].itemNameHyouji +
            " が" + result_kosu + "個 できたよ！";
            //+ "\n" + _ex_text +"パティシエ経験値 " + _getexp + "上がった！";
        }
        else
        {
            _text.text = "やったね！ " +
            database.items[_id1].itemNameHyouji + " と " + database.items[_id2].itemNameHyouji +
            " が" + result_kosu + "個 できたよ！";
            //+ "\n" + _ex_text +"パティシエ経験値は上がらなかった。";
        }

        Debug.Log(database.items[_id1].itemNameHyouji + " " + database.items[_id2].itemNameHyouji + "が出来ました！");

    }

    void Failed_Text()
    {
        _text.text = "調合失敗..！ ";

        Debug.Log("失敗..！");
    }

    public void GirlLikeText(int _getlove_exp, int _getmoney, int total_score)
    {

        text_area = canvas.transform.Find("MessageWindowMain").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        if(_getmoney > 0)
        {
            if (_getlove_exp != 0)
            {
                _text.text = "ハートが " + GameMgr.ColorYellow + _getlove_exp + "</color>" + "アップした！" + "\n" +
                    "ぱぱから仕送り " + GameMgr.ColorYellow + _getmoney + GameMgr.MoneyCurrency + "</color>" + " 送られてきた！";
            }
            else
            {
                _text.text = "ハートはかわらなかった。" + "\n" +
                    "ぱぱから仕送り " + GameMgr.ColorYellow + _getmoney + GameMgr.MoneyCurrency + "</color>" + " 送られてきた！";
            }
        }
        else
        {
            if (_getlove_exp != 0)
            {
                _text.text = "ハートが " + GameMgr.ColorYellow + _getlove_exp + "</color>" + "アップした！";
            }
            else
            {
                _text.text = "ハートはかわらなかった。";
            }
        }
    }

    public void GirlDisLikeText(int _getlove_exp)
    {

        text_area = canvas.transform.Find("MessageWindowMain").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        if (_getlove_exp != 0)
        {
            _text.text = "ハートが" + Mathf.Abs(_getlove_exp) + "下がった..。";
        }
        else
        {
            _text.text = "ハートはかわらなかった。";
        }
    }

    public void GirlNotEatText()
    {

        text_area = canvas.transform.Find("MessageWindowMain").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        _text.text = "今はこのお菓子じゃない気分のようだ。";
    }


    //確率判定処理
    void CompoundSuccess_judge()
    {
        switch (_success_judge_flag)
        {
            case 0: //必ず成功

                compound_success = true;
                break;

            case 1: //判定処理を行う

                
                _rate_final = _success_rate;

                //サイコロをふる
                dice = Random.Range(1, 100); //1~100までのサイコロをふる。

                Debug.Log("最終成功確率: " + _rate_final + " " + "ダイスの目: " + dice);

                if (dice <= (int)_rate_final) //出た目が、成功率より下なら成功
                {
                    compound_success = true;
                }
                else //失敗
                {
                    compound_success = false;
                }

                break;

            case 2: //必ず失敗

                compound_success = false;
                break;
        }


    }

}
