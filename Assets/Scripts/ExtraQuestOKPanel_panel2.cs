using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExtraQuestOKPanel_panel2 : MonoBehaviour {

    private Button button1;
    private Button button2;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private Girl1_status girl1_status;
    private SoundController sc;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

    }

    public void AnimSound()
    {
        //sc.PlaySe(84); //宝箱が落ちたときの音
    }
}
