﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveController : SingletonMonoBehaviour<SaveController>
{

    //保存するものリスト
    //GameMgrから、シナリオ・イベントのフラグ類
    //プレイヤーステータス　staticなので、インスタンスの取得の必要はなし。
    private PlayerItemList pitemlist; //プレイヤーアイテムのデータ
    private Girl1_status girl1_status; //女の子ステータス
    private ItemDataBase database; //前回の最高得点などを記録する
    private ItemCompoundDataBase databaseCompo; //調合DBのレシピフラグと前回の点数データ    
    private ItemMatPlaceDataBase matplace_database; //マップのオンフラグ
    private ExtremePanel extreme_panel; //エクストリームパネルに登録したものがあった場合は、そのアイテムも表示されるように。

    //保存するものリスト　ここまで

    private PlayerData playerData;
    private Compound_Keisan compound_keisan;
    private Debug_Panel debug_panel; //画面更新用のメソッドを借りる。
    private BGM sceneBGM;
    private Special_Quest special_quest;
    private MoneyStatus_Controller money_status;

    private GameObject canvas;

    private Text questname;
    private List<int> _tempplayeritemlist = new List<int>();
    private List<int> _tempmap_placeflaglist = new List<int>();

    private int i;

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
        for (i = 0; i < matplace_database.matplace_lists.Count; i++)
        {
            _tempmap_placeflaglist.Add(matplace_database.matplace_lists[i].placeFlag);
        }

        
        //セーブ保存用のクラスを新規作成。
        playerData = new PlayerData()
        {
            //プレイヤーステータス
            save_player_money = PlayerStatus.player_money, // 所持金
            save_player_kaeru_coin = PlayerStatus.player_kaeru_coin, //かえるコインの所持数。危ないお店などで使える。

            save_player_renkin_lv = PlayerStatus.player_renkin_lv, //錬金レベル
            save_player_renkin_exp = PlayerStatus.player_renkin_exp, //錬金経験

            save_player_ninki_param = PlayerStatus.player_ninki_param, //人気度。いるかな？とりあえず置き
            save_player_zairyobox = PlayerStatus.player_zairyobox, // 材料カゴの大きさ


            //妹のステータス
            save_player_girl_findpower = PlayerStatus.player_girl_findpower, //妹のアイテム発見力。高いと、マップの隠し場所を発見できたりする。
            save_girl_love_exp = girl1_status.girl1_Love_exp, //妹の好感度
            save_girl_love_lv = girl1_status.girl1_Love_lv, //妹の好感度レベル

            //日付・フラグ関係
            save_player_day = PlayerStatus.player_day, //現在の日付
            save_player_time = PlayerStatus.player_time, //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分

            save_First_recipi_on = PlayerStatus.First_recipi_on,
            save_First_extreme_on = PlayerStatus.First_extreme_on,

            //ステージ番号
            save_stage_number = GameMgr.stage_number,

            //シナリオの進み具合
            save_scenario_flag = GameMgr.scenario_flag,

            //初期アイテム取得フラグ
            save_gamestart_recipi_get = GameMgr.gamestart_recipi_get,

            //イベントフラグ
            save_GirlLoveEvent_num = GameMgr.GirlLoveEvent_num,
            save_GirlLoveEvent_stage1 = GameMgr.GirlLoveEvent_stage1,  //各イベントの、現在読み中かどうかのフラグ。           
            save_GirlLoveEvent_stage2 = GameMgr.GirlLoveEvent_stage2,
            save_GirlLoveEvent_stage3 = GameMgr.GirlLoveEvent_stage3,

            //お菓子クエストフラグ
            save_OkashiQuest_flag_stage1 = GameMgr.OkashiQuest_flag_stage1, //各SPイベントのクリアしたかどうかのフラグ。
            save_OkashiQuest_flag_stage2 = GameMgr.OkashiQuest_flag_stage2,
            save_OkashiQuest_flag_stage3 = GameMgr.OkashiQuest_flag_stage3,

            //マップイベントフラグ
            save_MapEvent_01 = GameMgr.MapEvent_01,         //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
            save_MapEvent_02 = GameMgr.MapEvent_02,         //井戸。
            save_MapEvent_03 = GameMgr.MapEvent_03,         //ストロベリーガーデン
            save_MapEvent_04 = GameMgr.MapEvent_04,         //ひまわりの丘
            save_MapEvent_05 = GameMgr.MapEvent_05,

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

            //コンテストのイベントリスト
            save_ContestEvent_stage = GameMgr.ContestEvent_stage,

            //コンテスト審査員の点数
            save_contest_Score = GameMgr.contest_Score,
            save_contest_TotalScore = GameMgr.contest_TotalScore,

            //牧場のイベントリスト
            save_FarmEvent_stage = GameMgr.FarmEvent_stage,

            //アイテムリスト＜デフォルト＞
            save_playeritemlist = _tempplayeritemlist,

            //プレイヤーのイベントアイテムリスト。
            save_eventitemlist = pitemlist.eventitemlist,

            //アイテムリスト＜オリジナル＞
            save_player_originalitemlist = pitemlist.player_originalitemlist,

            //アイテムの前回スコアなどを記録する
            save_itemdatabase = database.items,

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

        //プレイヤーステータス
        PlayerStatus.player_money = playerData.save_player_money; // 所持金
        PlayerStatus.player_kaeru_coin = playerData.save_player_kaeru_coin; //かえるコインの所持数。危ないお店などで使える。

        PlayerStatus.player_renkin_lv = playerData.save_player_renkin_lv; //錬金レベル
        PlayerStatus.player_renkin_exp = playerData.save_player_renkin_exp; //錬金経験

        PlayerStatus.player_ninki_param = playerData.save_player_ninki_param; //人気度。いるかな？とりあえず置き
        PlayerStatus.player_zairyobox = playerData.save_player_zairyobox; // 材料カゴの大きさ

        //妹のステータス
        PlayerStatus.player_girl_findpower = playerData.save_player_girl_findpower; //妹のアイテム発見力。高いと、マップの隠し場所を発見できたりする。
        girl1_status.girl1_Love_exp = playerData.save_girl_love_exp; //妹の好感度
        girl1_status.girl1_Love_lv = playerData.save_girl_love_lv; //妹の好感度レベル

        //日付・フラグ関係
        PlayerStatus.player_day = playerData.save_player_day; //現在の日付
        PlayerStatus.player_time = playerData.save_player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分

        PlayerStatus.First_recipi_on = playerData.save_First_recipi_on;
        PlayerStatus.First_extreme_on = playerData.save_First_extreme_on;

        //ステージ番号
        GameMgr.stage_number = playerData.save_stage_number;

        //シナリオの進み具合
        GameMgr.scenario_flag = playerData.save_scenario_flag;

        //初期アイテム取得フラグ
        GameMgr.gamestart_recipi_get = playerData.save_gamestart_recipi_get;

        //イベントフラグ
        GameMgr.GirlLoveEvent_num = playerData.save_GirlLoveEvent_num;
        GameMgr.GirlLoveEvent_stage1 = playerData.save_GirlLoveEvent_stage1;  //各イベントの、現在読み中かどうかのフラグ。
        GameMgr.GirlLoveEvent_stage2 = playerData.save_GirlLoveEvent_stage2;
        GameMgr.GirlLoveEvent_stage3 = playerData.save_GirlLoveEvent_stage3;

        //お菓子クエストフラグ
        GameMgr.OkashiQuest_flag_stage1 = playerData.save_OkashiQuest_flag_stage1; //各SPイベントのクリアしたかどうかのフラグ。
        GameMgr.OkashiQuest_flag_stage2 = playerData.save_OkashiQuest_flag_stage2;
        GameMgr.OkashiQuest_flag_stage3 = playerData.save_OkashiQuest_flag_stage3;

        //マップイベントフラグ
        GameMgr.MapEvent_01 = playerData.save_MapEvent_01;        //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
        GameMgr.MapEvent_02 = playerData.save_MapEvent_02;        //井戸。
        GameMgr.MapEvent_03 = playerData.save_MapEvent_03;        //ストロベリーガーデン
        GameMgr.MapEvent_04 = playerData.save_MapEvent_04;        //ひまわりの丘
        GameMgr.MapEvent_05 = playerData.save_MapEvent_05;

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

        //コンテストのイベントリスト
        GameMgr.ContestEvent_stage = playerData.save_ContestEvent_stage;

        //コンテスト審査員の点数
        GameMgr.contest_Score = playerData.save_contest_Score;
        GameMgr.contest_TotalScore = playerData.save_contest_TotalScore;

        //牧場のイベントリスト
        GameMgr.FarmEvent_stage = playerData.save_FarmEvent_stage;
        
        //アイテムリスト＜デフォルト＞
        for (i = 0; i < pitemlist.playeritemlist.Count; i++)
        {
            pitemlist.playeritemlist[i] = playerData.save_playeritemlist[i];
        }
        
        //プレイヤーのイベントアイテムリスト。
        pitemlist.eventitemlist.Clear();
        pitemlist.eventitemlist = playerData.save_eventitemlist;

        //アイテムリスト＜オリジナル＞
        pitemlist.player_originalitemlist.Clear();
        pitemlist.player_originalitemlist = playerData.save_player_originalitemlist;

        //アイテムの前回スコアなどを記録する
        database.items.Clear();
        database.items = playerData.save_itemdatabase;

        //調合のフラグ＋調合回数を記録する
        databaseCompo.compoitems.Clear();
        databaseCompo.compoitems = playerData.save_itemCompodatabase;
        
        //マップフラグの読み込み
        for (i = 0; i < matplace_database.matplace_lists.Count; i++)
        {
            matplace_database.matplace_lists[i].placeFlag = playerData.save_mapflaglist[i];
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

                money_status = canvas.transform.Find("MoneyStatus_panel").GetComponent<MoneyStatus_Controller>();

                //メイン画面に表示する、現在のクエスト
                questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();

                //BGMの取得
                sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
                break;
        }

        girl1_status.OkashiNew_Status = 1;
        girl1_status.special_animatFirst = true;
        special_quest.SetSpecialOkashi(GameMgr.GirlLoveEvent_num, 1);
        debug_panel.GirlLove_Koushin(girl1_status.girl1_Love_exp); //好感度ステータスに応じたキャラの表情やLive2Dモーション更新
        debug_panel.Debug_ReDraw();      

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                money_status.money_Draw();
                questname.text = girl1_status.OkashiQuest_Name;
                sceneBGM.PlayMain(); //BGMの更新

                if (GameMgr.sys_extreme_itemID != 9999)
                {
                    extreme_panel = canvas.transform.Find("ExtremePanel").GetComponentInChildren<ExtremePanel>();
                    extreme_panel.SetExtremeItem(GameMgr.sys_extreme_itemID, GameMgr.sys_extreme_itemType);

                }
                break;
        }
    }

    public void ResetAllParam()
    {
        PlayerStatus.Setup_PlayerStatus(); //プレイヤーステータスの初期化
        girl1_status.ResetDefaultStatus(); //女の子ステータスの初期化
        GameMgr.ResetGameDefaultStatus(); //ゲームステータスの初期化

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
    }
}
