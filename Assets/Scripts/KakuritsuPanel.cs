using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KakuritsuPanel : MonoBehaviour {

    private Text srate_hyouji;

	// Use this for initialization
	void Start () {

        srate_hyouji = this.transform.Find("Image/Kakuritsu_param").gameObject.GetComponent<Text>();
        srate_hyouji.text = "-";

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void KakuritsuYosoku_Img( int _srate )
    {
        srate_hyouji.text = _srate.ToString();
    }

    public void KakuritsuYosoku_Reset()
    {
        srate_hyouji.text = "-";
    }
}
