using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back_ShopFirst : MonoBehaviour {

    private GameObject shop_Main_obj;
    private Shop_Main shop_Main;
    private GameObject bar_Main_obj;
    private Bar_Main bar_Main;

    private GameObject farm_Main_obj;
    private Farm_Main farm_Main;

    private GameObject emeraldshop_Main_obj;
    private EmeraldShop_Main emeraldshop_Main;

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

                shop_Main_obj = GameObject.FindWithTag("Shop_Main");
                shop_Main = shop_Main_obj.GetComponent<Shop_Main>();

                shop_Main.shop_status = 0;
                break;

            case 30:

                bar_Main_obj = GameObject.FindWithTag("Bar_Main");
                bar_Main = bar_Main_obj.GetComponent<Bar_Main>();

                bar_Main.bar_status = 0;
                break;          

            case 40:

                farm_Main_obj = GameObject.FindWithTag("Farm_Main");
                farm_Main = farm_Main_obj.GetComponent<Farm_Main>();

                farm_Main.farm_status = 0;
                break;

            case 50:

                emeraldshop_Main_obj = GameObject.FindWithTag("EmeraldShop_Main");
                emeraldshop_Main = emeraldshop_Main_obj.GetComponent<EmeraldShop_Main>();

                emeraldshop_Main.shop_status = 0;
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
