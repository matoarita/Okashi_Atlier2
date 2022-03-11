using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestClearEffectPanel : MonoBehaviour
{
    private SoundController sc;

    private Text _questclear_text;

    // Use this for initialization
    void Start()
    {

    }

    void SetInit()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        _questclear_text = this.transform.Find("QuestClearImage/Text").GetComponent<Text>();

        this.transform.Find("QuestClearImage/clearImage_1").gameObject.SetActive(true);
        this.transform.Find("QuestClearImage/clearImage_2").gameObject.SetActive(false);
        this.transform.Find("QuestClearImage_Eff/clearImage_1").gameObject.SetActive(true);
        this.transform.Find("QuestClearImage_Eff/clearImage_2").gameObject.SetActive(false);
        /*
        if(GameMgr.high_score_flag)
        {
            //_questclear_text.text = "Excellent Clear!!";
            this.transform.Find("QuestClearImage/clearImage_1").gameObject.SetActive(false);
            this.transform.Find("QuestClearImage/clearImage_2").gameObject.SetActive(true);
        }
        else
        {
            //_questclear_text.text = "Quest Clear";
            this.transform.Find("QuestClearImage/clearImage_1").gameObject.SetActive(true);
            this.transform.Find("QuestClearImage/clearImage_2").gameObject.SetActive(false);
        }*/
    }

    private void OnEnable()
    {
        SetInit();

        //音を鳴らす。
        sc.PlaySe(91);

        //アニメーションスタート
        //OnStartAnim();

        StartCoroutine("WaitSeconds");
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnStartAnim()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.transform.Find("QuestClearImage").GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(this.transform.Find("QuestClearImage").DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.0f)
            .SetRelative());
        //sequence.Append(this.transform.Find("QuestPanel").transform.DOLocalMove(new Vector3(0f, 0, 0), 0.0f)
            //.SetRelative()); //元の位置から30px右に置いておく。

        //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/

        //画面をフェードイン
        sequence.Append(this.GetComponent<CanvasGroup>().DOFade(1, 0.3f)
            .SetEase(Ease.OutQuart));

        //次にクエストタイトルを登場させる
        sequence.Join(this.transform.Find("QuestClearImage").transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.3f)
            .SetEase(Ease.OutQuint)); //
        sequence.Join(this.transform.Find("QuestClearImage").GetComponent<CanvasGroup>().DOFade(1, 0.5f)
             .SetEase(Ease.OutQuart));
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(3.0f);
       
        //数秒たったら、フェードアウトで消す。
        FadeOut();

    }

    void FadeOut()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(this.GetComponent<CanvasGroup>().DOFade(0, 1.0f)
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
        StartCoroutine("WaitSeconds2");
        
    }

    IEnumerator WaitSeconds2()
    {
        yield return new WaitForSeconds(0.5f);

        //GameMgr.KeyInputOff_flag = true; //キー入力受付開始
        GameMgr.qclear_effect_endflag = true; //終わりの合図
        this.gameObject.SetActive(false);
    }
}
