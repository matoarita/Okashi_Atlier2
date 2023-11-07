//Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
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

    private Text _coin_cullency; //通貨　GameMgrで決めたものを自動で入力する

    private GameObject quest_Judge_CanvasPanel;

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

    private SoundController sc;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン

    private GameObject yes_no_panel;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject questListToggle_obj;
    private Toggle questListToggle;
    private GameObject nouhinToggle_obj;
    private Toggle nouhinToggle;
    private GameObject NouhinKetteiPanel_obj;

    private GameObject black_effect;

    public int toggle_ID; //こっちは、ショップデータベース上のIDを保持する。
    public int toggle_quest_ID; //リストの要素自体に、アイテムDB上のアイテムIDを保持する。
    public int toggle_quest_type; //リストの要素に、通常アイテムか、イベントアイテム判定用のタイプを保持する。
    public int toggle_itemID; //選択したクエストの納品用の、お菓子のアイテムID（アイテムDBの番号）

    private int i;

    private int pitemlist_max;
    private int count;
    private int _itemcount;
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

        quest_Judge_CanvasPanel = canvas.transform.Find("Quest_Judge_CanvasPanel").gameObject;
        NouhinKetteiPanel_obj = quest_Judge_CanvasPanel.transform.Find("NouhinKetteiPanel").gameObject;

        questjudge_obj = GameObject.FindWithTag("Quest_Judge");
        questjudge = questjudge_obj.GetComponent<Quest_Judge>();

        shopquestlistController_obj = quest_Judge_CanvasPanel.transform.Find("ShopQuestList_ScrollView").gameObject;
        shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();
        back_ShopFirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();
        
        questListToggle_obj = shopquestlistController_obj.transform.Find("CategoryView/Viewport/Content/Cate_QuestList").gameObject;
        questListToggle = questListToggle_obj.GetComponent<Toggle>();
        nouhinToggle_obj = shopquestlistController_obj.transform.Find("CategoryView/Viewport/Content/Cate_Nouhin").gameObject;
        nouhinToggle = nouhinToggle_obj.GetComponent<Toggle>();

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes_no_panel.SetActive(false);

        yes = yes_no_panel.transform.Find("Yes").gameObject;
        no = yes_no_panel.transform.Find("No").gameObject;

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

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

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //黒半透明パネルの取得
        black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

        text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        _coin_cullency = this.transform.Find("Background/Quest_money").GetComponent<Text>();
        _coin_cullency.text = GameMgr.MoneyCurrencyEn;

        i = 0;

        count = 0;

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


        _text.text = quest_database.questRandomset[count].Quest_desc + "\n" + "この依頼を受ける？";

        Debug.Log(count + "番が押されたよ");
        Debug.Log("クエスト:" + "が選択されました。");

        //Debug.Log("これでいいですか？");

        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < shopquestlistController._quest_listitem.Count; i++)
        {
            shopquestlistController._quest_listitem[i].GetComponent<Toggle>().interactable = false;
        }

        yes_no_panel.SetActive(true);
        yes.SetActive(true);
        no.SetActive(true);

        questListToggle.interactable = false;
        nouhinToggle.interactable = false;

        StartCoroutine("quest_select");

    }

    IEnumerator quest_select()
    {


        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

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

                //音を鳴らす。
                sc.PlaySe(21);

                Debug.Log("受注完了！");

                yes_no_panel.SetActive(false);

                questListToggle.interactable = true;
                nouhinToggle.interactable = true;

                back_ShopFirst_btn.interactable = true;        
                
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

                yes_no_panel.SetActive(false);

                questListToggle.interactable = true;
                nouhinToggle.interactable = true;

                back_ShopFirst_btn.interactable = true;

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
        shopquestlistController.quest_itemID = shopquestlistController._quest_listitem[count].GetComponent<shopQuestSelectToggle>().toggle_itemID; //選択アイテムのIDをいれる

        if (shopquestlistController.quest_itemID != 9999)
        {
            if (shopquestlistController.questType == 0) //お菓子タイプ
            {
                _text.text = "渡したいお菓子を選んでね。";
                //_text.text = "依頼のアイテムを納品する？";
            }
            else if (shopquestlistController.questType == 1) //材料タイプ
            {
                _itemcount = pitemlist.KosuCount(database.items[shopquestlistController.quest_itemID].itemName);
                _text.text = "依頼のアイテムを納品する？" + "\n" + "現在の所持数: " + GameMgr.ColorYellow + _itemcount + "</color>";
            }
        }
        else
        {
            _text.text = "依頼のアイテムを納品する？";
        }

        Debug.Log(count + "番が押されたよ");
        Debug.Log("受注クエスト:" + "が選択されました。");

        //Debug.Log("これでいいですか？");

        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < shopquestlistController._quest_listitem.Count; i++)
        {
            shopquestlistController._quest_listitem[i].GetComponent<Toggle>().interactable = false;
        }

        yes_no_panel.SetActive(true);
        yes.SetActive(true);
        no.SetActive(true);

        questListToggle.interactable = false;
        nouhinToggle.interactable = false;


        if (shopquestlistController.questType == 0) //お菓子タイプの判定
        {
            _text.text = "渡したいお菓子を選んでね。";

            //お菓子の納品なら、このタイミングでプレイヤーリストを開く。
            shopquestlistController.nouhin_select_on = 1;
            pitemlistController_obj.SetActive(true);
            NouhinKetteiPanel_obj.SetActive(true);

            yes_no_panel.SetActive(false);
            back_ShopFirst_btn.interactable = false;

            NouhinKetteiPanel_obj.transform.Find("NouhinButton").gameObject.SetActive(false);

            StartCoroutine("QuestTake_Pitemlist_wait");

            //itemselectToggleで入力処理を続けている。
        }
        else if (shopquestlistController.questType == 1)
        {
            //足りてるかどうかを事前チェック。材料系を納品する場合、のみ
            questjudge.Quest_result(shopquestlistController._count, false);
            if (questjudge.nouhinOK_flag)
            {

            }
            else //足りてないときは、そもそもyesが押せない
            {
                yes.SetActive(false);
                _text.text = "依頼のアイテムを納品する？" + "\n" + "現在の所持数: " + GameMgr.ColorYellow + _itemcount + "</color>" +
                    "\n" + "まだ数が足りてないようね..。";
            }

            StartCoroutine("questTake_select");
        }
            
    }


    IEnumerator questTake_select()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false;

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。

                //Debug.Log("ok");
                //解除

                yes_no_panel.SetActive(false);
                if (shopquestlistController.questType == 0) //お菓子タイプの判定
                {

                }
                else if (shopquestlistController.questType == 1) //材料タイプの判定
                {
                    shopquestlistController.nouhin_select_on = 1;
                    questListToggle.interactable = true;
                    nouhinToggle.interactable = true;

                    //足りてるかどうかをチェック、材料アイテムなら即納品。
                    questjudge.Quest_result(shopquestlistController._count, true);
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

                yes_no_panel.SetActive(false);

                shopquestlistController.nouhin_select_on = 0;
                questListToggle.interactable = true;
                nouhinToggle.interactable = true;

                back_ShopFirst_btn.interactable = true;

                break;
        }
    }

    //
    //納品するアイテムを選択中　「納品決定」ボタンをおすと、ここの処理再開。
    //
    IEnumerator QuestTake_Pitemlist_wait()
    {
        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、「納品決定」ボタンをおすと、処理再開。

        while (yes_selectitem_kettei.onclick2 != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick2 = false;

        switch (yes_selectitem_kettei.ketteiNouhin)
        {

            case true: //決定が押された。

                //カード削除
                card_view.DeleteCard_DrawView();

                //Debug.Log("ok");
                //解除

                //お菓子の判定処理
                //_listcountのidごとに、それぞれ計算する。
                questjudge.Okashi_Judge(shopquestlistController._count);

                shopquestlistController.nouhin_select_on = 0;
                shopquestlistController.final_select_flag = false;

                pitemlistController._listkosu.Clear();
                pitemlistController._listcount.Clear();

                pitemlistController_obj.SetActive(false);
                yes_no_panel.SetActive(false);
                NouhinKetteiPanel_obj.SetActive(false);
                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                if (shopquestlistController.final_select_flag)
                {
                    //最後の確認の時は、一つ前に戻る。
                    shopquestlistController.final_select_flag = false;

                    pitemlistController._listcount.RemoveAt(pitemlistController._listcount.Count - 1); //一番最後に挿入されたやつを、そのまま削除
                    pitemlistController._listkosu.RemoveAt(pitemlistController._listkosu.Count - 1); //個数は決定後に追加されるので、ここでは削除しない

                    for (i = 0; i < pitemlistController._listitem.Count; i++)
                    {
                        //まずは、一度全て表示を初期化
                        pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
                        pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;

                    }

                    //選択済みのやつだけONにしておく。
                    for (i = 0; i < pitemlistController._listcount.Count; i++)
                    {
                        //Debug.Log("pitemlistController._listcount[i]: " + i + ": " + pitemlistController._listcount[i]);
                        pitemlistController._listitem[pitemlistController._listcount[i]].GetComponent<Toggle>().interactable = false;
                        //pitemlistController._listitem[pitemlistController._listcount[i]].GetComponent<Toggle>().isOn = true;
                    }

                    card_view.DeleteCard_DrawView();

                    yes.SetActive(false);
                    black_effect.SetActive(false);
                    NouhinKetteiPanel_obj.SetActive(true);
                    NouhinKetteiPanel_obj.transform.Find("NouhinButton").gameObject.SetActive(false);

                    //yes_selectitem_kettei.onclick = false;
                    itemselect_cancel.kettei_on_waiting = false;

                    if (pitemlistController._listcount.Count <= 0) //すべて選択してないときは、noはOFF
                    {
                        no.SetActive(false);
                    }

                    StartCoroutine("QuestTake_Pitemlist_wait");
                }
                else
                {
                    //納品リスト画面に戻る。

                    //キャンセル時、リストのインタラクティブ解除。
                    for (i = 0; i < shopquestlistController._quest_listitem.Count; i++)
                    {
                        shopquestlistController._quest_listitem[i].GetComponent<Toggle>().interactable = true;
                        shopquestlistController._quest_listitem[i].GetComponent<Toggle>().isOn = false;
                    }

                    _text.text = "";

                    yes_no_panel.SetActive(false);

                    back_ShopFirst_btn.interactable = true;

                    pitemlistController_obj.SetActive(false);
                    NouhinKetteiPanel_obj.SetActive(false);

                    questListToggle.interactable = true;
                    nouhinToggle.interactable = true;

                    card_view.DeleteCard_DrawView();
                    updown_counter_obj.SetActive(false);

                    shopquestlistController.nouhin_select_on = 0;
                    yes_selectitem_kettei.onclick = false;

                    pitemlistController._listcount.Clear();
                    pitemlistController._listkosu.Clear();
                }
                break;
        }
    }
}
