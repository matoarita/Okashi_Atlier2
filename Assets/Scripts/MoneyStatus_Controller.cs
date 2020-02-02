using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyStatus_Controller : MonoBehaviour {

    private GameObject _money_param;

    private Text _money_text;

    private Transform moneyicon_transfrom;
    Vector3 moneypanel_localPos;

    private List<GameObject> _getmoney_obj = new List<GameObject>(); //お金アニメ表示用のゲームオブジェクト

    private GameObject _getmoneyPrefab;
    private Text _getmoney_text;

    private int _before_pmoney; //増減前のプレイヤーのお金
    private int zougen_sw;
    private int _moneymax;
    private bool moneyanim_on;
    private int _deg;

    private int list_size;

    private float timeOut;

    //SEを鳴らす
    public AudioClip sound1;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {

        //音の取得
        audioSource = GetComponent<AudioSource>();

        _money_param = this.transform.Find("Money_param").gameObject;
        _money_text = _money_param.GetComponent<Text>();

        _getmoneyPrefab = (GameObject)Resources.Load("Prefabs/GetmoneyObj_text");

        moneyicon_transfrom = this.transform;
        //moneypanel_localPos = moneyicon_transfrom.localPosition;
        //Debug.Log("moneypanel_localPos: " + moneypanel_localPos);

        _money_text.text = PlayerStatus.player_money.ToString();

        _deg = 1;
        moneyanim_on = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (moneyanim_on == true)
        {
            if ( zougen_sw == 0 )
            {
                _before_pmoney++;
            }
            else if (zougen_sw == 1)
            {
                _before_pmoney--;
            }        

            _moneymax -= _deg;
            
            if ( _moneymax <= 0 )
            {
                _money_text.text = PlayerStatus.player_money.ToString();
                moneyanim_on = false;
            }
            else
            {
                _money_text.text = _before_pmoney.ToString();
            }

            //時間減少
            timeOut -= Time.deltaTime;

            if (timeOut <= 0.0)
            {
                timeOut = 0.1f;
                //音を鳴らす
                //audioSource.PlayOneShot(sound1);
            }
        }
    }

    //お金が増えた
    public void GetMoney( int _getmoney )
    {
        _getmoney_obj.Add(Instantiate(_getmoneyPrefab, moneyicon_transfrom.transform));
        list_size = _getmoney_obj.Count;
        _getmoney_text = _getmoney_obj[list_size - 1].GetComponent<Text>();


        _getmoney_text.text = "+" + _getmoney.ToString() + "G";


        //お金の増減
        _before_pmoney = PlayerStatus.player_money;
        PlayerStatus.player_money += _getmoney;

        timeOut = 0.1f;

        zougen_sw = 0; //増える処理
        _moneymax = _getmoney;
        moneyanim_on = true;
    }

    //お金が減った
    public void UseMoney( int _usemoney )
    {
        _getmoney_obj.Add(Instantiate(_getmoneyPrefab, moneyicon_transfrom.transform));
        list_size = _getmoney_obj.Count;
        _getmoney_text = _getmoney_obj[list_size - 1].GetComponent<Text>();


        _getmoney_text.text = "-" + _usemoney.ToString() + "G";


        //お金の増減
        _before_pmoney = PlayerStatus.player_money;
        PlayerStatus.player_money -= _usemoney;

        timeOut = 0.1f;

        zougen_sw = 1; //減る処理
        _moneymax = _usemoney;
        moneyanim_on = true;
    }

}
