using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmoideListPanel : MonoBehaviour
{
    private GameObject canvas;

    private GameObject status_panel;

    public int toggle_id;
    public int toggle_omoide_utageid;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOmoideUtage()
    {
        //ƒLƒƒƒ“ƒoƒX‚Ì“Ç‚İ‚İ
        canvas = GameObject.FindWithTag("Canvas");
        status_panel = canvas.transform.Find("StatusPanel").gameObject;

        status_panel.GetComponent<StatusPanel>().ReadCGGallery(toggle_omoide_utageid);

    }
}
