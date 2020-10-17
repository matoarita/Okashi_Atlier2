using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestTitlePanel : MonoBehaviour {

    private SoundController sc;
    private Compound_Main compound_Main;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //音ならす
        sc.PlaySe(25); //25 鐘の音:50 キラリン:17
        //sc.PlaySe(27);

        //アニメーションスタート
        OnStartAnim();

        StartCoroutine("WaitSeconds");
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(2.0f);

        //数秒たったら、フェードアウトで消す。
        FadeOut();
       
    }

    void OnStartAnim()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.transform.Find("QuestPanel").GetComponent<CanvasGroup>().alpha = 0;
        //sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
        sequence.Append(this.transform.Find("QuestPanel").transform.DOLocalMove(new Vector3(0f, 20f, 0), 0.0f)
            .SetRelative()); //元の位置から30px右に置いておく。
        
                             //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/

        //まず画面をフェードイン
        sequence.Append(this.GetComponent<CanvasGroup>().DOFade(1, 0.3f)
            .SetEase(Ease.OutQuart));

        //次にクエストタイトルを登場させる
        sequence.Join(this.transform.Find("QuestPanel").transform.DOLocalMove(new Vector3(0.0f, 0.0f, 0.0f), 0.7f)
            .SetEase(Ease.OutQuart)); //30px右から、元の位置に戻る。
        sequence.Join(this.transform.Find("QuestPanel").GetComponent<CanvasGroup>().DOFade(1, 0.3f)
             .SetEase(Ease.OutQuart));
    }

    void FadeOut()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(this.GetComponent<CanvasGroup>().DOFade(0, 0.3f)
            .SetEase(Ease.OutQuart)
            .OnComplete(EndAnim));
        /*sequence.Join(this.transform.Find("QuestPanel").transform.DOLocalMove(new Vector3(0f, -30f, 0), 1.0f)
            .SetRelative()
            .SetEase(Ease.OutQuart));

        //元位置にもどしておく。
        sequence.Append(this.transform.Find("QuestPanel").transform.DOLocalMove(new Vector3(0f, 30f, 0), 0.0f)
            .SetRelative());*/

    }

    void EndAnim()
    {
        GameMgr.KeyInputOff_flag = true; //キー入力受付開始
        compound_Main.StartMessage();
        this.gameObject.SetActive(false);
    }
}
