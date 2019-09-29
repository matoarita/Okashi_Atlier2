using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinStatus_Controller : MonoBehaviour
{

    private GameObject _coin_param;

    private GameObject playeritemlist_obj;

    private Text _coin_text;

    // Use this for initialization
    void Start()
    {
        _coin_param = this.transform.Find("KaeruCoin_param").gameObject;
        _coin_text = _coin_param.GetComponent<Text>();

        playeritemlist_obj = GameObject.FindWithTag("PlayerItemList");

        _coin_text.text = PlayerStatus.player_kaeru_coin.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _coin_text.text = PlayerStatus.player_kaeru_coin.ToString();
    }

}
