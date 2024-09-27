using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewAreaReleasePanel : MonoBehaviour {

    private Text rank_toptext;
    private Text panel_text;
    private Image panel_imgIcon;

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    private GameObject contentPrefab;

    private List<GameObject> _listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。

    private bool ButtonON;

    // Use this for initialization
    void Start () {
		
	}

    private void OnEnable()
    {
        rank_toptext = this.transform.Find("NewAreaWindow/Image/RankText").GetComponent<Text>();
        content = this.transform.Find("NewAreaWindow/Image/Scroll_View/Viewport/Content").gameObject;
        contentPrefab = (GameObject)Resources.Load("Prefabs/NewAreaTogglePanel");
        _listitem.Clear();

        ButtonON = false;

        foreach (Transform obj in content.transform)
        {
            Destroy(obj.gameObject);
        }

        StartCoroutine("TimeWait");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Set_GohoubiPanel(string _gohoubitext, Sprite _icon)
    {
        _listitem.Add(Instantiate(contentPrefab, content.transform));
        panel_text = _listitem[_listitem.Count - 1].transform.Find("Text").GetComponent<Text>();
        panel_imgIcon = _listitem[_listitem.Count - 1].transform.Find("ImageIcon").GetComponent<Image>();

        panel_text.text = _gohoubitext;
        panel_imgIcon.sprite = _icon;
    }

    IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(1.0f); //1秒待つ

        ButtonON = true;

    }
    public void OnCloseWindow()
    {
        if (ButtonON)
        {
            //GameMgr.scenario_ON = false;
            GameMgr.newarea_read_endflag = false;
            this.gameObject.SetActive(false);
        }
    }
}
