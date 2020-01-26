using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PRenkinLevel_panel : MonoBehaviour
{

    private GameObject _prenkin_lv_obj;

    private Text _prenkin_lv_text;

    // Use this for initialization
    void Start()
    {
        _prenkin_lv_obj = this.transform.Find("PRenkin_param").gameObject;
        _prenkin_lv_text = _prenkin_lv_obj.GetComponent<Text>();

        _prenkin_lv_text.text = PlayerStatus.player_renkin_lv.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _prenkin_lv_text.text = PlayerStatus.player_renkin_lv.ToString();
    }

}
