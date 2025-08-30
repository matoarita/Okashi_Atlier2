using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MiniChocolate_Tempering_Panel : MonoBehaviour {

    private SoundController sc;
    private MagicSkillListDataBase magicskill_database;

    private bool stop_watch;
    private float timeOut;
    private float timeOut2;
    private float timeOut3;

    private float timeMax;
    private int guage_length;
    private float _interval;
    private int _guage_param;

    private float timeMax2;
    private int guage_length2;
    private float _interval2;
    private int _guage_param2;

    private float timeMax3;
    private int guage_length3;
    private float _interval3;
    private int _guage_param3;

    private Slider _tempslider;
    private Slider _tempslider2;
    private Slider _tempslider3;

    private GameObject KiraEffect_1;
    private GameObject KiraEffect_2;
    private GameObject KiraEffect_3;

    private int _status;
    private int _magiclv;
    private float _speed_hosei;
    private float _param_hosei;

    private bool success_1;
    private bool success_2;
    private bool success_3;

    private GameObject magicstart_panel;

    private Button closebutton;

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

        magicstart_panel = this.transform.parent.parent.gameObject;

        _magiclv = magicskill_database.skillName_SearchLearnLevel("Chocolate_Tempering");

        timeOut = 0.0f;
        stop_watch = false;

        //Debug.Log("stop_watch: " + stop_watch);
        _guage_param = 0;
        _guage_param2 = 500;
        _guage_param3 = 100;

        timeMax = 100.0f;
        timeMax2 = 90.0f;
        timeMax3 = 75.0f;

        guage_length = 600; //スライダの長さ　手動で入力
        guage_length2 = 600; //スライダの長さ　手動で入力
        guage_length3 = 600; //スライダの長さ　手動で入力

        switch(_magiclv) //LV1で1.5f がデフォ速度 めちゃはや
        {
            case 1:
                _speed_hosei = 1.25f;
                break;
            case 2:
                _speed_hosei = 1.15f;
                break;
            case 3:
                _speed_hosei = 0.9f;
                break;
        }
        _param_hosei = _magiclv * 0.05f;
        //_speed_hosei = 1.7f - (0.2f * _magiclv); //LV1で1.5f がデフォ速度

        //一本目
        _interval = guage_length / timeMax; //スピード
        _interval = _interval * _speed_hosei; //さらにスピード補正　早い
        //Debug.Log("_interval: " + _interval);

        //二本目
        _interval2 = guage_length2 / timeMax2; //スピード
        _interval2 = _interval2 * _speed_hosei * 1.1f; //さらにスピード補正　早い
        //Debug.Log("_interval: " + _interval);

        //三本目
        _interval3 = guage_length3 / timeMax3; //スピード
        _interval3 = _interval3 * _speed_hosei * 1.25f; //さらにスピード補正　早い
        //Debug.Log("_interval: " + _interval);

        _tempslider = this.transform.Find("Comp/Slider").GetComponent<Slider>();
        _tempslider.value = 0;

        _tempslider2 = this.transform.Find("Comp/Slider2").GetComponent<Slider>();
        _tempslider2.value = _guage_param2;

        _tempslider3 = this.transform.Find("Comp/Slider3").GetComponent<Slider>();
        _tempslider3.value = _guage_param3;

        //キラエフェクトはオフに。
        KiraEffect_1 = this.transform.Find("Comp/Slider/Handle Slide Area/Handle/effPanel").gameObject;
        KiraEffect_1.SetActive(false);
        KiraEffect_2 = this.transform.Find("Comp/Slider2/Handle Slide Area/Handle/effPanel").gameObject;
        KiraEffect_2.SetActive(false);
        KiraEffect_3 = this.transform.Find("Comp/Slider3/Handle Slide Area/Handle/effPanel").gameObject;
        KiraEffect_3.SetActive(false);

        success_1 = false;
        success_2 = false;
        success_3 = false;

        closebutton = this.transform.Find("Comp/Button").GetComponent<Button>();
        closebutton.interactable = true;

        _status = 0; //段階を分ける

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

            if (timeOut >= 0.016f)
            {
                timeOut = 0;

                switch (_status)
                {
                    case 0:

                        _guage_param = _guage_param + (int)(_interval);
                        if (_guage_param >= guage_length)
                        {
                            _guage_param = guage_length;
                            stop_watch = false;
                            Debug.Log("stop_watch: " + stop_watch);

                            //焼すぎで失敗
                            GameMgr.System_magic_playSuccess = false;
                            Debug.Log("テンパリング　温度を超えたので失敗");
                        }
                        _tempslider.value = _guage_param;
                        //Debug.Log("_guage_param: " + _guage_param);
                        //Debug.Log("_tempslider.value: " + _tempslider.value);
                        break;

                    case 1:

                        _guage_param2 = _guage_param2 - (int)(_interval2);
                        if (_guage_param2 <= 0)
                        {
                            _guage_param2 = 0;
                            stop_watch = false;
                            Debug.Log("stop_watch: " + stop_watch);

                            //冷やしすぎで失敗
                            GameMgr.System_magic_playSuccess = false;
                            Debug.Log("テンパリング　温度を超えたので失敗");
                        }
                        _tempslider2.value = _guage_param2;
                        //Debug.Log("_guage_param2: " + _guage_param2);
                        //Debug.Log("_tempslider.value: " + _tempslider.value);
                        break;

                    case 2:

                        _guage_param3 = _guage_param3 + (int)(_interval3);
                        if (_guage_param3 >= guage_length3)
                        {
                            _guage_param3 = guage_length3;
                            stop_watch = false;
                            Debug.Log("stop_watch: " + stop_watch);

                            //焼すぎで失敗
                            GameMgr.System_magic_playSuccess = false;
                            Debug.Log("テンパリング　温度を超えたので失敗");
                        }
                        _tempslider3.value = _guage_param3;
                        //Debug.Log("_guage_param3: " + _guage_param3);
                        //Debug.Log("_tempslider.value: " + _tempslider.value);
                        break;
                }
            }
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
        //sc.PlaySe(244); //チャージ音

        yield return new WaitForSeconds(0.5f); //1秒待つ

        stop_watch = true;
        this.transform.Find("Comp").GetComponent<CanvasGroup>().DOFade(1, 0.3f); //演出画面をON
        Debug.Log("stop_watch: " + stop_watch);        
    }

    //クリックでそこでゲージを止める
    public void OnClickStop()
    {
        switch(_status)
        {
            case 0:

                GuageStop1();
                break;

            case 1:

                GuageStop2();
                break;

            case 2:

                GuageStop3();
                break;
        }
        
    }

    void GuageStop1()
    {
        //初期設定
        GameMgr.System_magic_playSuccess = true;
        GameMgr.System_magic_playParamUp = 1.0f;


        //そのときのゲージの値によって、成功か不成功かもここで判定
        if (_guage_param >= 0 && _guage_param < 100)
        {
            GameMgr.System_magic_playParamUp = 0.5f;

            //はずれ判定のとき　ピキーン音ならす
            sc.PlaySe(16);
        }
        else if (_guage_param >= 100 && _guage_param < 400)
        {
            GameMgr.System_magic_playParamUp = 1.0f;

            //ピキーン音ならす
            sc.PlaySe(16);
        }
        else if (_guage_param >= 400 && _guage_param < 430)
        {
            GameMgr.System_magic_playParamUp = 1.1f;

            //Sound_OK1(); //成功判定のとき　キラ音
            //ピキーン音ならす
            sc.PlaySe(16);
        }
        else if (_guage_param >= 440 && _guage_param < 490)
        {
            GameMgr.System_magic_playParamUp = 1.2f + _param_hosei;

            Sound_OK1(); //成功判定のとき　キラ音
            KiraEffect_1.SetActive(true); //さらに光りのエフェクト
            success_1 = true;
        }
        else if (_guage_param >= 490 && _guage_param < 500)
        {
            GameMgr.System_magic_playParamUp = 1.1f;

            //Sound_OK1(); //成功判定のとき　キラ音
            //ピキーン音ならす
            sc.PlaySe(16);
        }
        else if (_guage_param >= 500)
        {
            //焼すぎで失敗
            GameMgr.System_magic_playSuccess = false;
            Debug.Log("テンパリング1段階目　焼すぎで失敗");

            //失敗音ならす
            sc.PlaySe(20);
        }

        //次のバーへのフラグ
        _status = 1;

        //_guage_param2 = _guage_param;
        //_tempslider2.value = _guage_param2;
    }

    void GuageStop2()
    {

        //そのときのゲージの値によって、成功か不成功かもここで判定
        if (_guage_param2 >= 0 && _guage_param2 < 40)
        {
            GameMgr.System_magic_playParamUp2 = 0.1f;

            //冷やしすぎで失敗
            GameMgr.System_magic_playSuccess = false;
            Debug.Log("テンパリング2段階目　冷やしすぎで失敗");

            //ピキーン音ならす
            sc.PlaySe(16);
        }
        else if (_guage_param2 >= 40 && _guage_param2 < 90)
        {
            GameMgr.System_magic_playParamUp2 = 0.9f;

            //ピキーン音ならす
            sc.PlaySe(16);
        }
        else if (_guage_param2 >= 90 && _guage_param2 < 130)
        {
            GameMgr.System_magic_playParamUp2 = 1.2f + _param_hosei;
            
            Sound_OK1(); //成功判定のとき　キラ音
            KiraEffect_2.SetActive(true); //さらに光りのエフェクト
            success_2 = true;
        }
        else if (_guage_param2 >= 130 && _guage_param2 < 400)
        {
            GameMgr.System_magic_playParamUp2 = 1.1f + _param_hosei;

            //ピキーン音ならす
            sc.PlaySe(16);
            //Sound_OK1(); //成功判定のとき　キラ音
        }
        else if (_guage_param2 >= 400 && _guage_param2 < 500)
        {
            GameMgr.System_magic_playParamUp2 = 0.7f;

            //ピキーン音ならす
            sc.PlaySe(16);
        }
        else if (_guage_param2 >= 500)
        {
            //焼すぎで失敗
            GameMgr.System_magic_playSuccess = false;
            Debug.Log("テンパリング2段階目　焼すぎで失敗");

            //失敗音ならす
            sc.PlaySe(20);
        }

        //次のバーへのフラグ
        _status = 2;

        //_guage_param3 = _guage_param2;
        //_tempslider3.value = _guage_param3;
    }

    void GuageStop3()
    {
        stop_watch = false;
        Debug.Log("stop_watch: " + stop_watch);
        //演出時間はExp_Controllerで設定

        closebutton.interactable = false;

        //そのときのゲージの値によって、成功か不成功かもここで判定
        if (_guage_param3 >= 0 && _guage_param3 < 150)
        {
            GameMgr.System_magic_playParamUp3 = 0.5f;

            //ピキーン音ならす
            sc.PlaySe(16);
        }
        else if (_guage_param3 >= 150 && _guage_param3 < 300)
        {
            GameMgr.System_magic_playParamUp3 = 1.0f + _param_hosei;

            //ピキーン音ならす
            sc.PlaySe(16);
            //Sound_OK1(); //成功判定のとき　キラ音
        }
        else if (_guage_param3 >= 300 && _guage_param3 < 330)
        {
            GameMgr.System_magic_playParamUp3 = 1.2f + _param_hosei;

            if (success_1 && success_2)
            {
                //3連続成功なら特別な音                
                sc.PlaySe(14);
                sc.PlaySe(27);
                sc.PlaySe(247);

                magicstart_panel.transform.DOShakePosition(0.5f, 5f, 30, 1, false, true);
            }
            else
            {
                Sound_OK1(); //成功判定のとき　キラ音   
            }
            KiraEffect_3.SetActive(true); //さらに光りのエフェクト
            success_3 = true;
        }
        else if (_guage_param3 >= 330 && _guage_param3 < 420)
        {
            GameMgr.System_magic_playParamUp3 = 1.0f + _param_hosei;

            //ピキーン音ならす
            sc.PlaySe(16);
            //Sound_OK1(); //成功判定のとき　キラ音  
        }
        else if (_guage_param3 >= 420)
        {
            //焼すぎで失敗
            GameMgr.System_magic_playSuccess = false;
            Debug.Log("テンパリング3段階目　焼すぎで失敗");

            //失敗音ならす
            sc.PlaySe(20);
        }

        //次のバーへのフラグ
        _status = 9;
    }

    void Sound_OK1()
    {
        //成功判定のとき　キラ音
        sc.PlaySe(245);
        //sc.PlaySe(27); //ジャキン音
    }

    void Sound_OK2()
    {
        //成功判定のとき　キラ音
        sc.PlaySe(246);
        //sc.PlaySe(27); //ジャキン音
    }

    void Sound_OK3()
    {
        //成功判定のとき　キラ音
        sc.PlaySe(247);
        //sc.PlaySe(27); //ジャキン音
    }
}
