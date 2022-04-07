using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class OptionPanel : MonoBehaviour {

    private GameObject canvas;

    private ItemDataBase database;
    private PlayerItemList pitemlist;

    private SoundController sc;
    private SaveController save_controller;

    private Slider mastervolume_Slider;
    private Text mastervolume_paramtext;
    private int mastervolume_param;

    private Slider BGMvolume_Slider;
    private Text BGMvolume_paramtext;
    private int BGMvolume_param;

    private Slider SEvolume_Slider;
    private Text SEvolume_paramtext;
    private int SEvolume_param;

    private Text GameSpeed_paramtext;
    private Text music_paramtext;

    private GameObject MusicSelect_panel;
    private GameObject bgm_select_content;
    private GameObject bgm_select_obj;

    private Toggle Autosave_on_toggle;
    private GameObject Autosave_text_obj;

    private GameObject system_panel;

    private Compound_Main compound_Main;

    private GameObject SystemSave_Panel;

    private GameObject Gamespeed_Panel;
    private GameObject BGMSelectPanel;

    private BGM sceneBGM;
    private List<GameObject> bgm_toggle = new List<GameObject>();
    private Dropdown Bgm_dropdown;
    private List<Toggle> gamespeed_toggle = new List<Toggle>();

    private int i;

    // Use this for initialization
    void Start () {

        OptionInit();
    }
	
	// Update is called once per frame
	void Update () {
		
        if(sc == null)
        {
            //サウンドコントローラーの取得
            sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
        }
	}

    void OptionInit()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        save_controller = SaveController.Instance.GetComponent<SaveController>();
        SystemSave_Panel = this.transform.Find("systemSavePanel").gameObject;

        //調合メイン取得
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();
                system_panel = canvas.transform.Find("SystemPanel").gameObject;
                system_panel.SetActive(false);

                //BGMの取得
                sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

                break;

            case "001_Title":

                break;
        }
       
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        mastervolume_Slider = this.transform.Find("OptionList/Viewport/Content/SoundPanel/MasterVolumeSliderPanel/MasterVolumeSlider").GetComponent<Slider>();
        mastervolume_paramtext = this.transform.Find("OptionList/Viewport/Content/SoundPanel/MasterVolumeSliderPanel/MasterVolumeSlider/Param").GetComponent<Text>();

        BGMvolume_Slider = this.transform.Find("OptionList/Viewport/Content/SoundPanel/BGMVolumeSliderPanel/BGMVolumeSlider").GetComponent<Slider>();
        BGMvolume_paramtext = this.transform.Find("OptionList/Viewport/Content/SoundPanel/BGMVolumeSliderPanel/BGMVolumeSlider/Param").GetComponent<Text>();

        SEvolume_Slider = this.transform.Find("OptionList/Viewport/Content/SoundPanel/SEVolumeSliderPanel/SEVolumeSlider").GetComponent<Slider>();
        SEvolume_paramtext = this.transform.Find("OptionList/Viewport/Content/SoundPanel/SEVolumeSliderPanel/SEVolumeSlider/Param").GetComponent<Text>();

        Autosave_on_toggle = this.transform.Find("OptionList/Viewport/Content/AutoSaveOn/AutoSaveToggle").GetComponent<Toggle>();
        Autosave_text_obj= this.transform.Find("OptionList/Viewport/Content/AutoSaveOn/autosave_text").gameObject;

        GameSpeed_paramtext = this.transform.Find("OptionList/Viewport/Content/GameSpeed/speed_text").GetComponent<Text>();
        music_paramtext = this.transform.Find("OptionList/Viewport/Content/BGMSelectPanel/music_text").GetComponent<Text>();
        Bgm_dropdown = this.transform.Find("OptionList/Viewport/Content/BGMSelectPanel/Dropdown").GetComponent<Dropdown>();

        Gamespeed_Panel = this.transform.Find("OptionList/Viewport/Content/GameSpeed").gameObject;
        BGMSelectPanel = this.transform.Find("OptionList/Viewport/Content/BGMSelectPanel").gameObject;

        MusicSelect_panel = this.transform.Find("MusicSelectPanel").gameObject;
        MusicSelect_panel.SetActive(false);
        bgm_select_content = this.transform.Find("MusicSelectPanel/Scroll_View/Viewport/Content").gameObject;
        bgm_select_obj = (GameObject)Resources.Load("Prefabs/BGMSelect");

        if (GameMgr.AUTOSAVE_ON)
        {
            Autosave_on_toggle.SetIsOnWithoutCallback(true);
            Autosave_text_obj.SetActive(true);
            //sc.PlaySe(21);
        }
        else
        {
            Autosave_on_toggle.SetIsOnWithoutCallback(false);
            Autosave_text_obj.SetActive(false);
            //sc.PlaySe(2);
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "001_Title":

                Gamespeed_Panel.SetActive(false);
                BGMSelectPanel.SetActive(false);
                break;

            case "Compound":

                if (GameMgr.Story_Mode == 0)
                {
                    Gamespeed_Panel.SetActive(false);
                    BGMSelectPanel.SetActive(false);
                }
                else
                {
                    Gamespeed_Panel.SetActive(true);
                    if(pitemlist.KosuCount("music_box") >= 1)
                    {
                        BGMSelectPanel.SetActive(true);
                    }
                    else
                    {
                        BGMSelectPanel.SetActive(false);
                    }

                    //BGM所持チェック
                    BGMFlagCheck();

                    //BGMリストの初期化
                    bgm_toggle.Clear();
                    foreach (Transform child in bgm_select_content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
                    {
                        Destroy(child.gameObject);
                    }

                    for (i = 0; i < GameMgr.bgm_collection_list.Count; i++)
                    {
                        bgm_toggle.Add(Instantiate(bgm_select_obj, bgm_select_content.transform));
                        bgm_toggle[i].GetComponent<bgmSelectToggle>().toggle_ID = i;

                        //デバッグ用　全部ON
                        //GameMgr.bgm_collection_list[i].Flag = true;
                        //デバッグ用  ここまで
                    }



                    for (i = 0; i < bgm_toggle.Count; i++)
                    {
                        bgm_toggle[i].transform.Find("Background/MusicTextBack").gameObject.SetActive(true);
                        bgm_toggle[i].transform.Find("Background/MusicText").gameObject.SetActive(false);

                        if (GameMgr.bgm_collection_list[i].Flag)
                        {                            
                            bgm_toggle[i].transform.Find("Background/MusicTextBack").GetComponent<Text>().text = GameMgr.bgm_collection_list[i].titleNameHyouji;
                            bgm_toggle[i].transform.Find("Background/MusicText").GetComponent<Text>().text = GameMgr.bgm_collection_list[i].titleNameHyouji;
                            bgm_toggle[i].transform.GetComponent<Toggle>().interactable = true;
                            bgm_toggle[i].transform.GetComponent<Sound_Trigger>().enabled = true;                           
                        }
                        else
                        {
                            bgm_toggle[i].transform.Find("Background/MusicTextBack").GetComponent<Text>().text = "???";
                            bgm_toggle[i].transform.GetComponent<Toggle>().interactable = false;
                            bgm_toggle[i].transform.GetComponent<Sound_Trigger>().enabled = false;
                        }
                    }

                    //現在のBGMの表示更新
                    DrawBGMSelectToggle();

                    //ゲームスピード変更のトグル
                    gamespeed_toggle.Clear();
                    foreach (Transform child in this.transform.Find("OptionList/Viewport/Content/GameSpeed/Scroll_View/Viewport/Content").transform) //
                    {
                        gamespeed_toggle.Add(child.gameObject.GetComponent<Toggle>());
                    }

                    for (i = 0; i < gamespeed_toggle.Count; i++)
                    {
                        gamespeed_toggle[i].SetIsOnWithoutCallback(false);
                    }
                    gamespeed_toggle[GameMgr.GameSpeedParam - 1].SetIsOnWithoutCallback(true);
                    GameSpeedChange();
                }
                break;
        }
        
    }

    private void OnEnable()
    {
        OptionInit();

        mastervolume_param = (int)(GameMgr.MasterVolumeParam * 100) / 2;
        mastervolume_Slider.value = (int)(GameMgr.MasterVolumeParam * 100);
        mastervolume_paramtext.text = mastervolume_param.ToString();

        BGMvolume_param = (int)(GameMgr.BGMVolumeParam * 100) / 2;
        BGMvolume_Slider.value = (int)(GameMgr.BGMVolumeParam * 100);
        BGMvolume_paramtext.text = BGMvolume_param.ToString();

        SEvolume_param = (int)(GameMgr.SeVolumeParam * 100) / 2;
        SEvolume_Slider.value = (int)(GameMgr.SeVolumeParam * 100);
        SEvolume_paramtext.text = SEvolume_param.ToString();
    }

    public void OnMasterVolume()
    {
        //初期値 0~200 100
        mastervolume_param = (int)(mastervolume_Slider.value / 2);
        mastervolume_paramtext.text = mastervolume_param.ToString();

        //反映
        GameMgr.MasterVolumeParam = mastervolume_Slider.value / 100;
        sc.VolumeSetting();
    }

    public void OnBGMVolume()
    {
        //初期値 0~200 100
        BGMvolume_param = (int)(BGMvolume_Slider.value / 2);
        BGMvolume_paramtext.text = BGMvolume_param.ToString();

        //反映
        GameMgr.BGMVolumeParam = BGMvolume_Slider.value / 100;        
    }

    public void OnSEVolume()
    {
        //初期値 0~200 100
        SEvolume_param = (int)(SEvolume_Slider.value / 2);
        SEvolume_paramtext.text = SEvolume_param.ToString();

        //反映
        GameMgr.SeVolumeParam = SEvolume_Slider.value / 100;
        sc.VolumeSetting();

        //ステージクリアボタンの音量は、「StageClear_Button」スクリプトで直接調整
    }

    public void SelectBGM(int _id)
    {
        GameMgr.userBGM_Num = _id;
        music_paramtext.text = "♪ " + GameMgr.bgm_collection_list[GameMgr.userBGM_Num].titleNameHyouji;

        DrawBGMSelectToggle();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                sceneBGM.PlayMain();
                break;
        }

    }

    void DrawBGMSelectToggle()
    {
        for (i = 0; i < bgm_toggle.Count; i++)
        {
            bgm_toggle[i].GetComponent<Toggle>().SetIsOnWithoutCallback(false);
            bgm_toggle[i].transform.Find("Background/MusicText").gameObject.SetActive(false);
            bgm_toggle[i].transform.Find("BG_Particle_Light").gameObject.SetActive(false);
        }

        bgm_toggle[GameMgr.userBGM_Num].GetComponent<Toggle>().SetIsOnWithoutCallback(true);
        bgm_toggle[GameMgr.userBGM_Num].transform.Find("Background/MusicText").gameObject.SetActive(true);
        bgm_toggle[GameMgr.userBGM_Num].transform.Find("BG_Particle_Light").gameObject.SetActive(true);
        music_paramtext.text = "♪ " + GameMgr.bgm_collection_list[GameMgr.userBGM_Num].titleNameHyouji;
    }

    public void GameSpeedChange()
    {

        if (gamespeed_toggle[0].isOn)
        {
            GameMgr.GameSpeedParam = 1;
            GameSpeed_paramtext.text = "めちゃはや";
        }
        else if (gamespeed_toggle[1].isOn)
        {
            GameMgr.GameSpeedParam = 2;
            GameSpeed_paramtext.text = "はやい";
        }
        else if (gamespeed_toggle[2].isOn)
        {
            GameMgr.GameSpeedParam = 3;
            GameSpeed_paramtext.text = "ふつう";
        }
        else if (gamespeed_toggle[3].isOn)
        {
            GameMgr.GameSpeedParam = 4;
            GameSpeed_paramtext.text = "ゆっくり";
        }
        else if (gamespeed_toggle[4].isOn)
        {
            GameMgr.GameSpeedParam = 5;
            GameSpeed_paramtext.text = "めちゃおそ";
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                
                break;
        }

    }

    void BGMFlagCheck()
    {
        if (pitemlist.KosuCount("Record_1") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm7", true);
        }
        if (pitemlist.KosuCount("Record_2") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm8", true);
        }
        if (pitemlist.KosuCount("Record_3") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm4", true);
        }
        if (pitemlist.KosuCount("Record_4") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm10", true);
        }
        if (pitemlist.KosuCount("Record_5") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm11", true);
        }

        //採取地系のBGM
        if (pitemlist.KosuCount("Record_6") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm12", true);
        }
        if (pitemlist.KosuCount("Record_7") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm14", true);
        }
        if (pitemlist.KosuCount("Record_8") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm9", true);
        }
        if (pitemlist.KosuCount("Record_9") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm13", true);
        }
        if (pitemlist.KosuCount("Record_10") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm15", true);
        }
        if (pitemlist.KosuCount("Record_11") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm16", true);
        }
        if (pitemlist.KosuCount("Record_12") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm17", true);
        }
        if (pitemlist.KosuCount("Record_13") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm18", true);
        }

        if (pitemlist.KosuCount("Record_14") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm18", true);
        }

        if (pitemlist.KosuCount("Record_15") >= 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm5", true);
        }

        //サブイベントなど
        /*if (GameMgr.GirlLoveSubEvent_stage1[60]) //きらきらぽんぽん
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm5", true);
        }*/
    }

    public void OnAutosaveON()
    {
        if(Autosave_on_toggle.isOn)
        {
            GameMgr.AUTOSAVE_ON = true;
            Autosave_text_obj.SetActive(true);
            sc.PlaySe(81); //21
        }
        else
        {
            GameMgr.AUTOSAVE_ON = false;
            Autosave_text_obj.SetActive(false);
            sc.PlaySe(81);
        }
    }

    public void BackOption()
    {

        //一回黒フェードして「システムデータセーブ中」でてから、シーンもどす。
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                break;

            case "001_Title":

                save_controller.SystemsaveCheck();
                break;
        }       

        SystemSave_Panel.SetActive(true);
        SystemSave_Panel.GetComponent<CanvasGroup>().alpha = 0;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(SystemSave_Panel.GetComponent<CanvasGroup>().DOFade(1, 0.5f));

        StartCoroutine("EndOptionPanel");
    }

    IEnumerator EndOptionPanel()
    {
        yield return new WaitForSeconds(2.0f);

        SystemSave_Panel.SetActive(false);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                GameMgr.compound_select = 200;
                system_panel.SetActive(true);
                this.gameObject.SetActive(false);
                break;

            case "001_Title":

                this.gameObject.SetActive(false);
                break;
        }
    }

    public void OnMusicSelectPanel()
    {
        //音楽セレクト画面をオンに。
        MusicSelect_panel.SetActive(true);
    }
    public void BackMusicSelect()
    {
        //音楽セレクト画面をオフに。
        MusicSelect_panel.SetActive(false);
    }
}
