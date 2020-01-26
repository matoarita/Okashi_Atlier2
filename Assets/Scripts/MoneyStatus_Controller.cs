using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyStatus_Controller : MonoBehaviour {

    private GameObject _money_param;

    private Text _money_text;

    // Use this for initialization
    void Start () {
        _money_param = this.transform.Find("Money_param").gameObject;
        _money_text = _money_param.GetComponent<Text>();

        _money_text.text = PlayerStatus.player_money.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        _money_text.text = PlayerStatus.player_money.ToString();
    }

}
