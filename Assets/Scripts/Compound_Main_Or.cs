using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;
using DG.Tweening;

public class Compound_Main_Or : MonoBehaviour {

    private void Awake()
    {
        //メインオブジェクト　シーンの読み込み。
        SceneManager.LoadSceneAsync("Hikari_CompMain", LoadSceneMode.Additive); //
    }

    // Use this for initialization
    void Start () {

        //今いるシーン番号を指定
        //GameMgr.Scene_Category_Num = 10;
        //Debug.Log("(GameMgr.Scene_Category_Num): " + GameMgr.Scene_Category_Num); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}


}
