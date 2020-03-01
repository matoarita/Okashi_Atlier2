using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUpObj : MonoBehaviour {

    private GameObject canvas;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    private Vector3 startPos;
    private Vector3 RandomPos;
    Vector3 pos;

    private GameObject heartPanel;
    Vector3 target_pos;

    private Vector3 _speedPos;

    private float _speed;
    private float _startspeed;

    private float _x;
    private float _y;
    private float dist; //座標の距離

    private int rnd, rnd2;
    private int i;
    private float timeOut;

    private bool begin_stop;

    private SoundController sc;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //女の子、お菓子の判定処理オブジェクトの取得
        GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
        girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

        heartPanel = canvas.transform.Find("Girl_love_exp_bar/Panel").gameObject;
        target_pos = heartPanel.transform.localPosition;

        pos = this.transform.localPosition;

        startPos = new Vector3(-220, -337,0);

        rnd = Random.Range(-100, 100);
        rnd2 = Random.Range(-100, 100);

        RandomPos = new Vector3(startPos.x + rnd, startPos.y + rnd2, startPos.z);

        pos = startPos;

        _startspeed = 0.06f;
        _speed = 0.01f + Random.Range(0f, 0.015f);


        _speedPos = pos - RandomPos;
        _speedPos = new Vector3(_speedPos.x * _startspeed * -1, _speedPos.y * _startspeed * -1, _speedPos.z);


        begin_stop = true;

        i = 1;
        timeOut = 0.24f; //fps = 4ごと
    }
	
	// Update is called once per frame
	void Update () {

        if( begin_stop )
        {
            /*timeOut -= Time.deltaTime;

            if (timeOut <= 0.0)
            {
                timeOut = 0.24f;
                i++;
                _startspeed = _startspeed - 0.00000001f;
                if(_startspeed <= 0 ) { _startspeed = 0.0f; }
                _speedPos = new Vector3(_speedPos.x * _startspeed * -1, _speedPos.y * _startspeed * -1, _speedPos.z);
            }*/
                              
                        
            pos = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(pos.x + _speedPos.x, pos.y + _speedPos.y, pos.z);

            //４パターンを検出し、ヒット判定
            if (_speedPos.x <= 0 && RandomPos.x >= pos.x)
            {
                if (_speedPos.y > 0 && RandomPos.y < pos.y)
                {
                    _speedPos = new Vector3(0, 0, 0);
                }
                else if (_speedPos.y <= 0 && RandomPos.y >= pos.y)
                {
                    _speedPos = new Vector3(0, 0, 0);
                }
            }

            if (_speedPos.x > 0 && RandomPos.x < pos.x)
            {
                if (_speedPos.y > 0 && RandomPos.y < pos.y)
                {
                    _speedPos = new Vector3(0, 0, 0);
                }
                else if (_speedPos.y <= 0 && RandomPos.y >= pos.y)
                {
                    _speedPos = new Vector3(0, 0, 0);
                }
            }

            //最初１～２秒、ちょっと停滞
            StartCoroutine("WaitSeconds");
        } else
        {
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
            }
        }
        
    }

    void DestObj()
    {
        //音鳴らす
        sc.PlaySe(33);
        //sc.PlaySe(34);

        //好感度ゲージを上昇
        girlEat_judge.GetHeartValue();

        Destroy(this.gameObject);
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(1.0f); //１秒待つ

        _speedPos = pos - target_pos;
        _speedPos = new Vector3(_speedPos.x * _speed * -1, _speedPos.y * _speed * -1, _speedPos.z);

        begin_stop = false;
    }
}
