using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BGMController : SingletonMonoBehaviour<BGMController>
{

    private AudioSource[] _bgm = new AudioSource[3];

    public float _mixRate = 0;

    private int fade_status;
    private float fade_volume;
    private float _fadedeg;

    private int _ambient_num;

    private float _start_val;

    private Tween _tw;

    private int i;

    // Use this for initialization
    void Start () {

        this.gameObject.AddComponent<AudioSource>(); //シーンのメインで使うBGMを入れる
        this.gameObject.AddComponent<AudioSource>(); //調合シーン用のBGMと、酒場クエスト高得点時のファンファーレを入れる
        this.gameObject.AddComponent<AudioSource>(); //メインクエストクリア時（1の時の）のタンタカターンのBGM用　２では恐らく使わないはずだけど、一応開けとく
        this.gameObject.AddComponent<AudioSource>(); //環境音を鳴らす用 _bgm[0]と同時に鳴らすかも。

        _ambient_num = 3;

        //使用するAudioSource取得。4つを取得。
        _bgm = GetComponents<AudioSource>();

        for (i = 0; i < _bgm.Length; i++) {
            _bgm[i].loop = true;
        }

        fade_status = 100; //0=fade_out  2=fade_in 100=待機状態
        fade_volume = 1.0f;
        _fadedeg = 0.03f; //フェードの音量減少量
        //_bgm[1]の調合用BGMやファンファーレを設定　_bgm[0]から切り替える
    }

    // Update is called once per frame
    void Update () {

        _bgm[0].volume = (1f - _mixRate) * 0.4f * fade_volume * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam; //シーンのメインBGM
        _bgm[_ambient_num].volume = (1f - _mixRate) * 0.4f * fade_volume * GameMgr.MasterVolumeParam * GameMgr.AmbientVolumeParam; //シーンの環境音

        //Debug.Log("fade_volume: " + fade_volume);
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                _bgm[1].volume = _mixRate * 0.4f * fade_volume * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
                break;

            /*case "Bar":

                _bgm[1].volume = 0.4f * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
                break;*/

            default:

                _bgm[1].volume = _mixRate * 0.4f * fade_volume * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
                break;
        }

        switch(fade_status)
        {
            case 0: //フェードアウトがON

                if (fade_volume <= 0)
                {
                    fade_status = 100; //100=待機状態
                }
                else
                {
                    fade_volume -= _fadedeg;
                }
                break;

            case 2: //フェードインがON

                if (fade_volume >= 1.0f)
                {
                    fade_status = 100;
                }
                else
                {
                    fade_volume += _fadedeg;
                }
                break;

            case 3: //ミックスフェードで切り替え　0 -> 1

                _mixRate += _fadedeg;
                if (_mixRate >= 1.0f)
                {
                    _mixRate = 1;
                    fade_status = 100;
                }
                break;

            case 4: //ミックスフェードで切り替え　1 -> 0

                _mixRate -= _fadedeg;
                if (_mixRate < 0.0f)
                {
                    _mixRate = 0;
                    fade_status = 100;
                }
                break;

            case 100: //待機状態

                break;
        }

    }


    public void BGMPlay(int _num, AudioClip _clip)
    {
        if (_bgm[_num].clip == _clip)
        {
            //同じBGMがなってる場合は、そのまま鳴らし続ける
        }
        else
        {
            _bgm[_num].clip = _clip;
            _bgm[_num].Play();
        }

        Debug.Log("OnBGMPlay");
    }

    //必ずBGMの頭に再生ヘッドを戻して、音を鳴らす
    public void BGMRestartPlay(int _num, AudioClip _clip)
    {
            _bgm[_num].clip = _clip;
            _bgm[_num].Play();

        Debug.Log("OnBGM RestartPlay");
    }

    public void BGMStop(int _num)
    {
        _bgm[_num].Stop();
    }

    public void BGMVolume(int _num) //一時的にボリューム変更するのに使う
    {
        _bgm[_num].volume = 0.4f * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
    }

    public void BGMMute(int _num, int _status)
    {
        switch(_status) //MuteかMuteOFFか
        {
            case 0:

                _bgm[_num].mute = true;
                break;

            case 1:

                _bgm[_num].mute = false;
                break;
        }
        
    }

    //環境音を鳴らす　_numは3番で固定
    public void AmbientPlay(AudioClip _clip)
    {
        if (_bgm[_ambient_num].clip == _clip)
        {
            //同じBGMがなってる場合は、そのまま鳴らし続ける
        }
        else
        {
            _bgm[_ambient_num].clip = _clip;
            _bgm[_ambient_num].Play();
        }
    }

    public void AmbientRestartPlay(AudioClip _clip)
    {

        _bgm[_ambient_num].clip = _clip;
        _bgm[_ambient_num].Play();

    }

    public void AmbientStop()
    {
        _bgm[_ambient_num].Stop();
    }

    public void AmbientMute(int _status)
    {
        switch (_status) //MuteかMuteOFFか
        {
            case 0:

                _bgm[_ambient_num].mute = true;
                break;

            case 1:

                _bgm[_ambient_num].mute = false;
                break;
        }

    }


    public void MixRateChange(float _rate)
    {
        _mixRate = _rate;
    }

    public void FadeStatusChange(int _status)
    {
        fade_status = _status;
    }

    public void FadeVolumeChange(float _volume)
    {
        DOTween.Kill(_tw);
        fade_volume = _volume;
    }



    //DoTweenを使ってボリュームをフェードするメソッドたち　FadeStatusChangeがいらないかも。
    public void DoFadeBGM(int _num)
    {
        _tw = _bgm[_num].DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            _bgm[_num].Stop();
        });
    }

    public void DoFadeVolumeOut(float _time)
    {
        _start_val = fade_volume;
        _tw = DOVirtual.Float(_start_val, 0f, _time, value =>
        {
            //Debug.Log("value: " + value);
            fade_volume = value;
        });
    }

    public void DoFadeVolumeIn(float _time)
    {
        _start_val = fade_volume;
        _tw = DOVirtual.Float(_start_val, 1f, _time, value =>
        {
            //Debug.Log("value: " + value);
            fade_volume = value;
        });
    }
}
