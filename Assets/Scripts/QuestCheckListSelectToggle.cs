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

public class QuestCheckListSelectToggle : MonoBehaviour
{
    private GameObject canvas;

    Toggle m_Toggle;
    public Text m_Text; //デバッグ用。未使用。

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private SoundController sc;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private Exp_Controller exp_Controller;
    private MoneyStatus_Controller moneyStatus_Controller;

    private GameObject questCheckList_Controller_obj;
    private QuestCheckListController questCheckList_Controller;

    private GameObject questKakuninHyoujiPanel_obj;
    private QuestKakuninHyoujiPanel questKakuninHyoujiPanel;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private ContestStartListDataBase conteststartList_database;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject black_effect;

    public int toggle_ID; //こっちは、クエストデータベース上のIDを保持する。
    public int toggle_listcount; //こっちは、リスト配列番号
    public string toggle_name; //
    public string toggle_nameHyouji; 

    private int i, _id, _list;

    private int count;
    private bool selectToggle;
    private string _nameHyouji;


    void Start()
    {
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        moneyStatus_Controller = MoneyStatus_Controller.Instance.GetComponent<MoneyStatus_Controller>();

        //Fetch the Toggle GameObject
        //m_Toggle = GetComponent<Toggle>();

        //Initialise the Text to say the first state of the Toggle デバッグ用テキスト
        //m_Text = m_Toggle.GetComponentInChildren<Text>();
        //m_Text.text = "First Value : " + m_Toggle.isOn;

        //Add listener for when the state of the Toggle changes, to take action アドリスナー　トグルの値が変化したときに、｛｝内のメソッドを呼び出す
        /*m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        });*/

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        questCheckList_Controller_obj = canvas.transform.Find("QuestKakuninHyoujiPanel/PanelA/QuestCheckList_ScrollView").gameObject;
        questCheckList_Controller = questCheckList_Controller_obj.GetComponent<QuestCheckListController>();

        questKakuninHyoujiPanel_obj = canvas.transform.Find("QuestKakuninHyoujiPanel").gameObject;
        questKakuninHyoujiPanel = questKakuninHyoujiPanel_obj.GetComponent<QuestKakuninHyoujiPanel>();

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
        //black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

        i = 0;
        count = 0;

    }


    void Update()
    {

    }

    //Output the new state of the Toggle into Text
    /*void ToggleValueChanged(Toggle change)
    {
        //m_Text.text = "New Value : " + m_Toggle.isOn;
        if (m_Toggle.isOn == true)
        {
            //itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

            QuestCheckSelect_active();

        }
    }*/

    public void OnToggleCheck()
    {
        questKakuninHyoujiPanel_obj.transform.Find("PanelB").gameObject.SetActive(true);
        count = toggle_listcount;
        QuestCheckSelect_active();
    }

    /* ### コンテストリスト表示中のシーン ### */

    public void QuestCheckSelect_active()
    {

        //アイテムを選択したときの処理

        questCheckList_Controller._count = count; //カウントしたリスト番号を保持
        _id = questCheckList_Controller._quest_listitem[count].GetComponent<QuestCheckListSelectToggle>().toggle_ID; //クエストのIDを入れる。（リスト番号ではないので注意）
        _nameHyouji = questCheckList_Controller._quest_listitem[count].GetComponent<QuestCheckListSelectToggle>().toggle_nameHyouji;

        Debug.Log(count + "番が押されたよ");
        Debug.Log("クエスト:" + _nameHyouji + " が選択されました。");

        //Debug.Log("これでいいですか？");

        //パネルBの描画を更新する
        questKakuninHyoujiPanel.UpdateQuestDetailedPanel(count);


    }

}
