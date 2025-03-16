using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerItemList : SingletonMonoBehaviour<PlayerItemList>
{

    private Entity_ItemDataBase excel_itemdatabase; //sample_excelクラス。エクセル読み込み時、XMLインポートで生成されたときのクラスを読む。リスト型と同じように扱える。

    private Entity_eventItemDataBase excel_eventitemdatabase; //イベント用アイテムデータベース。

    private ItemDataBase database;

    private Exp_Controller exp_Controller;

    private DateTime TodayNow;

    private int _id;
    private string _itemname;
    private int event_id;
    private int _comp_hosei;
    private string _file_name, _nameHyouji, _desc;
    private string _type;
    private string _subtype;
    private string _subtypeB;
    private string _subtype_category;
    private float _best_welldone;
    private int _base_score;
    private string[] _koyutp = new string[5];
    private int _judge_num;
    private int _eat_kaisu;
    private int _highscore_flag;
    private int _lasttotal_score;
    private string _hinttext;
    private int _rare;
    private int _manpuku;
    private int _magic;    
    private int _noteat;
    private int _secretFlag;
    private string _ms1_mariage;
    private int _ms1_pointup;
    private string _ms2_mariage;
    private int _ms2_pointup;
    private string _ms3_mariage;
    private int _ms3_pointup;
    private string _ms4_mariage;
    private int _ms4_pointup;
    private string _ms5_mariage;
    private int _ms5_pointup;
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
    private int tempID;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号
    private bool check_itemlist;

    private int _itemcount;
    private int _itemid;

    private bool delete_koyuID_flag;
    private bool break_on;

    private string _original_id_string;

    //プレイヤーの所持アイテムリスト。Dictionaryなので、｛アイテム名, 個数｝の関係で格納する。
    public Dictionary<string, int> playeritemlist = new Dictionary<string, int>();

    //プレイヤーのイベントアイテムリスト。
    public List<ItemEvent> eventitemlist = new List<ItemEvent>();

    //エメラルショップなどで購入できるアイテムやイベント関係のアイテム。コスチュームなど。
    public List<ItemEvent> emeralditemlist = new List<ItemEvent>();

    //プレイヤーが作成したオリジナルのアイテムリスト。
    public List<Item> player_originalitemlist = new List<Item>();

    //お菓子パネルにセッティングするアイテムリスト。基本的に、このリストには一個しかアイテムが入らないように使う。
    public List<Item> player_extremepanel_itemlist = new List<Item>();

    //ヒカリお菓子制作用。予測のオリジナルのアイテムリスト。プレイヤーから触ることはできない架空の所持リスト。
    public List<Item> player_yosokuitemlist = new List<Item>();

    //お菓子パネルにセッティングするアイテムチェック用のリスト。こちらも保存されず、架空のtemp所持リストで扱う。
    public List<Item> player_check_itemlist = new List<Item>();

    //コンテスト素材持ち込み不可の場合、預かり用の一時アイテムリスト(エクストリームは、コンテスト出場時にオリジナルへ移動するので不要)
    public Dictionary<string, int> keep_playeritemlist = new Dictionary<string, int>();
    public List<Item> keep_player_originalitemlist = new List<Item>();
    public Dictionary<string, int> temp_dic = new Dictionary<string, int>(); //素材のみOKなどの選別用

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
        delete_koyuID_flag = false;

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

                //Debug.Log(" _itemname: " + _itemname);
            }

            ++sheet_no;
            /*
            if (sheet_no < excel_itemdatabase.sheets.Count)
            {
                sheet_count = _id + 1; //一枚前のシートの要素数をカウント　_idのラストは、例えば2が入っているので、+1すれば、要素数になる。ここでは、前シートの要素数を取得している。

                for (i = 0; i < excel_itemdatabase.sheets[sheet_no].list[0].ItemID - sheet_count; i++) //次のシートの0行目のID番号をみる。例えば300とか。シートNoは2枚目から始まり
                {
                    playeritemlist.Add("Non" + (sheet_no-1).ToString() + " " + (sheet_count+i).ToString(), 0); //エクセルに登録されていないアイテムID分、空をいれている。
                }
            }*/
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
        //Debug.Log("itemName: " + itemName);
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

        //エクストリームパネルもみる
        for (i = 0; i < player_extremepanel_itemlist.Count; i++)
        {
            if (player_extremepanel_itemlist[i].itemName == itemName)
            {
                if (player_extremepanel_itemlist[i].ItemKosu > 0)
                {
                    _total_kosu += player_extremepanel_itemlist[i].ItemKosu;
                }
            }
        }

        return _total_kosu; //0個　持っていないときは、0
    }

    //イベントアイテムリストに、名前をいれると、所持個数を返してくれるメソッド
    public int ReturnEventItemKosu(string itemName)
    {
        _total_kosu = 0;

        //オリジナルアイテムリストも見る。
        for (i = 0; i < eventitemlist.Count; i++)
        {
            if (eventitemlist[i].event_itemName == itemName)
            {
                if (eventitemlist[i].ev_itemKosu > 0)
                {
                    _total_kosu += eventitemlist[i].ev_itemKosu;
                }
            }
        }

        return _total_kosu; //0個　持っていないときは、0
    }

    //アイテムリストに、固有ID（string）をいれると、配列番号を返すメソッド オリジナルとお菓子パネル両方を見る
    public int ReturnOriginalKoyuIDtoItemID(string originalID)
    {
        //オリジナルアイテムリストも見る。
        for (i = 0; i < player_originalitemlist.Count; i++)
        {
            //Debug.Log("player_originalitemlist[i].OriginalitemID: " + i + " " + player_originalitemlist[i].OriginalitemID);
            if (player_originalitemlist[i].OriginalitemID == originalID)
            {
                return i;
            }
        }

        //エクストリームパネルもみる
        for (i = 0; i < player_extremepanel_itemlist.Count; i++)
        {
            //Debug.Log("player_extremepanel_itemlist[i].OriginalitemID: " + i + " " + player_extremepanel_itemlist[i].OriginalitemID);
            if (player_extremepanel_itemlist[i].OriginalitemID == originalID)
            {
                return i;
            }
        }

        return 9999; //なかったときは9999
    }

    //アイテムリストに、固有ID（string）をいれると、そのアイテムが今どのアイテムタイプに所属するかを返す　オリジナルかお菓子パネルか
    public int ReturnOriginalKoyuIDtoItemType(string originalID)
    {
        //オリジナルアイテムリストも見る。
        for (i = 0; i < player_originalitemlist.Count; i++)
        {
            if (player_originalitemlist[i].OriginalitemID == originalID)
            {
                return 1;
            }
        }

        //エクストリームパネルもみる
        for (i = 0; i < player_extremepanel_itemlist.Count; i++)
        {
            if (player_extremepanel_itemlist[i].OriginalitemID == originalID)
            {
                return 2;
            }
        }

        return 9999; //なかったときは9999
    }

    //オリジナルorエクストリームパネルのお菓子をみて、固有IDがないものは自動で削除するメソッド
    public void DeleteETCOriginalKoyuIDItem()
    {
        //オリジナルアイテムリストを順番に見る。
        delete_koyuID_flag = false;
        while (!delete_koyuID_flag)
        {
            i = 0;
            break_on = false;
            while (i < player_originalitemlist.Count)
            {
                if (player_originalitemlist[i].OriginalitemID == "")
                {
                    break_on = true;
                    deleteOriginalItem(i, 99);                    
                    break;
                }
                i++;
            }

            if(!break_on)
            {
                delete_koyuID_flag = true;
            }
        }

        //エクストリームパネルもみる
        for (i = 0; i < player_extremepanel_itemlist.Count; i++)
        {
            if (player_extremepanel_itemlist[i].OriginalitemID == "")
            {
                deleteAllExtremePanelItem();
            }
        }
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
            check_itemlist = false;

            //オリジナルアイテムリストも見る。
            for (i = 0; i < player_originalitemlist.Count; i++)
            {
                if (player_originalitemlist[i].itemName == itemName)
                {
                    if (player_originalitemlist[i].ItemKosu > 0)
                    {
                        deleteOriginalItem(i, 1);
                        check_itemlist = true;
                    }
                }
            }

            //オリジナルアイテムリストにアイテムが見つからなかった場合　エクストリームパネルもみる。
            if(!check_itemlist)
            {
                for (i = 0; i < player_extremepanel_itemlist.Count; i++)
                {
                    if (player_extremepanel_itemlist[i].itemName == itemName)
                    {
                        if (player_extremepanel_itemlist[i].ItemKosu > 0)
                        {
                            deleteExtremePanelItem(i, 1);
                            check_itemlist = true;
                        }
                    }
                }
            }
        }    

        //特に見当たらない場合、処理は無視して終了
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

    //イベントアイテムIDをいれると、そのアイテムの(配列番号)を返すメソッド
    public int SearchEventItemID(int _itemID)
    {

        i = 0;
        while (i <= eventitemlist.Count)
        {
            if (eventitemlist[i].ev_ItemID == _itemID)
            {
                return i;
            }
            i++;
        }

        return 9999; //見つからなかった場合、9999

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

    //エメラルドアイテムIDをいれると、そのアイテムの(配列番号)を返すメソッド
    public int SearchEmeraldItemID(int _itemID)
    {

        i = 0;
        while (i <= emeralditemlist.Count)
        {
            if (emeralditemlist[i].ev_ItemID == _itemID)
            {
                return i;
            }
            i++;
        }

        return 9999; //見つからなかった場合、9999

    }

    //エメラルドアイテム名前をいれると、そのアイテムの(配列番号)を返すメソッド
    public int SearchEmeraldItemStringID(string _itemName)
    {

        i = 0;
        while (i <= emeralditemlist.Count)
        {
            if (emeralditemlist[i].event_itemName == _itemName)
            {
                return i;
            }
            i++;
        }

        return 9999; //見つからなかった場合、9999

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

    //イベントアイテム名＋個数で、個数分増やす
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

    //オリジナルアイテムを登録する。
    public void addOriginalItem(string _name, int _mp, int _day, int _quality, int _exp, float _ex_probabilty, 
        int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, int _powdery, int _oily, int _watery, int _beauty,
        int _juice, int _tea_flavor,
        int _sp_wind, int _sp_score2, int _sp_score3, int _sp_score4, int _sp_score5, int _sp_score6, int _sp_score7, int _sp_score8, int _sp_score9, int _sp_score10,
        float _girl1_like, int _cost, int _sell, 
        string _tp01, string _tp02, string _tp03, string _tp04, string _tp05, string _tp06, string _tp07, string _tp08, string _tp09, string _tp10, 
        int _itemkosu, int extreme_kaisu, int _item_hyouji, float _total_kyori, int _magic,
        string _MS01, string _MS02, string _MS03, string _MS04, string _MS05, string _MS06, string _MS07, string _MS08, string _MS09, string _MS10,
        int MSvalue01, int MSvalue02, int MSvalue03, int MSvalue04, int MSvalue05, int MSvalue06, int MSvalue07, int MSvalue08, int MSvalue09, int MSvalue10,
        int Attri1, int Attri2, int Attri3)
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
                _subtypeB = database.items[i].itemType_subB.ToString();
                _subtype_category = database.items[i].itemType_sub_category;
                _best_welldone = database.items[i].Best_Welldone;
                _base_score = database.items[i].Base_Score;
                _judge_num = database.items[i].SetJudge_Num;
                _eat_kaisu = database.items[i].Eat_kaisu;
                _highscore_flag = database.items[i].HighScore_flag;
                _lasttotal_score = database.items[i].last_total_score;
                _hinttext = database.items[i].last_hinttext;
                _rare = database.items[i].Rare;
                _manpuku = database.items[i].Manpuku;
                //_magic = database.items[i].Magic;                
                _noteat = database.items[i].NotEat;
                _secretFlag = database.items[i].SecretFlag;
                _ms1_mariage = database.items[i].MS1_mariage;
                _ms1_pointup = database.items[i].MS1_pointup;
                _ms2_mariage = database.items[i].MS2_mariage;
                _ms2_pointup = database.items[i].MS2_pointup;
                _ms3_mariage = database.items[i].MS3_mariage;
                _ms3_pointup = database.items[i].MS3_pointup;
                _ms4_mariage = database.items[i].MS4_mariage;
                _ms4_pointup = database.items[i].MS4_pointup;
                _ms5_mariage = database.items[i].MS5_mariage;
                _ms5_pointup = database.items[i].MS5_pointup;

                for ( k=0; k < _koyutp.Length; k++)
                {
                    _koyutp[k] = database.items[i].koyu_toppingtype[k];
                }
                break;
            }
            ++i;
        }

        KoyuID_Set(); //固有IDの設定

        player_originalitemlist.Add(new Item(_id, _original_id_string, _file_name, _name, _nameHyouji, _desc, _comp_hosei, _mp, _day, _quality, _exp, _ex_probabilty, 
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _beauty, _juice, _tea_flavor,
            _sp_wind, _sp_score2, _sp_score3, _sp_score4, _sp_score5, _sp_score6, _sp_score7, _sp_score8, _sp_score9, _sp_score10,
            _best_welldone,
            _type, _subtype, _subtypeB, _subtype_category, _base_score, _girl1_like, _cost, _sell, 
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10, _koyutp[0], _koyutp[1], _koyutp[2], _koyutp[3], _koyutp[4],
            _itemkosu, extreme_kaisu, _item_hyouji, _judge_num, _eat_kaisu, _highscore_flag, _lasttotal_score, _hinttext, _total_kyori, _rare, _manpuku, _magic,
            _noteat, _secretFlag,
            _MS01, _MS02, _MS03, _MS04, _MS05, _MS06, _MS07, _MS08, _MS09, _MS10,
            MSvalue01, MSvalue02, MSvalue03, MSvalue04, MSvalue05, MSvalue06, MSvalue07, MSvalue08, MSvalue09, MSvalue10,
            Attri1, Attri2, Attri3,
            _ms1_mariage, _ms1_pointup, _ms2_mariage, _ms2_pointup, _ms3_mariage, _ms3_pointup, _ms4_mariage, _ms4_pointup, _ms5_mariage, _ms5_pointup));
    }

    //エクストリームパネル設定用アイテムを登録する。
    public void addExtremeItem(string _name, int _mp, int _day, int _quality, int _exp, float _ex_probabilty,
        int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, int _powdery, int _oily, int _watery, int _beauty,
        int _juice, int _tea_flavor,
        int _sp_wind, int _sp_score2, int _sp_score3, int _sp_score4, int _sp_score5, int _sp_score6, int _sp_score7, int _sp_score8, int _sp_score9, int _sp_score10,
        float _girl1_like, int _cost, int _sell,
        string _tp01, string _tp02, string _tp03, string _tp04, string _tp05, string _tp06, string _tp07, string _tp08, string _tp09, string _tp10,
        int _itemkosu, int extreme_kaisu, int _item_hyouji, float _total_kyori, int _magic,
        string _MS01, string _MS02, string _MS03, string _MS04, string _MS05, string _MS06, string _MS07, string _MS08, string _MS09, string _MS10,
        int MSvalue01, int MSvalue02, int MSvalue03, int MSvalue04, int MSvalue05, int MSvalue06, int MSvalue07, int MSvalue08, int MSvalue09, int MSvalue10,
        int Attri1, int Attri2, int Attri3)
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
                _subtypeB = database.items[i].itemType_subB.ToString();
                _subtype_category = database.items[i].itemType_sub_category;
                _best_welldone = database.items[i].Best_Welldone;
                _base_score = database.items[i].Base_Score;
                _judge_num = database.items[i].SetJudge_Num;
                _eat_kaisu = database.items[i].Eat_kaisu;
                _highscore_flag = database.items[i].HighScore_flag;
                _lasttotal_score = database.items[i].last_total_score;
                _hinttext = database.items[i].last_hinttext;
                _rare = database.items[i].Rare;
                _manpuku = database.items[i].Manpuku;
                //_magic = database.items[i].Magic;               
                _noteat = database.items[i].NotEat;
                _secretFlag = database.items[i].SecretFlag;
                _ms1_mariage = database.items[i].MS1_mariage;
                _ms1_pointup = database.items[i].MS1_pointup;
                _ms2_mariage = database.items[i].MS2_mariage;
                _ms2_pointup = database.items[i].MS2_pointup;
                _ms3_mariage = database.items[i].MS3_mariage;
                _ms3_pointup = database.items[i].MS3_pointup;
                _ms4_mariage = database.items[i].MS4_mariage;
                _ms4_pointup = database.items[i].MS4_pointup;
                _ms5_mariage = database.items[i].MS5_mariage;
                _ms5_pointup = database.items[i].MS5_pointup;

                for (k = 0; k < _koyutp.Length; k++)
                {
                    _koyutp[k] = database.items[i].koyu_toppingtype[k];
                }
                break;
            }
            ++i;
        }

        KoyuID_Set(); //固有IDの設定

        player_extremepanel_itemlist.Add(new Item(_id, _original_id_string, _file_name, _name, _nameHyouji, _desc, _comp_hosei, _mp, _day, _quality, _exp, _ex_probabilty,
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _beauty, _juice, _tea_flavor,
            _sp_wind, _sp_score2, _sp_score3, _sp_score4, _sp_score5, _sp_score6, _sp_score7, _sp_score8, _sp_score9, _sp_score10,
            _best_welldone,
            _type, _subtype, _subtypeB, _subtype_category, _base_score, _girl1_like, _cost, _sell,
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10, _koyutp[0], _koyutp[1], _koyutp[2], _koyutp[3], _koyutp[4],
            _itemkosu, extreme_kaisu, _item_hyouji, _judge_num, _eat_kaisu, _highscore_flag, _lasttotal_score, _hinttext, _total_kyori, _rare, _manpuku, _magic,
            _noteat, _secretFlag,
            _MS01, _MS02, _MS03, _MS04, _MS05, _MS06, _MS07, _MS08, _MS09, _MS10,
            MSvalue01, MSvalue02, MSvalue03, MSvalue04, MSvalue05, MSvalue06, MSvalue07, MSvalue08, MSvalue09, MSvalue10,
            Attri1, Attri2, Attri3,
            _ms1_mariage, _ms1_pointup, _ms2_mariage, _ms2_pointup, _ms3_mariage, _ms3_pointup, _ms4_mariage, _ms4_pointup, _ms5_mariage, _ms5_pointup));
    }

    //ヒカリオリジナルアイテムを登録する。
    public void addYosokuOriginalItem(string _name, int _mp, int _day, int _quality, int _exp, float _ex_probabilty,
        int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, int _powdery, int _oily, int _watery, int _beauty,
        int _juice, int _tea_flavor,
        int _sp_wind, int _sp_score2, int _sp_score3, int _sp_score4, int _sp_score5, int _sp_score6, int _sp_score7, int _sp_score8, int _sp_score9, int _sp_score10,
        float _girl1_like, int _cost, int _sell,
        string _tp01, string _tp02, string _tp03, string _tp04, string _tp05, string _tp06, string _tp07, string _tp08, string _tp09, string _tp10,
        int _itemkosu, int extreme_kaisu, int _item_hyouji, float _total_kyori, int _magic,
        string _MS01, string _MS02, string _MS03, string _MS04, string _MS05, string _MS06, string _MS07, string _MS08, string _MS09, string _MS10,
        int MSvalue01, int MSvalue02, int MSvalue03, int MSvalue04, int MSvalue05, int MSvalue06, int MSvalue07, int MSvalue08, int MSvalue09, int MSvalue10,
        int Attri1, int Attri2, int Attri3)
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
                _subtypeB = database.items[i].itemType_subB.ToString();
                _subtype_category = database.items[i].itemType_sub_category;
                _best_welldone = database.items[i].Best_Welldone;
                _base_score = database.items[i].Base_Score;
                _judge_num = database.items[i].SetJudge_Num;
                _eat_kaisu = database.items[i].Eat_kaisu;
                _highscore_flag = database.items[i].HighScore_flag;
                _lasttotal_score = database.items[i].last_total_score;
                _hinttext = database.items[i].last_hinttext;
                _rare = database.items[i].Rare;
                _manpuku = database.items[i].Manpuku;
                //_magic = database.items[i].Magic;               
                _noteat = database.items[i].NotEat;
                _secretFlag = database.items[i].SecretFlag;
                _ms1_mariage = database.items[i].MS1_mariage;
                _ms1_pointup = database.items[i].MS1_pointup;
                _ms2_mariage = database.items[i].MS2_mariage;
                _ms2_pointup = database.items[i].MS2_pointup;
                _ms3_mariage = database.items[i].MS3_mariage;
                _ms3_pointup = database.items[i].MS3_pointup;
                _ms4_mariage = database.items[i].MS4_mariage;
                _ms4_pointup = database.items[i].MS4_pointup;
                _ms5_mariage = database.items[i].MS5_mariage;
                _ms5_pointup = database.items[i].MS5_pointup;

                for (k = 0; k < _koyutp.Length; k++)
                {
                    _koyutp[k] = database.items[i].koyu_toppingtype[k];
                }
                break;
            }
            ++i;
        }

        //KoyuID_Set(); //固有IDの設定
        _original_id_string = "Non";

        player_yosokuitemlist.Add(new Item(_id, _original_id_string, _file_name, _name, _nameHyouji, _desc, _comp_hosei, _mp, _day, _quality, _exp, _ex_probabilty,
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _beauty, _juice, _tea_flavor,
            _sp_wind, _sp_score2, _sp_score3, _sp_score4, _sp_score5, _sp_score6, _sp_score7, _sp_score8, _sp_score9, _sp_score10,
            _best_welldone,
            _type, _subtype, _subtypeB, _subtype_category, _base_score, _girl1_like, _cost, _sell,
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10, _koyutp[0], _koyutp[1], _koyutp[2], _koyutp[3], _koyutp[4],
            _itemkosu, extreme_kaisu, _item_hyouji, _judge_num, _eat_kaisu, _highscore_flag, _lasttotal_score, _hinttext, _total_kyori, _rare, _manpuku, _magic,
            _noteat, _secretFlag,
            _MS01, _MS02, _MS03, _MS04, _MS05, _MS06, _MS07, _MS08, _MS09, _MS10,
            MSvalue01, MSvalue02, MSvalue03, MSvalue04, MSvalue05, MSvalue06, MSvalue07, MSvalue08, MSvalue09, MSvalue10,
            Attri1, Attri2, Attri3,
            _ms1_mariage, _ms1_pointup, _ms2_mariage, _ms2_pointup, _ms3_mariage, _ms3_pointup, _ms4_mariage, _ms4_pointup, _ms5_mariage, _ms5_pointup));
    }

    //チェック用のオリジナルアイテムを登録する。
    public void addCheckOriginalItem(string _name, int _mp, int _day, int _quality, int _exp, float _ex_probabilty,
        int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, 
        int _powdery, int _oily, int _watery, int _beauty,
        int _juice, int _tea_flavor,
        int _sp_wind, int _sp_score2, int _sp_score3, int _sp_score4, int _sp_score5, int _sp_score6, int _sp_score7, int _sp_score8, int _sp_score9, int _sp_score10,
        float _girl1_like, int _cost, int _sell,
        string _tp01, string _tp02, string _tp03, string _tp04, string _tp05, string _tp06, string _tp07, string _tp08, string _tp09, string _tp10,
        int _itemkosu, int extreme_kaisu, int _item_hyouji, float _total_kyori, int _magic,
        string _MS01, string _MS02, string _MS03, string _MS04, string _MS05, string _MS06, string _MS07, string _MS08, string _MS09, string _MS10,
        int MSvalue01, int MSvalue02, int MSvalue03, int MSvalue04, int MSvalue05, int MSvalue06, int MSvalue07, int MSvalue08, int MSvalue09, int MSvalue10,
        int Attri1, int Attri2, int Attri3)
    {

        player_check_itemlist.Clear();

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
                _subtypeB = database.items[i].itemType_subB.ToString();
                _subtype_category = database.items[i].itemType_sub_category;
                _best_welldone = database.items[i].Best_Welldone;
                _base_score = database.items[i].Base_Score;
                _judge_num = database.items[i].SetJudge_Num;
                _eat_kaisu = database.items[i].Eat_kaisu;
                _highscore_flag = database.items[i].HighScore_flag;
                _lasttotal_score = database.items[i].last_total_score;
                _hinttext = database.items[i].last_hinttext;
                _rare = database.items[i].Rare;
                _manpuku = database.items[i].Manpuku;
                //_magic = database.items[i].Magic;               
                _noteat = database.items[i].NotEat;
                _secretFlag = database.items[i].SecretFlag;
                _ms1_mariage = database.items[i].MS1_mariage;
                _ms1_pointup = database.items[i].MS1_pointup;
                _ms2_mariage = database.items[i].MS2_mariage;
                _ms2_pointup = database.items[i].MS2_pointup;
                _ms3_mariage = database.items[i].MS3_mariage;
                _ms3_pointup = database.items[i].MS3_pointup;
                _ms4_mariage = database.items[i].MS4_mariage;
                _ms4_pointup = database.items[i].MS4_pointup;
                _ms5_mariage = database.items[i].MS5_mariage;
                _ms5_pointup = database.items[i].MS5_pointup;

                for (k = 0; k < _koyutp.Length; k++)
                {
                    _koyutp[k] = database.items[i].koyu_toppingtype[k];
                }
                break;
            }
            ++i;
        }

        //KoyuID_Set(); //固有IDの設定
        _original_id_string = "Non";

        player_check_itemlist.Add(new Item(_id, _original_id_string, _file_name, _name, _nameHyouji, _desc, _comp_hosei, _mp, _day, _quality, _exp, _ex_probabilty,
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _beauty, _juice, _tea_flavor,
            _sp_wind, _sp_score2, _sp_score3, _sp_score4, _sp_score5, _sp_score6, _sp_score7, _sp_score8, _sp_score9, _sp_score10,
            _best_welldone,
            _type, _subtype, _subtypeB, _subtype_category, _base_score, _girl1_like, _cost, _sell,
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10, _koyutp[0], _koyutp[1], _koyutp[2], _koyutp[3], _koyutp[4],
            _itemkosu, extreme_kaisu, _item_hyouji, _judge_num, _eat_kaisu, _highscore_flag, _lasttotal_score, _hinttext, _total_kyori, _rare, _manpuku, _magic,
            _noteat, _secretFlag,
            _MS01, _MS02, _MS03, _MS04, _MS05, _MS06, _MS07, _MS08, _MS09, _MS10,
            MSvalue01, MSvalue02, MSvalue03, MSvalue04, MSvalue05, MSvalue06, MSvalue07, MSvalue08, MSvalue09, MSvalue10,
            Attri1, Attri2, Attri3,
            _ms1_mariage, _ms1_pointup, _ms2_mariage, _ms2_pointup, _ms3_mariage, _ms3_pointup, _ms4_mariage, _ms4_pointup, _ms5_mariage, _ms5_pointup));
    }

    //お菓子パネルにすでにセットされてるアイテムを、オリジナルアイテムへコピーする。個数だけは計算したものをいれる。
    public void ExtremeToCopyOriginalItem(int _kosu)
    {
        tempID = 0;
        if (player_extremepanel_itemlist[tempID].ItemKosu <= _kosu)
        {
            _kosu = player_extremepanel_itemlist[tempID].ItemKosu;
        }
        
        player_originalitemlist.Add(
            new Item(player_extremepanel_itemlist[tempID].itemID, player_extremepanel_itemlist[tempID].OriginalitemID, player_extremepanel_itemlist[tempID].fileName, player_extremepanel_itemlist[tempID].itemName,
            player_extremepanel_itemlist[tempID].itemNameHyouji, player_extremepanel_itemlist[tempID].itemDesc, player_extremepanel_itemlist[tempID].itemComp_Hosei,
            player_extremepanel_itemlist[tempID].itemHP, player_extremepanel_itemlist[tempID].item_day, player_extremepanel_itemlist[tempID].Quality,
            player_extremepanel_itemlist[tempID].Exp, player_extremepanel_itemlist[tempID].Ex_Probability,
            player_extremepanel_itemlist[tempID].Rich, player_extremepanel_itemlist[tempID].Sweat, player_extremepanel_itemlist[tempID].Bitter,
            player_extremepanel_itemlist[tempID].Sour, player_extremepanel_itemlist[tempID].Crispy, player_extremepanel_itemlist[tempID].Fluffy,
            player_extremepanel_itemlist[tempID].Smooth, player_extremepanel_itemlist[tempID].Hardness, player_extremepanel_itemlist[tempID].Jiggly,
            player_extremepanel_itemlist[tempID].Chewy, player_extremepanel_itemlist[tempID].Powdery, player_extremepanel_itemlist[tempID].Oily,
            player_extremepanel_itemlist[tempID].Watery, player_extremepanel_itemlist[tempID].Beauty, player_extremepanel_itemlist[tempID].Juice,
            player_extremepanel_itemlist[tempID].Tea_Flavor,
            player_extremepanel_itemlist[tempID].SP_wind,
            player_extremepanel_itemlist[tempID].SP_Score2, player_extremepanel_itemlist[tempID].SP_Score3, player_extremepanel_itemlist[tempID].SP_Score4,
            player_extremepanel_itemlist[tempID].SP_Score5, player_extremepanel_itemlist[tempID].SP_Score6, player_extremepanel_itemlist[tempID].SP_Score7,
            player_extremepanel_itemlist[tempID].SP_Score8, player_extremepanel_itemlist[tempID].SP_Score9, player_extremepanel_itemlist[tempID].SP_Score10,
            player_extremepanel_itemlist[tempID].Best_Welldone,
            player_extremepanel_itemlist[tempID].itemType.ToString(), player_extremepanel_itemlist[tempID].itemType_sub.ToString(),
            player_extremepanel_itemlist[tempID].itemType_subB.ToString(), player_extremepanel_itemlist[tempID].itemType_sub_category,
            player_extremepanel_itemlist[tempID].Base_Score, player_extremepanel_itemlist[tempID].girl1_itemLike, player_extremepanel_itemlist[tempID].cost_price,
            player_extremepanel_itemlist[tempID].sell_price,
            player_extremepanel_itemlist[tempID].toppingtype[0], player_extremepanel_itemlist[tempID].toppingtype[1], player_extremepanel_itemlist[tempID].toppingtype[2],
            player_extremepanel_itemlist[tempID].toppingtype[3], player_extremepanel_itemlist[tempID].toppingtype[4], player_extremepanel_itemlist[tempID].toppingtype[5],
            player_extremepanel_itemlist[tempID].toppingtype[6], player_extremepanel_itemlist[tempID].toppingtype[7], player_extremepanel_itemlist[tempID].toppingtype[8],
            player_extremepanel_itemlist[tempID].toppingtype[9], 
            player_extremepanel_itemlist[tempID].koyu_toppingtype[0], player_extremepanel_itemlist[tempID].koyu_toppingtype[1], 
            player_extremepanel_itemlist[tempID].koyu_toppingtype[2], player_extremepanel_itemlist[tempID].koyu_toppingtype[3],
            player_extremepanel_itemlist[tempID].koyu_toppingtype[4],
            _kosu, player_extremepanel_itemlist[tempID].ExtremeKaisu, player_extremepanel_itemlist[tempID].item_Hyouji,
            player_extremepanel_itemlist[tempID].SetJudge_Num, player_extremepanel_itemlist[tempID].Eat_kaisu, player_extremepanel_itemlist[tempID].HighScore_flag,
            player_extremepanel_itemlist[tempID].last_total_score, player_extremepanel_itemlist[tempID].last_hinttext,
            player_extremepanel_itemlist[tempID].total_kyori, player_extremepanel_itemlist[tempID].Rare, player_extremepanel_itemlist[tempID].Manpuku,
            player_extremepanel_itemlist[tempID].Magic,
            player_extremepanel_itemlist[tempID].NotEat,
            player_extremepanel_itemlist[tempID].SecretFlag,
            player_extremepanel_itemlist[tempID].item_MagicSlot[0], player_extremepanel_itemlist[tempID].item_MagicSlot[1], player_extremepanel_itemlist[tempID].item_MagicSlot[2],
            player_extremepanel_itemlist[tempID].item_MagicSlot[3], player_extremepanel_itemlist[tempID].item_MagicSlot[4], player_extremepanel_itemlist[tempID].item_MagicSlot[5],
            player_extremepanel_itemlist[tempID].item_MagicSlot[6], player_extremepanel_itemlist[tempID].item_MagicSlot[7], player_extremepanel_itemlist[tempID].item_MagicSlot[8],
            player_extremepanel_itemlist[tempID].item_MagicSlot[9],
            player_extremepanel_itemlist[tempID].item_MagicSlotValue[0], player_extremepanel_itemlist[tempID].item_MagicSlotValue[1], player_extremepanel_itemlist[tempID].item_MagicSlotValue[2],
            player_extremepanel_itemlist[tempID].item_MagicSlotValue[3], player_extremepanel_itemlist[tempID].item_MagicSlotValue[4], player_extremepanel_itemlist[tempID].item_MagicSlotValue[5],
            player_extremepanel_itemlist[tempID].item_MagicSlotValue[6], player_extremepanel_itemlist[tempID].item_MagicSlotValue[7], player_extremepanel_itemlist[tempID].item_MagicSlotValue[8],
            player_extremepanel_itemlist[tempID].item_MagicSlotValue[9],
            player_extremepanel_itemlist[tempID].Attribute1, player_extremepanel_itemlist[tempID].Attribute2,
            player_extremepanel_itemlist[tempID].Attribute3,
            player_extremepanel_itemlist[tempID].MS1_mariage, player_extremepanel_itemlist[tempID].MS1_pointup,
            player_extremepanel_itemlist[tempID].MS2_mariage, player_extremepanel_itemlist[tempID].MS2_pointup,
            player_extremepanel_itemlist[tempID].MS3_mariage, player_extremepanel_itemlist[tempID].MS3_pointup,
            player_extremepanel_itemlist[tempID].MS4_mariage, player_extremepanel_itemlist[tempID].MS4_pointup,
            player_extremepanel_itemlist[tempID].MS5_mariage, player_extremepanel_itemlist[tempID].MS5_pointup));
    }

    void KoyuID_Set()
    {
        TodayNow = DateTime.Now;
        //System.Guid guid = System.Guid.NewGuid();
        //_original_id_string = guid.ToString();
        _original_id_string = TodayNow.Year.ToString("0000") + TodayNow.Month.ToString("00") + TodayNow.Day.ToString("00") + TodayNow.Hour.ToString("00") + TodayNow.Minute.ToString("00") + TodayNow.Second.ToString("00");
        Debug.Log("OriginalItemID: " + _original_id_string);
    }

    //指定したIDのオリジナルアイテムを削除する
    public void deleteOriginalItem(int _id, int _kosu)
    {
        player_originalitemlist[_id].ItemKosu -= _kosu;

        //0以下になったら、リストそのものから削除する。
        if (player_originalitemlist[_id].ItemKosu <= 0)
        {
            player_originalitemlist.RemoveAt(_id);
        }
    }

    //指定したIDのお菓子パネルアイテムを削除する
    public void deleteExtremePanelItem(int _id, int _kosu)
    {
        //extremepanelにセットされていたアイテムを使用した場合、個数が0以下になれば
        //expanel上のアイテムは消える。
        if (player_extremepanel_itemlist.Count > 0)
        {
            player_extremepanel_itemlist[_id].ItemKosu -= _kosu;

            //0以下になったら、リストそのものから削除する。
            if (player_extremepanel_itemlist[_id].ItemKosu <= 0)
            {
                deleteAllExtremePanelItem();
            }
            //個数がまだ残ってた場合、そのままオリジナルアイテムリストへ移動。お菓子パネル上のお菓子は削除する。仕様
            else if (player_extremepanel_itemlist[_id].ItemKosu > 0)
            {
                ExtremeToCopyOriginalItem(player_extremepanel_itemlist[0].ItemKosu);
                deleteAllExtremePanelItem();
            }

        }      
    }

    //お菓子パネルアイテムを全削除する。個数は関係なし。
    public void deleteAllExtremePanelItem()
    {
        player_extremepanel_itemlist.Clear();
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

    //アイテム名を入力すると、現在の所持数を返す処理。（店売り＋オリジナル＋エクストリームパネル）
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
            if (player_originalitemlist[i].itemName == _itemname)
            {
                _itemcount += player_originalitemlist[i].ItemKosu;
            }
        }

        for (i = 0; i < player_extremepanel_itemlist.Count; i++)
        {
            if (player_extremepanel_itemlist[i].itemName == _itemname)
            {
                _itemcount += player_extremepanel_itemlist[i].ItemKosu;
            }
        }

        return _itemcount; //該当するIDがない場合 0
    }

    //アイテムのサブタイプを入力すると、現在の所持数を返す処理。（店売り＋オリジナル＋エクストリームパネル）
    public int KosuCountSubType(string _item_subtype)
    {
        i = 0;
        _itemcount = 0;

        while (i < database.items.Count)
        {
            if (database.items[i].itemType_sub.ToString() == _item_subtype)
            {
                _itemcount += playeritemlist[database.items[i].itemName];
                _itemid = i;
            }
            i++;
        }

        for (i = 0; i < player_originalitemlist.Count; i++)
        {
            if (player_originalitemlist[i].itemType_sub.ToString() == _item_subtype)
            {
                _itemcount += player_originalitemlist[i].ItemKosu;
            }
        }

        for (i = 0; i < player_extremepanel_itemlist.Count; i++)
        {
            if (player_extremepanel_itemlist[i].itemType_sub.ToString() == _item_subtype)
            {
                _itemcount += player_extremepanel_itemlist[i].ItemKosu;
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

    //コンテスト支給品アイテムを全て削除するメソッド
    public void DeleteContestSurppliedItem()
    {
        for (i = 0; i < GameMgr.ContestItem_supplied_List.Count; i++)
        {
            deletePlayerItem(GameMgr.ContestItem_supplied_List[i], 99);
        }

        //最後にリストもクリア
        GameMgr.ContestItem_supplied_List.Clear();
    }

    //コンテスト素材持ち込み不可用に、一時的にアイテムを預かる処理
    public void Keep_PitemList(int _status)
    {
        temp_dic.Clear();
        keep_playeritemlist.Clear();
        keep_player_originalitemlist.Clear();

        switch (_status)
        {
            case 1: //生地など自分で作った素材＋魔法アイテムは持ち込めない 店売りの材料のみOK

                foreach (string key in playeritemlist.Keys)
                {
                    //Debug.Log("キーは" + key + "です。");

                    //生地or魔法アイテムを判別した場合、keepに入れた後に、それらアイテムを削除リストに保持して、一番最後に削除
                    tempID = database.SearchItemIDString(key);
                    if(database.items[tempID].itemType_sub.ToString() == "Source" || database.items[tempID].itemType_sub.ToString() == "Appaleil"
                        || database.items[tempID].itemType_sub.ToString() == "Appaleil_Icecream" || database.items[tempID].itemType_sub.ToString() == "Cream"
                        || database.items[tempID].itemType_sub.ToString() == "WhipeedCream"
                        || database.items[tempID].itemType_sub.ToString() == "GlowFruits"
                        || database.items[tempID].itemType_subB == "a_LumiSuger")
                    {
                        temp_dic.Add(key, playeritemlist[key]);
                    }
                }
                keep_playeritemlist = new Dictionary<string, int>(temp_dic); //一時保存

                //オリジナルの処理
                keep_player_originalitemlist = new List<Item>(player_originalitemlist);

                //削除処理
                foreach (string key in temp_dic.Keys)
                {
                    playeritemlist[key] = 0;
                }
                player_originalitemlist.Clear();
                break;

            case 2: //素材持ち込み全て不可　ただ、器具などはOKということ

                foreach (string key in playeritemlist.Keys)
                {
                    //Debug.Log("キーは" + key + "です。");

                    //生地or魔法アイテムを判別した場合、keepに入れた後に、それらアイテムを削除リストに保持して、一番最後に削除
                    tempID = database.SearchItemIDString(key);
                    if (database.items[tempID].itemType.ToString() == "Mat" || database.items[tempID].itemType.ToString() == "Potion")
                    {
                        temp_dic.Add(key, playeritemlist[key]);
                    }
                }
                keep_playeritemlist = new Dictionary<string, int>(temp_dic); //一時保存

                //オリジナルの処理
                keep_player_originalitemlist = new List<Item>(player_originalitemlist);

                //削除処理
                foreach (string key in temp_dic.Keys)
                {
                    playeritemlist[key] = 0;
                }
                player_originalitemlist.Clear();
                break;
        }
        
    }

    //コンテストで一時預かりしてたアイテムを返す
    public void Contest_ReturnKeepItem()
    {
        if(keep_playeritemlist.Count >= 0)
        {
            foreach (string key in keep_playeritemlist.Keys)
            {
                playeritemlist[key] = keep_playeritemlist[key];
            }
        }

        if (keep_player_originalitemlist.Count >= 0)
        {
            player_originalitemlist = new List<Item>(keep_player_originalitemlist);
        }
    }
}
