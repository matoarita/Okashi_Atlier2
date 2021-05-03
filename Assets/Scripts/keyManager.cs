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
    private Exp_Controller exp_Controller;

    private SoundController sc;

    private ItemSelect_Cancel itemselect_cancel;
    private Updown_counter updown_counter;
    private Button[] updown_button = new Button[2];

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private ScrollRect pitemlist_sr;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;
    private ScrollRect recipilist_sr;

    private GameObject menu_toggle;
    private GameObject girleat_toggle;
    private GameObject shop_toggle;
    private GameObject getmaterial_toggle;
    private GameObject stageclear_toggle;
    private GameObject sleep_toggle;
    private GameObject system_toggle;

    private List<GameObject> toggle_list = new List<GameObject>();

    //private GameObject cardImage_obj;
    private GameObject moneystatus_onoff;
    private SetImage cardImage;

    private GameObject check_ItemDataBase_obj;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject canvas;

    private OptionPanel option_panel;

    private Debug_Panel debug_panel;

    public bool Cursor_On;
    public bool itemCursor_On;

    private GameObject compoundselect_onoff_obj;

    private GameObject cursor2;
    private Vector3 cursor_startpos;

    private int cursor_list_count; //画面中のリストの数　メインシーンなら全部で7個ある
    public int cursor_cullent_num; //現在、何番を選択しているか
    private int cursor_cullent_num_before; //さっきいたカーソルの位置も記憶。スクロール用に使う。
    private float cursor_vel;

    private int i, count;
    private int _temp;
    private int bheight, height;

    public bool EnterKey_ON;

    private GameObject compobgA;
    private List<GameObject> compobgA_selectitem = new List<GameObject>();
    private int compobgA_listcount;

    private int pitemlist_column = 3;
    private int recipilist_column = 2;

    private bool OnDownKey, OnUpKey, OnRightKey, OnLeftKey;

    private int debug_command_status;
    private int command_count;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        Cursor_On = false;
        itemCursor_On = false;

        EnterKey_ON = false;
        OnDownKey = false;
        OnUpKey = false;
        OnRightKey = false;
        OnLeftKey = false;

        debug_command_status = 0;
        command_count = 0;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                InitCompoundMainScene();
                break;
        }

        //debug_panel = GameObject.FindWithTag("Debug_Panel").GetComponent<Debug_Panel>();
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
        if (GameMgr.DEBUG_MODE)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) //１キーでMain デバッグ用
            {
                //SceneManager.LoadScene("Main");
                FadeManager.Instance.LoadScene("001_Title", 0.3f);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)) //左シフトキーでデバッグ入力受付のON/OFF
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
        }

        //デバッグを実機でも実行できるコマンド
        if (Input.GetKeyUp(KeyCode.F12))
        {
            debug_command_status = 0;
            command_count = 0;
        }

        if (Input.GetKey(KeyCode.F12)) //F12を押し続ける
        {
            switch (debug_command_status)
            {
                case 0: //左を2回

                    if (Input.GetKeyDown(KeyCode.LeftArrow)) //左キー
                    {
                        if(command_count == 0)
                        {
                            command_count++;
                        }
                        else if (command_count == 1) {

                            debug_command_status = 1;
                            command_count = 0;
                        }                      
                    }

                    break;

                case 1: //上を2回

                    if (Input.GetKeyDown(KeyCode.UpArrow)) //上キー
                    {
                        if (command_count == 0)
                        {
                            command_count++;
                        }
                        else if (command_count == 1)
                        {

                            debug_command_status = 2;
                            command_count = 0;
                        }
                    }
                    break;

                case 2: //右を2回

                    if (Input.GetKeyDown(KeyCode.RightArrow)) //右キー
                    {
                        if (command_count == 0)
                        {
                            command_count++;
                        }
                        else if (command_count == 1)
                        {

                            debug_command_status = 3;
                            command_count = 0;
                        }
                    }
                    break;

                case 3: //下を2回

                    if (Input.GetKeyDown(KeyCode.DownArrow)) //下キー
                    {
                        if (command_count == 0)
                        {
                            command_count++;
                        }
                        else if (command_count == 1)
                        {

                            debug_command_status = 4;
                            command_count = 0;
                            GameMgr.DEBUG_MODE = !GameMgr.DEBUG_MODE;
                        }
                    }
                    break;

                default:

                    break;
            }
        }
        
        // *** ここまで ***//

        //本編でも使用
        //KeyInputMethod();

        //F4キーでフルスクリーンの切り替え
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
            }
            else
            {
                Screen.fullScreen = true;
            }
        }
    }

    //現在は使用停止。シーンによってバグがあるため。
    void KeyInputMethod()
    {       
        if (canvas == null)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    InitCompoundMainScene();

                    break;

                case "Shop":

                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) //下を押した
        {
            OnDownKey = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) //上を押した
        {
            OnUpKey = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) //右を押した
        {
            OnRightKey = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) //左を押した
        {
            OnLeftKey = true;
        }


        if (!GameMgr.scenario_ON)
        {

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.C)) //右クリックでキャンセル
            {

                switch (SceneManager.GetActiveScene().name)
                {
                    case "Compound":

                        if (GameMgr.KeyInputOff_flag)
                        {
                            if (compound_Main.compound_select == 6 || compound_Main.compound_select == 200)
                            {
                                compound_Main.OnCancel_Select();
                                sc.PlaySe(18);
                            }
                            else if (compound_Main.compound_select == 205)
                            {
                                option_panel.BackOption();
                                sc.PlaySe(18);
                            }
                        }
                        break;

                    case "Shop":

                        if (GameMgr.KeyInputOff_flag)
                        {

                        }
                        break;


                    default:

                        break;
                }
            }


            if (OnDownKey || OnRightKey || OnUpKey || OnLeftKey) //下or右キー
            {

                switch (SceneManager.GetActiveScene().name)
                {
                    case "Compound":

                        pitemlistController = canvas.transform.Find("PlayeritemList_ScrollView").GetComponent<PlayerItemListController>();
                        pitemlist_sr = canvas.transform.Find("PlayeritemList_ScrollView").GetComponent<ScrollRect>();

                        recipilistController = canvas.transform.Find("RecipiList_ScrollView").GetComponent<RecipiListController>();
                        recipilist_sr = canvas.transform.Find("RecipiList_ScrollView").GetComponent<ScrollRect>();

                        if (GameMgr.KeyInputOff_flag)
                        {
                            switch (compound_Main.compound_select)
                            {

                                case 0:

                                    if (OnDownKey || OnRightKey) //下or右キー
                                    {
                                        if (!Cursor_On)
                                        {
                                            if (compound_Main.compound_status == 110)
                                            {
                                                sc.PlaySe(2);
                                                cursor_cullent_num = 0;
                                                compselectlist_AlloffCursor();
                                                cursor2.SetActive(true);
                                                Cursor_On = true;
                                            }

                                        }
                                        else
                                        {
                                            cursor_cullent_num++;
                                            sc.PlaySe(2);
                                            if (cursor_cullent_num > cursor_list_count)
                                            {
                                                cursor_cullent_num = 0;
                                            }

                                            if (cursor_cullent_num == 0) //0のときだけ、別矢印をONにする。
                                            {
                                                cursor2.SetActive(true);
                                                compselectlist_AlloffCursor();
                                            }
                                            else
                                            {
                                                cursor2.SetActive(false);
                                                compselectlist_DrawCursor();
                                            }
                                        }
                                    }

                                    if (OnUpKey || OnLeftKey) //上or左キー
                                    {
                                        if (!Cursor_On)
                                        {
                                            if (compound_Main.compound_status == 110)
                                            {
                                                sc.PlaySe(2);
                                                cursor_cullent_num = 0;
                                                compselectlist_AlloffCursor();
                                                cursor2.SetActive(true);
                                                Cursor_On = true;
                                            }

                                        }
                                        else
                                        {
                                            cursor_cullent_num--;
                                            sc.PlaySe(2);
                                            if (cursor_cullent_num < 0)
                                            {
                                                cursor_cullent_num = cursor_list_count;
                                            }

                                            if (cursor_cullent_num == 0) //0のときだけ、別矢印をONにする。
                                            {
                                                cursor2.SetActive(true);
                                                compselectlist_AlloffCursor();
                                            }
                                            else
                                            {
                                                cursor2.SetActive(false);
                                                compselectlist_DrawCursor();
                                            }
                                        }
                                    }
                                    break;

                                case 1:

                                    if (itemselect_cancel.kettei_on_waiting != true)
                                    {
                                        sc.PlaySe(2);

                                        if (!itemCursor_On)
                                        {
                                            cursor_cullent_num = 0;
                                            cursor_cullent_num_before = 0;
                                            itemCursor_On = true;
                                        }
                                        else
                                        {
                                            if (OnDownKey) //下
                                            {
                                                cursor_cullent_num += recipilist_column;
                                                if (cursor_cullent_num > recipilistController._recipi_listitem.Count - 1)
                                                {
                                                    cursor_cullent_num = recipilistController._recipi_listitem.Count - 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                            if (OnRightKey) //右
                                            {
                                                cursor_cullent_num += 1;
                                                if (cursor_cullent_num > recipilistController._recipi_listitem.Count - 1)
                                                {
                                                    cursor_cullent_num -= 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                            if (OnUpKey) //上
                                            {
                                                cursor_cullent_num -= recipilist_column;
                                                if (cursor_cullent_num < 0)
                                                {
                                                    cursor_cullent_num = 0; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                            if (OnLeftKey) //左
                                            {
                                                cursor_cullent_num -= 1;
                                                if (cursor_cullent_num < 0)
                                                {
                                                    cursor_cullent_num = recipilistController._recipi_listitem.Count - 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                        }

                                        //描画更新
                                        recipiitemlist_DrawCursor();
                                    }
                                    else
                                    {
                                        updown_counter = canvas.transform.Find("updown_counter(Clone)").GetComponent<Updown_counter>();
                                        updown_button = updown_counter.GetComponentsInChildren<Button>();
                                        sc.PlaySe(2);

                                        if (OnRightKey) //右
                                        {
                                            if (updown_button[1].interactable == true)
                                            {
                                                updown_counter.OnClick_up();
                                            }
                                        }
                                        if (OnLeftKey) //左
                                        {
                                            updown_counter.OnClick_down();
                                        }
                                    }                                   

                                    break;

                                case 2:

                                    if (itemselect_cancel.kettei_on_waiting != true)
                                    {
                                        sc.PlaySe(2);

                                        if (!itemCursor_On)
                                        {
                                            cursor_cullent_num = 0;
                                            cursor_cullent_num_before = 0;
                                            itemCursor_On = true;
                                        }
                                        else
                                        {
                                            if (OnDownKey) //下
                                            {
                                                cursor_cullent_num += pitemlist_column;
                                                if (cursor_cullent_num > pitemlistController._listitem.Count - 1)
                                                {
                                                    cursor_cullent_num = pitemlistController._listitem.Count - 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                }


                                            }
                                            if (OnRightKey) //右
                                            {
                                                cursor_cullent_num += 1;
                                                if (cursor_cullent_num > pitemlistController._listitem.Count - 1)
                                                {
                                                    cursor_cullent_num -= 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                            if (OnUpKey) //上
                                            {
                                                cursor_cullent_num -= pitemlist_column;
                                                if (cursor_cullent_num < 0)
                                                {
                                                    cursor_cullent_num = 0; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                            if (OnLeftKey) //左
                                            {
                                                cursor_cullent_num -= 1;
                                                if (cursor_cullent_num < 0)
                                                {
                                                    cursor_cullent_num = pitemlistController._listitem.Count - 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                        }

                                        //描画更新
                                        pitemlist_DrawCursor();
                                    }
                                    else
                                    {
                                        //トッピングは、今のところ個数計算はしないので、OFFに。
                                        /*updown_counter = canvas.transform.Find("updown_counter(Clone)").GetComponent<Updown_counter>();
                                        sc.PlaySe(2);

                                        if (Input.GetKeyDown(KeyCode.RightArrow)) //右
                                        {
                                            updown_counter.OnClick_up();
                                        }
                                        if (Input.GetKeyDown(KeyCode.LeftArrow)) //左
                                        {
                                            updown_counter.OnClick_down();
                                        }*/
                                    }
                                   

                                    break;


                                case 3:

                                    if (itemselect_cancel.kettei_on_waiting != true)
                                    {
                                        sc.PlaySe(2);

                                        if (!itemCursor_On)
                                        {
                                            cursor_cullent_num = 0;
                                            cursor_cullent_num_before = 0;
                                            itemCursor_On = true;
                                        }
                                        else
                                        {
                                            if (OnDownKey) //下
                                            {
                                                cursor_cullent_num += pitemlist_column;
                                                if (cursor_cullent_num > pitemlistController._listitem.Count - 1)
                                                {
                                                    cursor_cullent_num = pitemlistController._listitem.Count - 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                }


                                            }
                                            if (OnRightKey) //右
                                            {
                                                cursor_cullent_num += 1;
                                                if (cursor_cullent_num > pitemlistController._listitem.Count - 1)
                                                {
                                                    cursor_cullent_num -= 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                            if (OnUpKey) //上
                                            {
                                                cursor_cullent_num -= pitemlist_column;
                                                if (cursor_cullent_num < 0)
                                                {
                                                    cursor_cullent_num = 0; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                            if (OnLeftKey) //左
                                            {
                                                cursor_cullent_num -= 1;
                                                if (cursor_cullent_num < 0)
                                                {
                                                    cursor_cullent_num = pitemlistController._listitem.Count - 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                }
                                            }
                                        }

                                        //描画更新
                                        pitemlist_DrawCursor();
                                    }
                                    else
                                    {
                                        updown_counter = canvas.transform.Find("updown_counter(Clone)").GetComponent<Updown_counter>();
                                        sc.PlaySe(2);

                                        if (OnRightKey) //右
                                        {
                                            updown_counter.OnClick_up();
                                        }
                                        if (OnLeftKey) //左
                                        {
                                            updown_counter.OnClick_down();
                                        }
                                    }


                                    break;

                                case 6:

                                    if (!Cursor_On)
                                    {
                                        sc.PlaySe(2);
                                        cursor_cullent_num = 0;
                                        Cursor_On = true;
                                    }
                                    else
                                    {
                                        sc.PlaySe(2);
                                        if (OnDownKey) //下キー
                                        {
                                            cursor_cullent_num += 2;
                                            if (cursor_cullent_num > compobgA_listcount - 1)
                                            {
                                                cursor_cullent_num = compobgA_listcount - 1;
                                            }
                                        }
                                        if (OnRightKey) //右キー
                                        {
                                            cursor_cullent_num += 1;
                                            if (cursor_cullent_num > compobgA_listcount - 1)
                                            {
                                                cursor_cullent_num -= 1;
                                            }
                                        }
                                        if (OnUpKey) //上キー
                                        {
                                            cursor_cullent_num -= 2;
                                            if (cursor_cullent_num < 0)
                                            {
                                                cursor_cullent_num = 0;
                                            }
                                        }
                                        if (OnLeftKey) //左キー
                                        {
                                            cursor_cullent_num -= 1;
                                            if (cursor_cullent_num < 0)
                                            {
                                                cursor_cullent_num = 0;
                                            }
                                        }
                                    }

                                    //描画更新
                                    composelect_DrawCursor();
                                    break;
                            }

                        }
                        break;

                }

                OnDownKey = false;
                OnUpKey = false;
                OnRightKey = false;
                OnLeftKey = false;

            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) //エンターキー
            {

                switch (SceneManager.GetActiveScene().name)
                {
                    case "Compound":

                        pitemlistController = canvas.transform.Find("PlayeritemList_ScrollView").GetComponent<PlayerItemListController>();

                        if (GameMgr.KeyInputOff_flag)
                        {
                            switch (compound_Main.compound_select)
                            {

                                case 0:

                                    if (Cursor_On)
                                    {

                                        if (compound_Main.compound_status == 110)
                                        {
                                            switch (cursor_cullent_num)
                                            {
                                                case 0:

                                                    sc.PlaySe(0);
                                                    extreme_panel.OnClick_ExtremeButton();

                                                    break;

                                                default:

                                                    sc.PlaySe(23);
                                                    switch (toggle_list[cursor_cullent_num - 1].transform.name)
                                                    {
                                                        case "GetMaterial_Toggle":
                                                            getmaterial_toggle.GetComponent<Toggle>().isOn = true;
                                                            break;

                                                        case "Shop_Toggle":
                                                            shop_toggle.GetComponent<Toggle>().isOn = true;
                                                            break;

                                                        case "ItemMenu_Toggle":
                                                            menu_toggle.GetComponent<Toggle>().isOn = true;
                                                            break;

                                                        case "Sleep_Toggle":
                                                            sleep_toggle.GetComponent<Toggle>().isOn = true;
                                                            break;

                                                        case "GirlEat_Toggle":
                                                            girleat_toggle.GetComponent<Toggle>().isOn = true;
                                                            break;

                                                        case "System_Toggle":
                                                            system_toggle.GetComponent<Toggle>().isOn = true;
                                                            break;
                                                    }

                                                    break;

                                            }
                                        }
                                    }

                                    break;

                                case 1:



                                        if (itemselect_cancel.kettei_on_waiting != true)
                                        {
                                            if (recipilistController._recipi_listitem[cursor_cullent_num].GetComponent<Toggle>().IsInteractable() != false)
                                            {
                                                recipilistController._recipi_listitem[cursor_cullent_num].GetComponent<Toggle>().isOn = true;
                                                sc.PlaySe(46);
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else
                                        {

                                        }
                                    

                                    break;

                                case 2:


                                        if (itemselect_cancel.kettei_on_waiting != true)
                                        {
                                            if (pitemlistController._listitem[cursor_cullent_num].GetComponent<Toggle>().IsInteractable() != false)
                                            {
                                                pitemlistController._listitem[cursor_cullent_num].GetComponent<Toggle>().isOn = true;
                                                sc.PlaySe(46);
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else
                                        {

                                        }
                                    
                                    break;

                                case 3:


                                        if (itemselect_cancel.kettei_on_waiting != true)
                                        {
                                            if (pitemlistController._listitem[cursor_cullent_num].GetComponent<Toggle>().IsInteractable() != false)
                                            {
                                                pitemlistController._listitem[cursor_cullent_num].GetComponent<Toggle>().isOn = true;
                                                sc.PlaySe(46);
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else
                                        {

                                        }
                                    
                                    break;

                                case 6:

                                    switch (cursor_cullent_num)
                                    {
                                        case 0:

                                            compound_Main.OnCheck_3_button();
                                            break;

                                        case 1:

                                            compound_Main.OnCheck_1_button();
                                            break;

                                        case 2:

                                            compound_Main.OnCheck_2_button();
                                            break;
                                    }
                                    break;

                            }
                        }

                        cursor2.SetActive(false);
                        Cursor_On = false;
                        break;

                }

            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(scene.name + " scene loaded");
        
    }

    void compselectlist_AlloffCursor()
    {
        for (i = 0; i < toggle_list.Count; i++) //一回全てのカーソルをOff
        {
            toggle_list[i].transform.Find("SelectCursor").gameObject.SetActive(false);
        }
    }

    void compselectlist_DrawCursor()
    {
        compselectlist_AlloffCursor();
        toggle_list[cursor_cullent_num-1].transform.Find("SelectCursor").gameObject.SetActive(true);
    }

    void pitemlist_DrawCursor()
    {
        for (i = 0; i < pitemlistController._listitem.Count; i++) //一回全てのカーソルをOff
        {
            pitemlistController._listitem[i].transform.Find("SelectCursor").gameObject.SetActive(false);
        }
        pitemlistController._listitem[cursor_cullent_num].transform.Find("SelectCursor").gameObject.SetActive(true);

        bheight = 0; //前カーソルがいた位置の列番号
        _temp = cursor_cullent_num_before;
        while (_temp >= pitemlist_column)
        {
            _temp = _temp - pitemlist_column;
            bheight++;
        }

        height = 0; //現在カーソルの列番号
        _temp = cursor_cullent_num;
        while (_temp >= pitemlist_column)
        {
            _temp = _temp - pitemlist_column;
            height++;
        }

        if (pitemlistController._listitem.Count != 0)
        {
            _temp = (pitemlistController._listitem.Count - 9) / 3;
            if(_temp <= 0)
            {
                _temp = 1;
            }
            cursor_vel = 1.0f / _temp;
        }
        else
        {
            cursor_vel = 0f;
        }

            
        if (height > bheight && height >= 3) //カーソルが下にいった場合、スクロールも下に動かす。
        {
            //pitemlist_sr.velocity = new Vector2(0f, 300f);
            pitemlist_sr.verticalNormalizedPosition = 1.0f - cursor_vel * (height - 2);
        }
        else if (height < bheight && height >= 0 ) //カーソルが上にいった場合
        {
            //pitemlist_sr.velocity = new Vector2(0f, -300f);
            pitemlist_sr.verticalNormalizedPosition = 1.0f - cursor_vel * (height - 2);
        }

        if (pitemlist_sr.verticalNormalizedPosition >= 1.0f)
        {
            pitemlist_sr.verticalNormalizedPosition = 1.0f;
        }
        else if (pitemlist_sr.verticalNormalizedPosition < 0.0f)
        {
            pitemlist_sr.verticalNormalizedPosition = 0.0f;
        }

        //最後に前いた位置を更新
        cursor_cullent_num_before = cursor_cullent_num;
    }

    void recipiitemlist_DrawCursor()
    {
        for (i = 0; i < recipilistController._recipi_listitem.Count; i++) //一回全てのカーソルをOff
        {
            recipilistController._recipi_listitem[i].transform.Find("SelectCursor").gameObject.SetActive(false);
        }
        recipilistController._recipi_listitem[cursor_cullent_num].transform.Find("SelectCursor").gameObject.SetActive(true);

        bheight = 0; //前カーソルがいた位置の列番号
        _temp = cursor_cullent_num_before;
        while (_temp >= recipilist_column)
        {
            _temp = _temp - recipilist_column;
            bheight++;
        }

        height = 0; //現在カーソルの列番号
        _temp = cursor_cullent_num;
        while (_temp >= recipilist_column)
        {
            _temp = _temp - recipilist_column;
            height++;
        }

        if (recipilistController._recipi_listitem.Count != 0)
        {
            _temp = (recipilistController._recipi_listitem.Count - 12) / 3;
            if (_temp <= 0)
            {
                _temp = 1;
            }
            cursor_vel = 1.0f / _temp;
        }
        else
        {
            cursor_vel = 0f;
        }


        if (height > bheight && height >= 3) //カーソルが下にいった場合、スクロールも下に動かす。
        {
            //pitemlist_sr.velocity = new Vector2(0f, 300f);
            recipilist_sr.verticalNormalizedPosition = 1.0f - cursor_vel * (height - 2);
        }
        else if (height < bheight && height >= 0) //カーソルが上にいった場合
        {
            //pitemlist_sr.velocity = new Vector2(0f, -300f);
            recipilist_sr.verticalNormalizedPosition = 1.0f - cursor_vel * (height - 2);
        }

        if (recipilist_sr.verticalNormalizedPosition >= 1.0f)
        {
            recipilist_sr.verticalNormalizedPosition = 1.0f;
        }
        else if (recipilist_sr.verticalNormalizedPosition < 0.0f)
        {
            recipilist_sr.verticalNormalizedPosition = 0.0f;
        }

        //最後に前いた位置を更新
        cursor_cullent_num_before = cursor_cullent_num;
    }

    void composelect_DrawCursor()
    {
        for (i = 0; i < compobgA_selectitem.Count; i++) //一回全てのカーソルをOff
        {
            compobgA_selectitem[i].transform.Find("SelectCursor").gameObject.SetActive(false);
        }
        compobgA_selectitem[cursor_cullent_num].transform.Find("SelectCursor").gameObject.SetActive(true);
    }

    public void SelectOff()
    {
        Cursor_On = false;
        itemCursor_On = false;

        for (i = 0; i < compobgA_selectitem.Count; i++) //一回全てのカーソルをOff
        {
            compobgA_selectitem[i].transform.Find("SelectCursor").gameObject.SetActive(false);
        }
    }



    public void InitCompoundMainScene()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //オプションパネル読み込み
        option_panel = canvas.transform.Find("OptionPanel").GetComponent<OptionPanel>();

        cursor2 = canvas.transform.Find("MainUIPanel/ExtremePanel/Comp/SelectCursor2").gameObject;        

        Cursor_On = false;
        itemCursor_On = false;

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        
        compobgA = canvas.transform.Find("Compound_BGPanel_A").gameObject;
        compobgA_selectitem.Clear();

        compobgA_listcount = 0;
        count = 0;
        foreach (Transform child in compobgA.transform.Find("SelectPanel_1/Scroll View/Viewport/Content").transform)
        {
            compobgA_selectitem.Add(child.gameObject);
            if(compobgA_selectitem[count].GetComponent<Button>().interactable == true)
            {
                compobgA_listcount++;
            }
            count++;
        }
        //Debug.Log("compobgA_listcount: " + compobgA_listcount);


        itemselect_cancel = GameObject.FindWithTag("ItemSelect_Cancel").GetComponent<ItemSelect_Cancel>();

        extreme_panel = canvas.transform.Find("MainUIPanel/ExtremePanel").GetComponent<ExtremePanel>();

        compoundselect_onoff_obj = canvas.transform.Find("MainUIPanel/CompoundSelect_ScrollView").gameObject;
        menu_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/ItemMenu_Toggle").gameObject;
        girleat_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GirlEat_Toggle").gameObject;
        shop_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Shop_Toggle").gameObject;
        getmaterial_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GetMaterial_Toggle").gameObject;
        stageclear_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/StageClear_Toggle").gameObject;
        sleep_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Sleep_Toggle").gameObject;
        system_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/System_Toggle").gameObject;

        toggle_list.Clear();
        foreach(Transform child in canvas.transform.Find("MainUIPanel/CompoundSelect_ScrollView/Viewport/Content_compound").transform)
        {
            if(child.gameObject.activeSelf)
            {
                toggle_list.Add(child.gameObject);
            }
        }

        cursor_list_count = toggle_list.Count;
        cursor_cullent_num = 0;
        cursor_cullent_num_before = 0;

        cursor2.SetActive(false);
        compselectlist_AlloffCursor();
    }
}
