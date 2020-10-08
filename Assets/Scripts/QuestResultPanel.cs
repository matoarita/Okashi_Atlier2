using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestResultPanel : MonoBehaviour {

    private GameObject questResult_obj;

    private GameObject Magic_effect_Prefab1;
    private List<GameObject> _listEffect = new List<GameObject>();

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //エフェクトプレファブの取得
        Magic_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_KiraExplode_QuestResult");

        _listEffect.Clear();

        questResult_obj = this.transform.Find("QuestResultImage").gameObject;

        _listEffect.Add(Instantiate(Magic_effect_Prefab1, this.transform));

        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        questResult_obj.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(questResult_obj.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.0f));

        //移動のアニメ
        sequence.Append(questResult_obj.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.75f)
            .SetEase(Ease.OutElastic));
        sequence.Join(questResult_obj.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }
}
