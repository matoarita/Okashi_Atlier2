using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyStatus_Controller : MonoBehaviour {

    private SoundController sc;

    private GameObject _money_param;
    private Text _money_text;

    private Text _coin_cullency;

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

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        _money_param = this.transform.Find("Money_param").gameObject;
        _money_text = _money_param.GetComponent<Text>();

        _getmoneyPrefab = (GameObject)Resources.Load("Prefabs/GetmoneyObj_text");

        moneyicon_transfrom = this.transform;
        //moneypanel_localPos = moneyicon_transfrom.localPosition;
        //Debug.Log("moneypanel_localPos: " + moneypanel_localPos);

        _money_text.text = PlayerStatus.player_money.ToString();

        _coin_cullency = this.transform.Find("Money_text").GetComponent<Text>();
        _coin_cullency.text = GameMgr.MoneyCurrencyEn;

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
                //アニメ中は、音がチャリチャリ鳴り続ける。ゼルダのルピー音
                sc.PlaySe(29);
                timeOut = 0.12f;
            }
        }
    }

    private void OnEnable()
    {
        _money_param = this.transform.Find("Money_param").gameObject;
        _money_text = _money_param.GetComponent<Text>();

        _money_text.text = PlayerStatus.player_money.ToString();
        moneyanim_on = false;
    }

    //お金が増えた
    public void GetMoney( int _getmoney )
    {
        _getmoney_obj.Add(Instantiate(_getmoneyPrefab, moneyicon_transfrom.transform));
        list_size = _getmoney_obj.Count;
        _getmoney_text = _getmoney_obj[list_size - 1].GetComponent<Text>();


        _getmoney_text.text = "+" + _getmoney.ToString();


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


        _getmoney_text.text = "-" + _usemoney.ToString();


        //お金の増減
        _before_pmoney = PlayerStatus.player_money;
        PlayerStatus.player_money -= _usemoney;

        timeOut = 0.1f;

        zougen_sw = 1; //減る処理
        _moneymax = _usemoney;
        moneyanim_on = true;
    }

    //表示をすぐに更新
    public void money_Draw()
    {
        _money_text.text = PlayerStatus.player_money.ToString();
    }
}
