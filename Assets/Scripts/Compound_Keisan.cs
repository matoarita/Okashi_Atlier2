using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Compound_Keisan : SingletonMonoBehaviour<Compound_Keisan>
{

    private GameObject canvas;

    private PlayerItemList pitemlist;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private MagicSkillListDataBase magicskill_database;
    private ItemCardEffectDataBase itemCardEffect_database;

    private Exp_Controller exp_Controller;

    private CombinationMain Combinationmain;

    private Buf_Power_Keisan bufpower_keisan;

    private SlotChangeName slotchangename;
    private string[] _slotHyouji1; //日本語に変換後の表記を格納する。スロット覧用
    private string itemslotname;
    private string itemfullname;

    private int i, j, n, count;
    private int itemNum, DBcount;
    private int _attri1;
    private bool Kosu_keisanmethod;

    private int total_qbox_money;

    private List<string> _itemIDtemp_result = new List<string>(); //調合リスト。アイテムネームに変換し、格納しておくためのリスト。itemNameと一致する。
    private List<string> _itemSubtype_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<string> _itemSubtypeB_temp_result = new List<string>(); //調合DBのサブタイプBの組み合わせリスト。
    private List<int> _itemKosutemp_result = new List<int>(); //調合の個数組み合わせ。
    private int inputcount;

    private bool compoDB_select_judge;

    //使用したアイテムのタイプなどを取得
    private int toggle_type1;
    private int toggle_type2;
    private int toggle_type3;
    private int base_toggle_type;

    //アイテムIDを参照している。
    private int final_base_kettei_item; //結局使わず。
    private int final_kettei_item1;
    private int final_kettei_item2;
    private int final_kettei_item3;

    //リストから選択した、リスト番号を参照している。
    private int base_kettei_item;
    private int kettei_item1;
    private int kettei_item2;
    private int kettei_item3;

    private int base_kosu;
    private int final_kette_kosu1;
    private int final_kette_kosu2;
    private int final_kette_kosu3;
    private int final_select_kaisu; //繰り返す回数　オレンジクッキー4個をレシピから作るなら、4セット。（1セットあたりの材料が、クッキーは1個、オレンジ2）　レシピでしか使ってない。
    private int nokori_kosu;

    private int result_item;
    private int result_compID;
    private int new_item;

    private bool Pate_flag;
    private int result_kosu;
    private bool Kosu_ExSetting;

    private string kettei_originalitemID1;
    private string kettei_originalitemID2;
    private string kettei_originalitemID3;

    private List<int> kyori_kosuSet = new List<int>();



    //トッピング調合用のパラメータ
    private int _id;

    private int Comp_method_bunki; //調合の分岐フラグ。Exp_Controllerで指定している。

    Dictionary<int, int> deleteOriginalList = new Dictionary<int, int>(); //オリジナルアイテムリストの削除用のリスト。ID, 個数のセット
    Dictionary<int, int> deleteExtremeList = new Dictionary<int, int>(); //お菓子パネルリストの削除用のリスト。ID, 個数のセット

    public int _baseID;
    public string _basename;
    public int _basehp;
    public int _baseday;
    public int _basequality;
    public int _baseexp;
    public float _baseprobability;
    public int _baserich;
    public int _basesweat;
    public int _basebitter;
    public int _basesour;
    public int _basecrispy;
    public int _basefluffy;
    public int _basesmooth;
    public int _basehardness;
    public int _basejiggly;
    public int _basechewy;
    public int _basejuice;
    public int _basetea_flavor;
    public int _basesp_wind;
    public int _basesp_score2;
    public int _basesp_score3;
    public int _basesp_score4;
    public int _basesp_score5;
    public int _basesp_score6;
    public int _basesp_score7;
    public int _basesp_score8;
    public int _basesp_score9;
    public int _basesp_score10;
    public float _base_bestwelldone;
    public int _basepowdery;
    public int _baseoily;
    public int _basewatery;
    public int _basebeauty;
    public float _basegirl1_like;
    public int _basecost;
    public int _basesell;
    public string[] _basetp;
    public string _base_itemType;
    public string _base_itemType_sub;
    public string _base_itemType_subB;
    public int _base_extreme_kaisu;
    public int _base_item_hyouji;
    public string _base_itemdesc;
    public string[] _baseMS;
    public int[] _baseMSvalue;
    public int _baseattri1;
    public int _baseattri2;
    public int _baseattri3;
    public int _basemagic;

    private string _addname;
    private int _addhp;
    private int _addday;
    private int _addquality;
    private int _addexp;
    private int _addrich;
    private int _addsweat;
    private int _addbitter;
    private int _addsour;
    private int _addcrispy;
    private int _addfluffy;
    private int _addsmooth;
    private int _addhardness;
    private int _addjiggly;
    private int _addchewy;
    private int _addpowdery;
    private int _addoily;
    private int _addwatery;
    private int _addbeauty;
    private int _addtea_flavor;
    private int _addsp_wind;
    private int _addsp_score2;
    private int _addsp_score3;
    private int _addsp_score4;
    private int _addsp_score5;
    private int _addsp_score6;
    private int _addsp_score7;
    private int _addsp_score8;
    private int _addsp_score9;
    private int _addsp_score10;
    private float _addbest_welldone;
    private int _addbase_score;
    private float _addgirl1_like;
    private int _addcost;
    private int _addsell;
    private string[] _addtp;
    private string[] _addkoyutp;
    private string _add_itemType;
    private string _add_itemType_sub;
    private int _addkosu;
    private string _addMS;
    private int _addMSvalue;
    private int _addmagic;

    //_baseに加算する前に、一時的に計算する用。
    private int _temphp;
    private int _tempday;
    private int _tempquality;
    private int _tempexp;
    private int _temprich;
    private int _tempsweat;
    private int _tempbitter;
    private int _tempsour;
    private int _tempcrispy;
    private int _tempfluffy;
    private int _tempsmooth;
    private int _temphardness;
    private int _tempjiggly;
    private int _tempchewy;
    private int _temppowdery;
    private int _tempoily;
    private int _tempwatery;
    private int _tempbeauty;
    private int _temptea_flavor;
    private int _tempsp_wind;
    private int _tempsp_score2;
    private int _tempsp_score3;
    private int _tempsp_score4;
    private int _tempsp_score5;
    private int _tempsp_score6;
    private int _tempsp_score7;
    private int _tempsp_score8;
    private int _tempsp_score9;
    private int _tempsp_score10;
    private float _tempgirl1_like;
    private int _tempcost;
    private int _tempsell;
    private string[] _temptp;
    private int _tempmagic;

    private string _before_itemtype_Sub;

    //小麦粉の比率計算時に使用。
    /*private int _komugikomp;
    private int _komugikoday;
    private int _komugikoquality;
    private int _komugikorich;
    private int _komugikosweat;
    private int _komugikobitter;
    private int _komugikosour;
    private int _komugikocrispy;
    private int _komugikofluffy;
    private int _komugikosmooth;
    private int _komugikohardness;
    private int _komugikojiggly;
    private int _komugikochewy;
    private int _komugikopowdery;
    private int _komugikooily;
    private int _komugikowatery;
    private float _komugikogirl1_like;
    private int _komugikocost;
    private int _komugikosell;*/

    private int total_kosu;

    //比率計算用パラメータ
    private float _add_ratio;
    private float _bad_ratio;
    private float _komugibad_ratio;
    private int etc_mat_count;
    private float komugiko_distance;

    private int keisan_method_flag;
    private float totalkyori;
    private float kyori_hosei;
    private int _add_hoseiparam;


    //計算用_ADDアイテムリスト 材料（最大３つまで）を、0,1,2の順に入れる。
    private List<Item> _additemlist = new List<Item>();


    //補正計算用パラメータ
    private int rich_result;
    private int sweat_result;
    private int bitter_result;
    private int sour_result;

    private float _quality_revise;

    private float _rich_revise;
    private float _sweat_revise;
    private float _bitter_revise;
    private float _sour_revise;

    private float _crispy_revise;
    private float _fluffy_revise;
    private float _smooth_revise;
    private float _hardness_revise;

    private float _powdery_revise;
    private float _oily_revise;
    private float _watery_revise;

    public int _getExp;

    private int mstatus;

    private float _tempature_param;
    //private float _well_done;
    private float _best_well_done;
    private float _well_done_kyori;
    private float _well_done_kyori_noabs;
    private float _well_done_kyori_hosei;

    private float hikari_okashilv_hosei;
    private float hikari_okashilv_paramup;
    private bool hikari_make_flag;

    // Use this for initialization
    void Start() {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //魔法エフェクトの計算データベース
        itemCardEffect_database = ItemCardEffectDataBase.Instance.GetComponent<ItemCardEffectDataBase>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //調合用メソッドの取得
        Combinationmain = CombinationMain.Instance.GetComponent<CombinationMain>();

        //スロット名前変換用オブジェクトの取得
        slotchangename = GameObject.FindWithTag("SlotChangeName").gameObject.GetComponent<SlotChangeName>();

        //バフ効果計算メソッドの取得
        bufpower_keisan = Buf_Power_Keisan.Instance.GetComponent<Buf_Power_Keisan>();        

        //トッピングスロットの配列
        _basetp = new string[database.items[0].toppingtype.Length];
        _addtp = new string[database.items[0].toppingtype.Length];
        _temptp = new string[database.items[0].toppingtype.Length];
        _addkoyutp = new string[database.items[0].koyu_toppingtype.Length];
        _slotHyouji1 = new string[database.items[0].toppingtype.Length];
        _baseMS = new string[database.items[0].item_MagicSlot.Length];
        _baseMSvalue = new int[database.items[0].item_MagicSlotValue.Length];

        //アイテムデータベースの味パラムを初期化。初期化は、ゲーム起動時の一回のみ。
        ResetDefaultTasteParam();

    }

    // Update is called once per frame
    void Update() {

        if(canvas == null)
        {
            //キャンバスの読み込み
            canvas = GameObject.FindWithTag("Canvas");
        }
    }

    //
    //アイテムデータベースの味パラムを初期化。初期化は、ゲーム起動時の一回のみ。
    //
    public void ResetDefaultTasteParam()
    {
        for (DBcount = 0; DBcount < databaseCompo.compoitems.Count; DBcount++)
        {
            if (databaseCompo.compoitems[DBcount].cmpitem_Name != "") //名前が空白の場合は無視する
            {
                if (databaseCompo.compoitems[DBcount].DefaultKeisan != 0) //デフォルト計算に設定してないやつも無視する
                {
                    //パラメータを取得
                    itemNum = 0;
                    while (itemNum < database.items.Count)
                    {
                        if (databaseCompo.compoitems[DBcount].cmpitem_Name == database.items[itemNum].itemName)
                        {
                            result_item = itemNum;
                            break;
                        }
                        itemNum++;
                    }

                    if (itemNum >= database.items.Count) //なかった場合は、次を見る。
                    {

                    }
                    else
                    {
                        //コンポ調合データベースのIDを代入
                        result_compID = DBcount;

                        Topping_Compound_Method(99);
                    }
                }
            }
        }
    }

    //決定アイテムなどのパラメータを取得
    void SetParamInit()
    {
        //プレイヤーアイテム表示用コントローラーの取得
        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        //レシピリストコントローラーの取得
        recipilistController_obj = canvas.transform.Find("RecipiList_ScrollView").gameObject;
        recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

        //分岐を取得
        Comp_method_bunki = exp_Controller.Comp_method_bunki;

        if (Comp_method_bunki == 0) //オリジナル調合の場合
        {
            //オリジナル調合の設定
            if (GameMgr.Extreme_On != true)
            {
                //**重要** 
                //kettei_itemは、プレイヤーリストのリスト番号が入っている。店売り 0, 1, 2, 3... , オリジナルリスト 0, 1, 2...といった具合。
                //店売りの場合は、実質アイテムIDと数字は一緒。
                //toggle_typeは、店売り(=0)か、オリジナルアイテム(=1)の判定。

                kettei_item1 = GameMgr.Final_list_itemID1;
                kettei_item2 = GameMgr.Final_list_itemID2;
                kettei_item3 = GameMgr.Final_list_itemID3;

                toggle_type1 = GameMgr.Final_toggle_Type1;
                toggle_type2 = GameMgr.Final_toggle_Type2;
                toggle_type3 = GameMgr.Final_toggle_Type3;

                final_kette_kosu1 = GameMgr.Final_kettei_kosu1;
                final_kette_kosu2 = GameMgr.Final_kettei_kosu2;
                final_kette_kosu3 = GameMgr.Final_kettei_kosu3;

                _before_itemtype_Sub = "";

                //Debug.Log("pitemlistController.final_kettei_kosu1: " + final_kette_kosu1);
                //Debug.Log("pitemlistController.final_kettei_kosu2: " + final_kette_kosu2);
            }
            else //仕上げで新しく閃く場合
            {
                kettei_item1 = GameMgr.Final_list_baseitemID;
                kettei_item2 = GameMgr.Final_list_itemID1;
                kettei_item3 = GameMgr.Final_list_itemID2;

                toggle_type1 = GameMgr.Final_toggle_baseType;
                toggle_type2 = GameMgr.Final_toggle_Type1;
                toggle_type3 = GameMgr.Final_toggle_Type2;

                final_kette_kosu1 = GameMgr.Final_kettei_basekosu;
                final_kette_kosu2 = GameMgr.Final_kettei_kosu1;
                final_kette_kosu3 = GameMgr.Final_kettei_kosu2;

                if (toggle_type1 == 0)
                {
                    _before_itemtype_Sub = database.items[kettei_item1].itemType_sub.ToString();
                }
                else if (toggle_type1 == 1)
                {
                    _before_itemtype_Sub = pitemlist.player_originalitemlist[kettei_item1].itemType_sub.ToString();
                }
                else if (toggle_type1 == 2)
                {
                    _before_itemtype_Sub = pitemlist.player_extremepanel_itemlist[kettei_item1].itemType_sub.ToString();
                }
            }

            /*Debug.Log("pitemlistController.kettei_item1: " + kettei_item1);
            Debug.Log("pitemlistController.kettei_item2: " + kettei_item2);
            Debug.Log("pitemlistController._toggle_type1: " + toggle_type1);
            Debug.Log("pitemlistController._toggle_type2: " + toggle_type2);
            Debug.Log("pitemlistController.final_kettei_kosu1: " + final_kette_kosu1);
            Debug.Log("pitemlistController.final_kettei_kosu2: " + final_kette_kosu2);*/

            //セット数　updowncounterの値をもとに設定してる。現在は、セット数は選択できないようにしているので、1に固定
            //final_select_kaisu = exp_Controller.set_kaisu;
            final_select_kaisu = 1;

            //パラメータを取得
            result_item = GameMgr.Final_result_itemID1;

            //コンポ調合データベースのIDを代入
            result_compID = GameMgr.Final_result_compID;
        }

        if (Comp_method_bunki == 2) //レシピ調合の場合
        {
            //レシピの場合。使うアイテムを自動的に選択する。
            //今のところ、店売りアイテムのみでしか、レシピの材料にならないので、以下の定め方にしている。
            //もし、オリジナルアイテムから使う場合は、toggle_typeなどの判定がちゃんと必要。

            kettei_item1 = GameMgr.Final_list_itemID1;
            kettei_item2 = GameMgr.Final_list_itemID2;
            kettei_item3 = GameMgr.Final_list_itemID3;

            toggle_type1 = 0;
            toggle_type2 = 0;
            toggle_type3 = 0;

            final_kette_kosu1 = GameMgr.Final_kettei_kosu1; //一回あたりの必要個数
            final_kette_kosu2 = GameMgr.Final_kettei_kosu2;
            final_kette_kosu3 = GameMgr.Final_kettei_kosu3;

            final_select_kaisu = GameMgr.Final_setCount;

            if (final_kette_kosu2 == 9999) //2個目が空の場合、トッピングは一個のみ。
            {
                kettei_item2 = 9999;
                kettei_item3 = 9999;
            }

            if (final_kette_kosu3 == 9999) //3個目が空の場合、トッピングは二個のみ。
            {
                kettei_item3 = 9999;
            }

            //パラメータを取得
            result_item = GameMgr.Final_result_itemID1;

            //コンポ調合データベースのIDを代入
            result_compID = GameMgr.Final_result_compID;
        }

        if (Comp_method_bunki == 3) //トッピング調合の場合
        {
            //プレイヤーリストコントローラーで更新した変数を、こっちでも一度代入
            kettei_item1 = GameMgr.Final_list_itemID1;
            kettei_item2 = GameMgr.Final_list_itemID2;
            kettei_item3 = GameMgr.Final_list_itemID3;
            base_kettei_item = GameMgr.Final_list_baseitemID;

            toggle_type1 = GameMgr.Final_toggle_Type1;
            toggle_type2 = GameMgr.Final_toggle_Type2;
            toggle_type3 = GameMgr.Final_toggle_Type3;
            base_toggle_type = GameMgr.Final_toggle_baseType;


            if (GameMgr.Final_list_itemID2 == 9999) //2個目が空の場合、トッピングは一個のみ。
            {
                kettei_item2 = 9999;
                kettei_item3 = 9999;
            }

            if (GameMgr.Final_list_itemID3 == 9999) //3個目が空の場合、トッピングは二個のみ。
            {
                kettei_item3 = 9999;
            }

            base_kosu = 1;
            final_kette_kosu1 = GameMgr.Final_kettei_kosu1;
            final_kette_kosu2 = GameMgr.Final_kettei_kosu2;
            final_kette_kosu3 = GameMgr.Final_kettei_kosu3;

            //オリジナル・トッピングは、現在のところ、1セットのみの対応
            final_select_kaisu = 1;
        }

        if (Comp_method_bunki == 20 || Comp_method_bunki == 22) //魔法調合の場合　アイテムDBに、あえて空のアイテムデータを用意し、それを計算する 他、処理はオリジナルと一緒
        {
            if (Comp_method_bunki == 20)
            {                
                kettei_item1 = GameMgr.Final_list_itemID1;
                kettei_item2 = database.SearchItemIDString("magic_comp_setting");
                kettei_item3 = 9999;

                toggle_type1 = GameMgr.Final_toggle_Type1;
                toggle_type2 = 0;
                toggle_type3 = 0;

                final_kette_kosu1 = GameMgr.Final_kettei_kosu1;
                final_kette_kosu2 = 1;
                final_kette_kosu3 = 0;
            }
            else if (Comp_method_bunki == 22) //こっちはトッピング調合として扱うため、一個ずれる
            {
                base_kettei_item = GameMgr.Final_list_itemID1;
                kettei_item1 = database.SearchItemIDString("magic_comp_setting");
                kettei_item2 = 9999;
                kettei_item3 = 9999;

                base_toggle_type = GameMgr.Final_toggle_Type1;
                toggle_type1 = 0;
                toggle_type2 = 0;
                toggle_type3 = 0;
                

                base_kosu = 1;
                final_kette_kosu1 = GameMgr.Final_kettei_kosu1;
                final_kette_kosu2 = 1;
                final_kette_kosu3 = 0;
            }

            _before_itemtype_Sub = "";

            //Debug.Log("pitemlistController.final_kettei_kosu1: " + final_kette_kosu1);
            //Debug.Log("pitemlistController.final_kettei_kosu2: " + final_kette_kosu2);


            /*Debug.Log("pitemlistController.kettei_item1: " + kettei_item1);
            Debug.Log("pitemlistController.kettei_item2: " + kettei_item2);
            Debug.Log("pitemlistController._toggle_type1: " + toggle_type1);
            Debug.Log("pitemlistController._toggle_type2: " + toggle_type2);
            Debug.Log("pitemlistController.final_kettei_kosu1: " + final_kette_kosu1);
            Debug.Log("pitemlistController.final_kettei_kosu2: " + final_kette_kosu2);*/

            //セット数　updowncounterの値をもとに設定してる。現在は、セット数は選択できないようにしているので、1に固定
            //final_select_kaisu = exp_Controller.set_kaisu;
            final_select_kaisu = 1;

            //パラメータを取得
            result_item = GameMgr.Final_result_itemID1;
            //Debug.Log("生成アイテム: " + database.items[result_item].itemName);

            //コンポ調合データベースのIDを代入
            result_compID = GameMgr.Final_result_compID;
        }

        //**ここまで**
    }

    //ヒカリがお菓子作る場合の初期化
    void SetParamHikariMakeInit()
    {
        //プレイヤーアイテム表示用コントローラーの取得
        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        //レシピリストコントローラーの取得
        recipilistController_obj = canvas.transform.Find("RecipiList_ScrollView").gameObject;
        recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

        //分岐を取得
        Comp_method_bunki = 0;

        if (Comp_method_bunki == 0) //オリジナル調合の場合
        {
            //オリジナル調合の設定

            //**重要** 
            //kettei_itemは、プレイヤーリストのリスト番号が入っている。店売り 0, 1, 2, 3... , オリジナルリスト 0, 1, 2...といった具合。
            //店売りの場合は、実質アイテムIDと数字は一緒。
            //toggle_typeは、店売り(=0)か、オリジナルアイテム(=1)の判定。

            kettei_item1 = GameMgr.hikari_kettei_item[0];
            kettei_item2 = GameMgr.hikari_kettei_item[1];
            kettei_item3 = GameMgr.hikari_kettei_item[2];

            kettei_originalitemID1 = GameMgr.hikari_kettei_originalID[0]; //オリジナルアイテム（お菓子パネルアイテム）の固有ID　タイムスタンプ
            kettei_originalitemID2 = GameMgr.hikari_kettei_originalID[1];
            kettei_originalitemID3 = GameMgr.hikari_kettei_originalID[2];

            toggle_type1 = GameMgr.hikari_kettei_toggleType[0];
            toggle_type2 = GameMgr.hikari_kettei_toggleType[1];
            toggle_type3 = GameMgr.hikari_kettei_toggleType[2];

            final_kette_kosu1 = GameMgr.hikari_kettei_kosu[0];
            final_kette_kosu2 = GameMgr.hikari_kettei_kosu[1];
            final_kette_kosu3 = GameMgr.hikari_kettei_kosu[2];

            _before_itemtype_Sub = "";

            //オリジナルアイテムかお菓子パネルのリストを選択していたら、アイテムの固有IDをもとに、配列番号を再設定。
            if (toggle_type1 == 1 || toggle_type1 == 2)
            {
                kettei_item1 = pitemlist.ReturnOriginalKoyuIDtoItemID(kettei_originalitemID1);
            }
            if (toggle_type2 == 1 || toggle_type2 == 2)
            {
                kettei_item2 = pitemlist.ReturnOriginalKoyuIDtoItemID(kettei_originalitemID2);
            }
            if (kettei_item3 != 9999)
            {
                if (toggle_type3 == 1 || toggle_type3 == 2)
                {
                    kettei_item3 = pitemlist.ReturnOriginalKoyuIDtoItemID(kettei_originalitemID3);
                }
            }

            //Debug.Log("pitemlistController.final_kettei_kosu1: " + final_kette_kosu1);
            //Debug.Log("pitemlistController.final_kettei_kosu2: " + final_kette_kosu2);

            //セット数
            final_select_kaisu = 1;

            //パラメータを取得
            result_item = GameMgr.hikari_make_okashiID;

            //コンポ調合データベースのIDを代入
            result_compID = GameMgr.hikari_make_okashi_compID;

            exp_Controller.result_ok = true; //オリジナル調合扱い
            exp_Controller.DoubleItemCreated = GameMgr.hikari_make_doubleItemCreated;
        }
    }


    //ゲーム最初に、アイテムデータベースの味パラメータを、コンポDBから計算して初期化
    void SetParamDatabaseInit()
    {
        Comp_method_bunki = 2;

        if (databaseCompo.compoitems[result_compID].cmpitemID >= 0 && databaseCompo.compoitems[result_compID].cmpitemID < 10000)
        {
            i = 0;
            while (i < database.items.Count)
            {
                if (databaseCompo.compoitems[result_compID].cmpitemID_1 == database.items[i].itemName)
                {
                    kettei_item1 = i;
                    break;
                }
                i++;
            }

            i = 0;
            while (i < database.items.Count)
            {
                if (databaseCompo.compoitems[result_compID].cmpitemID_2 == database.items[i].itemName)
                {
                    kettei_item2 = i;
                    break;
                }
                i++;
            }

            i = 0;
            while (i < database.items.Count)
            {
                if (databaseCompo.compoitems[result_compID].cmpitemID_3 == database.items[i].itemName)
                {
                    kettei_item3 = i;
                    break;
                }
                i++;
            }

            final_kette_kosu1 = databaseCompo.compoitems[result_compID].cmpitem_kosu1;
            final_kette_kosu2 = databaseCompo.compoitems[result_compID].cmpitem_kosu2;
            final_kette_kosu3 = databaseCompo.compoitems[result_compID].cmpitem_kosu3;

            if (final_kette_kosu2 == 9999) //2個目が空の場合、トッピングは一個のみ。
            {
                kettei_item2 = 9999;
                kettei_item3 = 9999;
            }

            if (final_kette_kosu3 == 9999) //3個目が空の場合、トッピングは二個のみ。
            {
                kettei_item3 = 9999;
            }
        }
        //魔法調合DBから初期値決める場合
        else if (databaseCompo.compoitems[result_compID].cmpitemID >= 10000)
        {
            if (database.SearchItemIDString(databaseCompo.compoitems[result_compID].cmpitemID_1) != 9999)
            {
                kettei_item1 = database.SearchItemIDString(databaseCompo.compoitems[result_compID].cmpitemID_1);
            }
            kettei_item2 = database.SearchItemIDString("magic_comp_setting");
            kettei_item3 = 9999;

            final_kette_kosu1 = databaseCompo.compoitems[result_compID].cmpitem_kosu1;
            final_kette_kosu2 = 1;
            final_kette_kosu3 = 0;
        }

        toggle_type1 = 0;
        toggle_type2 = 0;
        toggle_type3 = 0;

        //デバッグ用
        if(databaseCompo.compoitems[result_compID].cmpitem_Name == "bugget")
        {
            //DebugLogKetteiItem();
        }


        //**ここまで**
    }

    void DebugLogKetteiItem()
    {
        Debug.Log("###");
        Debug.Log("調合ネーム: " + databaseCompo.compoitems[result_compID].cmpitem_Name);
        Debug.Log("kettei_item1: " + kettei_item1 + " Name: " + database.items[kettei_item1].itemName);
        Debug.Log("kettei_item2: " + kettei_item2);
        if (kettei_item2 != 9999)
        {
            Debug.Log("kettei_item2 Name: " + database.items[kettei_item2].itemName);
        }
        Debug.Log("kettei_item3: " + kettei_item3);        
        if (kettei_item3 != 9999)
        {
            Debug.Log("kettei_item3 Name: " + database.items[kettei_item3].itemName);
        }
        Debug.Log("_toggle_type1: " + toggle_type1);
        Debug.Log("_toggle_type2: " + toggle_type2);
        Debug.Log("_toggle_type3: " + toggle_type3);
        Debug.Log("final_kettei_kosu1: " + final_kette_kosu1);
        Debug.Log("final_kettei_kosu2: " + final_kette_kosu2);
        Debug.Log("final_kettei_kosu3: " + final_kette_kosu3);
    }



    //           //
    //  合成処理 //
    //           //

    //Exp_Controllerからのみ読み出し
    public void Topping_Compound_Method(int _mstatus)
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //トッピングスロットの配列初期化
        _basetp = new string[database.items[0].toppingtype.Length];
        _addtp = new string[database.items[0].toppingtype.Length];
        _temptp = new string[database.items[0].toppingtype.Length];
        _addkoyutp = new string[database.items[0].koyu_toppingtype.Length];
        _baseMS = new string[database.items[0].item_MagicSlot.Length];
        _baseMSvalue = new int[database.items[0].item_MagicSlotValue.Length];

        mstatus = _mstatus;
        hikari_make_flag = false;

        //ベースアイテムのパラメータを取得する。その後、各トッピングアイテムの値を取得し、加算する。

        if (_mstatus == 0)
        {
            //パラメータを取得
            SetParamInit();
        }
        else if (_mstatus == 2) //ヒカリがお菓子を作る場合 予測アイテムリストへ処理。　予測で作ったアイテムを、後で受け取るときに、個数だけ計算してから、取得処理を行う。
        {
            //パラメータを取得
            SetParamInit();
        }
        else if (_mstatus == 99)
        {
            //パラメータを取得。アイテムデータベースを、ここで計算して初期化する。ゲーム開始時のみ使用。
            SetParamDatabaseInit();
            //SetParamKosuHosei();
        }


        //ベースアイテム　タイプを見て、プレイヤリストアイテムかオリジナルアイテムかを識別する。
        if (Comp_method_bunki == 0 || Comp_method_bunki == 2 || Comp_method_bunki == 20) //新規にアイテムを作成する場合 or レシピ調合の場合。空のパラメータに、材料のパラメータを総計していく。
        {
            _id = result_item;

            if (_mstatus == 99) //ゲーム開始時のみ使用。
            {
                if (database.items[_id].itemComp_Hosei == 0) //アイテム自体が持っている値を加算しない場合
                {
                    Setup_Param02();
                }
                else //アイテム自体が持っている値を加算する補正の処理。データベース最初の初期化のときのみ。
                {
                    Setup_Param01(0);
                }
            }
            else
            {
                if (database.items[_id].itemComp_Hosei == 0) //アイテム自体が持っている値を加算しない場合
                {
                    Setup_Param02();
                }
                else //アイテム自体が持っている値を加算する補正の処理。
                {
                    Setup_Param03();
                }
            }

            for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _basetp[i] = database.items[_id].toppingtype[i].ToString();
            }

            for (i = 0; i < database.items[_id].item_MagicSlot.Length; i++)
            {
                _baseMS[i] = database.items[_id].item_MagicSlot[i].ToString();
                _baseMSvalue[i] = database.items[_id].item_MagicSlotValue[i];
            }

            /*if (_mstatus == 99)
            {
                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _basetp[i] = "Non";
                }
            }
            else
            {
                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _basetp[i] = database.items[_id].toppingtype[i].ToString();
            }
            }*/


        }
        else if (Comp_method_bunki == 1 || Comp_method_bunki == 3 || Comp_method_bunki == 22) //生地合成、もしくはトッピング調合の場合。
            //もしくは、魔法調合でCompNoが入ってて、新規作成されない場合。
            //一個目に選んだアイテムをベースに、リザルトアイテムにする。
        {
            switch (base_toggle_type)
            {
                case 0: //プレイヤーアイテムリストから選択している。

                    _id = base_kettei_item;

                    Setup_Param01(1);

                    _base_extreme_kaisu--;

                    for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                    {
                        _basetp[i] = database.items[_id].toppingtype[i].ToString();
                    }

                    for (i = 0; i < database.items[_id].item_MagicSlot.Length; i++)
                    {
                        _baseMS[i] = database.items[_id].item_MagicSlot[i].ToString();
                        _baseMSvalue[i] = database.items[_id].item_MagicSlotValue[i];
                    }

                    break;

                case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                    //さらに、オリジナルのプレイヤーアイテムリストの番号を参照する。

                    _id = base_kettei_item;

                    //各パラメータを取得
                    _baseID = pitemlist.player_originalitemlist[_id].itemID;
                    _basename = pitemlist.player_originalitemlist[_id].itemName;
                    _basehp = pitemlist.player_originalitemlist[_id].itemHP;
                    _baseday = pitemlist.player_originalitemlist[_id].item_day;
                    _basequality = pitemlist.player_originalitemlist[_id].Quality;
                    _baseexp = pitemlist.player_originalitemlist[_id].Exp;
                    _baseprobability = pitemlist.player_originalitemlist[_id].Ex_Probability;
                    _baserich = pitemlist.player_originalitemlist[_id].Rich;
                    _basesweat = pitemlist.player_originalitemlist[_id].Sweat;
                    _basebitter = pitemlist.player_originalitemlist[_id].Bitter;
                    _basesour = pitemlist.player_originalitemlist[_id].Sour;
                    _basecrispy = pitemlist.player_originalitemlist[_id].Crispy;
                    _basefluffy = pitemlist.player_originalitemlist[_id].Fluffy;
                    _basesmooth = pitemlist.player_originalitemlist[_id].Smooth;
                    _basehardness = pitemlist.player_originalitemlist[_id].Hardness;
                    _basejiggly = pitemlist.player_originalitemlist[_id].Jiggly;
                    _basechewy = pitemlist.player_originalitemlist[_id].Chewy;
                    _basejuice = pitemlist.player_originalitemlist[_id].Juice;
                    _basepowdery = pitemlist.player_originalitemlist[_id].Powdery;
                    _baseoily = pitemlist.player_originalitemlist[_id].Oily;
                    _basewatery = pitemlist.player_originalitemlist[_id].Watery;
                    _basebeauty = pitemlist.player_originalitemlist[_id].Beauty;
                    _basetea_flavor = pitemlist.player_originalitemlist[_id].Tea_Flavor;
                    _basesp_wind = pitemlist.player_originalitemlist[_id].SP_wind;
                    _basesp_score2 = pitemlist.player_originalitemlist[_id].SP_Score2;
                    _basesp_score3 = pitemlist.player_originalitemlist[_id].SP_Score3;
                    _basesp_score4 = pitemlist.player_originalitemlist[_id].SP_Score4;
                    _basesp_score5 = pitemlist.player_originalitemlist[_id].SP_Score5;
                    _basesp_score6 = pitemlist.player_originalitemlist[_id].SP_Score6;
                    _basesp_score7 = pitemlist.player_originalitemlist[_id].SP_Score7;
                    _basesp_score8 = pitemlist.player_originalitemlist[_id].SP_Score8;
                    _basesp_score9 = pitemlist.player_originalitemlist[_id].SP_Score9;
                    _basesp_score10 = pitemlist.player_originalitemlist[_id].SP_Score10;
                    _base_bestwelldone = pitemlist.player_originalitemlist[_id].Best_Welldone;
                    _basegirl1_like = pitemlist.player_originalitemlist[_id].girl1_itemLike;
                    _basecost = pitemlist.player_originalitemlist[_id].cost_price;
                    _basesell = pitemlist.player_originalitemlist[_id].sell_price;
                    _base_itemType = pitemlist.player_originalitemlist[_id].itemType.ToString();
                    _base_itemType_sub = pitemlist.player_originalitemlist[_id].itemType_sub.ToString();
                    _base_itemType_subB = pitemlist.player_originalitemlist[_id].itemType_subB.ToString();
                    _base_extreme_kaisu = pitemlist.player_originalitemlist[_id].ExtremeKaisu;
                    _base_item_hyouji = pitemlist.player_originalitemlist[_id].item_Hyouji;
                    _base_itemdesc = pitemlist.player_originalitemlist[_id].itemDesc;
                    _baseattri1 = pitemlist.player_originalitemlist[_id].Attribute1;
                    _baseattri2 = pitemlist.player_originalitemlist[_id].Attribute2;
                    _baseattri3 = pitemlist.player_originalitemlist[_id].Attribute3;
                    _basemagic = pitemlist.player_originalitemlist[_id].Magic;

                    _base_extreme_kaisu--;

                    for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                    {
                        _basetp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
                    }

                    for (i = 0; i < database.items[_id].item_MagicSlot.Length; i++)
                    {
                        _baseMS[i] = pitemlist.player_originalitemlist[_id].item_MagicSlot[i].ToString();
                        _baseMSvalue[i] = pitemlist.player_originalitemlist[_id].item_MagicSlotValue[i];
                    }

                    break;

                case 2: //お菓子パネルアイテムリストから選択している場合

                    //さらに、オリジナルのプレイヤーアイテムリストの番号を参照する。

                    _id = base_kettei_item;
                    Debug.Log("base_kettei_item: " + base_kettei_item);

                    //各パラメータを取得
                    _baseID = pitemlist.player_extremepanel_itemlist[_id].itemID;
                    _basename = pitemlist.player_extremepanel_itemlist[_id].itemName;
                    _basehp = pitemlist.player_extremepanel_itemlist[_id].itemHP;
                    _baseday = pitemlist.player_extremepanel_itemlist[_id].item_day;
                    _basequality = pitemlist.player_extremepanel_itemlist[_id].Quality;
                    _baseexp = pitemlist.player_extremepanel_itemlist[_id].Exp;
                    _baseprobability = pitemlist.player_extremepanel_itemlist[_id].Ex_Probability;
                    _baserich = pitemlist.player_extremepanel_itemlist[_id].Rich;
                    _basesweat = pitemlist.player_extremepanel_itemlist[_id].Sweat;
                    _basebitter = pitemlist.player_extremepanel_itemlist[_id].Bitter;
                    _basesour = pitemlist.player_extremepanel_itemlist[_id].Sour;
                    _basecrispy = pitemlist.player_extremepanel_itemlist[_id].Crispy;
                    _basefluffy = pitemlist.player_extremepanel_itemlist[_id].Fluffy;
                    _basesmooth = pitemlist.player_extremepanel_itemlist[_id].Smooth;
                    _basehardness = pitemlist.player_extremepanel_itemlist[_id].Hardness;
                    _basejiggly = pitemlist.player_extremepanel_itemlist[_id].Jiggly;
                    _basechewy = pitemlist.player_extremepanel_itemlist[_id].Chewy;
                    _basejuice = pitemlist.player_extremepanel_itemlist[_id].Juice;
                    _basepowdery = pitemlist.player_extremepanel_itemlist[_id].Powdery;
                    _baseoily = pitemlist.player_extremepanel_itemlist[_id].Oily;
                    _basewatery = pitemlist.player_extremepanel_itemlist[_id].Watery;
                    _basebeauty = pitemlist.player_extremepanel_itemlist[_id].Beauty;
                    _basetea_flavor = pitemlist.player_extremepanel_itemlist[_id].Tea_Flavor;
                    _basesp_wind = pitemlist.player_extremepanel_itemlist[_id].SP_wind;
                    _basesp_score2 = pitemlist.player_extremepanel_itemlist[_id].SP_Score2;
                    _basesp_score3 = pitemlist.player_extremepanel_itemlist[_id].SP_Score3;
                    _basesp_score4 = pitemlist.player_extremepanel_itemlist[_id].SP_Score4;
                    _basesp_score5 = pitemlist.player_extremepanel_itemlist[_id].SP_Score5;
                    _basesp_score6 = pitemlist.player_extremepanel_itemlist[_id].SP_Score6;
                    _basesp_score7 = pitemlist.player_extremepanel_itemlist[_id].SP_Score7;
                    _basesp_score8 = pitemlist.player_extremepanel_itemlist[_id].SP_Score8;
                    _basesp_score9 = pitemlist.player_extremepanel_itemlist[_id].SP_Score9;
                    _basesp_score10 = pitemlist.player_extremepanel_itemlist[_id].SP_Score10;
                    _base_bestwelldone = pitemlist.player_extremepanel_itemlist[_id].Best_Welldone;
                    _basegirl1_like = pitemlist.player_extremepanel_itemlist[_id].girl1_itemLike;
                    _basecost = pitemlist.player_extremepanel_itemlist[_id].cost_price;
                    _basesell = pitemlist.player_extremepanel_itemlist[_id].sell_price;
                    _base_itemType = pitemlist.player_extremepanel_itemlist[_id].itemType.ToString();
                    _base_itemType_sub = pitemlist.player_extremepanel_itemlist[_id].itemType_sub.ToString();
                    _base_itemType_subB = pitemlist.player_extremepanel_itemlist[_id].itemType_subB.ToString();
                    _base_extreme_kaisu = pitemlist.player_extremepanel_itemlist[_id].ExtremeKaisu;
                    _base_item_hyouji = pitemlist.player_extremepanel_itemlist[_id].item_Hyouji;
                    _base_itemdesc = pitemlist.player_extremepanel_itemlist[_id].itemDesc;
                    _baseattri1 = pitemlist.player_extremepanel_itemlist[_id].Attribute1;
                    _baseattri2 = pitemlist.player_extremepanel_itemlist[_id].Attribute2;
                    _baseattri3 = pitemlist.player_extremepanel_itemlist[_id].Attribute3;
                    _basemagic = pitemlist.player_extremepanel_itemlist[_id].Magic;

                    _base_extreme_kaisu--;

                    for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                    {
                        _basetp[i] = pitemlist.player_extremepanel_itemlist[_id].toppingtype[i].ToString();
                    }

                    for (i = 0; i < database.items[_id].item_MagicSlot.Length; i++)
                    {
                        _baseMS[i] = pitemlist.player_extremepanel_itemlist[_id].item_MagicSlot[i].ToString();
                        _baseMSvalue[i] = pitemlist.player_extremepanel_itemlist[_id].item_MagicSlotValue[i];
                    }

                    break;

                default:
                    break;
            }
        }

        if (databaseCompo.compoitems[result_compID].KeisanMethod != "Non" && databaseCompo.compoitems[result_compID].KeisanMethod != "Use")
        {
            Kosu_keisanmethod = true; //CompoDBで、個数指定したいアイテム名の名前かタイプ(Sub,SubB)で指定。
                                      //これで指定すると、指定アイテム一個分のパラメータで、リザルトはfinal_kettei_kosu分になる。
                                      //魔法調合の場合、魔法の「KetteiKosu」指定（その場合CompoDBではNon表記）か、CompoDBのKeisanMethod指定のどっちかを使えばOK。被っても、たぶん大丈夫。
                                      //魔法の「KetteiKosu」指定の場合、下のほうで処理している。
                                      //ただし、「KetteiKosu」指定のみだと、元アイテムを入れた個数が、加算処理に反映されるので、(AddParamMethod()で、対応させてない）
                                      //出来上がるアイテムが、店売りアイテムの場合のみに限定すること。ルミベリーなどは大丈夫ということ。
        }
        else
        {
            Kosu_keisanmethod = false;
        }


        AddParamMethod(); //決定されたベースアイテムに、選んだアイテムの値を加算する処理



        //全て完了。最終的に完成された_baseのパラムを基に、新しくアイテムを生成し、ベースとトッピングアイテムは削除する。

        //ここまでで、生成されるアイテムの予測が出来る。

        if (_mstatus != 99)
        {
            Debug_TastePanel();
        }

        //
        /* 結果と制作個数 */
        //
        //以下、実際にアイテムリスト削除と、プレイヤーアイテムへの所持追加処理
        if (_mstatus == 0)
        {

            //最終的に生成されるアイテムの個数を決定
            ResultKosuKeisan(GameMgr.compound_select, result_compID, final_select_kaisu, 
                kettei_item1, kettei_item2, kettei_item3, toggle_type1, toggle_type2, toggle_type3, final_kette_kosu1, final_kette_kosu2, final_kette_kosu3);            

            // アイテムリストの削除処理 //
            Delete_playerItemList(0);

            //アイテム取得チェック
            GetItemCheck();

        }
        else if (_mstatus == 2) //ヒカリのアイテムの予測処理。予測の場合、アイテムの追加処理はいらない。
        {
            //新しく作ったアイテムを予測表示用のアイテムリストに追加。
            pitemlist.addYosokuOriginalItem(_basename, _basehp, _baseday, _basequality, _baseexp, _baseprobability,
            _baserich, _basesweat, _basebitter, _basesour, _basecrispy, _basefluffy, _basesmooth, _basehardness, _basejiggly, _basechewy, _basepowdery, _baseoily, _basewatery, _basebeauty,
            _basejuice, _basetea_flavor,
            _basesp_wind, _basesp_score2, _basesp_score3, _basesp_score4, _basesp_score5, _basesp_score6, _basesp_score7, _basesp_score8, _basesp_score9, _basesp_score10,
            _basegirl1_like, _basecost, _basesell,
            _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9],
            result_kosu, _base_extreme_kaisu, _base_item_hyouji, totalkyori, _basemagic,
            _baseMS[0], _baseMS[1], _baseMS[2], _baseMS[3], _baseMS[4], _baseMS[5], _baseMS[6], _baseMS[7], _baseMS[8], _baseMS[9],
            _baseMSvalue[0], _baseMSvalue[1], _baseMSvalue[2], _baseMSvalue[3], _baseMSvalue[4], _baseMSvalue[5], _baseMSvalue[6], _baseMSvalue[7], _baseMSvalue[8], _baseMSvalue[9],
            _baseattri1, _baseattri2, _baseattri3);

            new_item = pitemlist.player_yosokuitemlist.Count - 1; //最後に追加されたアイテムが、さっき作った新規アイテムなので、そのIDを入れて置き、リザルトで表示

            //カード正式名称（ついてるスロット名も含めた名前）
            slotchangename.slotChangeName(1, new_item, "yellow", 1); //4つ目の番号は、ステータス。予測の場合は1。

            itemslotname = "";
            for (i = 0; i < _slotHyouji1.Length; i++)
            {
                _slotHyouji1[i] = slotchangename._slotHyouji[i];
                itemslotname += _slotHyouji1[i];
            }

            pitemlist.player_yosokuitemlist[new_item].item_SlotName = itemslotname;
            itemfullname = itemslotname + pitemlist.player_yosokuitemlist[new_item].itemNameHyouji;
            pitemlist.player_yosokuitemlist[new_item].item_FullName = itemfullname;
        }
        else if (_mstatus == 99) //初期化の場合
        {
            //味のパラメータのみ、上書きする。
            database.items[itemNum].Sweat = _basesweat;
            database.items[itemNum].Bitter = _basebitter;
            database.items[itemNum].Sour = _basesour;
            database.items[itemNum].Rich = _baserich;
            database.items[itemNum].Crispy = _basecrispy;
            database.items[itemNum].Fluffy = _basefluffy;
            database.items[itemNum].Smooth = _basesmooth;
            database.items[itemNum].Hardness = _basehardness;
            database.items[itemNum].Jiggly = _basejiggly;
            database.items[itemNum].Chewy = _basechewy;
            database.items[itemNum].Powdery = _basepowdery;
            database.items[itemNum].Oily = _baseoily;
            database.items[itemNum].Watery = _basewatery;
            database.items[itemNum].Beauty = _basebeauty;
            database.items[itemNum].Tea_Flavor = _basetea_flavor;
            database.items[itemNum].Juice = _basesweat + _basebitter + _basesour;
            database.items[itemNum].SP_wind = _basesp_wind;
            database.items[itemNum].SP_Score2 = _basesp_score2;
            database.items[itemNum].SP_Score3 = _basesp_score3;
            database.items[itemNum].SP_Score4 = _basesp_score4;
            database.items[itemNum].SP_Score5 = _basesp_score5;
            database.items[itemNum].SP_Score6 = _basesp_score6;
            database.items[itemNum].SP_Score7 = _basesp_score7;
            database.items[itemNum].SP_Score8 = _basesp_score8;
            database.items[itemNum].SP_Score9 = _basesp_score9;
            database.items[itemNum].SP_Score10 = _basesp_score10;
        }
    }
    

    void Setup_Param01(int _status)
    {
        //各パラメータを取得
        _baseID = database.items[_id].itemID;
        _basename = database.items[_id].itemName;
        _basehp = database.items[_id].itemHP;

        if (_status == 0)
        {
            _baseday = 0;
            _basequality = 0;
            _baseexp = 0;
        }
        else if(_status == 1)
        {
            _baseday = database.items[_id].item_day;
            _basequality = database.items[_id].Quality;
            _baseexp = database.items[_id].Exp;
        }

        _baseprobability = database.items[_id].Ex_Probability;
        _baserich = database.items[_id].Rich;
        _basesweat = database.items[_id].Sweat;
        _basebitter = database.items[_id].Bitter;
        _basesour = database.items[_id].Sour;
        _basecrispy = database.items[_id].Crispy;
        _basefluffy = database.items[_id].Fluffy;
        _basesmooth = database.items[_id].Smooth;
        _basehardness = database.items[_id].Hardness;
        _basejiggly = database.items[_id].Jiggly;
        _basechewy = database.items[_id].Chewy;
        _basejuice = database.items[_id].Juice;
        _basepowdery = database.items[_id].Powdery;
        _baseoily = database.items[_id].Oily;
        _basewatery = database.items[_id].Watery;
        _basebeauty = database.items[_id].Beauty;
        _basetea_flavor = database.items[_id].Tea_Flavor;
        _basesp_wind = database.items[_id].SP_wind;
        _basesp_score2 = database.items[_id].SP_Score2;
        _basesp_score3 = database.items[_id].SP_Score3;
        _basesp_score4 = database.items[_id].SP_Score4;
        _basesp_score5 = database.items[_id].SP_Score5;
        _basesp_score6 = database.items[_id].SP_Score6;
        _basesp_score7 = database.items[_id].SP_Score7;
        _basesp_score8 = database.items[_id].SP_Score8;
        _basesp_score9 = database.items[_id].SP_Score9;
        _basesp_score10 = database.items[_id].SP_Score10;
        _base_bestwelldone = database.items[_id].Best_Welldone;
        _basegirl1_like = database.items[_id].girl1_itemLike;
        _basecost = database.items[_id].cost_price;
        _basesell = database.items[_id].sell_price;
        _base_itemType = database.items[_id].itemType.ToString();
        _base_itemType_sub = database.items[_id].itemType_sub.ToString();
        _base_itemType_subB = database.items[_id].itemType_subB.ToString();
        _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
        _base_item_hyouji = database.items[_id].item_Hyouji;
        _base_itemdesc = database.items[_id].itemDesc;
        _baseattri1 = database.items[_id].Attribute1;
        _baseattri2 = database.items[_id].Attribute2;
        _baseattri3 = database.items[_id].Attribute3;
        _basemagic = database.items[_id].Magic;
    }

    void Setup_Param02()
    {
        //各パラメータを取得
        _baseID = database.items[_id].itemID;
        _basename = database.items[_id].itemName;
        _basehp = database.items[_id].itemHP;
        _baseday = 0;
        _basequality = 0;
        _baseexp = 0;
        _baseprobability = database.items[_id].Ex_Probability;
        _baserich = 0;
        _basesweat = 0;
        _basebitter = 0;
        _basesour = 0;
        _basecrispy = 0;
        _basefluffy = 0;
        _basesmooth = 0;
        _basehardness = 0;
        _basejiggly = 0;
        _basechewy = 0;
        _basejuice = 0;
        _basepowdery = 0;
        _baseoily = 0;
        _basewatery = 0;
        _basebeauty = database.items[_id].Beauty;
        _basetea_flavor = 0;
        _basesp_wind = database.items[_id].SP_wind;
        _basesp_score2 = database.items[_id].SP_Score2;
        _basesp_score3 = database.items[_id].SP_Score3;
        _basesp_score4 = database.items[_id].SP_Score4;
        _basesp_score5 = database.items[_id].SP_Score5;
        _basesp_score6 = database.items[_id].SP_Score6;
        _basesp_score7 = database.items[_id].SP_Score7;
        _basesp_score8 = database.items[_id].SP_Score8;
        _basesp_score9 = database.items[_id].SP_Score9;
        _basesp_score10 = database.items[_id].SP_Score10;
        _base_bestwelldone = database.items[_id].Best_Welldone;
        _basegirl1_like = database.items[_id].girl1_itemLike;
        _basecost = database.items[_id].cost_price;
        _basesell = database.items[_id].sell_price;
        _base_itemType = database.items[_id].itemType.ToString();
        _base_itemType_sub = database.items[_id].itemType_sub.ToString();
        _base_itemType_subB = database.items[_id].itemType_subB.ToString();
        _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
        _base_item_hyouji = database.items[_id].item_Hyouji;
        _base_itemdesc = database.items[_id].itemDesc;
        _baseattri1 = database.items[_id].Attribute1;
        _baseattri2 = database.items[_id].Attribute2;
        _baseattri3 = database.items[_id].Attribute3;
        _basemagic = database.items[_id].Magic;
    }

    void Setup_Param03()
    {
        //各パラメータを取得
        _baseID = database.items_gamedefault[_id].itemID;
        _basename = database.items_gamedefault[_id].itemName;
        _basehp = database.items_gamedefault[_id].itemHP;
        _baseday = 0;
        _basequality = 0;
        _baseexp = 0;
        _baseprobability = database.items_gamedefault[_id].Ex_Probability;
        _baserich = database.items_gamedefault[_id].Rich;
        _basesweat = database.items_gamedefault[_id].Sweat;
        _basebitter = database.items_gamedefault[_id].Bitter;
        _basesour = database.items_gamedefault[_id].Sour;
        _basecrispy = database.items_gamedefault[_id].Crispy;
        _basefluffy = database.items_gamedefault[_id].Fluffy;
        _basesmooth = database.items_gamedefault[_id].Smooth;
        _basehardness = database.items_gamedefault[_id].Hardness;
        _basejiggly = database.items_gamedefault[_id].Jiggly;
        _basechewy = database.items_gamedefault[_id].Chewy;
        _basejuice = database.items_gamedefault[_id].Juice;
        _basepowdery = database.items_gamedefault[_id].Powdery;
        _baseoily = database.items_gamedefault[_id].Oily;
        _basewatery = database.items_gamedefault[_id].Watery;
        _basebeauty = database.items_gamedefault[_id].Beauty;
        _basetea_flavor = database.items_gamedefault[_id].Tea_Flavor;
        _basesp_wind = database.items_gamedefault[_id].SP_wind;
        _basesp_score2 = database.items_gamedefault[_id].SP_Score2;
        _basesp_score3 = database.items_gamedefault[_id].SP_Score3;
        _basesp_score4 = database.items_gamedefault[_id].SP_Score4;
        _basesp_score5 = database.items_gamedefault[_id].SP_Score5;
        _basesp_score6 = database.items_gamedefault[_id].SP_Score6;
        _basesp_score7 = database.items_gamedefault[_id].SP_Score7;
        _basesp_score8 = database.items_gamedefault[_id].SP_Score8;
        _basesp_score9 = database.items_gamedefault[_id].SP_Score9;
        _basesp_score10 = database.items_gamedefault[_id].SP_Score10;
        _base_bestwelldone = database.items_gamedefault[_id].Best_Welldone;
        _basegirl1_like = database.items_gamedefault[_id].girl1_itemLike;
        _basecost = database.items_gamedefault[_id].cost_price;
        _basesell = database.items_gamedefault[_id].sell_price;
        _base_itemType = database.items_gamedefault[_id].itemType.ToString();
        _base_itemType_sub = database.items_gamedefault[_id].itemType_sub.ToString();
        _base_itemType_subB = database.items_gamedefault[_id].itemType_subB.ToString();
        _base_extreme_kaisu = database.items_gamedefault[_id].ExtremeKaisu;
        _base_item_hyouji = database.items_gamedefault[_id].item_Hyouji;
        _base_itemdesc = database.items_gamedefault[_id].itemDesc;
        _baseattri1 = database.items_gamedefault[_id].Attribute1;
        _baseattri2 = database.items_gamedefault[_id].Attribute2;
        _baseattri3 = database.items_gamedefault[_id].Attribute3;
        _basemagic = database.items_gamedefault[_id].Magic;
    }

    //HikariMakeStartPanelから読み出し
    public void HikariMakeGetItem(int _status)
    {
        //パラメータを取得
        result_item = GameMgr.hikari_make_okashiID;

        //コンポ調合データベースのIDを代入
        result_compID = GameMgr.hikari_make_okashi_compID;
        //result_kosu = databaseCompo.compoitems[result_compID].cmpitem_result_kosu * GameMgr.hikari_make_okashiKosu; //compoDBの回数も含む個数

        if (databaseCompo.compoitems[result_compID].KeisanMethod != "Non" && databaseCompo.compoitems[result_compID].KeisanMethod != "Use")
        {
            Kosu_keisanmethod = true;
        }
        else
        {
            Kosu_keisanmethod = false;
        }

        ResultKosuKeisan(7, result_compID, GameMgr.hikari_make_okashiKosu, GameMgr.hikari_kettei_item[0], GameMgr.hikari_kettei_item[1], GameMgr.hikari_kettei_item[2],
                    GameMgr.hikari_kettei_toggleType[0], GameMgr.hikari_kettei_toggleType[1], GameMgr.hikari_kettei_toggleType[2], 
                    GameMgr.hikari_kettei_kosu[0], GameMgr.hikari_kettei_kosu[1], GameMgr.hikari_kettei_kosu[2]);
        
        

        hikari_make_flag = true; //ヒカリのお菓子作りの場合

        //ヒカリがお菓子を作る場合は、事前に材料削除してるので、ここでは材料削除処理は無視。

        _id = pitemlist.player_yosokuitemlist.Count-1;

        //各パラメータを取得
        _baseID = pitemlist.player_yosokuitemlist[_id].itemID;
        _basename = pitemlist.player_yosokuitemlist[_id].itemName;
        _basehp = pitemlist.player_yosokuitemlist[_id].itemHP;
        _baseday = pitemlist.player_yosokuitemlist[_id].item_day;
        _basequality = pitemlist.player_yosokuitemlist[_id].Quality;
        _baseexp = pitemlist.player_yosokuitemlist[_id].Exp;
        _baseprobability = pitemlist.player_yosokuitemlist[_id].Ex_Probability;
        _baserich = pitemlist.player_yosokuitemlist[_id].Rich;
        _basesweat = pitemlist.player_yosokuitemlist[_id].Sweat;
        _basebitter = pitemlist.player_yosokuitemlist[_id].Bitter;
        _basesour = pitemlist.player_yosokuitemlist[_id].Sour;
        _basecrispy = pitemlist.player_yosokuitemlist[_id].Crispy;
        _basefluffy = pitemlist.player_yosokuitemlist[_id].Fluffy;
        _basesmooth = pitemlist.player_yosokuitemlist[_id].Smooth;
        _basehardness = pitemlist.player_yosokuitemlist[_id].Hardness;
        _basejiggly = pitemlist.player_yosokuitemlist[_id].Jiggly;
        _basechewy = pitemlist.player_yosokuitemlist[_id].Chewy;
        _basepowdery = pitemlist.player_yosokuitemlist[_id].Powdery;
        _baseoily = pitemlist.player_yosokuitemlist[_id].Oily;
        _basewatery = pitemlist.player_yosokuitemlist[_id].Watery;
        _basebeauty = pitemlist.player_yosokuitemlist[_id].Beauty;
        _basetea_flavor = pitemlist.player_yosokuitemlist[_id].Tea_Flavor;
        _basesp_wind = pitemlist.player_yosokuitemlist[_id].SP_wind;
        _basesp_score2 = pitemlist.player_yosokuitemlist[_id].SP_Score2;
        _basesp_score3 = pitemlist.player_yosokuitemlist[_id].SP_Score3;
        _basesp_score4 = pitemlist.player_yosokuitemlist[_id].SP_Score4;
        _basesp_score5 = pitemlist.player_yosokuitemlist[_id].SP_Score5;
        _basesp_score6 = pitemlist.player_yosokuitemlist[_id].SP_Score6;
        _basesp_score7 = pitemlist.player_yosokuitemlist[_id].SP_Score7;
        _basesp_score8 = pitemlist.player_yosokuitemlist[_id].SP_Score8;
        _basesp_score9 = pitemlist.player_yosokuitemlist[_id].SP_Score9;
        _basesp_score10 = pitemlist.player_yosokuitemlist[_id].SP_Score10;
        _base_bestwelldone = pitemlist.player_yosokuitemlist[_id].Best_Welldone;
        _basegirl1_like = pitemlist.player_yosokuitemlist[_id].girl1_itemLike;
        _basecost = pitemlist.player_yosokuitemlist[_id].cost_price;
        _basesell = pitemlist.player_yosokuitemlist[_id].sell_price;
        _base_itemType = pitemlist.player_yosokuitemlist[_id].itemType.ToString();
        _base_itemType_sub = pitemlist.player_yosokuitemlist[_id].itemType_sub.ToString();
        _base_itemType_subB = pitemlist.player_yosokuitemlist[_id].itemType_subB.ToString();
        _base_extreme_kaisu = pitemlist.player_yosokuitemlist[_id].ExtremeKaisu;
        _base_item_hyouji = pitemlist.player_yosokuitemlist[_id].item_Hyouji;
        _base_itemdesc = pitemlist.player_yosokuitemlist[_id].itemDesc;
        _baseattri1 = pitemlist.player_yosokuitemlist[_id].Attribute1;
        _baseattri2 = pitemlist.player_yosokuitemlist[_id].Attribute2;
        _baseattri3 = pitemlist.player_yosokuitemlist[_id].Attribute3;
        _basemagic = pitemlist.player_yosokuitemlist[_id].Magic;

        for (i = 0; i < database.items[_id].toppingtype.Length; i++)
        {
            _basetp[i] = pitemlist.player_yosokuitemlist[_id].toppingtype[i].ToString();
        }

        for (i = 0; i < database.items[_id].item_MagicSlot.Length; i++)
        {
            _baseMS[i] = pitemlist.player_yosokuitemlist[_id].item_MagicSlot[i].ToString();
            _baseMSvalue[i] = pitemlist.player_yosokuitemlist[_id].item_MagicSlotValue[i];
        }

        if (_status == 0)
        {
            //アイテム取得処理
            GetItemMethod(0);
        }
        else if (_status == 1)
        {
            GetItemCheck();         
        }
        
    }

    void GetItemCheck()
    {
        //最初に、チェック用に一度お菓子をいれて、それが生地かどうか判定する。カードの表示は、このリストのものを使う。
        pitemlist.addCheckOriginalItem(_basename, _basehp, _baseday, _basequality, _baseexp, _baseprobability,
        _baserich, _basesweat, _basebitter, _basesour, _basecrispy, _basefluffy, _basesmooth, _basehardness, _basejiggly, 
        _basechewy, _basepowdery, _baseoily, _basewatery, _basebeauty,
        _basejuice, _basetea_flavor,
        _basesp_wind, _basesp_score2, _basesp_score3, _basesp_score4, _basesp_score5, _basesp_score6, _basesp_score7, _basesp_score8, _basesp_score9, _basesp_score10,
        _basegirl1_like, _basecost, _basesell,
        _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9],
        result_kosu, _base_extreme_kaisu, _base_item_hyouji, totalkyori, _basemagic,
        _baseMS[0], _baseMS[1], _baseMS[2], _baseMS[3], _baseMS[4], _baseMS[5], _baseMS[6], _baseMS[7], _baseMS[8], _baseMS[9],
        _baseMSvalue[0], _baseMSvalue[1], _baseMSvalue[2], _baseMSvalue[3], _baseMSvalue[4], _baseMSvalue[5], _baseMSvalue[6], _baseMSvalue[7], _baseMSvalue[8], _baseMSvalue[9],
        _baseattri1, _baseattri2, _baseattri3);

        //Debug.Log("_baseattri2: " + _baseattri2);

        GameMgr.MakeItemStatus = 0;
        if (_base_itemType == "Mat" || _base_itemType == "Potion")
        {
            //アイテム取得処理
            if (_base_itemType_sub == "Cream" || _base_itemType_sub == "Appaleil" || _base_itemType_sub == "Appaleil_Icecream" || 
                _base_itemType_sub == "Source" || _base_itemType_sub == "Potion" || _base_itemType_sub == "AromaPotion" || _base_itemType_sub == "WhipeedCream" ||
                _base_itemType_sub == "Figure" || _base_itemType_sub == "FrozenFruits" ||
                _base_itemType_subB == "a_WaterSoda" || _base_itemType_subB == "a_SugerWater" || _base_itemType_subB == "a_SugerFlower")
            {
                GetItemMethod(0); //生地作ったときは各ステータスオリジナルのものなので、オリジナルアイテムに登録
            }
            else
            {
                GetItemMethod(2); //上記以外は店売りアイテムとして登録
                GameMgr.MakeItemStatus = 2;
            }
        }
        else
        {
            //アイテム取得処理
            GetItemMethod(1); //お菓子なら、お菓子パネルにすでにお菓子があるかどうかを判定し、追加処理
        }
    }

    void GetItemMethod(int _status)
    {
        if(!hikari_make_flag)
        {
            if (exp_Controller.DoubleItemCreated == 0)
            {
                MakeMethod(_status);
            }
            else //2個以上できる場合。卵白と卵黄が同時にできる場合など。
            {
                MakeMethodExt();
            }

            //MPリジェネがある場合、制作したタイミングでMPも回復する。
            if (magicskill_database.skillName_SearchLearnLevel("MP_Regenaration") > 0)
            {
                PlayerStatus.player_mp += magicskill_database.skillName_SearchLearnLevel("MP_Regenaration") * 1;
                if(PlayerStatus.player_mp >= PlayerStatus.player_maxmp)
                {
                    PlayerStatus.player_mp = PlayerStatus.player_maxmp;
                }
            }
        }
        else //ヒカリが作る場合
        {
            if (GameMgr.hikari_make_doubleItemCreated == 0)
            {
                MakeMethod(_status);
            }
            else //2個以上できる場合。卵白と卵黄が同時にできる場合など。
            {
                MakeMethodExt();
            }
        }
        
    }

    void MakeMethod(int _status)
    {
        switch (_status)
        {
            case 0: //オリジナルアイテムリストに追加する場合

                //新しく作ったアイテムをオリジナルアイテムリストに追加。
                pitemlist.addOriginalItem(_basename, _basehp, _baseday, _basequality, _baseexp, _baseprobability,
                _baserich, _basesweat, _basebitter, _basesour, _basecrispy, _basefluffy, _basesmooth, _basehardness, _basejiggly, _basechewy, _basepowdery, _baseoily, _basewatery, 
                _basebeauty,
                _basejuice, _basetea_flavor,
                _basesp_wind, _basesp_score2, _basesp_score3, _basesp_score4, _basesp_score5, _basesp_score6, _basesp_score7, _basesp_score8, _basesp_score9, _basesp_score10,
                _basegirl1_like, _basecost, _basesell,
                _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9],
                result_kosu, _base_extreme_kaisu, _base_item_hyouji, totalkyori, _basemagic,
                _baseMS[0], _baseMS[1], _baseMS[2], _baseMS[3], _baseMS[4], _baseMS[5], _baseMS[6], _baseMS[7], _baseMS[8], _baseMS[9],
                _baseMSvalue[0], _baseMSvalue[1], _baseMSvalue[2], _baseMSvalue[3], _baseMSvalue[4], _baseMSvalue[5], _baseMSvalue[6], _baseMSvalue[7], _baseMSvalue[8], _baseMSvalue[9],
                _baseattri1, _baseattri2, _baseattri3);

                new_item = pitemlist.player_originalitemlist.Count - 1; //最後に追加されたアイテムが、さっき作った新規アイテムなので、そのIDを入れて置き、リザルトで表示

                //カード正式名称（ついてるスロット名も含めた名前）
                slotchangename.slotChangeName(1, new_item, "yellow", 0); //4つ目の番号は、ステータス。通常のオリジナルアイテムの場合は0。

                itemslotname = "";
                for (i = 0; i < _slotHyouji1.Length; i++)
                {
                    _slotHyouji1[i] = slotchangename._slotHyouji[i];
                    itemslotname += _slotHyouji1[i];
                }

                pitemlist.player_originalitemlist[new_item].item_SlotName = itemslotname;
                itemfullname = itemslotname + pitemlist.player_originalitemlist[new_item].itemNameHyouji;
                pitemlist.player_originalitemlist[new_item].item_FullName = itemfullname;

                break;

            case 1: //お菓子パネルにセットする場合。通常はこっちが多い。                    

                //もし、すでにお菓子パネルにお菓子がセットされてた場合、それをオリジナルアイテムに移動してから、お菓子パネルに新しくセットする。
                //①まずコピー
                if (pitemlist.player_extremepanel_itemlist.Count > 0)
                {
                    if (hikari_make_flag) //ヒカリお菓子作る場合は、仕上げは無いため、Comp_method_bunkiは考えなくて良い。
                    {
                        pitemlist.ExtremeToCopyOriginalItem(pitemlist.player_extremepanel_itemlist[0].ItemKosu);                        
                    }
                    else
                    {
                        if (Comp_method_bunki == 3 || Comp_method_bunki == 22) //トッピング調合の場合  お菓子パネルのお菓子を削除し、新しく登録するのみ。
                        {
                            pitemlist.ExtremeToCopyOriginalItem(pitemlist.player_extremepanel_itemlist[0].ItemKosu - 1); //仮に3個同時とか作ってた場合もあるので、-1で計算。
                        }
                        else
                        {
                            pitemlist.ExtremeToCopyOriginalItem(pitemlist.player_extremepanel_itemlist[0].ItemKosu);
                        }
                    }

                    //ヒカリが選択していたお菓子のタイプを再設定　オリジナルかお菓子パネルを選択していたかで、アイテムタイプが変わる可能性があるので、タイプを更新
                    if (GameMgr.hikari_make_okashiFlag) //作ってるときのみ判定
                    {
                        if (GameMgr.hikari_kettei_toggleType[0] == 1 || GameMgr.hikari_kettei_toggleType[0] == 2)
                        {
                            GameMgr.hikari_kettei_toggleType[0] = pitemlist.ReturnOriginalKoyuIDtoItemType(GameMgr.hikari_kettei_originalID[0]);
                        }
                        if (GameMgr.hikari_kettei_toggleType[1] == 1 || GameMgr.hikari_kettei_toggleType[1] == 2)
                        {
                            GameMgr.hikari_kettei_toggleType[1] = pitemlist.ReturnOriginalKoyuIDtoItemType(GameMgr.hikari_kettei_originalID[1]);
                        }

                        if (GameMgr.hikari_kettei_item[2] != 9999)
                        {
                            if (GameMgr.hikari_kettei_toggleType[2] == 1 || GameMgr.hikari_kettei_toggleType[2] == 2)
                            {
                                GameMgr.hikari_kettei_toggleType[2] = pitemlist.ReturnOriginalKoyuIDtoItemType(GameMgr.hikari_kettei_originalID[2]);
                            }
                        }
                    }

                    //②つづいて、お菓子パネルのアイテムを全て削除
                    pitemlist.deleteAllExtremePanelItem();
                }

                //③新しく作ったアイテムをお菓子パネルアイテムリストに追加。
                pitemlist.addExtremeItem(_basename, _basehp, _baseday, _basequality, _baseexp, _baseprobability,
                _baserich, _basesweat, _basebitter, _basesour, _basecrispy, _basefluffy, _basesmooth, _basehardness, _basejiggly, _basechewy, _basepowdery, _baseoily, _basewatery, 
                _basebeauty,
                _basejuice, _basetea_flavor,
                _basesp_wind, _basesp_score2, _basesp_score3, _basesp_score4, _basesp_score5, _basesp_score6, _basesp_score7, _basesp_score8, _basesp_score9, _basesp_score10,
                _basegirl1_like, _basecost, _basesell,
                _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9],
                result_kosu, _base_extreme_kaisu, _base_item_hyouji, totalkyori, _basemagic,
                _baseMS[0], _baseMS[1], _baseMS[2], _baseMS[3], _baseMS[4], _baseMS[5], _baseMS[6], _baseMS[7], _baseMS[8], _baseMS[9],
                _baseMSvalue[0], _baseMSvalue[1], _baseMSvalue[2], _baseMSvalue[3], _baseMSvalue[4], _baseMSvalue[5], _baseMSvalue[6], _baseMSvalue[7], _baseMSvalue[8], _baseMSvalue[9],
                _baseattri1, _baseattri2, _baseattri3);

                new_item = pitemlist.player_extremepanel_itemlist.Count - 1; //最後に追加されたアイテムが、さっき作った新規アイテムなので、そのIDを入れて置き、リザルトで表示

                //カード正式名称（ついてるスロット名も含めた名前）
                slotchangename.slotChangeName(1, new_item, "yellow", 2); //4つ目の番号は、ステータス。お菓子パネルアイテムの場合は2。

                itemslotname = "";
                for (i = 0; i < _slotHyouji1.Length; i++)
                {
                    _slotHyouji1[i] = slotchangename._slotHyouji[i];
                    itemslotname += _slotHyouji1[i];
                }

                pitemlist.player_extremepanel_itemlist[new_item].item_SlotName = itemslotname;
                itemfullname = itemslotname + pitemlist.player_extremepanel_itemlist[new_item].itemNameHyouji;
                pitemlist.player_extremepanel_itemlist[new_item].item_FullName = itemfullname;

                break;

            case 2:

                MakeMethodMaterial();
                break;
        }
    }

    void MakeMethodExt()
    {
        if (databaseCompo.compoitems[result_compID].cmpitemID_result2 != "Non")
        {
            pitemlist.addPlayerItemString(databaseCompo.compoitems[result_compID].cmpitemID_result, result_kosu);
            pitemlist.addPlayerItemString(databaseCompo.compoitems[result_compID].cmpitemID_result2, result_kosu);
        }
    }

    //店売りアイテムとして作る場合　フルーツなどの材料やトッピングアイテムが出来る場合
    void MakeMethodMaterial()
    {
        pitemlist.addPlayerItemString(databaseCompo.compoitems[result_compID].cmpitemID_result, result_kosu);
    }

    //個数計算メソッド
    public void ResultKosuKeisan(int _compo_select, int _result_cmpID, int _set_kaisu, int _kettei_id1, int _kettei_id2, int _kettei_id3, int _toggletype1, int _toggletype2, int _toggletype3, int _kosu1, int _kosu2, int _kosu3)
    {
        if (_compo_select == 3) //オリジナル調合の場合
        {
            if (Kosu_keisanmethod)
            {
                //特定の材料を指定した場合、その材料の個数がそのままリザルト個数になる
                Kosu_ExpSetting(_result_cmpID, _set_kaisu, _kettei_id1, _kettei_id2, _kettei_id3, _toggletype1, _toggletype2, _toggletype3, _kosu1, _kosu2, _kosu3);
            }
            else
            {
                result_kosu = databaseCompo.compoitems[_result_cmpID].cmpitem_result_kosu * _set_kaisu;
            }
        }
        else if (_compo_select == 1) //レシピ調合の場合
        {
            if (Kosu_keisanmethod)
            {
                //特定の材料を指定した場合、その材料の個数がそのままリザルト個数になる
                Kosu_ExpSetting(_result_cmpID, _set_kaisu, _kettei_id1, _kettei_id2, _kettei_id3, _toggletype1, _toggletype2, _toggletype3, _kosu1, _kosu2, _kosu3);
            }
            else
            {
                result_kosu = databaseCompo.compoitems[_result_cmpID].cmpitem_result_kosu * _set_kaisu;
            }
        }
        else if (_compo_select == 2) //トッピング調合の場合
        {
            result_kosu = 1;
        }
        /*else if (exp_Controller.roast_result_ok == true) //「焼く」の場合
        {
            result_kosu = GameMgr.Final_kettei_kosu1;
        }*/
        else if (_compo_select == 21) //魔法調合の場合
        {
            Debug.Log("_compo_select: " + _compo_select + "魔法調合の場合の、最終個数指定");

            if (magicskill_database.magicskill_lists[magicskill_database.SearchSkillString(GameMgr.UseMagicSkill)].skill_KosuSelect == "CompNo")
            {
                result_kosu = _kosu1 * _set_kaisu; //元のアイテムになにかをかける魔法も、元アイテム一個にかけるので生成も一個 _kosu1にしてるけど、1個でもいい
            }
            else
            {
                if (magicskill_database.magicskill_lists[magicskill_database.SearchSkillString(GameMgr.UseMagicSkill)].skill_KosuSelect == "KetteiKosu")
                {
                    result_kosu = _kosu1 * _set_kaisu; //入れた個数だけできる
                }
                else
                {
                    switch (GameMgr.UseMagicSkill)
                    {
                        case "Aroma_Potion": //アロマポーションは基本個数が一個（入れた材料の数が濃縮）　ただし、スキル習得レベルで個数増える

                            result_kosu = 1 * GameMgr.UseMagicSkillLv * _set_kaisu; //GameMgr.UseMagicSkillLvは使うときのレベルでもあるが、現在は習得レベルと同一。
                            break;

                        default: //その他　フリージングやテンパリングなど。compoDBを指定するものは、compoDBの個数

                            if (Kosu_keisanmethod)
                            {
                                //特定の材料を指定した場合、その材料の個数がそのままリザルト個数になる
                                result_kosu = databaseCompo.compoitems[_result_cmpID].cmpitem_result_kosu * _kosu1 * _set_kaisu;
                            }
                            else
                            {
                                result_kosu = databaseCompo.compoitems[_result_cmpID].cmpitem_result_kosu * _set_kaisu;
                            }
                            break;
                    }
                }
            }

        }
        else if (_compo_select == 7) //ヒカリお菓子作りの個数 set_kaisuがヒカリが作った回数
        {
            if (Kosu_keisanmethod)
            {
                //特定の材料を指定した場合、その材料の個数がそのままリザルト個数になる
                Kosu_ExpSetting(result_compID, _set_kaisu, _kettei_id1, _kettei_id2, _kettei_id3, _toggletype1, _toggletype2, _toggletype3, _kosu1, _kosu2, _kosu3);
            }
            else
            {
                result_kosu = databaseCompo.compoitems[result_compID].cmpitem_result_kosu * _set_kaisu;
            }

            if (GameMgr.hikari_make_okashiKosu_buf == 0) { GameMgr.hikari_make_okashiKosu_buf = 1.0f; } //例外処理　0で割らないようにする
            Debug.Log("ヒカリ制作の個数: 元" + result_kosu + " 個数のバフ（右の数字で割り算）: " + GameMgr.hikari_make_okashiKosu_buf);            
            result_kosu = (int)(result_kosu / GameMgr.hikari_make_okashiKosu_buf);

            if (GameMgr.hikari_make_success_count >= 1) //一回でも成功してたら、最低一個はできる。
            {
                if (result_kosu == 0) { result_kosu = 1; }
            }
            Debug.Log("ヒカリ制作の最終個数: " + result_kosu + "個");
        }
        GameMgr.Result_Kosu = result_kosu;
    }

    void Kosu_ExpSetting(int _result_cmpID, int _set_kaisu, int _kettei_id1, int _kettei_id2, int _kettei_id3, int _toggletype1, int _toggletype2, int _toggletype3, int _kosu1, int _kosu2, int _kosu3)
    {
        Debug.Log("特定材料選んだ　その材料を入れた個数にする。（たまご割りなど）");
        Kosu_ExSetting = false;

        //プレイヤーアイテムかエクストリームアイテムのIDをアイテムDBのIDに戻す。
        if (_toggletype1 == 0)
        {
            _id = _kettei_id1;
        }
        else if (_toggletype1 == 1)
        {
            _id = database.SearchItemID(pitemlist.player_originalitemlist[_kettei_id1].itemID);
        }
        else if (_toggletype1 == 2)
        {
            _id = database.SearchItemID(pitemlist.player_extremepanel_itemlist[_kettei_id1].itemID);
        }

        //特定の材料をdatabaseCompo.compoitems[result_compID].KeisanMethodで指定した場合、その材料を入れた個数がそのままリザルト個数になる           
        //Debug.Log("database.items[_id].itemName: " + database.items[_id].itemName + " " + database.items[_id].itemType_sub.ToString() + " " + database.items[_id].itemType_subB.ToString());
        if (database.items[_id].itemName == databaseCompo.compoitems[_result_cmpID].KeisanMethod ||
            database.items[_id].itemType_sub.ToString() == databaseCompo.compoitems[_result_cmpID].KeisanMethod ||
            database.items[_id].itemType_subB.ToString() == databaseCompo.compoitems[_result_cmpID].KeisanMethod)
        {
            Debug.Log("個数指定: " + database.items[_id].itemName + " " + _kosu1);
            result_kosu = databaseCompo.compoitems[_result_cmpID].cmpitem_result_kosu * _set_kaisu * _kosu1;
            Kosu_ExSetting = true;
        }

        //プレイヤーアイテムかエクストリームアイテムのIDをアイテムDBのIDに戻す。
        if (_toggletype2 == 0)
        {
            _id = _kettei_id2;
        }
        else if (_toggletype2 == 1)
        {
            _id = database.SearchItemID(pitemlist.player_originalitemlist[_kettei_id2].itemID);
        }
        else if (_toggletype2 == 2)
        {
            _id = database.SearchItemID(pitemlist.player_extremepanel_itemlist[_kettei_id2].itemID);
        }

        //Debug.Log("database.items[_id].itemName: " + database.items[_id].itemName + " " + database.items[_id].itemType_sub.ToString() + " " + database.items[_id].itemType_subB.ToString());
        if (_id != 9999)
        {
            if (database.items[_id].itemName == databaseCompo.compoitems[_result_cmpID].KeisanMethod ||
            database.items[_id].itemType_sub.ToString() == databaseCompo.compoitems[_result_cmpID].KeisanMethod ||
            database.items[_id].itemType_subB.ToString() == databaseCompo.compoitems[_result_cmpID].KeisanMethod)
            {
                Debug.Log("個数指定: " + database.items[_id].itemName + " " + _kosu2);
                result_kosu = databaseCompo.compoitems[_result_cmpID].cmpitem_result_kosu * _set_kaisu * _kosu2;
                Kosu_ExSetting = true;
            }
        }

        //プレイヤーアイテムかエクストリームアイテムのIDをアイテムDBのIDに戻す。
        if (_toggletype3 == 0)
        {
            _id = _kettei_id3;
        }
        else if (_toggletype3 == 1)
        {
            _id = database.SearchItemID(pitemlist.player_originalitemlist[_kettei_id3].itemID);
        }
        else if (_toggletype3 == 2)
        {
            _id = database.SearchItemID(pitemlist.player_extremepanel_itemlist[_kettei_id3].itemID);
        }

        if (_id != 9999)
        {
            if (database.items[_id].itemName == databaseCompo.compoitems[_result_cmpID].KeisanMethod ||
                database.items[_id].itemType_sub.ToString() == databaseCompo.compoitems[_result_cmpID].KeisanMethod ||
                database.items[_id].itemType_subB.ToString() == databaseCompo.compoitems[_result_cmpID].KeisanMethod)
            {
                Debug.Log("個数指定: " + database.items[_id].itemName + " " + _kosu3);
                result_kosu = databaseCompo.compoitems[_result_cmpID].cmpitem_result_kosu * _set_kaisu * _kosu3;
                Kosu_ExSetting = true;
            }
        }

        if (!Kosu_ExSetting)
        {
            Debug.Log("やっぱり特定材料での個数指定しない");
            result_kosu = databaseCompo.compoitems[_result_cmpID].cmpitem_result_kosu * _set_kaisu;
        }
    }





    //
    // 合成の処理・計算を行うメソッド。入口。
    //
    void AddParamMethod()
    {

        //初期化
        _additemlist.Clear();

        _temphp = 0;
        _tempday = 0;
        _tempquality = 0;
        _tempexp = 0;
        _temprich = 0;
        _tempsweat = 0;
        _tempbitter = 0;
        _tempsour = 0;
        _tempcrispy = 0;
        _tempfluffy = 0;
        _tempsmooth = 0;
        _temphardness = 0;
        _tempjiggly = 0;
        _tempchewy = 0;
        _temppowdery = 0;
        _tempoily = 0;
        _tempwatery = 0;
        _tempbeauty = 0;
        _temptea_flavor = 0;
        _tempsp_wind = 0;
        _tempsp_score2 = 0;
        _tempsp_score3 = 0;
        _tempsp_score4 = 0;
        _tempsp_score5 = 0;
        _tempsp_score6 = 0;
        _tempsp_score7 = 0;
        _tempsp_score8 = 0;
        _tempsp_score9 = 0;
        _tempsp_score10 = 0;
        _tempgirl1_like = 0;
        _tempcost = 0;
        _tempsell = 0;
        _tempmagic = 0;

        for (i = 0; i < database.items[0].toppingtype.Length; i++)
        {
            _temptp[i] = "Non";
        }

        //材料一個目のパラムを取得し、_addに代入
        //Debug.Log("toggle_type1: " + toggle_type1);

        switch (toggle_type1)
        {
            case 0: //プレイヤーアイテムリストから選択している。

                //Debug.Log("一個目店アイテム");

                _id = kettei_item1;

                //器具は、除外
                /*if (database.items[_id].itemType_sub.ToString() == "Machine")
                {

                }
                else
                {*/
                    if (Kosu_keisanmethod) //たまご割りみたいに、入れたたまごの数がそのままリザルト個数になる場合。品質は、元のアイテム一個分で計算
                    {
                        _addkosu = 1;
                    }
                    else
                    {
                        _addkosu = final_kette_kosu1;
                    }
                    
                    //Debug.Log("_id: " + _id);
                    //各パラメータを取得
                    Set_addparam();
                //}

                break;

            case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                //Debug.Log("一個目オリジナルアイテム");

                _id = kettei_item1;
                if (Kosu_keisanmethod) //たまご割りみたいに、入れたたまごの数がそのままリザルト個数になる場合。品質は、元のアイテム一個分で計算
                {
                    _addkosu = 1;
                }
                else
                {
                    _addkosu = final_kette_kosu1;
                }
                //Debug.Log("_id: " + _id);
                //各パラメータを取得
                Set_add_originparam();

                break;

            case 2: //お菓子パネルアイテムリストから選択している場合

                //Debug.Log("一個目オリジナルアイテム");

                _id = kettei_item1;
                if (Kosu_keisanmethod) //たまご割りみたいに、入れたたまごの数がそのままリザルト個数になる場合。品質は、元のアイテム一個分で計算
                {
                    _addkosu = 1;
                }
                else
                {
                    _addkosu = final_kette_kosu1;
                }
                //Debug.Log("_id: " + _id);
                //各パラメータを取得
                Set_add_extremeparam();

                break;

            default:
                break;
        }

        //追加処理終了。（一個目）　2個目、3個目も、同様に繰り返す。



        //材料二個目のパラムを取得し、_addに代入。

        if (kettei_item2 != 9999) //二個目のトッピングアイテムを選んでいなければ、この処理は無視する。
        {

            //Debug.Log("final_kette_kosu2: " + final_kette_kosu2);
            switch (toggle_type2)
            {
                case 0: //プレイヤーアイテムリストから選択している。

                    //Debug.Log("二個目店アイテム");

                    _id = kettei_item2;

                    //器具は、除外
                    /*if (database.items[_id].itemType_sub.ToString() == "Machine")
                    {

                    }
                    else
                    {*/
                        if (Kosu_keisanmethod) //たまご割りみたいに、入れたたまごの数がそのままリザルト個数になる場合。品質は、元のアイテム一個分で計算
                        {
                            _addkosu = 1;
                        }
                        else
                        {
                            _addkosu = final_kette_kosu2;
                        }
                        //Debug.Log("_id: " + _id);
                        //各パラメータを取得
                        Set_addparam();
                    //}

                    break;

                case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                    //Debug.Log("二個目オリジナルアイテム");

                    _id = kettei_item2;
                    if (Kosu_keisanmethod) //たまご割りみたいに、入れたたまごの数がそのままリザルト個数になる場合。品質は、元のアイテム一個分で計算
                    {
                        _addkosu = 1;
                    }
                    else
                    {
                        _addkosu = final_kette_kosu2;
                    }
                    //Debug.Log("_id: " + _id);
                    //各パラメータを取得
                    Set_add_originparam();

                    break;

                case 2: //お菓子パネルアイテムリストから選択している場合

                    //Debug.Log("二個目オリジナルアイテム");

                    _id = kettei_item2;
                    if (Kosu_keisanmethod) //たまご割りみたいに、入れたたまごの数がそのままリザルト個数になる場合。品質は、元のアイテム一個分で計算
                    {
                        _addkosu = 1;
                    }
                    else
                    {
                        _addkosu = final_kette_kosu2;
                    }
                    //Debug.Log("_id: " + _id);
                    //各パラメータを取得
                    Set_add_extremeparam();

                    break;

                default:
                    break;
            }

        }



        //材料三個目のパラムを取得し、_addに代入。

        if (kettei_item3 != 9999) //三個目のトッピングアイテムを選んでいなければ、この処理は無視する。
        {

            //Debug.Log("3個目のアイテムを使用 final_kette_kosu3: " + final_kette_kosu3);
            switch (toggle_type3)
            {
                case 0: //プレイヤーアイテムリストから選択している。

                    _id = kettei_item3;

                    //器具は、除外
                    /*if (database.items[_id].itemType_sub.ToString() == "Machine")
                    {

                    }
                    else
                    {*/
                        if (Kosu_keisanmethod) //たまご割りみたいに、入れたたまごの数がそのままリザルト個数になる場合。品質は、元のアイテム一個分で計算
                        {
                            _addkosu = 1;
                        }
                        else
                        {
                            _addkosu = final_kette_kosu3;
                        }

                        //各パラメータを取得
                        Set_addparam();
                    //}

                    break;

                case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                    _id = kettei_item3;
                    if (Kosu_keisanmethod) //たまご割りみたいに、入れたたまごの数がそのままリザルト個数になる場合。品質は、元のアイテム一個分で計算
                    {
                        _addkosu = 1;
                    }
                    else
                    {
                        _addkosu = final_kette_kosu3;
                    }

                    //各パラメータを取得
                    Set_add_originparam();

                    break;

                case 2: //お菓子パネルアイテムリストから選択している場合

                    _id = kettei_item3;
                    if (Kosu_keisanmethod) //たまご割りみたいに、入れたたまごの数がそのままリザルト個数になる場合。品質は、元のアイテム一個分で計算
                    {
                        _addkosu = 1;
                    }
                    else
                    {
                        _addkosu = final_kette_kosu3;
                    }

                    //各パラメータを取得
                    Set_add_extremeparam();

                    break;

                default:
                    break;
            }

        }

        /*for (i = 0; i < _additemlist.Count; i++) //デバッグ
        {
            Debug.Log("_additemlist._Addkosu: " + i + ": " + _additemlist[i]._Addkosu);
        }*/


        Comp_ParamAddMethod();

    }

    //実際のパラメータ計算
    void Comp_ParamAddMethod()
    {

        total_kosu = 0;
        GameMgr.tempature_control_Param_yakitext = "";

        //①まずは、日数やコストなどの、全てのジャンルに共通するパラメータ同士を加算する。料金（_basesell）は、品質に基づいて計算する。

        for (i = 0; i < _additemlist.Count; i++) //入れた材料の数だけ、繰り返す。その後、総個数割り算。
        {
            _tempexp += _additemlist[i].Exp * _additemlist[i].ItemKosu;
            _tempday += _additemlist[i].item_day * _additemlist[i].ItemKosu;
            _tempquality += _additemlist[i].Quality * _additemlist[i].ItemKosu;

            total_kosu += _additemlist[i].ItemKosu;
            //Debug.Log("各個数: " + _additemlist[i]._Addkosu);
        }

        //トッピングのときのみ、ベース個数を含む。
        if (Comp_method_bunki == 1 || Comp_method_bunki == 3 || Comp_method_bunki == 22)
        {
            total_kosu += base_kosu;
        }

        //0で割り算する恐れがあるので、回避
        if (total_kosu == 0) { total_kosu = 1; } 


        //日数・品質は、全て加算したあとに、トータル個数で割り算
        _baseday += _tempday;
        _basequality += _tempquality;

        _baseday /= total_kosu;
        _basequality /= total_kosu;

        //加算のみのパラメータ
        _baseexp += _tempexp;
        _getExp = _tempexp; //トッピング調合時に取得する経験値。パブリック


        if (mstatus == 2)//mstatus=2はヒカリが作るお菓子　totalkyoriはセーブしておく。
        {
            totalkyori = GameMgr.hikari_make_okashi_totalkyori;
        }
        else if (mstatus == 99)
        {
            kyori_kosuSet.Clear();
            kyori_kosuSet.Add(final_kette_kosu1);
            kyori_kosuSet.Add(final_kette_kosu2);
            kyori_kosuSet.Add(final_kette_kosu3);
            totalkyori = Combinationmain.GetKyoriKeisan(result_compID, kyori_kosuSet.ToArray()); //初期化のときは、ここで距離をとってくる            
        }
        else
        {
            totalkyori = Combinationmain.totalkyori; //_mstatus=99のときにこのスクリプトから計算するか、調合時にもCombinationmain.csで計算して、値が更新されてるはず。
        }





        //②次に、甘さやサクサク感などの計算処理。

        //1. 新規調合の場合、加算。

        //2. トッピング調合時は、フルーツ・トッピングアイテムのみ加算。

        //3. 新規調合で、かつ小麦粉を使った場合、甘さなどはそのまま加算するが、食感はそのまま加算はせず、小麦粉をベースに、バター・砂糖・たまごは、各比率を計算し、代入する。

        //***基本の味の計算方法***
        //各材料の、パラメータをそれぞれ加算する。食感のみ、小麦粉とその他材料の比率をだして、補正がかかる。イメージ。

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2 || Comp_method_bunki == 20 || Comp_method_bunki == 22)//オリジナル調合・レシピ調合・魔法　のときの計算。
        {
            //デバッグ用
            if (mstatus == 99)
            {               
                //Debug_AddKeisanCheck("bugget", 0);
            }

            //材料のパラメータ計算処理。
            AddParam_Method();

            //ベースのパラメータに、材料の各パラメータを加算する。
            _baserich += _temprich;
            _basesweat += _tempsweat;
            _basebitter += _tempbitter;
            _basesour += _tempsour;
            _basecrispy += _tempcrispy;
            _basefluffy += _tempfluffy;
            _basesmooth += _tempsmooth;
            _basehardness += _temphardness;
            _basejiggly += _tempjiggly;
            _basechewy += _tempchewy;
            _basepowdery += _temppowdery;
            _baseoily += _tempoily;
            _basewatery += _tempwatery;
            _basebeauty += _tempbeauty;
            _basetea_flavor += _temptea_flavor;
            _basesp_wind += _tempsp_wind;
            _basesp_score2 += _tempsp_score2;
            _basesp_score3 += _tempsp_score3;
            _basesp_score4 += _tempsp_score4;
            _basesp_score5 += _tempsp_score5;
            _basesp_score6 += _tempsp_score6;
            _basesp_score7 += _tempsp_score7;
            _basesp_score8 += _tempsp_score8;
            _basesp_score9 += _tempsp_score9;
            _basesp_score10 += _tempsp_score10;
            _basemagic = _tempmagic;

            //デバッグ用
            if (mstatus == 99)
            {                
                //Debug_AddKeisanCheck("bugget", 10);
            }


            if (keisan_method_flag == 1) //1=ベスト配合との距離の補正をかける。
            {
                
                if (mstatus != 99)
                {
                    Debug.Log("ベスト配合との距離: " + totalkyori);
                }

                if (totalkyori >= 0 && totalkyori < 0.1)
                {
                    kyori_hosei = 2.0f;
                    kyori_hosei = bufpower_keisan.Buf_KyoriHosei_Keisan(kyori_hosei, _basename);
                }
                else if (totalkyori >= 0.1 && totalkyori < 0.5)
                {
                    kyori_hosei = 1.5f;
                    kyori_hosei = bufpower_keisan.Buf_KyoriHosei_Keisan(kyori_hosei, _basename);
                }
                else if (totalkyori >= 0.5 && totalkyori < 1.0)
                {
                    kyori_hosei = 1.35f;
                    kyori_hosei = bufpower_keisan.Buf_KyoriHosei_Keisan(kyori_hosei, _basename);
                }
                else if (totalkyori >= 1.0 && totalkyori < 2.0)
                {
                    kyori_hosei = 1.2f;
                    kyori_hosei = bufpower_keisan.Buf_KyoriHosei_Keisan(kyori_hosei, _basename);
                }
                else if (totalkyori >= 2.0 && totalkyori < 4.0)
                {
                    kyori_hosei = 1.0f;
                    kyori_hosei = bufpower_keisan.Buf_KyoriHosei_Keisan(kyori_hosei, _basename);
                }
                else if (totalkyori >= 4.0 && totalkyori < 5.0)
                {
                    kyori_hosei = 0.75f;
                    kyori_hosei = bufpower_keisan.Buf_KyoriHosei_Keisan(kyori_hosei, _basename);
                }
                else if (totalkyori >= 5.0 && totalkyori < 6.0)
                {
                    kyori_hosei = 0.5f;
                    kyori_hosei = bufpower_keisan.Buf_KyoriHosei_Keisan(kyori_hosei, _basename);
                }
                else if (totalkyori >= 6.0 && totalkyori < 8.0)
                {
                    kyori_hosei = 0.25f;
                    kyori_hosei = bufpower_keisan.Buf_KyoriHosei_Keisan(kyori_hosei, _basename);
                }
                else if (totalkyori >= 8.0)
                {
                    kyori_hosei = 0.125f;
                    kyori_hosei = bufpower_keisan.Buf_KyoriHosei_Keisan(kyori_hosei, _basename);
                }

                //食感に補正値をかける。
                _basecrispy = (int)(_basecrispy * kyori_hosei);
                _basefluffy = (int)(_basefluffy * kyori_hosei);
                _basesmooth = (int)(_basesmooth * kyori_hosei);
                _basehardness = (int)(_basehardness * kyori_hosei);
                _basejiggly = (int)(_basejiggly * kyori_hosei);
                _basechewy = (int)(_basechewy * kyori_hosei);
            }           
        }

        //お菓子のタイプによって、食感の伸び率に補正がかかる。簡単なおかしは、60点までは伸びるが、100点以降はとたんに伸びなくなる。など
        OkashiType_ShokukanBuf();
        

        //デバッグ用
        if (mstatus == 99)
        {           
            //Debug_AddKeisanCheck("bugget", 20);
        }

        

        //③スロット同士の計算をする。
        AddSlot_Method();




        //④トッピングのときの計算。加算する。
        if (Comp_method_bunki == 3 || Comp_method_bunki == 22)//
        {
            for (i = 0; i < _additemlist.Count; i++)
            {
                //Debug.Log("フルーツ・トッピングの加算処理 ON");
                //各材料を加算していく。
                /*if (_additemlist[i]._Add_itemType_sub == "Fruits" || _additemlist[i]._Add_itemType_sub == "Potion" || _additemlist[i]._Add_itemType_sub == "Source" ||
                     _additemlist[i]._Add_itemType_sub == "Chocolate" || _additemlist[i]._Add_itemType_sub == "Chocolate_Mat" || _additemlist[i]._Add_itemType_sub == "IceCream")
                {*/
                _baserich += _additemlist[i].Rich * _additemlist[i].ItemKosu;
                _basesweat += _additemlist[i].Sweat * _additemlist[i].ItemKosu;
                _basebitter += _additemlist[i].Bitter * _additemlist[i].ItemKosu;
                _basesour += _additemlist[i].Sour * _additemlist[i].ItemKosu;
                _basecrispy += _additemlist[i].Crispy * _additemlist[i].ItemKosu;
                _basefluffy += _additemlist[i].Fluffy * _additemlist[i].ItemKosu;
                _basesmooth += _additemlist[i].Smooth * _additemlist[i].ItemKosu;
                _basehardness += _additemlist[i].Hardness * _additemlist[i].ItemKosu;
                _basejiggly += _additemlist[i].Jiggly * _additemlist[i].ItemKosu;
                _basechewy += _additemlist[i].Chewy * _additemlist[i].ItemKosu;
                _basepowdery += _additemlist[i].Powdery * _additemlist[i].ItemKosu;
                _baseoily += _additemlist[i].Oily * _additemlist[i].ItemKosu;
                _basewatery += _additemlist[i].Watery * _additemlist[i].ItemKosu;
                _basebeauty += _additemlist[i].Beauty * _additemlist[i].ItemKosu;
                _basetea_flavor += _additemlist[i].Tea_Flavor * _additemlist[i].ItemKosu;
                _basesp_wind += _additemlist[i].SP_wind * _additemlist[i].ItemKosu;
                _basesp_score2 += _additemlist[i].SP_Score2 * _additemlist[i].ItemKosu;
                _basesp_score3 += _additemlist[i].SP_Score3 * _additemlist[i].ItemKosu;
                _basesp_score4 += _additemlist[i].SP_Score4 * _additemlist[i].ItemKosu;
                _basesp_score5 += _additemlist[i].SP_Score5 * _additemlist[i].ItemKosu;
                _basesp_score6 += _additemlist[i].SP_Score6 * _additemlist[i].ItemKosu;
                _basesp_score7 += _additemlist[i].SP_Score7 * _additemlist[i].ItemKosu;
                _basesp_score8 += _additemlist[i].SP_Score8 * _additemlist[i].ItemKosu;
                _basesp_score9 += _additemlist[i].SP_Score9 * _additemlist[i].ItemKosu;
                _basesp_score10 += _additemlist[i].SP_Score10 * _additemlist[i].ItemKosu;              
                //}
            }
        }

        //ジュースののどごしを計算する。新規作成時のみトッピング・仕上げのときは再計算しない。
        if (Comp_method_bunki == 0 || Comp_method_bunki == 2 || Comp_method_bunki == 20)//オリジナル調合・レシピ調合・魔法調合　のときの計算。
        {
            _basejuice = _basesweat + _basebitter + _basesour;
        }

        //新規作成時の特殊処理
        if (Comp_method_bunki == 0 || Comp_method_bunki == 2 || Comp_method_bunki == 20 || Comp_method_bunki == 22)//オリジナル調合・レシピ調合・魔法調合　のときの計算。
        {           
            Okashi_SpecialKeisan();           
        }


        //⑤器具やアクセサリーなどによるバフ効果を追加する。
        if (Comp_method_bunki == 0 || Comp_method_bunki == 2)//オリジナル調合　または　レシピ調合　のときの計算。魔法のときはバフをかけない。Comp_method_bunki == 20
        {
            if (databaseCompo.compoitems[result_compID].buf_kouka_on != 0) //_before_itemtype_Sub != _base_itemType_sub クリーム系からまたクリーム系が出来る場合は、バフがかからないよう、重複防止処理
            {
                //A. お菓子の食感ごとに、バフをかける処理
                _basecrispy += bufpower_keisan.Buf_OkashiParamUp_Keisan(0, _basecrispy, _basename); //中の数字でどの食感パラムかの指定
                _basefluffy += bufpower_keisan.Buf_OkashiParamUp_Keisan(1, _basefluffy, _basename);
                _basesmooth += bufpower_keisan.Buf_OkashiParamUp_Keisan(2, _basesmooth, _basename);
                _basehardness += bufpower_keisan.Buf_OkashiParamUp_Keisan(3, _basehardness, _basename);
                _basejuice += bufpower_keisan.Buf_OkashiParamUp_Keisan(4, _basejuice, _basename);
                _basebeauty += bufpower_keisan.Buf_OkashiParamUp_Keisan(5, _basebeauty, _basename);
                _basetea_flavor += bufpower_keisan.Buf_OkashiParamUp_Keisan(6, _basetea_flavor, _basename);

                //B. 固有のお菓子のみにバフをかける処理
                _basecrispy += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(0, _basename); //中の数字でどの食感パラムかの指定
                _basefluffy += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(1, _basename);
                _basesmooth += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(2, _basename);
                _basehardness += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(3, _basename);
                _basejuice += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(4, _basename);
                _basebeauty += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(5, _basename);
                _basetea_flavor += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(6, _basename);

                //C. 特定の調合DBにのみバフをかける処理
                _basecrispy += bufpower_keisan.Buf_OkashiParamUp_CompoNameKeisan(0, databaseCompo.compoitems[result_compID].cmpitem_Name); //中の数字でどの食感パラムかの指定
                _basefluffy += bufpower_keisan.Buf_OkashiParamUp_CompoNameKeisan(1, databaseCompo.compoitems[result_compID].cmpitem_Name);
                _basesmooth += bufpower_keisan.Buf_OkashiParamUp_CompoNameKeisan(2, databaseCompo.compoitems[result_compID].cmpitem_Name);
                _basehardness += bufpower_keisan.Buf_OkashiParamUp_CompoNameKeisan(3, databaseCompo.compoitems[result_compID].cmpitem_Name);
                _basejuice += bufpower_keisan.Buf_OkashiParamUp_CompoNameKeisan(4, databaseCompo.compoitems[result_compID].cmpitem_Name);
                _basebeauty += bufpower_keisan.Buf_OkashiParamUp_CompoNameKeisan(5, databaseCompo.compoitems[result_compID].cmpitem_Name);
                _basetea_flavor += bufpower_keisan.Buf_OkashiParamUp_CompoNameKeisan(6, databaseCompo.compoitems[result_compID].cmpitem_Name);
               
            }
        }

        //⑥ヒカリのお菓子の場合　味に補正かかる。
        if (GameMgr.System_HikariMakeUse_Flag)
        {
            //ヒカリ制作の場合
            if (mstatus == 2)
            {
                if (databaseCompo.compoitems[result_compID].buf_kouka_on != 0) //バフ計算するものだけ、バフ計算。例えばクッキー×ぶどう＝ぶどうクッキーのときは、バフ計算しない
                {
                    //まず、作るお菓子のサブタイプをもとに、計算。制作時間なども計算する。
                    hikari_okashilv_hosei = bufpower_keisan.Buf_HikariOkashiLV_Keisan(_base_itemType_sub);
                    Debug.Log("ヒカリがお菓子作る場合　補正: " + hikari_okashilv_hosei);

                    _basecrispy = (int)(1.0f * _basecrispy * hikari_okashilv_hosei);
                    _basefluffy = (int)(1.0f * _basefluffy * hikari_okashilv_hosei);
                    _basesmooth = (int)(1.0f * _basesmooth * hikari_okashilv_hosei);
                    _basehardness = (int)(1.0f * _basehardness * hikari_okashilv_hosei);
                    _basejuice = (int)(1.0f * _basejuice * hikari_okashilv_hosei);
                    //_basebeauty = (int)(1.0f * _basebeauty  * hikari_okashilv_hosei);
                    _basetea_flavor = (int)(1.0f * _basetea_flavor * hikari_okashilv_hosei);

                    //専用アイテムがあれば、ヒカリのお菓子さらにパラメータアップ
                    _basecrispy += bufpower_keisan.Buf_HikariParamUp_Keisan(0, _base_itemType_sub); //_base_itemType_subは未使用だが、とりあえず置いてる。
                    _basefluffy += bufpower_keisan.Buf_HikariParamUp_Keisan(1, _base_itemType_sub);
                    _basesmooth += bufpower_keisan.Buf_HikariParamUp_Keisan(2, _base_itemType_sub);
                    _basehardness += bufpower_keisan.Buf_HikariParamUp_Keisan(3, _base_itemType_sub);
                    _basejuice += bufpower_keisan.Buf_HikariParamUp_Keisan(4, _base_itemType_sub);
                    //_basebeauty += bufpower_keisan.Buf_HikariParamUp_Keisan(5, _base_itemType_sub);
                    _basetea_flavor += bufpower_keisan.Buf_HikariParamUp_Keisan(6, _base_itemType_sub);
                }
            }

            //mstatus=0,1はおにいちゃんの場合
            if (mstatus == 0 || mstatus == 1)
            {
                if (Comp_method_bunki == 0 || Comp_method_bunki == 2 || Comp_method_bunki == 20 || Comp_method_bunki == 22)//オリジナル調合　または　レシピ調合　または魔法　のときの計算。
                {
                    //⑦ヒカリのお菓子レベルに応じて、ほんの少し最終的なお菓子の味にバフがかかる。にいちゃんが作る場合のみ。。
                    /*if (databaseCompo.compoitems[result_compID].buf_kouka_on != 0) //バフ計算するものだけ、バフ計算。例えばクッキー×ぶどう＝ぶどうクッキーのときは、バフ計算しない
                    {
                        if (_base_itemType == "Okashi")
                        {
                            //作るお菓子のサブタイプをもとに、ヒカリのお菓子レベルを算出し、補正値をだす。
                            hikari_okashilv_paramup = bufpower_keisan.Buf_HikariOkashiLV_HoseiParamUp(_base_itemType_sub);

                            _basecrispy = (int)(1.0f * _basecrispy * hikari_okashilv_paramup);
                            _basefluffy = (int)(1.0f * _basefluffy * hikari_okashilv_paramup);
                            _basesmooth = (int)(1.0f * _basesmooth * hikari_okashilv_paramup);
                            _basehardness = (int)(1.0f * _basehardness * hikari_okashilv_paramup);
                            _basejuice = (int)(1.0f * _basejuice * hikari_okashilv_paramup);
                            //_basebeauty = (int)(1.0f * _basebeauty  * hikari_okashilv_paramup);
                            _basetea_flavor = (int)(1.0f * _basetea_flavor * hikari_okashilv_paramup);
                        }
                    }*/

                    //⑧99ハートボーナス　HLV=99のときは、お菓子の味が1.3倍に上昇。にいちゃんが作る場合のみ。
                    if (databaseCompo.compoitems[result_compID].buf_kouka_on != 0) //バフ計算するものだけ、バフ計算。例えばクッキー×ぶどう＝ぶどうクッキーのときは、バフ計算しない
                    {
                        if (PlayerStatus.girl1_Love_lv >= 99)
                        {
                            _basecrispy = (int)(1.2f * _basecrispy);
                            _basefluffy = (int)(1.2f * _basefluffy);
                            _basesmooth = (int)(1.2f * _basesmooth);
                            _basehardness = (int)(1.2f * _basehardness);
                            _basejuice = (int)(1.2f * _basejuice);
                            //_basebeauty = (int)(1.2f * _basebeauty);
                            _basetea_flavor = (int)(1.2f * _basetea_flavor);
                        }
                    }
                }
            }
        }

        //⑦魔法使用による、食感のパラメーター上昇
        if(Comp_method_bunki == 20 || Comp_method_bunki == 22)
        {
            //A. お菓子の食感ごとに、バフをかける処理
            _basecrispy += bufpower_keisan.Buf_OkashiParamUp_MagicKeisan(0, _basecrispy, GameMgr.UseMagicSkill, _baseattri2); //中の数字でどの食感パラムかの指定
            _basefluffy += bufpower_keisan.Buf_OkashiParamUp_MagicKeisan(1, _basefluffy, GameMgr.UseMagicSkill, _baseattri2);
            _basesmooth += bufpower_keisan.Buf_OkashiParamUp_MagicKeisan(2, _basesmooth, GameMgr.UseMagicSkill, _baseattri2);
            _basehardness += bufpower_keisan.Buf_OkashiParamUp_MagicKeisan(3, _basehardness, GameMgr.UseMagicSkill, _baseattri2);
            _basejuice += bufpower_keisan.Buf_OkashiParamUp_MagicKeisan(4, _basejuice, GameMgr.UseMagicSkill, _baseattri2);
            _basebeauty += bufpower_keisan.Buf_OkashiParamUp_MagicKeisan(5, _basebeauty, GameMgr.UseMagicSkill, _baseattri2);
            _basetea_flavor += bufpower_keisan.Buf_OkashiParamUp_MagicKeisan(6, _basetea_flavor, GameMgr.UseMagicSkill, _baseattri2);

            //ここで魔法スロット追加
            AddMagicSlot_Method();
        }

        //⑧温度管理による、食感の補正
        //スキル温度管理を使ったとき、温度と時間によって仕上がりがさらに変わる。

        //ヒカリ制作の場合　事前に設定した温度で焼き具合を決めてくれる
        if (mstatus == 2)
        {
            _best_well_done = _base_bestwelldone; //200°で10分ほど焼いたときの焼き具合 60分焼けるけど、クッキーの場合30分以上は基本焦げる

            if (GameMgr.hikari_tempature_control_ON)
            {
                Debug.Log("--- ヒカリ温度管理ON --- ");
                Debug.Log("さくさく・ふわふわ・歯ごたえに補正がかかる");

                _well_done_kyori_hosei = bufpower_keisan.TempatureControlKeisan(_best_well_done, GameMgr.hikari_tempature_param_temp, GameMgr.hikari_tempature_param_time);

                //食感に補正値をかける。
                _basecrispy = (int)(_basecrispy * _well_done_kyori_hosei);
                _basefluffy = (int)(_basefluffy * _well_done_kyori_hosei);
                //_basesmooth = (int)(_basesmooth * kyori_hosei);
                _basehardness = (int)(_basehardness * _well_done_kyori_hosei);
                //_basejiggly = (int)(_basejiggly * kyori_hosei);
                //_basechewy = (int)(_basechewy * kyori_hosei);
            }

        }
        else
        {
            if (Comp_method_bunki == 0 || Comp_method_bunki == 2)//オリジナル調合・レシピ調合　のときの計算。
            {
                //_well_done = 0;
                _best_well_done = _base_bestwelldone; //200°で10分ほど焼いたときの焼き具合 60分焼けるけど、クッキーの場合30分以上は基本焦げる

                if (GameMgr.tempature_control_ON)
                {
                    Debug.Log("--- 温度管理ON --- ");
                    Debug.Log("さくさく・ふわふわ・歯ごたえに補正がかかる");

                    _well_done_kyori_hosei = bufpower_keisan.TempatureControlKeisan(_best_well_done, GameMgr.System_tempature_control_Param_temp, GameMgr.System_tempature_control_Param_time);

                    //食感に補正値をかける。
                    _basecrispy = (int)(_basecrispy * _well_done_kyori_hosei);
                    _basefluffy = (int)(_basefluffy * _well_done_kyori_hosei);
                    //_basesmooth = (int)(_basesmooth * kyori_hosei);
                    _basehardness = (int)(_basehardness * _well_done_kyori_hosei);
                    //_basejiggly = (int)(_basejiggly * kyori_hosei);
                    //_basechewy = (int)(_basechewy * kyori_hosei);
                }
            }
        }
    }

    void OkashiType_ShokukanBuf()
    {
        switch (_base_itemType_subB)
        {
            case "a_CookieSimple":

                if (_basecrispy < 60) //60までは伸びる
                {
                }
                else if (_basecrispy >= 60 && _basecrispy < 80)
                {
                    _basecrispy = (int)(_basecrispy * 0.9f);
                }
                else if (_basecrispy >= 80 && _basecrispy < 100)
                {
                    _basecrispy = (int)(_basecrispy * 0.85f);
                }
                else if (_basecrispy >= 100)
                {
                    _basecrispy = (int)(_basecrispy * 0.8f);
                }
                break;

            case "a_RuskSimple":

                if (_basecrispy < 60) //60までは伸びる
                {
                }
                else if (_basecrispy >= 60 && _basecrispy < 80)
                {
                    _basecrispy = (int)(_basecrispy * 0.9f);
                }
                else if (_basecrispy >= 80 && _basecrispy < 100)
                {
                    _basecrispy = (int)(_basecrispy * 0.85f);
                }
                else if (_basecrispy >= 100)
                {
                    _basecrispy = (int)(_basecrispy * 0.8f);
                }
                break;
        }
    }

    void Okashi_SpecialKeisan()
    {
        //ジュースの特殊処理　甘さが青天井で上がることはないように、上限をおさえる。
        if (_base_itemType_sub == "Juice" || _base_itemType_sub == "Soda")
        {
            if (_basename == "juice_float") //ジュースフロートは、無視する。アイスをのせるたびに、甘さがどんどん下がってしまうため。
            { }
            else
            {
                if (_basesweat >= 50)
                {
                    _add_hoseiparam = (_basesweat - 50) / 2;
                    _basesweat = 50 + _add_hoseiparam;
                }
            }
        }

        //特殊処理。カンノーリ生地ができるときは、生地をフライヤーであげるので、ふわふわ感をサクサク感に変換する。
        if (_basename == "crepe_flyed")
        {
            _basecrispy = _basefluffy;
            _basefluffy = 0;
        }
        if (_basename == "brioche") //ブリオッシュは、歯ごたえ（強力粉の値）をふわふわに変換
        {
            _basefluffy += _basehardness;
            _basefluffy += (int)(_basecrispy * 0.2f); //さくさくも若干影響する
            _basehardness = 0;
        }
        if (_basename == "cream_brulee") //クリームブリュレは、生地のなめらかさも半分足す
        {
            _basefluffy += _basesmooth/2;
        }
        //ただのアイスキャンディは、元の水のなめらかさと砂糖のなめらかがそのまま食感になるので必要ない
        if (_basename == "ice_candy_fruits" || _basename == "ice_candy_twister") //フルーツアイスキャンディは、ジュースをなめらかに変換          
        {
            _basesmooth += _basejuice;
            //_basehardness = 0;
        }
        if (_base_itemType_sub == "Soda") //ソーダは、のどごしになめらかの値も影響する
        {
            _basejuice += _basesmooth / 2;
        }
        if (_base_itemType_subB == "a_ChocolateTwister" || _base_itemType_subB == "a_ChocolateCrown") //ツイスターとクラウンは、食感が半減。見た目と芸術性で勝負する。
        {
            _basecrispy = _basecrispy / 2;
            _basefluffy = _basefluffy / 2;
            _basesmooth = _basesmooth / 2;
            _basehardness = _basehardness / 2;
        }
        /*if (_basename == "figure_bear_choco" || _basename == "figure_bear_whitechoco") //トッピング用くまさんは、なめらかを落とす
        {
            _basesmooth = _basesmooth / 8;
        }*/
        if (_basename == "bitter_potion") //ビターポーションはビターのみ抽出
        {
            _basesweat = 0;
            _basesour = 0;
        }
        if (_basename == "sour_potion") //サワーポーションなども同じような処理
        {
            _basesweat = 0;
            _basebitter = 0;
        }
        if (_basename == "sweat_potion") //
        {
            _basebitter = 0;
            _basesour = 0;
        }
    }


    void AddSlot_Method()
    {
        //重複した場合は、個別にスロットに入れる。新しいトッピング能力がある場合は、ベースの空のスロットに上書きしていく。
        //ベースの空スロットがなくなった時点で、それ以上合成はできない。

        //加算トッピングの一個目をもとに、ベースのスロット一個目から順番にみていく。

        for (count = 0; count < _additemlist.Count; count++)
        {

            for (n = 0; n < _additemlist[count].ItemKosu; n++)
            {

                i = 0;
                while (i < _additemlist[count].toppingtype.Length)
                {
                    //Debug.Log(_addtp[i]);

                    if (_additemlist[count].toppingtype[i] != "Non") //Nonではない、＝いちごとかオレンジとか、何かが入っている場合は、次にベースのTPを見る。
                    {

                        j = 0;
                        while (j < _basetp.Length) //ベースが全て空でない場合、全て無視したまま、処理だけ続く。
                        {

                            if (_basetp[j] == "Non") //ベースが空の場合は、そこに_addトッピングを入れる。
                            {
                                //Debug.Log(_basetp[j]);
                                _basetp[j] = _additemlist[count].toppingtype[i];
                                break;
                            }
                            else if (_basetp[j] == _additemlist[count].toppingtype[i]) //ベースに入っているトッピングと、_addが重複の場合。
                            {
                                //無視して、次の_baseトッピングのスロットを見る。
                            }
                            else //ベースが空でない場合。
                            {
                                //無視して、次の_baseトッピングのスロットを見る。
                            }

                            j++;
                        }

                    }
                    else if (_additemlist[count].toppingtype[i] == "Non") //Nonの場合、そのスロットは無視して、次のスロットをみる
                    {
                        //break;
                    }
                    i++;
                }

                //固有トッピングのものも、トッピングする
                i = 0;
                while (i < _additemlist[count].koyu_toppingtype.Length)
                {
                    //Debug.Log(_addtp[i]);

                    if (_additemlist[count].koyu_toppingtype[i] != "Non") //Nonではない、＝いちごとかオレンジとか、何かが入っている場合は、次にベースのTPを見る。
                    {

                        j = 0;
                        while (j < _basetp.Length) //ベースが全て空でない場合、全て無視したまま、処理だけ続く。
                        {

                            if (_basetp[j] == "Non") //ベースが空の場合は、そこに_addトッピングを入れる。
                            {
                                //Debug.Log(_basetp[j]);
                                _basetp[j] = _additemlist[count].koyu_toppingtype[i];
                                break;
                            }
                            else if (_basetp[j] == _additemlist[count].koyu_toppingtype[i]) //ベースに入っているトッピングと、_addが重複の場合。
                            {
                                //無視して、次の_baseトッピングのスロットを見る。
                            }
                            else //ベースが空でない場合。
                            {
                                //無視して、次の_baseトッピングのスロットを見る。
                            }

                            j++;
                        }

                    }
                    else if (_additemlist[count].koyu_toppingtype[i] == "Non") //Nonの場合、そのスロットは無視して、次のスロットをみる
                    {
                        //break;
                    }
                    i++;
                }
            }
        }
    }

    void AddMagicSlot_Method()
    {
        //重複した場合は、個別にスロットに入れる。すでに魔法がある場合は、ベースの空のスロットに上書きしていく。
        //ベースの空スロットがなくなった時点で、それ以上合成はできない。

        _addMSvalue = 0;
        _addMS = "Non";
        _addmagic = 0;

        //魔法に応じて、入れるスロット名を先にセットしておく。各SPスコアや見た目の値はgirleat_judgeで計算
        itemCardEffect_database.AddMagicSlot_Keisan(GameMgr.UseMagicSkill, GameMgr.UseMagicSkillLv);
        _addMS = itemCardEffect_database._addMS;
        _addMSvalue = itemCardEffect_database._addMSvalue;
        _addmagic = itemCardEffect_database._addMagic;

        if (!GameMgr.System_MagicSlot_MultipleON)
        {
            if (_addMS == "Non") { }
            else
            {
                if (_baseMS[0] == _addMS) //ベースに入っているトッピングと、_addが重複の場合。
                {
                    //効果を加算しない
                    _baseMSvalue[0] = _addMSvalue;
                }
                else
                {
                    _baseMS[0] = _addMS;
                    _baseMSvalue[0] = _addMSvalue;
                }

                _basemagic = _addmagic; //演出魔法かけると魔法属性のおかしになる。
            }
        }
        else
        {
            if (_addMS == "Non") { }
            else
            {
                //加算トッピングの一個目をもとに、ベースのスロット一個目から順番にみていく。
                //Debug.Log(_addtp[i]);
                j = 0;
                while (j < _baseMS.Length) //ベースが全て空でない場合、全て無視したまま、処理だけ続く。
                {

                    if (_baseMS[j] == "Non") //ベースが空の場合は、そこに_addトッピングを入れる。
                    {
                        //Debug.Log(_baseMS[j]);
                        _baseMS[j] = _addMS;
                        _baseMSvalue[j] = _addMSvalue;
                        break;
                    }
                    else if (_baseMS[j] == _addMS) //ベースに入っているトッピングと、_addが重複の場合。
                    {
                        //効果を加算しない
                        _baseMSvalue[j] = _addMSvalue;
                        break;
                    }
                    else //ベースが空でない場合。
                    {
                        //無視して、次の_baseトッピングのスロットを見る。
                    }

                    j++;
                }

                _basemagic = _addmagic; //演出魔法かけると魔法属性のおかしになる。
            }
        }

        if (mstatus == 2) //ヒカリお菓子作る場合は、現状魔法は使用できないため以下の処理は無視
        {
        }
        else
        {
            //魔法によって状態が変わる　WindArkの回数加算など
            if (GameMgr.UseMagicSkill == "Cookie_SecondBake")
            {
                _baseattri1 = 1;
            }
            if (GameMgr.UseMagicSkill == "Wind_Ark")
            {
                _baseattri2++;
            }
        }

    }



    void Set_addparam()
    {

        _addname = database.items[_id].itemName;
        _addhp = database.items[_id].itemHP;
        _addday = database.items[_id].item_day;
        _addquality = database.items[_id].Quality;
        _addexp = database.items[_id].Exp;
        _addrich = database.items[_id].Rich;
        _addsweat = database.items[_id].Sweat;
        _addbitter = database.items[_id].Bitter;
        _addsour = database.items[_id].Sour;
        _addcrispy = database.items[_id].Crispy;
        _addfluffy = database.items[_id].Fluffy;
        _addsmooth = database.items[_id].Smooth;
        _addhardness = database.items[_id].Hardness;
        _addjiggly = database.items[_id].Jiggly;
        _addchewy = database.items[_id].Chewy;
        _addpowdery = database.items[_id].Powdery;
        _addoily = database.items[_id].Oily;
        _addwatery = database.items[_id].Watery;
        _addbeauty = database.items[_id].Beauty;
        _addtea_flavor = database.items[_id].Tea_Flavor;
        _addsp_wind = database.items[_id].SP_wind;
        _addsp_score2 = database.items[_id].SP_Score2;
        _addsp_score3 = database.items[_id].SP_Score3;
        _addsp_score4 = database.items[_id].SP_Score4;
        _addsp_score5 = database.items[_id].SP_Score5;
        _addsp_score6 = database.items[_id].SP_Score6;
        _addsp_score7 = database.items[_id].SP_Score7;
        _addsp_score8 = database.items[_id].SP_Score8;
        _addsp_score9 = database.items[_id].SP_Score9;
        _addsp_score10 = database.items[_id].SP_Score10;
        _addbest_welldone = database.items[_id].Best_Welldone;
        _addbase_score = database.items[_id].Base_Score;
        _addgirl1_like = database.items[_id].girl1_itemLike;
        _addcost = database.items[_id].cost_price;
        _addsell = database.items[_id].sell_price;
        _add_itemType = database.items[_id].itemType.ToString();
        _add_itemType_sub = database.items[_id].itemType_sub.ToString();
        _addmagic = database.items[_id].Magic;

        //店売りアイテムを合成に使う場合。通常トッピング＋固有トッピングどちらも計算

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2 || Comp_method_bunki == 20) //オリジナル・レシピ調合時
        {
            for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _addtp[i] = database.items[_id].toppingtype[i].ToString();
            }

            for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
            {
                _addkoyutp[i] = database.items[_id].koyu_toppingtype[i].ToString();
            }
        }
        else if (Comp_method_bunki == 3 || Comp_method_bunki == 22) //トッピング時。通常トッピング＋固有トッピングどちらも計算
        {
            for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _addtp[i] = database.items[_id].toppingtype[i].ToString();
            }

            for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
            {
                _addkoyutp[i] = database.items[_id].koyu_toppingtype[i].ToString();
            }
        }

        if (Comp_method_bunki == 20 || Comp_method_bunki == 22) //魔法調合時 計算時の個数は1の時のパラメータで計算する
        {

            //_addkosu = 1;
        }

        //Debug.Log("_addkosu: " + _addkosu);
        _additemlist.Add(new Item(0, "", "", _addname, "", "", 0, _addhp, _addday, _addquality, _addexp, 0, _addrich, _addsweat, _addbitter, _addsour,
        _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _addbeauty, 0, _addtea_flavor,
        _addsp_wind, _addsp_score2, _addsp_score3, _addsp_score4, _addsp_score5, _addsp_score6, _addsp_score7, _addsp_score8, _addsp_score9, _addsp_score10,
        _addbest_welldone,
        _add_itemType, _add_itemType_sub, "", "",
        _addbase_score, _addgirl1_like, _addcost, _addsell,
        _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9], 
        _addkoyutp[0], _addkoyutp[1], _addkoyutp[2], _addkoyutp[3], _addkoyutp[4], _addkosu, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, _addmagic, 
        0, 0,
        "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0,
        "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0));
    }

    void Set_add_originparam()
    {
        _addname = pitemlist.player_originalitemlist[_id].itemName;
        _addhp = pitemlist.player_originalitemlist[_id].itemHP;
        _addday = pitemlist.player_originalitemlist[_id].item_day;
        _addquality = pitemlist.player_originalitemlist[_id].Quality;
        _addexp = pitemlist.player_originalitemlist[_id].Exp;
        _addrich = pitemlist.player_originalitemlist[_id].Rich;
        _addsweat = pitemlist.player_originalitemlist[_id].Sweat;
        _addbitter = pitemlist.player_originalitemlist[_id].Bitter;
        _addsour = pitemlist.player_originalitemlist[_id].Sour;
        _addcrispy = pitemlist.player_originalitemlist[_id].Crispy;
        _addfluffy = pitemlist.player_originalitemlist[_id].Fluffy;
        _addsmooth = pitemlist.player_originalitemlist[_id].Smooth;
        _addhardness = pitemlist.player_originalitemlist[_id].Hardness;
        _addjiggly = pitemlist.player_originalitemlist[_id].Jiggly;
        _addchewy = pitemlist.player_originalitemlist[_id].Chewy;
        _addpowdery = pitemlist.player_originalitemlist[_id].Powdery;
        _addoily = pitemlist.player_originalitemlist[_id].Oily;
        _addwatery = pitemlist.player_originalitemlist[_id].Watery;
        _addbeauty = pitemlist.player_originalitemlist[_id].Beauty;
        _addtea_flavor = pitemlist.player_originalitemlist[_id].Tea_Flavor;
        _addsp_wind = pitemlist.player_originalitemlist[_id].SP_wind;
        _addsp_score2 = pitemlist.player_originalitemlist[_id].SP_Score2;
        _addsp_score3 = pitemlist.player_originalitemlist[_id].SP_Score3;
        _addsp_score4 = pitemlist.player_originalitemlist[_id].SP_Score4;
        _addsp_score5 = pitemlist.player_originalitemlist[_id].SP_Score5;
        _addsp_score6 = pitemlist.player_originalitemlist[_id].SP_Score6;
        _addsp_score7 = pitemlist.player_originalitemlist[_id].SP_Score7;
        _addsp_score8 = pitemlist.player_originalitemlist[_id].SP_Score8;
        _addsp_score9 = pitemlist.player_originalitemlist[_id].SP_Score9;
        _addsp_score10 = pitemlist.player_originalitemlist[_id].SP_Score10;
        _addbest_welldone = pitemlist.player_originalitemlist[_id].Best_Welldone;
        _addbase_score = pitemlist.player_originalitemlist[_id].Base_Score;
        _addgirl1_like = pitemlist.player_originalitemlist[_id].girl1_itemLike;
        _addcost = pitemlist.player_originalitemlist[_id].cost_price;
        _addsell = pitemlist.player_originalitemlist[_id].sell_price;
        _add_itemType = pitemlist.player_originalitemlist[_id].itemType.ToString();
        _add_itemType_sub = pitemlist.player_originalitemlist[_id].itemType_sub.ToString();
        _addmagic = pitemlist.player_originalitemlist[_id].Magic;


        //オリジナルアイテムを合成に使う場合も、固有トッピングを計算する。

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2 || Comp_method_bunki == 20) //オリジナル・レシピ調合時
        {
            for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _addtp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
            }

            for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
            {
                //_addkoyutp[i] = "Non";
                _addkoyutp[i] = pitemlist.player_originalitemlist[_id].koyu_toppingtype[i].ToString();
            }
        }
        else if (Comp_method_bunki == 3 || Comp_method_bunki == 22) //トッピング時
        {
            for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _addtp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
            }

            for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
            {
                //_addkoyutp[i] = "Non";
                _addkoyutp[i] = pitemlist.player_originalitemlist[_id].koyu_toppingtype[i].ToString();
            }
        }

        if (Comp_method_bunki == 20 || Comp_method_bunki == 22) //魔法調合時 計算時の個数は1の時のパラメータで計算する
        {
            //_addkosu = 1;
        }

        //Debug.Log("_addkosu: " + _addkosu);
        _additemlist.Add(new Item(0, "", "", _addname, "", "", 0, _addhp, _addday, _addquality, _addexp, 0, _addrich, _addsweat, _addbitter, _addsour,
        _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _addbeauty, 0, _addtea_flavor,
        _addsp_wind, _addsp_score2, _addsp_score3, _addsp_score4, _addsp_score5, _addsp_score6, _addsp_score7, _addsp_score8, _addsp_score9, _addsp_score10,
        _addbest_welldone,
        _add_itemType, _add_itemType_sub, "", "",
        _addbase_score, _addgirl1_like, _addcost, _addsell,
        _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9],
        _addkoyutp[0], _addkoyutp[1], _addkoyutp[2], _addkoyutp[3], _addkoyutp[4], _addkosu, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, _addmagic, 
        0, 0,
        "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0,
        "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0));
    }

    void Set_add_extremeparam()
    {
        _addname = pitemlist.player_extremepanel_itemlist[_id].itemName;
        _addhp = pitemlist.player_extremepanel_itemlist[_id].itemHP;
        _addday = pitemlist.player_extremepanel_itemlist[_id].item_day;
        _addquality = pitemlist.player_extremepanel_itemlist[_id].Quality;
        _addexp = pitemlist.player_extremepanel_itemlist[_id].Exp;
        _addrich = pitemlist.player_extremepanel_itemlist[_id].Rich;
        _addsweat = pitemlist.player_extremepanel_itemlist[_id].Sweat;
        _addbitter = pitemlist.player_extremepanel_itemlist[_id].Bitter;
        _addsour = pitemlist.player_extremepanel_itemlist[_id].Sour;
        _addcrispy = pitemlist.player_extremepanel_itemlist[_id].Crispy;
        _addfluffy = pitemlist.player_extremepanel_itemlist[_id].Fluffy;
        _addsmooth = pitemlist.player_extremepanel_itemlist[_id].Smooth;
        _addhardness = pitemlist.player_extremepanel_itemlist[_id].Hardness;
        _addjiggly = pitemlist.player_extremepanel_itemlist[_id].Jiggly;
        _addchewy = pitemlist.player_extremepanel_itemlist[_id].Chewy;
        _addpowdery = pitemlist.player_extremepanel_itemlist[_id].Powdery;
        _addoily = pitemlist.player_extremepanel_itemlist[_id].Oily;
        _addwatery = pitemlist.player_extremepanel_itemlist[_id].Watery;
        _addbeauty = pitemlist.player_extremepanel_itemlist[_id].Beauty;
        _addtea_flavor = pitemlist.player_extremepanel_itemlist[_id].Tea_Flavor;
        _addsp_wind = pitemlist.player_extremepanel_itemlist[_id].SP_wind;
        _addsp_score2 = pitemlist.player_extremepanel_itemlist[_id].SP_Score2;
        _addsp_score3 = pitemlist.player_extremepanel_itemlist[_id].SP_Score3;
        _addsp_score4 = pitemlist.player_extremepanel_itemlist[_id].SP_Score4;
        _addsp_score5 = pitemlist.player_extremepanel_itemlist[_id].SP_Score5;
        _addsp_score6 = pitemlist.player_extremepanel_itemlist[_id].SP_Score6;
        _addsp_score7 = pitemlist.player_extremepanel_itemlist[_id].SP_Score7;
        _addsp_score8 = pitemlist.player_extremepanel_itemlist[_id].SP_Score8;
        _addsp_score9 = pitemlist.player_extremepanel_itemlist[_id].SP_Score9;
        _addsp_score10 = pitemlist.player_extremepanel_itemlist[_id].SP_Score10;
        _addbest_welldone = pitemlist.player_extremepanel_itemlist[_id].Best_Welldone;
        _addbase_score = pitemlist.player_extremepanel_itemlist[_id].Base_Score;
        _addgirl1_like = pitemlist.player_extremepanel_itemlist[_id].girl1_itemLike;
        _addcost = pitemlist.player_extremepanel_itemlist[_id].cost_price;
        _addsell = pitemlist.player_extremepanel_itemlist[_id].sell_price;
        _add_itemType = pitemlist.player_extremepanel_itemlist[_id].itemType.ToString();
        _add_itemType_sub = pitemlist.player_extremepanel_itemlist[_id].itemType_sub.ToString();
        _addmagic = pitemlist.player_extremepanel_itemlist[_id].Magic;


        //オリジナルアイテムを合成に使う場合も、固有トッピングを計算する。

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2 || Comp_method_bunki == 20) //オリジナル・レシピ調合時
        {
            for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _addtp[i] = pitemlist.player_extremepanel_itemlist[_id].toppingtype[i].ToString();
            }

            for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
            {
                //_addkoyutp[i] = "Non";
                _addkoyutp[i] = pitemlist.player_extremepanel_itemlist[_id].koyu_toppingtype[i].ToString();
            }
        }
        else if (Comp_method_bunki == 3 || Comp_method_bunki == 22) //トッピング時
        {
            for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _addtp[i] = pitemlist.player_extremepanel_itemlist[_id].toppingtype[i].ToString();
            }

            for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
            {
                //_addkoyutp[i] = "Non";
                _addkoyutp[i] = pitemlist.player_extremepanel_itemlist[_id].koyu_toppingtype[i].ToString();
            }
        }

        if (Comp_method_bunki == 20 || Comp_method_bunki == 22) //魔法調合時 計算時の個数は1の時のパラメータで計算する
        {
            //_addkosu = 1;
        }

        //Debug.Log("_addkosu: " + _addkosu);
        _additemlist.Add(new Item(0, "", "", _addname, "", "", 0, _addhp, _addday, _addquality, _addexp, 0, _addrich, _addsweat, _addbitter, _addsour,
        _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _addbeauty, 0, _addtea_flavor,
        _addsp_wind, _addsp_score2, _addsp_score3, _addsp_score4, _addsp_score5, _addsp_score6, _addsp_score7, _addsp_score8, _addsp_score9, _addsp_score10,
        _addbest_welldone,
        _add_itemType, _add_itemType_sub, "", "",
        _addbase_score, _addgirl1_like, _addcost, _addsell,
        _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9],
        _addkoyutp[0], _addkoyutp[1], _addkoyutp[2], _addkoyutp[3], _addkoyutp[4], _addkosu, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, _addmagic, 
        0, 0,
        "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0,
        "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0, "Non", 0));
    }






    //味の計算処理
    void AddParam_Method()
    {
        //初期化
        etc_mat_count = 0;

       
        //Useだと、ブレンドし、最適比率との距離をだす計算
        if(databaseCompo.compoitems[result_compID].KeisanMethod == "Use")
        {
            keisan_method_flag = 1;
        }
        else //特定のアイテムの場合は、加算のみでOK。例えばアマンドファリーヌのような、粉同士を組み合わせただけ、など。
        {
            keisan_method_flag = 0;

        }

        for (i = 0; i < _additemlist.Count; i++)
        {
            AddTasteParam(); //各材料を加算していく。     
        }
        //DivisionTasteparam(); //その後、個数で割り算する。

        //Debug.Log("_additemlist.Count: " + _additemlist.Count);
        //Debug.Log("_tempbeauty check2 " + _tempbeauty);
    }

    void AddTasteParam()
    {
        //そのまま加算する。
        _temprich += _additemlist[i].Rich * _additemlist[i].ItemKosu;
        _tempsweat += _additemlist[i].Sweat * _additemlist[i].ItemKosu;
        _tempbitter += _additemlist[i].Bitter * _additemlist[i].ItemKosu;
        _tempsour += _additemlist[i].Sour * _additemlist[i].ItemKosu;
        _tempcrispy += _additemlist[i].Crispy * _additemlist[i].ItemKosu;
        _tempfluffy += _additemlist[i].Fluffy * _additemlist[i].ItemKosu;
        _tempsmooth += _additemlist[i].Smooth * _additemlist[i].ItemKosu;
        _temphardness += _additemlist[i].Hardness * _additemlist[i].ItemKosu;
        _tempjiggly += _additemlist[i].Jiggly * _additemlist[i].ItemKosu;
        _tempchewy += _additemlist[i].Chewy * _additemlist[i].ItemKosu;
        _temppowdery += _additemlist[i].Powdery * _additemlist[i].ItemKosu;
        _tempoily += _additemlist[i].Oily * _additemlist[i].ItemKosu;
        _tempwatery += _additemlist[i].Watery * _additemlist[i].ItemKosu;
        
        _temptea_flavor += _additemlist[i].Tea_Flavor * _additemlist[i].ItemKosu;

        if (_additemlist[i].Magic != 0) //ひとつでも魔法属性がついてたら、そっちに上書きされる。
        {
            _tempmagic = _additemlist[i].Magic;
        }



        if (mstatus != 99) //DB初期化のときのみ、見た目は加算しないようにする。二重に計算されるため。
        {
            if (_additemlist[i].itemType.ToString() != "Okashi")
            {
                _tempbeauty += _additemlist[i].Beauty * _additemlist[i].ItemKosu; //無くした　→　beuatyはもともとのお菓子のパラメータをベースに使うので、新規調合では計算から除外

                //SP系も同じ計算
                _tempsp_wind += _additemlist[i].SP_wind * _additemlist[i].ItemKosu;
                _tempsp_score2 += _additemlist[i].SP_Score2 * _additemlist[i].ItemKosu;
                _tempsp_score3 += _additemlist[i].SP_Score3 * _additemlist[i].ItemKosu;
                _tempsp_score4 += _additemlist[i].SP_Score4 * _additemlist[i].ItemKosu;
                _tempsp_score5 += _additemlist[i].SP_Score5 * _additemlist[i].ItemKosu;
                _tempsp_score6 += _additemlist[i].SP_Score6 * _additemlist[i].ItemKosu;
                _tempsp_score7 += _additemlist[i].SP_Score7 * _additemlist[i].ItemKosu;
                _tempsp_score8 += _additemlist[i].SP_Score8 * _additemlist[i].ItemKosu;
                _tempsp_score9 += _additemlist[i].SP_Score9 * _additemlist[i].ItemKosu;
                _tempsp_score10 += _additemlist[i].SP_Score10 * _additemlist[i].ItemKosu;
            }
            else
            {
                _tempbeauty = 0; //お菓子タイプの見た目は、加算しない

                _tempsp_wind = 0;
                _tempsp_score2 = 0;
                _tempsp_score3 = 0;
                _tempsp_score4 = 0;
                _tempsp_score5 = 0;
                _tempsp_score6 = 0;
                _tempsp_score7 = 0;
                _tempsp_score8 = 0;
                _tempsp_score9 = 0;
                _tempsp_score10 = 0;
            }
        }

        //Debug.Log("_basetea_flavor check " + _temptea_flavor);
        //Debug.Log("_additemlist[i]._Addkosu: " + _additemlist[i]._Addkosu);
    }

    void DivisionTasteparam()
    {
        //総個数で割り算する

        _temprich /= total_kosu;
        _tempsweat /= total_kosu;
        _tempbitter /= total_kosu;
        _tempsour /= total_kosu;
        _tempcrispy /= total_kosu;
        _tempfluffy /= total_kosu;
        _tempsmooth /= total_kosu;
        _temphardness /= total_kosu;
        _tempjiggly /= total_kosu;
        _tempchewy /= total_kosu;
        _temppowdery /= total_kosu;
        _tempoily /= total_kosu;
        _tempwatery /= total_kosu;
        _tempbeauty /= total_kosu;
        _temptea_flavor /= total_kosu;
        _tempsp_wind /= total_kosu;
        _tempsp_score2 /= total_kosu;
        _tempsp_score3 /= total_kosu;
        _tempsp_score4 /= total_kosu;
        _tempsp_score5 /= total_kosu;
        _tempsp_score6 /= total_kosu;
        _tempsp_score7 /= total_kosu;
        _tempsp_score8 /= total_kosu;
        _tempsp_score9 /= total_kosu;
        _tempsp_score10 /= total_kosu;
    }   


    //プレイヤーアイテムリストから、選んだ材料の削除処理 Exp_ControllerやTimeControllerからも読み出し。
    public void Delete_playerItemList(int _dstatus)
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        if (_dstatus == 1) //Exp_contorollerから読み込む場合
        {
            //パラメータの取得
            SetParamInit();
        }
        else if (_dstatus == 2) //ヒカリがお菓子作り中　材料だけ減らす処理
        {
            //パラメータの取得
            SetParamHikariMakeInit();
        }

        deleteOriginalList.Clear();
        deleteExtremeList.Clear();

        //生地合成、もしくはトッピング調合などの場合、ベースアイテムを、プレイヤーのアイテムリストから選んでる場合は、ベースアイテムの削除処理を行う。
        if (Comp_method_bunki == 1 || Comp_method_bunki == 3 || Comp_method_bunki == 22)             
        {

            //ベースアイテムを削除する。
            switch (base_toggle_type)
            {
                case 0: //プレイヤーアイテムリストから選択している。

                    _id = base_kettei_item;

                    if (Comp_method_bunki == 22) //魔法調合でCompNoやBufの場合
                    {
                        pitemlist.deletePlayerItem(database.items[_id].itemName, final_kette_kosu1);                     
                    }
                    else
                    {
                        pitemlist.deletePlayerItem(database.items[_id].itemName, base_kosu);
                    }
                    break;

                case 1: //オリジナルアイテムリストから選択している。

                    _id = base_kettei_item;

                    //オリジナルアイテムリストから削除するときは、一度削除用リストにIDをとりまとめて、後で、まとめて、降順で削除していく。
                    if (Comp_method_bunki == 22) //魔法調合でCompNoやBufの場合
                    {
                        deleteOriginalList.Add(_id, final_kette_kosu1);
                    }
                    else
                    {                        
                        deleteOriginalList.Add(_id, base_kosu);
                    }
                    break;

                case 2: //お菓子パネルアイテムリストから選択している。

                    _id = base_kettei_item;

                    //オリジナルアイテムリストから削除するときは、一度削除用リストにIDをとりまとめて、後で、まとめて、降順で削除していく。
                    if (Comp_method_bunki == 22) //魔法調合でCompNoやBufの場合
                    {
                        deleteExtremeList.Add(_id, final_kette_kosu1);
                    }
                    else
                    {
                        deleteExtremeList.Add(_id, base_kosu);
                    }
                    break;

                default:
                    break;
            }
        }

        //削除処理

        if (Comp_method_bunki == 2) //レシピで生成する場合の削除処理
        {
            //セット数分、判定を繰り返す
            for (count = 0; count < final_select_kaisu; count++)
            {
                DeleteMethod2();
            }
        }
        else if (Comp_method_bunki == 0 || Comp_method_bunki == 20) //オリジナルか魔法調合の新規作成時
        {
            final_kette_kosu1 = final_kette_kosu1 * final_select_kaisu;
            final_kette_kosu2 = final_kette_kosu2 * final_select_kaisu;
            final_kette_kosu3 = final_kette_kosu3 * final_select_kaisu;

            DeleteMethod1();
        }
        else
        {
            DeleteMethod1();
        }        
    }

    //一括で、アイテムを削除するパターン
    void DeleteMethod1()
    {
        //トッピングアイテム①を削除する。
        switch (toggle_type1)
        {
            case 0: //プレイヤーアイテムリストから選択している。ただちに削除

                _id = kettei_item1;
                //Debug.Log("_id: " + _id + " final_kette_kosu1: " + final_kette_kosu1);

                //器具と一部の特殊なデータは、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine" || database.items[_id].itemType_sub.ToString() == "MagicData")
                {

                }
                else
                {
                    pitemlist.deletePlayerItem(database.items[_id].itemName, final_kette_kosu1);
                }
                break;

            case 1: //オリジナルアイテムリストから選択している。オリジナルの場合は、一度削除用リストにIDを追加し、降順にしてから、後の削除メソッドでまとめて削除する。

                _id = kettei_item1;

                deleteOriginalList.Add(_id, final_kette_kosu1);
                break;

            case 2: //お菓子パネルリストから選択している。

                _id = kettei_item1;

                deleteExtremeList.Add(_id, final_kette_kosu1);
                break;

            default:
                break;
        }

        if (kettei_item2 != 9999) //二個目のトッピングアイテムを選んでいなければ、この処理は無視する。
        {
            //トッピングアイテム②を削除する。
            switch (toggle_type2)
            {
                case 0: //プレイヤーアイテムリストから選択している。

                    _id = kettei_item2;
                    //Debug.Log("_id: " + _id + " final_kette_kosu2: " + final_kette_kosu2);

                    //器具と一部の特殊なデータは、削除しない
                    if (database.items[_id].itemType_sub.ToString() == "Machine" || database.items[_id].itemType_sub.ToString() == "MagicData")
                    {

                    }
                    else
                    {
                        pitemlist.deletePlayerItem(database.items[_id].itemName, final_kette_kosu2);
                    }
                    break;

                case 1: //オリジナルアイテムリストから選択している。オリジナルの場合は、一度削除用リストにIDを追加し、降順にしてから、後の削除メソッドでまとめて削除する。

                    _id = kettei_item2;

                    deleteOriginalList.Add(_id, final_kette_kosu2);
                    break;

                case 2: //お菓子パネルリストから選択している。

                    _id = kettei_item2;

                    deleteExtremeList.Add(_id, final_kette_kosu2);
                    break;

                default:
                    break;
            }
        }

        if (kettei_item3 != 9999) //三個目のトッピングアイテムを選んでいなければ、この処理は無視する。
        {
            //トッピングアイテム③を削除する。
            switch (toggle_type3)
            {
                case 0: //プレイヤーアイテムリストから選択している。

                    _id = kettei_item3;
                    //Debug.Log("_id: " + _id + " final_kette_kosu3: " + final_kette_kosu3);

                    //器具と一部の特殊なデータは、削除しない
                    if (database.items[_id].itemType_sub.ToString() == "Machine" || database.items[_id].itemType_sub.ToString() == "MagicData")
                    {

                    }
                    else
                    {
                        pitemlist.deletePlayerItem(database.items[_id].itemName, final_kette_kosu3);
                    }
                    break;

                case 1: //オリジナルアイテムリストから選択している。オリジナルの場合は、一度削除用リストにIDを追加し、降順にしてから、後の削除メソッドでまとめて削除する。

                    _id = kettei_item3;

                    deleteOriginalList.Add(_id, final_kette_kosu3);
                    break;

                case 2: //お菓子パネルリストから選択している。

                    _id = kettei_item3;

                    deleteExtremeList.Add(_id, final_kette_kosu3);
                    break;

                default:
                    break;
            }
        }

        //オリジナルアイテムリストからアイテムを選んでる場合の削除処理
        if (deleteOriginalList.Count > 0)
        {
            Debug.Log("調合にオリジナルアイテムを使用した");

            //オリジナルアイテムをトッピングに使用していた場合の削除処理。削除用リストに入れた分をもとに、削除の処理を行う。
            var newTable = deleteOriginalList.OrderByDescending(value => value.Key); //降順にする

            foreach (KeyValuePair<int, int> deletePair in newTable)
            {
                pitemlist.deleteOriginalItem(deletePair.Key, deletePair.Value);
                
                //Debug.Log("delete_originID: " + deletePair.Key + " 個数:" + deletePair.Value);
            }
        }

        //エクストリームアイテムリストからアイテムを選んでる場合の削除処理
        if (deleteExtremeList.Count > 0)
        {
            Debug.Log("調合にお菓子パネルアイテムを使用した");

            //エクストリームアイテムをトッピングに使用していた場合の削除処理。削除用リストに入れた分をもとに、削除の処理を行う。
            var newTable = deleteExtremeList.OrderByDescending(value => value.Key); //降順にする

            foreach (KeyValuePair<int, int> deletePair in newTable)
            {
                pitemlist.deleteExtremePanelItem(deletePair.Key, deletePair.Value);
            }
        }
    }

    //セット数分繰り返し　店売りとオリジナルアイテムをそれぞれ所持数を判定しながら削除するパターン
    //レシピ調合現在使用していないため、お菓子パネルに入っているものは、材料としてカウントしないようにしてる。余力があれば、こちらもカウントしたほうがよい。
    void DeleteMethod2()
    {
        deleteOriginalList.Clear();
        deleteExtremeList.Clear();

        //一個目　final_kettei_kosu1より、店売りアイテムを多く持っているか、もしくは同じ。それより足りない場合に、オリジナルアイテムを見始める。
        if (pitemlist.playeritemlist[database.items[kettei_item1].itemName] >= final_kette_kosu1)
        {
            _id = kettei_item1;
            //Debug.Log("_id: " + _id + " final_kette_kosu1: " + final_kette_kosu1);

            //器具と一部の特殊なデータは、削除しない
            if (database.items[_id].itemType_sub.ToString() == "Machine" || database.items[_id].itemType_sub.ToString() == "MagicData")
            {

            }
            else
            {
                pitemlist.deletePlayerItem(database.items[_id].itemName, final_kette_kosu1);
            }
        }
        else //足りてないときは、残りの店売り分を削除し、残り数値分でオリジナルのほうを削除しはじめる。
        {
            _id = kettei_item1;
            //Debug.Log("_id: " + _id + " final_kette_kosu1: " + final_kette_kosu1);

            nokori_kosu = final_kette_kosu1 - pitemlist.playeritemlist[database.items[kettei_item1].itemName];

            //器具と一部の特殊なデータは、削除しない
            if (database.items[_id].itemType_sub.ToString() == "Machine" || database.items[_id].itemType_sub.ToString() == "MagicData")
            {

            }
            else
            {
                if (pitemlist.playeritemlist[database.items[kettei_item1].itemName] > 0)
                {
                    pitemlist.deletePlayerItem(database.items[_id].itemName, pitemlist.playeritemlist[database.items[kettei_item1].itemName]);
                }
            }

            //オリジナルを探索する。頭から順番にみて、個数を消していく。
            i = 0;
            while( i < pitemlist.player_originalitemlist.Count)
            {
                if(pitemlist.player_originalitemlist[i].itemName == database.items[kettei_item1].itemName)
                {
                    if(pitemlist.player_originalitemlist[i].ItemKosu >= nokori_kosu )
                    {
                        deleteOriginalList.Add(i, nokori_kosu);
                        break;
                    }
                    else
                    {
                        nokori_kosu -= pitemlist.player_originalitemlist[i].ItemKosu;
                        deleteOriginalList.Add(i, pitemlist.player_originalitemlist[i].ItemKosu);
                    }
                }
                i++;
            }

        }       

        if (kettei_item2 != 9999) //二個目のトッピングアイテムを選んでいなければ、この処理は無視する。
        {
            //二個目　final_kettei_kosu1より、店売りアイテムを多く持っているか、もしくは同じ。それより足りない場合に、オリジナルアイテムを見始める。
            if (pitemlist.playeritemlist[database.items[kettei_item2].itemName] >= final_kette_kosu2)
            {
                _id = kettei_item2;
                //Debug.Log("_id: " + _id + " final_kette_kosu1: " + final_kette_kosu1);

                //器具と一部の特殊なデータは、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine" || database.items[_id].itemType_sub.ToString() == "MagicData")
                {

                }
                else
                {
                    pitemlist.deletePlayerItem(database.items[_id].itemName, final_kette_kosu2);
                }
            }
            else //足りてないときは、残りの店売り分を削除し、残り数値分でオリジナルのほうを削除しはじめる。
            {
                _id = kettei_item2;

                nokori_kosu = final_kette_kosu2 - pitemlist.playeritemlist[database.items[kettei_item2].itemName];

                //器具と一部の特殊なデータは、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine" || database.items[_id].itemType_sub.ToString() == "MagicData")
                {

                }
                else
                {
                    if (pitemlist.playeritemlist[database.items[kettei_item2].itemName] > 0)
                    {
                        pitemlist.deletePlayerItem(database.items[_id].itemName, pitemlist.playeritemlist[database.items[kettei_item2].itemName]);
                    }
                }

                //オリジナルを探索する。頭から順番にみて、個数を消していく。
                i = 0;
                while (i < pitemlist.player_originalitemlist.Count)
                {
                    if (pitemlist.player_originalitemlist[i].itemName == database.items[kettei_item2].itemName)
                    {
                        if (pitemlist.player_originalitemlist[i].ItemKosu >= nokori_kosu)
                        {
                            deleteOriginalList.Add(i, nokori_kosu);
                            break;
                        }
                        else
                        {
                            nokori_kosu -= pitemlist.player_originalitemlist[i].ItemKosu;
                            deleteOriginalList.Add(i, pitemlist.player_originalitemlist[i].ItemKosu);
                        }
                    }
                    i++;
                }

            }
        }

        if (kettei_item3 != 9999) //三個目のトッピングアイテムを選んでいなければ、この処理は無視する。
        {
            //三個目　final_kettei_kosu1より、店売りアイテムを多く持っているか、もしくは同じ。それより足りない場合に、オリジナルアイテムを見始める。
            if (pitemlist.playeritemlist[database.items[kettei_item3].itemName] >= final_kette_kosu3)
            {
                _id = kettei_item3;

                //器具と一部の特殊なデータは、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine" || database.items[_id].itemType_sub.ToString() == "MagicData")
                {

                }
                else
                {
                    pitemlist.deletePlayerItem(database.items[_id].itemName, final_kette_kosu3);
                }
            }
            else //足りてないときは、残りの店売り分を削除し、残り数値分でオリジナルのほうを削除しはじめる。
            {
                _id = kettei_item3;

                nokori_kosu = final_kette_kosu3 - pitemlist.playeritemlist[database.items[kettei_item3].itemName];

                //器具と一部の特殊なデータは、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine" || database.items[_id].itemType_sub.ToString() == "MagicData")
                {

                }
                else
                {
                    if (pitemlist.playeritemlist[database.items[kettei_item3].itemName] > 0)
                    {
                        pitemlist.deletePlayerItem(database.items[_id].itemName, pitemlist.playeritemlist[database.items[kettei_item3].itemName]);
                    }
                }

                //オリジナルを探索する。頭から順番にみて、個数を消していく。
                i = 0;
                while (i < pitemlist.player_originalitemlist.Count)
                {
                    if (pitemlist.player_originalitemlist[i].itemName == database.items[kettei_item3].itemName)
                    {
                        if (pitemlist.player_originalitemlist[i].ItemKosu >= nokori_kosu)
                        {
                            deleteOriginalList.Add(i, nokori_kosu);
                            break;
                        }
                        else
                        {
                            nokori_kosu -= pitemlist.player_originalitemlist[i].ItemKosu;
                            deleteOriginalList.Add(i, pitemlist.player_originalitemlist[i].ItemKosu);
                        }
                    }
                    i++;
                }
            }
        }

        //オリジナルアイテムリストからアイテムを選んでる場合の削除処理
        if (deleteOriginalList.Count > 0)
        {
            Debug.Log("調合にオリジナルアイテムを使用した");

            //オリジナルアイテムをトッピングに使用していた場合の削除処理。削除用リストに入れた分をもとに、削除の処理を行う。
            var newTable = deleteOriginalList.OrderByDescending(value => value.Key); //降順にする

            foreach (KeyValuePair<int, int> deletePair in newTable)
            {
                pitemlist.deleteOriginalItem(deletePair.Key, deletePair.Value);
                //Debug.Log("delete_originID: " + deletePair.Key + " 個数:" + deletePair.Value);
            }
        }
    }

    void Debug_AddKeisanCheck(string _cmpname, int _debugstatus)
    {
        if (databaseCompo.compoitems[result_compID].cmpitem_Name == _cmpname)
        {
            switch(_debugstatus)
            {
                case 0:

                    Debug.Log("ゲーム初期設定　パラメータ確認　加算前");
                    break;

                case 10:

                    Debug.Log("ゲーム初期設定　パラメータ確認　加算後　距離計算前");
                    break;

                case 20:

                    Debug.Log("ゲーム初期設定　パラメータ確認　距離計算後");
                    Debug.Log("ベスト配合との距離 totalkyori: " + totalkyori);
                    Debug.Log("kyori補正　この値を、各食感の値に掛け算: " + kyori_hosei); 
                    break;
            }
            Debug.Log("_basecrispy: " + _basecrispy);
            Debug.Log("_basefluffy: " + _basefluffy);
            Debug.Log("_basesmooth: " + _basesmooth);
            Debug.Log("_basehardness: " + _basehardness);            
        }
    }

    void Debug_TastePanel()
    {
        Debug.Log("名前: " + _basename + " _basehp: " + _basehp + " _baseday: " + _baseday + " 品質: " + _basequality);
        Debug.Log("甘さ: " + _basesweat + " 苦さ: " + _basebitter + " 酸味: " + _basesour);
        Debug.Log("コク: " + _baserich);
        Debug.Log("さくさく感: " + _basecrispy + " ふわふわ感: " + _basefluffy);
        Debug.Log("しっとり感: " + _basesmooth + " 歯ごたえ: " + _basehardness);
        Debug.Log("ジュースののどごし: " + _basejuice + " 香り: " + _basetea_flavor);
        Debug.Log("_basejiggly: " + _basejiggly + " _basechewy: " + _basechewy);
        Debug.Log("粉っぽさ: " + _basepowdery + " 油っぽさ: " + _baseoily + " 水っぽさ: " + _basewatery);
        Debug.Log("見た目: " + _basebeauty);
        Debug.Log("_basesp_wind: " + _basesp_wind + "_basesp_score2: " + _basesp_score2 + "_basesp_score3: " + _basesp_score3);
        Debug.Log("_basesp_score4: " + _basesp_score4 + "_basesp_score5: " + _basesp_score5 + "_basesp_score6: " + _basesp_score6);
        Debug.Log("_basesp_score7: " + _basesp_score7 + "_basesp_score8: " + _basesp_score8 + "_basesp_score9: " + _basesp_score9);
        Debug.Log("_basesp_score10: " + _basesp_score10);
        Debug.Log("_basegirl1_like:" + _basegirl1_like + " _basecost:" + _basecost + " _basesell:" + _basesell);
        Debug.Log("_base_itemType:" + _base_itemType + " _base_itemType_sub:" + _base_itemType_sub);

        Debug.Log("スロット1: " + _basetp[0]);
        Debug.Log("スロット2: " + _basetp[1]);
        Debug.Log("スロット3: " + _basetp[2]);
        Debug.Log("スロット4: " + _basetp[3]);
        Debug.Log("スロット5: " + _basetp[4]);
        Debug.Log("スロット6: " + _basetp[5]);
        Debug.Log("スロット7: " + _basetp[6]);
        Debug.Log("スロット8: " + _basetp[7]);
        Debug.Log("スロット9: " + _basetp[8]);
        Debug.Log("スロット10: " + _basetp[9]);

        Debug.Log("魔法状態1: " + _baseMS[0] + " " + _baseMSvalue[0]);
        Debug.Log("魔法状態2: " + _baseMS[1] + " " + _baseMSvalue[1]);
        Debug.Log("魔法状態3: " + _baseMS[2] + " " + _baseMSvalue[2]);
        Debug.Log("魔法状態4: " + _baseMS[3] + " " + _baseMSvalue[3]);
        Debug.Log("魔法状態5: " + _baseMS[4] + " " + _baseMSvalue[4]);
        Debug.Log("魔法状態6: " + _baseMS[5] + " " + _baseMSvalue[5]);
        Debug.Log("魔法状態7: " + _baseMS[6] + " " + _baseMSvalue[6]);
        Debug.Log("魔法状態8: " + _baseMS[7] + " " + _baseMSvalue[7]);
        Debug.Log("魔法状態9: " + _baseMS[8] + " " + _baseMSvalue[8]);
        Debug.Log("魔法状態10: " + _baseMS[9] + " " + _baseMSvalue[9]);
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
