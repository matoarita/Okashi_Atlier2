using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaeruCoin_Controller : MonoBehaviour
{

    private GameObject canvas;

    private Text _emeraldongri_text;
    private Text _sapphiredongri_text;

    private PlayerItemList pitemlist;

    private int emeraldonguriID;
    private int kaerucoin;

    private Transform moneyicon_transfrom;
    private Transform moneyicon2_transfrom;

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
    private int _dongriType;
    

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

        _getmoneyPrefab = (GameObject)Resources.Load("Prefabs/GetmoneyObj_text");

        _emeraldongri_text = canvas.transform.Find("KaeruCoin_Panel/EmeralDongriPanel/KeruCoin_param").GetComponent<Text>();
        _sapphiredongri_text = canvas.transform.Find("KaeruCoin_Panel/SapphireDongriPanel/KeruCoin_param").GetComponent<Text>();
       
        moneyicon_transfrom = this.transform.Find("EmeralDongriPanel").transform;
        moneyicon2_transfrom = this.transform.Find("SapphireDongriPanel").transform;
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

            switch (_dongriType)
            {
                case 0:

                    if (_moneymax <= 0)
                    {
                        _emeraldongri_text.text = _result_kaerucoin.ToString();
                        moneyanim_on = false;
                    }
                    else
                    {
                        _emeraldongri_text.text = _before_kaerucoin.ToString();
                    }
                    break;

                case 1:

                    if (_moneymax <= 0)
                    {
                        _sapphiredongri_text.text = _result_kaerucoin.ToString();
                        moneyanim_on = false;
                    }
                    else
                    {
                        _sapphiredongri_text.text = _before_kaerucoin.ToString();
                    }
                    break;
            }
            

            //時間減少
            timeOut -= Time.deltaTime;
            
        }
    }

    //数字を更新
    public void ReDrawParam()
    {
        InitParam();

        //エメラルどんぐり所持数
        emeraldonguriID = pitemlist.SearchItemString("emeralDongri");
        kaerucoin = pitemlist.playeritemlist[emeraldonguriID];

        _emeraldongri_text.text = kaerucoin.ToString();

        //サファイアどんぐり所持数
        emeraldonguriID = pitemlist.SearchItemString("sapphireDongri");
        kaerucoin = pitemlist.playeritemlist[emeraldonguriID];

        _sapphiredongri_text.text = kaerucoin.ToString();
    }

    //減った  エメラルどんぐり
    public void UseCoin(int _usemoney)
    {
        InitParam();

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

        _dongriType = 0;
    }

    //減った  サファイアどんぐり
    public void UseCoin2(int _usemoney)
    {
        InitParam();

        _getmoney_obj.Add(Instantiate(_getmoneyPrefab, moneyicon2_transfrom.transform));
        list_size = _getmoney_obj.Count;
        _getmoney_text = _getmoney_obj[list_size - 1].GetComponent<Text>();


        _getmoney_text.text = "-" + _usemoney.ToString() + "G";

        emeraldonguriID = pitemlist.SearchItemString("sapphireDongri");
        kaerucoin = pitemlist.playeritemlist[emeraldonguriID];

        //お金の増減
        _before_kaerucoin = kaerucoin;
        _result_kaerucoin = kaerucoin - _usemoney;
        pitemlist.playeritemlist[emeraldonguriID] -= _usemoney;

        timeOut = 0.1f;

        zougen_sw = 1; //減る処理
        _moneymax = _usemoney;
        moneyanim_on = true;

        _dongriType = 1;
    }
}
