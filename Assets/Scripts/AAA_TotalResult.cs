using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AAA_TotalResult : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnNextButton()
    {
        FadeManager.Instance.LoadScene("120_AutoSave", 0.3f);
    }
}
