using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;

public class MainUIPanel : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject canvas;
    private GameObject TimePanel_obj;
    private GameObject _CompObj;

    private SoundController sc;

    private Compound_Main compound_Main;

    private int total_obj_count;

    private CubismModel _model;

    private GameObject text_area_Main;
    private Text _textmain;

    private GameObject girl_love_exp_bar;
    private Image bar_sprite;
    private Sprite bar_sprite_1;
    private Sprite bar_sprite_2;

    private GameObject manpuku_bar;

    private Text stage_text;
    private GameObject stage_text_obj;
    private GameObject FreeModeText_obj;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //Live2Dモデル取得
        _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();

        TimePanel_obj = this.transform.Find("Comp/TimePanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        text_area_Main = canvas.transform.Find("MessageWindowMain").gameObject;
        _textmain = text_area_Main.GetComponentInChildren<Text>();

        stage_text_obj = this.transform.Find("StagePanel/Image/StageText").gameObject;
        stage_text = stage_text_obj.GetComponent<Text>();
        FreeModeText_obj = this.transform.Find("StagePanel/Image/FreeModeText").gameObject;

        _CompObj = this.transform.Find("Comp/").gameObject;

        //好感度バーの取得
        girl_love_exp_bar = canvas.transform.Find("MainUIPanel/Girl_love_exp_bar").gameObject;
        bar_sprite = girl_love_exp_bar.transform.Find("Fill Area/Fill").GetComponent<Image>();
        bar_sprite_1 = Resources.Load<Sprite>("Sprites/Window/gaugeC_bar_pink_h66");
        bar_sprite_2 = Resources.Load<Sprite>("Sprites/Window/gaugeC_bar_orange_h66");

        //満腹ゲージの取得
        manpuku_bar = canvas.transform.Find("MainUIPanel/ManpukuBar").gameObject;
        manpuku_bar.SetActive(false);

        SetStageMode();           

        total_obj_count = 0;
        foreach (Transform child in this.transform)
        {
            total_obj_count++;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //compoundmainから更新
    public void StageNumKoushin()
    {
        stage_text.text = GameMgr.stage_quest_num.ToString() + "-" + GameMgr.stage_quest_num_sub.ToString();
        SetStageMode();
    }

    void SetStageMode()
    {
        if (GameMgr.Story_Mode == 0)
        {
            bar_sprite.sprite = bar_sprite_1;
            stage_text_obj.SetActive(true);
            FreeModeText_obj.SetActive(false);
        }
        else
        {
            bar_sprite.sprite = bar_sprite_2;
            stage_text_obj.SetActive(false);
            FreeModeText_obj.SetActive(true);

            if (GameMgr.System_Manpuku_ON)
            {
                manpuku_bar.SetActive(true);
            }
            else
            {
                manpuku_bar.SetActive(false);
            }
        }
    }

    public void OnOpenButton() //未使用
    {
        /*
        //_CompObj.gameObject.SetActive(true);

        //girl_love_exp_bar.SetActive(true);       
        text_area_Main.SetActive(true); //テキストエリアメインは、こっちもON/OFFが必要


        if (GameMgr.TimeUSE_FLAG == false)
        {
            TimePanel_obj.SetActive(false);
        }

        compound_Main.CheckButtonFlag();
        compound_Main.QuestClearCheck();

        //カメラ左へ。キャラが左へいく。
        trans = 10; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);*/

        GameMgr.MenuOpenFlag = true; //現在メニューを開いている状態
    }

    public void OnCloseButton() //未使用
    {
        /*
        //_CompObj.SetActive(false);

        //girl_love_exp_bar.SetActive(false);       
        text_area_Main.SetActive(true);

        //カメラ正面に戻る。
        trans = 11; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);
        */
        GameMgr.MenuOpenFlag = false; //現在メニューを閉じている状態
    }

    
}
