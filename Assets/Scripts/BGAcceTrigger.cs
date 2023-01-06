using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using DG.Tweening;


public class BGAcceTrigger : MonoBehaviour {

    private SoundController sc;

    private Animator candle_live2d_animator;
    private Animator saboten_live2d_animator;
    private Animator minihouse_live2d_animator;

    private int trans_motion;
    private string Acce_input_name;

    private int i;
    private bool candle_onoff;
    private bool saboten_onoff;
    private bool minihouse_onoff;

    private List<string> _temp_Accename = new List<string>();

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        candle_live2d_animator = this.transform.Find("Candle/cgw01_candle_Live2D").FindCubismModel().GetComponent<Animator>();
        saboten_live2d_animator = this.transform.Find("MiniSaboten/saboten_Live2D").FindCubismModel().GetComponent<Animator>();
        minihouse_live2d_animator = this.transform.Find("MiniHouse/minihouse_Live2D").FindCubismModel().GetComponent<Animator>();

        candle_onoff = false;
        saboten_onoff = false;
        minihouse_onoff = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DrawBGAcce()
    {
        _temp_Accename.Clear();
        foreach (string key in GameMgr.BGAcceItemsName.Keys)
        {
            _temp_Accename.Add(key);
            //Debug.Log("キーは" + key + "です。");
            //ディクショナリーは、foreachしてる最中に、ディクショナリーの値を変更してしまうと、バグる。foreach使うときは、取得だけにする。
        }

        //先にフラグの処理をする。
        AcceFlagMethod();
        

        //そのあと、描画処理。フラグ用と描画用で二つ処理を記述しないといけない。
        DrawAcceMethod();
        
    }

    void AcceFlagMethod()
    {

        switch (Acce_input_name)
        {
            case "himmeli":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                { }
                else
                { }
                break;

            case "kuma_nuigurumi":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                { }
                else
                { }
                break;

            case "saboten_1":

                //Debug.Log("_temp_Accename[i] " + _temp_Accename[i]);
                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("saboten_1", true);
                    GameMgr.SetBGAcceFlag("saboten_2", false);
                    GameMgr.SetBGAcceFlag("saboten_3", false);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("saboten_1", false);
                }
                break;

            case "saboten_2":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("saboten_1", false);
                    GameMgr.SetBGAcceFlag("saboten_2", true);
                    GameMgr.SetBGAcceFlag("saboten_3", false);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("saboten_2", false);
                }
                break;

            case "saboten_3":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("saboten_1", false);
                    GameMgr.SetBGAcceFlag("saboten_2", false);
                    GameMgr.SetBGAcceFlag("saboten_3", true);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("saboten_3", false);
                }
                break;

            case "dryflowerpot_1":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("dryflowerpot_1", true);
                    GameMgr.SetBGAcceFlag("dryflowerpot_2", false);
                    GameMgr.SetBGAcceFlag("dryflowerpot_3", false);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("dryflowerpot_1", false);
                }
                break;

            case "dryflowerpot_2":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("dryflowerpot_1", false);
                    GameMgr.SetBGAcceFlag("dryflowerpot_2", true);
                    GameMgr.SetBGAcceFlag("dryflowerpot_3", false);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("dryflowerpot_2", false);
                }
                break;

            case "dryflowerpot_3":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("dryflowerpot_1", false);
                    GameMgr.SetBGAcceFlag("dryflowerpot_2", false);
                    GameMgr.SetBGAcceFlag("dryflowerpot_3", true);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("dryflowerpot_3", false);
                }
                break;

            case "aroma_candle1":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("aroma_candle1", true);
                    GameMgr.SetBGAcceFlag("aroma_candle2", false);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("aroma_candle1", false);
                }
                break;

            case "aroma_candle2":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("aroma_candle1", false);
                    GameMgr.SetBGAcceFlag("aroma_candle2", true);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("aroma_candle2", false);
                }
                break;

            case "mini_house":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                { }
                else
                { }
                break;

            case "aroma_potion1":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("aroma_potion1", true);
                    GameMgr.SetBGAcceFlag("aroma_potion2", false);
                    GameMgr.SetBGAcceFlag("aroma_potion3", false);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("aroma_potion1", false);
                }
                break;

            case "aroma_potion2":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("aroma_potion1", false);
                    GameMgr.SetBGAcceFlag("aroma_potion2", true);
                    GameMgr.SetBGAcceFlag("aroma_potion3", false);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("aroma_potion2", false);
                }
                break;

            case "aroma_potion3":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("aroma_potion1", false);
                    GameMgr.SetBGAcceFlag("aroma_potion2", false);
                    GameMgr.SetBGAcceFlag("aroma_potion3", true);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("aroma_potion3", false);
                }
                break;

            case "magic_crystal1":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("magic_crystal1", true);
                    GameMgr.SetBGAcceFlag("magic_crystal2", false);
                    GameMgr.SetBGAcceFlag("magic_crystal3", false);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("magic_crystal1", false);
                }
                break;

            case "magic_crystal2":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("magic_crystal1", false);
                    GameMgr.SetBGAcceFlag("magic_crystal2", true);
                    GameMgr.SetBGAcceFlag("magic_crystal3", false);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("magic_crystal2", false);
                }
                break;

            case "magic_crystal3":

                if (GameMgr.BGAcceItemsName[Acce_input_name])
                {
                    GameMgr.SetBGAcceFlag("magic_crystal1", false);
                    GameMgr.SetBGAcceFlag("magic_crystal2", false);
                    GameMgr.SetBGAcceFlag("magic_crystal3", true);
                }
                else
                {
                    GameMgr.SetBGAcceFlag("magic_crystal3", false);
                }
                break;
        }

    }

    void DrawAcceMethod()
    {
        //Live2Dフラグ系リセット
        candle_onoff = false;
        saboten_onoff = false;
        minihouse_onoff = false;

        for (i = 0; i < _temp_Accename.Count; i++)
        {
            switch (_temp_Accename[i])
            {
                case "himmeli":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("himmeliObj").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("himmeliObj").gameObject.SetActive(false);
                    }
                    break;

                case "kuma_nuigurumi":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("KumaNuigurumi").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("KumaNuigurumi").gameObject.SetActive(false);
                    }
                    break;

                case "saboten_1":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        //this.transform.Find("MiniSaboten/Saboten1").gameObject.SetActive(true);
                        this.transform.Find("MiniSaboten/saboten_Live2D").gameObject.SetActive(true);
                        trans_motion = 100;
                        saboten_live2d_animator.SetInteger("trans_motion", trans_motion);

                        saboten_onoff = true;
                    }
                    else
                    {
                        //this.transform.Find("MiniSaboten/Saboten1").gameObject.SetActive(false);
                    }
                    break;

                case "saboten_2":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        //this.transform.Find("MiniSaboten/Saboten2").gameObject.SetActive(true);
                        this.transform.Find("MiniSaboten/saboten_Live2D").gameObject.SetActive(true);
                        trans_motion = 200;
                        saboten_live2d_animator.SetInteger("trans_motion", trans_motion);

                        saboten_onoff = true;
                    }
                    else
                    {
                        //this.transform.Find("MiniSaboten/Saboten2").gameObject.SetActive(false);
                    }
                    break;

                case "saboten_3":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        //this.transform.Find("MiniSaboten/Saboten3").gameObject.SetActive(true);
                        this.transform.Find("MiniSaboten/saboten_Live2D").gameObject.SetActive(true);
                        trans_motion = 300;
                        saboten_live2d_animator.SetInteger("trans_motion", trans_motion);

                        saboten_onoff = true;
                    }
                    else
                    {
                        //this.transform.Find("MiniSaboten/Saboten3").gameObject.SetActive(false);
                    }
                    break;

                case "dryflowerpot_1":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("DryflowerPot/DryflowerPot1").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("DryflowerPot/DryflowerPot1").gameObject.SetActive(false);
                    }
                    break;

                case "dryflowerpot_2":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("DryflowerPot/DryflowerPot2").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("DryflowerPot/DryflowerPot2").gameObject.SetActive(false);
                    }
                    break;

                case "dryflowerpot_3":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("DryflowerPot/DryflowerPot3").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("DryflowerPot/DryflowerPot3").gameObject.SetActive(false);
                    }
                    break;

                case "aroma_candle1":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        //this.transform.Find("Candle/Candle1").gameObject.SetActive(true);
                        this.transform.Find("Candle/cgw01_candle_Live2D").gameObject.SetActive(true);
                        trans_motion = 100;
                        candle_live2d_animator.SetInteger("trans_motion", trans_motion);

                        candle_onoff = true;
                    }
                    else
                    {
                        //this.transform.Find("Candle/Candle1").gameObject.SetActive(false);
                        //this.transform.Find("Candle/cgw01_candle_Live2D").gameObject.SetActive(false);
                    }
                    break;

                case "aroma_candle2":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        //this.transform.Find("Candle/Candle2").gameObject.SetActive(true);
                        this.transform.Find("Candle/cgw01_candle_Live2D").gameObject.SetActive(true);
                        trans_motion = 200;
                        candle_live2d_animator.SetInteger("trans_motion", trans_motion);

                        candle_onoff = true;
                    }
                    else
                    {
                        //this.transform.Find("Candle/Candle2").gameObject.SetActive(false);
                        //this.transform.Find("Candle/cgw01_candle_Live2D").gameObject.SetActive(false);
                    }
                    break;

                case "mini_house":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        //this.transform.Find("MiniHouse/MiniHouse1").gameObject.SetActive(true);
                        this.transform.Find("MiniHouse/minihouse_Live2D").gameObject.SetActive(true);
                        trans_motion = 100;
                        minihouse_live2d_animator.SetInteger("trans_motion", trans_motion);

                        minihouse_onoff = true;
                    }
                    else
                    {
                        //this.transform.Find("MiniHouse/MiniHouse1").gameObject.SetActive(false);
                    }
                    break;

                case "aroma_potion1":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("AromaPotion/AromaPotion1").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("AromaPotion/AromaPotion1").gameObject.SetActive(false);
                    }
                    break;

                case "aroma_potion2":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("AromaPotion/AromaPotion2").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("AromaPotion/AromaPotion2").gameObject.SetActive(false);
                    }
                    break;

                case "aroma_potion3":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("AromaPotion/AromaPotion3").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("AromaPotion/AromaPotion3").gameObject.SetActive(false);
                    }
                    break;

                case "magic_crystal1":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("MagicCrystal/MagicCrystal1").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("MagicCrystal/MagicCrystal1").gameObject.SetActive(false);
                    }
                    break;

                case "magic_crystal2":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("MagicCrystal/MagicCrystal2").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("MagicCrystal/MagicCrystal2").gameObject.SetActive(false);
                    }
                    break;

                case "magic_crystal3":

                    if (GameMgr.BGAcceItemsName[_temp_Accename[i]])
                    {
                        this.transform.Find("MagicCrystal/MagicCrystal3").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.Find("MagicCrystal/MagicCrystal3").gameObject.SetActive(false);
                    }
                    break;
            }

            if(!candle_onoff)
            {
                this.transform.Find("Candle/cgw01_candle_Live2D").gameObject.SetActive(false);
            }
            if (!saboten_onoff)
            {
                this.transform.Find("MiniSaboten/saboten_Live2D").gameObject.SetActive(false);
            }
            if (!minihouse_onoff)
            {
                this.transform.Find("MiniHouse/minihouse_Live2D").gameObject.SetActive(false);
            }
        }
    }

    public void BGAcceOn(string _accenum)
    {

        GameMgr.BGAcceItemsName[_accenum] = !GameMgr.BGAcceItemsName[_accenum]; //OnとOffを切り替え
        Acce_input_name = _accenum;

        if (GameMgr.BGAcceItemsName[_accenum])
        {
            //飾りONのときの音
            sc.PlaySe(33); //ポニョ音

        }
        else
        {
            //飾りOFFのときの音[
            sc.PlaySe(18);
        }

        //描画更新
        DrawBGAcce();
    }

    //朝～夜で状態が変わる
    public void WeatherChangeMorning()
    {
        //アロマキャンドル
        if (GameMgr.BGAcceItemsName["aroma_candle1"])
        {
            trans_motion = 110;
            candle_live2d_animator.SetInteger("trans_motion", trans_motion);
        }
        else
        { }

        if (GameMgr.BGAcceItemsName["aroma_candle2"])
        {
            trans_motion = 210;
            candle_live2d_animator.SetInteger("trans_motion", trans_motion);
        }
        else
        { }

        //ミニ家
        if (GameMgr.BGAcceItemsName["mini_house"])
        {
            trans_motion = 100;
            minihouse_live2d_animator.SetInteger("trans_motion", trans_motion);
        }
        else
        { }
    }

    public void WeatherChangeNight()
    {

        //アロマキャンドル
        if (GameMgr.BGAcceItemsName["aroma_candle1"])
        {
            trans_motion = 100;
            candle_live2d_animator.SetInteger("trans_motion", trans_motion);
        }
        else
        { }

        if (GameMgr.BGAcceItemsName["aroma_candle2"])
        {
            trans_motion = 200;
            candle_live2d_animator.SetInteger("trans_motion", trans_motion);
        }
        else
        { }

        //ミニ家
        if (GameMgr.BGAcceItemsName["mini_house"])
        {
            trans_motion = 110;
            minihouse_live2d_animator.SetInteger("trans_motion", trans_motion);
        }
        else
        { }
    }
}
