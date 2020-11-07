using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour {

    private GameObject canvas;

    private SoundController sc;

    private Slider mastervolume_Slider;
    private Text mastervolume_paramtext;
    private int mastervolume_param;

    private GameObject system_panel;

    private GameObject StageClearButton_panel;
    private AudioSource StageClearbutton_audio;

    private Compound_Main compound_Main;

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
        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        system_panel = canvas.transform.Find("SystemPanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        mastervolume_Slider = this.transform.Find("OptionList/Viewport/Content/MasterVolumeSliderPanel/MasterVolumeSlider").GetComponent<Slider>();
        mastervolume_paramtext = this.transform.Find("OptionList/Viewport/Content/MasterVolumeSliderPanel/MasterVolumeSlider/Param").GetComponent<Text>();

        StageClearButton_panel = canvas.transform.Find("MainUIPanel/StageClearButton_Panel").gameObject;
        StageClearbutton_audio = StageClearButton_panel.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OptionInit();

        system_panel.SetActive(false);

        mastervolume_param = (int)(GameMgr.MasterVolumeParam * 100) / 2;
        mastervolume_Slider.value = (int)(GameMgr.MasterVolumeParam * 100);

        mastervolume_paramtext.text = mastervolume_param.ToString();
    }

    public void OnMasterVolume()
    {
        //初期値 0~200 100
        mastervolume_param = (int)(mastervolume_Slider.value / 2);
        mastervolume_paramtext.text = mastervolume_param.ToString();

        //反映
        GameMgr.MasterVolumeParam = mastervolume_Slider.value / 100;
        sc.VolumeSetting();

        //ステージクリアボタンの音量
        StageClearbutton_audio.volume = 1.0f * GameMgr.MasterVolumeParam;
        
    }

    public void BackOption()
    {
        compound_Main.compound_select = 200;
        system_panel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
