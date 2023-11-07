using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back_ShopFirst : MonoBehaviour {
   

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick_Back_ShopFirst()
    {
        switch (GameMgr.Scene_Category_Num)
        {
            case 20:

                GameMgr.Reset_SceneStatus = true;
                break;

            case 30:

                GameMgr.Reset_SceneStatus = true;
                break;          

            case 40:

                GameMgr.Reset_SceneStatus = true;
                break;

            case 50:

                GameMgr.Reset_SceneStatus = true;
                break;

            case 60:

                BackScene();
                break;
        }
                
    }

    void BackScene()
    {
        GameMgr.Scene_back_home = true;

        //メインシーン読み込み
        FadeManager.Instance.LoadScene("Compound", 0.3f);
    }
}
