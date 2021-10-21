using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_Main : MonoBehaviour {

    private SaveController save_controller;

    // Use this for initialization
    void Start () {

        save_controller = SaveController.Instance.GetComponent<SaveController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TitleBackButton()
    {
        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }

    public void LoadButton()
    {        
        save_controller.OnLoadMethod();

        GameMgr.GameLoadOn = true; //順番が大事。ロードより後にこっちはtrueにしとく。
        //FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("Compound", 0.3f);
    }
}
