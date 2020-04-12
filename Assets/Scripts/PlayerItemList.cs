using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemList : SingletonMonoBehaviour<PlayerItemList>
{

    private Entity_ItemDataBase excel_itemdatabase; //sample_excelクラス。エクセル読み込み時、XMLインポートで生成されたときのクラスを読む。リスト型と同じように扱える。

    private Entity_eventItemDataBase excel_eventitemdatabase; //イベント用アイテムデータベース。

    private ItemDataBase database;

    private int _id;
    private int _comp_hosei;
    private string _file_name, _nameHyouji, _desc;
    private string _type;
    private string _subtype;
    private string _koyutp;
    private int _judge_num;
    private int _first_eat;

    private string ev_fileName, ev_itemName, ev_itemNameHyouji;
    private int ev_kosu;
    private int ev_cost, ev_sell;
    private int ev_read_flag; //そのレシピを読み終えたかどうかをチェックするフラグ
    private int ev_list_on; //レシピリストに、表示するか否か。1の場合、リストに表示され、使用すると、そのレシピの内容を読むことができる。
    private string ev_memo;

    

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    //プレイヤーの所持アイテムリスト。Dictionaryなので、｛アイテムID, 個数｝の関係で格納する。
    public Dictionary<int, int> playeritemlist = new Dictionary<int, int>();

    //プレイヤーのイベントアイテムリスト。
    public List<ItemEvent> eventitemlist = new List<ItemEvent>();

    //プレイヤーが作成したオリジナルのアイテムリスト。
    public List<Item> player_originalitemlist = new List<Item>(); 

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this); //プレイヤーの所持アイテムリスト情報は、ゲーム中で全て共通。なので、破壊されないようにしておく。

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        excel_itemdatabase = Resources.Load("Excel/Entity_ItemDataBase") as Entity_ItemDataBase; //エクセルからアイテムIDを取得し、プレイヤーアイテムリストのIDと同期。

        excel_eventitemdatabase = Resources.Load("Excel/Entity_eventItemDataBase") as Entity_eventItemDataBase; //エクセルからアイテムIDを取得し、プレイヤーアイテムリストのIDと同期。

        _id = 0;
        sheet_no = 0;

        //エクセルのアイテムIDを順番に読み取り、プレイヤー所持リストに割り当て。
        while (sheet_no < excel_itemdatabase.sheets.Count)
        {
            count = 0;

            while (count < excel_itemdatabase.sheets[sheet_no].list.Count)
            {
                // 一旦代入 IDだけを取る。
                _id = excel_itemdatabase.sheets[sheet_no].list[count].ItemID;

                //ここでリストに追加している
                playeritemlist.Add(_id, 0);

                ++count;
            }

            ++sheet_no;

            if (sheet_no < excel_itemdatabase.sheets.Count)
            {
                sheet_count = _id + 1; //一枚前のシートの要素数をカウント　_idのラストは、例えば2が入っているので、+1すれば、要素数になる。ここでは、前シートの要素数を取得している。

                for (i = 0; i < excel_itemdatabase.sheets[sheet_no].list[0].ItemID - sheet_count; i++) //次のシートの0行目のID番号をみる。例えば300とか。
                {
                    playeritemlist.Add(_id+i+1, 0); //エクセルに登録されていないアイテムID分、空をいれている。
                }
            }
        }

        /*foreach (KeyValuePair<int, int> item in playeritemlist) //ここで使っているitemは、foreach用の変数
        {
            Debug.Log(item.Key + " " + item.Value);
        }*/
        //Debug.Log(playeritemlist.Count);

        _id = 0;
        sheet_no = 0;

        //エクセルのアイテムIDを順番に読み取り、プレイヤー所持リストに割り当て。
        while (sheet_no < excel_eventitemdatabase.sheets.Count)
        {
            count = 0;

            while (count < excel_eventitemdatabase.sheets[sheet_no].list.Count)
            {
                // 一旦代入 IDだけを取る。
                _id = excel_eventitemdatabase.sheets[sheet_no].list[count].ev_ItemID;
                ev_fileName = excel_eventitemdatabase.sheets[sheet_no].list[count].fileName;
                ev_itemName = excel_eventitemdatabase.sheets[sheet_no].list[count].name;
                ev_itemNameHyouji = excel_eventitemdatabase.sheets[sheet_no].list[count].nameHyouji;
                ev_cost = excel_eventitemdatabase.sheets[sheet_no].list[count].cost_price;
                ev_sell = excel_eventitemdatabase.sheets[sheet_no].list[count].sell_price;
                ev_kosu = excel_eventitemdatabase.sheets[sheet_no].list[count].kosu;
                ev_read_flag = excel_eventitemdatabase.sheets[sheet_no].list[count].read_flag;
                ev_list_on = excel_eventitemdatabase.sheets[sheet_no].list[count].list_hyouji_on;
                ev_memo = excel_eventitemdatabase.sheets[sheet_no].list[count].memo;

                //ここでリストに追加している
                eventitemlist.Add(new ItemEvent(_id, ev_fileName, ev_itemName, ev_itemNameHyouji, ev_cost, ev_sell, ev_kosu, ev_read_flag, ev_list_on, ev_memo));

                ++count;
            }

            ++sheet_no;

            if (sheet_no < excel_eventitemdatabase.sheets.Count)
            {
                sheet_count = _id + 1; //一枚前のシートの要素数をカウント　_idのラストは、例えば2が入っているので、+1すれば、要素数になる。ここでは、前シートの要素数を取得している。

                for (i = 0; i < excel_eventitemdatabase.sheets[sheet_no].list[0].ev_ItemID - sheet_count; i++) //次のシートの0行目のID番号をみる。例えば300とか。
                {
                    eventitemlist.Add(new ItemEvent(_id + i + 1, "", "", "", 0, 0, 0, 0, 0, "")); //エクセルに登録されていないアイテムID分、空をいれている。
                }
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addPlayerItem(int itemID, int count_kosu)
    {
        playeritemlist[itemID] = playeritemlist[itemID] + count_kosu;

        if ( playeritemlist[itemID] > 99 )
        {
            playeritemlist[itemID] = 99; //上限 99個
        }
    }

    public void deletePlayerItem(int deleteID, int count_kosu)
    {
        //Debug.Log("itemID: " + deleteID + " 所持数: " + playeritemlist[deleteID] + " を" + count_kosu + "個　消す");
        playeritemlist[deleteID] = playeritemlist[deleteID] - count_kosu;
        
        if (playeritemlist[deleteID] < 0)
        {
            playeritemlist[deleteID] = 0; //下限 0個 
        }
        //Debug.Log("itemID: " + deleteID + " 残り所持数: " + playeritemlist[deleteID]);
    }

    public void add_eventPlayerItem(int ev_id, int count_kosu)
    {

        eventitemlist[ev_id].ev_itemKosu = eventitemlist[ev_id].ev_itemKosu + count_kosu;

        if (eventitemlist[ev_id].ev_itemKosu > 99)
        {
            eventitemlist[ev_id].ev_itemKosu = 99; //上限 99個
        }
    }

    //　トッピングで、調節したオリジナルアイテムを登録する。
    public void addOriginalItem(string _name, int _mp, int _day, int _quality, int _exp, float _ex_probabilty, int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, int _powdery, int _oily, int _watery, int _girl1_like, int _cost, int _sell, string _tp01, string _tp02, string _tp03, string _tp04, string _tp05, string _tp06, string _tp07, string _tp08, string _tp09, string _tp10, int _itemkosu, int extreme_kaisu, int _item_hyouji)
    {
        //トッピングアイテムを追加の際は、アイテム名（_name）＋任意の数字のパラメータ。ファイルネームやアイコンなどは共通なので、データベースから取得。

        //データベースから_nameに一致するものを取得。
        i = 0;

        while (i < database.items.Count)
        {

            if (database.items[i].itemName == _name)
            {
                _id = database.items[i].itemID;　//アイテムIDのこと。
                _comp_hosei = database.items[i].itemComp_Hosei;
                _file_name = database.items[i].fileName;
                _nameHyouji = database.items[i].itemNameHyouji;
                _desc = database.items[i].itemDesc;
                _type = database.items[i].itemType.ToString();
                _subtype = database.items[i].itemType_sub.ToString();
                _koyutp = database.items[i].koyu_toppingtype[0];
                _judge_num = database.items[i].SetJudge_Num;
                _first_eat = database.items[i].First_eat;
                break;
            }
            ++i;
        }

        player_originalitemlist.Add(new Item(_id, _file_name, _name, _nameHyouji, _desc, _comp_hosei, _mp, _day, _quality, _exp, _ex_probabilty, _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _type, _subtype, _girl1_like, _cost, _sell, _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10, _koyutp, _itemkosu, extreme_kaisu, _item_hyouji, _judge_num, _first_eat));
    }

    public void deleteOriginalItem(int _id, int _kosu)
    {
        player_originalitemlist[_id].ItemKosu -= _kosu;

        if (player_originalitemlist[_id].ItemKosu <= 0)
        {
            player_originalitemlist.RemoveAt(_id);
        }
    }
}
