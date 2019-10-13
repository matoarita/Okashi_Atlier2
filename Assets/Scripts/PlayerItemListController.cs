using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//プレイヤーアイテムリストのスクロールビューのコントローラー。
//調合シーンや、アイテムを女の子にあげたときの処理は、「itemSelectToggle」スクリプトに記述してます。

public class PlayerItemListController : SingletonMonoBehaviour<PlayerItemListController>
{
    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject comp_text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _comp_text; //同じく、Scene「Compund」用。

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private PlayerItemList pitemlist;

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private Button getItemAddButton;
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数

    public List<GameObject> _prelistitem = new List<GameObject>(); //リストビューの個数　表示用に、事前に格納しておくリスト。
    public List<GameObject> _listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text[] _text = new Text[2];
    private itemSelectToggle _toggle_itemID;


    private string item_name;
    private int item_kosu;

    private int max;
    private int max_original;
    private int count;
    private int i;

    //各プレファブ共通で、変更できる値が必要。そのパラメータは、PlayerItemListControllerで管理する。
    public int _count1; //表示されているリスト中の選択番号 1
    public int _count2; //表示されているリスト中の選択番号 2
    public int _count3; //表示されているリスト中の選択番号 3
    public int _base_count;

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


    void Awake() //Startより手前で先に読みこんで、OnEnableの挙動のエラー回避
    {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();


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

        ResetKettei_item();
        kettei1_bunki = 0;

        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {

            compound_Main_obj = GameObject.FindWithTag("Compound_Main");
            compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

            // トッピング調合を選択した場合の処理
            if (compound_Main.compound_select == 2)
            {
                topping_DrawView_1();
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
                topping_DrawView_1();
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
            //Debug.Log("ID: " + i + "所持数: " + pitemlist.playeritemlist[i]);
            if (i > 500) //ID = 501以降、レシピ本などの特殊アイテムは表示しない。ゴミは表示する。
            {

            } else {

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

                                if (database.items[i].itemType.ToString() == "Mat" || database.items[i].itemType.ToString() == "Potion")
                                {
                                    itemlist_hyouji();
                                }
                                break;

                            case 5: //焼くとき。アイテムタイプサブが「生地」のみ表示。
                                if (database.items[i].itemType_sub.ToString() == "Pate")
                                {
                                    itemlist_hyouji();
                                }
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
                    else //調合以外のシーンでは、所持アイテム全て表示
                    {
                        itemlist_hyouji();
                    }
                }
            }
        }

        //次に、オリジナルプレイヤーアイテムリストを、上記のリスト（_listitem）に追加していく。
        for (i = 0; i < max_original; i++)
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

                        if (pitemlist.player_originalitemlist[i].itemType.ToString() == "Mat" || pitemlist.player_originalitemlist[i].itemType.ToString() == "Potion")
                        {
                            original_itemlist_hyouji();
                        }
                        break;

                    case 5: //焼くとき。アイテムタイプサブが「生地」のみ表示。

                        if (pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Pate")
                        {
                            original_itemlist_hyouji();
                        }
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
            else
            {
                original_itemlist_hyouji();
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

        //次に、オリジナルプレイヤーアイテムリストを、上記のリスト（_listitem）に追加していく。
        for (i = 0; i < max_original; i++)
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
            if (i > 500) //ID = 501以降、レシピ本などの特殊アイテムは表示しない。ゴミは表示する。
            {

            }
            else
            {

                if (pitemlist.playeritemlist[i] > 0) //持っている個数が1以上のアイテムのみ、表示。
                {
                    if (SceneManager.GetActiveScene().name == "Compound")
                    {
                        //トッピング材料（ポーションかフルーツ・ナッツ系など）のみ表示
                        if (database.items[i].itemType.ToString() == "Potion" || database.items[i].itemType_sub.ToString() == "Fruits" || database.items[i].itemType_sub.ToString() == "Nuts")
                        {
                            itemlist_hyouji();
                        }
                    }
                }
            }
        }

        //次に、オリジナルプレイヤーアイテムリストを、上記のリスト（_listitem）に追加していく。
        for (i = 0; i < max_original; i++)
        {
            //トッピング調合の時のみ使用。

            if (SceneManager.GetActiveScene().name == "Compound")
            {
                //トッピング材料（ポーションかフルーツ・ナッツ系など）のみ表示
                if (pitemlist.player_originalitemlist[i].itemType.ToString() == "Potion" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Fruits" || pitemlist.player_originalitemlist[i].itemType_sub.ToString() == "Nuts")
                {
                    original_itemlist_hyouji();
                }
            }
        }
    }

    void itemlist_hyouji()
    {
        //Debug.Log(i);
        _listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。

        _toggle_itemID = _listitem[list_count].GetComponent<itemSelectToggle>();

        _toggle_itemID.toggleitem_ID = i; //アイテムIDを、リストビューのトグル自体にも記録させておく。 
        _toggle_itemID.toggleitem_type = 0; //プレイヤーアイテムリストを識別するための番号。0を入れる。
        _toggle_itemID.toggle_originplist_ID = i; //店売りアイテムのアイテムリスト番号
        //Debug.Log("プレイヤ店売りアイテムリストID: " + _toggle_itemID.toggle_originplist_ID);


        item_name = database.items[i].itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

        _text[0].text = item_name;

        item_kosu = pitemlist.playeritemlist[i];

        _text[1].text = item_kosu.ToString(); //プレイヤーがそのアイテムをもっている個数

        ++list_count;
    }

    void original_itemlist_hyouji()
    {
        _listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。


        _toggle_itemID = _listitem[list_count].GetComponent<itemSelectToggle>();

        _toggle_itemID.toggleitem_ID = pitemlist.player_originalitemlist[i].itemID; //アイテムIDを、リストビューのトグル自体にも記録させておく。
        _toggle_itemID.toggleitem_type = 1; //プレイヤーアイテムリストを識別するための番号。オリジナルアイテムの場合、1を入れる。
        _toggle_itemID.toggle_originplist_ID = i; //オリジナルアイテムリストのリスト番号
        //Debug.Log("プレイヤオリジナルアイテムリストID: " + _toggle_itemID.toggle_originplist_ID + " " + "アイテムID: " + _toggle_itemID.toggleitem_ID);

        item_name = pitemlist.player_originalitemlist[i].itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。


        _text[0].text = item_name;
        _text[0].color = new Color(50f / 255f, 128f / 255f, 126f / 255f);

        item_kosu = pitemlist.player_originalitemlist[i].ItemKosu;

        _text[1].text = item_kosu.ToString(); //プレイヤーがそのアイテムをもっている個数
        _text[1].color = new Color(50f / 255f, 128f / 255f, 126f / 255f);

        //Debug.Log("Original: " + i + "　ItemID" + _toggle_itemID.toggleitem_ID + " アイテム名: " + item_name);
        ++list_count;
    }

}
