using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CompanyLogoMovie_Main : MonoBehaviour {

    private float timeOut;

	// Use this for initialization
	void Start () {

        timeOut = 1.5f;

    }
	
	// Update is called once per frame
	void Update () {
        timeOut -= Time.deltaTime;
    }

    public void MovieEnd()
    {
        FadeManager.Instance.fadeColor = new Color(1.0f, 1.0f, 1.0f);
        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }

    public void SkipMovie()
    {
        //Debug.Log("cullentstate: " + timeOut);
        if (timeOut <= 0.0f)
        {

            this.GetComponent<Animator>().enabled = false;
            MovieEnd();
        }
    }
}
