using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HikariOkashiExpTable : SingletonMonoBehaviour<HikariOkashiExpTable>
{

    private int i;
    private int _getexp;
    private int _nowexp, _nowlv;
    private string _itemType_subtext;

    private ItemSubTypeSetDatabase itemsubtypeset_database;

    private int Type_Num;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void hikariOkashi_ExpTableMethod(string _itemType_sub, int _getExp, int _status, int _mode) 
    {
        GameMgr.hikariokashiExpTable_noTypeflag = false;

        itemsubtypeset_database = ItemSubTypeSetDatabase.Instance.GetComponent<ItemSubTypeSetDatabase>();

        //モード説明
        //_mode=0, ヒカリがお菓子作った際のお菓子レベル計算
        //_mode=1, Buf_Power_Keisanでバフ計算するときに使用。

        itemsubtypeset_database.SetImageSub(_itemType_sub);
        

        if(GameMgr.Item_OkashiSubType_Num == 99)
        {
            GameMgr.hikariokashiExpTable_noTypeflag = true;
        }

        switch (_mode)
        {
            case 0:

                LVKeisanMethod(GameMgr.Item_OkashiSubType_Num, _status, _getExp);
                break;

            case 1:

                NowLvSetting(GameMgr.Item_OkashiSubType_Num);
                break;
        }
    }

    void LVKeisanMethod(int Type_num, int _status, int _getExp)
    {
        _getexp = _getExp;

        switch (Type_num)
        {
            case 0: //生地

                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                else
                {
                    _getexp = _getExp;
                }
                PlayerStatus.player_girl_appaleil_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_appaleil_exp;
                _nowlv = PlayerStatus.player_girl_appaleil_lv;
                Check_OkashilvUP2();
                PlayerStatus.player_girl_appaleil_exp = _nowexp;
                PlayerStatus.player_girl_appaleil_lv = _nowlv;
                _itemType_subtext = "生地";
                break;

            case 1: //クッキー

                PlayerStatus.player_girl_cookie_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cookie_exp;
                _nowlv = PlayerStatus.player_girl_cookie_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cookie_exp = _nowexp;
                PlayerStatus.player_girl_cookie_lv = _nowlv;
                _itemType_subtext = "クッキー";
                break;

            case 2: //チョコレート

                PlayerStatus.player_girl_chocolate_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_chocolate_exp;
                _nowlv = PlayerStatus.player_girl_chocolate_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_chocolate_exp = _nowexp;
                PlayerStatus.player_girl_chocolate_lv = _nowlv;
                _itemType_subtext = "チョコレート";
                break;

            case 3: //キャンディ

                PlayerStatus.player_girl_candy_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_candy_exp;
                _nowlv = PlayerStatus.player_girl_candy_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_candy_exp = _nowexp;
                PlayerStatus.player_girl_candy_lv = _nowlv;
                _itemType_subtext = "キャンディ";
                break;

            case 4: //クレープ

                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                else
                {
                    _getexp = _getExp;
                }
                PlayerStatus.player_girl_crepe_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_crepe_exp;
                _nowlv = PlayerStatus.player_girl_crepe_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_crepe_exp = _nowexp;
                PlayerStatus.player_girl_crepe_lv = _nowlv;
                _itemType_subtext = "クレープ";
                break;

            case 5: //シュークリーム

                PlayerStatus.player_girl_creampuff_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_creampuff_exp;
                _nowlv = PlayerStatus.player_girl_creampuff_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_creampuff_exp = _nowexp;
                PlayerStatus.player_girl_creampuff_lv = _nowlv;
                _itemType_subtext = "シュークリーム";
                break;

            case 6: //ドーナツ

                PlayerStatus.player_girl_donuts_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_donuts_exp;
                _nowlv = PlayerStatus.player_girl_donuts_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_donuts_exp = _nowexp;
                PlayerStatus.player_girl_donuts_lv = _nowlv;
                _itemType_subtext = "ドーナツ";

                break;
            case 7: //ジュース

                PlayerStatus.player_girl_juice_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_juice_exp;
                _nowlv = PlayerStatus.player_girl_juice_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_juice_exp = _nowexp;
                PlayerStatus.player_girl_juice_lv = _nowlv;
                _itemType_subtext = "ジュース";
                break;

            case 8: //ゼリー

                PlayerStatus.player_girl_jelly_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_jelly_exp;
                _nowlv = PlayerStatus.player_girl_jelly_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_jelly_exp = _nowexp;
                PlayerStatus.player_girl_jelly_lv = _nowlv;
                _itemType_subtext = "ゼリー";
                break;

            case 9: //アイス

                PlayerStatus.player_girl_icecream_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_icecream_exp;
                _nowlv = PlayerStatus.player_girl_icecream_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_icecream_exp = _nowexp;
                PlayerStatus.player_girl_icecream_lv = _nowlv;
                _itemType_subtext = "アイス";
                break;

            case 10: //ケーキ

                PlayerStatus.player_girl_cake_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cake_exp;
                _nowlv = PlayerStatus.player_girl_cake_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cake_exp = _nowexp;
                PlayerStatus.player_girl_cake_lv = _nowlv;
                _itemType_subtext = "ケーキ";
                break;

            case 11: //ラスク

                PlayerStatus.player_girl_rusk_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_rusk_exp;
                _nowlv = PlayerStatus.player_girl_rusk_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_rusk_exp = _nowexp;
                PlayerStatus.player_girl_rusk_lv = _nowlv;
                _itemType_subtext = "ラスク";
                break;

            case 12: //レアお菓子系

                PlayerStatus.player_girl_rareokashi_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_rareokashi_exp;
                _nowlv = PlayerStatus.player_girl_rareokashi_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_rareokashi_exp = _nowexp;
                PlayerStatus.player_girl_rareokashi_lv = _nowlv;
                _itemType_subtext = "高級";
                break;

            case 13: //ケーキ生地

                if (_status == 0)
                {
                    _getexp = 3 * GameMgr.hikari_make_okashiKosu;
                }
                else
                {
                    _getexp = _getExp;
                }
                PlayerStatus.player_girl_cake_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cake_exp;
                _nowlv = PlayerStatus.player_girl_cake_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cake_exp = _nowexp;
                PlayerStatus.player_girl_cake_lv = _nowlv;
                _itemType_subtext = "ケーキ";
                break;

            case 14: //パン系（ラスク扱いなので11とほぼ内容同じ）

                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                else
                {
                    _getexp = _getExp;
                }
                PlayerStatus.player_girl_rusk_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_rusk_exp;
                _nowlv = PlayerStatus.player_girl_rusk_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_rusk_exp = _nowexp;
                PlayerStatus.player_girl_rusk_lv = _nowlv;
                _itemType_subtext = "ラスク";
                break;

            case 15: //クリーム

                if (_status == 0)
                {
                    _getexp = 2 * GameMgr.hikari_make_okashiKosu;
                }
                else
                {
                    _getexp = _getExp;
                }
                PlayerStatus.player_girl_cream_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_cream_exp;
                _nowlv = PlayerStatus.player_girl_cream_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_cream_exp = _nowexp;
                PlayerStatus.player_girl_cream_lv = _nowlv;
                _itemType_subtext = "クリーム";
                break;

            case 20: //お茶系

                PlayerStatus.player_girl_tea_exp += _getexp;
                _nowexp = PlayerStatus.player_girl_tea_exp;
                _nowlv = PlayerStatus.player_girl_tea_lv;
                Check_OkashilvUP();
                PlayerStatus.player_girl_tea_exp = _nowexp;
                PlayerStatus.player_girl_tea_lv = _nowlv;
                _itemType_subtext = "ティー";
                break;
        }

        GameMgr.hikarimakeokashi_itemTypeSub_nameHyouji = _itemType_subtext;
        GameMgr.hikarimakeokashi_nowlv = _nowlv;
        GameMgr.hikarimakeokashi_finalgetexp = _getexp;
    }

    void NowLvSetting(int _status) //BufPowerKeisan用
    {
        switch (_status)
        {
            case 0: //生地

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_appaleil_lv;
                break;

            case 1: //クッキー

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_cookie_lv;
                break;

            case 2: //チョコレート

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_chocolate_lv;
                break;

            case 3: //キャンディ

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_candy_lv;
                break;

            case 4: //クレープ

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_crepe_lv;
                break;

            case 5: //シュークリーム

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_creampuff_lv;
                break;

            case 6: //ドーナツ

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_donuts_lv;
                break;

            case 7: //ジュース

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_juice_lv;
                break;

            case 8: //ゼリー

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_jelly_lv;
                break;

            case 9: //アイス

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_icecream_lv;
                break;

            case 10: //ケーキ

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_cake_lv;
                break;

            case 11: //ラスク

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_rusk_lv;
                break;

            case 12: //レアお菓子系

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_rareokashi_lv;
                break;

            case 13: //ケーキ生地

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_cake_lv;
                break;

            case 14: //パン系（ラスク扱い）

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_rusk_lv;
                break;

            case 15: //クリーム

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_cream_lv;
                break;

            case 20: //お茶系

                GameMgr.hikarimakeokashi_nowlv = PlayerStatus.player_girl_tea_lv;
                break;

            case 99:
                
                break;
        }

    }

    void Check_OkashilvUP()
    {

        if (_nowlv >= 9) //9がカンスト
        {
            _nowexp = 0;
        }
        else
        {
            if (_nowexp >= GameMgr.Hikariokashi_Exptable[_nowlv])
            {
                _nowexp = 0;
                _nowlv++;
            }
        }
    }

    //少し難しめのお菓子の経験テーブル
    void Check_OkashilvUP2()
    {

        if (_nowlv >= 9) //9がカンスト
        { }
        else
        {
            if (_nowexp >= GameMgr.Hikariokashi_Exptable2[_nowlv])
            {
                _nowexp = 0;
                _nowlv++;
            }
        }
    }

}
