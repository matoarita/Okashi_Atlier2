using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Ambience : MonoBehaviour {

    //[SerializeField]
    private AudioSource[] _bgm = new AudioSource[1];

    public AudioClip sound1;  //水のせせらぎ１

    private GameObject canvas;
    private GetMatPlace_Panel getmatplace_panel;

    private int fade_status;
    private float fade_volume;
    private float _fadedeg;

    private float sound_hosei;

    private int i;   

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();

        //使用するAudioSource取得。２つを取得。
        _bgm = GetComponents<AudioSource>();

        fade_status = 1; //0=fade_out 1=OFF 2=fade_in
        fade_volume = 1.0f;
        _fadedeg = 0.03f; //フェードの音量減少量

        sound_hosei = 1.0f; //各フィールドごとに、補正値をかけて音量調節

        _bgm[0].Stop();

    }
	
	// Update is called once per frame
	void Update () {

        _bgm[0].volume = 0.4f * sound_hosei * fade_volume * GameMgr.MasterVolumeParam;

        if (fade_status == 0) //フェードアウトがON
        {
            if (fade_volume <= 0)
            {
                fade_status = 1;
                Stop();
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
    }

    public void OnLavenderField()
    {
        
        FadeIn();
        sound_hosei = 0.5f;
        _bgm[0].clip = sound1;
        _bgm[0].Play();
    }

    public void Play()
    {
        _bgm[0].clip = sound1;
        _bgm[0].Play();

    }

    public void Stop()
    {
        _bgm[0].Stop();

    }

    public void Mute()
    {
        _bgm[0].mute = true;
    }

    public void MuteOFF()
    {
        _bgm[0].mute = false;
    }

    public void FadeOut()
    {
        fade_status = 0;
    }

    public void FadeIn()
    {
        fade_volume = 0.0f;
        fade_status = 2;
    }

    public void NowFadeVolumeON() //ただちにフェードのボリュームをもとに戻す。
    {
        fade_volume = 1.0f;
    }

    public void NowFadeVolumeOFF() //ただちにフェードのボリュームを0にする。ミュートと、効果的には一緒。
    {
        fade_volume = 0.0f;
    }
}
