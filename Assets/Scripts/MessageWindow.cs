using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MessageWindow : MonoBehaviour {

    public bool window_clickon;

    private GameObject SpQuestNamePanel;
    private GameObject FaceIconPanel;
    private GameObject CharaNamePanel;

    private Text chara_name;

    private bool StartRead;

    // Use this for initialization
    void Start () {

        //SetInit();

        /*if (this.gameObject.name == "MessageWindow")
        {
            switch (SceneManager.GetActiveScene().name)
            {*/
                //Compoundのように、キャラ名と顔グラが混在して表示するシーンでは、Startは設定しないほうがいい。一瞬顔グラがパチっとうつる現象発生。
                /*case "Compound":

                    FaceIconON();
                    break;

                case "Or_Compound":

                    FaceIconON();
                    break;

                case "Hikari_CompMain":

                    FaceIconON();
                    break;*/
        /*
                case "GetMaterial":

                    FaceIconON();
                    GameMgr.Window_FaceIcon_OnOff = true;
                    break;

                default:

                    CharaNameON();
                    break;
            }
        }*/
    }

    void SetInit()
    {
        FaceIconPanel = this.transform.Find("FaceIconPanel").gameObject;
        CharaNamePanel = this.transform.Find("CharaName").gameObject;
        chara_name = CharaNamePanel.GetComponent<Text>();

        if (this.gameObject.name == "MessageWindowMain")
        {
            SpQuestNamePanel = this.transform.Find("SpQuestNamePanel").gameObject;
        }        

        StartRead = false;
    }

    private void OnEnable()
    {
        DrawIcon();
    }

    // Update is called once per frame
    void Update () {

        /*if (!StartRead)
        {
            DrawIcon();
            StartRead = true;
        }*/
    }

    public void DrawIcon()
    {
        SetInit();

        if (this.gameObject.name == "MessageWindow")
        {
            GameMgr.Window_FaceIcon_OnOff = false;

            switch(GameMgr.Scene_Category_Num)
            {
                case 10:

                    //採取地選択画面のとき　顔グラをON
                    if (GameMgr.compound_select == 20)
                    {
                        if (!GameMgr.outgirl_Nowprogress)
                        {
                            GameMgr.Window_FaceIcon_OnOff = true;
                        }
                        else
                        {
                            GameMgr.Window_FaceIcon_OnOff = false;
                        }
                    }
                    break;
            }
            
            switch (SceneManager.GetActiveScene().name)
            {
                case "GetMaterial":

                    if (!GameMgr.outgirl_Nowprogress)
                    {
                        GameMgr.Window_FaceIcon_OnOff = true;
                    }
                    else
                    {
                        GameMgr.Window_FaceIcon_OnOff = false;
                    }
                    break;
            }

            //Debug.Log("GameMgr.Window_FaceIcon_OnOff: " + GameMgr.Window_FaceIcon_OnOff);
            if (GameMgr.Window_FaceIcon_OnOff)
            {
                FaceIconON();
            }
            else
            {
                CharaNameON();
            }
        }

        if (this.gameObject.name == "MessageWindowMain")
        {
            if (!GameMgr.SPquestPanelOff)
            {
                SpQuestNamePanel.SetActive(true);
            }
            else //trueのときは、パネル表示をオフにする。
            {
                SpQuestNamePanel.SetActive(false);
            }
        }
    }

    //Compound_Mainからも読む
    void FaceIconON()
    {
        SetInit();

        FaceIconPanel.SetActive(true);
        CharaNamePanel.SetActive(false);
    }

    public void CharaNameON()
    {
        SetInit();

        FaceIconPanel.SetActive(false);
        CharaNamePanel.SetActive(true);
        chara_name.text = GameMgr.Window_CharaName;
    }

    public void WindowClickDown()
    {
        window_clickon = true;
    }

    public void WindowClickUp()
    {
        window_clickon = false;
    }

    public void Window_Exit()
    {
        window_clickon = false;
    }

    public void KaigyoButton()
    {
        GameMgr.Kaigyo_ON = true;

    }
}
