using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MessageWindow : MonoBehaviour {

    public bool window_clickon;

    private GameObject FaceIconPanel;
    private GameObject CharaNamePanel;

    private Text chara_name;

    // Use this for initialization
    void Start () {

        if (this.gameObject.name == "MessageWindow")
        {
            FaceIconPanel = this.transform.Find("FaceIconPanel").gameObject;
            CharaNamePanel = this.transform.Find("CharaName").gameObject;
            chara_name = CharaNamePanel.GetComponent<Text>();

            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    FaceIconON();
                    break;

                case "Or_Compound":

                    FaceIconON();
                    break;

                case "Hikari_CompMain":

                    FaceIconON();
                    break;

                default:

                    CharaNameON();
                    break;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (this.gameObject.name == "MessageWindow")
        {
            GameMgr.Window_FaceIcon_OnOff = false;

            //採取地選択画面のとき　顔グラをON
            if (GameMgr.compound_select == 20)
            {
                GameMgr.Window_FaceIcon_OnOff = true;
            }           

            if (GameMgr.Window_FaceIcon_OnOff)
            {
                FaceIconON();
            }
            else
            {
                CharaNameON();
            }
        }
	}

    void FaceIconON()
    {
        FaceIconPanel.SetActive(true);
        CharaNamePanel.SetActive(false);
    }

    void CharaNameON()
    {
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
