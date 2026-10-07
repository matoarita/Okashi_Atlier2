using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBuf_IconPanel : MonoBehaviour
{
    private GameObject magicbbuficon_Prefab;
    private GameObject contentMBuf;
    public List<GameObject> magicbbuficon_list = new List<GameObject>();

    private int magicbuf_settingnum = 10;
    private Sprite[] magicbuf_icon_img;

    private int i, sta_id;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤー状態バフアイコン
        magicbuf_icon_img = new Sprite[magicbuf_settingnum];
        magicbuf_icon_img[0] = Resources.Load<Sprite>("Sprites/Items/" + "magic_statusdata4");
        magicbuf_icon_img[1] = Resources.Load<Sprite>("Sprites/Items/" + "magic_statusdata2");
        magicbuf_icon_img[2] = Resources.Load<Sprite>("Sprites/Items/" + "magic_statusdata3");
        contentMBuf = this.transform.Find("Scroll View/Viewport/Content").gameObject;
        magicbbuficon_Prefab = (GameObject)Resources.Load("Prefabs/MagicBuf_Icon");
        foreach (Transform child in contentMBuf.transform)
        {
            Destroy(child.gameObject);
        }
        magicbbuficon_list.Clear();
        for (i = 0; i < magicbuf_settingnum; i++) //頭から順番に、magicskillSelectToggle内の順番で各状態を設定　今のとこ３つ　0=Epiclesis, 1=Latria, 2=Three_Stars
        {
            magicbbuficon_list.Add(Instantiate(magicbbuficon_Prefab, contentMBuf.transform));
            magicbbuficon_list[i].transform.Find("Icon").GetComponent<Image>().sprite = magicbuf_icon_img[i];
            magicbbuficon_list[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤー状態バフのアイコン描画
        for (sta_id = 0; sta_id < magicbbuficon_list.Count; sta_id++) //頭から順番に、magicskillSelectToggle内の順番で各状態を設定　今のとこ３つ　0=Epiclesis, 1=Latria, 2=Three_Stars
        {
            if (PlayerStatus.player_girl_status[sta_id] > 0) //状態かかってるやつは、魔法LVが入っており0より上
            {
                magicbbuficon_list[sta_id].transform.Find("TimeSlider").GetComponent<Slider>().value = PlayerStatus.player_girl_status_timecounter[sta_id];
                magicbbuficon_list[sta_id].SetActive(true);
            }
            else
            {
                magicbbuficon_list[sta_id].SetActive(false);
            }
        }
    }
}
