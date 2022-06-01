//Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
//Set your own Text in the Inspector window

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

//***  アイテムの調合処理、プレイヤーのアイテム所持リストの処理はここでやっています。
//***  プレファブにとりつけているスクリプト、なので、privateの値は、インスタンスごとに変わってくるため、バグに注意。

public class ContestClearList : MonoBehaviour
{
    Toggle m_Toggle;
    public Text m_Text; //デバッグ用。未使用。

    private GameObject canvas;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject black_panel;
    private GameObject contest_effect_panel;
    private GameObject cance_panel;

    public int toggleitem_ID; //リストの要素自体に、アイテムIDを保持する。
    

    void Start()
    {

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

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");       
      
        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        black_panel = canvas.transform.Find("BlackPanel").gameObject;
        cance_panel = canvas.transform.Find("ContestClear_cardcancel").gameObject;
        contest_effect_panel = canvas.transform.Find("ContestClearEffectPanel").gameObject;
    }


    void Update()
    {

    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        //m_Text.text = "New Value : " + m_Toggle.isOn;
        if (m_Toggle.isOn == true)
        {
            ContestList_Open();
        }
    }
    
    //アイテム画面を開いた時の処理。アイテムを選択すると、カードを表示する。
    void ContestList_Open()
    {
        black_panel.SetActive(true);
        cance_panel.SetActive(true);
        contest_effect_panel.SetActive(true);
        card_view.ContestClearOkashi(toggleitem_ID);
    }
    
}
