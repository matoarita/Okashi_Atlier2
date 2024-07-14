using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestKakuninButtonPanel : MonoBehaviour {

    private TimeController time_controller;
    private QuestSetDataBase questset_database;

    private GameObject Limit_checkmark_obj;
    private GameObject Limit_checkmark_obj2;

    private int _Limit_day;
    private int _Nokori_day;

    private int i, j;
    private int counter;
    private int counter_chouka;

    // Use this for initialization
    void Start () {

        Init_Setting();
    }
	
    void Init_Setting()
    {
        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //クエスト受注データベース取得
        questset_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        Limit_checkmark_obj = this.transform.Find("QuestKakuninButton/TimeLimitMark").gameObject;
        Limit_checkmark_obj.SetActive(false);
        Limit_checkmark_obj2 = this.transform.Find("QuestKakuninButton/TimeLimitMark2").gameObject;
        Limit_checkmark_obj2.SetActive(false);
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

        while (i < questset_database.questTakeset.Count)
        {
            //あと何日
            _Limit_day = time_controller.CullenderKeisanInverse(questset_database.questTakeset[i].Quest_LimitMonth, questset_database.questTakeset[i].Quest_LimitDay);
            _Nokori_day = _Limit_day - PlayerStatus.player_day;

            if (!GameMgr.System_BarQuest_LimitDayON)
            {
            }
            else
            {

                if (_Nokori_day < 0)
                {
                    counter_chouka++;
                }
                else if (_Nokori_day == 0)
                {
                    counter++;                   
                }
                else
                {
                    //まだ余裕がある　なにも表示しない

                }
            }
            i++;
        }

        if (counter >= 1)
        {
            //本日中　ビックリマーク表示
            Limit_checkmark_obj.SetActive(true);
            Limit_checkmark_obj2.SetActive(false);
        }
        else
        {
            if (counter_chouka >= 1)
            {
                //過ぎたものがある 灰色のビックリマークとか？
                Limit_checkmark_obj.SetActive(false);
                Limit_checkmark_obj2.SetActive(true);
            }
        }
    }
}
