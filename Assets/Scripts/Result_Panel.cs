using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_Panel : MonoBehaviour {

    private Button button;

    private GirlEat_Judge girlEat_judge;

    private SoundController sc;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        button = this.transform.Find("Button").GetComponent<Button>();
        button.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {

        /*
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Enter");
            girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();
            girlEat_judge.ResultPanel_On();            
        }*/
    }

    private void OnEnable()
    {
        button = this.transform.Find("Button").GetComponent<Button>();
        button.interactable = false;

        StartCoroutine("WaitButton");
    }

    IEnumerator WaitButton()
    {
        yield return new WaitForSeconds(1.0f); //1~2秒まったら、ボタンがおせるようになる。連打防止。

        button.interactable = true;
    }
}
