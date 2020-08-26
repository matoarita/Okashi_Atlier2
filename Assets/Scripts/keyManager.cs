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

    private CompoundStartButton compostart_button;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private ScrollRect pitemlist_sr;

    private GameObject menu_toggle;
    private GameObject girleat_toggle;
    private GameObject shop_toggle;
    private GameObject getmaterial_toggle;
    private GameObject stageclear_toggle;
    private GameObject sleep_toggle;
    private GameObject system_toggle;

    //private GameObject cardImage_obj;
    private GameObject moneystatus_onoff;
    private SetImage cardImage;

    private GameObject check_ItemDataBase_obj;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject canvas;

    private Debug_Panel debug_panel;

    public bool Cursor_On;
    public bool itemCursor_On;

    private GameObject compoundselect_onoff_obj;

    private GameObject cursor;
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

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        Cursor_On = false;
        itemCursor_On = false;

        EnterKey_ON = false;

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
	void Update ()
    {       
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.Alpha1)) //１キーでMain デバッグ用
        {
            //SceneManager.LoadScene("Main");
            FadeManager.Instance.LoadScene("001_Title", 0.3f);
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

        if (canvas == null)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    InitCompoundMainScene();
                    
                    break;
            }
        }

        // *** ここまで ***//

        //本編でも使用

        
        if (!GameMgr.scenario_ON)
        {

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftControl)) //右クリック
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
                        }
                        break;
                }
            }


            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) //下or右キー
            {

                switch (SceneManager.GetActiveScene().name)
                {
                    case "Compound":

                        pitemlistController = canvas.transform.Find("PlayeritemList_ScrollView").GetComponent<PlayerItemListController>();
                        pitemlist_sr = canvas.transform.Find("PlayeritemList_ScrollView").GetComponent<ScrollRect>();

                        if (GameMgr.KeyInputOff_flag)
                        {
                            switch (compound_Main.compound_select)
                            {

                                case 0:

                                    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow)) //下or右キー
                                    {
                                        if (!Cursor_On)
                                        {
                                            if (compound_Main.compound_status == 110)
                                            {
                                                sc.PlaySe(2);
                                                cursor.transform.localPosition = cursor_startpos;
                                                cursor_cullent_num = 0;
                                                cursor2.SetActive(true);
                                                Cursor_On = true;
                                            }

                                        }
                                        else
                                        {
                                            cursor_cullent_num++;
                                            sc.PlaySe(2);
                                            if (cursor_cullent_num > cursor_list_count - 1)
                                            {
                                                cursor_cullent_num = 0;
                                            }

                                            if (cursor_cullent_num == 0) //0のときだけ、別矢印をONにする。
                                            {
                                                cursor2.SetActive(true);
                                                cursor.SetActive(false);
                                            }
                                            else
                                            {
                                                cursor.transform.localPosition = new Vector3(cursor_startpos.x + (110 * (cursor_cullent_num - 1)), cursor_startpos.y, 0);
                                                cursor.SetActive(true);
                                                cursor2.SetActive(false);
                                            }
                                        }
                                    }

                                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) //上or左キー
                                    {
                                        if (!Cursor_On)
                                        {
                                            if (compound_Main.compound_status == 110)
                                            {
                                                sc.PlaySe(2);
                                                cursor.transform.localPosition = cursor_startpos;
                                                cursor_cullent_num = 0;
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
                                                cursor_cullent_num = 6;
                                            }

                                            if (cursor_cullent_num == 0) //0のときだけ、別矢印をONにする。
                                            {
                                                cursor2.SetActive(true);
                                                cursor.SetActive(false);
                                            }
                                            else
                                            {
                                                cursor.transform.localPosition = new Vector3(cursor_startpos.x + (110 * (cursor_cullent_num - 1)), cursor_startpos.y, 0);
                                                cursor.SetActive(true);
                                                cursor2.SetActive(false);
                                            }
                                        }
                                    }
                                    break;

                                case 2:

                                    if (compostart_button.compofinal_flag != true)
                                    {
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
                                                if (Input.GetKeyDown(KeyCode.DownArrow)) //下
                                                {
                                                    cursor_cullent_num += pitemlist_column;
                                                    if (cursor_cullent_num > pitemlistController._listitem.Count - 1)
                                                    {
                                                        cursor_cullent_num = pitemlistController._listitem.Count - 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                    }


                                                }
                                                if (Input.GetKeyDown(KeyCode.RightArrow)) //右
                                                {
                                                    cursor_cullent_num += 1;
                                                    if (cursor_cullent_num > pitemlistController._listitem.Count - 1)
                                                    {
                                                        cursor_cullent_num -= 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                    }
                                                }
                                                if (Input.GetKeyDown(KeyCode.UpArrow)) //上
                                                {
                                                    cursor_cullent_num -= pitemlist_column;
                                                    if (cursor_cullent_num < 0)
                                                    {
                                                        cursor_cullent_num = 0; //空の箇所を押そうとした場合、位置を元に戻す
                                                    }
                                                }
                                                if (Input.GetKeyDown(KeyCode.LeftArrow)) //左
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
                                    }
                                    else
                                    {
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

                                    if (compostart_button.compofinal_flag != true)
                                    {
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
                                                if (Input.GetKeyDown(KeyCode.DownArrow)) //下
                                                {
                                                    cursor_cullent_num += pitemlist_column;
                                                    if (cursor_cullent_num > pitemlistController._listitem.Count - 1)
                                                    {
                                                        cursor_cullent_num = pitemlistController._listitem.Count - 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                    }


                                                }
                                                if (Input.GetKeyDown(KeyCode.RightArrow)) //右
                                                {
                                                    cursor_cullent_num += 1;
                                                    if (cursor_cullent_num > pitemlistController._listitem.Count - 1)
                                                    {
                                                        cursor_cullent_num -= 1; //空の箇所を押そうとした場合、位置を元に戻す
                                                    }
                                                }
                                                if (Input.GetKeyDown(KeyCode.UpArrow)) //上
                                                {
                                                    cursor_cullent_num -= pitemlist_column;
                                                    if (cursor_cullent_num < 0)
                                                    {
                                                        cursor_cullent_num = 0; //空の箇所を押そうとした場合、位置を元に戻す
                                                    }
                                                }
                                                if (Input.GetKeyDown(KeyCode.LeftArrow)) //左
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

                                            if (Input.GetKeyDown(KeyCode.RightArrow)) //右
                                            {
                                                updown_counter.OnClick_up();
                                            }
                                            if (Input.GetKeyDown(KeyCode.LeftArrow)) //左
                                            {
                                                updown_counter.OnClick_down();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        updown_counter = canvas.transform.Find("updown_counter(Clone)").GetComponent<Updown_counter>();
                                        sc.PlaySe(2);

                                        if (Input.GetKeyDown(KeyCode.RightArrow)) //右
                                        {
                                            updown_counter.OnClick_up();
                                        }
                                        if (Input.GetKeyDown(KeyCode.LeftArrow)) //左
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
                                        if (Input.GetKeyDown(KeyCode.DownArrow)) //下キー
                                        {
                                            cursor_cullent_num += 2;
                                            if (cursor_cullent_num > compobgA_listcount - 1)
                                            {
                                                cursor_cullent_num = compobgA_listcount - 1;
                                            }
                                        }
                                        if (Input.GetKeyDown(KeyCode.RightArrow)) //右キー
                                        {
                                            cursor_cullent_num += 1;
                                            if (cursor_cullent_num > compobgA_listcount - 1)
                                            {
                                                cursor_cullent_num -= 1;
                                            }
                                        }
                                        if (Input.GetKeyDown(KeyCode.UpArrow)) //上キー
                                        {
                                            cursor_cullent_num -= 2;
                                            if (cursor_cullent_num < 0)
                                            {
                                                cursor_cullent_num = 0;
                                            }
                                        }
                                        if (Input.GetKeyDown(KeyCode.LeftArrow)) //左キー
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

            }

            if (Input.GetKeyDown(KeyCode.Return) ) //エンターキー
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

                                                    extreme_panel.OnClick_ExtremeButton();

                                                    break;

                                                case 1:

                                                    getmaterial_toggle.GetComponent<Toggle>().isOn = true;
                                                    break;

                                                case 2:
                                                    shop_toggle.GetComponent<Toggle>().isOn = true;

                                                    break;

                                                case 3:
                                                    menu_toggle.GetComponent<Toggle>().isOn = true;
                                                    break;

                                                case 4:
                                                    sleep_toggle.GetComponent<Toggle>().isOn = true;
                                                    break;

                                                case 5:
                                                    girleat_toggle.GetComponent<Toggle>().isOn = true;

                                                    break;

                                                case 6:
                                                    system_toggle.GetComponent<Toggle>().isOn = true;
                                                    break;
                                            }
                                        }
                                    }

                                    break;

                                case 2:

                                    if (compostart_button.compofinal_flag != true)
                                    {
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
                                    }
                                    break;

                                case 3:

                                    if (compostart_button.compofinal_flag != true)
                                    {
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

                        cursor.SetActive(false);
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

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        cursor = canvas.transform.Find("CompoundSelect_ScrollView/Viewport/SelectCursor").gameObject;
        cursor2 = canvas.transform.Find("ExtremePanel/Comp/SelectCursor2").gameObject;
        cursor_startpos = cursor.transform.localPosition;

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

        extreme_panel = canvas.transform.Find("ExtremePanel").GetComponent<ExtremePanel>();

        //最終調合ボタンの取得 
        compostart_button = canvas.transform.Find("Compound_BGPanel_A/CompoundStartButton").GetComponent<CompoundStartButton>();

        compoundselect_onoff_obj = canvas.transform.Find("CompoundSelect_ScrollView").gameObject;
        menu_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/ItemMenu_Toggle").gameObject;
        girleat_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GirlEat_Toggle").gameObject;
        shop_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Shop_Toggle").gameObject;
        getmaterial_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GetMaterial_Toggle").gameObject;
        stageclear_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/StageClear_Toggle").gameObject;
        sleep_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Sleep_Toggle").gameObject;
        system_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/System_Toggle").gameObject;

        cursor_list_count = 7;
        cursor_cullent_num = 0;
        cursor_cullent_num_before = 0;
    }
}
