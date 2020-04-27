using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matplaceSelectToggle : MonoBehaviour {

    private GameObject canvas;

    public int placeNum; //トグルの番号

    private GameObject getmatplace_panel;
    private GetMatPlace_Panel getmatplace;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //材料採取地パネルの取得
        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel/Comp").gameObject;
        getmatplace = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick_Place()
    {
        getmatplace.OnClick_Place(placeNum);
    }
}
