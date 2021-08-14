using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BGM : MonoBehaviour {

    //[SerializeField]
    private AudioSource[] _bgm = new AudioSource[3];

    public AudioClip sound1;  //Stage1MainのBGM
    public AudioClip sound2;  //調合中のBGM
    public AudioClip sound3;  //材料採取画面
    public AudioClip sound4;  //「近くの森」BGM
    public AudioClip sound5;  //「井戸」BGM
    public AudioClip sound6;  //Stage2のBGM
    public AudioClip sound7;  //Stage3のBGM
    public AudioClip sound8;  //「ストロベリーガーデン」BGM
    public AudioClip sound9;  //「ひまわりの丘」BGM
    public AudioClip sound10;  //コンテスト時のメインBGM
    public AudioClip sound11;  //「ラベンダー畑」BGM
    public AudioClip sound12;  //「バードサンクチュアリ」BGM
    public AudioClip sound13;  //お好みBGM_01
    public AudioClip sound14;  //メインクリア後アイキャッチのBGM
    public AudioClip sound15;  //「ねこのお墓」BGM
    public AudioClip sound16;  //チュートリアルBGM
    public AudioClip sound17;  //「ベリーファーム」BGM
    public AudioClip sound18;  //Stage1MainのBGM2
    public AudioClip sound19;  //Stage1MainのBGM3
    public AudioClip sound20;  //Stage1MainのBGM4
    public AudioClip sound21;  //Stage1MainのBGM5

    [Range(0, 1)]
    public float _mixRate = 0;

    private int fade_status;
    private float fade_volume;
    private float _fadedeg;

    private int i;

    // Use this for initialization
    void Start () {

        //使用するAudioSource取得。２つを取得。
        _bgm = GetComponents<AudioSource>();

        fade_status = 1; //0=fade_out 1=OFF 2=fade_in
        fade_volume = 1.0f;
        _fadedeg = 0.03f; //フェードの音量減少量
        //_bgm[1]のほうに、各シーンごとのBGMを切り替えては入れて、その後_bgm[0]から切り替える

        switch (SceneManager.GetActiveScene().name)
        {
            case "001_Title":

                PlaySub();
                break;

            case "Compound":

                PlayMain();
                break;

            case "110_TotalResult":

                if(GameMgr.ending_number == 4)
                {
                    EndingBGM_A();
                }
                else
                {
                    EndingBGM_B();
                }
                break;

            default:

                PlaySub();
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {

        _bgm[0].volume = (1f - _mixRate) * 0.4f * fade_volume * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                _bgm[1].volume = _mixRate * 0.4f * fade_volume * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
                break;
        }

        if(fade_status == 0) //フェードアウトがON
        {           
            if(fade_volume <= 0)
            {
                fade_status = 1;
            }
            else
            {
                fade_volume -= _fadedeg;
            }
        }

        if (fade_status == 2) //フェードインがON
        {
            if (fade_volume >= 1.0f)
            {
                fade_status = 1;
            }
            else
            {
                fade_volume += _fadedeg;
            }
        }

        if (fade_status == 3) //ミックスフェードで切り替え　0 -> 1
        {
            _mixRate += _fadedeg;
            if (_mixRate >= 1.0f)
            {
                _mixRate = 1;
                fade_status = 1;
            }
        }

        if (fade_status == 4) //ミックスフェードで切り替え　1 -> 0
        {
            _mixRate -= _fadedeg;
            if (_mixRate < 0.0f)
            {
                _mixRate = 0;
                fade_status = 1;
            }
        }
    }

    public void PlayMain()
    {

        if (GameMgr.GirlLoveEvent_stage1[50]) //コンテストの日の曲
        {
            _bgm[0].clip = sound10;
        }
        else
        {
            switch (GameMgr.stage_number)
            {
                case 1:

                    Story_BGMSelect();
                    break;

                case 2:

                    _bgm[0].clip = sound6;
                    break;

                case 3:

                    _bgm[0].clip = sound7;
                    break;
            }
        }
        
        _bgm[0].Play();

        _bgm[1].clip = sound2;
        _bgm[1].Play();
    }

    public void PlaySub()
    {
        _bgm[0].clip = sound1;
        _bgm[0].Play();

    }

    public void EndingBGM_A()
    {
        _bgm[0].clip = sound1;
        _bgm[0].Play();
    }

    public void EndingBGM_B()
    {
        _bgm[0].clip = sound2;
        _bgm[0].Play();
    }

    void Story_BGMSelect()
    {
        switch (GameMgr.mainBGM_Num)
        {
            case 0:

                _bgm[0].clip = sound20;
                break;

            case 1:

                _bgm[0].clip = sound11;
                break;

            case 2:

                _bgm[0].clip = sound21;
                break;

            case 3:

                _bgm[0].clip = sound1;
                break;

            case 4:

                _bgm[0].clip = sound1;
                break;

            case 5:

                _bgm[0].clip = sound19;
                break;

            default:

                _bgm[0].clip = sound19;
                break;
        }

    }

    public void OnMainBGM()
    {
        Story_BGMSelect();
        _bgm[0].Play();

        _mixRate = 0;
    }
    public void OnMainBGMFade()
    {

        fade_status = 4;
    }

    public void OnCompoundBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound2;
        _bgm[1].Play();

        fade_status = 3; //フェードで切り替える
        //_mixRate = 1;
    }

    public void OnGetMatStartBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound3;
        _bgm[1].Play();

        //fade_status = 3;
        _mixRate = 1;
    }

    public void OnGetMat_ForestBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound4;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_LavenderFieldBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound11;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_StrawberryGardenBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound8;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_HimawariHillBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound9;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_BirdSanctualiBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound12;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_CatGraveBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound15;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_IdoBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound5;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_BerryFarmBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound17;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnTutorialBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound16;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnMainClearResultBGM()
    {
        _bgm[2].clip = sound14;
        _bgm[2].volume = 0.4f * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
        _bgm[2].Play();
    }

    public void OnMainClearResultBGMOFF()
    {
        _bgm[2].DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            _bgm[2].Stop();
        });
    }

    public void MuteBGM()
    {
        Debug.Log("Mute BGM");
        _bgm[0].mute = true;
        _bgm[1].mute = true;
    }

    public void MuteOFFBGM()
    {
        _bgm[0].mute = false;
        _bgm[1].mute = false;
    }

    public void FadeOutBGM()
    {
        fade_status = 0;
    }

    public void FadeInBGM()
    {
        fade_status = 2;
    }

    public void NowFadeVolumeONBGM() //ただちにフェードのボリュームをもとに戻す。
    {
        fade_volume = 1.0f;
    }

    public void NowFadeVolumeOFFBGM() //ただちにフェードのボリュームを0にする。ミュートと、効果的には一緒。
    {
        fade_volume = 0.0f;
    }
}
