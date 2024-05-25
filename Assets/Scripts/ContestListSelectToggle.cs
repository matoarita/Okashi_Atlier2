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

public class ContestListSelectToggle : MonoBehaviour
{
    private GameObject canvas;

    Toggle m_Toggle;
    public Text m_Text; //デバッグ用。未使用。

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private Text _coin_cullency; //通貨　GameMgrで決めたものを自動で入力する

    private SoundController sc;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private Exp_Controller exp_Controller;
    private MoneyStatus_Controller moneyStatus_Controller;

    private GameObject contest_listController_obj;
    private ContestListController contest_listController;
    private GameObject back_ShopFirst_obj;
    private Button back_ShopFirst_btn;

    private GameObject contest_detailedPanel;
    private GameObject contestList_ScrollView_obj;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private Button[] updown_button = new Button[2];

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private ContestStartListDataBase conteststartList_database;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン

    private GameObject yes_no_panel;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト


    private GameObject black_effect;

    public int toggle_ID; //こっちは、ショップデータベース上のIDを保持する。
    public string toggle_name; //リストの要素自体に、アイテムDB上のアイテムIDを保持する。
    public string toggle_nameHyouji; //リストの要素に、通常アイテムか、イベントアイテム判定用のタイプを保持する。
    public int toggle_RankType; //コンテストのランキングタイプ

    private int i, _id, _list;

    private int pitemlist_max;
    private int count;
    private int _itemcount;
    private bool selectToggle;
    private string _nameHyouji;

    private int kettei_item1; //このスクリプトは、プレファブのインスタンスに取り付けているので、各プレファブ共通で、変更できる値が必要。そのパラメータは、PlayerItemListControllerで管理する。

    private int count_1;

    private string _cost_text;


    void Start()
    {
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        moneyStatus_Controller = MoneyStatus_Controller.Instance.GetComponent<MoneyStatus_Controller>();

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

        contest_listController_obj = canvas.transform.Find("ContestListPanel/ContestList_ScrollView").gameObject;
        contest_listController = contest_listController_obj.GetComponent<ContestListController>();
        back_ShopFirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();

        contest_detailedPanel = canvas.transform.Find("ContestListPanel/Contest_DetailedPanel").gameObject;
        contestList_ScrollView_obj = canvas.transform.Find("ContestListPanel/ContestList_ScrollView").gameObject;

        yes_no_panel = canvas.transform.Find("Yes_no_Panel_ContestSelect").gameObject;
        yes_no_panel.SetActive(false);

        yes = yes_no_panel.transform.Find("Yes_ContestKettei").gameObject;
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

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

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
            itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

            back_ShopFirst_btn.interactable = false;
            contestSelect_active();

        }
    }


    /* ### コンテストリスト表示中のシーン ### */

    public void contestSelect_active()
    {

        //アイテムを選択したときの処理（トグルの処理）

        count = 0;

        while (count < contest_listController._contest_listitem.Count)
        {
            selectToggle = contest_listController._contest_listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        contest_listController._count = count; //カウントしたリスト番号を保持
        _id = contest_listController._contest_listitem[count].GetComponent<ContestListSelectToggle>().toggle_ID; //IDを入れる。
        toggle_RankType = contest_listController._contest_listitem[count].GetComponent<ContestListSelectToggle>().toggle_RankType;
        _nameHyouji = contest_listController._contest_listitem[count].GetComponent<ContestListSelectToggle>().toggle_nameHyouji;

        contest_listController._ID = _id;
        _list = conteststartList_database.SearchContestID(_id);

        GameMgr.Contest_Cate_Ranking = conteststartList_database.conteststart_lists[_list].Contest_RankingType;
        GameMgr.ContestSelectNum = conteststartList_database.conteststart_lists[_list].Contest_placeNumID;

        Debug.Log(count + "番が押されたよ");
        Debug.Log("コンテスト:" + _nameHyouji + " が選択されました。");

        //Debug.Log("これでいいですか？");

        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < contest_listController._contest_listitem.Count; i++)
        {
            contest_listController._contest_listitem[i].GetComponent<Toggle>().interactable = false;
        }

        yes_no_panel.SetActive(true);
        yes.transform.Find("Text").GetComponent<Text>().text = "決定";
        yes.SetActive(true);
        no.SetActive(true);

        _text.text = _nameHyouji + "ですね？";

        //さらにコンテスト詳細のパネルを表示する。
        contest_detailedPanel.SetActive(true);
        contest_detailedPanel.GetComponent<Contest_DetailedPanel>().OnContestSettingData(_id);

        back_ShopFirst_btn.interactable = false;

        StartCoroutine("Contestlist_wait");

    }

    //
    //コンテスト　詳細画面を開き、選択中
    //
    IEnumerator Contestlist_wait()
    {
        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、「決定」ボタンをおすと、処理再開。

        while (yes_selectitem_kettei.onclick != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false;

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。

                _list = conteststartList_database.SearchContestID(contest_listController._ID);

                //Debug.Log("ok");
                //解除

                sc.PlaySe(0);
                //sc.PlaySe(25);

                black_effect.SetActive(true);

                yes.transform.Find("Text").GetComponent<Text>().text = "出場する";

                if (conteststartList_database.conteststart_lists[_list].Contest_Cost == 0)
                {
                    _cost_text = "無料";
                }
                else
                {
                    _cost_text = conteststartList_database.conteststart_lists[_list].Contest_Cost.ToString() + GameMgr.MoneyCurrency;
                }

                _text.text = "出場費用: " + GameMgr.ColorYellow + _cost_text + "</color>" + "\n"
                    + "コンテストはすぐに開始されます。" + "\n"
                    + "本当に出場しますか？";

                //ほんとに出場するかを最終確認
                StartCoroutine("Contestlist_FinalCheck");

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                _text.text = "今開催しているコンテストです。";
                OffDetailedWindow();

                break;
        }
    }

    //
    //コンテスト　詳細画面を開き、選択中
    //
    IEnumerator Contestlist_FinalCheck()
    {
        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、「決定」ボタンをおすと、処理再開。

        while (yes_selectitem_kettei.onclick != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false;
        black_effect.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。

                _list = conteststartList_database.SearchContestID(contest_listController._ID);

                //ここで登録料　お金チェック
                if (PlayerStatus.player_money < conteststartList_database.conteststart_lists[_list].Contest_Cost)
                {
                    _text.text = "あら！" + "\n" + "どうやら登録料が足りてないようですね。";

                    OffDetailedWindow();

                    sc.PlaySe(6);

                }
                else
                {
                    //Debug.Log("ok");
                    //解除

                    //sc.PlaySe(0);
                    sc.PlaySe(25);

                    yes_no_panel.SetActive(false);
                    itemselect_cancel.kettei_on_waiting = false;
                    //back_ShopFirst_btn.interactable = true;

                    //お金支払い
                    moneyStatus_Controller.UseMoney(conteststartList_database.conteststart_lists[_list].Contest_Cost);



                    //ほかに受け付けてるコンテストがあった場合、全てキャンセルし、新しく一個が登録
                    for (i = 0; i < conteststartList_database.conteststart_lists.Count; i++)
                    {
                        conteststartList_database.conteststart_lists[i].Contest_Accepted = 0;
                    }
                    GameMgr.contest_accepted_list.Clear();
                    //

                    //新しく受け付けたコンテストは、リストに追加。締め切り日付も保存する。
                    //ただ、現在すぐ出場するので、フラグも終了後にすぐ消えるので、意味はないかも。
                    conteststartList_database.conteststart_lists[_list].Contest_Accepted = 1; //受付完了フラグ
                    GameMgr.contest_accepted_list.Add(new ContestSaveList(conteststartList_database.conteststart_lists[_list].ContestName,
                        GameMgr.Contest_OrganizeMonth, GameMgr.Contest_OrganizeDay, 0, conteststartList_database.conteststart_lists[_list].Contest_Accepted));
                    //

                    GameMgr.Contest_listnum = _list;
                    GameMgr.Contest_Cate_Ranking = conteststartList_database.conteststart_lists[_list].Contest_RankingType;
                    GameMgr.ContestSelectNum = conteststartList_database.conteststart_lists[_list].Contest_placeNumID;

                    //出場回数+1
                    conteststartList_database.conteststart_lists[_list].ContestFightsCount++;

                    //contest_listController.OnContestList_Draw(); //再描画して受付済のコンテストは触れなくなる
                    contest_detailedPanel.SetActive(false);
                    contestList_ScrollView_obj.SetActive(false);

                    //受付した時点で、宴のイベントが開始し、すぐに次の日の朝10時になりコンテスト開始
                    //_text.text = "ありがとうございます！"; //"コンテスト開催日の、朝10時までにきてくださいね！"
                    GameMgr.Contest_ReadyToStart2 = true;

                    //FadeManager.Instance.LoadScene("Or_Contest_A1", GameMgr.SceneFadeTime); //デバッグ用
                }


                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                _text.text = "今開催しているコンテストです。";
                OffDetailedWindow();


                break;
        }
    }


    void OffDetailedWindow()
    {
        //キャンセル時、リストのインタラクティブ解除。
        for (i = 0; i < contest_listController._contest_listitem.Count; i++)
        {
            contest_listController._contest_listitem[i].GetComponent<Toggle>().interactable = true;
            contest_listController._contest_listitem[i].GetComponent<Toggle>().isOn = false;
        }
        contest_listController.OnContestList_Draw(); //再描画して受付済のコンテストは触れなくなる


        yes_no_panel.SetActive(false);
        back_ShopFirst_btn.interactable = true;
        itemselect_cancel.kettei_on_waiting = false;

        contest_detailedPanel.SetActive(false);
    }

    //すでに受けてるコンテストをキャンセルする
    public void OnCancel_Contest()
    {
        //Debug.Log("コンテストキャンセル　おした　ContestID: " + toggle_ID);

        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < contest_listController._contest_listitem.Count; i++)
        {
            contest_listController._contest_listitem[i].GetComponent<Toggle>().interactable = false;
        }

        _id = toggle_ID;

        yes_no_panel.SetActive(true);
        yes.SetActive(true);
        no.SetActive(true);

        _list = conteststartList_database.SearchContestID(_id);
        _nameHyouji = conteststartList_database.conteststart_lists[_list].ContestNameHyouji;
        _text.text = _nameHyouji + "の出場をキャンセルしますか？" + "\n" + "※出場費用は、半分返ってきます。";

        //さらにコンテスト詳細のパネルを表示する。
        contest_detailedPanel.SetActive(true);
        contest_detailedPanel.GetComponent<Contest_DetailedPanel>().OnContestSettingData(_id);

        back_ShopFirst_btn.interactable = false;

        StartCoroutine("ContestCancel_wait");
    }

    //
    //コンテスト　キャンセルするかしないか
    //
    IEnumerator ContestCancel_wait()
    {

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

                _text.text = "キャンセルしました！";
                yes_no_panel.SetActive(false);
                back_ShopFirst_btn.interactable = true;

                _list = conteststartList_database.SearchContestID(_id);
                conteststartList_database.conteststart_lists[_list].Contest_Accepted = 0; //受付キャンセルのフラグ

                //新しく受け付けたコンテストは、リストに追加していく。締め切り日付も保存する。
                for (i = 0; i < GameMgr.contest_accepted_list.Count; i++)
                {
                    if (GameMgr.contest_accepted_list[i].contestName == conteststartList_database.conteststart_lists[_list].ContestName)
                    {
                        GameMgr.contest_accepted_list.RemoveAt(i);
                    }
                }
                contest_listController.OnContestList_Draw(); //再描画して受付済のコンテストは触れなくなる

                contest_detailedPanel.SetActive(false);

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                //キャンセル時、リストのインタラクティブ解除。
                for (i = 0; i < contest_listController._contest_listitem.Count; i++)
                {
                    contest_listController._contest_listitem[i].GetComponent<Toggle>().interactable = true;
                    contest_listController._contest_listitem[i].GetComponent<Toggle>().isOn = false;
                }
                contest_listController.OnContestList_Draw(); //再描画して受付済のコンテストは触れなくなる

                _text.text = "今開催しているコンテストです。";
                yes_no_panel.SetActive(false);
                back_ShopFirst_btn.interactable = true;

                contest_detailedPanel.SetActive(false);

                break;
        }
    }
}
