using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAcceTrigger : MonoBehaviour {

    //飾りデータを保持する辞書
    Dictionary<int, string> BGAcceInfo;

    private SoundController sc;

    private int i;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        InitDictBGAcce();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DrawBGAcce()
    {
        for (i = 0; i < GameMgr.DecoItems.Length; i++)
        {

            switch (BGAcceInfo[i])
            {
                case "himmeli":

                    if (GameMgr.DecoItems[i])
                    {
                        this.transform.Find("himmeliObj").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("himmeliObj").gameObject.SetActive(false);
                    }
                    break;

            }
        }
    }

    public void BGAcceOn(string _accenum)
    {
        for(i=0; i < BGAcceInfo.Count; i++)
        {
            if(BGAcceInfo[i] == _accenum)
            {
                GameMgr.DecoItems[i] = !GameMgr.DecoItems[i]; //OnとOffを切り替え
                if(GameMgr.DecoItems[i])
                {
                    //飾りONのときの音
                    sc.PlaySe(33);

                } else
                {
                    //飾りOFFのときの音[
                    sc.PlaySe(18);
                }
            }
        }

        //描画更新
        DrawBGAcce();
    }

    void InitDictBGAcce()
    {
        //まずは初期化。10個まで現在は登録可能。GameMgrのDecoItemsと連動。
        BGAcceInfo = new Dictionary<int, string>();
        BGAcceInfo.Add(0, "himmeli");
        BGAcceInfo.Add(1, "Non");
        BGAcceInfo.Add(2, "Non");
        BGAcceInfo.Add(3, "Non");
        BGAcceInfo.Add(4, "Non");
        BGAcceInfo.Add(5, "Non");
        BGAcceInfo.Add(6, "Non");
        BGAcceInfo.Add(7, "Non");
        BGAcceInfo.Add(8, "Non");
        BGAcceInfo.Add(9, "Non");
    }
}
