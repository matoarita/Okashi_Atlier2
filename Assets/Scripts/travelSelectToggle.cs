using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class travelSelectToggle : MonoBehaviour {

    Toggle m_Toggle;
    public Text m_Text; //デバッグ用。未使用。

    private GameObject travellistController_obj;
    private TravelListController travellistController;

    private WorldDataBase worlddatabase;

    private GameObject text_area; //
    private Text _text; //

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private int i, count;
    private int travel_no;

    // Use this for initialization
    void Start () {

        //世界・街情報の取得
        worlddatabase = WorldDataBase.Instance.GetComponent<WorldDataBase>();

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

        travellistController_obj = GameObject.FindWithTag("TravelList_ScrollView");
        travellistController = travellistController_obj.GetComponent<TravelListController>();

        yes = travellistController_obj.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = travellistController_obj.transform.Find("No").gameObject;
        yes_selectitem_kettei = yes.GetComponent<SelectItem_kettei>();

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();
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
            count = 0;

            while (count < travellistController._travellistitem.Count)
            {
                if (travellistController._travellistitem[count].GetComponent<Toggle>().isOn == true) break;
                ++count;
            }

            travellistController.kettei_travel = count; //リスト中の選択された番号を格納。
            travel_no = count;

            _text.text = worlddatabase.travel_name[travel_no] + "へ行きますか？" + "\n" + "滞在費 1日: " + worlddatabase.travel_cost[travel_no];

            yes.SetActive(true);
            no.SetActive(true);

            StartCoroutine("travelselect_kakunin");
        }
    }

    IEnumerator travelselect_kakunin()
    {
        for (i = 0; i < travellistController._travellistitem.Count; i++)
        {
            travellistController._travellistitem[i].GetComponent<Toggle>().interactable = false;
        }

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {
            case true: //その場所へ移動し、採集

                
               switch (travel_no)
               {
                    case 0: //近くの森

                        SceneManager.LoadScene("Travel_Map01");
                        break;

                    case 1: //エメラルドの森

                        SceneManager.LoadScene("Travel_Map01");
                        break;

                    case 2: //サファイアの丘

                        SceneManager.LoadScene("Travel_Map01");
                        break;

                    default:
                       break;
               }
                
               break;

            case false: //キャンセル

                _text.text = "";

                for (i = 0; i < travellistController._travellistitem.Count; i++)
                {
                    travellistController._travellistitem[i].GetComponent<Toggle>().interactable = true;
                    travellistController._travellistitem[i].GetComponent<Toggle>().isOn = false;
                }

                yes.SetActive(false);
                no.SetActive(false);

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                break;
        }
    }
}
