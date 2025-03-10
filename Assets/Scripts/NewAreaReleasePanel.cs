using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewAreaReleasePanel : MonoBehaviour {

    private SoundController sc;

    private Text rank_toptext;
    private Text panel_text;
    private Text panel_titletext;
    private Image panel_imgIcon;

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    private GameObject contentPrefab;

    private GameObject close_button_obj;
    private GameObject panel_obj;

    private List<GameObject> _listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。

    private bool ButtonON;


    // Use this for initialization
    void Start () {
		
	}

    private void OnEnable()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        rank_toptext = this.transform.Find("Pos/NewAreaWindow/Image/RankText").GetComponent<Text>();
        content = this.transform.Find("Pos/NewAreaWindow/Image/Scroll_View/Viewport/Content").gameObject;
        panel_obj = this.transform.Find("Pos").gameObject;
        contentPrefab = (GameObject)Resources.Load("Prefabs/NewAreaTogglePanel");
        _listitem.Clear();

        close_button_obj = this.transform.Find("CloseButton").gameObject;
        close_button_obj.SetActive(false);

        ButtonON = false;

        foreach (Transform obj in content.transform)
        {
            Destroy(obj.gameObject);
        }

        StartCoroutine("TimeWait");

        //アニメスタート
        Result_animOn();
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Compound_Mainから読み出し
    public void Set_PatissierRank(string _prank_text)
    {
        //rank_toptext.text = "ランクが　" + _prank_text + "　になりました！";
    }

    public void Set_GohoubiPanel(string _gohoubitext, string _titletext, int _starparam, Sprite _icon)
    {
        _listitem.Add(Instantiate(contentPrefab, content.transform));
        panel_text = _listitem[_listitem.Count - 1].transform.Find("Text").GetComponent<Text>();
        panel_imgIcon = _listitem[_listitem.Count - 1].transform.Find("ImageIcon").GetComponent<Image>();
        panel_titletext = _listitem[_listitem.Count - 1].transform.Find("TitleText").GetComponent<Text>();

        panel_text.text = _gohoubitext;
        panel_titletext.text = "★" + _starparam.ToString() + " " + _titletext;
        panel_imgIcon.sprite = _icon;
    }

    IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(1.0f); //1秒待つ

        close_button_obj.SetActive(true);
        ButtonON = true;

    }

    public void OnCloseWindow()
    {
        if (ButtonON)
        {
            //GameMgr.scenario_ON = false;
            GameMgr.newarea_read_endflag = true;
            this.gameObject.SetActive(false);
        }
    }

    //ボインとはじくようなアニメ
    void Result_animOn()
    {
        AnimPoyon();      

    }

    void AnimPoyon()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        panel_obj.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(panel_obj.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.0f));
        //sequence.Join(resulttransform.DOLocalMove(new Vector3(0, 0, 0), 0.0f)
        //); //
        //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        sequence.Append(panel_obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.75f)
            .SetEase(Ease.OutElastic));
        /*sequence.Join(resulttransform.DOLocalMove(new Vector3(0f, 80f, 0), 0.75f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //元の位置に戻る。*/
        sequence.Join(panel_obj.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }
}
