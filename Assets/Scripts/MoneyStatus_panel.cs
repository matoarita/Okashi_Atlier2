using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyStatus_panel : MonoBehaviour {

    private SoundController sc;

    private GameObject _money_param;
    private Text _money_text;

    private Text _coin_cullency;

    private Transform moneyicon_transfrom;
    Vector3 moneypanel_localPos;

    private List<GameObject> _getmoney_obj = new List<GameObject>(); //お金アニメ表示用のゲームオブジェクト

    private GameObject _getmoneyPrefab;
    private Text _getmoney_text;

    private int _counter_pmoney; //増減中のプレイヤーのお金の表記
    private bool start_flag;
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

        DrawMoney();
        

        _deg = 1;
        GameMgr.Money_counterAnim_on = false;
        start_flag = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (GameMgr.Money_counterOnly)
        {
            GameMgr.Money_counterOnly = false;

            CounterOnTextOBJ(GameMgr.Money_counterParam);
        }

        if (GameMgr.Money_counterAnim_on == true)
        {
            if (GameMgr.Money_counterAnim_StartSetting)
            {
                GameMgr.Money_counterAnim_StartSetting = false;

                StartSetting();
            }

            if (_counter_pmoney > PlayerStatus.player_money )
            {
                _counter_pmoney -= _deg;

                if(_counter_pmoney <= PlayerStatus.player_money) //等しくなった、もしくは超えてしまったとき　アニメが終わる
                {
                    _counter_pmoney = PlayerStatus.player_money;
                    GameMgr.Money_counterAnim_on = false;
                }
            }
            else if (_counter_pmoney < PlayerStatus.player_money)
            {
                _counter_pmoney += _deg;

                if (_counter_pmoney >= PlayerStatus.player_money) //等しくなった、もしくは超えてしまったとき　アニメが終わる
                {
                    _counter_pmoney = PlayerStatus.player_money;
                    GameMgr.Money_counterAnim_on = false;
                }
            } 
            else //ちょうど等しい場合
            {
                _counter_pmoney = PlayerStatus.player_money;
                GameMgr.Money_counterAnim_on = false;
            }

            //表記を更新
            _money_text.text = _counter_pmoney.ToString();

            //アニメが終了したタイミングでリセットする
            if (!GameMgr.Money_counterAnim_on)
            {
                start_flag = false;
            }

            if (_counter_pmoney >= 999999)
            {
                _money_text.text = "999999";
            }
            else if (_counter_pmoney <= 0)
            {
                _money_text.text = "0";
            }

            //時間減少
            timeOut -= Time.deltaTime;

            if (timeOut <= 0.0)
            {
                //アニメ中は、音がチャリチャリ鳴り続ける。ゼルダのルピー音
                sc.PlaySe(29);
                timeOut = 0.12f;
            }
        } else
        {
            DrawMoney();
        }
    }

    void DrawMoney()
    {
        //例外処理　お金の表記が-とかになってたら、0に戻す。
        if (PlayerStatus.player_money < 0)
        {
            PlayerStatus.player_money = 0;
        }
        _money_text.text = PlayerStatus.player_money.ToString();

        _coin_cullency = this.transform.Find("Money_text").GetComponent<Text>();
        _coin_cullency.text = GameMgr.MoneyCurrencyEn;
    }

    private void OnEnable()
    {
        _money_param = this.transform.Find("Money_param").gameObject;
        _money_text = _money_param.GetComponent<Text>();

        _money_text.text = PlayerStatus.player_money.ToString();
        
    }

    void StartSetting()
    {
        CounterOnTextOBJ(GameMgr.Money_counterParam);

        if(start_flag) //アニメ中にさらに更新があった場合　特になにもしない
        {

        }
        else //アニメ最初の場合のみ
        {
            //お金　アニメ前のスタート値
            _counter_pmoney = GameMgr.Money_StartParam;
            start_flag = true;
        }
        
        timeOut = 0.1f; //お金の更新間隔
    }

    void CounterOnTextOBJ(int _money)
    {
        _getmoney_obj.Add(Instantiate(_getmoneyPrefab, moneyicon_transfrom.transform));
        list_size = _getmoney_obj.Count;
        _getmoney_text = _getmoney_obj[list_size - 1].GetComponent<Text>();

        if (_money < 0)
        {
            _getmoney_text.text = "-" + _money.ToString();
        }
        else
        {
            _getmoney_text.text = "+" + _money.ToString();
        }
    }

    //表示をすぐに更新
    public void money_Draw()
    {
        _money_text.text = PlayerStatus.player_money.ToString();
    }
}
