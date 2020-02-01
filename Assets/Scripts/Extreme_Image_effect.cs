using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extreme_Image_effect : MonoBehaviour {

    private Transform myTransform;
    private Vector3 scale;
    private Image image_effect;

    private float alfa;    //A値を操作するための変数
    private float red, green, blue;    //RGBを操作するための変数

    private float _deg;
    private float _scaledeg;

    // Use this for initialization
    void Start () {       

        myTransform = this.transform;
        scale = myTransform.localScale;

        image_effect = this.gameObject.GetComponent<Image>();

        red = this.gameObject.GetComponent<Image>().color.r;
        green = this.gameObject.GetComponent<Image>().color.g;
        blue = this.gameObject.GetComponent<Image>().color.b;
        alfa = this.gameObject.GetComponent<Image>().color.a;

        _deg = 0.003f;
        _scaledeg = 0.003f;
    }
	
	// Update is called once per frame
	void Update () {

        alfa -= _deg;
        image_effect.color = new Color(red, green, blue, alfa);
        scale.x += _scaledeg;
        scale.y += _scaledeg;
        myTransform.localScale = scale;

        if (alfa <= 0.0 ) //リセットし、また中心から外へ広がっていく動き
        {
            alfa = 1.0f;
            scale.x = 0.0f;
            scale.y = 0.0f;
            myTransform.localScale = scale;
        }
    }
}
