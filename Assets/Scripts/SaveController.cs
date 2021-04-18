using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveController : SingletonMonoBehaviour<SaveController>
{

    //保存するものリスト
    //☆GameMgrのパラメータ全般。シナリオ・イベントのフラグ類
    //☆プレイヤーステータス　staticなので、インスタンスの取得の必要はなし。
    private PlayerItemList pitemlist; //プレイヤーアイテムのデータ
    private Girl1_status girl1_status; //女の子ステータス
    private ItemDataBase database; //前回の最高得点などを記録する
    private ItemCompoundDataBase databaseCompo; //調合DBのレシピフラグと前回の点数データ    
    private ItemMatPlaceDataBase matplace_database; //マップのオンフラグ
    private ExtremePanel extreme_panel; //エクストリームパネルに登録したものがあった場合は、そのアイテムも表示されるように。
    private ItemShopDataBase shop_database;

    //保存するものリスト　ここまで

    private PlayerData playerData;
    private Compound_Keisan compound_keisan;
    private BGAcceTrigger BGAccetrigger;
    private Debug_Panel debug_panel; //画面更新用のメソッドを借りる。
    private BGM sceneBGM;
    private Special_Quest special_quest;
    private MoneyStatus_Controller money_status;

    private GameObject canvas;

    private Compound_Main compound_Main;

    private Text questname;
    private List<int> _tempplayeritemlist = new List<int>();
    private List<int> _tempmap_placeflaglist = new List<int>();
    private List<int> _temp_shopzaiko = new List<int>();
    private List<int> _temp_farmzaiko = new List<int>();
    private List<int> _temp_emeraldshop_zaiko = new List<int>();
    private List<ItemSaveparam> _temp_itemscorelist = new List<ItemSaveparam>();

    private GameObject _model_obj;

    private int i;
    private int _itemID;

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //味パラメータ初期化
        compound_keisan = Compound_Keisan.Instance.GetComponent<Compound_Keisan>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //セーブ処理
    public void OnSaveMethod()
    {
        GameMgr.saveOK = true;

        //セーブデータを管理するデータバンクのインスタンスを取得します(シングルトン)
        DataBank bank = DataBank.Open();

        Debug.Log("DataBank.Open()");
        Debug.Log($"save path of bank is { bank.SavePath }");

        //アイテムリストの所持数を取得
        _tempplayeritemlist.Clear();
        for (i=0; i < pitemlist.playeritemlist.Count; i++)
        {
            _tempplayeritemlist.Add(pitemlist.playeritemlist[i]);
        }

        //マップのフラグのみ取得
        _tempmap_placeflaglist.Clear();
        for (i = 0; i < matplace_database.matplace_lists.Count; i++)
        {
            _tempmap_placeflaglist.Add(matplace_database.matplace_lists[i].placeFlag);
        }

        //ショップの在庫のみ取得
        _temp_shopzaiko.Clear();
        for (i = 0; i < shop_database.shopitems.Count; i++)
        {
            _temp_shopzaiko.Add(shop_database.shopitems[i].shop_itemzaiko);
        }

        //牧場の在庫のみ取得
        _temp_farmzaiko.Clear();
        for (i = 0; i < shop_database.farmitems.Count; i++)
        {
            _temp_farmzaiko.Add(shop_database.farmitems[i].shop_itemzaiko);
        }

        //エメラルドショップの在庫のみ取得
        _temp_emeraldshop_zaiko.Clear();
        for (i = 0; i < shop_database.emeraldshop_items.Count; i++)
        {
            _temp_emeraldshop_zaiko.Add(shop_database.emeraldshop_items[i].shop_itemzaiko);
        }

        //アイテムの前回得点のみ取得
        _temp_itemscorelist.Clear();
        for (i = 0; i < database.items.Count; i++)
        {
            _temp_itemscorelist.Add(new ItemSaveparam(database.items[i].itemID, database.items[i].Eat_kaisu, database.items[i].HighScore_flag, database.items[i].last_total_score,
                database.items[i].last_rich_score, database.items[i].last_sweat_score, database.items[i].last_bitter_score, database.items[i].last_sour_score,
                database.items[i].last_crispy_score, database.items[i].last_fluffy_score, database.items[i].last_smooth_score, database.items[i].last_hardness_score,
                database.items[i].last_jiggly_score, database.items[i].last_chewy_score, database.items[i].last_hinttext));
        }


        //セーブ保存用のクラスを新規作成。
        playerData = new PlayerData()
        {
            //プレイヤーステータス
            save_player_money = PlayerStatus.player_money, // 所持金
            save_player_kaeru_coin = PlayerStatus.player_kaeru_coin, //かえるコインの所持数。危ないお店などで使える。

            save_player_renkin_lv = PlayerStatus.player_renkin_lv, //パティシエレベル
            save_player_renkin_exp = PlayerStatus.player_renkin_exp, //パティシエ経験
            save_player_extreme_kaisu_Max = PlayerStatus.player_extreme_kaisu_Max, //仕上げ可能回数
            save_player_extreme_kaisu = PlayerStatus.player_extreme_kaisu,//現在の仕上げ可能回数


            save_player_ninki_param = PlayerStatus.player_ninki_param, //人気度。いるかな？とりあえず置き
            save_player_zairyobox_lv = PlayerStatus.player_zairyobox_lv, // 材料カゴの大きさ
            save_player_zairyobox = PlayerStatus.player_zairyobox, // 材料カゴの大きさ


            //妹のステータス
            save_player_girl_findpower = PlayerStatus.player_girl_findpower, //妹のアイテム発見力。高いと、マップの隠し場所を発見できたりする。
            save_girl_love_exp = PlayerStatus.girl1_Love_exp, //妹の好感度
            save_girl_love_lv = PlayerStatus.girl1_Love_lv, //妹の好感度レベル
            save_player_girl_lifepoint = PlayerStatus.player_girl_lifepoint, //妹の体力
            save_player_girl_maxlifepoint = PlayerStatus.player_girl_maxlifepoint,

            //日付・フラグ関係
            save_player_day = PlayerStatus.player_day, //現在の日付
            save_player_time = PlayerStatus.player_time, //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分

            save_First_recipi_on = PlayerStatus.First_recipi_on,
            save_First_extreme_on = PlayerStatus.First_extreme_on,

            save_special_animatFirst = girl1_status.special_animatFirst,

            //コスチューム番号
            save_costume_num = GameMgr.Costume_Num,
            save_acce_num = GameMgr.Accesory_Num,

            //飾っているアイテムのリスト
            save_DecoItems = GameMgr.DecoItems,

             //コレクションに登録したアイテムのリスト
            save_CollectionItems = GameMgr.CollectionItems,

            //エンディングカウント
            save_ending_count = GameMgr.ending_count,

            //ステージ番号
            save_stage_number = GameMgr.stage_number,

            //シナリオの進み具合
            save_scenario_flag = GameMgr.scenario_flag,

            //セーブ保存フラグ
            save_saveOK = GameMgr.saveOK,

            //初期アイテム取得フラグ
            save_gamestart_recipi_get = GameMgr.gamestart_recipi_get,

            //クエスト以外で、クリアするのに必要なハート量
            save_stageclear_love = GameMgr.stageclear_love,
            save_stageclear_cullentlove = GameMgr.stageclear_cullentlove,

            //イベントフラグ
            save_GirlLoveEvent_num = GameMgr.GirlLoveEvent_num,
            save_GirlLoveEvent_stage1 = GameMgr.GirlLoveEvent_stage1,  //各イベントの、現在読み中かどうかのフラグ。           
            save_GirlLoveEvent_stage2 = GameMgr.GirlLoveEvent_stage2,
            save_GirlLoveEvent_stage3 = GameMgr.GirlLoveEvent_stage3,

            save_GirlLoveSubEvent_stage1 = GameMgr.GirlLoveSubEvent_stage1,

            //ビギナーフラグ
            save_Beginner_flag = GameMgr.Beginner_flag,

            //お菓子クエストフラグ
            save_OkashiQuest_flag_stage1 = GameMgr.OkashiQuest_flag_stage1, //各SPイベントのクリアしたかどうかのフラグ。
            save_OkashiQuest_flag_stage2 = GameMgr.OkashiQuest_flag_stage2,
            save_OkashiQuest_flag_stage3 = GameMgr.OkashiQuest_flag_stage3,

            //現在のクエストクリアフラグ
            save_QuestClearflag = GameMgr.QuestClearflag,
            save_QuestClearButton_anim = GameMgr.QuestClearButton_anim,

            //さっき食べたお菓子の情報
            save_Okashi_lasthint = GameMgr.Okashi_lasthint, //さっき食べたお菓子のヒント。
            save_Okashi_lastname = GameMgr.Okashi_lastname, //さっき食べたお菓子の名前。
            save_Okashi_lastID = GameMgr.Okashi_lastID, //さっき食べたお菓子のアイテムID
            save_Okashi_totalscore = GameMgr.Okashi_totalscore, //さっき食べたお菓子の点数
            save_Okashi_lastshokukan_param = GameMgr.Okashi_lastshokukan_param, //さっき食べたお菓子のパラメータ
            save_Okashi_lastshokukan_mes = GameMgr.Okashi_lastshokukan_mes, //さっき食べたお菓子のパラメータ
            save_Okashi_lastsweat_param = GameMgr.Okashi_lastsweat_param, //さっき食べたお菓子のパラメータ
            save_Okashi_lastsour_param = GameMgr.Okashi_lastsour_param, //さっき食べたお菓子のパラメータ
            save_Okashi_lastbitter_param = GameMgr.Okashi_lastbitter_param, //さっき食べたお菓子のパラメータ

            //マップイベントフラグ
            save_MapEvent_01 = GameMgr.MapEvent_01,         //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
            save_MapEvent_02 = GameMgr.MapEvent_02,         //井戸。
            save_MapEvent_03 = GameMgr.MapEvent_03,         //ストロベリーガーデン
            save_MapEvent_04 = GameMgr.MapEvent_04,         //ひまわりの丘
            save_MapEvent_05 = GameMgr.MapEvent_05,         //ラベンダー
            save_MapEvent_06 = GameMgr.MapEvent_06,         //バードサンクチュアリ

            //広場でのイベント
            save_hiroba_event_end = GameMgr.hiroba_event_end,

            //ステージ１クリア時の好感度を保存
            save_stage1_girl1_loveexp = GameMgr.stage1_girl1_loveexp,
            save_stage2_girl1_loveexp = GameMgr.stage2_girl1_loveexp,
            save_stage3_girl1_loveexp = GameMgr.stage3_girl1_loveexp,

            save_stage1_clear_love = GameMgr.stage1_clear_love,
            save_stage2_clear_love = GameMgr.stage2_clear_love,
            save_stage3_clear_love = GameMgr.stage3_clear_love,

            //ショップのイベントリスト
            save_ShopEvent_stage = GameMgr.ShopEvent_stage,
            save_ShopLvEvent_stage = GameMgr.ShopLVEvent_stage,

            //ショップの在庫
            save_shopzaiko = _temp_shopzaiko,
            save_farmzaiko = _temp_farmzaiko,
            save_emeraldshop_zaiko = _temp_emeraldshop_zaiko,

            //ショップのうわさ話リスト
            save_ShopUwasa_stage1 = GameMgr.ShopUwasa_stage1,

            //コンテストのイベントリスト
            save_ContestEvent_stage = GameMgr.ContestEvent_stage,

            //コンテスト審査員の点数
            save_contest_Score = GameMgr.contest_Score,
            save_contest_TotalScore = GameMgr.contest_TotalScore,

            //牧場のイベントリスト
            save_FarmEvent_stage = GameMgr.FarmEvent_stage,

            //エメラルドショップのイベントリスト
            save_emeraldShopEvent_stage = GameMgr.emeraldShopEvent_stage,

            //アイテムリスト＜デフォルト＞
            save_playeritemlist = _tempplayeritemlist,

            //プレイヤーのイベントアイテムリスト。
            save_eventitemlist = pitemlist.eventitemlist,

            //プレイヤーのエメラルドアイテムリスト。
            save_player_emeralditemlist = pitemlist.emeralditemlist,

            //アイテムリスト＜オリジナル＞
            save_player_originalitemlist = pitemlist.player_originalitemlist,

            //アイテムの前回スコアなどを記録する
            save_itemdatabase = _temp_itemscorelist,

            //調合のフラグ＋調合回数を記録する
            save_itemCompodatabase = databaseCompo.compoitems,

            //マップフラグリスト
            save_mapflaglist = _tempmap_placeflaglist,

            //エクストリームパネルのアイテムIDも保存
            save_extreme_itemid = GameMgr.sys_extreme_itemID,
            save_extreme_itemtype = GameMgr.sys_extreme_itemType,
        };
       

        //デバッグ用
        Debug.Log("セーブ　GameMgr.GirlLoveEvent_num:" + GameMgr.GirlLoveEvent_num);
        /*for (i = 0; i < GameMgr.GirlLoveEvent_stage1.Length; i++)
        {          
            Debug.Log("セーブ　GameMgr.GirlLoveEvent_stage1: " + GameMgr.GirlLoveEvent_stage1[i]);
        }*/

        //Debug.Log(playerData);

        //データの一時保存。bankに、playerDataを「player1」という名前で現在のデータを保存。
        bank.Store("player1", playerData);
        //Debug.Log("bank.Store()");

        //一時データを永続的に保存。永続保存するときは、一度、一時データに保存しておく。
        bank.SaveAll();
        Debug.Log("bank.SaveAll()");

        //初期化(念のため)
        playerData = new PlayerData();

    }

    //ロード処理
    public void OnLoadMethod()
    {

        //セーブデータがあるかどうかをチェック
        SaveCheck();

        if (GameMgr.saveOK)
        {
            LoadingContents();
        }
        else
        {
            //なければ、無視
        }
               
    }

    void LoadingContents()
    {
        //プレイヤーステータス
        PlayerStatus.player_money = playerData.save_player_money; // 所持金
        PlayerStatus.player_kaeru_coin = playerData.save_player_kaeru_coin; //かえるコインの所持数。危ないお店などで使える。

        PlayerStatus.player_renkin_lv = playerData.save_player_renkin_lv; //パティシエレベル
        PlayerStatus.player_renkin_exp = playerData.save_player_renkin_exp; //パティシエ経験
        PlayerStatus.player_extreme_kaisu_Max = playerData.save_player_extreme_kaisu_Max; //仕上げ可能回数
        PlayerStatus.player_extreme_kaisu = playerData.save_player_extreme_kaisu;//現在の仕上げ可能回数

        PlayerStatus.player_ninki_param = playerData.save_player_ninki_param; //人気度。いるかな？とりあえず置き
        PlayerStatus.player_zairyobox_lv = playerData.save_player_zairyobox_lv; // 材料カゴの大きさ
        PlayerStatus.player_zairyobox = playerData.save_player_zairyobox; // 材料カゴの大きさ

        //妹のステータス
        PlayerStatus.player_girl_findpower = playerData.save_player_girl_findpower; //妹のアイテム発見力。高いと、マップの隠し場所を発見できたりする。
        PlayerStatus.girl1_Love_exp = playerData.save_girl_love_exp; //妹の好感度
        PlayerStatus.girl1_Love_lv = playerData.save_girl_love_lv; //妹の好感度レベル
        PlayerStatus.player_girl_lifepoint = playerData.save_player_girl_lifepoint; //妹の体力
        PlayerStatus.player_girl_maxlifepoint = playerData.save_player_girl_maxlifepoint; //妹のMax体力

        //日付・フラグ関係
        PlayerStatus.player_day = playerData.save_player_day; //現在の日付
        PlayerStatus.player_time = playerData.save_player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分

        PlayerStatus.First_recipi_on = playerData.save_First_recipi_on;
        PlayerStatus.First_extreme_on = playerData.save_First_extreme_on;

        girl1_status.special_animatFirst = playerData.save_special_animatFirst;

        //コスチューム番号
        GameMgr.Costume_Num = playerData.save_costume_num;
        GameMgr.Accesory_Num = playerData.save_acce_num;

        //飾っているアイテムのリスト
        GameMgr.DecoItems = playerData.save_DecoItems;

        //コレクションに登録したアイテムのリスト
        GameMgr.CollectionItems = playerData.save_CollectionItems;

        //エンディングカウント
        GameMgr.ending_count = playerData.save_ending_count;

        //ステージ番号
        GameMgr.stage_number = playerData.save_stage_number;

        //シナリオの進み具合
        GameMgr.scenario_flag = playerData.save_scenario_flag;

        //初期アイテム取得フラグ
        GameMgr.gamestart_recipi_get = playerData.save_gamestart_recipi_get;

        //クエスト以外で、クリアするのに必要なハート量
        GameMgr.stageclear_love = playerData.save_stageclear_love;
        GameMgr.stageclear_cullentlove = playerData.save_stageclear_cullentlove;

        //イベントフラグ
        GameMgr.GirlLoveEvent_num = playerData.save_GirlLoveEvent_num;
        GameMgr.GirlLoveEvent_stage1 = playerData.save_GirlLoveEvent_stage1;  //各イベントの、現在読み中かどうかのフラグ。
        GameMgr.GirlLoveEvent_stage2 = playerData.save_GirlLoveEvent_stage2;
        GameMgr.GirlLoveEvent_stage3 = playerData.save_GirlLoveEvent_stage3;

        GameMgr.GirlLoveSubEvent_stage1 = playerData.save_GirlLoveSubEvent_stage1;

        //ビギナーフラグ
        GameMgr.Beginner_flag = playerData.save_Beginner_flag;

        //お菓子クエストフラグ
        GameMgr.OkashiQuest_flag_stage1 = playerData.save_OkashiQuest_flag_stage1; //各SPイベントのクリアしたかどうかのフラグ。
        GameMgr.OkashiQuest_flag_stage2 = playerData.save_OkashiQuest_flag_stage2;
        GameMgr.OkashiQuest_flag_stage3 = playerData.save_OkashiQuest_flag_stage3;

        GameMgr.QuestClearflag = playerData.save_QuestClearflag;
        GameMgr.QuestClearButton_anim = playerData.save_QuestClearButton_anim;

        //さっき食べたお菓子の情報
        GameMgr.Okashi_lasthint = playerData.save_Okashi_lasthint; //さっき食べたお菓子のヒント。
        GameMgr.Okashi_lastname = playerData.save_Okashi_lastname; //さっき食べたお菓子の名前。
        GameMgr.Okashi_lastID = playerData.save_Okashi_lastID; //さっき食べたお菓子のアイテムID
        GameMgr.Okashi_totalscore = playerData.save_Okashi_totalscore; //さっき食べたお菓子の点数
        GameMgr.Okashi_lastshokukan_param = playerData.save_Okashi_lastshokukan_param; //さっき食べたお菓子のパラメータ
        GameMgr.Okashi_lastshokukan_mes = playerData.save_Okashi_lastshokukan_mes; //さっき食べたお菓子のパラメータ
        GameMgr.Okashi_lastsweat_param = playerData.save_Okashi_lastsweat_param; //さっき食べたお菓子のパラメータ
        GameMgr.Okashi_lastsour_param = playerData.save_Okashi_lastsour_param; //さっき食べたお菓子のパラメータ
        GameMgr.Okashi_lastbitter_param = playerData.save_Okashi_lastbitter_param; //さっき食べたお菓子のパラメータ

        //マップイベントフラグ
        GameMgr.MapEvent_01 = playerData.save_MapEvent_01;        //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
        GameMgr.MapEvent_02 = playerData.save_MapEvent_02;        //井戸。
        GameMgr.MapEvent_03 = playerData.save_MapEvent_03;        //ストロベリーガーデン
        GameMgr.MapEvent_04 = playerData.save_MapEvent_04;        //ひまわりの丘
        GameMgr.MapEvent_05 = playerData.save_MapEvent_05;        //ラベンダー
        GameMgr.MapEvent_06 = playerData.save_MapEvent_06;        //バードサンクチュアリ

        //広場でのイベント
        GameMgr.hiroba_event_end = playerData.save_hiroba_event_end;

        //ステージ１クリア時の好感度を保存
        GameMgr.stage1_girl1_loveexp = playerData.save_stage1_girl1_loveexp;
        GameMgr.stage2_girl1_loveexp = playerData.save_stage2_girl1_loveexp;
        GameMgr.stage3_girl1_loveexp = playerData.save_stage3_girl1_loveexp;

        GameMgr.stage1_clear_love = playerData.save_stage1_clear_love;
        GameMgr.stage2_clear_love = playerData.save_stage2_clear_love;
        GameMgr.stage3_clear_love = playerData.save_stage3_clear_love;

        //ショップのイベントリスト
        GameMgr.ShopEvent_stage = playerData.save_ShopEvent_stage;
        GameMgr.ShopLVEvent_stage = playerData.save_ShopLvEvent_stage;

        //ショップのうわさ話リスト
        GameMgr.ShopUwasa_stage1 = playerData.save_ShopUwasa_stage1;

        //コンテストのイベントリスト
        GameMgr.ContestEvent_stage = playerData.save_ContestEvent_stage;

        //コンテスト審査員の点数
        GameMgr.contest_Score = playerData.save_contest_Score;
        GameMgr.contest_TotalScore = playerData.save_contest_TotalScore;

        //牧場のイベントリスト
        GameMgr.FarmEvent_stage = playerData.save_FarmEvent_stage;

        //エメラルドショップのイベントリスト
        GameMgr.emeraldShopEvent_stage = playerData.save_emeraldShopEvent_stage;

        //アイテムリスト＜デフォルト＞
        for (i = 0; i < pitemlist.playeritemlist.Count; i++)
        {
            pitemlist.playeritemlist[i] = playerData.save_playeritemlist[i];
        }

        //プレイヤーのイベントアイテムリスト。
        pitemlist.eventitemlist.Clear();
        pitemlist.eventitemlist = playerData.save_eventitemlist;

        //プレイヤーのエメラルドアイテムリスト。
        pitemlist.emeralditemlist.Clear();
        pitemlist.emeralditemlist = playerData.save_player_emeralditemlist;

        //アイテムリスト＜オリジナル＞
        pitemlist.player_originalitemlist.Clear();
        pitemlist.player_originalitemlist = playerData.save_player_originalitemlist;

        //テクスチャのデータは保存すると壊れてしまうので、ここで入れ直す。
        for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
        {
            _itemID = pitemlist.SearchItemString(pitemlist.player_originalitemlist[i].itemName);
            pitemlist.player_originalitemlist[i].itemIcon_sprite = database.items[_itemID].itemIcon_sprite;
        }

        //アイテムの前回スコアなどを読み込み
        for (i = 0; i < database.items.Count; i++)
        {
            database.items[i].Eat_kaisu = playerData.save_itemdatabase[i].Eat_kaisu;
            database.items[i].HighScore_flag = playerData.save_itemdatabase[i].HighScore_flag;
            database.items[i].last_total_score = playerData.save_itemdatabase[i].last_total_score;
            database.items[i].last_rich_score = playerData.save_itemdatabase[i].last_rich_score;
            database.items[i].last_sweat_score = playerData.save_itemdatabase[i].last_sweat_score;
            database.items[i].last_bitter_score = playerData.save_itemdatabase[i].last_bitter_score;
            database.items[i].last_sour_score = playerData.save_itemdatabase[i].last_sour_score;
            database.items[i].last_crispy_score = playerData.save_itemdatabase[i].last_crispy_score;
            database.items[i].last_fluffy_score = playerData.save_itemdatabase[i].last_fluffy_score;
            database.items[i].last_smooth_score = playerData.save_itemdatabase[i].last_smooth_score;
            database.items[i].last_hardness_score = playerData.save_itemdatabase[i].last_hardness_score;
            database.items[i].last_jiggly_score = playerData.save_itemdatabase[i].last_jiggly_score;
            database.items[i].last_chewy_score = playerData.save_itemdatabase[i].last_chewy_score;
            database.items[i].last_hinttext = playerData.save_itemdatabase[i].last_hinttext;
        }

        //調合のフラグ＋調合回数を記録する
        databaseCompo.compoitems.Clear();
        databaseCompo.compoitems = playerData.save_itemCompodatabase;

        //マップフラグの読み込み
        for (i = 0; i < matplace_database.matplace_lists.Count; i++)
        {
            matplace_database.matplace_lists[i].placeFlag = playerData.save_mapflaglist[i];
        }

        //ショップの在庫読み込み
        for (i = 0; i < shop_database.shopitems.Count; i++)
        {
            shop_database.shopitems[i].shop_itemzaiko = playerData.save_shopzaiko[i];
        }
        for (i = 0; i < shop_database.farmitems.Count; i++)
        {
            shop_database.farmitems[i].shop_itemzaiko = playerData.save_farmzaiko[i];
        }
        for (i = 0; i < shop_database.emeraldshop_items.Count; i++)
        {
            shop_database.emeraldshop_items[i].shop_itemzaiko = playerData.save_emeraldshop_zaiko[i];
        }

        //エクストリームパネルのアイテムを読み込み
        GameMgr.sys_extreme_itemID = playerData.save_extreme_itemid;
        GameMgr.sys_extreme_itemType = playerData.save_extreme_itemtype;

        //デバッグ用
        //Debug.Log("ロード　GameMgr.GirlLoveEvent_num:" + GameMgr.GirlLoveEvent_num);
        /*for (i= 0; i < GameMgr.GirlLoveEvent_stage1.Length; i++)
        {
            
            Debug.Log("ロード　GameMgr.GirlLoveEvent_stage1: " + GameMgr.GirlLoveEvent_stage1[i]);
        }*/


        //画面の更新処理
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                DrawGameScreen();
                break;
        }
    }

    public void DrawGameScreen()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        debug_panel = GameObject.FindWithTag("Debug_Panel").GetComponent<Debug_Panel>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

                money_status = canvas.transform.Find("MainUIPanel/MoneyStatus_panel").GetComponent<MoneyStatus_Controller>();

                //Live2Dモデルの取得
                _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;

                //メイン画面に表示する、現在のクエスト
                questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();

                //BGMの取得
                sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

                //飾りアイテムのセット
                BGAccetrigger = GameObject.FindWithTag("BGAccessory").GetComponent<BGAcceTrigger>();

                money_status.money_Draw();
                questname.text = girl1_status.OkashiQuest_Name;
                sceneBGM.PlayMain(); //BGMの更新                               

                if (GameMgr.sys_extreme_itemID != 9999)
                {
                    extreme_panel = canvas.transform.Find("MainUIPanel/ExtremePanel").GetComponentInChildren<ExtremePanel>();
                    extreme_panel.SetExtremeItem(GameMgr.sys_extreme_itemID, GameMgr.sys_extreme_itemType);
                    extreme_panel.SetInitParamExtreme();

                }
                break;
        }

        girl1_status.OkashiNew_Status = 0;
        girl1_status.special_animatFirst = true;
        girl1_status.Girl_Hungry();

        special_quest.SetSpecialOkashi(GameMgr.GirlLoveEvent_num, 1); //クエスト番号を再設定ここで。
        special_quest.RedrawQeustName();       

        debug_panel.GirlLove_Koushin(PlayerStatus.girl1_Love_exp); //好感度ステータスに応じたキャラの表情やLive2Dモーション更新
        GameMgr.KeyInputOff_flag = true;

        //衣装チェンジ
        _model_obj.GetComponent<Live2DCostumeTrigger>().ChangeCostume();
        _model_obj.GetComponent<Live2DCostumeTrigger>().ChangeAcce();

        //飾りアイテムのセット
        BGAccetrigger.DrawBGAcce();

        compound_Main.compound_status = 0;

    }

    //ゲーム「はじめから」で、リセットされる項目
    public void ResetAllParam()
    {
        PlayerStatus.Setup_PlayerStatus(); //プレイヤーステータスの初期化
        girl1_status.ResetDefaultStatus(); //女の子ステータスの初期化
        GameMgr.ResetGameDefaultStatus(); //ゲームステータスの初期化　イベントフラグ関係は、ここで初期化

        //アイテムデータの初期化
        //アイテムリスト＜デフォルト＞
        for (i = 0; i < pitemlist.playeritemlist.Count; i++)
        {
            pitemlist.playeritemlist[i] = 0;
        }

        //プレイヤーのイベントアイテムリスト。
        pitemlist.ReseteventitemList();

        //アイテムリスト＜オリジナル＞
        pitemlist.player_originalitemlist.Clear();

        //アイテムの前回スコアなどを初期化
        database.ResetLastScore();

        //調合フラグ＋調合回数も初期化
        databaseCompo.ResetDefaultCompoExcel();

        //アイテムDBの味の初期化
        //compound_keisan.ResetDefaultTasteParam();

        //マップフラグの初期化
        matplace_database.ResetDefaultMapExcel();

        //各ショップのイベントアイテムの在庫の初期化
    }

    //セーブデータがあるかどうかだけをチェック
    public void SaveCheck()
    {
        //セーブデータを管理するデータバンクのインスタンスを取得します(シングルトン)
        DataBank bank = DataBank.Open();

        Debug.Log("DataBank.Open()");
        Debug.Log($"save path of bank is { bank.SavePath }");

        //初期化(念のため)
        playerData = new PlayerData();

        //永続的に保存しておいたデータを、一時データに読み込む。bankに読み込まれる。
        bank.Load<PlayerData>("player1");
        Debug.Log("ロード完了");

        //一時データに再度読み込んだので、Getすると、再びパラメータを取得できる。
        playerData = bank.Get<PlayerData>("player1");
        //Debug.Log(playerData);


        //
        //*** 以下、読み込み・更新 ***
        //

        //まず、セーブデータがあるかどうかをチェック
        GameMgr.saveOK = playerData.save_saveOK;
    }
}
