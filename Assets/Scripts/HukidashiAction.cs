using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HukidashiAction : MonoBehaviour {

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private Girl1_status girl1_status;

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;

    private GameObject hukidasi;
    private GameObject hukidasi_text;

    private int hukidasi_action_status;
    private int i, _id;

    private string _hint;

    private Vector3 _mypos;
    private Vector3 _myscale;

    private bool _enter_flag;
    private Animator _thisanim;
    private int _enteranim_trans;

    // Use this for initialization
    void Start () {

        /*canvas = GameObject.FindWithTag("Canvas");

        //テキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();*/

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //吹き出しオブジェクトの取得
        hukidasi = this.gameObject.transform.Find("hukidashi_Image").gameObject;
        hukidasi_text = this.gameObject.transform.Find("hukidashi_Text").gameObject;

        //自分のアニメーターを取得
        _thisanim = this.GetComponent<Animator>();
        _enteranim_trans = _thisanim.GetInteger("trans");

        //自分のポジションを取得
        _mypos = this.transform.position;
        _myscale = this.transform.localScale;

        _enter_flag = false;

    }
	
	// Update is called once per frame
	void Update () {


    }

    private void OnEnable()
    {
        //InitAnim();
    }

    void InitAnim()
    {
        
        //自分のポジションを取得
        _mypos = this.transform.localPosition;

        //tweenで生成時のアニメ
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(transform.DOScale(new Vector3(0f, 0f, 0f), 0.0f));
        sequence.Join(transform.DOLocalMove(new Vector3(-0.5f, 0, 0), 0.0f)
            ); //元の位置から30px右に置いておく。
                               //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        sequence.Append(transform.DOScale(new Vector3(0.008f, 0.008f, 0.08f), 0.5f)
            .SetEase(Ease.OutExpo));
        sequence.Append(transform.DOLocalMove(new Vector3(0.5f, 0, 0), 0.5f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px右から、元の位置に戻る。
        sequence.Join(this.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    void LateUpdate()
    {

    }

    public void NormalHint()
    {

    }

    public void SpecialHint()
    {

    }

    public void PointEnter()
    {
        //Debug.Log("Enter");
        //this.transform.DOScale(new Vector3(0.002f, 0.002f, 0.002f), 0.3f);

        
        if (!_enter_flag)
        {
            //this.transform.localScale = new Vector3(0.013f, 0.013f, 1);
            _enteranim_trans = 1; //transが1を超えたときに、ズームするように設定されている。

            //intパラメーターの値を設定する.
            _thisanim.SetInteger("trans", _enteranim_trans);
            _enter_flag = true;
        }
    }

    public void PointExit()
    {
        //Debug.Log("Exit");
        //this.transform.DOScale(new Vector3(0.0015f, 0.0015f, 0.0015f), 0.3f);
       
        //this.transform.localScale = _myscale;
        _enteranim_trans = 0; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        _thisanim.SetInteger("trans", _enteranim_trans);
        _enter_flag = false;
    }

    void Init_HukidashiHint()
    {
        /*
        switch (girl1_status.OkashiQuest_ID)
        {
            case 1010:

                if (GameMgr.scenario_flag == 160)
                {
                    _hint = "兄ちゃん、このラスクってやつ、あまりうまくないかも。。" + "\n" + "パンがちょっと粉っぽいのかなぁ？";
                }
                if (GameMgr.scenario_flag == 170)
                {
                    _hint = "兄ちゃん！井戸で水を汲んでこよう！";
                }
                break;
        }*/
    }
}
