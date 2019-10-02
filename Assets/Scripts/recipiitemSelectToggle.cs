﻿//Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
//Set your own Text in the Inspector window

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

//***  アイテムの調合処理、プレイヤーのアイテム所持リストの処理はここでやっています。
//***  プレファブにとりつけているスクリプト、なので、privateの値は、インスタンスごとに変わってくるため、バグに注意。

public class recipiitemSelectToggle : MonoBehaviour
{
    Toggle m_Toggle;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;
    private Exp_Controller exp_Controller;

    private GameObject updown_counter_obj;
    private Updown_counter_recipi updown_counter;
    private Button[] updown_button = new Button[2];

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private PlayerItemList pitemlist;
    //private PlayerStatus player_status;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private ItemShopDataBase shop_database;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    public int recipi_toggleitemType; //選んだアイテムが、イベントアイテムか、コンポ調合DBのアイテムかを判別する。0=イベントアイテム, 1=コンポ調合用DBアイテム
    public int recipi_toggleCompoitem_ID; //リストの要素自体に、コンポアイテムIDを保持する。
    public int recipi_toggleEventitem_ID; //リストの要素にイベントアイテムIDを保持する。
    public int recipi_itemID; //そのときのアイテムDB上のアイテムID。
    public string recipi_itemNameHyouji; //名前表示用

    

    private int i, j;

    private int pitemlist_max;
    private int count;
    private bool selectToggle;

    private int kettei_item1; //このスクリプトは、プレファブのインスタンスに取り付けているので、各プレファブ共通で、変更できる値が必要。そのパラメータは、PlayerItemListControllerで管理する。

    private int compo_itemID;

    private string compo_itemname;


    void Start()
    {

        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();


        //Add listener for when the state of the Toggle changes, to take action アドリスナー　トグルの値が変化したときに、｛｝内のメソッドを呼び出す
        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        });

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        recipilistController_obj = GameObject.FindWithTag("RecipiList_ScrollView");
        recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

        updown_counter_obj = recipilistController_obj.transform.Find("updown_counter").gameObject;
        updown_counter = updown_counter_obj.GetComponent<Updown_counter_recipi>();

        yes = recipilistController_obj.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = recipilistController_obj.transform.Find("No").gameObject;
        yes_selectitem_kettei = yes.GetComponent<SelectItem_kettei>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        updown_counter_obj.SetActive(false);

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        i = 0;
        count = 0;

    }

    void Update()
    {
        if (recipilistController.final_recipiselect_flag == true)
        {
            text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
            _text = text_area.GetComponentInChildren<Text>();

            compound_Main_obj = GameObject.FindWithTag("Compound_Main");
            compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

            StartCoroutine("recipiFinal_select");
        }
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        //m_Text.text = "New Value : " + m_Toggle.isOn;
        if (m_Toggle.isOn == true)
        {
            //compound_Main.compound_status = 100;
            itemselect_cancel.kettei_on_waiting = true;
            recipi_active();
        }
    }



    /* ### レシピ調合の処理 ### */

    public void recipi_active()
    {
        count = 0;
        
        while (count < recipilistController._recipi_listitem.Count)
        {
            selectToggle = recipilistController._recipi_listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        recipilistController._count1 = count; //リスト中の選択された番号を格納。


        //イベントアイテムの場合
        if (recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_toggleitemType == 0) 
        {
            compound_Main.event_itemID = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_toggleEventitem_ID;

            itemselect_cancel.kettei_on_waiting = false;

            //イベント処理を開始する。
            compound_Main.eventRecipi_ON();
        }

        //コンポ調合アイテムの場合
        else if (recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_toggleitemType == 1) 
        {
            compo_itemname = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_itemNameHyouji;
            compo_itemID = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_toggleCompoitem_ID; //itemID_1という変数に、プレイヤーが選択したアイテムIDを格納する。
            recipilistController.result_recipicompID = compo_itemID;

            //調合DBの生成されるアイテム名から、アイテムDBを検索し、IDを検出して、リザルトに代入
            while ( i < databaseCompo.compoitems.Count )
            {
                if ( database.items[i].itemName == databaseCompo.compoitems[compo_itemID].cmpitemID_result )
                {
                    recipilistController.result_recipiitem = i;
                    break;
                }
                i++;
            }
            

            Debug.Log(count + "番が押されたよ");
            Debug.Log("レシピリスト番号:" + compo_itemID + " " + compo_itemname + "が選択されました。");

            //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
            for (i = 0; i < recipilistController._recipi_listitem.Count; i++)
            {
                recipilistController._recipi_listitem[i].GetComponent<Toggle>().interactable = false;
            }

            card_view.SelectCard_DrawView(0, recipilistController.result_recipiitem); //選択したアイテムをカードで表示

            yes.SetActive(true);
            no.SetActive(true);
            updown_counter_obj.SetActive(true);
            updown_counter.updown_keisan_Method();

            StartCoroutine("recipiitemselect_kakunin");
        }    

    }

    IEnumerator recipiitemselect_kakunin()
    {
        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。
        //選択中の、必要個数をチェックする処理は、updown_counter_recipiのアップデート内で処理。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }


        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された 

                recipilistController._recipi_listitem[count].GetComponent<Toggle>().interactable = false;

                recipilistController.final_recipiselect_flag = true; //最後、これでよいかどうかを聞くフラグをオン

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                Debug.Log("レシピ選択完了！");
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                for (i = 0; i < recipilistController._recipi_listitem.Count; i++)
                {
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().interactable = true;
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().isOn = false;
                }

                //Debug.Log("キャンセルをおした");
                itemselect_cancel.All_cancel();

                break;
        }

    }

    IEnumerator recipiFinal_select()
    {
        _text.text = "これで作成しますか？";

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //Debug.Log("ok");
                //解除
                for (i = 0; i < recipilistController._recipi_listitem.Count; i++)
                {
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().interactable = true;
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().isOn = false;
                }
                
                recipilistController.final_recipiselect_flag = false;

                exp_Controller.recipiresult_ok = true; //レシピ調合完了のフラグ。これがONになったら、アイテムリストを更新する。

                itemselect_cancel.kettei_on_waiting = false;

                yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);

                compound_Main.compound_status = 4;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                Debug.Log("選択完了！");
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                recipilistController.final_recipiselect_flag = false;

                for (i = 0; i < recipilistController._recipi_listitem.Count; i++)
                {
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().interactable = true;
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().isOn = false;
                }

                itemselect_cancel.All_cancel();

                break;
        }
    }
}
