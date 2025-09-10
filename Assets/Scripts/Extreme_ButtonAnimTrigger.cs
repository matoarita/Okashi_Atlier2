using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extreme_ButtonAnimTrigger : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterAnimTrigger()
    {
        //Debug.Log("Enter エクストリームパネル");
        this.GetComponent<ButtonAnimTrigger>().OnImageEnterAnim();
    }
}
