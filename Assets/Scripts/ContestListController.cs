﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ContestListController : MonoBehaviour
{
    private GameObject canvas;

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    public List<GameObject> _contest_listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Sprite texture2d;
    private Image _Img;
    private ContestListSelectToggle _toggle_itemID;

    private GameObject contestitem_Prefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    private ContestStartListDataBase conteststartList_database;

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

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Viewport/Content").gameObject;
        contestitem_Prefab = (GameObject)Resources.Load("Prefabs/ContestListSelectToggle");

        //ボタンの取得
        categoryListToggle_obj = this.transform.Find("CategoryView/Viewport/Content/Cate_QuestList").gameObject;

        contest_detailedPanel = canvas.transform.Find("ContestListPanel/Contest_DetailedPanel").gameObject;
        contest_detailedPanel.SetActive(false);
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
        _contest_listitem.Clear();

        switch(GameMgr.Scene_Name)
        {
            case "Or_Contest_Reception_Spring":

                read_ID = 0; //ID=0～からread_endflag=1まで読む
                break;
        }

        i = 0;
        while ( i < conteststartList_database.conteststart_lists.Count)
        {
            if (conteststartList_database.conteststart_lists[i].ContestID >= read_ID)
            {
                if (conteststartList_database.conteststart_lists[i].Contest_Flag == 1)
                {
                    DrawContest();
                }

                if (conteststartList_database.conteststart_lists[i].read_endflag == 1)
                {
                    break;
                }
            }
            i++;
        }
    }

    void DrawContest()
    {
        _contest_listitem.Add(Instantiate(contestitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _Img = _contest_listitem[list_count].transform.Find("Background/ImageIcon").GetComponent<Image>(); //アイテムの画像データ

        _toggle_itemID = _contest_listitem[list_count].GetComponent<ContestListSelectToggle>();
        _toggle_itemID.toggle_ID = conteststartList_database.conteststart_lists[i].ContestID; //DBのID。上から順番
        _toggle_itemID.toggle_RankType = conteststartList_database.conteststart_lists[i].Contest_RankingType; //ランキングタイプも保存
        _name_Hyouji = conteststartList_database.conteststart_lists[i].ContestNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。
        _name = conteststartList_database.conteststart_lists[i].ContestName;
        _toggle_itemID.toggle_name = _name; //
        _toggle_itemID.toggle_nameHyouji = _name_Hyouji; //


        _contest_listitem[list_count].transform.Find("Background/Quest_name").GetComponent<Text>().text = _name_Hyouji;
        _contest_Grade = conteststartList_database.RankToGradeText(conteststartList_database.conteststart_lists[i].Contest_Lv);
        _contest_listitem[list_count].transform.Find("Background/ContestRank").GetComponent<Text>().text = _contest_Grade;

        texture2d = conteststartList_database.conteststart_lists[i].ContestIcon_sprite;
        _Img.sprite = texture2d;

        if(conteststartList_database.conteststart_lists[i].Contest_Accepted == 1)
        {
            _contest_listitem[list_count].GetComponent<Toggle>().interactable = false;
            _contest_listitem[list_count].transform.Find("AcceptedPanel").gameObject.SetActive(true);

            for (j = 0; j < GameMgr.contest_accepted_list.Count; j++)
            {
                if(GameMgr.contest_accepted_list[j].contestName == _name)
                {
                    _contest_listitem[list_count].transform.Find("AcceptedPanel/Text").GetComponent<Text>().text = 
                        GameMgr.contest_accepted_list[j].Month.ToString() + "/" + GameMgr.contest_accepted_list[j].Day.ToString() + "出場";
                }
            } 
            
        }
        else
        {
            _contest_listitem[list_count].transform.Find("AcceptedPanel").gameObject.SetActive(false);
        }

        //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);
        ++list_count;
    }
    
    

    public void OnContestList_Draw()
    {
        
        reset_and_DrawView();
        
    }

    //デバッグ用　全てのクエストを表示する。
    public void DebugQuestAllRequest()
    {
        for (i = 0; i < conteststartList_database.conteststart_lists.Count; i++)
        {

        }
    }
}
