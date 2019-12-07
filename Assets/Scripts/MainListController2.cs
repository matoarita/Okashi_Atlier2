using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainListController2 : MonoBehaviour
{

    private GameObject npc1_toggle_obj;
    private GameObject npc2_toggle_obj;
    private GameObject npc3_toggle_obj;
    private GameObject hiroba1_toggle_obj;


    private Toggle npc1_toggle;
    private Toggle npc2_toggle;
    private Toggle npc3_toggle;
    private Toggle hiroba1_toggle;



    // Use this for initialization
    void Start()
    {

        npc1_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC1_SelectToggle").gameObject;
        npc2_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC2_SelectToggle").gameObject;
        npc3_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC3_SelectToggle").gameObject;
        hiroba1_toggle_obj = this.transform.Find("Viewport/Content_Main/Hiroba1_SelectToggle").gameObject;


        npc1_toggle = npc1_toggle_obj.GetComponent<Toggle>();
        npc2_toggle = npc2_toggle_obj.GetComponent<Toggle>();
        npc3_toggle = npc3_toggle_obj.GetComponent<Toggle>();
        hiroba1_toggle = hiroba1_toggle_obj.GetComponent<Toggle>();

    }

    // Update is called once per frame
    void Update()
    {

        if (GameMgr.scenario_flag >= 110 && GameMgr.scenario_flag < 150) //1話の最初の調合シーンのときは、女の子と会うコマンドはOFF
        {
            
        }
    }

    public void OnNPC1_toggle()
    {
        if (npc1_toggle.isOn == true)
        {
            //SceneManager.LoadScene("Compound");
            //FadeManager.Instance.LoadScene("Compound", 0.3f);
        }
    }

    public void OnNPC2_toggle()
    {
        if (npc2_toggle.isOn == true)
        {
            //SceneManager.LoadScene("Shop");
            //FadeManager.Instance.LoadScene("Shop", 0.3f);
        }
    }

    public void OnNPC3_toggle()
    {
        if (npc3_toggle.isOn == true)
        {
            //SceneManager.LoadScene("GirlEat");
            //FadeManager.Instance.LoadScene("GirlEat", 0.3f);
        }
    }

    public void OnHiroba1_Button()
    {
        if (hiroba1_toggle.isOn == true)
        {
            FadeManager.Instance.LoadScene("Hiroba", 0.3f);
        }
    }
}
