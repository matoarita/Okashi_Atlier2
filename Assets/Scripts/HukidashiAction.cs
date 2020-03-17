using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HukidashiAction : MonoBehaviour {

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

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
    private int i, _id;

    private string _hint;

    // Use this for initialization
    void Start () {

        canvas = GameObject.FindWithTag("Canvas");

        //テキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //女の子の好みのお菓子セットの取得
        girlLikeSet_database = GirlLikeSetDataBase.Instance.GetComponent<GirlLikeSetDataBase>();

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

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

            girl1_status.GirlEat_Judge_on = true;
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
        _hint = girlLikeCompo_database.girllike_compoRandomset[girl1_status.Set_compID].hint_text;
        text_area.GetComponent<TextController>().SetText(_hint);
        text_area.GetComponent<TextController>().hint_on = true;
        //_text.text = "ヒントが表示されるよ～！";

        girl1_status.GirlEat_Judge_on = false;
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

        //_compIDをもとに_IDを決定
        i = 0;
        while (i < girlLikeCompo_database.girllike_composet.Count)
        {
            if (girlLikeCompo_database.girllike_composet[i].set_ID == girl1_status.OkashiQuest_ID)
            {
                _id = i;
                break;
            }
            i++;
        }

        _hint = girlLikeCompo_database.girllike_composet[_id].hint_text;
        text_area.GetComponent<TextController>().SetText(_hint);
        text_area.GetComponent<TextController>().hint_on = true;

        girl1_status.GirlEat_Judge_on = false;
    }

    public void ScaleUP()
    {

    }
}
