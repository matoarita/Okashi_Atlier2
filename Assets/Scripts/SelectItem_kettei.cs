using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectItem_kettei : MonoBehaviour {

    public bool kettei1;
    public bool onclick;

    public bool ketteiNouhin;
    public bool onclick2;

    public bool kettei3;

    // Use this for initialization
    void Start ()
    {
        kettei1 = false;
        onclick = false;

        ketteiNouhin = false;
        onclick2 = false;

        kettei3 = false;

        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick() //Yesが選択された時
    { // 必ず public にする
        //Debug.Log("clicked");
        onclick = true; //押された～というフラグ

        kettei1 = true;
    }

    public void OnClick2() //Noが選択された時
    { // 必ず public にする
        //Debug.Log("clicked");
        onclick = true;

        kettei1 = false;
    }

    public void NouhinOnClick() //Yesが選択された時
    { // 必ず public にする
        //Debug.Log("clicked");
        onclick2 = true; //押された～というフラグ

        ketteiNouhin = true;
    }

    public void NouhinOnClick2() //Noが選択された時
    { // 必ず public にする
        //Debug.Log("clicked");
        onclick2 = true;

        ketteiNouhin = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        kettei1 = false;
        onclick = false;

        ketteiNouhin = false;
        onclick2 = false;
    }
}
