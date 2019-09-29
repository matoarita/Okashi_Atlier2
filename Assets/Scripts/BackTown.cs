using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTown : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickToTown()
    {
        //SceneManager.LoadScene("Hiroba");
        FadeManager.Instance.LoadScene("Hiroba", 0.3f);
    }
}
