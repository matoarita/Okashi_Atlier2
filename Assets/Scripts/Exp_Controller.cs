using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

//
//アイテムの更新・経験値の増減処理を行うコントローラー　＋　調合の処理を担うメソッド
//

public class Exp_Controller : SingletonMonoBehaviour<Exp_Controller>
{
    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject shopitemlistController_obj;
    private ShopItemListController shopitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;

    private GameObject GirlEat_scene_obj;
    private GirlEat_Main girlEat_scene;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    private GameObject card_view_obj;
    private CardView card_view;

    private PlayerItemList pitemlist;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private ItemRoastDataBase databaseRoast;
    private ItemShopDataBase shop_database;
    private SlotNameDataBase slotnamedatabase;

    private int toggle_type1;
    private int toggle_type2;
    private int toggle_type3;
    private int base_toggle_type;

    //アイテムIDを参照している。
    private int final_base_kettei_item;
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

    private string[] _slot = new string[10];
    private string[] _slotHyouji1 = new string[10]; //日本語に変換後の表記を格納する。スロット覧用

    private int i, j, sw, count;

    private int komugiko_id; //小麦粉を使っている、材料の_addID
    private int komugiko_flag; //使っていた場合、1にする。
    private int Komugiko_count;
    private float komugiko_ratio;
    private float _add_ratio;
    private float _bad_ratio;
    private float _komugibad_ratio;
    private int etc_mat_count;

    private int Comp_method_bunki; //トッピング調合メソッドの分岐フラグ

    public bool compound_success; //調合の成功か失敗

    public int comp_judge_flag; //調合判定を行うかどうか。itemSelectToggleのjudge_flagと連動する。0の場合、必ず成功=生地に合成する処理。1の場合、新規調合を表す。

    public bool extreme_on; //エクストリーム調合から、新しいアイテムを閃いた場合は、ON

    public bool result_ok; // 調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool recipiresult_ok; //レシピ調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool topping_result_ok; //トッピング調合完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool roast_result_ok; //「焼く」完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。

    public bool blend_flag; //これがたっていたら、レシピ調合の際、プレイヤーが素材自体を変えたり、比率を変えている、というフラグ。現在は、とりあえずオフ。

    public bool girleat_ok; // 女の子にアイテムをあげた時の完了のフラグ。これがたっていたら、プレイヤーアイテムリストの中身を更新する。そしてフラグをオフに。
    public bool shop_buy_ok; //購入完了のフラグ。これがたっていたら、購入の処理を行い、フラグをオフに。
    public bool qbox_ok; // クエスト納品時の完了フラグ。


    //トッピング調合用のパラメータ
    private int _id;

    Dictionary<int, int> deleteOriginalList = new Dictionary<int, int>(); //オリジナルアイテムリストの削除用のリスト。ID, 個数のセット
    //List<int> deleteOriginalList = new List<int>(); //オリジナルアイリストの削除用のリスト
    //List<int> deleteOriginalList_result = new List<int>();


    private string _basename;
    private int _basemp;
    private int _baseday;
    private int _basequality;
    private int _baserich;
    private int _basesweat;
    private int _basebitter;
    private int _basesour;
    private int _basecrispy;
    private int _basefluffy;
    private int _basesmooth;
    private int _basehardness;
    private int _basejiggly;
    private int _basechewy;
    private int _basepowdery;
    private int _baseoily;
    private int _basewatery;
    private int _basegirl1_like;
    private int _basecost;
    private int _basesell;
    private string[] _basetp;
    private string _base_itemType;
    private string _base_itemType_sub;

    private string _addname;
    private int _addmp;
    private int _addday;
    private int _addquality;
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
    private string _add_itemType;
    private string _add_itemType_sub;
    private int _addkosu;

    //_baseに加算する前に、一時的に計算する用。
    private int _tempmp;
    private int _tempday;
    private int _tempquality;
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

    private int total_qbox_money;


    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

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


        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":
                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                break;

            default:
                break;
        }

        result_ok = false;
        recipiresult_ok = false;
        girleat_ok = false;

        blend_flag = false;

        compound_success = false;

        extreme_on = false;

        i = 0;
        j = 0;
        sw = 0;
        new_item = 0;

        Comp_method_bunki = 0;

        //トッピングスロットの配列
        _basetp = new string[10];
        _addtp = new string[10];
        _temptp = new string[10];
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {

            //シーン読み込みのたびに、一度リセットされてしまうので、アップデートで一度初期化
            if (compound_Main_obj == null)
            {
                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();
            }

            //
            //オリジナル調合完了の場合、ここでアイテムリストの更新行う。
            //

            if (result_ok == true)
            {
                pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
                pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

                text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
                _text = text_area.GetComponentInChildren<Text>();

                //オリジナル調合の設定
                if (extreme_on != true)
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

                //リザルトアイテムを代入
                result_item = pitemlistController.result_item;

                //調合データベースのIDを代入
                result_ID = pitemlistController.result_compID;

                //Debug.Log("comp_judge_flag: " + comp_judge_flag);

                //トッピング調合用メソッドを流用するために、kettei_itemの変換

                if (comp_judge_flag == 0) //新規調合の場合。
                {
                    Comp_method_bunki = 0;
                }

                else if (comp_judge_flag == 1) //生地を合成する処理で、新規にアイテムは作成されない場合
                {
                    Comp_method_bunki = 1; //生地を合成する時の分岐

                    base_kettei_item = kettei_item1;
                    kettei_item1 = kettei_item2;
                    kettei_item2 = kettei_item3;
                    kettei_item3 = 9999; //とりあえず、現状新規は3個までの調合なので、4個目のものは空に。

                    base_toggle_type = toggle_type1;
                    toggle_type1 = toggle_type2;
                    toggle_type2 = toggle_type3;
                    toggle_type3 = 0;

                    base_kosu = pitemlistController.final_kettei_kosu1;
                    final_kette_kosu1 = pitemlistController.final_kettei_kosu2;
                    final_kette_kosu2 = pitemlistController.final_kettei_kosu3;
                    final_kette_kosu3 = 0;

                    if (pitemlistController.final_kettei_item3 == 9999) //3個目が空の場合、二個で調合している。
                    {
                        kettei_item2 = 9999;
                        toggle_type2 = 0;
                        final_kette_kosu2 = 0;
                    }

                    
                }

                pitemlistController_obj.SetActive(false);

                //経験値の増減
                if (compound_success == true)
                {
                    compound_success = false;

                    //オリジナル調合なら、普通に生成
                    if (extreme_on != true)
                    {
                        //完成したアイテムの追加。
                        Topping_Compound_Method();

                        PlayerStatus.player_renkin_exp += databaseCompo.compoitems[result_ID].renkin_Bexp; //調合完成のアイテムに対応した経験値がもらえる。

                        StartCoroutine("renkin_exp_up");

                        card_view.ResultCard_DrawView(1, new_item);
                    }

                    //エクストリーム調合の場合、スロット計算などはせず、新しいアイテムのみ生成。
                    else
                    {
                        // アイテムリストの削除処理 //
                        Delete_playerItemList();

                        //新しく作ったアイテムをアイテムリストに追加。
                        //プレイヤーアイテムリストに追加。
                        pitemlist.addPlayerItem(result_item, 1);
                        
                        new_item = result_item;

                        PlayerStatus.player_renkin_exp += databaseCompo.compoitems[result_ID].renkin_Bexp; //調合完成のアイテムに対応した経験値がもらえる。
                        
                        //エクストリームの場合は、アイテムを閃く！
                        StartCoroutine("extreme_exp_up");
                        
                        card_view.ResultCard_DrawView(0, new_item);
                    }

                    //完成アイテムの、レシピフラグをONにする。
                    databaseCompo.compoitems[result_ID].cmpitem_flag = 1;

                    //はじめて、アイテムを制作した場合は、フラグをONに。
                    if (compound_Main.First_Recipi_on != true)
                    {
                        compound_Main.First_Recipi_on = true;
                    }
                                    

                    pitemlistController_obj.SetActive(true);


                } else
                {

                    _text.text = "調合失敗..！ ";

                    Debug.Log(database.items[result_item].itemNameHyouji + "調合失敗..！");

                    //完成したアイテムの追加。調合失敗の場合、ゴミが入っている。
                    pitemlist.addPlayerItem(result_item, result_kosu);

                    //失敗した場合でも、アイテムは消える。
                    Delete_playerItemList();

                    card_view.ResultCard_DrawView(0, result_item);
                        
                    pitemlistController_obj.SetActive(true);
                }

                result_ok = false;

                pitemlistController.AddItemList(); //リスト描画の更新
                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。

                //日数の経過
                PlayerStatus.player_day += databaseCompo.compoitems[result_ID].cost_Time;

            }




            //
            //レシピ調合完了の場合、ここでアイテムリストの更新行う。
            //

            if (recipiresult_ok == true)
            {
                recipilistController_obj = GameObject.FindWithTag("RecipiList_ScrollView");
                recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

                text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
                _text = text_area.GetComponentInChildren<Text>();

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

                result_kosu = recipilistController.final_select_kosu;

                result_item = recipilistController.result_recipiitem;
                result_ID = recipilistController.result_recipicompID;


                //レシピ調合の場合、材料は、デフォルトで店売りのアイテムを使う。出来るアイテムも、デフォルトでは店売り。
                //後々、ブレンドで比率を変えた場合は、アイテムの生成処理を、他の調合と同じメソッドを流用。

                if (blend_flag == true) //プレイヤーが任意で、アイテムを変えたり、比率を変更した場合
                {
                    Comp_method_bunki = 0;

                    //オリジナル調合のメソッドを流用する
                    Topping_Compound_Method();

                    card_view.ResultCard_DrawView(1, new_item);
                }
                else //こっちがデフォルト。店売りアイテムを使い、店売りアイテムを作成する。
                {
                    Comp_method_bunki = 0;

                    Delete_playerItemList();
                    pitemlist.addPlayerItem(result_item, result_kosu);

                    card_view.ResultCard_DrawView(0, result_item);
                }
               

                recipiresult_ok = false;


                PlayerStatus.player_renkin_exp += databaseCompo.compoitems[result_ID].renkin_Bexp; //調合完成のアイテムに対応した経験値がもらえる。

                StartCoroutine("renkin_exp_up");

                
            }





            //
            //トッピング調合完了の場合、ここでアイテムリストの更新行う。
            //

            if (topping_result_ok == true)
            {
                pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
                pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

                text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
                _text = text_area.GetComponentInChildren<Text>();


                //プレイヤーリストコントローラーで更新した変数を、こっちでも一度代入
                kettei_item1 = pitemlistController.kettei_item1;
                kettei_item2 = pitemlistController.kettei_item2;
                kettei_item3 = pitemlistController.kettei_item3;
                base_kettei_item = pitemlistController.base_kettei_item;

                toggle_type1 = pitemlistController._toggle_type1;
                toggle_type2 = pitemlistController._toggle_type2;
                toggle_type3 = pitemlistController._toggle_type3;
                base_toggle_type = pitemlistController._base_toggle_type;
                

                if ( pitemlistController.final_kettei_item2 == 9999 ) //2個目が空の場合、トッピングは一個のみ。
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

                //**ここまで**

                Comp_method_bunki = 3; //トッピング調合の処理。

                if (compound_success == true)
                {
                    Debug.Log("Topping_Compound_Sucess!!");

                    //トッピング調合完了なので、リザルトアイテムのパラメータ計算と、プレイヤーアイテムリストに追加処理
                    Topping_Compound_Method();

                    card_view.ResultCard_DrawView(1, new_item);

                    pitemlistController.AddItemList(); //リスト描画の更新

                    pitemlistController_obj.SetActive(false);

                    topping_result_ok = false;

                    compound_success = false;

                    StartCoroutine("renkin_exp_up");

                    pitemlistController_obj.SetActive(true);

                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                }

            }

            //
            //「焼く」完了の場合、ここでアイテムリストの更新行う。
            //

            if (roast_result_ok == true)
            {
                pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
                pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

                text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
                _text = text_area.GetComponentInChildren<Text>();

                //**重要** 
                //焼く場合は、元となる生地アイテムを削除し、対応するお菓子アイテムを追加する。

                kettei_item1 = pitemlistController.kettei_item1;
                kettei_item2 = 9999;
                kettei_item3 = 9999;

                toggle_type1 = pitemlistController._toggle_type1;
                toggle_type2 = pitemlistController._toggle_type2;
                toggle_type3 = pitemlistController._toggle_type3;


                final_kette_kosu1 = pitemlistController.final_kettei_kosu1;
                final_kette_kosu2 = 0;
                final_kette_kosu3 = 0;

                //result_kosu = pitemlistController.final_kettei_kosu1;

                final_kettei_item1 = pitemlistController.final_kettei_item1;

                Comp_method_bunki = 0;

                
                if (compound_success == true)
                {
                    Debug.Log("Roast_Okashi_Sucess!!");

                    //焼くデータベースをもとに、決定したアイテムから、リザルトアイテムへ変換
                    i = 0;
                    result_item = 9999;

                    while (i < databaseRoast.roastitems.Count)
                    {
                        if (databaseRoast.roastitems[i].roast_itemID == final_kettei_item1) //クッキー生地（final_kettei_item1=14）なら、「ねこクッキー」を作るパターン。サンプル。
                        {
                            result_item = databaseRoast.roastitems[i].roast_item_resultID;
                            break;
                        }
                        i++;
                    }

                    //例外として、もし、焼くデータベースに登録されていない場合、ゴミが出来上がってしまう。
                    if(result_item == 9999)
                    {
                        //例外発生
                        Debug.Log("例外発生！焼きデータベースの登録がありません");
                        result_item = 500;
                        card_view.ResultCard_DrawView(0, result_item);

                    }
                    else
                    {
                        Topping_Compound_Method();
                        card_view.ResultCard_DrawView(1, new_item);
                    }
                    
                    
                    

                    pitemlistController.AddItemList(); //リスト描画の更新

                    pitemlistController_obj.SetActive(false);

                    roast_result_ok = false;

                    compound_success = false;

                    StartCoroutine("renkin_exp_up");

                    pitemlistController_obj.SetActive(true);

                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                }       
            }
        }

        if (SceneManager.GetActiveScene().name == "Shop") // ショップシーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {

            //ここでアイテムリストの更新行う。
            if (shop_buy_ok == true)
            {
                shopitemlistController_obj = GameObject.FindWithTag("ShopitemList_ScrollView");
                shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

                text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
                _text = text_area.GetComponentInChildren<Text>();

                kettei_item1 = shopitemlistController.shop_kettei_item1;
                //Debug.Log("決定したアイテムID: " + kettei_item1 + " リスト番号: " + shopitemlistController.shop_count);

                toggle_type1 = shopitemlistController.shop_itemType;

                result_kosu = shopitemlistController.shop_final_itemkosu_1; //買った個数

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

                //所持金をへらす
                PlayerStatus.player_money -= shop_database.shopitems[shopitemlistController.shop_count].shop_costprice * result_kosu;

                //ショップの在庫をへらす
                shop_database.shopitems[shopitemlistController.shop_count].shop_itemzaiko -= result_kosu;

                _text.text = "購入しました！他にはなにか買う？";

                shopitemlistController.ShopList_DrawView(); //リスト描画の更新

                shop_buy_ok = false;
            }
        }

        if (SceneManager.GetActiveScene().name == "GirlEat") // ガールシーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {
            //女の子にアイテムをあげた後の、アイテムリストの更新を行う。
            if (girleat_ok == true)
            {

                GirlEat_scene_obj = GameObject.FindWithTag("GirlEat_scene");
                girlEat_scene = GirlEat_scene_obj.GetComponent<GirlEat_Main>();

                GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
                girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

                pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
                pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();


                //お菓子の判定処理を起動。引数は、決定したアイテムのアイテムIDと、店売りかオリジナルで制作したアイテムかの、判定用ナンバー 0or1
                girlEat_judge.Girleat_Judge_method(pitemlistController.kettei_item1, pitemlistController._toggle_type1);


                //アイテムリストの更新。選んだアイテムをリストから削除

                //削除の処理
                switch (pitemlistController._toggle_type1)
                {
                    case 0: //プレイヤーアイテムリストから選択している。

                        pitemlist.deletePlayerItem(pitemlistController.kettei_item1, 1);
                        break;

                    case 1: //オリジナルアイテムリストから選択している。

                        pitemlist.deleteOriginalItem(pitemlistController.kettei_item1, 1);
                        break;

                    default:
                        break;
                }

                pitemlistController.reset_and_DrawView();//リスト描画の更新

                girleat_ok = false;


              
                //お菓子に対して、コメントを言う女の子
                pitemlistController_obj.SetActive(false);
                girlEat_scene.girleat_status = 2; //アイテムあげたあとの処理へ移行できるフラグをたてる　-> GirlEat_Main.csへ移動
            }
        }

        if (SceneManager.GetActiveScene().name == "QuestBox") // Qboxシーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {

            if (qbox_ok == true)
            {

                pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
                pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

                text_area = GameObject.FindWithTag("Message_Window");
                _text = text_area.GetComponentInChildren<Text>();

                kettei_item1 = pitemlistController.kettei_item1;
                toggle_type1 = pitemlistController._toggle_type1;

                result_kosu = pitemlistController.final_kettei_kosu1;

                //アイテムの採点。良いお菓子は高く売れる。
                Judge_QuestOkashi();


                //アイテムリストの更新。選んだアイテムをリストから削除

                //削除の処理
                switch (pitemlistController._toggle_type1)
                {
                    case 0: //プレイヤーアイテムリストから選択している。

                        pitemlist.deletePlayerItem(pitemlistController.kettei_item1, result_kosu);
                        break;

                    case 1: //オリジナルアイテムリストから選択している。

                        pitemlist.deleteOriginalItem(pitemlistController.kettei_item1, result_kosu);
                        break;

                    default:
                        break;
                }

                pitemlistController.reset_and_DrawView();//リスト描画の更新

                qbox_ok = false;

                //所持金を増やす
                PlayerStatus.player_money += total_qbox_money;

                _text.text = "お菓子を売った！　" + total_qbox_money + "Gを獲得！";

            }
        }
    }



    //           //
    //  合成処理 //
    //           //

    void Topping_Compound_Method()
    {
        //ベースアイテムのパラメータを取得する。その後、各トッピングアイテムの値を取得し、加算する。


        //ベースアイテム　タイプを見て、プレイヤリストアイテムかオリジナルアイテムかを識別する。

        if (Comp_method_bunki == 0) //新規にアイテムを作成する場合。空のパラメータに、材料のパラメータを総計していく。
        {
            _id = result_item;

            //各パラメータを取得
            _basename = database.items[_id].itemName;
            _basemp = 0;
            _baseday = 0;
            _basequality = 0;
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
                    _basename = database.items[_id].itemName;
                    _basemp = database.items[_id].itemMP;
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

                    //result_itemに、アイテムIDを入れる。
                    result_item = _id;

                    break;

                case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                    //さらに、オリジナルのプレイヤーアイテムリストの番号を参照する。

                    _id = base_kettei_item;

                    //各パラメータを取得
                    _basename = pitemlist.player_originalitemlist[_id].itemName;
                    _basemp = pitemlist.player_originalitemlist[_id].itemMP;
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
                        //pitemlist.player_originalitemlist[_id] 各パラメータを取得できる。
                    break;

                default:
                    break;
            }
        }


        

        AddParamMethod(); //決定されたベースアイテムに、選んだアイテムの値を加算する処理



        //全て完了。最終的に完成された_baseのパラムを基に、新しくアイテムを生成し、ベースとトッピングアイテムは削除する。

        //ここまでで、生成されるアイテムの予測が出来る。
        /*Debug.Log("_basename: " + _basename + " _basemp: " + _basemp + " _baseday: " + _baseday + " _basequality: " + _basequality);
        Debug.Log("_baserich: " + _baserich + "_basesweat: " + _basesweat + " _basebitter: " + _basebitter + " _basesour: " + _basesour + " _basecrispy: " + _basecrispy + " _basefluffy: " + _basefluffy);
        Debug.Log("_basesmooth: " + _basesmooth + " _basejiggly: " + _basejiggly + " _basechewy: " + _basechewy);
        Debug.Log("_basegirl1_like:" + _basegirl1_like + " _basecost:" + _basecost + " _basesell:" + _basesell);
        Debug.Log("_base_itemType:" + _base_itemType + " _base_itemType_sub:" + _base_itemType_sub);*/

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




        //以下、実際にアイテムリスト削除と、プレイヤーアイテムへの所持追加処理


        //最終的に生成されるアイテムの個数を決定
        if (result_ok == true) //オリジナル調合の場合
        {
            switch (comp_judge_flag)
            {
                case 0: //新規調合

                    if (komugiko_flag == 0) //小麦粉を使っていない
                    {
                        i = 0;
                        Pate_flag = false;

                        while (i < _additemlist.Count) //かつ生地を使用している場合
                        {
                            if (_additemlist[i]._Add_itemType_sub == "Pate" || _additemlist[i]._Add_itemType_sub == "Cookie_base" || _additemlist[i]._Add_itemType_sub == "Pie_base" || _additemlist[i]._Add_itemType_sub == "Chocolate_base" || _additemlist[i]._Add_itemType_sub == "Cake_base")
                            {
                                Debug.Log("result_kosu: " + result_kosu);
                                Pate_flag = true;
                                result_kosu = _additemlist[i]._Addkosu;
                                break;
                            }
                            i++;
                        }

                        if (Pate_flag != true)
                        {
                            result_kosu = total_kosu / _additemlist.Count;

                            result_kosu *= 2; //2倍出来上がる。
                        }
                    }
                    else //小麦粉を使っている
                    {
                        result_kosu = _additemlist[komugiko_id]._Addkosu; //小麦粉の個数をベースにする。

                        //ランダム 0 ~ 2
                        var rand = Random.Range(0, 2);

                        result_kosu += rand;
                    }

                    break;

                case 1: //トッピング　もしくは、生地合成

                    result_kosu = pitemlistController.final_kettei_kosu1;
                    break;


                default:

                    break;
            }
        }
        else if (recipiresult_ok == true) //レシピ調合の場合
        {
            result_kosu = recipilistController.final_select_kosu;
        }
        else if (topping_result_ok == true) //トッピング調合の場合
        {
            result_kosu = 1;
        }
        else if (roast_result_ok == true) //「焼く」の場合
        {
            result_kosu = pitemlistController.final_kettei_kosu1;
        }

        // アイテムリストの削除処理 //
        Delete_playerItemList();
        
        //新しく作ったアイテムをオリジナルアイテムリストに追加。
        pitemlist.addOriginalItem(_basename, _basemp, _baseday, _basequality, _baserich, _basesweat, _basebitter, _basesour, _basecrispy, _basefluffy, _basesmooth, _basehardness, _basejiggly, _basechewy, _basepowdery, _baseoily, _basewatery, _basegirl1_like, _basecost, _basesell, _basetp[0], _basetp[1], _basetp[2], _basetp[3], _basetp[4], _basetp[5], _basetp[6], _basetp[7], _basetp[8], _basetp[9], result_kosu);
        new_item = pitemlist.player_originalitemlist.Count - 1; //最後に追加されたアイテムが、さっき作った新規アイテムなので、そのIDを入れて置き、リザルトで表示
    
    }


    //プレイヤーアイテムリストから、選んだ材料の削除処理
    void Delete_playerItemList()
    {
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

                pitemlist.deletePlayerItem(_id, final_kette_kosu1);
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

                    pitemlist.deletePlayerItem(_id, final_kette_kosu2);
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

                    pitemlist.deletePlayerItem(_id, final_kette_kosu3);
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





    //
    // 合成の処理・計算を行うメソッド。入口。
    //
    void AddParamMethod()
    {

        //初期化
        _additemlist.Clear();

        _tempmp = 0;
        _tempday = 0;
        _tempquality = 0;
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
                _addkosu = final_kette_kosu1;
                //Debug.Log("_id: " + _id);
                //各パラメータを取得
                Set_addparam();

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
                    _addkosu = final_kette_kosu2;
                    //Debug.Log("_id: " + _id);
                    //各パラメータを取得
                    Set_addparam();

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
                    _addkosu = final_kette_kosu3;

                    //各パラメータを取得
                    Set_addparam();

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

    void Comp_ParamAddMethod()
    {

        total_kosu = 0;

        //①まずは、日数やコストなどの、パラメータ同士を加算する。料金（_basesell）は、品質に基づいて計算する。

        for (i = 0; i < _additemlist.Count; i++) //入れた材料の数だけ、繰り返す。その後、総個数割り算。
        {
            _tempmp += _additemlist[i]._Addmp * _additemlist[i]._Addkosu;
            _tempday += _additemlist[i]._Addday * _additemlist[i]._Addkosu;
            _tempquality += _additemlist[i]._Addquality * _additemlist[i]._Addkosu;

            total_kosu += _additemlist[i]._Addkosu;
        }

        //生地合成のときのみ、ベース個数を含む。
        if (Comp_method_bunki == 1 || Comp_method_bunki == 3)
        {
            total_kosu += base_kosu;
        }

        if (total_kosu == 0) { total_kosu = 1; } //0で割り算する恐れがあるので、回避

        _basemp += _tempmp;
        _baseday += _tempday;
        _basequality += _tempquality;

        _basemp /= total_kosu;
        _baseday /= total_kosu;
        _basequality /= total_kosu;



        //②次に、甘さやサクサク感などの計算処理。

        //1. 新規調合の場合、基本的に全てのアイテム同士のパラメータを加算し、その後個数で割り算する。

        //2. 新規調合で、かつ小麦粉を使った生地作りの場合、
        //そのまま加算はせず、小麦粉をベースに、バター・砂糖・たまごは、各比率を計算し、代入する。

        //3. トッピング調合時、または、生地にアイテムを合成する処理は、加算し、その後割り算。


        if (Comp_method_bunki == 1 || Comp_method_bunki == 3) //トッピング、または生地合成のときの計算。
        {
            for (i = 0; i < _additemlist.Count; i++)
            {
                //各材料を加算していく。ただし、フルーツ・トッピングアイテムは、後に加算処理を行うので、ここでは無視する。
                if (_additemlist[i]._Add_itemType_sub == "Fruits" || _additemlist[i]._Add_itemType_sub == "Potion")
                {
                    total_kosu -= _additemlist[i]._Addkosu;
                    
                }
                else
                {
                    AddTasteParam();
                }
                               
            }

            //ベースのパラメータに、_tempパラメータを加算し、全体個数で割り算 

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

            
            _baserich /= total_kosu;
            _basesweat /= total_kosu;
            _basebitter /= total_kosu;
            _basesour /= total_kosu;
            _basecrispy /= total_kosu;
            _basefluffy /= total_kosu;
            _basesmooth /= total_kosu;
            _basehardness /= total_kosu;
            _basejiggly /= total_kosu;
            _basechewy /= total_kosu;
            _basepowdery /= total_kosu;
            _baseoily /= total_kosu;
            _basewatery /= total_kosu;
           

        }
        else
        {
            Komugiko_Method();

            //最後に、ベースのパラメータに、_tempパラメータを加算する。
            //_basemp += _tempmp;
            //_baseday += _tempday;
            //_basequality += _tempquality;
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


        //④フルーツ・トッピングアイテムの加算処理
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

    void Komugiko_Method()
    {
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

        //材料に小麦粉を使っているか否かで処理を分岐
        switch (komugiko_flag)
        {
            case 0: //材料に小麦粉を使っていない

                //Debug.Log("小麦粉を使っていない");

                for (i = 0; i < _additemlist.Count; i++)
                {
                    //各材料を加算していく。ただし、フルーツ・トッピングアイテムは、後に加算処理を行うので、ここでは無視する。
                    if (_additemlist[i]._Add_itemType_sub == "Fruits" || _additemlist[i]._Add_itemType_sub == "Potion")
                    {
                        total_kosu -= _additemlist[i]._Addkosu;
                        //Debug.Log("フルーツの個数: " + _additemlist[i]._Addkosu);
                    }
                    else
                    {
                        AddTasteParam(); //各材料を加算していく。
                    }
                    
                }

                DivisionTasteparam(); //その後、個数で割り算する。

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

                        //材料の値を計算する。
                        AddRatioTasteParam();
                    }
                }

                //小麦粉の値を、小麦粉以外の材料の種類数の分、割り算。その後、_tempに加える。

                _komugikorich /= etc_mat_count; //3個調合の場合、etc_mat_countは、2が入っているはず。
                _komugikosweat /= etc_mat_count;
                _komugikobitter /= etc_mat_count;
                _komugikosour /= etc_mat_count;
                _komugikocrispy /= etc_mat_count;
                _komugikofluffy /= etc_mat_count;
                _komugikosmooth /= etc_mat_count;
                _komugikohardness /= etc_mat_count;
                _komugikojiggly /= etc_mat_count;
                _komugikochewy /= etc_mat_count;
                _komugikopowdery /= etc_mat_count;
                _komugikooily /= etc_mat_count;
                _komugikowatery /= etc_mat_count;

                _temprich += _komugikorich;
                _tempsweat += _komugikosweat;
                _tempbitter += _komugikobitter;
                _tempsour += _komugikosour;
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
                //Debug.Log("total_kosu: " + total_kosu);
                DivisionTasteparam(); //その後、個数で割り算する。

                break;

            default:
                break;

        }
    }


    void AddSlot_Method()
    {
        //重複した場合は、個別にスロットに入れる。新しいトッピング能力がある場合は、ベースの空のスロットに上書きしていく。
        //ベースの空スロットがなくなった時点で、それ以上合成はできない。

        //加算トッピングの一個目をもとに、ベースのスロット一個目から順番にみていく。

        for (count = 0; count < _additemlist.Count; count++)
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

            _add_ratio = ((float)0.5 + Mathf.Abs(komugiko_distance)) * (float)1.1; //若干、材料のほうが多い分、材料の値が強くなる。
            komugiko_ratio = (float)0.9;
            _bad_ratio = ((float)0.5 + Mathf.Abs(komugiko_distance)) * (float)1.1;
            _komugibad_ratio = (float)0.9;
        }
        //誤差が1.2～2.2
        else if (komugiko_distance >= 1.2 && komugiko_distance < 2.2)
        {
            //小麦2: バター4などの場合。（2.0。　2.0-0.5=1.5の場合）　少し材料が多めになっているとき

            _add_ratio = (float)1.5; 
            komugiko_ratio = (float)0.75;
            _bad_ratio = (float)1.75;
            _komugibad_ratio = (float)0.9;
        }
        //誤差が2.2～3.0
        else if (komugiko_distance >= 2.2 && komugiko_distance < 3.0)
        {
            //小麦2: バター6などの場合。（3.0。　3.0-0.5=2.5の場合）　かなり材料が多めになっているとき

            _add_ratio = (float)1.7; 
            komugiko_ratio = (float)0.4;
            _bad_ratio = (float)2.0;
            _komugibad_ratio = (float)0.8;
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
        _temprich += (int)(_additemlist[i]._Addrich * _add_ratio);
        _tempsweat += (int)(_additemlist[i]._Addsweat * _add_ratio);
        _tempbitter += (int)(_additemlist[i]._Addbitter * _add_ratio);
        _tempsour += (int)(_additemlist[i]._Addsour * _add_ratio);
        _tempcrispy += (int)(_additemlist[i]._Addcrispy * _add_ratio);
        _tempfluffy += (int)(_additemlist[i]._Addfluffy * _add_ratio);
        _tempsmooth += (int)(_additemlist[i]._Addsmooth * _add_ratio);
        _temphardness += (int)(_additemlist[i]._Addhardness * _add_ratio);
        _tempjiggly += (int)(_additemlist[i]._Addjiggly * _add_ratio);
        _tempchewy += (int)(_additemlist[i]._Addchewy * _add_ratio);
        _temppowdery += (int)(_additemlist[i]._Addpowdery * _add_ratio * _bad_ratio);
        _tempoily += (int)(_additemlist[i]._Addoily * _add_ratio * _bad_ratio);
        _tempwatery += (int)(_additemlist[i]._Addwatery * _add_ratio * _bad_ratio);

        //小麦粉の値の計算用。
        _komugikorich += (int)(_additemlist[komugiko_id]._Addrich * komugiko_ratio);
        _komugikosweat += (int)(_additemlist[komugiko_id]._Addsweat * komugiko_ratio);
        _komugikobitter += (int)(_additemlist[komugiko_id]._Addbitter * komugiko_ratio);
        _komugikosour += (int)(_additemlist[komugiko_id]._Addsour * komugiko_ratio);
        _komugikocrispy += (int)(_additemlist[komugiko_id]._Addcrispy * komugiko_ratio);
        _komugikofluffy += (int)(_additemlist[komugiko_id]._Addfluffy * komugiko_ratio);
        _komugikosmooth += (int)(_additemlist[komugiko_id]._Addsmooth * komugiko_ratio);
        _komugikohardness += (int)(_additemlist[komugiko_id]._Addhardness * komugiko_ratio);
        _komugikojiggly += (int)(_additemlist[komugiko_id]._Addjiggly * komugiko_ratio);
        _komugikochewy += (int)(_additemlist[komugiko_id]._Addchewy * komugiko_ratio);
        _komugikopowdery += (int)(_additemlist[komugiko_id]._Addpowdery * komugiko_ratio * _komugibad_ratio);
        _komugikooily += (int)(_additemlist[komugiko_id]._Addoily * komugiko_ratio * _komugibad_ratio);
        _komugikowatery += (int)(_additemlist[komugiko_id]._Addwatery * komugiko_ratio * _komugibad_ratio);
    }

    

    void Set_addparam()
    {

        _addname = database.items[_id].itemName;
        _addmp = database.items[_id].itemMP;
        _addday = database.items[_id].item_day;
        _addquality = database.items[_id].Quality;
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

        for (i = 0; i < database.items[_id].toppingtype.Length; i++)
        {
            _addtp[i] = database.items[_id].toppingtype[i].ToString();
        }

        _additemlist.Add(new ItemAdd(_addname, _addmp, _addday, _addquality, _addrich, _addsweat, _addbitter, _addsour, _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _add_itemType, _add_itemType_sub, _addgirl1_like, _addcost, _addsell, _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9], _addkosu));
    }

    void Set_add_originparam()
    {
        _addname = pitemlist.player_originalitemlist[_id].itemName;
        _addmp = pitemlist.player_originalitemlist[_id].itemMP;
        _addday = pitemlist.player_originalitemlist[_id].item_day;
        _addquality = pitemlist.player_originalitemlist[_id].Quality;
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

        for (i = 0; i < database.items[_id].toppingtype.Length; i++)
        {
            _addtp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
        }

        _additemlist.Add(new ItemAdd(_addname, _addmp, _addday, _addquality, _addrich, _addsweat, _addbitter, _addsour, _addcrispy, _addfluffy, _addsmooth, _addhardness, _addjiggly, _addchewy, _addpowdery, _addoily, _addwatery, _add_itemType, _add_itemType_sub, _addgirl1_like, _addcost, _addsell, _addtp[0], _addtp[1], _addtp[2], _addtp[3], _addtp[4], _addtp[5], _addtp[6], _addtp[7], _addtp[8], _addtp[9], _addkosu));
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
                _basemp = database.items[_id].itemMP;
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
                _basemp = pitemlist.player_originalitemlist[_id].itemMP;
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



    IEnumerator renkin_exp_up()
    {

        //_slotHyouji1[]は、一度名前を、全て空白に初期化
        for (i = 0; i < _slotHyouji1.Length; i++)
        {
            _slotHyouji1[i] = "";
        }

        //カード正式名称（ついてるスロット名も含めた名前）
        for (i = 0; i < _slot.Length; i++)
        {
            count = 0;

            //スロット名を日本語に変換。DBから変換。Nonは、空白になる。
            while (count < slotnamedatabase.slotname_lists.Count)
            {
                if (slotnamedatabase.slotname_lists[count].slotName == pitemlist.player_originalitemlist[new_item].toppingtype[i].ToString())
                {
                    _slotHyouji1[i] = "<color=#0000FF>" + slotnamedatabase.slotname_lists[count].slot_Hyouki_2 + "</color>";
                    break;
                }
                count++;
            }
        }

        _text.text = "調合完了！ " +
            _slotHyouji1[0] + _slotHyouji1[1] + _slotHyouji1[2] + _slotHyouji1[3] + _slotHyouji1[4] + _slotHyouji1[5] + _slotHyouji1[6] + _slotHyouji1[7] + _slotHyouji1[8] + _slotHyouji1[9] + pitemlist.player_originalitemlist[new_item].itemNameHyouji + 
            " が" + result_kosu + "個 できました！" + "\n" + 
            "錬金経験値" + databaseCompo.compoitems[result_ID].renkin_Bexp + "上がった！";

        Debug.Log(database.items[result_item].itemNameHyouji + "が出来ました！");

        while (!Input.GetMouseButtonDown(0)) yield return null;

    }

    IEnumerator extreme_exp_up()
    {

        _text.text = "調合完了！ " + database.items[new_item].itemNameHyouji +
            " を閃いた！" + "\n" +
            "錬金経験値" + databaseCompo.compoitems[result_ID].renkin_Bexp + "上がった！";

        Debug.Log(database.items[result_item].itemNameHyouji + "が出来ました！");

        while (!Input.GetMouseButtonDown(0)) yield return null;

    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
