using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_ShopFirst : MonoBehaviour {

    private GameObject shop_Main_obj;
    private Shop_Main shop_Main;

    private GameObject farm_Main_obj;
    private Farm_Main farm_Main;

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick_Back_ShopFirst()
    {
        shop_Main_obj = GameObject.FindWithTag("Shop_Main");
        shop_Main = shop_Main_obj.GetComponent<Shop_Main>();

        shop_Main.shop_status = 0;
    }

    public void OnClick_Back_FarmFirst()
    {
        farm_Main_obj = GameObject.FindWithTag("Farm_Main");
        farm_Main = farm_Main_obj.GetComponent<Farm_Main>();

        farm_Main.farm_status = 0;
    }
}
