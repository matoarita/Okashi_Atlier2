using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back_ShopFirst : MonoBehaviour {

    private GameObject shop_Main_obj;
    private Shop_Main shop_Main;
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
        switch (SceneManager.GetActiveScene().name)
        {
            case "Bar":

                shop_Main_obj = GameObject.FindWithTag("Shop_Main");
                bar_Main = shop_Main_obj.GetComponent<Bar_Main>();

                bar_Main.shop_status = 0;
                break;

            case "Shop":

                shop_Main_obj = GameObject.FindWithTag("Shop_Main");
                shop_Main = shop_Main_obj.GetComponent<Shop_Main>();

                shop_Main.shop_status = 0;
                break;

            case "Farm":

                farm_Main_obj = GameObject.FindWithTag("Farm_Main");
                farm_Main = farm_Main_obj.GetComponent<Farm_Main>();

                farm_Main.farm_status = 0;
                break;

            case "Emerald_Shop":

                emeraldshop_Main_obj = GameObject.FindWithTag("EmeraldShop_Main");
                emeraldshop_Main = emeraldshop_Main_obj.GetComponent<EmeraldShop_Main>();

                emeraldshop_Main.shop_status = 0;
                break;
        }
                
    }

}
