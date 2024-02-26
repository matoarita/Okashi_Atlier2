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

    private int check_itemListType;
    private int check_item_Hyouji;
    private string check_itemName;
    private string check_itemType;
    private string check_itemType_sub;
    private string check_itemType_subB;
    private string check_itemType_sub_category;
    

    //各プレファブ共通で、変更できる値が必要。そのパラメータは、PlayerItemListControllerで管理する。
    public int _count1; //表示されているリスト中の選択番号 1
    public int _count2; //表示されているリスト中の選択番号 2
    public int _count3; //表示されているリスト中の選択番号 3
    public int _base_count;

    public List<int> _listcount = new List<int>(); //納品時用の選択番号リスト型

    public int _toggle_type1; //選択したアイテムが、店売りかオリジナルかの判定用。アイテムごとに３つ。
    public int _toggle_type2;
    public int _toggle_type3;
    public int _base_toggle_type;

    public int kettei_item1; //アイテムの配列番号。店売りアイテムの場合は、アイテムIDがそのまま入る。オリジナルplistアイテムの場合は、リストの番号が入っている。
    public int kettei_item2;
    public int kettei_item3;
    public int base_kettei_item; //トッピング調合時のベースアイテム。プレイヤーリストを選択している番号

    //public int kettei1_bunki; //調合どこまで選択したか、のステータス。0=なにも選択なし, 1=一個目を選択, 2=二個目を選択
    public bool kettei1_on;

    public int final_kettei_item1; //最終的に確定したアイテムのID（アイテムデータベースのIDを同一）
    public int final_kettei_item2;
    public int final_kettei_item3;
    public int final_base_kettei_item; //トッピング調合時の最終決定ベースアイテム。アイテムデータベースのIDと同一。

    public int result_item;
    public int result_compID; //最終的に確定したときの、コンポDBのアイテムID

    public int final_kettei_kosu1;
    public int final_kettei_kosu2;
    public int final_kettei_kosu3;
    public int final_base_kettei_kosu;

    public List<int> _listkosu = new List<int>(); //納品時用の個数リスト型

    private GameObject yes_button;
    private GameObject no_button;

    private bool SceneStartSetting_LoadOK;

    public bool shopsell_final_select_flag;

    void Awake() //Startより手前で先に読みこんで、OnEnableの挙動のエラー回避
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //キーマネージャー取得
        keymanager = keyManager.Instance.GetComponent<keyManager>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = GameObject.FindWithTag("PlayerItemListContent");
        textPrefab = (GameObject)Resources.Load("Prefabs/itemSelectToggle");

        SceneStartSetting_LoadOK = false;

        InitSetUp();
    }

    // Use this for initialization
    void Start()
    {
        
    }

    void InitSetUp()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //表示中リストの、選択した番号も保存
        _count1 = 9999;
        _count2 = 9999;
        _count3 = 9999;
        _base_count = 9999;

        //決定したアイテムが、店売りか、オリジナルかの判定用変数。
        _toggle_type1 = 0;
        _toggle_type2 = 0;
        _toggle_type3 = 0;
        _base_toggle_type = 0;

        //選択したアイテムの並び番号を格納しておく変数。(店売り 0, 1, 2..., オリジナル 0, 1, 2.. ) といった感じ。店売りアイテムは、アイテムIDと並びは一緒になる。
        kettei_item1 = 0;
        kettei_item2 = 0;
        kettei_item3 = 0;
        base_kettei_item = 0;

        GameMgr.Comp_kettei_bunki = 0;
        kettei1_on = false;

        //選んだアイテムのアイテムIDが入る。（店売り、オリジナル関係なし）
        final_kettei_item1 = 9999;
        final_kettei_item2 = 9999;
        final_kettei_item3 = 9999; //9999 = empty
        final_base_kettei_item = 9999;

        final_kettei_kosu1 = 1;
        final_kettei_kosu2 = 1;
        final_kettei_kosu3 = 1;
        final_base_kettei_kosu = 1;

        result_item = 0;

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

        ResetKettei_item();

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
                reset_and_DrawView();
            }

            OpenAnim();
        }
        else
        {
            if (SceneStartSetting_LoadOK) //シーン読み込み完了後は、以下の処理が動くようになる。初期化の際は無視する。
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

                        //アニメーション
                        /*if (GameMgr.compound_select == 99) //持ち物ひらいたときのデフォ位置
                        {
                            this.transform.localPosition = new Vector3(224f, 57f, 0);
                            OpenAnim2();
                        }
                        else
                        {
                            this.transform.localPosition = new Vector3(-224f, 57f, 0);
                            OpenAnim();
                        }*/

                        reset_and_DrawView();
                        OpenAnim();
                        break;

                    case 20:

                        switch (GameMgr.Scene_Select)
                        {
                            case 3: //納品時にアイテムを選択するときの処理

                                yes_button.SetActive(false);
                                no_button.SetActive(false);
                                reset_and_DrawView();

                                break;

                            case 5: //売るとき

                                yes_button.SetActive(false);
                                no_button.SetActive(false);
                                this.transform.localPosition = new Vector3(-180, 63, 0);
                                reset_and_DrawView();
                                break;

                            case 6: //あげるとき

                                yes_button.SetActive(false);
                                no_button.SetActive(true);
                                reset_and_DrawView();

                                break;
                        }

                        OpenAnim();
                        break;

                    case 30:

                        switch (GameMgr.Scene_Select)
                        {
                            case 3: //納品時にアイテムを選択するときの処理

                                yes_button.SetActive(false);
                                no_button.SetActive(false);
                                reset_and_DrawView();

                                break;

                            case 6: //あげるとき

                                yes_button.SetActive(false);
                                no_button.SetActive(true);
                                reset_and_DrawView();

                                break;
                        }

                        OpenAnim();
                        break;

                    default:

                        reset_and_DrawView();
                        OpenAnim();
                        break;
                }
            }
        }

        //開いたときは、必ず、全てのアイテムは未選択の状態にする。
        ResetAllItemSelected();
    }

    void OpenAnim()
    {
        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。

        sequence.Append(this.transform.DOLocalMove(new Vector3(50f, 0f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。
        sequence.Join(this.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    void OpenAnim2()
    {
        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOLocalMove(new Vector3(50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。

        sequence.Append(this.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。
        sequence.Join(this.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    public void ResetKettei_item()
    {
        _count1 = 9999;
        _count2 = 9999;
        _count3 = 9999;
        _base_count = 9999;

        kettei_item1 = 9999;
        kettei_item2 = 9999;
        kettei_item3 = 9999;
        base_kettei_item = 9999;

        //9999 = empty
        final_kettei_item1 = 9999;
        final_kettei_item2 = 9999;
        final_kettei_item3 = 9999;
        final_base_kettei_item = 9999;
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
                            else if (check_itemType_sub == "Source")
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

                        switch (GameMgr.UseMagicSkill)
                        {
                            case "Freezing_Spell":

                                if (check_itemType_sub == "Cookie" || 
                                    check_itemType_subB == "a_AppaleilChocolate" || check_itemType_subB == "a_AppaleilChocolateTwister")
                                {
                                    itemlist_hyouji_Check();
                                }
                                break;

                            case "Luminous_Suger":

                                if (check_itemType_sub == "Suger")
                                {
                                    itemlist_hyouji_Check();
                                }
                                break;

                            case "Luminous_Fruits":

                                if (check_itemType_sub == "Fruits")
                                {
                                    itemlist_hyouji_Check();
                                }
                                break;

                            case "Bake_Beans":

                                if (check_itemType_subB == "a_Cacao" || check_itemType_subB == "a_CoffeeBeans")
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

                            case "Wind_Twister":

                                if (check_itemType_sub == "Water" || check_itemType_sub == "Milk" ||
                                    check_itemName == "appaleil_chocolate" || check_itemType_subB == "a_AppaleiliceCream")
                                {
                                    itemlist_hyouji_Check();
                                }
                                break;

                            default: //例外処理　通常ここを通ることはない
                                
                                break;
                        }
                        

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
                else if (GameMgr.Scene_Category_Num == 20) //納品時にリストを開くとき
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
                                    check_itemType_sub == "Rare" || check_itemType_sub == "Equip")
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
                if (check_itemType == "Potion" || check_itemType_sub == "Potion" ||
                    check_itemType_sub == "Fruits" || check_itemType_sub == "Berry" ||
                    check_itemType_sub == "Nuts" || check_itemType_sub == "IceCream" ||
                    check_itemType_sub_category == "Potion")
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

        _toggle_itemID.toggleitem_ID = i; //アイテムIDを、リストビューのトグル自体にも記録させておく。 
        _toggle_itemID.toggleitem_type = 0; //プレイヤーアイテムリストを識別するための番号。0を入れる。
        _toggle_itemID.toggle_originplist_ID = i; //店売りアイテムのアイテムリスト番号
        //Debug.Log("プレイヤ店売りアイテムリストID: " + _toggle_itemID.toggle_originplist_ID);


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

    //初期化生成が終わったあとに呼ぶメソッド
    public void OnInitAfterSetting()
    {
        //店売りアイテムは先に生成し、ゲーム中ではONOFFのみにする。

        SceneStartSetting_LoadOK = true;
    }
}
