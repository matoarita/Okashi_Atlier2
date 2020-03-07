using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]
public class Touch_Controll : MonoBehaviour
{

    //Rigidbody rigidBody;
    public Vector3 force = new Vector3(0, 10, 0);
    public ForceMode forceMode = ForceMode.VelocityChange;

    private SoundController sc;

    // Use this for initialization
    void Start()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //rigidBody = gameObject.GetComponent<Rigidbody>();

    }
    public void OnTouchFace()
    {
        Debug.Log("Touch_Face");

        sc.PlaySe(2);
    }

    public void OnTouchHair()
    {
        Debug.Log("Touch_Hair");

        sc.PlaySe(0);
    }

    public void OnTouchBell()
    {
        Debug.Log("Touch_Bell");

        sc.PlaySe(16);
    }

    public void OnTouchFlower()
    {
        Debug.Log("Touch_Flower");

        sc.PlaySe(2);
    }
}