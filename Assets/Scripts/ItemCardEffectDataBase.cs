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

    // Start is called before the first frame update
    void Start()
    {
        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MagicEffect_SlotKeisan(string[] _magicslot, int[] _msvalue, int _itemID, int _mstatus) //_mstatusは、アクセスする元のオブジェクト場所
    {
        _MS_mariage.Clear();
        _MS_pointup.Clear();

        item_MS_aisho = "";
        aisho_text1 = "";
        aisho_text2 = "";
        aisho_text3 = "";

        _basemagicslot_on = 0;
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
        _MS_pointup.Add(database.items[check_counter].MS1_pointup);
        _MS_pointup.Add(database.items[check_counter].MS2_pointup);
        _MS_pointup.Add(database.items[check_counter].MS3_pointup);

        for (i = 0; i < _magicslot.Length; i++)
        {
            Debug.Log("_magicslot " + i + ": " + _magicslot[i]);

            if (_magicslot[i] == GameMgr.System_MagicSlotName01) //FireFlowerの場合　花火が周りにとびちるエフェクト
            {
                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点　なければ0点か減点。
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName01) //FireFlowers
                    {
                        _compatible = _MS_pointup[j] * _msvalue[i];
                        _ms_sp_score6 = _MS_pointup[j] / 3 * _msvalue[i]; //子供っぽさを足す
                        _ms_sp_score8 = _MS_pointup[j] / 5 * _msvalue[i]; //芸術性を足す　パーティのお客さん向け         

                        aisho_text1 = "見た目 + " + _compatible.ToString();
                        aisho_text2 = "子供っぽい + " + _ms_sp_score6.ToString();
                    }
                }
                MS_aisho_database(_compatible);
                item_MS_aisho = "花火: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2;

                _basemagicslot_on = 1; //加点がなくても、魔法はかかってるので、魔法のおかし扱いにはなる。
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName02) //Butterflyの場合、光のちょうちょがとぶ
            {
                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点　なければ0点か減点。
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName02)
                    {
                        _compatible = _MS_pointup[j] * _msvalue[i];
                        _ms_sp_score7 = _MS_pointup[j] / 3 * _msvalue[i]; //メルヘンを足す

                        aisho_text1 = "見た目 + " + _compatible.ToString();
                        aisho_text2 = "メルヘン + " + _ms_sp_score7.ToString();
                    }
                }
                MS_aisho_database(_compatible);
                item_MS_aisho = "ちょうちょ: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2;

                _basemagicslot_on = 1; //加点がなくても、魔法はかかってるので、魔法のおかし扱いにはなる。
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName03) //Bubbleは泡がでる
            {
                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点　なければ0点か減点。
                {
                    //Debug.Log("_MS_mariage " + j + ": " + _MS_mariage[j]);
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName03)
                    {
                        //Debug.Log("あわあわ一致　テキスト表示");
                        _compatible = _MS_pointup[j] * _msvalue[i];
                        _ms_sp_score2 = _MS_pointup[j] / 2 * _msvalue[i]; //海らしさを加算

                        aisho_text1 = "見た目 + " + _compatible.ToString();
                        aisho_text2 = "海らしさ + " + _ms_sp_score2.ToString();
                    }
                }
                MS_aisho_database(_compatible);
                item_MS_aisho = "あわあわ: " + _ms_aisho + "　" + aisho_text1 + "\n" + aisho_text2;

                _basemagicslot_on = 1; //加点がなくても、魔法はかかってるので、魔法のおかし扱いにはなる。
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName04) //Starは星くずがキラキラする
            {
                for (j = 0; j < _MS_mariage.Count; j++) //おかしごとのその魔法との相性をみて、相性が入ってれば加点　なければ0点か減点。
                {
                    if (_MS_mariage[j] == GameMgr.System_MagicSlotName04)
                    {
                        _compatible = _MS_pointup[j] * _msvalue[i];

                        aisho_text1 = "見た目 + " + _compatible.ToString();
                    }
                }
                MS_aisho_database(_compatible);
                item_MS_aisho = "星屑: " + _ms_aisho + "　" + aisho_text1;

                _basemagicslot_on = 1; //加点がなくても、魔法はかかってるので、魔法のおかし扱いにはなる。
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName05) //WindArc　風の円弧が周りにとびちる
            {
                _basemagicslot_on = 1; //加点がなくても、魔法はかかってるので、魔法のおかし扱いにはなる。
            }
        }
    }

    void MS_aisho_database(int _compa)
    {
        _ms_aisho = "";

        if (_compa >= 0 && _compa < 5)
        {
            _ms_aisho = "-";
            aisho_text1 = "相性なし";
        }
        else if (_compa >= 5 && _compa < 10)
        {
            _ms_aisho = "△";
        }
        else if (_compa >= 10 && _compa < 30)
        {
            _ms_aisho = "〇";
        }
        else if (_compa >= 30)
        {
            _ms_aisho = "◎";
        }
    }
}
