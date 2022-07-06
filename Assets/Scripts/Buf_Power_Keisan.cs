using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buf_Power_Keisan : SingletonMonoBehaviour<Buf_Power_Keisan>
{
    private PlayerItemList pitemlist;
    private HikariOkashiExpTable hikariOkashiExpTable;

    private int _buf_findpower;
    private int _buf_kakuritsuup;
    private float _buf_kakuritsuup_f;
    private int _buf_shokukanup;

    private float _buf_hikari_okashiparam;
    private float _buf_hikari_okashi_paramup;
    private int hikari_okashiLV;

    private int i;

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //ヒカリお菓子EXPデータベースの取得
        hikariOkashiExpTable = HikariOkashiExpTable.Instance.GetComponent<HikariOkashiExpTable>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //アイテム発見力のバフ
    public int Buf_findpower_Keisan()
    {
        _buf_findpower = 0;

        /*for (i = 0; i < GameMgr.CollectionItemsName.Count; i++)
        {
            if (GameMgr.CollectionItemsName[i] == "aquamarine_pendant" && GameMgr.CollectionItems[i] == true) //コレクションが登録されていれば、アイテム発見力発動
            {
                _buf_findpower += 10;
            }
        }*/
      
        if (pitemlist.KosuCount("aquamarine_pendant") >= 1) //持ってるだけで効果アップ
        {
            _buf_findpower += 100;
        }

        if (pitemlist.KosuCount("compass") >= 1) //持ってるだけで効果アップ
        {
            _buf_findpower += 50;
        }

        return _buf_findpower;
    }

    //調合成功率のバフ
    public int Buf_CompKakuritsu_Keisan()
    {
        _buf_kakuritsuup = 0;

        /*for(i=0; i < GameMgr.CollectionItemsName.Count; i++)
        {
            if (GameMgr.CollectionItemsName[i] == "green_pendant" && GameMgr.CollectionItems[i] == true) //コレクションが登録されていれば、確率アップ効果発動
            {
                _buf_kakuritsuup += 30;
            }

            if (GameMgr.CollectionItemsName[i] == "star_pendant" && GameMgr.CollectionItems[i] == true) //コレクションが登録されていれば、確率アップ効果発動
            {
                _buf_kakuritsuup += 10;
            }
        }*/

        if (pitemlist.KosuCount("green_pendant") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup += 5;
        }
        /*if (pitemlist.KosuCount("maneki_cat") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup += 5;
        }*/

        //かまどレベルによるバフ
        if (pitemlist.KosuCount("gold_oven") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup += 20;
        }
        else
        {
            if (pitemlist.KosuCount("silver_oven") >= 1) //持ってるだけで効果アップ
            {
                _buf_kakuritsuup += 10;
            }
        }


        return _buf_kakuritsuup;
    }

    //仕送り額のバフ
    public float Buf_CompFatherMoneyUp_Keisan()
    {
        _buf_kakuritsuup_f = 1.0f;


        if (pitemlist.KosuCount("star_pendant") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup_f *= 1.3f;
        }


        return _buf_kakuritsuup_f;
    }

    //食感などのパラメータのバフ これのみ、ゲームスタート前に一度読み込む可能性あるので、アイテムリストを取得
    public int Buf_OkashiParamUp_Keisan(int _status, string _itemType_sub)
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        _buf_shokukanup = 0;

        switch (_status)
        {
            case 0: //さくさく感のバフ

                // かまどレベルによるバフ
                switch (_itemType_sub)
                {
                    case "Bread":

                        OvenBuf();
                        break;

                    case "Cookie":

                        OvenBuf();
                        break;

                    /*case "Cookie_Mat":

                        OvenBuf();
                        break;*/

                    case "Rusk":

                        OvenBuf();
                        break;
                }
                
                return _buf_shokukanup;

            case 1: //ふわふわ感のバフ

                // かまどレベルによるバフ
                switch (_itemType_sub)
                {
                    case "Creampuff":

                        OvenBuf();
                        break;

                    case "Cake_Mat":

                        OvenBuf();
                        break;

                    case "Financier":

                        OvenBuf();
                        break;

                    case "Maffin":

                        OvenBuf();
                        break;

                    case "Castella":

                        OvenBuf();
                        break;
                }

                return _buf_shokukanup;

            case 2: //なめらか感のバフ

                break;

            case 3: //歯ごたえ感のバフ

                // かまどレベルによるバフ
                switch (_itemType_sub)
                {
                    case "Biscotti":

                        OvenBuf();
                        break;

                    case "Cookie_Hard":

                        OvenBuf();
                        break;
                }

                return _buf_shokukanup;
        }

        return 0; //なにもない場合や例外は0
    }

    void OvenBuf()
    {
        // かまどレベルによるバフ
        if (PlayerStatus.player_kamado_lv >= 3) //持ってるだけで効果アップ
        {
            _buf_shokukanup += 150;
        }
        else
        {
            if (PlayerStatus.player_kamado_lv >= 2) //持ってるだけで効果アップ
            {
                _buf_shokukanup += 75;
            }
            else
            {
                _buf_shokukanup = 0;
            }
        }
    }


    //メイン調合シーンで確認する
    public void CheckEquip_Keisan()
    {
        //かごレベル
        PlayerStatus.player_zairyobox_lv = 1;

        //条件分岐
        if (pitemlist.KosuCount("itembox_1") >= 1) //アイテムかごLv2
        {
            if (PlayerStatus.player_zairyobox_lv < 2) //LV3とか買った後で買っても、持てる量は更新されない。
            {
                PlayerStatus.player_zairyobox = 10;
                PlayerStatus.player_zairyobox_lv = 2;
            }
        }
        if (pitemlist.KosuCount("itembox_2") >= 1) //アイテムかごLv3
        {
            if (PlayerStatus.player_zairyobox_lv < 3)
            {
                PlayerStatus.player_zairyobox = 20;
                PlayerStatus.player_zairyobox_lv = 3;
            }
        }
        if (pitemlist.KosuCount("itembox_3") >= 1) //アイテムかごLv4
        {
            if (PlayerStatus.player_zairyobox_lv < 4)
            {
                PlayerStatus.player_zairyobox = 30;
                PlayerStatus.player_zairyobox_lv = 4;
            }
        }

        // かまどレベル
        if (pitemlist.KosuCount("gold_oven") >= 1) //持ってるだけで効果アップ
        {
            PlayerStatus.player_kamado_lv = 3;
        }
        else
        {
            if (pitemlist.KosuCount("silver_oven") >= 1) //持ってるだけで効果アップ
            {
                PlayerStatus.player_kamado_lv = 2;
            }
            else
            {
                PlayerStatus.player_kamado_lv = 1;
            }
        }
    }

    //作るお菓子の種類によって、ヒカリのお菓子レベルの補正をかける。
    public float Buf_HikariOkashiLV_Keisan(string _itemType_sub)
    {
        _buf_hikari_okashiparam = 1.0f;
        
        hikariBuf_okashilv(_itemType_sub);

        return _buf_hikari_okashiparam;
    }

    //上記でreturnを使わずに、計算だけしたい場合。直接これを読む。compound_Checkで成功率だすときなどに使用。
    public void hikariBuf_okashilv(string _itemType_sub)
    {
        //ヒカリお菓子Expテーブルを起動
        hikariOkashiExpTable.hikariOkashi_ExpTableMethod(_itemType_sub, 0, 0, 1);
        
        if (GameMgr.hikariokashiExpTable_noTypeflag)
        {
            //どのお菓子タイプにもあてはまらなかったら、計算しない。
            GameMgr.hikari_make_okashiTime_costbuf = 1.0f;
            GameMgr.hikari_make_okashiTime_successrate_buf = 1.0f;
        }
        else
        {
            hikari_okashiLV = GameMgr.hikarimakeokashi_nowlv;
            HikariOkashilv_Keisan(_itemType_sub); //実際のバフ率を計算
        }
        
    }

    void HikariOkashilv_Keisan(string _itemType_sub)
    {
        _buf_hikari_okashiparam = 0.1f + SujiMap(hikari_okashiLV, 1.0f, 9.0f, 0.2f, 1.5f);

        //最終的にかかる時間は、Exp_Controllerで計算
        GameMgr.hikari_make_okashiTime_costbuf = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 3.0f, 0.3f); //LV1~9 を　3~1倍に変換。LV9で、通常の兄ちゃんの速度の2倍

        //最終的な成功率は、Compound_Checkで計算
        GameMgr.hikari_make_okashiTime_successrate_buf = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 0.2f, 1.3f); //成功率　LV1~9 を　0.5から1.1に変換。
        Debug.Log("GameMgr.hikari_make_okashiTime_successrate_buf: " + GameMgr.hikari_make_okashiTime_successrate_buf + " " + "hikari_okashiLV: " + hikari_okashiLV);

        if (GameMgr.hikari_make_okashiTime_costbuf <= 0.1f)
        {
            GameMgr.hikari_make_okashiTime_costbuf = 0.1f;
        }
        //Debug.Log("hikari_okashiLV: " + hikari_okashiLV + " " + "GameMgr.hikari_make_okashiTime_costbuf: " + GameMgr.hikari_make_okashiTime_costbuf);

        //タイプごとの例外処理 生地系は95%になる。
        switch (_itemType_sub)
        {
            case "Appaleil":
                GameMgr.hikari_make_okashiTime_successrate_buf = 0.95f;
                break;
            case "Water":
                GameMgr.hikari_make_okashiTime_successrate_buf = 0.95f;
                break;
            case "Cream":
                GameMgr.hikari_make_okashiTime_successrate_buf = 0.95f;
                break;
        }
    }

    //ヒカリのお菓子レベルにより、お菓子のパラメータにバフがかかる計算。
    public float Buf_HikariOkashiLV_HoseiParamUp(string _itemType_sub)
    {
        _buf_hikari_okashi_paramup = 1.0f;

        //ヒカリお菓子Expテーブルを起動
        hikariOkashiExpTable.hikariOkashi_ExpTableMethod(_itemType_sub, 0, 0, 1);

        if (GameMgr.hikariokashiExpTable_noTypeflag)
        {
            //どのお菓子タイプにもあてはまらなかったら、計算しない。
        }
        else
        {
            hikari_okashiLV = GameMgr.hikarimakeokashi_nowlv;
            _buf_hikari_okashi_paramup = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 1.0f, 1.3f); //LV1~9までで、1~1.3倍まで上昇
        }

        return _buf_hikari_okashi_paramup;
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
