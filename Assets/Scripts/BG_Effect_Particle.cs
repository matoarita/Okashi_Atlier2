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

        //äeÉpÅ[ÉeÉBÉNÉãéÊìæ
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

            case "effect_sc03":

                particleEm_Light1 = Eff_obj.transform.Find("BG_Particle_Light").GetComponent<ParticleSystem>().emission;
                particleEm_Light2 = Eff_obj.transform.Find("BG_Particle_Light_2").GetComponent<ParticleSystem>().emission;
                particleEm_Light3 = Eff_obj.transform.Find("BG_Particle_Light_Ball").GetComponent<ParticleSystem>().emission;
                particleEm_Light4 = Eff_obj.transform.Find("BG_Particle_Light_Night").GetComponent<ParticleSystem>().emission;

                //particleEm_Light5 = Eff_obj.transform.Find("BG_Particle_Light_Morning").GetComponent<ParticleSystem>().emission;
                //particleEm_Light5 = Eff_obj.transform.Find("BG_Particle_Light_Night").GetComponent<ParticleSystem>().emission;
                //particleEm_Light6 = Eff_obj.transform.Find("BG_Particle_Light_twilight").GetComponent<ParticleSystem>().emission;
                //particleEm_Light7 = Eff_obj.transform.Find("BG_Particle_Light_moon").GetComponent<ParticleSystem>().emission;
                break;
        }
        
        SetOK = true;
    }

    public void Koushin()
    {
        switch (eff_name)
        {
            case "effect_sc01":

                EffLibrary_1();             
                break;

            case "effect_sc03":

                EffLibrary_3();
                break;

            default:

                break;
        }
    }

    void EffLibrary_1()
    {
        switch (GameMgr.BG_cullent_weather) //TimeControllerÇ≈ïœçX
        {
            case 1:

                break;

            case 2: //ê[ñÈÅ®í©

                break;

            case 3: //í©

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(200);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);

                break;

            case 4: //íã

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);

                break;

            case 5: //ó[ï˚

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(5);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light5.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light6.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light7.rateOverTime = new ParticleSystem.MinMaxCurve(0);

                break;

            case 6: //ñÈ

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

    void EffLibrary_3()
    {
        switch (GameMgr.BG_cullent_weather) //TimeControllerÇ≈ïœçX
        {
            case 1:

                break;

            case 2: //ê[ñÈÅ®í©

                break;

            case 3: //í©

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                break;

            case 4: //íã

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                break;

            case 5: //ó[ï˚

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(1);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(3);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                break;

            case 6: //ñÈ

                particleEm_Light1.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light2.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light3.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                particleEm_Light4.rateOverTime = new ParticleSystem.MinMaxCurve(5);
                break;
        }
    }
}
