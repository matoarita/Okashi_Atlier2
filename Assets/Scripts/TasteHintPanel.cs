using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TasteHintPanel : MonoBehaviour {

    private Compound_Main compound_Main;

    private ItemDataBase database;

    private Text Okashi_lasthint_text;
    private Text Okashi_lastname_text;
    private Text Okashi_lastscore_text;
    private Text Okashi_lastshokukan_param_text;
    private Text Okashi_lastshokukan_mes_text;
    private Text Okashi_lastsweat_param_text;
    private Text Okashi_lastsour_param_text;
    private Text Okashi_lastbitter_param_text;
    private Sprite Okashi_Img;
    private Image Okashi_Icon;
    private GameObject HikariIcon_Angry;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        SetInit();
    }

    private void SetInit()
    {
        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        Okashi_lasthint_text = this.transform.Find("HintPanel/HintText").GetComponent<Text>();
        Okashi_lasthint_text.text = GameMgr.Okashi_lasthint;

        Okashi_lastname_text = this.transform.Find("HintPanel/OkashiName").GetComponent<Text>();
        Okashi_lastname_text.text = GameMgr.Okashi_lastname;

        Okashi_lastscore_text = this.transform.Find("HintPanel/OkashiScore").GetComponent<Text>();
        Okashi_lastscore_text.text = GameMgr.Okashi_totalscore.ToString();

        Okashi_lastshokukan_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelA/PanelA_Param/Text").GetComponent<Text>();
        Okashi_lastshokukan_param_text.text = GameMgr.Okashi_lastshokukan_param.ToString();

        Okashi_lastshokukan_mes_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelA/PanelA_Title/Text").GetComponent<Text>();
        Okashi_lastshokukan_mes_text.text = GameMgr.Okashi_lastshokukan_mes;

        Okashi_lastsweat_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelB/PanelB_Param/Text").GetComponent<Text>();
        Okashi_lastsweat_param_text.text = GameMgr.Okashi_lastsweat_param.ToString();

        Okashi_lastsour_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelC/PanelC_Param/Text").GetComponent<Text>();
        Okashi_lastsour_param_text.text = GameMgr.Okashi_lastsour_param.ToString();

        Okashi_lastbitter_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelD/PanelD_Param/Text").GetComponent<Text>();
        Okashi_lastbitter_param_text.text = GameMgr.Okashi_lastbitter_param.ToString();

        Okashi_Img = database.items[GameMgr.Okashi_lastID].itemIcon_sprite;
        Okashi_Icon = this.transform.Find("HintPanel/OkashiImage").GetComponent<Image>(); //画像アイコン
        Okashi_Icon.sprite = Okashi_Img;

        HikariIcon_Angry = this.transform.Find("HintPanel/CharaIcon/HikariIcon2").gameObject;
        if(GameMgr.Okashi_totalscore <= 30)
        {
            HikariIcon_Angry.SetActive(true);
        }
        else
        {
            HikariIcon_Angry.SetActive(false);
        }
    }

    public void BackOption()
    {

        compound_Main.compound_status = 0;
        this.gameObject.SetActive(false);

    }
}
