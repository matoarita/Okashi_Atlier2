using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatAnimPanel : MonoBehaviour {

    private SoundController sc;
    private GameObject EatStartEffect;

    private ItemDataBase database;

    private GameObject effectPrefab;
    private GameObject effectPrefab_Init;
    private GameObject itemEffectPanel;

    private Sprite texture2d;
    private Image itemImage;

    private Image PlateImg;

    private Sprite _plate_sprite1;
    private Sprite _plate_sprite2;
    private Sprite _plate_sprite3;
    private Sprite _plate_sprite4;
    private Sprite _plate_sprite5;

    // Use this for initialization
    void Start () {

        InitSetting();
    }
	
    void InitSetting()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        itemImage = this.transform.Find("ItemImage").GetComponent<Image>();        
        
        PlateImg = this.transform.Find("Plate/plate_img1").GetComponent<Image>();

        EatStartEffect = GameObject.FindWithTag("EatAnim_Effect").transform.Find("Comp").gameObject;
        EatStartEffect.SetActive(false);

        _plate_sprite1 = Resources.Load<Sprite>("Sprites/Icon/PlateImg_01");
        _plate_sprite2 = Resources.Load<Sprite>("Sprites/Icon/PlateImg_02");
        _plate_sprite3 = Resources.Load<Sprite>("Sprites/Icon/PlateImg_03");

        switch (GameMgr.PlateSetNum)
        {
            case 0:

                PlateImg.sprite = _plate_sprite1;
                break;

            case 1:

                PlateImg.sprite = _plate_sprite2;
                break;

            case 2:

                PlateImg.sprite = _plate_sprite3;
                break;
        }

        //魔法のエフェクトパネル
        if (effectPrefab_Init == null)
        {
            effectPrefab = (GameObject)Resources.Load("Prefabs/ItemCardEffectPanel");
            effectPrefab_Init = Instantiate(effectPrefab, this.transform.Find("ItemImage").transform);
            effectPrefab_Init.name = "ItemCardEffectPanel";
            effectPrefab_Init.transform.localPosition = new Vector3(0, 0, 0);
        }
        itemEffectPanel = this.transform.Find("ItemImage/ItemCardEffectPanel").gameObject; //エフェクトパネル

        
    }

	// Update is called once per frame
	void Update () {
		
        if(GameMgr.EatAnim_End)
        {
            GameMgr.EatAnim_End = false;
            Effect_End();
            this.gameObject.SetActive(false);
        }
	}

    public void Effect_Start()
    {
        InitSetting();

        //食べ始めアニメエフェクト
        EatStartEffect.SetActive(true);       
    }

    public void ItemSetting(string[] _MS, string _itemname) //GirlEat_Judgeから読み出し
    {
        InitSetting();

        //アイテムデータ設定
        texture2d = database.items[database.SearchItemIDString(_itemname)].itemIcon_sprite;
        itemImage.sprite = texture2d;

        //アイテムのマジックスロットをみて、魔法エフェクトも表示
        itemEffectPanel.GetComponent<ItemCardEffectPanel>().MagicEffect_Hyouji(_MS, 2); //2番目の数字は、アクセスする場所を指定　2=お菓子をあげるとき
    }


    public void OnSound_Hit()
    {
        sc.PlaySe(71);
        sc.PlaySe(72);
    }

    public void Effect_End()
    {
        EatStartEffect.SetActive(false);
    }
}
