using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaeruCoin_Controller : MonoBehaviour
{

    private GameObject canvas;

    private Text _kaerucoin_text;

    private PlayerItemList pitemlist;

    private int emeraldonguriID;
    private int kaerucoin;

    private Transform moneyicon_transfrom;

    private GameObject _getmoneyPrefab;
    private List<GameObject> _getmoney_obj = new List<GameObject>(); //お金アニメ表示用のゲームオブジェクト
    private Text _getmoney_text;

    private int list_size;
    private int _before_kaerucoin;
    private int _result_kaerucoin;
    private int _moneymax;

    private float timeOut;

    private bool moneyanim_on;
    private int zougen_sw;
    

    // Use this for initialization
    void Start()
    {
        InitParam();

        ReDrawParam();
    }

    void InitParam()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        _kaerucoin_text = canvas.transform.Find("KaeruCoin_Panel/KeruCoin_param").GetComponent<Text>();

        _getmoneyPrefab = (GameObject)Resources.Load("Prefabs/GetmoneyObj_text");

        moneyicon_transfrom = this.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (moneyanim_on == true)
        {
            if (timeOut <= 0.0)
            {                               
                timeOut = 0.1f;
            }

            if (zougen_sw == 1)
            {
                _before_kaerucoin--;
                _moneymax--;
            }


            if (_moneymax <= 0)
            {
                _kaerucoin_text.text = _result_kaerucoin.ToString();
                moneyanim_on = false;
            }
            else
            {
                _kaerucoin_text.text = _before_kaerucoin.ToString();
            }

            //時間減少
            timeOut -= Time.deltaTime;
            
        }
    }

    //すぐに数字を更新
    public void ReDrawParam()
    {
        emeraldonguriID = pitemlist.SearchItemString("emeralDongri");
        kaerucoin = pitemlist.playeritemlist[emeraldonguriID];

        _kaerucoin_text.text = kaerucoin.ToString();
    }

    //減った
    public void UseCoin(int _usemoney)
    {
        _getmoney_obj.Add(Instantiate(_getmoneyPrefab, moneyicon_transfrom.transform));
        list_size = _getmoney_obj.Count;
        _getmoney_text = _getmoney_obj[list_size - 1].GetComponent<Text>();


        _getmoney_text.text = "-" + _usemoney.ToString() + "G";

        emeraldonguriID = pitemlist.SearchItemString("emeralDongri");
        kaerucoin = pitemlist.playeritemlist[emeraldonguriID];

        //お金の増減
        _before_kaerucoin = kaerucoin;
        _result_kaerucoin = kaerucoin - _usemoney;
        pitemlist.playeritemlist[emeraldonguriID] -= _usemoney;

        timeOut = 0.1f;

        zougen_sw = 1; //減る処理
        _moneymax = _usemoney;
        moneyanim_on = true;
    }

}
