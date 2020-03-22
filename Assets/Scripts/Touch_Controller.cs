using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_Controller : MonoBehaviour {

    private GameObject BG;
    private GameObject Character;

    private List<GameObject> touch_obj = new List<GameObject>();

    private int i;

	// Use this for initialization
	void Start () {

        BG = GameObject.FindWithTag("BG");
        Character = GameObject.FindWithTag("Character");

        touch_obj.Add(BG.transform.Find("TouchBell").gameObject);
        touch_obj.Add(BG.transform.Find("TouchFlower").gameObject);
        touch_obj.Add(BG.transform.Find("TouchWindow").gameObject);
        touch_obj.Add(BG.transform.Find("TouchWindow2").gameObject);
        touch_obj.Add(Character.transform.Find("TouchFace").gameObject);
        touch_obj.Add(Character.transform.Find("TouchHair").gameObject);
        touch_obj.Add(Character.transform.Find("TouchChest").gameObject);
        touch_obj.Add(Character.transform.Find("TouchRibbonR").gameObject);
        touch_obj.Add(Character.transform.Find("TouchRibbonL").gameObject);
        touch_obj.Add(Character.transform.Find("TouchLongHairR").gameObject);
        touch_obj.Add(Character.transform.Find("TouchLongHairL").gameObject);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //全てのオブジェクトのタッチをオフにする。
    public void Touch_OnAllOFF()
    {
        i= 0;
        while( i < touch_obj.Count)
        {
            touch_obj[i].SetActive(false);
            i++;
        }
    }

    //全てのオブジェクトのタッチをオフにする。
    public void Touch_OnAllON()
    {
        i = 0;
        while (i < touch_obj.Count)
        {
            touch_obj[i].SetActive(true);
            i++;
        }
    }
}
