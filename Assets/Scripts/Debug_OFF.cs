using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_OFF : MonoBehaviour {

    //デバッグオフボタンは、つけたオブジェクトの「子オブジェクト」を全てオフにする。
    //直接this.SetActiveで自身をオフにしてしまうと、
    //このスクリプト自体Updateも動かなくなってしまうので、そういうルールで運用する。

    private List<GameObject> child_obj = new List<GameObject>();

	// Use this for initialization
	void Start () {

        child_obj.Clear();
        foreach (Transform child in this.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            child_obj.Add(child.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {

		if(GameMgr.DEBUG_MODE)
        {
            On_Myself();
        }
        else
        {
            Off_Myself();
        }

	}

    public void Off_Myself()
    {
        foreach(GameObject child in child_obj)
        {
            child.SetActive(false);
        }
        //this.GetComponent<CanvasGroup>().alpha = 0;
        //this.GetComponent<Button>().interactable = false;

    }

    public void On_Myself()
    {
        foreach (GameObject child in child_obj)
        {
            child.SetActive(true);
        }
        //this.GetComponent<CanvasGroup>().alpha = 1;
        //this.GetComponent<Button>().interactable = true;
    }
}
