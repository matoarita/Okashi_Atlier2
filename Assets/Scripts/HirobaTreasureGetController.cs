using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HirobaTreasureGetController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EventReadingStart()
    {
        StartCoroutine("EventReading");
    }

    IEnumerator EventReading()
    {
        GameMgr.hiroba_treasureget_flag = true;
        GameMgr.scenario_ON = true;

        GameMgr.Scene_Select = 1000; //シナリオイベント読み中の状態
        GameMgr.Scene_Status = 1000;

        //Debug.Log("宝箱イベント　読み中");

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;
        GameMgr.scenario_ON = false;

        GameMgr.Scene_Select = 0; //何もしていない状態
        GameMgr.Scene_Status = 0;

        //読み終わったら、またウィンドウなどを元に戻す。
        //GameMgr.Scene_Status=0で、hirobaMainControllerのほうで自動で戻る
    }
}
