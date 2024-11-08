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

    private Sprite hikari_faceicon_01_normal;
    private Sprite hikari_faceicon_02_joukigen;
    private Sprite hikari_faceicon_03_bad;
    private Sprite hikari_faceicon_04_littlefine;
    private Sprite hikari_faceicon_05_fine;
    private Sprite hikari_faceicon_07_yorokobi;
    private Sprite hikari_faceicon_11_mazui2;
    private Sprite hikari_faceicon_12_iya;
    private Sprite hikari_faceicon_13_surprise;
    private Sprite hikari_faceicon_17_mazui3;
    private Sprite hikari_faceicon_18_surprise2;
    private Sprite hikari_faceicon_32_joukigen2;
    private Sprite hikari_faceicon_35_metoji;
    private Sprite hikari_faceicon_38_nakinagarauttae_bou;
    private Sprite hikari_faceicon_43_Bad2;

    private Image window_FaceImg;

    private Text chara_name;

    private bool StartRead = false;

    // Use this for initialization
    void Start () {

        //SetInit();

    }

    void SetInit()
    {
        if (!StartRead)
        {
            //顔アイコン画像登録
            hikari_faceicon_01_normal = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_01_Normal");
            hikari_faceicon_02_joukigen = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_02_Joukigen");
            hikari_faceicon_03_bad = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_03_Bad");
            hikari_faceicon_04_littlefine = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_04_Little_fine");
            hikari_faceicon_05_fine = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_05_Fine");
            hikari_faceicon_07_yorokobi = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_07_Yorokobi");
            hikari_faceicon_11_mazui2 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_11_Mazui2");
            hikari_faceicon_12_iya = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_12_Iya");
            hikari_faceicon_13_surprise = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_13_Surprise");
            hikari_faceicon_17_mazui3 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_17_Mazui3");
            hikari_faceicon_18_surprise2 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_18_Surprise2");
            hikari_faceicon_32_joukigen2 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_32_Joukigen2");
            hikari_faceicon_35_metoji = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_35_Metoji");
            hikari_faceicon_38_nakinagarauttae_bou = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_38_nakinagarauttae_bou");
            hikari_faceicon_43_Bad2 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Icon/" + "Icon_face_43_Bad2");

            StartRead = true;
        }

        if (this.gameObject.name == "MessageWindow")
        {
            FaceIconPanel = this.transform.Find("FaceIconPanel").gameObject;
            CharaNamePanel = this.transform.Find("CharaName").gameObject;
            chara_name = CharaNamePanel.GetComponent<Text>();
            window_FaceImg = FaceIconPanel.transform.Find("MaskImg/FaceImg").GetComponent<Image>();
        }

        if (this.gameObject.name == "MessageWindowMain")
        {
            SpQuestNamePanel = this.transform.Find("SpQuestNamePanel").gameObject;
        }
    }

    private void OnEnable()
    {
        DrawIcon();
    }

    // Update is called once per frame
    void Update () {

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
    public void Setting_WindowIcon(int _facenum)
    {
        SetInit();
        SetFaceIcon(_facenum);
    }

    void SetFaceIcon(int _num)
    {
        switch (_num)
        {
            case 1:

                window_FaceImg.sprite = hikari_faceicon_01_normal;
                break;

            case 2:

                window_FaceImg.sprite = hikari_faceicon_02_joukigen;
                break;

            case 3:

                window_FaceImg.sprite = hikari_faceicon_03_bad;
                break;

            case 4:

                window_FaceImg.sprite = hikari_faceicon_04_littlefine;
                break;

            case 5:

                window_FaceImg.sprite = hikari_faceicon_05_fine;
                break;

            case 7:

                window_FaceImg.sprite = hikari_faceicon_07_yorokobi;
                break;

            case 11:

                window_FaceImg.sprite = hikari_faceicon_11_mazui2;
                break;

            case 12:

                window_FaceImg.sprite = hikari_faceicon_12_iya;
                break;

            case 13:

                window_FaceImg.sprite = hikari_faceicon_13_surprise;
                break;

            case 17:

                window_FaceImg.sprite = hikari_faceicon_17_mazui3;
                break;

            case 18:

                window_FaceImg.sprite = hikari_faceicon_18_surprise2;
                break;

            case 32:

                window_FaceImg.sprite = hikari_faceicon_32_joukigen2;
                break;

            case 35:

                window_FaceImg.sprite = hikari_faceicon_35_metoji;
                break;                

            case 38:

                window_FaceImg.sprite = hikari_faceicon_38_nakinagarauttae_bou;
                break;

            case 43:

                window_FaceImg.sprite = hikari_faceicon_43_Bad2;
                break;

        }
    }
}
