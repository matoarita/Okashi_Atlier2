using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsaSelectPanel : MonoBehaviour
{
    private GameObject canvas;

    private GameObject catGetStartPanel_obj;
    private CatGetStartPanel catGetStartPanel;
    private CatDataBase catDataBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Init()
    {
        //ねこデータベースの取得
        catDataBase = CatDataBase.Instance.GetComponent<CatDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        catGetStartPanel_obj = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/CatGetStartPanel").gameObject;
        catGetStartPanel = catGetStartPanel_obj.GetComponent<CatGetStartPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEsaSelectButton()
    {
        Init();

        switch (this.transform.parent.gameObject.name)
        {
            case "EsaSelectPanel_1":

                catGetStartPanel.OnEsaSelectButton(1);
                break;

            case "EsaSelectPanel_2":

                catGetStartPanel.OnEsaSelectButton(2);
                break;

            case "EsaSelectPanel_3":

                catGetStartPanel.OnEsaSelectButton(3);
                break;

            case "EsaSelectPanel_4":

                catGetStartPanel.OnEsaSelectButton(4);
                break;

            case "EsaSelectPanel_cancel":

                catGetStartPanel.OnEsaSelectButton(9);
                break;
        }
    }
}
