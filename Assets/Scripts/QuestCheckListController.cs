using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class QuestCheckListController : MonoBehaviour
{
    private GameObject canvas;

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    public List<GameObject> _quest_listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Sprite texture2d;
    private Image _Img;
    private QuestCheckListSelectToggle _toggle_itemID;

    private GameObject questitem_Prefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。

    private PlayerItemList pitemlist;
    private TimeController time_controller;
    private ItemDataBase database;

    private QuestSetDataBase questset_database;

    private GameObject categoryListToggle_obj;
    private Toggle categoryListToggle;

    private GameObject contest_detailedPanel;

    private string _name;
    private string _name_Hyouji;
    private int item_kosu;

    private string _contest_Grade;

    private int max;
    private int count;
    private int i, j;
    private int _hoshu;

    private int _Limit_day;
    private int _Nokori_day;

    public int _count; //選択したリスト番号が入る。
    public int _ID; //ショップデータベースIDが入る。

    private int read_ID;

    private int rand;

    void Awake() //Startより手前で先に読みこんで、OnEnableの挙動のエラー回避
    {     
    }

    // Use this for initialization
    void Start()
    {
        InitSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitSetting()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //クエスト受注データベース取得
        questset_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Viewport/Content").gameObject;
        questitem_Prefab = (GameObject)Resources.Load("Prefabs/questCheckListSelectToggle");
    }

    void OnEnable()
    {
        //ウィンドウがアクティヴになった瞬間だけ読み出される
        //Debug.Log("OnEnable");

        InitSetting();
        reset_and_DrawView();
    }

    // リストビューの描画部分。重要。
    public void reset_and_DrawView()
    {
        //現在、受注リストを開いている状態       

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _quest_listitem.Clear();

        i = 0;
        while (i < questset_database.questTakeset.Count)
        {
            DrawQuestCheck();
            i++;
        }
    }

    void DrawQuestCheck()
    {
        _quest_listitem.Add(Instantiate(questitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _Img = _quest_listitem[list_count].transform.Find("Background/ImageIcon").GetComponent<Image>(); //アイテムの画像データ

        _toggle_itemID = _quest_listitem[list_count].GetComponent<QuestCheckListSelectToggle>();
        _toggle_itemID.toggle_listcount = i; //TakeSet上のリスト番号も保持
        _toggle_itemID.toggle_ID = questset_database.questTakeset[i].Quest_ID; //DBのID。上から順番
        _name_Hyouji = questset_database.questTakeset[i].Quest_Title; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。
        //_name = questset_database.questTakeset[i].Quest_itemName; //クエストで納品するアイテム名
        //_toggle_itemID.toggle_name = _name; //
        _toggle_itemID.toggle_nameHyouji = _name_Hyouji; //


        _quest_listitem[list_count].transform.Find("Background/Quest_name").GetComponent<Text>().text = _name_Hyouji;

        //あと何日
        _Limit_day = time_controller.CullenderKeisanInverse(questset_database.questTakeset[i].Quest_LimitMonth, questset_database.questTakeset[i].Quest_LimitDay);
        _Nokori_day = _Limit_day - PlayerStatus.player_day;
        _quest_listitem[list_count].transform.Find("Background/Quest_day").GetComponent<Text>().text = _Nokori_day.ToString() + "日";

        //texture2d = questset_database.questTakeset[i].ContestIcon_sprite;
        //_Img.sprite = texture2d;


        //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);
        ++list_count;
    }
    
    

    public void OnQuestCheckList_Draw()
    {      
        reset_and_DrawView();      
    }

}
