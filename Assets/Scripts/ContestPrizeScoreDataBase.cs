using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContestPrizeScoreDataBase : SingletonMonoBehaviour<ContestPrizeScoreDataBase>
{

    //コンテスト　ランク
    private Dictionary<int, string> PrizeRankList = new Dictionary<int, string>();

    private PlayerItemList pitemlist;
    private ItemDataBase database;

    private int i;

    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。 

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        PrizeRankDict();
    }

    void Update()
    {

    }

    public void OnPrizeListSet(int _ContestSelectNum)
    {
        switch(_ContestSelectNum)
        {
            case 1000:

                PrizeSet01();
                break;
        }
        
    }

    public void OnPrizeListRankingSet(int _ContestSelectNum)
    {
        switch (_ContestSelectNum)
        {

            case 10000:

                PrizeRankingSet01();
                break;
        }

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
                    PlayerStatus.player_money += GameMgr.PrizeGetMoneyList[i];
                    GameMgr.Contest_PrizeGet_Money = GameMgr.PrizeGetMoneyList[i];
                    Debug.Log("ランク: " + PrizeRankList[i]);
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
                        PlayerStatus.player_money += GameMgr.PrizeGetMoneyList[i];
                        GameMgr.Contest_PrizeGet_Money = GameMgr.PrizeGetMoneyList[i];
                        Debug.Log("ランク: " + PrizeRankList[i]);
                        break;
                    }
                }
                else //リストの一番最後
                {
                    if (GameMgr.contest_PrizeScore >= GameMgr.PrizeScoreAreaList[i - 1])
                    {
                        pitemlist.addPlayerItemString(GameMgr.PrizeItemList[i], 1);
                        GameMgr.Contest_PrizeGet_ItemName = database.items[database.SearchItemIDString(GameMgr.PrizeItemList[i])].itemNameHyouji;
                        PlayerStatus.player_money += GameMgr.PrizeGetMoneyList[i];
                        GameMgr.Contest_PrizeGet_Money = GameMgr.PrizeGetMoneyList[i];
                        Debug.Log("ランク: " + PrizeRankList[i]);
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
        GameMgr.PrizeCharacterList.Clear();
        GameMgr.PrizeCharacterList.Add("アマクサ");
        GameMgr.PrizeCharacterList.Add("ジャッキー・チェン");
        GameMgr.PrizeCharacterList.Add("ナタリー");
        GameMgr.PrizeCharacterList.Add("ハーマイオニー");
    }

    void PrizeRankingSet01()
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