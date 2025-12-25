using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buf_Power_Keisan : SingletonMonoBehaviour<Buf_Power_Keisan>
{
    private PlayerItemList pitemlist;
    private HikariOkashiExpTable hikariOkashiExpTable;

    private ItemDataBase database;
    private MagicSkillListDataBase magicskill_database;
    private ItemCompoundDataBase databaseCompo;

    private int _buf_findpower;
    private int _buf_kakuritsuup;
    private float _buf_kakuritsuup_f;
    private int _buf_kosuup;
    private int _buf_shokukanup;
    private float _buf_kyori;
    private int _buf_compotime_up;
    private int _statusup, _magicup;
    private float _magicup_f;
    private int original_shokukan_p;
    private int taste_score;
    private int cook_kaisu, cook_count_buf;
    private int magic_attri;
    private int _magic_rate;
    private int _magicLearnLv;
    private int _magic_kakuritsu;
    private int _attri2, _attri5, _attri6, _attri7, _attri8, _attri9, _attri10;

    private float _buf_hikari_okashiparam;
    private float _buf_hikari_okashi_paramup;
    private int hikari_okashiLV;
    private float _a, _b, _kosuhosei;

    private int i, rnd;
    private int _id, _magicid, _magicid2;
    private string _itemType;
    private string _itemType_sub;
    private string _itemType_subB;
    private int _compoID;

    private float _tempature_param;
    private float _well_done;
    private float _best_well_done;
    private float _well_done_kyori;
    private float _well_done_kyori_noabs;
    private float _well_done_kyori_hosei;
    private float _yonetsu_hosei;
    private float _choco_hose1, _choco_hose2;

    // Use this for initialization
    void Start() {

        InitSetup();
    }

    void InitSetup()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //ヒカリお菓子EXPデータベースの取得
        hikariOkashiExpTable = HikariOkashiExpTable.Instance.GetComponent<HikariOkashiExpTable>();
    }

    // Update is called once per frame
    void Update() {

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
    public int Buf_CompKakuritsu_Keisan(string _result_item, int _compID, int _attri4)
    {
        _buf_kakuritsuup = 0;

        //アイテムによって、特定のお菓子のときのみ成功率をあげる。
        _id = database.SearchItemIDString(_result_item);
        _itemType = database.items[_id].itemType.ToString();
        _itemType_sub = database.items[_id].itemType_sub.ToString();
        _itemType_subB = database.items[_id].itemType_subB.ToString();
        _compoID = _compID;

        switch (_itemType_sub)
        {
            case "Appaleil": //_attri4 生地の混ぜ回数が影響にはいる

                KakuritsuUp_Appaleil(_attri4);
                break;

            case "Appaleil_Icecream":

                KakuritsuUp_Appaleil(_attri4);
                break;

            case "Biscotti":

                //かまどレベルによるバフ
                KakuritsuUp_Oven();
                break;

            case "Bread":

                //かまどレベルによるバフ
                KakuritsuUp_Oven();
                break;

            case "Cookie":

                //めん棒系
                KakuritsuUp_WoodRod();
                //かまどレベルによるバフ
                KakuritsuUp_Oven();
                //魔法でのバフ
                KakuritsuUp_Cookie();
                break;

            case "Cookie_Mat":

                //めん棒系
                KakuritsuUp_WoodRod();
                //かまどレベルによるバフ
                KakuritsuUp_Oven();
                //魔法でのバフ
                KakuritsuUp_Cookie();
                break;

            case "Cookie_Hard":

                //めん棒系
                KakuritsuUp_WoodRod();
                //かまどレベルによるバフ
                KakuritsuUp_Oven();
                //魔法でのバフ
                KakuritsuUp_Cookie();
                break;

            case "Chocolate":

                KakuritsuUp_Chocolate();
                break;

            case "Cake_MatCream":

                KakuritsuUp_CakeMatCream();
                break;

            case "Cake_MatSpongeBaked":

                KakuritsuUp_CakeMatSpongeBaked(0);
                break;

            case "CheeseCake":

                KakuritsuUp_CakeMatSpongeBaked(1);
                break;

            case "Financier":

                //かまどレベルによるバフ
                KakuritsuUp_Oven();
                break;

            case "IceCream":

                KakuritsuUp_IceCream();
                break;

            case "Juice":

                //魔法でのバフ
                KakuritsuUp_Soda();
                break;

            case "Maffin":

                //かまどレベルによるバフ
                KakuritsuUp_Oven();
                break;

            case "Parfe":

                KakuritsuUp_Parfe();
                break;

            case "Rusk":

                //かまどレベルによるバフ
                KakuritsuUp_Oven();

                //魔法でのバフ
                KakuritsuUp_Cookie();
                break;

            case "Soda":

                //魔法でのバフ
                KakuritsuUp_Soda();
                break;
        }

        switch (_itemType_subB)
        {
            case "a_CacaoNibs":

                KakuritsuUp_BakeBeans();
                break;

            case "a_CacaoMass":

                KakuritsuUp_CacaoMass();
                break;

            case "a_CoffeeBeans":

                KakuritsuUp_BakeBeans();
                break;

            case "a_Yakimaron":

                KakuritsuUp_BakeBeans();
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


        //一回でも成功したことがあれば、+3%ほど成功率が上昇する。
        if (databaseCompo.compoitems[_compoID].cmpitem_flag >= 1 && databaseCompo.compoitems[_compoID].cmpitem_flag != 9999) //9999は除外するので計算しない
        {
            _buf_kakuritsuup += 3;
        }

        //成功率　ヒカリのおかし経験値とLVによって、成功率も上昇する。
        KakuritsuUp_HikariBuf();

        //ステータスによる成功率バフ
        KakuritsuUp_PStatusBuf();

        //魔法によるプレイヤー状態のバフ
        KakuritsuUp_MagicPStatusBuf();

        return _buf_kakuritsuup;
    }

    void KakuritsuUp_HikariBuf()
    {
        Debug.Log("ヒカリのおかしLVにより、成功率の上昇バフ");
        hikariBuf_okashilv(_itemType_sub);
        _b = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 0.0f, 3.0f); //LV1~9までで、1.0~3.0倍まで上昇 LV1だと、バフはかからない 最大30%までアップ
        _buf_kakuritsuup += (int)(10 * _b);
    }

    void KakuritsuUp_PStatusBuf()
    {
        _statusup = 0;
        _statusup = (int)(PlayerStatus.player_okashi_kakuritsuup * 0.2f); //5で1%上昇ぐらい？
        _buf_kakuritsuup += _statusup;
    }

    void KakuritsuUp_MagicPStatusBuf()
    {
        _statusup = 0;

        //エピクレイシス状態をチェック　LVに応じて確率が上がる
        if (PlayerStatus.player_girl_status[0] > 0)
        {
            _statusup = 5 + (PlayerStatus.player_girl_status[0] * 10); //15~35%も上がる！
        }
        _buf_kakuritsuup += _statusup;
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

    void KakuritsuUp_Appaleil(int _baseattri4)
    {
        //生地を混ぜた回数があがるほど、成功率が下がる
        if (_baseattri4 >= 1 && _baseattri4 < 3) //0回は何もしない
        {
            _buf_kakuritsuup += (-10 * _baseattri4);
        }
        else if (_baseattri4 >= 3 && _baseattri4 < 6)
        {
            _buf_kakuritsuup += (-15 * _baseattri4);
        }
        else if (_baseattri4 >= 6)
        {
            _buf_kakuritsuup += (-20 * _baseattri4);
        }

        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") >= 1) //アパレイユのお勉強で、生地を混ぜるときの成功率が上がる
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") * 5; //LV*10
            _buf_kakuritsuup += _magicup;
        }
    }

    void KakuritsuUp_Oven()
    {
        /*if (pitemlist.KosuCount("platinum_oven") >= 1) //持ってるだけで効果アップ
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
        }*/

        //よねつ石の効果で食感上がる
        if (pitemlist.KosuCount("residual_heatstone") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup += 5;
        }
    }

    void KakuritsuUp_Cookie()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Cookie_Study") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Cookie_Study") * 2; //LV*2
            _buf_kakuritsuup += _magicup;
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
        /*if (pitemlist.KosuCount("cake_rolltable") < 1) //所持すると成功率あがる
        {
            _buf_kakuritsuup += 30;
        }*/

        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Nappe") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Nappe") * 3; //LV*10
            _buf_kakuritsuup += _magicup;
        }
    }

    void KakuritsuUp_CakeMatSpongeBaked(int _mstatus)
    {

        if (pitemlist.KosuCount("cakemold_stainless") > 1) //所持すると成功率上がる
        {
            if (_mstatus == 0)
            {
                _buf_kakuritsuup += 30;
            }
            else if (_mstatus == 1) //チーズケーキの場合
            {
                _buf_kakuritsuup += 30;
            }
        }

        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") >= 1) //アパレイユのお勉強で、ケーキ生地を焼くときの成功率が上がる
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") * 5; //LV*10
            _buf_kakuritsuup += _magicup;
        }
    }

    void KakuritsuUp_IceCream()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") * 4; //LV*4
            _buf_kakuritsuup += _magicup;
        }

        //フリージングの習得LVでもちょっと成功率上がる
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Freezing_Spell") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Freezing_Spell") * 1; //LV*1
            _buf_kakuritsuup += _magicup;
        }
    }

    void KakuritsuUp_Parfe()
    {

        if (pitemlist.KosuCount("glass_bowl") < 1) //所持してないと成功率下がる
        {
            _buf_kakuritsuup -= 50;
        }

    }

    void KakuritsuUp_BakeBeans()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Bake_Beans") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Bake_Beans") * 5; //LV*10
            _buf_kakuritsuup += _magicup;
        }
    }

    void KakuritsuUp_CacaoMass()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Chocolate_Philosophy") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Chocolate_Philosophy") * 2; //LV*10
            _buf_kakuritsuup += _magicup;
        }
    }

    void KakuritsuUp_Soda()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Soda_Study") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Soda_Study") * 2; //LV*2%
            _buf_kakuritsuup += _magicup;
        }
    }

    //
    //調合成功率　魔法使用時のバフ
    //魔法名を指定し、中に補正値をかけばOK
    //
    public int Buf_CompKakuritsuMagic_Keisan(string _magic_name)
    {
        _magic_kakuritsu = 0;
        _magic_rate = 0;
        _magicLearnLv = magicskill_database.skillName_SearchLearnLevel(_magic_name);
        _magicid = magicskill_database.SearchSkillString(_magic_name);
        _attri2 = GameMgr.UseMagic_ItemAttri[1]; //魔法使用時のitemselecttoggleで参照
        _attri5 = GameMgr.UseMagic_ItemAttri[4];
        _attri6 = GameMgr.UseMagic_ItemAttri[5]; //ライトニンググレープなどの変質回数
        _attri7 = GameMgr.UseMagic_ItemAttri[6]; //ライトニンググレープなどの変質回数

        switch (_magic_name)
        {
            case "Freezing_Spell":

                _magic_rate += _magicLearnLv * 3;
                break;

            case "SugerPot":

                _magic_rate += _magicLearnLv * 5;
                break;

            case "Luminous_Suger":

                _magic_rate += _magicLearnLv * 5;
                break;

            case "Luminous_Fruits":

                _magic_rate += _magicLearnLv * 5;
                break;

            case "Aroma_Potion":

                _magic_rate += _magicLearnLv * 3;
                break;

            case "Wind_Ark":

                Kakuritsu_ArkKeisan(_attri2);
                _magic_rate += _magicLearnLv * 3;
                break;

            case "Fire_Ark":

                Kakuritsu_ArkKeisan(_attri5);
                _magic_rate += _magicLearnLv * 3;
                break;

            case "Lightning_Grape":

                Kakuritsu_ArkKeisan(_attri6);
                _magic_rate += _magicLearnLv * 10;
                break;

            case "Dreamy_Sapphire":

                Kakuritsu_ArkKeisan(_attri6);
                _magic_rate += _magicLearnLv * 10;
                break;

            case "Warming_Handmade":

                Kakuritsu_ArkKeisan(_attri7);
                _magic_rate += _magicLearnLv * 3;
                break;
        }

        //装備品による成功率アップ
        if (pitemlist.KosuCount("green_pendant") >= 1) //持ってるだけで効果アップ
        {
            _magic_rate += 3;
        }

        if (pitemlist.KosuCount("star_pendant") >= 1) //持ってるだけで効果アップ
        {
            _magic_rate += 5;
        }

        if (pitemlist.KosuCount("aquamarine_pendant") >= 1) //持ってるだけで効果アップ
        {
            _magic_rate += 7;
        }

        /*if (pitemlist.KosuCount("blue_jemstone") >= 1) //持ってるだけで効果アップ
        {
            _magic_rate += pitemlist.KosuCount("blue_jemstone") * 1;
        }*/

        //各スキルの使用回数に応じて、成功率が少し上がる。
        _magic_kakuritsu = (int)(magicskill_database.magicskill_lists[_magicid].skill_usecount * 0.334f); //3回使えば+1%
        if (_magic_kakuritsu >= 30) //30%が上限
        {
            _magic_kakuritsu = 30;
        }
        _magic_rate = _magic_rate + _magic_kakuritsu;
        Debug.Log("その魔法を使った回数: " + magicskill_database.magicskill_lists[_magicid].skill_usecount + " 成功率up: " + _magic_kakuritsu);

        //ステータスによる魔法成功率バフ
        KakuritsuUpMagic_PStatusBuf();

        //魔法によるプレイヤー状態のバフ
        KakuritsuUpMagic_MagicPStatusBuf();

        return _magic_rate;
    }

    void Kakuritsu_ArkKeisan(int _attri_kaisu)
    {
        Debug.Log("そのアイテムのAttri: " + _attri_kaisu);
        if (_attri_kaisu < 2)
        {
            _magic_rate = (int)(-5f * _attri_kaisu * 1.25f); //重ね掛けするほど、確率が減っていく
        }
        else if (_attri_kaisu >= 2 && _attri_kaisu < 3)
        {
            _magic_rate = (int)(-5f * _attri_kaisu * 1.5f); //重ね掛けするほど、確率が減っていく
        }
        else if (_attri_kaisu >= 3 && _attri_kaisu < 4)
        {
            _magic_rate = (int)(-5f * _attri_kaisu * 3.0f); //重ね掛けするほど、確率が減っていく
        }
        else if (_attri_kaisu >= 4 && _attri_kaisu < 6)
        {
            _magic_rate = (int)(-5f * _attri_kaisu * 3.5f); //重ね掛けするほど、確率が減っていく
        }
        else if (_attri_kaisu >= 6)
        {
            _magic_rate = (int)(-5f * _attri_kaisu * 4.0f); //重ね掛けするほど、確率が減っていく 6回以上はほぼ０
        }
    }

    void KakuritsuUpMagic_PStatusBuf()
    {
        _statusup = 0;
        _statusup = (int)(PlayerStatus.player_okashi_magic_kakuritsuup * 0.34f); //3で1%上昇ぐらい？
        _magic_rate += _statusup;
    }

    void KakuritsuUpMagic_MagicPStatusBuf()
    {
        _statusup = 0;

        //エピクレイシス状態をチェック
        if (PlayerStatus.player_girl_status[0] > 0)
        {
            _statusup = 20 + (PlayerStatus.player_girl_status[0] * 10); //30~50%も上がる！
        }
        _magic_rate += _statusup;
    }



    //
    //制作時間のバフ（短縮される）
    //調合で生成されるアイテムの_itemType_subを指定し、中に補正値をかけばOK
    //
    public int Buf_CompoTime_Keisan(string _result_item)
    {
        _buf_compotime_up = 0;

        //アイテムによって、特定のお菓子のときのみ成功率をあげる。
        _id = database.SearchItemIDString(_result_item);
        _itemType = database.items[_id].itemType.ToString();
        _itemType_sub = database.items[_id].itemType_sub.ToString();
        _itemType_subB = database.items[_id].itemType_subB.ToString();

        switch (_itemType_sub)
        {
            case "Cookie":

                //CostTimeUp_Cookie();

                break;

            case "Cookie_Hard":

                //CostTimeUp_Cookie();
                break;

            case "Chocolate":

                CostTimeUp_Chocolate();
                break;

            case "Cake_MatCream":

                CostTimeUp_Cake_MatCream();
                break;

            case "Cake_MatSpongeBaked":

                CostTimeUp_Cake_MatSpongeBaked();
                break;

            case "IceCream": //フリージングでアイスを作る場合は、こっちは通らないので注意　下のFreezing_Spellでかく

                CostTimeUp_IceCream();
                break;

            case "Parfe":

                CostTimeUp_IceCream();
                break;
        }

        switch (_itemType_subB)
        {
            case "a_CacaoMass":

                CostTimeUp_Chocolate();
                break;
        }

        //全般
        /*if (pitemlist.KosuCount("measuring spoon") >= 1) //持ってるだけで効果アップ
        {
            _buf_kakuritsuup += 5;
        }*/

        //ステータスによる時間短縮バフ
        CostTimeUp_PStatusBuf();

        return _buf_compotime_up;
    }

    void CostTimeUp_PStatusBuf()
    {
        _statusup = 0;
        _statusup = (int)(PlayerStatus.player_okashi_costtimeup * 0.5f); //2で1分上昇ぐらい？
        _buf_compotime_up += _statusup;
    }

    void CostTimeUp_Cookie()
    {
        //魔法のバフ
        _magicup = 0;
        _magicid = magicskill_database.SearchSkillString("Cookie_Study");
        if (magicskill_database.magicskill_lists[_magicid].skillLv >= 1)
        {
            _magicup = magicskill_database.magicskill_lists[_magicid].skillLv * 3; //LV*10
            _buf_compotime_up += _magicup;
        }
    }

    void CostTimeUp_Cake_MatCream()
    {
        //魔法のバフ
        _magicup = 0;
        _magicid = magicskill_database.SearchSkillString("Nappe");
        if (magicskill_database.magicskill_lists[_magicid].skillLv >= 1)
        {
            _magicup = (int)(magicskill_database.magicskill_lists[_magicid].skillLv * magicskill_database.magicskill_lists[_magicid].cost_time * 0.1f); //costtimeの10％
            _buf_compotime_up += _magicup;
        }
    }

    void CostTimeUp_Cake_MatSpongeBaked()
    {
        //魔法のバフ
        _magicup = 0;
        _magicid = magicskill_database.SearchSkillString("Appaleil_Study");
        if (magicskill_database.magicskill_lists[_magicid].skillLv >= 1)
        {
            _magicup = (int)(magicskill_database.magicskill_lists[_magicid].skillLv * magicskill_database.magicskill_lists[_magicid].cost_time * 0.2f); //costtimeの20％
            _buf_compotime_up += _magicup;
        }
    }

    void CostTimeUp_IceCream()
    {
        //魔法のバフ
        _magicup = 0;
        _magicid = magicskill_database.SearchSkillString("Heart_of_Icecream");
        if (magicskill_database.magicskill_lists[_magicid].skillLv >= 1)
        {
            _magicup = (int)(magicskill_database.magicskill_lists[_magicid].skillLv * magicskill_database.magicskill_lists[_magicid].cost_time * 0.01f); //costtimeの1％
            _buf_compotime_up += _magicup;
        }
    }

    void CostTimeUp_Chocolate()
    {
        //魔法のバフ
        _magicup = 0;
        _magicid = magicskill_database.SearchSkillString("Chocolate_Philosophy");
        if (magicskill_database.magicskill_lists[_magicid].skillLv >= 1)
        {
            _magicup = (int)(magicskill_database.magicskill_lists[_magicid].skillLv * magicskill_database.magicskill_lists[_magicid].cost_time * 0.03f); //costtimeの3％
            _buf_compotime_up += _magicup;
        }
    }



    //
    //特定の魔法使用時の制作時間を短縮する
    //
    public int Buf_CompoTimeMagic_Keisan(string _magicname)
    {
        _buf_compotime_up = 0;

        //たとえば、祝福状態なら、制作時間が10%短縮されるなど。もココで書けばおｋ
        switch (_magicname)
        {
            case "Freezing_Spell": //アイス制作やフローズンベリーなど使うとき デフォルトでは、等しく2時間かかる

                CostTimeUpMagic_IceCream();
                break;

        }

        //ステータスによる魔法時間短縮バフ
        CostTimeUpMagic_PStatusBuf();

        Debug.Log("魔法制作時間短縮: " + _buf_compotime_up + "分");
        return _buf_compotime_up;
    }

    void CostTimeUpMagic_PStatusBuf()
    {
        _statusup = 0;
        _statusup = (int)(PlayerStatus.player_okashi_magic_costtimeup * 0.5f); //2で1分上昇ぐらい？
        _buf_compotime_up += _statusup;
    }

    void CostTimeUpMagic_IceCream()
    {
        //魔法のバフ
        _magicup = 0;
        _magicid = magicskill_database.SearchSkillString("Heart_of_Icecream");
        _magicid2 = magicskill_database.SearchSkillString("Freezing_Spell");

        //アイスの気持ちのバフ
        _magicup = (int)(magicskill_database.magicskill_lists[_magicid].skillLv * magicskill_database.magicskill_lists[_magicid2].cost_time * 0.1f); //costtimeの10％
                                                                                                                                                    
        //フリージングそのものも上がる
        _magicup += (int)(magicskill_database.magicskill_lists[_magicid2].skillLv * magicskill_database.magicskill_lists[_magicid2].cost_time * 0.05f); //costtimeの5％

        _buf_compotime_up += _magicup;
    }




    //
    //個数のバフ
    //調合で生成されるアイテムの_itemType_subを指定し、中に補正値をかけばOK
    //
    public int Buf_Kosu_Keisan(string _result_item, int _compID)
    {
        _buf_kosuup = 0;

        //アイテムによって、特定のお菓子のときのみ成功率をあげる。
        _id = database.SearchItemIDString(_result_item);
        _itemType = database.items[_id].itemType.ToString();
        _itemType_sub = database.items[_id].itemType_sub.ToString();
        _itemType_subB = database.items[_id].itemType_subB.ToString();
        _compoID = _compID;

        switch (_itemType_sub)
        {
            case "Appaleil":

                //魔法でのバフ
                KosuUp_Appaleil();
                break;

            case "Cream":

                //魔法でのバフ
                KosuUp_Cream();
                break;
        }

        /*switch (_itemType_subB)
        {
            case "a_CacaoNibs":

                KakuritsuUp_BakeBeans();
                break;
            
        }*/

        //全般


        return _buf_kosuup;
    }

    void KosuUp_Appaleil()
    {
        if (databaseCompo.compoitems[_compoID].buf_kouka_on != 2)
        {
            //魔法のバフ
            _magicup = 0;
            if (magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") >= 5)
            {
                _magicup = 1; //LV*1
                _buf_kosuup += _magicup;
            }
            /*if (magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") >= 3 && magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") < 5)
            {
                _magicup = 1; //LV*1
                _buf_kosuup += _magicup;
            }
            else if (magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") >= 5)
            {
                _magicup = 2; //LV*1
                _buf_kosuup += _magicup;
            }*/
        }
        { } //2のときは、個数計算しない
    }

    void KosuUp_Cream()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Nappe") >= 3 && magicskill_database.skillName_SearchLearnLevel("Nappe") < 5)
        {
            _magicup = 1; //LV*1
            _buf_kosuup += _magicup;
        }
        else if (magicskill_database.skillName_SearchLearnLevel("Nappe") >= 5)
        {
            _magicup = 2; //LV*1
            _buf_kosuup += _magicup;
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
    public int Buf_OkashiParamUp_Keisan(int _status, int _origin_param, string _result_item, int _compID, int _magic_attri)
    {
        InitSetup();

        _buf_shokukanup = 0;

        _id = database.SearchItemIDString(_result_item);
        _itemType = database.items[_id].itemType.ToString();
        _itemType_sub = database.items[_id].itemType_sub.ToString();
        _itemType_subB = database.items[_id].itemType_subB.ToString();
        cook_kaisu = database.items[_id].Cook_kaisu;
        magic_attri = _magic_attri;
        _compoID = _compID;

        original_shokukan_p = _origin_param;


        switch (_status)
        {
            case 0: //さくさく感のバフ(ほくほく感のバフ)

                switch (_itemType_sub)
                {
                    case "Appaleil":

                        CreamBuf();
                        AppaleilBuf();
                        break;

                    case "Bread":

                        OvenBuf();
                        break;

                    case "Biscotti":

                        OvenBuf();
                        break;

                    case "BakedSweets":

                        HokuhokuBuf();
                        break;

                    case "Cookie":

                        OvenBuf();
                        CookieBuf();
                        RodsBuf(); //めんぼう
                        break;

                    case "Cookie_Hard":

                        OvenBuf();
                        CookieBuf();
                        RodsBuf(); //めんぼう
                        break;

                    case "Rusk":

                        OvenBuf();
                        RuskBuf();
                        break;

                }

                //おかし全てに効果があるバフ
                if (_itemType == "Okashi")
                {                  
                    YonetsuStoneBuf();
                }

                //魔法おかしにかかるバフ
                MagicOkashiBuf();

                //光りおかしにかかるバフ
                MagicGlowBuf();

                AllShokukanBuf();

                //ステータスによる食感バフ
                Shokukanup_PStatusBuf(0);

                return _buf_shokukanup;

            case 1: //ふわふわ感のバフ

                switch (_itemType_sub)
                {
                    case "Appaleil":

                        CreamBuf();
                        AppaleilBuf();
                        break;

                    case "Cream":

                        CreamBuf();
                        break;

                    /*case "Crepe":

                        CrepeBuf();
                        break;*/

                    case "Crepe_Mat": //クレープ生地ができるときだけ効果がある

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

                //おかし全てに効果があるバフ
                if (_itemType == "Okashi")
                {
                    YonetsuStoneBuf();
                }

                //魔法おかしにかかるバフ
                MagicOkashiBuf();

                //光りおかしにかかるバフ
                MagicGlowBuf();

                AllShokukanBuf();

                //ステータスによる食感バフ
                Shokukanup_PStatusBuf(1);

                return _buf_shokukanup;

            case 2: //なめらか感のバフ

                switch (_itemType_sub)
                {
                    case "Appaleil":

                        CreamBuf();
                        AppaleilBuf();
                        break;

                    case "Appaleil_Icecream":

                        AppaleilIcecreamBuf();
                        break;

                    case "Chocolate":

                        ChocolateBuf();
                        break;
                }

                //魔法おかしにかかるバフ
                MagicOkashiBuf();

                //光りおかしにかかるバフ
                MagicGlowBuf();

                AllShokukanBuf();

                //ステータスによる食感バフ
                Shokukanup_PStatusBuf(2);

                return _buf_shokukanup;

            case 3: //歯ごたえ感のバフ

                switch (_itemType_sub)
                {
                    case "Appaleil":

                        AppaleilBuf();
                        break;

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

                //魔法おかしにかかるバフ
                MagicOkashiBuf();

                //光りおかしにかかるバフ
                MagicGlowBuf();

                AllShokukanBuf();

                //ステータスによる食感バフ
                Shokukanup_PStatusBuf(3);

                return _buf_shokukanup;

            case 4: //ジュースのバフ

                switch (_itemType_sub)
                {
                    case "Juice":

                        JuiceSodaBuf();
                        JuiceMixerBuf();
                        break;

                    case "Soda":

                        JuiceSodaBuf();
                        JuiceMixerBuf();
                        break;
                }

                //魔法おかしにかかるバフ
                MagicOkashiBuf();

                //光りおかしにかかるバフ
                MagicGlowBuf();

                AllShokukanBuf();

                //ステータスによる食感バフ
                Shokukanup_PStatusBuf(4);

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

                //魔法おかしにかかるバフ
                MagicOkashiBuf();

                //光りおかしにかかるバフ
                MagicGlowBuf();

                return _buf_shokukanup;

            case 6: //香りのバフ

                switch (_itemType_sub)
                {
                    case "Coffee":

                        FlavorBuf();
                        break;

                    case "Tea":

                        TeaBuf();
                        FlavorBuf();
                        break;

                    case "Tea_Mat":

                        TeaBuf();
                        FlavorBuf();
                        break;

                    case "Tea_Potion":

                        TeaBuf();
                        FlavorBuf();
                        break;
                }

                //魔法おかしにかかるバフ
                MagicOkashiBuf();

                //光りおかしにかかるバフ
                MagicGlowBuf();
               
                AllShokukanBuf();

                //ステータスによる食感バフ
                Shokukanup_PStatusBuf(6);

                return _buf_shokukanup;

            case 50: //粉っぽさ

                Shokukanup_MagicPStatusBuf();
                return _buf_shokukanup;

            case 51: //油っぽさ

                Shokukanup_MagicPStatusBuf();
                return _buf_shokukanup;

            case 52: //水っぽさ

                Shokukanup_MagicPStatusBuf();
                return _buf_shokukanup;
        }

        return 0; //なにもない場合や例外は0
    }

    void Shokukanup_MagicPStatusBuf()
    {
        _statusup = 0;

        //ラトリア状態をチェック
        if (PlayerStatus.player_girl_status[1] > 0)
        {
            _statusup = -10 + (PlayerStatus.player_girl_status[1] * -3); //各粉っぽさ系をちょっと減らす 使用LVが入ってるはずなので、LVで倍数になる。
            Debug.Log("ラトリア効果あり　粉っぽさなどの減少: " + _statusup);
        }
        _buf_shokukanup += _statusup;
    }

    void Shokukanup_PStatusBuf(int _mstatus)
    {
        _statusup = 0;

        switch (_mstatus)
        {
            case 0:

                _statusup = (int)(PlayerStatus.player_okashi_crispyup * 1.0f); //1で1上昇ぐらい？
                break;

            case 1:

                _statusup = (int)(PlayerStatus.player_okashi_fluffyup * 1.0f); //2で1分上昇ぐらい？
                break;

            case 2:

                _statusup = (int)(PlayerStatus.player_okashi_smoothup * 1.0f); //2で1分上昇ぐらい？
                break;

            case 3:

                _statusup = (int)(PlayerStatus.player_okashi_hardnessup * 1.0f); //2で1分上昇ぐらい？
                break;

            case 4: //ジュース

                _statusup = (int)(PlayerStatus.player_okashi_juiceup * 1.0f); //2で1分上昇ぐらい？
                break;

            case 5: //見た目なので、現在なし

                break;

            case 6: //香り

                _statusup = (int)(PlayerStatus.player_okashi_tea_flavorup * 1.0f); //2で1分上昇ぐらい？
                break;
        }

        _buf_shokukanup += _statusup;
    }

    void CreamBuf()
    {
        if (pitemlist.KosuCount("whisk_gold") >= 1) //金の泡だて器をもっている
        {
            _buf_shokukanup = (int)(_buf_shokukanup * 1.3f);
        }
        else
        {
            if (pitemlist.KosuCount("whisk_silver") >= 1) //銀の泡だて器をもっている
            {
                _buf_shokukanup = (int)(_buf_shokukanup * 1.22f);
            }
            else
            {
                if (pitemlist.KosuCount("whisk_magic") >= 1) //魔力の泡だて器をもっている
                {
                    _buf_shokukanup = (int)(_buf_shokukanup * 1.15f);
                }
            }
        }
    }

    void ChocolateBuf()
    {

    }

    void AppaleilBuf()
    {
        if (databaseCompo.compoitems[_compoID].buf_kouka_on == 1) //初期作成時は大きいバフ
        {
            //魔法のバフ
            _magicup = 0;
            if (magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") >= 1)
            {
                _magicup = magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") * 20; //LV*10
                _buf_shokukanup += _magicup;
            }
        }
        else if (databaseCompo.compoitems[_compoID].buf_kouka_on == 2) //生地を泡だて器で混ぜることによるバフ　少し上がり幅せまい
        {
            //魔法のバフ
            _magicup = 0;
            if (magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") >= 1)
            {
                _magicup = magicskill_database.skillName_SearchLearnLevel("Appaleil_Study") * 5; //LV*10
                _buf_shokukanup += _magicup;
            }
        }
    }

    void AppaleilIcecreamBuf()
    {
        if (databaseCompo.compoitems[_compoID].buf_kouka_on == 1) //アイス水溶液を泡だて器で混ぜることによるバフ　少し上がり幅せまい
        {
            //魔法のバフ
            _magicup = 0;
            if (magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") >= 1)　//アイスの気持ちでわずかに上昇値あがる
            {
                _magicup = magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") * 5; //LV*3
                _buf_shokukanup += _magicup;
            }
        }
        else if (databaseCompo.compoitems[_compoID].buf_kouka_on == 2) //アイス水溶液を泡だて器で混ぜることによるバフ　少し上がり幅せまい
        {
            //魔法のバフ
            _magicup = 0;
            if (magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") >= 1)　//アイスの気持ちでわずかに上昇値あがる
            {
                _magicup = magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") * 3; //LV*3
                _buf_shokukanup += _magicup;
            }
        }
    }

    void HokuhokuBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Bake_Beans") >= 2)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Bake_Beans") * 30; //LV*30
            _buf_shokukanup += _magicup;
        }
    }

    void JuiceSodaBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Soda_Study") >= 1)
        {
            _magicup = (int)(original_shokukan_p * magicskill_database.skillName_SearchLearnLevel("Soda_Study") * 0.06f); //元の値の6%上昇*LV

            _buf_shokukanup += _magicup;
        }
    }

    void JuiceMixerBuf()
    {
        //クリスタルジュースミキサーの効果で食感上がる
        if (pitemlist.KosuCount("juice_mixer_high") >= 1) //持ってるだけで効果アップ
        {
            _buf_shokukanup += 100;
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
                    _buf_shokukanup += 50;
                }
                else
                {
                    _buf_shokukanup = 0;
                }
            }
        }


    }

    void YonetsuStoneBuf()
    {
        //よねつ石の効果で食感上がる
        if (pitemlist.KosuCount("residual_heatstone") >= 1) //持ってるだけで効果アップ
        {
            _buf_shokukanup += 20;
        }
    }

    void RodsBuf()
    {
        //めん棒系
        if (pitemlist.KosuCount("wood_rod_doillan") >= 1)
        {
            _buf_shokukanup += 80;
        }
        else
        {
            if (pitemlist.KosuCount("wood_rod_great") >= 1)
            {
                _buf_shokukanup += 50;
            }
            else
            {
                if (pitemlist.KosuCount("wood_rod_good") >= 1)
                {
                    _buf_shokukanup += 30;
                }
                else
                {
                    if (pitemlist.KosuCount("wood_rod_normal") >= 1)
                    {
                        _buf_shokukanup += 15;
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
    }

    void CookieBuf()
    {

        if (pitemlist.KosuCount("cookie_powerup1") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.05f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("cookie_powerup2") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.1f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("cookie_powerup3") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.2f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("cookie_powerup4") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("cookie_powerup5") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.5f) - original_shokukan_p;
        }

    }

    void MagicGlowBuf()
    {
        switch (_itemType_subB)
        {
            case "a_GlowCookie":

                MagicGlowBuf_method();
                break;

            case "a_GlowCookie_Hard":

                MagicGlowBuf_method();
                break;

            case "a_GlowRusk":

                MagicGlowBuf_method();
                break;

            case "a_GlowPudding":

                MagicGlowBuf_method();
                break;

            case "a_GlowCheeseCake":

                MagicGlowBuf_method();
                break;

            case "a_GlowCake":

                MagicGlowBuf_method();
                break;

            case "a_GlowJuice":

                MagicGlowBuf_method();
                break;

            case "a_GlowJelly":

                MagicGlowBuf_method();
                break;

            case "a_GlowCandy":

                MagicGlowBuf_method();
                break;
        }

    }

    void MagicOkashiBuf()
    {
        if(magic_attri >= 1)
        {
            //家によるバフ
            if (GameMgr.OrCompound_RoomNum == 3)
            {
                _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
            }           
        }
    }

    void MagicGlowBuf_method()
    {
        //魔法のバフ
        /*_magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Beautiful_Power") >= 1)
        {
            _magicup = (int)(original_shokukan_p * magicskill_database.skillName_SearchLearnLevel("Beautiful_Power") * 0.06f); //元の値の6%上昇
            _buf_shokukanup += _magicup;
        }*/
    }

    void RuskBuf()
    {

        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Cookie_Study") >= 1)
        {
            _magicup = (int)(original_shokukan_p * magicskill_database.skillName_SearchLearnLevel("Cookie_Study") * 0.06f); //元の値の6%上昇

            _buf_shokukanup += _magicup;
        }

    }

    void CandyBuf()
    {
        if (pitemlist.KosuCount("candy_powerup1") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
        }
    }

    void CrepeBuf()
    {
        if (pitemlist.KosuCount("crepe_powerup1") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.05f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("crepe_powerup2") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.1f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("crepe_powerup3") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.2f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("crepe_powerup4") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("crepe_powerup5") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.5f) - original_shokukan_p;
        }
    }

    void CakeBuf()
    {

        if (pitemlist.KosuCount("cakemold_stainless") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.25f) - original_shokukan_p;
        }
        else
        {
            if (pitemlist.KosuCount("cakemold_black") >= 1) //ケーキ型ブラック
            {
                _buf_shokukanup += (int)(_buf_shokukanup * 1.15f) - original_shokukan_p;
            }
        }

    }



    void TeaBuf()
    {
        if (pitemlist.KosuCount("tea_powerup1") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.05f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("tea_powerup2") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.1f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("tea_powerup3") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.2f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("tea_powerup4") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
        }
        if (pitemlist.KosuCount("tea_powerup5") >= 1) //
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.5f) - original_shokukan_p;
        }
    }

    void FlavorBuf()
    {
        //魔法のバフ
        _magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Tea_Study") >= 1)
        {
            _magicup = (int)(original_shokukan_p * magicskill_database.skillName_SearchLearnLevel("Tea_Study") * 0.06f); //元の値の6%上昇*LV

            _buf_shokukanup += _magicup;
        }

        //家によるバフ
        if (GameMgr.OrCompound_RoomNum == 2)
        {
            _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
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
        /*if (pitemlist.KosuCount("glass_bowl") >= 1) //もってないと、見た目が下がる
        {
            _buf_shokukanup -= 50;
        }*/
    }

    void AllShokukanBuf()
    {
        if (pitemlist.KosuCount("shokukan_powerup1") >= 1) //
        {
            _buf_shokukanup += 5;
        }
        if (pitemlist.KosuCount("shokukan_powerup2") >= 1) //
        {
            _buf_shokukanup += 10;
        }
        if (pitemlist.KosuCount("shokukan_powerup3") >= 1) //
        {
            _buf_shokukanup += 30;
        }

        //作った回数がわずかに食感に影響する
        if (GameMgr.System_CookCount_Buf)
        {
            cook_count_buf = 0;
            if (cook_kaisu > 0)
            {
                cook_count_buf = Mathf.FloorToInt(cook_kaisu / 3);
                if(cook_count_buf >= 25) { cook_count_buf = 25; } //上限
                _buf_shokukanup += (2 * cook_count_buf); //つまり、3回作ると、2*1になり、+2される。
                Debug.Log("cook_count_buf: " + cook_count_buf + " 食感アップ: +" + 2 * cook_count_buf);
            }
        }
    }

    void AllBeautifulBuf()
    {
        //魔法のバフ
        /*_magicup = 0;
        if (magicskill_database.skillName_SearchLearnLevel("Beautiful_Power") >= 1)
        {
            _magicup = magicskill_database.skillName_SearchLearnLevel("Beautiful_Power") * 10; //LV*10
            _buf_shokukanup += _magicup;
        }*/
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
    public int Buf_OkashiParamUp_ItemNameKeisan(int _status, string _basename, int _origin_param)
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        _buf_shokukanup = 0;
        original_shokukan_p = _origin_param;

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
                            _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
                        }

                        return _buf_shokukanup;

                    case 1: //ふわふわ感のバフ

                        if (_basename == "tiramisu" || _basename == "cafeaulait_creampuff")
                        {
                            _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
                        }

                        return _buf_shokukanup;

                    case 2: //なめらか感のバフ

                        break;

                    case 3: //歯ごたえ感のバフ

                        if (_basename == "biscotti")
                        {
                            _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
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
    public int Buf_OkashiParamUp_CompoNameKeisan(int _status, string _componame, int _origin_param)
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        _buf_shokukanup = 0;
        original_shokukan_p = _origin_param;

        switch (_componame)
        {
            case "whipped cream_row":

                //_buf_shokukanup = (int)(_buf_shokukanup * 1.3f);
                break;

            case "whipped cream_row_Free":

                //_buf_shokukanup = (int)(_buf_shokukanup * 1.3f);
                break;

            case "cream_row_ricotta":

                //_buf_shokukanup = (int)(_buf_shokukanup * 1.3f);
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
    //魔法の名前を直接指定して、どの食感(_status)に補正をかけるか指定して、書き込めばOK  各アトリは必要に応じて要素数増やす
    public int Buf_OkashiParamUp_MagicKeisan(int _status, int _baseparam, string _magicname, int _attri2, int _attri5, int _attri6, int _attri7)
    {

        _buf_shokukanup = 0;
        _magicup = 0;

        switch (_magicname)
        {
            case "Cookie_SecondBake":

                if (_status == 0 || _status == 1 || _status == 3)//さくさくか歯ごたえかふわふわのバフ
                {
                    _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Cookie_SecondBake");
                    _magicup = (int)(_baseparam * GameMgr.System_magic_playParamUp) - _baseparam;

                    Debug.Log("補正値: " + "_baseparam" + " * " + GameMgr.System_magic_playParamUp);
                    Debug.Log("各ゲージ補正値: " + GameMgr.System_magic_playParamUp);
                    Debug.Log("セカンドベイクの最終バフ: " + _magicup);
                    _buf_shokukanup += _magicup;
                }
                break;

            case "Chocolate_Tempering":

                if (_status == 2)//なめらかのバフ
                {
                    _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Chocolate_Philosophy");
                    _choco_hose1 = 1.0f + _magicLearnLv * 0.02f;

                    _magicup = (int)(_baseparam * 0.3f * GameMgr.System_magic_playParamUp * GameMgr.System_magic_playParamUp2 * GameMgr.System_magic_playParamUp3 *
                        _choco_hose1);

                    Debug.Log("_baseparam: " + _baseparam);
                    Debug.Log("補正値: " +
                       "_baseparam * 0.3f" + " * " + GameMgr.System_magic_playParamUp * GameMgr.System_magic_playParamUp2 * GameMgr.System_magic_playParamUp3 + " * " +
                        "(チョコレート哲学習得LV補正): " + _choco_hose1);
                    Debug.Log("各ゲージ補正値: " + GameMgr.System_magic_playParamUp + " " + GameMgr.System_magic_playParamUp2 + " " + GameMgr.System_magic_playParamUp3);
                    Debug.Log("テンパリングの最終バフ: " + _magicup);
                    _buf_shokukanup += _magicup;
                }
                break;

            case "Freezing_Spell": //フリージング　LVに応じてほんの少し上昇

                if (_status >= 0 && _status <= 6)//すべての食感
                {
                    if (_status != 5) //ただし、見た目はバフを無視。
                    {
                        _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Freezing_Spell");
                        _magicup = (int)(_baseparam * 0.04f * _magicLearnLv);
                        if (_magicup < 1) { _magicup = 1; } //必ず１は上がる

                        Debug.Log("フリージングバフ(LV*1.04倍): " + _magicup);
                        _buf_shokukanup += _magicup;
                    }
                }
                break;

            case "Wind_Ark":

                if (_status == 1)//ふわふわのバフ
                {
                    _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Wind_Ark");

                    if (_attri2 < 3) //重ね掛け2回までだと効果が小さい
                    {
                        _magicup = (int)(_baseparam * (0.1f + _magicLearnLv * 0.03f)); //大体元値の1.1倍 LV3で1.2倍
                        Debug.Log("_baseparam * (0.1f + ウィンドアーク習得LV * 0.03f) 習得LV: " + _magicLearnLv);
                    }
                    else
                    {    //3回以上重ね掛けするとき、効果が大きくなる                    
                        _magicup = (int)(_baseparam * (0.1f + _magicLearnLv * 0.06f)); //大体元値の1.25倍
                        Debug.Log("_baseparam * (0.1f + ウィンドアーク習得LV * 0.06f) 習得LV: " + _magicLearnLv);
                    }

                    if (_magicup < 1) { _magicup = 1; } //必ず１は上がる

                    Debug.Log("ウィンドアークの最終バフ: " + _magicup);
                    _buf_shokukanup += _magicup;
                }
                if (_status == 2 || _status == 3)//なめらかor歯ごたえのバフ
                {
                    _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Wind_Ark");

                    if (_attri2 < 3) //重ね掛け2回までだと効果が小さい
                    {
                        _magicup = (int)(_baseparam * (0.1f + _magicLearnLv * 0.02f)); //大体元値の1.15倍
                        Debug.Log("_baseparam * (0.1f + ウィンドアーク習得LV * 0.02f) 習得LV: " + _magicLearnLv);
                    }
                    else
                    {    //3回以上重ね掛けするとき、効果が大きくなる 
                        _magicup = (int)(_baseparam * (0.1f + _magicLearnLv * 0.05f)); //1.2倍
                        Debug.Log("_baseparam * (0.1f + ウィンドアーク習得LV * 0.05f) 習得LV: " + _magicLearnLv);
                    }
                    if (_magicup < 1) { _magicup = 1; } //必ず１は上がる

                    Debug.Log("ウィンドアークの最終バフ: " + _magicup);
                    _buf_shokukanup += _magicup;
                }
                break;

            case "Fire_Ark":

                if (_status == 0)//さくさくのバフ
                {
                    _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Fire_Ark");

                    if (_attri5 < 3) //重ね掛け2回までだと効果が小さい
                    {
                        _magicup = (int)(_baseparam * (0.1f + _magicLearnLv * 0.03f)); //大体元値の1.1倍 LV3で1.2倍
                        Debug.Log("_baseparam * (0.1f + ファイアアーク習得LV * 0.03f) 習得LV: " + _magicLearnLv);
                    }
                    else
                    {    //3回以上重ね掛けするとき、効果が大きくなる                    
                        _magicup = (int)(_baseparam * (0.1f + _magicLearnLv * 0.06f)); //大体元値の1.25倍
                        Debug.Log("_baseparam * (0.1f + ファイアアーク習得LV * 0.06f) 習得LV: " + _magicLearnLv);
                    }

                    if (_magicup < 1) { _magicup = 1; } //必ず１は上がる

                    Debug.Log("ファイアアークの最終バフ: " + _magicup);
                    _buf_shokukanup += _magicup;
                }

                break;

            case "Warming_Handmade": //手作りの温もり

                if (_status >= 0 && _status <= 6)//すべての食感
                {
                    if (_status != 5) //ただし、見た目はバフを無視。
                    {
                        _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Warming_Handmade");
                        _magicup = (int)(_baseparam * (0.12f + _magicLearnLv * 0.03f)); //1.12倍～1.2倍
                        if (_magicup < 1) { _magicup = 1; } //必ず１は上がる

                        Debug.Log("_baseparam * (0.12f + 手作りの温もり習得LV * 0.03f) 習得LV: " + _magicLearnLv); //大体1.12~1.2倍 ただしハートを消費してしまう。
                        Debug.Log("手作りの温もりの最終バフ: " + _magicup);
                        _buf_shokukanup += _magicup;
                    }
                }
                break;

            case "Dreamy_Sapphire": //夢見るサファイア

                if (_status == 5) //見た目
                {
                    _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Dreamy_Sapphire");
                    _magicup = _magicLearnLv * 10;
                    if (_magicup < 1) { _magicup = 1; } //必ず１は上がる

                    Debug.Log("夢見るサファイアの見た目　最終バフ: " + _magicup);
                    _buf_shokukanup += _magicup;
                }
                break;

            case "AbraCadabra": //アブタラカブタラ

                if (_status >= 0 && _status <= 6)//すべての食感
                {
                    if (_status != 5) //ただし、見た目はバフを無視。
                    {
                        rnd = Random.Range(0, 300);
                        _magicup = rnd;
                        _buf_shokukanup += _magicup;
                    }
                }
                break;

        }

        return _buf_shokukanup;
    }

    //
    //特定の魔法で甘さ・苦さ・酸味の味パラメータに補正をかける _statusで、甘さ・苦さ・酸味を決定
    //
    public int Buf_SweatsParamUp_MagicKeisan(int _status, int _baseparam, string _magicname, int _attri6)
    {
        Debug.Log("魔法で甘さ・酸味・苦さを変質");

        _buf_shokukanup = 0;
        _magicup = 0;

        switch (_magicname)
        {
            case "Lightning_Grape":

                if (_status == 1) //酸味を変化 すっぱくする
                {
                    _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Lightning_Grape");
                    if (_magicLearnLv == 1)
                    {
                        _magicup = 10;
                    }
                    else if (_magicLearnLv >= 2)
                    {
                        _magicup = -1 * (_baseparam - GameMgr.UseMagicParamCustom_FinalScore);
                    }                   

                    _buf_shokukanup += _magicup;
                }
                break;

            case "Dreamy_Sapphire":

                if (_status == 0) //甘さを変化 具体的には甘さを減らす
                {
                    _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Dreamy_Sapphire");
                    if (_magicLearnLv == 1)
                    {
                        _magicup = -(_baseparam / 2);
                    }
                    else if (_magicLearnLv >= 2)
                    {
                        _magicup = -1 * (_baseparam - GameMgr.UseMagicParamCustom_FinalScore);
                    }

                    _buf_shokukanup += _magicup;
                }
                /*if (_status == 1) //酸味を変化
                {
                    _magicLearnLv = magicskill_database.skillName_SearchLearnLevel("Dreamy_Sapphire");
                    _magicup = 2;

                    _buf_shokukanup += _magicup;
                }*/
                break;
        }

        return _buf_shokukanup;
    }


    //
    //甘さ・苦さ・酸味の点数に補正をかける
    //アイテムのサブタイプ(_itemType_sub)を指定し、中で補正をかければOK
    //
    public int Buf_SweatBitterSour_Keisan(int _taste_score, string _itemType_sub_t, string _itemType_sub_tb)
    {
        _itemType_sub = _itemType_sub_t;
        _itemType_subB = _itemType_sub_tb;
        taste_score = _taste_score;

        switch (_itemType_sub)
        {
            case "Suger":

                break;

            case "Cake":

                break;
        }

        return taste_score;
    }


    //
    //配合比率の距離に補正をかける。
    //アイテムのサブタイプ(_itemType_sub)を指定し、中で補正をかければOK
    //
    public float Buf_KyoriHosei_Keisan(float _param, string _result_item)
    {
        InitSetup();

        _buf_kyori = _param;

        _id = database.SearchItemIDString(_result_item);
        _itemType = database.items[_id].itemType.ToString();
        _itemType_sub = database.items[_id].itemType_sub.ToString();
        _itemType_subB = database.items[_id].itemType_subB.ToString();

        switch(_itemType_sub)
        {
            
            case "Cookie":

                //魔法のバフ
                _magicup_f = 0;
                if (magicskill_database.skillName_SearchLearnLevel("Cookie_Study") >= 1)
                {
                    _magicup_f = 1.0f + magicskill_database.skillName_SearchLearnLevel("Cookie_Study") * 0.1f; //LV*0.1
                    _buf_kyori = _buf_kyori * _magicup_f;
                    
                }
                break;

            case "Cookie_Hard":

                //魔法のバフ
                _magicup_f = 0;
                if (magicskill_database.skillName_SearchLearnLevel("Cookie_Study") >= 1)
                {
                    _magicup_f = 1.0f + magicskill_database.skillName_SearchLearnLevel("Cookie_Study") * 0.1f; //LV*0.1
                    _buf_kyori = _buf_kyori * _magicup_f;

                }
                break;

            case "Chocolate":

                //魔法のバフ
                _magicup_f = 0;
                if (magicskill_database.skillName_SearchLearnLevel("Chocolate_Philosophy") >= 1)
                {
                    _magicup_f = 1.0f + magicskill_database.skillName_SearchLearnLevel("Chocolate_Philosophy") * 0.1f; //LV*
                    _buf_kyori = _buf_kyori * _magicup_f;
                }
                break;

            case "Appaleil_Icecream":

                //魔法のバフ
                _magicup_f = 0;
                if (magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") >= 1)
                {
                    _magicup_f = 1.0f + magicskill_database.skillName_SearchLearnLevel("Heart_of_Icecream") * 0.2f; //LV*5
                    _buf_kyori = _buf_kyori * _magicup_f;
                }
                break;
        }

        //Debug.Log("材料距離　補正後: " + _buf_kyori + " 種類: " + _itemType_sub);
        return _buf_kyori; //なにもない場合は、そのまま入れた数値がかえる。
    }

    //ヒカリの作ったお菓子に、バフをかける処理
    public int Buf_HikariParamUp_Keisan(int _status, string _itemType_sub, int _origin_param)
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        _buf_shokukanup = 0;
        original_shokukan_p = _origin_param;

        if (pitemlist.KosuCount("hikari_powerup1") >= 1) //
        {
            if (_status != 5) //見た目以外は上昇
            {
                _buf_shokukanup += (int)(original_shokukan_p * 1.1f) - original_shokukan_p;
            }
        }
        if (pitemlist.KosuCount("hikari_powerup2") >= 1) //
        {
            if (_status != 5) //見た目以外は上昇
            {
                _buf_shokukanup += (int)(original_shokukan_p * 1.3f) - original_shokukan_p;
            }
        }
        if (pitemlist.KosuCount("hikari_powerup3") >= 1) //
        {
            if (_status != 5) //見た目以外は上昇
            {
                _buf_shokukanup += (int)(original_shokukan_p * 1.5f) - original_shokukan_p;
            }
        }

        switch (_status)
        {
            case 50: //粉っぽさ

                Shokukanup_MagicPStatusBuf(); //ヒカリのおかしにも、ラトリアの効果がのる
                break;

            case 51: //油っぽさ

                Shokukanup_MagicPStatusBuf();
                break;

            case 52: //水っぽさ

                Shokukanup_MagicPStatusBuf();
                break;
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
        hikariOkashiExpTable.hikariOkashi_ExpTableMethod(_itemType_sub, 0, 0, 1, 0, GameMgr.hikari_makingmethod);
        
        if (GameMgr.hikariokashiExpTable_noTypeflag)
        {
            //どのお菓子タイプにもあてはまらなかったら、計算しない。
            //GameMgr.hikari_make_okashiTime_costbuf = 1.0f;
            //GameMgr.hikari_make_okashiTime_successrate_buf = 1.0f;
            //GameMgr.hikari_make_okashiKosu_buf = 1.0f;
            //GameMgr.hikari_make_okashiKosu_buf_keisan = 1.0f;

            hikari_okashiLV = 1;
            HikariOkashilv_Keisan(_itemType_sub, 1); //実際のバフ率を計算
        }
        else
        {
            hikari_okashiLV = GameMgr.hikarimakeokashi_nowlv;
            HikariOkashilv_Keisan(_itemType_sub, 0); //実際のバフ率を計算
        }        
    }

    void HikariOkashilv_Keisan(string _itemType_sub, int _mstatus)
    {
        //食感への補正
        if (_mstatus == 0)
        {
            _a = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 0.8f, 1.5f); //最大LVで、にいちゃんの1.5倍上がる LVあげなくても普通に作れる　あまりやると強すぎ
            _buf_hikari_okashiparam = 0.1f + _a;
        }
        else if (_mstatus == 1)
        {
            _buf_hikari_okashiparam = 1.0f;
        }

        //個数の補正　最終的ににいちゃんと同じ数 LVをあげなくても普通に個数　LV上がると個数がどんどん上がる
        if (_mstatus == 0)
        {
            if (hikari_okashiLV >= 1.0f && hikari_okashiLV < 3.0f)
            {
                _kosuhosei = 1.0f;
            }
            else if (hikari_okashiLV >= 3.0f && hikari_okashiLV < 7.0f)
            {
                _kosuhosei = 0.5f;
            }
            else if (hikari_okashiLV >= 7.0f && hikari_okashiLV < 9.0f)
            {
                _kosuhosei = 0.25f;
            }
            else if (hikari_okashiLV >= 9.0f)
            {
                _kosuhosei = 0.125f;
            }
        }
        else if (_mstatus == 1)
        {
            _kosuhosei = 1.0f;
            GameMgr.hikari_make_okashiKosu_buf = 1.0f;
        }
        GameMgr.hikari_make_okashiKosu_buf_keisan = _kosuhosei; //Exp_Controllerでhikari_make_okashiKosu_bufに入れて確定させる
        if (GameMgr.hikari_make_okashiKosu_buf_keisan == 0) { GameMgr.hikari_make_okashiKosu_buf_keisan = 1.0f; }//例外処理　０で割らないようにする。

        //最終的にかかる時間は、Exp_Controllerで計算
        if (_mstatus == 0)
        {
            GameMgr.hikari_make_okashiTime_costbuf = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 1.0f, 0.3f); //LV1~9 を　3~1倍に変換。LV9で、通常の兄ちゃんの速度の3倍
        }
        else if (_mstatus == 1)
        {
            GameMgr.hikari_make_okashiTime_costbuf = 1.0f;
        }

        if (pitemlist.KosuCount("hikari_speed_up1") >= 1) //
        {
            GameMgr.hikari_make_okashiTime_costbuf = GameMgr.hikari_make_okashiTime_costbuf * 0.75f;
        }
        if (pitemlist.KosuCount("hikari_speed_up2") >= 1) //持ってるだけで効果アップ
        {
            GameMgr.hikari_make_okashiTime_costbuf = GameMgr.hikari_make_okashiTime_costbuf * 0.5f;
        }
        

        if (GameMgr.hikari_make_okashiTime_costbuf <= 0.1f)
        {
            GameMgr.hikari_make_okashiTime_costbuf = 0.1f;
        }
        //Debug.Log("hikari_okashiLV: " + hikari_okashiLV + " " + "GameMgr.hikari_make_okashiTime_costbuf: " + GameMgr.hikari_make_okashiTime_costbuf);


        //最終的な成功率は、Compound_Checkで計算
        if (_mstatus == 0)
        {
            GameMgr.hikari_make_okashiTime_successrate_buf = SujiMap(hikari_okashiLV, 1.0f, 9.0f, 0.8f, 1.5f); //成功率　LV1~9 を　0.8から1.5に変換。
        }
        else if (_mstatus == 1)
        {
            GameMgr.hikari_make_okashiTime_successrate_buf = 1.0f;
        }

        if (pitemlist.KosuCount("green_pendant") >= 1) //持ってるだけで効果アップ
        {
            GameMgr.hikari_make_okashiTime_successrate_buf += 0.1f;
        }        

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
            case "Appaleil_Icecream":
                GameMgr.hikari_make_okashiTime_successrate_buf = 0.95f;
                break;
            case "Source":
                GameMgr.hikari_make_okashiTime_successrate_buf = 0.95f;
                break;
            case "Potion":
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
        hikariOkashiExpTable.hikariOkashi_ExpTableMethod(_itemType_sub, 0, 0, 1, 0, GameMgr.hikari_makingmethod);

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

        Debug.Log("ヒカリのおかしLVによる作ったお菓子へのバフ LV: " + hikari_okashiLV + " Param: " + _buf_hikari_okashi_paramup);
        return _buf_hikari_okashi_paramup;
    }


    //温度管理によるバフの計算
    public float TempatureControlKeisan(float _basewelldone, int _control_temp, int _control_time)
    {
        _best_well_done = _basewelldone;

        _tempature_param = SujiMap(_control_temp * _control_temp,
                        GameMgr.System_tempature_control_tempMin * GameMgr.System_tempature_control_tempMin,
                        GameMgr.System_tempature_control_tempMax * GameMgr.System_tempature_control_tempMax,
                        2.0f, 6.0f); //ここで焼き具合ゲージを決定してる。
        _well_done = _tempature_param * _control_time;

        Debug.Log("_tempature_param: " + _tempature_param);
        Debug.Log("_well_done: " + _well_done);
        Debug.Log("_best_well_done: " + _best_well_done);

        _well_done_kyori = Mathf.Abs(_best_well_done - _well_done); //ベストな焼き具合と、今回の焼き具合との差　差が近いほど、高得点
        _well_done_kyori_noabs = _best_well_done - _well_done;
        Debug.Log("ベスト温度との距離: " + _well_done_kyori);

        //余熱石をもってると、さらに温度管理の効果あがる
        if (pitemlist.KosuCount("residual_heatstone") >= 1) //持ってるだけで効果アップ
        {
            _yonetsu_hosei = 1.2f;
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
