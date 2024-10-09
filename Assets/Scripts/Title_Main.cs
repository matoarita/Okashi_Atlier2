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
    private BGM sceneBGM;

    private Girl1_status girl1_status;

    private GameObject system_panel;
    private GameObject saveload_panel;
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

    private GameObject version_text;

    private bool StartRead;

    // Use this for initialization
    void Start () {

        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 1000;

        //Prefab内の、コンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子   

        system_panel = canvas.transform.Find("SystemPanel").gameObject;
        saveload_panel = canvas.transform.Find("SystemPanel/SaveLoadPanel").gameObject;
        saveload_panel.SetActive(false);
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

        version_text = canvas.transform.Find("VersionText").gameObject;
        version_text.GetComponent<Text>().text = "ver " + GameMgr.GameVersion.ToString("f2");

        //システムロード
        save_controller.SystemloadCheck();

        //これ以降、システムのデータに応じて、処理を分けて大丈夫。

        if (GameMgr.ending_count >= 1) //一回でもEDクリア。トップ画面はLive2Dモードになる。
        {
            galleryButton_obj.SetActive(true);           

            chara_Icon.SetActive(false);
            _model_move.SetActive(true);
            live2d_animator.SetLayerWeight(3, 0.0f); //メインでは、最初宴用表情はオフにしておく。

            PlayerStatus.girl1_Love_lv = GameMgr.stage1_clear_girl1_lovelv; //タイトル画面でのみの、一時的な好感度レベル。最後にクリアした時のレベルにしておく。         
            if(PlayerStatus.girl1_Love_lv <= 0) { PlayerStatus.girl1_Love_lv = 1; } //例外処理
            //Debug.Log("PlayerStatus.girl1_Love_lv: " + PlayerStatus.girl1_Love_lv);
            girl1_status.CheckGokigen();
            girl1_status.DefaultFace();

            
            /* if (GameMgr.bestend_on_flag) //エクストラモード出現条件　ED:Aをみる
            {
                freeModeButton_obj.SetActive(true);
                //freeModeButton_obj.GetComponent<Button>().interactable = true;
                //freeModeButton_obj.GetComponent<Sound_Trigger>().se_sound_ON = true;
                //freeModeButton_obj.transform.Find("Text").GetComponent<Text>().text = "エクストラモード";
            }
            else
            {
                freeModeButton_obj.SetActive(false);
                //freeModeButton_obj.GetComponent<Button>().interactable = false;
                //freeModeButton_obj.GetComponent<Sound_Trigger>().se_sound_ON = false;
                //freeModeButton_obj.transform.Find("Text").GetComponent<Text>().text = "???";
            }*/
            
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

        StartRead = false;

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }
	
	// Update is called once per frame
	void Update () {

        if (!StartRead) //シーン最初だけ読み込む
        {
            StartRead = true;
            sceneBGM.PlaySub();
            sceneBGM.NowFadeVolumeONBGM();
        }
    }

    public void OnStartButton()
    {
        save_controller.ResetAllParam(); //まず全てのパラメータを初期化
        save_controller.SystemloadCheck(); //システムデータロード　お菓子手帳やED回数など引継ぎデータはロード
        save_controller.ResetParamSecondTime();//いくつかのパラメータは、システムロード後に、またリセットする。食べた回数など。

        sceneBGM.NowFadeVolumeOFFBGM();
        GameMgr.Story_Mode = 0;
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("010_Prologue", GameMgr.SceneFadeTime);
    }

    public void OnStartButton2() //フリーモード　強くてニューゲーム
    {
        save_controller.ResetAllParam(); //まず全てのパラメータを初期化
        save_controller.SystemloadCheck(); //システムデータロード　お菓子手帳やED回数など引継ぎデータはロード
        save_controller.ResetParamSecondTime();//いくつかのパラメータは、システムロード後に、またリセットする。食べた回数など。

        sceneBGM.NowFadeVolumeOFFBGM();
        GameMgr.Story_Mode = 1;
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("010_Prologue", GameMgr.SceneFadeTime);
    }

    public void OnLoadButton()
    {
        GameMgr.SaveLoadPanel_mode = 1;
        saveload_panel.SetActive(true);

        //FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        //save_controller.OnLoadMethod(GameMgr.System_save_nowslot);

    }

    public void OnGalleryButton()
    {
        save_controller.SystemloadCheck(); //システムデータロード　お菓子手帳やED回数など引継ぎデータはロード
        FadeManager.Instance.fadeColor = new Color(1.0f, 1.0f, 1.0f);
        FadeManager.Instance.LoadScene("200_Omake", GameMgr.SceneFadeTime);
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

    //別シーンからこのシーンが読み込まれたときに、読み込む
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameMgr.Scene_LoadedOn_End = true;
    }

    //シーンがアンロードされたタイミングで呼び出しされる
    void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
        GameMgr.Scene_LoadedOn_End = false;

        //次シーン行く際のロード中　シーン番号
        GameMgr.Scene_Category_Num = 9999;
    }
}
