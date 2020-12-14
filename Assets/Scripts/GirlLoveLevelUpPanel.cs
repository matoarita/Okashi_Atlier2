using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GirlLoveLevelUpPanel : MonoBehaviour {

    private Girl1_status girl1_status;
    private SoundController sc;

    private GameObject _comp;

    private Text glovelv_param;
    private Text glovestatus_param;

    public bool OnPanelflag = false; //GirlEat_Judgeから読み込み

    // Use this for initialization
    void Start () {        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitStart()
    {
        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        _comp = this.transform.Find("Comp").gameObject;

        glovelv_param = this.transform.Find("Comp/Image/LvParam").GetComponent<Text>();
        glovelv_param.text = PlayerStatus.girl1_Love_lv.ToString();

        glovestatus_param = this.transform.Find("Comp/Image/GenkiMessage").GetComponent<Text>();

        switch(PlayerStatus.girl1_Love_lv)
        {
            case 1:

                glovestatus_param.text = "";
                break;

            case 2:

                glovestatus_param.text = "ちょっと元気になってきた！";
                break;

            case 3:

                glovestatus_param.text = "かなり元気になってきた！";
                break;

            case 4:

                glovestatus_param.text = "元気がすごくもどってきた！";
                break;

            case 5:

                glovestatus_param.text = "ごキゲンになってきた！！";
                break;

            case 6:

                glovestatus_param.text = "元気がパワフルにたぎってきた！！";
                break;

            default: //7~以降

                glovestatus_param.text = "元気が絶頂してきた！！";
                break;
        }
        
    }

    private void OnEnable()
    {
        if (!OnPanelflag)
        {
            InitStart();
            StartAnim();
        }

        StartCoroutine("EndAnim");
        
    }

    void StartAnim()
    {
        OnPanelflag = true;
        sc.PlaySe(19); //ファンファーレ

        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        _comp.GetComponent<CanvasGroup>().alpha = 0;

        //sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
        sequence.Append(_comp.transform.DOLocalMove(new Vector3(0f, 30f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。
                             //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/
        sequence.Append(_comp.transform.DOLocalMove(new Vector3(0f, -30f, 0), 1.0f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。

        sequence.Join(_comp.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    IEnumerator EndAnim()
    {
        yield return new WaitForSeconds(2.0f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_comp.GetComponent<CanvasGroup>().DOFade(0, 0.3f)
            .OnComplete(OffObj));       
    }

    void OffObj()
    {
        OnPanelflag = false;
        this.gameObject.SetActive(false);
    }
}
