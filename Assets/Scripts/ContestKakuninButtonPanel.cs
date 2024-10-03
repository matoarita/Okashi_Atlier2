using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContestKakuninButtonPanel : MonoBehaviour {

    private TimeController time_controller;

    private GameObject Limit_checkmark_obj;
    private GameObject Limit_checkmark_obj2;
    private GameObject Limit_checkmark_obj3;

    private int _Limit_day;
    private int _Nokori_day;

    private int i, j;
    private int counter;
    private int counter_chouka;

    private GameObject text_day_obj;
    private Text text_day;

    // Use this for initialization
    void Start () {

        Init_Setting();
    }
	
    void Init_Setting()
    {
        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        Limit_checkmark_obj = this.transform.Find("ContestKakuninButton/TimeLimitMark").gameObject;
        Limit_checkmark_obj.SetActive(false);
        Limit_checkmark_obj2 = this.transform.Find("ContestKakuninButton/TimeLimitMark2").gameObject;
        Limit_checkmark_obj2.SetActive(false);
        Limit_checkmark_obj3 = this.transform.Find("ContestKakuninButton/TimeLimitMark3").gameObject;
        Limit_checkmark_obj3.SetActive(false);
        text_day_obj = this.transform.Find("ContestKakuninButton/Text_day").gameObject;
        text_day_obj.SetActive(false);
        text_day = text_day_obj.GetComponent<Text>();

        if (!GameMgr.System_ContestIcon_OnFlag)
        {
            this.transform.Find("ContestKakuninButton").gameObject.SetActive(false);
        }
        
    } 

	// Update is called once per frame
	void Update () {
		
	}

    //Compound_Mainから読み出し
    public void Check_LimitMarkDraw()
    {
        Init_Setting();

        i = 0;
        counter = 0;
        counter_chouka = 0;

        if (GameMgr.contest_accepted_list.Count > 0)
        {
            //あと何日
            /*Debug.Log("GameMgr.contest_accepted_list[0].Month: " + GameMgr.contest_accepted_list[0].Month +
                " GameMgr.contest_accepted_list[0].Day: " + GameMgr.contest_accepted_list[0].Day);*/
            _Limit_day = time_controller.CullenderKeisanInverse(GameMgr.contest_accepted_list[0].Month, GameMgr.contest_accepted_list[0].Day);
            _Nokori_day = _Limit_day - PlayerStatus.player_day;

            if (_Nokori_day == 0)
            {
                //コンテスト当日　ビックリマーク表示
                Limit_checkmark_obj.SetActive(true);
                Limit_checkmark_obj2.SetActive(false);
                Limit_checkmark_obj3.SetActive(false);
            }
            else if (_Nokori_day > 0)
            {
                Limit_checkmark_obj.SetActive(false);
                Limit_checkmark_obj2.SetActive(false);
                Limit_checkmark_obj3.SetActive(true);
            }
            else //超過している場合
            {
                Limit_checkmark_obj.SetActive(false);
                Limit_checkmark_obj2.SetActive(true);
                Limit_checkmark_obj3.SetActive(false);
            }

            text_day_obj.SetActive(true);
            text_day.text = " 出場日: " + GameMgr.contest_accepted_list[0].Month.ToString() + "/" + GameMgr.contest_accepted_list[0].Day.ToString();
        }
        else
        {
            text_day_obj.SetActive(false);
        }
    }
}
