using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        
        switch (transform.name)
        {
            case "No": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(0);
                break;

            case "GetMaterial_Toggle": //Get_Materialを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。「GetMaterial」スクリプトで鳴らすようにした。
                //SE_point_click(9);
                break;

            default: //特に指定がない場合

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(0);
                break;
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SE_point_enter( int index )
    {
        
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerEnter; //Eventのタイプ。

        entry.callback.AddListener((eventDate) => {
            //Debug.Log("Bang");
            sc.PlaySe(index);
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
        }); //ここのDebug.Logのメソッドを、音を鳴らすメソッドに割り当てれば、音がなるはず

        trigger2.triggers.Add(entry2);
    }
}
