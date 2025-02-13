using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCardEffectPanel : MonoBehaviour
{
    //こっちは、エフェクト表示用

    private int i, j;
    private ParticleSystemRenderer m_ParticleSystem;

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
        }
    }

    void DrawParticle(string _pname, int _status)
    {
        this.transform.Find(_pname).gameObject.SetActive(true);
        m_ParticleSystem = this.transform.Find(_pname).GetChild(0).GetComponent<ParticleSystemRenderer>();

        if (_status == 1) //お菓子パネルで表示する場合　描画順変える
        {           
            m_ParticleSystem.sortingOrder = 500;
        }
        else if (_status == 2) //お菓子あげるときに表示する場合　描画順変える
        {
            m_ParticleSystem.sortingOrder = 660;
        }
        else //デフォ　元のオーダー使う　5010とかになってる
        {
            m_ParticleSystem.sortingOrder = 5010;
        }
    }
}
