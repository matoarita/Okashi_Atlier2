//Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
//Set your own Text in the Inspector window

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


public class magicskillSelectToggle : MonoBehaviour
{
    private GameObject canvas;

    Toggle m_Toggle;
    public Text m_Text; //デバッグ用。未使用。

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private Exp_Controller exp_Controller;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject card_view_obj;
    private CardView card_view;
    private GameObject blackpanel_A;

    private GameObject magicskilllistController_obj;
    private MagicSkillListController magicskilllistController;

    private GameObject back_ShopFirst_obj;
    private Button back_ShopFirst_btn;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private MagicSkillListDataBase magicskill_database;

    private GameObject yes_no_panel;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    public int toggle_skill_ID; //スキルデータベース上のIDを保持する。IDは、単なる行の番号なので、あとで番号自体が変わる可能性がある。処理を分けるなら、名前で分けるようにする。
    public string toggle_skill_name; //使用するスキルの名前　
    public string toggle_skill_nameHyouji; //表示用名前
    public int toggle_skill_type; //リストの要素に、スキルタイプを保持
    public int toggle_skill_cost;

    private int i;

    private int _itemcount; //現在の所持数　店売り＋オリジナル
    private string _item_Namehyouji;
    private int pitemlist_max;
    private int count;
    private bool selectToggle;

    private List<GameObject> category_toggle = new List<GameObject>();

    private int kettei_item1; //このスクリプトは、プレファブのインスタンスに取り付けているので、各プレファブ共通で、変更できる値が必要。そのパラメータは、PlayerItemListControllerで管理する。

    void Start()
    {
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

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

        magicskilllistController_obj = canvas.transform.Find("MagicSkillList_Panel/MagicSkillList_ScrollView").gameObject;
        magicskilllistController = magicskilllistController_obj.GetComponent<MagicSkillListController>();
        //back_ShopFirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        //back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes_no_panel.SetActive(false);

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        blackpanel_A = canvas.transform.Find("Black_Panel_A").gameObject;

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();


        //カテゴリータブの取得
        category_toggle.Clear();
        foreach (Transform child in magicskilllistController_obj.transform.Find("CategoryView/Viewport/Content/").transform)
        {
            //Debug.Log(child.name);           
            category_toggle.Add(child.gameObject);
        }        


        text_area = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/MessageWindowComp").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        i = 0;
        count = 0;

    }


    void Update()
    {
        if (magicskilllistController.skill_final_select_flag == true) //最後、これを使うかどうかを待つフラグ
        {

            magicskilllistController.skill_final_select_flag = false;
            StartCoroutine("skilluse_Final_select");
        }
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        //m_Text.text = "New Value : " + m_Toggle.isOn;
        if (m_Toggle.isOn == true)
        {
            itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する
            GameMgr.compound_status = 100; //トグルを押して、調合中の状態。All_cancelで、status=4に戻る。status=4でキャンセルすると、最初の調合選択シーンに戻る。

            //back_ShopFirst_btn.interactable = false;
            skill_use_active();
        }
    }


    /* ### スキルを使うときのシーン ### */

    public void skill_use_active()
    {

        //アイテムを選択したときの処理（トグルの処理）

        count = 0;

        while (count < magicskilllistController._skill_listitem.Count)
        {
            selectToggle = magicskilllistController._skill_listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        magicskilllistController.skill_count = count; //カウントしたリスト番号を保持
        magicskilllistController.skill_kettei_ID = magicskilllistController._skill_listitem[count].GetComponent<magicskillSelectToggle>().toggle_skill_ID; //IDを入れる。
        magicskilllistController.skill_Type = magicskilllistController._skill_listitem[count].GetComponent<magicskillSelectToggle>().toggle_skill_type; //
        magicskilllistController.skill_Name = magicskilllistController._skill_listitem[count].GetComponent<magicskillSelectToggle>().toggle_skill_name; //使用するスキル英字ネーム
        _item_Namehyouji = magicskilllistController._skill_listitem[count].GetComponent<magicskillSelectToggle>().toggle_skill_nameHyouji; //表示用ネームを入れる。
        magicskilllistController.skill_itemName_Hyouji = _item_Namehyouji;
        magicskilllistController.skill_cost = magicskilllistController._skill_listitem[count].GetComponent<magicskillSelectToggle>().toggle_skill_cost;

        _text.text = _item_Namehyouji + "を使いますか？";
        //card_view.ShopSelectCard_DrawView(1, magicskilllistController.skill_kettei_item1);

        Debug.Log(count + "番が押されたよ");
        Debug.Log("アイテム:" + _item_Namehyouji + "が選択されました。");

        blackpanel_A.SetActive(true);

        //Debug.Log("これでいいですか？");

        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < magicskilllistController._skill_listitem.Count; i++)
        {
            magicskilllistController._skill_listitem[i].GetComponent<Toggle>().interactable = false;
        }
        for (i = 0; i < category_toggle.Count; i++)
        {
            category_toggle[i].GetComponent<Toggle>().interactable = false;
        }

        yes_no_panel.SetActive(true);

        magicskilllistController.skill_final_select_flag = true; //確認のフラグ
        //StartCoroutine("shop_buy_kosu_select");

    }

    /*IEnumerator shop_buy_kosu_select()
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

                magicskilllistController.skill_final_select_flag = true; //確認のフラグ

                Debug.Log("選択完了！");
                
                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                _text.text = "何にしますか？";

                //キャンセル時、リストのインタラクティブ解除。その時、プレイヤーの所持金をチェックし、足りないものはOFF表示にする。
                Skill_Check();

                yes_no_panel.SetActive(false);

                back_ShopFirst_btn.interactable = true;

                card_view.DeleteCard_DrawView();
                blackpanel_A.SetActive(false);

                break;
        }
    }*/


    IEnumerator skilluse_Final_select()
    {
        _text.text = magicskilllistController.skill_itemName_Hyouji + "を使いますか？";

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。


                for (i = 0; i < magicskilllistController._skill_listitem.Count; i++)
                {
                    magicskilllistController._skill_listitem[i].GetComponent<Toggle>().interactable = true;
                    magicskilllistController._skill_listitem[i].GetComponent<Toggle>().isOn = false;
                }
                for (i = 0; i < category_toggle.Count; i++)
                {
                    category_toggle[i].GetComponent<Toggle>().interactable = true;
                }

                card_view.DeleteCard_DrawView();
                blackpanel_A.SetActive(false);

                yes_no_panel.SetActive(false);
                //back_ShopFirst_btn.interactable = true;

                itemselect_cancel.kettei_on_waiting = false;

                //スキルに応じて、次の処理を決める。
                //スキルネームが「Freezing_Cookie」なら、それに応じた処理などにケースを分ける。
                GameMgr.UseMagicSkill = magicskilllistController.skill_Name;
                GameMgr.UseMagicSkill_nameHyouji = magicskilllistController.skill_itemName_Hyouji;
                GameMgr.UseMagicSkill_ID = magicskilllistController.skill_kettei_ID;

                SkillUseLibrary();                              

                break;

            case false:

                //Debug.Log("cancel");

                _text.text = "どの魔法を使う？";

                //キャンセル時、リストのインタラクティブ解除。
                Skill_Check();

                card_view.DeleteCard_DrawView();
                blackpanel_A.SetActive(false);

                yes_selectitem_kettei.kettei1 = false;
                yes_no_panel.SetActive(false);

                itemselect_cancel.kettei_on_waiting = false;
                GameMgr.compound_status = 4;
                //back_ShopFirst_btn.interactable = true;

                break;
        }

    }

    void Skill_Check()
    {
        SkillCheck_Method();          
    }

    void SkillCheck_Method()
    {
        for (i = 0; i < magicskilllistController._skill_listitem.Count; i++)
        {
            magicskilllistController._skill_listitem[i].GetComponent<Toggle>().interactable = true;
            magicskilllistController._skill_listitem[i].GetComponent<Toggle>().isOn = false;
        }

        for (i = 0; i < category_toggle.Count; i++)
        {
            category_toggle[i].GetComponent<Toggle>().interactable = true;
        }
    }

    public void OnSkillMemoButton_ON()
    {
        GameMgr.UseMagicSkill_ID = toggle_skill_ID;
        if (magicskilllistController_obj.transform.Find("SkillMemo_Result").gameObject.activeInHierarchy)
        {
            magicskilllistController_obj.transform.Find("SkillMemo_Result").gameObject.SetActive(false);
        }
        else
        {
            magicskilllistController_obj.transform.Find("SkillMemo_Result").gameObject.SetActive(true);
        }
    }

    public void OnSkillLevelUpButton_ON()
    {
        magicskill_database.magicskill_lists[toggle_skill_ID].skillLv++;
        //上限処理
        if(magicskill_database.magicskill_lists[toggle_skill_ID].skillLv > magicskill_database.magicskill_lists[toggle_skill_ID].skillMaxLv)
        {
            magicskill_database.magicskill_lists[toggle_skill_ID].skillLv = magicskill_database.magicskill_lists[toggle_skill_ID].skillMaxLv;
        }
        magicskilllistController.ReDraw();
    }


    //
    //魔法の処理分岐//
    //

    void SkillUseLibrary()
    {
        //PlayerItemListControllerでも以下の魔法分岐を行ってるので、そちらでも書く

        switch (GameMgr.UseMagicSkill)
        {
            case "Freezing_Spell":

                GameMgr.compound_status = 21; //21は魔法を選んで、かけるアイテムを選択する場合の処理　他数字を使う場合、CompoundMainControllerにも記述する
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい材料を選んでね。";
                break;

            case "Luminous_Suger":
                GameMgr.compound_status = 21;
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい砂糖を選んでね。";
                break;

            case "Luminous_Fruits":
                GameMgr.compound_status = 21;
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいフルーツを選んでね。";
                break;

            case "Bake_Beans":
                GameMgr.compound_status = 21;
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい豆を選んでね。";
                break;

            case "Removing_Shells":
                GameMgr.compound_status = 21;
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "殻をむく豆を選んでね。";
                break;

            case "Chocolate_Tempering":
                GameMgr.compound_status = 21;
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "テンパリングするカカオを選んでね。";
                break;

            case "Wind_Twister":
                GameMgr.compound_status = 21;
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かける素材を選んでね。";
                break;

            default: //例外処理　通常ここを通ることはない..が、処理を未登録などの場合、ひとまずここを通る。
                GameMgr.compound_status = 21;
                break;
        }
    }
}
