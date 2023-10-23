using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RecipiMemoButton : MonoBehaviour {

    private GameObject canvas;
    private GameObject recipimemoController_obj;

    private GameObject compoBG_A;

    // Use this for initialization
    void Start () {

        canvas = GameObject.FindWithTag("Canvas");

        //コンポBGパネルの取得
        compoBG_A = this.transform.parent.gameObject;
        recipimemoController_obj = compoBG_A.transform.Find("RecipiMemo_ScrollView").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RecipiMemoOn()
    {
        recipimemoController_obj.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
