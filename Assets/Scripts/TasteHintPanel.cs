using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TasteHintPanel : MonoBehaviour {

    private Compound_Main compound_Main;

    private Text Okashi_lasthint_text;
    private int i;

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        SetInit();
    }

    private void SetInit()
    {
        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        Okashi_lasthint_text = this.transform.Find("HintPanel/HintText").GetComponent<Text>();
        Okashi_lasthint_text.text = GameMgr.Okashi_lasthint;
    }

    public void BackOption()
    {

        compound_Main.compound_status = 0;
        this.gameObject.SetActive(false);

    }
}
