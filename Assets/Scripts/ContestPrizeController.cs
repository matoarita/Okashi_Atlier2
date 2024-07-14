using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ContestPrizeController : MonoBehaviour
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

    private Toggle categoryListToggle;

    private GameObject closebutton;

    private string _name;
    private string _rank_score;
    private int item_kosu;
    private string _rank_name;
    private int _Type;

    private string _contest_Grade;

    public List<int> final_PrizeScoreList = new List<int>();
    public List<string> final_PrizeCharacterNameList = new List<string>();

    private int max;
    private int counter;
    private int i, j;
    private int _hoshu;

    public int _count; //選択したリスト番号が入る。
    public int _ID; //ショップデータベースIDが入る。

    private int read_ID;

    private int _sw;

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
        contestitem_Prefab = (GameObject)Resources.Load("Prefabs/ContestPrizeSelectToggle");

        closebutton = this.transform.parent.Find("closeButtonPanel").gameObject;

        final_PrizeScoreList.Clear();
        final_PrizeCharacterNameList.Clear();

        _Type = 0;
        _sw = 0;

        switch(GameMgr.Scene_Category_Num)
        {
            case 100: //コンテスト本番シーン　景品獲得シーンで開くとき

                _Type = GameMgr.Utage_Prizepanel_Type;
                closebutton.SetActive(false);
                break;

            case 120: //受付シーンで開くとき

                _Type = 1;
                closebutton.SetActive(true);
                break;
        }
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

        if(GameMgr.Contest_Cate_Ranking == 0) //トーナメント形式の表示
        {
            i = 0;
            while (i < GameMgr.PrizeItemList.Count)
            {
                DrawContest_TounamentPrize01();
                i++;
            }
        }
        else //ランキング形式の表示
        {
            if (_Type == 0) //コンテスト本番で景品獲得シーンから開いた場合　順位とキャラのリストが表示
            {
                //先にアキラくんの順位と点数をいれた、表示用のリストを作る
                list_count = 0;
                counter = 0;
                while (counter < GameMgr.PrizeItemList.Count)
                {
                    RankingKeisan();
                    counter++;
                }

                //表示する
                list_count = 0;
                i = 0;
                while (i < GameMgr.PrizeItemList.Count)
                {
                    DrawContest_RankingPrize01();
                    i++;
                }
            }
            else //コンテストの詳細画面から開いた場合　順位と賞品のリストが表示
            {
                //表示する
                list_count = 0;
                i = 0;
                while (i < GameMgr.PrizeItemList.Count)
                {
                    DrawContest_RankingPrize02();
                    i++;
                }
            }
        }
        
    }

    //ランキング計算　アキラくんの名前をいれて、改めて表示リストを作る
    void RankingKeisan()
    {
        //先にアキラくんの点数と順位をだす。
        //一位から順番に更新
        if (GameMgr.contest_Rank_Count == counter + 1) //アキラくんの順位がi=0の場合、1位かどうかみる。
        {
            //一位なら、その位置に、アキラくんの順位と点数いれる。
            final_PrizeCharacterNameList.Add(GameMgr.player_Name);
            final_PrizeScoreList.Add(GameMgr.contest_TotalScore);
        }
        else
        {
            //一位ではなかった。ということは、別の参加者を入れる。
            //_rank_name = counter + 1 + "位"; //一位　二位の数字

            //ランキング形式の場合
            if (list_count == 0)
            {
                final_PrizeCharacterNameList.Add(GameMgr.PrizeCharacterList[GameMgr.PrizeCharacterList.Count - 1]);
                final_PrizeScoreList.Add(GameMgr.PrizeScoreAreaList[GameMgr.PrizeScoreAreaList.Count - 1]);

            }
            else
            {
                final_PrizeCharacterNameList.Add(GameMgr.PrizeCharacterList[GameMgr.PrizeCharacterList.Count - 1 - list_count]);
                final_PrizeScoreList.Add(GameMgr.PrizeScoreAreaList[GameMgr.PrizeScoreAreaList.Count - 1 - list_count]);
            }

            list_count++;
        }
    }

    //ランキング形式　順位とキャラ名 宴の点数発表シーンで使う
    void DrawContest_RankingPrize01()
    {
        _contest_listitem.Add(Instantiate(contestitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        //_Img = _contest_listitem[list_count].transform.Find("Background/ImageIcon").GetComponent<Image>(); //アイテムの画像データ


        //一位から順番に更新
        _rank_name = i + 1 + "位"; //一位　二位の数字

        _rank_score = final_PrizeScoreList[i].ToString() + "点"; //一位の得点
        _name = final_PrizeCharacterNameList[i]; //名前             
        
        Debug.Log("順位: " + _rank_name + " " + _rank_score + " " + _name);


        _contest_listitem[list_count].transform.Find("Background/Rank_name").GetComponent<Text>().text = _rank_name;
        _contest_listitem[list_count].transform.Find("Background/Rank_Score").GetComponent<Text>().text = _rank_score;
        _contest_listitem[list_count].transform.Find("Background/Rank_Score").gameObject.SetActive(true);
        _contest_listitem[list_count].transform.Find("Background/Rank_Money").gameObject.SetActive(false);
        _contest_listitem[list_count].transform.Find("Background/Prize_characterName").GetComponent<Text>().text = _name;
        _contest_listitem[list_count].transform.Find("Background/Prize_ItemName").gameObject.SetActive(false);
        _contest_listitem[list_count].transform.Find("Background/Prize_characterName").gameObject.SetActive(true);

        //texture2d = conteststartList_database.conteststart_lists[i].ContestIcon_sprite;
        //_Img.sprite = texture2d;


        //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);
        ++list_count;
    }

    //ランキング形式　順位ごとの景品　コンテストの詳細画面で使う
    void DrawContest_RankingPrize02()
    {
        _contest_listitem.Add(Instantiate(contestitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        //_Img = _contest_listitem[list_count].transform.Find("Background/ImageIcon").GetComponent<Image>(); //アイテムの画像データ

        //一位から順番に更新
        _rank_name = i + 1 + "位"; //一位　二位の数字

        //ランキング形式の場合 _rank_scoreは賞金の表示になる
        _rank_score = GameMgr.PrizeGetMoneyList[GameMgr.PrizeGetMoneyList.Count - 1 - i].ToString(); //
        if (GameMgr.PrizeItemList[GameMgr.PrizeItemList.Count - 1 - i] != "Non")
        {
            _ID = database.SearchItemIDString(GameMgr.PrizeItemList[GameMgr.PrizeItemList.Count - 1 - i]);
            _name = database.items[_ID].itemNameHyouji;

        }
        else
        {
            _name = "-";
        }
        

        Debug.Log("順位: " + _rank_name + " " + _rank_score + "Lp " + _name);


        _contest_listitem[list_count].transform.Find("Background/Rank_name").GetComponent<Text>().text = _rank_name;
        _contest_listitem[list_count].transform.Find("Background/Rank_Money/text").GetComponent<Text>().text = _rank_score;
        _contest_listitem[list_count].transform.Find("Background/Rank_Score").gameObject.SetActive(false);
        _contest_listitem[list_count].transform.Find("Background/Rank_Money").gameObject.SetActive(true);
        _contest_listitem[list_count].transform.Find("Background/Prize_ItemName").GetComponent<Text>().text = _name;
        _contest_listitem[list_count].transform.Find("Background/Prize_ItemName").gameObject.SetActive(true);
        _contest_listitem[list_count].transform.Find("Background/Prize_characterName").gameObject.SetActive(false);

        //texture2d = conteststartList_database.conteststart_lists[i].ContestIcon_sprite;
        //_Img.sprite = texture2d;


        //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);
        ++list_count;
    }

    //トーナメント形式　ランクごとの景品　コンテスト本番・詳細画面両方で共通
    void DrawContest_TounamentPrize01()
    {
        _contest_listitem.Add(Instantiate(contestitem_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        //_Img = _contest_listitem[list_count].transform.Find("Background/ImageIcon").GetComponent<Image>(); //アイテムの画像データ

        //一位から順番に更新
        switch(i)
        {
            case 0:
                _rank_name = "S"; //一位　二位の数字
                break;

            case 1:
                _rank_name = "A";
                break;

            case 2:
                _rank_name = "B";
                break;

            case 3:
                _rank_name = "C";
                break;

            case 4:
                _rank_name = "D";
                break;
        }        

        //トーナメント形式
        if(i == 0)
        {
            _rank_score = GameMgr.PrizeScoreAreaList[GameMgr.PrizeScoreAreaList.Count - 1 - i].ToString() + "点～";
        }
        else if (i == GameMgr.PrizeItemList.Count - 1) //一番最後の数字　GameMgr.PrizeItemListの配列数は5だが、スコアは4人分なので、配列間違いに注意
        {
            _rank_score = "～" + GameMgr.PrizeScoreAreaList[0].ToString() + "点";
        }
        else
        {
            _rank_score = GameMgr.PrizeScoreAreaList[GameMgr.PrizeScoreAreaList.Count - 1 - i].ToString() + "点～";
        }
        if (GameMgr.PrizeItemList[GameMgr.PrizeItemList.Count - 1 - i] != "Non")
        {
            _ID = database.SearchItemIDString(GameMgr.PrizeItemList[GameMgr.PrizeItemList.Count - 1 - i]);
            _name = database.items[_ID].itemNameHyouji;

        }
        else
        {
            _name = "-";
        }

        Debug.Log("順位: " + _rank_name + " " + _rank_score + "Lp " + _name);


        _contest_listitem[list_count].transform.Find("Background/Rank_name").GetComponent<Text>().text = _rank_name;
        _contest_listitem[list_count].transform.Find("Background/Rank_Score").GetComponent<Text>().text = _rank_score;
        _contest_listitem[list_count].transform.Find("Background/Rank_Score").gameObject.SetActive(true);
        _contest_listitem[list_count].transform.Find("Background/Rank_Money").gameObject.SetActive(false);
        _contest_listitem[list_count].transform.Find("Background/Prize_ItemName").GetComponent<Text>().text = _name;
        _contest_listitem[list_count].transform.Find("Background/Prize_ItemName").gameObject.SetActive(true);
        _contest_listitem[list_count].transform.Find("Background/Prize_characterName").gameObject.SetActive(false);

        //texture2d = conteststartList_database.conteststart_lists[i].ContestIcon_sprite;
        //_Img.sprite = texture2d;


        //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);
        ++list_count;
    }


    public void OnContestPrize_Draw()
    {      
        reset_and_DrawView();       
    }

    public void OnClosePrizePanel()
    {
        GameMgr.Scene_Status = 51; //閉じた
        this.transform.parent.gameObject.SetActive(false);
    }

    public void OnDebugContestPrize_PanelChange()
    {
        if (_sw == 2)
        {
            _sw = 0;
        }
        else
        {
            _sw++;
        }

        switch (_sw)
        {
            case 0:
                GameMgr.Contest_Cate_Ranking = 0;
                break;

            case 1:

                GameMgr.Contest_Cate_Ranking = 1;
                _Type = 0;
                break;

            case 2:

                GameMgr.Contest_Cate_Ranking = 1;
                _Type = 1;
                break;
        }
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
