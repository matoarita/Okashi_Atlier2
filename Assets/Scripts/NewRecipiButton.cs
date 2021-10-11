using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRecipiButton : MonoBehaviour {

    private GameObject canvas;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject CompleteImage;

    private GameObject extremePanel_obj;
    private ExtremePanel extremePanel;

    private Exp_Controller exp_Controller;

    private SoundController sc;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //エクストリームパネルオブジェクトの取得
        extremePanel_obj = canvas.transform.Find("MainUIPanel/ExtremePanel").gameObject;
        extremePanel = extremePanel_obj.GetComponent<ExtremePanel>();

        //完成時パネルの取得
        CompleteImage = canvas.transform.Find("Compound_BGPanel_A/CompletePanel").gameObject; //調合成功時のイメージパネル
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        sc.PlaySe(2);

        if (GameMgr.tutorial_ON == true)
        {
            if (GameMgr.tutorial_Num == 75)
            {
                GameMgr.tutorial_Progress = true;
                GameMgr.tutorial_Num = 80;
            }
            if (GameMgr.tutorial_Num == 265)
            {
                GameMgr.tutorial_Progress = true;
                GameMgr.tutorial_Num = 270;
            }
        }

        switch (compound_Main.compound_select)
        {
            case 1: //レシピ調合

                if (extremePanel.extreme_itemID != 9999) //生地系などのアイテムの場合は、利便性のため、すぐに調合画面に戻る。現状は、新しいお菓子がセットされてない場合。
                {
                    compound_Main.compound_status = 0;
                }
                else
                {
                    compound_Main.compound_status = 1; // もう一回、オリジナル調合の画面に戻る。
                }
                break;

            case 3: //オリジナル調合

                if (extremePanel.extreme_itemID != 9999) //生地系などのアイテムの場合は、利便性のため、すぐに調合画面に戻る。
                {
                    compound_Main.compound_status = 0;
                }
                else
                {
                    compound_Main.compound_status = 3; // もう一回、オリジナル調合の画面に戻る。W
                }
                break;

            default:

                compound_Main.compound_status = 0;
                break;

        }

        compound_Main.compo_ON = false;

        CompleteImage.SetActive(false);

        exp_Controller.EffectListClear();
        extremePanel.LifeAnimeOnTrue();

        card_view.DeleteCard_DrawView();
        Destroy(this.gameObject);
        
    }
}
