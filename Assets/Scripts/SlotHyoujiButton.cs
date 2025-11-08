using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotHyoujiButton : MonoBehaviour
{
    private int card_sortorder;


    // Start is called before the first frame update
    void Start()
    {
        Koushin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //SetImageÇ©ÇÁì«Ç›èoÇµ
    public void Koushin()
    {
        card_sortorder = this.transform.parent.parent.parent.GetComponent<Canvas>().sortingOrder;

        this.GetComponent<Canvas>().sortingOrder = card_sortorder + 100;
    }
}

