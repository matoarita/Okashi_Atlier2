using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemList : SingletonMonoBehaviour<PlayerItemList>
{

    private Entity_ItemDataBase excel_itemdatabase; //sample_excelクラス。エクセル読み込み時、XMLインポートで生成されたときのクラスを読む。リスト型と同じように扱える。

    private Entity_eventItemDataBase excel_eventitemdatabase; //イベント用アイテムデータベース。

    private ItemDataBase database;

    private Exp_Controller exp_Controller;

    private int _id;
    private string _itemname;
    private int event_id;
    private int _comp_hosei;
    private string _file_name, _nameHyouji, _desc;
    private string _type;
    private string _subtype;
    private int _base_score;
    private string[] _koyutp = new string[5];
    private int _judge_num;
    private int _eat_kaisu;
    private int _highscore_flag;
    private int _lasttotal_score;
    private string _hinttext;
    private int _rare;
    private int _total_kosu;

    private string ev_fileName, ev_itemName, ev_itemNameHyouji;
    private int ev_kosu;
    private int ev_cost, ev_sell;
    private int ev_itemType;
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

    //プレイヤーの所持アイテムリスト。Dictionaryなので、｛アイテム名, 個数｝の関係で格納する。
    public Dictionary<string, int> playeritemlist = new Dictionary<string, int>();

    //プレイヤーのイベントアイテムリスト。
    public List<ItemEvent> eventitemlist = new List<ItemEvent>();

    //エメラルショップなどで購入できるアイテムやイベント関係のアイテム。コスチュームなど。
    public List<ItemEvent> emeralditemlist = new List<ItemEvent>();

    //プレイヤーが作成したオリジナルのアイテムリスト。
    public List<Item> player_originalitemlist = new List<Item>();

    //予測用のオリジナルのアイテムリスト。プレイヤーから触ることはできない架空の所持リスト。
    public List<Item> player_yosokuitemlist = new List<Item>();

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this); //プレイヤーの所持アイテムリスト情報は、ゲーム中で全て共通。なので、破壊されないようにしておく。

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

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
                _itemname = excel_itemdatabase.sheets[sheet_no].list[count].name;

                //ここでリストに追加している
                playeritemlist.Add(_itemname, 0);

                ++count;

            }

            ++sheet_no;

            if (sheet_no < excel_itemdatabase.sheets.Count)
            {
                sheet_count = _id + 1; //一枚前のシートの要素数をカウント　_idのラストは、例えば2が入っているので、+1すれば、要素数になる。ここでは、前シートの要素数を取得している。

                for (i = 0; i < excel_itemdatabase.sheets[sheet_no].list[0].ItemID - sheet_count; i++) //次のシートの0行目のID番号をみる。例えば300とか。シートNoは2枚目から始まり
                {
                    playeritemlist.Add("Non" + (sheet_no-1).ToString() + " " + (sheet_count+i).ToString(), 0); //エクセルに登録されていないアイテムID分、空をいれている。
                }
            }
        }

        /*foreach (KeyValuePair<string, int> item in playeritemlist) //ここで使っているitemは、foreach用の変数
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
        emeralditemlist.Clear();

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
                ev_itemType = excel_eventitemdatabase.sheets[sheet_no].list[count].item_Type;
                ev_list_on = excel_eventitemdatabase.sheets[sheet_no].list[count].list_hyouji_on;
                ev_memo = excel_eventitemdatabase.sheets[sheet_no].list[count].memo;
                ev_reflag_num = excel_eventitemdatabase.sheets[sheet_no].list[count].Re_flag_num;
                ev_evflag_num = excel_eventitemdatabase.sheets[sheet_no].list[count].Ev_flag_num;

                //ここでリストに追加している
                eventitemlist.Add(new ItemEvent(_id, ev_fileName, ev_itemName, ev_itemNameHyouji, ev_cost, ev_sell, ev_kosu, ev_read_flag, ev_itemType, ev_list_on, ev_memo, ev_reflag_num, ev_evflag_num));

                ++count;
            }

            ++sheet_count;

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
                ev_itemType = excel_eventitemdatabase.sheets[sheet_no].list[count].item_Type;
                ev_list_on = excel_eventitemdatabase.sheets[sheet_no].list[count].list_hyouji_on;
                ev_memo = excel_eventitemdatabase.sheets[sheet_no].list[count].memo;
                ev_reflag_num = excel_eventitemdatabase.sheets[sheet_no].list[count].Re_flag_num;
                ev_evflag_num = excel_eventitemdatabase.sheets[sheet_no].list[count].Ev_flag_num;

                //ここでリストに追加している
                emeralditemlist.Add(new ItemEvent(_id, ev_fileName, ev_itemName, ev_itemNameHyouji, ev_cost, ev_sell, ev_kosu, ev_read_flag, ev_itemType, ev_list_on, ev_memo, ev_reflag_num, ev_evflag_num));

                ++count;
            }

            ++sheet_count;

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //アイテムID＋個数で、追加
    public void addPlayerItem(string itemName, int count_kosu)
    {
        playeritemlist[itemName] = playeritemlist[itemName] + count_kosu;

        if ( playeritemlist[itemName] > 99 )
        {
            playeritemlist[itemName] = 99; //上限 99個
        }
    }

    //アイテム名＋個数で、追加
    public void addPlayerItemString(string itemName, int count_kosu)
    {
        addPlayerItem(itemName, count_kosu);
       
    }

    //アイテム名＋個数で、指定した個数に変更する。（加算とは別。）
    public void ReSetPlayerItemString(string itemName, int count_kosu)
    {
        for(i=0; i < database.items.Count; i++) //前回セーブしたときのアイテムが、今のアイテムDBにも残っていたら、個数を上書き。
        {
            if(database.items[i].itemName == itemName)
            {
                playeritemlist[itemName] = count_kosu;
            }
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
        _total_kosu = 0;
        _total_kosu += playeritemlist[itemName];

        //オリジナルアイテムリストも見る。

        for (i = 0; i < player_originalitemlist.Count; i++)
        {
            if (player_originalitemlist[i].itemName == itemName)
            {
                if (player_originalitemlist[i].ItemKosu > 0)
                {
                    _total_kosu += player_originalitemlist[i].ItemKosu;
                }
            }
        }

        return _total_kosu; //0個　持っていないときは、0
    }

    //アイテムリストに、名前をいれると、アイテムリスト・オリジナルアイテムリストのどちらかに所持していた場合は、削除するメソッド
    public void SearchDeleteItem(string itemName)
    {
        //先にアイテムリストをみて、ない場合オリジナルアイテムリストを見る。
        if (playeritemlist[itemName] > 0)
        {
            deletePlayerItem(itemName, 1);
        }
        else
        {
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
     
    }



    public void deletePlayerItem(string delete_itemName, int count_kosu)
    {
        //Debug.Log("itemID: " + deleteID + " 所持数: " + playeritemlist[deleteID] + " を" + count_kosu + "個　消す");
        playeritemlist[delete_itemName] = playeritemlist[delete_itemName] - count_kosu;
        
        if (playeritemlist[delete_itemName] < 0)
        {
            playeritemlist[delete_itemName] = 0; //下限 0個 
        }
        //Debug.Log("itemID: " + deleteID + " 残り所持数: " + playeritemlist[deleteID]);
    }

    //イベントアイテムを追加
    public void add_eventPlayerItem(int ev_id, int count_kosu)
    {

        eventitemlist[ev_id].ev_itemKosu = eventitemlist[ev_id].ev_itemKosu + count_kosu;

        if (eventitemlist[ev_id].ev_itemKosu > 99)
        {
            eventitemlist[ev_id].ev_itemKosu = 99; //上限 99個
        }
    }

    //イベントアイテム名＋個数で、指定した個数に変更する。フラグも更新できる。
    public void ReSetEventItemString(string itemName, int count_kosu, int _read_flag)
    {
        i = 0;
        while (i < eventitemlist.Count)
        {
            if(eventitemlist[i].event_itemName == itemName)
            {
                eventitemlist[i].ev_itemKosu = count_kosu;
                eventitemlist[i].ev_ReadFlag = _read_flag;
                break;
            }           
            i++;
        }
    }

    //エメラルドアイテムを追加
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

    //エメラルドアイテム名＋個数で、指定した個数に変更する。
    public void ReSetEmeraldItemString(string itemName, int count_kosu)
    {
        i = 0;
        while (i < emeralditemlist.Count)
        {
            if (emeralditemlist[i].event_itemName == itemName)
            {
                emeralditemlist[i].ev_itemKosu = count_kosu;
                break;
            }
            i++;
        }
    }

    //トッピングで、調節したオリジナルアイテムを登録する。
    public void addOriginalItem(string _name, int _mp, int _day, int _quality, int _exp, float _ex_probabilty, 
        int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, int _powdery, int _oily, int _watery, int _beauty,
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
                _rare = database.items[i].Rare;

                for ( k=0; k < _koyutp.Length; k++)
                {
                    _koyutp[k] = database.items[i].koyu_toppingtype[k];
                }
                break;
            }
            ++i;
        }

        player_originalitemlist.Add(new Item(_id, _file_name, _name, _nameHyouji, _desc, _comp_hosei, _mp, _day, _quality, _exp, _ex_probabilty, 
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _beauty, _type, _subtype, _base_score, _girl1_like, _cost, _sell, 
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10, _koyutp[0], _koyutp[1], _koyutp[2], _koyutp[3], _koyutp[4],
            _itemkosu, extreme_kaisu, _item_hyouji, _judge_num, _eat_kaisu, _highscore_flag, _lasttotal_score, _hinttext, _total_kyori, _rare));
    }

    //予測表示用オリジナルアイテムを登録する。
    public void addYosokuOriginalItem(string _name, int _mp, int _day, int _quality, int _exp, float _ex_probabilty,
        int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, int _powdery, int _oily, int _watery, int _beauty,
        float _girl1_like, int _cost, int _sell,
        string _tp01, string _tp02, string _tp03, string _tp04, string _tp05, string _tp06, string _tp07, string _tp08, string _tp09, string _tp10,
        int _itemkosu, int extreme_kaisu, int _item_hyouji, float _total_kyori)
    {

        player_yosokuitemlist.Clear();

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
                _rare = database.items[i].Rare;

                for (k = 0; k < _koyutp.Length; k++)
                {
                    _koyutp[k] = database.items[i].koyu_toppingtype[k];
                }
                break;
            }
            ++i;
        }

        player_yosokuitemlist.Add(new Item(_id, _file_name, _name, _nameHyouji, _desc, _comp_hosei, _mp, _day, _quality, _exp, _ex_probabilty,
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _beauty, _type, _subtype, _base_score, _girl1_like, _cost, _sell,
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10, _koyutp[0], _koyutp[1], _koyutp[2], _koyutp[3], _koyutp[4],
            _itemkosu, extreme_kaisu, _item_hyouji, _judge_num, _eat_kaisu, _highscore_flag, _lasttotal_score, _hinttext, _total_kyori, _rare));
    }

    //指定したIDのオリジナルアイテムを削除する
    public void deleteOriginalItem(int _id, int _kosu)
    {
        player_originalitemlist[_id].ItemKosu -= _kosu;

        //extremepanelにセットされていたアイテムを消した場合は、
        //expanel上のアイテムも消える。
        if (exp_Controller._temp_extremeSetting)
        {
            //パネルのアイテムを直接削除した場合
            if (_id == exp_Controller._temp_extreme_id && player_originalitemlist[_id].ItemKosu <= 0)
            {
                exp_Controller._temp_extremeSetting = false;
                exp_Controller._temp_extreme_id = 9999;
            }
            //エクストリームパネルにアイテムが設定されているかつ、オリジナルアイテムを削除した場合、列が一個ずれる
            else if (_id != exp_Controller._temp_extreme_id && player_originalitemlist[_id].ItemKosu <= 0)
            {
                exp_Controller._temp_extreme_id--;
            }
        }

        //0以下になったら、リストそのものから削除する。
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
                _itemcount = playeritemlist[_itemname];
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

        return _itemcount; //該当するIDがない場合 0
    }

    //アイテム名を入力すると、イベントアイテムの現在の所持数を返す処理
    public int KosuCountEvent(string _itemname)
    {
        i = 0;
        _itemcount = 0;

        while (i < eventitemlist.Count)
        {
            if (eventitemlist[i].event_itemName == _itemname)
            {
                _itemcount = eventitemlist[i].ev_itemKosu;
                Debug.Log("アイテム名: " + eventitemlist[i].event_itemName + "　所持数: " + eventitemlist[i].ev_itemKosu);
                break;
            }
            i++;
        }

        return _itemcount; //該当するIDがない場合 0
    }

    //アイテム名を入力すると、イベントアイテムを読んだことにする処理
    public void EventReadOn(string _itemname)
    {
        i = 0;

        while (i < eventitemlist.Count)
        {
            if (eventitemlist[i].event_itemName == _itemname)
            {
                eventitemlist[i].ev_ReadFlag = 1;
                break;
            }
            i++;
        }
    }


    //イベントアイテムのリストを参照。
    public void eventitemlist_Sansho()
    {
        for (i = 0; i < eventitemlist.Count; i++)
        {
            Debug.Log("アイテム名: " + eventitemlist[i].event_itemName + "　所持数: " + eventitemlist[i].ev_itemKosu);
        }
    }

    //アイテム名を入力すると、該当するemeralditem_IDを返す処理
    public int Find_emeralditemdatabase(string emerald_itemname)
    {
        j = 0;
        while (j < emeralditemlist.Count)
        {
            if (emerald_itemname == emeralditemlist[j].event_itemName)
            {
                return j;
            }
            j++;
        }

        return 9999; //該当するIDがない場合
    }

    //アイテム名を入力すると、エメラルドアイテムの現在の所持数を返す処理
    public int KosuCountEmerald(string _itemname)
    {
        i = 0;
        _itemcount = 0;

        while (i < eventitemlist.Count)
        {
            if (emeralditemlist[i].event_itemName == _itemname)
            {
                _itemcount = emeralditemlist[i].ev_itemKosu;
                //Debug.Log("アイテム名: " + emeralditemlist[i].event_itemName + "　所持数: " + emeralditemlist[i].ev_itemKosu);
                break;
            }
            i++;
        }

        return _itemcount; //該当するIDがない場合 0
    }

    //エメラルドアイテムの衣装アイテムの総所持数をカウント
    public int emeralditemlist_CostumeCount()
    {
        _itemcount = 0;

        for (i = 0; i < emeralditemlist.Count; i++)
        {
            if(emeralditemlist[i].ev_itemType == 1 && emeralditemlist[i].ev_itemKosu >= 1)
            {
                _itemcount++;
            }
            //Debug.Log("アイテム名: " + emeralditemlist[i].event_itemName + "　所持数: " + emeralditemlist[i].ev_itemKosu);
        }

        return _itemcount;
    }

    //エメラルドアイテムの衣装アイテムの総数をカウント
    public int emeralditemlist_CostumeAllCount()
    {
        _itemcount = 0;

        for (i = 0; i < emeralditemlist.Count; i++)
        {
            if (emeralditemlist[i].ev_itemType == 1 && emeralditemlist[i].ev_ListOn == 1)
            {
                _itemcount++;
            }
            //Debug.Log("アイテム名: " + emeralditemlist[i].event_itemName + "　所持数: " + emeralditemlist[i].ev_itemKosu);
        }

        return _itemcount;
    }

    //アイテムIDを入力すると、エメラルドアイテム名を返す処理
    public string NameFindEmerald(int _itemid)
    {
        i = 0;
        _itemcount = 0;

        while (i < eventitemlist.Count)
        {
            if (emeralditemlist[i].ev_ItemID == _itemid)
            {
                _itemcount = emeralditemlist[i].ev_itemKosu;
                Debug.Log("アイテム名: " + emeralditemlist[i].event_itemName + "　所持数: " + emeralditemlist[i].ev_itemKosu);


                return emeralditemlist[i].event_itemName;
            }
            i++;
        }

        return ""; //該当するIDがない場合 0
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
