using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTitlePanel : MonoBehaviour {

    private SoundController sc;
    private Compound_Main compound_Main;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //音ならす
        sc.PlaySe(25);

        StartCoroutine("WaitSeconds");
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(2.0f);

        GameMgr.KeyInputOff_flag = true; //キー入力受付開始
        compound_Main.FlagEvent(); //イベント出現
        this.gameObject.SetActive(false);
    }
}
