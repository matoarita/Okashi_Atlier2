using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

//プレイヤーアイテムリストのスクロールビューのコントローラー。
//調合シーンや、アイテムを女の子にあげたときの処理は、「itemSelectToggle」スクリプトに記述してます。

public class PlayerItemListController : SingletonMonoBehaviour<PlayerItemListController>
{
    private GameObject canvas;

    private keyManager keymanager;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private MagicSkillListDataBase magicskill_database;

    private SlotNameDataBase slotnamedatabase;
    private string[] _slot = new string[10]; //とりあえず、スロットの数の設定用。
    private string[] _slotHyouji1 = new string[10]; //日本語に変換後の表記を格納する。

    private PlayerItemList pitemlist;

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数

    public List<GameObject> _prelistitem = new List<GameObject>(); //リストビューの個数　表示用に、事前に格納しておくリスト。
    public List<GameObject> _listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text[] _text = new Text[2];
    private itemSelectToggle _toggle_itemID;

    private Sprite texture2d;
    private Image _Img;

    private string item_name;
    private int item_kosu;

    private int max;
    private int max_original;
    private int count;
    private int i, n;
    private int _lv;

    private int check_itemListType;
    private int check_item_Hyouji;
    private string check_itemName;
    private string check_itemType;
    private string check_itemType_sub;
    private string check_itemType_subB;
    private string check_itemType_sub_category;
    private int check_attribute1;
    

    public List<int> _listcount = new List<int>(); //納品時用の選択番号リスト型

    public bool kettei1_on;

    //public int result_item;
    //public int result_compID; //最終的に確定したときの、コンポDBのアイテムID

    public List<int> _listkosu = new List<int>(); //納品時用の個数リスト型

    private GameObject yes_button;
    private GameObject no_button;

    public bool shopsell_final_select_flag;

    // Use this for initialization
    void Start()
    {
        
    }

    void InitSetUp()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //キーマネージャー取得
        keymanager = keyManager.Instance.GetComponent<keyManager>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = GameObject.FindWithTag("PlayerItemListContent");
        textPrefab = (GameObject)Resources.Load("Prefabs/itemSelectToggle");

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        GameMgr.Comp_kettei_bunki = 0;
        kettei1_on = false;       

        i = 0;

        shopsell_final_select_flag = false;

        _listitem.Clear();
        _prelistitem.Clear();

    }

    // Update is called once per frame
    void Update()
    {
        /* アイテムの増減処理は、Exp_Controllerで行っている。*/
   
    }

    void OnEnable()
    {

        //ウィンドウがアクティヴになった瞬間だけ読み出される
        //Debug.Log("OnEnable");   

        InitSetUp();


        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();
        yes_selectitem_kettei.onclick = false;

        yes_button = this.transform.Find("Yes").gameObject;
        no_button = this.transform.Find("No").gameObject;

        keymanager.cursor_cullent_num = 0;
        keymanager.itemCursor_On = false;

        no_button.SetActive(true);


        // 調合専用シーンでやりたい処理。
        if (GameMgr.CompoundSceneStartON)
        {

            if (GameMgr.tutorial_ON == true)
            {
                no_button.SetActive(false);
            }
            else
            {
                no_button.SetActive(true);
            }

            // トッピング調合を選択した場合の処理
            if (GameMgr.compound_select == 2)
            {
                if (GameMgr.Comp_kettei_bunki == 0)
                {
                    topping_DrawView_1();
                }
                else
                {
                    topping_DrawView_2();
                }
            }
            else //トッピング調合以外
            {
                ResetViewCheck();
            }

        }
        else
        {

            switch (GameMgr.Scene_Category_Num)
            {
                case 10: // 調合シーン以外でやりたい処理。それ以外のシーンでは、この中身の処理は無視。

                    if (GameMgr.tutorial_ON == true)
                    {
                        no_button.SetActive(false);
                    }
                    else
                    {
                        no_button.SetActive(true);
                    }

                    ResetViewCheck();
                    break;

                case 20:

                    switch (GameMgr.Scene_Select)
                    {
                        case 3: //納品時にアイテムを選択するときの処理

                            yes_button.SetActive(false);
                            no_button.SetActive(false);
                            ResetViewCheck();

                            break;

                        case 5: //売るとき

                            yes_button.SetActive(false);
                            no_button.SetActive(true);
                            this.transform.localPosition = new Vector3(-180, 63, 0);
                            ResetViewCheck();
                            break;

                        case 6: //あげるとき

                            yes_button.SetActive(false);
                            no_button.SetActive(true);
                            ResetViewCheck();

                            break;
                    }

                    break;

                case 30:

                    switch (GameMgr.Scene_Select)
                    {
                        case 3: //納品時にアイテムを選択するときの処理

                            yes_button.SetActive(false);
                            no_button.SetActive(false);
                            ResetViewCheck();

                            break;

                        case 6: //あげるとき

                            yes_button.SetActive(false);
                            no_button.SetActive(true);
                            ResetViewCheck();

                            break;
                    }

                    break;

                default:

                    ResetViewCheck();
                    break;
            }

        }


        //開いたときは、必ず、全てのアイテムは未選択の状態にする。
        ResetAllItemSelected();
        ResetKettei_item();
    }
    

    public void ResetKettei_item()
    {
        GameMgr.List_count1 = 9999;
        GameMgr.List_count2 = 9999;
        GameMgr.List_count3 = 9999;
        GameMgr.List_basecount = 9999;

        //9999 = empty
        GameMgr.Final_list_itemID1 = 9999;
        GameMgr.Final_list_itemID2 = 9999;
        GameMgr.Final_list_itemID3 = 9999;
        GameMgr.Final_list_baseitemID = 9999;

        GameMgr.Final_kettei_kosu1 = 1;
        GameMgr.Final_kettei_kosu2 = 1;
        GameMgr.Final_kettei_kosu3 = 1;
        GameMgr.Final_kettei_basekosu = 1;
    }

    void ResetAllItemSelected()
    {
        if (_listitem.Count > 0)
        {
            for (i = 0; i < _listitem.Count; i++)
            {
                _listitem[i].GetComponent<Toggle>().interactable = true;
                _listitem[i].GetComponent<Toggle>().isOn = false;
            }
        }
    }


    public void reset_and_DrawView_Topping()
    {
        if (GameMgr.Comp_kettei_bunki == 0)
        {
            topping_DrawView_1();
        }
        else
        {
            topping_DrawView_2();
        }
    }

    //リストビュー更新
    void ResetViewCheck()
    {
        reset_and_DrawView();
    }

    // リストビューの描画部分。重要。
    public void reset_and_DrawView()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _listitem.Clear();


        //まず、プレイヤーアイテムリストを、表示
        check_itemListType = 0;
        for (i = 0; i < pitemlist.playeritemlist.Count; i++)
        {
            check_item_Hyouji = database.items[i].item_Hyouji;
            check_itemName = database.items[i].itemName;
            check_itemType = database.items[i].itemType.ToString();
            check_itemType_sub = database.items[i].itemType_sub.ToString();
            check_itemType_sub_category = database.items[i].itemType_sub_category;
            check_itemType_subB = database.items[i].itemType_subB.ToString();
            check_attribute1 = database.items[i].Attribute1;

            if (pitemlist.playeritemlist[check_itemName] > 0) //持っている個数が1以上のアイテムのみ、表示。
            {
                Check_ListHyouji();
            }
        }

        //次に、オリジナルプレイヤーアイテムリストを、上記のリスト（_listitem）に追加していく。
        check_itemListType = 1;
        for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
        {
            check_item_Hyouji = pitemlist.player_originalitemlist[i].item_Hyouji;
            check_itemName = pitemlist.player_originalitemlist[i].itemName;
            check_itemType = pitemlist.player_originalitemlist[i].itemType.ToString();
            check_itemType_sub = pitemlist.player_originalitemlist[i].itemType_sub.ToString();
            check_itemType_sub_category = pitemlist.player_originalitemlist[i].itemType_sub_category;
            check_itemType_subB = pitemlist.player_originalitemlist[i].itemType_subB.ToString();
            check_attribute1 = pitemlist.player_originalitemlist[i].Attribute1;

            Check_ListHyouji();
        }

        //次に、お菓子パネルリストを、上記のリスト（_listitem）に追加していく。
        check_itemListType = 2;
        for (i = 0; i < pitemlist.player_extremepanel_itemlist.Count; i++)
        {
            check_item_Hyouji = pitemlist.player_extremepanel_itemlist[i].item_Hyouji;
            check_itemName = pitemlist.player_extremepanel_itemlist[i].itemName;
            check_itemType = pitemlist.player_extremepanel_itemlist[i].itemType.ToString();
            check_itemType_sub = pitemlist.player_extremepanel_itemlist[i].itemType_sub.ToString();
            check_itemType_sub_category = pitemlist.player_extremepanel_itemlist[i].itemType_sub_category;
            check_itemType_subB = pitemlist.player_extremepanel_itemlist[i].itemType_subB.ToString();
            check_attribute1 = pitemlist.player_extremepanel_itemlist[i].Attribute1;

            Check_ListHyouji();
        }
    }

    void Check_ListHyouji()
    {
        if (check_item_Hyouji > 0) //item_hyoujiが1のものを表示する。未使用アイテムなどは0にして表示しない。
        {
            // 調合専用シーンでやりたい処理。
            if (GameMgr.CompoundSceneStartON)
            {
                switch (GameMgr.compound_select) //さらに、調合シーンによって、アイテム種類ごとに表示／非表示を分ける。
                {
                    case 1: //レシピ調合のとき。「レシピリストコントローラー」で処理を行うため、このスクリプト上では無視される。

                        break;

                    case 2: //トッピング調合。お菓子と、トッピング系アイテムを表示し、ベース＝「お菓子」タイプのみ選択可能、その後、トッピングできるアイテムのみ選択可能。（フルーツ・ナッツ・チョコはOK。トッピング系アイテム。材料は×）                        

                        if (check_itemType == "Okashi" || check_itemType == "Potion" || check_itemType_sub_category == "Potion")
                        {
                            itemlist_hyouji_Check();
                        }

                        break;

                    case 3: //オリジナル調合。材料・生地などの素材アイテムのみ表示。

                        if (GameMgr.tutorial_ON == true)
                        {
                            if (check_itemName == "komugiko" || check_itemName == "butter" ||
                                check_itemName == "suger")
                            {
                                itemlist_hyouji_Check();
                            }
                        }
                        else
                        {
                            if (check_itemType == "Mat" || check_itemType_sub_category == "Mat")
                            {
                                itemlist_hyouji_Check();
                            }
                            else if (check_itemType_sub == "Source" || check_itemType_sub == "GlowFruits")
                            {
                                itemlist_hyouji_Check();
                            }
                            else if (check_itemType_sub == "Garbage" || check_itemType_sub == "Machine")
                            {
                                itemlist_hyouji_Check();
                            }
                        }
                        break;

                    case 5: //焼くとき。アイテムタイプサブが「生地」のみ表示。

                        if (check_itemType_sub == "Pate" || check_itemType_sub == "Cookie_base" ||
                            check_itemType_sub == "Pie_base" || check_itemType_sub == "Chocorate_base" || check_itemType_sub == "Cake_base")
                        {
                            itemlist_hyouji_Check();
                        }
                        break;

                    case 7: //ヒカリに作らせる。材料・生地などの素材アイテムのみ表示。

                        if (check_itemType == "Mat" || check_itemType == "Okashi" || check_itemType_sub_category == "Mat")
                        {
                            itemlist_hyouji_Check();
                        }
                        else if (check_itemType_sub == "Source" )
                        {
                            itemlist_hyouji_Check();
                        }
                        else if (check_itemType_sub == "Garbage" || check_itemType_sub == "Machine")
                        {
                            itemlist_hyouji_Check();
                        }
                        else if (check_itemType == "Potion" || check_itemType_sub_category == "Potion")
                        {
                            itemlist_hyouji_Check();
                        }

                        break;

                    case 21: //魔法使用時のアイテムリスト　魔法に応じて表示を切り替える                    

                        MagicItemListHyouji();
                                              

                        break;
                }
            }
            else
            {
                if (GameMgr.Scene_Category_Num == 10)
                {
                    switch (GameMgr.compound_select) //さらに、調合シーンによって、アイテム種類ごとに表示／非表示を分ける。
                    {                       
                        case 10: //お菓子をあげるとき。アイテムタイプが「お菓子」のみ表示

                            if (check_itemType == "Okashi")
                            {
                                itemlist_hyouji_Check();
                            }
                            break;

                        case 99: //メニュー画面を開いたとき

                            itemlist_hyouji_Check();
                            break;

                        case 1000: //イベント


                            if (check_itemType == "Okashi")
                            {
                                itemlist_hyouji_Check();
                            }
                            break;

                        default:

                            itemlist_hyouji_Check();
                            break;
                    }
                }
                else if (GameMgr.Scene_Category_Num == 20) //お店でリストを開くとき
                {
                    switch (GameMgr.Scene_Select)
                    {
                        case 3:

                            //お菓子のみ表示
                            if (check_itemType == "Okashi")
                            {
                                itemlist_hyouji_Check();
                            }
                            break;

                        case 5: //売るとき

                            //フルーツかレアアイテムを表示
                            if (check_itemType == "Mat" || check_itemType == "Potion" || check_itemType == "Okashi" ||
                                    check_itemType_sub == "Rare" || check_itemType_sub == "Equip" || check_itemType_sub == "Garbage" || check_itemType_sub == "Object")
                            {

                                itemlist_hyouji_Check();
                            }
                            break;

                        case 6:

                            //お菓子のみ表示
                            if (check_itemType == "Okashi")
                            {
                                itemlist_hyouji_Check();
                            }
                            break;

                        default:

                            itemlist_hyouji_Check();
                            break;
                    }
                }
                else if (GameMgr.Scene_Category_Num == 30) //納品時にリストを開くとき
                {
                    switch (GameMgr.Scene_Select)
                    {
                        case 3:

                            //お菓子のみ表示
                            if (check_itemType == "Okashi")
                            {
                                itemlist_hyouji_Check();
                            }
                            break;

                        case 5:

                            //フルーツかレアアイテムを表示
                            if (check_itemType == "Mat" || check_itemType_sub == "Rare")
                            {

                                itemlist_hyouji_Check();
                            }
                            break;

                        case 6:

                            //お菓子のみ表示
                            if (check_itemType == "Okashi")
                            {
                                itemlist_hyouji_Check();
                            }
                            break;

                        default:

                            itemlist_hyouji_Check();
                            break;
                    }
                }
                else
                {
                    if (check_itemType == "Okashi")
                    {
                        itemlist_hyouji_Check();
                    }
                }
            }
        }
    }

    //トッピング調合の時のみ使用。ベースアイテムを決める時。
    public void topping_DrawView_1()
    {  

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _listitem.Clear();

        //まず、プレイヤーアイテムリストを、表示
        check_itemListType = 0;
        for (i = 0; i < pitemlist.playeritemlist.Count; i++)
        {
            if (database.items[i].item_Hyouji > 0)
            {

                if (pitemlist.playeritemlist[database.items[i].itemName] > 0) //持っている個数が1以上のアイテムのみ、表示。
                {

                    //お菓子タイプのみ表示
                    if (database.items[i].itemType.ToString() == "Okashi")
                    {
                        itemlist_hyouji_Check();
                    }

                }
            }
        }

        //次に、オリジナルプレイヤーアイテムリストを、上記のリスト（_listitem）に追加していく。
        check_itemListType = 1;
        for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
        {
            if (pitemlist.player_originalitemlist[i].item_Hyouji > 0)
            {
                //トッピング調合の時のみ使用。

                //お菓子タイプのみ表示
                if (pitemlist.player_originalitemlist[i].itemType.ToString() == "Okashi")
                {
                    itemlist_hyouji_Check();
                }
            }
        }

        //次に、お菓子パネルアイテムリストを、上記のリスト（_listitem）に追加していく。
        check_itemListType = 2;
        for (i = 0; i < pitemlist.player_extremepanel_itemlist.Count; i++)
        {
            if (pitemlist.player_extremepanel_itemlist[i].item_Hyouji > 0)
            {
                //トッピング調合の時のみ使用。

                //お菓子タイプのみ表示
                if (pitemlist.player_extremepanel_itemlist[i].itemType.ToString() == "Okashi")
                {
                    itemlist_hyouji_Check();
                }

            }
        }
    }


    
    //トッピング調合の時のみ使用する。
    public void topping_DrawView_2()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _listitem.Clear();

        //まず、プレイヤーアイテムリストを、表示
        check_itemListType = 0;          
        for (i = 0; i < pitemlist.playeritemlist.Count; i++)
        {
            check_item_Hyouji = database.items[i].item_Hyouji;
            check_itemName = database.items[i].itemName;
            check_itemType = database.items[i].itemType.ToString();
            check_itemType_sub = database.items[i].itemType_sub.ToString();
            check_itemType_sub_category = database.items[i].itemType_sub_category;
            check_itemType_subB = database.items[i].itemType_subB.ToString();
            check_attribute1 = database.items[i].Attribute1;

            if (pitemlist.playeritemlist[check_itemName] > 0) //持っている個数が1以上のアイテムのみ、表示。
            {
                Check_ListToppingHyouji();
            }
        }

        //次に、オリジナルプレイヤーアイテムリストを、上記のリスト（_listitem）に追加していく。
        check_itemListType = 1;
        for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
        {
            check_item_Hyouji = pitemlist.player_originalitemlist[i].item_Hyouji;
            check_itemName = pitemlist.player_originalitemlist[i].itemName;
            check_itemType = pitemlist.player_originalitemlist[i].itemType.ToString();
            check_itemType_sub = pitemlist.player_originalitemlist[i].itemType_sub.ToString();
            check_itemType_sub_category = pitemlist.player_originalitemlist[i].itemType_sub_category;
            check_itemType_subB = pitemlist.player_originalitemlist[i].itemType_subB.ToString();
            check_attribute1 = pitemlist.player_originalitemlist[i].Attribute1;

            //Debug.Log("check_itemName check_itemType check_item_Hyouji: " + check_itemName + " " + check_itemType + " " + check_item_Hyouji);
            Check_ListToppingHyouji();
        }

        //次に、お菓子パネルリストを、上記のリスト（_listitem）に追加していく。
        check_itemListType = 2;
        for (i = 0; i < pitemlist.player_extremepanel_itemlist.Count; i++)
        {
            check_item_Hyouji = pitemlist.player_extremepanel_itemlist[i].item_Hyouji;
            check_itemName = pitemlist.player_extremepanel_itemlist[i].itemName;
            check_itemType = pitemlist.player_extremepanel_itemlist[i].itemType.ToString();
            check_itemType_sub = pitemlist.player_extremepanel_itemlist[i].itemType_sub.ToString();
            check_itemType_sub_category = pitemlist.player_extremepanel_itemlist[i].itemType_sub_category;
            check_itemType_subB = pitemlist.player_extremepanel_itemlist[i].itemType_subB.ToString();
            check_attribute1 = pitemlist.player_extremepanel_itemlist[i].Attribute1;

            Check_ListToppingHyouji();
        }
    }

    void Check_ListToppingHyouji()
    {
        if (check_item_Hyouji > 0)
        {
            if (GameMgr.tutorial_ON == true)
            {
                //チュートリアル時は、とりあえずオレンジだけ表示
                if (check_itemName == "orange")
                {
                    itemlist_hyouji_Check();
                }
            }
            else
            {
                //トッピング材料（ポーションかフルーツ・ナッツ系など）のみ表示
                if (check_itemType == "Potion" || check_itemType_sub_category == "Potion" ||
                    check_itemType_sub == "Fruits" || check_itemType_sub == "Berry" || check_itemType_sub == "Harb" ||
                    check_itemType_sub == "Nuts" || check_itemType_sub == "IceCream"
                    )
                {
                    itemlist_hyouji_Check();
                }
            }
        }
    }

    void itemlist_hyouji_Check()
    {
        switch(check_itemListType)
        {
            case 0: //アイテムリスト

                itemlist_hyouji();
                break;

            case 1: //プレイヤーオリジナルアイテムリスト

                original_itemlist_hyouji();
                break;

            case 2: //エクストリームパネルアイテムリスト

                extreme_itemlist_hyouji();
                break;
        }
    }

    //リストにアイテム名（デフォルトアイテム）を表示する処理
    void itemlist_hyouji()
    {
        //Debug.Log(i);
        _listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。
        _Img = _listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ

        _toggle_itemID = _listitem[list_count].GetComponent<itemSelectToggle>();

        _toggle_itemID.toggleitem_ID = database.items[i].itemID; //アイテムIDを、リストビューのトグル自体にも記録させておく。 
        _toggle_itemID.toggleitem_type = 0; //プレイヤーアイテムリストを識別するための番号。0を入れる。
        _toggle_itemID.toggle_originplist_ID = i; //店売りアイテムのアイテムリスト番号
        //Debug.Log("プレイヤ店売りリスト配列番号: " + _toggle_itemID.toggle_originplist_ID);


        item_name = database.items[i].itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

        _text[0].text = item_name;

        item_kosu = pitemlist.playeritemlist[database.items[i].itemName];

        _text[1].text = item_kosu.ToString(); //プレイヤーがそのアイテムをもっている個数

        //画像を変更
        texture2d = database.items[i].itemIcon_sprite;
        _Img.sprite = texture2d;

        ++list_count;
    }


    //リストにアイテム名（作ったアイテム）を表示する処理
    void original_itemlist_hyouji()
    {
        _listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。
        _Img = _listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ

        _toggle_itemID = _listitem[list_count].GetComponent<itemSelectToggle>();

        _toggle_itemID.toggleitem_ID = pitemlist.player_originalitemlist[i].itemID; //アイテムIDを、リストビューのトグル自体にも記録させておく。
        _toggle_itemID.toggleitem_type = 1; //プレイヤーアイテムリストを識別するための番号。オリジナルアイテムの場合、1を入れる。
        _toggle_itemID.toggle_originplist_ID = i; //オリジナルアイテムリストのリスト番号
        //Debug.Log("プレイヤオリジナルアイテムリストID: " + _toggle_itemID.toggle_originplist_ID + " " + "アイテムID: " + _toggle_itemID.toggleitem_ID);


        //アイテム名の表示
        item_name = pitemlist.player_originalitemlist[i].itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

        _text[0].text = item_name;

        item_kosu = pitemlist.player_originalitemlist[i].ItemKosu;

        _text[1].text = item_kosu.ToString(); //プレイヤーがそのアイテムをもっている個数
        _text[1].color = new Color(255f / 255f, 234f / 255f, 64f / 255f);
        //_listitem[list_count].transform.Find("Background/Item_count").GetComponent<>().

        //Debug.Log("Original: " + i + "　ItemID" + _toggle_itemID.toggleitem_ID + " アイテム名: " + item_name);
        //Debug.Log("Original: " + i + "　Item固有ID" + pitemlist.player_originalitemlist[i].OriginalitemID + " アイテム名: " + item_name);

        //画像を変更
        texture2d = pitemlist.player_originalitemlist[i].itemIcon_sprite;
        _Img.sprite = texture2d;

        ++list_count;
    }


    //リストにアイテム名（作ったアイテム）を表示する処理
    void extreme_itemlist_hyouji()
    {
        _listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。
        _Img = _listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ

        _toggle_itemID = _listitem[list_count].GetComponent<itemSelectToggle>();

        _toggle_itemID.toggleitem_ID = pitemlist.player_extremepanel_itemlist[i].itemID; //アイテムIDを、リストビューのトグル自体にも記録させておく。
        _toggle_itemID.toggleitem_type = 2; //プレイヤーアイテムリストを識別するための番号。オリジナルアイテムの場合、1を入れる。
        _toggle_itemID.toggle_originplist_ID = i; //オリジナルアイテムリストのリスト番号
        //Debug.Log("プレイヤオリジナルアイテムリストID: " + _toggle_itemID.toggle_originplist_ID + " " + "アイテムID: " + _toggle_itemID.toggleitem_ID);


        //アイテム名の表示
        item_name = pitemlist.player_extremepanel_itemlist[i].itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

        _text[0].text = item_name;

        item_kosu = pitemlist.player_extremepanel_itemlist[i].ItemKosu;

        _text[1].text = item_kosu.ToString(); //プレイヤーがそのアイテムをもっている個数
        _text[1].color = new Color(255f / 255f, 234f / 255f, 64f / 255f);
        //_listitem[list_count].transform.Find("Background/Item_count").GetComponent<>().

        //Debug.Log("Original: " + i + "　ItemID" + _toggle_itemID.toggleitem_ID + " アイテム名: " + item_name);
        //Debug.Log("Extreme: " + i + "　Item固有ID: " + pitemlist.player_extremepanel_itemlist[i].OriginalitemID + " アイテム名: " + item_name);

        //画像を変更
        texture2d = pitemlist.player_extremepanel_itemlist[i].itemIcon_sprite;
        _Img.sprite = texture2d;

        ++list_count;
    }



    //
    //アイテムリストを表示中に、アイテムを追加した場合、リアルタイムに表示を更新する
    //
    public void AddItemList()
    {
          
        if (GameMgr.CompoundSceneStartON) // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {
            // トッピング調合を選択した場合の処理
            if (GameMgr.compound_select == 2)
            {
                if (GameMgr.Comp_kettei_bunki == 0)
                {
                    topping_DrawView_1();
                }
                else
                {
                    topping_DrawView_2();
                }
            }
            else
            {
                reset_and_DrawView();
            }
        }
        else
        {
            reset_and_DrawView();
        }
    }

    void MagicItemListHyouji()
    {

        switch (GameMgr.UseMagicSkill)
        {
            case "Caramelized":

                if (check_itemType_subB == "a_SourceSyrup")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Bake_Beans":

                if (check_itemType_subB == "a_Cacao" || check_itemType_subB == "a_CoffeeBeans" || check_itemType_subB == "a_Maron")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Removing_Shells":

                if (check_itemType_subB == "a_CacaoRoasted")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Chocolate_Tempering":

                if (check_itemType_subB == "a_CacaoMass")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Cookie_SecondBake":

                if (check_itemType_sub == "Cookie" || check_itemType_sub == "Cookie_Hard" || check_itemType_sub == "Cookie_Mat" ||
                    check_itemType_sub == "Bread" || check_itemType_sub == "Biscotti" || check_itemType_sub == "Financier" || 
                    check_itemType_sub == "Maffin" || check_itemType_sub == "Rusk")
                {
                    if (check_attribute1 == 0) //まだ二度焼きしてないやつだけ
                    {
                        itemlist_hyouji_Check();
                    }
                }
                break;

            case "Freezing_Spell":

                _lv = magicskill_database.skillName_SearchLearnLevel("Freezing_Spell");

                if (_lv >= 1) //1のときはアイス水溶液のみ
                {
                    if (check_itemType_subB == "a_AppaleiliceCream")
                    {
                        itemlist_hyouji_Check();
                    }
                }
                if (_lv >= 2) //水・ミルク系全般
                {
                    if (check_itemType_subB == "a_AppaleilChocolate" || check_itemType_subB == "a_AppaleilChocolateTwister" ||
                        check_itemType_subB == "a_AppaleilChocolateBar" || check_itemType_subB == "a_AppaleilChocolateTwisterHeart" ||
                        check_itemType_subB == "a_AppaleilChocolateCrown" ||
                        check_itemType_subB == "a_AppaleilJelly" ||
                        check_itemType_sub == "Water" || check_itemType_sub == "Milk")
                    {
                        itemlist_hyouji_Check();
                    }
                }
                if (_lv >= 3) //くだもの
                {
                    if (check_itemType_sub == "Fruits" || check_itemType_sub == "Berry" || check_itemType_sub == "Harb")
                    {
                        itemlist_hyouji_Check();
                    }
                }
                if (_lv >= 4) //おはな
                {
                    if (check_itemType_sub == "Flower")
                    {
                        itemlist_hyouji_Check();
                    }
                }
                if (_lv >= 5) //お菓子全て
                {
                    if (check_itemType == "Okashi")
                    {
                        if (check_itemType_sub != "Tea" && check_itemType_sub != "Coffee" && check_itemType_sub != "Bread")
                        {
                            itemlist_hyouji_Check();
                        }
                    }
                }

                break;

            case "Freezing_OverRun":

                if (check_itemType_subB == "a_AppaleiliceCream")
                {
                        itemlist_hyouji_Check();
                }
                break;

            case "Ice_Cube":

                if (check_itemType_sub == "Water")
                {
                    if (check_itemType_sub_category == "Non") //ツイスターや加工されたものは対象外
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "SugerPot":

                if (check_itemType_sub == "Water" || check_itemType_sub == "Milk" || check_itemType_subB == "a_AromaPotion")
                {
                    if (check_itemType_sub_category == "Non") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Luminous_Suger":

                if (check_itemType_sub == "Suger")
                {
                    if (check_itemType_subB != "a_SugerSimple") //シンプルな砂糖は光らせれない
                    {

                        if (check_itemType_sub_category != "Glow") //一回グローされたものはもうグローできない
                        {
                            itemlist_hyouji_Check();
                        }
                    }
                }
                break;

            case "Luminous_Fruits":

                if (check_itemType_sub == "Fruits" || check_itemType_sub == "Berry" || check_itemType_sub == "Harb")
                {
                    if (check_itemType_sub_category != "Glow") //一回グローされたものはもうグローできない
                    {
                        itemlist_hyouji_Check();
                    }
                }
                break;

            case "Buttelfy_illumination":

                if (check_itemType_sub == "Cake")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Aroma_Potion":

                if (check_itemType_sub == "Flower" || check_itemType_subB == "a_Sakura")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Wind_Ark":

                if (check_itemType_sub == "Water" || check_itemType_sub == "Milk" ||
                    check_itemType_subB == "a_AppaleilChocolate" || check_itemType_subB == "a_AppaleiliceCream")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Wind_Twister":

                if (check_itemType_sub == "Water" || check_itemType_sub == "Milk" ||
                    check_itemType_subB == "a_AppaleilChocolate" || check_itemType_subB == "a_AppaleiliceCream")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Wind_Heart":

                if (check_itemType_subB == "a_AppaleilChocolate")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Wind_FlatBar":

                if (check_itemType_subB == "a_AppaleilChocolate")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Wind_Crown":

                if (check_itemType_subB == "a_AppaleilChocolate")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Wind_Roll":

                if (check_itemName == "langue_de_chat")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Wind_Pen":

                if (check_itemType_subB == "a_AppaleilChocolate")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Float_Material":

                if (check_itemType_sub == "Suger" || 
                    check_itemType_sub == "Fruits" || check_itemType_sub == "GlowFruits"
                    || check_itemType_sub == "Berry")
                {
                    if (check_itemType_subB != "a_SugerSimple") //基本の砂糖などは外す
                    {
                        itemlist_hyouji_Check();
                    }
                }
                break;

            case "Bubble_Mist":

                if (check_itemType_subB == "a_AromaPotion")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Statue_of_Penguin":

                if (check_itemType_sub == "a_AppaleilMizuame" ||
                    check_itemType_subB == "a_AppaleilChocolate")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Statue_of_Bear":

                if (check_itemType_sub == "a_AppaleilMizuame" ||
                    check_itemType_subB == "a_AppaleilChocolate")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Statue_of_Cat":

                if (check_itemType_sub == "a_AppaleilMizuame" ||
                    check_itemType_subB == "a_AppaleilChocolate")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Statue_of_Rabitts":

                if (check_itemType_sub == "a_AppaleilMizuame" ||
                    check_itemType_subB == "a_AppaleilChocolate")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Statue_of_AngelWing":

                if (check_itemType_sub == "a_AppaleilMizuame" ||
                    check_itemType_subB == "a_AppaleilChocolate")
                {
                    if (check_itemType_sub_category != "Twister") //ツイスターや加工されたものはもうツイストできない
                    {
                        itemlist_hyouji_Check();
                    }

                }
                break;

            case "Star_Blessing":

                if (check_itemType_sub == "Juice" || check_itemType_sub == "Tea" || check_itemType_sub == "Soda")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Latte_Art":

                if (check_itemType_subB == "a_Cafelatte")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Moonlight_Banana":

                if (check_itemType_subB == "a_Banana")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Magic_Soda":

                if (check_itemType_sub == "Soda")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Rainbow_Rain":

                if (check_itemType_sub == "Soda" || check_itemType_sub == "Berry")
                {
                    itemlist_hyouji_Check();
                }
                break;


            case "Warming_Handmade":

                if (check_itemType == "Okashi")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "Life_Stream":

                if (check_itemType == "Okashi")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "AbraCadabra":

                if (check_itemType == "Okashi")
                {
                    itemlist_hyouji_Check();
                }
                break;

            case "True_of_Myheart":

                if (check_itemType == "Okashi")
                {
                    itemlist_hyouji_Check();
                }
                break;


            default: //例外処理　通常ここを通ることはないが、上で未登録のスキルはここを通る

                break;
        }
    }


    //一時的に全てのアイテムを触れなくする。
    public void Offinteract()
    {
        for(i=0; i < _listitem.Count; i++)
        {
            _listitem[i].GetComponent<Toggle>().interactable = false;
        }
    }

    //全てのアイテムをONにする
    public void Oninteract()
    {
        for (i = 0; i < _listitem.Count; i++)
        {
            _listitem[i].GetComponent<Toggle>().interactable = true;
        }
    }

}
