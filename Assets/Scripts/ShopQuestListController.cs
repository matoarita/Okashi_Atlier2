using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ShopQuestListController : MonoBehaviour
{
    private GameObject canvas;

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    public List<GameObject> _quest_listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text[] _text = new Text[3];
    private Sprite texture2d;
    private Image _Img;
    private shopQuestSelectToggle _toggle_itemID;

    private GameObject questitem_Prefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。

    private PlayerItemList pitemlist;

    public GameObject cardImage_onoff_pcontrol;

    private ItemDataBase database;

    private QuestSetDataBase quest_database;

    private GameObject questListToggle_obj;
    private Toggle questListToggle;
    private Color color1;
    private GameObject nouhinToggle_obj;
    private Toggle nouhinToggle;
    private Color color2;

    private string item_name;
    private string item_name_Hyouji;
    private int item_kosu;

    private int max;
    private int count;
    private int i, j;

    public int _count; //選択したリスト番号が入る。
    public int _ID; //ショップデータベースIDが入る。
    public int questID; //選択したアイテムのアイテムIDが入る。通常アイテムなら、アイテムID、イベントアイテムならイベントリストのアイテムID。
    public int questType;
    public int quest_itemID;

    public int qlist_status;
    public int nouhin_select_on;
    public bool final_select_flag;

    private int rand;
    private int[] sel_questID = new int[3];
    private List<int> selectquestDB = new List<int>();

    private int story_num;

    void Awake() //Startより手前で先に読みこんで、OnEnableの挙動のエラー回避
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Viewport/Content").gameObject;
        questitem_Prefab = (GameObject)Resources.Load("Prefabs/shopQuestSelectToggle");

        //ボタンの取得
        questListToggle_obj = this.transform.Find("CategoryView/Viewport/Content/Cate_QuestList").gameObject;
        questListToggle = questListToggle_obj.GetComponent<Toggle>();
        color1 = questListToggle_obj.transform.Find("Background").GetComponent<Image>().color;
        nouhinToggle_obj = this.transform.Find("CategoryView/Viewport/Content/Cate_Nouhin").gameObject;
        nouhinToggle = nouhinToggle_obj.GetComponent<Toggle>();
        color2 = nouhinToggle_obj.transform.Find("Background").GetComponent<Image>().color;

        i = 0;
        qlist_status = 0;
        nouhin_select_on = 0;
        final_select_flag = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        //ウィンドウがアクティヴになった瞬間だけ読み出される
        //Debug.Log("OnEnable");

        
        RandomQuestSelect();

        questListToggle.isOn = true;
        nouhinToggle.isOn = false;

        qlist_status = 0;

        reset_and_DrawView();

    }

    //ランダムでクエストを３つ選ぶ。
    void RandomQuestSelect()
    {
        InitiallizeRandomQuestDatabase(); //ストーリー進行にそって、どのクエストが選ばれるかを初期化

        quest_database.questRandomset.Clear();

        //ランダムでセット３つを選ぶ。
        for (i = 0; i < sel_questID.Length; i++)
        {
            rand = Random.Range(0, selectquestDB.Count);

            quest_database.RandomNewSetInit(selectquestDB[rand]);

        }
    }

    void InitiallizeRandomQuestDatabase()
    {
        selectquestDB.Clear();

        story_num = GameMgr.GirlLoveEvent_num; //ステージ1なら　クエストごとに0~5

        for (j = 0; j < quest_database.questset.Count; j++)
        {
            if (quest_database.questset[j].QuestHyouji <= story_num)
            {
                selectquestDB.Add(j);
            }
        }
    }

    // リストビューの描画部分。重要。
    public void reset_and_DrawView()
    {
        //現在、受注リストを開いている状態       

        color1 = new Color(1f, 1f, 1f, 1.0f);
        color2 = new Color(1f, 1f, 1f, 0.5f);

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _quest_listitem.Clear();

        for (i = 0; i < quest_database.questRandomset.Count; i++)
        {
            //if (quest_database.questRandomset[i].shop_item_hyouji > 0) //1だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。
            //{


            _quest_listitem.Add(Instantiate(questitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
            _text = _quest_listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、6つのテキストコンポを格納する。
            _Img = _quest_listitem[list_count].transform.Find("Background/ImageIcon").GetComponent<Image>(); //アイテムの画像データ

            _toggle_itemID = _quest_listitem[list_count].GetComponent<shopQuestSelectToggle>();
            _toggle_itemID.toggle_ID = quest_database.questRandomset[i]._ID; //DBのID。上から順番
            _toggle_itemID.toggle_quest_ID = quest_database.questRandomset[i].Quest_ID; //クエスト固有のID
            _toggle_itemID.toggle_quest_type = quest_database.questRandomset[i].QuestType; //クエストのタイプ　0なら材料採取　1ならお菓子系。1は、プレイヤーが選択


            item_name_Hyouji = quest_database.questRandomset[i].Quest_Title; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

            item_name = quest_database.questRandomset[i].Quest_itemName;
            _toggle_itemID.toggle_itemID = pitemlist.SearchItemString(item_name);

            _text[0].text = item_name_Hyouji;

            item_kosu = quest_database.questRandomset[i].Quest_kosu_default;

            _text[2].text = item_kosu.ToString(); //個数

            //進行中表示はオフ
            _text[3].text = "";
            _text[4].text = "";

            _text[6].text = quest_database.questRandomset[i].Quest_buy_price.ToString();

            texture2d = quest_database.questRandomset[i].questIcon;
            _Img.sprite = texture2d;

            //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);

            ++list_count;

            //}
        }

    }
    
    public void NouhinList_DrawView()
    {
        //現在、納品リストを開いている状態

        color1 = new Color(1, 1, 1, 0.5f);
        color2 = new Color(1, 1, 1, 1.0f);

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _quest_listitem.Clear();

        for (i = 0; i < quest_database.questTakeset.Count; i++)
        {

            _quest_listitem.Add(Instantiate(questitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
            _text = _quest_listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、３つのテキストコンポを格納する。
            _Img = _quest_listitem[list_count].transform.Find("Background/ImageIcon").GetComponent<Image>(); //アイテムの画像データ

            _toggle_itemID = _quest_listitem[list_count].GetComponent<shopQuestSelectToggle>();
            _toggle_itemID.toggle_ID = quest_database.questTakeset[i]._ID; //DBのID。上から順番
            _toggle_itemID.toggle_quest_ID = quest_database.questTakeset[i].Quest_ID; //クエスト固有のID
            _toggle_itemID.toggle_quest_type = quest_database.questTakeset[i].QuestType; //クエストのタイプ　0なら材料採取　1ならお菓子系。1は、プレイヤーが選択


            item_name_Hyouji = quest_database.questTakeset[i].Quest_Title; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。
            item_name = quest_database.questTakeset[i].Quest_itemName;
            _toggle_itemID.toggle_itemID = pitemlist.SearchItemString(item_name);

            _text[0].text = item_name_Hyouji;

            item_kosu = quest_database.questTakeset[i].Quest_kosu_default;

            _text[2].text = item_kosu.ToString(); //価格

            _text[3].text = "進行中"; //受注マーク
            _text[4].text = ""; //締め切り日時 締切: ○月△日

            _text[6].text = quest_database.questTakeset[i].Quest_buy_price.ToString();

            texture2d = quest_database.questTakeset[i].questIcon;
            _Img.sprite = texture2d;

            //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);

            ++list_count;

        }
    }

    public void OnQuestList()
    {

        qlist_status = 0;
        
        reset_and_DrawView();
        
    }

    public void OnNouhinList()
    {

        qlist_status = 1;

        NouhinList_DrawView();
        
    }
}
