using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AAA_AutoSave_Main : MonoBehaviour {

	// Use this for initialization
	void Start () {

        StartCoroutine("WaitResetTitle");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator WaitResetTitle()
    {
        yield return new WaitForSeconds(3.0f);

        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }
}
