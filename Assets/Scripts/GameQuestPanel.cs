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
        quest_text.text = GameMgr.mainquest_message_list[GameMgr.MainQuest_Mesnum];
        //Debug.Log("メインクエメッセージ: " + GameMgr.mainquest_message_list[GameMgr.MainQuest_Mesnum]);

        /*switch (GameMgr.MainQuest_Mesnum)
        {
            case 0: //一番最初は、ガールおかしあげるイベ終わって、街へでようとなる。なので、街へでてみよう！

                quest_text.text = GameMgr.mainquest_message_list[0];
                break;

            case 1: //

                quest_text.text = GameMgr.mainquest_message_list[1];
                break;

            case 2: //

                quest_text.text = GameMgr.mainquest_message_list[2];
                break;
        }*/
    }
}
