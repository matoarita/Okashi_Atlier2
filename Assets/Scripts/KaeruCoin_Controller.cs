using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaeruCoin_Controller : MonoBehaviour {

    private GameObject canvas;

    private Text _kaerucoin_text;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        _kaerucoin_text = canvas.transform.Find("KaeruCoin_Panel/KeruCoin_param").GetComponent<Text>();

        ReDrawParam();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        _kaerucoin_text = canvas.transform.Find("KaeruCoin_Panel/KeruCoin_param").GetComponent<Text>();

        ReDrawParam();

    }

    public void ReDrawParam()
    {
        _kaerucoin_text.text = PlayerStatus.player_kaeru_coin.ToString();
    }
}
