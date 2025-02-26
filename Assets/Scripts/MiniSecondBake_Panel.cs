using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MiniSecondBake_Panel : MonoBehaviour {

    private SoundController sc;
    private MagicSkillListDataBase magicskill_database;

    private bool stop_watch;
    private float timeOut;

    private float timeMax;
    private int guage_length;
    private float _interval;
    private int _guage_param;

    private Slider _tempslider;

    private int _magiclv;
    private float _speed_hosei;

    // Use this for initialization
    void Start () {

        //SetInit();
    }

    void SetInit()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        _magiclv = magicskill_database.skillName_SearchLearnLevel("Cookie_SecondBake");

        timeOut = 0.0f;
        stop_watch = false;
        //Debug.Log("stop_watch: " + stop_watch);
        _guage_param = 0;

        timeMax = 3.0f;
        guage_length = 600; //スライダの長さ　手動で入力
        _interval = guage_length / timeMax; //スピード

        switch (_magiclv) //LV1で1.5f がデフォ速度 めちゃはや
        {
            case 1:
                _speed_hosei = 1.5f;
                break;
            case 2:
                _speed_hosei = 1.25f;
                break;
            case 3:
                _speed_hosei = 1.15f;
                break;
        }
        _interval = _interval * _speed_hosei; //さらにスピード補正　早い
        //Debug.Log("_interval: " + _interval);

        _tempslider = this.transform.Find("Comp/Slider").GetComponent<Slider>();
        _tempslider.value = 0;

        this.transform.Find("Comp").GetComponent<CanvasGroup>().alpha = 0;       
    }

    private void OnEnable()
    {
        SetInit();
    }

    // Update is called once per frame
    void Update () {

        if (stop_watch)
        {
            //時間減少
            timeOut += Time.deltaTime;
            //Debug.Log("timeOut: " + timeOut);

            _guage_param = (int)(_interval * timeOut * timeOut);
            if(_guage_param >= guage_length)
            {
                _guage_param = guage_length;
                stop_watch = false;
                Debug.Log("stop_watch: " + stop_watch);

                //焼すぎで失敗
                GameMgr.System_magic_playSuccess = false;
                Debug.Log("セカンドベイク　焼すぎで失敗");
            }
            _tempslider.value = _guage_param;
            //Debug.Log("_guage_param: " + _guage_param);
            //Debug.Log("_tempslider.value: " + _tempslider.value);
        }
    }

    //CompoundMainController.csから読み出し
    public void OnStartAnim()
    {
        Debug.Log("魔法ミニゲームOnStart");

        GameMgr.System_magic_playON = true; //ミニゲーム上での成功率判定に切り替え
        StartCoroutine(WaitForMiniGame());
        
    }

    IEnumerator WaitForMiniGame()
    {
        yield return new WaitForSeconds(0.5f); //1秒待つ

        stop_watch = true;
        this.transform.Find("Comp").GetComponent<CanvasGroup>().DOFade(1, 0.3f); //演出画面をON
        Debug.Log("stop_watch: " + stop_watch);
    }

    //クリックでそこでゲージを止める
    public void OnClickStop()
    {
        stop_watch = false;
        Debug.Log("stop_watch: " + stop_watch);

        //初期設定
        GameMgr.System_magic_playSuccess = true;
        GameMgr.System_magic_playParamUp = 1.0f;

        //ピキーン音ならす
        sc.PlaySe(16);

        //そのときのゲージの値によって、成功か不成功かもここで判定
        if (_guage_param >= 0 && _guage_param < 100)
        {
            GameMgr.System_magic_playParamUp = 0.5f;
        }
        else if (_guage_param >= 100 && _guage_param < 250)
        {
            GameMgr.System_magic_playParamUp = 1.0f;
        }
        else if (_guage_param >= 250 && _guage_param < 400)
        {
            GameMgr.System_magic_playParamUp = 1.1f;
        }
        else if (_guage_param >= 400 && _guage_param < 430)
        {
            GameMgr.System_magic_playParamUp = 1.2f;
        }
        else if (_guage_param >= 440 && _guage_param < 460)
        {
            GameMgr.System_magic_playParamUp = 1.35f;
        }
        else if (_guage_param >= 460 && _guage_param < 500)
        {
            GameMgr.System_magic_playParamUp = 1.2f;
        }
        else if (_guage_param >= 500)
        {
            //焼すぎで失敗
            GameMgr.System_magic_playSuccess = false;
            Debug.Log("セカンドベイク　焼すぎで失敗");
        }
    }
}
