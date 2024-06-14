using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HirobaMove_SelectToggle : MonoBehaviour {

    private GameObject hiroba_Main;
    private Hiroba1_Main_Controller hiroba_mainController;

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnHirobaToggle()
    {
        hiroba_Main = GameObject.FindWithTag("Hiroba_Main");
        hiroba_mainController = hiroba_Main.GetComponent<Hiroba1_Main_Controller>();

        switch (this.gameObject.name)
        {
            case "NPC1_SelectToggle":

                //this.gameObject.GetComponent<Toggle>().SetIsOnWithoutCallback(true);
                hiroba_mainController.OnNPC1_toggle();
                break;

            case "NPC2_SelectToggle":

                //this.gameObject.GetComponent<Toggle>().SetIsOnWithoutCallback(true);
                hiroba_mainController.OnNPC2_toggle();
                break;

            case "NPC3_SelectToggle":

                //this.gameObject.GetComponent<Toggle>().SetIsOnWithoutCallback(true);
                hiroba_mainController.OnNPC3_toggle();
                break;

            case "NPC4_SelectToggle":

                //this.gameObject.GetComponent<Toggle>().SetIsOnWithoutCallback(true);
                hiroba_mainController.OnNPC4_toggle();
                break;

            case "NPC5_SelectToggle":

                //this.gameObject.GetComponent<Toggle>().SetIsOnWithoutCallback(true);
                hiroba_mainController.OnNPC5_toggle();
                break;

            case "NPC6_SelectToggle":

                //this.gameObject.GetComponent<Toggle>().SetIsOnWithoutCallback(true);
                hiroba_mainController.OnNPC6_toggle();
                break;

            case "NPC7_SelectToggle":

                //this.gameObject.GetComponent<Toggle>().SetIsOnWithoutCallback(true);
                hiroba_mainController.OnNPC7_toggle();
                break;

            case "NPC8_SelectToggle":

                //this.gameObject.GetComponent<Toggle>().SetIsOnWithoutCallback(true);
                hiroba_mainController.OnNPC8_toggle();
                break;
        }
    }

}
