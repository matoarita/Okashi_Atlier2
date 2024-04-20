using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HirobaBlockText : MonoBehaviour {

    private GameObject ScrollView;
    private GameObject BlockText_obj;
    private Text BlockText;

    // Use this for initialization
    void Start () {

    }

    void InitSetting()
    {
        ScrollView = this.transform.parent.parent.parent.gameObject; //自分が今どこのViewにいるか
        BlockText_obj = this.transform.Find("Background/BlockText").gameObject;
        BlockText = BlockText_obj.GetComponent<Text>();

        BlockCheck();
    }

    void BlockCheck()
    {
        switch(ScrollView.name)
        {
            case "MainList_ScrollView_04":

                if(this.gameObject.name == "NPC4_SelectToggle")
                {
                    //ハートレベルで通れない箇所のチェック
                    if (PlayerStatus.girl1_Love_lv < 10)
                    {
                        BlockText_obj.SetActive(true);
                        BlockText.text = "ハートLVが" + "\n" + "10" + "必要";
                    }
                    else
                    {
                        BlockText_obj.SetActive(false);
                    }
                }
                break;
        }
    }

    private void OnEnable()
    {
        InitSetting();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
