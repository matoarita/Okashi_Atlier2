using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MagicStartPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCancel_MagicSelect()
    {
        if (GameMgr.compound_select == 9) //ヒカリ作り中の場合は、ヒカリの選択画面に戻る
        {
            GameMgr.compound_status = 8;
        }
        else
        {
            GameMgr.compound_status = 6;
        }

        this.gameObject.SetActive(false);
    }
}
