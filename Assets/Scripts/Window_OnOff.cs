using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Window_OnOff : MonoBehaviour {

    private GameObject shopitemlist_onoff;

    // Use this for initialization
    void Start () {

        //ショップリスト画面を開く。初期設定で最初はOFF。
        shopitemlist_onoff = GameObject.FindWithTag("ShopitemList_ScrollView");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnWindow_OnOff()
    {
        if(shopitemlist_onoff.activeInHierarchy == true)
        {
            shopitemlist_onoff.SetActive(false);
        }
        else
        {
            shopitemlist_onoff.SetActive(true);
        }
    }
}
