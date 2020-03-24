using UnityEngine;
using System.Collections;


public class Touch_Controll2 : MonoBehaviour
{
    private SoundController sc;

    private bool touch_flag;

    private int draghair_count;

    private GameObject character;

    // Use this for initialization
    void Start()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        character = GameObject.FindWithTag("Character").gameObject;

        touch_flag = true;

        draghair_count = 0;
    }

    public void OnTouchFace()
    {
        if (touch_flag)
        {
            Debug.Log("Touch_Face");

            character.GetComponent<Character_Shop>().TouchCharacterFace();
            //sc.PlaySe(2);
        }
    }


    public void OnTouchHair()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Hair");

            //sc.PlaySe(0);
        }
    }

    public void DragTouchHair()
    {
        if (touch_flag)
        {
            //Debug.Log("Drag_Hair: " + draghair_count);

                draghair_count++;


        }
    }

    public void EndDragTouchHair()
    {
        if (touch_flag)
        {
            //Debug.Log("EndDrag_Hair");

            //girl1_status.Girl1_touchhair_start = false;
            draghair_count = 0;
        }
    }

    public void OnTouchRibbon()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Ribbon");

            //sc.PlaySe(0);
        }
    }

    public void OnTouchTwinTail()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_LongHair");

            //sc.PlaySe(0);
        }
    }

    public void OnTouchChest()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Chest");

            //sc.PlaySe(0);
        }
    }

    public void OnTouchBell()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Bell");

            sc.PlaySe(16);
        }
    }


    public void OnTouchWindow()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Window");

            //音を鳴らす。被り無し
            //audioSource.clip = sound1;
            //audioSource.PlayOneShot(sound1);
            sc.PlaySe(40);
        }
    }

    //タッチを一時的にオフ
    public void TouchOff()
    {
        touch_flag = false;
    }

    //タッチをオン
    public void TouchOn()
    {
        touch_flag = true;
    }
}