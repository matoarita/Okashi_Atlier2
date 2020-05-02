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

public class shopQuestSelectToggle : MonoBehaviour
{
    private GameObject canvas;

    Toggle m_Toggle;
    public Text m_Text; //デバッグ用。未使用。

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject shopMain_obj;
    private Shop_Main shopMain;

    private GameObject questjudge_obj;
    private Quest_Judge questjudge;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private Exp_Controller exp_Controller;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject shopquestlistController_obj;
    private ShopQuestListController shopquestlistController;
    private GameObject back_ShopFirst_obj;
    private Button back_ShopFirst_btn;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private Button[] updown_button = new Button[2];

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private QuestSetDataBase quest_database;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject questListButton_obj;
    private Button questListButton;
    private GameObject nouhinButton_obj;
    private Button nouhinButton;
    private GameObject NouhinKetteiPanel_obj;

    public int toggle_ID; //こっちは、ショップデータベース上のIDを保持する。
    public int toggle_quest_ID; //リストの要素自体に、アイテムDB上のアイテムIDを保持する。
    public int toggle_quest_type; //リストの要素に、通常アイテムか、イベントアイテム判定用のタイプを保持する。

    private int i;

    private int pitemlist_max;
    private int count;
    private bool selectToggle;

    private int kettei_item1; //このスクリプトは、プレファブのインスタンスに取り付けているので、各プレファブ共通で、変更できる値が必要。そのパラメータは、PlayerItemListControllerで管理する。

    private int count_1;


    void Start()
    {
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();

        //Initialise the Text to say the first state of the Toggle デバッグ用テキスト
        //m_Text = m_Toggle.GetComponentInChildren<Text>();
        //m_Text.text = "First Value : " + m_Toggle.isOn;

        //Add listener for when the state of the Toggle changes, to take action アドリスナー　トグルの値が変化したときに、｛｝内のメソッドを呼び出す
        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        });

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
        updown_counter = updown_counter_obj.GetComponent<Updown_counter>();
       
        NouhinKetteiPanel_obj = canvas.transform.Find("NouhinKetteiPanel").gameObject;        

        shopMain_obj = GameObject.FindWithTag("Shop_Main");
        shopMain = shopMain_obj.GetComponent<Shop_Main>();

        questjudge_obj = GameObject.FindWithTag("Quest_Judge");
        questjudge = questjudge_obj.GetComponent<Quest_Judge>();

        shopquestlistController_obj = canvas.transform.Find("ShopQuestList_ScrollView").gameObject;
        shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();
        back_ShopFirst_obj = shopquestlistController_obj.transform.Find("Back_ShopFirst").gameObject;
        back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();

        
        questListButton_obj = shopquestlistController_obj.transform.Find("QuestListButton").gameObject;
        questListButton = questListButton_obj.GetComponent<Button>();
        nouhinButton_obj = shopquestlistController_obj.transform.Find("NouhinButton").gameObject;
        nouhinButton = nouhinButton_obj.GetComponent<Button>();

        yes = shopquestlistController_obj.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = shopquestlistController_obj.transform.Find("No").gameObject;
        no_text = no.GetComponentInChildren<Text>();

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();



        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();


        text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        i = 0;

        count = 0;

        yes.SetActive(false);
        no.SetActive(false);
    }


    void Update()
    {

    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        //m_Text.text = "New Value : " + m_Toggle.isOn;
        if (m_Toggle.isOn == true)
        {
            //クエストリストを開いてる場合の処理
            if (shopquestlistController.qlist_status == 0)
            {
                back_ShopFirst_btn.interactable = false;
                questselect_active();
            }
            //受注リストを開いている場合の処理
            else if (shopquestlistController.qlist_status == 1) 
            {
                back_ShopFirst_btn.interactable = false;
                questTake_active();
            }
        }
    }


    /* ### クエスト表示中のシーン ### */

    public void questselect_active()
    {

        //アイテムを選択したときの処理（トグルの処理）

        count = 0;

        while (count < shopquestlistController._quest_listitem.Count)
        {
            selectToggle = shopquestlistController._quest_listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        shopquestlistController._count = count; //カウントしたリスト番号を保持
        shopquestlistController._ID = shopquestlistController._quest_listitem[count].GetComponent<shopQuestSelectToggle>().toggle_ID; //IDを入れる。
        shopquestlistController.questID = shopquestlistController._quest_listitem[count].GetComponent<shopQuestSelectToggle>().toggle_quest_ID; //クエスト固有IDを入れる


        _text.text = quest_database.questset[shopquestlistController.questID].Quest_desc + "この依頼を受ける？";

        Debug.Log(count + "番が押されたよ");
        Debug.Log("クエスト:" + "が選択されました。");

        //Debug.Log("これでいいですか？");

        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < shopquestlistController._quest_listitem.Count; i++)
        {
            shopquestlistController._quest_listitem[i].GetComponent<Toggle>().interactable = false;
        }

        yes.SetActive(true);
        no.SetActive(true);

        questListButton.interactable = false;
        nouhinButton.interactable = false;

        StartCoroutine("quest_select");

    }

    IEnumerator quest_select()
    {


        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。これでいいですか？の確認。

                //Debug.Log("ok");
                //解除

                //受注したクエストを、リストに登録。RandomSetのIDを入れる。
                quest_database.QuestTakeSetInit(shopquestlistController._count);

                //その後、クエストリストから選んだやつを削除
                quest_database.questRandomset.RemoveAt(shopquestlistController._count);

                //画面の更新
                shopquestlistController.reset_and_DrawView();

                _text.text = "受注しました！" + "頑張ってね～！";
                
                //ジャキーンみたいな音を鳴らす。

                Debug.Log("受注完了！");

                yes.SetActive(false);
                no.SetActive(false);

                questListButton.interactable = true;
                nouhinButton.interactable = true;

                back_ShopFirst_btn.interactable = true;
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
        
                
                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                _text.text = "今はこんな依頼があるわよ";

                //キャンセル時、リストのインタラクティブ解除。
                for (i = 0; i < shopquestlistController._quest_listitem.Count; i++)
                {
                    shopquestlistController._quest_listitem[i].GetComponent<Toggle>().interactable = true;
                    shopquestlistController._quest_listitem[i].GetComponent<Toggle>().isOn = false;
                }


                yes.SetActive(false);
                no.SetActive(false);

                questListButton.interactable = true;
                nouhinButton.interactable = true;

                back_ShopFirst_btn.interactable = true;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                break;
        }
    }    

    /* ### 受注リスト表示中のシーン ### */

    public void questTake_active()
    {

        //アイテムを選択したときの処理（トグルの処理）

        count = 0;

        while (count < shopquestlistController._quest_listitem.Count)
        {
            selectToggle = shopquestlistController._quest_listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        shopquestlistController._count = count; //カウントしたリスト番号を保持
        shopquestlistController._ID = shopquestlistController._quest_listitem[count].GetComponent<shopQuestSelectToggle>().toggle_ID; //IDを入れる。
        shopquestlistController.questID = shopquestlistController._quest_listitem[count].GetComponent<shopQuestSelectToggle>().toggle_quest_ID; //クエスト固有IDを入れる
        shopquestlistController.questType = shopquestlistController._quest_listitem[count].GetComponent<shopQuestSelectToggle>().toggle_quest_type; //クエストのタイプをいれる


        _text.text = "依頼のアイテムを納品する？";

        Debug.Log(count + "番が押されたよ");
        Debug.Log("受注クエスト:" + "が選択されました。");

        //Debug.Log("これでいいですか？");

        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < shopquestlistController._quest_listitem.Count; i++)
        {
            shopquestlistController._quest_listitem[i].GetComponent<Toggle>().interactable = false;
        }

        yes.SetActive(true);
        no.SetActive(true);

        questListButton.interactable = false;
        nouhinButton.interactable = false;

        StartCoroutine("questTake_select");

    }


    IEnumerator questTake_select()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        //Debug.Log("shopquestlistController.questType: " + shopquestlistController.questType);

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。

                //Debug.Log("ok");
                //解除

                if(shopquestlistController.questType == 0) //お菓子タイプの判定
                {
                    _text.text = "渡したいお菓子を選んでね。";

                    //お菓子の納品なら、このタイミングでプレイヤーリストを開く。
                    pitemlistController_obj.SetActive(true);
                    NouhinKetteiPanel_obj.SetActive(true);

                    yes.SetActive(false);
                    no.SetActive(false);
                    back_ShopFirst_btn.interactable = false;

                    yes_selectitem_kettei.onclick = false;

                    NouhinKetteiPanel_obj.transform.Find("NouhinButton").gameObject.SetActive(false);

                    StartCoroutine("QuestTake_Pitemlist_wait");
                }
                else if (shopquestlistController.questType == 1) //材料タイプの判定
                {
                    questListButton.interactable = true;
                    nouhinButton.interactable = true;

                    //足りてる場合、材料アイテムなら即納品。お菓子ならお菓子の判定。ちなみにチェック中は、「.. 」のアニメも入れたい。
                    questjudge.Quest_result(shopquestlistController._count);


                }               
               
                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                _text.text = "";

                //キャンセル時、リストのインタラクティブ解除。
                for (i = 0; i < shopquestlistController._quest_listitem.Count; i++)
                {
                    shopquestlistController._quest_listitem[i].GetComponent<Toggle>().interactable = true;
                    shopquestlistController._quest_listitem[i].GetComponent<Toggle>().isOn = false;
                }


                yes.SetActive(false);
                no.SetActive(false);

                questListButton.interactable = true;
                nouhinButton.interactable = true;

                back_ShopFirst_btn.interactable = true;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                break;
        }
    }

    //納品するアイテムを選択中
    IEnumerator QuestTake_Pitemlist_wait()
    {
        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、「納品決定」ボタンをおすと、処理再開。

        while (yes_selectitem_kettei.onclick2 != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.ketteiNouhin)
        {

            case true: //決定が押された。

                //Debug.Log("ok");
                //解除

                //お菓子の判定処理
                questjudge.Okashi_Judge(shopquestlistController._count);

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                //納品リスト画面に戻る。

                //キャンセル時、リストのインタラクティブ解除。
                for (i = 0; i < shopquestlistController._quest_listitem.Count; i++)
                {
                    shopquestlistController._quest_listitem[i].GetComponent<Toggle>().interactable = true;
                    shopquestlistController._quest_listitem[i].GetComponent<Toggle>().isOn = false;
                }

                _text.text = "";

                yes.SetActive(false);
                no.SetActive(false);
                back_ShopFirst_btn.interactable = true;

                pitemlistController_obj.SetActive(false);
                NouhinKetteiPanel_obj.SetActive(false);

                questListButton.interactable = true;
                nouhinButton.interactable = true;

                card_view.DeleteCard_DrawView();
                updown_counter_obj.SetActive(false);

                yes_selectitem_kettei.onclick = false;
                yes_selectitem_kettei.onclick2 = false; //オンクリック2のフラグはオフにしておく。

                pitemlistController._listcount.Clear();
                pitemlistController._listkosu.Clear();
                break;
        }
    }
}