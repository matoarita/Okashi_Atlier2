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

    [Range(0, 1)]
    public float _mixRate = 0;

    // Use this for initialization
    void Start () {

        //使用するAudioSource取得。２つを取得。
        _bgm = GetComponents<AudioSource>();

        //_bgm[1]のほうに、各シーンごとのBGMを切り替えては入れて、その後_bgm[0]から切り替える

        Play();
    }
	
	// Update is called once per frame
	void Update () {

        _bgm[0].volume = (1f - _mixRate) * 0.4f;
        _bgm[1].volume = _mixRate * 0.4f;
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

    public void OnCompoundBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound2;
        _bgm[1].Play();

        _mixRate = 1;
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
}
