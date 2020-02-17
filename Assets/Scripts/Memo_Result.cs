using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memo_Result : MonoBehaviour {

    private GameObject canvas;
    private GameObject recipimemoController_obj;

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


        recipimemoController_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiMemo_ScrollView").gameObject;
        recipimemoController_obj.SetActive(false);

        //テキストエリアの読み込み
        _text = this.transform.Find("Viewport/Content/Text").gameObject.GetComponent<Text>();

        //メモのデータの読み込み
        Set_text();

        _text.text = text_recipi_memo;
    }

    public void SeteventID(int _ev_id )
    {
        event_ID = _ev_id;
    }

    void Set_text()
    {
        //いべんとアイテムDBのIDと一致する。
        switch(event_ID)
        {
            case 1:

                text_recipi_memo = "小麦粉と砂糖とバター" + "\n" + "\n" + "2 : 1 : 1";
                break;

            default:
                break;

        }
    }

    public void CloseMemo()
    {
        recipimemoController_obj.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
