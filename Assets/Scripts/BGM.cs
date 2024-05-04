using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BGM : MonoBehaviour {


    private BGMController bgmController;

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
    public AudioClip sound38;  //大会コンテストのBGM エデンコンテスト
    public AudioClip sound39;  //オランジーナ調合メインのBGM予定1
    public AudioClip sound40;  //オランジーナ調合メインのBGM予定2
    public AudioClip sound41;  //オランジーナ街BGM
    public AudioClip sound42;  //魔法の先生のテーマ01
    public AudioClip sound43;  //秋エリアBGM
    public AudioClip sound44;  //冬エリアBGM
    public AudioClip sound45;  //夏エリア遊園地BGM
    public AudioClip sound46;  //大会コンテストのBGM 初級
    public AudioClip sound47;  //大会コンテスト受付のBGM
    public AudioClip sound48;  //「サクラフォレスト」BGM
    public AudioClip sound49;  //夏エリアBGM
    public AudioClip sound50;  //魔法の先生のテーマ02
    public AudioClip sound51;  //秘密の花園テーマ
    public AudioClip sound1000;  //空のサウンド

    private AudioClip _send_clip;

    private int i;

    // Use this for initialization
    void Start () {

        bgmController = BGMController.Instance.GetComponent<BGMController>();

    }
	
	// Update is called once per frame
	void Update () {
        
    }
    
    public void PlayMain()
    {
        BGMMainChange();

        bgmController.BGMPlay(0, _send_clip);
        bgmController.BGMPlay(1, sound2);

    }

    //各シーンのBGM選択
    public void PlaySub()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "001_Title":

                _send_clip = sound24;
                break;

            case "Farm":

                _send_clip = sound28;
                break;

            case "Shop":

                _send_clip = sound27;
                break;

            case "Emerald_Shop":

                _send_clip = sound35;
                break;

            case "Or_Emerald_Shop":

                _send_clip = sound35;
                break;

            case "Bar":

                _send_clip = sound29;
                break;

            case "Hiroba2":

                _send_clip = sound23;
                break;

            case "Hiroba3":

                _send_clip = sound37;
                break;

            case "Contest":

                _send_clip = sound36;
                break;

            case "GetMaterial":

                //シーン最初は無音。GetMatPlace_Panelで決めてる。
                break;

            case "Station":

                _send_clip = sound23;
                break;

            default:

                //Debug.Log("GameMgr.Scene_Category_Num: " + GameMgr.Scene_Category_Num);
                //特定シーン以外で、シーンカテゴリーでざっくりBGMを設定する場合はここ

                //オランジーナは、こっちが中心
                switch(GameMgr.Scene_Category_Num)
                {
                    case 11: //アトリエ前

                        _send_clip = sound41;
                        break;

                    case 20: //オランジーナショップ

                        _send_clip = sound27;
                        break;

                    case 30: //オランジーナ酒場

                        _send_clip = sound29;
                        break;

                    case 40: //オランジーナファーム

                        _send_clip = sound28;
                        break;

                    case 60: //オランジーナ広場系

                        switch (GameMgr.Scene_Name)
                        {
                            case "Or_Hiroba_CentralPark": //中央噴水

                                _send_clip = sound41;
                                break;

                            case "Or_Hiroba_CentralPark2": //中央噴水のお散歩小道

                                _send_clip = sound37;
                                break;

                            case "Or_Hiroba_Spring_Entrance": //春のエリア入口

                                _send_clip = sound41;
                                break;

                            case "Or_Hiroba_Spring_Shoping_Moll": //春のエリア商店街

                                _send_clip = sound41;
                                break;

                            case "Or_Hiroba_Spring_Oku": //秘密の花園

                                _send_clip = sound51;
                                break;

                            case "Or_Hiroba_Spring_UraStreet": //春のエリア裏通り

                                _send_clip = sound41;
                                break;

                            case "Or_Hiroba_Summer_Entrance": //夏のエリア入口

                                _send_clip = sound49;
                                break;

                            case "Or_Hiroba_Summer_Street": //夏のエリア入口　奥側

                                _send_clip = sound49;
                                break;

                            case "Or_Hiroba_Summer_MainStreet": //夏のエリア　メインストリート

                                _send_clip = sound49;
                                break;

                            case "Or_Hiroba_Summer_MainStreet_Shop": //夏のエリア　メインストリート

                                _send_clip = sound49;
                                break;

                            case "Or_Hiroba_Summer_ThemePark_Map": //夏エリア　遊園地　全体マップ

                                _send_clip = sound45;
                                break;

                            case "Or_Hiroba_Summer_ThemePark_Enter": //夏エリア　遊園地入口

                                _send_clip = sound45;
                                break;

                            case "Or_Hiroba_Summer_ThemePark_StreetA": //夏エリア　遊園地　右の通り

                                _send_clip = sound45;
                                break;

                            case "Or_Hiroba_Autumn_Entrance": //秋のエリア入口

                                _send_clip = sound43;
                                break;

                            case "Or_Hiroba_Autumn_Entrance_bridge": //秋エリア　入口大橋

                                _send_clip = sound1000;
                                break;

                            case "Or_Hiroba_Autumn_MainStreet": //秋エリア　メインストリート

                                _send_clip = sound43;
                                break;

                            case "Or_Hiroba_Autumn_DepartMae": //秋エリア　百貨店前

                                _send_clip = sound43;
                                break;

                            case "Or_Hiroba_Autumn_BarStreet": //秋エリア　酒場通り

                                _send_clip = sound43;
                                break;

                            case "Or_Hiroba_Autumn_UraStreet": //秋エリア　裏通り

                                _send_clip = sound43;
                                break;

                            case "Or_Hiroba_Autumn_UraStreet2": //秋エリア　裏通り奥

                                _send_clip = sound43;
                                break;

                            case "Or_Hiroba_Winter_Entrance": //冬のエリア入口　雪道

                                _send_clip = sound44;
                                break;

                            case "Or_Hiroba_Winter_EntranceHiroba": //冬のエリア入口

                                _send_clip = sound44;
                                break;

                            case "Or_Hiroba_Winter_Street1": //冬のエリア入口から奥の広場通り

                                _send_clip = sound44;
                                break;

                            case "Or_Hiroba_Winter_MainStreet": //冬のエリア入口から奥の広場通り

                                _send_clip = sound44;
                                break;

                            case "Or_Hiroba_Winter_MainHiroba": //冬のエリア入口から奥の広場通り

                                _send_clip = sound44;
                                break;

                            case "Or_Hiroba_Winter_Street2": //冬のエリア入口から奥の広場通り

                                _send_clip = sound1000;
                                break;

                            case "Or_Hiroba_Winter_ContestBridge": //冬のエリア入口から奥の広場通り

                                _send_clip = sound1000;
                                break;

                            case "Or_Hiroba_Winter_Street3": //冬のエリア入口から奥の広場通り

                                _send_clip = sound44;
                                break;

                            case "Or_Hiroba_Winter_PatissierHouseMae": //冬のエリア入口から奥の広場通り

                                _send_clip = sound44;
                                break;

                            case "Or_Hiroba_Catsle_Garden": //城エリア　大通り前庭

                                _send_clip = sound44;
                                break;

                            case "Or_Hiroba_Catsle_MainStreet": //城エリア　大通り

                                _send_clip = sound44;
                                break;

                            default:

                                break;
                        }
                        break;


                    case 100: //コンテスト系

                        switch(GameMgr.Contest_Name)
                        {
                            case "Or_Contest_001_1":

                                _send_clip = sound38;
                                break;

                            case "Or_Contest_001_2":

                                _send_clip = sound38;
                                break;

                            case "Or_Contest_001_3":

                                _send_clip = sound38;
                                break;

                            default:

                                _send_clip = sound46;
                                break;
                        }
                        
                        break;

                    case 110: //コンテスト会場前系

                        switch (GameMgr.Scene_Name)
                        {
                            case "Or_Contest_Out_Spring":

                                _send_clip = sound51;
                                break;

                            case "Or_Contest_Out_Summer":

                                _send_clip = sound41;
                                break;

                            case "Or_Contest_Out_Autumn":

                                _send_clip = sound41;
                                break;

                            case "Or_Contest_Out_Winter":

                                _send_clip = sound41;
                                break;

                        }
                        
                        break;

                    case 120: //コンテスト会場受付系

                        _send_clip = sound47;
                        break;

                    case 150: //NPCの家系

                        _send_clip = sound50;
                        break;

                    case 160: //城のBGM

                        _send_clip = sound42;
                        break;

                    default:

                        _send_clip = sound1;
                        break;
                }
                break;

        }

        bgmController.BGMPlay(0, _send_clip);
        bgmController.MixRateChange(0); //bgm[0]に音を切り替える


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
                _send_clip = sound40;

                break;

            default:

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

                    _send_clip = sound6;
                    break;

                case 3:

                    _send_clip = sound7;
                    break;
            }
        }
        else
        {
            if (GameMgr.GirlLoveEvent_stage1[50]) //コンテストの日の曲
            {
                _send_clip = sound10;
            }
            else
            {

                _send_clip = sound21; //エクストラモード専用曲　旧: sound1
            }
        }
    }

    void Story_BGMSelect()
    {
        if (GameMgr.GirlLoveEvent_stage1[50]) //コンテストの日の曲
        {
            _send_clip = sound10;
        }
        else
        {
            if (GameMgr.GirlLoveSubEvent_stage1[60]) //HLV15~できらぽんイベント発生後
            {
                _send_clip = sound19;
            }
            else
            {
                switch (GameMgr.mainBGM_Num)
                {
                    case 0:

                        _send_clip = sound20;
                        break;

                    case 1:
                        _send_clip = sound11;
                        break;

                    case 2:

                        _send_clip = sound21;
                        break;

                    case 3:

                        _send_clip = sound1;
                        break;

                    case 4:

                        _send_clip = sound1;
                        break;

                    case 5:

                        _send_clip = sound19;
                        break;

                    default:

                        _send_clip = sound19;
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

                _send_clip = sound20;
                break;

            case "bgm3":

                _send_clip = sound21;
                break;

            case "bgm4":

                _send_clip = sound18;
                break;

            case "bgm5":

                _send_clip = sound19;
                break;

            case "bgm6":

                _send_clip = sound10;
                break;

            case "bgm7":

                _send_clip = sound6;
                break;

            case "bgm8":

                _send_clip = sound7;
                break;

            case "bgm9":

                _send_clip = sound11;
                break;

            case "bgm10":

                _send_clip = sound1;
                break;

            case "bgm11":

                _send_clip = sound22;
                break;

            case "bgm12":

                _send_clip = sound4;
                break;

            case "bgm13":

                _send_clip = sound8;
                break;

            case "bgm14":

                _send_clip = sound17;
                break;

            case "bgm15":

                _send_clip = sound9;
                break;

            case "bgm16":

                _send_clip = sound5;
                break;

            case "bgm17":

                _send_clip = sound12;
                break;

            case "bgm18":

                _send_clip = sound15;
                break;

            case "bgm19":

                _send_clip = sound23;
                break;

            case "bgm20":

                _send_clip = sound24;
                break;

            case "bgm21":

                _send_clip = sound25;
                break;

            case "bgm22":

                _send_clip = sound26;
                break;

            case "bgm23":

                _send_clip = sound27;
                break;

            case "bgm24":

                _send_clip = sound28;
                break;

            case "bgm25":

                _send_clip = sound29;
                break;

            case "bgm26":

                _send_clip = sound30;
                break;

            case "bgm27":

                _send_clip = sound31;
                break;

            case "bgm28":

                _send_clip = sound32;
                break;

            case "bgm29":

                _send_clip = sound33;
                break;

            case "bgm30":

                _send_clip = sound2;
                break;

            default:

                Story_BGMSelect();
                break;
        }
    }

    public void OnMainBGM()
    {
        BGMMainChange();

        bgmController.BGMRestartPlay(0, _send_clip);
        bgmController.MixRateChange(0);
    }

    //メインBGMと調合時のBGMシーンをミックスしながら切り替える
    public void OnChangeCompoBGMFade()
    {
        bgmController.FadeStatusChange(4);
    }

    public void OnCompoundBGM()
    {
        bgmController.BGMStop(1);
        bgmController.BGMPlay(1, sound2);

        bgmController.FadeStatusChange(3);
    }

    public void OnGetMatStartBGM()
    {
        bgmController.BGMStop(0);
        bgmController.BGMPlay(0, sound3);

    }

    public void OnGetMat_MapBGM(int _sound_num)
    {
        bgmController.BGMStop(1);

        switch (_sound_num)
        {
            case 0: //近くの森

                _send_clip = sound4;
                break;

            case 1: //ラベンダー畑

                _send_clip = sound11;
                break;

            case 2: //ストロベリーガーデン

                _send_clip = sound8;
                break;

            case 3: //ベリーファーム

                _send_clip = sound17;
                break;

            case 4: //ひまわり畑

                _send_clip = sound9;
                break;

            case 5: //バードサンクチュアリ

                _send_clip = sound12;
                break;

            case 6: //白猫のお墓

                _send_clip = sound15;
                break;

            case 7: //井戸

                _send_clip = sound5;
                break;

            case 100: //サクラフォレスト

                _send_clip = sound48;
                break;
        }

        bgmController.BGMRestartPlay(0, _send_clip);
        //bgmController.MixRateChange(1);

    }


    public void OnTutorialBGM()
    {
        bgmController.BGMStop(1);
        bgmController.BGMPlay(1, sound16);
        bgmController.MixRateChange(1);

        //_mixRate = 1;
    }

    public void OnMainClearResultBGM()
    {
        bgmController.BGMPlay(2, sound4);
        bgmController.BGMVolume(2);
    }

    public void OnMainClearResultBGMOFF()
    {
        bgmController.DoFadeBGM(2);
    }

    public void OnEndingBGM()
    {
        if (GameMgr.ending_number == 3 || GameMgr.ending_number == 4)
        {
            EndingBGM_A();
        }
        else
        {
            EndingBGM_B();
        }
    }

    void EndingBGM_A()
    {
        bgmController.BGMPlay(0, sound1);

    }

    void EndingBGM_B()
    {
        bgmController.BGMPlay(0, sound2);
    }

    //バーで使う用
    public void PlayFanfare1()
    {
        bgmController.BGMStop(0);
        bgmController.BGMPlay(1, sound34);
    }

    public void StopFanfare()
    {
        bgmController.BGMStop(1);
    }


    public void StopBGM(int _num) //音をStopで停止
    {
        bgmController.BGMStop(_num);
    }

    public void MuteBGM()
    {
        //Debug.Log("Mute BGM");
        bgmController.BGMMute(0, 0); //2番目が0ならMute
        bgmController.BGMMute(1, 0);
    }

    public void MuteOFFBGM()
    {
        bgmController.BGMMute(0, 1); //2番目が1ならMuteOFF
        bgmController.BGMMute(1, 1);
    }

    public void FadeOutBGM(float _time)
    {
        //bgmController.DoFadeBGM(0);
        //bgmController.FadeStatusChange(0);
        bgmController.DoFadeVolumeOut(_time);
    }

    public void FadeInBGM(float _time)
    {
        //bgmController.FadeStatusChange(2);
        bgmController.DoFadeVolumeIn(_time);
    }

    public void NowFadeVolumeONBGM() //ただちにフェードのボリュームをもとに戻す。
    {
        bgmController.FadeStatusChange(100); //フェード途中の場合は、強制的に待機状態にして、1にすぐ切り替える
        bgmController.FadeVolumeChange(1.0f);
    }

    public void NowFadeVolumeOFFBGM() //ただちにフェードのボリュームを0にする。ミュートと、効果的には一緒。
    {
        bgmController.FadeStatusChange(100);
        bgmController.FadeVolumeChange(0.0f);
    }
}
