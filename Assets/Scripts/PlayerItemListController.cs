using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//プレイヤーアイテムリストのスクロールビューのコントローラー。
//調合シーンや、アイテムを女の子にあげたときの処理は、「itemSelectToggle」スクリプトに記述してます。

public class PlayerItemListController : SingletonMonoBehaviour<PlayerItemListController>
{
    private GameObject canvas;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private SlotNameDataBase slotnamedatabase;
    private string[] _slot = new string[10]; //とりあえず、スロットの数の設定用。
    private string[] _slotHyouji1 = new string[10]; //日本語に変換後の表記を格納する。

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;

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

    public int kettei1_bunki; //調合どこまで選択したか、のステータス。0=なにも選択なし, 1=一個目を選択, 2=二個目を選択
    public bool kettei1_on;
    public bool final_select_flag;

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

    public bool extremepanel_on; //extremeパネルからのエクストリーム調合かどうか。

    private GameObject yes_button;
    private GameObject no_button;


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


        //スクロールビュー内の、コンテンツ要素を取得
        content = GameObject.FindWithTag("PlayerItemListContent");
        textPrefab = (GameObject)Resources.Load("Prefabs/itemSelectToggle");


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

        kettei1_bunki = 0;
        kettei1_on = false;
        final_select_flag = false;

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

        extremepanel_on = false;
    }

    // Use this for initialization
    void Start()
    {
        
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

        yes_button = this.transform.Find("Yes").gameObject;
        no_button = this.transform.Find("No").gameObject;

        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {
            if (compound_Main_obj != null) //ゲームのセットアップ時は無視
            {
                //キャンバスの読み込み
                canvas = GameObject.FindWithTag("Canvas");

                updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
                updown_counter = updown_counter_obj.GetComponent<Updown_counter>();

                //シーン移動などで、リセットされない場合があるので、念の為ここでリセット
                updown_counter_obj.SetActive(true);
                updown_counter.updown_kosu = 1;

                updown_counter_obj.SetActive(false);
            }

            ResetKettei_item();

            if (GameMgr.tutorial_ON == true)
            {
                no_button.SetActive(false);
            }
            else
            {
                no_button.SetActive(true);
            }

            if (extremepanel_on == true)
            {

            }
            else
            {
                kettei1_bunki = 0;
            }


            if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
            {

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                // トッピング調合を選択した場合の処理
                if (compound_Main.compound_select == 2)
                {
                    if (kettei1_bunki == 0)
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

            }
            else
            {
                reset_and_DrawView();
            }
        }

        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            //納品時にアイテムを選択するときの処理
            yes_button.SetActive(false);
            no_button.SetActive(false);
            reset_and_DrawView();
        }

    }

    public void reset_and_DrawView_Topping()
    {

        if (kettei1_bunki == 0)
        {
            topping_DrawView_1();
        }
        else
        {
            topping_DrawView_2();
        }
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

    public void AddItemList()
    {
        //アイテムリストを表示中に、アイテムを追加した場合、リアルタイムに表示を更新する      

        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {
            compound_Main_obj = GameObject.FindWithTag("Compound_Main");
            compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

            // トッピング調合を選択した場合の処理
            if (compound_Main.compound_select == 2)
            {
                if (kettei1_bunki == 0)
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

    // リストビューの描画部分。重要。
    public void reset_and_DrawView()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        max = pitemlist.playeritemlist.Count; //現在のプレイヤーアイテムリストの最大数を更新
        max_original = pitemlist.player_originalitemlist.Count; //現在のプレイヤーオリジナルアイテムリストの最大数を更新
        //Debug.Log("max: " + max);
        //Debug.Log("max_original: " + max_original);

        list_count = 0;
        _listitem.Clear();

        //まず、プレイヤーアイテムリストを、表示
        for (i = 0; i < max; i++)
        {
            if (database.items[i].item_Hyouji > 0) //item_hyoujiが1のものを表示する。未使用アイテムなどは0にして表示しない。
            {
                //Debug.Log("ID: " + i + "所持数: " + pitemlist.playeritemlist[i]);
                /*if (i > 500) //ID = 501以降、レシピ本などの特殊アイテムは表示しない。ゴミは表示する。
                {

                }
                else
                {*/

                    if (pitemlist.playeritemlist[i] > 0) //持っている個数が1以上のアイテムのみ、表示。
                    {

                        if (SceneManager.GetActiveScene().name == "Compound")
                        {
                            switch (compound_Main.compound_select) //さらに、調合シーンによって、アイテム種類ごとに表示／非表示を分ける。
                            {
                                case 1: //レシピ調合のとき。「レシピリストコントローラー」で処理を行うため、このスクリプト上では無視される。

                                    break;

                                case 2: //トッピング調合。ベース＝「お菓子」タイプのみ表示。その後、トッピングできるアイテムのみ表示。（フルーツ・ナッツ・チョコはOK。トッピング系アイテム。材料は×）

                                    if (database.items[i].itemType.ToString() == "Okashi" || database.items[i].itemType.ToString() == "Potion")
                                    {
                                        itemlist_hyouji();
                                    }

                                    break;

                                case 3: //オリジナル調合。材料・生地などの素材アイテムのみ表示。お菓子アイテムは表示しない。

                                    if (database.items[i].itemType_sub.ToString() == "Komugiko" || database.items[i].itemType_sub.ToString() == "Butter" || 
                                        database.items[i].itemType_sub.ToString() == "Suger" || database.items[i].itemType_sub.ToString() == "Egg" ||
                                        database.items[i].itemType_sub.ToString() == "Salt" ||
                                        database.items[i].itemType_sub.ToString() == "Source" || database.items[i].itemType_sub.ToString() == "Appaleil" ||
                                        database.items[i].itemType_sub.ToString() == "Cream" ||
                                        database.items[i].itemType_sub.ToString() == "Chocolate_Mat" || database.items[i].itemType_sub.ToString() == "IceCream" ||
                                        database.items[i].itemType_sub.ToString() == "Bread" || database.items[i].itemType_sub.ToString() == "Machine")
                                    {
                                        itemlist_hyouji();
                                    }
                                    break;

                                case 5: //焼くとき。アイテムタイプサブが「生地」のみ表示。

                                    if (database.items[i].itemType_sub.ToString() == "Pate" || database.items[i].itemType_sub.ToString() == "Cookie_base" || database.items[i].itemType_sub.ToString() == "Pie_base" || database.items[i].itemType_sub.ToString() == "Chocorate_base" || database.items[i].itemType_sub.ToString() == "Cake_base")
                                    {
                                        itemlist_hyouji();
                                    }
                                    break;

                                case 10: //お菓子をあげるとき。アイテムタイプが「お菓子」のみ表示

                                    if (database.items[i].itemType.ToString() == "Okashi")
                                    {
                                        itemlist_hyouji();
                                    }
                                    break;

                                case 99: //メニュー画面を開いたとき

                                    itemlist_hyouji();
                                    break;

                                default:

                                    break;
                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "GirlEat" || SceneManager.GetActiveScene().name == "QuestBox")
                        {
                            //お菓子のみ表示
                            if (database.items[i].itemType.ToString() == "Okashi")
                            {
                                itemlist_hyouji();
                            }
                        }

                    else if (SceneManager.GetActiveScene().name == "Shop") //納品時にリストを開くとき
                    {
                        //お菓子のみ表示
                        if (database.items[i].itemType.ToString() == "Okashi")
                        {
                            itemlist_hyouji();
                        }
                    }

                    else //調合以外のシーンでは、所持アイテム全て表示
                        {
                            itemlist_hyouji();
                        }
                    //}
                }
            }
        }

        //次に、オリジナルプレイヤーアイテムリストを、上記のリスト（_listitem）に追加していく。
        for (i = 0; i < max_original; i++)
        {
            if (pitemlist.player_originalitemlist[i].item_Hyouji > 0) //item_hyoujiが1のものを表示する。未使用アイテムなどは0にして表示しない。
            {
                //Debug.Log("ID: " + i + " 所持アイテム: " + pitemlist.player_originalitemlist[i].itemName);
                if (SceneManager.GetActiveScene().name == "Compound")
                {
                    switch (compound_Main.compound_select) //さらに、調合シーンによって、アイテム種類ごとに表示／非表示を分ける。
                    {
                        case 1: //レシピ調合のとき。「レシピリストコントローラー」で処理を行うため、このスクリプト上では無視される。

                            break;

                        case 2: //トッピング調合。お菓子と、トッピング系アイテムを表示し、ベース＝「お菓子」タイプのみ選択可能、その後、トッピングできるアイテムのみ選択可能。（フルーツ・ナッツ・チョコはOK。トッピング系アイテム。材料は×）                        

                            if (pitemlist.player_originalitemlist[i].itemType.ToString() == "Okashi" || pitemlist.player_originalitemlist[i].itemType.ToString() == "Potion")
                            {
                                original_itemlist_hyouji();
                            }

                            break;

                        case 3: //オリジナル調合。材料・生地などの素材アイテムのみ表示。お菓子タイプは表示しない。

                            if (pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Komugiko" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Butter" 
                                || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Suger" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Egg" ||
                                pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Salt" ||
                                pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Source" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Appaleil" ||
                                pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Cream" ||
                                pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Chocolate_Mat" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "IceCream" ||
                                pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Bread")
                            {
                                original_itemlist_hyouji();
                            }
                            break;

                        case 5: //焼くとき。アイテムタイプサブが「生地」のみ表示。

                            if (pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Pate" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Cookie_base" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Pie_base" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Chocorate_base" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Cake_base")
                            {
                                original_itemlist_hyouji();
                            }
                            break;

                        case 10: //お菓子をあげるとき。アイテムタイプが「お菓子」のみ表示

                            if (pitemlist.player_originalitemlist[i].itemType.ToString() == "Okashi")
                            {
                                original_itemlist_hyouji();
                            }
                            break;

                        case 99: //メニュー画面を開いたとき

                            original_itemlist_hyouji();
                            break;

                        default:
                            break;
                    }
                }
                else if (SceneManager.GetActiveScene().name == "GirlEat" || SceneManager.GetActiveScene().name == "QuestBox")
                {
                    //お菓子のみ表示
                    if (pitemlist.player_originalitemlist[i].itemType.ToString() == "Okashi")
                    {
                        original_itemlist_hyouji();
                    }
                }
                else if (SceneManager.GetActiveScene().name == "Shop") //納品時にリストを開くとき
                {
                    //お菓子のみ表示
                    if (pitemlist.player_originalitemlist[i].itemType.ToString() == "Okashi")
                    {
                        original_itemlist_hyouji();
                    }
                }
                else
                {
                    original_itemlist_hyouji();
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

        max = pitemlist.playeritemlist.Count; //現在のプレイヤーアイテムリストの最大数を更新
        max_original = pitemlist.player_originalitemlist.Count; //現在のプレイヤーオリジナルアイテムリストの最大数を更新
        //Debug.Log("max: " + max);
        //Debug.Log("max_original: " + max_original);

        list_count = 0;
        _listitem.Clear();

        //まず、プレイヤーアイテムリストを、表示
        for (i = 0; i < max; i++)
        {
            if ( database.items[i].item_Hyouji > 0 )
            {
                if (i > 500) //ID = 501以降、レシピ本などの特殊アイテムは表示しない。ゴミは表示する。
                {

                }
                else
                {

                    if (pitemlist.playeritemlist[i] > 0) //持っている個数が1以上のアイテムのみ、表示。
                    {
                        if (SceneManager.GetActiveScene().name == "Compound")
                        {
                            //お菓子タイプのみ表示
                            if (database.items[i].itemType.ToString() == "Okashi")
                            {
                                itemlist_hyouji();
                            }
                        }
                    }
                }
            }
        }

        //次に、オリジナルプレイヤーアイテムリストを、上記のリスト（_listitem）に追加していく。
        for (i = 0; i < max_original; i++)
        {
            if (pitemlist.player_originalitemlist[i].item_Hyouji > 0)
            {
                //トッピング調合の時のみ使用。

                if (SceneManager.GetActiveScene().name == "Compound")
                {
                    //お菓子タイプのみ表示
                    if (pitemlist.player_originalitemlist[i].itemType.ToString() == "Okashi")
                    {
                        original_itemlist_hyouji();
                    }
                }
            }
        }
    }


    
    //トッピング調合の時のみ使用。トッピング材料を決める時。

    public void topping_DrawView_2()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        max = pitemlist.playeritemlist.Count; //現在のプレイヤーアイテムリストの最大数を更新
        max_original = pitemlist.player_originalitemlist.Count; //現在のプレイヤーオリジナルアイテムリストの最大数を更新
        //Debug.Log("max: " + max);
        //Debug.Log("max_original: " + max_original);

        list_count = 0;
        _listitem.Clear();

        //まず、プレイヤーアイテムリストを、表示
        for (i = 0; i < max; i++)
        {
            if (database.items[i].item_Hyouji > 0)
            {
                if (i > 500) //ID = 501以降、レシピ本などの特殊アイテムは表示しない。ゴミは表示する。
                {

                }
                else
                {

                    if (pitemlist.playeritemlist[i] > 0) //持っている個数が1以上のアイテムのみ、表示。
                    {
                        if (SceneManager.GetActiveScene().name == "Compound")
                        {
                            if (GameMgr.tutorial_ON == true)
                            {
                                //チュートリアル時は、とりあえずオレンジだけ表示
                                if (database.items[i].itemName == "orange")
                                {
                                    itemlist_hyouji();
                                }
                            }
                            else
                            {
                                //トッピング材料（ポーションかフルーツ・ナッツ系など）のみ表示
                                if (database.items[i].itemType.ToString() == "Potion" || database.items[i].itemType_sub.ToString() == "Fruits" ||
                                    database.items[i].itemType_sub.ToString() == "Nuts" || database.items[i].itemType_sub.ToString() == "Chocolate_Mat" ||
                                    database.items[i].itemType_sub.ToString() == "IceCream")
                                {
                                    itemlist_hyouji();
                                }
                            }
                            
                        }
                    }
                }
            }
        }

        //次に、オリジナルプレイヤーアイテムリストを、上記のリスト（_listitem）に追加していく。
        for (i = 0; i < max_original; i++)
        {
            if (pitemlist.player_originalitemlist[i].item_Hyouji > 0)
            {
                //トッピング調合の時のみ使用。

                if (SceneManager.GetActiveScene().name == "Compound")
                {
                    //トッピング材料（ポーションかフルーツ・ナッツ系など）のみ表示
                    if (pitemlist.player_originalitemlist[i].itemType.ToString() == "Potion" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Fruits" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Nuts" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Chocolate" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "IceCream")
                    {
                        original_itemlist_hyouji();
                    }
                }
            }
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

        item_kosu = pitemlist.playeritemlist[i];

        _text[1].text = item_kosu.ToString(); //プレイヤーがそのアイテムをもっている個数

        //画像を変更
        texture2d = database.items[i].itemIcon_sprite;
        _Img.sprite = texture2d;
        /*_Img.sprite = Sprite.Create(texture2d,
                       new Rect(0, 0, texture2d.width, texture2d.height),
                       Vector2.zero);*/

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

        //_slotHyouji1[]は、一度名前を、全て空白に初期化
        for (n = 0; n < _slotHyouji1.Length; n++)
        {
            _slotHyouji1[n] = "";
        }

        //カード正式名称（ついてるスロット名も含めた名前）
        for (n = 0; n < _slot.Length; n++)
        {
            count = 0;

            //スロット名を日本語に変換。DBから変換。Nonは、空白になる。
            while (count < slotnamedatabase.slotname_lists.Count)
            {
                if (slotnamedatabase.slotname_lists[count].slotName == pitemlist.player_originalitemlist[i].toppingtype[n].ToString())
                {

                    _slotHyouji1[n] = GameMgr.ColorYellow + slotnamedatabase.slotname_lists[count].slot_Hyouki_2 + "</color>";

                    break;
                }
                count++;
            }
        }

        _text[0].text = _slotHyouji1[0] + _slotHyouji1[1] + _slotHyouji1[2] + _slotHyouji1[3] + _slotHyouji1[4] + _slotHyouji1[5] + _slotHyouji1[6] + _slotHyouji1[7] + _slotHyouji1[8] + _slotHyouji1[9] + item_name;
                

        item_kosu = pitemlist.player_originalitemlist[i].ItemKosu;

        _text[1].text = item_kosu.ToString(); //プレイヤーがそのアイテムをもっている個数
        _text[1].color = new Color(50f / 255f, 128f / 255f, 126f / 255f);

        //Debug.Log("Original: " + i + "　ItemID" + _toggle_itemID.toggleitem_ID + " アイテム名: " + item_name);

        //画像を変更
        texture2d = pitemlist.player_originalitemlist[i].itemIcon_sprite;
        _Img.sprite = texture2d;
        /*_Img.sprite = Sprite.Create(texture2d,
                       new Rect(0, 0, texture2d.width, texture2d.height),
                       Vector2.zero);*/

        ++list_count;
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
