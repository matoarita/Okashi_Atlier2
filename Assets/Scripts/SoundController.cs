using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    private AudioClip[] seClips; //オーディオファイルの読み込み用。

    private Dictionary<string, int> seIndexes = new Dictionary<string, int>(); //読み込んだファイルにインデックスと、ファイル名をつける。

    const int cNumChannel = 4;
    private AudioSource[] seSources = new AudioSource[cNumChannel]; //オーディオソース
    //

    private AudioSource audioSource;


    void Awake(){

        //初期化
        for (int i = 0; i < seSources.Length; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
        }

        audioSource = GetComponent<AudioSource>();

        //オーディオファイルを全て読み込みしている。
        seClips = Resources.LoadAll<AudioClip>("Utage_Scenario/Sound/SE");


        for (int i = 0; i < seClips.Length; ++i)
        {
            seIndexes[seClips[i].name] = i;
        }

        /*Debug.Log("se ========================");
        foreach(var ac in seClips ) { Debug.Log( ac.name ); }*/
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySe( int index )
    {
        //seSources[0].clip = seClips[index];
        //seSources[0].Play();
        audioSource.PlayOneShot(seClips[index]);
    }

    public void StopSe()
    {
        //seSources[0].clip = seClips[index];
        //seSources[0].Play();
        audioSource.Stop();
    }
}
