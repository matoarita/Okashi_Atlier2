using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameQuestPanel : MonoBehaviour {

    private Text quest_text;

	// Use this for initialization
	void Start () {

        quest_text = this.transform.Find("Panel/QuestText").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TextKoushin()
    {
        //進行具合に応じて、メインクエのテキストが変わる。
        quest_text.text = GameMgr.System_spquest_message;
        Debug.Log("メインクエメッセージ: " + GameMgr.System_spquest_message);
    }
}
