﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Compound_Keisan : MonoBehaviour {

    private GameObject canvas;

    private PlayerItemList pitemlist;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private Exp_Controller exp_Controller;

    private int i, j, n, count;
    private int itemNum, DBcount;

    private int total_qbox_money;



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

    private int result_item;
    private int result_ID;
    private int new_item;

    private bool Pate_flag;
    private int result_kosu;

    

    //トッピング調合用のパラメータ
    private int _id;

    private int Comp_method_bunki; //調合の分岐フラグ。Exp_Controllerで指定している。

    Dictionary<int, int> deleteOriginalList = new Dictionary<int, int>(); //オリジナルアイテムリストの削除用のリスト。ID, 個数のセット

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
    public int _basepowdery;
    public int _baseoily;
    public int _basewatery;
    public int _basegirl1_like;
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
    private int _addgirl1_like;
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
    private int _tempgirl1_like;
    private int _tempcost;
    private int _tempsell;
    private string[] _temptp;


    //小麦粉の比率計算時に使用。
    private int _komugikomp;
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
    private int _komugikogirl1_like;
    private int _komugikocost;
    private int _komugikosell;

    private int total_kosu;
    
    //小麦粉との比率計算用パラメータ
    private int komugiko_id; //小麦粉を使っている、材料の_addID
    private int komugiko_flag; //使っていた場合、1にする。
    private int Komugiko_count;
    private float komugiko_ratio;
    private float _add_ratio;
    private float _bad_ratio;
    private float _komugibad_ratio;
    private int etc_mat_count;
    private float komugiko_distance;


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

    

    // Use this for initialization
    void Start () {

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

        //トッピングスロットの配列
        _basetp = new string[10];
        _addtp = new string[10];
        _temptp = new string[10];

        _addkoyutp = new string[3];


        //
        //アイテムデータベースの味パラムを初期化
        //
        
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
	
	// Update is called once per frame
	void Update () {
		
	}




    //           //
    //  合成処理 //
    //           //

    public void Topping_Compound_Method(int _mstatus)
    {
        //ベースアイテムのパラメータを取得する。その後、各トッピングアイテムの値を取得し、加算する。

        if (_mstatus == 0)
        {
            //パラメータを取得
            SetParamInit();
        }
        else if (_mstatus == 1)
        {
            //パラメータを取得。予測用
            SetParamYosokuInit();
        }

        else if (_mstatus == 99)
        {
            //パラメータを取得。アイテムデータベースを、ここで計算して初期化する。
            SetParamDatabaseInit();
        }


        //ベースアイテム　タイプを見て、プレイヤリストアイテムかオリジナルアイテムかを識別する。
        if (Comp_method_bunki == 0 || Comp_method_bunki == 2) //新規にアイテムを作成する場合 or レシピ調合の場合。空のパラメータに、材料のパラメータを総計していく。
        {
            _id = result_item;

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
                _basepowdery = 0;
                _baseoily = 0;
                _basewatery = 0;
                _basegirl1_like = database.items[_id].girl1_itemLike;
                _basecost = database.items[_id].cost_price;
                _basesell = database.items[_id].sell_price;
                _base_itemType = database.items[_id].itemType.ToString();
                _base_itemType_sub = database.items[_id].itemType_sub.ToString();
                _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
                _base_item_hyouji = database.items[_id].item_Hyouji;
                _base_itemdesc = database.items[_id].itemDesc;
            }
            else //アイテム自体が持っている値を加算する場合。使わないかも。
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
                _basepowdery = database.items[_id].Powdery;
                _baseoily = database.items[_id].Oily;
                _basewatery = database.items[_id].Watery;
                _basegirl1_like = database.items[_id].girl1_itemLike;
                _basecost = database.items[_id].cost_price;
                _basesell = database.items[_id].sell_price;
                _base_itemType = database.items[_id].itemType.ToString();
                _base_itemType_sub = database.items[_id].itemType_sub.ToString();
                _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
                _base_item_hyouji = database.items[_id].item_Hyouji;
                _base_itemdesc = database.items[_id].itemDesc;
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
                    _basepowdery = database.items[_id].Powdery;
                    _baseoily = database.items[_id].Oily;
                    _basewatery = database.items[_id].Watery;
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
                    _basepowdery = pitemlist.player_originalitemlist[_id].Powdery;
                    _baseoily = pitemlist.player_originalitemlist[_id].Oily;
                    _basewatery = pitemlist.player_originalitemlist[_id].Watery;
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
                result_kosu = databaseCompo.compoitems[result_ID].cmpitem_result_kosu;
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
            Delete_playerItemList();

            //新しく作ったアイテムをオリジナルアイテムリストに追加。
            pitemlist.addOriginalItem(_basename, _basehp, _baseday, _basequality, _baseexp, _baseprobability,
                _baserich, _basesweat, _basebitter, _basesour, _basecrispy, _basefluffy, _basesmooth, _basehardness, _basejiggly, _basechewy, _basepowdery, _baseoily, _basewatery,
                _basegirl1_like, _basecost, _basesell,
                _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9],
                result_kosu, _base_extreme_kaisu, _base_item_hyouji);

            new_item = pitemlist.player_originalitemlist.Count - 1; //最後に追加されたアイテムが、さっき作った新規アイテムなので、そのIDを入れて置き、リザルトで表示
        }
        else if (_mstatus == 1) //予測の場合、アイテムの追加処理はいらない。
        {

        }
        else if (_mstatus == 99 ) //初期化の場合
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
            }

            //エクストリーム調合から閃いた場合
            else
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
            }

            /*Debug.Log("pitemlistController.kettei_item1: " + kettei_item1);
            Debug.Log("pitemlistController.kettei_item2: " + kettei_item2);
            Debug.Log("pitemlistController._toggle_type1: " + toggle_type1);
            Debug.Log("pitemlistController._toggle_type2: " + toggle_type2);
            Debug.Log("pitemlistController.final_kettei_kosu1: " + final_kette_kosu1);
            Debug.Log("pitemlistController.final_kettei_kosu2: " + final_kette_kosu2);*/

            //パラメータを取得
            result_item = pitemlistController.result_item;

            //コンポ調合データベースのIDを代入
            result_ID = pitemlistController.result_compID;
        }

        if (Comp_method_bunki == 2) //レシピ調合の場合
        {
            //レシピの場合。今のところ、店売りアイテムのみでしか、レシピの材料にならないので、以下の定め方にしている。もし、オリジナルアイテムから使う場合は、toggle_typeなどの判定がちゃんと必要。

            kettei_item1 = recipilistController.kettei_recipiitem1;
            kettei_item2 = recipilistController.kettei_recipiitem2;
            kettei_item3 = recipilistController.kettei_recipiitem3;            

            toggle_type1 = 0;
            toggle_type2 = 0;
            toggle_type3 = 0;

            final_kette_kosu1 = recipilistController.final_kettei_recipikosu1;
            final_kette_kosu2 = recipilistController.final_kettei_recipikosu2;
            final_kette_kosu3 = recipilistController.final_kettei_recipikosu3;

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
        }

        //**ここまで**
    }

    //決定アイテムなどのパラメータを取得
    void SetParamYosokuInit()
    {
        //プレイヤーアイテム表示用コントローラーの取得
        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        //レシピリストコントローラーの取得
        recipilistController_obj = canvas.transform.Find("RecipiList_ScrollView").gameObject;
        recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

        //パラメータを取得
        result_item = recipilistController.result_recipiitem;

        //コンポ調合データベースのIDを代入
        result_ID = recipilistController.result_recipicompID;

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
        _tempgirl1_like = 0;
        _tempcost = 0;
        _tempsell = 0;

        _komugikorich = 0;
        _komugikosweat = 0;
        _komugikobitter = 0;
        _komugikosour = 0;
        _komugikocrispy = 0;
        _komugikofluffy = 0;
        _komugikosmooth = 0;
        _komugikohardness = 0;
        _komugikojiggly = 0;
        _komugikochewy = 0;
        _komugikopowdery = 0;
        _komugikooily = 0;
        _komugikowatery = 0;

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

        //生地合成・トッピングのときのみ、ベース個数を含む。
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

        }



        //③スロット同士の計算をする。
        AddSlot_Method();




        //④トッピングのときの計算。加算する。
        if (Comp_method_bunki == 3)//オリジナル調合　または　レシピ調合　のときの計算。
        {
            for (i = 0; i < _additemlist.Count; i++)
            {
                //Debug.Log("フルーツ・トッピングの加算処理 ON");
                //各材料を加算していく。
                if (_additemlist[i]._Add_itemType_sub == "Fruits" || _additemlist[i]._Add_itemType_sub == "Potion")
                {
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
        _addgirl1_like = database.items[_id].girl1_itemLike;
        _addcost = database.items[_id].cost_price;
        _addsell = database.items[_id].sell_price;
        _add_itemType = database.items[_id].itemType.ToString();
        _add_itemType_sub = database.items[_id].itemType_sub.ToString();

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2) //オリジナル・レシピ調合時は固有トッピングの値は計算しない。
        {
            if (exp_Controller.extreme_on) //エクストリームから新規作成される場合はトッピングの計算をしない。
            {
                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _addtp[i] = "Non";
                }

                for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
                {
                    _addkoyutp[i] = "Non";
                }
            }
            else //通常のオリジナル・レシピ調合の場合は、トッピングの値だけ計算する。固有は無視。
            {
                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _addtp[i] = database.items[_id].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
                {
                    _addkoyutp[i] = "Non";
                }
            }
        }
        else if (Comp_method_bunki == 3) //トッピング時は、通常トッピング＋固有トッピングどちらも計算
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


        _additemlist.Add(new ItemAdd(_addname, _addhp, _addday, _addquality, _addexp, _addrich, _addsweat, _addbitter, _addsour, _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _add_itemType, _add_itemType_sub, _addgirl1_like, _addcost, _addsell, _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9], _addkoyutp[0], _addkosu));
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
        _addgirl1_like = pitemlist.player_originalitemlist[_id].girl1_itemLike;
        _addcost = pitemlist.player_originalitemlist[_id].cost_price;
        _addsell = pitemlist.player_originalitemlist[_id].sell_price;
        _add_itemType = pitemlist.player_originalitemlist[_id].itemType.ToString();
        _add_itemType_sub = pitemlist.player_originalitemlist[_id].itemType_sub.ToString();

        if (Comp_method_bunki == 0 || Comp_method_bunki == 2) //オリジナル・レシピ調合時は固有トッピングの値は計算しない。
        {
            if (exp_Controller.extreme_on) //エクストリームから新規作成される場合はトッピングの計算をしない。
            {
                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _addtp[i] = "Non";
                }

                for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
                {
                    _addkoyutp[i] = "Non";
                }
            }
            else //通常のオリジナル・レシピ調合の場合は、トッピングの値だけ計算する。固有は無視。
            {
                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _addtp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
                {
                    _addkoyutp[i] = "Non";
                }
            }
        }
        else if (Comp_method_bunki == 3) //トッピング時は、通常トッピング＋固有トッピングどちらも計算
        {
            for (i = 0; i < database.items[_id].toppingtype.Length; i++)
            {
                _addtp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
            }

            for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
            {
                _addkoyutp[i] = pitemlist.player_originalitemlist[_id].koyu_toppingtype[i].ToString();
            }
        }

        _additemlist.Add(new ItemAdd(_addname, _addhp, _addday, _addquality, _addexp, _addrich, _addsweat, _addbitter, _addsour, _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _add_itemType, _add_itemType_sub, _addgirl1_like, _addcost, _addsell, _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9], _addkoyutp[0], _addkosu));
    }






    //小麦粉をベースにした、計算処理
    void AddParam_Method()
    {
        //初期化
        komugiko_id = 0;
        komugiko_flag = 0;
        Komugiko_count = 0;
        etc_mat_count = 0;


        //まず、材料に小麦粉を使ってるか否かをチェック。小麦粉の種類が複数の場合、現状バグるので、その処理は後に実装すること。
        for (i = 0; i < _additemlist.Count; i++)
        {

            if (_additemlist[i]._Add_itemType_sub == "Komugiko")
            {
                komugiko_id = i;
                komugiko_flag = 1;
                Komugiko_count++;
            }
        }
        //ただし特定のアイテムの場合は、加算のみでOK。例えばアマンドファリーヌのような、粉同士を組み合わせただけ、など。
        if(database.items[result_item].itemName == "financier_powder" || database.items[result_item].itemName == "bugget")
        {
            komugiko_flag = 0;
        }

        //材料に小麦粉を使っているか否かで処理を分岐
        switch (komugiko_flag)
        {
            case 0: //材料に小麦粉を使っていない

                //Debug.Log("小麦粉を使っていない");

                for (i = 0; i < _additemlist.Count; i++)
                {
                        AddTasteParam(); //各材料を加算していく。                   
                }

                //DivisionTasteparam(); //その後、個数で割り算する。

                break;

            case 1: //小麦粉を使っている場合

                //Debug.Log("小麦粉を使っている" + "  材料ID(0 or 1 or 2): " + komugiko_id);

                //また一からみていき、今度は加算していく。小麦粉IDだけ取り除く

                for (i = 0; i < _additemlist.Count; i++)
                {
                    if (i == komugiko_id) //小麦粉それ自体の計算は取り除く
                    {

                    }
                    else
                    {
                        etc_mat_count++;
                        komugiko_ratio = (float)_additemlist[i]._Addkosu / _additemlist[komugiko_id]._Addkosu; // 小麦粉に対する、各材料の分量・比率
                        //Debug.Log("komugiko_ratio（材料の個数 / 小麦粉の個数）: " + _additemlist[i]._Addname + " " + komugiko_ratio);

                        //食感の計算。材料の値を計算する。
                        AddRatioTasteParam();

                        //甘さなどの味4パラメータは、入れた材料分だけ加算。
                        _temprich += _additemlist[i]._Addrich * _additemlist[i]._Addkosu;
                        _tempsweat += _additemlist[i]._Addsweat * _additemlist[i]._Addkosu;
                        _tempbitter += _additemlist[i]._Addbitter * _additemlist[i]._Addkosu;
                        _tempsour += _additemlist[i]._Addsour * _additemlist[i]._Addkosu;
                    }
                }

                //最後に、小麦粉の値を計算し、加算。

                //小麦粉以外の材料の「種類数」（個数ではない）の分、AddRatioTasteParam()メソッドで、小麦粉の値もその都度加算している。
                //なので、あとで加算した回数分を割り算して、小麦粉1種類あたりの平均値のパラメータをだし、加算する。
                //例えば、3個調合の場合、小麦粉を除いて、残り2種類なので、etc_mat_countは、2が入っているはず。
                _komugikocrispy /= etc_mat_count;
                _komugikofluffy /= etc_mat_count;
                _komugikosmooth /= etc_mat_count;
                _komugikohardness /= etc_mat_count;
                _komugikojiggly /= etc_mat_count;
                _komugikochewy /= etc_mat_count;
                _komugikopowdery /= etc_mat_count;
                _komugikooily /= etc_mat_count;
                _komugikowatery /= etc_mat_count;

                //_tempに加算。
                _tempcrispy += _komugikocrispy;
                _tempfluffy += _komugikofluffy;
                _tempsmooth += _komugikosmooth;
                _temphardness += _komugikohardness;
                _tempjiggly += _komugikojiggly;
                _tempchewy += _komugikochewy;
                _temppowdery += _komugikopowdery;
                _tempoily += _komugikooily;
                _tempwatery += _komugikowatery;

                total_kosu = _additemlist.Count;

                break;

            default:
                break;

        }
    }


    
    void AddRatioTasteParam()
    {
        //小麦粉 1に対して、 バターや砂糖などの材料の比率で、加算する値が変わる。
        //大体　2:1:1がほどよいとされている。
        //あまりに小麦粉の量に対して、材料を多く入れすぎていると、マイナス。

        komugiko_distance = (komugiko_ratio - (float)0.5); //0.5は、小麦粉*材料=2:1の場合の値。材料ごとに設定してもよいかも。
        //Debug.Log("小麦粉と " + _additemlist[i]._Addname + " との距離: " + komugiko_ratio  + " - 0.5 = " + komugiko_distance);

        //誤差が、-0.3以上　極端に小麦粉を入れすぎた場合
        if (komugiko_distance < -0.3)
        {
            //小麦8: バター1などの場合。（比率的には、比率0.125で、0.125-0.5=-0.375になる。） 小麦粉を入れすぎたときの補正
            _add_ratio = -(Mathf.Abs(komugiko_distance) * (float)10.0);
            komugiko_ratio = -(Mathf.Abs(komugiko_distance) * (float)10.0);
            _bad_ratio = (float)3.0;
            _komugibad_ratio = (float)3.0;
        }

        //誤差が-0.0 ~ -0.3
        else if (komugiko_distance >= -0.3 && komugiko_distance < 0)
        {
            //小麦4: バター1などの場合。（比率的には、4:1で入れた場合。比率0.25で、0.25-0.5=-0.25になる。） 小麦粉を多めにしたときの補正
            _add_ratio = (float)0.7;
            komugiko_ratio = (float)1.2;
            _bad_ratio = (float)0.7;
            _komugibad_ratio = (float)1.5;
        }

        //もし、小麦粉1に対して、材料0.5を基準に、誤差が0.0~0.5未満なら。一番ほどよい距離である。
        else if (komugiko_distance >= 0 && komugiko_distance < 0.5)
        {
            _add_ratio = (float)1.0;
            komugiko_ratio = (float)1.0;
            _bad_ratio = (float)1.0;
            _komugibad_ratio = (float)1.0;
        }

        //誤差が0.5～1.2
        else if (komugiko_distance >= 0.5 && komugiko_distance < 1.2)
        {
            //小麦2: バター2などの場合。（比率的には、1:1で入れた場合。）

            _add_ratio = ((float)0.5 + Mathf.Abs(komugiko_distance)) * (float)1.5; //若干、材料のほうが多い分、材料の値が強くなる。
            komugiko_ratio = (float)0.9 * komugiko_distance;
            _bad_ratio = ((float)0.5 + Mathf.Abs(komugiko_distance)) * (float)1.5;
            _komugibad_ratio = (float)0.9 * komugiko_distance;
        }
        //誤差が1.2～2.2
        else if (komugiko_distance >= 1.2 && komugiko_distance < 2.2)
        {
            //小麦2: バター4などの場合。（2.0。　2.0-0.5=1.5の場合）　少し材料が多めになっているとき

            _add_ratio = (float)1.5 * komugiko_distance;
            komugiko_ratio = (float)0.75 * komugiko_distance;
            _bad_ratio = (float)1.0 * komugiko_distance;
            _komugibad_ratio = (float)0.9 * komugiko_distance;
        }
        //誤差が2.2～3.0
        else if (komugiko_distance >= 2.2 && komugiko_distance < 3.0)
        {
            //小麦2: バター6などの場合。（3.0。　3.0-0.5=2.5の場合）　かなり材料が多めになっているとき

            _add_ratio = (float)1.7 * komugiko_distance;
            komugiko_ratio = (float)0.4 * komugiko_distance;
            _bad_ratio = (float)0.9 * komugiko_distance;
            _komugibad_ratio = (float)0.8 * komugiko_distance;
        }
        //誤差が3.0以上　材料を入れすぎた
        else if (komugiko_distance >= 3.0)
        {
            //小麦2: バター10などの場合。（5.0。　5.0-0.5=4.5の場合）　明らかに材料を入れすぎた

            _add_ratio = -(Mathf.Abs(komugiko_distance) * (float)10.0);
            komugiko_ratio = -(Mathf.Abs(komugiko_distance) * (float)10.0);
            _bad_ratio = (float)3.5;
            _komugibad_ratio = (float)3.5;
        }


        //Debug.Log("_add_ratio: " + _add_ratio);
        //Debug.Log("_komugiko_ratio: " + komugiko_ratio);

        //その時の材料の値　×　_add_ratioで、加算する。       
        _tempcrispy += (int)(_additemlist[i]._Addcrispy * _add_ratio);
        _tempfluffy += (int)(_additemlist[i]._Addfluffy * _add_ratio);
        _tempsmooth += (int)(_additemlist[i]._Addsmooth * _add_ratio);
        _temphardness += (int)(_additemlist[i]._Addhardness * _add_ratio);
        _tempjiggly += (int)(_additemlist[i]._Addjiggly * _add_ratio);
        _tempchewy += (int)(_additemlist[i]._Addchewy * _add_ratio);
        _temppowdery += (int)(_additemlist[i]._Addpowdery * _bad_ratio);
        _tempoily += (int)(_additemlist[i]._Addoily * _bad_ratio);
        _tempwatery += (int)(_additemlist[i]._Addwatery * _bad_ratio);

        //小麦粉の値の計算用。
        _komugikocrispy += (int)(_additemlist[komugiko_id]._Addcrispy * komugiko_ratio);
        _komugikofluffy += (int)(_additemlist[komugiko_id]._Addfluffy * komugiko_ratio);
        _komugikosmooth += (int)(_additemlist[komugiko_id]._Addsmooth * komugiko_ratio);
        _komugikohardness += (int)(_additemlist[komugiko_id]._Addhardness * komugiko_ratio);
        _komugikojiggly += (int)(_additemlist[komugiko_id]._Addjiggly * komugiko_ratio);
        _komugikochewy += (int)(_additemlist[komugiko_id]._Addchewy * komugiko_ratio);
        _komugikopowdery += (int)(_additemlist[komugiko_id]._Addpowdery * _komugibad_ratio);
        _komugikooily += (int)(_additemlist[komugiko_id]._Addoily * _komugibad_ratio);
        _komugikowatery += (int)(_additemlist[komugiko_id]._Addwatery * _komugibad_ratio);
    }


    //プレイヤーアイテムリストから、選んだ材料の削除処理
    public void Delete_playerItemList()
    {
        //パラメータの取得
        SetParamInit();

        deleteOriginalList.Clear();

        if (Comp_method_bunki == 1 || Comp_method_bunki == 3) //生地合成、もしくはトッピング調合などの場合、ベースアイテムを、プレイヤーのアイテムリストから選んでる場合は、ベースアイテムの削除処理を行う。
        {

            //ベースアイテムを削除する。
            switch (base_toggle_type)
            {
                case 0: //プレイヤーアイテムリストから選択している。

                    _id = base_kettei_item;

                    pitemlist.deletePlayerItem(_id, 1);
                    break;

                case 1: //オリジナルアイテムリストから選択している。

                    _id = base_kettei_item;

                    //オリジナルアイテムリストから削除するときは、一度削除用リストにIDをとりまとめて、後で、まとめて、降順で削除していく。
                    deleteOriginalList.Add(_id, 1);
                    break;

                default:
                    break;
            }
        }

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
                    pitemlist.deletePlayerItem(_id, final_kette_kosu1);
                }
                break;

            case 1: //オリジナルアイテムリストから選択している。オリジナルの場合は、一度削除用リストにIDを追加し、降順にしてから、後の削除メソッドでまとめて削除する。

                _id = kettei_item1;

                deleteOriginalList.Add(_id, final_kette_kosu1);
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
                        pitemlist.deletePlayerItem(_id, final_kette_kosu2);
                    }
                    break;

                case 1: //オリジナルアイテムリストから選択している。オリジナルの場合は、一度削除用リストにIDを追加し、降順にしてから、後の削除メソッドでまとめて削除する。

                    _id = kettei_item2;

                    deleteOriginalList.Add(_id, final_kette_kosu2);
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
                        pitemlist.deletePlayerItem(_id, final_kette_kosu3);
                    }
                    break;

                case 1: //オリジナルアイテムリストから選択している。オリジナルの場合は、一度削除用リストにIDを追加し、降順にしてから、後の削除メソッドでまとめて削除する。

                    _id = kettei_item3;

                    deleteOriginalList.Add(_id, final_kette_kosu3);
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
    }

    void Debug_TastePanel()
    {
        Debug.Log("名前: " + _basename + " _basehp: " + _basehp + " _baseday: " + _baseday + " 品質: " + _basequality);
        Debug.Log("甘さ: " + _basesweat + " 苦さ: " + _basebitter + " 酸味: " + _basesour);
        Debug.Log("コク: " + _baserich);
        Debug.Log("さくさく感: " + _basecrispy + " ふわふわ感: " + _basefluffy);
        Debug.Log("しっとり感: " + _basesmooth + " ほろほろ感: " + _basehardness);
        Debug.Log("_basejiggly: " + _basejiggly + " _basechewy: " + _basechewy);
        Debug.Log("粉っぽさ: " + _basepowdery + " 油っぽさ: " + _baseoily + " 水っぽさ: " + _basewatery);
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




    //
    //Qboxにお菓子を入れたときの、お菓子の評価＋報酬額の決定
    //

    void Judge_QuestOkashi()
    {

        //まず、アイテムの値を取得

        switch (toggle_type1)
        {
            case 0: //プレイヤーアイテムリストから選択している。

                _id = kettei_item1;

                //各パラメータを取得
                _basename = database.items[_id].itemName;
                _basehp = database.items[_id].itemHP;
                _baseday = database.items[_id].item_day;
                _basequality = database.items[_id].Quality;
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
                _basepowdery = database.items[_id].Powdery;
                _baseoily = database.items[_id].Oily;
                _basewatery = database.items[_id].Watery;
                _basegirl1_like = database.items[_id].girl1_itemLike;
                _basecost = database.items[_id].cost_price;
                _basesell = database.items[_id].sell_price;
                _base_itemType = database.items[_id].itemType.ToString();
                _base_itemType_sub = database.items[_id].itemType_sub.ToString();

                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _basetp[i] = database.items[_id].toppingtype[i].ToString();
                }

                break;

            case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                //さらに、オリジナルのプレイヤーアイテムリストの番号を参照する。

                _id = kettei_item1;

                //各パラメータを取得
                _basename = pitemlist.player_originalitemlist[_id].itemName;
                _basehp = pitemlist.player_originalitemlist[_id].itemHP;
                _baseday = pitemlist.player_originalitemlist[_id].item_day;
                _basequality = pitemlist.player_originalitemlist[_id].Quality;
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
                _basepowdery = pitemlist.player_originalitemlist[_id].Powdery;
                _baseoily = pitemlist.player_originalitemlist[_id].Oily;
                _basewatery = pitemlist.player_originalitemlist[_id].Watery;
                _basegirl1_like = pitemlist.player_originalitemlist[_id].girl1_itemLike;
                _basecost = pitemlist.player_originalitemlist[_id].cost_price;
                _basesell = pitemlist.player_originalitemlist[_id].sell_price;
                _base_itemType = pitemlist.player_originalitemlist[_id].itemType.ToString();
                _base_itemType_sub = pitemlist.player_originalitemlist[_id].itemType_sub.ToString();

                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _basetp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
                }

                break;

            default:
                break;
        }

        // アイテムに対して、各パラメータの値をもとに、報酬額を計算する。
        //
        // 【計算方法】
        // 基本売値価格（_basesell）×　品質値の修正　×　４味による修正　×　各４食感による特殊修正　×　３雑味によるマイナス補正　×　個数
        // 修正の値は、依頼の種類（時期・ストーリー進行度）によって変わる。
        // 序盤は、フィナンシェを売って金を稼ぐ。後半はもっと、すごいアイテムなどを渡すとより多く稼げるように。
        //
        // 納品ボックスには締め切りがない。
        //

        _quality_revise = 1;
        _rich_revise = 1;
        _sweat_revise = 1;
        _bitter_revise = 1;
        _sour_revise = 1;
        _crispy_revise = 1;
        _fluffy_revise = 1;
        _smooth_revise = 1;
        _hardness_revise = 1;
        _powdery_revise = 1;
        _oily_revise = 1;
        _watery_revise = 1;

        //品質値補正の計算
        if (_basequality > 95 && _basequality <= 100) //95.1 ~ 100
        {
            _quality_revise = 3;
        }
        else if (_basequality > 80 && _basequality <= 95) // 80.1 ~ 95
        {
            _quality_revise = 2;
        }
        else if (_basequality > 60 && _basequality <= 80) // 60.1 ~ 80
        {
            _quality_revise = (float)1.5;
        }
        else if (_basequality <= 60) // 60以下
        {
            _quality_revise = 1;
        }

        //味のよる修正。依頼によって、求められる味が変わる。値に近いほど、高得点の補正がかかる。
        //あまみ・にがみ・さんみに対して、それぞれの評価。差の値により、6段階で評価する。

        //味のコク
        rich_result = _baserich - 50;
        if (Mathf.Abs(rich_result) == 0)
        {
            Debug.Log("味の深み: Perfect!!");
            _rich_revise = 3;
        }
        else if (Mathf.Abs(rich_result) < 5)
        {
            Debug.Log("味の深み: Great!!");
            _rich_revise = 2;
        }
        else if (Mathf.Abs(rich_result) < 15)
        {
            Debug.Log("味の深み: Good!");
            _rich_revise = (float)1.5;
        }
        else if (Mathf.Abs(rich_result) < 50)
        {
            Debug.Log("味の深み: Normal");
            _rich_revise = 1;
        }
        else if (Mathf.Abs(rich_result) < 80)
        {
            Debug.Log("味の深み: poor");
            _rich_revise = (float)0.5;
        }
        else if (Mathf.Abs(rich_result) <= 100)
        {
            Debug.Log("味の深み: death..");
            _rich_revise = (float)0.25;
        }
        else
        {
            Debug.Log("100を超える場合はなし");
        }


        //甘味
        sweat_result = _basesweat - 50;
        if (Mathf.Abs(sweat_result) == 0)
        {
            Debug.Log("甘み: Perfect!!");
            _sweat_revise = 3;
        }
        else if (Mathf.Abs(sweat_result) < 5)
        {
            Debug.Log("甘み: Great!!");
            _sweat_revise = 2;
        }
        else if (Mathf.Abs(sweat_result) < 15)
        {
            Debug.Log("甘み: Good!");
            _sweat_revise = (float)1.5;
        }
        else if (Mathf.Abs(sweat_result) < 50)
        {
            Debug.Log("甘み: Normal");
            _sweat_revise = 1;
        }
        else if (Mathf.Abs(sweat_result) < 80)
        {
            Debug.Log("甘み: poor");
            _sweat_revise = (float)0.5;
        }
        else if (Mathf.Abs(sweat_result) <= 100)
        {
            Debug.Log("甘み: death..");
            _sweat_revise = (float)0.25;
        }
        else
        {
            Debug.Log("100を超える場合はなし");
        }


        //苦味
        bitter_result = _basebitter - 50;
        if (Mathf.Abs(bitter_result) == 0)
        {
            Debug.Log("苦味: Perfect!!");
            _bitter_revise = 3;
        }
        else if (Mathf.Abs(bitter_result) < 5)
        {
            Debug.Log("苦味: Great!!");
            _bitter_revise = 2;
        }
        else if (Mathf.Abs(bitter_result) < 15)
        {
            Debug.Log("苦味: Good!");
            _bitter_revise = (float)1.5;
        }
        else if (Mathf.Abs(bitter_result) < 50)
        {
            Debug.Log("苦味: Normal");
            _bitter_revise = 1;
        }
        else if (Mathf.Abs(bitter_result) < 80)
        {
            Debug.Log("苦味: poor");
            _bitter_revise = (float)0.5;
        }
        else if (Mathf.Abs(bitter_result) <= 100)
        {
            Debug.Log("苦味: death..");
            _bitter_revise = (float)0.25;
        }
        else
        {
            Debug.Log("100を超える場合はなし");
        }


        //酸味
        sour_result = _basesour - 50;
        if (Mathf.Abs(sour_result) == 0)
        {
            Debug.Log("酸味: Perfect!!");
            _sour_revise = 3;
        }
        else if (Mathf.Abs(sour_result) < 5)
        {
            Debug.Log("酸味: Great!!");
            _sour_revise = 2;
        }
        else if (Mathf.Abs(sour_result) < 15)
        {
            Debug.Log("酸味: Good!");
            _sour_revise = (float)1.5;
        }
        else if (Mathf.Abs(sour_result) < 50)
        {
            Debug.Log("酸味: Normal");
            _sour_revise = 1;
        }
        else if (Mathf.Abs(sour_result) < 80)
        {
            Debug.Log("酸味: poor");
            _sour_revise = (float)0.5;
        }
        else if (Mathf.Abs(sour_result) <= 100)
        {
            Debug.Log("酸味: death..");
            _sour_revise = (float)0.25;
        }
        else
        {
            Debug.Log("100を超える場合はなし");
        }


        //食感による修正。ある閾値を超えた分で、補正が変わる。依頼の内容で、求められる食感が変わる。
        if (_basecrispy >= 50) //50以上のとき、超えた分が補正にかかる
        {
            _crispy_revise = (float)_basecrispy / 50; //最大2.0倍
        }
        if (_basefluffy >= 50) //50以上のとき、超えた分が補正にかかる
        {
            _fluffy_revise = (float)_basecrispy / 50; //最大2.0倍
        }
        if (_basesmooth >= 50) //50以上のとき、超えた分が補正にかかる
        {
            _smooth_revise = (float)_basecrispy / 50; //最大2.0倍
        }
        if (_basehardness >= 50) //50以上のとき、超えた分が補正にかかる
        {
            _hardness_revise = (float)_basecrispy / 50; //最大2.0倍
        }

        //マイナス補正。あまりに粉っぽかったり、油っこかったりすると、まずくなってしまい、補正がかかる。
        if (_basepowdery >= 50) //50以上のとき、超えた分が補正にかかる
        {
            _powdery_revise = SujiMap(_basepowdery / 50, 1, 2, (float)0.75, (float)0.2); //1.0~2.0倍の間で、0.75~0.2の補正がかかる。
        }
        if (_baseoily >= 50) //50以上のとき、超えた分が補正にかかる
        {
            _oily_revise = SujiMap(_baseoily / 50, 1, 2, (float)0.75, (float)0.2); //1.0~2.0倍の間で、0.75~0.2の補正がかかる。
        }
        if (_basewatery >= 50) //50以上のとき、超えた分が補正にかかる
        {
            _watery_revise = SujiMap(_basewatery / 50, 1, 2, (float)0.75, (float)0.2); //1.0~2.0倍の間で、0.75~0.2の補正がかかる。
        }

        //合計額
        total_qbox_money = (int)(_basesell * _quality_revise * _rich_revise * _sweat_revise * _bitter_revise * _sour_revise * _crispy_revise * _fluffy_revise * _smooth_revise * _hardness_revise
            * _powdery_revise * _oily_revise * _watery_revise) * result_kosu;

        Debug.Log("_basesell: " + _basesell + " _quality_revise: " + _quality_revise + " _rich_revise: " + _rich_revise + " _sweat_revise: " + _sweat_revise + " _bitter_revise: " + _bitter_revise +
           " _sour_revise: " + _sour_revise);
        Debug.Log("_crispy_revise: " + _crispy_revise + " _fluffy_revise: " + _fluffy_revise + " _smooth_revise: " + _smooth_revise + " _hardness_revise: " + _hardness_revise);
        Debug.Log("_powdery_revise: " + _powdery_revise + " _oily_revise: " + _oily_revise + " _watery_revise: " + _watery_revise);
        Debug.Log("result_kosu: " + result_kosu);
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
