using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContestEnshutu_Panelin : MonoBehaviour {

    private GameObject canvas;

    private GameObject _comp;

    private GameObject contestFirstEnshutuPanel_obj;
    private ContestFirstEnshutuPanel contestFirstEnshutuPanel;

    private GameObject start_effect;

    private SoundController sc;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        contestFirstEnshutuPanel_obj = canvas.transform.Find("ContestFirstEnshutuPanel").gameObject;
        contestFirstEnshutuPanel = contestFirstEnshutuPanel_obj.GetComponent<ContestFirstEnshutuPanel>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        _comp = this.gameObject;

        switch (this.gameObject.name)
        {
            case "PanelContestName":

                StartCoroutine("StartAnim");
                break;

            case "PanelReady":

                StartCoroutine("StartAnim");
                break;

            case "PanelStart":

                start_effect = this.transform.Find("EffectPanel").gameObject;
                start_effect.SetActive(false);

                StartCoroutine("StartAnim2");
                break;
        }
    }

    IEnumerator StartAnim()
    {
        //まず、初期値。
        _comp.GetComponent<CanvasGroup>().alpha = 0;

        yield return new WaitForSeconds(0.1f); //ワンテンポおく

        sc.PlaySe(14); //シュイン

        OpenAnim();
    }

    void OpenAnim()
    {
        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_comp.transform.DOLocalMove(new Vector3(50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。

        sequence.Append(_comp.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.5f)
            .SetRelative()
            .SetEase(Ease.OutQuad)); //30px上から、元の位置に戻る。
        sequence.Join(_comp.GetComponent<CanvasGroup>().DOFade(1, 0.2f)).OnComplete(EndAnim);
    }

    void EndAnim()
    {
        StartCoroutine("EndAnimWait");
    }

    IEnumerator EndAnimWait()
    {
        yield return new WaitForSeconds(0.5f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_comp.transform.DOLocalMove(new Vector3(-30f, 0f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.InQuad)); //30px上から、元の位置に戻る。
        sequence.Join(_comp.GetComponent<CanvasGroup>().DOFade(0, 0.3f)
            .OnComplete(OffObj));
    }



    IEnumerator StartAnim2()
    {
        //まず、初期値。
        _comp.GetComponent<CanvasGroup>().alpha = 0;

        yield return new WaitForSeconds(0.1f); //ワンテンポおく



        OpenAnim2();
    }

    void OpenAnim2()
    {
        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_comp.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.0f)); //元の位置から30px上に置いておく。

        sequence.Append(_comp.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.0f)
            .SetEase(Ease.OutSine)); //30px上から、元の位置に戻る。
        sequence.Join(_comp.GetComponent<CanvasGroup>().DOFade(1, 0.2f)).OnComplete(EndAnim2);
    }

    void EndAnim2()
    {
        sc.PlaySe(99); //ピピーーー
        start_effect.SetActive(true);
        StartCoroutine("EndAnimWait2");
    }

    IEnumerator EndAnimWait2()
    {
        yield return new WaitForSeconds(0.5f);

        Sequence sequence = DOTween.Sequence();

        //sequence.Append(_comp.GetComponent<CanvasGroup>().DOFade(0, 1.0f).OnComplete(OffObj));
        OffObj();
    }

    void OffObj()
    {
        switch(this.gameObject.name)
        {
            case "PanelContestName":

                contestFirstEnshutuPanel.OnStartAnim2();
                break;

            case "PanelReady":

                contestFirstEnshutuPanel.OnStartAnim3();
                break;

            case "PanelStart":

                contestFirstEnshutuPanel.OnEndAnim();
                break;
        }
    }
}
