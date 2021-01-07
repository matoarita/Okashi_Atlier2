using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Result_Panel : MonoBehaviour
{
    //カメラ関連
    private Camera main_cam;

    private Button button;

    private GirlEat_Judge girlEat_judge;

    private SoundController sc;

    private GameObject Resultimage;
    private GameObject Getlove_panel;
    private GameObject Getlove_param;
    private GameObject Manzoku_star_content;
    private GameObject starPrefab;
    private GameObject hint_text_obj;
    private GameObject GoukakuPanel;
    private Color text_default_color;
    private List<GameObject> _liststar = new List<GameObject>();

    private int getlove_exp;
    private int Total_score;
    private int star_count;

    private Tween coinTween;
    private int currentDispCoin;
    private int preDispCoin;

    private int _poncount;

    private Text okashi_score_text;

    private float countTime;

    private bool AnimEnd;

    private GameObject Magic_effect_Prefab1;
    private GameObject Magic_effect_Prefab2;
    private List<GameObject> _listEffect = new List<GameObject>();
    private List<GameObject> _listEffect2 = new List<GameObject>();

    private int i;

    // Use this for initialization
    void Start()
    {
        //カメラの取得
        main_cam = Camera.main;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //エフェクトプレファブの取得
        Magic_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_KiraExplode_2");
        Magic_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Particle_ResultKamiHubuki");

        button = this.transform.Find("Button").GetComponent<Button>();
        button.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Enter");
            girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();
            girlEat_judge.ResultPanel_On();            
        }*/
    }

    private void OnEnable()
    {

        girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        Resultimage = this.transform.Find("Image").gameObject;
        Getlove_panel = this.transform.Find("GetLovePanelBG").gameObject;
        Getlove_param = Getlove_panel.transform.Find("Result_GetLoveText/Result_ParamText").gameObject;

        button = this.transform.Find("Button").GetComponent<Button>();
        button.interactable = false;

        okashi_score_text = this.transform.Find("Image/Okashi_Score").GetComponent<Text>();
        okashi_score_text.text = "";

        Manzoku_star_content = this.transform.Find("Image/Manzoku_Score_star/Viewport/Content").gameObject;
        starPrefab = (GameObject)Resources.Load("Prefabs/StarScore");

        hint_text_obj = this.transform.Find("Image/Hint_Text").gameObject;
        hint_text_obj.GetComponent<CanvasGroup>().alpha = 0;

        GoukakuPanel = this.transform.Find("Image/GoukakuPanel").gameObject;
        GoukakuPanel.GetComponent<CanvasGroup>().alpha = 0;
        text_default_color = GoukakuPanel.transform.Find("Text").GetComponent<Text>().color;

        currentDispCoin = 0;
        coinTween = null;

        getlove_exp = girlEat_judge.Getlove_exp;
        Total_score = girlEat_judge.total_score;
        star_count = girlEat_judge.star_Count;

        //スター消す
        foreach (Transform child in Manzoku_star_content.transform)
        {
            Destroy(child.gameObject);
        }
        _liststar.Clear();

        AnimEnd = false;

        //数字演出をいれる。
        SujiCountUpAnimation();

        StartCoroutine("WaitButton");
    }

    IEnumerator WaitButton()
    {
        while (!AnimEnd) //アニメ終了まで待つ。
        {
            yield return null;
        }

        //yield return new WaitForSeconds(1.0f); //1~2秒まったら、ボタンがおせるようになる。連打防止。
        AnimEnd = false;
        button.interactable = true;
    }


    void SujiCountUpAnimation()
    {
        //カウントアップのための秒数を割り出す。
        countTime = Total_score * 0.03f; //1ごとに0.03fで表示する

        //①まずはウィンドウをふわっとだす。
        StartAnim();
        
    }

    //①
    void StartAnim()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        Resultimage.GetComponent<CanvasGroup>().alpha = 0;
        Getlove_panel.GetComponent<CanvasGroup>().alpha = 0;
        Getlove_param.GetComponent<CanvasGroup>().alpha = 0;

        //sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
        sequence.Append(Resultimage.transform.DOLocalMove(new Vector3(0f, 30f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。
        sequence.Join(Getlove_panel.transform.DOLocalMove(new Vector3(0f, 30f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。
                             //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/
        sequence.Append(Resultimage.transform.DOLocalMove(new Vector3(0f, -30f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutExpo) //30px上から、元の位置に戻る。
            .OnComplete(() => UpdateCoin(Total_score))); //②数字演出開始　再生終了時

        sequence.Join(Resultimage.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    //②数字演出
    void UpdateCoin(int num)
    {
        DOTween.Kill(coinTween);
        coinTween = DOTween.To(() => currentDispCoin, (val) =>
        {
            //Debug.Log("bang");
            currentDispCoin = val;            

            if (currentDispCoin < GameMgr.low_score) //文字色をかえる。
            {
                okashi_score_text.color = new Color(255f / 255f, 255f / 255f, 255f / 255f); //茶色　(129f / 255f, 87f / 255f, 60f / 255f) 青文字(105f / 255f, 168f / 255f, 255f / 255f)      
            }
            else if (currentDispCoin >= GameMgr.low_score && currentDispCoin < GameMgr.high_score)
            {
                okashi_score_text.color = new Color(255f / 255f, 252f / 255f, 158f / 255f); //ピンク
            }
            else
            {
                okashi_score_text.color = new Color(255f / 255f, 105f / 255f, 139f / 255f); //みどり　(118f / 255f, 255f / 255f, 142f / 255f) 黄色(255f / 255f, 252f / 255f, 158f / 255f)
            }

            okashi_score_text.text = string.Format("{0:#,0}", val);

            if (currentDispCoin != preDispCoin)
            {
                sc.PlaySe(37); //トゥルルルルという文字送り音
            }
            preDispCoin = currentDispCoin; //前回の値も保存
        }, num, countTime).SetEase(Ease.OutQuart)
        .OnComplete(EndCountUpAnim); //③エンドアニメ　再生終了時;
    }

    //③数字がすべて表示された後のアニメ
    void EndCountUpAnim()
    {
        _poncount = 0;
       
        StartCoroutine("StarPon"); //満足度の☆表示              
    }

    IEnumerator StarPon()
    {
        if(_poncount >= star_count)
        {
            HintTextAnim();
            GetLoveBarAnim();           
            yield break;
        }

        _liststar.Add(Instantiate(starPrefab, Manzoku_star_content.transform));
        sc.PlaySe(30);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_liststar[_poncount].transform.DOScale(new Vector3(-0.5f, -0.5f, -0.5f), 0.0f)
        .SetRelative());
        sequence.Append(_liststar[_poncount].transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1.0f)
        .SetRelative()
        .SetEase(Ease.OutElastic)); //30px上から、元の位置に戻る。

        if (_poncount >= star_count-1) //最後の星が出るタイミング
        {
            

            
            if (Total_score < GameMgr.low_score)
            { }
            else {
                _listEffect2.Clear();
                for (i = 0; i < _liststar.Count; i++)
                {
                    _listEffect2.Add(Instantiate(Magic_effect_Prefab2, _liststar[i].transform));
                    _listEffect2.Add(Instantiate(Magic_effect_Prefab1, _liststar[i].transform));
                }
            }

            //合格演出
            if(Total_score < 30) //まずい
            {
                GoukakuPanel.transform.Find("Text").GetComponent<Text>().text = "マズい..。";
            }
            if (Total_score >= 30 && Total_score < GameMgr.low_score)
            {
                //sc.PlaySe(19);
                //GoukakuPanel.transform.Find("Text").GetComponent<Text>().color = new Color(91f / 255f, 55f / 255f, 206f / 255f);
                GoukakuPanel.transform.Find("Text").GetComponent<Text>().text = "兄ちゃん！あとひといき..！";                
            }
            else if (Total_score >= GameMgr.low_score && Total_score < GameMgr.high_score)
            {
                //sc.PlaySe(19);
                GoukakuPanel.transform.Find("Text").GetComponent<Text>().text = "うみゃあ！！";
            }
            else if (Total_score >= GameMgr.high_score && Total_score < 100)
            {
                sc.PlaySe(19);
                GoukakuPanel.transform.Find("Text").GetComponent<Text>().text = "大好きぃ！！";
            }
            else if (Total_score >= 100)
            {
                sc.PlaySe(19);
                GoukakuPanel.transform.Find("Text").GetComponent<Text>().text = "このお菓子は最高だ！！";
            }

            GoukakuPanelOn();
        }

        yield return new WaitForSeconds(0.2f);
       
        _poncount++;
        StartCoroutine("StarPon");

    }

    //④ヒントテキストの表示
    void HintTextAnim()
    {       
        Sequence sequence = DOTween.Sequence();

        sequence.Append(hint_text_obj.transform.DOLocalMoveX(-30f, 0.0f)
        .SetRelative());

        sequence.Append(hint_text_obj.transform.DOLocalMoveX(30f, 0.3f)
        .SetRelative()
        .SetEase(Ease.OutElastic)); //30px上から、元の位置に戻る。
        sequence.Join(hint_text_obj.GetComponent<CanvasGroup>().DOFade(1, 0.3f));
    }

    //⑤好感度バー表示
    void GetLoveBarAnim()
    {
        GetLoveTextKoushin();

        Sequence sequence = DOTween.Sequence();

        //好感度ゲットバーも表示する。
        sequence.Append(Getlove_panel.transform.DOLocalMove(new Vector3(0f, -30f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutQuart)); //30px上から、元の位置に戻る。

        sequence.Join(Getlove_panel.GetComponent<CanvasGroup>().DOFade(1, 0.2f));


        //取得した好感度をキラキラアニメーション       
        sequence.Append(Getlove_param.GetComponent<CanvasGroup>().DOFade(1, 0.01f));

        if (getlove_exp >= 0)
        {

            sc.PlaySe(17);

            //エフェクト生成＋アニメ開始
            _listEffect.Clear();
            _listEffect.Add(Instantiate(Magic_effect_Prefab1));
            _listEffect[0].GetComponent<Canvas>().worldCamera = main_cam;
            _listEffect[0].transform.Find("Pos").transform.localPosition = Getlove_panel.transform.localPosition;
            _listEffect[0].transform.Find("Pos").transform.DOLocalMove(new Vector3(0f, -20f, 0), 0.0f).SetRelative();

        }
        else //さがったときの音
        {
            sc.PlaySe(20);
        }

        

        sequence.Append(Getlove_param.transform.DOLocalMove(new Vector3(0f, 30f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutQuart));
        sequence.Join(Getlove_param.GetComponent<Text>().DOColor(new Color(0.3f, 0.3f, 0.3f), 0.2f)
            .SetRelative()
            .SetEase(Ease.OutCubic));
        sequence.Append(Getlove_param.transform.DOLocalMove(new Vector3(0f, -30f, 0), 0.7f)
            .SetRelative()
            .SetEase(Ease.OutBounce)
            .OnComplete(() => AnimEnd = true));
        sequence.Join(Getlove_param.GetComponent<Text>().DOColor(new Color(-0.3f, -0.3f, -0.3f), 0.2f)
            .SetRelative()
            .SetEase(Ease.InCubic));

        //AnimEnd = true;
    }

    void GetLoveTextKoushin()
    {
        if (getlove_exp > 0)
        {
            Getlove_panel.transform.Find("Result_GetLoveText").gameObject.SetActive(true);
            Getlove_panel.transform.Find("Result_NoLoveText").gameObject.SetActive(false);

            Getlove_panel.transform.Find("Result_GetLoveText/Result_ParamText").GetComponent<Text>().text = getlove_exp.ToString();
            Getlove_panel.transform.Find("Result_GetLoveText/Result_Text_end").GetComponent<Text>().text =  " アップ！";
        }
        else if (getlove_exp == 0)
        {
            Getlove_panel.transform.Find("Result_GetLoveText").gameObject.SetActive(false);
            Getlove_panel.transform.Find("Result_NoLoveText").gameObject.SetActive(true);
        }
        else
        {
            Getlove_panel.transform.Find("Result_GetLoveText").gameObject.SetActive(true);
            Getlove_panel.transform.Find("Result_NoLoveText").gameObject.SetActive(false);

            Getlove_panel.transform.Find("Result_GetLoveText/Result_ParamText").GetComponent<Text>().text = Mathf.Abs(getlove_exp).ToString();
            Getlove_panel.transform.Find("Result_GetLoveText/Result_Text_end").GetComponent<Text>().text = " 下がった..。";
        }
    }

    void GoukakuPanelOn()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(GoukakuPanel.transform.DOScale(new Vector3(-0.5f, -0.5f, -0.5f), 0.0f)
        .SetRelative());

        sequence.Append(GoukakuPanel.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1.0f)
        .SetRelative()
        .SetEase(Ease.OutElastic)); 
        sequence.Join(GoukakuPanel.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }
}
