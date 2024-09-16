using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KakuritsuPanel : MonoBehaviour {

    private Text srate_hyouji;

	// Use this for initialization
	void Start () {

        srate_hyouji = this.transform.Find("Image/Kakuritsu_param").gameObject.GetComponent<Text>();
        //srate_hyouji.text = "-";

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        srate_hyouji = this.transform.Find("Image/Kakuritsu_param").gameObject.GetComponent<Text>();
        //srate_hyouji.text = "-";
    }

    public void KakuritsuYosoku_Img( float _srate )
    {
        srate_hyouji = this.transform.Find("Image/Kakuritsu_param").gameObject.GetComponent<Text>();
        srate_hyouji.text = _srate.ToString("f1");
    }

    public void KakuritsuYosoku_HatenaImg()
    {
        srate_hyouji = this.transform.Find("Image/Kakuritsu_param").gameObject.GetComponent<Text>();
        srate_hyouji.text = "??.?";
    }

    public void KakuritsuYosoku_Reset()
    {
        srate_hyouji = this.transform.Find("Image/Kakuritsu_param").gameObject.GetComponent<Text>();
        srate_hyouji.text = "-";
    }

    
}
