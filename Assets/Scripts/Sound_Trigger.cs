using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//クリック・ポインタが入ってきたときに音を鳴らすスクリプト。
//コンポーネントをそのオブジェクトにつけるだけで、音が鳴る。

public class Sound_Trigger : MonoBehaviour {

    private SoundController sc;

	// Use this for initialization
	void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        gameObject.AddComponent<EventTrigger>(); //gameObjectは、自分自身のこと。thisと同義。


        Set_Check_SE();
    }

    void Set_Check_SE()
    {
        switch (transform.name)
        {
            case "Yes": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(0);
                break;

            case "Yes_tansaku": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(30);
                break;

            case "Yes_Clear": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(28);
                break;

            case "No": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(18);
                break;

            case "No_tansaku": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(0);
                break;

            case "up": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(23);
                break;

            case "down": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(23);
                break;

            case "ExtremeButton": //Get_Materialを押したときのSE

                //Debug.Log("this.transform.gameObject.GetComponent<Button>().IsInteractable(): " + this.transform.gameObject.GetComponent<Button>().IsInteractable());

                if (this.transform.gameObject.GetComponent<Button>().IsInteractable() == false)
                {

                    Debug.Log("このボタンは、今触れない状態");
                }
                else
                {
                    //ポインタが入ったときに鳴る音
                    SE_point_enter(2);


                    //クリックしたときに鳴る音。「GetMaterial」スクリプトで鳴らすようにした。
                    SE_point_click(0);
                }
                break;



            case "CompoundResultButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(0);
                break;

            case "ShopOn_Toggle_Buy":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(23);
                break;

            case "ShopOn_Toggle_Quest":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(23);
                break;

            case "ShopOn_Toggle_Talk":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(23);
                break;

            case "Button_modoru":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(23);
                break;

            default: //特に指定がない場合

                //Debug.Log("リストボタンを押した");


                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(0); //23
                break;

        }
    }

    // Update is called once per frame
    void Update() {

        
	}

    void SE_point_enter( int index )
    {
        
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerEnter; //Eventのタイプ。

        entry.callback.AddListener((eventDate) => {
            //Debug.Log("Bang");

            //オブジェクトの状態をチェック
            //ボタンがついているオブジェクトの場合
            if (this.transform.gameObject.GetComponent<Button>() != null)
            {
                if (this.transform.gameObject.GetComponent<Button>().IsInteractable() == false)
                {

                    //Debug.Log("このボタンは、今触れない状態");
                }
                else
                {
                    sc.PlaySe(index);
                }

            }
            else if (this.transform.gameObject.GetComponent<Toggle>() != null)
            {
                //Debug.Log("これはトグル");

                if (this.transform.gameObject.GetComponent<Toggle>().IsInteractable() == false)
                {

                    //Debug.Log("このボタンは、今触れない状態");
                }
                else
                {
                    sc.PlaySe(index);
                }

            }
            else
            {
                sc.PlaySe(index);
            }
            
        }); //ここのDebug.Logのメソッドを、音を鳴らすメソッドに割り当てれば、音がなるはず

        trigger.triggers.Add(entry);
    }

    void SE_point_click( int index2 )
    {

        EventTrigger trigger2 = gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();

        entry2.eventID = EventTriggerType.PointerClick; //Eventのタイプ。

        entry2.callback.AddListener((eventDate) => {
            //Debug.Log("Bang");

            sc.PlaySe(index2);

            /*//オブジェクトの状態をチェック
            //ボタンがついているオブジェクトの場合
            if (this.transform.gameObject.GetComponent<Button>() != null)
            {
                if (this.transform.gameObject.GetComponent<Button>().IsInteractable() == false)
                {

                    //Debug.Log("このボタンは、今触れない状態");
                }
                else
                {
                    sc.PlaySe(index2);
                }

            }
            else if (this.transform.gameObject.GetComponent<Toggle>() != null)
            {
                //Debug.Log("これはトグル");

                if (this.transform.gameObject.GetComponent<Toggle>().IsInteractable() == false)
                {

                    //Debug.Log("このボタンは、今触れない状態");
                }
                else
                {
                    sc.PlaySe(index2);
                }

            }
            else
            {
                sc.PlaySe(index2);
            }*/

        }); //ここのDebug.Logのメソッドを、音を鳴らすメソッドに割り当てれば、音がなるはず

        trigger2.triggers.Add(entry2);
    }

}
