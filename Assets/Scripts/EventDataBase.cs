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

    private int i, random;
    private int picnic_exprob;
    private int ev_id;
    private bool _fire;
    private string _basename, _baseitemtype_sub, _baseitemtype_subB;

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


                                    special_quest.SetSpecialOkashi(40, 0);
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
            ReturnHome_check(10, false); //酒場はじめていって帰ってきた
            ReturnHome_check(20, false); //牧場はじめていって帰ってきた
            ReturnHome_check(30, true); //コンテストはじめていって帰ってきた            
            ReturnHome_check(110, false); //ミラボ先生にはじめて会って帰ってきた

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

    void ReturnHome_check(int _num, bool utagebgm_ON) //二個目は宴のBGMをonにする。
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
                HeartEvent_check(GameMgr.System_HeartLVevent_01, 301, 1); //ヒカリお菓子作る

                //LV3ごとに発生するイベント
                /*HeartEvent_check(3, 350, 1);
                HeartEvent_check(6, 351, 1);
                //HeartEvent_check(9, 352, 1); ヒカリお菓子作るとLV被るので、off
                HeartEvent_check(12, 353, 1);
                HeartEvent_check(15, 354, 1);
                HeartEvent_check(18, 355, 1);
                HeartEvent_check(21, 356, 1);
                HeartEvent_check(24, 357, 1);
                HeartEvent_check(27, 358, 1);
                HeartEvent_check(30, 359, 1);*/

                //Heartevent_Grt(); //１の頃のイベント


                //
                //スターで発生するイベント系
                //
                StarEvent_check(GameMgr.System_StarBlockLv_04, 500, 1); //スター10で、お城へいけるように。手紙がくる。


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
                            Event_startcheck(80, 0, false, false);
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
                            Event_startcheck(82, 1, false, false);
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
                                Event_startcheck(83, 1, false, false);
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
                                Event_startcheck(85, 1, false, false);
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
                                        Event_startcheck(85, 1, false, false);
                                    }
                                }
                            }
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
                                        Event_startcheck(70, 1, true, true);
                                    }
                                    break;

                                case "Sukumizu_Costume":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[71])
                                    {
                                        Event_startcheck(71, 1, true, true);
                                    }
                                    break;

                                case "Meid_Black_Costume":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[72])
                                    {
                                        Event_startcheck(72, 1, true, true);
                                    }
                                    break;

                                case "PinkGoth_Costume":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[73])
                                    {
                                        Event_startcheck(73, 1, true, true);
                                    }
                                    break;

                                case "RedDress_Costume":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[74])
                                    {
                                        Event_startcheck(74, 1, true, true);
                                    }
                                    break;

                                case "BalloonHat_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[75])
                                    {
                                        Event_startcheck(75, 1, true, true);
                                    }
                                    break;

                                case "AngelWing_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[76])
                                    {
                                        Event_startcheck(76, 1, true, true);
                                    }
                                    break;

                                case "Nekomimi_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[77])
                                    {
                                        Event_startcheck(77, 1, true, true);
                                    }
                                    break;

                                case "FlowerHairpin_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[78])
                                    {
                                        Event_startcheck(78, 1, true, true);
                                    }
                                    break;

                                case "TwincleStarDust_Acce":

                                    if (!GameMgr.GirlLoveSubEvent_stage1[79])
                                    {
                                        Event_startcheck(79, 1, true, true);
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


                //置物や土産を買った 100番台～
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (!GameMgr.GirlLoveSubEvent_stage1[100])
                    {
                        if (pitemlist.KosuCount("kuma_nuigurumi") >= 1)
                        {
                            Event_startcheck(100, 1, false, true);
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
                        Event_startcheck(101, 1, false, false);

                        ev_id = pitemlist.Find_eventitemdatabase("silver_neko_cookie_recipi");
                        pitemlist.add_eventPlayerItem(ev_id, 1); //銀のねこクッキーのレシピを追加
                    }
                }

                /*
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

                //調合後にチェック　はじめて、各特別なお菓子作ったイベント
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
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
                                Event_startcheck(GameMgr.OkashiAtFirst_eventlist[_basename], 0, false, false);
                                break;
                            }                            
                        }

                        if (_baseitemtype_sub == items)
                        {
                            if (!GameMgr.GirlLoveSubEvent_stage1[GameMgr.OkashiAtFirst_eventlist[_baseitemtype_sub]])
                            {
                                Event_startcheck(GameMgr.OkashiAtFirst_eventlist[_baseitemtype_sub], 0, false, false);
                                break;
                            }                           
                        }

                        if (_baseitemtype_subB == items)
                        {
                            if (!GameMgr.GirlLoveSubEvent_stage1[GameMgr.OkashiAtFirst_eventlist[_baseitemtype_subB]])
                            {
                                Event_startcheck(GameMgr.OkashiAtFirst_eventlist[_baseitemtype_subB], 0, false, false);
                                break;
                            }                           
                        }
                    }

                }

                //食べた後にチェック　１５０点以上で特別なイベント
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.SpecialSubevent_EatAfterflag) //
                    {
                        GameMgr.SpecialSubevent_EatAfterflag = false;

                        Event_startcheck(GameMgr.SpecialSubevent_Num, 1, false, false);
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
                            Debug.Log("チェック　本日が１０・２０・３０日かどうか");
                            Debug.Log("本日の日: " + PlayerStatus.player_cullent_day); 

                            //10日ごとチェックバージョン
                            if(PlayerStatus.player_cullent_day % GameMgr.System_Yachin_Day == 0)
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
                                                GameMgr.GirlLoveSubEvent_num = 1110;
                                                GameMgr.GirlTalk_num = 11;

                                                GameMgr.yachin_otetsuki_count++; //お手付き　２回たまるとゲームオーバー
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
                                                                
                                GameMgr.check_GirlLoveSubEvent_flag = false;

                                GameMgr.Mute_on = true;
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

                            /*if (!GameMgr.NPCMagic_eventList[0]) //コンテスト終了後、一回寝て起きる。露店通りへいく
                            {
                                matplace_database.ReSetMapFlagString("Or_Hiroba1_Roten", 1);

                                GameMgr.NPCMagic_eventList[0] = true;

                                if (!GameMgr.Contest_Cookie_VictoryHoleinOne)
                                {
                                    GameMgr.GirlLoveSubEvent_num = 2000;
                                    GameMgr.Utage_MapMoveON = true;
                                }
                                else
                                {
                                    GameMgr.GirlLoveSubEvent_num = 2001; //初回出場でいきなり優勝した
                                    GameMgr.Utage_MapMoveON = false;
                                }

                                GameMgr.Contest_Cookie_VictoryHoleinOne = false;
                                
                                GameMgr.Mute_on = true;

                                GameMgr.check_GirlLoveSubEvent_flag = false;
                                GameMgr.Contest_afterHomeHeartUpFlag = false; //大き目イベントが発生したときは、ハートアップイベントを中止。
                            }*/                            
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

                            if (GameMgr.contest_Disqualification || GameMgr.contest_Disqualification2) //失格だった場合はこっち
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
                                GameMgr.GirlLoveSubEvent_num = 1200;
                                GameMgr.SubEvAfterHeartGet_num = 200;
                            }
                            
                            
                            //GameMgr.Mute_on = true;

                            GameMgr.SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                            
                            GameMgr.check_GirlLoveSubEvent_flag = false;
                            GameMgr.contest_Disqualification = false;
                            GameMgr.contest_Disqualification2 = false;
                        }
                    }
                }

                //寝て起きた後、NPCがくるイベント
                if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
                { }
                else
                {
                    if (GameMgr.check_SleepEnd_Eventflag[4]) //ねておきたあとにチェック
                    {
                        GameMgr.check_SleepEnd_Eventflag[4] = false;
                        Debug.Log("コンテスト終了後　NPCがくるチェック");

                        //コンテスト初出場し、ミラボ先生にあった後から、発生する
                        if (GameMgr.NPCMagic_eventList[0])
                        {
                            if (conteststartList_database.ReturnVictoryCount(1) >= 1) //一位のトータル取得数をゲット
                            {
                                //一位を一回以上取った場合、アマクサが初優勝時にほめてくれるイベント発生
                                if (!GameMgr.NPCHiroba_eventList[1031])
                                {
                                    GameMgr.NPCHiroba_eventList[1031] = true;

                                    GameMgr.GirlLoveSubEvent_num = 3000;
                                    GameMgr.check_GirlLoveSubEvent_flag = false;
                                    GameMgr.Mute_on = true;

                                    //アマノシャンメリーと白紙メモくれる
                                    pitemlist.addPlayerItemString("amano_champmery", 1);
                                    pitemlist.add_eventPlayerItemString("MemoWhite", 1);                                    
                                }
                            }
                        }
                    }
                }

                //
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

    //通常サブイベントとは別で、時間で発生するイベント
    public void GirlLove_SubTimeEventMethod()
    {
        GameMgr.girlloveevent_bunki = 1; //サブイベントの発生のチェック。宴用に分岐。

        if (GameMgr.GirlLove_loading)
        { }
        else
        {
            GameMgr.check_GirlLoveTimeEvent_flag = true;

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
            if (!GameMgr.check_GirlLoveTimeEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                //ピクニックイベントチェック
                if (!GameMgr.outgirl_Nowprogress)
                {
                    if (GameMgr.PicnicSkipFlag) { } //ピクニックスキップON
                    else
                    {
                        //クレープ以降　一回目は必ず発生               
                        if (PlayerStatus.player_cullent_hour >= 12 && PlayerStatus.player_cullent_hour <= 14
                        && GameMgr.GirlLoveEvent_num >= 20) //12時から15時の間に、サイコロふる
                        {
                            PicnicEvent();
                        }
                    }
                }
            }

            //最後エデンのイベント
            if (!GameMgr.check_GirlLoveTimeEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                //エデンイベントチェック
                if (!GameMgr.outgirl_Nowprogress)
                {
                    //一回目食べている
                    if (GameMgr.GirlLoveSubEvent_stage1[600])
                    {
                        //くじらさんから満月の夜を聞いていた
                        if (GameMgr.NPCHiroba_eventList[271])
                        {
                            if (PlayerStatus.player_cullent_month == GameMgr.System_Fullmoon_month &&
                               PlayerStatus.player_cullent_day == GameMgr.System_Fullmoon_day &&
                               PlayerStatus.player_cullent_hour >= GameMgr.NightDay_hour)
                            {
                                if (pitemlist.KosuCount("Eden") >= 1)
                                {
                                    GameMgr.ending_on = true;

                                    //満月の夜にエデン持っているのでED
                                    GameMgr.girlloveevent_bunki = 2;
                                    GameMgr.GirlLoveEvent_num = 100;
                                    GameMgr.girlEat_ON = false;
                                    GameMgr.Mute_on = true;
                                    GameMgr.Utage_MapMoveON = true; //EDシーンへマップ移動もするのでtrue
                                    GameMgr.ending_number = 1;

                                    GameMgr.check_GirlLoveTimeEvent_flag = false;
                                }
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

    void HeartEvent_check(int _lv, int _evnum, int _bgm)
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

                switch(_evnum)
                {
                    case 350: //りんごのハンカチゲット

                        pitemlist.addPlayerItemString("crepe_powerup4", 1);
                        break;
                }
            }
        }
    }

    void StarEvent_check(int _lv, int _evnum, int _bgm)
    {
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            //スター10?で、お城へいけるように。手紙がくる。
            if (PlayerStatus.player_ninki_param >= _lv && GameMgr.GirlLoveSubEvent_stage1[_evnum] == false)
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

    void Event_startcheck(int _evnum, int _bgm, bool _getemerald, bool _subheart)
    {
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
            GameMgr.SubEvAfterHeartGet_num = _evnum;
        }
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
            if(matplace_database.matplace_lists[i].placeFlag == 1 && matplace_database.matplace_lists[i].placeType == 1)
            {
                map_list.Add(i);
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
                picnic_exprob = 60; //60%の確率で発生。
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
                GameMgr.picnic_count = 3; //次のピクニックイベントまでの日数カウンタ

                GameMgr.check_GirlLoveTimeEvent_flag = false;
                /*if (GameMgr.Story_Mode == 0)
                {
                    GameMgr.check_GirlLoveSubEvent_flag = false;
                }
                else
                {
                    GameMgr.check_GirlLoveTimeEvent_flag = false;
                }*/

                GameMgr.Mute_on = true;
                GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。お菓子をあげて採点してもらう場合など。

                GameMgr.SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                GameMgr.SubEvAfterHeartGet_num = 61;
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

        //エクストラモードのみのイベント　ハートレベル99 レコードをゲット
        if (!GameMgr.check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
        { }
        else
        {
            if (GameMgr.Story_Mode == 1)
            {
                if (PlayerStatus.girl1_Love_lv >= 99 && GameMgr.GirlLoveSubEvent_stage1[67] == false) //
                {
                    GameMgr.GirlLoveSubEvent_num = 67;
                    GameMgr.GirlLoveSubEvent_stage1[67] = true;

                    GameMgr.check_GirlLoveSubEvent_flag = false;

                    GameMgr.Mute_on = true;

                    pitemlist.addPlayerItemString("Record_16", 1); //レコード
                    pitemlist.addPlayerItemString("rubyDongri", 1); //るびーどんぐり
                }
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
