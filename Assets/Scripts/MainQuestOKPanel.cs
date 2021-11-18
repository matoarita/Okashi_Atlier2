using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainQuestOKPanel : MonoBehaviour {

    private Button button;

    private Text stagenum_text;
    private Image okashiImage;

    private GirlEat_Judge girlEat_judge;
    private Special_Quest special_quest;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        button = this.transform.Find("Button").GetComponent<Button>();
        button.interactable = false;

        stagenum_text = this.transform.Find("QuestPanel/QuestClear/stageNumberText").GetComponent<Text>();
        stagenum_text.text = GameMgr.stage_quest_num.ToString();

        okashiImage = this.transform.Find("ItemImgPanel/ItemImg").GetComponent<Image>();
        okashiImage.sprite = special_quest.OkashiQuest_sprite;

        StartCoroutine("WaitButton");
    }

    IEnumerator WaitButton()
    {

        yield return new WaitForSeconds(1.0f); //1~2秒まったら、ボタンがおせるようになる。連打防止。
        button.interactable = true;
    }

    public void MainQuestOKButtonON()
    {
        girlEat_judge.PanelResultOFF();
    }
}
