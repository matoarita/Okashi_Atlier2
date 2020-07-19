using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//主に、ウィンドウやその他のゲームオブジェクトのアクティブ／非アクティブを切り替え。
//オブジェクト自体にスクリプトをつけると、非アクティブにしたときに、入力も効かなくなってしまうため、スクリプトを分けている。

    //今は未使用。

public class keyManager : SingletonMonoBehaviour<keyManager>
{
    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private ExtremePanel extreme_panel;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject menu_toggle;
    private GameObject girleat_toggle;
    private GameObject shop_toggle;
    private GameObject getmaterial_toggle;
    private GameObject stageclear_toggle;
    private GameObject sleep_toggle;

    //private GameObject cardImage_obj;
    private GameObject moneystatus_onoff;
    private SetImage cardImage;

    private GameObject check_ItemDataBase_obj;
    private Check_ItemDataBase check_IDB;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject canvas;

    private Debug_Panel debug_panel;

    private bool Cursor_On;

    private GameObject compoundselect_onoff_obj;

    private GameObject cursor;
    private GameObject cursor2;
    private Vector3 cursor_startpos;

    private int cursor_list_count; //画面中のリストの数　メインシーンなら全部で6個ある
    private int cursor_cullent_num; //現在、何番を選択しているか

    // Use this for initialization
    void Start () {

        Cursor_On = false;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //キャンバスの読み込み
                canvas = GameObject.FindWithTag("Canvas");

                cursor = canvas.transform.Find("CompoundSelect_ScrollView/Viewport/SelectCursor").gameObject;
                cursor2 = canvas.transform.Find("ExtremePanel/SelectCursor2").gameObject;
                cursor_startpos = cursor.transform.localPosition;

                cursor_list_count = 6;
                cursor_cullent_num = 0;
                break;
        }

        //debug_panel = GameObject.FindWithTag("Debug_Panel").GetComponent<Debug_Panel>();
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド
    }
	
	// Update is called once per frame
	void Update ()
    {       
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.Alpha1)) //１キーでMain デバッグ用
        {
            //SceneManager.LoadScene("Main");
            FadeManager.Instance.LoadScene("000_Prologue", 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.Space)) //Spaceキーでデバッグ入力受付のON/OFF
        {
            debug_panel = GameObject.FindWithTag("Debug_Panel").GetComponent<Debug_Panel>();

            //SceneManager.LoadScene("Main");
            if (debug_panel.Debug_INPUT_ON)
            {
                debug_panel.Debug_INPUT_ON = false;
            }
            else
            {
                debug_panel.Debug_INPUT_ON = true;
            }
        }

        // *** ここまで ***//

        //本編でも使用
        /*
        if (Input.GetKeyDown(KeyCode.DownArrow)) //下キー
        {
            
            switch(SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    //キャンバスの読み込み
                    canvas = GameObject.FindWithTag("Canvas");

                    cursor = canvas.transform.Find("CompoundSelect_ScrollView/Viewport/SelectCursor").gameObject;
                    cursor2 = canvas.transform.Find("ExtremePanel/SelectCursor2").gameObject;

                    compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                    compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                    if (!Cursor_On)
                    {
                        if (compound_Main.compound_status == 110)
                        {
                            cursor.transform.localPosition = cursor_startpos;
                            cursor_cullent_num = 0;
                            cursor.SetActive(true);
                            Cursor_On = true;
                        }                      

                    }
                    else
                    {
                        cursor_cullent_num++;
                        if (cursor_cullent_num > cursor_list_count - 1)
                        {
                            cursor_cullent_num = 0;
                        }

                        if (cursor_cullent_num == cursor_list_count - 1) //５のときだけ、別矢印をONにする。
                        {
                            cursor2.SetActive(true);
                            cursor.SetActive(false);
                        }
                        else
                        {                           
                            cursor.transform.localPosition = new Vector3(cursor_startpos.x, cursor_startpos.y + (-30 * cursor_cullent_num), 0);
                            cursor.SetActive(true);
                            cursor2.SetActive(false);
                        }
                    }
                   
                    break;
            }

        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) //上キー
        {

            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    //キャンバスの読み込み
                    canvas = GameObject.FindWithTag("Canvas");

                    cursor = canvas.transform.Find("CompoundSelect_ScrollView/Viewport/SelectCursor").gameObject;
                    cursor2 = canvas.transform.Find("ExtremePanel/SelectCursor2").gameObject;

                    compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                    compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                    if (!Cursor_On)
                    {
                        if (compound_Main.compound_status == 110)
                        {
                            cursor.transform.localPosition = cursor_startpos;
                            cursor_cullent_num = 5;
                            cursor2.SetActive(true);
                            Cursor_On = true;
                        }

                    }
                    else
                    {
                        cursor_cullent_num--;
                        if (cursor_cullent_num < 0)
                        {
                            cursor_cullent_num = 5;
                        }

                        if (cursor_cullent_num == cursor_list_count - 1) //５のときだけ、別矢印をONにする。
                        {
                            cursor2.SetActive(true);
                            cursor.SetActive(false);
                        }
                        else
                        {
                            cursor.transform.localPosition = new Vector3(cursor_startpos.x, cursor_startpos.y + (-30 * cursor_cullent_num), 0);
                            cursor.SetActive(true);
                            cursor2.SetActive(false);
                        }
                    }

                    break;
            }

        }

        if (Input.GetKeyDown(KeyCode.Return)) //エンターキー
        {

            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    //キャンバスの読み込み
                    canvas = GameObject.FindWithTag("Canvas");

                    compoundselect_onoff_obj = canvas.transform.Find("CompoundSelect_ScrollView").gameObject;
                    cursor = canvas.transform.Find("CompoundSelect_ScrollView/Viewport/SelectCursor").gameObject;
                    cursor2 = canvas.transform.Find("ExtremePanel/SelectCursor2").gameObject;

                    compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                    compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                    extreme_panel = canvas.transform.Find("ExtremePanel").GetComponent<ExtremePanel>();

                    menu_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/ItemMenu_Toggle").gameObject;
                    girleat_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GirlEat_Toggle").gameObject;
                    shop_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Shop_Toggle").gameObject;
                    getmaterial_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GetMaterial_Toggle").gameObject;
                    stageclear_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/StageClear_Toggle").gameObject;
                    sleep_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Sleep_Toggle").gameObject;

                    if (Cursor_On)
                    {
                        if (compound_Main.compound_status == 110)
                        {
                            switch (cursor_cullent_num)
                            {
                                case 0:
                                    getmaterial_toggle.GetComponent<Toggle>().isOn = true;
                                    break;

                                case 1:
                                    shop_toggle.GetComponent<Toggle>().isOn = true;
                                    break;

                                case 2:
                                    menu_toggle.GetComponent<Toggle>().isOn = true;
                                    break;

                                case 3:
                                    girleat_toggle.GetComponent<Toggle>().isOn = true;
                                    break;

                                case 4:
                                    sleep_toggle.GetComponent<Toggle>().isOn = true;
                                    break;

                                case 5:
                                    extreme_panel.OnClick_ExtremeButton();
                                    break;
                            }
                        }
                    }
                    
                    break;
            }

            cursor.SetActive(false);
            cursor2.SetActive(false);
            Cursor_On = false;
        }
        */
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(scene.name + " scene loaded");
        
    }

}
