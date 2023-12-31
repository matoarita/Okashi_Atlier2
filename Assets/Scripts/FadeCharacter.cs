﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCharacter : MonoBehaviour {

    private bool fade_flag;
    private int fade_sw;

    private float alfa;    //A値を操作するための変数
    private float red, green, blue;    //RGBを操作するための変数
    private float _speed;   //透明化の速さ

    // Use this for initialization
    void Start()
    {

        red = 1;//this.gameObject.GetComponent<SpriteRenderer>().color.r;
        green = 1; // this.gameObject.GetComponent<SpriteRenderer>().color.g;
        blue = 1; // this.gameObject.GetComponent<SpriteRenderer>().color.b;
        alfa = this.gameObject.GetComponent<SpriteRenderer>().color.a;

        fade_flag = false;
        _speed = 0.1f;

        if (alfa > 0.5)
        {
            fade_sw = 0;
        }
        else
        {
            fade_sw = 1;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (fade_flag == true) //アニメ開始
        {
            switch (fade_sw)
            {
                case 0: //255 -> 0

                    alfa -= _speed;
                    this.GetComponent<SpriteRenderer>().color = new Color(red, green, blue, alfa);

                    if (alfa < 0.0)
                    {
                        fade_flag = false;
                    }
                    break;

                case 1: //0 -> 255

                    alfa += _speed;
                    this.GetComponent<SpriteRenderer>().color = new Color(red, green, blue, alfa);

                    if (alfa > 1.0)
                    {
                        fade_flag = false;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public void FadeImageOn() //0 -> 255
    {

        fade_flag = true;

        fade_sw = 1;
    }

    public void FadeImageOff() //255 -> 0
    {

        fade_flag = true;

        fade_sw = 0;
    }

    public void SetOff() //画像を透明にし、なくす。
    {
        this.GetComponent<SpriteRenderer>().color = new Color(red, green, blue, 0);
    }

    public void SetOn() //画像を不透明度=255にし、表示。
    {
        this.GetComponent<SpriteRenderer>().color = new Color(red, green, blue, 1);
    }

}
