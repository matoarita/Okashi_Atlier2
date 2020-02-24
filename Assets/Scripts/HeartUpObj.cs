using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUpObj : MonoBehaviour {

    private GameObject canvas;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    Vector3 pos;

    private GameObject heartPanel;
    Vector3 target_pos;

    private Vector3 _speedPos;

    private float _speed;
    private float speed_param;

    private float _x;
    private float _y;
    private float dist; //座標の距離

    private int rnd, rnd2;

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

        speed_param = 0.005f + Random.Range(0f, 0.015f);
        _speed = speed_param;
        
        _speedPos = pos - target_pos;
        _speedPos = new Vector3(_speedPos.x * _speed * -1, _speedPos.y * _speed * -1, _speedPos.z);

        begin_stop = true;
    }
	
	// Update is called once per frame
	void Update () {

        if( begin_stop )
        {
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

        begin_stop = false;
    }
}
