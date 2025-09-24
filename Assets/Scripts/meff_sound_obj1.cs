using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meff_sound_obj1 : MonoBehaviour {

    private SoundController sc;

    // Use this for initialization
    void Start () {

        Init_Setting();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {        
        Init_Setting();
    }

    void Init_Setting()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
    }

    public void OnSound01() //光りがとんでいく音
    {
        //Debug.Log("OnSound01()");
        sc.PlaySe(172);
    }

    public void OnSound02() //光りが収束したときの音
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(173);
        sc.PlaySe(174);
        sc.PlaySe(177);
    }

    public void OnSound03() //フルーツのぷにゅ音
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(180);
    }

    public void OnSound20() //凍り付いたときの音
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(175);        
    }

    public void OnSound21() //氷発生の音
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(176);       
    }

    public void OnSound22() //チャージの音
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(177);
        //sc.PlaySe(178);
    }

    public void OnSound23() //ジェラートが凍り付く音
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(185);
        sc.PlaySe(186);
    }

    public void OnSound30() //炎の着火音
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(181);
        sc.PlaySe(183);
    }

    public void OnSound31() //炎の発射音
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(182);
        //sc.PlaySe(183);
    }

    public void OnSound40() //水滴がおちたときのキラーン音
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(248);
        sc.PlaySe(249);
    }

    public void OnSound100() //トッピング時　パパパパと↑から粒が振った時の音
    {
        sc.PlaySe(252);
    }
}
