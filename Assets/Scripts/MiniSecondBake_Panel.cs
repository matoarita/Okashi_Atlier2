using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MiniSecondBake_Panel : MonoBehaviour {

    private SoundController sc;

    private bool stop_watch;
    private float timeOut;

    private float timeMax;
    private int guage_length;
    private float _interval;
    private int _guage_param;

    private Slider _tempslider;

    // Use this for initialization
    void Start () {

        //SetInit();
    }

    void SetInit()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        timeOut = 0.0f;
        stop_watch = false;
        //Debug.Log("stop_watch: " + stop_watch);
        _guage_param = 0;

        timeMax = 3.0f;
        guage_length = 600; //スライダの長さ　手動で入力
        _interval = guage_length / timeMax; //スピード
        _interval = _interval * 1.5f; //さらにスピード補正　早い
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

            _guage_param = (int)(_interval * timeOut);
            if(_guage_param >= guage_length)
            {
                _guage_param = guage_length;
                stop_watch = false;
                Debug.Log("stop_watch: " + stop_watch);
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
        GameMgr.System_magic_playSucess = true;
        GameMgr.System_magic_playParamUp = 1.0f;

        //ピキーン音ならす
        sc.PlaySe(16);

        //そのときのゲージの値によって、成功か不成功かもここで判定
        if (_guage_param >= 0 && _guage_param < 100)
        {
            GameMgr.System_magic_playParamUp = 0.5f;
        }
        else if (_guage_param >= 100 && _guage_param < 400)
        {
            GameMgr.System_magic_playParamUp = 1.1f;
        }
        else if (_guage_param >= 400 && _guage_param < 430)
        {
            GameMgr.System_magic_playParamUp = 1.3f;
        }
        else if (_guage_param >= 440 && _guage_param < 460)
        {
            GameMgr.System_magic_playParamUp = 2.0f;
        }
        else if (_guage_param >= 460 && _guage_param < 500)
        {
            GameMgr.System_magic_playParamUp = 1.3f;
        }
        else if (_guage_param >= 500)
        {
            //焼すぎで失敗
            GameMgr.System_magic_playSucess = false;
        }
    }
}
