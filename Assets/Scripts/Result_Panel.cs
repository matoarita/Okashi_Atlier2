using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_Panel : MonoBehaviour {

    private Button button;

	// Use this for initialization
	void Start () {

        button = this.transform.Find("Button").GetComponent<Button>();
        button.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {
		
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
