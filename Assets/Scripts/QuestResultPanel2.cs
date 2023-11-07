﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestResultPanel2 : MonoBehaviour {

    private GameObject canvas;

    private GameObject questResult_obj;

    private GameObject Magic_effect_Prefab1;
    private List<GameObject> _listEffect = new List<GameObject>();

    private GameObject quest_Judge_CanvasPanel;
    private GameObject WhiteFade;
    private GameObject WhiteFadeCanvas;

    private GameObject questjudge_obj;
    private Quest_Judge questjudge;

    private GameObject ResultButton;

    private int i;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //エフェクトプレファブの取得
        Magic_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_KiraExplode_QuestResult");

        _listEffect.Clear();

        quest_Judge_CanvasPanel = canvas.transform.Find("Quest_Judge_CanvasPanel").gameObject;

        questResult_obj = this.transform.Find("QuestResultImage").gameObject;
        WhiteFade = this.transform.Find("WhiteEffect").gameObject;
        WhiteFadeCanvas = quest_Judge_CanvasPanel.transform.Find("WhiteFadeCanvas").gameObject;
        ResultButton = this.transform.Find("QuestResultCloseButton").gameObject;
        ResultButton.SetActive(false);

        questjudge_obj = GameObject.FindWithTag("Quest_Judge");
        questjudge = questjudge_obj.GetComponent<Quest_Judge>();

        if (GameMgr.bar_quest_okashiScore < GameMgr.low_score) //60点より下だとエフェクトがでない
        { }
        else
        {
            _listEffect.Add(Instantiate(Magic_effect_Prefab1, this.transform));
        }

        //ぽよんと飛び出るアニメーション
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        questResult_obj.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(questResult_obj.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.0f));
       
        WhiteFadeCanvas.GetComponent<CanvasGroup>().alpha = 0;
        WhiteFadeCanvas.SetActive(false);
        //WhiteFade.GetComponent<CanvasGroup>().alpha = 1;

        //白でフラッシュ
        //sequence.Append(WhiteFade.GetComponent<CanvasGroup>().DOFade(0, 0.3f));
        //移動のアニメ
        sequence.Append(questResult_obj.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.75f)
            .SetEase(Ease.OutElastic));
        sequence.Join(questResult_obj.GetComponent<CanvasGroup>().DOFade(1, 0.2f));

        StartCoroutine("AfterButtonOn");
    }

    private void OnDisable()
    {
        // 子オブジェクトをループして取得
        for (i=0; i<_listEffect.Count; i++)
        {
            // 一つずつ破棄する
            Destroy(_listEffect[(_listEffect.Count-1) - i].gameObject);
        }
        _listEffect.Clear();
    }

    IEnumerator AfterButtonOn()
    {

        yield return new WaitForSeconds(2f); //2秒待つ

        ResultButton.SetActive(true);
    }

    //結果確認のボタンを押した
    public void OnEndResultButton()
    {
        questjudge.OnEndResultButton();
    }
}
