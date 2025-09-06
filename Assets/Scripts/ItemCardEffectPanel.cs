using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCardEffectPanel : MonoBehaviour
{
    //こっちは、エフェクト表示用

    private int i, j, count;
    private ParticleSystemRenderer m_ParticleSystem;

    private string pt_root;
    private string img_root;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MagicEffect_Hyouji(string[] _magicslot, int _mstatus) //_mstatusは、アクセスする元のオブジェクト場所　パネルからかカードからか
    {
        foreach(Transform child in this.transform)
        {
            child.gameObject.SetActive(false);
        }

        for (i = 0; i < _magicslot.Length; i++)
        {
            //Debug.Log("_magicslot " + i + ": " + _magicslot[i]);
            if (_magicslot[i] == GameMgr.System_MagicSlotName01) //FireFlowerの場合　花火が周りにとびちるエフェクト
            {
                DrawParticle("effect01_Fire", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName02) //Butterflyの場合、光のちょうちょがとぶ
            {
                DrawParticle("effect02_Butterfly", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName03) //Bubbleは泡がでる
            {
                DrawParticle("effect03_Bubble", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName04) //Starは星くずがキラキラする
            {
                DrawParticle("effect04_Star", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName05) //WindArc　風の円弧が周りにとびちる
            {
                DrawParticle("effect05_Arc", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName06) //WindArc　風の円弧が周りにとびちる
            {
                DrawParticle("effect06_Mnemonic", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName07) //WindArc　風の円弧が周りにとびちる
            {
                DrawParticle("effect07_Glitter", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName08) //WindArc　風の円弧が周りにとびちる
            {
                DrawParticle("effect08_Sakura", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName09) //WindArc　風の円弧が周りにとびちる
            {
                DrawParticle("effect09_Flower", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName10) //WindArc　風の円弧が周りにとびちる
            {
                DrawParticle("effect10_Heart", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName11) //WindArc　風の円弧が周りにとびちる
            {
                DrawParticle("effect11_RainbowRain", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName12) //WindArc　風の円弧が周りにとびちる
            {
                DrawParticle("effect12_NightBarron", _mstatus);
            }
            if (_magicslot[i] == GameMgr.System_MagicSlotName13) //WindArc　風の円弧が周りにとびちる
            {
                DrawParticle("effect13_CrescentMoon", _mstatus);
            }
        }
    }

    void DrawParticle(string _pname, int _status)
    {
        this.transform.Find(_pname).gameObject.SetActive(true);
        //m_ParticleSystem = this.transform.Find(_pname).GetChild(0).GetComponent<ParticleSystemRenderer>();

        pt_root = _pname + "/pt_root";
        if (_status == 1) //お菓子パネルで表示する場合　描画順変える
        {
            count = 0;
            foreach (Transform child in this.transform.Find(pt_root).transform)
            {
                m_ParticleSystem = child.GetComponent<ParticleSystemRenderer>();
                m_ParticleSystem.sortingOrder = 500 + count;
                count++;
            }
            
        }
        else if (_status == 2) //お菓子あげるときに表示する場合　描画順変える
        {
            count = 0;
            foreach (Transform child in this.transform.Find(pt_root).transform)
            {
                m_ParticleSystem = child.GetComponent<ParticleSystemRenderer>();
                m_ParticleSystem.sortingOrder = 660 + count;
                count++;
            }
            
        }
        else //デフォ　元のオーダー使う　5010とかになってる
        {
            count = 0;
            foreach (Transform child in this.transform.Find(pt_root).transform)
            {
                m_ParticleSystem = child.GetComponent<ParticleSystemRenderer>();
                m_ParticleSystem.sortingOrder = 5010 + count;
                count++;
            }
            
        }

        //画像データある場合、画像も描画順を変える
        img_root = _pname + "/Img_root";
        if (_status == 1) //お菓子パネルで表示する場合　描画順変える
        {
            count = 0;
            foreach (Transform child in this.transform.Find(img_root).transform)
            {
                child.GetComponent<Canvas>().sortingOrder = 510 + count;
                count++;
            }
            
        }
        else if (_status == 2) //お菓子あげるときに表示する場合　描画順変える
        {
            count = 0;
            foreach (Transform child in this.transform.Find(img_root).transform)
            {
                child.GetComponent<Canvas>().sortingOrder = 670 + count;
                count++;
            }
            
        }
        else //デフォ　元のオーダー使う　5010とかになってる
        {
            count = 0;
            foreach (Transform child in this.transform.Find(img_root).transform)
            {
                child.GetComponent<Canvas>().sortingOrder = 5020 + count;
                count++;
            }
            
        }
    }
}
