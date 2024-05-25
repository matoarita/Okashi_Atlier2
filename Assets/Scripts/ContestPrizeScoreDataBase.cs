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

    private int i;
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

                PrizeSet01();
                break;

            case 2000:

                PrizeSet02();
                break;

            case 3000:

                PrizeSet03();
                break;

            case 4000:

                PrizeSet04();
                break;
        }

        GameMgr.PrizeGetninkiparam = conteststartList_database.conteststart_lists[conteststartList_database.SearchContestPlaceNum(GameMgr.ContestSelectNum)].GetPatissierPoint;
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

        GameMgr.PrizeGetninkiparam = conteststartList_database.conteststart_lists[conteststartList_database.SearchContestPlaceNum(GameMgr.ContestSelectNum)].GetPatissierPoint;
        GameMgr.contest_boss_name = GameMgr.PrizeCharacterList[GameMgr.PrizeCharacterList.Count - 1];
    }

    //Contest_Main_OrA1から読む
    public void PrizeGet()
    {
        //5段階ぐらいで分ける？
        i = 0;
        while (i <= GameMgr.PrizeScoreAreaList.Count)
        {
            if (i == 0)
            {
                if (GameMgr.contest_PrizeScore < GameMgr.PrizeScoreAreaList[i])
                {
                    pitemlist.addPlayerItemString(GameMgr.PrizeItemList[i], 1);
                    GameMgr.Contest_PrizeGet_ItemName = database.items[database.SearchItemIDString(GameMgr.PrizeItemList[i])].itemNameHyouji;
                    moneyStatus_Controller.Getmoney_noAnim(GameMgr.PrizeGetMoneyList[i]);
                    GameMgr.Contest_PrizeGet_Money = GameMgr.PrizeGetMoneyList[i];
                    _getninki = 1;
                    ninkiStatus_Controller.GetNinki(_getninki); //人気の獲得 最低でも1は入る
                    Debug.Log("ランク: " + PrizeRankList[i] + "人気獲得: " + _getninki);
                    break;
                }
            }
            else
            {
                if (i != GameMgr.PrizeScoreAreaList.Count)
                {
                    if (GameMgr.contest_PrizeScore >= GameMgr.PrizeScoreAreaList[i - 1] && GameMgr.contest_PrizeScore < GameMgr.PrizeScoreAreaList[i])
                    {
                        pitemlist.addPlayerItemString(GameMgr.PrizeItemList[i], 1);
                        GameMgr.Contest_PrizeGet_ItemName = database.items[database.SearchItemIDString(GameMgr.PrizeItemList[i])].itemNameHyouji;
                        moneyStatus_Controller.Getmoney_noAnim(GameMgr.PrizeGetMoneyList[i]);
                        GameMgr.Contest_PrizeGet_Money = GameMgr.PrizeGetMoneyList[i];
                        _getninki = (int)(GameMgr.PrizeGetninkiparam * PrizeNinkiRankList[i]);
                        ninkiStatus_Controller.GetNinki(_getninki); //人気の獲得
                        Debug.Log("ランク: " + PrizeRankList[i] + "人気獲得: " + _getninki);
                        break;
                    }
                }
                else //リストの一番最後
                {
                    if (GameMgr.contest_PrizeScore >= GameMgr.PrizeScoreAreaList[i - 1])
                    {
                        pitemlist.addPlayerItemString(GameMgr.PrizeItemList[i], 1);
                        GameMgr.Contest_PrizeGet_ItemName = database.items[database.SearchItemIDString(GameMgr.PrizeItemList[i])].itemNameHyouji;
                        moneyStatus_Controller.Getmoney_noAnim(GameMgr.PrizeGetMoneyList[i]);
                        GameMgr.Contest_PrizeGet_Money = GameMgr.PrizeGetMoneyList[i];
                        _getninki = (int)(GameMgr.PrizeGetninkiparam * PrizeNinkiRankList[i]);
                        ninkiStatus_Controller.GetNinki(_getninki); //人気の獲得
                        Debug.Log("ランク: " + PrizeRankList[i] + "人気獲得: " + _getninki);
                        break;
                    }
                }

            }
            i++;
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
        PrizeNinkiRankList.Add(0, 0f);
        PrizeNinkiRankList.Add(1, 0.1f); //GetPatissierPointの10分の一
        PrizeNinkiRankList.Add(2, 0.25f); //4分の一
        PrizeNinkiRankList.Add(3, 0.5f); //2分の一
        PrizeNinkiRankList.Add(4, 1.0f); //まるっともらえる
    }

    //トーナメント形式の賞品設定　選手名はContestStartListDBで決める
    void PrizeSet01()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //トーナメント形式では使わない　boss_scoreに直接いれるため
        //相手の点数リスト 5位から順番に入れる
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(60);
        GameMgr.PrizeScoreAreaList.Add(120);
        GameMgr.PrizeScoreAreaList.Add(180);
        GameMgr.PrizeScoreAreaList.Add(240);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる 最下位から順番に入れる
        /*GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");*/
    }

    void PrizeSet02()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);
    }

    void PrizeSet03()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);
    }

    void PrizeSet04()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);
    }
    //




    //
    //ランキング形式
    //
    void PrizeRankingSet01()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位 ↓
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(112);        

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チューン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポットマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet02()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet03()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet04()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet05()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet06()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet07()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet08()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet09()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet20()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet21()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet22()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet23()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet24()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet25()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet26()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet27()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet40()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet41()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet42()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet43()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet44()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet45()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet46()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet47()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet60()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet61()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet62()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet63()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet64()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet65()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet66()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet67()
    {
        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。5位から順番に入れる
        GameMgr.PrizeItemList.Clear();
        GameMgr.PrizeItemList.Add("nuts"); //5位
        GameMgr.PrizeItemList.Add("ice_box");
        GameMgr.PrizeItemList.Add("neko_badge2");
        GameMgr.PrizeItemList.Add("whisk_magic");
        GameMgr.PrizeItemList.Add("gold_oven");

        //賞金リスト 5位から順番に入れる
        GameMgr.PrizeGetMoneyList.Clear();
        GameMgr.PrizeGetMoneyList.Add(0);
        GameMgr.PrizeGetMoneyList.Add(100);
        GameMgr.PrizeGetMoneyList.Add(500);
        GameMgr.PrizeGetMoneyList.Add(1000);
        GameMgr.PrizeGetMoneyList.Add(3000);

        //相手の点数リスト
        GameMgr.PrizeScoreAreaList.Clear();
        GameMgr.PrizeScoreAreaList.Add(30);
        GameMgr.PrizeScoreAreaList.Add(56);
        GameMgr.PrizeScoreAreaList.Add(83);
        GameMgr.PrizeScoreAreaList.Add(92);

        //参加者名リスト(上位4人) + 5人目がアキラくんになる
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー・ポートマン");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }
}