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
    private float _a, _b;

    private int i;
    private int _id;
    private string _itemType;
    private string _itemType_sub;
    private string _itemType_subB;

    private float _tempature_param;
    private float _well_done;
    private float _best_well_done;
    private float _well_done_kyori;
    private float _well_done_kyori_noabs;
    private float _well_done_kyori_hosei;
    private float _yonetsu_hosei;

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
      
        /*if (pitemlist.KosuCount("aquamarine_pendant") >= 1) //持ってるだけで効果アップ
        {
            _buf_findpower += 100;
        }*/

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
        _itemType = database.items[_id].itemType.ToString();
        _itemType_sub = database.items[_id].itemType_sub.ToString();

        
        switch (_itemType_sub)
        {
            case "Cookie":

                //めん棒系
                KakuritsuUp_WoodRod();

                //かまどレベルによるバフ
                KakuritsuUp_Oven();

                break;

            case "Cookie_Hard":

                //めん棒系
                KakuritsuUp_WoodRod();

                //かまどレベルによるバフ
                KakuritsuUp_Oven();
                break;

            case "Chocolate":

                KakuritsuUp_Chocolate();
                break;

            case "Cake_MatCream":

                KakuritsuUp_CakeMatCream();
                break;

            case "Parfe":

                KakuritsuUp_CakeMatCream();
                break;
        }

        //全般
        if (pitemlist.KosuCount("measuring spoon") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup += 5;
        }
        /*if (pitemlist.KosuCount("maneki_cat") >= 1) //持ってるだけで効果アップ
            {
                _buf_kakuritsuup += 5;
            }*/

        //魔法成功率
        if (GameMgr.Comp_kettei_bunki == 20 || GameMgr.Comp_kettei_bunki == 21 || GameMgr.Comp_kettei_bunki == 22)
        {
            if (pitemlist.KosuCount("green_pendant") >= 1) //持ってるだけで効果アップ
            {
                _buf_kakuritsuup += 5;
            }

            if (pitemlist.KosuCount("star_pendant") >= 1) //持ってるだけで効果アップ
            {
                _buf_kakuritsuup += 10;
            }

            if (pitemlist.KosuCount("aquamarine_pendant") >= 1) //持ってるだけで効果アップ
            {
                _buf_kakuritsuup += 15;
            }

            if (pitemlist.KosuCount("blue_jemstone") >= 1) //持ってるだけで効果アップ
            {
                _buf_kakuritsuup += pitemlist.KosuCount("blue_jemstone") * 1;
            }
        }

        //ヒカリのおかし経験値とLVによって、成功率も上昇する。
        hikariBuf_okashilv(_itemType_sub);
        _b = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 0.0f, 3.0f); //LV1~9までで、1.0~3.0倍まで上昇 LV1だと、バフはかからない
        _buf_kakuritsuup += (int)(10 * _b); 

        return _buf_kakuritsuup;
    }

    void KakuritsuUp_WoodRod()
    {
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
    }

    void KakuritsuUp_Oven()
    {
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
    }


    void KakuritsuUp_Chocolate()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Chocolate_Philosophy") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Chocolate_Philosophy") * 2; //LV*10
            _buf_kakuritsuup += _magicup;
        }
    }

    void KakuritsuUp_CakeMatCream()
    {

        if (pitemlist.KosuCount("cake_rolltable") < 1) //所持してないと成功率下がる
        {
            _buf_kakuritsuup -= 40;
        }

        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Nappe") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Nappe") * 3; //LV*10
            _buf_kakuritsuup += _magicup;
        }
    }

    void KakuritsuUp_Parfe()
    {

        if (pitemlist.KosuCount("glass_bowl") < 1) //所持してないと成功率下がる
        {
            _buf_kakuritsuup -= 20;
        }


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
    public int Buf_OkashiParamUp_Keisan(int _status, string _itemType, string _itemType_sub)
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
                        AppaleilFluffyBuf();
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

                    case "Cake_MatSponge":

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

                    case "Appaleil_Icecream":

                        CreamBuf();
                        AppaleilIcecreamBuf();
                        break;

                    case "Chocolate":

                        ChocolateBuf();
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

                switch (_itemType_sub)
                {
                    case "Suger":

                        SugerBuf();
                        break;

                    case "Cake":

                        CakeBeautyBuf();
                        break;
                }

                switch (_itemType_sub)
                {
                    case "Coffee":

                        CoffeeBeautyBuf();
                        break;
                }

                switch (_itemType_sub)
                {
                    case "Parfe":

                        ParfeBeautyBuf();
                        break;
                }

                switch (_itemType)
                {
                    case "Okashi":

                        AllBeautifulBuf();
                        break;
                }               

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

                AllShokukanBuf();

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

    void ChocolateBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Chocolate_Philosophy") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Chocolate_Philosophy") * 10; //LV*10
            _buf_shokukanup += _magicup;
        }
    }

    void AppaleilFluffyBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") * 10; //LV*10
            _buf_shokukanup += _magicup;
        }
    }

    void AppaleilIcecreamBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") * 10; //LV*10
            _buf_shokukanup += _magicup;
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
            _magicup = magicskill_database.skillName_SearchLearnLevel("Cookie_Study") * 10; //LV*10
            _buf_shokukanup += _magicup;
        }

    }

    void RuskBuf()
    {
        
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Cookie_Study") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Cookie_Study") * 10;
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

    void SugerBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Luminous_Suger") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Luminous_Suger") * 5; //LV*10
            _buf_shokukanup += _magicup;
        }
    }

    

    void CakeBeautyBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Nappe") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Nappe") * 15; //LV*10
            _buf_shokukanup += _magicup;
        }
    }

    void ParfeBeautyBuf()
    {
        if (pitemlist.KosuCount("glass_bowl") >= 1) //もってないと、見た目が下がる
        {
            _buf_shokukanup -= 70;
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

    void AllBeautifulBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Beautiful_Power") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Beautiful_Power") * 10; //LV*10
            _buf_shokukanup += _magicup;
        }
    }

    //星魔法関係
    void CoffeeBeautyBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Latte_Art") >= 1)
        {
            if (magicskill_database.skillName_SearchLearnLevel("Star_Gazer") > 0)//星魔法は、天体観測のレベルでさらに効果があがる
            {
                _magicup = magicskill_database.skillName_SearchLearnLevel("Latte_Art") * 30 * magicskill_database.skillName_SearchLearnLevel("Star_Gazer"); //LV*10
            }
            else
            {
                _magicup = magicskill_database.skillName_SearchLearnLevel("Latte_Art") * 30; //LV*10
            }
            _buf_shokukanup += _magicup;
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

        if (magicskill_database.skillName_SearchLearnLevel("Star_Gazer") > 0) //星魔法　天体観測の効果が働く
        {
            if (_basename == "lumi_banana")
            {
                switch (_status)
                {
                    case 0: //さくさく感のバフ

                        if (_basename == "lumi_banana")
                        {
                            _buf_shokukanup += magicskill_database.skillName_SearchLearnLevel("Star_Gazer") * 30;
                        }

                        return _buf_shokukanup;

                    case 1: //ふわふわ感のバフ

                        if (_basename == "lumi_banana")
                        {
                            _buf_shokukanup += magicskill_database.skillName_SearchLearnLevel("Star_Gazer") * 30;
                        }

                        return _buf_shokukanup;

                    case 2: //なめらか感のバフ

                        if (_basename == "lumi_banana")
                        {
                            _buf_shokukanup += magicskill_database.skillName_SearchLearnLevel("Star_Gazer") * 30;
                        }
                        break;

                    case 3: //歯ごたえ感のバフ

                        if (_basename == "lumi_banana")
                        {
                            _buf_shokukanup += magicskill_database.skillName_SearchLearnLevel("Star_Gazer") * 30;
                        }

                        return _buf_shokukanup;

                    case 4: //ジュースのバフ

                        break;

                    case 5: //見た目のバフ

                        if (_basename == "lumi_banana")
                        {
                            _buf_shokukanup += magicskill_database.skillName_SearchLearnLevel("Star_Gazer") * 15;
                        }
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
    public int Buf_OkashiParamUp_MagicKeisan(int _status, int _baseparam, string _magicname)
    {

        _buf_shokukanup = 0;
        _magicup = 0;

        switch (_magicname)
        {
            case "Cookie_SecondBake":

                if (_status == 0 || _status == 3)//さくさくか歯ごたえのバフ
                {
                    _magicup = (int)(_baseparam * 0.1f * GameMgr.System_magic_playParamUp) + (int)(magicskill_database.skillName_SearchLearnLevel("Cookie_SecondBake") * _baseparam * 0.1f);
                    Debug.Log("補正値: " + _baseparam * 0.1f +  " * " + GameMgr.System_magic_playParamUp + " + " + (int)(magicskill_database.skillName_SearchLearnLevel("Cookie_SecondBake") * _baseparam * 0.1f));
                    Debug.Log("セカンドベイクの最終バフ: " + _magicup);
                    _buf_shokukanup += _magicup;
                }
                break;

            case "Warming_Handmade": //手作りの温もり

                if (_status >= 0 && _status <= 6)//すべての食感
                {
                    if (_status != 5) //ただし、見た目はバフを無視。
                    {
                        _magicup = (int)(_baseparam * (1.0f + magicskill_database.skillName_SearchLearnLevel("Warming_Handmade") * 0.1));
                        _buf_shokukanup += _magicup;
                    }
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
        _a = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 0.8f, 2.5f);
        _buf_hikari_okashiparam = 0.1f + _a * _a;

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
            _buf_hikari_okashi_paramup = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 1.0f, 1.8f); //LV1~9までで、1.0~1.8倍まで上昇
        }

        return _buf_hikari_okashi_paramup;
    }


    //温度管理によるバフの計算
    public float TempatureControlKeisan()
    {
        _tempature_param = SujiMap(GameMgr.System_tempature_control_Param_temp * GameMgr.System_tempature_control_Param_temp,
                        GameMgr.System_tempature_control_tempMin * GameMgr.System_tempature_control_tempMin,
                        GameMgr.System_tempature_control_tempMax * GameMgr.System_tempature_control_tempMax,
                        2.0f, 5.0f); //ここで焼き具合ゲージを決定してる。
        _well_done = _tempature_param * GameMgr.System_tempature_control_Param_time;

        Debug.Log("_tempature_param: " + _tempature_param);
        Debug.Log("_well_done: " + _well_done);
        Debug.Log("_best_well_done: " + _best_well_done);

        _well_done_kyori = Mathf.Abs(_best_well_done - _well_done); //ベストな焼き具合と、今回の焼き具合との差　差が近いほど、高得点
        _well_done_kyori_noabs = _best_well_done - _well_done;
        Debug.Log("ベスト温度との距離: " + _well_done_kyori);

        //余熱石をもってると、さらに温度管理の効果あがる
        if (pitemlist.KosuCount("residual_heatstone") >= 1) //持ってるだけで効果アップ
        {
            _yonetsu_hosei = 1.3f;
            Debug.Log("余熱石　補正あり: " + _yonetsu_hosei);
        }
        else
        {
            _yonetsu_hosei = 1.0f;
            Debug.Log("余熱石　補正なし: " + _yonetsu_hosei);
        }

        if (_well_done_kyori >= 0 && _well_done_kyori < 3.0)
        {
            _well_done_kyori_hosei = 2.0f * _yonetsu_hosei;
            GameMgr.tempature_control_Param_yakitext = "最高の焼き具合だ。";
        }
        else if (_well_done_kyori >= 3.0 && _well_done_kyori < 6.0)
        {
            _well_done_kyori_hosei = 1.5f * _yonetsu_hosei;
            GameMgr.tempature_control_Param_yakitext = "とてもいい焼き具合だ。";
        }
        else if (_well_done_kyori >= 6.0 && _well_done_kyori < 10.0)
        {
            _well_done_kyori_hosei = 1.35f * _yonetsu_hosei;
            GameMgr.tempature_control_Param_yakitext = "いい焼き具合に仕上がった。";
        }
        else if (_well_done_kyori >= 10.0 && _well_done_kyori < 15.0)
        {
            _well_done_kyori_hosei = 1.2f * _yonetsu_hosei;
            GameMgr.tempature_control_Param_yakitext = "ほどよい焼きに仕上がった。";
        }
        else if (_well_done_kyori >= 15.0 && _well_done_kyori < 22.0)
        {
            _well_done_kyori_hosei = 1.0f * _yonetsu_hosei;
            if (_well_done_kyori_noabs >= 0) //+は焼きが足りない
            {
                GameMgr.tempature_control_Param_yakitext = "もう少し焼いてもよさそう。";
            }
            else //-は焼きすぎ
            {
                GameMgr.tempature_control_Param_yakitext = "少し焼きが強かったかな。";
            }
        }
        else if (_well_done_kyori >= 22.0 && _well_done_kyori < 30.0)
        {
            _well_done_kyori_hosei = 0.9f * _yonetsu_hosei;
            if (_well_done_kyori_noabs >= 0) //+は焼きが足りない
            {
                GameMgr.tempature_control_Param_yakitext = "もう少し焼いてもよさそう。";
            }
            else //-は焼きすぎ
            {
                GameMgr.tempature_control_Param_yakitext = "少し焼きが強かったかな。";
            }
        }
        else if (_well_done_kyori >= 30.0 && _well_done_kyori < 45.0)
        {
            _well_done_kyori_hosei = 0.75f * _yonetsu_hosei;
            if (_well_done_kyori_noabs >= 0) //+は焼きが足りない
            {
                GameMgr.tempature_control_Param_yakitext = "焼きが足りなさそうだ..。";
            }
            else //-は焼きすぎ
            {
                GameMgr.tempature_control_Param_yakitext = "焼きすぎたかも。";
            }
        }
        else if (_well_done_kyori >= 45.0 && _well_done_kyori < 60.0)
        {
            _well_done_kyori_hosei = 0.5f * _yonetsu_hosei;
            if (_well_done_kyori_noabs >= 0) //+は焼きが足りない
            {
                GameMgr.tempature_control_Param_yakitext = "生焼けっぽい..。";
            }
            else //-は焼きすぎ
            {
                GameMgr.tempature_control_Param_yakitext = "焼きすぎたかな..。";
            }
        }
        else if (_well_done_kyori >= 60.0)
        {
            _well_done_kyori_hosei = 0.125f * _yonetsu_hosei;
            if (_well_done_kyori_noabs >= 0) //+は焼きが足りない
            {
                GameMgr.tempature_control_Param_yakitext = "げ..。生焼けだ..。";
            }
            else //-は焼きすぎ
            {
                GameMgr.tempature_control_Param_yakitext = "げ..。焼きすぎた..。";
            }
        }
        Debug.Log("_well_done_kyori_hosei（温度で食感にかかる補正値*）: " + _well_done_kyori_hosei);

        return _well_done_kyori_hosei;
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
