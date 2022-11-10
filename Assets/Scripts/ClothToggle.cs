using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothToggle : MonoBehaviour {

    private GameObject canvas;
    private GameObject status_panel;

    private SoundController sc;

    Toggle m_Toggle;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //ステータスパネルの取得
        status_panel = canvas.transform.Find("StatusPanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();

        //Initialise the Text to say the first state of the Toggle デバッグ用テキスト
        //m_Text = m_Toggle.GetComponentInChildren<Text>();
        //m_Text.text = "First Value : " + m_Toggle.isOn;

        //Add listener for when the state of the Toggle changes, to take action アドリスナー　トグルの値が変化したときに、｛｝内のメソッドを呼び出す
        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        //m_Text.text = "New Value : " + m_Toggle.isOn;
        if (m_Toggle.isOn == true)
        {
            //sc.PlaySe(127);
            sc.PlaySe(128); //128 11
        }
        else
        {
            //sc.PlaySe(18);
        }
    }

    public void OnCostumeToggle()
    {
        status_panel.GetComponent<StatusPanel>().OnCostumeChange();
    }
}
