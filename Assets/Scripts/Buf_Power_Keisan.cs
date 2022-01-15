using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buf_Power_Keisan : SingletonMonoBehaviour<Buf_Power_Keisan>
{
    private PlayerItemList pitemlist;

    private int _buf_findpower;
    private int _buf_kakuritsuup;
    private float _buf_kakuritsuup_f;
    private int _buf_shokukanup;

    private int i;

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();
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

                    case "Cookie_Mat":

                        OvenBuf();
                        break;

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
        //条件分岐
        if (pitemlist.KosuCount("itembox_1") >= 1) //アイテムかごLv2
        {
            if (PlayerStatus.player_zairyobox_lv < 2) //LV3とか買った後で買っても、持てる量は更新されない。
            {
                PlayerStatus.player_zairyobox = 15;
                PlayerStatus.player_zairyobox_lv = 2;
            }
        }
        if (pitemlist.KosuCount("itembox_2") >= 1) //アイテムかごLv3
        {
            if (PlayerStatus.player_zairyobox_lv < 3)
            {
                PlayerStatus.player_zairyobox = 30;
                PlayerStatus.player_zairyobox_lv = 3;
            }
        }
        if (pitemlist.KosuCount("itembox_3") >= 1) //アイテムかごLv4
        {
            if (PlayerStatus.player_zairyobox_lv < 4)
            {
                PlayerStatus.player_zairyobox = 50;
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
        }
    }
}
