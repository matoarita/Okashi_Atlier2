using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Effect_Particle : MonoBehaviour
{
    private string eff_name;
    private GameObject Eff_obj;
    private bool SetOK = false;

    private ParticleSystem.EmissionModule particleEm_Light1;
    private ParticleSystem.EmissionModule particleEm_Light2;
    private ParticleSystem.EmissionModule particleEm_Light3;
    private ParticleSystem.EmissionModule particleEm_Light4;
    private ParticleSystem.EmissionModule particleEm_Light5;
    private ParticleSystem.EmissionModule particleEm_Light6;
    private ParticleSystem.EmissionModule particleEm_Light7;
    private ParticleSystem.EmissionModule particleEm_Light8;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SetOK)
        {
            
        }
    }

    public void SetEffectName(string _name)
    {
        eff_name = _name;
        Eff_obj = this.transform.Find(eff_name).gameObject;
        Debug.Log("BGエフェクト名: " + eff_name);

        //各パーティクル取得
        switch (eff_name)
        {
            case "effect_sc01":

                particleEm_Light1 = Eff_obj.transform.Find("BG_Particle_Light").GetComponent<ParticleSystem>().emission;
                particleEm_Light2 = Eff_obj.transform.Find("BG_Particle_Light_Ball").GetComponent<ParticleSystem>().emission;
                particleEm_Light3 = Eff_obj.transform.Find("BG_Particle_Light_Kira").GetComponent<ParticleSystem>().emission;
                particleEm_Light4 = Eff_obj.transform.Find("BG_Particle_Light_Morning").GetComponent<ParticleSystem>().emission;
                particleEm_Light5 = Eff_obj.transform.Find("BG_Particle_Light_Night").GetComponent<ParticleSystem>().emission;
                particleEm_Light6 = Eff_obj.transform.Find("BG_Particle_Light_twilight").GetComponent<ParticleSystem>().emission;
                particleEm_Light7 = Eff_obj.transform.Find("BG_Particle_Light_moon").GetComponent<ParticleSystem>().emission;
                particleEm_Light8 = Eff_obj.transform.Find("BG_Particle_Rain").GetComponent<ParticleSystem>().emission;
                break;

            case "effect_sc02":

                particleEm_Light1 = Eff_obj.transform.Find("BG_Particle_Light").GetComponent<ParticleSystem>().emission;
                particleEm_Light8 = Eff_obj.transform.Find("BG_Particle_Light (1)").GetComponent<ParticleSystem>().emission;
                particleEm_Light2 = Eff_obj.transform.Find("BG_Particle_Light_Ball").GetComponent<ParticleSystem>().emission;
                particleEm_Light3 = Eff_obj.transform.Find("BG_Particle_Light_Kira").GetComponent<ParticleSystem>().emission;
                particleEm_Light4 = Eff_obj.transform.Find("BG_Particle_Light_Morning").GetComponent<ParticleSystem>().emission;
                particleEm_Light5 = Eff_obj.transform.Find("BG_Particle_Light_Night").GetComponent<ParticleSystem>().emission;
                particleEm_Light6 = Eff_obj.transform.Find("BG_Particle_Light_twilight").GetComponent<ParticleSystem>().emission;
                particleEm_Light7 = Eff_obj.transform.Find("BG_Particle_Light_moon").GetComponent<ParticleSystem>().emission;

                //particleEm_Light8 = Eff_obj.transform.Find("BG_Particle_Rain").GetComponent<ParticleSystem>().emission;
                break;

            case "effect_sc03":

                particleEm_Light1 = Eff_obj.transform.Find("BG_Particle_Light").GetComponent<ParticleSystem>().emission;
                particleEm_Light2 = Eff_obj.transform.Find("BG_Particle_Light_2").GetComponent<ParticleSystem>().emission;
                particleEm_Light3 = Eff_obj.transform.Find("BG_Particle_Light_Ball").GetComponent<ParticleSystem>().emission;
                particleEm_Light4 = Eff_obj.transform.Find("BG_Particle_Light_Night").GetComponent<ParticleSystem>().emission;

                //particleEm_Light5 = Eff_obj.transform.Find("BG_Particle_Light_Morning").GetComponent<ParticleSystem>().emission;
                //particleEm_Light6 = Eff_obj.transform.Find("BG_Particle_Light_Night").GetComponent<ParticleSystem>().emission;
                //particleEm_Light7 = Eff_obj.transform.Find("BG_Particle_Light_twilight").GetComponent<ParticleSystem>().emission;
                //particleEm_Light8 = Eff_obj.transform.Find("BG_Particle_Light_moon").GetComponent<ParticleSystem>().emission;
                break;

            case "effect_sc04":

                particleEm_Light1 = Eff_obj.transform.Find("BG_Particle_Light").GetComponent<ParticleSystem>().emission;
                particleEm_Light2 = Eff_obj.transform.Find("BG_Particle_Light_2").GetComponent<ParticleSystem>().emission;
                particleEm_Light3 = Eff_obj.transform.Find("BG_Particle_Light_Ball").GetComponent<ParticleSystem>().emission;
                particleEm_Light4 = Eff_obj.transform.Find("BG_Particle_Light_Night").GetComponent<ParticleSystem>().emission;

                //particleEm_Light5 = Eff_obj.transform.Find("BG_Particle_Light_Morning").GetComponent<ParticleSystem>().emission;
                //particleEm_Light6 = Eff_obj.transform.Find("BG_Particle_Light_Night").GetComponent<ParticleSystem>().emission;
                //particleEm_Light7 = Eff_obj.transform.Find("BG_Particle_Light_twilight").GetComponent<ParticleSystem>().emission;
                //particleEm_Light8 = Eff_obj.transform.Find("BG_Particle_Light_moon").GetComponent<ParticleSystem>().emission;
                break;
        }
        
        SetOK = true;
    }

    public void WeatherEffect_Koushin()
    {
        Debug.Log("BGエフェクト名 更新: " + eff_name);
        switch (eff_name)
        {
            case "effect_sc01":

                EffLibrary_1();             
                break;

            case "effect_sc02":

                EffLibrary_2();
                break;

            case "effect_sc03":

                EffLibrary_3();
                break;

            case "effect_sc04":

                EffLibrary_3();
                break;

            default:

                break;
        }
    }

    void EffLibrary_1()
    {
        switch (GameMgr.BG_cullent_weather) //TimeControllerで変更
        {
            case 1: //深夜→朝

                break;

            case 2: //朝

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(200);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                break;

            case 3: //昼

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);

                break;

            case 4: //昼

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);

                break;

            case 5: //夕方

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(5);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);

                break;

            case 6: //夜

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(200);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(1);

                break;
        }
    }

    void EffLibrary_2()
    {
        switch (GameMgr.BG_cullent_weather) //TimeControllerで変更
        {
            case 1: //深夜→朝

                break;

            case 2: //朝

                //Debug.Log("BGエフェクト　朝更新");
                Eff_obj.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
                Eff_obj.transform.Find("BG_Particle_Light (1)").gameObject.SetActive(true);
                Eff_obj.transform.Find("BG_Particle_Light_Night").gameObject.SetActive(false);
                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light8.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(2);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                break;

            case 3: //昼

                //Debug.Log("BGエフェクト　昼更新");
                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light8.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);

                break;

            case 4: //昼

                //Debug.Log("BGエフェクト　昼更新");
                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light8.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);

                break;

            case 5: //夕方

                //Debug.Log("BGエフェクト　夕方更新");
                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light8.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(5);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);

                break;

            case 6: //夜

                //Debug.Log("BGエフェクト　夜更新");
                Eff_obj.transform.Find("BG_Particle_Light").gameObject.SetActive(false);
                Eff_obj.transform.Find("BG_Particle_Light (1)").gameObject.SetActive(false);
                Eff_obj.transform.Find("BG_Particle_Light_Night").gameObject.SetActive(true);

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light8.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(200);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(1);

                break;
        }
    }

    void EffLibrary_3()
    {
        switch (GameMgr.BG_cullent_weather) //TimeControllerで変更
        {
            case 1: //深夜→朝

                break;

            case 2: //朝

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                break;

            case 3: //昼

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                break;

            case 4: //昼

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                break;

            case 5: //夕方

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                break;

            case 6: //夜

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(5);
                break;
        }
    }
}
