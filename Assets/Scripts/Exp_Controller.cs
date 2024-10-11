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
    //private Animator maincam_animator;
    //private int trans; //トランジション用のパラメータ

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private HikariMakeStartPanel Hikarimake_StartPanel;
    private HikariOkashiExpTable hikariOkashiExpTable;

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

    private GameObject GirlEat_scene_obj;
    private GirlEat_Main girlEat_scene;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    private Girl1_status girl1_status;

    private GameObject card_view_obj;
    private CardView card_view;

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
    private bool character_ON;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private ItemRoastDataBase databaseRoast;
    private ItemShopDataBase shop_database;
    private SlotNameDataBase slotnamedatabase;

    private GameObject hukidashiitem;
    private Text _hukidashitext;
  
    private int toggle_type1;
    private int dongri_type;
    private int kettei_item1;
    private int shopbuy_kettei_item1;

    private int result_item;
    public int result_ID; //SetImageなどからも読む可能性あり
    private int new_item;

    private int result_kosu;
    public int set_kaisu; //オリジナル調合時、その組み合わせで何個作るか、の個数

    private int _getexp;
    private int _getexp2;

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

    private int i, count;

    public int Comp_method_bunki; //トッピング調合メソッドの分岐フラグ Compound_Keisanから読み出ししてるので注意

    public bool NewRecipiFlag;  //新しいレシピをひらめいたフラグをON
    public int NewRecipi_compoID;   //そのときの、調合DBのID
    private int _releaseID;
    public int DoubleItemCreated; //一つの調合から、2つ以上のアイテムが生まれる場合のフラグ

    public bool NewRecipiflag_check;
    //public bool extreme_on; //エクストリーム調合から、新しいアイテムを閃いた場合は、ON

    public bool result_ok; // 調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。Exp_Controllerで指定、Compound_Keisanで使用。
    public bool recipiresult_ok; //レシピ調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool topping_result_ok; //トッピング調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool magic_result_ok; //魔法調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool roast_result_ok; //「焼く」完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。

    public bool ResultSuccess; //成功か失敗かのフラグ

    public bool girleat_ok; // 女の子にアイテムをあげた時の完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    //public bool shop_buy_ok; //購入完了のフラグ。これがたっていたら、購入の処理を行い、フラグをオフに。
    public bool qbox_ok; // クエスト納品時の完了フラグ。

    //アニメーション用
    private int compo_anim_status;
    private bool compo_anim_on;
    private bool compo_anim_end;
    private bool magiccompo_anim_on;
    private bool magiccompo_anim_end;
    private float timeOut;

    private GameObject Debug_timeCount_Panel;
    private Text Debug_timeCount_Panel_text;
    private float Debug_timeCount;
    private bool Debug_stopwatch;

    private GameObject Compo_Magic_effect_Prefab1;
    private GameObject Compo_Magic_effect_Prefab2;
    private GameObject Compo_Magic_effect_Prefab3;
    private GameObject Compo_Magic_effect_Prefab4;
    private GameObject Compo_Magic_effect_Prefab5;
    private GameObject Compo_Magic_effect_Prefab6;
    private GameObject Compo_Magic_effect_Prefab_kiraexplode;   
    private List<GameObject> _listEffect = new List<GameObject>();

    private ParticleSystem.MainModule main;
    private ParticleSystem compo1_particle;
    private ParticleSystem compo2_particle;
    private Color p_color1;
    private Color p_color2;

    private GameObject HikariMake_effect_Particle_KiraExplode;

    private GameObject BlackImage;

    private GameObject SpecialwhiteEffect;
    private GameObject SpecialOkashiEffectView;
    private List<GameObject> sp_okashieffect_List = new List<GameObject>();
    private Image SpecialOkashi_ItemImg;
    private Sprite texture2d;

    private GameObject CompleteImage;

    //エクストリームパネルで制作したお菓子の一時保存用パラメータ。シーン移動しても、削除されない。
    //public int _temp_extreme_id;
    //public int _temp_extreme_itemtype;
    //public bool _temp_extremeSetting;

    private string renkin_hyouji;

    private int _id1, _id2;
    private string _a, _b, _c;
    private string _yaki;

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

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

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //お金の増減用コントローラーの取得
        moneyStatus_Controller = MoneyStatus_Controller.Instance.GetComponent<MoneyStatus_Controller>();

        //ヒカリお菓子EXPデータベースの取得
        hikariOkashiExpTable = HikariOkashiExpTable.Instance.GetComponent<HikariOkashiExpTable>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //レベルアップチェック用オブジェクトの取得
        exp_table = ExpTable.Instance.GetComponent<ExpTable>();    

        result_ok = false;
        recipiresult_ok = false;
        magic_result_ok = false;
        girleat_ok = false;

        ResultSuccess = false;

        //blend_flag = false;

        NewRecipiFlag = false;

        GameMgr.Extreme_On = false;
        NewRecipiflag_check = false;

        i = 0;
        new_item = 0;

        Comp_method_bunki = 0;
        DoubleItemCreated = 0;

        Debug_stopwatch = false;

        //_temp_extreme_id = 9999;
        //_temp_extremeSetting = false;
    }

    private void InitObject()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        //maincam_animator = main_cam.GetComponent<Animator>();
        //trans = maincam_animator.GetInteger("trans");              

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

        compo_anim_status = 0;
        compo_anim_on = false;
        compo_anim_end = false;
        magiccompo_anim_on = false;
        magiccompo_anim_end = false;

        //Live2Dモデルの取得
        character_ON = false;
        for (i = 0; i < SceneManager.sceneCount; i++)
        {
            //読み込まれているシーンを取得し、その名前をログに表示
            string sceneName = SceneManager.GetSceneAt(i).name;
            Debug.Log(sceneName);

            GameObject[] rootObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();

            
            foreach (var obj in rootObjects)
            {
                //Debug.LogFormat("RootObject = {0}", obj.name);
                if (obj.name == "CharacterRoot")
                {
                    Debug.Log("character_On: ヒカリちゃん　シーン内に存在する");
                    character_ON = true;
                    _model_obj = GameObject.FindWithTag("CharacterRoot").transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;
                    live2d_animator = _model_obj.GetComponent<Animator>();
                    character_move = GameObject.FindWithTag("CharacterRoot").transform.Find("CharacterMove").gameObject;
                }
                else
                {

                }
            }
        }     
    }

    void CompInitSetting()
    {
        //コンポBGパネルの取得
        compoBG_A = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A").gameObject;

        text_area = compoBG_A.transform.Find("MessageWindowComp").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        //レシピメモボタンを取得
        recipimemoController_obj = compoBG_A.transform.Find("RecipiMemo_ScrollView").gameObject;
        recipiMemoButton = compoBG_A.transform.Find("RecipiMemoButton").gameObject;
        memoResult_obj = compoBG_A.transform.Find("Memo_Result").gameObject;

        //レシピ達成率を取得
        recipi_archivement_obj = compoBG_A.transform.Find("RecipiCompoImage/Panel").gameObject;

        //黒半透明パネルの取得
        BlackImage = compoBG_A.transform.Find("BlackImage").gameObject; //魔法エフェクト用の半透明で幕

        //スペシャル演出用のホワイト
        SpecialwhiteEffect = compoBG_A.transform.Find("SpecialOkashiWhiteEffect").gameObject; //スペシャル演出用のホワイト
        SpecialOkashiEffectView = compoBG_A.transform.Find("SpecialOkashiEffectView").gameObject;
        SpecialOkashi_ItemImg = compoBG_A.transform.Find("SpecialOkashiEffectView/panel01/ItemImg").GetComponent<Image>();

        sp_okashieffect_List.Clear();
        foreach (Transform child in SpecialOkashiEffectView.transform)
        {
            sp_okashieffect_List.Add(child.gameObject);
        }

        //完成時パネルの取得
        CompleteImage = compoBG_A.transform.Find("CompletePanel").gameObject; //調合成功時のイメージパネル 

        Debug_timeCount_Panel = compoBG_A.transform.Find("DebugTimeEnshutuPanel").gameObject; //デバッグ用　時間カウントパネル
        Debug_timeCount_Panel_text = Debug_timeCount_Panel.transform.Find("TimeText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMgr.Scene_LoadedOn_End) //シーン読み込み完了してから動き出す
        {
            //シーン移動時バグるのでUpdateで初期化
            if (canvas == null)
            {
                InitObject();
            }
        }

        if (GameMgr.CompoundSceneStartON)
        {
            //調合中ウェイト+アニメ
            if (compo_anim_on == true)
            {
                GameMgr.check_GirlLoveSubEvent_flag = false;

                //アニメスタート
                Compo_Animation();
            }

            //魔法調合中ウェイト+アニメ
            if (magiccompo_anim_on == true)
            {
                GameMgr.check_GirlLoveSubEvent_flag = false;

                //アニメスタート
                MagicCompo_Animation();
            }
        }
    }        


    //
    //オリジナル調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void ResultOK()
    {
        InitObject();
        CompInitSetting();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();       

        //リザルトアイテムを代入
        result_item = GameMgr.Final_result_itemID1;

        //コンポ調合データベースのIDを代入
        result_ID = GameMgr.Final_result_compID;

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
            GameMgr.Result_compound_success = true;
        }
        else
        {
            CompoundSuccess_judge();
        }

        //調合の成功有無にかかわらず、お菓子の経験値をあげる。
        OkashiExpUp();

        //調合成功
        if (GameMgr.Result_compound_success == true)
        {
            //個数の決定
            if (set_kaisu == 0) //例外処理。ロードしたてのときは、回数0のまま、仕上げから新規作成される際、0になることがある。
            {
                set_kaisu = 1;
            }
            //result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * set_kaisu; //セット数set_kaisuは、Compound_Checkから参照。
            result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * 1; //現状セット数使用してないので、１に。

            //調合処理
            Compo_1(0);



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

                //経験値獲得
                GetExpMethod();

                //NewRecipiFlag = true;
                NewRecipi_compoID = result_ID;

                //_ex_text = "<color=#FF78B4>" + "新しいレシピ" + "</color>" + "を閃いた！" + "\n";
                _ex_text = "";
            }
            //すでに作っていたことがある場合
            else if (databaseCompo.compoitems[result_ID].comp_count > 0)
            {
                //作った回数をカウント
                databaseCompo.compoitems[result_ID].comp_count++;

                //経験値獲得
                GetExpMethod();

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


            if (GameMgr.Extreme_On) //トッピング調合から、新規作成に分岐した場合
            {
                if (!PlayerStatus.First_extreme_on) //仕上げを一度もやったことがなかったら、フラグをON
                {
                    PlayerStatus.First_extreme_on = true;
                }
            }

            //ジョブ経験値の増減後、レベルアップしたかどうかをチェック
            //exp_table.SkillCheckPatissierLV();

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
            if (GameMgr.Special_OkashiEnshutsuFlag) //trueのときの特別演出では通常エフェクト表示しない
            {
                EffectListClear();
            }
            else
            {
                ResultEffect_OK(0);
                CompleteAnim(); //完成背景切り替え＋アニメ
            }

            //調合完了＋成功
            GameMgr.ResultComplete_flag = 1;
            ResultSuccess = true;
            
        }
        else //調合失敗
        {
            if (GameMgr.Special_OkashiEnshutsuFlag) //特別演出　失敗したら白をとく
            {
                SpecialwhiteEffect.GetComponent<CanvasGroup>().alpha = 0;
                SpecialwhiteEffect.SetActive(false);
            }
             
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

            result_kosu = 1;
            NewRecipiFlag = false;

            //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
            pitemlist.addPlayerItem(database.items[result_item].itemName, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList(1);
            //deleteExtreme_Item();
            GameMgr.extremepanel_Koushin = true; //エクストリームパネルの表示を更新するON　無いシーンではtrueのまま無視。

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
        if (!GameMgr.Contest_ON)
        {
            time_controller.SetMinuteToHour(databaseCompo.compoitems[result_ID].cost_Time);           
        }
        else
        {
            //コンテストのときは、コンテスト時間を計算
            time_controller.SetMinuteToHourContest(databaseCompo.compoitems[result_ID].cost_Time);
        }
        time_controller.HikarimakeTimeCheck(databaseCompo.compoitems[result_ID].cost_Time); //ヒカリのお菓子作り時間を計算

        _ex_text = "";

        //シーンごとの後処理
        SceneAfterSetting();

        //時間の項目リセット
        time_controller.ResetTimeFlag();

        //温度管理していた場合は、ここでリセット
        GameMgr.tempature_control_ON = false;

        //作った直後のサブイベントをチェック
        GameMgr.check_CompoAfter_flag = true;
    }

    void Compo_1(int _status)
    {
        //①調合処理 オリジナルアイテムとして生成
        compound_keisan.Topping_Compound_Method(0);
                
        if (DoubleItemCreated == 0)
        {
            result_item = pitemlist.player_check_itemlist.Count - 1;
            
            renkin_hyouji = pitemlist.player_check_itemlist[result_item].itemNameHyouji;
            GameMgr.Okashi_makeID = database.SearchItemID(pitemlist.player_check_itemlist[result_item].itemID); //今作ったやつのお菓子ID
            GameMgr.ResultItem_nameHyouji = pitemlist.player_check_itemlist[result_item].itemNameHyouji; //別スクリプトでの使用用

            //制作したアイテムが材料、もしくはポーション類ならエクストリームパネルに設定はしない。
            if (pitemlist.player_check_itemlist[result_item].itemType.ToString() == "Mat" || 
                pitemlist.player_check_itemlist[result_item].itemType.ToString() == "Potion")
            {
                GameMgr.OkashiMake_PanelSetType = 0;
            }
            else
            {
                GameMgr.OkashiMake_PanelSetType = 1;

                //お菓子パネルに、作ったやつをセット。
                GameMgr.extremepanel_Koushin = true; //エクストリームパネルの表示を更新するON　無いシーンではtrueのまま無視。

                //仕上げ回数をリセット
                if (GameMgr.Extreme_On) //トッピングのときに新しいお菓子に変化する場合は回数が減る
                {
                    //仕上げ回数を減らす
                    PlayerStatus.player_extreme_kaisu--;
                }
                else
                {
                    PlayerStatus.player_extreme_kaisu = PlayerStatus.player_extreme_kaisu_Max;
                }

            }

            new_item = result_item;

            //カード表示の演出　レアなおかしをはじめて作るときは、特別な登場 Compound_Checkで事前に演出するかどうか判定
            if (GameMgr.Special_OkashiEnshutsuFlag) //trueのときに特別演出　特別スチルが表示される
            {
                BlackImage.GetComponent<CanvasGroup>().alpha = 0;

                //効果音ならす
                sc.PlaySe(78);
                sc.PlaySe(88);

                SpecialOkashiEffectView.SetActive(true);
                for(i=0; i< sp_okashieffect_List.Count; i++)
                {
                    if (sp_okashieffect_List[i].gameObject.name == GameMgr.Special_OkashiEnshutsuName)
                    {
                        sp_okashieffect_List[i].SetActive(true);
                    }
                    if (sp_okashieffect_List[i].gameObject.name == "CloseButton")
                    {
                        sp_okashieffect_List[i].SetActive(true);
                    }
                }

                texture2d = database.items[GameMgr.Okashi_makeID].itemIcon_sprite;
                SpecialOkashi_ItemImg.sprite = texture2d;

                SpecialwhiteEffect.GetComponent<CanvasGroup>().alpha = 1;
                SpecialwhiteEffect.GetComponent<CanvasGroup>().DOFade(0, 1.0f);
            }
            else
            {
                switch (_status)
                {
                    case 0: //通常調合時のリザルトカード表示
                        card_view.ResultCard_DrawView(3, new_item);
                        break;

                    case 1: //魔法調合時のリザルトカード表示
                        card_view.MagicResultCard_DrawView(3, new_item);
                        break;
                }
            }
            
        }
        else //2個同時に生成するときの処理。卵白と卵黄が同時にできる場合など。
        {
            if (databaseCompo.compoitems[result_ID].cmpitemID_result2 != "Non")
            {
                _id1 = database.SearchItemIDString(databaseCompo.compoitems[result_ID].cmpitemID_result);
                _id2 = database.SearchItemIDString(databaseCompo.compoitems[result_ID].cmpitemID_result2);
                card_view.ResultCard_DrawView2(0, _id1, _id2);
            }
        }
    }

    //使ってない
    /*void Compo_2()
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
            GameMgr.extremepanel_Koushin = true; //エクストリームパネルの表示を更新するON　無いシーンではtrueのまま無視。

        }

        card_view.RecipiResultCard_DrawView(0, result_item);
    }*/



    //
    //レシピ調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void Recipi_ResultOK()
    {
        InitObject();
        CompInitSetting();

        recipilistController_obj = GameObject.FindWithTag("RecipiList_ScrollView");
        recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

       
        //コンポ調合データベースのIDを代入
        result_ID = GameMgr.Final_result_compID;

        //個数の決定
        result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * GameMgr.Final_setCount;

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
            GameMgr.Result_compound_success = true;
        }
        else
        {
            CompoundSuccess_judge();
        }

        //調合の成功有無にかかわらず、お菓子の経験値をあげる。
        OkashiExpUp();

        //調合成功
        if (GameMgr.Result_compound_success == true)
        {

            //調合処理
            Compo_1(0);
            
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

                //経験値獲得
                GetExpMethod();
            }
            else if (databaseCompo.compoitems[result_ID].comp_count > 0)
            {
                //作った回数をカウント
                databaseCompo.compoitems[result_ID].comp_count++;

                //経験値獲得
                GetExpMethod();

            }

            //はじめて、アイテムを制作した場合は、フラグをONに。
            if (!GameMgr.tutorial_ON)
            {
                if (PlayerStatus.First_recipi_on != true)
                {
                    PlayerStatus.First_recipi_on = true;
                }
            }

            //ジョブ経験値の増減後、レベルアップしたかどうかをチェック
            //exp_table.SkillCheckPatissierLV();

            //テキストの表示            
            renkin_default_exp_up();

            //完成エフェクト
            if (GameMgr.Special_OkashiEnshutsuFlag) //trueのときの特別演出では通常エフェクト表示しない
            {
                EffectListClear();
            }
            else
            {
                ResultEffect_OK(0);
                CompleteAnim(); //完成背景切り替え＋アニメ
            }

            //調合完了＋成功
            GameMgr.ResultComplete_flag = 1;
            ResultSuccess = true;
            
        }
        else //失敗した
        {
            if (GameMgr.Special_OkashiEnshutsuFlag) //特別演出　失敗したら白をとく
            {
                SpecialwhiteEffect.GetComponent<CanvasGroup>().alpha = 0;
                SpecialwhiteEffect.SetActive(false);
            }

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

            result_kosu = 1;

            //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
            pitemlist.addPlayerItem(database.items[result_item].itemName, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList(1);
            //deleteExtreme_Item();
            GameMgr.extremepanel_Koushin = true; //エクストリームパネルの表示を更新するON　無いシーンではtrueのまま無視。

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

        //シーンごとの後処理
        SceneAfterSetting();

        //日数の経過
        if (!GameMgr.Contest_ON)
        {
            time_controller.SetMinuteToHour(databaseCompo.compoitems[result_ID].cost_Time);            
        }
        else
        {
            time_controller.SetMinuteToHourContest(databaseCompo.compoitems[result_ID].cost_Time);
        }
        time_controller.HikarimakeTimeCheck(databaseCompo.compoitems[result_ID].cost_Time); //ヒカリのお菓子作り時間を計算

        //時間の項目リセット
        time_controller.ResetTimeFlag();

        //温度管理していた場合は、ここでリセット
        GameMgr.tempature_control_ON = false;

        //作った直後のサブイベントをチェック
        GameMgr.check_CompoAfter_flag = true;
    }



    //
    //トッピング調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void Topping_Result_OK()
    {
        InitObject();
        CompInitSetting();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        

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
            GameMgr.Result_compound_success = true;
        }
        else
        {
            CompoundSuccess_judge();
        }*/

        //エクストリーム調合は必ず成功　トッピングなので。
        GameMgr.Result_compound_success = true;


        if (GameMgr.Result_compound_success == true)
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
                GetExpMethodTopping();               

                _ex_text = "";
            }

            //新しいアイテムを閃くと、そのレシピを解禁
            else
            {
                //調合データベースのIDを代入
                result_ID = GameMgr.Final_result_compID;


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

                    //経験値獲得
                    GetExpMethod();

                    //NewRecipiFlag = true;
                    NewRecipi_compoID = result_ID;

                    //_ex_text = "<color=#FF78B4>" + "新しいレシピ" + "</color>" + "を閃いた！" + "\n";
                    _ex_text = "";

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

                    //経験値獲得
                    GetExpMethod();

                    _ex_text = "";
                }


                GameMgr.Extreme_On = false;
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
            GameMgr.extremepanel_Koushin = true; //エクストリームパネルの表示を更新するON　無いシーンではtrueのまま無視。

            //ジョブ経験値の増減後、レベルアップしたかどうかをチェック
            //exp_table.SkillCheckPatissierLV();

            //テキストの表示
            renkin_exp_up();

            //完成エフェクト
            if (GameMgr.Special_OkashiEnshutsuFlag) //trueのときの特別演出では通常エフェクト表示しない
            {
                EffectListClear();
            }
            else
            {
                ResultEffect_OK(0);
                CompleteAnim(); //完成背景切り替え＋アニメ
            }

            //調合完了＋成功
            GameMgr.ResultComplete_flag = 1;
            ResultSuccess = true;
            
        }
        else //失敗の場合
        {

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

            result_kosu = 1;
            NewRecipiFlag = false;

            //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
            pitemlist.addPlayerItem(database.items[result_item].itemName, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList(1);
            //deleteExtreme_Item();
            GameMgr.extremepanel_Koushin = true; //エクストリームパネルの表示を更新するON　無いシーンではtrueのまま無視。

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

        //シーンごとの後処理
        SceneAfterSetting();

        //日数の経過
        if (!GameMgr.Contest_ON)
        {
            time_controller.SetMinuteToHour(15);
        }
        else
        {
            time_controller.SetMinuteToHourContest(15);
        }
        time_controller.HikarimakeTimeCheck(15); //ヒカリのお菓子作り時間を計算

        //時間の項目リセット
        time_controller.ResetTimeFlag();

        //温度管理していた場合は、ここでリセット
        GameMgr.tempature_control_ON = false;

        //作った直後のサブイベントをチェック
        GameMgr.check_CompoAfter_flag = true;
    }

    //
    //魔法調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void MagicResultOK()
    {
        InitObject();
        CompInitSetting();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        //リザルトアイテムを代入
        result_item = GameMgr.Final_result_itemID1;

        //コンポ調合データベースのIDを代入
        result_ID = GameMgr.Final_result_compID;

        Comp_method_bunki = 20;

        //ウェイトアニメーション開始
        pitemlistController_obj.SetActive(false);
        magiccompo_anim_on = true; //アニメスタート

        StartCoroutine("Magic_Compo_anim");

    }

    IEnumerator Magic_Compo_anim()
    {
        while (magiccompo_anim_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        magiccompo_anim_on = false;
        magiccompo_anim_end = false;
        compo_anim_status = 0;

        //調合判定
        //チュートリアルモードのときは100%成功
        if (GameMgr.tutorial_ON == true)
        {
            GameMgr.Result_compound_success = true;
        }
        else
        {
            if (GameMgr.System_magic_playON) //魔法ミニゲームの成功率を使う
            {
                if (!GameMgr.System_magic_playSucess)
                {
                    GameMgr.Result_compound_success = false;
                }
                else
                {
                    GameMgr.Result_compound_success = true;
                }
            }
            else
            {
                CompoundSuccess_judge();
            }
        }

        //調合の成功有無にかかわらず、お菓子の経験値をあげる。
        OkashiExpUp();

        //調合成功
        if (GameMgr.Result_compound_success == true)
        {
            //個数の決定
            if (set_kaisu == 0) //例外処理。ロードしたてのときは、回数0のまま、仕上げから新規作成される際、0になることがある。
            {
                set_kaisu = 1;
            }
            //result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * set_kaisu; //セット数set_kaisuは、Compound_Checkから参照。
            result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * 1; //現状セット数使用してないので、１に。

            //調合処理
            Compo_1(1);

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

                //経験値獲得
                GetExpMethod();                

                //NewRecipiFlag = true;
                NewRecipi_compoID = result_ID;

                //_ex_text = "<color=#FF78B4>" + "新しいレシピ" + "</color>" + "を閃いた！" + "\n";
                _ex_text = "";
            }
            //すでに作っていたことがある場合
            else if (databaseCompo.compoitems[result_ID].comp_count > 0)
            {
                //作った回数をカウント
                databaseCompo.compoitems[result_ID].comp_count++;

                //経験値獲得
                GetExpMethod();

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


            if (GameMgr.Extreme_On) //トッピング・魔法調合から、新規作成に分岐した場合
            {
                if (!PlayerStatus.First_extreme_on) //仕上げを一度もやったことがなかったら、フラグをON
                {
                    PlayerStatus.First_extreme_on = true;
                }
            }

            //ジョブ経験値の増減後、レベルアップしたかどうかをチェック
            //exp_table.SkillCheckPatissierLV();

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
            if (GameMgr.Special_OkashiEnshutsuFlag) //trueのときの特別演出では通常エフェクト表示しない
            {
                EffectListClear();
            }
            else
            {
                ResultEffect_OK(1);
                CompleteMagicAnim(); //完成背景切り替え＋アニメ
            }

            //調合完了＋成功
            GameMgr.ResultComplete_flag = 1;
            ResultSuccess = true;

        }
        else //調合失敗
        {
            if (GameMgr.Special_OkashiEnshutsuFlag) //特別演出　失敗したら白をとく
            {
                SpecialwhiteEffect.GetComponent<CanvasGroup>().alpha = 0;
                SpecialwhiteEffect.SetActive(false);
            }

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

            result_kosu = 1;
            NewRecipiFlag = false;

            //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
            pitemlist.addPlayerItem(database.items[result_item].itemName, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList(1);
            //deleteExtreme_Item();
            GameMgr.extremepanel_Koushin = true; //エクストリームパネルの表示を更新するON　無いシーンではtrueのまま無視。

            card_view.ResultCard_DrawView(0, result_item);

            //テキストの表示
            Failed_Text();

            //完成エフェクト
            ResultEffect_NG();

            //調合完了＋失敗
            GameMgr.ResultComplete_flag = 2;
            ResultSuccess = false;
        }        

        //完成用の背景を登場　成功・失敗かかわらず共通
        GameMgr.compound_status = 23;

        magic_result_ok = false;

        //日数の経過
        if (!GameMgr.Contest_ON)
        {
            time_controller.SetMinuteToHour(databaseCompo.compoitems[result_ID].cost_Time);           
        }
        else
        {
            time_controller.SetMinuteToHourContest(databaseCompo.compoitems[result_ID].cost_Time);
        }
        time_controller.HikarimakeTimeCheck(databaseCompo.compoitems[result_ID].cost_Time); //ヒカリのお菓子作り時間を計算

        _ex_text = "";

        //シーンごとの後処理
        SceneAfterSetting();        

        //時間の項目リセット
        time_controller.ResetTimeFlag();

        //温度管理していた場合は、ここでリセット
        GameMgr.tempature_control_ON = false;

        //作った直後のサブイベントをチェック
        GameMgr.check_CompoAfter_flag = true;
    }

    //シーンごとの後処理
    void SceneAfterSetting()
    {
        switch (GameMgr.Scene_Category_Num) 
        {
            case 10: // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                //メインテキストも更新
                compound_Main.StartMessage();
                
                break;
        }

        //ヒカリが厨房のセンター位置に戻る準備で右に。
        if (character_ON)
        {
            //character_moveでなく、Live2Dのポスを直接アニメートさせてる。また、Live2Dモデルの位置情報は、スクリプトやTweenとかで直接変えることができない。
            //ここでinterger=99で事前に右にし、integer=100にすると、戻るアニメスタート
            trans_motion = 99;
            live2d_animator.SetInteger("trans_motion", trans_motion);
        }
    }

    //
    //ヒカリが作る完了の場合
    //
    public void HikariMakeOK()
    {
        InitObject();
        CompInitSetting();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        HikariMakeImage = compoBG_A.transform.Find("HikariMakeImage").gameObject;
        HikariMake_effect_Particle_KiraExplode = HikariMakeImage.transform.Find("Particle_KiraExplode").gameObject;

        Hikarimake_StartPanel = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/HikariMakeStartPanel").GetComponent<HikariMakeStartPanel>();

        //一個以上作ってた場合、先にそれは入手する。
        if (GameMgr.hikari_make_okashiKosu >= 1)
        {
            Hikarimake_StartPanel.GetYosokuItem();
        }

        //リザルトアイテムを代入
        //result_item = GameMgr.Final_result_itemID1;

        //コンポ調合データベースのIDを代入
        //result_ID = GameMgr.Final_result_compID;

        Comp_method_bunki = 0;
        //GameMgr.Extreme_On = false; //念のため、エクストリーム調合で新規作成される場合のフラグもオフにしておく。ヒカリは、新しいお菓子をひらめくことは、今の仕様では無い。


        //調合の予測処理 予測用オリジナルアイテムを生成　パラメータも予測して表示する（アイテム消費はしない）
        compound_keisan.Topping_Compound_Method(2);

        //使用する材料と個数を別に保存する。すぐにはアイテムの使用はせず、時間イベントに合わせて、処理を行う。
        GameMgr.hikari_kettei_item[0] = GameMgr.Final_list_itemID1; //店売りかオリジナルアイテムのリスト配列番号
        GameMgr.hikari_kettei_item[1] = GameMgr.Final_list_itemID2;
        GameMgr.hikari_kettei_item[2] = GameMgr.Final_list_itemID3;
        GameMgr.hikari_kettei_toggleType[0] = GameMgr.Final_toggle_Type1;
        GameMgr.hikari_kettei_toggleType[1] = GameMgr.Final_toggle_Type2;
        GameMgr.hikari_kettei_toggleType[2] = GameMgr.Final_toggle_Type3;
        GameMgr.hikari_kettei_kosu[0] = GameMgr.Final_kettei_kosu1;
        GameMgr.hikari_kettei_kosu[1] = GameMgr.Final_kettei_kosu2;
        GameMgr.hikari_kettei_kosu[2] = GameMgr.Final_kettei_kosu3;        
        GameMgr.hikari_make_okashiFlag = true; //現在制作中。このフラグをもとに、キャンセルできるようにもする。
        GameMgr.hikari_make_okashiID = GameMgr.Final_result_itemID1;
        GameMgr.hikari_make_okashi_compID = GameMgr.Final_result_compID;
        GameMgr.hikari_make_success_rate = _success_rate;

        //オリジナルアイテムかお菓子パネルのリストを選択していたら、アイテムの固有IDを保存しておく。
        if(GameMgr.hikari_kettei_toggleType[0] == 1)
        {
            GameMgr.hikari_kettei_originalID[0] = pitemlist.player_originalitemlist[GameMgr.hikari_kettei_item[0]].OriginalitemID;
        }
        else if (GameMgr.hikari_kettei_toggleType[0] == 2)
        {
            GameMgr.hikari_kettei_originalID[0] = pitemlist.player_extremepanel_itemlist[GameMgr.hikari_kettei_item[0]].OriginalitemID;
        }
        if (GameMgr.hikari_kettei_toggleType[1] == 1)
        {
            GameMgr.hikari_kettei_originalID[1] = pitemlist.player_originalitemlist[GameMgr.hikari_kettei_item[1]].OriginalitemID;
        }
        else if (GameMgr.hikari_kettei_toggleType[1] == 2)
        {
            GameMgr.hikari_kettei_originalID[1] = pitemlist.player_extremepanel_itemlist[GameMgr.hikari_kettei_item[1]].OriginalitemID;
        }
        if (GameMgr.hikari_kettei_item[2] != 9999)
        {
            if (GameMgr.hikari_kettei_toggleType[2] == 1)
            {
                GameMgr.hikari_kettei_originalID[2] = pitemlist.player_originalitemlist[GameMgr.hikari_kettei_item[2]].OriginalitemID;
            }
            else if (GameMgr.hikari_kettei_toggleType[2] == 2)
            {
                GameMgr.hikari_kettei_originalID[2] = pitemlist.player_extremepanel_itemlist[GameMgr.hikari_kettei_item[2]].OriginalitemID;
            }
        }
        //Debug.Log("GameMgr.hikari_kettei_originalID[0]; " + GameMgr.hikari_kettei_originalID[0]);
        //Debug.Log("GameMgr.hikari_kettei_originalID[1]; " + GameMgr.hikari_kettei_originalID[1]);
        //Debug.Log("GameMgr.hikari_kettei_originalID[2]; " + GameMgr.hikari_kettei_originalID[2]);

        GameMgr.hikari_make_success_count = 0;
        GameMgr.hikari_make_failed_count = 0;
        
        GameMgr.hikari_make_doubleItemCreated = DoubleItemCreated;
        GameMgr.hikari_make_okashiKosu = 0;

        GameMgr.hikari_makeokashi_startcounter = 9999; //作り始めのフラグ。10なら、10秒たったら、TimeControllerでfalseにする。カウンターは今使ってない。
        GameMgr.hikari_makeokashi_startflag = true;

        GameMgr.hikari_make_Allfailed = false;
        GameMgr.hikari_zairyo_no_flag = false;

        //制作にかかる時間(compoDBのコストタイムで兄ちゃんと共通）とタイマーをセット cost_time=1が1分なので、*1。さらに、ヒカリの場合時間が2倍かかり、お菓子LVによってさらに遅くなる。
        GameMgr.hikari_make_okashiTimeCost = 
            (int)(databaseCompo.compoitems[GameMgr.hikari_make_okashi_compID].cost_Time * 1f * 2 * GameMgr.hikari_make_okashiTime_costbuf);
        GameMgr.hikari_make_okashiTimeCounter = GameMgr.hikari_make_okashiTimeCost;
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
        renkin_hyouji = pitemlist.player_yosokuitemlist[pitemlist.player_yosokuitemlist.Count - 1].itemNameHyouji;
        new_item = pitemlist.player_yosokuitemlist.Count - 1;


        //リザルトアニメーション開始
        pitemlistController_obj.SetActive(false);
        //compo_anim_on = true; //アニメスタート
        //StartCoroutine("Original_Compo_anim");

        //メモは全てオフに
        recipiMemoButton.SetActive(false);
        recipimemoController_obj.SetActive(false);
        memoResult_obj.SetActive(false);
        recipi_archivement_obj.SetActive(false);

        //YesNoパネル
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
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

        //温度管理していた場合は、ここでリセット
        GameMgr.tempature_control_ON = false;
    }

    void GetExpMethod()
    {
        if (GameMgr.System_MagicUse_Flag)
        {
            _getexp = databaseCompo.compoitems[result_ID].renkin_Bexp;
            //_getexp = databaseCompo.compoitems[result_ID].renkin_Bexp / databaseCompo.compoitems[result_ID].comp_count;
            if (_getexp <= 0)
            {
                _getexp = 1;
            }
            PlayerStatus.player_renkin_exp += _getexp; //すでに作ったことがある場合、取得量は少なくなる
        } else
        {
            _getexp = 0;
        }
    }

    void GetExpMethodTopping()
    {
        if (GameMgr.System_MagicUse_Flag)
        {
            _getexp = compound_keisan._getExp;
            PlayerStatus.player_renkin_exp += _getexp; //エクストリーム経験値。確率が低いものほど、経験値が大きくなる。
        }
        else
        {
            _getexp = 0;
        }
    }

    //おかしレベルの経験値とレベルアップ処理　ヒカリの経験値だけど、おにいちゃんが作っても上がる、という仕様。だったが、現在オフ
    void OkashiExpUp()
    {
        /*
        _getexp2 = 2;
        hikariOkashiExpTable.hikariOkashi_ExpTableMethod(database.items[result_item].itemType_sub.ToString(), _getexp2, 1, 0);
        */
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

        shopbuy_kettei_item1 = shopitemlistController.shop_kettei_item1; //ショップ購入時の決定アイテムIDは、参照先がdatabaseの場合とeventdatabaseで分かれているので、下の処理内で、配列番号を取得するようにしている。
        //Debug.Log("決定したアイテムID: " + kettei_item1 + " リスト番号: " + shopitemlistController.shop_count);

        toggle_type1 = shopitemlistController.shop_itemType;
        dongri_type = shopitemlistController.shop_dongriType;

        result_kosu = shopitemlistController.shop_final_itemkosu_1; //買った個数

        //通常アイテム
        if (toggle_type1 == 0)
        {
            //プレイヤーアイテムリストに追加。
            pitemlist.addPlayerItem(database.items[database.SearchItemID(shopbuy_kettei_item1)].itemName, result_kosu);
        }
        else if (toggle_type1 == 1) //shop_itemType=1のものは、レシピのこと。買うことで、あとでアトリエに戻ったときに、本を読み、いくつかのレシピを解禁するフラグになる。
        {
            //イベントプレイヤーアイテムリストに追加。レシピのフラグなど。            
            pitemlist.add_eventPlayerItem(pitemlist.SearchEventItemID(shopbuy_kettei_item1), result_kosu);
            pitemlist.eventitemlist_Sansho(); //デバッグ用

        }
        else if (toggle_type1 == 5) //shop_itemType = 5 のものは、エメラルどんぐりで買うアイテムで特殊。レアアイテム・コスチュームなどのアイテム系。
        {
            //イベントプレイヤーアイテムリストに追加。レシピのフラグなど。            
            pitemlist.add_EmeraldPlayerItem(pitemlist.SearchEmeraldItemID(shopbuy_kettei_item1), result_kosu);
            pitemlist.emeralditemlist_Sansho(); //デバッグ用。コメントアウトしても大丈夫。
            
        }
        else if (toggle_type1 == 2 || toggle_type1 == 6) //2は機材。shop_itemType = 6 は、装備品や飾りなどの特殊アイテム。買うことでパラメータを上昇させたり、フラグをたてる。
        {
            //かごの大きさ計算やバフの計算は「Buf_Power_keisan.cs」
            //プレイヤーアイテムリストに追加。
            pitemlist.addPlayerItem(database.items[database.SearchItemID(shopbuy_kettei_item1)].itemName, result_kosu);
            
        }
        else //トッピングなど
        {
            //プレイヤーアイテムリストに追加。
            pitemlist.addPlayerItem(database.items[database.SearchItemID(shopbuy_kettei_item1)].itemName, result_kosu);
        }

        switch (GameMgr.Scene_Category_Num)
        {
            case 20:

                //所持金をへらす
                moneyStatus_Controller.UseMoney(shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_costprice * result_kosu);

                //ショップの在庫をへらす。
                shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko -= result_kosu;
                break;

            case 40:

                //所持金をへらす
                moneyStatus_Controller.UseMoney(shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_costprice * result_kosu);

                //ショップの在庫をへらす。
                shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko -= result_kosu;
                break;

            case 50:

                kaeruCoin_Controller = canvas.transform.Find("KaeruCoin_Panel").GetComponent<KaeruCoin_Controller>();

                switch(dongri_type)
                {
                    case 0:

                        //エメラルどんぐり数をへらす
                        kaeruCoin_Controller.UseCoin(shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_costprice * result_kosu);
                        break;

                    case 1:

                        //サファイアどんぐり数をへらす
                        kaeruCoin_Controller.UseCoin2(shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_costprice * result_kosu);
                        break;
                }
                
                //ショップの在庫をへらす。
                shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko -= result_kosu;
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

        kettei_item1 = GameMgr.Final_list_itemID1;
        toggle_type1 = GameMgr.Final_toggle_Type1;
        result_kosu = GameMgr.Final_kettei_kosu1; //買った個数

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
        else if (toggle_type1 == 2) //お菓子パネルアイテム
        {
            //イベントプレイヤーアイテムリストに追加。レシピのフラグなど。
            pitemlist.deleteExtremePanelItem(kettei_item1, result_kosu);

        }

        switch (GameMgr.Scene_Category_Num)
        {
            case 20:

                //所持金を増やす
                moneyStatus_Controller.GetMoney(database.items[GameMgr.Final_list_itemID1].sell_price * GameMgr.Final_kettei_kosu1);

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
    void Compo_Animation()
    {

        switch (compo_anim_status)
        {
            case 0: //初期化 状態１

                //メモは全てオフに
                recipiMemoButton.SetActive(false);
                recipimemoController_obj.SetActive(false);
                memoResult_obj.SetActive(false);
                recipi_archivement_obj.SetActive(false);

                //YesNoパネル
                yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
                yes_no_panel.SetActive(false);

                //半透明の黒をON
                Sequence sequence = DOTween.Sequence();

                //まず、初期値。
                BlackImage.GetComponent<CanvasGroup>().alpha = 0;
                sequence.Append(BlackImage.GetComponent<CanvasGroup>().DOFade(1, 0.5f));

                if (character_ON)
                {
                    //ヒカリちゃんを右にずらす
                    character_move.transform.DOMoveX(8f, 1f)
                        .SetEase(Ease.InOutSine);
                }

                //パーティクルエフェクト生成＋アニメ開始
                StartParticleEffect(0); //0は通常調合
                

                

                //一時的にお菓子のHP減少をストップ
                //extremePanel.LifeAnimeOnFalse();

                Debug_timeCount = 0.0f; //デバッグ用　演出時間の計測
                Debug_stopwatch = true;
                if (GameMgr.DEBUG_MagicPlayTime_ON)
                {
                    Debug_timeCount_Panel.SetActive(true);
                }

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
                        AddParticleEffect(0);

                        //音を鳴らす
                        sc.PlaySe(89);

                        _text.text = "ガシャ　ガシャ . . . ";
                    }
                    else
                    {
                        timeOut = 0.5f;
                        compo_anim_status = 10;
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

                if (timeOut <= 1.0)
                {
                    //スペシャルなお菓子演出が入る場合、ここらへんでホワイトアウト
                    if (GameMgr.Special_OkashiEnshutsuFlag)
                    {
                        SpecialwhiteEffect.SetActive(true);
                        SpecialwhiteEffect.GetComponent<CanvasGroup>().DOFade(1, 0.8f);
                    }
                }

                if (timeOut <= 0.0)
                {
                    timeOut = 0.5f;
                    compo_anim_status = 10;
                }
                break;

            case 10: //アニメ終了。判定する


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

                //デバッグ用計測終了
                Debug_stopwatch = false;

                break;

            default:
                break;
        }        

        //時間減少
        timeOut -= Time.deltaTime;

        //デバッグ用時間計測
        if(Debug_stopwatch)
        {
            Debug_timeCount += Time.deltaTime;
            Debug_timeCount_Panel_text.text = Debug_timeCount.ToString();
        }
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



    //
    //魔法演出中のアニメ
    //
    void MagicCompo_Animation()
    {

        switch (compo_anim_status)
        {
            case 0: //初期化 状態１

                //メモは全てオフに
                recipiMemoButton.SetActive(false);
                recipimemoController_obj.SetActive(false);
                memoResult_obj.SetActive(false);
                recipi_archivement_obj.SetActive(false);

                //YesNoパネル
                yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
                yes_no_panel.SetActive(false);

                //半透明の黒をON
                //Sequence sequence = DOTween.Sequence();

                //まず、初期値。
                //BlackImage.GetComponent<CanvasGroup>().alpha = 0;
                //sequence.Append(BlackImage.GetComponent<CanvasGroup>().DOFade(1, 0.5f));

                /*if (character_ON)
                {
                    //ヒカリちゃんを右にずらす
                    character_move.transform.DOMoveX(8f, 1f)
                        .SetEase(Ease.InOutSine);
                }*/

                //エフェクト生成＋アニメ開始
                StartParticleEffect(1); //1は魔法調合時                

                //一時的にお菓子のHP減少をストップ
                //extremePanel.LifeAnimeOnFalse();

                Debug_timeCount = 0.0f; //デバッグ用　演出時間の計測
                Debug_stopwatch = true;
                if (GameMgr.DEBUG_MagicPlayTime_ON)
                {
                    Debug_timeCount_Panel.SetActive(true);
                }

                timeOut = GameMgr.System_magic_playtime;
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
                        AddParticleEffect(0);

                        //音を鳴らす
                        sc.PlaySe(89);

                        _text.text = "ガシャ　ガシャ . . . ";
                    }
                    else
                    {
                        timeOut = 0.5f;
                        compo_anim_status = 10;
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

                if (timeOut <= 1.0)
                {
                    //スペシャルなお菓子演出が入る場合、ここらへんでホワイトアウト
                    if (GameMgr.Special_OkashiEnshutsuFlag)
                    {
                        SpecialwhiteEffect.SetActive(true);
                        SpecialwhiteEffect.GetComponent<CanvasGroup>().DOFade(1, 0.8f);
                    }
                }

                if (timeOut <= 0.0)
                {
                    timeOut = 0.5f;
                    compo_anim_status = 10;
                }
                break;

            case 10: //アニメ終了。判定する


                //カードビューのカードアニメもストップ
                card_view.cardcompo_anim_on = false;
                card_view.DeleteCard_DrawView();

                //音を止める
                sc.StopSe();

                //Debug.Log("アニメ終了");
                magiccompo_anim_end = true;

                //デバッグ用計測終了
                Debug_stopwatch = false;

                break;

            default:
                break;
        }

        //時間減少
        timeOut -= Time.deltaTime;

        //デバッグ用時間計測
        if (Debug_stopwatch)
        {           
            Debug_timeCount += Time.deltaTime;
            Debug_timeCount_Panel_text.text = Debug_timeCount.ToString();
        }
    }

    void CompleteMagicAnim()
    {
        //完成～出来たー！という変化をつけるために、背景を変える。
        


        //アニメーション
        /*//まず、初期値。
        Sequence sequence2 = DOTween.Sequence();
        CompleteImage.transform.Find("Image").GetComponent<CanvasGroup>().alpha = 0;
        sequence2.Append(CompleteImage.transform.Find("Image").DOScale(new Vector3(0.3f, 0.3f, 1.0f), 0.0f)
            ); //

        //移動のアニメ
        sequence2.Append(CompleteImage.transform.Find("Image").DOScale(new Vector3(0.5f, 0.5f, 1.0f), 0.75f)
        //.SetEase(Ease.OutElastic)); //はねる動き
        .SetEase(Ease.OutExpo)); //スケール小からフェードイン
        sequence2.Join(CompleteImage.transform.Find("Image").GetComponent<CanvasGroup>().DOFade(1, 0.2f));*/
    }

    void StartParticleEffect(int _colorstatus)
    {      
        switch(_colorstatus)
        {
            case 1: //魔法調合時

                //音を鳴らす キラララーン
                sc.PlaySe(129);
                sc.PlaySe(131);

                //パーティクルと色の取得
                /*compo1_particle = _listEffect[0].GetComponent<ParticleSystem>();
                p_color1 = _listEffect[0].GetComponent<Particle_Compo1>().color_red;

                main = compo1_particle.main;
                main.startColor = new ParticleSystem.MinMaxGradient(p_color1); //色の指定*/
                break;

            default:

                //キラパーティクルON
                _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab1));

                //音を鳴らす　シュイイイン
                sc.PlaySe(10);

                //通常調合　温度管理使用時、色が赤になる。
                if (GameMgr.tempature_control_ON)
                {
                    if (GameMgr.System_tempature_control_Param_time != 0) //時間を0分にしたときは、無視
                    {
                        Debug.Log("温度管理ONでパーティクル赤になる");
                        //パーティクルと色の取得
                        compo1_particle = _listEffect[0].GetComponent<ParticleSystem>();
                        p_color1 = _listEffect[0].GetComponent<Particle_Compo1>().color_red;

                        main = compo1_particle.main;
                        main.startColor = new ParticleSystem.MinMaxGradient(p_color1); //色の指定
                    }
                }
                else
                { }
                break;
        }
        
    }

    void AddParticleEffect(int _colorstatus)
    {
        _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab6));

        switch (_colorstatus)
        {
            case 1: //魔法調合時

                //パーティクルと色の取得
                compo2_particle = _listEffect[1].GetComponent<ParticleSystem>();
                p_color2 = _listEffect[1].GetComponent<Particle_Compo1>().color_red;

                main = compo2_particle.main;
                main.startColor = new ParticleSystem.MinMaxGradient(p_color2); //色の指定
                break;

            default:

                break;
        }

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

    void ResultEffect_OK(int _status)
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

        //音を鳴らす　ブィィン！
        switch(_status)
        {
            case 0: //通常調合用のエフェクトと効果音

                sc.PlaySe(4);
                sc.PlaySe(27);
                sc.PlaySe(78);
                break;

            case 1: //魔法調合用のエフェクトと効果音
                sc.PlaySe(130);
                //sc.PlaySe(132);
                sc.PlaySe(78);
                break;
        }
        

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
                //+ "\n" + _ex_text +"ジョブ経験値 " + _getexp + "上がった！";
        }
        else
        {
            _text.text = "やったね！ " +
                renkin_hyouji +
                " が" + result_kosu + "個 できたよ！";
                //+ "\n" + _ex_text;
        }

        Debug.Log(renkin_hyouji + "が出来ました！");

    }

    void renkin_exp_up()
    {
        //Debug.Log("_getexp: " + _getexp);

        _yaki = "";

        if (_getexp != 0)
        {
            if (GameMgr.tempature_control_ON)
            {
                _yaki = "　" + GameMgr.tempature_control_Param_yakitext;
            }

            _text.text = "やったね！ " +
            //GameMgr.ColorYellow + pitemlist.player_originalitemlist[new_item].item_SlotName + "</color>" 
            pitemlist.player_check_itemlist[new_item].itemNameHyouji +
            " が" + result_kosu + "個 できたよ！" + _yaki;
            //+ "\n" + _ex_text + "ジョブ経験値 " + _getexp + "上がった！";
            
        }
        else
        {
            if (GameMgr.tempature_control_ON)
            {
                _yaki = "　" + GameMgr.tempature_control_Param_yakitext;
            }

            _text.text = "やったね！ " +
            //GameMgr.ColorYellow + pitemlist.player_originalitemlist[new_item].item_SlotName + "</color>" + 
            pitemlist.player_check_itemlist[new_item].itemNameHyouji +
            " が" + result_kosu + "個 できたよ！" + _yaki;
            //+ "\n" + _ex_text;
        }

        Debug.Log(pitemlist.player_check_itemlist[new_item].itemNameHyouji + "が出来ました！");

    }

    void renkin_exp_up2()
    {
        _yaki = "";

        if (_getexp != 0)
        {
            if (GameMgr.tempature_control_ON)
            {
                _yaki = "　" + GameMgr.tempature_control_Param_yakitext;
            }

            _text.text = "やったね！ " +
            database.items[_id1].itemNameHyouji + " と " + database.items[_id2].itemNameHyouji +
            " が" + result_kosu + "個 できたよ！" + _yaki;
            //+ "\n" + _ex_text +"ジョブ経験値 " + _getexp + "上がった！";
        }
        else
        {
            if (GameMgr.tempature_control_ON)
            {
                _yaki = "　" + GameMgr.tempature_control_Param_yakitext;
            }

            _text.text = "やったね！ " +
            database.items[_id1].itemNameHyouji + " と " + database.items[_id2].itemNameHyouji +
            " が" + result_kosu + "個 できたよ！" + _yaki;
            //+ "\n" + _ex_text;
        }

        Debug.Log(database.items[_id1].itemNameHyouji + " " + database.items[_id2].itemNameHyouji + "が出来ました！");

    }

    void Failed_Text()
    {
        _text.text = "失敗しちゃった..！"; ;

        Debug.Log(database.items[result_item].itemNameHyouji + "調合失敗..！");
    }

    public void GirlLikeText(int _getlove_exp, int _getmoney, int total_score, int _mp)
    {

        text_area = canvas.transform.Find("MessageWindowMain").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        _a = "";
        _b = "";
        _c = "";

        if (_getlove_exp != 0)
        {
            _a = "ハートが " + GameMgr.ColorYellow + _getlove_exp + "</color>" + "アップした！";
        }
        else
        {
            _a = "ハートはかわらなかった。";
        }

        if (_getmoney > 0)
        {
            
            _b = "\n" + "ぱぱから仕送り " + GameMgr.ColorYellow + _getmoney + GameMgr.MoneyCurrency + "</color>" + " 送られてきた！";
        }
        else
        {
            _b = "";
        }

        if (_mp > 0)
        {
            _c = "\n" + "MPが " + GameMgr.ColorYellow + _mp + "</color>" + " 上がった！";
        }
        else
        {
            _c = "";
        }

        _text.text = _a + _b + _c;
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


    //成功判定処理 Compound_Checkで事前に成功確率は計算し、ここではダイスをふるだけ。
    void CompoundSuccess_judge()
    {
        switch (_success_judge_flag)
        {
            case 0: //必ず成功

                GameMgr.Result_compound_success = true;
                break;

            case 1: //判定処理を行う
             
                _rate_final = _success_rate; //_success_rateは、事前にCompound_checkで計算したものを代入してるだけ。

                //サイコロをふる
                dice = Random.Range(1, 100); //1~100までのサイコロをふる。

                Debug.Log("最終成功確率: " + _rate_final + " " + "ダイスの目: " + dice);

                if (dice <= (int)_rate_final) //出た目が、成功率より下なら成功
                {
                    GameMgr.Result_compound_success = true;
                }
                else //失敗
                {
                    GameMgr.Result_compound_success = false;
                }

                break;

            case 2: //必ず失敗

                GameMgr.Result_compound_success = false;
                break;
        }


    }
}
