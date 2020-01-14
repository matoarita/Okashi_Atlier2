using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemSelect_Cancel : SingletonMonoBehaviour<ItemSelect_Cancel>
{

    private GameObject text_area; //Sceneテキスト表示エリアのこと。
    private Text _text; //

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject GirlEat_scene_obj;
    private GirlEat_Main girlEat_scene;

    private GameObject QuestBox_scene_obj;
    private QuestBox_Main questBox_scene;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;
    private Exp_Controller exp_Controller;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject canvas;

    private GameObject shopitemlistController_obj;
    private ShopItemListController shopitemlistController;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private Updown_counter_recipi updown_counter_recipi;
    private Button[] updown_button = new Button[2];

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject item_tsuika; //PlayeritemList_ScrollViewの子オブジェクト「item_tsuika」ボタン

    private ItemDataBase database;

    private GameObject black_effect;

    public int update_ListSelect_Flag;

    public bool kettei_on_waiting;

    private int i;

    // Use this for initialization
    void Start() {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //黒半透明パネルの取得
        //black_effect = GameObject.FindWithTag("Black_Effect");

        canvas = GameObject.FindWithTag("Canvas");

        update_ListSelect_Flag = 0;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Hiroba":
                break;

            case "Compound":
                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                break;

            case "Shop":

                shopitemlistController_obj = GameObject.FindWithTag("ShopitemList_ScrollView");
                shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

                updown_counter_obj = shopitemlistController_obj.transform.Find("updown_counter").gameObject;
                updown_counter = updown_counter_obj.GetComponent<Updown_counter>();
                updown_button = updown_counter_obj.GetComponentsInChildren<Button>();

                yes = shopitemlistController_obj.transform.Find("Yes").gameObject;
                yes_text = yes.GetComponentInChildren<Text>();
                no = shopitemlistController_obj.transform.Find("No").gameObject;
                no_text = no.GetComponentInChildren<Text>();

                selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
                yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

                break;

            case "GirlEat":
                GirlEat_scene_obj = GameObject.FindWithTag("GirlEat_scene");
                girlEat_scene = GirlEat_scene_obj.GetComponent<GirlEat_Main>();

                break;

            case "QuestBox":
                QuestBox_scene_obj = GameObject.FindWithTag("QuestBox_Main");
                questBox_scene = QuestBox_scene_obj.GetComponent<QuestBox_Main>();

                break;

            case "Travel":
                //Setup_Scene1();
                break;

            default:
                
                break;

        }
       
    }



    // Update is called once per frame
    void Update() {

        //初期化
        switch (SceneManager.GetActiveScene().name)
        {

            case "000_Prologue": //シナリオ系のシーンでは読み込まない。
                break;

            case "001_Chapter1":
                break;

            case "Compound":

                //プレイヤーアイテムリストオブジェクトの初期化
                if (pitemlistController_obj == null)
                {
                    pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
                    pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();
                }

                //レシピリストオブジェクトの初期化
                if (recipilistController_obj == null)
                {
                    recipilistController_obj = canvas.transform.Find("RecipiList_ScrollView").gameObject;
                    recipilistController = recipilistController_obj.GetComponent<RecipiListController>();
                }

                break;

            default:

                //プレイヤーアイテムリストオブジェクトの初期化
                if (pitemlistController_obj == null)
                {
                    pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
                    pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();
                }

                break;
        }



        //各シーンごとの、待機処理

        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {

            //調合中か、あげる処理に入っているか、もしくはアイテムリストを開いているとき
            if (compound_Main.compound_status == 4)
            {
                if (compound_Main.compound_select == 1) //レシピ調合のときは、参照するオブジェクトが変わる。
                {
                    yes = recipilistController_obj.transform.Find("Yes").gameObject;
                    yes_text = yes.GetComponentInChildren<Text>();
                    no = recipilistController_obj.transform.Find("No").gameObject;

                    selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
                    yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

                    updown_counter_obj = recipilistController_obj.transform.Find("updown_counter").gameObject;
                    updown_counter_recipi = updown_counter_obj.GetComponent<Updown_counter_recipi>();
                    updown_button = updown_counter_obj.GetComponentsInChildren<Button>();
                }
                else
                {
                    yes = pitemlistController_obj.transform.Find("Yes").gameObject;
                    yes_text = yes.GetComponentInChildren<Text>();
                    no = pitemlistController_obj.transform.Find("No").gameObject;

                    selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
                    yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

                    updown_counter_obj = pitemlistController_obj.transform.Find("updown_counter").gameObject;
                    updown_counter = updown_counter_obj.GetComponent<Updown_counter>();
                    updown_button = updown_counter_obj.GetComponentsInChildren<Button>();
                }

                if (yes_selectitem_kettei.onclick == true) //Yes, No ボタンが押された
                {
                    if (kettei_on_waiting == false) //トグルが押されていない時で、調合選択最中の状態を表す。
                    {
                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                        {
                            //Debug.Log("調合シーンキャンセル");

                            card_view.DeleteCard_DrawView();

                            compound_Main.compound_status = 0; //何も選択していない状態にもどる。
                            compound_Main.compound_select = 0;

                            yes_selectitem_kettei.onclick = false;

                        }
                    }
                }
            }

            //compound_status = 100のとき。一度トグルをおし、カードなどを選択し始めた場合、status=100になる。
            else
            {

                //調合選択中のとき、キャンセル待ち処理
                if (compound_Main.compound_select == 3) //オリジナル調合のときの処理
                {
                    if (pitemlistController.final_select_flag == false) //最後、これで調合するかどうかを待つフラグ
                    {

                        //オリジナル調合時の、待機中の処理
                        {
                            if (yes_selectitem_kettei.onclick == true) //Yes, No ボタンが押された
                            {
                                if (kettei_on_waiting == false) //待機状態を表す。トグルが押されると、kettei_on_waiting=trueになり、トグルの処理が優先される。
                                {
                                    if (pitemlistController.kettei1_bunki == 1) //現在一個目を選択している状態
                                    {
                                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                        {
                                            //Debug.Log("一個目はcancel");

                                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                                            All_cancel();

                                        }
                                    }

                                    if (pitemlistController.kettei1_bunki == 2) //現在二個目を選択している状態
                                    {
                                        if (yes_selectitem_kettei.kettei1 == true) //調合二個で決定した状態
                                        {

                                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                                            pitemlistController.final_select_flag = true; //最後調合するかどうかのフラグをオンに。

                                        }

                                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                        {
                                            //Debug.Log("二個目はcancel");

                                            Two_cancel();

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            

                if (compound_Main.compound_select == 2) //トッピング調合のときの処理
                {

                    if (pitemlistController.final_select_flag == false) //最後、これで調合するかどうかを待つフラグ
                    {

                        //トッピング調合時の、待機中の処理
                        {
                            if (yes_selectitem_kettei.onclick == true) //Yes, No ボタンが押された
                            {
                                if (kettei_on_waiting == false) //待機状態を表す。トグルが押されると、kettei_on_waiting=trueになり、トグルの処理が優先される。
                                {
                                    if (pitemlistController.kettei1_bunki == 10) //現在ベースアイテムを選択している状態
                                    {
                                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                        {
                                            //Debug.Log("一個目はcancel");

                                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                                            pitemlistController.topping_DrawView_1(); //リストビューを更新し、トッピング材料だけ表示する。

                                            All_cancel();

                                        }
                                    }

                                    if (pitemlistController.kettei1_bunki == 11) //現在一個目を選択している状態
                                    {
                                        if (yes_selectitem_kettei.kettei1 == true) //ベースアイテム＋調合１個で決定した状態
                                        {

                                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                                            pitemlistController.final_select_flag = true; //最後調合するかどうかのフラグをオンに。

                                        }

                                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                        {
                                            //Debug.Log("一個目はcancel");

                                            Two_cancel();
                                        }
                                    }

                                    if (pitemlistController.kettei1_bunki == 12) //現在二個目を選択している状態
                                    {
                                        if (yes_selectitem_kettei.kettei1 == true) //ベースアイテム＋調合二個で決定した状態
                                        {

                                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                                            pitemlistController.final_select_flag = true; //最後調合するかどうかのフラグをオンに。

                                        }

                                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                        {
                                            //Debug.Log("二個目はcancel");

                                            Three_cancel();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (compound_Main.compound_select == 99) //アイテム欄を開いているときで、カードが表示されている。
                {
                    if (yes_selectitem_kettei.onclick == true) //Yes, No ボタンが押された
                    {
                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                        {
                            All_cancel();

                            //カード表示を消す
                            card_view.DeleteCard_DrawView();

                            yes.SetActive(false);
                            no.SetActive(true);

                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                        }
                    }
                }
            }
        }


        if (SceneManager.GetActiveScene().name == "GirlEat") // 女の子シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {
            if (yes_selectitem_kettei.onclick == true) //Yes, No ボタンが押された
            {
                if (kettei_on_waiting == false) //トグルが押されていない時
                {
                    if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                    {
                        //Debug.Log("キャンセル");

                        GirlEat_scene_obj = GameObject.FindWithTag("GirlEat_scene");
                        girlEat_scene = GirlEat_scene_obj.GetComponent<GirlEat_Main>();

                        girlEat_scene.girleat_status = 0;
                        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                        //All_cancel();
                    }
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "QuestBox")
        {
            if (yes_selectitem_kettei.onclick == true) //Yes, No ボタンが押された
            {
                if (kettei_on_waiting == false) //トグルが押されていない時で、調合選択最中の状態を表す。トグルが押されると、これはfalseになり、トグルの処理が優先される。
                {
                    if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                    {
                        //Debug.Log("キャンセル");

                        questBox_scene.qbox_status = 0;
                        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                        //All_cancel();
                    }
                }
            }
        }
    }



    //一個目の選択をキャンセルする処理
    public void All_cancel()
    {
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        kettei_on_waiting = false;

        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理
        {
            compound_Main.compound_status = 4;

            //まずは、レシピ・それ以外の調合用にオブジェクト取得
            if (compound_Main.compound_select == 1) //レシピ調合のときは、参照するオブジェクトが変わる。
            {
                yes = recipilistController_obj.transform.Find("Yes").gameObject;
                yes_text = yes.GetComponentInChildren<Text>();
                no = recipilistController_obj.transform.Find("No").gameObject;

                updown_counter_obj = recipilistController_obj.transform.Find("updown_counter").gameObject;
                updown_counter_recipi = updown_counter_obj.GetComponent<Updown_counter_recipi>();
                updown_button = updown_counter_obj.GetComponentsInChildren<Button>();
            }
            else
            {
                yes = pitemlistController_obj.transform.Find("Yes").gameObject;
                yes_text = yes.GetComponentInChildren<Text>();
                no = pitemlistController_obj.transform.Find("No").gameObject;

                updown_counter_obj = pitemlistController_obj.transform.Find("updown_counter").gameObject;
                updown_counter = updown_counter_obj.GetComponent<Updown_counter>();
                updown_button = updown_counter_obj.GetComponentsInChildren<Button>();
            }


            //オリジナル調合の処理
            if (compound_Main.compound_select == 3) 
            {
                if (pitemlistController.kettei1_bunki == 1)
                {
                    _text.text = "一つ目の材料を選択してね。";
                }

                pitemlistController.kettei1_bunki = 0;

                update_ListSelect_Flag = 0; //オールリセットするのみ。
                update_ListSelect(); //アイテム選択時の、リストの表示処理
            }

            //エクストリーム調合のときの処理
            else if (compound_Main.compound_select == 2) 
            {
                if (pitemlistController.kettei1_bunki == 10)
                {
                    _text.text = "ベースのお菓子を選択してね。";
                }

                pitemlistController.kettei1_bunki = 0;

                update_ListSelect_Flag = 0; //オールリセットするのみ。
                update_ListSelect(); //アイテム選択時の、リストの表示処理
            }

            //レシピ調合のときの処理
            else if (compound_Main.compound_select == 1) 
            {
                _text.text = "レシピを選択してね。";

                //レシピのキャンセル(recipiitemlistController)は、recipiitemSelectToggleの中で処理。
                //なので、recipiitemlistControllerは、このスクリプト内では記述していない。
            }

            //焼くのときの処理
            else if (compound_Main.compound_select == 5)
            {
                _text.text = "一つ目の材料を選択してね。";


                pitemlistController.kettei1_bunki = 0;

                update_ListSelect_Flag = 0; //オールリセットするのみ。
                update_ListSelect(); //アイテム選択時の、リストの表示処理
            }

            //お菓子をあげるときの処理
            if (compound_Main.compound_select == 10)
            {
                _text.text = "あげるお菓子を選択してね。";


                pitemlistController.kettei1_bunki = 0;

                update_ListSelect_Flag = 0; //オールリセットするのみ。
                update_ListSelect(); //アイテム選択時の、リストの表示処理
            }

        }
        else if (SceneManager.GetActiveScene().name == "GirlEat") // 女の子シーンでやりたい処理。
        {
            //girlEat_scene.girleat_status = 0;
            yes = pitemlistController_obj.transform.Find("Yes").gameObject;
            yes_text = yes.GetComponentInChildren<Text>();
            no = pitemlistController_obj.transform.Find("No").gameObject;


            update_ListSelect_Flag = 0; //オールリセットするのみ。
            update_ListSelect(); //アイテム選択時の、リストの表示処理
        }
        else
        {
            yes = pitemlistController_obj.transform.Find("Yes").gameObject;
            yes_text = yes.GetComponentInChildren<Text>();
            no = pitemlistController_obj.transform.Find("No").gameObject;

            update_ListSelect_Flag = 0; //オールリセットするのみ。
            update_ListSelect(); //アイテム選択時の、リストの表示処理
        }


        card_view.DeleteCard_DrawView();

        yes.SetActive(false);
        no.SetActive(true);
        updown_counter_obj.SetActive(false);

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        //黒半透明パネルの取得
        //black_effect = GameObject.FindWithTag("Black_Effect");
        //black_effect.SetActive(false); //黒半透明パネルをオフ
    }



    //以下の処理は、調合シーンのみで使う。

    //二個目の選択をキャンセルする処理（一個目は選択中）

    public void Two_cancel()
    {
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        kettei_on_waiting = false;
        //pitemlistController.kettei1_on = true; //トグル選択が持続している状態を表す

        if (pitemlistController.kettei1_bunki == 2)
            {
                update_ListSelect_Flag = 1; //二個目まで、選択できないようにする。
                update_ListSelect(); //アイテム選択時の、リストの表示処理

                pitemlistController._listitem[pitemlistController._count2].GetComponent<Toggle>().isOn = false; //選択していたものをキャンセル。

                pitemlistController.kettei1_bunki = 1;

                _text.text = "一個目: " + database.items[pitemlistController.final_kettei_item1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目を選択してください。";
            }

            if (pitemlistController.kettei1_bunki == 11)
            {
                update_ListSelect_Flag = 10; //ベースアイテム選択のみの状態
                update_ListSelect(); //アイテム選択時の、リストの表示処理

                pitemlistController._listitem[pitemlistController._count1].GetComponent<Toggle>().isOn = false;

                pitemlistController.kettei1_bunki = 10;

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "\n" + "一つ目のトッピングアイテムを選択してください。";
            }


            card_view.DeleteCard_DrawView02();
            card_view.OKCard_DrawView();

            yes.SetActive(false);
            //no.SetActive(false);
            updown_counter_obj.SetActive(false);

            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
    }



    //三個目の選択をキャンセルする処理
    public void Three_cancel()
    {
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        kettei_on_waiting = false;
        //pitemlistController.kettei1_on = true; //トグル選択が持続している状態を表す

        if (pitemlistController.kettei1_bunki == 3)
            {
                update_ListSelect_Flag = 2; //二個目まで、選択できないようにする。
                update_ListSelect(); //アイテム選択時の、リストの表示処理

                pitemlistController._listitem[pitemlistController._count3].GetComponent<Toggle>().isOn = false; //三個目の選択はキャンセル

                pitemlistController.kettei1_bunki = 2;

                _text.text = "一個目: " + database.items[pitemlistController.final_kettei_item1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目: " + database.items[pitemlistController.final_kettei_item2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + "最後に一つ追加できます。";
            }

            if (pitemlistController.kettei1_bunki == 12)
            {
                update_ListSelect_Flag = 11; //ベース・一個目の選択の状態に戻る。
                update_ListSelect(); //アイテム選択時の、リストの表示処理

                pitemlistController._listitem[pitemlistController._count2].GetComponent<Toggle>().isOn = false;

                pitemlistController.kettei1_bunki = 11;

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "\n" + "一個目: " + database.items[pitemlistController.final_kettei_item1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目を選択してください。";
            }

            card_view.DeleteCard_DrawView03();
            card_view.OKCard_DrawView02();

            yes_text.text = "決定";
            //yes.SetActive(false);
            //no.SetActive(false);
            updown_counter_obj.SetActive(false);

            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
    }


    //四個目の選択をキャンセルする処理
    public void Four_cancel()
    {
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //トッピング調合のときのみ、使う。

        kettei_on_waiting = false;
        //pitemlistController.kettei1_on = true; //一個目は選択が持続している状態を表す

            update_ListSelect_Flag = 12; //ベースアイテムと一個目・二個目を選択できないようにする。
            update_ListSelect();

            pitemlistController._listitem[pitemlistController._count3].GetComponent<Toggle>().isOn = false;

            pitemlistController.kettei1_bunki = 12;

            _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "\n" + "一個目: " + database.items[pitemlistController.final_kettei_item1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目: " + database.items[pitemlistController.final_kettei_item2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + "最後に一つ追加できます。";

            card_view.DeleteCard_DrawView04();
            card_view.OKCard_DrawView03();


            yes_text.text = "決定";
            //yes.SetActive(false);
            //no.SetActive(false);
            updown_counter_obj.SetActive(false);

            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
    }

    //リストからアイテム選択時に、選択したアイテムを再度入力できなくする処理
    public void update_ListSelect()
        {

            for (i = 0; i < pitemlistController._listitem.Count; i++)
            {
                //まずは、一度全て表示を初期化
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
            }

            if (update_ListSelect_Flag == 0)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;
                }
            }
            else if (update_ListSelect_Flag == 1)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_1();
                }
            }
            else if (update_ListSelect_Flag == 2)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_1();
                    update_ListSelect_2();
                }
            }
            else if (update_ListSelect_Flag == 3)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_1();
                    update_ListSelect_2();
                    update_ListSelect_3();
                }
            }
            else if (update_ListSelect_Flag == 10)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_base();
                }
            }
            else if (update_ListSelect_Flag == 11)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_base();
                    update_ListSelect_1();
                }
            }
            else if (update_ListSelect_Flag == 12)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_base();
                    update_ListSelect_1();
                    update_ListSelect_2();
                }
            }
            else if (update_ListSelect_Flag == 13)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_base();
                    update_ListSelect_1();
                    update_ListSelect_2();
                    update_ListSelect_3();
                }
            }
    }


    void update_ListSelect_base()
    {
            //トッピング調合時、ベースアイテムを選択できないようにする。

            //とりあえず、表示されてるリストを上から順番に見ていく。店売りかオリジナルの判定＋その時のプレイヤーリスト番号が一致するものが、一個目に選択したもの。

            if (pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggleitem_type == pitemlistController._base_toggle_type && pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggle_originplist_ID == pitemlistController.base_kettei_item)
            {
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false; //一個目も選択できないようにする
                //Debug.Log("一個目選択したもの: アイテムトグルタイプ" + pitemlistController._toggle_type1 + " リストID: " + pitemlistController.kettei_item1);
            }

    }

    void update_ListSelect_1()
    {
            //一個目選択したものを選択できないようにする。

            //とりあえず、表示されてるリストを上から順番に見ていく。店売りかオリジナルの判定＋その時のプレイヤーリスト番号が一致するものが、一個目に選択したもの。

            if (pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggleitem_type == pitemlistController._toggle_type1 && pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggle_originplist_ID == pitemlistController.kettei_item1)
            {
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false; //一個目も選択できないようにする
                //Debug.Log("一個目選択したもの: アイテムトグルタイプ" + pitemlistController._toggle_type1 + " リストID: " + pitemlistController.kettei_item1);
            }

    }

    void update_ListSelect_2()
        {
            //二個目選択したものを選択できないようにする。一個目と同様の処理。
            if (pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggleitem_type == pitemlistController._toggle_type2 && pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggle_originplist_ID == pitemlistController.kettei_item2)
            {
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false; //二個目も選択できないようにする
                //Debug.Log("二個目選択したもの: アイテムトグルタイプ" + pitemlistController._toggle_type2 + " リストID: " + pitemlistController.kettei_item2);
            }

    }

    void update_ListSelect_3()
        {
            //三個目選択したものを選択できないようにする。
            if (pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggleitem_type == pitemlistController._toggle_type3 && pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggle_originplist_ID == pitemlistController.kettei_item3)
            {
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false; //三個目も選択できないようにする
                //Debug.Log("三個目選択したもの: アイテムトグルタイプ" + pitemlistController._toggle_type3 + " リストID: " + pitemlistController.kettei_item3);
            }

    }
}
