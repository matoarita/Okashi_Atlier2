using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CompanyLogoMovie_Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MovieEnd()
    {
        FadeManager.Instance.fadeColor = new Color(1.0f, 1.0f, 1.0f);
        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }
}
