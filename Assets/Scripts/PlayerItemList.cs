using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemList : SingletonMonoBehaviour<PlayerItemList>
{

    private Entity_ItemDataBase excel_itemdatabase; //sample_excelクラス。エクセル読み込み時、XMLインポートで生成されたときのクラスを読む。リスト型と同じように扱える。

    private Entity_eventItemDataBase excel_eventitemdatabase; //イベント用アイテムデータベース。

    private ItemDataBase database;

    private int _id;
    private int event_id;
    private int _comp_hosei;
    private string _file_name, _nameHyouji, _desc;
    private string _type;
    private string _subtype;
    private int _base_score;
    private string[] _koyutp = new string[5];
    private int _judge_num;
    private int _eat_kaisu;
    private bool _highscore_flag;
    private int _lasttotal_score;
    private string _hinttext;

    private string ev_fileName, ev_itemName, ev_itemNameHyouji;
    private int ev_kosu;
    private int ev_cost, ev_sell;
    private int ev_read_flag; //そのレシピを読み終えたかどうかをチェックするフラグ
    private int ev_list_on; //レシピリストに、表示するか否か。1の場合、リストに表示され、使用すると、そのレシピの内容を読むことができる。
    private string ev_memo;
    private int ev_reflag_num;
    private int ev_evflag_num;

    private int i, j, k;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    private int _itemcount;
    private int _itemid;

    //プレイヤーの所持アイテムリスト。Dictionaryなので、｛アイテムID, 個数｝の関係で格納する。
    public Dictionary<int, int> playeritemlist = new Dictionary<int, int>();

    //プレイヤーのイベントアイテムリスト。
    public List<ItemEvent> eventitemlist = new List<ItemEvent>();

    //エメラルショップなどで購入できるアイテムやイベント関係のアイテム。コスチュームなど。
    public List<ItemEvent> emeralditemlist = new List<ItemEvent>();

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

        ReseteventitemList();
        

    }
    public void ReseteventitemList()
    {
        sheet_no = 0;
        sheet_count = 0;
        eventitemlist.Clear();

        //エクセルのアイテムIDを順番に読み取り、プレイヤー所持リストに割り当て。
        while (sheet_count < 1)
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
                ev_reflag_num = excel_eventitemdatabase.sheets[sheet_no].list[count].Re_flag_num;
                ev_evflag_num = excel_eventitemdatabase.sheets[sheet_no].list[count].Ev_flag_num;

                //ここでリストに追加している
                eventitemlist.Add(new ItemEvent(_id, ev_fileName, ev_itemName, ev_itemNameHyouji, ev_cost, ev_sell, ev_kosu, ev_read_flag, ev_list_on, ev_memo, ev_reflag_num, ev_evflag_num));

                ++count;
            }

            ++sheet_count;

            /*
            if (sheet_no < excel_eventitemdatabase.sheets.Count)
            {
                sheet_count = _id + 1; //一枚前のシートの要素数をカウント　_idのラストは、例えば2が入っているので、+1すれば、要素数になる。ここでは、前シートの要素数を取得している。

                for (i = 0; i < excel_eventitemdatabase.sheets[sheet_no].list[0].ev_ItemID - sheet_count; i++) //次のシートの0行目のID番号をみる。例えば300とか。
                {
                    eventitemlist.Add(new ItemEvent(_id + i + 1, "", "", "", 0, 0, 0, 0, 0, "", 9999, 9999)); //エクセルに登録されていないアイテムID分、空をいれている。
                }
            }
            */
        }

        sheet_no = 1;
        sheet_count = 0;

        while (sheet_count < 1)
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
                ev_reflag_num = excel_eventitemdatabase.sheets[sheet_no].list[count].Re_flag_num;
                ev_evflag_num = excel_eventitemdatabase.sheets[sheet_no].list[count].Ev_flag_num;

                //ここでリストに追加している
                emeralditemlist.Add(new ItemEvent(_id, ev_fileName, ev_itemName, ev_itemNameHyouji, ev_cost, ev_sell, ev_kosu, ev_read_flag, ev_list_on, ev_memo, ev_reflag_num, ev_evflag_num));

                ++count;
            }

            ++sheet_count;

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //アイテムID＋個数で、追加
    public void addPlayerItem(int itemID, int count_kosu)
    {
        playeritemlist[itemID] = playeritemlist[itemID] + count_kosu;

        if ( playeritemlist[itemID] > 99 )
        {
            playeritemlist[itemID] = 99; //上限 99個
        }
    }

    //アイテム名＋個数で、追加
    public void addPlayerItemString(string itemName, int count_kosu)
    {
        for (i = 0; i <= database.sheet_topendID[1]; i++)
        {
            if (database.items[i].itemName == itemName)
            {
                addPlayerItem(i, count_kosu);
            }
        }

        //お菓子タイプ
        count = database.sheet_topendID[2];

        j = database.sheet_topendID[3] - database.sheet_topendID[2];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                addPlayerItem(count, count_kosu);
            }
            ++count;
        }

        //ポーションタイプ
        count = database.sheet_topendID[4];

        j = database.sheet_topendID[5] - database.sheet_topendID[4];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                addPlayerItem(count, count_kosu);
            }
            ++count;
        }

        //その他タイプ
        count = database.sheet_topendID[6];

        j = database.sheet_topendID[7] - database.sheet_topendID[6];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                addPlayerItem(count, count_kosu);
            }
            ++count;
        }

    }

    //アイテム名＋個数で、指定した個数に変更する。（加算とは別。）
    public void ReSetPlayerItemString(string itemName, int count_kosu)
    {
        for (i = 0; i <= database.sheet_topendID[1]; i++)
        {
            if (database.items[i].itemName == itemName)
            {
                playeritemlist[i] = count_kosu;
            }
        }

        //お菓子タイプ
        count = database.sheet_topendID[2];

        j = database.sheet_topendID[3] - database.sheet_topendID[2];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                playeritemlist[count] = count_kosu;
            }
            ++count;
        }

        //ポーションタイプ
        count = database.sheet_topendID[4];

        j = database.sheet_topendID[5] - database.sheet_topendID[4];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                playeritemlist[count] = count_kosu;
            }
            ++count;
        }

        //その他タイプ
        count = database.sheet_topendID[6];

        j = database.sheet_topendID[7] - database.sheet_topendID[6];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                playeritemlist[count] = count_kosu;
            }
            ++count;
        }

    }

    //アイテム名をいれると、そのアイテムIDを返すメソッド
    public int SearchItemString(string itemName)
    {
        if (itemName == "Non")
        {
            return 9999;
        }
        else
        {
            i = 0;
            while (i <= database.items.Count)
            {
                if (database.items[i].itemName == itemName)
                {
                    return i;
                }
                i++;
            }

            return 9999; //見つからなかった場合、9999
        }
    }


    //アイテムリストに、名前をいれると、所持個数を返してくれるメソッド
    public int ReturnItemKosu(string itemName)
    {
        for (i = 0; i <= database.sheet_topendID[1]; i++)
        {
            if (database.items[i].itemName == itemName)
            {
                if (database.items[i].ItemKosu > 0)
                {
                    return database.items[i].ItemKosu;
                }
            }
        }

        //お菓子タイプ
        count = database.sheet_topendID[2];

        j = database.sheet_topendID[3] - database.sheet_topendID[2];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                if (database.items[count].ItemKosu > 0)
                {
                    return database.items[count].ItemKosu;
                }
            }
            ++count;
        }

        //ポーションタイプ
        count = database.sheet_topendID[4];

        j = database.sheet_topendID[5] - database.sheet_topendID[4];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                if (database.items[count].ItemKosu > 0)
                {
                    return database.items[count].ItemKosu;
                }
            }
            ++count;
        }

        //その他タイプ
        count = database.sheet_topendID[6];

        j = database.sheet_topendID[7] - database.sheet_topendID[6];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                if (database.items[count].ItemKosu > 0)
                {
                    return database.items[count].ItemKosu;
                }
            }
            ++count;
        }

        //オリジナルアイテムリストも見る。

        for(i=0; i < player_originalitemlist.Count; i++)
        {
            if( player_originalitemlist[i].itemName == itemName)
            {
                if (player_originalitemlist[i].ItemKosu > 0)
                {
                    return player_originalitemlist[i].ItemKosu;
                }
            }
        }
        
        return 9999; //0個　持っていないときは、9999がかえる。
    }

    //アイテムリストに、名前をいれると、アイテムリスト・オリジナルアイテムリストのどちらかに所持していた場合は、削除するメソッド
    public void SearchDeleteItem(string itemName)
    {
        for (i = 0; i <= database.sheet_topendID[1]; i++)
        {
            if (database.items[i].itemName == itemName)
            {
                if (database.items[i].ItemKosu > 0)
                {
                    deletePlayerItem(i, 1);
                }
            }
        }

        //お菓子タイプ
        count = database.sheet_topendID[2];

        j = database.sheet_topendID[3] - database.sheet_topendID[2];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                if (database.items[count].ItemKosu > 0)
                {
                    deletePlayerItem(count, 1);
                }
            }
            ++count;
        }

        //ポーションタイプ
        count = database.sheet_topendID[4];

        j = database.sheet_topendID[5] - database.sheet_topendID[4];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                if (database.items[count].ItemKosu > 0)
                {
                    deletePlayerItem(count, 1);
                }
            }
            ++count;
        }

        //その他タイプ
        count = database.sheet_topendID[6];

        j = database.sheet_topendID[7] - database.sheet_topendID[6];

        for (i = 0; i <= j; i++)
        {
            if (database.items[count].itemName == itemName)
            {
                if (database.items[count].ItemKosu > 0)
                {
                    deletePlayerItem(count, 1);
                }
            }
            ++count;
        }

        //オリジナルアイテムリストも見る。

        for (i = 0; i < player_originalitemlist.Count; i++)
        {
            if (player_originalitemlist[i].itemName == itemName)
            {
                if (player_originalitemlist[i].ItemKosu > 0)
                {
                    deleteOriginalItem(i, 1);
                }
            }
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

    public void add_EmeraldPlayerItem(int ev_id, int count_kosu)
    {

        emeralditemlist[ev_id].ev_itemKosu = emeralditemlist[ev_id].ev_itemKosu + count_kosu;

        if (emeralditemlist[ev_id].ev_itemKosu > 99)
        {
            emeralditemlist[ev_id].ev_itemKosu = 99; //上限 99個
        }
    }

    public void add_eventPlayerItemString(string itemName, int count_kosu)
    {
        event_id = Find_eventitemdatabase(itemName);
        add_eventPlayerItem(event_id, count_kosu);
    }

    //　トッピングで、調節したオリジナルアイテムを登録する。
    public void addOriginalItem(string _name, int _mp, int _day, int _quality, int _exp, float _ex_probabilty, 
        int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, int _powdery, int _oily, int _watery, 
        float _girl1_like, int _cost, int _sell, 
        string _tp01, string _tp02, string _tp03, string _tp04, string _tp05, string _tp06, string _tp07, string _tp08, string _tp09, string _tp10, 
        int _itemkosu, int extreme_kaisu, int _item_hyouji, float _total_kyori)
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
                _base_score = database.items[i].Base_Score;
                _judge_num = database.items[i].SetJudge_Num;
                _eat_kaisu = database.items[i].Eat_kaisu;
                _highscore_flag = database.items[i].HighScore_flag;
                _lasttotal_score = database.items[i].last_total_score;
                _hinttext = database.items[i].last_hinttext;

                for( k=0; k < _koyutp.Length; k++)
                {
                    _koyutp[k] = database.items[i].koyu_toppingtype[k];
                }
                break;
            }
            ++i;
        }

        player_originalitemlist.Add(new Item(_id, _file_name, _name, _nameHyouji, _desc, _comp_hosei, _mp, _day, _quality, _exp, _ex_probabilty, 
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _type, _subtype, _base_score, _girl1_like, _cost, _sell, 
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10, _koyutp[0], _koyutp[1], _koyutp[2], _koyutp[3], _koyutp[4],
            _itemkosu, extreme_kaisu, _item_hyouji, _judge_num, _eat_kaisu, _highscore_flag, _lasttotal_score, _hinttext, _total_kyori));
    }

    //指定したIDのオリジナルアイテムを削除する
    public void deleteOriginalItem(int _id, int _kosu)
    {
        player_originalitemlist[_id].ItemKosu -= _kosu;

        if (player_originalitemlist[_id].ItemKosu <= 0)
        {
            player_originalitemlist.RemoveAt(_id);
        }
    }

    //アイテム名を入力すると、該当するeventitem_IDを返す処理
    public int Find_eventitemdatabase(string compo_itemname)
    {
        j = 0;
        while (j < eventitemlist.Count)
        {
            if (compo_itemname == eventitemlist[j].event_itemName)
            {
                return j;
            }
            j++;
        }

        return 9999; //該当するIDがない場合
    }

    //アイテム名を入力すると、現在の所持数を返す処理。（店売り＋オリジナル）
    public int KosuCount(string _itemname)
    {
        i = 0;
        _itemcount = 0;

        while ( i < database.items.Count)
        {
            if(database.items[i].itemName == _itemname)
            {
                _itemcount = playeritemlist[i];
                _itemid = i;
                break;
            }
            i++;
        }
        
        for (i = 0; i < player_originalitemlist.Count; i++)
        {
            if (database.items[_itemid].itemName == player_originalitemlist[i].itemName)
            {
                _itemcount += player_originalitemlist[i].ItemKosu;
            }
        }

        return _itemcount; //該当するIDがない場合
    }

    //イベントアイテムのリストを参照。
    public void eventitemlist_Sansho()
    {
        for (i = 0; i < eventitemlist.Count; i++)
        {
            Debug.Log("アイテム名: " + eventitemlist[i].event_itemName + "　所持数: " + eventitemlist[i].ev_itemKosu);
        }
    }

    //エメラルドアイテムのリストを参照。
    public void emeralditemlist_Sansho()
    {
        for(i=0; i < emeralditemlist.Count; i++)
        {
            Debug.Log("アイテム名: " + emeralditemlist[i].event_itemName + "　所持数: " + emeralditemlist[i].ev_itemKosu);
        }
    }
}
