using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//クリック・ポインタが入ってきたときに音を鳴らすスクリプト。
//コンポーネントをそのオブジェクトにつけるだけで、音が鳴る。

public class Sound_Trigger : MonoBehaviour {

    private SoundController sc;

    public bool se_sound_ON;

	// Use this for initialization
	void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        gameObject.AddComponent<EventTrigger>(); //gameObjectは、自分自身のこと。thisと同義。

        se_sound_ON = true;
        Set_Check_SE();
    }

    void Set_Check_SE()
    {
        switch (transform.name)
        {
            case "Yes": //yesを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(46);
                break;

            case "Yes_Clear": //ステージクリアを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(28);
                
                break;

            case "Yes_okashiSet": //

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                //SE_point_click(46);
                break;

            case "No_okashiSet": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                //SE_point_click(18);
                break;

            case "NouhinButton": //納品決定を押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(46);
                break;

            case "CompoundStartButton": //yesを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(46);
                break;


            case "No": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(18);
                break;

            case "SlotHyoujiButton": //スロット表示切替を押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(81);
                break;


            case "NouhinCancelButton": //納品決定を押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(18);
                break;

            case "up": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(23);
                break;

            case "up_big": //noを押したときのSE

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(23);
                break;

            case "up_small": //noを押したときのSE

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

            case "recipiMemoSelectToggle_content(Clone)": //メモ開くときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(34);
                break;

            case "HintTasteButton": //メモ開くときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(34);
                break;

            case "HintTaste_Toggle": //メモ開くときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(34);
                break;

            case "Recipi_Toggle": //レシピ開くときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(34);
                break;

            case "CGgelleryButton": //メモ開くときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(34);
                break;

            case "SpecialTitleButton": //メモ開くときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(34);
                break;

            case "ContestClearButton": //メモ開くときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(34);
                break;

            case "RecipiBookButton": //メモ開くときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(34);
                break;

            case "itemSelectToggle(Clone)": //アイテム欄で、アイテム選択するときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(2);
                break;

            case "recipiitemSelectToggle(Clone)": //レシピリストで、アイテム選択するときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(2);
                break;

            case "shopitemSelectToggle(Clone)": //レシピリストで、アイテム選択するときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(46);
                break;

            case "shopQuestSelectToggle(Clone)": //レシピリストで、アイテム選択するときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(46);
                break;

            case "MatPlace_toggle1(Clone)": //レシピリストで、アイテム選択するときの音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(2);
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

            case "ClothToggle":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                //SE_point_click(128);
                
                break;

            case "Button_modoru":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(23);
                break;

            case "Yes_tansaku":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(30);
                break;

            case "No_tansaku":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(30);
                break;

            case "Next_tansaku":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(30);
                break;

            case "KaigyoButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(30);
                break;

            case "Open_treasure":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(30);
                break;

            case "Emo_Hukidashi_Anim(Clone)":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                //SE_point_click(23);
                break;

            case "hukidashi_Image":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                //SE_point_click(23);
                break;

            case "hukidashi_Image_special":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                //SE_point_click(23);
                break;


            case "EDTitleBackButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(28);
                break;

            case "GameStartButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(28);
                break;

            case "GameStartButton_2":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(28);
                break;

            case "GameLoadButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(28);
                break;

            case "OriginalButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(46);
                break;

            case "RecipiButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(46);
                break;

            case "ExButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(46);
                break;

            case "HikariMakeButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(46);
                break;

            case "SaveButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(0);
                break;

            case "LoadButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(0);
                break;

            case "GalleryButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(28);
                break;

            case "OptionButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(0);
                break;

            case "TitleButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                SE_point_click(0);
                break;

            case "AutoSaveToggle":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                //SE_point_click(0);
                break;

            case "CompoBGMToggle":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                //SE_point_click(0);
                break;

            case "SleepSkipToggle":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                //SE_point_click(0);
                break;

            case "PicnicSkipToggle":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                //SE_point_click(0);
                break;

            case "OutGirlSkipToggle":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);

                //クリックしたときに鳴る音。
                //SE_point_click(0);
                break;

            case "MainUIOpenButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(36);
                break;

            case "MainUICloseButton":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(36);
                break;

            case "CardDeco_Toggle":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                //SE_point_click(36);
                break;

            case "Yes_collect":

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                //SE_point_click(36);
                break;

            case "Speed1": //ゲーム中の時間速度変更のボタン音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(126);
                break;

            case "Speed2": //ゲーム中の時間速度変更のボタン音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(125);
                break;

            case "Speed3": //ゲーム中の時間速度変更のボタン音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(124);
                break;

            case "Speed4": //ゲーム中の時間速度変更のボタン音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(123);
                break;

            case "Speed5": //ゲーム中の時間速度変更のボタン音

                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音。
                SE_point_click(122);
                break;

            default: //特に指定がない場合

                //Debug.Log("リストボタンを押した");


                //ポインタが入ったときに鳴る音
                SE_point_enter(2);


                //クリックしたときに鳴る音
                SE_point_click(23); //0 or 23
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

            entry.callback.AddListener((eventDate) =>
            {
                //Debug.Log("Bang");

                if (se_sound_ON)
                {
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
                }

            }); //ここのDebug.Logのメソッドを、音を鳴らすメソッドに割り当てれば、音がなるはず

            trigger.triggers.Add(entry);
        
    }

    void SE_point_click( int index2 )
    {
        
            EventTrigger trigger2 = gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry2 = new EventTrigger.Entry();

            entry2.eventID = EventTriggerType.PointerClick; //Eventのタイプ。

            entry2.callback.AddListener((eventDate) =>
            {
                if (se_sound_ON)
                {
                    //Debug.Log("Bang");

                    sc.PlaySe(index2);
                }

            }); //ここのDebug.Logのメソッドを、音を鳴らすメソッドに割り当てれば、音がなるはず

            trigger2.triggers.Add(entry2);
        }
    
}
