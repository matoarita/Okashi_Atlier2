using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeartUpObj : MonoBehaviour {

    private GameObject canvas;

    //カメラ関連
    private Camera main_cam;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;
    private GameObject Girl_love_exp_bar;

    private Vector3 startPos;
    private Vector3 RandomPos;
    Vector3 pos;

    private GameObject heartPanel;
    Vector3 target_pos;
    private float rnd_time;

    private Vector3 _speedPos;

    private float _speed;
    private float _startspeed;

    private float _x;
    private float _y;
    private float dist; //座標の距離

    private int rndx, rndy;
    private int i;

    private bool begin_stop;

    private SoundController sc;

    private int _deg;
    public int _id; //ハート一個一個にIDをつけとく

    private GameObject Magic_effect_Prefab1;
    private GameObject _listEffect;

    // Use this for initialization
    void Start () {

        //InitParam();
    }

    void InitParam()
    {
        //カメラの取得
        main_cam = Camera.main;

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //女の子、お菓子の判定処理オブジェクトの取得
        GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
        girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

        Girl_love_exp_bar = canvas.transform.Find("Girl_love_exp_bar").gameObject;
        heartPanel = canvas.transform.Find("Girl_love_exp_bar/Panel").gameObject;
        target_pos = heartPanel.transform.localPosition;

        //エフェクトプレファブの取得
        Magic_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_KiraExplode_Heart");

        startPos = new Vector3(Girl_love_exp_bar.transform.localPosition.x, 
            Girl_love_exp_bar.transform.localPosition.y - 400, 
            Girl_love_exp_bar.transform.localPosition.z);

        rndx = Random.Range(-400, 400);
        rndy = Random.Range(-400, 400);
        RandomPos = new Vector3(startPos.x + rndx, startPos.y + rndy, startPos.z);

        //最初の生成位置
        this.transform.localPosition = RandomPos;
        //Debug.Log("this.transform.localPosition" + this.transform.localPosition);

        pos = RandomPos;

        //_speed = 0.01f + Random.Range(0f, 0.015f);        

        begin_stop = true;
    }

    private void OnEnable()
    {
        InitParam();

        StartScaleAnim();

        //ハートの数が多い場合、エフェクトを少し間引く
        CountRecycle();

        if (girlEat_judge._listHeart.Count >= 10)
        {
            if (_id % _deg == 0)
            {
                //エフェクト生成
                AttackEffect();
            }
            else
            {

            }
        }
        else
        {
            //エフェクト生成
            AttackEffect();
        }

        StartCoroutine("WaitSeconds");
    }

    void AttackEffect()
    {
        _listEffect = null;
        //エフェクト生成
        _listEffect = Instantiate(Magic_effect_Prefab1, Girl_love_exp_bar.transform);
        _listEffect.GetComponent<Canvas>().worldCamera = main_cam;
        _listEffect.transform.localPosition = this.transform.localPosition;
        //Debug.Log("this.transform.localPosition" + this.transform.localPosition);
    }

    void StartScaleAnim()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOScale(new Vector3(-0.2f, -0.2f, -0.2f), 0.0f)
        .SetRelative());
        sequence.Append(this.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 1.0f)
        .SetRelative()
        .SetEase(Ease.OutElastic)); //30px上から、元の位置に戻る。
    }

    // Update is called once per frame
    void Update () {

        if( begin_stop )
        {                     
            //最初１～２秒、ちょっと停滞
            //StartCoroutine("WaitSeconds");
        } else
        {           
            /*
            pos = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(pos.x + _speedPos.x, pos.y + _speedPos.y, pos.z);

            _x = pos.x - target_pos.x;
            _y = pos.y - target_pos.y;

            dist = Mathf.Sqrt((_x * _x) + (_y * _y));

            //Debug.Log("ターゲットとの距離: " + dist);

            //距離半径に入ったらヒット
            if (dist <= 10)
            {
                DestObj();

            }

            //４パターンを検出し、ヒット判定
            if (_speedPos.x <= 0 && target_pos.x >= pos.x)
            {
                if (_speedPos.y > 0 && target_pos.y < pos.y)
                {
                    DestObj();
                }
                else if (_speedPos.y <= 0 && target_pos.y >= pos.y)
                {
                    DestObj();
                }
            }

            if (_speedPos.x > 0 && target_pos.x < pos.x)
            {
                if (_speedPos.y > 0 && target_pos.y < pos.y)
                {
                    DestObj();
                }
                else if (_speedPos.y <= 0 && target_pos.y >= pos.y)
                {
                    DestObj();
                }
            }*/
        }
        
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(1.0f); //１秒待つ

        //_speedPos = pos - target_pos;
        //_speedPos = new Vector3(_speedPos.x * _speed * -1, _speedPos.y * _speed * -1, _speedPos.z);

        begin_stop = false;

        //対象までの移動のアニメーション
        rnd_time = Random.Range(0.5f, 2.0f);
        Sequence sequence = DOTween.Sequence();

        sequence.Append(this.transform.DOLocalMoveX(target_pos.x, rnd_time)
        .SetEase(Ease.InBack)
        .OnComplete(DestObj));
        sequence.Join(this.transform.DOLocalMoveY(target_pos.y, rnd_time)
        .SetEase(Ease.InQuad));
    }

    void DestObj()
    {
        CountRecycle();
        
        //音鳴らす
        if (_id % _deg == 0)
        {
            //Debug.Log("bang");
            sc.PlaySe(33);
        }
        else
        {
            
        }

        //好感度ゲージを上昇
        girlEat_judge.GetHeartValue();

        girlEat_judge.heart_count--;
        Destroy(this.gameObject);
    }

    void CountRecycle()
    {
        if (girlEat_judge._listHeart.Count < 50)
        {
            _deg = 3;
        }
        else if (girlEat_judge._listHeart.Count >= 50 && girlEat_judge._listHeart.Count < 100)
        {
            _deg = 5;
        }
        else if (girlEat_judge._listHeart.Count >= 100 && girlEat_judge._listHeart.Count < 200)
        {
            _deg = 7;
        }
        else if (girlEat_judge._listHeart.Count >= 200)
        {
            _deg = 10;
        }
    }
}
