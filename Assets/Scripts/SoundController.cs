using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : SingletonMonoBehaviour<SoundController>
{
    private AudioClip[] seClips; //オーディオファイルの読み込み用。

    private Dictionary<string, int> seIndexes = new Dictionary<string, int>(); //読み込んだファイルにインデックスと、ファイル名をつける。

    const int cNumChannel = 16;
    private AudioSource[] seSources = new AudioSource[cNumChannel]; //オーディオソース
    //

    Queue<int> seRequestQueue = new Queue<int>();


    void Awake(){

        //初期化
        for (int i = 0; i < seSources.Length; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
        }

        //オーディオファイルを全て読み込みしている。
        seClips = Resources.LoadAll<AudioClip>("Utage_Scenario/Sound/SE");


        for (int i = 0; i < seClips.Length; ++i)
        {
            seIndexes[seClips[i].name] = i;
        }

        //Debug.Log("se ========================");
        //foreach(var ac in seClips ) { Debug.Log( ac.name ); }
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        int count = seRequestQueue.Count;
        if (count != 0)
        {
            int sound_index = seRequestQueue.Dequeue();
            playSeImpl(sound_index);
        }
    }

    //------------------------------------------------------------------------------
    private void playSeImpl(int index)
    {
        if (0 > index || seClips.Length <= index)
        {
            return;
        }

        foreach (AudioSource source in seSources)
        {
            if (false == source.isPlaying)
            {
                source.clip = seClips[index];
                source.Play();
                return;
            }
        }
    }

    public void PlaySe(string name)
    {
        PlaySe(GetSeIndex(name));
    }

    //一旦queueに溜め込んで重複を回避しているので
    //再生が1frame遅れる時がある

    //------------------------------------------------------------------------------
    public void PlaySe(int index)
    {
        if (!seRequestQueue.Contains(index))
        {
            seRequestQueue.Enqueue(index);
        }
    }

    //------------------------------------------------------------------------------
    public void StopSe()
    {
        ClearAllSeRequest();
        foreach (AudioSource source in seSources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    //------------------------------------------------------------------------------
    public void ClearAllSeRequest()
    {
        seRequestQueue.Clear();
    }

    public int GetSeIndex(string name)
    {
        return seIndexes[name];
    }

    public void VolumeSetting()
    {
        foreach (var source in seSources)
        {
            source.volume = 1.0f * GameMgr.MasterVolumeParam * GameMgr.SeVolumeParam;
        }
        //audioSource.volume = 1.0f * GameMgr.MasterVolumeParam;
    }
}
