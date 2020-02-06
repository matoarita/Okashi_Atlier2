using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkashiParamKeisanMethod : MonoBehaviour {

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private PlayerItemList pitemlist;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private SlotNameDataBase slotnamedatabase;

    private int kettei_item1;
    private int itemType;

    private int _basecost;
    private string[] _basetp = new string[10];
    private string[] _koyutp = new string[3];

    private int i, count;

    private int slot_money, GetMoney;

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //アイテムID　＋　アイテムタイプ（店売りかオリジナル）を入力すると、
    //そのアイテムについている、ベースパラメータやスロットの値を計算し、お金や好感度などを出力してくれる。計算用オブジェクト


    public int Sell_Okashi( int _itemID, int _itemtype )
    {
        kettei_item1 = _itemID;
        itemType = _itemtype;

        //初期化
        Resetparam();

        //_itemIDとitemTypeをもとに、パラメータをセット
        SetParam();

        //スロットの計算。該当するスロット効果があれば、それを得点にする。
        for (i = 0; i < _basetp.Length; i++)
        {
            count = 0;
            while (count < slotnamedatabase.slotname_lists.Count)
            {
                if (_basetp[i] == slotnamedatabase.slotname_lists[count].slotName)
                {
                    slot_money += slotnamedatabase.slotname_lists[count].slot_Money;
                    break;
                }
                count++;
            }
        }

        //お金の取得
        GetMoney = _basecost + slot_money;

        return GetMoney;
    }

    void Resetparam()
    {
        //初期化
        slot_money = 0;

        for (i = 0; i < _basetp.Length; i++)
        {
            _basetp[i] = "";
        }
    }

    void SetParam()
    {
        //アイテムパラメータの取得

        switch (itemType)
        {
            case 0:

                /*
                _basenameHyouji = database.items[kettei_item1].itemNameHyouji;
                _basequality = database.items[kettei_item1].Quality;
                _basesweat = database.items[kettei_item1].Sweat;
                _basebitter = database.items[kettei_item1].Bitter;
                _basesour = database.items[kettei_item1].Sour;
                _basecrispy = database.items[kettei_item1].Crispy;
                _basefluffy = database.items[kettei_item1].Fluffy;
                _basesmooth = database.items[kettei_item1].Smooth;
                _basehardness = database.items[kettei_item1].Hardness;
                _basejiggly = database.items[kettei_item1].Jiggly;
                _basechewy = database.items[kettei_item1].Chewy;
                _basepowdery = database.items[kettei_item1].Powdery;
                _baseoily = database.items[kettei_item1].Oily;
                _basewatery = database.items[kettei_item1].Watery;
                _basegirl1_like = database.items[kettei_item1].girl1_itemLike;
                _baseitemtype = database.items[kettei_item1].itemType.ToString();
                _baseitemtype_sub = database.items[kettei_item1].itemType_sub.ToString();*/
                _basecost = database.items[kettei_item1].cost_price;

                for (i = 0; i < database.items[kettei_item1].toppingtype.Length; i++)
                {
                    _basetp[i] = database.items[kettei_item1].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[kettei_item1].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = database.items[kettei_item1].koyu_toppingtype[i].ToString();
                }

                break;

            case 1:

                /*
                _basenameHyouji = pitemlist.player_originalitemlist[kettei_item1].itemNameHyouji;
                _basequality = pitemlist.player_originalitemlist[kettei_item1].Quality;
                _basesweat = pitemlist.player_originalitemlist[kettei_item1].Sweat;
                _basebitter = pitemlist.player_originalitemlist[kettei_item1].Bitter;
                _basesour = pitemlist.player_originalitemlist[kettei_item1].Sour;
                _basecrispy = pitemlist.player_originalitemlist[kettei_item1].Crispy;
                _basefluffy = pitemlist.player_originalitemlist[kettei_item1].Fluffy;
                _basesmooth = pitemlist.player_originalitemlist[kettei_item1].Smooth;
                _basehardness = pitemlist.player_originalitemlist[kettei_item1].Hardness;
                _basejiggly = pitemlist.player_originalitemlist[kettei_item1].Jiggly;
                _basechewy = pitemlist.player_originalitemlist[kettei_item1].Chewy;
                _basepowdery = pitemlist.player_originalitemlist[kettei_item1].Powdery;
                _baseoily = pitemlist.player_originalitemlist[kettei_item1].Oily;
                _basewatery = pitemlist.player_originalitemlist[kettei_item1].Watery;
                _basegirl1_like = pitemlist.player_originalitemlist[kettei_item1].girl1_itemLike;
                _baseitemtype = pitemlist.player_originalitemlist[kettei_item1].itemType.ToString();
                _baseitemtype_sub = pitemlist.player_originalitemlist[kettei_item1].itemType_sub.ToString();*/
                _basecost = pitemlist.player_originalitemlist[kettei_item1].cost_price;

                for (i = 0; i < database.items[kettei_item1].toppingtype.Length; i++)
                {
                    _basetp[i] = pitemlist.player_originalitemlist[kettei_item1].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[kettei_item1].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = pitemlist.player_originalitemlist[kettei_item1].koyu_toppingtype[i].ToString();
                }

                break;

            default:
                break;
        }
    }

}
