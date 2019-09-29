using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Updown_counter_recipi : MonoBehaviour
{

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;

    private PlayerItemList pitemlist;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private Text _count_text;
    public int updown_kosu;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private Button[] updown_button = new Button[2];

    private int _zaiko_max;

    private int i, count;

    private int itemID_1;
    private string itemname_1;

    private string cmpitem_1;
    private string cmpitem_2;
    private string cmpitem_3;

    private string _a;
    private string _b;
    private string _c;

    private int cmpitem_kosu1;
    private int cmpitem_kosu2;
    private int cmpitem_kosu3;

    private int cmpitem_kosu1_select;
    private int cmpitem_kosu2_select;
    private int cmpitem_kosu3_select;

    private int itemdb_id1;
    private int itemdb_id2;
    private int itemdb_id3;

    // Use this for initialization
    void Start()
    {
        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        yes = recipilistController_obj.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = recipilistController_obj.transform.Find("No").gameObject;
        yes_selectitem_kettei = yes.GetComponent<SelectItem_kettei>();

        updown_button = this.GetComponentsInChildren<Button>();
        updown_button[0].interactable = true;
        updown_button[1].interactable = true;

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        recipilistController_obj = GameObject.FindWithTag("RecipiList_ScrollView");
        recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

        updown_kosu = 1;

        
        _count_text = transform.GetChild(0).gameObject.GetComponent<Text>();
        _count_text.text = "1";


    }

    public void OnClick_up()
    {

        //_zaiko_max = pitemlist.playeritemlist[pitemlistController.final_kettei_item1]; //一個目の決定アイテムの所持数

        _zaiko_max = 9;

        ++updown_kosu;

        if (updown_kosu > _zaiko_max)
        {
            updown_kosu = _zaiko_max;
        }

        updown_keisan_Method();

        _count_text.text = updown_kosu.ToString();
    }

    public void OnClick_down()
    {
        --updown_kosu;
        if (updown_kosu <= 1)
        {
            updown_kosu = 1;
        }

        updown_keisan_Method();

        _count_text.text = updown_kosu.ToString();
    }

    public void updown_keisan_Method()
    {
        count = recipilistController._count1;
        itemID_1 = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_toggleCompoitem_ID; //itemID_1という変数に、プレイヤーが選択した調合DBの配列番号を格納する。
        itemname_1 = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_itemNameHyouji;

        recipilistController.final_select_kosu = updown_kosu;

        //必要アイテム・個数の代入
        cmpitem_kosu1 = databaseCompo.compoitems[itemID_1].cmpitem_kosu1;
        cmpitem_kosu2 = databaseCompo.compoitems[itemID_1].cmpitem_kosu2;
        cmpitem_kosu3 = databaseCompo.compoitems[itemID_1].cmpitem_kosu3;

        i = 0;

        while (i < database.items.Count)
        {
            if (database.items[i].itemName == databaseCompo.compoitems[itemID_1].cmpitemID_1)
            {
                cmpitem_1 = database.items[i].itemNameHyouji; //調合DB一個目のnameを日本語表示に。
                itemdb_id1 = i; //その時のアイテムDB番号も、保存
                break;
            }
            ++i;
        }

        i = 0;

        while (i < database.items.Count)
        {
            if (database.items[i].itemName == databaseCompo.compoitems[itemID_1].cmpitemID_2)
            {
                cmpitem_2 = database.items[i].itemNameHyouji; //調合DB一個目のnameを日本語表示に。
                itemdb_id2 = i; //その時のアイテムDB番号も、保存
                break;
            }
            ++i;
        }

        i = 0;

        itemdb_id3 = 9999; //空の可能性もあるので、もし空なら9999にしておく。あれば、iで更新される。

        while (i < database.items.Count)
        {
            if (database.items[i].itemName == databaseCompo.compoitems[itemID_1].cmpitemID_3)
            {
                cmpitem_3 = database.items[i].itemNameHyouji; //調合DB一個目のnameを日本語表示に。
                itemdb_id3 = i; //その時のアイテムDB番号も、保存
                break;
            }
            ++i;
        }


        //最終的なアイテムを決定
        recipilistController.kettei_recipiitem1 = database.items[itemdb_id1].itemID;
        recipilistController.kettei_recipiitem2 = database.items[itemdb_id2].itemID;

        recipilistController.final_kettei_recipikosu1 = cmpitem_kosu1_select;
        recipilistController.final_kettei_recipikosu2 = cmpitem_kosu2_select;

        if (databaseCompo.compoitems[itemID_1].cmpitemID_3 == "empty") //2個のアイテムが必要な場合。３個めは空＝9999
        {
            recipilistController.kettei_recipiitem3 = 9999;
            recipilistController.final_kettei_recipikosu3 = 0;
        }
        else //3個アイテムが必要な場合
        {
            recipilistController.kettei_recipiitem3 = database.items[itemdb_id3].itemID;
            recipilistController.final_kettei_recipikosu3 = cmpitem_kosu3_select;
        }




        cmpitem_kosu1_select = cmpitem_kosu1 * updown_kosu; //必要個数×選択している作成数
        cmpitem_kosu2_select = cmpitem_kosu2 * updown_kosu; //必要個数×選択している作成数
        cmpitem_kosu3_select = cmpitem_kosu3 * updown_kosu; //必要個数×選択している作成数

        _a = cmpitem_1 + ": " + "<color=#0000ff>" + cmpitem_kosu1_select + "</color>" + "／" + pitemlist.playeritemlist[itemdb_id1];
        _b = cmpitem_2 + ": " + "<color=#0000ff>" + cmpitem_kosu2_select + "</color>" + "／" + pitemlist.playeritemlist[itemdb_id2];
        _c = cmpitem_3 + ": " + "<color=#0000ff>" + cmpitem_kosu3_select + "</color>" + "／" + pitemlist.playeritemlist[itemdb_id3];

        if (cmpitem_kosu1_select > pitemlist.playeritemlist[itemdb_id1])
        {
            _a = "<color=#ff0000>" + cmpitem_1 + ": " + cmpitem_kosu1_select + "／" + pitemlist.playeritemlist[itemdb_id1] + "</color>";
        }
        if (cmpitem_kosu2_select > pitemlist.playeritemlist[itemdb_id2])
        {
            _b = "<color=#ff0000>" + cmpitem_2 + ": " + cmpitem_kosu2_select + "／" + pitemlist.playeritemlist[itemdb_id2] + "</color>";
        }
        if (cmpitem_kosu3_select > pitemlist.playeritemlist[itemdb_id3])
        {
            _c = "<color=#ff0000>" + cmpitem_3 + ": " + cmpitem_kosu3_select + "／" + pitemlist.playeritemlist[itemdb_id3] + "</color>";
        }

        

        //材料個数が足りてるかの判定

        if (databaseCompo.compoitems[itemID_1].cmpitemID_3 == "empty") //2個のアイテムが必要な場合
        {

            if (cmpitem_kosu1_select > pitemlist.playeritemlist[itemdb_id1] || cmpitem_kosu2_select > pitemlist.playeritemlist[itemdb_id2])
            {
                _text.text = itemname_1 + "材料が足りない..。" + "\n" + _a + "\n" + _b;
                updown_button[1].interactable = false;
                yes.SetActive(false);
            }
            else
            {
                _text.text = itemname_1 + "が選択されました。" + "\n" + _a + "\n" + _b;
                yes.SetActive(true);
                updown_button[1].interactable = true;

            }

        }
        else //3個アイテムが必要な場合
        {
            if (cmpitem_kosu1_select > pitemlist.playeritemlist[itemdb_id1] || cmpitem_kosu2_select > pitemlist.playeritemlist[itemdb_id2] || cmpitem_kosu3_select > pitemlist.playeritemlist[itemdb_id3])
            {
                _text.text = "材料が足りない..。" + "\n" + _a + "\n" + _b + "\n" + _c;
                updown_button[1].interactable = false;
                yes.SetActive(false);
            }
            else
            {
                _text.text = itemname_1 + "が選択されました。" + "\n" + _a + "\n" + _b + "\n" + _c;
                yes.SetActive(true);
                updown_button[1].interactable = true;

            }
        }
    }
}
