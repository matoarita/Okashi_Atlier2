using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ShopItemListController : MonoBehaviour
{
    //
    //ショップの品揃えのコントローラー
    //

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    public List<GameObject> _shop_listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text[] _text = new Text[3];
    private Sprite texture2d;
    private Sprite texture_emeraldIcon;
    private Sprite texture_sapphireIcon;
    private Sprite touchon, touchoff;
    private Image _Img;
    private Image _ImgDongriIcon;
    private Image _togglebg;
    private shopitemSelectToggle _toggle_itemID;

    private Girl1_status girl1_status;

    private GameObject shopitem_Prefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private GameObject shopitem_Prefab2; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。

    private PlayerItemList pitemlist;

    public GameObject cardImage_onoff_pcontrol;

    private ItemDataBase database;

    private ItemShopDataBase shop_database;

    private string item_name;
    private int item_cost;
    private int item_zaiko;

    private int max;
    private int count;
    private int i;
    private int rnd;
    private int shop_hyouji_flag;

    public int shop_count; //選択したリスト番号が入る。
    public int shop_kettei_ID; //ショップデータベースIDが入る。
    public int shop_kettei_item1; //選択したアイテムのアイテムIDが入る。通常アイテムなら、アイテムID、イベントアイテムならイベントリストのアイテムID。
    public int shop_itemType;
    public int shop_dongriType;
    public int shop_costprice; //金額
    public string shop_itemName_Hyouji; //最終的に買うアイテム名がはいる。

    public int shop_final_itemkosu_1; //選択したアイテムIDの個数が入る。

    public bool shop_final_select_flag;

    public List<GameObject> category_toggle = new List<GameObject>();
    private int category_status;

    private int emeraldonguriID;
    private bool sale_ON;


    void Awake() //Startより手前で先に読みこんで、OnEnableの挙動のエラー回避
    {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子


        //スクロールビュー内の、コンテンツ要素を取得
        content = GameObject.FindWithTag("ShopitemListContent");
        shopitem_Prefab = (GameObject)Resources.Load("Prefabs/shopitemSelectToggle");
        shopitem_Prefab2 = (GameObject)Resources.Load("Prefabs/emeralditemSelectToggle");

        //アイコン背景画像データの取得
        touchon = Resources.Load<Sprite>("Sprites/Window/sabwindowB");
        touchoff = Resources.Load<Sprite>("Sprites/Window/checkbox");

        //どんぐりアイコンの取得
        texture_emeraldIcon = Resources.Load<Sprite>("Sprites/Icon/emeralDonguri_icon");
        texture_sapphireIcon = Resources.Load<Sprite>("Sprites/Icon/sapphireDonguri_icon");

        foreach (Transform child in this.transform.Find("CategoryView/Viewport/Content/").transform)
        {
            //Debug.Log(child.name);           
            category_toggle.Add(child.gameObject);
        }

        i = 0;
        category_status = 0;
        shop_final_select_flag = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        //ウィンドウがアクティヴになった瞬間だけ読み出される
        //Debug.Log("OnEnable");

        //ショップセール判定用　日付を更新
        if (PlayerStatus.player_day != GameMgr.Shopday)
        {
            //Debug.Log("セール 判定");
            GameMgr.Shopday = PlayerStatus.player_day;

            //セール判定
            rnd = Random.Range(0, 5);
            if (rnd <= 1)
            {
                GameMgr.Sale_ON = true;
                Debug.Log("セール ON");
            }
            else
            {
                GameMgr.Sale_ON = false;
                Debug.Log("セール OFF");
            }
        }

        //一度表示フラグはリセット
        OFFShopListFlag();
        //リスト表示のフラグチェック　パティシエレベルが○○以上だと、品ぞろえが増えるなど。
        Check_ShopListFlag();

        for (i = 0; i < category_toggle.Count; i++)
        {
            category_toggle[i].GetComponent<Toggle>().isOn = false;
        }
        category_toggle[0].GetComponent<Toggle>().isOn = true;
        reset_and_DrawView();
        
    }

    public void ShopList_DrawView()
    {
        if (category_toggle[0].GetComponent<Toggle>().isOn == true)
        {
            category_status = 0;
            reset_and_DrawView();
        }
    }

    public void ShopList_DrawView2()
    {
        if (category_toggle[1].GetComponent<Toggle>().isOn == true)
        {
            category_status = 1;
            reset_and_DrawView_Topping();
        }
    }

    public void ShopList_DrawView3()
    {
        if (category_toggle[2].GetComponent<Toggle>().isOn == true)
        {
            category_status = 2;
            reset_and_DrawView_Machine();
        }
    }

    public void ShopList_DrawView4()
    {
        if (category_toggle[3].GetComponent<Toggle>().isOn == true)
        {
            category_status = 3;
            reset_and_DrawView_Recipi();
        }
    }

    public void ShopList_DrawView5()
    {
        if (category_toggle[4].GetComponent<Toggle>().isOn == true)
        {
            category_status = 4;
            reset_and_DrawView_Etc();
        }
    }

    public void ReDraw()
    {       

        switch(category_status)
        {
            case 0:

                reset_and_DrawView();
                break;

            case 1:

                reset_and_DrawView_Topping();
                break;

            case 2:

                reset_and_DrawView_Machine();
                break;

            case 3:

                reset_and_DrawView_Recipi();
                break;

            case 4:

                reset_and_DrawView_Etc();
                break;

        }
    }

    // リストビューの描画部分。重要。
    void reset_and_DrawView()
    {

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        check_ShopTypeToDraw(0, 5, 1); //3番目のフラグは、2番目の数字をtypeとして使うか否か。1でなければ、2番目の数字は無視してよい。
                
    }    

    // トッピング系
    void reset_and_DrawView_Topping()
    {


        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        check_ShopTypeToDraw(3, 0, 0);
    }

    // 器材系
    void reset_and_DrawView_Machine()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        check_ShopTypeToDraw(2, 0, 0);

    }

    // レシピ系
    void reset_and_DrawView_Recipi()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        check_ShopTypeToDraw(1, 0, 0);


    }

    // お土産系
    void reset_and_DrawView_Etc()
    {

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        check_ShopTypeToDraw(6, 0, 0);

    }

    void check_ShopTypeToDraw(int _shop_itemtype, int _shop_itemtype2, int checkFlag)
    {
        switch (GameMgr.Scene_Category_Num)
        {
            case 20: //お店

                switch (GameMgr.Scene_Name)
                {
                    //オランジーナショップ関係は10000~以降
                    case "Or_Shop_A1":

                        CheckHyouji(10000, _shop_itemtype);                       
                        break;

                    case "Or_Shop_B1":

                        CheckHyouji(20000, _shop_itemtype);
                        break;

                    case "Or_Shop_C1":

                        CheckHyouji(30000, _shop_itemtype);
                        break;

                    case "Or_Shop_D1":

                        CheckHyouji(40000, _shop_itemtype);
                        break;

                    default:

                        CheckHyouji(0, _shop_itemtype);
                        break;
                }

                break;

            case 40: //ファーム

                CheckHyouji(1000, _shop_itemtype);
                break;

            case 50: //エメラルショップ

                CheckHyoujiEmerald(2000, _shop_itemtype, _shop_itemtype2, checkFlag);               
                break;

            default:

                break;
        }
    }

    void CheckHyouji(int _id, int _type)
    {
        i = 0;
        while (i < shop_database.shopitems.Count)
        {
            if (shop_database.shopitems[i].shop_ID >= _id) //shopIDの先頭のIDから読み出し
            {
                //1～だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemType=0は基本の材料系
                if (shop_database.shopitems[i].shop_item_hyouji > 0 && shop_database.shopitems[i].shop_item_hyouji_on
                    && shop_database.shopitems[i].shop_itemType == _type)
                {
                    if (shop_database.shopitems[i].shop_itemzaiko > 0)
                    {
                        drawItem();
                    }
                }

                //そのシートの終わりにエンドフラッグ=1をうってるはずなので、そこがシートの終点になる。
                if (shop_database.shopitems[i].read_endflag == 1)
                {
                    break;
                }
            }
            i++;
        }
    }

    void CheckHyoujiEmerald(int _id, int _type1, int _type2, int _chkflag)
    {
        i = 0;
        while (i < shop_database.shopitems.Count)
        {
            if (shop_database.shopitems[i].shop_ID >= _id) //shopIDの先頭のIDから読み出し エメラルドは　10000~
            {
                //1だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemTypeは、エメラルショップの場合、ひとまずなし。
                if (shop_database.shopitems[i].shop_item_hyouji > 0 && shop_database.shopitems[i].shop_item_hyouji_on)
                {
                    if (_chkflag == 1) //itemtype　2種類
                    {
                        if (shop_database.shopitems[i].shop_itemType == _type1 || shop_database.shopitems[i].shop_itemType == _type2)
                        {
                            if (shop_database.shopitems[i].shop_itemzaiko > 0)
                            {
                                drawEmerarldShopItem();
                            }
                        }
                    }
                    else
                    {
                        if (shop_database.shopitems[i].shop_itemType == _type1)
                        {
                            if (shop_database.shopitems[i].shop_itemzaiko > 0)
                            {
                                drawEmerarldShopItem();
                            }
                        }
                    }
                }

                //そのシートの終わりにエンドフラッグ=1をうってるはずなので、そこがシートの終点になる。
                if (shop_database.shopitems[i].read_endflag == 1)
                {
                    break;
                }
            }
            i++;
        }
    }

    void drawItem()
    {
        _shop_listitem.Add(Instantiate(shopitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _shop_listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、３つのテキストコンポを格納する。
        _Img = _shop_listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ
        _togglebg = _shop_listitem[list_count].transform.Find("Background").GetComponent<Image>(); //アイコン背景データ

        _toggle_itemID = _shop_listitem[list_count].GetComponent<shopitemSelectToggle>();
        _toggle_itemID.toggle_shop_ID = shop_database.shopitems[i].shop_ID; //ショップに登録されている、ショップデータベース上のアイテムID。iと同じ値になる。
        _toggle_itemID.toggle_shopitem_ID = shop_database.shopitems[i].shop_itemID; //ショップに登録されている、アイテムDB上のアイテムID
        _toggle_itemID.toggle_shopitem_type = shop_database.shopitems[i].shop_itemType; //通常アイテムか、イベントアイテムの判定用タイプ
        _toggle_itemID.toggle_shopitem_nameHyouji = shop_database.shopitems[i].shop_itemNameHyouji; //表示用の名前
        _toggle_itemID.toggle_shopitem_costprice = shop_database.shopitems[i].shop_costprice; //単価

        //セール表示
        if(shop_database.shopitems[i].shop_item_hyouji == 100)
        {
            _shop_listitem[list_count].transform.Find("SalePanel").gameObject.SetActive(true);
        }


        item_name = shop_database.shopitems[i].shop_itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

        _text[0].text = item_name;

        item_cost = shop_database.shopitems[i].shop_costprice;

        _text[2].text = item_cost.ToString(); //価格

        item_zaiko = shop_database.shopitems[i].shop_itemzaiko;

        //_text[4].text = item_zaiko.ToString(); //在庫

        texture2d = shop_database.shopitems[i].shop_itemIcon;
        _Img.sprite = texture2d;

        //お金が足りない場合は、選択できないようにする。
        if (PlayerStatus.player_money < shop_database.shopitems[i].shop_costprice)
        {
            _shop_listitem[list_count].GetComponent<Toggle>().interactable = false;
            //_togglebg.sprite = touchoff;
        }
        else
        {
            _shop_listitem[list_count].GetComponent<Toggle>().interactable = true;
            //_togglebg.sprite = touchon;
        }
        //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);

        ++list_count;
    }

    void drawEmerarldShopItem()
    {
        _shop_listitem.Add(Instantiate(shopitem_Prefab2, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _shop_listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、３つのテキストコンポを格納する。
        _Img = _shop_listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ
        _ImgDongriIcon = _shop_listitem[list_count].transform.Find("Background/Item_price_emerald").GetComponent<Image>(); //どんぐりアイコンデータ
        _togglebg = _shop_listitem[list_count].transform.Find("Background").GetComponent<Image>(); //アイコン背景データ

        _toggle_itemID = _shop_listitem[list_count].GetComponent<shopitemSelectToggle>();
        _toggle_itemID.toggle_shop_ID = shop_database.shopitems[i].shop_ID; //ショップに登録されている、ショップデータベース上のアイテムID。iと同じ値になる。
        _toggle_itemID.toggle_shopitem_ID = shop_database.shopitems[i].shop_itemID; //ショップに登録されている、アイテムDB上のアイテムID
        _toggle_itemID.toggle_shopitem_type = shop_database.shopitems[i].shop_itemType; //通常アイテムか、イベントアイテムの判定用タイプ
        _toggle_itemID.toggle_shopitem_nameHyouji = shop_database.shopitems[i].shop_itemNameHyouji; //表示用の名前
        _toggle_itemID.toggle_shopitem_costprice = shop_database.shopitems[i].shop_costprice; //単価
        _toggle_itemID.toggle_shopitem_dongri_type = shop_database.shopitems[i].shop_dongriType; //どんぐりタイプ

        //セール表示
        if (shop_database.shopitems[i].shop_item_hyouji == 100)
        {
            _shop_listitem[list_count].transform.Find("SalePanel").gameObject.SetActive(true);
        }

        item_name = shop_database.shopitems[i].shop_itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

        _text[0].text = item_name;

        item_cost = shop_database.shopitems[i].shop_costprice;

        _text[2].text = item_cost.ToString(); //必要なエメラルどんぐりの数

        item_zaiko = shop_database.shopitems[i].shop_itemzaiko;

        //_text[4].text = item_zaiko.ToString(); //在庫

        texture2d = shop_database.shopitems[i].shop_itemIcon;
        _Img.sprite = texture2d;

        //エメラルどんぐりが足りない場合は、選択できないようにする。
        switch(shop_database.shopitems[i].shop_dongriType)
        {
            case 0: //エメラルどんぐり

                _ImgDongriIcon.sprite = texture_emeraldIcon;                
                //emeraldonguriID = pitemlist.SearchItemString("emeralDongri");
                if (pitemlist.playeritemlist["emeralDongri"] < shop_database.shopitems[i].shop_costprice)
                {
                    _shop_listitem[list_count].GetComponent<Toggle>().interactable = false;
                    //_togglebg.sprite = touchoff;
                }
                else
                {
                    _shop_listitem[list_count].GetComponent<Toggle>().interactable = true;
                    //_togglebg.sprite = touchon;
                }
                break;

            case 1: //サファイアどんぐり

                _ImgDongriIcon.sprite = texture_sapphireIcon;
                //emeraldonguriID = pitemlist.SearchItemString("sapphireDongri");
                if (pitemlist.playeritemlist["sapphireDongri"] < shop_database.shopitems[i].shop_costprice)
                {
                    _shop_listitem[list_count].GetComponent<Toggle>().interactable = false;
                    //_togglebg.sprite = touchoff;
                }
                else
                {
                    _shop_listitem[list_count].GetComponent<Toggle>().interactable = true;
                    //_togglebg.sprite = touchon;
                }
                break;
        }
        
        //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);

        ++list_count;
    }

    //品物追加イベントにしたがって、ショップのリストに追加
    void Check_ShopListFlag()
    {
        shop_hyouji_flag = 1; //最小は1
        Check_ONShopListFlag(shop_hyouji_flag);

        /*
        if(GameMgr.ShopLVEvent_stage[0])
        {
            shop_hyouji_flag = 2;
            Check_ONShopListFlag(shop_hyouji_flag);
        }
        if (GameMgr.ShopLVEvent_stage[1])
        {
            shop_hyouji_flag = 3;
            Check_ONShopListFlag(shop_hyouji_flag);
        }
        if (GameMgr.ShopLVEvent_stage[2])
        {
            shop_hyouji_flag = 4;
            Check_ONShopListFlag(shop_hyouji_flag);
        }
        if (GameMgr.ShopLVEvent_stage[3]) //かわいいトッピング追加
        {
            shop_hyouji_flag = 5;
            Check_ONShopListFlag(shop_hyouji_flag);
        }*/

        //２の場合、最初から全てでている。
        shop_hyouji_flag = 2;
        Check_ONShopListFlag(shop_hyouji_flag);
        shop_hyouji_flag = 3;
        Check_ONShopListFlag(shop_hyouji_flag);
        shop_hyouji_flag = 4;
        Check_ONShopListFlag(shop_hyouji_flag);
        shop_hyouji_flag = 5;
        Check_ONShopListFlag(shop_hyouji_flag);
        shop_hyouji_flag = 2000;
        Check_ONShopListFlag(shop_hyouji_flag);

        //セールや日によって出たりでなかったりする品物
        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                break;

            case "Farm":

                if (GameMgr.Sale_ON)
                {
                    //Debug.Log("セール品　表示");
                    shop_hyouji_flag = 100; //100番台はセール品
                    Check_ONShopListFlag(shop_hyouji_flag);
                }
                break;
        }

        //その他、条件をみたすとでてくるショップ品
        switch (SceneManager.GetActiveScene().name)
        {
            case "Emerald_Shop":

                if (GameMgr.GirlLoveSubEvent_stage1[101])
                {
                    shop_hyouji_flag = 1000;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }
                if (GameMgr.GirlLoveSubEvent_stage1[102])
                {
                    shop_hyouji_flag = 1001;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }

                if (GameMgr.Story_Mode != 0)
                {
                    //エクストラモードのときにでるアイテム
                    shop_hyouji_flag = 2000;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }

                /*
                if (GameMgr.MapEvent_01[0]) //近くの森へいったことがある
                {
                    shop_hyouji_flag = 2100;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }
                if (GameMgr.MapEvent_07[0]) //ベリーファームへいったことがある
                {
                    shop_hyouji_flag = 2101;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }
                if (GameMgr.MapEvent_05[0]) //ラベンダー畑へいったことがある
                {
                    shop_hyouji_flag = 2102;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }
                if (GameMgr.MapEvent_03[0]) //いちご畑へいったことがある
                {
                    shop_hyouji_flag = 2103;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }
                if (GameMgr.MapEvent_04[0]) //ひまわり畑へいったことがある
                {
                    shop_hyouji_flag = 2104;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }
                if (GameMgr.MapEvent_02[0]) //井戸へいったことがある
                {
                    shop_hyouji_flag = 2105;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }
                if (GameMgr.MapEvent_06[0]) //バードサンクチュアリへいったことがある
                {
                    shop_hyouji_flag = 2106;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }
                if (GameMgr.SearchEventCollectionFlag("event9")) //白猫のお墓へいったことがある
                {
                    shop_hyouji_flag = 2107;
                    Check_ONShopListFlag(shop_hyouji_flag);
                }*/

                break;
        }
    }

    void OFFShopListFlag()
    {
        for (i = 0; i < shop_database.shopitems.Count; i++)
        {

            shop_database.shopitems[i].shop_item_hyouji_on = false;

        }
    }

    void Check_ONShopListFlag(int flag_num)
    {
        for (i = 0; i < shop_database.shopitems.Count; i++)
        {
            if (shop_database.shopitems[i].shop_item_hyouji == flag_num)
            {
                shop_database.shopitems[i].shop_item_hyouji_on = true;
            }
        }
    }
}
