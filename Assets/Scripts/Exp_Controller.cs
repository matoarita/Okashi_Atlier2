using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//
//アイテムの更新・経験値の増減処理を行うコントローラー　＋　調合の処理を担うメソッド
//

public class Exp_Controller : SingletonMonoBehaviour<Exp_Controller>
{
    private GameObject canvas;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

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

    private ExpTable exp_table;
    private SoundController sc;

    private PlayerItemList pitemlist;

    private Compound_Keisan compound_keisan;

    private GameObject black_panel_A;
    private GameObject compoBG_A;

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
    private int kettei_item1;

    private int result_item;
    private int result_ID;
    private int new_item;

    private int result_kosu;
    public int set_kaisu; //オリジナル調合時、その組み合わせで何個作るか、の個数

    private int _getexp;

    //成功確率(外部スクリプトから保存・読み込み用）
    public float _temp_srate_1;
    public float _temp_srate_2;
    public float _temp_srate_3;
    //**ここまで**//

    public float _success_rate;
    public float _rate_final;
    public int _success_judge_flag; // 0=必ず成功, 1=計算する, 2=必ず失敗
    private int dice; //確率計算用サイコロ

    private string[] _slot = new string[10];
    private string[] _slotHyouji1 = new string[10]; //日本語に変換後の表記を格納する。スロット覧用

    private int i, sw, count;    

    public int Comp_method_bunki; //トッピング調合メソッドの分岐フラグ

    public bool compound_success; //調合の成功か失敗

    public bool NewRecipiFlag;  //新しいレシピをひらめいたフラグをON
    public int NewRecipi_compoID;   //そのときの、調合DBのID

    public bool NewRecipiflag_check;
    public bool extreme_on; //エクストリーム調合から、新しいアイテムを閃いた場合は、ON

    public bool result_ok; // 調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool recipiresult_ok; //レシピ調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool topping_result_ok; //トッピング調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool roast_result_ok; //「焼く」完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。

    public bool girleat_ok; // 女の子にアイテムをあげた時の完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool shop_buy_ok; //購入完了のフラグ。これがたっていたら、購入の処理を行い、フラグをオフに。
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
    private List<GameObject> _listEffect = new List<GameObject>();

    private GameObject ResultBGimage;

    //SEを鳴らす
    public AudioClip sound1;
    AudioSource audioSource;

    //エクストリームパネルで制作したお菓子の一時保存用パラメータ。シーン移動しても、削除されない。
    public int _temp_extreme_id;
    public int _temp_extreme_itemtype;
    public bool _temp_extremeSetting;
    public float _temp_extreme_money;
    public float _temp_moneydeg;
    public bool _temp_life_anim_on;
    public float _temp_Starthp;

    private string renkin_hyouji;


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

        //音声ファイルの取得。SCを使わずに鳴らす場合はこっち。
        //sound1 = (AudioClip)Resources.Load("Utage_Scenario/Sound/SE/SE_10");

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();
       

        //エフェクトプレファブの取得
        Compo_Magic_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_Compo1");
        Compo_Magic_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Particle_Compo2");
        Compo_Magic_effect_Prefab3 = (GameObject)Resources.Load("Prefabs/Particle_Compo3");
        Compo_Magic_effect_Prefab4 = (GameObject)Resources.Load("Prefabs/Particle_Compo4");
        Compo_Magic_effect_Prefab5 = (GameObject)Resources.Load("Prefabs/Particle_Compo5");

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //キャンバスの読み込み
                canvas = GameObject.FindWithTag("Canvas");

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                //エクストリームパネルオブジェクトの取得
                extremePanel_obj = GameObject.FindWithTag("ExtremePanel");
                extremePanel = extremePanel_obj.GetComponent<ExtremePanel>();

                //レベルアップチェック用オブジェクトの取得
                exp_table = GameObject.FindWithTag("ExpTable").GetComponent<ExpTable>();

                //確率パネルの取得
                kakuritsuPanel_obj = canvas.transform.Find("KakuritsuPanel").gameObject;
                kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

                //レシピメモボタンを取得
                recipimemoController_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiMemo_ScrollView").gameObject;
                recipiMemoButton = canvas.transform.Find("Compound_BGPanel_A/RecipiMemoButton").gameObject;
                memoResult_obj = canvas.transform.Find("Compound_BGPanel_A/Memo_Result").gameObject;

                //黒半透明パネルの取得
                black_panel_A = canvas.transform.Find("Black_Panel_A").gameObject;

                //コンポBGパネルの取得
                compoBG_A = canvas.transform.Find("Compound_BGPanel_A").gameObject;               

                slotchangename = GameObject.FindWithTag("SlotChangeName").gameObject.GetComponent<SlotChangeName>();
                ResultBGimage = compoBG_A.transform.Find("ResultBG").gameObject;
                ResultBGimage.SetActive(false);

                break;

            default:
                break;
        }

        audioSource = GetComponent<AudioSource>();

        result_ok = false;
        recipiresult_ok = false;
        girleat_ok = false;

        //blend_flag = false;

        compound_success = false;
        NewRecipiFlag = false;

        extreme_on = false;
        NewRecipiflag_check = false;

        i = 0;
        sw = 0;
        new_item = 0;

        Comp_method_bunki = 0;

        compo_anim_status = 0;
        compo_anim_on = false;
        compo_anim_end = false;        

        _temp_extreme_id = 9999;
        _temp_extremeSetting = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {

            //シーン読み込みのたびに、一度リセットされてしまうので、アップデートで一度初期化
            if (compound_Main_obj == null)
            {

                //キャンバスの読み込み
                canvas = GameObject.FindWithTag("Canvas");

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                extremePanel_obj = GameObject.FindWithTag("ExtremePanel");
                extremePanel = extremePanel_obj.GetComponent<ExtremePanel>();
                
                text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
                _text = text_area.GetComponentInChildren<Text>();

                //確率パネルの取得
                kakuritsuPanel_obj = canvas.transform.Find("KakuritsuPanel").gameObject;
                kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

                //レシピメモボタンを取得
                recipimemoController_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiMemo_ScrollView").gameObject;
                recipiMemoButton = canvas.transform.Find("Compound_BGPanel_A/RecipiMemoButton").gameObject;
                memoResult_obj = canvas.transform.Find("Compound_BGPanel_A/Memo_Result").gameObject;

                //レベルアップチェック用オブジェクトの取得
                exp_table = GameObject.FindWithTag("ExpTable").gameObject.GetComponent<ExpTable>();

                //黒半透明パネルの取得
                black_panel_A = canvas.transform.Find("Black_Panel_A").gameObject;

                //コンポBGパネルの取得
                compoBG_A = canvas.transform.Find("Compound_BGPanel_A").gameObject;
                ResultBGimage = compoBG_A.transform.Find("ResultBG").gameObject;
                ResultBGimage.SetActive(false);

                //スロット名前変換用オブジェクトの取得
                slotchangename = GameObject.FindWithTag("SlotChangeName").gameObject.GetComponent<SlotChangeName>();
            }

            //調合中ウェイト+アニメ
            if (compo_anim_on == true)
            {

                //ウェイト
                Compo_Magic_Animation();
            }

        }
    }        


    //
    //オリジナル調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void ResultOK()
    {

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
            result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * set_kaisu;


            //①調合処理
            compound_keisan.Topping_Compound_Method(0);

            result_item = pitemlist.player_originalitemlist.Count - 1;

            renkin_hyouji = pitemlist.player_originalitemlist[result_item].itemNameHyouji;

            //制作したアイテムが材料、もしくはポーション類ならエクストリームパネルに設定はしない。
            if (pitemlist.player_originalitemlist[result_item].itemType.ToString() == "Mat" || pitemlist.player_originalitemlist[result_item].itemType.ToString() == "Potion")
            {
                //ただし、例外として、ホイップクリーム（絞り袋セット前）はセットされる。その他もあるかも。
                if(pitemlist.player_originalitemlist[result_item].itemType_sub.ToString() == "Cream")
                {
                    //パネルに、作ったやつを表示する。
                    extremePanel.SetExtremeItem(result_item, 1);

                }
            }
            else
            {
                //パネルに、作ったやつを表示する。
                extremePanel.SetExtremeItem(result_item, 1);

            }

            new_item = result_item;

            card_view.ResultCard_DrawView(1, new_item);

            /*②店売りアイテムとして生成
            //アイテム削除
            compound_keisan.Delete_playerItemList();

            renkin_hyouji = database.items[result_item].itemNameHyouji;

            //店売りアイテムとして生成
            pitemlist.addPlayerItem(result_item, result_kosu);

            //制作したアイテムが材料、もしくはポーション類ならエクストリームパネルに設定はしない。
            if (database.items[result_item].itemType.ToString() == "Mat" || database.items[result_item].itemType.ToString() == "Potion")
            {
            }
            else
            {
                //右側パネルに、作ったやつを表示する。
                extremePanel.SetExtremeItem(result_item, 0);

            }

            new_item = result_item;

            card_view.ResultCard_DrawView(0, new_item);*/

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


            //作ったことがあるかどうかをチェック
            if (databaseCompo.compoitems[result_ID].comp_count == 0)
            {
                //作った回数をカウント
                databaseCompo.compoitems[result_ID].comp_count++;

                //完成アイテムの、レシピフラグをONにする。
                databaseCompo.compoitems[result_ID].cmpitem_flag = 1;

                _getexp = databaseCompo.compoitems[result_ID].renkin_Bexp;
                PlayerStatus.player_renkin_exp += _getexp; //調合完成のアイテムに対応した経験値がもらえる。

                NewRecipiFlag = true;
                NewRecipi_compoID = result_ID;

                _ex_text = "<color=#FF78B4>" + "新しいレシピ" + "</color>" + "を閃いた！"  + "\n";

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
            if (PlayerStatus.First_recipi_on != true)
            {
                PlayerStatus.First_recipi_on = true;
            }

            if (extreme_on) //トッピング調合から、新規作成に分岐した場合
            {
                if (!PlayerStatus.First_extreme_on) //仕上げを一度もやったことがなかったら、フラグをON
                {
                    PlayerStatus.First_extreme_on = true;
                }
            }

            //テキストの表示
            renkin_default_exp_up();

            //完成エフェクト
            ResultEffect_OK();
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

            //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
            pitemlist.addPlayerItem(result_item, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList();
            extremePanel.deleteExtreme_Item();

            card_view.ResultCard_DrawView(0, result_item);

            //テキストの表示
            Failed_Text();

            //完成エフェクト
            ResultEffect_NG();
        }

        result_ok = false;

        //black_panel_A.SetActive(true);

        //日数の経過
        PlayerStatus.player_time += databaseCompo.compoitems[result_ID].cost_Time;

        _ex_text = "";

        //経験値の増減後、レベルアップしたかどうかをチェック
        exp_table.Check_LevelUp();
    }



    //
    //レシピ調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void Recipi_ResultOK()
    {


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

            //①調合処理＜予測で処理＞
            //compound_keisan.Topping_Compound_Method(1);
            /*
            result_item = pitemlist.player_originalitemlist.Count - 1;

            renkin_hyouji = pitemlist.player_originalitemlist[result_item].itemNameHyouji;

            
            //制作したアイテムが材料、もしくはポーション類ならエクストリームパネルに設定はしない。
            if (pitemlist.player_originalitemlist[result_item].itemType.ToString() == "Mat" || pitemlist.player_originalitemlist[result_item].itemType.ToString() == "Potion")
            {
            }
            else
            {
                //右側パネルに、作ったやつを表示する。
                extremePanel.SetExtremeItem(result_item, 1);

            }

            new_item = result_item;

            card_view.ResultCard_DrawView(1, new_item);*/


            //②店売りアイテムとして生成し、実際にアイテムを追加。

            //リザルトアイテムを代入
            result_item = recipilistController.result_recipiitem;

            compound_keisan.Delete_playerItemList();
            renkin_hyouji = database.items[result_item].itemNameHyouji;
            pitemlist.addPlayerItem(result_item, result_kosu);

            if (database.items[result_item].itemType.ToString() == "Mat" || database.items[result_item].itemType.ToString() == "Potion")
            {
                //ただし、例外として、ホイップクリーム（絞り袋セット前）はセットされる。その他もあるかも。
                if (database.items[result_item].itemType_sub.ToString() == "Cream")
                {
                    //パネルに、作ったやつを表示する。
                    extremePanel.SetExtremeItem(result_item, 0);

                }
            }
            else
            {
                //右側パネルに、作ったやつを表示する。
                extremePanel.SetExtremeItem(result_item, 0);

            }

            card_view.RecipiResultCard_DrawView(0, result_item);
            

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

            //テキストの表示            
            renkin_default_exp_up();

            //完成エフェクト
            ResultEffect_OK();
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
            pitemlist.addPlayerItem(result_item, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList();
            extremePanel.deleteExtreme_Item();

            card_view.ResultCard_DrawView(0, result_item);

            //テキストの表示
            Failed_Text();

            //完成エフェクト
            ResultEffect_NG();
        }

        recipiresult_ok = false;

        //black_panel_A.SetActive(true);

        //日数の経過
        PlayerStatus.player_time += databaseCompo.compoitems[result_ID].cost_Time;

        //経験値の増減後、レベルアップしたかどうかをチェック
        exp_table.Check_LevelUp();
    }



    //
    //トッピング調合完了の場合、ここでアイテムリストの更新行う。
    //
    public void Topping_Result_OK()
    {
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

            new_item = pitemlist.player_originalitemlist.Count - 1;

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
                    databaseCompo.compoitems[result_ID].cmpitem_flag = 1;

                    _getexp = databaseCompo.compoitems[result_ID].renkin_Bexp;
                    PlayerStatus.player_renkin_exp += _getexp; //エクストリームで新しく閃いた場合の経験値

                    NewRecipiFlag = true;
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
            card_view.ResultCard_DrawView(1, new_item);

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
            extremePanel.SetExtremeItem(new_item, 1);
            
            //テキストの表示
            renkin_exp_up();

            //完成エフェクト
            ResultEffect_OK();
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

            //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
            pitemlist.addPlayerItem(result_item, result_kosu);

            //失敗した場合でも、アイテムは消える。
            compound_keisan.Delete_playerItemList();
            extremePanel.deleteExtreme_Item();

            card_view.ResultCard_DrawView(0, result_item);

            //テキストの表示
            Failed_Text();

            //完成エフェクト
            ResultEffect_NG();
        }

        topping_result_ok = false;

        //テキスト表示後、閃いた～をリセットしておく
        _ex_text = "";

        //経験値の増減後、レベルアップしたかどうかをチェック
        exp_table.Check_LevelUp();
    }   


    //
    //「ショップで購入」の場合、ここでアイテムリストの更新行う。
    //
    public void Shop_ResultOK()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        shopitemlistController_obj = GameObject.FindWithTag("ShopitemList_ScrollView");
        shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = GameObject.FindWithTag("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        kettei_item1 = shopitemlistController.shop_kettei_item1;
        //Debug.Log("決定したアイテムID: " + kettei_item1 + " リスト番号: " + shopitemlistController.shop_count);

        toggle_type1 = shopitemlistController.shop_itemType;

        result_kosu = shopitemlistController.shop_final_itemkosu_1; //買った個数

        //通常アイテム
        if (toggle_type1 == 0)
        {
            //プレイヤーアイテムリストに追加。
            pitemlist.addPlayerItem(kettei_item1, result_kosu);
        }
        else if (toggle_type1 == 1) //shop_itemType=1のものは、レシピのこと。買うことで、あとでアトリエに戻ったときに、本を読み、いくつかのレシピを解禁するフラグになる。
        {
            //イベントプレイヤーアイテムリストに追加。レシピのフラグなど。
            pitemlist.add_eventPlayerItem(kettei_item1, result_kosu);

        }
        else //トッピング・機材アイテムなど
        {
            //プレイヤーアイテムリストに追加。
            pitemlist.addPlayerItem(kettei_item1, result_kosu);
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

            default:
                break;
        }

        //効果音
        sc.PlaySe(32);

        _text.text = "購入しました！他にはなにか買う？";

        shopitemlistController.ReDraw(); //リスト描画の更新

        shop_buy_ok = false;

    }


    void Compo_Magic_Animation()
    {
        //ウェイトアニメ


        switch (compo_anim_status)
        {
            case 0: //初期化 状態１
              
                //メモは全てオフに
                recipiMemoButton.SetActive(false);
                recipimemoController_obj.SetActive(false);
                memoResult_obj.SetActive(false);
                kakuritsuPanel_obj.SetActive(false);

                //エフェクト生成＋アニメ開始
                _listEffect.Add(Instantiate(Compo_Magic_effect_Prefab1));

                //音を鳴らす
                sc.PlaySe(10);
                //audioSource.PlayOneShot(sound1);

                //一時的にお菓子のHP減少をストップ
                extremePanel.LifeAnimeOnFalse();

                //背景変更
                //compoBG_A.SetActive(true);

                timeOut = 2.0f;
                compo_anim_status = 1;

                _text.text = "調合中 .";
                break;

            case 1: // 状態2

                if (timeOut <= 0.0)
                {
                    timeOut = 1.0f;
                    compo_anim_status = 2;

                    _text.text = "調合中 . .";
                }
                break;

            case 2:

                if (timeOut <= 0.0)
                {
                    timeOut = 0.5f;
                    compo_anim_status = 3;

                }
                break;

            case 3: //アニメ終了。判定する

                
                //カードビューのカードアニメもストップ
                card_view.cardcompo_anim_on = false;
                card_view.DeleteCard_DrawView();

                //音を止める
                //audioSource.Stop();
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

        //音を鳴らす
        sc.PlaySe(4);
        sc.PlaySe(15);
        sc.PlaySe(27);

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
                " が" + result_kosu + "個 できました！" + "\n" + _ex_text +
                "錬金経験値 " + _getexp + "上がった！";
        }
        else
        {
            _text.text = "やったね！ " +
                renkin_hyouji +
                " が" + result_kosu + "個 できました！" + "\n" + _ex_text +
                "錬金経験値は上がらなかった。";
        }

        Debug.Log(renkin_hyouji + "が出来ました！");

    }

    void renkin_exp_up()
    {

        //_slotHyouji1[]は、一度名前を、全て空白に初期化
        for (i = 0; i < _slotHyouji1.Length; i++)
        {
            _slotHyouji1[i] = "";
        }

        //カード正式名称（ついてるスロット名も含めた名前）
        slotchangename.slotChangeName(1, new_item, "yellow");

        _slotHyouji1[0] = slotchangename._slotHyouji[0];
        _slotHyouji1[1] = slotchangename._slotHyouji[1];
        _slotHyouji1[2] = slotchangename._slotHyouji[2];
        _slotHyouji1[3] = slotchangename._slotHyouji[3];
        _slotHyouji1[4] = slotchangename._slotHyouji[4];
        _slotHyouji1[5] = slotchangename._slotHyouji[5];
        _slotHyouji1[6] = slotchangename._slotHyouji[6];
        _slotHyouji1[7] = slotchangename._slotHyouji[7];
        _slotHyouji1[8] = slotchangename._slotHyouji[8];
        _slotHyouji1[9] = slotchangename._slotHyouji[9];

        if (_getexp != 0)
        {
            _text.text = "やったね！ " +
            _slotHyouji1[0] + _slotHyouji1[1] + _slotHyouji1[2] + _slotHyouji1[3] + _slotHyouji1[4] + _slotHyouji1[5] + _slotHyouji1[6] + _slotHyouji1[7] + _slotHyouji1[8] + _slotHyouji1[9] + pitemlist.player_originalitemlist[new_item].itemNameHyouji +
            " が" + result_kosu + "個 できました！" + "\n" + _ex_text +
            "錬金経験値 " + _getexp + "上がった！";
        }
        else
        {
            _text.text = "やったね！ " +
            _slotHyouji1[0] + _slotHyouji1[1] + _slotHyouji1[2] + _slotHyouji1[3] + _slotHyouji1[4] + _slotHyouji1[5] + _slotHyouji1[6] + _slotHyouji1[7] + _slotHyouji1[8] + _slotHyouji1[9] + pitemlist.player_originalitemlist[new_item].itemNameHyouji +
            " が" + result_kosu + "個 できました！" + "\n" + _ex_text +
            "錬金経験値は上がらなかった。"; ;
        }

        Debug.Log(database.items[result_item].itemNameHyouji + "が出来ました！");

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

        _text.text = "";
        /*_text.text = "好感度が " + GameMgr.ColorPink + _getlove_exp + "</color>" + "アップ！　" 
            + "お金を " + GameMgr.ColorLemon + _getmoney + "</color>" + "G ゲットした！";*/
    }

    public void GirlDisLikeText(int _getlove_exp)
    {
        text_area = canvas.transform.Find("MessageWindowMain").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        _text.text = "好感度が" + Mathf.Abs(_getlove_exp) + "下がった..。";
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
