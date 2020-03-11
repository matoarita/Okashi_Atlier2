using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HukidashiAction : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private Girl1_status girl1_status;

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;

    private GameObject hukidasi;
    private GameObject hukidasi_sp;
    private GameObject hukidasi1, hukidasi2;
    private GameObject hukidasi_text;

    private int hukidasi_action_status;

    // Use this for initialization
    void Start () {

        canvas = GameObject.FindWithTag("Canvas");

        //テキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //吹き出しオブジェクトの取得
        hukidasi = this.gameObject.transform.Find("Image").gameObject;
        hukidasi1 = this.gameObject.transform.Find("Image (1)").gameObject;
        hukidasi2 = this.gameObject.transform.Find("Image (2)").gameObject;
        hukidasi_text = this.gameObject.transform.Find("hukidashi_Text").gameObject;

        //スペシャル用の吹きだしオブジェクト
        hukidasi_sp = this.gameObject.transform.Find("Image_special").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
        if(text_area.GetComponent<TextController>().textend_flag == true)
        {
            switch(hukidasi_action_status)
            {
                case 0:

                    hukidasi.GetComponent<Image>().raycastTarget = true;
                    hukidasi_text.SetActive(true);
                    hukidasi.SetActive(true);
                    hukidasi1.SetActive(true);
                    hukidasi2.SetActive(true);
                    break;

                case 1:

                    hukidasi_sp.GetComponent<Image>().raycastTarget = true;
                    hukidasi_text.SetActive(true);
                    hukidasi_sp.SetActive(true);
                    hukidasi1.SetActive(true);
                    hukidasi2.SetActive(true);
                    break;
            }
            

            //カメラ元に戻す
            trans = 0; //transが1を超えたときに、ズームするように設定されている。

            //intパラメーターの値を設定する.
            maincam_animator.SetInteger("trans", trans);

            text_area.GetComponent<TextController>().textend_flag = false;
        }
	}

    public void NormalHint()
    {

        hukidasi.GetComponent<Image>().raycastTarget = false;
        hukidasi_text.SetActive(false);
        hukidasi.SetActive(false);
        hukidasi1.SetActive(false);
        hukidasi2.SetActive(false);

        hukidasi_action_status = 0;

        //カメラ寄る。
        trans = 1; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);

        //Debug.Log("ヒントを表示する");        
        text_area.GetComponent<TextController>().SetText("ヒントが表示されるよ～！");
        text_area.GetComponent<TextController>().hint_on = true;
        //_text.text = "ヒントが表示されるよ～！";
    }

    public void SpecialHint()
    {
        hukidasi_sp.GetComponent<Image>().raycastTarget = false;
        hukidasi_text.SetActive(false);
        hukidasi_sp.SetActive(false);
        hukidasi1.SetActive(false);
        hukidasi2.SetActive(false);

        hukidasi_action_status = 1;

        //カメラ寄る。
        trans = 1; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);

        switch ( girl1_status.OkashiQuest_ID)
        {
            case 12: //クエスト　＜自由＞お兄ちゃんのオリジナルクッキーが食べたい

                text_area.GetComponent<TextController>().SetText("お兄ちゃんの作ったクッキーが食べたいなあ。" + "\n" + "見た目がかわいいのがいいな～！");
                text_area.GetComponent<TextController>().hint_on = true;

                break;

            default:
                break;
        }
    }

    public void ScaleUP()
    {

    }
}
