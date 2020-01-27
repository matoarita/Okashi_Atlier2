using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Updown_counter : MonoBehaviour {

    private GameObject canvas;

    private ItemShopDataBase shop_database;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject text_area;
    private Text _text;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject shopitemlistcontroller_obj;
    private ShopItemListController shopitemlistcontroller;

    private PlayerItemList pitemlist;

    private Text _count_text;
    public int updown_kosu;

    private int _zaiko_max;

    // Use this for initialization
    void Start () {

        this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnEnable()
    {
        //Debug.Log("Reset Updown Counter");

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //windowテキストエリアの取得
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        if (SceneManager.GetActiveScene().name == "Shop")
        {
            //ショップリスト画面を開く。初期設定で最初はOFF。
            shopitemlistcontroller_obj = GameObject.FindWithTag("ShopitemList_ScrollView");
            shopitemlistcontroller = shopitemlistcontroller_obj.GetComponent<ShopItemListController>();
        }
        else
        {
            pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
            pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();
        }
            
        updown_kosu = 1;
        _zaiko_max = 0;

        _count_text = transform.GetChild(0).gameObject.GetComponent<Text>();
        _count_text.text = updown_kosu.ToString();

    }

    public void OnClick_up()
    {
        if (SceneManager.GetActiveScene().name == "Compound")
        {
            /*Debug.Log("updown_kosu: " + updown_kosu);
            Debug.Log("分岐: " + pitemlistController.kettei1_bunki);
            Debug.Log("選択したアイテムID: " + pitemlistController.final_kettei_item1);
            Debug.Log("選択したアイテム所持数: " + pitemlist.playeritemlist[pitemlistController.final_kettei_item1]);
            Debug.Log("選択したリスト番号: " + pitemlistController.kettei_item1);*/

            compound_Main_obj = GameObject.FindWithTag("Compound_Main");
            compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

            if (compound_Main.compound_select == 2 || compound_Main.compound_select == 3)
            {
                switch (pitemlistController.kettei1_bunki)
                {
                    case 1:

                        switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item1]; //一個目の決定アイテムの所持数
                                break;

                            case 1:

                                _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        break;

                    case 2:

                        switch (pitemlistController._listitem[pitemlistController._count2].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item2]; //二個目の決定アイテムの所持数

                                break;

                            case 1:

                                _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item2].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        break;

                    case 3:

                        switch (pitemlistController._listitem[pitemlistController._count3].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item3]; //三個目の決定アイテムの所持数
                                break;

                            case 1:

                                _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item3].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        break;

                    case 10:

                        switch (pitemlistController._listitem[pitemlistController._base_count].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _zaiko_max = pitemlist.playeritemlist[pitemlistController.base_kettei_item]; //ベース決定アイテムの所持数
                                break;

                            case 1:

                                _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.base_kettei_item].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        break;

                    case 11:

                        switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item1]; //一個目の決定アイテムの所持数
                                break;

                            case 1:

                                _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        break;

                    case 12:

                        switch (pitemlistController._listitem[pitemlistController._count2].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item2]; //二個目の決定アイテムの所持数                           
                                break;

                            case 1:

                                _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item2].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        break;

                    case 13:

                        switch (pitemlistController._listitem[pitemlistController._count3].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item3]; //三個目の決定アイテムの所持数                           
                                break;

                            case 1:

                                _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item3].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        break;

                    default:
                        break;
                }
            }
            else if ( compound_Main.compound_select == 5)
            {
                switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                {
                    case 0:

                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item1]; //一個目の決定アイテムの所持数
                        break;

                    case 1:

                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                        break;

                    default:
                        break;
                }
            }

            ++updown_kosu;
            if (updown_kosu > _zaiko_max)
            {
                updown_kosu = _zaiko_max;
            }

            _count_text.text = updown_kosu.ToString();
        }

        else if (SceneManager.GetActiveScene().name == "Shop")
        { 
            _zaiko_max = shop_database.shopitems[shopitemlistcontroller.shop_kettei_ID].shop_itemzaiko;

            ++updown_kosu;
            if (updown_kosu > _zaiko_max)
            {
                updown_kosu = _zaiko_max;
            }

            if ( PlayerStatus.player_money < shop_database.shopitems[shopitemlistcontroller.shop_kettei_ID].shop_costprice * updown_kosu)
            {
                //お金が足りない
                _text.text = "お金が足りない。";

                updown_kosu--;
            }

            _count_text.text = updown_kosu.ToString();
        }

        else
        {
            switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
            {
                case 0:

                    _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item1]; //一個目の決定アイテムの所持数
                    break;

                case 1:

                    _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                    break;

                default:
                    break;
            }

            ++updown_kosu;
            if (updown_kosu > _zaiko_max)
            {
                updown_kosu = _zaiko_max;
            }
            

            _count_text.text = updown_kosu.ToString();
        }
    }

    public void OnClick_down()
    {
        --updown_kosu;
        if (updown_kosu <= 1)
        {
            updown_kosu = 1;
        }

        _count_text.text = updown_kosu.ToString();
    }
}
