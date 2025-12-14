using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventDataBase : SingletonMonoBehaviour<EventDataBase>
{
    private GameObject canvas;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private ItemDataBase database;
    private CatDataBase catDataBase;
    private QuestSetDataBase quest_database;

    private Girl1_status girl1_status;
    private Special_Quest special_quest;
    private ItemMatPlaceDataBase matplace_database;
    private PlayerItemList pitemlist;
    private TimeController time_controller;
    private Exp_Controller exp_Controller;
    private MoneyStatus_Controller moneyStatus_Controller;
    private ContestStartListDataBase conteststartList_database;

    private GetMatPlace_Panel getmatplace_panel;
    private GetMaterial get_material;

    private int event_num;
    private bool GetEmeraldItem;

    private int i, count, random, rnd;
    private int picnic_exprob;
    private int _id, ev_id, read_ID, _qid;
    private int contest_allcount, contest_victorycount;
    private bool contest_Master_TasseiFlag;
    private bool contest_Master_TasseiFlag_half;
    private int archive_area;
    private float archivement_percent;
    private bool _fire;
    private string _basename, _baseitemtype_sub, _baseitemtype_subB;
    private bool cat_comecheck;
    private int cat_maxcount;
    private int cat_come_day;

    private int _Limit_day;
    private int _Nokori_day;
    private bool KoyuNPCQuest_OkashiTeishutuON;
    private int _setjudge_num;

    private List<int> map_list = new List<int>();

    // Use this for initialization
    void Start () {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        //ねこデータベースの取得
        catDataBase = CatDataBase.Instance.GetComponent<CatDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //お金オブジェクト
        moneyStatus_Controller = MoneyStatus_Controller.Instance.GetComponent<MoneyStatus_Controller>();

        GetEmeraldItem = false;
        _fire = false;
        cat_comecheck = false;
        contest_Master_TasseiFlag = false;
        contest_Master_TasseiFlag_half = false;
    }
	
	// Update is called once per frame
	void Update () {

    }

    //好感度によって発生する、メインイベント。基本、クエストクリアボタンを押さないと発動しない。
    public void GirlLoveMainEvent()
    {
        if (GameMgr.GirlLove_loading)
        { }
        else
        {
            GameMgr.girlloveevent_bunki = 0; //サブイベントが発生しない限り、メインの好感度イベントを発生するようにする。

            GameMgr.check_GirlLoveEvent_flag = true;
            GameMgr.check_GirlLoveSubEvent_flag = false; //好感度イベントのチェック後に、サブイベントの発生チェック

            switch (GameMgr.stage_number)
            {
                //ステージ１のメインイベント
                case 1:

                    if (!GameMgr.OkashiQuest_flag_stage1[0]) //レベル１のときのイベント。一番最初で起こるイベント。
                    {
                        event_num = 0;

                        if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント０
                        {
                            GameMgr.GirlLoveEvent_stage1[event_num] = true; //0番がtrueになってたら、現在は、ステージ１－１のクエストが発生中という意味。

                            //クッキー作りのクエスト発生
                            Debug.Log("ハートメインイベント１をON: 開始");

                            GameMgr.check_GirlLoveEvent_flag = true; //GirlLoveEventは発生しない。

                            //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                            if (GameMgr.Story_Mode == 0)
                            {
                                special_quest.SetSpecialOkashi(0, 0);
                            }
                            else
                            {
                                special_quest.SetSpecialOkashi(0, 2); //エクストラモード
                            }
                        }
                    }


                    if (!GameMgr.check_GirlLoveEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                    { }
                    else
                    {
                        if (GameMgr.OkashiQuest_flag_stage1[0] && GameMgr.questclear_After) //レベル２のときのイベント
                        {                           
                            event_num = 10;

                            if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント１
                            {
                                GameMgr.questclear_After = false;
                                GameMgr.GirlLoveEvent_stage1[event_num] = true; //1番がtrueになってたら、現在は、ステージ１－２のクエストが発生中という意味。

                                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                                if (GameMgr.Story_Mode == 0)
                                {
                                    GameMgr.check_GirlLoveEvent_flag = false;

                                    //レシピの追加
                                    //pitemlist.add_eventPlayerItemString("rusk_recipi", 1);//ラスクのレシピを追加                            

                                    //クエスト発生
                                    Debug.Log("ハートメインイベント２をON: 開始");


                                    special_quest.SetSpecialOkashi(10, 0);
                                }
                                else
                                {
                                    GameMgr.check_GirlLoveEvent_flag = true; //GirlLoveEventは発生しない。
                                    special_quest.SetSpecialOkashi(10, 2); //エクストラモード
                                }
                            }
                        }
                    }

                    if (!GameMgr.check_GirlLoveEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                    { }
                    else
                    {
                        if (GameMgr.OkashiQuest_flag_stage1[1] && GameMgr.questclear_After) //レベル３のときのイベント。
                        {
                            
                            event_num = 20;

                            if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント２
                            {
                                GameMgr.questclear_After = false;
                                GameMgr.GirlLoveEvent_stage1[event_num] = true;

                                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                                if (GameMgr.Story_Mode == 0)
                                {                                   
                                    GameMgr.check_GirlLoveEvent_flag = false;

                                    //レシピの追加
                                    //pitemlist.add_eventPlayerItemString("crepe_recipi", 1); //クレープのレシピを追加   
                                    
                                    GameMgr.picnic_count = 3; //ピクニックこのイベント以降、カウント開始する。

                                    //クエスト発生
                                    Debug.Log("ハートメインイベント３をON: 開始");

                                    special_quest.SetSpecialOkashi(20, 0);
                                }
                                else
                                {
                                    GameMgr.check_GirlLoveEvent_flag = true; //GirlLoveEventは発生しない。
                                    special_quest.SetSpecialOkashi(20, 2);
                                }
                            }
                        }
                    }

                    if (!GameMgr.check_GirlLoveEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                    { }
                    else
                    {
                        if (GameMgr.OkashiQuest_flag_stage1[2] && GameMgr.questclear_After) //レベル４のときのイベント。
                        {
                            
                            event_num = 30;

                            if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント３
                            {
                                GameMgr.questclear_After = false;
                                GameMgr.GirlLoveEvent_stage1[event_num] = true;

                                if (GameMgr.Story_Mode == 0)
                                {
                                    GameMgr.check_GirlLoveEvent_flag = false;

                                    //クエスト発生
                                    Debug.Log("ハートメインイベント４をON: 開始");


                                    special_quest.SetSpecialOkashi(30, 0);
                                }
                                else
                                {
                                    GameMgr.check_GirlLoveEvent_flag = true; //GirlLoveEventは発生しない。
                                    special_quest.SetSpecialOkashi(30, 2);
                                }
                            }
                        }
                    }

                    if (!GameMgr.check_GirlLoveEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                    { }
                    else
                    {
                        if (GameMgr.OkashiQuest_flag_stage1[3] && GameMgr.questclear_After) //レベル５のときのイベント。
                        {
                            
                            event_num = 40;

                            if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント４
                            {
                                GameMgr.questclear_After = false;
                                GameMgr.GirlLoveEvent_stage1[event_num] = true;

                                if (GameMgr.Story_Mode == 0)
                                {
                                    GameMgr.check_GirlLoveEvent_flag = false;

                                    //クエスト発生
                                    Debug.Log("ハートメインイベント５をON: 開始");


                                    special_quest.SetSpecialOkashi(40, 0);
                                }
                                else
                                {
                                    GameMgr.check_GirlLoveEvent_flag = true; //GirlLoveEventは発生しない。
                                    special_quest.SetSpecialOkashi(40, 2);
                                }
                            }
                        }
                    }

                    if (!GameMgr.check_GirlLoveEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                    { }
                    else
                    {
                        if (GameMgr.OkashiQuest_flag_stage1[4] && GameMgr.questclear_After) //ステージ１　５つクリアしたので、コンテストイベント
                        {
                            
                            event_num = 50;

                            if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　ラストイベント
                            {
                                GameMgr.questclear_After = false;
                                GameMgr.GirlLoveEvent_stage1[event_num] = true;

                                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                                if (GameMgr.Story_Mode == 0)
                                {
                                    GameMgr.check_GirlLoveEvent_flag = false;

                                    //コンテストの締め切り日を設定
                                    GameMgr.stage1_limit_day = PlayerStatus.player_day + 7;

                                    //クエスト発生
                                    Debug.Log("ハートラストイベントをON: 開始");

                                    //イベントCG解禁
                                    GameMgr.SetEventCollectionFlag("event10", true);

                                    special_quest.SetSpecialOkashi(50, 0);
                                }
                                else
                                {
                                    GameMgr.check_GirlLoveEvent_flag = false; //GirlLoveEventは発生する。
                                    special_quest.SetSpecialOkashi(50, 2);
                                }
                               

                                //広場は必ずでる。
                                matplace_database.matPlaceKaikin("Hiroba"); //広場解禁
                                                                            //matplace_database.matPlaceKaikin("HimawariHill"); //ひまわり解禁
                                
                            }
                        }
                    }

                    break;

                //ステージ２のイベント
                case 2:

                    break;

                //ステージ３のイベント
                case 3:

                    break;

                default:
                    break;

            }

            //最後のタイミングで、決定したサブイベントの宴を再生
            if (!GameMgr.check_GirlLoveEvent_flag) //サブイベント発生した
            {
                girl1_status.HukidashiFlag = false;
                GameMgr.ResultComplete_flag = 0; //イベント読み始めたら、調合終了の合図をたてておく。

                //クエスト発生
                Debug.Log("メイン好感度イベントの発生");

                //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                ReadGirlLoveEvent();
            }
            else //全てのイベントチェックし、発生しなかったら、このスクリプトでのイベントチェック完了
            { }

        }
    }

    //家に帰ってきたときに発生するイベント
    public void ReturnHomeCompoundEvent()
    {
        if (GameMgr.GirlLove_loading)
        { }
        else
        {
            GameMgr.check_ReturnHomeEvent_flag = true;

            ReturnHome_check(0, false); //プリンさん再会して、お店から帰ってきた
            //ReturnHome_check(10, false); //酒場はじめていって帰ってきた
            ReturnHome_check(20, false); //牧場はじめていって帰ってきた
            ReturnHome_check(30, true); //コンテストはじめていって帰ってきた      
            ReturnHome_check(40, true); //コンテストはじめていって帰ってきた 
            ReturnHome_check(110, false); //ミラボ先生にはじめて会って帰ってきた
            ReturnHome_check(120, false); //ぬねちゃんにはじめて会って帰ってきた
            ReturnHome_check(130, false); //遊園地で遊んで帰ってきた 何度でも発生する

            if (!GameMgr.CompoundEvent_num[30]) //コンテストについて知ったので、アマクサ帰りのコンテストどこ～？イベントは発生しなくなる。
            {
                ReturnHome_check(100, true); //アマクサにエデンのありか聞いて帰ってきた
            }

            //最後のタイミングで、決定したサブイベントの宴を再生
            if (!GameMgr.check_ReturnHomeEvent_flag) //サブイベント発生した
            {
                girl1_status.HukidashiFlag = false;

                //クエスト発生
                Debug.Log("家に帰ってきたときに発生するイベントの発生");

                GameMgr.GirlLoveEvent_bunki_status = 1; //家に帰ってきたときに発生するイベントとして、分岐する　リセットは、イベント終了後と、Compシーン最初

                //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                ReadGirlLoveEvent();
            }
            else //全てのイベントチェックし、発生しなかったら、このスクリプトでのイベントチェック完了
            {
                GameMgr.CompoundEvent_flag = false; //家にかえったらイベント発生確認するフラグ　読み終えたのでfalseに。
            }

        }
    }

    void ReturnHome_check(int _num, bool utagebgm_ON) //二個目は宴のBGMをonにする。3個目は、何度でも発生するイベント
    {
        if (!GameMgr.check_ReturnHomeEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.CompoundEvent_num[_num] && !GameMgr.CompoundEvent_readend[_num]) //発生かつ、まだ読みおわってないイベント
            {
                GameMgr.CompoundEvent_storynum = _num;
                GameMgr.CompoundEvent_readend[_num] = true;

                GameMgr.check_ReturnHomeEvent_flag = false;

                if (utagebgm_ON)
                {
                    GameMgr.Mute_on = true;
                }
                else
                {
                    GameMgr.Mute_on = false;
                }
            }

        }
    }

    

    //SPお菓子とは別で、パティシエレベルor好感度が一定に達すると発生するサブイベント Compound_Mainから読み出す。
    public void GirlLove_SubEventMethod()
    {
        GameMgr.girlloveevent_bunki = 1; //サブイベントの発生のチェック。宴用に分岐。

        
        if (GameMgr.GirlLove_loading)
        { }
        else
        {
            GameMgr.check_GirlLoveSubEvent_flag = true;

            if (GameMgr.Story_Mode == 0)
            {
                //クエストで発生するサブイベント
                switch (GameMgr.GirlLoveEvent_num)
                {
                    case 0: //クッキー 2でもこれは使う

                        //はじめてのお菓子。食べた直後に発生する。
                        if (GameMgr.GirlLoveSubEvent_stage1[0] == false)
                        {
                            if (GameMgr.check_OkashiAfter_flag) //お菓子をあげたあとのフラグ
                            {

                                GameMgr.GirlLoveSubEvent_stage1[0] = true;

                                if (database.items[GameMgr.Okashi_lastID].itemType_sub.ToString() != "Cookie") //そもそもクッキー以外のものをあげたとき
                                {
                                    if (GameMgr.Okashi_totalscore < GameMgr.low_score) //クリアできないときのヒントをだす。＋クッキーを食べたいなぁ～。
                                    {
                                        GameMgr.GirlLoveSubEvent_stage1[3] = true;
                                        GameMgr.GirlLoveSubEvent_num = 3;
                                        GameMgr.Okashi_OnepointHint_num = 0;

                                        GameMgr.check_GirlLoveSubEvent_flag = false;
                                    }
                                    else //クリアできたら、そのままOK!　＋　でもクッキーが食べたいから、にいちゃん、クッキーを作って！！
                                    {
                                        GameMgr.GirlLoveSubEvent_stage1[4] = true;
                                        GameMgr.GirlLoveSubEvent_num = 4;

                                        GameMgr.check_GirlLoveSubEvent_flag = false;
                                    }
                                }
                                else
                                {
                                    if (GameMgr.Okashi_totalscore < GameMgr.low_score) //クリアできなかった場合、ヒントをだす。
                                    {
                                        GameMgr.GirlLoveSubEvent_num = 0;
                                        GameMgr.Okashi_OnepointHint_num = 0;

                                        GameMgr.check_GirlLoveSubEvent_flag = false;
                                    }
                                    else if (GameMgr.Okashi_totalscore < GameMgr.high_score)//クリアできた。60~85。現在未使用。
                                    {
                                        GameMgr.GirlLoveSubEvent_stage1[1] = true;
                                        GameMgr.GirlLoveSubEvent_num = 1;

                                        GameMgr.check_GirlLoveSubEvent_flag = true; //trueにすると、そのイベントを無視できる。
                                    }
                                    else //クリアできた。85~
                                    {
                                        GameMgr.GirlLoveSubEvent_stage1[2] = true;
                                        GameMgr.GirlLoveSubEvent_num = 2;

                                        GameMgr.check_GirlLoveSubEvent_flag = true;
                                    }
                                }
                            }
                        }

                        //一度お菓子を作って失敗し、次に作って成功した。または、クッキー以外のお菓子を作り、その後、クッキーを作って成功した。
                        if (GameMgr.GirlLoveSubEvent_stage1[0] == true && GameMgr.GirlLoveSubEvent_stage1[1] == false && GameMgr.GirlLoveSubEvent_stage1[2] == false)
                        {
                            if (GameMgr.check_OkashiAfter_flag)
                            {
                                if (!GameMgr.GirlLoveSubEvent_stage1[5] || !GameMgr.GirlLoveSubEvent_stage1[6])
                                {
                                    if (GameMgr.Okashi_dislike_status == 2) //そもそもクッキー以外のものをあげたとき
                                    {

                                    }
                                    else
                                    {
                                        if (GameMgr.Okashi_totalscore < GameMgr.low_score) //クリアできなかった場合。フラグはたたず、やり直し
                                        {

                                        }
                                        else if (GameMgr.Okashi_totalscore < GameMgr.high_score)//クリアできた。60~85
                                        {
                                            GameMgr.GirlLoveSubEvent_stage1[5] = true;
                                            GameMgr.GirlLoveSubEvent_num = 5;
                                            GameMgr.Okashi_OnepointHint_num = 9999;

                                            GameMgr.check_GirlLoveSubEvent_flag = false;
                                        }
                                        else //クリアできた。85~
                                        {
                                            GameMgr.GirlLoveSubEvent_stage1[6] = true;
                                            GameMgr.GirlLoveSubEvent_num = 6;
                                            GameMgr.Okashi_OnepointHint_num = 9999;

                                            GameMgr.check_GirlLoveSubEvent_flag = false;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case 1: //さくらクッキー

                        if (GameMgr.GirlLoveSubEvent_stage1[7] == false) //はじめてぶどうをとってきた
                        {
                            if (GameMgr.check_GetMat_flag)
                            {
                                if (pitemlist.KosuCount("sakura_chip") >= 1)
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[7] = true;
                                    GameMgr.GirlLoveSubEvent_num = 7;

                                    GameMgr.check_GirlLoveSubEvent_flag = false;
                                }
                            }
                        }
                        break;

                    case 2: //かわいいクッキー

                        if (GameMgr.GirlLoveSubEvent_stage1[8] == false && girl1_status.special_animatFirst == true)
                        {
                            GameMgr.GirlLoveSubEvent_stage1[8] = true;
                            GameMgr.GirlLoveSubEvent_num = 8;
                            GameMgr.check_GirlLoveSubEvent_flag = false;

                            GameMgr.Mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                        }
                        break;

                        /*
                    case 11: //ラスク2

                        if (girl1_status.special_animatFirst) //ステージ2-2 はじまってから、ベリーファーム開始
                        {
                            if (!GameMgr.GirlLoveSubEvent_stage1[10])
                            {
                                GameMgr.GirlLoveSubEvent_stage1[10] = true;
                                GameMgr.GirlLoveSubEvent_num = 10;

                                GameMgr.check_GirlLoveSubEvent_flag = false;
                                GameMgr.Mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。

                                //ベリーファームへ行けるようになる。
                                matplace_database.matPlaceKaikin("BerryFarm"); //ベリーファーム解禁

                            }
                        }

                        break;

                    case 13: //キラキララスク 10から分岐１

                        if (girl1_status.special_animatFirst) //ステージ2-2 はじまってから、ベリーファーム開始
                        {
                            if (!GameMgr.GirlLoveSubEvent_stage1[10])
                            {
                                GameMgr.GirlLoveSubEvent_stage1[10] = true;
                                GameMgr.GirlLoveSubEvent_num = 10;

                                GameMgr.check_GirlLoveSubEvent_flag = false;
                                GameMgr.Mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。

                                //ベリーファームへ行けるようになる。
                                matplace_database.matPlaceKaikin("BerryFarm"); //ベリーファーム解禁

                            }
                        }

                        break;

                    case 20: //クレープ1

                        if (GameMgr.check_CompoAfter_flag) //お菓子を作ったあとのフラグ. Exp_Controllerから読み出し。
                        {
                            if (GameMgr.GirlLoveSubEvent_stage1[20] == false && database.items[GameMgr.Okashi_makeID].itemType_sub.ToString() == "Crepe")
                            {
                                GameMgr.GirlLoveSubEvent_stage1[20] = true;
                                GameMgr.GirlLoveSubEvent_num = 20;
                                GameMgr.check_GirlLoveSubEvent_flag = false;

                                GameMgr.Mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                            }
                        }

                        if (GameMgr.GirlLoveSubEvent_stage1[21] == false)
                        {
                            GameMgr.GirlLoveSubEvent_stage1[21] = true;
                            GameMgr.GirlLoveSubEvent_num = 21;
                            GameMgr.check_GirlLoveSubEvent_flag = false;

                            GameMgr.Mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                        }
                        break;

                    case 40: //ドーナツ　ひまわりのたね

                        if (GameMgr.GirlLoveSubEvent_stage1[40] == false) //ひまわりのたねをとってきた
                        {
                            if (GameMgr.check_GetMat_flag)
                            {
                                if (pitemlist.KosuCount("himawari_seed") >= 1)
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[40] = true;
                                    GameMgr.GirlLoveSubEvent_num = 40;

                                    GameMgr.check_GirlLoveSubEvent_flag = false;
                                }
                            }
                        }

                        //ひまわり油
                        if (GameMgr.check_CompoAfter_flag) //お菓子を作ったあとのフラグ. Exp_Controllerから読み出し。
                        {
                            if (GameMgr.GirlLoveSubEvent_stage1[41] == false && database.items[GameMgr.Okashi_makeID].itemName == "himawari_Oil")
                            {
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[41] = true;
                                    GameMgr.GirlLoveSubEvent_num = 41;
                                    GameMgr.check_GirlLoveSubEvent_flag = false;

                                    GameMgr.Mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                                }
                            }
                        }

                        if (GameMgr.check_CompoAfter_flag) //ドーナツをはじめて作った。
                        {
                            if (GameMgr.GirlLoveSubEvent_stage1[42] == false && database.items[GameMgr.Okashi_makeID].itemType_sub.ToString() == "Donuts")
                            {
                                GameMgr.GirlLoveSubEvent_stage1[42] = true;
                                GameMgr.GirlLoveSubEvent_num = 42;
                                GameMgr.check_GirlLoveSubEvent_flag = false;

                                GameMgr.Mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                            }
                        }
                        break;
                        */
                }
            }


            //その他イベント、ロード後イベントなど。90番台～　優先度高いのでここでチェック。
            if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                if (GameMgr.Load_eventflag)
                {
                    if (GameMgr.GirlLoveSubEvent_stage1[90] == false)
                    {
                        GameMgr.Load_eventflag = false;

                        if (!GameMgr.outgirl_Nowprogress)
                        {
                            if (GameMgr.GirlLoveEvent_num == 50) //コンテストのとき
                            {

                                GameMgr.GirlLoveSubEvent_num = 91;
                                GameMgr.check_GirlLoveSubEvent_flag = false;
                            }
                            else if (GameMgr.GirlLoveEvent_num >= 40 && GameMgr.GirlLoveEvent_num < 50) //ステージ４　コンテストが近い
                            {

                                GameMgr.GirlLoveSubEvent_num = 92;
                                GameMgr.check_GirlLoveSubEvent_flag = false;
                            }
                            else
                            {
                                GameMgr.GirlLoveSubEvent_num = 90;
                                GameMgr.check_GirlLoveSubEvent_flag = false;
                            }
                        }
                        else
                        {
                            GameMgr.GirlLoveSubEvent_num = 93;
                            GameMgr.check_GirlLoveSubEvent_flag = false;
                        }
                    }
                }
            }


            //
            //サブイベント・ハート・スターで進むなどのイベント関係
            //

            if(GameMgr.outgirl_Nowprogress)
            { }
            else
            {

                //
                //ハートレベル系のイベント
                //
                //HeartEvent_check(GameMgr.System_HeartBlockLv_01, 300, 1); //秘密の花園へいこうよ
                HeartEvent_check(GameMgr.System_HeartLVevent_01, 301, 1, "Non"); //ヒカリお菓子作る

                if (pitemlist.ReturnEventItemKosu("eden_recipi_03") < 1) //持ってない場合に発生　持ってるときは、コンテストでイセヤを倒しゲットしている
                {
                    HeartEvent_check(GameMgr.System_HeartBlockLv_10, 380, 1, "Non"); //エデンレシピの場所解放　星
                }
                if (pitemlist.ReturnEventItemKosu("eden_recipi_04") < 1) //持ってない場合に発生　持ってるときは、コンテストでベルを倒しゲットしている
                {
                    HeartEvent_check(GameMgr.System_HeartBlockLv_11, 381, 1, "Non"); //エデンレシピの場所解放　月
                }

                //HLVごとに発生するイベント 350番台～
                //"Non"だと、思い出イベントのフラグ解放はなし　入れる場合は、GameMgrのHikariOmoide_Eventlistに登録する

                //HeartEvent_check(9, 352, 1); ヒカリお菓子作るとLV被るので、off
                //HeartEvent_check(15, 350, 1);
                //HeartEvent_check(20, 302, 1, "Non"); //ヒカリ二個トッピング仕上げできるようになる

                HeartEvent_check(40, 355, 1, "dragon_carnival"); //ドラゴンカーニバル
                HeartEvent_check(50, 356, 1, "ramen"); //らーめん              
                //HeartEvent_check(70, 357, 1);
                //HeartEvent_check(80, 358, 1, "Non");
                //HeartEvent_check(90, 359, 1, "Non");

                //Heartevent_Grt(); //１の頃のイベント


                //
                //スターで発生するイベント系
                //
                //StarEvent_check(GameMgr.System_StarBlockLv_04, 500, 1); //スター10で、お城へいけるように。手紙がくる。
                StarEvent_check(5, 503, 1); //スター5で、プラトンアカデミーコンテスト解放 ここでスターの数値決めてOK
                StarEvent_check(12, 501, 1); //スター12で、サマードリームフェスティバル解放 ここでスターの数値決めてOK
                //StarEvent_check(22, 502, 1); //スター25で、アルクアンシェル解放　はなしに。夏コンクリアで次の秋コンがでるように変更。

                //
                //スターパネル解放で発生するリリースイベント系
                //
                //StarRank_ReleaseListの配列番号をみる　例)1 = starが7のときに解放されるイベントのこと GameMgr.Star_Eventlistを参照
                //2番目はsubEventのnum 3番目はBGM　1のときは宴のBGMを鳴らす
                //お宝イベントは、ここのイベント発生でなくスターパネル内で完結させる
                StarReleaseEvent_check(1, 600, 0, "Non"); //7なのでショートケーキのレシピゲット
                StarReleaseEvent_check(2, 601, 0, "Non"); //9なのでコスチュームゲット
                StarReleaseEvent_check(3, 302, 1, "Non"); //15なので、トッピング二個同時解放
                //StarReleaseEvent_check(4, 355, 1, "dragon_carnival"); //18なので、休憩イベント
                StarReleaseEvent_check(5, 603, 1, "Non"); //20なのでおふろいけるイベント　温泉地の解放？
                StarReleaseEvent_check(6, 604, 0, "Non"); //22なのでコスチューム2ゲット
                //StarReleaseEvent_check(8, 605, 1, "Non"); //30なのでマリトッツォのレシピゲット
                StarReleaseEvent_check(9, 606, 1, "Non"); //32なのでスウィートホテルいけるイベント
                StarReleaseEvent_check(10, 610, 1, "event_sakuraring"); //43なのでラストイベント　ヒカリからさくらの指輪をもらう

                //
                //ビギナー系のサブイベント関係は、80番台～
                //

                //はじめてお菓子を作ったら発生
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (PlayerStatus.First_recipi_on)
                    {
                        if (GameMgr.GirlLoveSubEvent_stage1[80] == false)
                        {
                            Event_startcheck(80, 0, false, false, 0);
                        }
                    }
                }

                //はじめてコレクションアイテムを手に入れたら発生
                /*if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                {}
                else
                {
                    if (!GameMgr.Beginner_flag[2]) //はじめてコレクションアイテム手に入れた
                    {
                        //所持数チェック
                        GetFirstCollectionItem = false;
                        for (i=0; i< GameMgr.CollectionItemsName.Count; i++)
                        {
                            if(pitemlist.KosuCount(GameMgr.CollectionItemsName[i]) >= 1)
                            {
                                GetFirstCollectionItem = true;
                            }
                        }

                        if (GetFirstCollectionItem)
                        {
                            GameMgr.Beginner_flag[2] = true;
                            GameMgr.GirlLoveSubEvent_stage1[81] = true;
                            GameMgr.GirlLoveSubEvent_num = 81;
                            GameMgr.check_GirlLoveSubEvent_flag = false;
                        }
                    }
                }*/

                //はじめて体力が0
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (!GameMgr.Beginner_flag[4])
                    {

                        if (PlayerStatus.player_girl_lifepoint <= 0)
                        {
                            GameMgr.Beginner_flag[4] = true;
                            Event_startcheck(82, 1, false, false, 0);
                        }
                    }
                }

                //はじめてお金が半分を下回った
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    //酒場でていなければイベント発生
                    if (matplace_database.matplace_lists[matplace_database.SearchMapString("Or_Bar_A1")].placeFlag == 1)
                    {

                    }
                    else
                    {
                        if (!GameMgr.Beginner_flag[5])
                        {

                            if (PlayerStatus.player_money <= 1000)
                            {
                                GameMgr.Beginner_flag[5] = true;
                                Event_startcheck(83, 1, false, false, 0);
                            }
                        }
                    }
                }

                //はじめてエメラルどんぐりをとったら発生　衣装交換アイテムの説明がある。
                /*if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (pitemlist.KosuCount("emeralDongri") >= 1 || pitemlist.KosuCount("sapphireDongri") >= 1)
                    {
                        if (GameMgr.GirlLoveSubEvent_stage1[84] == false)
                        {
                            GameMgr.GirlLoveSubEvent_stage1[84] = true;
                            GameMgr.GirlLoveSubEvent_num = 84;

                            GameMgr.Mute_on = true;
                            GameMgr.check_GirlLoveSubEvent_flag = false;
                        }
                    }
                }*/

                //はじめて水っぽいなどのマイナス効果がつくお菓子を作った
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (!GameMgr.Beginner_flag[6])
                    {
                        if (pitemlist.player_extremepanel_itemlist.Count > 0)
                        {
                            if (pitemlist.player_extremepanel_itemlist[0].Oily > GameMgr.Watery_Line ||
                                pitemlist.player_extremepanel_itemlist[0].Powdery > GameMgr.Watery_Line)
                            {
                                GameMgr.Beginner_flag[6] = true;
                                Event_startcheck(85, 1, false, false, 0);
                            }
                            else
                            {
                                if (pitemlist.player_extremepanel_itemlist[0].itemType_sub.ToString() == "Juice" ||
                                    pitemlist.player_extremepanel_itemlist[0].itemType_sub.ToString() == "Tea" ||
                                    pitemlist.player_extremepanel_itemlist[0].itemType_sub.ToString() == "Tea_Potion" ||
                                    pitemlist.player_extremepanel_itemlist[0].itemType_sub.ToString() == "Coffee_Mat")
                                { }
                                else
                                {
                                    if (pitemlist.player_extremepanel_itemlist[0].Watery > GameMgr.Watery_Line)
                                    {
                                        GameMgr.Beginner_flag[6] = true;
                                        Event_startcheck(85, 1, false, false, 0);
                                    }
                                }
                            }
                        }
                    }
                }

                //はじめてA+かSを甘さなどの味でだしたとき
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (!GameMgr.Beginner_flag[7])
                    {
                        if (GameMgr.check_TasteHighScore_Hintflag)
                        {
                            GameMgr.check_TasteHighScore_Hintflag = false;

                            GameMgr.Beginner_flag[7] = true;
                            Event_startcheck(87, 1, true, false, 0);
                        }
                    }
                }

                //はじめて衣装装備を買った 70番台～
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    //所持数チェック
                    GetEmeraldItem = false;
                    i = 0;
                    while (i < pitemlist.emeralditemlist.Count)
                    {
                        if (pitemlist.KosuCountEmerald(pitemlist.emeralditemlist[i].event_itemName) >= 1)
                        {

                            switch (pitemlist.emeralditemlist[i].event_itemName)
                            {
                                case "Glass_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[70])
                                    {
                                        Event_startcheck(70, 1, true, true, 70);
                                    }
                                    break;

                                case "Sukumizu_Costume":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[71])
                                    {
                                        Event_startcheck(71, 1, true, true, 71);
                                    }
                                    break;

                                case "Meid_Black_Costume":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[72])
                                    {
                                        Event_startcheck(72, 1, true, true, 72);
                                    }
                                    break;

                                case "PinkGoth_Costume":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[73])
                                    {
                                        Event_startcheck(73, 1, true, true, 73);
                                    }
                                    break;

                                case "RedDress_Costume":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[74])
                                    {
                                        Event_startcheck(74, 1, true, true, 74);
                                    }
                                    break;

                                case "BalloonHat_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[75])
                                    {
                                        Event_startcheck(75, 1, true, true, 75);
                                    }
                                    break;

                                case "AngelWing_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[76])
                                    {
                                        Event_startcheck(76, 1, true, true, 76);
                                    }
                                    break;

                                case "Nekomimi_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[77])
                                    {
                                        Event_startcheck(77, 1, true, true, 77);
                                    }
                                    break;

                                case "FlowerHairpin_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[78])
                                    {
                                        Event_startcheck(78, 1, true, true, 78);
                                    }
                                    break;

                                case "TwincleStarDust_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[79])
                                    {
                                        Event_startcheck(79, 1, true, true, 79);
                                    }
                                    break;

                                case "LavenderDress_Costume":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[450])
                                    {
                                        Event_startcheck(450, 1, true, true, 80);
                                    }
                                    break;

                                case "DongriPochet_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[451])
                                    {
                                        Event_startcheck(451, 1, true, true, 81);
                                    }
                                    break;

                                case "PatissierHat_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[452])
                                    {
                                        Event_startcheck(452, 1, true, true, 82);
                                    }
                                    break;

                                default:

                                    break;
                            }

                            if (GetEmeraldItem)
                            {
                                break;
                            }
                        }
                        i++;
                    }
                }

                //
                //はじめてお皿をとったときのイベントチェック
                //
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.GirlLoveSubEvent_stage1[86] == false) //はじめておさら系アイテムげっと
                    {
                        i = 0;
                        foreach (string items in GameMgr.PlateSetItemsName.Keys)
                        {
                            if (items == "teaset_normal") { } //デフォルトは無視
                            else
                            {
                                if (pitemlist.KosuCount(items) >= 1)
                                {
                                    Event_startcheck(86, 1, false, false, 0);
                                    break;
                                }
                            }
                            i++;
                        }
                        
                    }
                }

                //置物や土産を買った
                //
                //はじめてムゲンニワトリをとったときのイベントチェック
                //
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.GirlLoveSubEvent_stage1[470] == false) 
                    {
                        if (pitemlist.KosuCount("mugen_niwatori") >= 1)
                        {
                            Event_startcheck(470, 1, false, false, 0);
                        }
                    }
                }


                /*if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (!GameMgr.GirlLoveSubEvent_stage1[100])
                    {
                        if (pitemlist.KosuCount("kuma_nuigurumi") >= 1)
                        {
                            Event_startcheck(100, 1, false, true, 0);
                        }
                    }
                }*/

                //紫色の小瓶
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.GirlLoveSubEvent_stage1[453] == false)
                    {
                        if (pitemlist.KosuCount("shokukan_powerup3") >= 1)
                        {
                            Event_startcheck(453, 1, false, false, 0);
                        }
                    }
                }

                //エンジェルハート
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.GirlLoveSubEvent_stage1[454] == false)
                    {
                        if (pitemlist.KosuCount("otona_powerup1") >= 1)
                        {
                            Event_startcheck(454, 1, false, false, 0);
                        }
                    }
                }





                //レシピ100%達成
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.game_Recipi_archivement_rate >= 100.0f && GameMgr.GirlLoveSubEvent_stage1[101] == false) //4になったときのサブイベントを使う。
                    {
                        Event_startcheck(101, 1, false, false, 0);

                        ev_id = pitemlist.Find_eventitemdatabase("silver_neko_cookie_recipi");
                        pitemlist.add_eventPlayerItem(ev_id, 1); //銀のねこクッキーのレシピを追加
                    }
                }

                //ハートレベル99 コスチュームをゲット
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (PlayerStatus.girl1_Love_lv >= 99 && GameMgr.GirlLoveSubEvent_stage1[103] == false) //
                    {
                        GameMgr.GirlLoveSubEvent_num = 103;
                        GameMgr.GirlLoveSubEvent_stage1[103] = true;

                        GameMgr.check_GirlLoveSubEvent_flag = false;

                        GameMgr.Mute_on = true;

                        //pitemlist.addPlayerItemString("rubyDongri", 1); //るびーどんぐり

                        //ピンクのうさぎコスチューム
                        _id = pitemlist.SearchEmeraldItemStringID("PinkUsagi_Costume");
                        pitemlist.add_EmeraldPlayerItem(_id, 1);
                    }
                }

                
                //お金10万ルピア達成
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (PlayerStatus.player_money >= GameMgr.GoldMasterMoneyLine && GameMgr.GirlLoveSubEvent_stage1[102] == false) //4になったときのサブイベントを使う。
                    {
                        GameMgr.GirlLoveSubEvent_num = 102;
                        GameMgr.GirlLoveSubEvent_stage1[102] = true;

                        GameMgr.check_GirlLoveSubEvent_flag = false;

                        GameMgr.Mute_on = true;

                        ev_id = pitemlist.Find_eventitemdatabase("gold_neko_cookie_recipi");
                        pitemlist.add_eventPlayerItem(ev_id, 1); //金のねこクッキーのレシピを追加
                    }
                }

                /*
                //エクストラモードのみのイベント　ゲーム中で点数が777点をこえた。
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.Story_Mode == 1)
                    {
                        if (GameMgr.SpecialSubevent_EatAfterflag == true && GameMgr.GirlLoveSubEvent_stage1[103] == false) //
                        {
                            GameMgr.SpecialSubevent_EatAfterflag = false;

                            GameMgr.GirlLoveSubEvent_num = 103;
                            GameMgr.GirlLoveSubEvent_stage1[103] = true;

                            GameMgr.check_GirlLoveSubEvent_flag = false;

                            GameMgr.Mute_on = true;

                            //pitemlist.addPlayerItemString("Record_17", 1); //レコード
                        }
                    }
                }

                //エクストラモードのみのイベント　ヒカリにあげたお菓子総数150回超えた　レコードゲット
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.Story_Mode == 1)
                    {
                        if (PlayerStatus.player_girl_eatCount >= 150 && GameMgr.GirlLoveSubEvent_stage1[104] == false) //
                        {
                            GameMgr.GirlLoveSubEvent_num = 104;
                            GameMgr.GirlLoveSubEvent_stage1[104] = true;

                            GameMgr.check_GirlLoveSubEvent_flag = false;

                            GameMgr.Mute_on = true;

                            pitemlist.addPlayerItemString("Record_21", 1); //レコード パティシエールレッスン
                        }
                    }
                }*/




                //
                //はじめてアイテムをとったときのイベントチェック さくら花びらとかは、GirlLoveEvent_numの影響うけるので、上でチェックしてる
                //
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                {}
                else
                {
                    if (GameMgr.check_GetMat_flag)
                    {
                        if (GameMgr.GirlLoveSubEvent_stage1[405] == false) //はじめてブラックロータスをとってきた
                        {
                            if (pitemlist.KosuCount("blacklotus") >= 1)
                            {
                                GameMgr.GirlLoveSubEvent_stage1[405] = true;
                                GameMgr.GirlLoveSubEvent_num = 405;

                                GameMgr.check_GirlLoveSubEvent_flag = false;

                                GameMgr.Mute_on = true;
                            }
                        }
                    }
                }

                //
                //調合後にチェック　はじめて、各特別なお菓子作ったイベント
                //
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.check_CompoAfter_SubEventflag)
                    {
                        GameMgr.check_CompoAfter_SubEventflag = false;

                        _basename = database.items[GameMgr.Okashi_makeID].itemName;
                        _baseitemtype_sub = database.items[GameMgr.Okashi_makeID].itemType_sub.ToString();
                        _baseitemtype_subB = database.items[GameMgr.Okashi_makeID].itemType_subB.ToString();

                        //はじめてルミエメラルドシュガー作ったなど
                        foreach (string items in GameMgr.OkashiAtFirst_eventlist.Keys)
                        {
                            if (_basename == items)
                            {
                                if (!GameMgr.GirlLoveSubEvent_stage1[GameMgr.OkashiAtFirst_eventlist[_basename]])
                                {
                                    Event_startcheck(GameMgr.OkashiAtFirst_eventlist[_basename], 1, false, false, 0);
                                    break;
                                }
                            }
                            else if (_baseitemtype_sub == items)
                            {
                                if (!GameMgr.GirlLoveSubEvent_stage1[GameMgr.OkashiAtFirst_eventlist[_baseitemtype_sub]])
                                {
                                    Event_startcheck(GameMgr.OkashiAtFirst_eventlist[_baseitemtype_sub], 1, false, false, 0);
                                    break;
                                }
                            }
                            else if (_baseitemtype_subB == items)
                            {
                                if (!GameMgr.GirlLoveSubEvent_stage1[GameMgr.OkashiAtFirst_eventlist[_baseitemtype_subB]])
                                {
                                    Event_startcheck(GameMgr.OkashiAtFirst_eventlist[_baseitemtype_subB], 1, false, false, 0);
                                    break;
                                }
                            }
                        }
                    }
                }

                //
                //食べた後にチェック　１５０点以上で特別なイベント GirlEatJudge.csで発生の判定
                //
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.SpecialSubevent_EatAfterflag) //思い出イベントフラグ解禁の点数以上でないと発生しない
                    {
                        GameMgr.SpecialSubevent_EatAfterflag = false;

                        Event_startcheck(GameMgr.SpecialSubevent_Num, 1, false, false, 0);
                    }
                }


                //
                //スターパネル閉じたあとにチェック
                //
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.check_StarPanel_Eventflag) //
                    {
                        GameMgr.check_StarPanel_Eventflag = false;

                        if (!GameMgr.GirlLoveSubEvent_stage1[700])
                        {
                            GameMgr.GirlLoveSubEvent_stage1[700] = true;
                            Event_startcheck(700, 0, false, false, 0);
                        }
                    }
                }





                //
                //寝ておきたあとにチェックする系のイベント
                //

                //
                //コンテストの開催日になった
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.check_SleepEnd_Eventflag[0]) //ねておきたあとにチェック
                    {
                        GameMgr.check_SleepEnd_Eventflag[0] = false;

                        Debug.Log("チェック　本日がコンテスト開催日かどうか");
                        i = 0;
                        while (i<GameMgr.contest_accepted_list.Count)
                        {
                            if(GameMgr.contest_accepted_list[i].Month == PlayerStatus.player_cullent_month &&
                                GameMgr.contest_accepted_list[i].Day == PlayerStatus.player_cullent_day)
                            {
                                Debug.Log("本日コンテスト開催日 " + GameMgr.contest_accepted_list[i].Month + "/" + GameMgr.contest_accepted_list[i].Day + " " +
                                    GameMgr.contest_accepted_list[i].contestName);

                                GameMgr.GirlLoveSubEvent_num = 1000;
                                GameMgr.check_GirlLoveSubEvent_flag = false;

                                GameMgr.Mute_on = true;
                                break;
                            }
                            i++;
                        }

                    }
                }

                //月日をまたいだ場合、家賃が発生 5月～
                if (GameMgr.System_Yachin_ON)
                {
                    if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                    { }
                    else
                    {
                        if (GameMgr.check_SleepEnd_Eventflag[1]) //ねておきたあとにチェック
                        {
                            GameMgr.check_SleepEnd_Eventflag[1] = false;

                            if (!GameMgr.yachinSPRoomON_Flag) //家賃がない家の場合、家賃なくなる
                            { }
                            else
                            {
                                Debug.Log("チェック　本日が１０・２０・３０日かどうか");
                                Debug.Log("本日の日: " + PlayerStatus.player_cullent_day);

                                //10日ごとチェックバージョン
                                if (PlayerStatus.player_cullent_day % GameMgr.System_Yachin_Day == 0)
                                {
                                    //月はこのタイミングでも更新する。
                                    GameMgr.SleepBefore_Month = PlayerStatus.player_cullent_month;

                                    if (GameMgr.yachin_counter == 0) //はじめて家賃を支払った
                                    {
                                        GameMgr.yachin_counter++;

                                        //家賃発生　事前に所持金をチェックし、払えない場合はお手付きかゲームオーバー
                                        if (PlayerStatus.player_money < GameMgr.Yachin_Cost_cullent)
                                        {
                                            //払えない場合
                                            GameMgr.GirlLoveSubEvent_num = 1100;
                                            GameMgr.GirlTalk_num = 10;

                                            GameMgr.yachin_otetsuki_count++; //お手付き　２回たまるとゲームオーバー
                                            GameMgr.yachin_tainou_count++; //トータルの滞納回数
                                        }
                                        else
                                        {
                                            //払える
                                            moneyStatus_Controller.UseMoney(GameMgr.Yachin_Cost_cullent);
                                            GameMgr.GirlLoveSubEvent_num = 1100;
                                            GameMgr.GirlTalk_num = 0;
                                        }
                                    }
                                    else
                                    {
                                        //家賃　二回目以降
                                        GameMgr.yachin_counter++;

                                        //家賃発生　事前に所持金をチェックし、払えない場合はお手付きかゲームオーバー
                                        switch (GameMgr.yachin_otetsuki_count)
                                        {
                                            case 0:

                                                if (PlayerStatus.player_money < GameMgr.Yachin_Cost_cullent)
                                                {
                                                    //払えない場合
                                                    GameMgr.GirlLoveSubEvent_num = 1101;
                                                    GameMgr.GirlTalk_num = 10;

                                                    GameMgr.yachin_otetsuki_count++; //お手付き　２回たまるとゲームオーバー
                                                    GameMgr.yachin_tainou_count++; //トータルの滞納回数
                                                }
                                                else
                                                {
                                                    //払える
                                                    moneyStatus_Controller.UseMoney(GameMgr.Yachin_Cost_cullent);
                                                    GameMgr.GirlLoveSubEvent_num = 1101;
                                                    GameMgr.GirlTalk_num = 0;
                                                }
                                                break;

                                            case 1:

                                                if (PlayerStatus.player_money < GameMgr.Yachin_Cost_cullent)
                                                {
                                                    //払えない場合
                                                    GameMgr.GirlLoveSubEvent_num = 1110; //２回たまったのでゲームーオーバー
                                                    GameMgr.GirlTalk_num = 11;

                                                    GameMgr.yachin_otetsuki_count++; //お手付き　２回たまったのでゲームオーバー
                                                    GameMgr.yachin_tainou_count++; //トータルの滞納回数

                                                    //一回たまって二回目も支払えなかったのでゲームオーバー　宴終了後自動でゲームオーバー画面へいく
                                                    GameMgr.Utage_MapMoveON = true;
                                                }
                                                else
                                                {
                                                    //払える
                                                    moneyStatus_Controller.UseMoney(GameMgr.Yachin_Cost_cullent);
                                                    GameMgr.GirlLoveSubEvent_num = 1101;
                                                    GameMgr.GirlTalk_num = 0;

                                                    GameMgr.yachin_otetsuki_count = 0; //お手付きリセット
                                                }
                                                break;

                                            default: //例外処理用

                                                if (PlayerStatus.player_money < GameMgr.Yachin_Cost_cullent)
                                                {
                                                    //払えない場合
                                                    GameMgr.GirlLoveSubEvent_num = 1101;
                                                    GameMgr.GirlTalk_num = 10;

                                                    GameMgr.yachin_otetsuki_count++; //お手付き　２回たまるとゲームオーバー
                                                    GameMgr.yachin_tainou_count++; //トータルの滞納回数
                                                }
                                                else
                                                {
                                                    //払える
                                                    moneyStatus_Controller.UseMoney(GameMgr.Yachin_Cost_cullent);
                                                    GameMgr.GirlLoveSubEvent_num = 1101;
                                                    GameMgr.GirlTalk_num = 0;

                                                    GameMgr.yachin_otetsuki_count = 0; //お手付きリセット
                                                }
                                                break;
                                        }

                                    }

                                    if (GameMgr.YachinSkipFlag) //会話スキップがONのとき　会話イベントは表示しない　家賃はとられる
                                    {
                                        if (GameMgr.GirlLoveSubEvent_num == 1110) //ただし、ゲームオーバーのときはイベント表示
                                        {
                                            GameMgr.check_GirlLoveSubEvent_flag = false;
                                            GameMgr.Mute_on = true;
                                        }
                                        else
                                        { }
                                    }
                                    else
                                    {
                                        GameMgr.check_GirlLoveSubEvent_flag = false;
                                        GameMgr.Mute_on = true;
                                    }
                                }

                                //月はじめバージョン
                                /*if (PlayerStatus.player_cullent_month > GameMgr.SleepBefore_Month)
                                {
                                    //月はこのタイミングでも更新する。
                                    GameMgr.SleepBefore_Month = PlayerStatus.player_cullent_month;

                                    //寝る前の月　起きた後の月で、月が変わっていた　家賃発生
                                    moneyStatus_Controller.UseMoney(GameMgr.System_Yachin_Cost01);

                                    GameMgr.GirlLoveSubEvent_num = 1100;
                                    GameMgr.check_GirlLoveSubEvent_flag = false;

                                    GameMgr.Mute_on = true;
                                }*/

                                //家賃とられる5日前　アテンションイベント
                                if (PlayerStatus.player_cullent_day == (GameMgr.System_Yachin_Day / 2))
                                {
                                    if (!GameMgr.GirlLoveSubEvent_stage1[180])
                                    {
                                        GameMgr.GirlLoveSubEvent_stage1[180] = true;

                                        GameMgr.GirlLoveSubEvent_num = 180;
                                        GameMgr.check_GirlLoveSubEvent_flag = false;
                                        GameMgr.Mute_on = true;

                                    }
                                }
                            }
                        }
                    }
                }
               

                //コンテスト終了後、いったん寝てから発生するイベント
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.check_SleepEnd_Eventflag[3]) //ねておきたあとにチェック
                    {
                        GameMgr.check_SleepEnd_Eventflag[3] = false;
                        Debug.Log("コンテスト終了後　寝て起きて、ハートが上がるイベントチェック");

                        if (GameMgr.Contest_afterHomeHeartUpFlag)
                        {
                            GameMgr.Contest_afterHomeHeartUpFlag = false;

                            if (GameMgr.contest_LimitTimeOver_After_flag) //時間過ぎて失格の場合はこっち
                            {
                                GameMgr.GirlLoveSubEvent_num = 1212;
                                GameMgr.SubEvAfterHeartGet_num = 211;
                            }
                            else
                            {
                                if (GameMgr.contest_Disqualification || GameMgr.contest_Disqualification2) //提出おかしが違って失格だった場合はこっち
                                {
                                    if (GameMgr.contest_Disqualification)
                                    {
                                        GameMgr.GirlLoveSubEvent_num = 1210;
                                        GameMgr.SubEvAfterHeartGet_num = 210;
                                    }
                                    if (GameMgr.contest_Disqualification2)
                                    {
                                        GameMgr.GirlLoveSubEvent_num = 1211;
                                        GameMgr.SubEvAfterHeartGet_num = 210;
                                    }
                                }
                                else
                                {
                                    switch(GameMgr.contest_Rank_Count) //上から1位～5位
                                    {
                                        case 1:

                                            GameMgr.GirlLoveSubEvent_num = 1200;
                                            break;

                                        case 2:

                                            GameMgr.GirlLoveSubEvent_num = 1201;
                                            break;

                                        case 3:

                                            GameMgr.GirlLoveSubEvent_num = 1201;
                                            break;

                                        case 4:

                                            GameMgr.GirlLoveSubEvent_num = 1202;
                                            break;

                                        case 5:

                                            GameMgr.GirlLoveSubEvent_num = 1203;
                                            break;

                                        case 0: //エデンコンで敗退した場合

                                            GameMgr.GirlLoveSubEvent_num = 1204;
                                            break;
                                    }
                                    
                                    GameMgr.SubEvAfterHeartGet_num = 200;
                                }
                            }
                            
                            
                            //GameMgr.Mute_on = true;

                            GameMgr.SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                            
                            GameMgr.check_GirlLoveSubEvent_flag = false;
                            GameMgr.contest_Disqualification = false;
                            GameMgr.contest_Disqualification2 = false;
                            GameMgr.contest_LimitTimeOver_After_flag = false;
                        }
                    }
                }

                //コンテスト終了後、いったん寝てから発生するイベント
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.check_SleepEnd_Eventflag[2]) //ねておきたあとにチェック
                    {
                        GameMgr.check_SleepEnd_Eventflag[2] = false;
                        Debug.Log("コンテスト終了後　イベントチェック");

                        if (GameMgr.Contest_afterHomeEventFlag)
                        {
                            GameMgr.Contest_afterHomeEventFlag = false;

                            //コンテスト一回でたあと、コンテストメモについてのイベント
                            if (conteststartList_database.SearchContestVictory("Or_Contest_010") != 0) //クッキーコンテストでとりあえず出場し順位入った。
                            {
                                //
                                if (!GameMgr.GirlLoveSubEvent_stage1[420])
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[420] = true;

                                    GameMgr.GirlLoveSubEvent_num = 420;
                                    GameMgr.check_GirlLoveSubEvent_flag = false;
                                    GameMgr.Mute_on = true;

                                }
                            }

                            //夏コンテスト優勝した場合、エデン２つめをゲットしたぞ～のイベント
                            if (conteststartList_database.SearchContestVictory("Or_Contest_002") == 1) //一位をゲットしてた＝エデン２をゲット
                            {
                                //エデン2枚目を見るイベント
                                if (!GameMgr.GirlLoveSubEvent_stage1[400])
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[400] = true;

                                    GameMgr.GirlLoveSubEvent_num = 400;
                                    GameMgr.check_GirlLoveSubEvent_flag = false;
                                    GameMgr.Mute_on = true;

                                }
                            }

                            //秋コンテスト優勝した場合、エデン３つめをゲットしたぞ～のイベント
                            if (conteststartList_database.SearchContestVictory("Or_Contest_003") == 1) //一位をゲットしてた＝エデン３をゲット
                            {
                                //エデン3枚目を見るイベント
                                if (!GameMgr.GirlLoveSubEvent_stage1[401])
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[401] = true;

                                    GameMgr.GirlLoveSubEvent_num = 401;
                                    GameMgr.check_GirlLoveSubEvent_flag = false;
                                    GameMgr.Mute_on = true;

                                }
                            }

                            //コンテストでたけど、シンプルなおかしを提出した＆100点未満の場合 特定の自由課題のお菓子コンテストのとき　初級あたりのみ出る
                            if (GameMgr.Contest_Name == "Or_Contest_030" || GameMgr.Contest_Name == "Or_Contest_110" || 
                                GameMgr.Contest_Name == "Or_Contest_290" || GameMgr.Contest_Name == "Or_Contest_220" || 
                                GameMgr.Contest_Name == "Or_Contest_490") //
                            {
                                if (GameMgr.contest_TotalScore < 100)
                                {
                                    _baseitemtype_subB = database.items[database.SearchItemID(GameMgr.contest_okashiID)].itemType_subB;

                                    //シンプルなクッキーやラスクなどを提出していた場合
                                    if (_baseitemtype_subB == "a_Cookie" || _baseitemtype_subB == "a_Cookie_Hard" || _baseitemtype_subB == "a_Rusk"
                                        || _baseitemtype_subB == "a_Crepe_Mat" || _baseitemtype_subB == "a_Crepe_Mat" || _baseitemtype_subB == "a_CreampuffSimple"
                                        || _baseitemtype_subB == "a_Cake_Mat" || _baseitemtype_subB == "a_Bread" || _baseitemtype_subB == "a_Bread_Sliced"
                                        || _baseitemtype_subB == "a_JuiceSimple")
                                    {
                                        //コンテストでは、単純なおかしは点数が出ないというヒントをいう
                                        if (!GameMgr.GirlLoveSubEvent_stage1[422])
                                        {
                                            GameMgr.GirlLoveSubEvent_stage1[422] = true;

                                            GameMgr.GirlLoveSubEvent_num = 422;
                                            GameMgr.check_GirlLoveSubEvent_flag = false;
                                            GameMgr.Mute_on = true;

                                        }
                                    }
                                }
                            }


                        }
                    }
                }

                //寝て起きた後、ヒカリかNPCがくるイベント
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.check_SleepEnd_Eventflag[4]) //ねておきたあとにチェック
                    {
                        GameMgr.check_SleepEnd_Eventflag[4] = false;
                        Debug.Log("コンテスト終了後　ヒカリorNPCがくるイベントチェック");

                        //コンテスト初出場し、ミラボ先生にあった後から、発生する
                        if (GameMgr.NPCMagic_eventList[0])
                        {
                            if (conteststartList_database.ReturnVictoryCount(1) >= 1) //一位のトータル取得数をゲット
                            {
                                if (GameMgr.NPCHiroba_eventDayCounter[0] <= 0) //カウンタは寝たあとで1減っていく
                                {
                                    //一位を一回以上取った場合、アマクサが初優勝時にほめてくれるイベント発生　2日後ぐらいに発生する。
                                    if (!GameMgr.NPCHiroba_eventList[1031])
                                    {
                                        GameMgr.NPCHiroba_eventList[1031] = true;

                                        GameMgr.GirlLoveSubEvent_num = 3000;
                                        GameMgr.check_GirlLoveSubEvent_flag = false;
                                        GameMgr.Mute_on = true;

                                        //アマノシャンメリーくれる
                                        pitemlist.addPlayerItemString("amano_champmery", 1);
                                        //pitemlist.add_eventPlayerItemString("MemoWhite", 1);                                    
                                    }
                                }
                            }
                        }

                        //街へでよう！クエストのとき、まだコンテスト会場いってない場合 会場へいこうと促すイベント
                        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                        { }
                        else
                        {
                            if(GameMgr.GirlLoveEvent_num == 3 && matplace_database.GetMapFlagString("Or_Contest_A1") == 0)
                            {
                                if (GameMgr.GirlLoveSubEvent_stage1_Counter[303] <= 0)
                                {
                                    if (!GameMgr.GirlLoveSubEvent_stage1[303])
                                    {
                                        GameMgr.GirlLoveSubEvent_stage1[303] = true;

                                        GameMgr.GirlLoveSubEvent_num = 303;
                                        GameMgr.check_GirlLoveSubEvent_flag = false;
                                        //GameMgr.Mute_on = true;

                                    }
                                }
                            }
                        }
                    }
                }

                //寝て起きた後、猫逃亡が発生するイベント
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.check_SleepEnd_Eventflag[5]) //ねておきたあとにチェック
                    {
                        GameMgr.check_SleepEnd_Eventflag[5] = false;
                        Debug.Log("コンテスト終了後　猫逃亡イベントチェック");

                        //ヒント系
                        /*if (GameMgr.GirlLoveSubEvent_stage1[421] == false)
                        {
                            if (conteststartList_database.SearchContestVictory("Or_Contest_010") == 1 ||
                                conteststartList_database.SearchContestVictory("Or_Contest_010") == 2) //クッキーコンテストで1位か2位に入った。ラスクコンテスト解禁されるタイミング
                            {
                                GameMgr.GirlLoveSubEvent_stage1[421] = true;
                                GameMgr.GirlLoveSubEvent_num = 421;

                                GameMgr.check_GirlLoveSubEvent_flag = false;
                            }
                        }*/

                        //ねこが逃亡フラグもここで処理
                        if (GameMgr.CatEscapeFlag)
                        {
                            GameMgr.CatEscapeFlag = false;

                            GameMgr.GirlLoveSubEvent_num = 1300;
                            GameMgr.check_GirlLoveSubEvent_flag = false;

                            GameMgr.Mute_on = true;
                        }
                    }
                }

                //寝て起きた後、コンテスト100％達成でご褒美が発生するイベント
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.check_SleepEnd_Eventflag[6]) //ねておきたあとにチェック
                    {
                        GameMgr.check_SleepEnd_Eventflag[6] = false;
                        Debug.Log("コンテスト終了後　コンテスト達成率イベントチェック");

                        contest_Master_TasseiFlag = false;
                        contest_Master_TasseiFlag_half = false;

                        conteststartList_database.Contest_ArchivementKeisan(); //各コンテスト達成率を計算

                        //A 春・夏・秋・冬とコンテスト分かれてる場合のチェックパターン
                        Check_ContestTasseiListA(); 

                        //B 春コンのみでコンテストを一括管理してる場合のパターン
                        //Check_ContestTasseiListB();
                    }
                }
            }


            //フラグは必ずリセット           
            GameMgr.check_OkashiAfter_flag = false;
            GameMgr.check_GetMat_flag = false;

            //最後のタイミングで、決定したサブイベントの宴を再生
            if (!GameMgr.check_GirlLoveSubEvent_flag) //サブイベント発生した
            {

                //クエスト発生
                Debug.Log("サブ好感度イベントの発生");

                //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                ReadGirlLoveEvent();

            }
            else //全てのイベントチェックし、発生しなかったら、このスクリプトでのイベントチェック完了
            { }
        }
    }

    void Check_ContestTasseiListA()
    {
        //各コンテストでチェック
        i = 0;
        while (i < 4)
        {
            switch (i)
            {
                case 0:

                    read_ID = 0; //春
                    archive_area = 0;
                    archivement_percent = GameMgr.Contest_archivement_percent[0];
                    break;

                case 1:

                    read_ID = 1000; //春
                    archive_area = 1000;
                    archivement_percent = GameMgr.Contest_archivement_percent[1];
                    break;

                case 2:

                    read_ID = 2000; //春
                    archive_area = 2000;
                    archivement_percent = GameMgr.Contest_archivement_percent[2];
                    break;

                case 3:

                    read_ID = 3000; //春
                    archive_area = 3000;
                    archivement_percent = GameMgr.Contest_archivement_percent[3];
                    break;
            }

            contest_allcount = conteststartList_database.ContestAll_PlayOKCounter(read_ID);
            contest_victorycount = conteststartList_database.ReturnVictoryCount_Area(1, read_ID); //そのエリアの取得済　1位をカウント

            Debug.Log("contest_allcount: " + contest_allcount);
            Debug.Log("contest_victorycount: " + contest_victorycount);
            Debug.Log("contest_archivement_percent: " + archivement_percent);

            //100%達成をまずチェック
            if (contest_allcount == contest_victorycount)
            {

                //どのエリアを達成したか
                switch (archive_area)
                {
                    case 0:

                        //春エリア100%達成
                        if (!GameMgr.GirlLoveSubEvent_stage1[720])
                        {
                            GameMgr.GirlLoveSubEvent_stage1[720] = true;

                            GameMgr.GirlLoveSubEvent_num = 720;

                            //GameMgr.OrRoomRelease[1] = true;
                            GameMgr.OrRoomRelease[2] = true;

                            contest_Master_TasseiFlag = true;
                        }
                        break;

                    case 1000:

                        //夏エリア100%達成
                        if (!GameMgr.GirlLoveSubEvent_stage1[721])
                        {
                            GameMgr.GirlLoveSubEvent_stage1[721] = true;

                            GameMgr.GirlLoveSubEvent_num = 721;

                            //GameMgr.OrRoomRelease[3] = true;
                            GameMgr.OrRoomRelease[4] = true;
                            GameMgr.OrRoomRelease[5] = true;

                            contest_Master_TasseiFlag = true;
                        }
                        break;

                    case 2000:

                        //秋エリア100%達成
                        if (!GameMgr.GirlLoveSubEvent_stage1[722])
                        {
                            GameMgr.GirlLoveSubEvent_stage1[722] = true;

                            GameMgr.GirlLoveSubEvent_num = 722;

                            //GameMgr.OrRoomRelease[6] = true;
                            GameMgr.OrRoomRelease[7] = true;
                            GameMgr.OrRoomRelease[8] = true;

                            contest_Master_TasseiFlag = true;
                        }
                        break;

                    case 3000:

                        //冬エリア100%達成
                        if (!GameMgr.GirlLoveSubEvent_stage1[723])
                        {
                            GameMgr.GirlLoveSubEvent_stage1[723] = true;

                            GameMgr.GirlLoveSubEvent_num = 723;

                            contest_Master_TasseiFlag = true;
                        }
                        break;
                }

                if (contest_Master_TasseiFlag)
                {
                    GameMgr.check_GirlLoveSubEvent_flag = false;
                    GameMgr.Mute_on = true;

                    break;
                }

            }
            else
            {
                //100%以下をチェック　こっちは数字より上だったらでOK
                //５０％達成
                if (50.0f <= archivement_percent) //Mathf.CeilToInt(contest_allcount / 2)
                {
                    //どのエリアを達成したか
                    switch (archive_area)
                    {
                        case 0:

                            //春エリア50%達成
                            if (!GameMgr.GirlLoveSubEvent_stage1[724])
                            {
                                GameMgr.GirlLoveSubEvent_stage1[724] = true;

                                GameMgr.GirlLoveSubEvent_num = 724;

                                GameMgr.OrRoomRelease[1] = true;
                                //GameMgr.OrRoomRelease[2] = true;

                                contest_Master_TasseiFlag_half = true;
                            }
                            break;

                        case 1000:

                            //夏エリア50%達成
                            if (!GameMgr.GirlLoveSubEvent_stage1[725])
                            {
                                GameMgr.GirlLoveSubEvent_stage1[725] = true;

                                GameMgr.GirlLoveSubEvent_num = 725;

                                GameMgr.OrRoomRelease[3] = true;
                                //GameMgr.OrRoomRelease[4] = true;
                                //GameMgr.OrRoomRelease[5] = true;

                                contest_Master_TasseiFlag_half = true;
                            }
                            break;

                        case 2000:

                            //秋エリア50%達成
                            if (!GameMgr.GirlLoveSubEvent_stage1[726])
                            {
                                GameMgr.GirlLoveSubEvent_stage1[726] = true;

                                GameMgr.GirlLoveSubEvent_num = 726;

                                GameMgr.OrRoomRelease[6] = true;
                                //GameMgr.OrRoomRelease[7] = true;
                                //GameMgr.OrRoomRelease[8] = true;

                                contest_Master_TasseiFlag_half = true;
                            }
                            break;

                        case 3000:

                            //冬エリア50%達成
                            if (!GameMgr.GirlLoveSubEvent_stage1[727])
                            {
                                GameMgr.GirlLoveSubEvent_stage1[727] = true;

                                GameMgr.GirlLoveSubEvent_num = 727;

                                contest_Master_TasseiFlag_half = true;
                            }
                            break;
                    }

                    if (contest_Master_TasseiFlag_half)
                    {
                        GameMgr.check_GirlLoveSubEvent_flag = false;
                        GameMgr.Mute_on = true;

                        break;
                    }
                }
            }

            i++;
        }
    }

    void Check_ContestTasseiListB()
    {
        read_ID = 0; //春
        archive_area = 0;
        archivement_percent = GameMgr.Contest_archivement_percent[0];

        contest_allcount = conteststartList_database.ContestAll_PlayOKCounter(read_ID);
        contest_victorycount = conteststartList_database.ReturnVictoryCount_Area(1, read_ID); //そのエリアの取得済　1位をカウント

        Debug.Log("contest_allcount: " + contest_allcount);
        Debug.Log("contest_victorycount: " + contest_victorycount);
        Debug.Log("contest_archivement_percent: " + archivement_percent);

        //100%達成をまずチェック
        if (contest_allcount == contest_victorycount)
        {
            //エリア100%達成
            if (!GameMgr.GirlLoveSubEvent_stage1[720])
            {
                GameMgr.GirlLoveSubEvent_stage1[720] = true;

                GameMgr.GirlLoveSubEvent_num = 720;

                //GameMgr.OrRoomRelease[1] = true;
                GameMgr.OrRoomRelease[2] = true;

                contest_Master_TasseiFlag = true;
            }
        }
        else
        {
            //100%以下をチェック　こっちは数字より上だったらでOK

            //90％達成
            if (!contest_Master_TasseiFlag_half)
            {
                if (90.0f <= archivement_percent) //Mathf.CeilToInt(contest_allcount / 2)
                {
                    if (!GameMgr.GirlLoveSubEvent_stage1[722])
                    {
                        GameMgr.GirlLoveSubEvent_stage1[722] = true;

                        GameMgr.GirlLoveSubEvent_num = 722;

                        GameMgr.OrRoomRelease[1] = true;
                        //GameMgr.OrRoomRelease[2] = true;

                        contest_Master_TasseiFlag_half = true;
                    }
                }
            }

            //75％達成
            if (!contest_Master_TasseiFlag_half)
            {
                if (75.0f <= archivement_percent) //Mathf.CeilToInt(contest_allcount / 2)
                {
                    if (!GameMgr.GirlLoveSubEvent_stage1[721])
                    {
                        GameMgr.GirlLoveSubEvent_stage1[721] = true;

                        GameMgr.GirlLoveSubEvent_num = 721;

                        GameMgr.OrRoomRelease[1] = true;
                        //GameMgr.OrRoomRelease[2] = true;

                        contest_Master_TasseiFlag_half = true;
                    }
                }
            }

            //50％達成
            if (!contest_Master_TasseiFlag_half)
            {
                if (50.0f <= archivement_percent) //Mathf.CeilToInt(contest_allcount / 2)
                {
                    if (!GameMgr.GirlLoveSubEvent_stage1[726])
                    {
                        GameMgr.GirlLoveSubEvent_stage1[726] = true;

                        GameMgr.GirlLoveSubEvent_num = 726;

                        GameMgr.OrRoomRelease[1] = true;
                        //GameMgr.OrRoomRelease[2] = true;

                        contest_Master_TasseiFlag_half = true;
                    }
                }
            }

            //35％達成
            if (!contest_Master_TasseiFlag_half)
            {
                if (35.0f <= archivement_percent) //Mathf.CeilToInt(contest_allcount / 2)
                {
                    if (!GameMgr.GirlLoveSubEvent_stage1[725])
                    {
                        GameMgr.GirlLoveSubEvent_stage1[725] = true;

                        GameMgr.GirlLoveSubEvent_num = 725;

                        GameMgr.OrRoomRelease[1] = true;
                        //GameMgr.OrRoomRelease[2] = true;

                        contest_Master_TasseiFlag_half = true;
                    }
                }
            }

            //20％達成
            if (!contest_Master_TasseiFlag_half)
            {
                if (20.0f <= archivement_percent) //Mathf.CeilToInt(contest_allcount / 2)
                {
                    if (!GameMgr.GirlLoveSubEvent_stage1[724])
                    {
                        GameMgr.GirlLoveSubEvent_stage1[724] = true;

                        GameMgr.GirlLoveSubEvent_num = 724;

                        GameMgr.OrRoomRelease[1] = true;
                        //GameMgr.OrRoomRelease[2] = true;

                        contest_Master_TasseiFlag_half = true;
                    }
                }
            }
        }

        if (contest_Master_TasseiFlag_half)
        {
            GameMgr.check_GirlLoveSubEvent_flag = false;
            GameMgr.Mute_on = true;
        }
    }

    //通常サブイベントとは別で、時間で発生するイベント
    public void GirlLove_SubTimeEventMethod()
    {
        GameMgr.girlloveevent_bunki = 1; //サブイベントの発生のチェック。宴用に分岐。

        if (GameMgr.GirlLove_loading)
        { }
        else
        {
            GameMgr.check_GirlLoveTimeEvent_flag = true;
            GameMgr.GirlLoveSubEvent_NPC_QuestID = 0; //個人依頼クエストIDは一応ここでリセット

            //お外勝手に遊びにいく
            if (!GameMgr.check_GirlLoveTimeEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                //ヒカリお外へ遊びにいく発生
                if (!GameMgr.outgirl_Nowprogress)
                {
                    if (GameMgr.OutGirlSkipFlag) { } //外出スキップON
                    else
                    {
                        if (PlayerStatus.player_cullent_hour >= 9 && PlayerStatus.player_cullent_hour < 13
                            && PlayerStatus.girl1_Love_lv >= 10) //9時から12時の間に、サイコロふる
                        {
                            if (GameMgr.outgirl_count <= 0)
                            {
                                GameMgr.outgirl_event_ON = true;
                            }

                            if (GameMgr.outgirl_event_ON)
                            {
                                random = Random.Range(0, 100);
                                Debug.Log("外出イベント　抽選スタート　20以下で成功: " + random);
                                Debug.Log("機嫌度player_girl_express_param: " + PlayerStatus.player_girl_express_param);

                                picnic_exprob = (int)(40f * PlayerStatus.player_girl_express_param * 0.01f); //20%の確率で発生。player_girl_express_paramは大体50。10~13時
                                if (picnic_exprob <= 0)
                                {
                                    picnic_exprob = 0;
                                }

                                if (PlayerStatus.player_girl_expression <= 1) { }
                                else
                                {
                                    Debug.Log("picnic_exprob: " + picnic_exprob);
                                    if (random <= picnic_exprob)
                                    {
                                        GameMgr.GirlLoveSubEvent_num = 150;
                                        GameMgr.GirlLoveSubEvent_stage1[150] = true; //イベント初発生の分をフラグっておく。

                                        GameMgr.outgirl_event_ON = false;
                                        outGirlCounterReset();//次の外出るイベントまでの日数カウンタ                                       

                                        GameMgr.check_GirlLoveTimeEvent_flag = false;

                                        //GameMgr.Mute_on = true;

                                    }
                                }
                            }
                        }
                    }
                }
                else //すでに外出中　15時ぐらいまでには帰ってくる。もし、帰ってくる前に寝るイベントが発生（お菓子で時間がたつなど）したら、そのときの条件分岐が必要。
                {
                    if (GameMgr.ReadGirlLoveTimeEvent_reading_now) //すでにこのイベント読み中の場合、スキップするように。
                    { }
                    else
                    {
                        if (PlayerStatus.player_cullent_hour >= 16 && PlayerStatus.player_cullent_hour < 18)
                        {
                            random = Random.Range(0, 100);
                            Debug.Log("外出から帰ってくる　抽選スタート　20以下で成功: " + random);

                            picnic_exprob = 20; //20%の確率で発生。

                            if (random <= picnic_exprob)
                            {
                                //ただいま～
                                OutGirlReturnHome();
                                GameMgr.check_GirlLoveTimeEvent_flag = false;

                            }
                        }
                        else if (PlayerStatus.player_cullent_hour >= 18 && PlayerStatus.player_cullent_hour < 19)
                        {
                            //18時を超えたら、必ず帰ってくる。ただいま～
                            OutGirlReturnHome();
                            GameMgr.check_GirlLoveTimeEvent_flag = false;
                        }
                        else if (PlayerStatus.player_cullent_hour >= 19 && PlayerStatus.player_cullent_hour <= 24)
                        {
                            Debug.Log("19時以降兄が家にかえってきたあと、ヒカリが採取に出てた場合、先にヒカリが帰っておりおかえり～というイベント");
                            
                            OutGirlReturnHome2();

                            GameMgr.check_GirlLoveTimeEvent_flag = false;
                        }
                        else if (PlayerStatus.player_cullent_hour >= 0 && PlayerStatus.player_cullent_hour < 8)
                        {
                            Debug.Log("19時以降兄が家にかえってきたあと、ヒカリが採取に出てた場合、先にヒカリが帰っておりおかえり～というイベント");

                            OutGirlReturnHome2();

                            GameMgr.check_GirlLoveTimeEvent_flag = false;
                        }
                    }
                }               
            }

            //ピクニック
            /*if (!GameMgr.check_GirlLoveTimeEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                //ピクニックイベントチェック
                if (!GameMgr.outgirl_Nowprogress)
                {
                    if (GameMgr.PicnicSkipFlag) { } //ピクニックスキップON
                    else
                    {
                        //HLV12~  
                        if (PlayerStatus.girl1_Love_lv >= 12)
                        {
                            if (PlayerStatus.player_cullent_hour >= 12 && PlayerStatus.player_cullent_hour <= 14) //12時から15時の間に、サイコロふる
                            {
                                PicnicEvent();
                            }
                        }
                    }
                }
            }*/

            //ねこがランダムでやってくる
            if (!GameMgr.check_GirlLoveTimeEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                //ねこイベントチェック
                if (!GameMgr.outgirl_Nowprogress)
                {
                    //HLV12~  
                    if (GameMgr.System_CatGetMat_Flag)
                    {
                        if(GameMgr.OrCompound_RoomNum == 7) //ねこの家にいると、猫がよくくるようになる。
                        {
                            cat_come_day = 5;
                        }
                        else
                        {
                            cat_come_day = 15;
                        }

                        if (PlayerStatus.player_cullent_day % cat_come_day == 0) //15日or30日だけ、抽選する
                        {
                            if (PlayerStatus.player_cullent_hour >= 9 && PlayerStatus.player_cullent_hour <= 15) //12時から15時の間に、サイコロふる
                            {
                                CatRandomComingEvent();
                            }
                        }
                    }
                }
            }

            //街の人がきて、おかしのご依頼
            if (GameMgr.System_BarNPC_FriendEventFlag)
            {                
                if (!GameMgr.check_GirlLoveTimeEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    //ご依頼イベントチェック
                    if (!GameMgr.outgirl_Nowprogress)
                    {
                        if (PlayerStatus.player_cullent_hour >= 10 && PlayerStatus.player_cullent_hour <= 12) //10時から12時の間に、サイコロふる
                        {
                            //②ご依頼の品を受け取りにくるフェーズ　こっちが発生したら、下のご依頼がくるフェーズはチェックを無視　次の日までチェックは無視する
                            //クエスト受注の「個人依頼」の日付をチェックする
                            PeopleQuest_DayCheck();

                            if (!GameMgr.GirlLoveSubEvent_NPC_OkashiPresentON)
                            {
                                //①ご依頼がランダムでくるフェーズ
                                random = Random.Range(0, 100);
                                //Debug.Log("NPCご依頼イベント　抽選スタート　50以下で成功: " + random);

                                picnic_exprob = 50; //5%の確率で発生。
                                if (random <= picnic_exprob)
                                {
                                    //各NPCと酒場NPCの友好度をすべてチェックする
                                    PeopleQuestEvent();
                                }
                            }
                        }
                    }
                }

                //③ご依頼の品の事後報告のフェーズ
                if (!GameMgr.check_GirlLoveTimeEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    //ご依頼イベントチェック
                    if (!GameMgr.outgirl_Nowprogress)
                    {
                        if (PlayerStatus.player_cullent_hour >= 13 && PlayerStatus.player_cullent_hour <= 15) //15~17時
                        {
                            random = Random.Range(0, 100);
                            //Debug.Log("NPCご依頼イベント　抽選スタート　20以下で成功: " + random);

                            picnic_exprob = 20; //20%の確率で発生。
                            if (random <= picnic_exprob)
                            {
                                PeopleQuest_AfterEvent();
                            }

                        }
                    }
                }
            }



            //最後のタイミングで、決定したサブイベントの宴を再生
            if (!GameMgr.check_GirlLoveTimeEvent_flag) //サブイベント発生した
            {
                //クエスト発生
                Debug.Log("サブ時間イベントの発生");

                //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                ReadGirlLoveTimeEvent();
            }
            else //全てのイベントチェックし、発生しなかったら、このスクリプトでのイベントチェック完了
            { }
        }
    }

    void HeartEvent_check(int _lv, int _evnum, int _bgm, string _omoidename)
    {
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (PlayerStatus.girl1_Love_lv >= _lv && GameMgr.GirlLoveSubEvent_stage1[_evnum] == false)
            {
                GameMgr.GirlLoveSubEvent_num = _evnum;
                GameMgr.GirlLoveSubEvent_stage1[_evnum] = true;

                GameMgr.check_GirlLoveSubEvent_flag = false;

                if (_bgm == 1) //宴BGMに切り替え
                {
                    GameMgr.Mute_on = true;
                }

                if (_omoidename != "Non")
                {
                    GameMgr.SetHikariOmoideFlag(_omoidename, true);
                }
            }
        }
    }

    void StarEvent_check(int _starparam, int _evnum, int _bgm)
    {
        if (GameMgr.check_StarPanel_Endflag) //スターパネルチェック中かチェック前は、イベント開始しない
        { }
        else
        {
            if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                //スター10?で、お城へいけるように。手紙がくる。
                if (PlayerStatus.player_ninki_param >= _starparam && GameMgr.GirlLoveSubEvent_stage1[_evnum] == false)
                {
                    GameMgr.GirlLoveSubEvent_num = _evnum;
                    GameMgr.GirlLoveSubEvent_stage1[_evnum] = true;

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    if (_bgm == 1) //宴BGMに切り替え
                    {
                        GameMgr.Mute_on = true;
                    }
                }
            }
        }
    }

    void StarReleaseEvent_check(int _starev, int _evnum, int _bgm, string _omoidename)
    {
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.StarRank_ReleaseList[_starev] == true && GameMgr.GirlLoveSubEvent_stage1[_evnum] == false)
            {
                Debug.Log("EvDB スターのイベントの発生GameMgr.GirlLoveSubEvent_num: " + _evnum);
                GameMgr.GirlLoveSubEvent_num = _evnum;
                GameMgr.GirlLoveSubEvent_stage1[_evnum] = true;

                GameMgr.check_GirlLoveSubEvent_flag = false;

                if (_bgm == 1) //宴BGMに切り替え
                {
                    GameMgr.Mute_on = true;
                }

                if (_omoidename != "Non")
                {
                    GameMgr.SetHikariOmoideFlag(_omoidename, true);
                }
            }
        }
    }

    void Event_startcheck(int _evnum, int _bgm, bool _getemerald, bool _subheart, int _subheart_evnum)
    {
        /*if (GameMgr.check_StarPanel_Endflag) //スターパネルチェック中かチェック前は、イベント開始しない
        { }
        else
        {*/
            //メイン画面にもどったときに、イベントを発生させるフラグをON
            GameMgr.GirlLoveSubEvent_num = _evnum;
            GameMgr.GirlLoveSubEvent_stage1[_evnum] = true;

            GameMgr.check_GirlLoveSubEvent_flag = false;

            if (_bgm == 1) //宴BGMに切り替え
            {
                GameMgr.Mute_on = true;
            }
            if (_getemerald) //コスチュームアイテムのときは、ここをtrueにする。
            {
                GetEmeraldItem = true;
            }
            if (_subheart)
            {
                GameMgr.SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                GameMgr.SubEvAfterHeartGet_num = _subheart_evnum;
            }

            switch (_evnum)
            {
                case 253: //くまのおにいさんかいもうとで150点以上とったとき

                    ev_id = pitemlist.Find_eventitemdatabase("house_for_noisette_recipi");
                    pitemlist.add_eventPlayerItem(ev_id, 1); //ふたりのおうちのレシピを追加
                    break;
            }
        //}
    }




    //外出カウンタリセット　compound_mainからも読まれる。
    public void outGirlCounterReset()
    {
        random = Random.Range(0, 3);
        GameMgr.outgirl_count = 3 + random; //次の外出るイベントまでの日数カウンタ
    }

    //ヒカリが外出から帰ってくる　直接Compound_Mainからも読む
    public void OutGirlReturnHome()
    {
        GameMgr.GirlLoveSubEvent_num = 151;
        GameMgr.GirlLoveSubEvent_stage1[151] = true; //イベント初発生の分をフラグっておく。

        GameMgr.outgirl_event_ON = false;
        outGirlCounterReset(); //次の外出るイベントまでの日数カウンタ
        //GameMgr.outgirl_Nowprogress = false;

        GameMgr.outgirl_returnhome_reading_now = true;
        GameMgr.ReadGirlLoveTimeEvent_reading_now = true; //152が終わったときに、フラグもoffにする。

        PlayerStatus.player_girl_manpuku -= 30;

        //ヒカリ取得アイテムの計算
        OutGirlGetItems();

        //外にいくたびに、アイテム発見力も少し上がる。
        PlayerStatus.player_girl_findpower += 5; //20ごとに一回探索回数が増える

        StartCoroutine("eventOutGirlReturnHome_end");　//シナリオ読み終わり待ち
    }

    //
    void OutGirlReturnHome2()
    {
        GameMgr.GirlLoveSubEvent_num = 153;
        GameMgr.GirlLoveSubEvent_stage1[153] = true; //イベント初発生の分をフラグっておく。
        GameMgr.girlloveevent_bunki = 1;

        GameMgr.outgirl_event_ON = false;
        outGirlCounterReset(); //次の外出るイベントまでの日数カウンタ      
        GameMgr.outgirl_Nowprogress = false;                           

        GameMgr.ReadGirlLoveTimeEvent_reading_now = true;
        GameMgr.girl_returnhome_flag = true;
        GameMgr.girl_returnhome_num = 0;        
        
        GameMgr.girl_returnhome_endflag = true;

        StartCoroutine("HikariOkaeri");
    }

    IEnumerator HikariOkaeri()
    {
        while (GameMgr.girl_returnhome_endflag)
        {
            yield return null;
        }
        GameMgr.girl_returnhome_endflag = false;

        //ヒカリ取得アイテムの計算
        OutGirlGetItems();

        getmatplace_panel.ResultPanelOn();
        GameMgr.girl_returnhome_endflag2 = true;
        GameMgr.girl_returnhome_num = 1;

        while (GameMgr.girl_returnhome_endflag2)
        {
            yield return null;
        }
        GameMgr.girl_returnhome_endflag2 = false;

        Debug.Log("GetmatPlace 時間更新＆チェック");
        //メインシーンのデフォルトに戻る。
        //time_controller.TimeCheck_flag = true; //寝るかどうかの判定する   
        time_controller.TimeReturnHomeSleep_Status = true;
        time_controller.TimeKoushin(0, false);
        girl1_status.hukidasiOn();

        GameMgr.ReadGirlLoveTimeEvent_reading_now = false;
    }

    //GetMatPlace_Panelからも読み出し
    public void OutGirlGetItems()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        get_material = GameObject.FindWithTag("GetMaterial").GetComponent<GetMaterial>();
        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();

        getmatplace_panel.InitializeResultItemDicts();

        //採取地とアイテムの決定　今までいったことがある採取地をランダムで決定
        map_list.Clear();
        for(i=0; i < matplace_database.matplace_lists.Count; i++)
        {
            if (matplace_database.matplace_lists[i].matplaceID >= 100) //オランジーナのみ
            {
                if (matplace_database.matplace_lists[i].placeFlag == 1 && matplace_database.matplace_lists[i].placeType == 1)
                {
                    if (matplace_database.matplace_lists[i].placeHP >= 4) //冬エリア関係は、ハートLVが一定以上ないと行かない
                    {
                        if (PlayerStatus.girl1_Love_lv >= 25)
                        {
                            map_list.Add(i);
                        }
                        else { }
                    }
                    else
                    {
                        if (matplace_database.matplace_lists[i].placeHP == 3) //HP3消費する場所は、ハートLVが一定以上ないと行かない
                        {
                            if (PlayerStatus.girl1_Love_lv >= 17)
                            {
                                map_list.Add(i);
                            }
                            else { }
                        }
                        else
                        {
                            map_list.Add(i);
                        }
                    }

                }
            }
        }

        random = Random.Range(0, map_list.Count);
        get_material.OutGirlGetRandomMaterials(map_list[random]);
        Debug.Log("取ってきた場所: " + matplace_database.matplace_lists[map_list[random]].placeNameHyouji);

    }

    void PicnicEvent()
    {
        if (GameMgr.picnic_count <= 0)
        {
            GameMgr.picnic_event_ON = true;
        }

        if (GameMgr.picnic_event_ON)
        {
            random = Random.Range(0, 100);
            Debug.Log("ピクニックイベント　抽選スタート　60以下で成功: " + random);

            if (GameMgr.GirlLoveSubEvent_stage1[61])
            {
                picnic_exprob = 30; //30%の確率で発生。
            }
            else
            {
                picnic_exprob = 100; //初回は100%
            }

            if (random <= picnic_exprob)
            {
                GameMgr.GirlLoveSubEvent_num = 61;
                GameMgr.GirlLoveSubEvent_stage1[61] = true; //イベント初発生の分をフラグっておく。
                GameMgr.picnic_event_ON = false;
                GameMgr.picnic_event_reading_now = true; //ピクニックイベント発生のフラグ　宴で使用
                GameMgr.picnic_count = 5; //次のピクニックイベントまでの日数カウンタ

                GameMgr.check_GirlLoveTimeEvent_flag = false;

                GameMgr.Mute_on = true;
                GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。お菓子をあげて採点してもらう場合など。

                GameMgr.SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                GameMgr.SubEvAfterHeartGet_num = 61;
            }
        }
    }

    void CatRandomComingEvent()
    {
        cat_comecheck = false;
        GameMgr.catcoming_event_ON = false;

        if (GameMgr.catcoming_count <= 0)
        {
            cat_comecheck = true;
        }

        if (cat_comecheck)
        {
            cat_maxcount = 1;

            if (PlayerStatus.girl1_Love_lv >= 15 && PlayerStatus.girl1_Love_lv < 30)
            {
                cat_maxcount = 2;
            }
            else if (PlayerStatus.girl1_Love_lv >= 30 && PlayerStatus.girl1_Love_lv < 45)
            {
                cat_maxcount = 3;
            }
            else if (PlayerStatus.girl1_Love_lv >= 45 && PlayerStatus.girl1_Love_lv < 60)
            {
                cat_maxcount = 4;
            }
            else if (PlayerStatus.girl1_Love_lv >= 60 && PlayerStatus.girl1_Love_lv < 75)
            {
                cat_maxcount = 5;
            }
            else if (PlayerStatus.girl1_Love_lv >= 75)
            {
                cat_maxcount = 6;
            }

            if (catDataBase.catdata_list.Count >= cat_maxcount) //6匹以上いるときは、もうねこは来なくなる
            {
            }
            else
            {
                random = Random.Range(0, 100);
                Debug.Log("ねこ家くるイベント　抽選スタート　10以下で成功: " + random);

                if (GameMgr.GirlLoveSubEvent_stage1[170])
                {
                    picnic_exprob = 10; //10%の確率で発生。
                }
                else
                {
                    picnic_exprob = 100; //初回は100%
                }

                if (random <= picnic_exprob)
                {
                    GameMgr.GirlLoveSubEvent_num = 170;
                    GameMgr.GirlLoveSubEvent_stage1[170] = true; //イベント初発生の分をフラグっておく。
                                                                 //GameMgr.catcoming_event_ON = false;
                    GameMgr.catcoming_count = 10; //次の猫イベントまでの日数カウンタ
                    GameMgr.catcoming_event_ON = true;

                    GameMgr.check_GirlLoveTimeEvent_flag = false;

                    GameMgr.Mute_on = true;
                }
            }
        }
    }

    void PeopleQuestEvent()
    {
        //酒場NPC
        for(i = 0; i < GameMgr.NPC_BarFriendPoint.Length; i++)
        {
            switch(i)
            {
                case 22: //カフェモナムール

                    //Debug.Log("GameMgr.NPC_BarFriendEventProgress[i]: " + GameMgr.NPC_BarFriendEventProgress[i]);
                    //Debug.Log("GameMgr.NPC_BarFriendTimeCounter[i]: " + GameMgr.NPC_BarFriendTimeCounter[i]);
                    //Debug.Log("GameMgr.NPC_BarFriendFlag[i]: " + GameMgr.NPC_BarFriendFlag[i]);

                    //ご依頼がくるフェーズ
                    if (GameMgr.NPC_BarFriendEventProgress[i] == 0) //EventProgressはご依頼イベントの段階を表す。ご依頼[0]→お菓子渡す[1]→事後報告[2]までみたして、次のフラグへ進む。　
                    {
                        if (GameMgr.NPC_BarFriendTimeCounter[i] <= 0) //一回目に断ったりした場合、次同じ依頼がくるのはある程度時間を置いてから。
                        {
                            switch (GameMgr.NPC_BarFriendFlag[i]) //FriendFlagはそのNPCのイベント進行度合いをしめす。0, 1, 2..
                            {
                                case 0: //一個目の依頼

                                    if (GameMgr.NPC_BarFriendPoint[i] >= 53) //友好度が53以上で発生　チョコレートクエストで300点以上とるか、200点~300点で3回依頼こなした
                                    {
                                        GameMgr.NPC_BarFriendQuestEventNum[i] = 100001;
                                        PerpleQuestStartSetting(i, 800, GameMgr.NPC_BarFriendQuestEventNum[i]);

                                        _qid = quest_database.SearchQuestID(GameMgr.NPC_BarFriendQuestEventNum[i]);
                                        GameMgr.GirlLoveSubEvent_NPC_LimitDay = quest_database.questset[_qid].Quest_AfterDay;

                                        GameMgr.check_GirlLoveTimeEvent_flag = false;

                                        GameMgr.Mute_on = true;
                                    }
                                    break;

                            }
                        }
                    }
                    break;
            }
        }       
    }

    void PeopleQuest_DayCheck()
    {

        KoyuNPCQuest_OkashiTeishutuON = false;

        //受注クエストの個人依頼をみて、当日かどうかをチェックする。
        KoyuNPCQuest_OkashiTeishutuON = quest_database.CheckKojinQuest_ToDay();
        _id = quest_database.SearchKojinQuest_ToDay();

        if (KoyuNPCQuest_OkashiTeishutuON)
        {
            GameMgr.GirlLoveSubEvent_NPC_OkashiPresentON = true; //寝るとオフになる
                                                                 //当日なので、受け取りにくる。

            PerpleQuestStartSetting(quest_database.questTakeset[_id].Quest_ClientNumber, 801, quest_database.questTakeset[_id].Quest_ID); //801は依頼でお菓子を受け取りにくる会話                   
            _setjudge_num = quest_database.questTakeset[_id].GirlSetJudge_Num;
            GameMgr.GirlLoveSubEvent_NPC_score = quest_database.questTakeset[_id].GirlSetScore; //クリア条件の点数　依頼ごとに変えてもOK
        }

        //チェックし、当日だった
        if (KoyuNPCQuest_OkashiTeishutuON)
        {
            GameMgr.check_GirlLoveTimeEvent_flag = false;

            GameMgr.Mute_on = true;

            //下は、使うときだけtrueにすればOK
            GameMgr.NPC_event_ON = true; //アイテム選択画面だすときに、どのシーンで選択しているかを判定するフラグ
            GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。お菓子をあげて採点してもらう場合など。
            GameMgr.KoyuJudge_ON = true;//固有のセット判定を使う場合は、使うを宣言するフラグと、そのときのGirlLikeSetの番号も入れる。
            GameMgr.KoyuJudge_num = _setjudge_num;//GirlLikeSetの番号を直接指定 QuestDatabaseに入力し、指定する。
            GameMgr.NPC_Dislike_UseON = true; //判定時、そのお菓子の種類が合ってるかどうかのチェックもする
        }
    }

    void PeopleQuest_AfterEvent()
    {
        //酒場NPC
        for (i = 0; i < GameMgr.NPC_BarFriendPoint.Length; i++)
        {
            switch (i)
            {
                case 22: //カフェモナムール

                    //ご依頼がくるフェーズ
                    PeopleQuestAfterMethod(i);
                    
                    break;
            }
        }
    }


    void PerpleQuestStartSetting(int _num, int _status, int _progress)
    {
        GameMgr.GirlLoveSubEvent_num = _status;
        GameMgr.GirlLoveSubEvent_NPC_num = _num + 1000; //酒場NPCの場合、1000番台～　つまり1022
        GameMgr.GirlLoveSubEvent_NPC_koyunum = _num; //宴用に固有のNPC番号ももっていく。通常NPCと酒場NPCでちゃんと区別するよう注意。
        GameMgr.GirlLoveSubEvent_NPC_progress = _progress;
        GameMgr.GirlLoveSubEvent_NPC_QuestID = _progress; //クエストDBのクエストIDを指定
    }

    void PeopleQuestAfterMethod(int _num)
    {
        if (GameMgr.NPC_BarFriendEventProgress[_num] == 2) //Flagはイベント進行度を表す。ご依頼[0]→お菓子渡す[1]→事後報告[2]までみたして、次のフラグへ進む。　
        {
            if (GameMgr.NPC_BarFriendTimeCounter[_num] <= 0)
            {
                GameMgr.NPC_BarFriendEventProgress[_num] = 0; //進行度はリセット
                GameMgr.NPC_BarFriendTimeCounter[_num] = GameMgr.System_KojinNPC_Count02; //次に別の依頼がくるときまでの日数

                PerpleQuestStartSetting(_num, 802, GameMgr.NPC_BarFriendQuestEventNum[_num]);
                GameMgr.GirlLoveSubEvent_NPC_comment = GameMgr.NPC_BarFriendOkashiJudge[_num]; //クエスト　おかし渡したときにでたコメントのGameMgr.event_judge_status
                _qid = quest_database.SearchQuestID(GameMgr.NPC_BarFriendQuestEventNum[_num]);
                Debug.Log("コメントの番号: " + GameMgr.NPC_BarFriendOkashiJudge[_num]);


                //条件クリアしたので、次のご依頼ステップへ進む  //0 =まずい 1=おいしいが条件は届かず 2=クリア 100,101=おかしが違う
                if (GameMgr.NPC_BarFriendOkashiJudge[_num] == 2) 
                {
                    GameMgr.NPC_BarFriendFlag[_num]++; //ご依頼イベントが一つ進行する
                    GameMgr.NPC_BarFriendPoint[_num] += 3; //友好度も上昇

                    rnd = Random.Range(0, 300);
                    GameMgr.GirlLoveSubEvent_NPC_PrizeMoney = quest_database.questset[_qid].Quest_buy_price + rnd;
                    moneyStatus_Controller.GetMoney(GameMgr.GirlLoveSubEvent_NPC_PrizeMoney);
                }
                else if (GameMgr.NPC_BarFriendOkashiJudge[_num] == 1) //条件はクリアできてないが、お金はもらえる
                {
                    GameMgr.GirlLoveSubEvent_NPC_PrizeMoney = quest_database.questset[_qid].Quest_buy_price / 3;
                    moneyStatus_Controller.GetMoney(GameMgr.GirlLoveSubEvent_NPC_PrizeMoney);
                }

                GameMgr.check_GirlLoveTimeEvent_flag = false;

                GameMgr.Mute_on = true;
            }
        }
    }

    void ReadGirlLoveEvent()
    {
        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        compound_Main.ReadGirlLoveEvent_Fire();
    }

    void ReadGirlLoveTimeEvent()
    {
        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        compound_Main.ReadGirlLoveTimeEvent_Fire();
    }

    IEnumerator eventOutGirlReturnHome_end()
    {

        while (GameMgr.outgirl_returnhome_reading_now)
        {
            yield return null;
        }

        getmatplace_panel.ResultPanelOn();
    }

    public void eventOutGirlHomeru()
    {
        GameMgr.girlloveevent_bunki = 1; //サブイベントの発生のチェック。宴用に分岐。
        GameMgr.GirlLoveSubEvent_num = 152;
        GameMgr.GirlLoveSubEvent_stage1[152] = true; //イベント初発生の分をフラグっておく。

        getmatplace_panel.slot_view_status = 0;

        ReadGirlLoveEvent();
    }


    //
    //** イベントデータベース **//
    //
    void Heartevent_Grt()
    {
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            //モーセ家にくる
            if (PlayerStatus.girl1_Love_lv >= 10) //PlayerStatus.player_cullent_hour >= 9 && PlayerStatus.player_cullent_hour <= 12 && GameMgr.GirlLoveEvent_num >= 1
            {
                //random = Random.Range(0, 100);
                //Debug.Log("モーセくるイベント　10以下で成功: " + random);
                //if (random <= 10)
                //{
                if (!GameMgr.GirlLoveSubEvent_stage1[160]) //160番～　サブイベントNPC系　フラグ３つか５つずつぐらい余分をとっておく。
                {
                    GameMgr.GirlLoveSubEvent_num = 160;
                    GameMgr.GirlLoveSubEvent_stage1[160] = true; //イベント初発生の分をフラグっておく。

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    GameMgr.Mute_on = true;

                    //下は、使うときだけtrueにすればOK
                    GameMgr.NPC_event_ON = true; //アイテム選択画面だすときに、どのシーンで選択しているかを判定するフラグ
                    GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。お菓子をあげて採点してもらう場合など。
                    GameMgr.event_pitem_itemtype_select = "okashi";
                    GameMgr.KoyuJudge_ON = true;//固有のセット判定を使う場合は、使うを宣言するフラグと、そのときのGirlLikeSetの番号も入れる。
                    GameMgr.KoyuJudge_num = GameMgr.NPC_OkashiJudge_num[0];//GirlLikeSetの番号を直接指定
                    GameMgr.NPC_Dislike_UseON = true; //判定時、そのお菓子の種類が合ってるかどうかのチェックもする
                }
                //}
            }
        }

        //キラキラポンポン 発生すると、さらに親睦を深めて、BGMが変わる。
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (PlayerStatus.girl1_Love_lv >= 15 && GameMgr.GirlLoveSubEvent_stage1[60] == false) //4になったときのサブイベントを使う。
            {
                GameMgr.GirlLoveSubEvent_num = 60;
                GameMgr.GirlLoveSubEvent_stage1[60] = true;

                GameMgr.check_GirlLoveSubEvent_flag = false;

                GameMgr.Mute_on = true;

                GameMgr.SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                GameMgr.SubEvAfterHeartGet_num = 60;

                //イベントCG解禁
                GameMgr.SetEventCollectionFlag("event1", true);
                GameMgr.SetEventCollectionFlag("event2", true);
            }
        }



        /*
        //エクストラモードのみのイベント　どっこいステーキ
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.Story_Mode == 1)
            {
                if (PlayerStatus.girl1_Love_lv >= 40 && GameMgr.GirlLoveSubEvent_stage1[63] == false) //
                {
                    GameMgr.GirlLoveSubEvent_num = 63;
                    GameMgr.GirlLoveSubEvent_stage1[63] = true;

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    GameMgr.Mute_on = true;
                }
            }
        }

        //エクストラモードのみのイベント　すみれのお花のお菓子
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.Story_Mode == 1)
            {
                if (PlayerStatus.girl1_Love_lv >= 50 && GameMgr.GirlLoveSubEvent_stage1[62] == false) //
                {
                    GameMgr.GirlLoveSubEvent_num = 62;
                    GameMgr.GirlLoveSubEvent_stage1[62] = true;

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    GameMgr.Mute_on = true;

                    //天気も変更
                    time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day + 1, 8, 0); //次の日の朝に。
                    PlayerStatus.player_day = PlayerStatus.player_day + 1;

                    //イベントCG解禁
                    //GameMgr.SetEventCollectionFlag("event1", true);
                    //GameMgr.SetEventCollectionFlag("event2", true);
                }
            }
        }

        //エクストラモードのみのイベント　カマキリ
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.Story_Mode == 1)
            {
                if (PlayerStatus.girl1_Love_lv >= 60 && GameMgr.GirlLoveSubEvent_stage1[69] == false) //
                {
                    GameMgr.GirlLoveSubEvent_num = 69;
                    GameMgr.GirlLoveSubEvent_stage1[69] = true;

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    GameMgr.Mute_on = true;

                    //天気も変更
                    time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day + 1, 8, 0); //次の日の朝に。
                    PlayerStatus.player_day = PlayerStatus.player_day + 1;

                    //イベントCG解禁
                    //GameMgr.SetEventCollectionFlag("event1", true);
                    //GameMgr.SetEventCollectionFlag("event2", true);
                }
            }
        }

        //エクストラモードのみのイベント　わたあめ
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.Story_Mode == 1)
            {
                if (PlayerStatus.girl1_Love_lv >= 70 && GameMgr.GirlLoveSubEvent_stage1[64] == false) //
                {
                    GameMgr.GirlLoveSubEvent_num = 64;
                    GameMgr.GirlLoveSubEvent_stage1[64] = true;

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    GameMgr.Mute_on = true;

                    //天気も変更
                    time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day + 1, 8, 0); //次の日の朝に。
                    PlayerStatus.player_day = PlayerStatus.player_day + 1;
                }
            }
        }

        //エクストラモードのみのイベント　クリスタルキャッチャー
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.Story_Mode == 1)
            {
                if (PlayerStatus.girl1_Love_lv >= 80 && GameMgr.GirlLoveSubEvent_stage1[65] == false) //
                {
                    GameMgr.GirlLoveSubEvent_num = 65;
                    GameMgr.GirlLoveSubEvent_stage1[65] = true;

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    GameMgr.Mute_on = true;

                    pitemlist.addPlayerItemString("heart_jewery", 1); //ハート宝石ゲット
                    if (PlayerStatus.player_money >= 100)
                    {
                        PlayerStatus.player_money -= 100; //100ルピア消費
                    }
                    else
                    {
                        PlayerStatus.player_money = 0;
                    }

                    //天気も変更
                    time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day + 1, 8, 0); //次の日の朝に。
                    PlayerStatus.player_day = PlayerStatus.player_day + 1;
                }
            }
        }

        //エクストラモードのみのイベント　カミナリ
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.Story_Mode == 1)
            {
                if (PlayerStatus.girl1_Love_lv >= 90 && GameMgr.GirlLoveSubEvent_stage1[66] == false) //
                {
                    GameMgr.GirlLoveSubEvent_num = 66;
                    GameMgr.GirlLoveSubEvent_stage1[66] = true;

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    GameMgr.Mute_on = true;

                    //天気も変更
                    time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day + 1, 8, 0); //次の日の朝に。
                    PlayerStatus.player_day = PlayerStatus.player_day + 1;
                }
            }
        }

        //ハートレベル99 レコードをゲット
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (PlayerStatus.girl1_Love_lv >= 99 && GameMgr.GirlLoveSubEvent_stage1[67] == false) //
            {
                GameMgr.GirlLoveSubEvent_num = 67;
                GameMgr.GirlLoveSubEvent_stage1[67] = true;

                GameMgr.check_GirlLoveSubEvent_flag = false;

                GameMgr.Mute_on = true;

                //pitemlist.addPlayerItemString("Record_16", 1); //レコード
                pitemlist.addPlayerItemString("rubyDongri", 1); //るびーどんぐり
            }
        }

        
        //エクストラモードのみのイベント　ヒカリに食べたいお菓子あげた回数50回超えた　レコードゲット
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.Story_Mode == 1)
            {
                if (PlayerStatus.player_girl_eatCount_tabetai >= 50 && GameMgr.GirlLoveSubEvent_stage1[68] == false) //
                {
                    GameMgr.GirlLoveSubEvent_num = 68;
                    GameMgr.GirlLoveSubEvent_stage1[68] = true;

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    GameMgr.Mute_on = true;

                    pitemlist.addPlayerItemString("Record_17", 1); //レコード
                }
            }
        }
        //GirlLoveSubEvent_stage1 サブイベントは69まで。70~は、衣装買ったときのセリフが入っている。
        */
    }
}
