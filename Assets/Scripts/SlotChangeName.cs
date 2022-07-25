using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SlotChangeName : SingletonMonoBehaviour<SlotChangeName>
{
    private int itemID;              
    private int itemType;

    public string[] _slotHyouji; //日本語に変換後の表記を格納する。フルネーム用

    private PlayerItemList pitemlist;
    private SlotNameDataBase slotnamedatabase;
    private ItemDataBase database;

    private string[] _slot;
    private List<string> slotInfo = new List<string>();
    private List<string> slotInfo_Hyouji = new List<string>();
    private List<int> slotScore = new List<int>();
    private List<string> slot_HyoujiList = new List<string>();

    private int i, count;
    private string color;

    // Use this for initialization
    void Start()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        _slot = new string[database.items[0].toppingtype.Length];
        _slotHyouji = new string[database.items[0].toppingtype.Length];
    }

    //指定したIDの、プレイヤーオリジナルアイテムの、正式名称を表示する。 
    public void slotChangeName( int _itemtype, int _itemID, string _name_color, int _status)
    {
        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();

        itemID = _itemID;
        itemType = _itemtype;
        color = _name_color;

        if (itemType == 0)
        {
            for (i = 0; i < _slot.Length; i++)
            {
                _slot[i] = "";
            }
        }
        else
        {
            for (i = 0; i < _slot.Length; i++)
            {
                if (_status == 0)
                {
                    _slot[i] = pitemlist.player_originalitemlist[itemID].toppingtype[i].ToString();
                }
                else if (_status == 1)
                {
                    _slot[i] = pitemlist.player_yosokuitemlist[itemID].toppingtype[i].ToString();
                }
                else if(_status == 2)
                {
                    _slot[i] = pitemlist.player_extremepanel_itemlist[itemID].toppingtype[i].ToString();
                }
            }
        }

        slot_Name_change();
    }

    void slot_Name_change()
    {
        slot_HyoujiList.Clear();

        for (i = 0; i < _slotHyouji.Length; i++)
        {
            _slotHyouji[i] = "";
        }

        //トッピング、ダブっているものを検出

        //まず、スコアを計算
        count = 0;

        while (count < _slot.Length)
        {
            for (i = 0; i < slotInfo.Count; i++)
            {
                if (_slot[count] == slotInfo[i])
                {
                    slotScore[i]++;
                }
            }
            count++;
        }

        //頭からみていって、1以上のものを表示用リストに追加していく。Nonは省く。
        for (i = 0; i < slotScore.Count; i++)
        {
            if (i != 0) //先頭は、Nonが入っているので除外
            {
                if (slotScore[i] > 0)
                {
                    switch (slotScore[i])
                    {
                        case 1:
                            slot_HyoujiList.Add(slotInfo_Hyouji[i]);
                            break;

                        case 2:
                            slot_HyoujiList.Add("ダブル" + slotInfo_Hyouji[i]);
                            break;

                        case 3:
                            slot_HyoujiList.Add("トリプル" + slotInfo_Hyouji[i]);
                            break;

                        case 4:
                            slot_HyoujiList.Add("かなり" + slotInfo_Hyouji[i]);
                            break;

                        case 5:
                            slot_HyoujiList.Add("とっても" + slotInfo_Hyouji[i]);
                            break;

                        case 6:
                            slot_HyoujiList.Add("濃縮" + slotInfo_Hyouji[i]);
                            break;

                        case 7:
                            slot_HyoujiList.Add("スーパー濃縮" + slotInfo_Hyouji[i]);
                            break;

                        case 8:
                            slot_HyoujiList.Add("ハイパー濃縮" + slotInfo_Hyouji[i]);
                            break;

                        case 9:
                            slot_HyoujiList.Add("デラックス濃縮" + slotInfo_Hyouji[i]);
                            break;

                        case 10:
                            slot_HyoujiList.Add("アルティメット濃縮" + slotInfo_Hyouji[i]);
                            break;

                        default:
                            slot_HyoujiList.Add("∞濃縮" + slotInfo_Hyouji[i]);
                            break;
                    }
                }
            }
        }

        for (i = 0; i < slot_HyoujiList.Count; i++)
        {
            _slotHyouji[i] = slot_HyoujiList[i];
        }       

    }

    void InitializeItemSlotDicts()
    {
        slotInfo.Clear();
        slotInfo_Hyouji.Clear();
        slotScore.Clear();


        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            slotInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            slotInfo_Hyouji.Add(slotnamedatabase.slotname_lists[i].slot_Hyouki_2);
            slotScore.Add(0);

        }
    }
}