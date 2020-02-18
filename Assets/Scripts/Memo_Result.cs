using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memo_Result : MonoBehaviour {

    private GameObject canvas;
    private GameObject recipimemoController_obj;

    private PlayerItemList pitemlist;

    private int event_ID;

    private Text _text;
    private string text_recipi_memo;

	// Use this for initialization
	void Start () {

        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        recipimemoController_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiMemo_ScrollView").gameObject;
        recipimemoController_obj.SetActive(false);

        //テキストエリアの読み込み
        _text = this.transform.Find("Viewport/Content/Text").gameObject.GetComponent<Text>();

        //メモのデータの読み込み
        text_recipi_memo = pitemlist.eventitemlist[event_ID].ev_memo;

        _text.text = text_recipi_memo;

        //チュートリアル時
        if (GameMgr.tutorial_ON == true)
        {
            if (GameMgr.tutorial_Num == 30)
            {
                GameMgr.tutorial_Progress = true;
                GameMgr.tutorial_Num = 40;
            }
        }
    }

    public void SeteventID(int _ev_id )
    {
        event_ID = _ev_id;
    }

    public void CloseMemo()
    {
        recipimemoController_obj.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
