using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ContestFirstEnshutuPanel : MonoBehaviour
{
    private GameObject PanelContestName_obj;
    private GameObject PanelReady_obj;
    private GameObject PanelStart_obj;

    private TextMeshProUGUI contest_name;

    // Start is called before the first frame update
    void Start()
    {
        PanelContestName_obj = this.transform.Find("PanelContestName").gameObject;
        PanelReady_obj = this.transform.Find("PanelReady").gameObject;
        PanelStart_obj = this.transform.Find("PanelStart").gameObject;

        contest_name = PanelContestName_obj.transform.Find("TextContestName").GetComponent<TextMeshProUGUI>();
        contest_name.text = GameMgr.Contest_EnshutuNameHyouji;

        PanelContestName_obj.SetActive(false);
        PanelReady_obj.SetActive(false);
        PanelStart_obj.SetActive(false);

        this.GetComponent<CanvasGroup>().DOFade(1, 0.0f);
        this.transform.Find("BGBlack").GetComponent<CanvasGroup>().DOFade(0.3f, 0.0f);
        StartCoroutine("OnStartAnimWait");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetContestName()
    {
        PanelContestName_obj = this.transform.Find("PanelContestName").gameObject;
        contest_name = PanelContestName_obj.transform.Find("TextContestName").GetComponent<TextMeshProUGUI>();
        contest_name.text = GameMgr.Contest_EnshutuNameHyouji;
    }

    IEnumerator OnStartAnimWait()
    {
        yield return new WaitForSeconds(1.0f); //ÉèÉìÉeÉìÉ|Ç®Ç≠

        this.transform.Find("BGBlack").GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        PanelContestName_obj.SetActive(true);
    }

    void OnStartAnim1()
    {
        
    }

    public void OnStartAnim2()
    {
        PanelContestName_obj.SetActive(false);
        PanelReady_obj.SetActive(true);
    }

    public void OnStartAnim3()
    {
        PanelReady_obj.SetActive(false);
        PanelStart_obj.SetActive(true);
    }

    public void OnEndAnim()
    {       
        EndAnim();
    }

    void EndAnim()
    {
        StartCoroutine("EndAnimWait");
        
    }

    IEnumerator EndAnimWait()
    {
        yield return new WaitForSeconds(0.2f);

        this.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(OffObj);
    }

    void OffObj()
    {
        GameMgr.ContestStartEnshutu_Flag = false;
        this.gameObject.SetActive(false);
    }
}
