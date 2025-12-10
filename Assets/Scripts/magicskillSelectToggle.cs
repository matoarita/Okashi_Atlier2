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
    private GameObject compoBG_A;
    private GameObject magic_Effect_Panel;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    public int toggle_skill_ID; //スキルデータベース上のIDを保持する。IDは、単なる行の番号なので、あとで番号自体が変わる可能性がある。処理を分けるなら、名前で分けるようにする。
    public string toggle_skill_name; //使用するスキルの名前　
    public string toggle_skill_nameHyouji; //表示用名前
    public int toggle_skill_type; //リストの要素に、スキルタイプを保持
    public int toggle_skill_cost;
    public int toggle_skill_timecost;

    private int i;

    private int _itemcount; //現在の所持数　店売り＋オリジナル
    private string _item_Namehyouji;
    private string _skillname;
    private int pitemlist_max;
    private int count;
    private bool selectToggle;
    private int _id, _mlv;

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

        if (GameMgr.MagicSkillSelectStatus == 0) //魔法を使う場合 
        {
            magicskilllistController_obj = canvas.transform.Find("MagicSkillList_Panel/MagicSkillList_ScrollView2").gameObject;
        }
        else
        {
            magicskilllistController_obj = canvas.transform.Find("MagicSkillList_Panel/MagicSkillList_ScrollView").gameObject;
        }
        
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
        compoBG_A = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A").gameObject;
        magic_Effect_Panel = compoBG_A.transform.Find("MagicStartPanel/magicComp1/MagicEffectPanel").gameObject;
        OffMagicEffect();

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

    void OffMagicEffect()
    {
        foreach (Transform child in magic_Effect_Panel.transform)
        {
            //Debug.Log(child.name);           
            child.gameObject.SetActive(false);
        }
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
        _skillname = magicskilllistController.skill_Name;
        _item_Namehyouji = magicskilllistController._skill_listitem[count].GetComponent<magicskillSelectToggle>().toggle_skill_nameHyouji; //表示用ネームを入れる。
        magicskilllistController.skill_itemName_Hyouji = _item_Namehyouji;
        magicskilllistController.skill_cost = magicskilllistController._skill_listitem[count].GetComponent<magicskillSelectToggle>().toggle_skill_cost;
        magicskilllistController.skill_timecost = magicskilllistController._skill_listitem[count].GetComponent<magicskillSelectToggle>().toggle_skill_timecost;

        //_text.text = _item_Namehyouji + "を使いますか？";
        //card_view.ShopSelectCard_DrawView(1, magicskilllistController.skill_kettei_item1);

        Debug.Log(count + "番が押されたよ");
        Debug.Log("アイテム:" + _item_Namehyouji + "が選択されました。");

        _id = magicskill_database.SearchSkillString(_skillname);
        if (magicskill_database.magicskill_lists[_id].skill_CompSelect != "PlayerBuf")
        {
            //①
            UseKetteiMagicMethod(); //すぐに魔法の次処理画面へ
        }
        else
        {
            //プレイヤーにかける魔法やその他の支援魔法など　一回使うかを確認する

            //②
            //以下処理は、使いますかで最終確認する場合の処理　確認したい場合は、チェックを外す

            compoBG_A.GetComponent<Compound_BGPanel_A>().BlackImageON();

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

            //魔法のエフェクトを表示する
            //SkillUseLibrary(0);

            magicskilllistController.skill_final_select_flag = true; //確認のフラグ
        }
    }


    IEnumerator skilluse_Final_select()
    {
        _text.text = magicskilllistController.skill_itemName_Hyouji + "を使いますか？" + "\n" + "MP: " + 
            magicskilllistController.skill_cost + "　消費時間: " + magicskilllistController.skill_timecost + "分";

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        compoBG_A.GetComponent<Compound_BGPanel_A>().BlackImageOFF();
        OffMagicEffect(); //エフェクトは消す

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。

                UseKetteiMagicMethod();                                 
                break;

            case false:

                //Debug.Log("cancel");

                _text.text = "どの魔法を使う？";

                //キャンセル時、リストのインタラクティブ解除。
                Skill_Check();

                card_view.DeleteCard_DrawView();

                yes_selectitem_kettei.kettei1 = false;
                yes_no_panel.SetActive(false);

                itemselect_cancel.kettei_on_waiting = false;
                GameMgr.compound_status = 4;
                //back_ShopFirst_btn.interactable = true;

                break;
        }

    }

    void UseKetteiMagicMethod()
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

        card_view.DeleteCard_DrawView();

        yes_no_panel.SetActive(false);
        //back_ShopFirst_btn.interactable = true;

        itemselect_cancel.kettei_on_waiting = false;

        //スキルに応じて、次の処理を決める。
        //スキルネームが「Freezing_Cookie」なら、それに応じた処理などにケースを分ける。
        GameMgr.UseMagicSkill = magicskilllistController.skill_Name;
        GameMgr.UseMagicSkill_nameHyouji = magicskilllistController.skill_itemName_Hyouji;
        GameMgr.UseMagicSkill_ID = magicskilllistController.skill_kettei_ID;
        GameMgr.UseMagicSkill_TimeCost = magicskilllistController.skill_timecost;
        GameMgr.UseMagicParamCustom = 0; //使用前にリセット

        _skillname = GameMgr.UseMagicSkill;
        SkillUseLibrary(1);
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

            magicskilllistController.UseHyouji_ONOFF(i, magicskilllistController._skill_listitem[i].GetComponent<magicskillSelectToggle>().toggle_skill_ID);
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

    void SkillUseLibrary(int _mstatus)
    {
        //PlayerItemListControllerでも以下の魔法分岐を行ってるので、そちらでも書く

        switch (_skillname)
        {
            case "Caramelized":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいお菓子を選んでね。";
                break;

            case "Bake_Beans":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい豆を選んでね。";
                break;

            case "Fire_Flowers":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいお菓子を選んでね。";
                break;

            case "Removing_Shells":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "豆を選んでね。";
                break;

            case "Chocolate_Tempering":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいカカオマスを選んでね。";
                break;

            case "Cookie_SecondBake":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "クッキーを選んでね。";
                break;

            case "Freezing_Spell":

                CompoStatusMethod(0);    //21は魔法を選んで、かけるアイテムを選択する場合の処理　他数字を使う場合、CompoundMainControllerにも記述する
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい材料を選んでね。";
                break;

            case "Ice_Cube":

                CompoStatusMethod(0);    //21は魔法を選んで、かけるアイテムを選択する場合の処理　他数字を使う場合、CompoundMainControllerにも記述する
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "水の材料を選んでね。";
                break;

            case "SugerPot":

                CompoStatusMethod(0);    //21は魔法を選んで、かけるアイテムを選択する場合の処理　他数字を使う場合、CompoundMainControllerにも記述する
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい材料を選んでね。";
                break;

            case "Luminous_Suger":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい砂糖を選んでね。";
                break;

            case "Luminous_Fruits":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいフルーツを選んでね。";
                break;

            case "Buttelfy_illumination":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいお菓子を選んでね。";
                break;

            case "Aroma_Potion":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "お花を選んでね。";
                break;

            case "Wind_Ark":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かける素材を選んでね。";
                break;

            case "Wind_Twister":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かける素材を選んでね。";
                break;

            case "Wind_Heart":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かける素材を選んでね。";
                break;

            case "Float_Material":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かける素材を選んでね。";
                break;

            case "Bubble_Mist":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいお菓子を選んでね。";
                break;

            case "Statue_of_Penguin":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい液体を選んでね。";
                break;

            case "Statue_of_Bear":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい液体を選んでね。";
                break;

            case "Statue_of_Rabitts":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい液体を選んでね。";
                break;

            case "Statue_of_AngelWing":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい液体を選んでね。";
                break;

            case "Star_Blessing":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいお菓子を選んでね。";
                break;

            case "Latte_Art":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい飲み物を選んでね。";
                break;

            case "Moonlight_Banana":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいバナナを選んでね。";
                break;

            case "Magic_Soda":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいソーダを選んでね。";
                break;

            case "Rainbow_Rain":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい材料を選んでね。";
                break;           

            case "Warming_Handmade":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいお菓子を選んでね。";
                break;

            case "Life_Stream":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいお菓子を選んでね。";
                break;

            case "AbraCadabra":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいお菓子を選んでね。";
                break;

            case "True_of_Myheart":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいお菓子を選んでね。";
                break;

            //パラメータ変えれる関係
            case "Dreamy_Sapphire":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいソーダを選んでね。";
                Check_ParamUpdownCounter();
                break;

            case "Lightning_Grape":

                CompoStatusMethod(0);
                _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたいソーダを選んでね。";
                Check_ParamUpdownCounter();
                break;

            //プレイヤーバフ関係
            case "Epiclesis":

                CompoStatusMethod(1);
             
                PStatusBuf(0);

                //効果のテキストもここで書く
                GameMgr.MagicUseType_StatusText = "「エピクレイシス」状態になった！" + "\n" + "効果: 次に作るお菓子の成功率が、大きく上がる！";
                break;

            case "Latria":

                CompoStatusMethod(1);

                PStatusBuf(1);

                //効果のテキストもここで書く
                GameMgr.MagicUseType_StatusText = "「ラトリア」状態になった！" + "\n" + "効果: " + "\n" + "しばらくの間、油っぽさ・水っぽさ・粉っぽさが上がりにくくなる。";
                
                break;

            case "Three_Stars":

                CompoStatusMethod(1);

                PStatusBuf(2);

                //効果のテキストもここで書く
                if (magicskill_database.magicskill_lists[_id].skillLv == 1)
                {
                    GameMgr.MagicUseType_StatusText = "「スリースターズ」状態になった！" + "\n" + "効果: しばらくの間、消費MPが 1/2 になる。";
                }
                else if (magicskill_database.magicskill_lists[_id].skillLv == 2)
                {
                    GameMgr.MagicUseType_StatusText = "「スリースターズ」状態になった！" + "\n" + "効果: しばらくの間、消費MPが 1/3 になる。";
                }

                break;

            

            default: //例外処理　通常ここを通ることはない..が、処理を未登録などの場合、ひとまずここを通る。

                switch (_mstatus)
                {
                    case 0: //魔法選択時のエフェクトを表示

                        MagicEffectOn("effect01_Luminous_Suger");
                        break;

                    case 1: //最終決定後。魔法の次の処理をかく

                        CompoStatusMethod(0);                        
                        _text.text = magicskilllistController.skill_itemName_Hyouji + "→ " + "\n" + "かけたい材料を選んでね。";
                        break;
                }

                break;
        }

    }

    void CompoStatusMethod(int _mstatus)
    {
        switch(_mstatus)
        {
            case 0: //アイテムに魔法をかけるタイプの場合

                GameMgr.MagicUseTypeSelect = 0;

                if (GameMgr.compound_select == 20)
                {
                    GameMgr.compound_status = 21; //21は魔法を選んで、かけるアイテムを選択する場合の処理　他数字を使う場合、CompoundMainControllerにも記述する
                }
                else if (GameMgr.compound_select == 9)
                {
                    GameMgr.compound_status = 10;
                }
                break;

            case 1: //プレイヤーに魔法をかけて状態変化させる場合

                GameMgr.MagicUseTypeSelect = 1;

                if (GameMgr.compound_select == 20)
                {
                    GameMgr.compound_status = 22; //21は魔法を選んで、かけるアイテムを選択する場合の処理　他数字を使う場合、CompoundMainControllerにも記述する
                }
                break;
        }      
    }

    void PStatusBuf(int sta_id)
    {
        _id = magicskill_database.SearchSkillString(GameMgr.UseMagicSkill);
        GameMgr.UseMagicSkillLv = magicskill_database.magicskill_lists[_id].skillLv;

        //入れる前に、重ね掛けかどうかをチェック
        if(PlayerStatus.player_girl_status[sta_id] > 0) //すでにかかってる場合は、0より大きい
        {
            GameMgr.Magic_CheckKasaneGake = true; //すでにかかってますよ
        } else
        {
            GameMgr.Magic_CheckKasaneGake = false; //重ね掛けでない。一回目使用。
        }

        PlayerStatus.player_girl_status[sta_id] = magicskill_database.magicskill_lists[_id].skillLv; //習得LVで数字を入れる。すなわち、そのスキルのLV。
        PlayerStatus.player_girl_status_timecounter[sta_id] =
            magicskill_database.magicskill_lists[_id].status_time * magicskill_database.magicskill_lists[_id].skillLv; //持続時間　分単位

        GameMgr.Compo_FinalCostTime = magicskill_database.magicskill_lists[_id].cost_time; //魔法使用にかかる時間
        GameMgr.Magic_CheckIgnore = sta_id; //チェック無視　使用したばかりの魔法は、そのときの使用のカウントはしない。おもにエピクレイシス。ExpControllerでチェック用に使う。
        GameMgr.Magic_AfterSettingTime = PlayerStatus.player_girl_status_timecounter[sta_id]; //あとで再設定する用の持続時間

        //Debug.Log("プレイヤー状態チェックPlayerStatus.player_girl_status[sta_id]: " + PlayerStatus.player_girl_status[sta_id]);
    }

    //魔法によって、パラメータを操作できる。LVによっても表示のON/OFFを変えるので、一度ここでチェック
    void Check_ParamUpdownCounter()
    {
        _id = magicskill_database.SearchSkillString(GameMgr.UseMagicSkill);
        _mlv = magicskill_database.magicskill_lists[_id].skillLv;

        switch(_skillname)
        {
            case "Dreamy_Sapphire":

                if(_mlv >= 2)
                {
                    GameMgr.UseMagicParamCustom = 1; //あまさを参照
                    GameMgr.UseMagicParamCustomText = "あまさの値";
                    GameMgr.UseMagicParamCustom_ScoreMinMax = 15;
                }
                break;

            case "Lightning_Grape":

                if (_mlv >= 2)
                {
                    GameMgr.UseMagicParamCustom = 2; //すっぱさを参照
                    GameMgr.UseMagicParamCustomText = "すっぱさの値";
                    GameMgr.UseMagicParamCustom_ScoreMinMax = 10;
                }
                break;
        }
    }

    //今は使ってない。魔法選択時にカードとエフェクトが表示される。
    void MagicEffectOn(string _effname)
    {
        foreach (Transform child in magic_Effect_Panel.transform)
        {
            //Debug.Log(child.name);  
            if(child.gameObject.name == _effname && _effname != "")
            {
                child.gameObject.SetActive(true);
            }           
        }
    }
}
