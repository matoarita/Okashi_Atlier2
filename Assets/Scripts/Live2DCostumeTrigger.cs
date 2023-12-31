﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

public class Live2DCostumeTrigger : MonoBehaviour {

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
    }

    private void OnEnable()
    {
        InitSetting();

        ChangeCostume();
        ChangeAcce();
    }

    public void ChangeCostume()
    {
        //01 黒エプロン　02 スク水　03 白い服　04 赤い服
        trans_costume = GameMgr.Costume_Num;
        live2d_animator.SetInteger("trans_costume", trans_costume);
    }

    public void ChangeAcce()
    {
        for (i = 0; i < GameMgr.Accesory_Num.Length; i++)
        {
            switch (i)
            {

                case 0: //メガネ

                    if (GameMgr.Accesory_Num[i] == 0) //OFF
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

                case 1: //バルーンハット

                    if (GameMgr.Accesory_Num[i] == 0) //OFF
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

                case 2: //天使のはね

                    if (GameMgr.Accesory_Num[i] == 0) //OFF
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

                case 3: //ねこみみ

                    if (GameMgr.Accesory_Num[i] == 0) //OFF
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

                case 4: //お花のヘアピン

                    if (GameMgr.Accesory_Num[i] == 0) //OFF
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

                case 5: //ティンクルスターダスト

                    if (GameMgr.Accesory_Num[i] == 0) //OFF
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

                default:

                    break;
            }
        }
    }
}
