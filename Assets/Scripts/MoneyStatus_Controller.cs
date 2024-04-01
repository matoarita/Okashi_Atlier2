using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyStatus_Controller : SingletonMonoBehaviour<MoneyStatus_Controller>
{

    private SoundController sc;

    private Transform moneyicon_transfrom;
    Vector3 moneypanel_localPos;

    private List<GameObject> _getmoney_obj = new List<GameObject>(); //お金アニメ表示用のゲームオブジェクト

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

    }
	
	// Update is called once per frame
	void Update () {

    }

    //お金が増えた
    public void GetMoney( int _getmoney )
    {
        //アニメ前にまずスタートするときの所持金を先に設定　アニメ中は変わらない
        if (!GameMgr.Money_counterAnim_on)
        {
            GameMgr.Money_StartParam = PlayerStatus.player_money;
        }

        //お金の増減
        PlayerStatus.player_money += _getmoney;

        if(PlayerStatus.player_money >= 999999)
        {
            PlayerStatus.player_money = 999999;
        }

        GameMgr.Money_counterParam = _getmoney;
        GameMgr.Money_counterAnim_on = true;
        GameMgr.Money_counterAnim_StartSetting = true;
    }

    //お金が減った
    public void UseMoney( int _usemoney )
    {
        //アニメ前にまずスタートするときの所持金を先に設定　アニメ中は変わらない
        if (!GameMgr.Money_counterAnim_on)
        {
            GameMgr.Money_StartParam = PlayerStatus.player_money;
        }

        //お金の増減
        PlayerStatus.player_money -= _usemoney;

        if (PlayerStatus.player_money <= 0)
        {
            PlayerStatus.player_money = 0;
        }

        GameMgr.Money_counterParam = -(_usemoney);
        GameMgr.Money_counterAnim_on = true;
        GameMgr.Money_counterAnim_StartSetting = true;
    }

    //表示をすぐに更新
    public void money_Draw()
    {
        
    }
}
