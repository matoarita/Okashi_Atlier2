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

public class recipimemoSelectToggle : MonoBehaviour
{
    private GameObject canvas;

    Toggle m_Toggle;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject recipimemoController_obj;
    private RecipiMemoController recipimemoController;

    private GameObject memoResult_obj;
    private Memo_Result memoResult;

    private PlayerItemList pitemlist;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    public int recipi_toggleitemType; //選んだアイテムが、イベントアイテムか、コンポ調合DBのアイテムかを判別する。0=イベントアイテム, 1=コンポ調合用DBアイテム
    public int recipi_toggleEventitem_ID; //リストの要素にイベントアイテムIDを保持する。
    public int recipi_itemID; //そのときのアイテムDB上のアイテムID。
    public string recipi_itemNameHyouji; //名前表示用


    private int i, j;

    private int pitemlist_max;
    private int count;
    private bool selectToggle;

    private int compo_itemID;
    private string compo_itemname;

    private int event_itemID;


    void Start()
    {

        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();


        //Add listener for when the state of the Toggle changes, to take action アドリスナー　トグルの値が変化したときに、｛｝内のメソッドを呼び出す
        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        });

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        recipimemoController_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiMemo_ScrollView").gameObject;
        recipimemoController = recipimemoController_obj.GetComponent<RecipiMemoController>();

        memoResult_obj = canvas.transform.Find("Compound_BGPanel_A/Memo_Result").gameObject;
        memoResult = memoResult_obj.GetComponent<Memo_Result>();

        yes = recipimemoController_obj.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = recipimemoController_obj.transform.Find("No").gameObject;

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();


        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();


        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

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
            recipi_memo_active();
        }
    }



    /* ### レシピ調合の処理 ### */

    public void recipi_memo_active()
    {
        count = 0;

        while (count < recipimemoController._recipi_listitem.Count)
        {
            selectToggle = recipimemoController._recipi_listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        recipimemoController._count1 = count; //リスト中の選択された番号を格納。


        //おすと、イベントIDに応じて、レシピのメモを表示する。
        if (recipimemoController._recipi_listitem[count].GetComponent<recipimemoSelectToggle>().recipi_toggleitemType == 0)
        {
            event_itemID = recipimemoController._recipi_listitem[count].GetComponent<recipimemoSelectToggle>().recipi_toggleEventitem_ID;

            memoResult.SeteventID(event_itemID);
            memoResult_obj.SetActive(true);
        } 

    }

}
