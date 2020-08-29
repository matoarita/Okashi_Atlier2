using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour {

    //[SerializeField]
    private AudioSource[] _bgm = new AudioSource[2];

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
            case "Compound":
                PlayMain();
                break;

            default:

                PlaySub();
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {

        _bgm[0].volume = (1f - _mixRate) * 0.4f * fade_volume * GameMgr.MasterVolumeParam;
        _bgm[1].volume = _mixRate * 0.4f * fade_volume * GameMgr.MasterVolumeParam;

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

        if (GameMgr.GirlLoveEvent_stage1[5]) //コンテストの日の曲
        {
            _bgm[0].clip = sound10;
        }
        else
        {
            switch (GameMgr.stage_number)
            {
                case 1:

                    _bgm[0].clip = sound1;
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

    public void OnMainBGM()
    {
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

        fade_status = 3;
        //_mixRate = 1;
    }

    public void OnGetMatStartBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound3;
        _bgm[1].Play();

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

    public void OnGetMat_IdoBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound5;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void MuteBGM()
    {
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
