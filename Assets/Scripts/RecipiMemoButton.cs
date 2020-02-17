using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RecipiMemoButton : MonoBehaviour {

    private GameObject canvas;
    private GameObject recipimemoController_obj;

    // Use this for initialization
    void Start () {

        canvas = GameObject.FindWithTag("Canvas");
        recipimemoController_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiMemo_ScrollView").gameObject;
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
