using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAcceTrigger : MonoBehaviour {

    private SoundController sc;

    private int i;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DrawBGAcce()
    {
        for (i = 0; i < GameMgr.DecoItems.Length; i++)
        {

            switch (GameMgr.BGAcceItemsName[i])
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

                case "kuma_nuigurumi":

                    if (GameMgr.DecoItems[i])
                    {
                        this.transform.Find("KumaNuigurumi").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("KumaNuigurumi").gameObject.SetActive(false);
                    }
                    break;

            }
        }
    }

    public void BGAcceOn(string _accenum)
    {
        for(i=0; i < GameMgr.BGAcceItemsName.Count; i++)
        {
            if(GameMgr.BGAcceItemsName[i] == _accenum)
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

}
