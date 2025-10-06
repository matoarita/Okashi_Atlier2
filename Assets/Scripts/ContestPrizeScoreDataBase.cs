using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContestPrizeScoreDataBase : SingletonMonoBehaviour<ContestPrizeScoreDataBase>
{

    //コンテスト　ランク
    private Dictionary<int, string> PrizeRankList = new Dictionary<int, string>();

    //獲得人気度　順位ごとに、なん分の一とかになる
    private Dictionary<int, float> PrizeNinkiRankList = new Dictionary<int, float>();

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ContestStartListDataBase conteststartList_database;

    private MoneyStatus_Controller moneyStatus_Controller;
    private NinkiStatus_Controller ninkiStatus_Controller;

    private int i, ev_id;
    private int _getninki;

    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。 

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        //お金の増減用パネルの取得
        moneyStatus_Controller = MoneyStatus_Controller.Instance.GetComponent<MoneyStatus_Controller>();

        //人気コントローラー取得
        ninkiStatus_Controller = NinkiStatus_Controller.Instance.GetComponent<NinkiStatus_Controller>();

        PrizeRankDict();
        PrizeNinkiRankDict();
    }

    void Update()
    {

    }

    //トーナメント形式の設定
    public void OnPrizeListSet(int _ContestSelectNum)
    {
        switch(_ContestSelectNum)
        {
            case 1000:

                //PrizeSet01();
                PrizeSet02();
                break;

            case 2000:

                PrizeSet03();
                break;

            case 3000:

                PrizeSet04();
                break;

            case 4000:

                PrizeSet04();
                break;
        }

        GameMgr.PrizeGetninkiparam_before = conteststartList_database.conteststart_lists[conteststartList_database.SearchContestPlaceNum(GameMgr.ContestSelectNum)].GetPatissierPoint;
        //ContestStartListDBのほうで、指定しているのでここではボス名入力不要
        //GameMgr.contest_boss_name = GameMgr.PrizeCharacterList[GameMgr.PrizeCharacterList.Count - 1];
    }

    //ランキング形式の設定
    public void OnPrizeListRankingSet(int _ContestSelectNum)
    {
        switch (_ContestSelectNum)
        {
            case 10000:

                PrizeRankingSet01();
                break;

            case 10100:

                PrizeRankingSet02();
                break;

            case 10200:

                PrizeRankingSet03();
                break;

            case 10300:

                PrizeRankingSet04();
                break;

            case 10400:

                PrizeRankingSet05();
                break;

            case 10500:

                PrizeRankingSet06();
                break;

            case 10600:

                PrizeRankingSet07();
                break;

            case 10700:

                PrizeRankingSet08();
                break;

            case 10800:

                PrizeRankingSet09();
                break;

            case 10900:

                PrizeRankingSet10();
                break;

            case 20000:

                PrizeRankingSet20();
                break;

            case 20100:

                PrizeRankingSet21();
                break;

            case 20200:

                PrizeRankingSet22();
                break;

            case 20300:

                PrizeRankingSet23();
                break;

            case 20400:

                PrizeRankingSet24();
                break;

            case 20500:

                PrizeRankingSet25();
                break;

            case 20600:

                PrizeRankingSet26();
                break;

            case 20700:

                PrizeRankingSet27();
                break;

            case 20800:

                PrizeRankingSet28();
                break;

            case 20900:

                PrizeRankingSet29();
                break;

            case 30000:

                PrizeRankingSet40();
                break;

            case 30100:

                PrizeRankingSet41();
                break;

            case 30200:

                PrizeRankingSet42();
                break;

            case 30300:

                PrizeRankingSet43();
                break;

            case 30400:

                PrizeRankingSet44();
                break;

            case 30500:

                PrizeRankingSet45();
                break;

            case 30600:

                PrizeRankingSet46();
                break;

            case 30700:

                PrizeRankingSet47();
                break;

            case 30800:

                PrizeRankingSet47(); //デザインコンテスト　現在未実装
                break;

            case 30900:

                PrizeRankingSet49();
                break;

            case 40000:

                PrizeRankingSet60();
                break;

            case 40100:

                PrizeRankingSet61();
                break;

            case 40200:

                PrizeRankingSet62();
                break;

            case 40300:

                PrizeRankingSet63();
                break;

            case 40400:

                PrizeRankingSet64();
                break;

            case 40500:

                PrizeRankingSet65();
                break;

            case 40600:

                PrizeRankingSet66();
                break;

            case 40700:

                PrizeRankingSet67();
                break;
        }

        GameMgr.PrizeGetninkiparam_before = conteststartList_database.conteststart_lists[conteststartList_database.SearchContestPlaceNum(GameMgr.ContestSelectNum)].GetPatissierPoint;
        GameMgr.contest_boss_name = GameMgr.PrizeCharacterList[GameMgr.PrizeCharacterList.Count - 1];
    }

    //Contest_Main_OrA1から読む
    public void PrizeGet()
    {
        GameMgr.Contest_pastVictory_on = false;

        if (GameMgr.Contest_Cate_Ranking == 0) //コンテストがトーナメント形式=0
        {
            //トーナメント形式の賞品獲得
            //負けるとそこでゲームオーバーなので、実質決勝戦優勝したときだけ、ここで賞品獲得
            switch (GameMgr.ContestRoundNum)
            {
                case 3:

                    i = GameMgr.PrizeItemList.Count - 1;

                    if (GameMgr.PrizeItemList[i] != "Non")
                    {
                        if (GameMgr.EdenPrizeChange) //該当コンテスト　賞品が切り替わる
                        {
                            if (GameMgr.EdenFirstVictory)
                            {
                                GetPlayerItem(GameMgr.PrizeItemList[i]);
                            }
                            else
                            {
                                GetPlayerItem(GameMgr.PrizeItemSecond);
                            }
                        }
                        else
                        {
                            GetPlayerItem(GameMgr.PrizeItemList[i]);
                        }
                    }
                    else
                    {
                        GameMgr.Contest_PrizeGet_ItemName = "Non";
                    }

                    moneyStatus_Controller.Getmoney_noAnim(GameMgr.PrizeGetMoneyList[i]);
                    GameMgr.Contest_PrizeGet_Money = GameMgr.PrizeGetMoneyList[i];

                    if (GameMgr.System_ContestStarGet_ON)
                    {
                        _getninki = GameMgr.PrizeGetninkiparam_before;
                    }
                    else
                    {
                        _getninki = 0;
                    }
                    GameMgr.Contest_PrizeGetninkiparam = _getninki;
                    ninkiStatus_Controller.GetNinki(_getninki); //人気の獲得　
                                                                //ninkiStatus_Controller.GetNinki(1); 優勝時のみ、優勝回数として人気＋１
                    Debug.Log("ランク: " + "優勝" + "人気獲得: " + _getninki);
                    break;
            }

        }
        else //ランキング形式の賞品獲得
        {
            //5段階ぐらいで分ける？
            i = 0;
            while (i < 5) //5位まで判定
            {
                if (GameMgr.contest_Rank_Count == 5 - i) //5位
                {
                    //賞品　獲得処理
                    if (GameMgr.PrizeItemList[i] != "Non")
                    {
                        GetPlayerItem(GameMgr.PrizeItemList[i]);

                        //１位のときのみ、ほかにガッポリとたくさんのアイテムを獲得
                        if(GameMgr.contest_Rank_Count == 1)
                        {

                        }
                    }
                    else
                    {
                        GameMgr.Contest_PrizeGet_ItemName = "Non";
                    }


                    if (i == 3) //2位のときのみ
                    {
                        if (GameMgr.System_ContestStarGet_ON)
                        {
                            //過去2位をとったor優勝したことがある
                            if (conteststartList_database.conteststart_lists[conteststartList_database.SearchContestPlaceNum(GameMgr.ContestSelectNum)].ContestVictory == 2 ||
                                conteststartList_database.conteststart_lists[conteststartList_database.SearchContestPlaceNum(GameMgr.ContestSelectNum)].ContestVictory == 1)
                            {
                                _getninki = 0;
                                GameMgr.Contest_pastVictory_on = true;
                            }
                            else
                            {
                                _getninki = 1; //２位だと1もらえる
                            }
                        }
                        else
                        {
                            _getninki = 0;
                        }
                    }
                    else
                    {
                        //過去優勝したことがある
                        if (conteststartList_database.conteststart_lists[conteststartList_database.SearchContestPlaceNum(GameMgr.ContestSelectNum)].ContestVictory == 1)
                        {
                            _getninki = 0;
                            GameMgr.Contest_pastVictory_on = true;
                        }
                        else
                        {
                            _getninki = (int)(GameMgr.PrizeGetninkiparam_before * PrizeNinkiRankList[i]);
                        }
                    }

                    //賞金
                    //過去優勝したことがあると賞金なし
                    if (GameMgr.Contest_pastVictory_on)
                    {
                        GameMgr.Contest_PrizeGet_Money = 0;
                    }
                    else
                    {
                        GameMgr.Contest_PrizeGet_Money = GameMgr.PrizeGetMoneyList[i];
                    }

                    //獲得の処理
                    moneyStatus_Controller.Getmoney_noAnim(GameMgr.Contest_PrizeGet_Money);
                    GameMgr.Contest_PrizeGetninkiparam = _getninki;
                    ninkiStatus_Controller.GetNinki(_getninki); //人気の獲得
                    Debug.Log("ランク: " + PrizeRankList[i] + " 人気獲得: " + _getninki);
                    break;
                } 

                i++;
            }
        }
    }

    void GetPlayerItem(string _itemName)
    {
        if (pitemlist.Find_eventitemdatabase(_itemName) == 9999) //イベントアイテムが該当してないか先にチェック
        {
            pitemlist.addPlayerItemString(_itemName, 1);
            GameMgr.Contest_PrizeGet_ItemName = database.items[database.SearchItemIDString(_itemName)].itemNameHyouji;
        }
        else //イベントアイテム該当してた場合は、イベントアイテムを追加する処理に。
        {
            ev_id = pitemlist.Find_eventitemdatabase(_itemName);
            pitemlist.add_eventPlayerItem(ev_id, 1); //
            GameMgr.Contest_PrizeGet_ItemName = pitemlist.eventitemlist[ev_id].event_itemNameHyouji;
        }
    }

    void PrizeRankDict()
    {
        PrizeRankList.Clear();
        PrizeRankList.Add(0, "D");
        PrizeRankList.Add(1, "C");
        PrizeRankList.Add(2, "B");
        PrizeRankList.Add(3, "A");
        PrizeRankList.Add(4, "S");
    }

    void PrizeNinkiRankDict()
    {
        PrizeNinkiRankList.Clear();
        if (GameMgr.System_ContestStarGet_ON)
        {           
            PrizeNinkiRankList.Add(0, 0f);
            PrizeNinkiRankList.Add(1, 0f); //
            PrizeNinkiRankList.Add(2, 0f); //GetPatissierPointの10分の一
            PrizeNinkiRankList.Add(3, 0f); //3分の一
            PrizeNinkiRankList.Add(4, 1.0f); //一位　まるっともらえる
        }
        else
        {
            //スター獲得できない仕様
            PrizeNinkiRankList.Add(0, 0f);
            PrizeNinkiRankList.Add(1, 0f); //
            PrizeNinkiRankList.Add(2, 0f); //GetPatissierPointの10分の一
            PrizeNinkiRankList.Add(3, 0f); //3分の一
            PrizeNinkiRankList.Add(4, 0f); //一位　まるっともらえる
        }
    }

    //トーナメント形式の賞品設定　選手名はContestStartListDBで決める 1・2回戦敗退は何ももらえない
    void PrizeSet01() //現在未使用
    {
        //賞品リスト　トーナメントは3回戦なので3つまで。
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //3位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("card_alice");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(10000);

        //二回目出場以降の賞品
        GameMgr.PrizeItemSecond = "card_alice";

        //トーナメント形式では使わない　boss_scoreに直接いれるため
        //相手の点数リスト 5位から順番に入れる
        /*GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(60);
        GameMgr.PrizeScoreAreaList.Add(120);
        GameMgr.PrizeScoreAreaList.Add(180);
        GameMgr.PrizeScoreAreaList.Add(240);*/

        //参加者名リスト(上位4人) + 5人目がアキラくんになる 最下位から順番に入れる
        /*GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");*/
    }

    void PrizeSet02()
    {
        //賞品リスト　トーナメントは3回戦なので3つまで。
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //3位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("eden_recipi_02");

        //賞金リスト
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(5000);

        //二回目出場以降の賞品
        GameMgr.PrizeItemSecond = "trophy_spring";
    }

    void PrizeSet03()
    {
        //賞品リスト　トーナメントは3回戦なので3つまで。
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //3位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("eden_recipi_03");

        //賞金リスト
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(10000);

        //二回目出場以降の賞品
        GameMgr.PrizeItemSecond = "trophy_summer";
    }

    void PrizeSet04()
    {
        //賞品リスト　トーナメントは3回戦なので3つまで。
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //3位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("eden_recipi_04");

        //賞金リスト
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(10000);

        //二回目出場以降の賞品
        GameMgr.PrizeItemSecond = "trophy_autumn";
    }
    //







    //
    //ランキング形式
    //

    //〇クッキー初級コンテスト
    void PrizeRankingSet01()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(300);
        GameMgr.PrizeGetMoneyList.Add(700);
        GameMgr.PrizeGetMoneyList.Add(1500);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(62);
        GameMgr.PrizeScoreAreaList.Add(103);
        GameMgr.PrizeScoreAreaList.Add(137);        

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("サモ・ハーン");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チューン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポットマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    //〇オランジーナ・パティスリーアワード　ケーキかクリームブリュレ　秋に移動したので点数高め
    void PrizeRankingSet02()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(300);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(5000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(237);
        GameMgr.PrizeScoreAreaList.Add(301);
        GameMgr.PrizeScoreAreaList.Add(335);
        GameMgr.PrizeScoreAreaList.Add(351);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("クルル");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポットマン");
        GameMgr.PrizeCharacterList.Add("魔女ティリス");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    //〇ベオルブ家のディナー　自由課題
    void PrizeRankingSet03()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("beorv_iron");
        GameMgr.PrizeItemList.Add("strawberry_milfiyu_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(300);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2500);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(75);
        GameMgr.PrizeScoreAreaList.Add(107);
        GameMgr.PrizeScoreAreaList.Add(125);
        GameMgr.PrizeScoreAreaList.Add(179);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("キリコ");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("レイナート");
        GameMgr.PrizeCharacterList.Add("魔女ティリス");
    }

    //〇ラスクブロカント
    void PrizeRankingSet04()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("pocket_tissue");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("teaset_wizard");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(300);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(1500);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(62);
        GameMgr.PrizeScoreAreaList.Add(106);
        GameMgr.PrizeScoreAreaList.Add(122);
        GameMgr.PrizeScoreAreaList.Add(145);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ナタリー・ポットマン");
        GameMgr.PrizeCharacterList.Add("ルッカティエル");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("シスター・リーシュ");
    }

    //〇ルミエール・エピファニア
    void PrizeRankingSet05()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("emerald_suger");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("teaset_flower");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(88);
        GameMgr.PrizeScoreAreaList.Add(115);
        GameMgr.PrizeScoreAreaList.Add(192);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ミント");
        GameMgr.PrizeCharacterList.Add("セリーヌ");
        GameMgr.PrizeCharacterList.Add("おそうじアリス");
        GameMgr.PrizeCharacterList.Add("シスター・リーシュ");
    }

    //〇ルミエール・カンデラ
    void PrizeRankingSet06() //光ラスクか光チーズケーキ　光りジュース
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("aquamarine_chocolate_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(800);
        GameMgr.PrizeGetMoneyList.Add(1500);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(75);
        GameMgr.PrizeScoreAreaList.Add(150);
        GameMgr.PrizeScoreAreaList.Add(189);
        GameMgr.PrizeScoreAreaList.Add(232);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();        
        GameMgr.PrizeCharacterList.Add("セリーヌ");
        GameMgr.PrizeCharacterList.Add("ミント");
        GameMgr.PrizeCharacterList.Add("シスター・リーシュ");
        GameMgr.PrizeCharacterList.Add("おそうじアリス");
    }

    //〇ガレット・デ・ロワ　オペラ・ザッハトルテ・ファンタジアン　終盤レベル
    void PrizeRankingSet07()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_gold");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(2500);
        GameMgr.PrizeGetMoneyList.Add(10000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(165);
        GameMgr.PrizeScoreAreaList.Add(189);
        GameMgr.PrizeScoreAreaList.Add(200);
        GameMgr.PrizeScoreAreaList.Add(404);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("メンデル・スーザン");
        GameMgr.PrizeCharacterList.Add("ハーマウズ");
        GameMgr.PrizeCharacterList.Add("モツァール三世");
    }

    //〇ディオ・ショコラ・チャンピオンシップ チョコで一番　終盤レベル
    void PrizeRankingSet08()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(2500);
        GameMgr.PrizeGetMoneyList.Add(10000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(135);
        GameMgr.PrizeScoreAreaList.Add(212);
        GameMgr.PrizeScoreAreaList.Add(267);
        GameMgr.PrizeScoreAreaList.Add(351);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("黄ずきん");
        GameMgr.PrizeCharacterList.Add("白桃姫");
        GameMgr.PrizeCharacterList.Add("青ずきん");
        GameMgr.PrizeCharacterList.Add("黒ずきん");
    }

    //〇フィナンシェバターズカップ
    void PrizeRankingSet09()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(200);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(99);
        GameMgr.PrizeScoreAreaList.Add(113);
        GameMgr.PrizeScoreAreaList.Add(161);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ギリガン");
        GameMgr.PrizeCharacterList.Add("ヴィクター");
        GameMgr.PrizeCharacterList.Add("ガリー");
        GameMgr.PrizeCharacterList.Add("シスター・リーシュ");
    }

    //〇春のおかし大祭典
    void PrizeRankingSet10()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("mg_parfect_princess_book");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(91);
        GameMgr.PrizeScoreAreaList.Add(135);
        GameMgr.PrizeScoreAreaList.Add(157);
        GameMgr.PrizeScoreAreaList.Add(220);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("シスター・リーシュ");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポットマン");
        GameMgr.PrizeCharacterList.Add("セリーヌ");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    //〇ひんやりお菓子コンテスト
    void PrizeRankingSet20()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("cream_brulee_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(300);
        GameMgr.PrizeGetMoneyList.Add(800);
        GameMgr.PrizeGetMoneyList.Add(1500);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(67);
        GameMgr.PrizeScoreAreaList.Add(102);
        GameMgr.PrizeScoreAreaList.Add(121);
        GameMgr.PrizeScoreAreaList.Add(152);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ミント");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("おそうじアリス");       
        GameMgr.PrizeCharacterList.Add("ジェラット");
    }

    //〇フライング・ソーダコンテスト　ソーダ限定
    void PrizeRankingSet21()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("cheese_cake_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(700);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(67);
        GameMgr.PrizeScoreAreaList.Add(75);
        GameMgr.PrizeScoreAreaList.Add(98);
        GameMgr.PrizeScoreAreaList.Add(125);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ウリユ");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("ベル");
        GameMgr.PrizeCharacterList.Add("おそうじアリス");
    }

    //〇ボンボヤージュ・カップ　ジュース系
    void PrizeRankingSet22()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("residual_heatstone");
        GameMgr.PrizeItemList.Add("choco_banana_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(1500);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(67);
        GameMgr.PrizeScoreAreaList.Add(88);
        GameMgr.PrizeScoreAreaList.Add(122);
        GameMgr.PrizeScoreAreaList.Add(197);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ギュント");
        GameMgr.PrizeCharacterList.Add("バニラ");
        GameMgr.PrizeCharacterList.Add("ノーマリー");
        GameMgr.PrizeCharacterList.Add("ウリユ");
    }

    //〇おみやげおかしコンテスト　チョコばなな・マリトッツォ・シュークリーム・ふわころ等　こどもが喜びそう、または持ち帰りが簡単なお菓子系で一番
    void PrizeRankingSet23()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("langue de chat_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(117);
        GameMgr.PrizeScoreAreaList.Add(125);
        GameMgr.PrizeScoreAreaList.Add(138);
        GameMgr.PrizeScoreAreaList.Add(185);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("エリカ");
        GameMgr.PrizeCharacterList.Add("おそうじアリス");
        GameMgr.PrizeCharacterList.Add("ミント");
        GameMgr.PrizeCharacterList.Add("ウリユ");
    }

    //〇スカーレットマイスター　いちご系のおかしで一番をとる
    void PrizeRankingSet24()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("juice_mixer_high");
        GameMgr.PrizeItemList.Add("montblanc_cake_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(156);
        GameMgr.PrizeScoreAreaList.Add(208);
        GameMgr.PrizeScoreAreaList.Add(242);
        GameMgr.PrizeScoreAreaList.Add(383);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("おそうじアリス");
        GameMgr.PrizeCharacterList.Add("ノーマリー");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("エリカ");
    }

    //〇遥かなる蒼賞　海をテーマにしたチョコレート
    void PrizeRankingSet25()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("bushdenoel_cake_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(1500);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(5000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(178);
        GameMgr.PrizeScoreAreaList.Add(272);
        GameMgr.PrizeScoreAreaList.Add(325);
        GameMgr.PrizeScoreAreaList.Add(432);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ウリユ");
        GameMgr.PrizeCharacterList.Add("エリカ");
        GameMgr.PrizeCharacterList.Add("ノーマリー");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    //マジックパティスリー・アワード　魔法おかし限定　見た目が変わっているお菓子でクリア　難しい　ウィンドツイスター系・ライフストリームのおかし・マジックソーダ・フォーゲットの上位種
    void PrizeRankingSet26()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("mg_moonlight_banana_book");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(1111);
        GameMgr.PrizeGetMoneyList.Add(2222);
        GameMgr.PrizeGetMoneyList.Add(3333);
        GameMgr.PrizeGetMoneyList.Add(7777);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(138);
        GameMgr.PrizeScoreAreaList.Add(191);
        GameMgr.PrizeScoreAreaList.Add(234);
        GameMgr.PrizeScoreAreaList.Add(407);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("シスターリーシュ");
        GameMgr.PrizeCharacterList.Add("ノーマリー");
        GameMgr.PrizeCharacterList.Add("カリン");
    }

    //プラム洋菓子技術コンテスト　ケーキ限定　難易度高い　点数がともかくでない
    void PrizeRankingSet27()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("wood_rod_doillan");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(30000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(160);
        GameMgr.PrizeScoreAreaList.Add(250);
        GameMgr.PrizeScoreAreaList.Add(335);
        GameMgr.PrizeScoreAreaList.Add(458);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ミント");
        GameMgr.PrizeCharacterList.Add("黒ずきん");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("ベル");
    }

    //チョコレート初級コンテスト
    void PrizeRankingSet28()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(1500);
        GameMgr.PrizeGetMoneyList.Add(2500);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(152);
        GameMgr.PrizeScoreAreaList.Add(197);
        GameMgr.PrizeScoreAreaList.Add(213);
        GameMgr.PrizeScoreAreaList.Add(252);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ウリユ");
        GameMgr.PrizeCharacterList.Add("ノーマリー");
        GameMgr.PrizeCharacterList.Add("エリカ");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    //パティシエの森　自由課題初級
    void PrizeRankingSet29()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("confiserie_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(1500);
        GameMgr.PrizeGetMoneyList.Add(2500);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(102);
        GameMgr.PrizeScoreAreaList.Add(155);
        GameMgr.PrizeScoreAreaList.Add(178);
        GameMgr.PrizeScoreAreaList.Add(208);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("エリカ");
        GameMgr.PrizeCharacterList.Add("ウリユ");
        GameMgr.PrizeCharacterList.Add("ベル");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    //〇クレープ・ドゥ・シャノワール　クレープ系　クレープは屋台で手に入れるので、入手のヒントがないと困るかも。
    void PrizeRankingSet40()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("mg_nappe_book");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(200);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1500);
        GameMgr.PrizeGetMoneyList.Add(2000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(68);
        GameMgr.PrizeScoreAreaList.Add(101);
        GameMgr.PrizeScoreAreaList.Add(119);
        GameMgr.PrizeScoreAreaList.Add(137);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("クルル");
        GameMgr.PrizeCharacterList.Add("おそうじアリス");
        GameMgr.PrizeCharacterList.Add("クラリス");
        GameMgr.PrizeCharacterList.Add("フォルトーネ");
    }

    //〇アデュルティ・ガトー　大人なお菓子　チョコorコーヒーorカンノーリorオペラやモンブランなどの大人おかし
    void PrizeRankingSet41()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("affo_gato_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(4000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(159);
        GameMgr.PrizeScoreAreaList.Add(196);
        GameMgr.PrizeScoreAreaList.Add(237);
        GameMgr.PrizeScoreAreaList.Add(288);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("クルル");
        GameMgr.PrizeCharacterList.Add("フォルトーネ");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("シャルロット");
    }

    //〇メルヘンランド♪カップ　メルヘンなお菓子　通らないと、先へ進めない
    void PrizeRankingSet42()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("sachertorte_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(4000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(266);
        GameMgr.PrizeScoreAreaList.Add(282);
        GameMgr.PrizeScoreAreaList.Add(302);
        GameMgr.PrizeScoreAreaList.Add(355);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("ミント");
        GameMgr.PrizeCharacterList.Add("ベル");
        GameMgr.PrizeCharacterList.Add("クルル");
    }

    //〇キラキラ・ボンボンズ　あめに限らず　りんごあめ・チョコばなな系・じゃがバター・宝石キャンディ・琥珀糖・パチパチソーダなど
    void PrizeRankingSet43()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("patipati_soda_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(1500);
        GameMgr.PrizeGetMoneyList.Add(2000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(89);
        GameMgr.PrizeScoreAreaList.Add(135);
        GameMgr.PrizeScoreAreaList.Add(159);
        GameMgr.PrizeScoreAreaList.Add(275);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("エリカ");
        GameMgr.PrizeCharacterList.Add("フォルトーネ");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("クルル");
    }

    //〇英国ティータイムコンテスト
    void PrizeRankingSet44()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("infinity_fountain");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(2000);
        GameMgr.PrizeGetMoneyList.Add(5000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(105);
        GameMgr.PrizeScoreAreaList.Add(114);
        GameMgr.PrizeScoreAreaList.Add(137);
        GameMgr.PrizeScoreAreaList.Add(222);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("フランソワ");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("エリヤ");
        GameMgr.PrizeCharacterList.Add("フォルトーネ");
    }

    //〇ピエスモンテ　場を彩る彫刻お菓子　造形系魔法で作るおかし・ウィンドツイスター・フローティング　彫刻お菓子は特殊な判定で、見た目のみを判定する
    void PrizeRankingSet45()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("mg_beautifulpower_book");
        GameMgr.PrizeItemList.Add("forgetmenot_recipi");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(5000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(163);
        GameMgr.PrizeScoreAreaList.Add(215);
        GameMgr.PrizeScoreAreaList.Add(349);
        GameMgr.PrizeScoreAreaList.Add(451);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("フランソワ");
        GameMgr.PrizeCharacterList.Add("フォルトーネ");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("ジョーカー");
    }

    //〇コンチェルティーノ・イン・ブルー　青をテーマにしたお菓子　アクアマリンチョコやブルーチョコ、レーブドゥヴィオレッタ、すみれの青紅茶
    void PrizeRankingSet46()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("mugen_niwatori");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2500);
        GameMgr.PrizeGetMoneyList.Add(5000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(255);
        GameMgr.PrizeScoreAreaList.Add(276);
        GameMgr.PrizeScoreAreaList.Add(298);
        GameMgr.PrizeScoreAreaList.Add(354);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("クラリス");
        GameMgr.PrizeCharacterList.Add("フォルトーネ");
        GameMgr.PrizeCharacterList.Add("クルル");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    //〇ビジョウ・パティスリー・カップ　鉱石お菓子限定　宝石キャンディ（点数低い）・鉱石マフィン・琥珀糖　お花のシュガーで作るおかし（未実装）
    void PrizeRankingSet47()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("jewery_master_proof");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(2000);
        GameMgr.PrizeGetMoneyList.Add(4000);
        GameMgr.PrizeGetMoneyList.Add(6000);
        GameMgr.PrizeGetMoneyList.Add(8000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(154);
        GameMgr.PrizeScoreAreaList.Add(196);
        GameMgr.PrizeScoreAreaList.Add(305);
        GameMgr.PrizeScoreAreaList.Add(424);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("エリヤ");
        GameMgr.PrizeCharacterList.Add("フランソワ");
        GameMgr.PrizeCharacterList.Add("ジョーカー");
        GameMgr.PrizeCharacterList.Add("クルル");
    }

    //秋のお菓子コンテスト　自由課題　中級
    void PrizeRankingSet49()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(200);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(128);
        GameMgr.PrizeScoreAreaList.Add(156);
        GameMgr.PrizeScoreAreaList.Add(168);
        GameMgr.PrizeScoreAreaList.Add(204);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("エリヤ");
        GameMgr.PrizeCharacterList.Add("クルル");
        GameMgr.PrizeCharacterList.Add("フランソワ");
        GameMgr.PrizeCharacterList.Add("フォルトーネ");
    }

    //〇クワイットスノウ　自由課題
    void PrizeRankingSet60()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(94);
        GameMgr.PrizeScoreAreaList.Add(123);
        GameMgr.PrizeScoreAreaList.Add(135);
        GameMgr.PrizeScoreAreaList.Add(172);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("エルメス");
        GameMgr.PrizeCharacterList.Add("アイリン");
        GameMgr.PrizeCharacterList.Add("エリヤ");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    //〇アムール・チョコレイト・コンテスト　愛の点数が高いチョコレート
    void PrizeRankingSet61()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(700);
        GameMgr.PrizeGetMoneyList.Add(1500);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(6000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(98);
        GameMgr.PrizeScoreAreaList.Add(181);
        GameMgr.PrizeScoreAreaList.Add(197);
        GameMgr.PrizeScoreAreaList.Add(225);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アイリン");
        GameMgr.PrizeCharacterList.Add("ユディー");
        GameMgr.PrizeCharacterList.Add("シャリー");
        GameMgr.PrizeCharacterList.Add("シュバルツヴェルダー");
    }

    //〇ネオユニバース・カップ　宇宙をテーマにしたお菓子　チーズケーキ・シリウスやプルート、ソーダギャラクシーのみ　レシピ解放してないとクリアは難しい
    void PrizeRankingSet62()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(5000);
        GameMgr.PrizeGetMoneyList.Add(10000);
        GameMgr.PrizeGetMoneyList.Add(15000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(159);
        GameMgr.PrizeScoreAreaList.Add(212);
        GameMgr.PrizeScoreAreaList.Add(253);
        GameMgr.PrizeScoreAreaList.Add(275);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("マドカ");
        GameMgr.PrizeCharacterList.Add("リディア");
        GameMgr.PrizeCharacterList.Add("シャルロット");
    }

    //〇フェド・フルラージュ　お花がテーマのお菓子限定　お花のクッキーやフローラルバターを使ったケーキかチョコ、チーズケーキにフリーズフラワーをトッピングしたものなど
    void PrizeRankingSet63()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("mg_spring_pharmacy_book");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(2000);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(5000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(267);
        GameMgr.PrizeScoreAreaList.Add(298);
        GameMgr.PrizeScoreAreaList.Add(310);
        GameMgr.PrizeScoreAreaList.Add(343);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("フォルトーネ");
        GameMgr.PrizeCharacterList.Add("シャリー");
        GameMgr.PrizeCharacterList.Add("シャルロット");
        GameMgr.PrizeCharacterList.Add("カリン");
    }

    //〇ルミエール・ドゥ・ソレイユ　あたたかいお菓子限定　アフォガートやアップルパイ
    void PrizeRankingSet64()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("mg_warming_handmade_book");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(3000);
        GameMgr.PrizeGetMoneyList.Add(5000);
        GameMgr.PrizeGetMoneyList.Add(15000);
        GameMgr.PrizeGetMoneyList.Add(30000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(186);
        GameMgr.PrizeScoreAreaList.Add(211);
        GameMgr.PrizeScoreAreaList.Add(245);
        GameMgr.PrizeScoreAreaList.Add(367);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("シャリー");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポットマン");
        GameMgr.PrizeCharacterList.Add("シスター・リーシュ");
        GameMgr.PrizeCharacterList.Add("シュバルツヴェルダー");
    }

    //〇ミルフイユ・ドゥ・パリ
    void PrizeRankingSet65()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("cakemold_stainless");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(5000);
        GameMgr.PrizeGetMoneyList.Add(10000);
        GameMgr.PrizeGetMoneyList.Add(20000);
        GameMgr.PrizeGetMoneyList.Add(30000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(272);
        GameMgr.PrizeScoreAreaList.Add(276);
        GameMgr.PrizeScoreAreaList.Add(287);
        GameMgr.PrizeScoreAreaList.Add(339);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ナタリー・ポットマン");
        GameMgr.PrizeCharacterList.Add("ミント");
        GameMgr.PrizeCharacterList.Add("クラリス");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    //〇チーズケーキ・パティスリーアワード　チーズケーキ限定　
    void PrizeRankingSet66()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(15000);
        GameMgr.PrizeGetMoneyList.Add(25000);
        GameMgr.PrizeGetMoneyList.Add(35000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(168);
        GameMgr.PrizeScoreAreaList.Add(337);
        GameMgr.PrizeScoreAreaList.Add(391);
        GameMgr.PrizeScoreAreaList.Add(435);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("シャルロット");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("エリー");
        GameMgr.PrizeCharacterList.Add("シュバルツヴェルダー");
    }

    //〇夢見るチョコレート選手権　愛＋メルヘンが高いチョコレート（チョコケーキも可）のお菓子　ウィンドアークを使ったチョコケーキ・天使の羽根のチョコ
    void PrizeRankingSet67()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("Non"); //5位 ↓
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("Non");
        GameMgr.PrizeItemList.Add("platinum_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(5000);
        GameMgr.PrizeGetMoneyList.Add(15000);
        GameMgr.PrizeGetMoneyList.Add(25000);
        GameMgr.PrizeGetMoneyList.Add(50000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(422);
        GameMgr.PrizeScoreAreaList.Add(465);
        GameMgr.PrizeScoreAreaList.Add(524);
        GameMgr.PrizeScoreAreaList.Add(607);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
        GameMgr.PrizeCharacterList.Add("アイリン");
        GameMgr.PrizeCharacterList.Add("クルル");
        GameMgr.PrizeCharacterList.Add("シュバルツヴェルダー");
    }
}