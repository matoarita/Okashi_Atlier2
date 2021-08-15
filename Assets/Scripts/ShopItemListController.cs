using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ShopItemListController : MonoBehaviour
{

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
        //リスト表示のフラグチェック　パティシエレベルが○○以上だと、品ぞろえが増えるなど。
        //Check_ShopListFlag();

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        switch(SceneManager.GetActiveScene().name)
        {
            case "Shop":

                for (i = 0; i < shop_database.shopitems.Count; i++)
                {
                    //1～だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemType=0は基本の材料系
                    if (shop_database.shopitems[i].shop_item_hyouji > 0 && shop_database.shopitems[i].shop_item_hyouji_on
                        && shop_database.shopitems[i].shop_itemType == 0)
                    {
                        if (shop_database.shopitems[i].shop_itemzaiko > 0)
                        {

                            drawItem();

                        }
                    }
                }
                break;

            case "Farm":

                for (i = 0; i < shop_database.farmitems.Count; i++)
                {
                    //1だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemType=0は基本の材料系
                    if (shop_database.farmitems[i].shop_item_hyouji > 0 && shop_database.farmitems[i].shop_item_hyouji_on
                        && shop_database.farmitems[i].shop_itemType == 0)
                    {
                        if (shop_database.farmitems[i].shop_itemzaiko > 0)
                        {

                            drawFarmItem();

                        }
                    }
                }
                break;

            case "Emerald_Shop":

                for (i = 0; i < shop_database.emeraldshop_items.Count; i++)
                {
                    //1だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemTypeは、エメラルショップの場合、ひとまずなし。
                    if (shop_database.emeraldshop_items[i].shop_item_hyouji > 0 && shop_database.emeraldshop_items[i].shop_item_hyouji_on)
                    {
                        if (shop_database.emeraldshop_items[i].shop_itemType == 0 || shop_database.emeraldshop_items[i].shop_itemType == 5)
                        {
                            if (shop_database.emeraldshop_items[i].shop_itemzaiko > 0)
                            {
                                drawEmerarldShopItem();
                            }
                        }
                    }
                }
                break;

            default:

                break;
        }
        
    }

    // トッピング系
    void reset_and_DrawView_Topping()
    {
        //リスト表示のフラグチェック　パティシエレベルが○○以上だと、品ぞろえが増えるなど。
        //Check_ShopListFlag();

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                for (i = 0; i < shop_database.shopitems.Count; i++)
                {
                    if (shop_database.shopitems[i].shop_item_hyouji > 0 && shop_database.shopitems[i].shop_item_hyouji_on
                        && shop_database.shopitems[i].shop_itemType == 3)
                    {
                        if (shop_database.shopitems[i].shop_itemzaiko > 0)
                        {

                            drawItem();

                        }
                    }
                }
                break;

            case "Farm":

                for (i = 0; i < shop_database.farmitems.Count; i++)
                {
                    if (shop_database.farmitems[i].shop_item_hyouji > 0 && shop_database.farmitems[i].shop_item_hyouji_on
                        && shop_database.farmitems[i].shop_itemType == 3)
                    {
                        if (shop_database.farmitems[i].shop_itemzaiko > 0)
                        {

                            drawFarmItem();

                        }
                    }
                }
                break;

            case "Emerald_Shop":

                for (i = 0; i < shop_database.emeraldshop_items.Count; i++)
                {
                    //1だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemTypeは、エメラルショップの場合、ひとまずなし。
                    if (shop_database.emeraldshop_items[i].shop_item_hyouji > 0 && shop_database.emeraldshop_items[i].shop_item_hyouji_on
                        && shop_database.emeraldshop_items[i].shop_itemType == 3)
                    {
                        if (shop_database.emeraldshop_items[i].shop_itemzaiko > 0)
                        {
                            drawEmerarldShopItem();
                        }
                    }
                }
                break;

            default:

                break;
        }
        
    }

    // 器材系
    void reset_and_DrawView_Machine()
    {
        //リスト表示のフラグチェック　パティシエレベルが○○以上だと、品ぞろえが増えるなど。
        //Check_ShopListFlag();

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                for (i = 0; i < shop_database.shopitems.Count; i++)
                {
                    if (shop_database.shopitems[i].shop_item_hyouji > 0 && shop_database.shopitems[i].shop_item_hyouji_on)
                    {
                        if (shop_database.shopitems[i].shop_itemType == 2)
                        {
                            if (shop_database.shopitems[i].shop_itemzaiko > 0)
                            {

                                drawItem();

                            }
                        }
                    }
                }
                break;

            case "Farm":

                for (i = 0; i < shop_database.farmitems.Count; i++)
                {
                    if (shop_database.farmitems[i].shop_item_hyouji > 0 && shop_database.farmitems[i].shop_item_hyouji_on)
                    {
                        if (shop_database.farmitems[i].shop_itemType == 2)
                        {
                            if (shop_database.farmitems[i].shop_itemzaiko > 0)
                            {

                                drawFarmItem();

                            }
                        }
                    }
                }
                break;

            case "Emerald_Shop":

                for (i = 0; i < shop_database.emeraldshop_items.Count; i++)
                {
                    //1だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemTypeは、エメラルショップの場合、ひとまずなし。
                    if (shop_database.emeraldshop_items[i].shop_item_hyouji > 0 && shop_database.emeraldshop_items[i].shop_item_hyouji_on
                        && shop_database.emeraldshop_items[i].shop_itemType == 2)
                    {
                        if (shop_database.emeraldshop_items[i].shop_itemzaiko > 0)
                        {
                            drawEmerarldShopItem();
                        }
                    }
                }
                break;

            default:

                break;
        }
        
    }

    // レシピ系
    void reset_and_DrawView_Recipi()
    {
        //リスト表示のフラグチェック　パティシエレベルが○○以上だと、品ぞろえが増えるなど。
        //Check_ShopListFlag();

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                for (i = 0; i < shop_database.shopitems.Count; i++)
                {
                    if (shop_database.shopitems[i].shop_item_hyouji > 0 && shop_database.shopitems[i].shop_item_hyouji_on
                        && shop_database.shopitems[i].shop_itemType == 1)
                    {
                        if (shop_database.shopitems[i].shop_itemzaiko > 0)
                        {

                            drawItem();

                        }
                    }
                }
                break;

            case "Farm":

                for (i = 0; i < shop_database.farmitems.Count; i++)
                {
                    if (shop_database.farmitems[i].shop_item_hyouji > 0 && shop_database.farmitems[i].shop_item_hyouji_on
                        && shop_database.farmitems[i].shop_itemType == 1)
                    {
                        if (shop_database.farmitems[i].shop_itemzaiko > 0)
                        {

                            drawFarmItem();

                        }
                    }
                }
                break;

            case "Emerald_Shop":

                for (i = 0; i < shop_database.emeraldshop_items.Count; i++)
                {
                    //1だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemTypeは、エメラルショップの場合、ひとまずなし。
                    if (shop_database.emeraldshop_items[i].shop_item_hyouji > 0 && shop_database.emeraldshop_items[i].shop_item_hyouji_on
                        && shop_database.emeraldshop_items[i].shop_itemType == 1)
                    {
                        if (shop_database.emeraldshop_items[i].shop_itemzaiko > 0)
                        {
                            drawEmerarldShopItem();
                        }
                    }
                }
                break;

            default:

                break;
        }

        
    }

    // お土産系
    void reset_and_DrawView_Etc()
    {
        //リスト表示のフラグチェック　パティシエレベルが○○以上だと、品ぞろえが増えるなど。
        //Check_ShopListFlag();

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _shop_listitem.Clear();
        //Debug.Log(shop_database.shopitems.Count);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                for (i = 0; i < shop_database.shopitems.Count; i++)
                {
                    if (shop_database.shopitems[i].shop_item_hyouji > 0 && shop_database.shopitems[i].shop_item_hyouji_on
                        && shop_database.shopitems[i].shop_itemType == 6)
                    {
                        if (shop_database.shopitems[i].shop_itemzaiko > 0)
                        {

                            drawItem();

                        }
                    }
                }
                break;

            case "Farm":

                for (i = 0; i < shop_database.farmitems.Count; i++)
                {
                    if (shop_database.farmitems[i].shop_item_hyouji > 0 && shop_database.farmitems[i].shop_item_hyouji_on
                        && shop_database.farmitems[i].shop_itemType == 6)
                    {
                        if (shop_database.farmitems[i].shop_itemzaiko > 0)
                        {

                            drawFarmItem();

                        }
                    }
                }
                break;

            case "Emerald_Shop":

                for (i = 0; i < shop_database.emeraldshop_items.Count; i++)
                {
                    //1だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemTypeは、エメラルショップの場合、ひとまずなし。
                    if (shop_database.emeraldshop_items[i].shop_item_hyouji > 0 && shop_database.emeraldshop_items[i].shop_item_hyouji_on
                        && shop_database.emeraldshop_items[i].shop_itemType == 6)
                    {
                        if (shop_database.emeraldshop_items[i].shop_itemzaiko > 0)
                        {
                            drawEmerarldShopItem();
                        }
                    }
                }
                break;

            default:

                break;
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

    void drawFarmItem()
    {
        _shop_listitem.Add(Instantiate(shopitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _shop_listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、３つのテキストコンポを格納する。
        _Img = _shop_listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ
        _togglebg = _shop_listitem[list_count].transform.Find("Background").GetComponent<Image>(); //アイコン背景データ

        _toggle_itemID = _shop_listitem[list_count].GetComponent<shopitemSelectToggle>();
        _toggle_itemID.toggle_shop_ID = shop_database.farmitems[i].shop_ID; //ショップに登録されている、ショップデータベース上のアイテムID。iと同じ値になる。
        _toggle_itemID.toggle_shopitem_ID = shop_database.farmitems[i].shop_itemID; //ショップに登録されている、アイテムDB上のアイテムID
        _toggle_itemID.toggle_shopitem_type = shop_database.farmitems[i].shop_itemType; //通常アイテムか、イベントアイテムの判定用タイプ
        _toggle_itemID.toggle_shopitem_nameHyouji = shop_database.farmitems[i].shop_itemNameHyouji; //表示用の名前
        _toggle_itemID.toggle_shopitem_costprice = shop_database.farmitems[i].shop_costprice; //単価


        item_name = shop_database.farmitems[i].shop_itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

        _text[0].text = item_name;

        item_cost = shop_database.farmitems[i].shop_costprice;

        _text[2].text = item_cost.ToString(); //価格

        item_zaiko = shop_database.farmitems[i].shop_itemzaiko;

        //_text[4].text = item_zaiko.ToString(); //在庫

        texture2d = shop_database.farmitems[i].shop_itemIcon;
        _Img.sprite = texture2d;

        //お金が足りない場合は、選択できないようにする。
        if (PlayerStatus.player_money < shop_database.farmitems[i].shop_costprice)
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
        _toggle_itemID.toggle_shop_ID = shop_database.emeraldshop_items[i].shop_ID; //ショップに登録されている、ショップデータベース上のアイテムID。iと同じ値になる。
        _toggle_itemID.toggle_shopitem_ID = shop_database.emeraldshop_items[i].shop_itemID; //ショップに登録されている、アイテムDB上のアイテムID
        _toggle_itemID.toggle_shopitem_type = shop_database.emeraldshop_items[i].shop_itemType; //通常アイテムか、イベントアイテムの判定用タイプ
        _toggle_itemID.toggle_shopitem_nameHyouji = shop_database.emeraldshop_items[i].shop_itemNameHyouji; //表示用の名前
        _toggle_itemID.toggle_shopitem_costprice = shop_database.emeraldshop_items[i].shop_costprice; //単価
        _toggle_itemID.toggle_shopitem_dongri_type = shop_database.emeraldshop_items[i].shop_dongriType; //どんぐりタイプ


        item_name = shop_database.emeraldshop_items[i].shop_itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

        _text[0].text = item_name;

        item_cost = shop_database.emeraldshop_items[i].shop_costprice;

        _text[2].text = item_cost.ToString(); //必要なエメラルどんぐりの数

        item_zaiko = shop_database.emeraldshop_items[i].shop_itemzaiko;

        //_text[4].text = item_zaiko.ToString(); //在庫

        texture2d = shop_database.emeraldshop_items[i].shop_itemIcon;
        _Img.sprite = texture2d;

        //エメラルどんぐりが足りない場合は、選択できないようにする。
        switch(shop_database.emeraldshop_items[i].shop_dongriType)
        {
            case 0: //エメラルどんぐり

                _ImgDongriIcon.sprite = texture_emeraldIcon;                
                emeraldonguriID = pitemlist.SearchItemString("emeralDongri");
                if (pitemlist.playeritemlist[emeraldonguriID] < shop_database.emeraldshop_items[i].shop_costprice)
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
                emeraldonguriID = pitemlist.SearchItemString("sapphireDongri");
                if (pitemlist.playeritemlist[emeraldonguriID] < shop_database.emeraldshop_items[i].shop_costprice)
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

        for (i = 0; i < shop_database.farmitems.Count; i++)
        {
            if (shop_database.farmitems[i].shop_item_hyouji == flag_num)
            {
                shop_database.farmitems[i].shop_item_hyouji_on = true;
            }
        }

        for (i = 0; i < shop_database.emeraldshop_items.Count; i++)
        {
            if (shop_database.emeraldshop_items[i].shop_item_hyouji == flag_num)
            {
                shop_database.emeraldshop_items[i].shop_item_hyouji_on = true;
            }
        }
    }
}
