using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NinkiStatus_Controller : SingletonMonoBehaviour<NinkiStatus_Controller>
{ 

    private SoundController sc;

    private List<GameObject> _getmoney_obj = new List<GameObject>(); //お金アニメ表示用のゲームオブジェクト

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    //人気が増えたor減った　アニメなし
    public void GetNinki( int _getninki )
    {
        PlayerStatus.player_ninki_param += _getninki;

        if(PlayerStatus.player_ninki_param >= 9999)
        {
            PlayerStatus.player_ninki_param = 9999;
        }

        if (PlayerStatus.player_ninki_param <= 0)
        {
            PlayerStatus.player_ninki_param = 0;
        }
    }

    //人気が減った
    /*public void DegNinki( int _degninki)
    {
        PlayerStatus.player_ninki_param -= _degninki;
    }*/

}
