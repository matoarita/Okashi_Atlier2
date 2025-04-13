using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StarStampPanel : MonoBehaviour
{
    private List<Vector3> dot_pos = new List<Vector3>();
    private List<Vector3> chara_pos = new List<Vector3>();

    private GameObject canvas;

    private GameObject character_obj;
    private GameObject close_button_obj;

    private GameObject compound_main_obj;
    private Compound_Main compound_main;

    private SoundController sc;
    private ItemMatPlaceDataBase matplace_database;
    private ItemDataBase database;
    private PlayerItemList pitemlist;

    private GameObject newAreaRelease_Panel;
    private string newarea_gohoubitext;
    private string newarea_titletext;
    private Sprite newarea_gohoubiicon;

    private Sprite itemIcon_sprite1;
    private Sprite itemIcon_sprite2;

    private Text star_hyoujiparam;

    private int _before_ninki;
    private int goal_star;
    private bool ButtonON;
    private bool InitCheck;
    private bool starrank_Release_ON;
    private bool starevent_endcheck;

    private GameObject sugoroku_board;
    private GameObject dotevent_obj;
    private GameObject effect_obj;

    private int newarea_num;
    private int newarea_star;

    private int _chara_temp_star; //移動用
    private bool chara_moving;
    private bool chara_move_on; //キャラ移動中

    private int _id;
    private int count;

    private float move_time = 0.7f; //１マスの移動時間


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //初期設定完了後、各スターをチェックしていく。
        if(InitCheck)
        {
            if (!starevent_endcheck) //スターイベントエンドをふむまでは、checkは何度でも見る。
            {
                if (starrank_Release_ON) //スターイベント確認中
                { }
                else
                {
                    if (chara_move_on) //キャラ移動中
                    { }
                    else
                    {
                        starrank_Release_ON = false;

                        //ここに入れた数だけ全てチェックしていく　0が目標スター、1がリリースフラグイベントの番号
                        if (_chara_temp_star >= GameMgr.System_StampStarMax)
                        {
                            //ゴールについているのでチェック終了
                        }
                        else
                        {
                            foreach (var keyValuePair in GameMgr.Star_Eventlist)
                            {
                                star_ReleaseEventCheck(keyValuePair.Key, keyValuePair.Value);
                            }
                        }

                        if (!starrank_Release_ON) //falseのまま、最後にここにくれば、発生イベント全チェック完了ということになる。
                        {
                            Debug.Log("スターパネルイベント全てチェック完了");
                            if (_chara_temp_star >= GameMgr.System_StampStarMax)
                            {
                                //ゴールについているので移動も完了
                                EndStarEvent();
                            }
                            else
                            {
                                //現在のスターと、今いる位置をさらに比較して、まだ移動が残ってたら、そこを移動する
                                if (PlayerStatus.player_ninki_param >= GameMgr.System_StampStarMax)
                                {
                                    goal_star = GameMgr.System_StampStarMax;
                                }
                                else
                                {
                                    goal_star = PlayerStatus.player_ninki_param;
                                }

                                if (goal_star - _chara_temp_star > 0)
                                {
                                    //キャラ移動アニメを開始
                                    Debug.Log("キャラ移動開始 status=100");
                                    count = 0;
                                    StartCoroutine(Character_Move(goal_star, 100));
                                }
                                else
                                {
                                    EndStarEvent();
                                }
                            }
                        }
                    }
                }
            }
        }

        //現在の★数を表示
        star_hyoujiparam.text = _chara_temp_star.ToString();
    }

    void InitSetting()
    {
        InitCheck = false;
        starrank_Release_ON = false;
        chara_move_on = false;
        starevent_endcheck = false;
        count = 0;

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        itemIcon_sprite1 = Resources.Load<Sprite>("Sprites/Icon/" + "treasure_extra1");
        itemIcon_sprite2 = Resources.Load<Sprite>("Sprites/Icon/" + "Book01");

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        character_obj = this.transform.Find("PanelPos/Stamprally/pos/SugorokuBoard/CharacterPanel").gameObject;
        close_button_obj = this.transform.Find("CloseButton").gameObject;
        close_button_obj.SetActive(false);

        compound_main_obj = GameObject.FindWithTag("Compound_Main");
        compound_main = compound_main_obj.GetComponent<Compound_Main>();       

        newAreaRelease_Panel = canvas.transform.Find("NewAreaReleasePanel").gameObject;
        newAreaRelease_Panel.SetActive(false);

        star_hyoujiparam = this.transform.Find("PanelPos/StarParamPanel/StarParamText").GetComponent<Text>();

        sugoroku_board = this.transform.Find("PanelPos/Stamprally/pos/SugorokuBoard").gameObject;

        effect_obj = this.transform.Find("Effect").gameObject;
        effect_obj.SetActive(false);

        dot_pos.Clear();
        foreach(Transform child in this.transform.Find("PanelPos/Stamprally/pos/SugorokuBoard").transform)
        {
            if (child.name == "CharacterPanel") //キャラの座標はとらない
            {

            }
            else
            {
                dot_pos.Add(child.gameObject.transform.localPosition); //0~から、各ドットのローカルの位置座標が入っている。
                //Debug.Log("dot_pos: " + dot_pos[dot_pos.Count-1]);
            }
        }

        if(GameMgr.Before_Player_ninkiparam >= GameMgr.System_StampStarMax) //ボードのこま上限
        {
            _before_ninki = GameMgr.System_StampStarMax;
        }
        else if (GameMgr.Before_Player_ninkiparam < 0)
        {
            _before_ninki = 0;
        }
        else
        {
            _before_ninki = GameMgr.Before_Player_ninkiparam;
        }

        //おたからの状態　初期
        foreach (var keyValuePair in GameMgr.Star_Eventlist)
        {
            NewAreaKaikin_Library(keyValuePair.Value, keyValuePair.Key, 0);
        }

        Debug.Log("_before_ninki: " + _before_ninki);
        Debug.Log("現在の人気: " + PlayerStatus.player_ninki_param);
        character_obj.transform.localPosition = dot_pos[_before_ninki];
        _chara_temp_star = _before_ninki;

        star_hyoujiparam.text = _chara_temp_star.ToString();

        InitCheck = true;       
    }

    private void OnEnable()
    {
        InitSetting();
        
    }

    

    void star_ReleaseEventCheck(int _star, int _evnum)
    {
        if (starrank_Release_ON) //スターイベント発生したら、被らないようにする。
        { }
        else
        {
            if (_star <= PlayerStatus.player_ninki_param && GameMgr.StarRank_ReleaseList[_evnum] == false) //スター目標の数とった
            {
                Debug.Log("スターイベント発生");

                //スターイベント発生　特有のほっこりイベントやコスチュームをゲット
                GameMgr.StarRank_ReleaseList[_evnum] = true;
                newarea_num = _evnum;
                newarea_star = _star; //移動先のスターになる。

                //発生したので、該当のスターの位置までキャラが移動してから、アイテム獲得のパネルを表示
                starrank_Release_ON = true;

                //キャラ移動アニメを開始
                count = 0;
                StartCoroutine(Character_Move(newarea_star, 1));
            }
            else
            {
                //starrank_Release_ON = false;
            }
        }
        
    }

    IEnumerator Character_Move(int _movestar, int _status)
    {
        //歩きスタート

        count++;
        //Debug.Log("Character_Move読み込み回数: " + count);
        Debug.Log("現在の座標のstar: " + _chara_temp_star);
        Debug.Log("移動先のstar: " + _movestar);
        Debug.Log("_status: " + _status);
        //Debug.Log("dot_posの座標: " + dot_pos[_movestar]);
        chara_moving = false;
        chara_move_on = true; //移動中

        StartCoroutine("walksound");
        character_obj.transform.DOLocalMove(dot_pos[_chara_temp_star+1], move_time).OnComplete(EndMoveMethod); //マスを一個移動　1秒で移動

        while (!chara_moving) //目標starに到達するまで、マスを一個ずつ動いていく処理
        {
            yield return null;
        }

        _chara_temp_star++;

        //移動先が目標の場所であれば、そこでイベント発生か移動の終了
        if(_chara_temp_star >= _movestar)
        {
            Debug.Log("スターパネルイベントマス到着 " + _movestar);
            Debug.Log("newarea_star: " + newarea_star);
            Debug.Log("イベント発生時の座標のstar: " + _chara_temp_star);

            //イベントを発生
            chara_move_on = false;
            _chara_temp_star = _movestar; //発生したスターイベントの位置

            //スター発生イベントマスに到達したので、アイテム画面をだす
            if (_status == 1)
            {
                OnNewAreaReleasePanel();
            }
            else if (_status == 100) //スターイベント発生せず、残りのマスの移動も完了したので、スタースタンプラリー画面の終了
            {
                EndStarEvent();
                
            }
        }else
        {
            //まだ移動を続ける
            StartCoroutine(Character_Move(_movestar, _status));
        }
       
    }

    void EndStarEvent()
    {
        starevent_endcheck = true;

        //falseのままなら、チェック終了
        //GameMgr.Before_Patissier_Rank = PlayerStatus.player_patissier_Rank;
        GameMgr.Before_Player_ninkiparam = PlayerStatus.player_ninki_param;

        //終わったら再開
        //girl1_status.GirlEat_Judge_on = true;        

        Debug.Log("スタースタンプラリー　全てチェック完了");

        StartCoroutine("TimeWait");
    }

    IEnumerator walksound()
    {
        yield return new WaitForSeconds(move_time-0.23f); //
        sc.PlaySe(208);
    }

    IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(1.0f); //1秒待つ

        close_button_obj.SetActive(true);
        ButtonON = true;
    }

    void EndMoveMethod()
    {
        //1マス移動した後        
        chara_moving = true;
    }

    //StarStampパネルから、アイテム獲得画面の呼び出し
    void OnNewAreaReleasePanel()
    {
        //パネルを実際に表示し、ボタン押すまで表示
        StartCoroutine("GetPanel_StartEffect");
    }

    //①まず、シュイイイインでための演出
    IEnumerator GetPanel_StartEffect()
    {
        //ピタ
        yield return new WaitForSeconds(0.5f); //1秒待つ

        //シュイイイイン

        //ちょっと画面暗く
        //音を鳴らす
        sc.PlaySe(72); //209
        effect_obj.SetActive(true);
        yield return new WaitForSeconds(3.0f); //1秒待つ

        //パネルを実際に表示し、ボタン押すまで表示
        sc.StopSe();
        effect_obj.SetActive(false);
        StartCoroutine("GetPanel_Hyouji");
    }

    //②ドギュン！　エフェクトと共に、パネルを表示する。
    IEnumerator GetPanel_Hyouji()
    {
        //NewAreaCheck_loading = true; //解禁パネル表示中のフラグ

        /* スターフラグ解禁処理 */
        GameMgr.newarea_read_endflag = false;
        newAreaRelease_Panel.SetActive(true); //ボタンおすまではパネルが表示される
        NewAreaKaikin_Library(newarea_num, newarea_star, 1); //フラグの解禁項目をチェックし、その後パネルの表示用オブジェクトに更新する。
        Debug.Log("新エリア解禁: " + "☆" + newarea_star + " 解禁フラグ" + newarea_num + " を読んだ");

        while (!GameMgr.newarea_read_endflag) //trueになるまではここで待つ　NewAreaReleasePanelのボタン,またはそのイベント終了後でtrueになる
        {
            yield return null;
        }

        //NewAreaCheck_loading = false;

        //アイテム獲得画面を閉じた
        //まだ移動が必要かどうかに処理を戻す。
        starrank_Release_ON = false;

        //おたからの状態　画面更新
        foreach (var keyValuePair in GameMgr.Star_Eventlist)
        {
            NewAreaKaikin_Library(keyValuePair.Value, keyValuePair.Key, 0);
        }
    }
   

    public void OnCloseButton()
    {
        if (ButtonON)
        {
            Debug.Log("スタースタンプラリー　パネル閉じた");
            
            compound_main.EndStarReleaseCheck();
            this.gameObject.SetActive(false);
        }
    }



    void NewAreaKaikin_Library(int _num, int _star, int _mstatus)
    {
        switch (_num)
        {
            case 0: //おたから

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。
                {
                    if(GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev1", 1);
                    }
                    else
                    {
                        TreasureStatus("ev1", 0);
                    }
                }
                else if (_mstatus == 1)
                {
                    //アクアマリンの湖
                    //_id = matplace_database.SearchMapString("Aquamarine_Lake");
                    newarea_titletext = "おたから";
                    newarea_gohoubitext = "３つの中から好きなアイテムを選んでね。";
                    newarea_gohoubiicon = itemIcon_sprite1;
                    newAreaRelease_panelKoushin(_star);
                }

                break;

            case 1: //思い出イベント

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。変化がないのもあり。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev2", 2);
                    }
                    else
                    {
                        TreasureStatus("ev2", 0);
                    }
                }
                else if (_mstatus == 1)
                {
                    //ショートケーキの思い出　仮
                    _id = pitemlist.Find_eventitemdatabase("strawberry_sponge_cake_recipi");
                    newarea_titletext = "レシピ";
                    newarea_gohoubitext = "ショートケーキのレシピ！" + "\n" + "ゲット！";
                    newarea_gohoubiicon = pitemlist.eventitemlist[_id].itemIcon_sprite;
                    newAreaRelease_panelKoushin(_star);

                    pitemlist.add_eventPlayerItem(_id, 1);
                }
                break;

            case 2: //コスチュームゲット

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。変化がないのもあり。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev3", 3);
                    }
                    else
                    {
                        TreasureStatus("ev3", 0); //コスチュームはアイコン見た目変わらない
                    }
                }
                else if (_mstatus == 1)
                {
                    _id = pitemlist.SearchEmeraldItemStringID("PinkGoth_Costume");
                    newarea_titletext = "コスチューム";
                    newarea_gohoubitext = "コスチューム１をゲット！！";
                    newarea_gohoubiicon = pitemlist.emeralditemlist[_id].itemIcon_sprite;
                    newAreaRelease_panelKoushin(_star);

                    pitemlist.add_EmeraldPlayerItem(_id, 1);
                    //matplace_database.matPlaceKaikin("Amber_Lake");
                }
                break;

            case 3: //ソーダアイランド解放

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。変化がないのもあり。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev4", 0);
                    }
                    else
                    {
                        TreasureStatus("ev4", 0); //コスチュームはアイコン見た目変わらない
                    }
                }
                else if (_mstatus == 1)
                {
                    //ソーダアイランド解放
                    _id = matplace_database.SearchMapString("Or_Hiroba_Summer_SodaIsland");
                    newarea_titletext = "特別な場所";
                    newarea_gohoubitext = matplace_database.matplace_lists[_id].placeNameHyouji + "\n" + "解放！";
                    newarea_gohoubiicon = matplace_database.matplace_lists[_id].mapIcon_sprite;
                    newAreaRelease_panelKoushin(_star);

                    matplace_database.ReSetMapFlagString("Or_Hiroba_Summer_SodaIsland", 1);
                }
                break;

            case 4: //思い出イベント

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。変化がないのもあり。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev5", 0);
                    }
                    else
                    {
                        TreasureStatus("ev5", 0); //アイコン見た目変わらない
                    }
                }
                else if (_mstatus == 1)
                {
                    //ショートケーキの思い出　仮
                    _id = matplace_database.SearchMapString("Emerald_Forest");
                    newarea_titletext = "休憩イベント";
                    newarea_gohoubitext = "街へ買い出しに行こう！" + "\n" + "解放！";
                    newarea_gohoubiicon = matplace_database.matplace_lists[_id].mapIcon_sprite;
                    newAreaRelease_panelKoushin(_star);

                    //GameMgr.SetHikariOmoideFlag("strawberry_sponge_cake", true);
                }
                break;

            case 5: //思い出イベント いっしょにおふろ♪

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。変化がないのもあり。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev6", 0);
                    }
                    else
                    {
                        TreasureStatus("ev6", 0); //アイコン見た目変わらない
                    }
                }
                else if (_mstatus == 1)
                {
                    //ショートケーキの思い出　仮
                    _id = matplace_database.SearchMapString("Emerald_Forest");
                    newarea_titletext = "思い出イベント";
                    newarea_gohoubitext = "いっしょにおふろ♪" + "\n" + "解放！";
                    newarea_gohoubiicon = matplace_database.matplace_lists[_id].mapIcon_sprite;
                    newAreaRelease_panelKoushin(_star);

                    //GameMgr.SetHikariOmoideFlag("strawberry_sponge_cake", true);
                }
                break;

            case 6: //

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。変化がないのもあり。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev7", 3);
                    }
                    else
                    {
                        TreasureStatus("ev7", 0); //コスチュームはアイコン見た目変わらない
                    }
                }
                else if (_mstatus == 1)
                {
                    _id = pitemlist.SearchEmeraldItemStringID("RedDress_Costume");
                    newarea_titletext = "コスチューム";
                    newarea_gohoubitext = "コスチューム３をゲット！！";
                    newarea_gohoubiicon = pitemlist.emeralditemlist[_id].itemIcon_sprite;
                    newAreaRelease_panelKoushin(_star);

                    pitemlist.add_EmeraldPlayerItem(_id, 1);
                }
                break;

            case 7: //おたから

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev8", 1);
                    }
                    else
                    {
                        TreasureStatus("ev8", 0);
                    }
                }
                else if (_mstatus == 1)
                {
                    //
                    _id = pitemlist.Find_eventitemdatabase("cheese_cake_recipi");
                    newarea_titletext = "レシピ";
                    newarea_gohoubitext = "チーズケーキ＜上級＞のレシピをゲット！";
                    newarea_gohoubiicon = itemIcon_sprite2;
                    newAreaRelease_panelKoushin(_star);

                    pitemlist.add_eventPlayerItem(_id, 1);
                }

                break;

            case 8: //思い出イベント 

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。変化がないのもあり。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev9", 2);
                    }
                    else
                    {
                        TreasureStatus("ev9", 0);
                    }
                }
                else if (_mstatus == 1)
                {
                    //マリトッツォの思い出
                    _id = pitemlist.Find_eventitemdatabase("maritozzo_recipi");
                    newarea_titletext = "レシピ";
                    newarea_gohoubitext = "マリトッツォのレシピ！" + "\n" + "ゲット！";
                    newarea_gohoubiicon = pitemlist.eventitemlist[_id].itemIcon_sprite;
                    newAreaRelease_panelKoushin(_star);

                    pitemlist.add_eventPlayerItem(_id, 1);
                }
                break;

            case 9: //思い出イベント いっしょにおふろ♪

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。変化がないのもあり。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev10", 0);
                    }
                    else
                    {
                        TreasureStatus("ev10", 0); //アイコン見た目変わらない
                    }
                }
                else if (_mstatus == 1)
                {
                    //ショートケーキの思い出　仮
                    _id = matplace_database.SearchMapString("Emerald_Forest");
                    newarea_titletext = "思い出イベント";
                    newarea_gohoubitext = "スウィートホテル♪" + "\n" + "解放！";
                    newarea_gohoubiicon = matplace_database.matplace_lists[_id].mapIcon_sprite;
                    newAreaRelease_panelKoushin(_star);

                    //GameMgr.SetHikariOmoideFlag("strawberry_sponge_cake", true);
                }
                break;

            case 10: //おたから

                if (_mstatus == 0) //そこのおたからの状態をチェック　すでに取得済なら空アイコンに。
                {
                    if (GameMgr.StarRank_ReleaseList[_num]) //trueならすでに取得
                    {
                        TreasureStatus("ev11", 1);
                    }
                    else
                    {
                        TreasureStatus("ev11", 0);
                    }
                }
                else if (_mstatus == 1)
                {
                    //アクアマリンの湖
                    _id = matplace_database.SearchMapString("Aquamarine_Lake");
                    newarea_titletext = "おたから";
                    newarea_gohoubitext = "３つの中から好きなアイテムを選んでね。";
                    newarea_gohoubiicon = matplace_database.matplace_lists[_id].mapIcon_sprite;
                    newAreaRelease_panelKoushin(_star);
                }

                break;

            default:

                if (_mstatus == 1)
                {
                    newarea_titletext = "-";
                    newarea_gohoubitext = "";
                    newAreaRelease_panelKoushin(_star);
                }
                break;
        }
    }

    void TreasureStatus(string _objname, int _status)
    {
        dotevent_obj = sugoroku_board.transform.Find(_objname).gameObject;

        if (_status == 0)
        {
            dotevent_obj.transform.Find("dotEvent/IconRoot").gameObject.SetActive(true);
            dotevent_obj.transform.Find("dotEvent/ImgIcon_empty").gameObject.SetActive(false);
            dotevent_obj.transform.Find("dotEvent/ImgIcon_plateempty").gameObject.SetActive(false);
            dotevent_obj.transform.Find("dotEvent/IconRoot2").gameObject.SetActive(false);
        }
        else if (_status == 1)
        {
            dotevent_obj.transform.Find("dotEvent/IconRoot").gameObject.SetActive(false);
            dotevent_obj.transform.Find("dotEvent/ImgIcon_empty").gameObject.SetActive(true);
            dotevent_obj.transform.Find("dotEvent/ImgIcon_plateempty").gameObject.SetActive(false);
            dotevent_obj.transform.Find("dotEvent/IconRoot2").gameObject.SetActive(false);
        }
        else if (_status == 2)
        {
            dotevent_obj.transform.Find("dotEvent/IconRoot").gameObject.SetActive(false);
            dotevent_obj.transform.Find("dotEvent/ImgIcon_empty").gameObject.SetActive(false);
            dotevent_obj.transform.Find("dotEvent/ImgIcon_plateempty").gameObject.SetActive(true);
            dotevent_obj.transform.Find("dotEvent/IconRoot2").gameObject.SetActive(false);
        }
        else if (_status == 3) //アニメ系の空アイコン
        {
            dotevent_obj.transform.Find("dotEvent/IconRoot").gameObject.SetActive(false);
            dotevent_obj.transform.Find("dotEvent/ImgIcon_empty").gameObject.SetActive(false);
            dotevent_obj.transform.Find("dotEvent/ImgIcon_plateempty").gameObject.SetActive(false);
            dotevent_obj.transform.Find("dotEvent/IconRoot2").gameObject.SetActive(true);
        }
    }

    void newAreaRelease_panelKoushin(int _star)
    {
        newAreaRelease_Panel.GetComponent<NewAreaReleasePanel>().Set_GohoubiPanel(newarea_gohoubitext, newarea_titletext, _star, newarea_gohoubiicon);
        //newAreaRelease_Panel.GetComponent<NewAreaReleasePanel>().Set_PatissierRank(PlayerStatus.player_patissier_Rank_hyoukiList[_num + 1]);
    }
}
