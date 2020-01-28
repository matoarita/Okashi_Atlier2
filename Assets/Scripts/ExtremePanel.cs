using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtremePanel : MonoBehaviour {

    public int extreme_itemID;
    public int extreme_itemtype;

    private int extreme_kaisu;

    private GameObject image_effect;

    private GameObject canvas;

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    private Texture2D texture2d;

    private Image item_Icon;
    private Text item_Name;
    private Text extreme_Param;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject compoundselect_onoff_obj;

    private GameObject text_area;
    private Text _text;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //windowテキストエリアの取得
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        item_Icon = this.transform.Find("Extreme_Image").gameObject.GetComponent<Image>(); //画像アイコン

        extreme_Param = this.transform.Find("ExtremeKaisu/Text/ExtremeKaisuParam").gameObject.GetComponent<Text>(); //エクストリーム残り回数
        extreme_Param.text = "-";

        image_effect = this.transform.Find("Extreme_Image_effect").gameObject;
        image_effect.SetActive(false);

        item_Icon.color = new Color(1, 1, 1, 0);

        extreme_itemID = 9999;
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SetExtremeItem( int item_id, int itemtype )
    {

        extreme_itemID = item_id;
        extreme_itemtype = itemtype;

        if(extreme_itemtype == 0 ) //デフォルトアイテムの場合
        {
            texture2d = database.items[extreme_itemID].itemIcon;
            extreme_kaisu = database.items[extreme_itemID].ExtremeKaisu;
        }
        else if (extreme_itemtype == 1) //オリジナルアイテムの場合
        {
            texture2d = pitemlist.player_originalitemlist[extreme_itemID].itemIcon;
            extreme_kaisu = pitemlist.player_originalitemlist[extreme_itemID].ExtremeKaisu;
        }


        item_Icon.color = new Color(1, 1, 1, 1);
        item_Icon.sprite = Sprite.Create(texture2d,
                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                   Vector2.zero);

        //エクストリーム残り回数の表示更新。
        extreme_Param.text = extreme_kaisu.ToString();

        //エフェクトの表示
        image_effect.SetActive(true);

    }

    public void OnClick_ExtremeButton()
    {
        //card_view.ResultCard_DrawView(extreme_itemtype, extreme_itemID);

        if (extreme_itemID != 9999)
        {
            //Compound_Mainのトッピング時と処理が同じ
            pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

            compoundselect_onoff_obj = canvas.transform.Find("CompoundSelect_ScrollView").gameObject;

            //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
            yes = pitemlistController_obj.transform.Find("Yes").gameObject;
            yes_text = yes.GetComponentInChildren<Text>();
            no = pitemlistController_obj.transform.Find("No").gameObject;
            no_text = no.GetComponentInChildren<Text>();

            _text.text = "エクストリーム調合をするよ！ 一個目の材料を選んでね。";

            compoundselect_onoff_obj.SetActive(false);

            compound_Main.compound_status = 4; //調合シーンに入っています、というフラグ
            compound_Main.compound_select = 2; //トッピング調合を選択
            pitemlistController_obj.SetActive(true); //プレイヤーアイテム画面を表示。
            pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
            yes.SetActive(false);
            no.SetActive(true);


            //以下、エクストリーム用に再度パラメータを設定
            pitemlistController.extremepanel_on = true;

            if (extreme_itemtype == 0) //デフォルトアイテムの場合
            {
                pitemlistController.final_base_kettei_item = database.items[extreme_itemID].itemID;
            }
            else if (extreme_itemtype == 1) //オリジナルアイテムの場合
            {
                pitemlistController.final_base_kettei_item = pitemlist.player_originalitemlist[extreme_itemID].itemID;
            }

            
            pitemlistController.base_kettei_item = extreme_itemID;
            pitemlistController._base_toggle_type = extreme_itemtype;

            pitemlistController.final_base_kettei_kosu = 1;

            pitemlistController.kettei1_bunki = 10; //トッピング材料から選び始める。
            pitemlistController.reset_and_DrawView_Topping();

            card_view.SelectCard_DrawView(pitemlistController._base_toggle_type, pitemlistController.base_kettei_item);
            card_view.OKCard_DrawView();

            itemselect_cancel.update_ListSelect_Flag = 10; //ベースアイテムを選択できないようにする。
            itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理
        }
        else //何もまだ作られていない場合は、エクストリームできない。
        {
            _text.text = "エクストリームパネルは空だよ。";
        }    
    }

    public void deleteExtreme_Item() //削除
    {

        item_Icon.color = new Color(1, 1, 1, 0);

        extreme_itemID = 9999;

        image_effect.SetActive(false);
    }
}
