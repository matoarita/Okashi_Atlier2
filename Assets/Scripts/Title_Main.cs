using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

public class Title_Main : MonoBehaviour {

    private SaveController save_controller;
    private SoundController sc;

    private Debug_Panel_Init debug_panel_init;

    private Girl1_status girl1_status;

    private GameObject option_panel_obj;
    private GameObject galleryButton_obj;
    private GameObject freeModeButton_obj;
    private GameObject loadButton_obj;

    private GameObject canvas;

    //Live2Dモデルの取得    
    private GameObject _model_root_obj;
    private GameObject _model_move;
    private GameObject _model_obj;
    private CubismRenderController cubism_rendercontroller;
    private Animator live2d_animator;
    private GameObject chara_Icon;

    // Use this for initialization
    void Start () {

        //Prefab内の、コンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子   

        option_panel_obj = canvas.transform.Find("OptionPanel").gameObject;
        galleryButton_obj = canvas.transform.Find("TitleMenu/Viewport/Content/GalleryButton").gameObject;
        freeModeButton_obj = canvas.transform.Find("TitleMenu/Viewport/Content/GameStartButton_2").gameObject;
        freeModeButton_obj.SetActive(false);
        loadButton_obj = canvas.transform.Find("TitleMenu/Viewport/Content/GameLoadButton").gameObject;

        chara_Icon = GameObject.FindWithTag("Character").gameObject;

        //Live2Dモデルの取得
        _model_root_obj = GameObject.FindWithTag("CharacterRoot").gameObject;
        _model_move = _model_root_obj.transform.Find("CharacterMove").gameObject;
        _model_obj = _model_root_obj.transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;
        cubism_rendercontroller = _model_obj.GetComponent<CubismRenderController>();
        live2d_animator = _model_obj.GetComponent<Animator>();       

        //システムロード
        save_controller.SystemloadCheck();
        //これ以降、システムのデータに応じて、処理を分けて大丈夫。
        
        if (GameMgr.ending_count >= 1)
        {
            galleryButton_obj.SetActive(true);
            //freeModeButton_obj.SetActive(true);

            chara_Icon.SetActive(false);
            _model_move.SetActive(true);
            live2d_animator.SetLayerWeight(3, 0.0f); //メインでは、最初宴用表情はオフにしておく。

            PlayerStatus.girl1_Love_lv = 99;
            girl1_status.CheckGokigen();
            //girl1_status.DefaultFace();
            //girl1_status.GirlGokigenStatus = 99;
        }
        else
        {
            galleryButton_obj.SetActive(false);
            freeModeButton_obj.SetActive(false);

            chara_Icon.SetActive(true);
            _model_move.SetActive(false);
        }

        if(GameMgr.saveOK)
        {
            //ロードボタンを表示
            loadButton_obj.GetComponent<Button>().interactable = true;
            loadButton_obj.SetActive(true);
        }
        else
        {
            //ロードボタンを表示しない
            loadButton_obj.GetComponent<Button>().interactable = false;
            loadButton_obj.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartButton()
    {
        save_controller.ResetAllParam(); //まず全てのパラメータを初期化
        save_controller.SystemloadCheck(); //システムデータロード　お菓子手帳やED回数など引継ぎデータはロード
        save_controller.ResetParamSecondTime();//いくつかのパラメータは、システムロード後に、またリセットする。食べた回数など。

        GameMgr.Story_Mode = 0;
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("010_Prologue", 0.3f);
    }

    public void OnStartButton2() //フリーモード　強くてニューゲーム
    {
        save_controller.ResetAllParam(); //まず全てのパラメータを初期化
        save_controller.SystemloadCheck(); //システムデータロード　お菓子手帳やED回数など引継ぎデータはロード
        save_controller.ResetParamSecondTime();//いくつかのパラメータは、システムロード後に、またリセットする。食べた回数など。

        GameMgr.Story_Mode = 1;
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("010_Prologue", 0.3f);
    }

    public void OnLoadButton()
    {
        //save_controller.ResetAllParam();
        
        save_controller.OnLoadMethod();
        GameMgr.GameLoadOn = true; //順番が大事。ロードより後にこっちはtrueにしとく。

        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("Compound", 0.3f);
    }

    public void OnGalleryButton()
    {
        FadeManager.Instance.fadeColor = new Color(1.0f, 1.0f, 1.0f);
        FadeManager.Instance.LoadScene("200_Omake", 0.3f);
    }

    public void OnOptionButton()
    {
        option_panel_obj.SetActive(true);
    }

    public void OnGameEndButton()
    {
        Quit();
    }

    void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
        #endif
    }
}
