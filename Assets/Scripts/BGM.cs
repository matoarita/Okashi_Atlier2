using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BGM : MonoBehaviour {

    //[SerializeField]
    private AudioSource[] _bgm = new AudioSource[3];

    public AudioClip sound1;  //Stage1MainのBGM
    public AudioClip sound2;  //調合中のBGM
    public AudioClip sound3;  //材料採取画面
    public AudioClip sound4;  //「近くの森」BGM
    public AudioClip sound5;  //「井戸」BGM
    public AudioClip sound6;  //Stage2のBGM
    public AudioClip sound7;  //Stage3のBGM
    public AudioClip sound8;  //「ストロベリーガーデン」BGM
    public AudioClip sound9;  //「ひまわりの丘」BGM
    public AudioClip sound10;  //コンテスト時のメインBGM
    public AudioClip sound11;  //「ラベンダー畑」BGM
    public AudioClip sound12;  //「バードサンクチュアリ」BGM
    public AudioClip sound13;  //お好みBGM_01
    public AudioClip sound14;  //メインクリア後アイキャッチのBGM
    public AudioClip sound15;  //「ねこのお墓」BGM
    public AudioClip sound16;  //チュートリアルBGM
    public AudioClip sound17;  //「ベリーファーム」BGM
    public AudioClip sound18;  //Stage1MainのBGM2
    public AudioClip sound19;  //Stage1MainのBGM3
    public AudioClip sound20;  //Stage1MainのBGM4
    public AudioClip sound21;  //Stage1MainのBGM5
    public AudioClip sound22;  //Stage1MainのBGM6
    public AudioClip sound23;  //広場のBGM
    public AudioClip sound24;  //タイトルのBGM
    public AudioClip sound25;  //EDのBGM
    public AudioClip sound26;  //調合不思議なクッキングのBGM
    public AudioClip sound27;  //ショップのBGM
    public AudioClip sound28;  //モタリケ牧場のBGM
    public AudioClip sound29;  //酒場のBGM
    public AudioClip sound30;  //ピクニック1のBGM
    public AudioClip sound31;  //ピクニック2のBGM
    public AudioClip sound32;  //ピクニック3のBGM
    public AudioClip sound33;  //ピクニック帰りのBGM
    public AudioClip sound34;  //クエストに豪勢にお金をもらったときの曲
    public AudioClip sound35;  //エメラルショップのBGM
    public AudioClip sound36;  //コンテスト会場のBGM
    public AudioClip sound37;  //広場３のBGM
    public AudioClip sound38;  //大会コンテストのBGM Aランク
    public AudioClip sound39;  //オランジーナ調合メインのBGM予定1
    public AudioClip sound40;  //オランジーナ調合メインのBGM予定2

    [Range(0, 1)]
    public float _mixRate = 0;

    private int fade_status;
    private float fade_volume;
    private float _fadedeg;

    private int i;

    // Use this for initialization
    void Start () {

        //使用するAudioSource取得。２つを取得。
        _bgm = GetComponents<AudioSource>();

        fade_status = 1; //0=fade_out 1=OFF 2=fade_in
        fade_volume = 1.0f;
        _fadedeg = 0.03f; //フェードの音量減少量
        //_bgm[1]のほうに、各シーンごとのBGMを切り替えては入れて、その後_bgm[0]から切り替える

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                PlayMain();
                break;

            case "110_TotalResult":

                if(GameMgr.ending_number == 3 || GameMgr.ending_number == 4)
                {
                    EndingBGM_A();
                }
                else
                {
                    EndingBGM_B();
                }
                break;

            default:

                PlaySub();
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {

        _bgm[0].volume = (1f - _mixRate) * 0.4f * fade_volume * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                _bgm[1].volume = _mixRate * 0.4f * fade_volume * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
                break;

            case "Bar":

                _bgm[1].volume = 0.4f * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
                break;

            default:

                _bgm[1].volume = _mixRate * 0.4f * fade_volume * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
                break;
        }

        if(fade_status == 0) //フェードアウトがON
        {           
            if(fade_volume <= 0)
            {
                fade_status = 1;
            }
            else
            {
                fade_volume -= _fadedeg;
            }
        }

        if (fade_status == 2) //フェードインがON
        {
            if (fade_volume >= 1.0f)
            {
                fade_status = 1;
            }
            else
            {
                fade_volume += _fadedeg;
            }
        }

        if (fade_status == 3) //ミックスフェードで切り替え　0 -> 1
        {
            _mixRate += _fadedeg;
            if (_mixRate >= 1.0f)
            {
                _mixRate = 1;
                fade_status = 1;
            }
        }

        if (fade_status == 4) //ミックスフェードで切り替え　1 -> 0
        {
            _mixRate -= _fadedeg;
            if (_mixRate < 0.0f)
            {
                _mixRate = 0;
                fade_status = 1;
            }
        }
    }

    public void StopPlayMain() //音の一時停止などに使用。あまり使わないかも。
    {
        _bgm[0].Stop();
    }

    public void PlayMain()
    {
        BGMMainChange();

        _bgm[0].Play();

        _bgm[1].clip = sound2;
        _bgm[1].Play();
    }

    //各シーンのBGM選択
    public void PlaySub()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "001_Title":

                _bgm[0].clip = sound24;
                break;

            case "Farm":

                _bgm[0].clip = sound28;
                break;

            case "Shop":

                _bgm[0].clip = sound27;
                break;

            case "Emerald_Shop":

                _bgm[0].clip = sound35;
                break;

            case "Or_Emerald_Shop":

                _bgm[0].clip = sound35;
                break;

            case "Bar":

                _bgm[0].clip = sound29;
                break;

            case "Hiroba2":

                _bgm[0].clip = sound23;
                break;

            case "Hiroba3":

                _bgm[0].clip = sound37;
                break;

            case "Contest":

                _bgm[0].clip = sound36;
                break;

            default:

                //Debug.Log("GameMgr.Scene_Category_Num: " + GameMgr.Scene_Category_Num);
                //特定シーン以外で、シーンカテゴリーでざっくりBGMを設定する場合はここ
                //オランジーナは、こっちが中心
                switch(GameMgr.Scene_Category_Num)
                {
                    case 20: //オランジーナショップ

                        _bgm[0].clip = sound27;
                        break;

                    case 30: //オランジーナ酒場

                        _bgm[0].clip = sound29;
                        break;

                    case 60: //オランジーナ広場系

                        _bgm[0].clip = sound23;
                        break;

                    case 100: //コンテスト系

                        _bgm[0].clip = sound38;
                        break;

                    case 110: //コンテスト会場前系

                        _bgm[0].clip = sound23;
                        break;

                    default:

                        _bgm[0].clip = sound1;
                        break;
                }
                break;

        }
        _bgm[0].Play();

        
    }

    public void EndingBGM_A()
    {
        _bgm[0].clip = sound1;
        _bgm[0].Play();
    }

    public void EndingBGM_B()
    {
        _bgm[0].clip = sound2;
        _bgm[0].Play();
    }

    void BGMMainChange()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                if (GameMgr.Story_Mode == 0)
                {
                    BGMDefault();
                }
                else
                {
                    if (GameMgr.userBGM_Num == 0) //デフォルト　ユーザーが1をおした場合、デフォルトのBGM
                    {
                        BGMDefault();
                    }
                    else
                    {
                        OngakuZukanSelect();
                    }
                }
                break;

            case "Or_Compound":

                //Debug.Log("BGM　オランジーナ調合シーン");
                //_bgm[0].clip = sound39;
                _bgm[0].clip = sound40; //sound21

                break;

            default:
                break;
        }
          
    }

    void BGMDefault()
    {
        if (GameMgr.Story_Mode == 0)
        {
            switch (GameMgr.stage_number)
            {
                case 1:

                    Story_BGMSelect();
                    break;

                case 2:

                    _bgm[0].clip = sound6;
                    break;

                case 3:

                    _bgm[0].clip = sound7;
                    break;
            }
        }
        else
        {
            if (GameMgr.GirlLoveEvent_stage1[50]) //コンテストの日の曲
            {
                _bgm[0].clip = sound10;
            }
            else
            {
                _bgm[0].clip = sound21; //エクストラモード専用曲　旧: sound1
            }
        }
    }

    void Story_BGMSelect()
    {
        if (GameMgr.GirlLoveEvent_stage1[50]) //コンテストの日の曲
        {
            _bgm[0].clip = sound10;
        }
        else
        {
            if (GameMgr.GirlLoveSubEvent_stage1[60]) //HLV15~できらぽんイベント発生後
            {
                _bgm[0].clip = sound19;
            }
            else
            {
                switch (GameMgr.mainBGM_Num)
                {
                    case 0:

                        _bgm[0].clip = sound20;
                        break;

                    case 1:

                        _bgm[0].clip = sound11;
                        break;

                    case 2:

                        _bgm[0].clip = sound21;
                        break;

                    case 3:

                        _bgm[0].clip = sound1;
                        break;

                    case 4:

                        _bgm[0].clip = sound1;
                        break;

                    case 5:

                        _bgm[0].clip = sound19;
                        break;

                    default:

                        _bgm[0].clip = sound19;
                        break;
                }
            }
        }
    }

    void OngakuZukanSelect()
    {
               
        switch(GameMgr.bgm_collection_list[GameMgr.userBGM_Num].titleName)
        {
            //case 0はストーリーのデフォルト

            case "bgm2":

                _bgm[0].clip = sound20;
                break;

            case "bgm3":

                _bgm[0].clip = sound21;
                break;

            case "bgm4":

                _bgm[0].clip = sound18;
                break;

            case "bgm5":

                _bgm[0].clip = sound19;
                break;

            case "bgm6":

                _bgm[0].clip = sound10;
                break;

            case "bgm7":

                _bgm[0].clip = sound6;
                break;

            case "bgm8":

                _bgm[0].clip = sound7;
                break;

            case "bgm9":

                _bgm[0].clip = sound11;
                break;

            case "bgm10":

                _bgm[0].clip = sound1;
                break;

            case "bgm11":

                _bgm[0].clip = sound22;
                break;

            case "bgm12":

                _bgm[0].clip = sound4;
                break;

            case "bgm13":

                _bgm[0].clip = sound8;
                break;

            case "bgm14":

                _bgm[0].clip = sound17;
                break;

            case "bgm15":

                _bgm[0].clip = sound9;
                break;

            case "bgm16":

                _bgm[0].clip = sound5;
                break;

            case "bgm17":

                _bgm[0].clip = sound12;
                break;

            case "bgm18":

                _bgm[0].clip = sound15;
                break;

            case "bgm19":

                _bgm[0].clip = sound23;
                break;

            case "bgm20":

                _bgm[0].clip = sound24;
                break;

            case "bgm21":

                _bgm[0].clip = sound25;
                break;

            case "bgm22":

                _bgm[0].clip = sound26;
                break;

            case "bgm23":

                _bgm[0].clip = sound27;
                break;

            case "bgm24":

                _bgm[0].clip = sound28;
                break;

            case "bgm25":

                _bgm[0].clip = sound29;
                break;

            case "bgm26":

                _bgm[0].clip = sound30;
                break;

            case "bgm27":

                _bgm[0].clip = sound31;
                break;

            case "bgm28":

                _bgm[0].clip = sound32;
                break;

            case "bgm29":

                _bgm[0].clip = sound33;
                break;

            case "bgm30":

                _bgm[0].clip = sound2;
                break;

            default:

                Story_BGMSelect();
                break;
        }
    }

    public void OnMainBGM()
    {
        BGMMainChange();
        _bgm[0].Play();

        _mixRate = 0;
    }
    public void OnMainBGMFade()
    {
        fade_status = 4;
    }

    public void OnCompoundBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound2;
        _bgm[1].Play();

        fade_status = 3; //フェードで切り替える
        //_mixRate = 1;
    }

    public void OnGetMatStartBGM()
    {
        _bgm[0].Stop();
        _bgm[0].clip = sound3;
        _bgm[0].Play();

        //fade_status = 3;
        //_mixRate = 1;
    }

    public void OnGetMat_ForestBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound4;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_LavenderFieldBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound11;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_StrawberryGardenBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound8;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_HimawariHillBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound9;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_BirdSanctualiBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound12;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_CatGraveBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound15;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_IdoBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound5;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnGetMat_BerryFarmBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound17;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnTutorialBGM()
    {
        _bgm[1].Stop();
        _bgm[1].clip = sound16;
        _bgm[1].Play();

        _mixRate = 1;
    }

    public void OnMainClearResultBGM()
    {
        _bgm[2].clip = sound14;
        _bgm[2].volume = 0.4f * GameMgr.MasterVolumeParam * GameMgr.BGMVolumeParam;
        _bgm[2].Play();
    }

    public void OnMainClearResultBGMOFF()
    {
        _bgm[2].DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            _bgm[2].Stop();
        });
    }

    //バーで使う用
    public void PlayFanfare1()
    {
        _bgm[0].Stop();
        _bgm[1].clip = sound34;
        _bgm[1].Play();
    }

    public void StopFanfare()
    {
        _bgm[1].Stop();
    }

    public void MuteBGM()
    {
        //Debug.Log("Mute BGM");
        _bgm[0].mute = true;
        _bgm[1].mute = true;
    }

    public void MuteOFFBGM()
    {
        _bgm[0].mute = false;
        _bgm[1].mute = false;
    }

    public void FadeOutBGM()
    {
        fade_status = 0;
    }

    public void FadeInBGM()
    {
        fade_status = 2;
    }

    public void NowFadeVolumeONBGM() //ただちにフェードのボリュームをもとに戻す。
    {
        fade_volume = 1.0f;
    }

    public void NowFadeVolumeOFFBGM() //ただちにフェードのボリュームを0にする。ミュートと、効果的には一緒。
    {
        fade_volume = 0.0f;
    }
}
