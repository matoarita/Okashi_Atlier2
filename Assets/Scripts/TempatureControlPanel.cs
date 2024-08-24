using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempatureControlPanel : MonoBehaviour {

    //Compound_BGPanel_AをONにしたとき、TempatureControlPanelも、常にONの状態にして、Updateを監視できるようにしとく。
    //CompoundMainControllerのPrefab更新時は注意（Compound_BGPanel_A自体はオフにしてて大丈夫。）

    private MagicSkillListDataBase magicskill_database;

    private GameObject canvas;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private BGM sceneBGM;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private Slider time_slider;

    private GameObject temp_hari;
    private Transform temp_hariTransform;
    private Vector3 localAngle1;

    private float angle_f;

    private string _skillname;

    private int _tempature, _time;
    private int _tempMax, _tempMin, _timeMax, _timeMin;
    private Text _text_temp, _text_time;


    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //テキストウィンドウの取得
        text_area = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/MessageWindowComp").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //各ゲージ
        _text_temp = this.transform.Find("Comp/ParameterPanel/temp_counter/counter_num").GetComponent<Text>();
        _text_time = this.transform.Find("Comp/ParameterPanel/time_counter/counter_num").GetComponent<Text>();

        time_slider = this.transform.Find("Comp/ParameterPanel/TimeWatch/TimeSlider").GetComponent<Slider>();
        temp_hari = this.transform.Find("Comp/ParameterPanel/TempatureWatch/TempClockRoot").gameObject;
        temp_hariTransform = temp_hari.transform;
        localAngle1 = temp_hariTransform.localEulerAngles;

        _tempMax = GameMgr.System_tempature_control_tempMax; //230
        _tempMin = GameMgr.System_tempature_control_tempMin; //150
        _timeMax = 60;
        _timeMin = 0;


        GameMgr.System_tempature_control_Param_temp = _tempMin;
        GameMgr.System_tempature_control_Param_time = 30;       

        _text_temp.text = GameMgr.System_tempature_control_Param_temp.ToString();
        _text_time.text = GameMgr.System_tempature_control_Param_time.ToString();

        time_slider.maxValue = _timeMax;
        time_slider.value = GameMgr.System_tempature_control_Param_time;

        //温度計表示    
        TempClockHyouji();
    }
	
	// Update is called once per frame
	void Update () {
		
        if(GameMgr.tempature_control_select_flag)
        {
            GameMgr.tempature_control_select_flag = false;

            GameMgr.compound_status = 110;
            this.transform.Find("Comp").gameObject.SetActive(true);
            this.transform.Find("Comp/Yes_no_Panel_temp").gameObject.SetActive(true);

            _skillname = magicskill_database.magicskill_lists[magicskill_database.SearchSkillString("Temperature_of_Control")].skillNameHyouji;
            _text.text = "温度と時間を設定してね。" + "\n" + "※0分に設定すると、「" + _skillname + "」を使用しない。";

            //環境音鳴らす
            sceneBGM.PlayAmbient(1);

            StartCoroutine("Tempature_select"); //決定かキャンセルを待つ状態
        }

        if(GameMgr.tempature_control_Offflag)
        {
            GameMgr.tempature_control_Offflag = false;

            this.transform.Find("Comp").gameObject.SetActive(false);
        }
	}

    IEnumerator Tempature_select()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        //環境音とめる
        sceneBGM.StopAmbient();

        switch (yes_selectitem_kettei.kettei1)
        {

            case true:

                //Debug.Log("ok");

                GameMgr.final_select_flag = true; //オリジナル調合の最終確認 Compound_Checkで確認

                this.transform.Find("Comp/Yes_no_Panel_temp").gameObject.SetActive(false); //yes noボタンだけオフ
                break;

            case false:

                Debug.Log("温度管理画面をcancel");

                GameMgr.compound_status = 100;
                this.transform.Find("Comp").gameObject.SetActive(false);

                if(GameMgr.Comp_kettei_bunki == 2) //二個選択から来ていた場合
                {
                    itemselect_cancel.Two_cancel();
                }
                else if (GameMgr.Comp_kettei_bunki == 3) //三個選択から来ていた場合
                {
                    itemselect_cancel.Three_cancel();
                }

                break;
        }

    }

    void DrawParam()
    {
        _text_temp.text = GameMgr.System_tempature_control_Param_temp.ToString();
        _text_time.text = GameMgr.System_tempature_control_Param_time.ToString();

        time_slider.value = GameMgr.System_tempature_control_Param_time;

        //温度計表示    
        TempClockHyouji();        
    }

    void TempClockHyouji()
    {
        angle_f = SujiMap(GameMgr.System_tempature_control_Param_temp, GameMgr.System_tempature_control_tempMin, GameMgr.System_tempature_control_tempMax,
            120, -120); //150~230 を　角度　-120~120に変換
        localAngle1.z = angle_f; // ローカル座標を基準に、z軸を軸にした回転
        temp_hariTransform.localEulerAngles = localAngle1; // 回転角度を設定
    }

    //温度を→で増減する
    public void Upcount_Temp()
    {
        GameMgr.System_tempature_control_Param_temp += 10;

        if(GameMgr.System_tempature_control_Param_temp >= _tempMax)
        {
            GameMgr.System_tempature_control_Param_temp = _tempMax;
        }

        DrawParam();
    }

    public void Downcount_Temp()
    {
        GameMgr.System_tempature_control_Param_temp -= 10;

        if (GameMgr.System_tempature_control_Param_temp <= _tempMin)
        {
            GameMgr.System_tempature_control_Param_temp = _tempMin;
        }

        DrawParam();
    }

    public void Upcount_TempBig()
    {
        GameMgr.System_tempature_control_Param_temp += 1;

        if (GameMgr.System_tempature_control_Param_temp >= _tempMax)
        {
            GameMgr.System_tempature_control_Param_temp = _tempMax;
        }

        DrawParam();
    }

    public void Downcount_TempBig()
    {
        GameMgr.System_tempature_control_Param_temp -= 1;

        if (GameMgr.System_tempature_control_Param_temp <= _tempMin)
        {
            GameMgr.System_tempature_control_Param_temp = _tempMin;
        }

        DrawParam();
    }

    //時間を→で増減する
    public void Upcount_Timer()
    {
        GameMgr.System_tempature_control_Param_time += 10;

        if (GameMgr.System_tempature_control_Param_time >= _timeMax)
        {
            GameMgr.System_tempature_control_Param_time = _timeMax;
        }

        DrawParam();
    }

    public void Downcount_Timer()
    {
        GameMgr.System_tempature_control_Param_time -= 10;

        if (GameMgr.System_tempature_control_Param_time <= _timeMin)
        {
            GameMgr.System_tempature_control_Param_time = _timeMin;
        }

        DrawParam();
    }

    public void Upcount_TimerBig()
    {
        GameMgr.System_tempature_control_Param_time += 1;

        if (GameMgr.System_tempature_control_Param_time >= _timeMax)
        {
            GameMgr.System_tempature_control_Param_time = _timeMax;
        }

        DrawParam();
    }

    public void Downcount_TimerBig()
    {
        GameMgr.System_tempature_control_Param_time -= 1;

        if (GameMgr.System_tempature_control_Param_time <= _timeMin)
        {
            GameMgr.System_tempature_control_Param_time = _timeMin;
        }

        DrawParam();
    }

    public void OnTimeSlider_ValueChange()
    {

    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
