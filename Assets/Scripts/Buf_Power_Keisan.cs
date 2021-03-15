﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buf_Power_Keisan : SingletonMonoBehaviour<Buf_Power_Keisan>
{
    private PlayerItemList pitemlist;

    private int _buf_findpower;

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int Buf_findpower_Keisan()
    {
        _buf_findpower = 0;

        if (pitemlist.KosuCount("aquamarine_pendant") >= 1)
        {
            _buf_findpower += 10;
        }

        return _buf_findpower;
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
    }
}