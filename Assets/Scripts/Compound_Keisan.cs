﻿using System.Collections;
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

    private Exp_Controller exp_Controller;
    private ExtremePanel extreme_panel;

    private CombinationMain Combinationmain;

    private Buf_Power_Keisan bufpower_keisan;

    private SlotChangeName slotchangename;
    private string[] _slotHyouji1; //日本語に変換後の表記を格納する。スロット覧用
    private string itemslotname;
    private string itemfullname;

    private int i, j, n, count;
    private int itemNum, DBcount;

    private int total_qbox_money;

    private List<string> _itemIDtemp_result = new List<string>(); //調合リスト。アイテムネームに変換し、格納しておくためのリスト。itemNameと一致する。
    private List<string> _itemSubtype_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<int> _itemKosutemp_result = new List<int>(); //調合の個数組み合わせ。

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
    private int result_ID;
    private int new_item;

    private bool Pate_flag;
    private int result_kosu;

    private string kettei_originalitemID1;
    private string kettei_originalitemID2;
    private string kettei_originalitemID3;



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
    public int _base_extreme_kaisu;
    public int _base_item_hyouji;
    public string _base_itemdesc;

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
    private int _addbase_score;
    private float _addgirl1_like;
    private int _addcost;
    private int _addsell;
    private string[] _addtp;
    private string[] _addkoyutp;
    private string _add_itemType;
    private string _add_itemType_sub;
    private int _addkosu;

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
    private float _tempgirl1_like;
    private int _tempcost;
    private int _tempsell;
    private string[] _temptp;

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
    private List<ItemAdd> _additemlist = new List<ItemAdd>();


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

        //
        //アイテムデータベースの味パラムを初期化。初期化は、ゲーム起動時の一回のみ。
        //
        ResetDefaultTasteParam();

    }

    // Update is called once per frame
    void Update() {

    }

    public void ResetDefaultTasteParam()
    {
        for (DBcount = 0; DBcount < databaseCompo.compoitems.Count; DBcount++)
        {
            if (databaseCompo.compoitems[DBcount].cmpitem_Name != "") //名前が空白の場合は無視する
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
                    result_ID = DBcount;

                    Topping_Compound_Method(99);
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
            if (exp_Controller.extreme_on != true)
            {
                //**重要** 
                //kettei_itemは、プレイヤーリストのリスト番号が入っている。店売り 0, 1, 2, 3... , オリジナルリスト 0, 1, 2...といった具合。
                //店売りの場合は、実質アイテムIDと数字は一緒。
                //toggle_typeは、店売り(=0)か、オリジナルアイテム(=1)の判定。

                kettei_item1 = pitemlistController.kettei_item1;
                kettei_item2 = pitemlistController.kettei_item2;
                kettei_item3 = pitemlistController.kettei_item3;

                toggle_type1 = pitemlistController._toggle_type1;
                toggle_type2 = pitemlistController._toggle_type2;
                toggle_type3 = pitemlistController._toggle_type3;

                final_kette_kosu1 = pitemlistController.final_kettei_kosu1;
                final_kette_kosu2 = pitemlistController.final_kettei_kosu2;
                final_kette_kosu3 = pitemlistController.final_kettei_kosu3;

                _before_itemtype_Sub = "";

                //Debug.Log("pitemlistController.final_kettei_kosu1: " + final_kette_kosu1);
                //Debug.Log("pitemlistController.final_kettei_kosu2: " + final_kette_kosu2);
            }
            else //仕上げで新しく閃く場合
            {
                kettei_item1 = pitemlistController.base_kettei_item;
                kettei_item2 = pitemlistController.kettei_item1;
                kettei_item3 = pitemlistController.kettei_item2;

                toggle_type1 = pitemlistController._base_toggle_type;
                toggle_type2 = pitemlistController._toggle_type1;
                toggle_type3 = pitemlistController._toggle_type2;

                final_kette_kosu1 = pitemlistController.final_base_kettei_kosu;
                final_kette_kosu2 = pitemlistController.final_kettei_kosu1;
                final_kette_kosu3 = pitemlistController.final_kettei_kosu2;

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
            result_item = pitemlistController.result_item;

            //コンポ調合データベースのIDを代入
            result_ID = pitemlistController.result_compID;
        }

        if (Comp_method_bunki == 2) //レシピ調合の場合
        {
            //レシピの場合。使うアイテムを自動的に選択する。
            //今のところ、店売りアイテムのみでしか、レシピの材料にならないので、以下の定め方にしている。もし、オリジナルアイテムから使う場合は、toggle_typeなどの判定がちゃんと必要。

            kettei_item1 = recipilistController.kettei_recipiitem1;
            kettei_item2 = recipilistController.kettei_recipiitem2;
            kettei_item3 = recipilistController.kettei_recipiitem3;

            toggle_type1 = 0;
            toggle_type2 = 0;
            toggle_type3 = 0;

            final_kette_kosu1 = recipilistController.final_kettei_recipikosu1; //一回あたりの必要個数×セット回数
            final_kette_kosu2 = recipilistController.final_kettei_recipikosu2;
            final_kette_kosu3 = recipilistController.final_kettei_recipikosu3;

            final_select_kaisu = recipilistController.final_select_kosu;

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
            result_item = recipilistController.result_recipiitem;

            //コンポ調合データベースのIDを代入
            result_ID = recipilistController.result_recipicompID;
        }

        if (Comp_method_bunki == 3) //トッピング調合の場合
        {
            //プレイヤーリストコントローラーで更新した変数を、こっちでも一度代入
            kettei_item1 = pitemlistController.kettei_item1;
            kettei_item2 = pitemlistController.kettei_item2;
            kettei_item3 = pitemlistController.kettei_item3;
            base_kettei_item = pitemlistController.base_kettei_item;

            toggle_type1 = pitemlistController._toggle_type1;
            toggle_type2 = pitemlistController._toggle_type2;
            toggle_type3 = pitemlistController._toggle_type3;
            base_toggle_type = pitemlistController._base_toggle_type;


            if (pitemlistController.final_kettei_item2 == 9999) //2個目が空の場合、トッピングは一個のみ。
            {
                kettei_item2 = 9999;
                kettei_item3 = 9999;
            }

            if (pitemlistController.final_kettei_item3 == 9999) //3個目が空の場合、トッピングは二個のみ。
            {
                kettei_item3 = 9999;
            }

            base_kosu = 1;
            final_kette_kosu1 = pitemlistController.final_kettei_kosu1;
            final_kette_kosu2 = pitemlistController.final_kettei_kosu2;
            final_kette_kosu3 = pitemlistController.final_kettei_kosu3;

            //オリジナル・トッピングは、現在のところ、1セットのみの対応
            final_select_kaisu = 1;
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
            result_ID = GameMgr.hikari_make_okashi_compID;

            exp_Controller.result_ok = true; //オリジナル調合扱い
            exp_Controller.DoubleItemCreated = GameMgr.hikari_make_doubleItemCreated;
        }
    }


    //ゲーム最初に、アイテムデータベースの味パラメータを、コンポDBから計算して初期化
    void SetParamDatabaseInit()
    {

        Comp_method_bunki = 2;

        i = 0;
        while (i < database.items.Count)
        {
            if (databaseCompo.compoitems[result_ID].cmpitemID_1 == database.items[i].itemName)
            {
                kettei_item1 = i;
                break;
            }
            i++;
        }

        i = 0;
        while (i < database.items.Count)
        {
            if (databaseCompo.compoitems[result_ID].cmpitemID_2 == database.items[i].itemName)
            {
                kettei_item2 = i;
                break;
            }
            i++;
        }

        i = 0;
        while (i < database.items.Count)
        {
            if (databaseCompo.compoitems[result_ID].cmpitemID_3 == database.items[i].itemName)
            {
                kettei_item3 = i;
                break;
            }
            i++;
        }

        toggle_type1 = 0;
        toggle_type2 = 0;
        toggle_type3 = 0;

        final_kette_kosu1 = databaseCompo.compoitems[result_ID].cmpitem_kosu1;
        final_kette_kosu2 = databaseCompo.compoitems[result_ID].cmpitem_kosu2;
        final_kette_kosu3 = databaseCompo.compoitems[result_ID].cmpitem_kosu3;

        if (final_kette_kosu2 == 9999) //2個目が空の場合、トッピングは一個のみ。
        {
            kettei_item2 = 9999;
            kettei_item3 = 9999;
        }

        if (final_kette_kosu3 == 9999) //3個目が空の場合、トッピングは二個のみ。
        {
            kettei_item3 = 9999;
        }

        //**ここまで**
    }

    void SetParamKosuHosei()
    {
        _itemIDtemp_result.Clear();
        _itemKosutemp_result.Clear();
        _itemSubtype_temp_result.Clear();

        _itemIDtemp_result.Add(database.items[kettei_item1].itemName);
        _itemIDtemp_result.Add(database.items[kettei_item2].itemName);

        _itemSubtype_temp_result.Add(database.items[kettei_item1].itemType_sub.ToString());
        _itemSubtype_temp_result.Add(database.items[kettei_item2].itemType_sub.ToString());

        _itemKosutemp_result.Add(final_kette_kosu1);
        _itemKosutemp_result.Add(final_kette_kosu2);

        if (final_kette_kosu3 == 9999) //二個しか選択していないときは、9999が入っている。
        {
            _itemIDtemp_result.Add("empty");
            _itemSubtype_temp_result.Add("empty");
            _itemKosutemp_result.Add(final_kette_kosu3);
        }
        else
        {
            _itemIDtemp_result.Add(database.items[kettei_item3].itemName);
            _itemSubtype_temp_result.Add(database.items[kettei_item3].itemType_sub.ToString());
            _itemKosutemp_result.Add(final_kette_kosu3);
        }


        compoDB_select_judge = false;


        //判定処理//

        //一個目に選んだアイテムが生地タイプでもなく、フルーツ同士の合成でもない場合、
        //新規作成のため、以下の判定処理を行う。個数は、判定に関係しない。


        //①固有の名称同士の組み合わせか、②固有＋サブの組み合わせか、③サブ同士のジャンルで組み合わせが一致していれば、制作する。

        //①３つの入力をもとに、組み合わせ計算するメソッド＜固有名称の組み合わせ確認＞     
        Combinationmain.Combination(_itemIDtemp_result.ToArray(), _itemKosutemp_result.ToArray(), 99); //決めた３つのアイテム＋それぞれの個数、の配列

        compoDB_select_judge = Combinationmain.compFlag;


        //②　①の組み合わせにない場合は、2通りが考えられる。　アイテム名＋サブ＋サブ　か　アイテム名＋アイテム名＋サブの組み合わせ
        if (compoDB_select_judge == false)
        {
            //個数計算していないので、バグあり
            Combinationmain.Combination2(_itemIDtemp_result.ToArray(), _itemSubtype_temp_result.ToArray(), _itemKosutemp_result.ToArray(), 99);

            compoDB_select_judge = Combinationmain.compFlag;
        }


        //③固有の組み合わせがなかった場合のみ、サブジャンル同士の組み合わせがないかも見る。サブ＋サブ＋サブ

        if (compoDB_select_judge == false)
        {
            Combinationmain.Combination3(_itemSubtype_temp_result.ToArray(), _itemKosutemp_result.ToArray(), 99);

            compoDB_select_judge = Combinationmain.compFlag;
        }
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
            SetParamKosuHosei();
        }


        //ベースアイテム　タイプを見て、プレイヤリストアイテムかオリジナルアイテムかを識別する。
        if (Comp_method_bunki == 0 || Comp_method_bunki == 2) //新規にアイテムを作成する場合 or レシピ調合の場合。空のパラメータに、材料のパラメータを総計していく。
        {
            _id = result_item;

            if (_mstatus == 99) //ゲーム開始時のみ使用。
            {
                if (database.items[_id].itemComp_Hosei == 0) //アイテム自体が持っている値を加算しない場合
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
                    _basegirl1_like = database.items[_id].girl1_itemLike;
                    _basecost = database.items[_id].cost_price;
                    _basesell = database.items[_id].sell_price;
                    _base_itemType = database.items[_id].itemType.ToString();
                    _base_itemType_sub = database.items[_id].itemType_sub.ToString();
                    _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
                    _base_item_hyouji = database.items[_id].item_Hyouji;
                    _base_itemdesc = database.items[_id].itemDesc;
                }
                else //アイテム自体が持っている値を加算する補正の処理。データベース最初の初期化のときのみ。
                {
                    //各パラメータを取得
                    _baseID = database.items[_id].itemID;
                    _basename = database.items[_id].itemName;
                    _basehp = database.items[_id].itemHP;
                    _baseday = 0;
                    _basequality = 0;
                    _baseexp = 0;
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
                    _basegirl1_like = database.items[_id].girl1_itemLike;
                    _basecost = database.items[_id].cost_price;
                    _basesell = database.items[_id].sell_price;
                    _base_itemType = database.items[_id].itemType.ToString();
                    _base_itemType_sub = database.items[_id].itemType_sub.ToString();
                    _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
                    _base_item_hyouji = database.items[_id].item_Hyouji;
                    _base_itemdesc = database.items[_id].itemDesc;
                }
            }
            else
            {
                if (database.items[_id].itemComp_Hosei == 0) //アイテム自体が持っている値を加算しない場合
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
                    _basegirl1_like = database.items[_id].girl1_itemLike;
                    _basecost = database.items[_id].cost_price;
                    _basesell = database.items[_id].sell_price;
                    _base_itemType = database.items[_id].itemType.ToString();
                    _base_itemType_sub = database.items[_id].itemType_sub.ToString();
                    _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
                    _base_item_hyouji = database.items[_id].item_Hyouji;
                    _base_itemdesc = database.items[_id].itemDesc;
                }
                else //アイテム自体が持っている値を加算する補正の処理。
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
                    _basegirl1_like = database.items_gamedefault[_id].girl1_itemLike;
                    _basecost = database.items_gamedefault[_id].cost_price;
                    _basesell = database.items_gamedefault[_id].sell_price;
                    _base_itemType = database.items_gamedefault[_id].itemType.ToString();
                    _base_itemType_sub = database.items_gamedefault[_id].itemType_sub.ToString();
                    _base_extreme_kaisu = database.items_gamedefault[_id].ExtremeKaisu;
                    _base_item_hyouji = database.items_gamedefault[_id].item_Hyouji;
                    _base_itemdesc = database.items_gamedefault[_id].itemDesc;
                }
            }

            for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _basetp[i] = "Non";
            }

        }
        else if (Comp_method_bunki == 1 || Comp_method_bunki == 3) //生地合成、もしくはトッピング調合の場合。　一個目に選んだアイテムをベースに、リザルトアイテムにする。
        {
            switch (base_toggle_type)
            {
                case 0: //プレイヤーアイテムリストから選択している。

                    _id = base_kettei_item;

                    //各パラメータを取得
                    _baseID = database.items[_id].itemID;
                    _basename = database.items[_id].itemName;
                    _basehp = database.items[_id].itemHP;
                    _baseday = database.items[_id].item_day;
                    _basequality = database.items[_id].Quality;
                    _baseexp = 0; //元アイテムの経験値は影響なし。材料のみの経験値を加算する。
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
                    _basegirl1_like = database.items[_id].girl1_itemLike;
                    _basecost = database.items[_id].cost_price;
                    _basesell = database.items[_id].sell_price;
                    _base_itemType = database.items[_id].itemType.ToString();
                    _base_itemType_sub = database.items[_id].itemType_sub.ToString();
                    _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
                    _base_item_hyouji = database.items[_id].item_Hyouji;
                    _base_itemdesc = database.items[_id].itemDesc;

                    _base_extreme_kaisu--;

                    for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                    {
                        _basetp[i] = database.items[_id].toppingtype[i].ToString();
                    }

                    //result_itemに、アイテムIDを入れる。
                    result_item = _id;

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
                    _basegirl1_like = pitemlist.player_originalitemlist[_id].girl1_itemLike;
                    _basecost = pitemlist.player_originalitemlist[_id].cost_price;
                    _basesell = pitemlist.player_originalitemlist[_id].sell_price;
                    _base_itemType = pitemlist.player_originalitemlist[_id].itemType.ToString();
                    _base_itemType_sub = pitemlist.player_originalitemlist[_id].itemType_sub.ToString();
                    _base_extreme_kaisu = pitemlist.player_originalitemlist[_id].ExtremeKaisu;
                    _base_item_hyouji = pitemlist.player_originalitemlist[_id].item_Hyouji;
                    _base_itemdesc = pitemlist.player_originalitemlist[_id].itemDesc;

                    _base_extreme_kaisu--;

                    for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                    {
                        _basetp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
                    }

                    //result_itemに、アイテムIDを入れる。
                    //データベースから_nameに一致するものを取得。
                    i = 0;

                    while (i < database.items.Count)
                    {

                        if (database.items[i].itemName == _basename)
                        {
                            result_item = database.items[i].itemID; //アイテムIDのこと。
                            break;
                        }
                        ++i;
                    }

                    break;

                case 2: //お菓子パネルアイテムリストから選択している場合

                    //さらに、オリジナルのプレイヤーアイテムリストの番号を参照する。

                    _id = base_kettei_item;

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
                    _basegirl1_like = pitemlist.player_extremepanel_itemlist[_id].girl1_itemLike;
                    _basecost = pitemlist.player_extremepanel_itemlist[_id].cost_price;
                    _basesell = pitemlist.player_extremepanel_itemlist[_id].sell_price;
                    _base_itemType = pitemlist.player_extremepanel_itemlist[_id].itemType.ToString();
                    _base_itemType_sub = pitemlist.player_extremepanel_itemlist[_id].itemType_sub.ToString();
                    _base_extreme_kaisu = pitemlist.player_extremepanel_itemlist[_id].ExtremeKaisu;
                    _base_item_hyouji = pitemlist.player_extremepanel_itemlist[_id].item_Hyouji;
                    _base_itemdesc = pitemlist.player_extremepanel_itemlist[_id].itemDesc;

                    _base_extreme_kaisu--;

                    for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                    {
                        _basetp[i] = pitemlist.player_extremepanel_itemlist[_id].toppingtype[i].ToString();
                    }

                    //result_itemに、アイテムIDを入れる。
                    //データベースから_nameに一致するものを取得。
                    i = 0;

                    while (i < database.items.Count)
                    {

                        if (database.items[i].itemName == _basename)
                        {
                            result_item = database.items[i].itemID; //アイテムIDのこと。
                            break;
                        }
                        ++i;
                    }

                    break;

                default:
                    break;
            }
        }




        AddParamMethod(); //決定されたベースアイテムに、選んだアイテムの値を加算する処理



        //全て完了。最終的に完成された_baseのパラムを基に、新しくアイテムを生成し、ベースとトッピングアイテムは削除する。

        //ここまでで、生成されるアイテムの予測が出来る。

        if (_mstatus != 99)
        {
            Debug_TastePanel();
        }


        //以下、実際にアイテムリスト削除と、プレイヤーアイテムへの所持追加処理
        if (_mstatus == 0)
        {
            //最終的に生成されるアイテムの個数を決定

            if (exp_Controller.result_ok == true) //オリジナル調合の場合
            {
                result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * final_select_kaisu;
            }
            else if (exp_Controller.recipiresult_ok == true) //レシピ調合の場合
            {
                result_kosu = recipilistController.final_select_kosu;
            }
            else if (exp_Controller.topping_result_ok == true) //トッピング調合の場合
            {
                result_kosu = 1;
            }
            else if (exp_Controller.roast_result_ok == true) //「焼く」の場合
            {
                result_kosu = pitemlistController.final_kettei_kosu1;
            }

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
            _basejuice,
            _basegirl1_like, _basecost, _basesell,
            _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9],
            result_kosu, _base_extreme_kaisu, _base_item_hyouji, totalkyori);

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
            database.items[itemNum].Juice = _basesweat + _basebitter + _basesour;
        }
    }

    //HikariMakeStartPanelから読み出し
    public void HikariMakeGetItem(int _status)
    {
        //パラメータを取得
        result_item = GameMgr.hikari_make_okashiID;

        //コンポ調合データベースのIDを代入
        result_ID = GameMgr.hikari_make_okashi_compID;
        //result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu * GameMgr.hikari_make_okashiKosu; //compoDBの回数も含む個数
        result_kosu = GameMgr.hikari_make_okashiKosu;

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
        _basegirl1_like = pitemlist.player_yosokuitemlist[_id].girl1_itemLike;
        _basecost = pitemlist.player_yosokuitemlist[_id].cost_price;
        _basesell = pitemlist.player_yosokuitemlist[_id].sell_price;
        _base_itemType = pitemlist.player_yosokuitemlist[_id].itemType.ToString();
        _base_itemType_sub = pitemlist.player_yosokuitemlist[_id].itemType_sub.ToString();
        _base_extreme_kaisu = pitemlist.player_yosokuitemlist[_id].ExtremeKaisu;
        _base_item_hyouji = pitemlist.player_yosokuitemlist[_id].item_Hyouji;
        _base_itemdesc = pitemlist.player_yosokuitemlist[_id].itemDesc;

        for (i = 0; i < database.items[_id].toppingtype.Length; i++)
        {
            _basetp[i] = pitemlist.player_yosokuitemlist[_id].toppingtype[i].ToString();
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
        _basejuice,
        _basegirl1_like, _basecost, _basesell,
        _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9],
        result_kosu, _base_extreme_kaisu, _base_item_hyouji, totalkyori);

        if (_base_itemType == "Mat" || _base_itemType == "Potion")
        {
            //アイテム取得処理
            GetItemMethod(0); //生地・ポーション系作ったときは何もせず、オリジナルアイテムに登録
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
            else //例外処理。卵白と卵黄が同時にできる場合など。
            {
                MakeMethodExt();
            }
        }
        else //ヒカリが作る場合
        {
            if (GameMgr.hikari_make_doubleItemCreated == 0)
            {
                MakeMethod(_status);
            }
            else //例外処理。卵白と卵黄が同時にできる場合など。
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
                _baserich, _basesweat, _basebitter, _basesour, _basecrispy, _basefluffy, _basesmooth, _basehardness, _basejiggly, _basechewy, _basepowdery, _baseoily, _basewatery, _basebeauty,
                _basejuice,
                _basegirl1_like, _basecost, _basesell,
                _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9],
                result_kosu, _base_extreme_kaisu, _base_item_hyouji, totalkyori);

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
                        if (Comp_method_bunki == 3) //トッピング調合の場合  お菓子パネルのお菓子を削除し、新しく登録するのみ。
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
                _baserich, _basesweat, _basebitter, _basesour, _basecrispy, _basefluffy, _basesmooth, _basehardness, _basejiggly, _basechewy, _basepowdery, _baseoily, _basewatery, _basebeauty,
                _basejuice,
                _basegirl1_like, _basecost, _basesell,
                _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9],
                result_kosu, _base_extreme_kaisu, _base_item_hyouji, totalkyori);

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
        }
    }

    void MakeMethodExt()
    {
        if (databaseCompo.compoitems[result_ID].cmpitem_Name == "egg_split")
        {
            pitemlist.addPlayerItemString("egg_white", result_kosu);
            pitemlist.addPlayerItemString("egg_yellow", result_kosu);
        }
        if (databaseCompo.compoitems[result_ID].cmpitem_Name == "egg_split_premiaum")
        {
            pitemlist.addPlayerItemString("egg_premiaum_white", result_kosu);
            pitemlist.addPlayerItemString("egg_premiaum_yellow", result_kosu);
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
        //_tempbeauty = 0;
        _tempgirl1_like = 0;
        _tempcost = 0;
        _tempsell = 0;

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
                if (database.items[_id].itemType_sub.ToString() == "Machine")
                {

                }
                else
                {
                    _addkosu = final_kette_kosu1;
                    //Debug.Log("_id: " + _id);
                    //各パラメータを取得
                    Set_addparam();
                }

                break;

            case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                //Debug.Log("一個目オリジナルアイテム");

                _id = kettei_item1;
                _addkosu = final_kette_kosu1;
                //Debug.Log("_id: " + _id);
                //各パラメータを取得
                Set_add_originparam();

                break;

            case 2: //お菓子パネルアイテムリストから選択している場合

                //Debug.Log("一個目オリジナルアイテム");

                _id = kettei_item1;
                _addkosu = final_kette_kosu1;
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
                    if (database.items[_id].itemType_sub.ToString() == "Machine")
                    {

                    }
                    else
                    {
                        _addkosu = final_kette_kosu2;
                        //Debug.Log("_id: " + _id);
                        //各パラメータを取得
                        Set_addparam();
                    }

                    break;

                case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                    //Debug.Log("二個目オリジナルアイテム");

                    _id = kettei_item2;
                    _addkosu = final_kette_kosu2;
                    //Debug.Log("_id: " + _id);
                    //各パラメータを取得
                    Set_add_originparam();

                    break;

                case 2: //お菓子パネルアイテムリストから選択している場合

                    //Debug.Log("二個目オリジナルアイテム");

                    _id = kettei_item2;
                    _addkosu = final_kette_kosu2;
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
                    if (database.items[_id].itemType_sub.ToString() == "Machine")
                    {

                    }
                    else
                    {
                        _addkosu = final_kette_kosu3;

                        //各パラメータを取得
                        Set_addparam();
                    }

                    break;

                case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                    _id = kettei_item3;
                    _addkosu = final_kette_kosu3;

                    //各パラメータを取得
                    Set_add_originparam();

                    break;

                case 2: //お菓子パネルアイテムリストから選択している場合

                    _id = kettei_item3;
                    _addkosu = final_kette_kosu3;

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


        //①まずは、日数やコストなどの、全てのジャンルに共通するパラメータ同士を加算する。料金（_basesell）は、品質に基づいて計算する。

        for (i = 0; i < _additemlist.Count; i++) //入れた材料の数だけ、繰り返す。その後、総個数割り算。
        {
            _tempexp += _additemlist[i]._Addexp * _additemlist[i]._Addkosu;
            _tempday += _additemlist[i]._Addday * _additemlist[i]._Addkosu;
            _tempquality += _additemlist[i]._Addquality * _additemlist[i]._Addkosu;

            total_kosu += _additemlist[i]._Addkosu;
            //Debug.Log("各個数: " + _additemlist[i]._Addkosu);
        }

        //トッピングのときのみ、ベース個数を含む。
        if (Comp_method_bunki == 1 || Comp_method_bunki == 3)
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

        if (mstatus != 2)
        {
            totalkyori = Combinationmain.totalkyori; //_mstatus=99のときにこのスクリプトから計算するか、調合時にもCombinationmain.csで計算して、値が更新されてるはず。
        }
        else //mstatus=2はヒカリが作るお菓子　totalkyoriはセーブしておく。
        {
            totalkyori = GameMgr.hikari_make_okashi_totalkyori;
        }





        //②次に、甘さやサクサク感などの計算処理。

        //1. 新規調合の場合、加算。

        //2. トッピング調合時は、フルーツ・トッピングアイテムのみ加算。

        //3. 新規調合で、かつ小麦粉を使った場合、甘さなどはそのまま加算するが、食感はそのまま加算はせず、小麦粉をベースに、バター・砂糖・たまごは、各比率を計算し、代入する。

        //***基本の味の計算方法***
        //各材料の、パラメータをそれぞれ加算する。食感のみ、小麦粉とその他材料の比率をだして、補正がかかる。イメージ。

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2)//オリジナル調合　または　レシピ調合　のときの計算。
        {
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
            //_basebeauty += _tempbeauty;


            if (keisan_method_flag == 1) //1=ベスト配合との距離の補正をかける。
            {
                
                if (mstatus != 99)
                {
                    Debug.Log("ベスト配合との距離: " + totalkyori);
                }

                if (totalkyori >= 0 && totalkyori < 0.1)
                {
                    kyori_hosei = 2.0f;
                }
                else if (totalkyori >= 0.1 && totalkyori < 0.5)
                {
                    kyori_hosei = 1.8f;
                }
                else if (totalkyori >= 0.5 && totalkyori < 1.0)
                {
                    kyori_hosei = 1.5f;
                }
                else if (totalkyori >= 1.0 && totalkyori < 2.0)
                {
                    kyori_hosei = 1.2f;
                }
                else if (totalkyori >= 2.0 && totalkyori < 4.0)
                {
                    kyori_hosei = 1.0f;
                }
                else if (totalkyori >= 4.0 && totalkyori < 5.0)
                {
                    kyori_hosei = 0.75f;
                }
                else if (totalkyori >= 5.0 && totalkyori < 6.0)
                {
                    kyori_hosei = 0.5f;
                }
                else if (totalkyori >= 6.0 && totalkyori < 8.0)
                {
                    kyori_hosei = 0.25f;
                }
                else if (totalkyori >= 8.0)
                {
                    kyori_hosei = 0.125f;
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

        //③スロット同士の計算をする。
        AddSlot_Method();




        //④トッピングのときの計算。加算する。
        if (Comp_method_bunki == 3)//
        {
            for (i = 0; i < _additemlist.Count; i++)
            {
                //Debug.Log("フルーツ・トッピングの加算処理 ON");
                //各材料を加算していく。
                /*if (_additemlist[i]._Add_itemType_sub == "Fruits" || _additemlist[i]._Add_itemType_sub == "Potion" || _additemlist[i]._Add_itemType_sub == "Source" ||
                     _additemlist[i]._Add_itemType_sub == "Chocolate" || _additemlist[i]._Add_itemType_sub == "Chocolate_Mat" || _additemlist[i]._Add_itemType_sub == "IceCream")
                {*/
                _baserich += _additemlist[i]._Addrich * _additemlist[i]._Addkosu;
                _basesweat += _additemlist[i]._Addsweat * _additemlist[i]._Addkosu;
                _basebitter += _additemlist[i]._Addbitter * _additemlist[i]._Addkosu;
                _basesour += _additemlist[i]._Addsour * _additemlist[i]._Addkosu;
                _basecrispy += _additemlist[i]._Addcrispy * _additemlist[i]._Addkosu;
                _basefluffy += _additemlist[i]._Addfluffy * _additemlist[i]._Addkosu;
                _basesmooth += _additemlist[i]._Addsmooth * _additemlist[i]._Addkosu;
                _basehardness += _additemlist[i]._Addhardness * _additemlist[i]._Addkosu;
                _basejiggly += _additemlist[i]._Addjiggly * _additemlist[i]._Addkosu;
                _basechewy += _additemlist[i]._Addchewy * _additemlist[i]._Addkosu;
                _basepowdery += _additemlist[i]._Addpowdery * _additemlist[i]._Addkosu;
                _baseoily += _additemlist[i]._Addoily * _additemlist[i]._Addkosu;
                _basewatery += _additemlist[i]._Addwatery * _additemlist[i]._Addkosu;
                _basebeauty += _additemlist[i]._Addbeauty * _additemlist[i]._Addkosu;
                //}
            }
        }

        //ジュースののどごしを計算する。
        _basejuice = _basesweat + _basebitter + _basesour;

        //新規作成時の特殊処理
        if (Comp_method_bunki == 0 || Comp_method_bunki == 2)//オリジナル調合　または　レシピ調合　のときの計算。
        {
            //ジュースの特殊処理　甘さが青天井で上がることはないように、上限をおさえる。
            if (_base_itemType_sub == "Juice")
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
        }


        //⑤器具やアクセサリーなどによるバフ効果を追加する。
        if (Comp_method_bunki == 0 || Comp_method_bunki == 2)//オリジナル調合　または　レシピ調合　のときの計算。
        {
            if (databaseCompo.compoitems[result_ID].buf_kouka_on != 0) //_before_itemtype_Sub != _base_itemType_sub クリーム系からまたクリーム系が出来る場合は、バフがかからないよう、重複防止処理
            {
                //お菓子の食感ごとに、バフをかける処理
                _basecrispy += bufpower_keisan.Buf_OkashiParamUp_Keisan(0, _base_itemType_sub); //中の数字でどの食感パラムかの指定
                _basefluffy += bufpower_keisan.Buf_OkashiParamUp_Keisan(1, _base_itemType_sub);
                _basesmooth += bufpower_keisan.Buf_OkashiParamUp_Keisan(2, _base_itemType_sub);
                _basehardness += bufpower_keisan.Buf_OkashiParamUp_Keisan(3, _base_itemType_sub);
                _basejuice += bufpower_keisan.Buf_OkashiParamUp_Keisan(4, _base_itemType_sub);

                //固有のお菓子のみにバフをかける処理
                _basecrispy += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(0, _basename); //中の数字でどの食感パラムかの指定
                _basefluffy += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(1, _basename);
                _basesmooth += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(2, _basename);
                _basehardness += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(3, _basename);
                _basejuice += bufpower_keisan.Buf_OkashiParamUp_ItemNameKeisan(4, _basename);

                //魔力の泡だて器をもっている
                if (pitemlist.ReturnItemKosu("whisk_magic") >= 1)
                {
                    if (_base_itemType_sub.ToString() == "Appaleil") //生地系を作るときは、全てにバフ
                    {
                        _basecrispy = (int)(_basecrispy * 1.3f);
                        _basefluffy = (int)(_basefluffy * 1.3f);
                        _basesmooth = (int)(_basesmooth * 1.3f);
                    }
                    else
                    {
                        //クリーム系の補正 魔法泡だて器を使って、作りたてクリーム　か　リコッタクリーム
                        if (result_ID == databaseCompo.SearchCompoIDString("whipped cream_row_magic") ||
                            result_ID == databaseCompo.SearchCompoIDString("whipped cream_row_magic_Free") ||
                            result_ID == databaseCompo.SearchCompoIDString("cream_row_ricotta"))
                        {
                            _basecrispy = (int)(_basecrispy * 1.3f);
                            _basefluffy = (int)(_basefluffy * 1.3f);
                            _basesmooth = (int)(_basesmooth * 1.3f);
                        }
                    }
                }
            }
        }

        //⑥ヒカリのお菓子の場合　味に補正かかる。
        if(mstatus == 2)
        {
            if (databaseCompo.compoitems[result_ID].buf_kouka_on != 0) //バフ計算するものだけ、バフ計算。例えばクッキー×ぶどう＝ぶどうクッキーのときは、バフ計算しない
            {
                //まず、作るお菓子のサブタイプをもとに、計算。制作時間なども計算する。
                hikari_okashilv_hosei = bufpower_keisan.Buf_HikariOkashiLV_Keisan(_base_itemType_sub);
                Debug.Log("ヒカリがお菓子作る場合　補正: " + hikari_okashilv_hosei);

                _basecrispy = (int)(1.0f * _basecrispy * hikari_okashilv_hosei);
                _basefluffy = (int)(1.0f * _basefluffy * hikari_okashilv_hosei);
                _basesmooth = (int)(1.0f * _basesmooth * hikari_okashilv_hosei);
                _basehardness = (int)(1.0f * _basehardness * hikari_okashilv_hosei);
                _basejuice = (int)(1.0f * _basejuice * hikari_okashilv_hosei);

                //専用アイテムがあれば、ヒカリのお菓子さらにパラメータアップ
                _basecrispy += bufpower_keisan.Buf_HikariParamUp_Keisan(0, _base_itemType_sub); //_base_itemType_subは未使用だが、とりあえず置いてる。
                _basefluffy += bufpower_keisan.Buf_HikariParamUp_Keisan(1, _base_itemType_sub);
                _basesmooth += bufpower_keisan.Buf_HikariParamUp_Keisan(2, _base_itemType_sub);
                _basehardness += bufpower_keisan.Buf_HikariParamUp_Keisan(3, _base_itemType_sub);
                _basejuice += bufpower_keisan.Buf_HikariParamUp_Keisan(4, _base_itemType_sub);
            }
        }

        
        if (mstatus == 0 || mstatus == 1)
        {
            if (Comp_method_bunki == 0 || Comp_method_bunki == 2)//オリジナル調合　または　レシピ調合　のときの計算。
            {
                //⑦ヒカリのお菓子レベルに応じて、ほんの少し最終的なお菓子の味にバフがかかる。にいちゃんが作る場合のみ。。
                if (databaseCompo.compoitems[result_ID].buf_kouka_on != 0) //バフ計算するものだけ、バフ計算。例えばクッキー×ぶどう＝ぶどうクッキーのときは、バフ計算しない
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
                    }
                }

                //⑧ハートボーナス　HLV=99のときは、お菓子の味が1.3倍に上昇。にいちゃんが作る場合のみ。
                if (databaseCompo.compoitems[result_ID].buf_kouka_on != 0) //バフ計算するものだけ、バフ計算。例えばクッキー×ぶどう＝ぶどうクッキーのときは、バフ計算しない
                {
                    if (PlayerStatus.girl1_Love_lv >= 99)
                    {
                        _basecrispy = (int)(1.2f * _basecrispy);
                        _basefluffy = (int)(1.2f * _basefluffy);
                        _basesmooth = (int)(1.2f * _basesmooth);
                        _basehardness = (int)(1.2f * _basehardness);
                        _basejuice = (int)(1.2f * _basejuice);
                    }
                }
            }
        }
    }




    void AddSlot_Method()
    {
        //重複した場合は、個別にスロットに入れる。新しいトッピング能力がある場合は、ベースの空のスロットに上書きしていく。
        //ベースの空スロットがなくなった時点で、それ以上合成はできない。

        //加算トッピングの一個目をもとに、ベースのスロット一個目から順番にみていく。

        for (count = 0; count < _additemlist.Count; count++)
        {

            for (n = 0; n < _additemlist[count]._Addkosu; n++)
            {
                i = 0;

                while (i < _additemlist[count]._Addtp.Length)
                {
                    //Debug.Log(_addtp[i]);

                    if (_additemlist[count]._Addtp[i] != "Non") //Nonではない、＝いちごとかオレンジとか、何かが入っている場合は、次にベースのTPを見る。
                    {

                        j = 0;
                        while (j < _basetp.Length) //ベースが全て空でない場合、全て無視したまま、処理だけ続く。
                        {

                            if (_basetp[j] == "Non") //ベースが空の場合は、そこに_addトッピングを入れる。
                            {
                                //Debug.Log(_basetp[j]);
                                _basetp[j] = _additemlist[count]._Addtp[i];
                                break;
                            }
                            else if (_basetp[j] == _additemlist[count]._Addtp[i]) //ベースに入っているトッピングと、_addが重複の場合。
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
                    else if (_additemlist[count]._Addtp[i] == "Non") //Nonの場合、そのスロットは無視して、次のスロットをみる
                    {
                        //break;
                    }

                    i++;
                }
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
        _addbase_score = database.items[_id].Base_Score;
        _addgirl1_like = database.items[_id].girl1_itemLike;
        _addcost = database.items[_id].cost_price;
        _addsell = database.items[_id].sell_price;
        _add_itemType = database.items[_id].itemType.ToString();
        _add_itemType_sub = database.items[_id].itemType_sub.ToString();

        //店売りアイテムを合成に使う場合は、固有トッピングを計算する。

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2) //オリジナル・レシピ調合時
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
        else if (Comp_method_bunki == 3) //トッピング時。通常トッピング＋固有トッピングどちらも計算
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

        //Debug.Log("_addkosu: " + _addkosu);
        _additemlist.Add(new ItemAdd(_addname, _addhp, _addday, _addquality, _addexp, _addrich, _addsweat, _addbitter, _addsour,
            _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _addbeauty, _add_itemType, _add_itemType_sub,
            _addbase_score, _addgirl1_like, _addcost, _addsell, 
            _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9], _addkoyutp[0], _addkosu));
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
        _addbase_score = pitemlist.player_originalitemlist[_id].Base_Score;
        _addgirl1_like = pitemlist.player_originalitemlist[_id].girl1_itemLike;
        _addcost = pitemlist.player_originalitemlist[_id].cost_price;
        _addsell = pitemlist.player_originalitemlist[_id].sell_price;
        _add_itemType = pitemlist.player_originalitemlist[_id].itemType.ToString();
        _add_itemType_sub = pitemlist.player_originalitemlist[_id].itemType_sub.ToString();


        //オリジナルアイテムを合成に使う場合も、固有トッピングを計算する。

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2) //オリジナル・レシピ調合時
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
        else if (Comp_method_bunki == 3) //トッピング時
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

        //Debug.Log("_addkosu: " + _addkosu);
        _additemlist.Add(new ItemAdd(_addname, _addhp, _addday, _addquality, _addexp, _addrich, _addsweat, _addbitter, _addsour, 
            _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _addbeauty, _add_itemType, _add_itemType_sub,
            _addbase_score, _addgirl1_like, _addcost, _addsell, 
            _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9], _addkoyutp[0], _addkosu));
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
        _addbase_score = pitemlist.player_extremepanel_itemlist[_id].Base_Score;
        _addgirl1_like = pitemlist.player_extremepanel_itemlist[_id].girl1_itemLike;
        _addcost = pitemlist.player_extremepanel_itemlist[_id].cost_price;
        _addsell = pitemlist.player_extremepanel_itemlist[_id].sell_price;
        _add_itemType = pitemlist.player_extremepanel_itemlist[_id].itemType.ToString();
        _add_itemType_sub = pitemlist.player_extremepanel_itemlist[_id].itemType_sub.ToString();


        //オリジナルアイテムを合成に使う場合も、固有トッピングを計算する。

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2) //オリジナル・レシピ調合時
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
        else if (Comp_method_bunki == 3) //トッピング時
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

        //Debug.Log("_addkosu: " + _addkosu);
        _additemlist.Add(new ItemAdd(_addname, _addhp, _addday, _addquality, _addexp, _addrich, _addsweat, _addbitter, _addsour,
            _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _addbeauty, _add_itemType, _add_itemType_sub,
            _addbase_score, _addgirl1_like, _addcost, _addsell,
            _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9], _addkoyutp[0], _addkosu));
    }






    //味の計算処理
    void AddParam_Method()
    {
        //初期化
        etc_mat_count = 0;

       
        //特定のアイテムの場合は、加算のみでOK。例えばアマンドファリーヌのような、粉同士を組み合わせただけ、など。
        if(databaseCompo.compoitems[result_ID].KeisanMethod == "Non")
        {
            keisan_method_flag = 0;
        }
        else
        {
            keisan_method_flag = 1;

        }

        for (i = 0; i < _additemlist.Count; i++)
        {
            AddTasteParam(); //各材料を加算していく。     
        }
        //DivisionTasteparam(); //その後、個数で割り算する。
        
    }

    void AddTasteParam()
    {
        //そのまま加算する。
        _temprich += _additemlist[i]._Addrich * _additemlist[i]._Addkosu;
        _tempsweat += _additemlist[i]._Addsweat * _additemlist[i]._Addkosu;
        _tempbitter += _additemlist[i]._Addbitter * _additemlist[i]._Addkosu;
        _tempsour += _additemlist[i]._Addsour * _additemlist[i]._Addkosu;
        _tempcrispy += _additemlist[i]._Addcrispy * _additemlist[i]._Addkosu;
        _tempfluffy += _additemlist[i]._Addfluffy * _additemlist[i]._Addkosu;
        _tempsmooth += _additemlist[i]._Addsmooth * _additemlist[i]._Addkosu;
        _temphardness += _additemlist[i]._Addhardness * _additemlist[i]._Addkosu;
        _tempjiggly += _additemlist[i]._Addjiggly * _additemlist[i]._Addkosu;
        _tempchewy += _additemlist[i]._Addchewy * _additemlist[i]._Addkosu;
        _temppowdery += _additemlist[i]._Addpowdery * _additemlist[i]._Addkosu;
        _tempoily += _additemlist[i]._Addoily * _additemlist[i]._Addkosu;
        _tempwatery += _additemlist[i]._Addwatery * _additemlist[i]._Addkosu;
        //_tempbeauty += _additemlist[i]._Addbeauty * _additemlist[i]._Addkosu;

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
        //_tempbeauty /= total_kosu;
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

        if (Comp_method_bunki == 1 || Comp_method_bunki == 3) //生地合成、もしくはトッピング調合などの場合、ベースアイテムを、プレイヤーのアイテムリストから選んでる場合は、ベースアイテムの削除処理を行う。
        {

            //ベースアイテムを削除する。
            switch (base_toggle_type)
            {
                case 0: //プレイヤーアイテムリストから選択している。

                    _id = base_kettei_item;

                    pitemlist.deletePlayerItem(database.items[_id].itemName, 1);
                    break;

                case 1: //オリジナルアイテムリストから選択している。

                    _id = base_kettei_item;

                    //オリジナルアイテムリストから削除するときは、一度削除用リストにIDをとりまとめて、後で、まとめて、降順で削除していく。
                    deleteOriginalList.Add(_id, 1);
                    break;

                case 2: //お菓子パネルアイテムリストから選択している。

                    _id = base_kettei_item;

                    //オリジナルアイテムリストから削除するときは、一度削除用リストにIDをとりまとめて、後で、まとめて、降順で削除していく。
                    deleteExtremeList.Add(_id, 1);
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
        else if (Comp_method_bunki == 0)
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

                //器具は、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine")
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

                    //器具は、削除しない
                    if (database.items[_id].itemType_sub.ToString() == "Machine")
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

                    deleteExtremeList.Add(_id, final_kette_kosu1);
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

                    //器具は、削除しない
                    if (database.items[_id].itemType_sub.ToString() == "Machine")
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

                    deleteExtremeList.Add(_id, final_kette_kosu1);
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
                /*if (deletePair.Key == exp_Controller._temp_extreme_id && exp_Controller._temp_extremeSetting == true)
                {
                    extreme_panel = canvas.transform.Find("MainUIPanel/ExtremePanel").GetComponent<ExtremePanel>();
                    extreme_panel.deleteExtreme_Item();
                }*/
                pitemlist.deleteOriginalItem(deletePair.Key, deletePair.Value);
                
                //Debug.Log("delete_originID: " + deletePair.Key + " 個数:" + deletePair.Value);
            }
        }

        //オリジナルアイテムリストからアイテムを選んでる場合の削除処理
        if (deleteExtremeList.Count > 0)
        {
            Debug.Log("調合にお菓子パネルアイテムを使用した");

            //オリジナルアイテムをトッピングに使用していた場合の削除処理。削除用リストに入れた分をもとに、削除の処理を行う。
            var newTable = deleteExtremeList.OrderByDescending(value => value.Key); //降順にする

            foreach (KeyValuePair<int, int> deletePair in newTable)
            {
                /*if (deletePair.Key == exp_Controller._temp_extreme_id && exp_Controller._temp_extremeSetting == true)
                {
                    extreme_panel = canvas.transform.Find("MainUIPanel/ExtremePanel").GetComponent<ExtremePanel>();
                    extreme_panel.deleteExtreme_Item();
                }*/
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

            //器具は、削除しない
            if (database.items[_id].itemType_sub.ToString() == "Machine")
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

            //器具は、削除しない
            if (database.items[_id].itemType_sub.ToString() == "Machine")
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

                //器具は、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine")
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

                //器具は、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine")
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

                //器具は、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine")
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

                //器具は、削除しない
                if (database.items[_id].itemType_sub.ToString() == "Machine")
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
                if (deletePair.Key == exp_Controller._temp_extreme_id && exp_Controller._temp_extremeSetting == true)
                {
                    extreme_panel = canvas.transform.Find("MainUIPanel/ExtremePanel").GetComponent<ExtremePanel>();
                    extreme_panel.deleteExtreme_Item();
                }
                pitemlist.deleteOriginalItem(deletePair.Key, deletePair.Value);
                //Debug.Log("delete_originID: " + deletePair.Key + " 個数:" + deletePair.Value);
            }
        }
    }

    void Debug_TastePanel()
    {
        Debug.Log("名前: " + _basename + " _basehp: " + _basehp + " _baseday: " + _baseday + " 品質: " + _basequality);
        Debug.Log("甘さ: " + _basesweat + " 苦さ: " + _basebitter + " 酸味: " + _basesour);
        Debug.Log("コク: " + _baserich);
        Debug.Log("さくさく感: " + _basecrispy + " ふわふわ感: " + _basefluffy);
        Debug.Log("しっとり感: " + _basesmooth + " 歯ごたえ: " + _basehardness);
        Debug.Log("ジュースののどごし: " + _basejuice);
        Debug.Log("_basejiggly: " + _basejiggly + " _basechewy: " + _basechewy);
        Debug.Log("粉っぽさ: " + _basepowdery + " 油っぽさ: " + _baseoily + " 水っぽさ: " + _basewatery);
        Debug.Log("見た目: " + _basebeauty);
        Debug.Log("_basegirl1_like:" + _basegirl1_like + " _basecost:" + _basecost + " _basesell:" + _basesell);
        Debug.Log("_base_itemType:" + _base_itemType + " _base_itemType_sub:" + _base_itemType_sub);

        /*Debug.Log("スロット1: " + _basetp[0]);
        Debug.Log("スロット2: " + _basetp[1]);
        Debug.Log("スロット3: " + _basetp[2]);
        Debug.Log("スロット4: " + _basetp[3]);
        Debug.Log("スロット5: " + _basetp[4]);
        Debug.Log("スロット6: " + _basetp[5]);
        Debug.Log("スロット7: " + _basetp[6]);
        Debug.Log("スロット8: " + _basetp[7]);
        Debug.Log("スロット9: " + _basetp[8]);
        Debug.Log("スロット10: " + _basetp[9]);*/
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
