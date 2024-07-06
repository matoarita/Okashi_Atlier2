using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buf_Power_Keisan : SingletonMonoBehaviour<Buf_Power_Keisan>
{
    private PlayerItemList pitemlist;
    private HikariOkashiExpTable hikariOkashiExpTable;

    private ItemDataBase database;
    private MagicSkillListDataBase magicskill_database;

    private int _buf_findpower;
    private int _buf_kakuritsuup;
    private float _buf_kakuritsuup_f;
    private int _buf_shokukanup;
    private int _magicup;
    private int _magic_attri;

    private float _buf_hikari_okashiparam;
    private float _buf_hikari_okashi_paramup;
    private int hikari_okashiLV;

    private int i;
    private int _id;
    private string _itemType_sub;
    private string _itemType_subB;

    // Use this for initialization
    void Start () {

        InitSetup();
    }

    void InitSetup()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //ヒカリお菓子EXPデータベースの取得
        hikariOkashiExpTable = HikariOkashiExpTable.Instance.GetComponent<HikariOkashiExpTable>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //
    //アイテム発見力のバフ
    //
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



    //
    //調合成功率のバフ
    //調合で生成されるアイテムの_itemType_subを指定し、中に補正値をかけばOK
    //
    public int Buf_CompKakuritsu_Keisan(string _result_item)
    {
        _buf_kakuritsuup = 0;

        //アイテムによって、特定のお菓子のときのみ成功率をあげる。
        _id = database.SearchItemIDString(_result_item);
        _itemType_sub = database.items[_id].itemType_sub.ToString();

        
        switch (_itemType_sub)
        {
            case "Cookie":

                //めん棒系
                if (pitemlist.KosuCount("wood_rod_doillan") >= 1)
                {
                    _buf_kakuritsuup += 15;
                }
                else
                {
                    if (pitemlist.KosuCount("wood_rod_great") >= 1)
                    {
                        _buf_kakuritsuup += 12;
                    }
                    else
                    {
                        if (pitemlist.KosuCount("wood_rod_good") >= 1)
                        {
                            _buf_kakuritsuup += 8;
                        }
                        else
                        {
                            if (pitemlist.KosuCount("wood_rod_normal") >= 1)
                            {
                                _buf_kakuritsuup += 5;
                            }
                            else
                            {
                                if (pitemlist.KosuCount("wood_rod_boro") >= 1)
                                {
                                    _buf_kakuritsuup += 2;
                                }
                            }
                        }
                    }
                }
                break;

            case "Cookie_Hard":

                //めん棒系
                if (pitemlist.KosuCount("wood_rod_doillan") >= 1)
                {
                    _buf_kakuritsuup += 15;
                }
                else
                {
                    if (pitemlist.KosuCount("wood_rod_great") >= 1)
                    {
                        _buf_kakuritsuup += 12;
                    }
                    else
                    {
                        if (pitemlist.KosuCount("wood_rod_good") >= 1)
                        {
                            _buf_kakuritsuup += 8;
                        }
                        else
                        {
                            if (pitemlist.KosuCount("wood_rod_normal") >= 1)
                            {
                                _buf_kakuritsuup += 5;
                            }
                            else
                            {
                                if (pitemlist.KosuCount("wood_rod_boro") >= 1)
                                {
                                    _buf_kakuritsuup += 2;
                                }
                            }
                        }
                    }
                }
                break;
        }

        if (pitemlist.KosuCount("green_pendant") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup += 10;
        }
        /*if (pitemlist.KosuCount("maneki_cat") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup += 5;
        }*/

        //かまどレベルによるバフ
        if (pitemlist.KosuCount("platinum_oven") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup += 30;
        }
        else
        {
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
        }


        return _buf_kakuritsuup;
    }




    //
    //仕送り額のバフ
    //
    public float Buf_CompFatherMoneyUp_Keisan()
    {
        _buf_kakuritsuup_f = 1.0f;


        if (pitemlist.KosuCount("star_pendant") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup_f *= 1.3f;
        }


        return _buf_kakuritsuup_f;
    }



    //
    //食感などのパラメータのバフ これのみ、ゲームスタート前に一度読み込む可能性あるので、アイテムリストを取得
    //アイテムのサブタイプ(_itemType_sub)を指定し、中で補正をかければOK
    //
    public int Buf_OkashiParamUp_Keisan(int _status, string _itemType_sub)
    {
        InitSetup();

        _buf_shokukanup = 0;

        switch (_status)
        {
            case 0: //さくさく感のバフ

                switch (_itemType_sub)
                {
                    case "Appaleil":

                        CreamBuf();
                        break;

                    case "Bread":

                        OvenBuf();
                        break;

                    case "Cookie":

                        OvenBuf();
                        CookieBuf();
                        break;

                    case "Cookie_Hard":

                        OvenBuf();
                        CookieBuf();
                        break;

                    case "Rusk":

                        OvenBuf();
                        RuskBuf();
                        break;
                    
                }

                AllShokukanBuf();

                return _buf_shokukanup;

            case 1: //ふわふわ感のバフ

                switch (_itemType_sub)
                {
                    case "Appaleil":

                        CreamBuf();
                        break;

                    case "Crepe":

                        CrepeBuf();
                        break;

                    case "Crepe_Mat":
                        CrepeBuf();
                        break;

                    case "Creampuff":

                        OvenBuf();
                        break;

                    case "Cake_Mat":

                        OvenBuf();
                        CakeBuf();
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

                AllShokukanBuf();

                return _buf_shokukanup;

            case 2: //なめらか感のバフ

                switch (_itemType_sub)
                {
                    case "Appaleil":

                        CreamBuf();
                        break;
                }

                AllShokukanBuf();

                return _buf_shokukanup;

            case 3: //歯ごたえ感のバフ

                switch (_itemType_sub)
                {
                    case "Biscotti":

                        OvenBuf();
                        break;

                    case "Candy":
                        CandyBuf();
                        break;

                    case "Cookie_Hard":

                        OvenBuf();
                        break;
                }

                AllShokukanBuf();

                return _buf_shokukanup;

            case 4: //ジュースのバフ

                AllShokukanBuf();

                return _buf_shokukanup;

            case 5: //見た目のバフ

                return _buf_shokukanup;

            case 6: //香りのバフ

                switch (_itemType_sub)
                {
                    case "Tea":

                        TeaBuf();
                        break;

                    case "Tea_Mat":

                        TeaBuf();
                        break;

                    case "Tea_Potion":

                        TeaBuf();
                        break;
                }

                return _buf_shokukanup;
        }

        return 0; //なにもない場合や例外は0
    }




    void CreamBuf()
    {
        if (pitemlist.KosuCount("whisk_gold") >= 1) //魔力の泡だて器をもっている
        {
            _buf_shokukanup = (int)(_buf_shokukanup * 2.2f);
        }
        else
        {
            if (pitemlist.KosuCount("whisk_silver") >= 1) //魔力の泡だて器をもっている
            {
                _buf_shokukanup = (int)(_buf_shokukanup * 1.5f);
            }
            else
            {
                if (pitemlist.KosuCount("whisk_magic") >= 1) //魔力の泡だて器をもっている
                {
                    _buf_shokukanup = (int)(_buf_shokukanup * 1.3f);
                }
            }
        }
    }

    void OvenBuf()
    {
        // かまどレベルによるバフ
        if (PlayerStatus.player_kamado_lv >= 4) //持ってるだけで効果アップ
        {
            _buf_shokukanup += 200;
        }
        else
        {
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
    }

    void CookieBuf()
    {
        //めん棒系
        if (pitemlist.KosuCount("wood_rod_doillan") >= 1)
        {
            _buf_shokukanup += 120;
        }
        else
        {
            if (pitemlist.KosuCount("wood_rod_great") >= 1)
            {
                _buf_shokukanup += 80;
            }
            else
            {
                if (pitemlist.KosuCount("wood_rod_good") >= 1)
                {
                    _buf_shokukanup += 40;
                }
                else
                {
                    if (pitemlist.KosuCount("wood_rod_normal") >= 1)
                    {
                        _buf_shokukanup += 20;
                    }
                    else
                    {
                        if (pitemlist.KosuCount("wood_rod_boro") >= 1)
                        {
                            _buf_shokukanup += 5;
                        }
                    }
                }
            }
        }

        if (pitemlist.KosuCount("cookie_powerup1") >= 1) //
        {
            _buf_shokukanup += 5;
        }
        if (pitemlist.KosuCount("cookie_powerup2") >= 1) //
        {
            _buf_shokukanup += 10;
        }
        if (pitemlist.KosuCount("cookie_powerup3") >= 1) //
        {
            _buf_shokukanup += 15;
        }
        if (pitemlist.KosuCount("cookie_powerup4") >= 1) //
        {
            _buf_shokukanup += 25;
        }
        if (pitemlist.KosuCount("cookie_powerup5") >= 1) //
        {
            _buf_shokukanup += 50;
        }

        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Cookie_Study") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Cookie_Study") * 5; //LV*5
            _buf_shokukanup += _magicup;
        }

    }

    void RuskBuf()
    {
        
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Cookie_Study") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Cookie_Study") * 5;
            _buf_shokukanup += _magicup;
        }

    }

    void CandyBuf()
    {
        if (pitemlist.KosuCount("candy_powerup1") >= 1) //
        {
            _buf_shokukanup += 20;
        }
    }

    void CrepeBuf()
    {
        if (pitemlist.KosuCount("crepe_powerup1") >= 1) //
        {
            _buf_shokukanup += 5;
        }
        if (pitemlist.KosuCount("crepe_powerup2") >= 1) //
        {
            _buf_shokukanup += 10;
        }
        if (pitemlist.KosuCount("crepe_powerup3") >= 1) //
        {
            _buf_shokukanup += 15;
        }
        if (pitemlist.KosuCount("crepe_powerup4") >= 1) //
        {
            _buf_shokukanup += 25;
        }
        if (pitemlist.KosuCount("crepe_powerup5") >= 1) //
        {
            _buf_shokukanup += 50;
        }
    }

    void CakeBuf()
    {

        if (pitemlist.KosuCount("cakemold_stainless") >= 1) //
        {
            _buf_shokukanup = (int)(_buf_shokukanup * 2.5f);
        }
        else
        {
            if (pitemlist.KosuCount("cakemold_black") >= 1) //ケーキ型ブラック
            {
                _buf_shokukanup = (int)(_buf_shokukanup * 1.2f);
            }
        }

    }

    void TeaBuf()
    {
        if (pitemlist.KosuCount("tea_powerup1") >= 1) //
        {
            _buf_shokukanup += 5;
        }
        if (pitemlist.KosuCount("tea_powerup2") >= 1) //
        {
            _buf_shokukanup += 10;
        }
        if (pitemlist.KosuCount("tea_powerup3") >= 1) //
        {
            _buf_shokukanup += 20;
        }
        if (pitemlist.KosuCount("tea_powerup4") >= 1) //
        {
            _buf_shokukanup += 30;
        }
        if (pitemlist.KosuCount("tea_powerup5") >= 1) //
        {
            _buf_shokukanup += 50;
        }
    }

    void AllShokukanBuf()
    {
        if (pitemlist.KosuCount("shokukan_powerup1") >= 1) //
        {
            _buf_shokukanup += 10;
        }
        if (pitemlist.KosuCount("shokukan_powerup2") >= 1) //
        {
            _buf_shokukanup += 25;
        }
        if (pitemlist.KosuCount("shokukan_powerup3") >= 1) //
        {
            _buf_shokukanup += 50;
        }
    }

    //特定のアイテムにのみ、バフをかける処理
    //特定のお菓子の名前を指定し、どの食感(_status)に補正をかけるか指定して、書き込めばOK
    public int Buf_OkashiParamUp_ItemNameKeisan(int _status, string _basename)
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        _buf_shokukanup = 0;

        if (pitemlist.KosuCount("otona_powerup1") >= 1) //
        {
            if (_basename == "cannoli" || _basename == "tiramisu" || _basename == "sea_losanonos" || _basename == "cream_coffee"
                            || _basename == "cafeaulait_creampuff" || _basename == "cocoa_cookie" || _basename == "biscotti")
            {
                switch (_status)
                {
                    case 0: //さくさく感のバフ

                        if (_basename == "cannoli" || _basename == "sea_losanonos" || _basename == "cream_coffee" || _basename == "cocoa_cookie")
                        {
                            _buf_shokukanup += 15;
                        }

                        return _buf_shokukanup;

                    case 1: //ふわふわ感のバフ

                        if (_basename == "tiramisu" || _basename == "cafeaulait_creampuff")
                        {
                            _buf_shokukanup += 15;
                        }

                        return _buf_shokukanup;

                    case 2: //なめらか感のバフ

                        break;

                    case 3: //歯ごたえ感のバフ

                        if (_basename == "biscotti")
                        {
                            _buf_shokukanup += 15;
                        }

                        return _buf_shokukanup;

                    case 4: //ジュースのバフ

                        break;

                    case 5: //見た目のバフ

                        break;

                    case 6: //香りのバフ

                        break;
                }                
            }            
        }

        return _buf_shokukanup;
    }

    //特定の調合処理にのみ、バフをかける処理
    //コンポ調合の名前を直接指定して、どの食感(_status)に補正をかけるか指定して、書き込めばOK
    public int Buf_OkashiParamUp_CompoNameKeisan(int _status, string _componame)
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        _buf_shokukanup = 0;

        switch (_componame)
        {
            case "whipped cream_row":

                _buf_shokukanup = (int)(_buf_shokukanup * 1.3f);
                break;

            case "whipped cream_row_Free":

                _buf_shokukanup = (int)(_buf_shokukanup * 1.3f);
                break;

            case "cream_row_ricotta":

                _buf_shokukanup = (int)(_buf_shokukanup * 1.3f);
                break;

            case "blacklotus_sponge_cake_sliced": //パンナイフでスポンジケーキを切ったとき　マイナスになる。

                if (_status == 5)//見た目のバフ
                {
                    _buf_shokukanup -= 50;
                    return _buf_shokukanup;
                }
                break;
        }

        return _buf_shokukanup;
    }

    //特定の魔法で、バフをかける処理
    //魔法の名前を直接指定して、どの食感(_status)に補正をかけるか指定して、書き込めばOK
    public int Buf_OkashiParamUp_MagicKeisan(int _status, string _magicname)
    {

        _buf_shokukanup = 0;
        _magicup = 0;

        switch (_magicname)
        {
            case "Cookie_SecondBake":

                if (_status == 0)//さくさくのバフ
                {
                    _magicup = magicskill_database.skillName_SearchLearnLevel("Cookie_SecondBake") * 30;
                    _buf_shokukanup += _magicup;
                }
                break;

        }

        return _buf_shokukanup;
    }

    //魔法によって状態が変わる
    public int Buf_OkashiAttribute_Magic(string _magicname)
    {
        _magic_attri = 0;

        switch (_magicname)
        {
            case "Cookie_SecondBake":

                _magic_attri = 1; //二度焼きしたというフラグ
                break;

        }

        return _magic_attri;
    }



    //ヒカリの作ったお菓子に、バフをかける処理
    public int Buf_HikariParamUp_Keisan(int _status, string _itemType_sub)
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        _buf_shokukanup = 0;

        if (pitemlist.KosuCount("hikari_powerup1") >= 1) //
        {
            _buf_shokukanup += 10;
        }
        if (pitemlist.KosuCount("hikari_powerup2") >= 1) //
        {
            _buf_shokukanup += 20;
        }
        if (pitemlist.KosuCount("hikari_powerup3") >= 1) //
        {
            _buf_shokukanup += 30;
        }

        return _buf_shokukanup;
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
        if (pitemlist.KosuCount("platinum_oven") >= 1) //持ってるだけで効果アップ
        {
            PlayerStatus.player_kamado_lv = 4;
        }
        else
        {
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
    }

    //作るお菓子の種類によって、ヒカリのお菓子レベルの補正をかける。
    public float Buf_HikariOkashiLV_Keisan(string _itemType_sub)
    {
        _buf_hikari_okashiparam = 1.0f;
        
        hikariBuf_okashilv(_itemType_sub);

        Debug.Log("ヒカリのバフ補正値　_buf_hikari_okashiparam: " + _buf_hikari_okashiparam);
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
        //食感への補正
        _buf_hikari_okashiparam = 0.1f + SujiMap(hikari_okashiLV, 1.0f, 9.0f, 0.8f, 1.5f);

        //最終的にかかる時間は、Exp_Controllerで計算
        GameMgr.hikari_make_okashiTime_costbuf = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 1.1f, 0.3f); //LV1~9 を　3~1倍に変換。LV9で、通常の兄ちゃんの速度の3倍

        if (pitemlist.KosuCount("hikari_speed_up2") >= 1) //持ってるだけで効果アップ
        {
            GameMgr.hikari_make_okashiTime_costbuf = GameMgr.hikari_make_okashiTime_costbuf * 0.5f;
        }
        else if (pitemlist.KosuCount("hikari_speed_up1") >= 1) //
        {
            GameMgr.hikari_make_okashiTime_costbuf = GameMgr.hikari_make_okashiTime_costbuf　* 0.75f;
        }

        //最終的な成功率は、Compound_Checkで計算
        GameMgr.hikari_make_okashiTime_successrate_buf = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 0.8f, 1.5f); //成功率　LV1~9 を　0.8から1.5に変換。

        if (pitemlist.KosuCount("green_pendant") >= 1) //持ってるだけで効果アップ
        {
            GameMgr.hikari_make_okashiTime_successrate_buf += 0.1f;
        }

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

        Debug.Log("GameMgr.hikari_make_okashiTime_successrate_buf: " + GameMgr.hikari_make_okashiTime_successrate_buf + " " + "hikari_okashiLV: " + hikari_okashiLV);
    }

    //ヒカリのお菓子レベルに応じて、にいちゃんが作るお菓子のパラメータにもバフがかかる計算。
    public float Buf_HikariOkashiLV_HoseiParamUp(string _itemType_sub)
    {
        _buf_hikari_okashi_paramup = 1.0f;

        //ヒカリお菓子Expテーブルを起動
        hikariOkashiExpTable.hikariOkashi_ExpTableMethod(_itemType_sub, 0, 0, 1);

        if (GameMgr.hikariokashiExpTable_noTypeflag)
        {
            //どのお菓子タイプにもあてはまらなかったら、計算しない。
            _buf_hikari_okashi_paramup = 1.0f;
        }
        else
        {
            hikari_okashiLV = GameMgr.hikarimakeokashi_nowlv;
            _buf_hikari_okashi_paramup = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 1.0f, 1.3f); //LV1~9までで、1.0~1.3倍まで上昇
        }

        return _buf_hikari_okashi_paramup;
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
