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
    private shopitemSelectToggle _toggle_itemID;

    private GameObject shopitem_Prefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private Button getItemAddButton;

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

    public int shop_count; //選択したリスト番号が入る。
    public int shop_kettei_ID; //ショップデータベースIDが入る。
    public int shop_kettei_item1; //選択したアイテムのアイテムIDが入る。通常アイテムなら、アイテムID、イベントアイテムならイベントリストのアイテムID。
    public int shop_itemType;

    public int shop_final_itemkosu_1; //選択したアイテムIDの個数が入る。

    public bool shop_final_select_flag;


    void Awake() //Startより手前で先に読みこんで、OnEnableの挙動のエラー回避
    {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();


        //スクロールビュー内の、コンテンツ要素を取得
        content = GameObject.FindWithTag("ShopitemListContent");
        shopitem_Prefab = (GameObject)Resources.Load("Prefabs/shopitemSelectToggle");

        i = 0;
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

        reset_and_DrawView();

    }

    public void ShopList_DrawView()
    {
        //アイテムリストを表示中に、アイテムを追加した場合、リアルタイムに表示を更新する

        reset_and_DrawView();
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

        for (i = 0; i < shop_database.shopitems.Count; i++)
        {
            if (shop_database.shopitems[i].shop_item_hyouji > 0) //1だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。
            {
                if (shop_database.shopitems[i].shop_itemzaiko > 0)
                {


                    _shop_listitem.Add(Instantiate(shopitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
                    _text = _shop_listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、３つのテキストコンポを格納する。

                    _toggle_itemID = _shop_listitem[list_count].GetComponent<shopitemSelectToggle>();
                    _toggle_itemID.toggle_shop_ID = shop_database.shopitems[i].shop_ID; //ショップに登録されている、ショップデータベース上のアイテムID。iと同じ値になる。
                    _toggle_itemID.toggle_shopitem_ID = shop_database.shopitems[i].shop_itemID; //ショップに登録されている、アイテムDB上のアイテムID
                    _toggle_itemID.toggle_shopitem_type = shop_database.shopitems[i].shop_itemType; //通常アイテムか、イベントアイテムの判定用タイプ


                    item_name = shop_database.shopitems[i].shop_itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

                    _text[0].text = item_name;

                    item_cost = shop_database.shopitems[i].shop_costprice;

                    _text[2].text = item_cost.ToString(); //価格

                    item_zaiko = shop_database.shopitems[i].shop_itemzaiko;

                    _text[4].text = item_zaiko.ToString(); //在庫

                    //お金が足りない場合は、選択できないようにする。
                    if (PlayerStatus.player_money < shop_database.shopitems[i].shop_costprice)
                    {
                        _shop_listitem[list_count].GetComponent<Toggle>().interactable = false;
                    }
                    else
                    {
                        _shop_listitem[list_count].GetComponent<Toggle>().interactable = true;
                    }
                    //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);

                    ++list_count;


                }
            }
        }

    }

}
