using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardEffectDataBase : SingletonMonoBehaviour<ItemCardEffectDataBase>
{
    //演出魔法のエフェクトと計算を実装するときは、
    //このDB・ItemCardEffectPanelの2つにエフェクトデータを実装すること
    //こっちは計算用

    private ItemDataBase database;
    private MagicSkillListDataBase magicskill_database;

    private int i, j;

    private List<string> _MS_mariage = new List<string>();
    private List<int> _MS_pointup = new List<int>();

    private int check_counter;
    private string _ms_aisho;
    private string aisho_text1, aisho_text2, aisho_text3;

    public int _compatible;
    public string item_MS_aisho;  
    public int _ms_sp_score1;
    public int _ms_sp_score2;
    public int _ms_sp_score3;
    public int _ms_sp_score4;
    public int _ms_sp_score5;
    public int _ms_sp_score6;
    public int _ms_sp_score7;
    public int _ms_sp_score8;
    public int _ms_sp_score9;
    public int _ms_sp_score10;
    public int _basemagicslot_on;
    public string _basemagicslot_Name;
    public int _add_magicbeauty;

    public string _addMS;
    public int _addMSvalue;
    public int _addMagic;

    private int _mlv;
    private float _tempf, up_magni;

    // Start is called before the first frame update
    void Start()
    {
        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMagicSlot_Keisan(string _magicname, int _value)
    {
        _addMS = "Non";
        _addMSvalue = 0;
        _addMagic = 0;

        switch (_magicname)
        {
            case "Fire_Flowers":
                _addMS = GameMgr.System_MagicSlotName01;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Buttelfy_illumination":
                _addMS = GameMgr.System_MagicSlotName02;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Bubble_Mist":
                _addMS = GameMgr.System_MagicSlotName03;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Star_Blessing":
                _addMS = GameMgr.System_MagicSlotName04;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Wind_Ark": //今は演出魔法でなく、バフをかけるのみ
                _addMS = "Non"; //GameMgr.System_MagicSlotName05
                _addMSvalue = 0;
                _addMagic = 1;
                break;

            case "Mnemonic":
                _addMS = GameMgr.System_MagicSlotName06;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Glitter":
                _addMS = GameMgr.System_MagicSlotName07;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Spring_Pharmacy":
                _addMS = GameMgr.System_MagicSlotName08;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Saint_Fleur":
                _addMS = GameMgr.System_MagicSlotName09;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Santiman":
                _addMS = GameMgr.System_MagicSlotName10;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Rainbow_Rain":
                _addMS = GameMgr.System_MagicSlotName11;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Night_Barron":
                _addMS = GameMgr.System_MagicSlotName12;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            case "Crescent_Moon":
                _addMS = GameMgr.System_MagicSlotName13;
                _addMSvalue = _value;
                _addMagic = 1;
                break;

            default:
                
                break;
        }
    }


    //
    //MagicSlotの計算用＋カード表示用
    //
    public void MagicEffect_SlotKeisan(string[] _magicslot, int[] _msvalue, int _itemID, int _mstatus) //_mstatusは、アクセスする元のオブジェクト場所
    {
        _MS_mariage.Clear();
        _MS_pointup.Clear();

        item_MS_aisho = "";
        aisho_text1 = "";
        aisho_text2 = "";
        aisho_text3 = "";

        _basemagicslot_on = 0;
        _basemagicslot_Name = "";
        _compatible = 0;
        _ms_sp_score1 = 0;
        _ms_sp_score2 = 0;
        _ms_sp_score3 = 0;
        _ms_sp_score4 = 0;
        _ms_sp_score5 = 0;
        _ms_sp_score6 = 0;
        _ms_sp_score7 = 0;
        _ms_sp_score8 = 0;
        _ms_sp_score9 = 0;
        _ms_sp_score10 = 0;
        _add_magicbeauty = 0;

        GameMgr.UseMagicSkill_HikariCommentFlag = 0; //Girl1_statusのヒカリのふきだしでしか使わない
        GameMgr.UseMagicSkill_HikariComment = "";

        if (_mstatus == 0)
        {
            check_counter = database.SearchItemID(_itemID);
        }
        else
        {
            check_counter = _itemID; //直接リスト番号入れてる
        }
        _MS_mariage.Add(database.items[check_counter].MS1_mariage);
        _MS_mariage.Add(database.items[check_counter].MS2_mariage);
        _MS_mariage.Add(database.items[check_counter].MS3_mariage);
        _MS_mariage.Add(database.items[check_counter].MS4_mariage);
        _MS_mariage.Add(database.items[check_counter].MS5_mariage);
        _MS_mariage.Add(database.items[check_counter].MS6_mariage);
        _MS_mariage.Add(database.items[check_counter].MS7_mariage);
        _MS_mariage.Add(database.items[check_counter].MS8_mariage);
        _MS_mariage.Add(database.items[check_counter].MS9_mariage);
        _MS_mariage.Add(database.items[check_counter].MS10_mariage);
        _MS_pointup.Add(database.items[check_counter].MS1_pointup);
        _MS_pointup.Add(database.items[check_counter].MS2_pointup);
        _MS_pointup.Add(database.items[check_counter].MS3_pointup);
        _MS_pointup.Add(database.items[check_counter].MS4_pointup);
        _MS_pointup.Add(database.items[check_counter].MS5_pointup);
        _MS_pointup.Add(database.items[check_counter].MS6_pointup);
        _MS_pointup.Add(database.items[check_counter].MS7_pointup);
        _MS_pointup.Add(database.items[check_counter].MS8_pointup);
        _MS_pointup.Add(database.items[check_counter].MS9_pointup);
        _MS_pointup.Add(database.items[check_counter].MS10_pointup);

        for (i = 0; i < _magicslot.Length; i++)
        {
            //Debug.Log("_magicslot " + i + ": " + _magicslot[i]);

            if (_magicslot[i] == GameMgr.System_MagicSlotName01) //FireFlowerの場合　花火が周りにとびちるエフェクト
            {                                
                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName01) //FireFlowers
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni); //_msvalue[i]は習得LV
                        _ms_sp_score2 += (int)(_MS_pointup[j] / 4 * up_magni); //夏らしさを足す   
                        _ms_sp_score6 += (int)(_MS_pointup[j] / 5 * up_magni); //子供っぽさを足す                         
                    }
                }

                //相性に関係なく、必ず点数を足す
                _ms_sp_score2 += 10; //海らしさを足す   
                _ms_sp_score6 += 10;
                
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "海らしさ + " + _ms_sp_score2.ToString();
                aisho_text3 = "子供っぽい + " + _ms_sp_score6.ToString();
                item_MS_aisho = "花火: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;
                
                _basemagicslot_Name = GameMgr.System_MagicSlotName01;

                GameMgr.UseMagicSkill_HikariComment = "花火～～！きれい～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName02) //Butterflyの場合、光のちょうちょがとぶ
            {
                
                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName02)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score7 += (int)(_MS_pointup[j] / 4 * up_magni); //メルヘンを足す
                        _ms_sp_score5 -= (int)(_MS_pointup[j] / 4 * up_magni); //大人っぽさを減らす 
                    }
                }

                //相性に関係なく、必ず点数を足す                
                _ms_sp_score7 += 10;
                _ms_sp_score9 += 5; //光らしさを加算
              
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "メルヘン + " + _ms_sp_score7.ToString();
                aisho_text3 = "大人 " + _ms_sp_score5.ToString();
                item_MS_aisho = "ちょうちょ: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName02;

                GameMgr.UseMagicSkill_HikariComment = "にいちゃん！　ちょうちょ～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName03) //Bubbleは泡がでる
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    //Debug.Log("_MS_mariage " + j + ": " + _MS_mariage[j]);
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName03)
                    {
                        //Debug.Log("あわあわ一致　テキスト表示");
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score2 += (int)(_MS_pointup[j] / 4 * up_magni); //海らしさを加算

                        
                    }
                }

                //相性に関係なく、必ず点数を足す
                _ms_sp_score2 += 5; //海らしさを加算
               
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "海らしさ + " + _ms_sp_score2.ToString();
                item_MS_aisho = "あわあわ: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName03;

                GameMgr.UseMagicSkill_HikariComment = "あわあわ～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName04) //Starは星くずがキラキラする
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName04)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score6 += (int)(_MS_pointup[j] / 6 * up_magni); //子供っぽさを足す  
                        _ms_sp_score5 -= (int)(_MS_pointup[j] / 6 * up_magni); //大人っぽさを減らす 
                    }
                }

                //相性に関係なく、必ず点数を足す
                _ms_sp_score6 += 7;
                
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "子供っぽい + " + _ms_sp_score6.ToString();
                aisho_text3 = "大人 " + _ms_sp_score5.ToString();
                item_MS_aisho = "星屑: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName04;

                GameMgr.UseMagicSkill_HikariComment = "おほしさまキラキラ～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName05) //WindArc　風の円弧が周りにとびちる
            {
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目
                _basemagicslot_Name = GameMgr.System_MagicSlotName05;
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName06) //Mnemonic 貝殻やおさかなを飛ばす魔法
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName06)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score2 += (int)(_MS_pointup[j] / 4 * up_magni); //海らしさを加算
                        _ms_sp_score6 += (int)(_MS_pointup[j] / 4 * up_magni); //子供っぽさを足す
                       
                    }
                }

                //相性に関係なく、必ず点数を足す
                _ms_sp_score2 += 10; //海らしさを加算
                _ms_sp_score6 += 5; //子供っぽさを足す
                
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "海らしさ + " + _ms_sp_score2.ToString();
                aisho_text3 = "子供っぽい + " + _ms_sp_score6.ToString();
                item_MS_aisho = "貝殻: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName06;

                GameMgr.UseMagicSkill_HikariComment = "かいがらきれい～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName07) //Glitter 光りのキラキラを飛ばす魔法
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName07)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score9 += (int)(_MS_pointup[j] / 4 * up_magni); //光らしさを加算
                    }
                }
                //相性に関係なく、必ず点数を足す
                _ms_sp_score9 += 10; //光らしさを加算
                
                _add_magicbeauty += 5; //見た目は+5

                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "キラキラ感 + " + _ms_sp_score9.ToString();
                item_MS_aisho = "キラキラ: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName07;

                GameMgr.UseMagicSkill_HikariComment = "キラキラがかわいい～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName08) //Spring_Pharmacy さくらの花びらを飛ばす魔法
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName08)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score10 += (int)(_MS_pointup[j] / 4 * up_magni); //和風感を加算

                    }
                }
                //相性に関係なく、必ず点数を足す
                _ms_sp_score10 += 10; //和風感を加算
                
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "和風感 + " + _ms_sp_score10.ToString();
                item_MS_aisho = "さくら: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName08;

                GameMgr.UseMagicSkill_HikariComment = "風流だねぇ～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName09) //Saint_Fleur お花を飛ばす魔法
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName09)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score7 += (int)(_MS_pointup[j] / 4 * up_magni); //メルヘンを加算
                        _ms_sp_score5 -= (int)(_MS_pointup[j] / 4 * up_magni); //大人っぽさを減らす 
                    }
                }
                //相性に関係なく、必ず点数を足す
                _ms_sp_score7 += 10; //メルヘンを加算
               
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "メルヘン + " + _ms_sp_score7.ToString();
                aisho_text3 = "大人 " + _ms_sp_score5.ToString();
                item_MS_aisho = "お花: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName09;

                GameMgr.UseMagicSkill_HikariComment = "お花、かわいい～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName10) //Santiman ハートを飛ばす魔法
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName10)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score3 += (int)(_MS_pointup[j] / 3 * up_magni); //愛を加算
                        
                    }
                }

                //相性に関係なく、必ず点数を足す
                _ms_sp_score3 += 10; //愛を加算
                _add_magicbeauty += 10; //見た目は+10

                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "愛らしさ + " + _ms_sp_score3.ToString();
                item_MS_aisho = "ハート: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName10;

                GameMgr.UseMagicSkill_HikariComment = "ハートかわいい～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName11) //Rainbow_Rain
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName11)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score4 += (int)(_MS_pointup[j] / 3 * up_magni); //宇宙を加算
                        _ms_sp_score8 += (int)(_MS_pointup[j] / 3 * up_magni); //芸術を加算
                    }
                }

                //相性に関係なく、必ず点数を足す
                _ms_sp_score7 += 10; //メルヘンを加算
                _ms_sp_score8 += 12; //芸術加算
                _ms_sp_score6 += 10; //子供も加算（表記無）
                
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "メルヘン + " + _ms_sp_score7.ToString();
                aisho_text3 = "芸術 " + _ms_sp_score8.ToString();
                item_MS_aisho = "虹: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName11;

                GameMgr.UseMagicSkill_HikariComment = "虹がういてる～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName12) //Night_Barron
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName12)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score5 += (int)(_MS_pointup[j] / 3 * up_magni); //大人を加算

                    }
                }

                //相性に関係なく、必ず点数を足す
                _ms_sp_score5 += 10; //大人を加算
                
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "大人 + " + _ms_sp_score5.ToString();
                item_MS_aisho = "ダンディ: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName12;

                GameMgr.UseMagicSkill_HikariComment = "かっこいい。ダンディ～♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }

            if (_magicslot[i] == GameMgr.System_MagicSlotName13) //Crescent_Moon
            {

                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName13)
                    {
                        up_magni = UpParamKeisan(_msvalue[i]);
                        _compatible = (int)(_MS_pointup[j] * up_magni);
                        _ms_sp_score4 += (int)(_MS_pointup[j] / 4 * up_magni); //愛を加算
                        _ms_sp_score8 += (int)(_MS_pointup[j] / 2 * up_magni); //芸術を加算

                    }
                }

                //相性に関係なく、必ず点数を足す
                _ms_sp_score4 += 12; //宇宙を加算
                _ms_sp_score8 += 6; //芸術を加算
                _ms_sp_score7 += 10; //メルヘンを加算（表記無）
                
                MS_aisho_database(_compatible, _msvalue[i]);
                Common_Keisan(_compatible, _msvalue[i]); //演出魔法をかけると必ず上がる項目

                aisho_text2 = "宇宙 + " + _ms_sp_score4.ToString();
                aisho_text3 = "芸術 " + _ms_sp_score8.ToString();
                item_MS_aisho = "三日月: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2 + " " + aisho_text3;

                _basemagicslot_Name = GameMgr.System_MagicSlotName13;

                GameMgr.UseMagicSkill_HikariComment = "ふしぎなお月様♪"; //Girl1_statusのヒカリ吹き出しでコメント使用
            }
        }
    }

    //魔法習得LVに応じた上げ幅の設定
    float UpParamKeisan(int _magiclv)
    {
        if (_magiclv > 1)
        {
            _tempf = 1.0f + (0.3f * _magiclv); //LV*0.3ずつ
        }
        else if (_magiclv == 1)
        {
            _tempf = 1.0f;
        }
        return _tempf;
    }

    void MS_aisho_database(int _compa, int _magiclv)
    {
        _ms_aisho = "";

        if (_compa < 0)
        {
            _add_magicbeauty -= _compa * 2;
            _ms_aisho = "×";           
        }
        if (_compa >= 0 && _compa < 10) //相性とくになし
        {
            _add_magicbeauty += _compa;
            _ms_aisho = "▲";
        }
        else if (_compa >= 10 && _compa < 30) //まあまあ
        {
            _add_magicbeauty += (int)(_compa * 1.12f);
            _ms_aisho = "〇";
        }
        else if (_compa >= 30 && _compa < 50) //相性ぴったり
        {
            _add_magicbeauty += (int)(_compa * 1.25f);
            _ms_aisho = "◎";

            GameMgr.UseMagicSkill_HikariCommentFlag = 1;
        }
        else if (_compa >= 50) //最高に相性がいい
        {
            _add_magicbeauty += (int)(_compa * 1.35f);
            _ms_aisho = "☆";
            GameMgr.UseMagicSkill_HikariCommentFlag = 1;
        }

        if (GameMgr.OrCompound_RoomNum == 8)
        {
            _add_magicbeauty = (int)(_add_magicbeauty * 1.3f);
        }

        
    }

    void RoomBufKeisan()
    {
        if (GameMgr.OrCompound_RoomNum == 8)
        {
            Buf_SpScoreKeisan(1.3f);
        }
    }

    void Common_Keisan(int _compa, int _magiclv)
    {
        _basemagicslot_on = 1; //加点がなくても、魔法はかかってるので、魔法のおかし扱いにはなる。

        _add_magicbeauty += 10; //演出魔法をかけると一律上がる
        if(_magiclv > 1)//LV2以上だと、さらに3ずつ見た目上昇
        {
            _add_magicbeauty += _magiclv * 3;
        }

        if (_compa >= 0 && _compa < 10)
        {
            _ms_sp_score8 += 10; //演出をかけると、どの演出魔法も全てのおかしに芸術性を10点加える。
        }
        else if (_compa >= 10)
        {
            _ms_sp_score8 += 20; //こっちは20
        }
        else
        {
            _ms_sp_score8 = 0;
        }

        RoomBufKeisan();

        //美の研究を習得していれば、上昇値があがる
        _mlv = magicskill_database.skillName_SearchLearnLevel("Beautiful_Power");
        if(_mlv > 0)
        {
            Buf_SpScoreKeisan(1.0f + (0.18f * _mlv));
            _add_magicbeauty = (int)(_add_magicbeauty * (1.0f + (0.04f * _mlv)));
        }

        aisho_text1 = "見た目 + " + _add_magicbeauty.ToString();
    }

    void Buf_SpScoreKeisan(float _buf)
    {
        _ms_sp_score1 = (int)(_ms_sp_score1 * _buf);
        _ms_sp_score2 = (int)(_ms_sp_score2 * _buf);
        _ms_sp_score3 = (int)(_ms_sp_score3 * _buf);
        _ms_sp_score4 = (int)(_ms_sp_score4 * _buf);
        _ms_sp_score5 = (int)(_ms_sp_score5 * _buf);
        _ms_sp_score6 = (int)(_ms_sp_score6 * _buf);
        _ms_sp_score7 = (int)(_ms_sp_score7 * _buf);
        _ms_sp_score8 = (int)(_ms_sp_score8 * _buf);
        _ms_sp_score9 = (int)(_ms_sp_score9 * _buf);
        _ms_sp_score10 = (int)(_ms_sp_score10 * _buf);
    }
}
