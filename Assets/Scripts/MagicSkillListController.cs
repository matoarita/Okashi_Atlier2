﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MagicSkillListController : MonoBehaviour
{
    //
    //魔法スキルのコントローラー
    //

    private GameObject canvas;

    private GameObject text_area_comp;
    private Text _text_comp;

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    public List<GameObject> _skill_listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text[] _text = new Text[3];
    private Sprite texture2d;
    private Sprite touchon, touchoff;
    private Image _Img;
    private Image _togglebg;
    private magicskillSelectToggle _toggle_itemID;
    private magicskillLearnToggle _toggle_learn_itemID;

    private Girl1_status girl1_status;

    private GameObject skill_Prefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private GameObject skill_Prefab_learn;

    private MagicSkillListDataBase magicskill_database;

    private GameObject player_patissierjob_panel;
    private GameObject skillExTextPanel;
    private Text[] skillExtext;
    private Anim_TextScroll[] skillExtextAnim;

    private GridLayoutGroup grid_layout_content;

    private int max;
    private int count;
    private int i;
    private int rnd;
    private int shop_hyouji_flag;

    //一時保存用変数
    public int skill_count; //選択したリスト番号が入る。
    public int skill_kettei_ID; //スキルデータベースIDが入る。
    public int skill_Type;
    public int skill_cost; //消費MP
    public string skill_Name;
    public string skill_itemName_Hyouji; //最終的なスキル名がはいる。

    public bool skill_final_select_flag;

    public List<GameObject> category_toggle = new List<GameObject>();
    private int category_status;
    private bool StartRead = false;

    void InitSetup()
    {
        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Viewport/Content").gameObject;
        grid_layout_content = this.transform.Find("CategoryView/Viewport/Content").GetComponent<GridLayoutGroup>();
        skill_Prefab = (GameObject)Resources.Load("Prefabs/magicskillSelectToggle");
        skill_Prefab_learn = (GameObject)Resources.Load("Prefabs/magicskillLearnToggle");

        //アイコン背景画像データの取得
        touchon = Resources.Load<Sprite>("Sprites/Window/sabwindowB");
        touchoff = Resources.Load<Sprite>("Sprites/Window/checkbox");

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //windowテキストエリアの取得
        text_area_comp = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/MessageWindowComp").gameObject;
        _text_comp = text_area_comp.GetComponentInChildren<Text>();

        if (this.gameObject.name == "MagicSkillList_ScrollView")
        {
            player_patissierjob_panel = this.transform.Find("PlayerJobPanel").gameObject;
            player_patissierjob_panel.transform.Find("player_Plv").GetComponent<Text>().text = PlayerStatus.player_patissier_lv.ToString();
            player_patissierjob_panel.transform.Find("player_jp").GetComponent<Text>().text = PlayerStatus.player_patissier_job_pt.ToString();
        }

        skillExTextPanel = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/MagicLearnPanel/SkillExTextPanel").gameObject;
        skillExtext = skillExTextPanel.transform.Find("MaskPanel").GetComponentsInChildren<Text>();
        skillExtextAnim = skillExTextPanel.transform.Find("MaskPanel").GetComponentsInChildren<Anim_TextScroll>();

        for (i = 0; i < skillExtext.Length; i++)
        {
            skillExtextAnim[i].SetInit();
        }

        //今解放済みの魔法のみ、カテゴリーViewも表示
        if (!StartRead)
        {
            i = 0;
            foreach (Transform child in this.transform.Find("CategoryView/Viewport/Content/").transform)
            {
                //Debug.Log(child.name);           
                category_toggle.Add(child.gameObject);
                category_toggle[i].SetActive(false);
                i++;
            }

            ViewFlagCheck();
        }

        //レイアウトの再配置　SetActiveのon/offだけだと、再配置されない
        grid_layout_content.enabled = false;
        grid_layout_content.enabled = true;

        i = 0;
        //category_status = 9; //基本がなくなったので、火のスキルがデフォルトになる
    }

    void ViewFlagCheck()
    {
        //光
        if (magicskill_database.skillType_SearchAllLearnCount(2) >= 1) //光先生に魔法教えてもらった
        {
            OnCateViewName("Cate_03");
        }

        //火
        if (magicskill_database.skillType_SearchAllLearnCount(9) >= 1) //
        {
            OnCateViewName("Cate_10");
        }

        //氷
        if (magicskill_database.skillType_SearchAllLearnCount(1) >= 1) //
        {
            OnCateViewName("Cate_02");
        }

        //風
        if (magicskill_database.skillType_SearchAllLearnCount(3) >= 1) //
        {
            OnCateViewName("Cate_04");
        }

        //星
        if (magicskill_database.skillType_SearchAllLearnCount(4) >= 1) //
        {
            OnCateViewName("Cate_05");
        }

        //森
        if (magicskill_database.skillType_SearchAllLearnCount(5) >= 1) //
        {
            OnCateViewName("Cate_06");
        }

        //時
        if (magicskill_database.skillType_SearchAllLearnCount(6) >= 1) //
        {
            OnCateViewName("Cate_07");
        }

        //音
        if (magicskill_database.skillType_SearchAllLearnCount(7) >= 1) //
        {
            OnCateViewName("Cate_08");
        }

        //心
        if (magicskill_database.skillType_SearchAllLearnCount(8) >= 1) //
        {
            OnCateViewName("Cate_09");
        }

        StartRead = true;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //最初開いたときのデフォルトのテキスト
    public void OnDefaultText(int _type)
    {
        InitSetup();

        switch (_type)
        {
            case 0:
                _text_comp.text = "どの魔法をつかう？　にいちゃん！";
                break;

            case 1:
                _text_comp.text = "どの魔法をおぼえようかなぁ？　にいちゃん！";
                break;
        }
        
    }

    void OnEnable()
    {

        switch (GameMgr.Scene_Category_Num)
        {
            case 10:

                CompScene_Init();
                break;

            case 100:

                CompScene_Init();
                break;
        }       
        
    }

    void CompScene_Init()
    {
        InitSetup();

        //ウィンドウがアクティヴになった瞬間だけ読み出される
        Debug.Log("OnEnable MagicPanel");

        for (i = 0; i < category_toggle.Count; i++)
        {
            category_toggle[i].GetComponent<Toggle>().isOn = false;
        }
        category_toggle[1].GetComponent<Toggle>().isOn = true;
        //SkillList_DrawView10();

           
    }

    void OnCateViewName(string _name)
    {
        for (i = 0; i < category_toggle.Count; i++)
        {
            if (category_toggle[i].name == _name)
            {
                category_toggle[i].SetActive(true);
            }
        }
    }

    public void SkillList_DrawView() //基本
    {

        category_status = 0;
        reset_and_DrawView(0);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "基本のスキルだよ！";
            }
            _text_comp.text = "基本のスキルだよ！";
        }
    }

    public void SkillList_DrawView2() //氷
    {

        category_status = 1;
        reset_and_DrawView(1);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "氷の魔法は何でも冷やす！　アイスクリームに強いよ！";
            }
            _text_comp.text = "氷の魔法は何でも冷やす！" + "\n" + "アイスや冷たいお菓子が得意だよ！";
        }
    }

    public void SkillList_DrawView3() //光
    {

        category_status = 2;
        reset_and_DrawView(2);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "光の魔法はきらきら！　フルーツやシュガー、ケーキを光らせたりできるよ！";
            }
            _text_comp.text = "光の魔法はきらきら！" + "\n" + "フルーツやシュガー、ケーキを光らせたりできるよ！";
        }
    }

    public void SkillList_DrawView4() //風
    {

        category_status = 3;
        reset_and_DrawView(3);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "風の魔法は少し特殊！　形を自由に作れるよ！　ふわふわのお菓子に強い！";
            }
            _text_comp.text = "風の魔法は少し特殊！　形を自由に作れるよ！" + "\n" + "クレープとかケーキとかのお菓子が得意だよ！";
        }
    }


    public void SkillList_DrawView5() //星
    {

        category_status = 4;
        reset_and_DrawView(4);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "星魔法は、夜や星、天気に関係する変わった魔法！　ソーダが作れる！";
            }
            _text_comp.text = "星魔法は、夜や星、天気に関係する変わった魔法だよ！" + "\n" + "飲み物に強い魔法だよ！";
        }
    }

    public void SkillList_DrawView6() //森
    {

        category_status = 5;
        reset_and_DrawView(5);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "森の魔法は、いろんな植物や木のお菓子を作ったりできるよ！";
            }
            _text_comp.text = "森の魔法は、いろんな植物や木のお菓子を作ったりできるよ！";
        }
    }

    public void SkillList_DrawView7() //時
    {

        category_status = 6;
        reset_and_DrawView(6);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "時の魔法は、時間を戻したり早めたりする、ちょっと不思議な魔法だよ！";
            }
            _text_comp.text = "時の魔法は、時間を戻したり早めたりできる！" + "\n" + "ちょっと不思議な魔法だよ！";
        }
    }

    public void SkillList_DrawView8() //音
    {

        category_status = 7;
        reset_and_DrawView(7);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "音の魔法は、食感の音や素材に眠る音を聞く.. ベテランの魔法だよ！";
            }
            _text_comp.text = "音の魔法は、食感の音や素材に眠る音を聞く.. ベテランの魔法だよ！";
        }
    }

    public void SkillList_DrawView9() //心
    {

        category_status = 8;
        reset_and_DrawView(8);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "心の魔法は、ハートをお菓子にいっぱいこめるよ！　おいしくなぁれ！";
            }
            _text_comp.text = "心の魔法は、ハートをお菓子にいっぱいこめるよ・・☆" + "\n" + "おいしくなぁれ～♪";
        }
    }

    public void SkillList_DrawView10() //火
    {

        category_status = 9;
        reset_and_DrawView(9);

        if (GameMgr.MagicSkillSelectStatus == 1) //1のときは、魔法おぼえる
        {
            for (i = 0; i < skillExtext.Length; i++)
            {
                skillExtextAnim[i].ResetStartPos();
                skillExtext[i].text = "火の魔法はお菓子の基本！　クッキーや焼き菓子に強いよ！";
            }
            _text_comp.text = "火の魔法はお菓子の基本！　クッキーや焼き菓子に強いよ！";
        }
    }

    //再度描画
    public void ReDraw()
    {       

        switch(category_status)
        {
            case 0:

                reset_and_DrawView(0);
                break;

            case 1:

                reset_and_DrawView(1);
                break;

            case 2:

                reset_and_DrawView(2);
                break;

            case 3:

                reset_and_DrawView(3);
                break;

            case 4:

                reset_and_DrawView(4);
                break;

            case 5:

                reset_and_DrawView(5);
                break;

            case 6:

                reset_and_DrawView(6);
                break;

            case 7:

                reset_and_DrawView(7);
                break;

            case 8:

                reset_and_DrawView(8);
                break;

            case 9:

                reset_and_DrawView(9);
                break;

        }

        if (this.gameObject.name == "MagicSkillList_ScrollView")
        {
            player_patissierjob_panel = this.transform.Find("PlayerJobPanel").gameObject;
            player_patissierjob_panel.transform.Find("player_Plv").GetComponent<Text>().text = PlayerStatus.player_patissier_lv.ToString();
            player_patissierjob_panel.transform.Find("player_jp").GetComponent<Text>().text = PlayerStatus.player_patissier_job_pt.ToString();
        }
    }

    // リストビューの描画部分。重要。
    void reset_and_DrawView(int _cate_status)
    {

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _skill_listitem.Clear();

        switch(_cate_status)
        {
            case 0:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 0)
                        {
                            drawSkill();
                        }
                    }
                }
                else if(GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 0)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
              
                break;

            case 1:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 1)
                        {
                            drawSkill();
                        }
                    }
                }
                else if (GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 1)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
                
                break;

            case 2:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 2)
                        {
                            drawSkill();
                        }
                    }
                }
                else if (GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 2)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
                                
                break;

            case 3:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 3)
                        {
                            drawSkill();
                        }
                    }
                }
                else if (GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 3)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
                
                break;

            case 4:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 4)
                        {
                            drawSkill();
                        }
                    }
                }
                else if (GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 4)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
                
                break;

            case 5:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 5)
                        {
                            drawSkill();
                        }
                    }
                }
                else if (GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 5)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
                
                break;

            case 6:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 6)
                        {
                            drawSkill();
                        }
                    }
                }
                else if (GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 6)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
                
                break;

            case 7:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 7)
                        {
                            drawSkill();
                        }
                    }
                }
                else if (GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 7)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
                
                break;

            case 8:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 8)
                        {
                            drawSkill();
                        }
                    }
                }
                else if (GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 8)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
                
                break;

            case 9:

                if (GameMgr.MagicSkillSelectStatus == 0)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillType == 1 &&
                            magicskill_database.magicskill_lists[i].skillLv >= 1 && magicskill_database.magicskill_lists[i].skillCategory == 9)
                        {
                            drawSkill();
                        }
                    }
                }
                else if (GameMgr.MagicSkillSelectStatus == 1)
                {
                    for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                    {
                        if (magicskill_database.magicskill_lists[i].skillFlag == 1 && magicskill_database.magicskill_lists[i].skillCategory == 9)
                        {
                            LearnJoukenCheck();
                        }
                    }
                }
                
                break;


            default:

                break;
        }
        
    }

    void LearnJoukenCheck()
    {
        //Debug.Log("magicskill_database.magicskill_lists[i].skill_Jouken_name1: " + magicskill_database.magicskill_lists[i].skill_Jouken_name1);
        if (magicskill_database.magicskill_lists[i].skill_Jouken_name1 != "Non") //前提条件がある
        {
            if (magicskill_database.magicskill_lists[i].skill_Jouken_lv1 <=
                magicskill_database.skillName_SearchLearnLevel(magicskill_database.magicskill_lists[i].skill_Jouken_name1))
            {
                drawLearnSkill();
            }
        }
        else
        {
            drawLearnSkill();
        }
    }


    void drawSkill()
    {
        _skill_listitem.Add(Instantiate(skill_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _skill_listitem[list_count].transform.Find("Background").GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、３つのテキストコンポを格納する。
        _Img = _skill_listitem[list_count].transform.Find("Background/Icon").GetComponent<Image>(); //アイテムの画像データ
        _togglebg = _skill_listitem[list_count].transform.Find("Background").GetComponent<Image>(); //アイコン背景データ

        _toggle_itemID = _skill_listitem[list_count].GetComponent<magicskillSelectToggle>();
        _toggle_itemID.toggle_skill_ID = magicskill_database.magicskill_lists[i].magicskillID; //スキルデータベース上のアイテムID。iと同じ値になる。
        _toggle_itemID.toggle_skill_type = magicskill_database.magicskill_lists[i].skillType; //スキルがパッシヴかアクティブか
        _toggle_itemID.toggle_skill_name = magicskill_database.magicskill_lists[i].skillName; //データ上のスキル名
        _toggle_itemID.toggle_skill_nameHyouji = magicskill_database.magicskill_lists[i].skillNameHyouji; //表示用の名前

        _text[0].text = magicskill_database.magicskill_lists[i].skillNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。;
        _text[1].text = magicskill_database.magicskill_lists[i].skillComment; //i = itemIDと一致する。スキルの説明文。
        _text[2].text = "Lv " + magicskill_database.magicskill_lists[i].skillLv + " / " + magicskill_database.magicskill_lists[i].skillMaxLv;
        _text[3].text = "MP " + magicskill_database.magicskill_lists[i].skillCost;

        texture2d = magicskill_database.magicskill_lists[i].skillIcon_sprite;
        _Img.sprite = texture2d;

        if(PlayerStatus.player_mp < magicskill_database.magicskill_lists[i].skillCost)
        {
            _skill_listitem[list_count].GetComponent<Toggle>().interactable = false;
        }

        //心系の魔法は、ハートも使用することがあるので、ハートもチェックする。また、スキルによっては仕上げ回数もチェック。
        switch(magicskill_database.magicskill_lists[i].skillName)
        {
            case "Cookie_SecondBake":

                if(PlayerStatus.player_extreme_kaisu == 0)
                {
                    _skill_listitem[list_count].GetComponent<Toggle>().interactable = false;
                }
                break;

            case "Warming_Handmade":

                if (PlayerStatus.player_extreme_kaisu == 0)
                {
                    _skill_listitem[list_count].GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    if (PlayerStatus.girl1_Love_exp < magicskill_database.magicskill_lists[i].skillLv * 30)
                    {
                        _skill_listitem[list_count].GetComponent<Toggle>().interactable = false;
                    }
                }
                break;
        }

        ++list_count;
    }

    void drawLearnSkill()
    {

        _skill_listitem.Add(Instantiate(skill_Prefab_learn, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        
        if(magicskill_database.magicskill_lists[i].skillLv >= 1) //習得すみのスキルは見た目が変わる
        {
            _text = _skill_listitem[list_count].transform.Find("Background_LearnOK").GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、３つのテキストコンポを格納する。
            _Img = _skill_listitem[list_count].transform.Find("Background_LearnOK/Icon").GetComponent<Image>(); //アイテムの画像データ
            _togglebg = _skill_listitem[list_count].transform.Find("Background_LearnOK").GetComponent<Image>(); //アイコン背景データ

            //LVアップと説明ボタンだけ押せるようにする。
            _skill_listitem[list_count].transform.Find("Background_LearnOK").gameObject.SetActive(true);
            _skill_listitem[list_count].transform.Find("Background").gameObject.SetActive(false);
            _skill_listitem[list_count].GetComponent<Toggle>().enabled = false;
            _skill_listitem[list_count].GetComponent<ButtonAnimTrigger>().enabled = false;
            _skill_listitem[list_count].GetComponent<Sound_Trigger>().enabled = false;
        }
        else
        {
            _text = _skill_listitem[list_count].transform.Find("Background").GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、３つのテキストコンポを格納する。
            _Img = _skill_listitem[list_count].transform.Find("Background/Icon").GetComponent<Image>(); //アイテムの画像データ
            _togglebg = _skill_listitem[list_count].transform.Find("Background").GetComponent<Image>(); //アイコン背景データ
        }

        _toggle_learn_itemID = _skill_listitem[list_count].GetComponent<magicskillLearnToggle>();
        _toggle_learn_itemID.toggle_skill_ID = magicskill_database.magicskill_lists[i].magicskillID; //スキルデータベース上のアイテムID。iと同じ値になる。
        _toggle_learn_itemID.toggle_skill_type = magicskill_database.magicskill_lists[i].skillType; //スキルがパッシヴかアクティブか
        _toggle_learn_itemID.toggle_skill_name = magicskill_database.magicskill_lists[i].skillName; //データ上のスキル名
        _toggle_learn_itemID.toggle_skill_nameHyouji = magicskill_database.magicskill_lists[i].skillNameHyouji; //表示用の名前

        _text[0].text = magicskill_database.magicskill_lists[i].skillNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。;
        //_text[1].text = magicskill_database.magicskill_lists[i].skillComment; //i = itemIDと一致する。スキルの説明文。
        _text[1].text = "Lv " + magicskill_database.magicskill_lists[i].skillLv + " / " + magicskill_database.magicskill_lists[i].skillMaxLv;
        //_text[3].text = "MP " + magicskill_database.magicskill_lists[i].skillCost; //習得のほうは、表示そもそもしない

        texture2d = magicskill_database.magicskill_lists[i].skillIcon_sprite;
        _Img.sprite = texture2d;

        //マックスLVまでとってたら、レベルアップボタンは表示しない
        if (magicskill_database.magicskill_lists[i].skillLv >= magicskill_database.magicskill_lists[i].skillMaxLv)
        {
            _skill_listitem[list_count].transform.Find("Background_LearnOK/SkillLvupButton").gameObject.SetActive(false);
        }

        //ジョブポイント足りなかったら押せない
        if (PlayerStatus.player_patissier_job_pt < 1)
        {
            _skill_listitem[list_count].GetComponent<Toggle>().interactable = false;
            _skill_listitem[list_count].transform.Find("Background_LearnOK/SkillLvupButton").gameObject.SetActive(false);
        }

        ++list_count;
    }

    public void DebugAllSkillFlagON()
    {
        magicskill_database.DebugAllSkillFlagKaikin();
        reset_and_DrawView(category_status);

        for (i = 1; i < category_toggle.Count; i++) //0は、トグルデザインのテンプレートなのでONにしない
        {
            category_toggle[i].SetActive(true);
        }
        grid_layout_content.enabled = false;
        grid_layout_content.enabled = true;
    }

    public void DebugJobPointMax()
    {
        PlayerStatus.player_patissier_job_pt = 99;
        player_patissierjob_panel.transform.Find("player_Plv").GetComponent<Text>().text = PlayerStatus.player_patissier_lv.ToString();
        player_patissierjob_panel.transform.Find("player_jp").GetComponent<Text>().text = PlayerStatus.player_patissier_job_pt.ToString();
        reset_and_DrawView(category_status);
    }
}
