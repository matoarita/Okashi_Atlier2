using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTitlePanel : MonoBehaviour {

    private SoundController sc;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
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

        this.gameObject.SetActive(false);
    }
}
