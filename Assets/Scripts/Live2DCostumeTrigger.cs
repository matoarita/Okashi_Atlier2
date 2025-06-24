using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

public class Live2DCostumeTrigger : MonoBehaviour {

    private PlayerItemList pitemlist;

    private Animator live2d_animator;
    private int trans_costume;
    private int trans_acce;

    private int i;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitSetting()
    {
        live2d_animator = this.GetComponent<Animator>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();
    }

    private void OnEnable()
    {
        //InitSetting();

        ChangeCostume();
        ChangeAcce();
    }

    public void ChangeCostume()
    {
        InitSetting();

        //01 黒エプロン　02 スク水　03 白い服　04 赤い服 05 ラベンダー 06 ピンクうさぎ 07 パティシエ
        trans_costume = GameMgr.Costume_Num;
        live2d_animator.SetInteger("trans_costume", trans_costume);
    }

    public void ChangeAcce()
    {
        InitSetting();

        for (i = 0; i < pitemlist.emeralditemlist.Count; i++)
        {
            if (pitemlist.emeralditemlist[i].ev_itemType == 2 && pitemlist.emeralditemlist[i].ev_ListOn == 1)
            {

                switch (pitemlist.emeralditemlist[i].event_itemName)
                {

                    case "Glass_Acce": //メガネ

                        if (pitemlist.emeralditemlist[i].ev_costumeEquip == 0) //OFF
                        {
                            trans_acce = 0;
                            live2d_animator.SetInteger("trans_acce01", trans_acce);
                            //Debug.Log("trans_acce OFF: " + trans_acce);
                        }
                        else //ON
                        {
                            trans_acce = 1;
                            live2d_animator.SetInteger("trans_acce01", trans_acce);
                            //Debug.Log("trans_acce ON: " + trans_acce);
                        }
                        break;

                    case "BalloonHat_Acce": //バルーンハット

                        if (pitemlist.emeralditemlist[i].ev_costumeEquip == 0) //OFF
                        {
                            trans_acce = 0;
                            live2d_animator.SetInteger("trans_acce02", trans_acce);
                            //Debug.Log("trans_acce OFF: " + trans_acce);
                        }
                        else //ON
                        {
                            trans_acce = 1;
                            live2d_animator.SetInteger("trans_acce02", trans_acce);
                            //Debug.Log("trans_acce ON: " + trans_acce);
                        }
                        break;

                    case "AngelWing_Acce": //天使のはね

                        if (pitemlist.emeralditemlist[i].ev_costumeEquip == 0) //OFF
                        {
                            trans_acce = 0;
                            live2d_animator.SetInteger("trans_acce03", trans_acce);
                            //Debug.Log("trans_acce OFF: " + trans_acce);
                        }
                        else //ON
                        {
                            trans_acce = 1;
                            live2d_animator.SetInteger("trans_acce03", trans_acce);
                            //Debug.Log("trans_acce ON: " + trans_acce);
                        }
                        break;

                    case "Nekomimi_Acce": //ねこみみ

                        if (pitemlist.emeralditemlist[i].ev_costumeEquip == 0) //OFF
                        {
                            trans_acce = 0;
                            live2d_animator.SetInteger("trans_acce04", trans_acce);
                            //Debug.Log("trans_acce OFF: " + trans_acce);
                        }
                        else //ON
                        {
                            trans_acce = 1;
                            live2d_animator.SetInteger("trans_acce04", trans_acce);
                            //Debug.Log("trans_acce ON: " + trans_acce);
                        }
                        break;

                    case "FlowerHairpin_Acce": //お花のヘアピン

                        if (pitemlist.emeralditemlist[i].ev_costumeEquip == 0) //OFF
                        {
                            trans_acce = 0;
                            live2d_animator.SetInteger("trans_acce05", trans_acce);
                            //Debug.Log("trans_acce OFF: " + trans_acce);
                        }
                        else //ON
                        {
                            trans_acce = 1;
                            live2d_animator.SetInteger("trans_acce05", trans_acce);
                            //Debug.Log("trans_acce ON: " + trans_acce);
                        }
                        break;

                    case "TwincleStarDust_Acce": //ティンクルスターダスト

                        if (pitemlist.emeralditemlist[i].ev_costumeEquip == 0) //OFF
                        {
                            trans_acce = 0;
                            live2d_animator.SetInteger("trans_acce06", trans_acce);
                            //Debug.Log("trans_acce OFF: " + trans_acce);
                        }
                        else //ON
                        {
                            trans_acce = 1;
                            live2d_animator.SetInteger("trans_acce06", trans_acce);
                            //Debug.Log("trans_acce ON: " + trans_acce);
                        }
                        break;

                    case "DongriPochet_Acce": //どんぐりポシェット

                        if (pitemlist.emeralditemlist[i].ev_costumeEquip == 0) //OFF
                        {
                            trans_acce = 0;
                            live2d_animator.SetInteger("trans_acce07", trans_acce);
                            //Debug.Log("trans_acce OFF: " + trans_acce);
                        }
                        else //ON
                        {
                            trans_acce = 1;
                            live2d_animator.SetInteger("trans_acce07", trans_acce);
                            //Debug.Log("trans_acce ON: " + trans_acce);
                        }
                        break;

                    case "PatissierHat_Acce": //パティシエハット

                        if (pitemlist.emeralditemlist[i].ev_costumeEquip == 0) //OFF
                        {
                            trans_acce = 0;
                            live2d_animator.SetInteger("trans_acce08", trans_acce);
                            //Debug.Log("trans_acce OFF: " + trans_acce);
                        }
                        else //ON
                        {
                            trans_acce = 1;
                            live2d_animator.SetInteger("trans_acce08", trans_acce);
                            //Debug.Log("trans_acce ON: " + trans_acce);
                        }
                        break;

                    default:

                        break;
                }
            }
        }
    }
}
