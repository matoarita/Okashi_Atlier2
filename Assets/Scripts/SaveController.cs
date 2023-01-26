using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System; //DateTimeを使用する為追加。

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
    private QuestSetDataBase quest_database;

    //保存するものリスト　ここまで

    private PlayerData playerData;
    private PlayerData systemData;

    private GirlEat_Judge girlEat_judge;
    private Compound_Keisan compound_keisan;
    private BGAcceTrigger BGAccetrigger;
    private Debug_Panel debug_panel; //画面更新用のメソッドを借りる。
    private BGM sceneBGM;
    private SoundController sc;
    private Special_Quest special_quest;
    private MoneyStatus_Controller money_status;

    private GameObject canvas;

    private Compound_Main compound_Main;

    private Text questname;
    private List<ItemSaveKosu> _tempplayeritemlist = new List<ItemSaveKosu>();
    private List<ItemSaveKosu> _tempdongrilist = new List<ItemSaveKosu>();
    private List<ItemSaveKosu> _temp_eventitemlist = new List<ItemSaveKosu>();
    private List<ItemSaveKosu> _temp_emeralditemlist = new List<ItemSaveKosu>();
    private List<ItemSaveCompoFlag> _temp_cmpflaglist = new List<ItemSaveCompoFlag>();
    private List<ItemSaveKosu> _tempmap_placeflaglist = new List<ItemSaveKosu>();
    private List<ItemSaveKosu> _temp_shopzaiko = new List<ItemSaveKosu>();
    private List<ItemSaveKosu> _temp_farmzaiko = new List<ItemSaveKosu>();
    private List<ItemSaveKosu> _temp_emeraldshop_zaiko = new List<ItemSaveKosu>();
    private List<ItemSaveparam> _temp_itemscorelist = new List<ItemSaveparam>();
    private List<ItemSaveFlag> _temp_titlecollectionlist = new List<ItemSaveFlag>();
    private List<ItemSaveFlag> _temp_eventcollectionlist = new List<ItemSaveFlag>();
    private List<ItemSaveFlag> _temp_contestclearcollectionlist = new List<ItemSaveFlag>();
    private List<ItemSaveFlag> _temp_bgmcollectionlist = new List<ItemSaveFlag>();
    private List<ItemSaveFlag> _temp_bgacce_flaglist = new List<ItemSaveFlag>();
    private List<Item> _temp_contestclearcollectionlistItemData = new List<Item>();

    private GameObject _model_obj;
    private GameObject StageClearButton_panel;
    private AudioSource StageClearbutton_audio;

    private int i, count;
    private int _itemID;

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //GirlEatJudgeを取得
        girlEat_judge = GirlEat_Judge.Instance.GetComponent<GirlEat_Judge>();

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

        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();
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
        foreach (KeyValuePair <string, int> table in pitemlist.playeritemlist)
        {
            _tempplayeritemlist.Add(new ItemSaveKosu(table.Key, table.Value, 0));
        }

        //イベントアイテムの所持数取得
        _temp_eventitemlist.Clear();
        for (i = 0; i < pitemlist.eventitemlist.Count; i++)
        {
            _temp_eventitemlist.Add(new ItemSaveKosu(pitemlist.eventitemlist[i].event_itemName, pitemlist.eventitemlist[i].ev_itemKosu, pitemlist.eventitemlist[i].ev_ReadFlag));
        }

        //エメラルドアイテムの所持数取得
        /*_temp_emeralditemlist.Clear();
        for (i = 0; i < pitemlist.emeralditemlist.Count; i++)
        {
            _temp_emeralditemlist.Add(new ItemSaveKosu(pitemlist.emeralditemlist[i].event_itemName, pitemlist.emeralditemlist[i].ev_itemKosu, 0));
        }*/

        //調合フラグと調合回数の取得
        _temp_cmpflaglist.Clear();
        for (i = 0; i < databaseCompo.compoitems.Count; i++)
        {
            _temp_cmpflaglist.Add(new ItemSaveCompoFlag(databaseCompo.compoitems[i].cmpitem_Name, databaseCompo.compoitems[i].cmpitem_flag, 
                databaseCompo.compoitems[i].comp_count, databaseCompo.compoitems[i].hikari_make_count));
        }

        //マップのフラグのみ取得
        _tempmap_placeflaglist.Clear();
        for (i = 0; i < matplace_database.matplace_lists.Count; i++)
        {
            _tempmap_placeflaglist.Add(new ItemSaveKosu(matplace_database.matplace_lists[i].placeName, 0, matplace_database.matplace_lists[i].placeFlag));
        }

        //ショップの在庫のみ取得
        _temp_shopzaiko.Clear();
        for (i = 0; i < shop_database.shopitems.Count; i++)
        {
            _temp_shopzaiko.Add(new ItemSaveKosu(shop_database.shopitems[i].shop_itemName, shop_database.shopitems[i].shop_itemzaiko, 0));
        }

        //牧場の在庫のみ取得
        _temp_farmzaiko.Clear();
        for (i = 0; i < shop_database.farmitems.Count; i++)
        {
            _temp_farmzaiko.Add(new ItemSaveKosu(shop_database.farmitems[i].shop_itemName, shop_database.farmitems[i].shop_itemzaiko,0));
        }

        //エメラルドショップの在庫のみ取得
        _temp_emeraldshop_zaiko.Clear();
        for (i = 0; i < shop_database.emeraldshop_items.Count; i++)
        {
            _temp_emeraldshop_zaiko.Add(new ItemSaveKosu(shop_database.emeraldshop_items[i].shop_itemName, shop_database.emeraldshop_items[i].shop_itemzaiko, 0));
        }

        //背景アイテムの表示フラグリスト
        _temp_bgacce_flaglist.Clear();
        foreach (string key in GameMgr.BGAcceItemsName.Keys)
        {
            _temp_bgacce_flaglist.Add(new ItemSaveFlag(key, 0, GameMgr.BGAcceItemsName[key]));
        }

        //アイテムの前回得点のみ取得
        /*_temp_itemscorelist.Clear();
        for (i = 0; i < database.items.Count; i++)
        {
            _temp_itemscorelist.Add(new ItemSaveparam(database.items[i].itemID, database.items[i].itemName, database.items[i].Eat_kaisu, database.items[i].HighScore_flag, database.items[i].last_total_score,
                database.items[i].last_rich_score, database.items[i].last_sweat_score, database.items[i].last_bitter_score, database.items[i].last_sour_score,
                database.items[i].last_crispy_score, database.items[i].last_fluffy_score, database.items[i].last_smooth_score, database.items[i].last_hardness_score,
                database.items[i].last_jiggly_score, database.items[i].last_chewy_score, database.items[i].last_hinttext));
        }*/


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
            save_player_girl_maxlifepoint = PlayerStatus.player_girl_maxlifepoint, //妹のMAX体力
            save_player_girl_eatCount = PlayerStatus.player_girl_eatCount, //妹が食べたお菓子の回数
            save_player_girl_eatCount_tabetai = PlayerStatus.player_girl_eatCount_tabetai, //妹が食べたいお菓子をあげた回数
            save_player_girl_manpuku = PlayerStatus.player_girl_manpuku, //妹の満腹度
            //save_player_girl_yaruki = PlayerStatus.player_girl_yaruki, //妹のやる気

            //お菓子経験値　全１５種類
            save_player_girl_appaleil_exp = PlayerStatus.player_girl_appaleil_exp,
            save_player_girl_cream_exp = PlayerStatus.player_girl_cream_exp,
            save_player_girl_cookie_exp = PlayerStatus.player_girl_cookie_exp,
            save_player_girl_chocolate_exp = PlayerStatus.player_girl_chocolate_exp,
            save_player_girl_crepe_exp = PlayerStatus.player_girl_crepe_exp,
            save_player_girl_creampuff_exp = PlayerStatus.player_girl_creampuff_exp,
            save_player_girl_donuts_exp = PlayerStatus.player_girl_donuts_exp,
            save_player_girl_cake_exp = PlayerStatus.player_girl_cake_exp,
            save_player_girl_rusk_exp = PlayerStatus.player_girl_rusk_exp,
            save_player_girl_candy_exp = PlayerStatus.player_girl_candy_exp,
            save_player_girl_jelly_exp = PlayerStatus.player_girl_jelly_exp,
            save_player_girl_juice_exp = PlayerStatus.player_girl_juice_exp,
            save_player_girl_tea_exp = PlayerStatus.player_girl_tea_exp,
            save_player_girl_icecream_exp = PlayerStatus.player_girl_icecream_exp,
            save_player_girl_rareokashi_exp = PlayerStatus.player_girl_rareokashi_exp,

            save_player_girl_appaleil_lv = PlayerStatus.player_girl_appaleil_lv,
            save_player_girl_cream_lv = PlayerStatus.player_girl_cream_lv,
            save_player_girl_cookie_lv = PlayerStatus.player_girl_cookie_lv,
            save_player_girl_chocolate_lv = PlayerStatus.player_girl_chocolate_lv,
            save_player_girl_crepe_lv = PlayerStatus.player_girl_crepe_lv,
            save_player_girl_creampuff_lv = PlayerStatus.player_girl_creampuff_lv,
            save_player_girl_donuts_lv = PlayerStatus.player_girl_donuts_lv,
            save_player_girl_cake_lv = PlayerStatus.player_girl_cake_lv,
            save_player_girl_rusk_lv = PlayerStatus.player_girl_rusk_lv,
            save_player_girl_candy_lv = PlayerStatus.player_girl_candy_lv,
            save_player_girl_jelly_lv = PlayerStatus.player_girl_jelly_lv,
            save_player_girl_juice_lv = PlayerStatus.player_girl_juice_lv,
            save_player_girl_tea_lv = PlayerStatus.player_girl_tea_lv,
            save_player_girl_icecream_lv = PlayerStatus.player_girl_icecream_lv,
            save_player_girl_rareokashi_lv = PlayerStatus.player_girl_rareokashi_lv,


            //日付・フラグ関係
            save_player_day = PlayerStatus.player_day, //現在の日付
            //save_player_time = PlayerStatus.player_time, //現在の時刻
            save_player_cullent_hour = PlayerStatus.player_cullent_hour, //現在の時間
            save_player_cullent_minute = PlayerStatus.player_cullent_minute, //現在の分

            save_First_recipi_on = PlayerStatus.First_recipi_on,
            save_First_extreme_on = PlayerStatus.First_extreme_on,

            save_special_animatFirst = girl1_status.special_animatFirst,

            //コスチューム番号
            //save_costume_num = GameMgr.Costume_Num,
            //save_acce_num = GameMgr.Accesory_Num,

            //飾っているアイテムのリスト
            save_BGAcceItemsName = _temp_bgacce_flaglist,

            //コレクションに登録したアイテムのリスト
            save_CollectionItems = GameMgr.CollectionItems,

            //ステージ番号
            save_stage_number = GameMgr.stage_number,
            save_stage_quest_num = GameMgr.stage_quest_num,
            save_stage_quest_num_sub = GameMgr.stage_quest_num_sub,

            //シナリオの進み具合
            save_scenario_flag = GameMgr.scenario_flag,

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

            //好感度ハイスコアイベントの取得フラグ
            save_OkashiQuestHighScore_event = GameMgr.OkashiQuestHighScore_event,

            //ビギナーフラグ
            save_Beginner_flag = GameMgr.Beginner_flag,

            //お菓子クエストフラグ
            save_OkashiQuest_flag_stage1 = GameMgr.OkashiQuest_flag_stage1, //各SPイベントのクリアしたかどうかのフラグ。
            save_OkashiQuest_flag_stage2 = GameMgr.OkashiQuest_flag_stage2,
            save_OkashiQuest_flag_stage3 = GameMgr.OkashiQuest_flag_stage3,

            //現在のクエストクリアフラグ
            save_QuestClearflag = GameMgr.QuestClearflag,
            save_QuestClearButton_anim = GameMgr.QuestClearButton_anim,

            //ヒカリのお菓子作り系フラグ
            save_hikari_kettei_item = GameMgr.hikari_kettei_item,
            save_hikari_kettei_originalID = GameMgr.hikari_kettei_originalID,
            save_hikari_kettei_kosu = GameMgr.hikari_kettei_kosu,
            save_hikari_kettei_toggleType = GameMgr.hikari_kettei_toggleType,
            save_hikari_kettei_itemName = GameMgr.hikari_kettei_itemName,
            save_hikari_make_okashiFlag = GameMgr.hikari_make_okashiFlag, //ヒカリがお菓子を制作中かどうかのフラグ
            save_hikari_makeokashi_startflag = GameMgr.hikari_makeokashi_startflag,
            save_hikari_make_okashiID = GameMgr.hikari_make_okashiID,
            save_hikari_make_okashi_compID = GameMgr.hikari_make_okashi_compID, //CompoDBのID
            save_hikari_make_okashiTimeCost = GameMgr.hikari_make_okashiTimeCost, //かかる時間
            save_hikari_make_okashiTimeCounter = GameMgr.hikari_make_okashiTimeCounter, //制作時間のタイマー
            save_hikari_make_success_rate = GameMgr.hikari_make_success_rate, //成功率
            save_hikari_make_doubleItemCreated = GameMgr.hikari_make_doubleItemCreated,
            save_hikari_make_okashi_totalkyori = GameMgr.hikari_make_okashi_totalkyori,
            save_hikari_make_okashiKosu = GameMgr.hikari_make_okashiKosu,
            save_hikari_make_success_count = GameMgr.hikari_make_success_count,
            save_hikari_make_failed_count = GameMgr.hikari_make_failed_count,

            //クリアお菓子の情報
            save_SpecialQuestClear_okashiItemID = GameMgr.SpecialQuestClear_okashiItemID,

            //さっき食べたお菓子の情報
            save_Okashi_lasthint = GameMgr.Okashi_lasthint, //さっき食べたお菓子のヒント。
            save_Okashi_lastname = GameMgr.Okashi_lastname, //さっき食べたお菓子の名前。
            save_Okashi_lastslot = GameMgr.Okashi_lastslot, //さっき食べたお菓子のスロット。
            save_Okashi_lastID = GameMgr.Okashi_lastID, //さっき食べたお菓子のアイテムID
            save_Okashi_totalscore = GameMgr.Okashi_totalscore, //さっき食べたお菓子の点数
            save_Okashi_lastshokukan_param = GameMgr.Okashi_lastshokukan_param, //さっき食べたお菓子のパラメータ
            save_Okashi_lastshokukan_mes = GameMgr.Okashi_lastshokukan_mes, //さっき食べたお菓子のパラメータ
            save_Okashi_lastsweat_param = GameMgr.Okashi_lastsweat_param, //さっき食べたお菓子のパラメータ
            save_Okashi_lastsour_param = GameMgr.Okashi_lastsour_param, //さっき食べたお菓子のパラメータ
            save_Okashi_lastbitter_param = GameMgr.Okashi_lastbitter_param, //さっき食べたお菓子のパラメータ
            save_Okashi_quest_bunki_on = GameMgr.Okashi_quest_bunki_on, //条件分岐しているか否かのフラグ
            save_high_score_flag = GameMgr.high_score_flag, //高得点でクリアしたというフラグ。
            save_Okashi_lasttotalscore = GameMgr.Okashi_last_totalscore, //前回食べたお菓子の点数。メモ保存用。

            save_Okashi_toplast_score = GameMgr.Okashi_toplast_score,
            save_Okashi_toplast_heart = GameMgr.Okashi_toplast_heart,

            save_Okashi_spquest_eatkaisu = GameMgr.Okashi_spquest_eatkaisu, //そのクエスト内で、お菓子を食べた回数をカウント
            save_Okashi_spquest_MaxScore = GameMgr.Okashi_spquest_MaxScore, //そのクエスト内で、お菓子の最高得点
            save_Okashi_Extra_SpEvent_Start = GameMgr.Okashi_Extra_SpEvent_Start, //ハート系クエストで、食べたお菓子が一定回数以下のとき、発動するクエスト
            save_ExtraClear_QuestItemRank = GameMgr.ExtraClear_QuestItemRank,      //エクストラクエストクリア時のご褒美のランク

            save_NowEatOkashiName = GameMgr.NowEatOkashiName, //今食べたいお菓子の名前表示
            save_NowEatOkashiID = GameMgr.NowEatOkashiID, //今食べたいお菓子ID表示

            //マップイベントフラグ
            save_MapEvent_01 = GameMgr.MapEvent_01,         //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
            save_MapEvent_02 = GameMgr.MapEvent_02,         //井戸。
            save_MapEvent_03 = GameMgr.MapEvent_03,         //ストロベリーガーデン
            save_MapEvent_04 = GameMgr.MapEvent_04,         //ひまわりの丘
            save_MapEvent_05 = GameMgr.MapEvent_05,         //ラベンダー
            save_MapEvent_06 = GameMgr.MapEvent_06,         //バードサンクチュアリ
            save_MapEvent_07 = GameMgr.MapEvent_07,         //ベリーファーム
            save_MapEvent_08 = GameMgr.MapEvent_08,         //白猫のおはか

            //広場でのイベント
            save_hiroba_event_end = GameMgr.hiroba_event_end,           

            //ショップのイベントリスト
            save_ShopEvent_stage = GameMgr.ShopEvent_stage,
            save_ShopLvEvent_stage = GameMgr.ShopLVEvent_stage,

            //ショップの在庫
            save_shopzaiko = _temp_shopzaiko,
            save_farmzaiko = _temp_farmzaiko,
            save_emeraldshop_zaiko = _temp_emeraldshop_zaiko,

            //酒場のイベントリスト
            save_BarEvent_stage = GameMgr.BarEvent_stage,

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
            save_eventitemlist = _temp_eventitemlist,

            //プレイヤーのエメラルドアイテムリスト。
            //save_player_emeralditemlist = _temp_emeralditemlist,

            //アイテムリスト＜オリジナル＞
            save_player_originalitemlist = pitemlist.player_originalitemlist,

            //アイテムリスト＜予測＞
            save_player_yosokuitemlist = pitemlist.player_yosokuitemlist,

            //お菓子パネルのアイテムリスト。
            save_player_extremepanel_itemlist = pitemlist.player_extremepanel_itemlist,

            //アイテムの前回スコアなどを記録する
            save_itemdatabase = _temp_itemscorelist,

            //調合のフラグ＋調合回数を記録する
            save_itemCompodatabase = _temp_cmpflaglist,

            //今うけてるクエストを保存する。
            save_questTakeset = quest_database.questTakeset,

            //マップフラグリスト
            save_mapflaglist = _tempmap_placeflaglist,

            //エクストリームパネルのアイテムIDも保存
            save_extreme_itemid = GameMgr.sys_extreme_itemID,
            save_extreme_itemtype = GameMgr.sys_extreme_itemType,

            //お菓子の一度にトッピングできる回数
            save_topping_Set_Count = GameMgr.topping_Set_Count,

            save_mainBGM_Num = GameMgr.mainBGM_Num,

            //save_picnic_End = GameMgr.picnic_End,
            save_picnic_count = GameMgr.picnic_count,
            save_picnic_event_ON = GameMgr.picnic_event_ON,

            //outgirl_End = GameMgr.outgirl_End,
            outgirl_count = GameMgr.outgirl_count,
            outgirl_event_ON = GameMgr.outgirl_event_ON,
            outgirl_Nowprogress = GameMgr.outgirl_Nowprogress,

            save_hiroba_ichigo_first = GameMgr.hiroba_ichigo_first,
            save_ichigo_collection_listFlag = GameMgr.ichigo_collection_listFlag,

            //ユーザーが設定した好みの音楽
            save_userBGM_Num = GameMgr.userBGM_Num,

            //ゲームスピード
            save_GameSpeedParam = GameMgr.GameSpeedParam,
        };

        //デバッグ用
        Debug.Log("セーブ　GameMgr.GirlLoveEvent_num:" + GameMgr.GirlLoveEvent_num);
        //Debug.Log(playerData);

        //データの一時保存。bankに、playerDataを「player1」という名前で現在のデータを保存。
        bank.Store("player1", playerData);
        Debug.Log("bank.Store()");

        //一時データを永続的に保存。永続保存するときは、一度、一時データに保存しておく。
        bank.Save("player1");
        Debug.Log("bank.Save(player1)");

        //システムデータもセーブ
        SystemsaveCheck();

    }

    //ロード処理
    public void OnLoadMethod()
    {
        //ロード前は一度初期化
        ResetAllParam();

        SystemloadCheck(); //システムデータロード
        PlayerDataLoad();
        

        //セーブデータがあるかチェック
        if (GameMgr.saveOK)
        {
            LoadingContents();
            Debug.Log("ロード完了");
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
        PlayerStatus.player_girl_eatCount = playerData.save_player_girl_eatCount; //妹が食べたお菓子の回数
        PlayerStatus.player_girl_eatCount_tabetai = playerData.save_player_girl_eatCount_tabetai; //妹が食べたいお菓子をあげた回数
        PlayerStatus.player_girl_manpuku = playerData.save_player_girl_manpuku; //妹の満腹度
                                                                                //PlayerStatus.player_girl_yaruki = playerData.save_player_girl_yaruki; //妹のやる気

        //お菓子経験値　全１５種類
        PlayerStatus.player_girl_appaleil_exp = playerData.save_player_girl_appaleil_exp;
        PlayerStatus.player_girl_cream_exp = playerData.save_player_girl_cream_exp;
        PlayerStatus.player_girl_cookie_exp = playerData.save_player_girl_cookie_exp;
        PlayerStatus.player_girl_chocolate_exp = playerData.save_player_girl_chocolate_exp;
        PlayerStatus.player_girl_crepe_exp = playerData.save_player_girl_crepe_exp;
        PlayerStatus.player_girl_creampuff_exp = playerData.save_player_girl_creampuff_exp;
        PlayerStatus.player_girl_donuts_exp = playerData.save_player_girl_donuts_exp;
        PlayerStatus.player_girl_cake_exp = playerData.save_player_girl_cake_exp;
        PlayerStatus.player_girl_rusk_exp = playerData.save_player_girl_rusk_exp;
        PlayerStatus.player_girl_candy_exp = playerData.save_player_girl_candy_exp;
        PlayerStatus.player_girl_jelly_exp = playerData.save_player_girl_jelly_exp;
        PlayerStatus.player_girl_juice_exp = playerData.save_player_girl_juice_exp;
        PlayerStatus.player_girl_tea_exp = playerData.save_player_girl_tea_exp;
        PlayerStatus.player_girl_icecream_exp = playerData.save_player_girl_icecream_exp;
        PlayerStatus.player_girl_rareokashi_exp = playerData.save_player_girl_rareokashi_exp;

        if (playerData.save_player_girl_appaleil_lv == 0)
        { }
        else
        {
            PlayerStatus.player_girl_appaleil_lv = playerData.save_player_girl_appaleil_lv;
            PlayerStatus.player_girl_cream_lv = playerData.save_player_girl_cream_lv;
            PlayerStatus.player_girl_cookie_lv = playerData.save_player_girl_cookie_lv;
            PlayerStatus.player_girl_chocolate_lv = playerData.save_player_girl_chocolate_lv;
            PlayerStatus.player_girl_crepe_lv = playerData.save_player_girl_crepe_lv;
            PlayerStatus.player_girl_creampuff_lv = playerData.save_player_girl_creampuff_lv;
            PlayerStatus.player_girl_donuts_lv = playerData.save_player_girl_donuts_lv;
            PlayerStatus.player_girl_cake_lv = playerData.save_player_girl_cake_lv;
            PlayerStatus.player_girl_rusk_lv = playerData.save_player_girl_rusk_lv;
            PlayerStatus.player_girl_candy_lv = playerData.save_player_girl_candy_lv;
            PlayerStatus.player_girl_jelly_lv = playerData.save_player_girl_jelly_lv;
            PlayerStatus.player_girl_juice_lv = playerData.save_player_girl_juice_lv;
            PlayerStatus.player_girl_tea_lv = playerData.save_player_girl_tea_lv;
            PlayerStatus.player_girl_icecream_lv = playerData.save_player_girl_icecream_lv;
            PlayerStatus.player_girl_rareokashi_lv = playerData.save_player_girl_rareokashi_lv;
        }

        //日付・フラグ関係
        PlayerStatus.player_day = playerData.save_player_day; //現在の日付
        //PlayerStatus.player_time = playerData.save_player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分
        PlayerStatus.player_cullent_hour = playerData.save_player_cullent_hour; //現在の時間
        PlayerStatus.player_cullent_minute = playerData.save_player_cullent_minute; //現在の分

        PlayerStatus.First_recipi_on = playerData.save_First_recipi_on;
        PlayerStatus.First_extreme_on = playerData.save_First_extreme_on;

        girl1_status.special_animatFirst = playerData.save_special_animatFirst;

        //コスチューム番号
        //GameMgr.Costume_Num = playerData.save_costume_num;
        //GameMgr.Accesory_Num = playerData.save_acce_num;

        //コレクションに登録したアイテムのリスト
        GameMgr.CollectionItems = playerData.save_CollectionItems;

        //ステージ番号
        GameMgr.stage_number = playerData.save_stage_number;
        GameMgr.stage_quest_num = playerData.save_stage_quest_num;
        GameMgr.stage_quest_num_sub = playerData.save_stage_quest_num_sub;

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

        //好感度ハイスコアイベントの取得フラグ
        GameMgr.OkashiQuestHighScore_event = playerData.save_OkashiQuestHighScore_event;

        //ビギナーフラグ
        GameMgr.Beginner_flag = playerData.save_Beginner_flag;

        //お菓子クエストフラグ
        GameMgr.OkashiQuest_flag_stage1 = playerData.save_OkashiQuest_flag_stage1; //各SPイベントのクリアしたかどうかのフラグ。
        GameMgr.OkashiQuest_flag_stage2 = playerData.save_OkashiQuest_flag_stage2;
        GameMgr.OkashiQuest_flag_stage3 = playerData.save_OkashiQuest_flag_stage3;

        GameMgr.QuestClearflag = playerData.save_QuestClearflag;
        GameMgr.QuestClearButton_anim = playerData.save_QuestClearButton_anim;

        //ヒカリのお菓子作り系フラグ
        GameMgr.hikari_kettei_item = playerData.save_hikari_kettei_item;
        GameMgr.hikari_kettei_originalID = playerData.save_hikari_kettei_originalID;
        GameMgr.hikari_kettei_kosu = playerData.save_hikari_kettei_kosu;
        GameMgr.hikari_kettei_toggleType = playerData.save_hikari_kettei_toggleType;
        GameMgr.hikari_make_okashiFlag = playerData.save_hikari_make_okashiFlag; //ヒカリがお菓子を制作中かどうかのフラグ
        GameMgr.hikari_makeokashi_startflag = playerData.save_hikari_makeokashi_startflag;
        GameMgr.hikari_make_okashiID = playerData.save_hikari_make_okashiID;
        GameMgr.hikari_make_okashi_compID = playerData.save_hikari_make_okashi_compID; //CompoDBのID
        GameMgr.hikari_make_okashiTimeCost = playerData.save_hikari_make_okashiTimeCost; //かかる時間
        GameMgr.hikari_make_okashiTimeCounter = playerData.save_hikari_make_okashiTimeCounter; //制作時間のタイマー
        GameMgr.hikari_make_success_rate = playerData.save_hikari_make_success_rate; //成功率
        GameMgr.hikari_make_doubleItemCreated = playerData.save_hikari_make_doubleItemCreated;
        GameMgr.hikari_make_okashi_totalkyori = playerData.save_hikari_make_okashi_totalkyori;
        GameMgr.hikari_make_okashiKosu = playerData.save_hikari_make_okashiKosu;
        GameMgr.hikari_make_success_count = playerData.save_hikari_make_success_count;
        GameMgr.hikari_make_failed_count = playerData.save_hikari_make_failed_count;


        if (playerData.save_hikari_kettei_itemName != null)
        {
            GameMgr.hikari_kettei_itemName = playerData.save_hikari_kettei_itemName;
        }

        //クリアお菓子の情報
        GameMgr.SpecialQuestClear_okashiItemID = playerData.save_SpecialQuestClear_okashiItemID;

        //さっき食べたお菓子の情報
        GameMgr.Okashi_lasthint = playerData.save_Okashi_lasthint; //さっき食べたお菓子のヒント。
        GameMgr.Okashi_lastname = playerData.save_Okashi_lastname; //さっき食べたお菓子の名前。
        GameMgr.Okashi_lastslot = playerData.save_Okashi_lastslot; //さっき食べたお菓子のスロット。
        GameMgr.Okashi_lastID = playerData.save_Okashi_lastID; //さっき食べたお菓子のアイテムID
        GameMgr.Okashi_totalscore = playerData.save_Okashi_totalscore; //さっき食べたお菓子の点数
        GameMgr.Okashi_lastshokukan_param = playerData.save_Okashi_lastshokukan_param; //さっき食べたお菓子のパラメータ
        GameMgr.Okashi_lastshokukan_mes = playerData.save_Okashi_lastshokukan_mes; //さっき食べたお菓子のパラメータ
        GameMgr.Okashi_lastsweat_param = playerData.save_Okashi_lastsweat_param; //さっき食べたお菓子のパラメータ
        GameMgr.Okashi_lastsour_param = playerData.save_Okashi_lastsour_param; //さっき食べたお菓子のパラメータ
        GameMgr.Okashi_lastbitter_param = playerData.save_Okashi_lastbitter_param; //さっき食べたお菓子のパラメータ
        GameMgr.Okashi_quest_bunki_on = playerData.save_Okashi_quest_bunki_on; //条件分岐しているか否かのフラグ
        GameMgr.high_score_flag = playerData.save_high_score_flag; //高得点でクリアしたというフラグ。
        GameMgr.Okashi_last_totalscore = playerData.save_Okashi_lasttotalscore; //前回食べたお菓子の点数。メモ保存用。

        GameMgr.Okashi_toplast_score = playerData.save_Okashi_toplast_score;
        GameMgr.Okashi_toplast_heart = playerData.save_Okashi_toplast_heart;

        GameMgr.Okashi_spquest_eatkaisu = playerData.save_Okashi_spquest_eatkaisu; //そのクエスト内で、お菓子を食べた回数をカウント
        GameMgr.Okashi_spquest_MaxScore = playerData.save_Okashi_spquest_MaxScore; //そのクエスト内で、お菓子の最高得点
        GameMgr.Okashi_Extra_SpEvent_Start = playerData.save_Okashi_Extra_SpEvent_Start; //ハート系クエストで、食べたお菓子が一定回数以下のとき、発動するクエスト
        GameMgr.ExtraClear_QuestItemRank = playerData.save_ExtraClear_QuestItemRank;      //エクストラクエストクリア時のご褒美のランク

        if (playerData.save_NowEatOkashiName != null)
        {
            GameMgr.NowEatOkashiName = playerData.save_NowEatOkashiName; //今食べたいお菓子の名前表示
            GameMgr.NowEatOkashiID = playerData.save_NowEatOkashiID; //今食べたいお菓子ID表示
        }

        //マップイベントフラグ
        GameMgr.MapEvent_01 = playerData.save_MapEvent_01;        //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
        GameMgr.MapEvent_02 = playerData.save_MapEvent_02;        //井戸。
        GameMgr.MapEvent_03 = playerData.save_MapEvent_03;        //ストロベリーガーデン
        GameMgr.MapEvent_04 = playerData.save_MapEvent_04;        //ひまわりの丘
        GameMgr.MapEvent_05 = playerData.save_MapEvent_05;        //ラベンダー
        GameMgr.MapEvent_06 = playerData.save_MapEvent_06;        //バードサンクチュアリ

        if (playerData.save_MapEvent_07 == null)
        {
            //バージョンが新しくなっていた場合は、ロードしてしまうと、nullが入ってしまうので、回避用。ver1.20で以下二つのマップイベント配列を追加。
        }
        else
        {
            GameMgr.MapEvent_07 = playerData.save_MapEvent_07;        //ベリーファーム          
        }
        if (playerData.save_MapEvent_08 == null)
        {
            //バージョンが新しくなっていた場合は、ロードしてしまうと、nullが入ってしまうので、回避用。ver1.20で以下二つのマップイベント配列を追加。
        }
        else
        {
            GameMgr.MapEvent_08 = playerData.save_MapEvent_08;        //白猫のおはか
        }

        //広場でのイベント
        GameMgr.hiroba_event_end = playerData.save_hiroba_event_end;       

        //ショップのイベントリスト
        GameMgr.ShopEvent_stage = playerData.save_ShopEvent_stage;
        GameMgr.ShopLVEvent_stage = playerData.save_ShopLvEvent_stage;

        //酒場のイベントリスト
        GameMgr.BarEvent_stage = playerData.save_BarEvent_stage;

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


        //飾っているアイテムのリスト
        for (i = 0; i < playerData.save_BGAcceItemsName.Count; i++)
        {
            GameMgr.SetBGAcceFlag(playerData.save_BGAcceItemsName[i].itemName, playerData.save_BGAcceItemsName[i].Flag);
        }

        //アイテムリスト＜デフォルト＞
        for (i = 0; i < playerData.save_playeritemlist.Count; i++)
        {       
            pitemlist.ReSetPlayerItemString(playerData.save_playeritemlist[i].itemName, playerData.save_playeritemlist[i].itemKosu);
        }

        //プレイヤーのイベントアイテムリスト。
        for (i = 0; i < playerData.save_eventitemlist.Count; i++)
        {
            pitemlist.ReSetEventItemString(playerData.save_eventitemlist[i].itemName, playerData.save_eventitemlist[i].itemKosu, playerData.save_eventitemlist[i].Flag);
        }

        //プレイヤーのエメラルドアイテムリスト。
        /*for (i = 0; i < playerData.save_player_emeralditemlist.Count; i++)
        {
            pitemlist.ReSetEmeraldItemString(playerData.save_player_emeralditemlist[i].itemName, playerData.save_player_emeralditemlist[i].itemKosu);
        }*/

        //アイテムリスト＜オリジナル＞
        pitemlist.player_originalitemlist.Clear();
        pitemlist.player_originalitemlist = playerData.save_player_originalitemlist;
       
        //テクスチャのデータは保存すると壊れてしまうので、ここで入れ直す。アイテムIDも、後でDB更新の際に全てずれる可能性があるので入れ直し。その他、
        //アップデートでパラメータをどうしても書き換えたいものは、ここで入れる。
        for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
        {
            _itemID = pitemlist.SearchItemString(pitemlist.player_originalitemlist[i].itemName);
            pitemlist.player_originalitemlist[i].itemIcon_sprite = database.items[_itemID].itemIcon_sprite;
            pitemlist.player_originalitemlist[i].itemID = database.items[_itemID].itemID;
            pitemlist.player_originalitemlist[i].girl1_itemLike = database.items[_itemID].girl1_itemLike;
            pitemlist.player_originalitemlist[i].cost_price = database.items[_itemID].cost_price;
            pitemlist.player_originalitemlist[i].sell_price = database.items[_itemID].sell_price;
        }

        //アイテムリスト＜予測＞
        pitemlist.player_yosokuitemlist.Clear();
        pitemlist.player_yosokuitemlist = playerData.save_player_yosokuitemlist;      

        //テクスチャのデータは保存すると壊れてしまうので、ここで入れ直す。アイテムIDも、後でDB更新の際に全てずれる可能性があるので入れ直し。
        for (i = 0; i < pitemlist.player_yosokuitemlist.Count; i++)
        {
            _itemID = pitemlist.SearchItemString(pitemlist.player_yosokuitemlist[i].itemName);
            pitemlist.player_yosokuitemlist[i].itemIcon_sprite = database.items[_itemID].itemIcon_sprite;
            pitemlist.player_yosokuitemlist[i].itemID = database.items[_itemID].itemID;
            pitemlist.player_yosokuitemlist[i].girl1_itemLike = database.items[_itemID].girl1_itemLike;
            pitemlist.player_yosokuitemlist[i].cost_price = database.items[_itemID].cost_price;
            pitemlist.player_yosokuitemlist[i].sell_price = database.items[_itemID].sell_price;
        }

        //お菓子パネルのアイテムリスト。
        pitemlist.player_extremepanel_itemlist.Clear();
        pitemlist.player_extremepanel_itemlist = playerData.save_player_extremepanel_itemlist;

        //テクスチャのデータは保存すると壊れてしまうので、ここで入れ直す。アイテムIDも、後でDB更新の際に全てずれる可能性があるので入れ直し。
        for (i = 0; i < pitemlist.player_extremepanel_itemlist.Count; i++)
        {
            _itemID = pitemlist.SearchItemString(pitemlist.player_extremepanel_itemlist[i].itemName);
            pitemlist.player_extremepanel_itemlist[i].itemIcon_sprite = database.items[_itemID].itemIcon_sprite;
            pitemlist.player_extremepanel_itemlist[i].itemID = database.items[_itemID].itemID;
            pitemlist.player_extremepanel_itemlist[i].girl1_itemLike = database.items[_itemID].girl1_itemLike;
            pitemlist.player_extremepanel_itemlist[i].cost_price = database.items[_itemID].cost_price;
            pitemlist.player_extremepanel_itemlist[i].sell_price = database.items[_itemID].sell_price;
        }

        //アイテムの前回スコアなどを読み込み ゲームデータのほうは、前回スコアは触らない。
        /*for (count = 0; count < playerData.save_itemdatabase.Count; count++)
        {
            i = 0;
            while (i < database.items.Count)
            {
                if (playerData.save_itemdatabase[count].itemName == database.items[i].itemName)
                {
                    database.items[i].HighScore_flag = playerData.save_itemdatabase[count].HighScore_flag;
                    database.items[i].Eat_kaisu = playerData.save_itemdatabase[count].Eat_kaisu;    
                    
                    database.items[i].last_total_score = playerData.save_itemdatabase[count].last_total_score;
                    database.items[i].last_rich_score = playerData.save_itemdatabase[count].last_rich_score;
                    database.items[i].last_sweat_score = playerData.save_itemdatabase[count].last_sweat_score;
                    database.items[i].last_bitter_score = playerData.save_itemdatabase[count].last_bitter_score;
                    database.items[i].last_sour_score = playerData.save_itemdatabase[count].last_sour_score;
                    database.items[i].last_crispy_score = playerData.save_itemdatabase[count].last_crispy_score;
                    database.items[i].last_fluffy_score = playerData.save_itemdatabase[count].last_fluffy_score;
                    database.items[i].last_smooth_score = playerData.save_itemdatabase[count].last_smooth_score;
                    database.items[i].last_hardness_score = playerData.save_itemdatabase[count].last_hardness_score;
                    database.items[i].last_jiggly_score = playerData.save_itemdatabase[count].last_jiggly_score;
                    database.items[i].last_chewy_score = playerData.save_itemdatabase[count].last_chewy_score;
                    database.items[i].last_hinttext = playerData.save_itemdatabase[count].last_hinttext;
                    break;
                }
                i++;
            }                 
            
        }*/

        //調合のフラグ＋調合回数を記録　プレイヤーデータではしない。ヒカリの制作フラグだけ読み込み。
        for (count = 0; count < playerData.save_itemCompodatabase.Count; count++)
        {
            i = 0;
            while (i < databaseCompo.compoitems.Count)
            {
                if (playerData.save_itemCompodatabase[count].comp_name == databaseCompo.compoitems[i].cmpitem_Name)
                {
                    //databaseCompo.compoitems[i].cmpitem_flag = playerData.save_itemCompodatabase[count].comp_Flag;
                    //databaseCompo.compoitems[i].comp_count= playerData.save_itemCompodatabase[count].comp_Count;
                    //databaseCompo.compoitems[i].hikari_make_count = playerData.save_itemCompodatabase[count].hikarimake_Count;
                    break;
                }
                i++;
            }
        }


        //今うけてるクエストをロード。
        quest_database.questTakeset.Clear();
        quest_database.questTakeset = playerData.save_questTakeset;

        //テクスチャのデータは入れ直し。
        for (i = 0; i < quest_database.questTakeset.Count; i++)
        {
            quest_database.ResetSpriteTex(i);
        }

        //マップフラグの読み込み
        for (i = 0; i < playerData.save_mapflaglist.Count; i++)
        {
            matplace_database.ReSetMapFlagString(playerData.save_mapflaglist[i].itemName, playerData.save_mapflaglist[i].Flag);
        }

        //ショップの在庫読み込み
        for (i = 0; i < playerData.save_shopzaiko.Count; i++)
        {
            shop_database.ReSetShopItemString(playerData.save_shopzaiko[i].itemName, playerData.save_shopzaiko[i].itemKosu);
        }
        for (i = 0; i < playerData.save_farmzaiko.Count; i++)
        {
            shop_database.ReSetFarmItemString(playerData.save_farmzaiko[i].itemName, playerData.save_farmzaiko[i].itemKosu);
        }
        for (i = 0; i < playerData.save_emeraldshop_zaiko.Count; i++)
        {
            shop_database.ReSetEmeraldItemString(playerData.save_emeraldshop_zaiko[i].itemName, playerData.save_emeraldshop_zaiko[i].itemKosu);
        }

        //エクストリームパネルのアイテムを読み込み
        GameMgr.sys_extreme_itemID = playerData.save_extreme_itemid;
        GameMgr.sys_extreme_itemType = playerData.save_extreme_itemtype;

        //お菓子の一度にトッピングできる回数
        GameMgr.topping_Set_Count = playerData.save_topping_Set_Count;       

        GameMgr.mainBGM_Num = playerData.save_mainBGM_Num;

        GameMgr.picnic_count = playerData.save_picnic_count;
        GameMgr.picnic_event_ON = playerData.save_picnic_event_ON;
        if (GameMgr.picnic_count <= 0)
        {
            GameMgr.picnic_event_ON = true;
        }

        GameMgr.outgirl_count = playerData.outgirl_count;
        GameMgr.outgirl_event_ON = playerData.outgirl_event_ON;
        GameMgr.outgirl_Nowprogress = playerData.outgirl_Nowprogress;

        if(GameMgr.outgirl_count <= 0)
        {
            GameMgr.outgirl_event_ON = true;
        }

        Debug.Log("GameMgr.outgirl_count: " + GameMgr.outgirl_count);
        Debug.Log("GameMgr.outgirl_event_ON " + GameMgr.outgirl_event_ON);

        GameMgr.hiroba_ichigo_first = playerData.save_hiroba_ichigo_first;
        GameMgr.ichigo_collection_listFlag = playerData.save_ichigo_collection_listFlag;

        //ユーザーが設定した好みの音楽
        if(playerData.save_userBGM_Num <= 0 || playerData.save_userBGM_Num > GameMgr.bgm_collection_list.Count) { GameMgr.userBGM_Num = 0; }
        else { GameMgr.userBGM_Num = playerData.save_userBGM_Num; }

        //ゲームスピード
        if (playerData.save_GameSpeedParam < 1 || playerData.save_GameSpeedParam > 6) { GameMgr.GameSpeedParam = 3; } //例外処理
        else { GameMgr.GameSpeedParam = playerData.save_GameSpeedParam; }

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

                money_status = canvas.transform.Find("MainUIPanel/Comp/MoneyStatus_panel").GetComponent<MoneyStatus_Controller>();

                //Live2Dモデルの取得
                _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;

                //メイン画面に表示する、現在のクエスト
                questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();

                //BGMの取得
                sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

                //サウンドコントローラーの取得
                sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
                sc.VolumeSetting();

                StageClearButton_panel = canvas.transform.Find("MainUIPanel/StageClearButton_Panel").gameObject;
                StageClearbutton_audio = StageClearButton_panel.GetComponent<AudioSource>();
                StageClearbutton_audio.volume = 1.0f * GameMgr.MasterVolumeParam * GameMgr.SeVolumeParam;

                //飾りアイテムのセット
                BGAccetrigger = GameObject.FindWithTag("BGAccessory").GetComponent<BGAcceTrigger>();

                money_status.money_Draw();
                questname.text = girl1_status.OkashiQuest_Name;
                sceneBGM.PlayMain(); //BGMの更新                               

                if (pitemlist.player_extremepanel_itemlist.Count > 0)
                {
                    extreme_panel = canvas.transform.Find("MainUIPanel/ExtremePanel").GetComponentInChildren<ExtremePanel>();
                    //Debug.Log("GameMgr.sys_extreme_itemID: " + GameMgr.sys_extreme_itemID);
                    //Debug.Log("GameMgr.sys_extreme_itemType: " + GameMgr.sys_extreme_itemType);

                    extreme_panel.SetExtremeItem(0, 2);
                    extreme_panel.SetInitParamExtreme();

                }

                //衣装チェンジ
                _model_obj.GetComponent<Live2DCostumeTrigger>().ChangeCostume();
                _model_obj.GetComponent<Live2DCostumeTrigger>().ChangeAcce();

                //飾りアイテムのセット
                BGAccetrigger.DrawBGAcce();

                //背景を自動で変更
                compound_Main.Change_BGimage();

                GameMgr.compound_status = 0;

                //ロード直後のサブイベントを発生させる
                GameMgr.Load_eventflag = true; //ロード直後に、おかえりなさい～のようなサブイベントを発生
                GameMgr.check_GirlLoveEvent_flag = false; //compound_Mainからロードしても、おかえりなさい～が発生               
                break;
        }

        //まずクエストを再設定
        special_quest.SetSpecialOkashi(GameMgr.GirlLoveEvent_num, 1); //クエスト番号を再設定ここで。
        special_quest.RedrawQuestName();

        //そのあと、クエストに応じて、各要素の再設定
        if (GameMgr.Story_Mode == 0)
        {
            girl1_status.OkashiNew_Status = 0;
        }
        else
        {
            girl1_status.OkashiNew_Status = 2; //エクストラモード
        }
        girl1_status.special_animatFirst = true;
        girl1_status.Girl_EatDecide();         

        debug_panel.GirlLove_Koushin(PlayerStatus.girl1_Love_exp); //好感度ステータスに応じたキャラの表情やLive2Dモーション更新
        GameMgr.KeyInputOff_flag = true;
        
    }

    //ゲーム「はじめから」で、リセットされる項目
    public void ResetAllParam()
    {
        PlayerStatus.Setup_PlayerStatus(); //プレイヤーステータスの初期化
        girl1_status.ResetDefaultStatus(); //女の子ステータスの初期化
        girlEat_judge.ResetLoadStatus(); //女の子ステータスの初期化
        GameMgr.ResetGameDefaultStatus(); //ゲームステータスの初期化　イベントフラグ関係は、ここで初期化

        //アイテムデータの初期化
        //アイテムリスト＜デフォルト＞
        for (i = 0; i < database.items.Count; i++)
        {
            pitemlist.playeritemlist[database.items[i].itemName] = 0;           
        }

        //プレイヤーのイベントアイテムリスト。
        pitemlist.ReseteventitemList();

        //アイテムリスト＜オリジナル＞
        pitemlist.player_originalitemlist.Clear();

        //アイテムの前回スコアなどを初期化
        database.ResetLastScore();

        //調合フラグ＋調合回数も初期化
        //databaseCompo.ResetDefaultCompoExcel();

        //アイテムDBの味の初期化
        //compound_keisan.ResetDefaultTasteParam();

        //マップフラグの初期化
        matplace_database.ResetDefaultMapExcel();

        //各ショップのイベントアイテムの在庫の初期化
        shop_database.ShopZaiko_Reset();

        //受けていた酒場クエストのリセット
        quest_database.ResetQuestTakeSet();

        //エクストリームパネルも空に。
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //キャンバスの読み込み
                canvas = GameObject.FindWithTag("Canvas");
                extreme_panel = canvas.transform.Find("MainUIPanel/ExtremePanel").GetComponentInChildren<ExtremePanel>();
                extreme_panel.deleteExtreme_Item();
                break;
        }

    }

    //ゲーム「はじめから」で、システムロード後に、再度リセットする。絶対にリセットしておきたいパラメータだけ、再度リセットするイメージ。
    public void ResetParamSecondTime()
    {
        for (i = 0; i < database.items.Count; i++)
        {
            database.items[i].Eat_kaisu = 0;

            //装備品の引き継ぎは、強制的にオフ
            if (database.items[i].itemType_sub.ToString() == "Equip")
            {
                pitemlist.playeritemlist[database.items[i].itemName] = 0;
            }

            //どんぐりはリスタートで必ず消す。
            //database.items[i].HighScore_flag = 0;
        }

        //ヒカリ制作フラグも必ず消す。
        for (i = 0; i < databaseCompo.compoitems.Count; i++)
        {
            databaseCompo.compoitems[i].hikari_make_count = 0;
        }
            

        //体力は全回復
        PlayerStatus.player_girl_lifepoint = PlayerStatus.player_girl_maxlifepoint;       

    }

    //ロードの準備
    void PlayerDataLoad()
    {
        //セーブデータを管理するデータバンクのインスタンスを取得します(シングルトン)
        DataBank bank = DataBank.Open();

        Debug.Log("DataBank.Open()");
        Debug.Log($"save path of bank is { bank.SavePath }");

        //初期化(念のため)
        playerData = new PlayerData();

        //永続的に保存しておいたデータを、一時データに読み込む。bankに読み込まれる。
        bank.Load<PlayerData>("player1");
        //Debug.Log("ロード完了");

        //一時データに再度読み込んだので、Getすると、再びパラメータを取得できる。
        playerData = bank.Get<PlayerData>("player1");

    }


    //システムデータのセーブ
    public void SystemsaveCheck()
    {
        //セーブデータを管理するデータバンクのインスタンスを取得します(シングルトン)
        DataBank bank = DataBank.Open();

        Debug.Log("DataBank.Open()");
        Debug.Log($"save path of bank is { bank.SavePath }");


        //周回しても引き継がれる要素

        //どんぐりと装備品やオーブン置物
        _tempdongrilist.Clear();
        for (i = 0; i < database.items.Count; i++)
        {
            if(database.items[i].itemType_sub.ToString() == "Donguri" ||
                database.items[i].itemType_sub.ToString() == "Object"
                || database.items[i].itemType_sub.ToString() == "Record")
            {
                _tempdongrilist.Add(new ItemSaveKosu(database.items[i].itemName, pitemlist.playeritemlist[database.items[i].itemName], 0));
            }
        }

        //エメラルドアイテム
        _temp_emeralditemlist.Clear();
        for (i = 0; i < pitemlist.emeralditemlist.Count; i++)
        {
            _temp_emeralditemlist.Add(new ItemSaveKosu(pitemlist.emeralditemlist[i].event_itemName, pitemlist.emeralditemlist[i].ev_itemKosu, 0));
        }

        //調合フラグと調合回数の取得 
        _temp_cmpflaglist.Clear();
        for (i = 0; i < databaseCompo.compoitems.Count; i++)
        {
            _temp_cmpflaglist.Add(new ItemSaveCompoFlag(databaseCompo.compoitems[i].cmpitem_Name, databaseCompo.compoitems[i].cmpitem_flag, 
                databaseCompo.compoitems[i].comp_count, databaseCompo.compoitems[i].hikari_make_count));
            //Debug.Log("databaseCompo.compoitems[i].cmpitem_flag: " + databaseCompo.compoitems[i].cmpitem_Name + " " + databaseCompo.compoitems[i].cmpitem_flag);
        }

        //アイテムの前回得点のみ取得
        _temp_itemscorelist.Clear();
        for (i = 0; i < database.items.Count; i++)
        {
            /*if (database.items[i].HighScore_flag < database.items_system[i].HighScore_flag)
            {
                _temp_itemscorelist.Add(new ItemSaveparam(database.items[i].itemID, database.items[i].itemName, database.items[i].Eat_kaisu, database.items_system[i].HighScore_flag, database.items[i].last_total_score,
                    database.items[i].last_rich_score, database.items[i].last_sweat_score, database.items[i].last_bitter_score, database.items[i].last_sour_score,
                    database.items[i].last_crispy_score, database.items[i].last_fluffy_score, database.items[i].last_smooth_score, database.items[i].last_hardness_score,
                    database.items[i].last_jiggly_score, database.items[i].last_chewy_score, database.items[i].last_hinttext));
            }
            else
            {
                _temp_itemscorelist.Add(new ItemSaveparam(database.items[i].itemID, database.items[i].itemName, database.items[i].Eat_kaisu, database.items[i].HighScore_flag, database.items[i].last_total_score,
                    database.items[i].last_rich_score, database.items[i].last_sweat_score, database.items[i].last_bitter_score, database.items[i].last_sour_score,
                    database.items[i].last_crispy_score, database.items[i].last_fluffy_score, database.items[i].last_smooth_score, database.items[i].last_hardness_score,
                    database.items[i].last_jiggly_score, database.items[i].last_chewy_score, database.items[i].last_hinttext));
            }*/

            _temp_itemscorelist.Add(new ItemSaveparam(database.items[i].itemID, database.items[i].itemName, database.items[i].Eat_kaisu, database.items[i].HighScore_flag, database.items[i].last_total_score,
                    database.items[i].last_rich_score, database.items[i].last_sweat_score, database.items[i].last_bitter_score, database.items[i].last_sour_score,
                    database.items[i].last_crispy_score, database.items[i].last_fluffy_score, database.items[i].last_smooth_score, database.items[i].last_hardness_score,
                    database.items[i].last_jiggly_score, database.items[i].last_chewy_score, database.items[i].last_hinttext));
        }

        //称号リスト
        _temp_titlecollectionlist.Clear();
        for (i = 0; i < GameMgr.title_collection_list.Count; i++)
        {
            _temp_titlecollectionlist.Add(new ItemSaveFlag(GameMgr.title_collection_list[i].titleName, 0, GameMgr.title_collection_list[i].Flag));
        }

        //イベントリスト
        _temp_eventcollectionlist.Clear();
        for (i = 0; i < GameMgr.event_collection_list.Count; i++)
        {
            _temp_eventcollectionlist.Add(new ItemSaveFlag(GameMgr.event_collection_list[i].titleName, 0, GameMgr.event_collection_list[i].Flag));
        }

        //コンテストクリアお菓子リスト
        _temp_contestclearcollectionlist.Clear();
        _temp_contestclearcollectionlistItemData.Clear();
        for (i = 0; i < GameMgr.contestclear_collection_list.Count; i++)
        {
            _temp_contestclearcollectionlist.Add(new ItemSaveFlag(GameMgr.contestclear_collection_list[i].titleName, GameMgr.contestclear_collection_list[i].Score, GameMgr.contestclear_collection_list[i].Flag));
            _temp_contestclearcollectionlistItemData.Add(GameMgr.contestclear_collection_list[i].ItemData);
        }

        //音楽図鑑リスト
        _temp_bgmcollectionlist.Clear();
        for (i = 0; i < GameMgr.bgm_collection_list.Count; i++)
        {
            _temp_bgmcollectionlist.Add(new ItemSaveFlag(GameMgr.bgm_collection_list[i].titleName, 0, GameMgr.bgm_collection_list[i].Flag));
        }

        //システムデータに、セーブしたかどうかのフラグをセット
        systemData = new PlayerData()
        {
            save_saveOK = GameMgr.saveOK,
            save_ending_count = GameMgr.ending_count,
            save_bestend_on_flag = GameMgr.bestend_on_flag,

            //ステージ１クリア時の好感度を保存
            save_stage1_clear_girl1_loveexp = GameMgr.stage1_clear_girl1_loveexp,
            save_stage2_clear_girl1_loveexp = GameMgr.stage2_clear_girl1_loveexp,
            save_stage3_clear_girl1_loveexp = GameMgr.stage3_clear_girl1_loveexp,

            save_stage1_clear_girl1_lovelv = GameMgr.stage1_clear_girl1_lovelv,
            save_stage2_clear_girl1_lovelv = GameMgr.stage2_clear_girl1_lovelv,
            save_stage3_clear_girl1_lovelv = GameMgr.stage3_clear_girl1_lovelv,

            //オートセーブフラグ
            save_Autosave_ON = GameMgr.AUTOSAVE_ON,

            //調合シーンでBGM切り替えるフラグ
            save_CompoBGMChange_ON = GameMgr.CompoBGMCHANGE_ON,

            //スキップ系フラグ
            save_SleepSkipFlag = GameMgr.SleepSkipFlag,
            save_PicnicSkipFlag = GameMgr.PicnicSkipFlag,
            save_OutGirlSkipFlag = GameMgr.OutGirlSkipFlag,

            save_player_money_system = PlayerStatus.player_money, // 所持金 システム引継ぎ用
            save_player_girl_maxlifepoint_system = PlayerStatus.player_girl_maxlifepoint, //妹のMAX体力 システム引継ぎ用

            //コスチューム番号
            save_costume_num = GameMgr.Costume_Num,
            save_acce_num = GameMgr.Accesory_Num,

            //アイテムリスト＜どんぐりと装備品関係＞
            save_dongurilist = _tempdongrilist,

            //エメラルドアイテム
            save_player_emeralditemlist = _temp_emeralditemlist,

            //調合のフラグ＋調合回数を記録する
            save_itemCompodatabase = _temp_cmpflaglist,

            //アイテムの前回得点
            save_itemdatabase = _temp_itemscorelist,

            //衣装などのサブイベントのフラグ
            save_GirlLoveSubEvent_stage1_system = GameMgr.GirlLoveSubEvent_stage1,

            //称号リストを記録する
            save_titlecollectionlist = _temp_titlecollectionlist,

            //イベントリストを記録する
            save_event_collection_list = _temp_eventcollectionlist,

            //コンテストクリアお菓子リストを記録する
            save_contestclear_collection_list = _temp_contestclearcollectionlist,
            save_contestclear_collection_listItemData = _temp_contestclearcollectionlistItemData,

            //獲得音楽図鑑のフラグ
            save_bgm_collection_list = _temp_bgmcollectionlist, //音楽リスト。 

            //音設定データ
            save_masterVolumeparam = GameMgr.MasterVolumeParam,
            save_BGMVolumeParam = GameMgr.BGMVolumeParam,
            save_SeVolumeParam = GameMgr.SeVolumeParam,

            //ストーリーモード
            save_Story_Mode = GameMgr.Story_Mode,

            //バージョン情報
            save_GameVersion = GameMgr.GameVersion,
            save_GameSaveDaytime = DateTime.Now.ToString("yyyy.MM.dd.HH.mm"),
        };

        //データの一時保存。bankに、playerDataを「player1」という名前で現在のデータを保存。
        bank.Store("System", systemData);
        //Debug.Log("bank.Store()");

        //一時データを永続的に保存。永続保存するときは、一度、一時データに保存しておく。
        bank.Save("System");
        Debug.Log("bank.Save(System)");

        Debug.Log("システムセーブ完了");
    }

    //システムデータのロード
    public void SystemloadCheck()
    {
        //セーブデータを管理するデータバンクのインスタンスを取得します(シングルトン)
        DataBank bank = DataBank.Open();

        Debug.Log("DataBank.Open()");
        Debug.Log($"save path of bank is { bank.SavePath }");

        //初期化(念のため)
        systemData = new PlayerData();

        //永続的に保存しておいたデータを、一時データに読み込む。bankに読み込まれる。
        bank.Load<PlayerData>("System");


        //一時データに再度読み込んだので、Getすると、再びパラメータを取得できる。
        systemData = bank.Get<PlayerData>("System");

        if (systemData != null)
        {
            //
            //*** 以下、読み込み・更新 ***
            //

            //バージョン情報 セーブデータのバージョン情報を読み込む。現行バージョンとあとで比較して、なんらかの処理を行う。
            GameMgr.Load_GameVersion = systemData.save_GameVersion;
            GameMgr.Load_GameSaveDaytime = systemData.save_GameSaveDaytime;

            //セーブやED関係
            GameMgr.saveOK = systemData.save_saveOK;
            GameMgr.ending_count = systemData.save_ending_count;
            GameMgr.bestend_on_flag = systemData.save_bestend_on_flag;

            //ステージ１クリア時の好感度を保存
            GameMgr.stage1_clear_girl1_loveexp = systemData.save_stage1_clear_girl1_loveexp;
            GameMgr.stage2_clear_girl1_loveexp = systemData.save_stage2_clear_girl1_loveexp;
            GameMgr.stage3_clear_girl1_loveexp = systemData.save_stage3_clear_girl1_loveexp;

            GameMgr.stage1_clear_girl1_lovelv = systemData.save_stage1_clear_girl1_lovelv;
            GameMgr.stage2_clear_girl1_lovelv = systemData.save_stage2_clear_girl1_lovelv;
            GameMgr.stage3_clear_girl1_lovelv = systemData.save_stage3_clear_girl1_lovelv;

            //オートセーブフラグ
            GameMgr.AUTOSAVE_ON = systemData.save_Autosave_ON;

            //スキップ系フラグ
            GameMgr.SleepSkipFlag = systemData.save_SleepSkipFlag;
            GameMgr.PicnicSkipFlag = systemData.save_PicnicSkipFlag;
            GameMgr.OutGirlSkipFlag = systemData.save_OutGirlSkipFlag;

            //調合シーンでBGM切り替えるフラグ
            GameMgr.CompoBGMCHANGE_ON = systemData.save_CompoBGMChange_ON;

            //PlayerStatus.player_money = systemData.save_player_money_system; // 所持金　システム引継ぎ用
            if (systemData.save_player_girl_maxlifepoint_system != 0) //ver途中から引継ぎするように仕様変更。なので例外処理をいれる。
            {
                PlayerStatus.player_girl_maxlifepoint = systemData.save_player_girl_maxlifepoint_system; // 女の子のMAX体力　システム引継ぎ用
            }
            else
            {
                PlayerStatus.player_girl_maxlifepoint = PlayerStatus.player_girl_maxlifepoint_default;
            }

            //コスチューム番号
            GameMgr.Costume_Num = systemData.save_costume_num;
            GameMgr.Accesory_Num = systemData.save_acce_num;
            
            //アイテムリスト＜どんぐりと装備品＞
            for (i = 0; i < systemData.save_dongurilist.Count; i++)
            {
                pitemlist.ReSetPlayerItemString(systemData.save_dongurilist[i].itemName, systemData.save_dongurilist[i].itemKosu);
            }

            //プレイヤーのエメラルドアイテムリスト。
            for (i = 0; i < systemData.save_player_emeralditemlist.Count; i++)
            {
                pitemlist.ReSetEmeraldItemString(systemData.save_player_emeralditemlist[i].itemName, systemData.save_player_emeralditemlist[i].itemKosu);
            }
            
            //調合のフラグ＋調合回数を記録する システムデータでは、ヒカリのお菓子制作フラグはONになったものだけ更新する。
            for (count = 0; count < systemData.save_itemCompodatabase.Count; count++)
            {
                i = 0;
                while (i < databaseCompo.compoitems.Count)
                {
                    if (systemData.save_itemCompodatabase[count].comp_name == databaseCompo.compoitems[i].cmpitem_Name)
                    {
                        databaseCompo.compoitems[i].cmpitem_flag = systemData.save_itemCompodatabase[count].comp_Flag;
                        databaseCompo.compoitems[i].comp_count = systemData.save_itemCompodatabase[count].comp_Count;
                        databaseCompo.compoitems[i].hikari_make_count = systemData.save_itemCompodatabase[count].hikarimake_Count;
                        //Debug.Log("databaseCompo.compoitems[i].cmpitem_flag: " + databaseCompo.compoitems[i].cmpitem_Name + " " + databaseCompo.compoitems[i].cmpitem_flag);
                        break;
                    }
                    i++;
                }
            }

            //アイテムの前回スコアなどを読み込み
            for (count = 0; count < systemData.save_itemdatabase.Count; count++)
            {
                i = 0;
                while (i < database.items.Count)
                {
                    if (systemData.save_itemdatabase[count].itemName == database.items[i].itemName)
                    {
                        database.items[i].Eat_kaisu = systemData.save_itemdatabase[count].Eat_kaisu;
                        database.items[i].HighScore_flag = systemData.save_itemdatabase[count].HighScore_flag;
                        //database.items_system[i].HighScore_flag = systemData.save_itemdatabase[count].HighScore_flag; //引継ぎせず、毎回リセットする仕様はこっち
                        database.items[i].last_total_score = systemData.save_itemdatabase[count].last_total_score;
                        database.items[i].last_rich_score = systemData.save_itemdatabase[count].last_rich_score;
                        database.items[i].last_sweat_score = systemData.save_itemdatabase[count].last_sweat_score;
                        database.items[i].last_bitter_score = systemData.save_itemdatabase[count].last_bitter_score;
                        database.items[i].last_sour_score = systemData.save_itemdatabase[count].last_sour_score;
                        database.items[i].last_crispy_score = systemData.save_itemdatabase[count].last_crispy_score;
                        database.items[i].last_fluffy_score = systemData.save_itemdatabase[count].last_fluffy_score;
                        database.items[i].last_smooth_score = systemData.save_itemdatabase[count].last_smooth_score;
                        database.items[i].last_hardness_score = systemData.save_itemdatabase[count].last_hardness_score;
                        database.items[i].last_jiggly_score = systemData.save_itemdatabase[count].last_jiggly_score;
                        database.items[i].last_chewy_score = systemData.save_itemdatabase[count].last_chewy_score;
                        database.items[i].last_hinttext = systemData.save_itemdatabase[count].last_hinttext;
                        break;
                    }
                    i++;
                }

            }

            //衣装などのサブイベントのフラグのみ引継ぎ
            for (i = 0; i < systemData.save_GirlLoveSubEvent_stage1_system.Length; i++)
            {
                if (i >= 70 && i < 80)
                {
                    //Debug.Log("i+ " + i);
                    GameMgr.GirlLoveSubEvent_stage1[i] = systemData.save_GirlLoveSubEvent_stage1_system[i];
                }
                if (i >= 100 && i < 110)
                {
                    //Debug.Log("i+ " + i);
                    GameMgr.GirlLoveSubEvent_stage1[i] = systemData.save_GirlLoveSubEvent_stage1_system[i];
                }
            }

            //称号リスト
            for (count = 0; count < systemData.save_titlecollectionlist.Count; count++)
            {
                i = 0;
                while (i < GameMgr.title_collection_list.Count)
                {
                    if (systemData.save_titlecollectionlist[count].itemName == GameMgr.title_collection_list[i].titleName)
                    {
                        GameMgr.title_collection_list[i].Flag = systemData.save_titlecollectionlist[count].Flag;
                        break;
                    }
                    i++;
                }
            }


            //イベントリスト
            for (count = 0; count < systemData.save_event_collection_list.Count; count++)
            {
                i = 0;
                while (i < GameMgr.event_collection_list.Count)
                {
                    if (systemData.save_event_collection_list[count].itemName == GameMgr.event_collection_list[i].titleName)
                    {
                        GameMgr.event_collection_list[i].Flag = systemData.save_event_collection_list[count].Flag;
                        break;
                    }
                    i++;
                }
            }

            //コンテストクリアお菓子リスト
            for (count = 0; count < systemData.save_contestclear_collection_list.Count; count++)
            {
                i = 0;
                while (i < GameMgr.contestclear_collection_list.Count)
                {
                    if (systemData.save_contestclear_collection_list[count].itemName == GameMgr.contestclear_collection_list[i].titleName)
                    {
                        GameMgr.contestclear_collection_list[i].Flag = systemData.save_contestclear_collection_list[count].Flag;
                        GameMgr.contestclear_collection_list[i].Score = systemData.save_contestclear_collection_list[count].Param;
                        break;
                    }
                    i++;
                }
            }

            for (count = 0; count < systemData.save_contestclear_collection_listItemData.Count; count++)
            {
                i = 0;
                while (i < GameMgr.contestclear_collection_list.Count)
                {
                    if (systemData.save_contestclear_collection_list[count].itemName == GameMgr.contestclear_collection_list[i].titleName)
                    {

                        GameMgr.contestclear_collection_list[i].ItemData = systemData.save_contestclear_collection_listItemData[count];

                        //テクスチャのデータは保存すると壊れてしまうので、ここで入れ直す。アイテムIDも、後でDB更新の際に全てずれる可能性があるので入れ直し。
                        if (GameMgr.contestclear_collection_list[i].ItemData.itemID != 9999)
                        {
                            _itemID = pitemlist.SearchItemString(GameMgr.contestclear_collection_list[i].ItemData.itemName);
                            GameMgr.contestclear_collection_list[i].ItemData.itemIcon_sprite = database.items[_itemID].itemIcon_sprite;
                        }
                        break;
                    }
                    i++;
                }
            }

            //音楽図鑑リスト
            for (count = 0; count < systemData.save_bgm_collection_list.Count; count++)
            {
                i = 0;
                while (i < GameMgr.bgm_collection_list.Count)
                {
                    if (systemData.save_bgm_collection_list[count].itemName == GameMgr.bgm_collection_list[i].titleName)
                    {
                        GameMgr.bgm_collection_list[i].Flag = systemData.save_bgm_collection_list[count].Flag;
                        break;
                    }
                    i++;
                }
            }


            //音設定データ
            GameMgr.MasterVolumeParam = systemData.save_masterVolumeparam;
            GameMgr.BGMVolumeParam = systemData.save_BGMVolumeParam;
            GameMgr.SeVolumeParam = systemData.save_SeVolumeParam;

            //ストーリーモード
            if(GameMgr.Load_GameVersion >= 1.20f) //バージョン1.2以降で追加したので、それ以前のセーブデータではstory_modeは0に。
            {
                GameMgr.Story_Mode = systemData.save_Story_Mode;
            }
            else //そもそも古いバージョンではセーブデータ自体がない。その場合、デタラメな数値が入る可能性があるので、その際の例外処理
            {
                GameMgr.Story_Mode = 0; //強制的に本編のモードに。
            }        

            
            Debug.Log("システムロード完了");
            Debug.Log("エンディング回数: " + GameMgr.ending_count);
            Debug.Log("ストーリーモード: " + GameMgr.Story_Mode);
            Debug.Log("現行のバージョン情報: " + GameMgr.GameVersion);
            Debug.Log("セーブデータのバージョン情報: " + GameMgr.Load_GameVersion);
            Debug.Log("セーブデータの日付: " + GameMgr.Load_GameSaveDaytime);
        }

        //音量データの再読み込み
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
        sc.VolumeSetting(); //効果音音量調整　BGMはupdateで常にチェックしてるのでいらない
    }

    //セーブデータがあるかどうかのみチェック
    public void SystemloadCheck_SaveOnly()
    {
        //セーブデータを管理するデータバンクのインスタンスを取得します(シングルトン)
        DataBank bank = DataBank.Open();

        Debug.Log("DataBank.Open()");
        Debug.Log($"save path of bank is { bank.SavePath }");

        //初期化(念のため)
        systemData = new PlayerData();

        //永続的に保存しておいたデータを、一時データに読み込む。bankに読み込まれる。
        bank.Load<PlayerData>("System");


        //一時データに再度読み込んだので、Getすると、再びパラメータを取得できる。
        systemData = bank.Get<PlayerData>("System");

        if (systemData != null)
        {
            //
            //*** 以下、読み込み・更新 ***
            //

            //
            GameMgr.saveOK = systemData.save_saveOK;         

            Debug.Log("システムロード　セーブデータあるかどうかのみ確認");

        }
    }
}
