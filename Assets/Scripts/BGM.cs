using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

    //[SerializeField]
    private AudioSource[] _bgm = new AudioSource[2];

    public AudioClip sound1;  //Mainの調合BGM
    public AudioClip sound2;  //
    public AudioClip sound3;  //材料採取画面
    public AudioClip sound4;  //「近くの森」BGM
    public AudioClip sound5;  //「井戸」BGM

    [Range(0, 1)]
    public float _mixRate = 0;

    private int fade_status;
    private float fade_volume;
    private float _fadedeg;

    // Use this for initialization
    void Start () {

        //使用するAudioSource取得。２つを取得。
        _bgm = GetComponents<AudioSource>();

        fade_status = 1; //0=fade_out 1=OFF 2=fade_in
        fade_volume = 1.0f;
        _fadedeg = 0.03f; //フェードの音量減少量
        //_bgm[1]のほうに、各シーンごとのBGMを切り替えては入れて、その後_bgm[0]から切り替える

        Play();
    }
	
	// Update is called once per frame
	void Update () {

        _bgm[0].volume = (1f - _mixRate) * 0.4f * fade_volume;
        _bgm[1].volume = _mixRate * 0.4f * fade_volume;

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

    public void Play()
    {
        _bgm[0].clip = sound1;
        _bgm[0].Play();

        _bgm[1].clip = sound2;
        _bgm[1].Play();
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
