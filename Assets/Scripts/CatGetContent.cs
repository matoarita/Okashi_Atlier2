using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGetContent : MonoBehaviour
{
    public int toggle_listid;
    public string toggle_catid;

    private GameObject canvas;

    private GameObject catGetStartPanel_obj;
    private CatGetStartPanel catGetStartPanel;

    private CatDataBase catDataBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInit()
    {
        //ねこデータベースの取得
        catDataBase = CatDataBase.Instance.GetComponent<CatDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        catGetStartPanel_obj = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/CatGetStartPanel").gameObject;
        catGetStartPanel = catGetStartPanel_obj.GetComponent<CatGetStartPanel>();
    }

    public void OnCatGetMatButton()
    {
        SetInit();

        GameMgr.Select_cat_num = toggle_listid;
        GameMgr.Select_cat_nameHyouji = catDataBase.catdata_list[toggle_listid].catnameHyouji;
        GameMgr.Select_cat_lv = catDataBase.catdata_list[toggle_listid].catLv;

        //採取ボタン　採取してないなら、マップ画面ひらく　採取中なら、採取をやめる？と聞く。
        if (catDataBase.catdata_list[toggle_listid].catStatus == 0)
        {

            catGetStartPanel.OnCatGet_MapSelect();
        }
        else
        {
            //採取中　やめるかどうかを聞くボタンに。
            catGetStartPanel.OnCatGet_PlayCancel();
        }
    }

    public void OnFireButton()
    {
        SetInit();

        GameMgr.Select_cat_num = toggle_listid;
        GameMgr.Select_cat_nameHyouji = catDataBase.catdata_list[toggle_listid].catnameHyouji;

        //解雇ボタン　解雇するかどうかをきく
        catGetStartPanel.OnCatFirePanel();
    }

    public void OnEsaButton()
    {
        SetInit();

        GameMgr.Select_cat_num = toggle_listid;
        GameMgr.Select_cat_nameHyouji = catDataBase.catdata_list[toggle_listid].catnameHyouji;

        //エサボタン　エサ代を決める　安いとあんまり働かない　高いと通常よりスピード早くなる
        catGetStartPanel.OnCatEsaPanel();
    }

    public void OnCatIconButton()
    {
        SetInit();

        GameMgr.Select_cat_num = toggle_listid;
        GameMgr.Select_cat_nameHyouji = catDataBase.catdata_list[toggle_listid].catnameHyouji;

        //ねこアイコンおす　ステータス表示するか、名前変更できるように。
        catGetStartPanel.OnNameChangePanel();
    }

    public void OnZairyoKakuninButton()
    {
        SetInit();

        GameMgr.Select_cat_num = toggle_listid;
        GameMgr.Select_cat_nameHyouji = catDataBase.catdata_list[toggle_listid].catnameHyouji;

        catGetStartPanel.OnZairyoCheckPanel();
    }
}
