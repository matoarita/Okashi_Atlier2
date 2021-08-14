using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionPanel : MonoBehaviour {

    private GameObject canvas;

    private SoundController sc;

    private Slider mastervolume_Slider;
    private Text mastervolume_paramtext;
    private int mastervolume_param;

    private Slider BGMvolume_Slider;
    private Text BGMvolume_paramtext;
    private int BGMvolume_param;

    private Slider SEvolume_Slider;
    private Text SEvolume_paramtext;
    private int SEvolume_param;

    private GameObject system_panel;

    private Compound_Main compound_Main;

    private BGM sceneBGM;
    private List<Toggle> bgm_toggle = new List<Toggle>(); 

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

        mastervolume_Slider = this.transform.Find("OptionList/Viewport/Content/MasterVolumeSliderPanel/MasterVolumeSlider").GetComponent<Slider>();
        mastervolume_paramtext = this.transform.Find("OptionList/Viewport/Content/MasterVolumeSliderPanel/MasterVolumeSlider/Param").GetComponent<Text>();

        BGMvolume_Slider = this.transform.Find("OptionList/Viewport/Content/BGMVolumeSliderPanel/BGMVolumeSlider").GetComponent<Slider>();
        BGMvolume_paramtext = this.transform.Find("OptionList/Viewport/Content/BGMVolumeSliderPanel/BGMVolumeSlider/Param").GetComponent<Text>();

        SEvolume_Slider = this.transform.Find("OptionList/Viewport/Content/SEVolumeSliderPanel/SEVolumeSlider").GetComponent<Slider>();
        SEvolume_paramtext = this.transform.Find("OptionList/Viewport/Content/SEVolumeSliderPanel/SEVolumeSlider/Param").GetComponent<Text>();

        //BGMセレクトをONにするときは、以下の命令もONにする。

        /*bgm_toggle.Clear();
        foreach (Transform child in this.transform.Find("OptionList/Viewport/Content/BGMSelectPanel/Scroll_View/Viewport/Content").transform) //
        {
            bgm_toggle.Add(child.gameObject.GetComponent<Toggle>());           
        }

        for(i=0; i < bgm_toggle.Count; i++)
        {
            bgm_toggle[i].SetIsOnWithoutCallback(false);
        }
        bgm_toggle[GameMgr.mainBGM_Num].SetIsOnWithoutCallback(true);*/
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

    public void SelectBGM()
    {

        if (bgm_toggle[0].isOn)
        {
            GameMgr.mainBGM_Num = 0;
        }
        else if (bgm_toggle[1].isOn)
        {
            GameMgr.mainBGM_Num = 1;
        }
        else if (bgm_toggle[2].isOn)
        {
            GameMgr.mainBGM_Num = 2;
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                sceneBGM.PlayMain();
                break;
        }

    }

    public void BackOption()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                compound_Main.compound_select = 200;
                system_panel.SetActive(true);
                this.gameObject.SetActive(false);
                break;

            case "001_Title":

                this.gameObject.SetActive(false);
                break;
        }
    }
}
