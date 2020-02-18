using UnityEngine;
using System.Collections;
using Utage;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Utage_scenario : MonoBehaviour
{

    AdvEngine Engine { get { return engine ?? (engine = FindObjectOfType<AdvEngine>()); } }
    public AdvEngine engine;
    public string scenarioLabel; //シナリオラベルを定義しておく必要がある

    private bool scenario_loading;

    private int event_ID;
    private int recipi_read_ID;
    private int itemuse_recipi_ID;
    private int map_ev_ID;

    private int girlloveev_read_ID;

    private PlayerItemList pitemlist;

    private Girl1_status girl1_status; //女の子１のステータスを取得。

    //採点結果を取得
    private int girl_kettei_item;
    private int itemLike_score;

    private int quality_score;
    private int sweat_score;
    private int bitter_score;
    private int sour_score;

    private int crispy_score;
    private int fluffy_score;
    private int smooth_score;
    private int hardness_score;
    private int jiggly_score;
    private int chewy_score;

    private int subtype1_score;
    private int subtype2_score;

    private int total_score;

    private int j;
    private string recipi_Name;

    private bool tutorial_flag;


    // Use this for initialization
    void Start()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        scenario_loading = false; //「Utage」シーンを最初に読み込むときに、falseに初期化。宴のシナリオを読み中はtrue。コルーチンのリセットを回避する。
    }

    void Update()
    {

        if (!scenario_loading) // scenario_loading=false のときは、中の処理を実行する。
        {
            //シナリオに関する処理
            if (SceneManager.GetActiveScene().name == "000_Prologue")
            { // hogehogeシーンでのみやりたい処理

                switch (GameMgr.scenario_flag)
                {
                    case 0:

                        GameMgr.scenario_flag = 1; //アップデートを繰り返さないようにする。
                        scenarioLabel = "Chapter_1";
                        StartCoroutine(Scenario_Start());
                        break;

                    default:
                        break;
                }

            }

            if (SceneManager.GetActiveScene().name == "002_Stage2")
            {

                switch (GameMgr.scenario_flag)
                {

                    case 200: //2話はじまり

                        GameMgr.scenario_flag = 201; //アップデートを繰り返さないようにする。
                        scenarioLabel = "Chapter_2";
                        StartCoroutine(Scenario_Start());
                        break;

                    case 290: //1話調合パート終了

                        break;

                    default:
                        break;
                }
                    
            }

            if (SceneManager.GetActiveScene().name == "003_Stage3")
            {

                switch (GameMgr.scenario_flag)
                {

                    case 300: //1話はじまり

                        GameMgr.scenario_flag = 301; //アップデートを繰り返さないようにする。
                        scenarioLabel = "Chapter_3";
                        StartCoroutine(Scenario_Start());
                        break;

                    case 390: //1話調合パート終了

                        break;

                    default:
                        break;
                }

            }

            //調合シーンでのテキスト処理
            if (SceneManager.GetActiveScene().name == "Compound")
            {
                switch (GameMgr.scenario_flag)
                {

                    case 110: //調合パート開始時にアトリエへ初めて入る。一番最初に工房へ来た時のセリフ。また、何を作ればよいかを指示してくれる。

                        GameMgr.scenario_flag = 111; //アップデートを繰り返さないようにする。
                        scenarioLabel = "Tutorial";
                        StartCoroutine(Tutorial_Start());
                        break;

                    case 130: //調合パート開始時にアトリエへ初めて入る。一番最初に工房へ来た時のセリフ。また、何を作ればよいかを指示してくれる。
                      
                        GameMgr.scenario_flag = 131; //アップデートを繰り返さないようにする。
                        scenarioLabel = "Chapter1_Story";                        
                        StartCoroutine(Scenario_Start());
                        break;

                    default:
                        break;
                }
                
                if (GameMgr.girlloveevent_flag == true)
                {
                    GameMgr.girlloveevent_flag = false;
                    girlloveev_read_ID = GameMgr.GirlLoveEvent_num;
                    //Debug.Log("recipi_read_ID: " + recipi_read_ID);

                    //イベントレシピを表示
                    StartCoroutine(Girllove_event_Hyouji());
                }

                if (GameMgr.recipi_read_flag == true)
                {
                    GameMgr.recipi_read_flag = false;
                    recipi_read_ID = GameMgr.recipi_read_ID;
                    //Debug.Log("recipi_read_ID: " + recipi_read_ID);

                    //イベントレシピを表示
                    StartCoroutine(Recipi_read_Hyouji());
                }

                if (GameMgr.itemuse_recipi_flag == true)
                {
                    GameMgr.itemuse_recipi_flag = false;
                    itemuse_recipi_ID = GameMgr.recipi_read_ID;

                    //イベントレシピを表示
                    StartCoroutine(ItemUse_Recipi_Hyouji());
                }

                if (GameMgr.map_event_flag == true)
                {
                    GameMgr.map_event_flag = false;
                    map_ev_ID = GameMgr.map_ev_ID;

                    //マップイベントのテキストを表示
                    StartCoroutine(MapEvent_Hyouji());
                }
            }
                
            //ガールシーンでのテキスト処理
            if (SceneManager.GetActiveScene().name == "GirlEat")
            {

                //女の子データの取得。
                //感想コメントフラグの管理も、girl1_statusで行っている。なぜか別シーンのオブジェクトの検索ができないため、
                //最初にstaticで生成するオブジェクトを介して、GirlEat_MainとUtageシーンのUtageScenarioスクリプトを連携させることにする。
                girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子


                //女の子の、お菓子に対する感想の処理
                //お菓子のスコアによって、感想を分岐する。

                if (girl1_status.girl_comment_flag == true) //GirlEatから、フラグをONにしたら、お菓子の感想前セリフを言う
                {

                    //Debug.Log(girl1_status.girl_comment_flag);
                    girl1_status.girl_comment_flag = false;

                    //感想の前に、お菓子の得点から、表示するコメントを決定する。
                    girl_kettei_item = girl1_status.girl_final_kettei_item;
                    itemLike_score = girl1_status.itemLike_score_final;

                    quality_score = girl1_status.quality_score_final;
                    sweat_score = girl1_status.sweat_score_final;
                    bitter_score = girl1_status.bitter_score_final;
                    sour_score = girl1_status.sour_score_final;

                    crispy_score = girl1_status.crispy_score_final;
                    fluffy_score = girl1_status.fluffy_score_final;
                    smooth_score = girl1_status.smooth_score_final;
                    hardness_score = girl1_status.hardness_score_final;
                    jiggly_score = girl1_status.jiggly_score_final;
                    chewy_score = girl1_status.chewy_score_final;

                    subtype1_score = girl1_status.subtype1_score_final;
                    subtype2_score = girl1_status.subtype2_score_final;

                    total_score = girl1_status.total_score_final;

                    //感想を表示
                    StartCoroutine(Girl_Comment());

                }
            }

            //ショップシーンでのテキスト処理
            if (SceneManager.GetActiveScene().name == "Shop")
            {
                switch (GameMgr.scenario_flag)
                {

                    case 120: //調合パート開始時にショップへ初めて入る。お店のアイドル娘

                        GameMgr.scenario_flag = 121;
                        GameMgr.scenario_ON = true; //これがONのときは、調合シーンの、調合ボタンなどはオフになり、シナリオを優先する。
                        scenarioLabel = "Chapter1_Shop_AtFirst";
                        StartCoroutine(Scenario_Start());
                        break;

                    default:
                        break;
                }

                if ( GameMgr.talk_flag == true )
                {

                    GameMgr.scenario_ON = true; //これがONのときは、調合シーンの、調合ボタンなどはオフになり、シナリオを優先する。
                    StartCoroutine(Shop_Talk());

                }
            }
        }
    }

    //
    // シナリオ読むだけの処理
    //
    IEnumerator Scenario_Start()
    {
        
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        engine.Param.TrySetParameter("Story_num", GameMgr.scenario_flag);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario( scenarioLabel );

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }


        switch (GameMgr.scenario_flag)
        {
            case 1:

                GameMgr.scenario_flag = 100; //プロローグ終了。一話＝100。
                break;

            /*case 111:

                GameMgr.scenario_ON = false;
                GameMgr.scenario_flag = 120;
                break;*/

            case 121:

                GameMgr.scenario_ON = false;
                GameMgr.scenario_flag = 130;
                break;

            case 131:

                GameMgr.scenario_ON = false;
                GameMgr.scenario_flag = 140;
                break;

            case 201:

                GameMgr.scenario_flag = 209;
                break;

            case 301:

                GameMgr.scenario_flag = 309;
                break;

            default:
                break;
        }
       
    }

    //
    // チュートリアルの処理
    //
    IEnumerator Tutorial_Start()
    {

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);


        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        tutorial_flag = (bool)engine.Param.GetParameter("Tutorial_Flag");

        if(tutorial_flag) //チュートリアルを見る場合
        {
            StartCoroutine(Tutorial_Start_Content());
        }
        else //見ない場合
        {
            switch (GameMgr.scenario_flag)
            {

                case 111:

                    GameMgr.scenario_ON = false;
                    GameMgr.scenario_flag = 120;
                    break;

                default:
                    break;
            }
        }        

    }

    //
    // チュートリアル開始した場合の中身の処理
    //
    IEnumerator Tutorial_Start_Content()
    {

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        //はいを押した時の処理。エクストリームパネルを表示する。コンテントも表示してもいいかもだけど、触れないようにしておく。
        GameMgr.tutorial_ON = true;
        GameMgr.tutorial_Num = 0;

        //「宴」のシナリオを呼び出す
        scenarioLabel = "Tutorial_Content";
        Engine.JumpScenario(scenarioLabel);


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }

        //ゲームの再開処理を書く
        GameMgr.tutorial_Num = 10;

        while (!GameMgr.tutorial_Progress) //エクストリームパネルを押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 30;

        while (!GameMgr.tutorial_Progress) //右のレシピメモ開き押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();



        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 50;

        while (!GameMgr.tutorial_Progress) //調合完了待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;


        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 70;

        while (!GameMgr.tutorial_Progress) //レシピ閃いた！ボタンの押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 90;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 100;

        while (!GameMgr.tutorial_Progress) //あげるボタンの押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();





        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        switch (GameMgr.scenario_flag)
        {

            case 111:

                GameMgr.scenario_ON = false;
                GameMgr.scenario_flag = 120;
                GameMgr.tutorial_ON = false;
                break;

            default:
                break;
        }
    }

    //
    // 現状使用しない（シナリオ内でアイテムを獲得するスクリプト）
    //
    /*
    IEnumerator Chapter1_Start()
    {
        GameMgr.scenario_flag = 201; //アップデートを繰り返さないようにする。

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Chapter1_1_old";
   

        engine.Param.TrySetParameter("Kaeru_Coin", PlayerStatus.player_kaeru_coin);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);       

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        PlayerStatus.player_kaeru_coin = (int)engine.Param.GetParameter("Kaeru_Coin");

        pitemlist.add_eventPlayerItem(0, 1); //オレンジクッキーのレシピを追加

        GameMgr.scenario_flag = 209; //110 最初の調合パートの開始 109で一度シーンを読み込み開始し、読み終えると110になる。

    }*/


    //
    // 女の子の感想・コメント
    //
    IEnumerator Girl_Comment()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Girl_Comment1"; //まずは、食べたときの前セリフ

        scenario_loading = true;

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }

        //一呼吸おき、反応する処理。キャラクタの表情が変わったり、エフェクトがキラキラとか。
        //ゲーム側の処理を挟む。

        //感想を述べる処理へ。
        engine.ResumeScenario();

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        girl1_status.girl_comment_endflag = true; //感想を言い終えたフラグ。

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }


    //
    // イベントレシピ表示
    //
    IEnumerator Recipi_read_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Recipi_read"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定

        //event_IDから、レシピの名前を検索
        j = 0;
        while (j < pitemlist.eventitemlist.Count)
        {
            if (recipi_read_ID == pitemlist.eventitemlist[j].ev_ItemID)
            {
                recipi_Name = pitemlist.eventitemlist[j].event_itemName;
                break;
            }
            j++;
        }

        switch (recipi_Name)
        {
            case "najya_start_recipi":

                engine.Param.TrySetParameter("Re_flag1", true);
                break;

            case "cookie_base_recipi":

                engine.Param.TrySetParameter("Re_flag2", true);
                break;

            case "ice_cream_recipi":

                engine.Param.TrySetParameter("Re_flag3", true);
                break;

            case "financier_recipi":

                engine.Param.TrySetParameter("Re_flag4", true);
                break;

            default:
                break;
        }
      

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        //オレンジクッキーのレシピをはじめて読みおえた
        if (GameMgr.scenario_flag == 115)
        {
            if (event_ID == 0)
            {
                GameMgr.scenario_flag = 120; //レシピを読み終えた
            }
        }

        GameMgr.recipi_read_endflag = true; //レシピを読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // レシピリストから使用したときの、イベントレシピ表示
    //
    IEnumerator ItemUse_Recipi_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Event_Recipi"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定

        //event_IDから、レシピの名前を検索
        j = 0;
        while (j < pitemlist.eventitemlist.Count)
        {
            if (itemuse_recipi_ID == pitemlist.eventitemlist[j].ev_ItemID)
            {
                recipi_Name = pitemlist.eventitemlist[j].event_itemName;
                break;
            }
            j++;
        }

        switch (recipi_Name)
        {
            case "ev00_orange_cookie_recipi":

                engine.Param.TrySetParameter("Ev_flag1", true);
                break;

            case "ev01_neko_cookie_recipi":

                engine.Param.TrySetParameter("Ev_flag2", true);
                break;

            case "ev02_orangeneko_cookie_memo":

                engine.Param.TrySetParameter("Ev_flag3", true);
                break;

            case "financier_recipi":

                engine.Param.TrySetParameter("Ev_flag4", true);
                break;

            default:
                break;
        }


        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = true; //レシピを読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // 好感度イベント
    //
    IEnumerator Girllove_event_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "GirlLove_Event"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("Girllove_event_num", girlloveev_read_ID);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }


        GameMgr.girlloveevent_endflag = true; //レシピを読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // マップイベント表示
    //
    IEnumerator MapEvent_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "MapEvent"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定

        switch (map_ev_ID)
        {
            case 1:

                engine.Param.TrySetParameter("MapEv_num", 1);
                break;

            default:
                break;
        }


        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = true; //レシピを読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // ショップの「話す」コマンド
    //
    IEnumerator Shop_Talk()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Shop_Talk"; //ショップ話すタグのシナリオを再生。

        GameMgr.talk_flag = false; // アップデートの更新を止める。
        scenario_loading = true;

        //ここで、宴で呼び出したいイベント番号を設定する。
        engine.Param.TrySetParameter("Shop_Talk_Num", GameMgr.talk_number);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

        
        GameMgr.scenario_ON = false;

    }
}