using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRecipiButton : MonoBehaviour {

    private GameObject canvas;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject CompleteImage;

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

        //完成時パネルの取得
        CompleteImage = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/CompletePanel").gameObject; //調合成功時のイメージパネル
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

        //別画面で（たとえばピクニック中など）戻るときは、status=0にならず、また調合画面に戻る。
        if (GameMgr.picnic_event_reading_now)
        {
            GameMgr.compound_status = 6; // 調合の画面に戻る。
        }
        else
        {
            switch (GameMgr.compound_select)
            {
                case 1: //レシピ調合

                    if (GameMgr.OkashiMake_PanelSetType != 0) 
                    {
                        GameMgr.compound_status = 0;
                    }
                    else //生地系などのアイテムの場合は、利便性のため、すぐに調合画面に戻る。
                    {
                        GameMgr.compound_status = 1; // もう一回、オリジナル調合の画面に戻る。
                    }
                    break;

                case 3: //オリジナル調合

                    if (GameMgr.OkashiMake_PanelSetType != 0) 
                    {
                        GameMgr.compound_status = 0;
                    }
                    else
                    {
                        GameMgr.compound_status = 3; //生地系などのアイテムの場合は、利便性のため、すぐに調合画面に戻る。
                    }
                    break;

                default:

                    GameMgr.compound_status = 0;
                    break;

            }

            GameMgr.CompoundSceneStartON = false;　//調合シーン終了　新しいレシピひらめいたときは、現状必ず一度戻るようにした。
        }       

        CompleteImage.SetActive(false);

        exp_Controller.EffectListClear();

        card_view.DeleteCard_DrawView();
        Destroy(this.gameObject);
        
    }
}
