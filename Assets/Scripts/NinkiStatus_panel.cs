using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NinkiStatus_panel : MonoBehaviour {

    private GameObject _ninki_param;
    private Text _ninki_text;

    private Transform moneyicon_transfrom;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        DrawNinki();

    }

    void DrawNinki()
    {
        //例外処理　お金の表記が-とかになってたら、0に戻す。
        if (PlayerStatus.player_ninki_param < 0)
        {
            PlayerStatus.player_ninki_param = 0;
        }
        _ninki_text.text = PlayerStatus.player_ninki_param.ToString();

        
    }

    private void OnEnable()
    {
        _ninki_param = this.transform.Find("Ninki_param").gameObject;
        _ninki_text = _ninki_param.GetComponent<Text>();

        _ninki_text.text = PlayerStatus.player_ninki_param.ToString();

    }
}
