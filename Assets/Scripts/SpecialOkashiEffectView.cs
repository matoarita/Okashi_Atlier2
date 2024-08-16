using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialOkashiEffectView : MonoBehaviour {

    private Exp_Controller exp_Controller;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //調合完了後、ボタンを押すと呼び出される。
    public void CompoundResult_Button()
    {
        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        GameMgr.Special_OkashiEnshutsuFlag = false;
        exp_Controller.EffectListClear();
        GameMgr.CompoundSceneStartON = false; //調合シーン終了
        GameMgr.compound_status = 0;
    }
}
