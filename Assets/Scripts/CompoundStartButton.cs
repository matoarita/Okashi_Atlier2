using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundStartButton : MonoBehaviour {

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト
    private SoundController sc;
    private keyManager keymanager;

    public bool compofinal_flag;

    // Use this for initialization
    void Start () {
        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //キー入力受付コントローラーの取得
        keymanager = keyManager.Instance.GetComponent<keyManager>();

        compofinal_flag = false;
    }
	
	// Update is called once per frame
	void Update () {

		if(compofinal_flag)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {

                compofinal_flag = false;

                //Debug.Log("Enter");
                selectitem_kettei.onclick = true;

                selectitem_kettei.kettei3 = true;
                sc.PlaySe(46);
            }
        }
	}

    public void OnCompoundStart()
    {
        compofinal_flag = false;
        selectitem_kettei.onclick = true;

        selectitem_kettei.kettei3 = true;
    }
}
