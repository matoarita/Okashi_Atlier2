using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;

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
    private GameObject hukidasi_sp;
    private GameObject hukidasi_text;

    private int hukidasi_action_status;
    private int i, _id;

    private string _hint;

    private CubismModel _model;

    private Vector3 _mypos;
    private Vector3 _temppos;
    private Vector3 _Startpos;
    private Vector3 _myscale;

    private bool _enter_flag;
    private Animator _thisanim;
    private int _enteranim_trans;

    private bool _temp_status1;
    private bool _temp_status2;

    // Use this for initialization
    void Start () {

        canvas = GameObject.FindWithTag("Canvas");

        //テキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //女の子の好みのお菓子セットの取得
        girlLikeSet_database = GirlLikeSetDataBase.Instance.GetComponent<GirlLikeSetDataBase>();

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //吹き出しオブジェクトの取得
        hukidasi = this.gameObject.transform.Find("hukidashi_Image").gameObject;
        hukidasi_text = this.gameObject.transform.Find("hukidashi_Text").gameObject;

        //スペシャル用の吹きだしオブジェクト
        hukidasi_sp = this.gameObject.transform.Find("hukidashi_Image_special").gameObject;

        //Live2Dモデルの取得
        _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
        
        //自分のアニメーターを取得
        _thisanim = this.GetComponent<Animator>();
        _enteranim_trans = _thisanim.GetInteger("trans");

        //自分のポジションを取得
        _mypos = this.transform.position;
        _temppos = _mypos;

        _Startpos = new Vector3(1.9f, 2.0f, -0.2f);
        _myscale = this.transform.localScale;

        _enter_flag = false;

        _temp_status1 = false;
        _temp_status2 = false;
    }
	
	// Update is called once per frame
	void Update () {
		
        /*
        if(text_area.GetComponent<TextController>().textend_flag == true)
        {
            //Debug.Log("text end: " + text_area.GetComponent<TextController>().textend_flag);
            switch(hukidasi_action_status)
            {
                case 0:

                    hukidasi.GetComponent<Image>().raycastTarget = true;
                    hukidasi_text.SetActive(true);
                    hukidasi.SetActive(true);

                    break;

                case 1:

                    hukidasi_sp.GetComponent<Image>().raycastTarget = true;
                    hukidasi_text.SetActive(true);
                    hukidasi_sp.SetActive(true);

                    break;
            }
            

            //カメラ元に戻す
            trans = 0; //transが1を超えたときに、ズームするように設定されている。

            //intパラメーターの値を設定する.
            maincam_animator.SetInteger("trans", trans);

            text_area.GetComponent<TextController>().textend_flag = false;

            //吹き出しのカウントを元に戻す
            if(_temp_status1)
            {
                girl1_status.GirlEat_Judge_on = true;
            }
            else if(_temp_status2)
            {
                girl1_status.WaitHint_on = true;
            }
        }
        */
    }

    void LateUpdate()
    {
        /*
        _mypos = _model.transform.position + _Startpos;

        //Live2D値を取得
        var parameter = _model.Parameters[22];
        //Debug.Log(parameter);
        //Debug.Log("parameter: " + parameter.Value);

        //吹き出し＜全体＞の位置を更新
        this.transform.position = _mypos;
        _temppos = _mypos;
       
        _temppos.x += parameter.Value * 0.02f;
        _temppos.y += parameter.Value * -0.01f;
        this.transform.position = _temppos;
        //Debug.Log("_temppos: " + _temppos);
        */
    }

    public void NormalHint()
    {
        /*
        if (girlLikeCompo_database.girllike_compoRandomset.Count > 0) //一番最初の状態。Randomsetに何も入ってないときは無視
        {
            hukidasi.GetComponent<Image>().raycastTarget = false;
            hukidasi_text.SetActive(false);
            hukidasi.SetActive(false);

            hukidasi_action_status = 0;

            //カメラ寄る。
            trans = 1; //transが1を超えたときに、ズームするように設定されている。

            //intパラメーターの値を設定する.
            maincam_animator.SetInteger("trans", trans);

            //Debug.Log("ヒントを表示する");    

            _hint = girlLikeCompo_database.girllike_compoRandomset[girl1_status.Set_compID].hint_text;
            text_area.GetComponent<TextController>().SetText(_hint);
            text_area.GetComponent<TextController>().hint_on = true;

            //一時的にどっちのカウントが進んでいたか保存
            _temp_status1 = girl1_status.GirlEat_Judge_on;
            _temp_status2 = girl1_status.WaitHint_on;

            girl1_status.GirlEat_Judge_on = false;
            girl1_status.WaitHint_on = false;
        }
        */
    }

    public void SpecialHint()
    {
        /*
        hukidasi_sp.GetComponent<Image>().raycastTarget = false;
        hukidasi_text.SetActive(false);
        hukidasi_sp.SetActive(false);

        hukidasi_action_status = 1;

        //カメラ寄る。
        trans = 1; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);

        //_compIDをもとに_IDを決定
        i = 0;
        while (i < girlLikeCompo_database.girllike_composet.Count)
        {
            if (girlLikeCompo_database.girllike_composet[i].set_ID == girl1_status.OkashiQuest_ID)
            {
                _id = i;
                break;
            }
            i++;
        }

        if (girl1_status.girl_Mazui_flag) //まずいフラグがたっていた場合、その時のクエストのヒントを教えてくれる。口と違い、下のウィンドウに表示される。
        {
            Init_HukidashiHint();            
        }
        else
        {
            _hint = girlLikeCompo_database.girllike_composet[_id].hint_text;
        }
        text_area.GetComponent<TextController>().SetText(_hint);
        text_area.GetComponent<TextController>().hint_on = true;

        //一時的にどっちのカウントが進んでいたか保存
        _temp_status1 = girl1_status.GirlEat_Judge_on;
        _temp_status2 = girl1_status.WaitHint_on;

        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
        */
    }

    public void PointEnter()
    {
        //Debug.Log("Enter");

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

        //this.transform.localScale = _myscale;
        _enteranim_trans = 0; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        _thisanim.SetInteger("trans", _enteranim_trans);
        _enter_flag = false;
    }

    void Init_HukidashiHint()
    {
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
        }
    }
}
