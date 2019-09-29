using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainListController : MonoBehaviour {

    private GameObject Atlier_toggle_obj;
    private GameObject Shop_toggle_obj;
    private GameObject GirlEat_toggle_obj;
    private GameObject Travel_toggle_obj;
    private GameObject QuestBox_toggle_obj;
    private GameObject Save_toggle_obj;
    private GameObject Load_toggle_obj;

    private Toggle Atlier_toggle;
    private Toggle Shop_toggle;
    private Toggle GirlEat_toggle;
    private Toggle Travel_toggle;
    private Toggle QuestBox_toggle;
    private Toggle Save_toggle;
    private Toggle Load_toggle;


    // Use this for initialization
    void Start () {

        Atlier_toggle_obj = this.transform.Find("Viewport/Content_Main/Atlier_SelectToggle").gameObject;
        Shop_toggle_obj = this.transform.Find("Viewport/Content_Main/Shop_SelectToggle").gameObject;
        GirlEat_toggle_obj = this.transform.Find("Viewport/Content_Main/GirlEat_SelectToggle").gameObject;
        Travel_toggle_obj = this.transform.Find("Viewport/Content_Main/Travel_SelectToggle").gameObject;
        QuestBox_toggle_obj = this.transform.Find("Viewport/Content_Main/QuestBox_SelectToggle").gameObject;
        //Save_toggle_obj = this.transform.Find("Viewport/Content_Main/Save_SelectToggle").gameObject;
        //Load_toggle_obj = this.transform.Find("Viewport/Content_Main/Load_SelectToggle").gameObject;

        Atlier_toggle = Atlier_toggle_obj.GetComponent<Toggle>();
        Shop_toggle = Shop_toggle_obj.GetComponent<Toggle>();
        GirlEat_toggle = GirlEat_toggle_obj.GetComponent<Toggle>();
        Travel_toggle = Travel_toggle_obj.GetComponent<Toggle>();
        QuestBox_toggle = QuestBox_toggle_obj.GetComponent<Toggle>();
        //Save_toggle = Save_toggle_obj.GetComponent<Toggle>();
        //Load_toggle = Load_toggle_obj.GetComponent<Toggle>();

    }
	
	// Update is called once per frame
	void Update () {

		if( GameMgr.scenario_flag >= 110 && GameMgr.scenario_flag < 120) //1話の最初の調合シーンのときは、女の子と会うコマンドはOFF
        {
            GirlEat_toggle_obj.SetActive(false);
        }
	}

    public void OnAtlier_toggle()
    {
        if ( Atlier_toggle.isOn == true )
        {
            //SceneManager.LoadScene("Compound");
            FadeManager.Instance.LoadScene("Compound", 0.3f);
        }
    }

    public void OnShop_toggle()
    {
        if (Shop_toggle.isOn == true)
        {
            //SceneManager.LoadScene("Shop");
            FadeManager.Instance.LoadScene("Shop", 0.3f);
        }
    }

    public void OnGirlEat_toggle()
    {
        if (GirlEat_toggle.isOn == true)
        {
            //SceneManager.LoadScene("GirlEat");
            FadeManager.Instance.LoadScene("GirlEat", 0.3f);
        }
    }

    public void OnTravel_toggle()
    {
        if (Travel_toggle.isOn == true)
        {
            //SceneManager.LoadScene("Travel");
            FadeManager.Instance.LoadScene("Travel", 0.3f);
        }
    }

    public void OnQuestBox_toggle()
    {
        if (QuestBox_toggle.isOn == true)
        {
            //SceneManager.LoadScene("Travel");
            FadeManager.Instance.LoadScene("QuestBox", 0.3f);
        }
    }

    public void OnSave_toggle()
    {
        if (Save_toggle.isOn == true)
        {

        }
    }

    public void OnLoad_toggle()
    {
        if (Load_toggle.isOn == true)
        {

        }
    }
}
