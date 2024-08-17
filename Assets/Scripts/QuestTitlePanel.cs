using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestTitlePanel : MonoBehaviour {

    private SoundController sc;
    private Compound_Main compound_Main;
    private Special_Quest special_quest;

    private float qtitlepanel_pos_y;

    private Image okashiImage;
    private GameObject questpanel_num_obj;
    private GameObject text_quest;
    private GameObject text_extra;

    private Text questpanel_text;
    private Text questpanel_num;

    private GameObject questprogress_Prefab1;
    private List<GameObject> questview_obj = new List<GameObject>();
    private Sprite questprogress_nowImg;

    private int i;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetInit()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //メイン画面に表示する、現在のクエスト
        questpanel_text = this.transform.Find("QuestPanel/QuestName").GetComponent<Text>();
        questpanel_num = this.transform.Find("QuestPanel/TitleImage/Questnum").GetComponent<Text>();

        questpanel_num_obj = this.transform.Find("QuestPanel/TitleImage/Questnum").gameObject;
        text_quest = this.transform.Find("QuestPanel/TitleImage/Text_quest").gameObject;
        text_extra = this.transform.Find("QuestPanel/TitleImage/Text_Extra").gameObject;

        questprogress_Prefab1 = (GameObject)Resources.Load("Prefabs/QProgressButton");
        questprogress_nowImg = Resources.Load<Sprite>("Sprites/Window/pageguideD_pink_50");

        okashiImage = this.transform.Find("OkashiImage/Image").GetComponent<Image>();

        questpanel_text.text = GameMgr.MainQuestTitleName;

        if (GameMgr.Story_Mode == 0)
        {
            questpanel_num.text = special_quest.OkashiQuest_Number;
            questpanel_num_obj.SetActive(true);
            text_quest.SetActive(true);
            text_extra.SetActive(false);
        }
        else
        {
            questpanel_num_obj.SetActive(false);
            text_quest.SetActive(false);
            text_extra.SetActive(true);
        }

        questview_obj.Clear();
        foreach (Transform child in this.transform.Find("QuestPanel/QuestProgressView/Viewport/Content").gameObject.transform) //
        {
            Destroy(child.gameObject);
        }

        //Debug.Log("special_quest.OkashiQuest_AllCount: " + special_quest.OkashiQuest_AllCount);
        for (i = 0; i < special_quest.OkashiQuest_AllCount; i++)
        {
            questview_obj.Add(Instantiate(questprogress_Prefab1, this.transform.Find("QuestPanel/QuestProgressView/Viewport/Content").gameObject.transform));
        }

        if (special_quest.OkashiQuest_Count <= special_quest.OkashiQuest_AllCount)
        {
            questview_obj[special_quest.OkashiQuest_Count - 1].GetComponent<Image>().sprite = questprogress_nowImg;
        }
        else
        {

        }

        okashiImage.sprite = special_quest.OkashiQuest_sprite;
    }

    private void OnEnable()
    {
        SetInit();

        //音ならす
        sc.PlaySe(25); //25 鐘の音:50 キラリン:17
        //sc.PlaySe(27);

        qtitlepanel_pos_y = 65f; //パネルの初期値

        //アニメーションスタート
        OnStartAnim();

        StartCoroutine("WaitSeconds");
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(2.5f);

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
        sequence.Append(this.transform.Find("QuestPanel").transform.DOLocalMove(new Vector3(0f, qtitlepanel_pos_y+20f, 0), 0.0f)
            .SetRelative()); //元の位置から30px右に置いておく。
        
                             //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/

        //まず画面をフェードイン
        sequence.Append(this.GetComponent<CanvasGroup>().DOFade(1, 0.3f)
            .SetEase(Ease.OutQuart));

        //次にクエストタイトルを登場させる
        sequence.Join(this.transform.Find("QuestPanel").transform.DOLocalMove(new Vector3(0.0f, qtitlepanel_pos_y, 0.0f), 0.7f)
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
        GameMgr.MesaggeKoushinON = true; //メイン下枠のメッセージを更新するフラグ
        compound_Main.StartMessage();

        GameMgr.check_GirlLoveSubEvent_flag = false; //パネル閉じたあとに、サブイベントだけチェックする 
        this.gameObject.SetActive(false);
    }
}
