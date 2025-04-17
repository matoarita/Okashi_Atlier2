//スクリプト(シェーダープログラム、C#)はコピー、流用、改変、2次配布、再販禁止です(そのほか諸々は利用規約をご覧ください)
//憩いの噴水(Version2.0) 最終更新日:2024/01/04 著:ねじにんじん
//
//変更点(Version1.0 -> Version2.0)
//1.インスペクタ入力値の名称を変更
//  (Main_Water_Moved -> Main_Water_Playing)
//  (Main_Water_Rotation -> Main_Water_Interval)
//  (Sub_Water_Moved -> Sub_Water_Playing)
//  (Sub_Water_Rotation -> Sub_Water_Interval)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain_of_rest : MonoBehaviour
{
    public float Main_Water_Power;//水量(メイン)
    public float Main_Water_Radial;//噴水の角度(メイン)
    /*Version1.0
    public float Main_Water_Moved;//噴水の出ている時間(メイン)
    public float Main_Water_Rotation;//次の噴水までの間隔(メイン)
    Version1.0 */
    //Version2.0 名称を変更
    public float Main_Water_Playing;//噴水の出ている時間(メイン)
    public float Main_Water_Interval;//次の噴水までの間隔(メイン)
    //Version2.0
    public float Sub_Water_Power;//水量(サブ)
    public float Sub_Water_Radial;//噴水の角度(サブ)
    /*Version1.0
    public float Sub_Water_Moved;//噴水の出ている時間(サブ)
    public float Sub_Water_Rotation;//次の噴水までの間隔(サブ)
    Version1.0 */
    //Version2.0 名称を変更
    public float Sub_Water_Playing;//噴水の出ている時間(サブ)
    public float Sub_Water_Interval;//次の噴水までの間隔(サブ)
    //Version2.0

    private ParticleSystem[] waterjet_particles;//0が中央。1以降はサブ
    private int waterjet_count;//パーティクル数(0が中央。1以降はサブ)
    private ParticleSystem.MainModule[] waterjet_main;//0が中央。1以降はサブ
    private ParticleSystem.EmissionModule[] waterjet_emission;//0が中央。1以降はサブ
    private ParticleSystem.ShapeModule[] waterjet_shape;//0が中央。1以降はサブ

    private ParticleSystem.MinMaxCurve main_startspeed_curve;//パーティクル速度用(メイン)
    private ParticleSystem.MinMaxCurve sub_startspeed_curve;//パーティクル速度用(サブ)

    private const float min_water_power = 0.0f;//水量最小値
    private const float max_water_power = 100.0f;//水量最大値

    private const float min_water_radial = 1.0f;//角度最小値
    private const float max_water_radial = 50.0f;//角度最大値

    private const float min_water_moved = 1.0f;//稼働最小値
    private const float max_water_moved = 300.0f;//稼働最大値

    private const float min_water_rotation = 1.0f;//間隔最小値
    private const float max_water_rotation = 300.0f;//間隔最大値

    private const int power_plus = 10;//水量の入力値に対する補正計算値
    private const int min_startspeed = 2;//パーティクル速度最小値
    private const int speed_calc = 30;//水量によるパーティクル速度への補正計算値


    // Start is called before the first frame update
    void Start()
    {
        //初期化

        waterjet_count = this.GetComponentsInChildren<ParticleSystem>().Length;

        if(waterjet_count > 1){//パーティクルがメインとサブで2個未満なら、処理を行わない
            waterjet_particles = new ParticleSystem[waterjet_count];
            waterjet_main = new ParticleSystem.MainModule[waterjet_count];
            waterjet_emission = new ParticleSystem.EmissionModule[waterjet_count];
            waterjet_shape = new ParticleSystem.ShapeModule[waterjet_count];
            main_startspeed_curve = new ParticleSystem.MinMaxCurve();
            sub_startspeed_curve = new ParticleSystem.MinMaxCurve();

            //初期設定
            waterjet_particles = this.GetComponentsInChildren<ParticleSystem>();
            main_startspeed_curve.mode = ParticleSystemCurveMode.TwoConstants;
            sub_startspeed_curve.mode = ParticleSystemCurveMode.TwoConstants;
            for(int no=0;no<waterjet_count;no++){
                waterjet_particles[no].Stop();
                waterjet_particles[no].Clear();
                waterjet_main[no] = waterjet_particles[no].main;
                waterjet_emission[no] = waterjet_particles[no].emission;
                waterjet_shape[no] = waterjet_particles[no].shape;
            }
            StartCoroutine("main_water_check");
            StartCoroutine("sub_water_check");
        }
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */

    //パーティクルコントロール(メイン)
    private IEnumerator main_water_check(){
        float times = new float();
        /*Version1.0
        float wait_times = Main_Water_Moved;
        
        check_watardata(Main_Water_Power,Main_Water_Radial,Main_Water_Moved,true);//Rotaion以外の設定値のチェック
        Version1.0 */
        //Version2.0 名称を変更
        float wait_times = Main_Water_Playing;
        
        check_watardata(Main_Water_Power,Main_Water_Radial,Main_Water_Playing,true);//Rotaion以外の設定値のチェック
        //Verison2.0

        set_waterdata();//値を設定に入力
        if(Main_Water_Power > min_water_power){
            playstop_waterjet(0,true);//パーティクルの再生
        }
        
        while(times<wait_times){
            times += Time.deltaTime;
            yield return null;
        }

        playstop_waterjet(0,false);//パーティクルの停止

        /*Version1.0
        check_watarrotation(Main_Water_Rotation,true);//Rotaionのみ待ち時間を開始する直前にチェック
        Version1.0 */
        //Version2.0 名称を変更
        check_watarrotation(Main_Water_Interval,true);//Rotaionのみ待ち時間を開始する直前にチェック
        //Verison2.0

        StartCoroutine("main_water_wait");
        yield break;
    }

    //パーティクルコントロール(サブ)
    private IEnumerator sub_water_check(){
        float times = new float();
        /*Version1.0
        float wait_times = Sub_Water_Moved;
        
        check_watardata(Sub_Water_Power,Sub_Water_Radial,Sub_Water_Moved,false);//Rotaion以外の設定値のチェック
        Version1.0 */
        //Version2.0 名称を変更
        float wait_times = Sub_Water_Playing;
        
        check_watardata(Sub_Water_Power,Sub_Water_Radial,Sub_Water_Playing,false);//Rotaion以外の設定値のチェック
        //Version2.0

        set_waterdata();//値を設定に入力
        for(int subno=1;subno<waterjet_count;subno++){
            if(Sub_Water_Power > min_water_power){
                playstop_waterjet(subno,true);//パーティクルの再生
            }
        }

        while(times<wait_times){
            times += Time.deltaTime;
            yield return null;
        }

        for(int subno=1;subno<waterjet_count;subno++){
            playstop_waterjet(subno,false);//パーティクルの停止
        }

        /*Version1.0
        check_watarrotation(Sub_Water_Rotation,false);//Rotaionのみ待ち時間を開始する直前にチェック
        Version1.0 */
        //Version2.0 名称を変更
        check_watarrotation(Sub_Water_Interval,false);//Rotaionのみ待ち時間を開始する直前にチェック
        //Version2.0


        StartCoroutine("sub_water_wait");
        yield break;
    }

    //設定値のチェック(共通)
    private void check_watardata(float water_power,float water_radial,float water_moved,bool main_bool){
        if(water_power < min_water_power){
            water_power = min_water_power;
        }
        if(water_power > max_water_power){
            water_power = max_water_power;
        }
        if(water_radial < min_water_radial){
            water_radial = min_water_radial;
        }
        if(water_radial > max_water_radial){
            water_radial = max_water_radial;
        }
        if(water_moved < min_water_moved){
            water_moved = min_water_moved;
        }
        if(water_moved > max_water_moved){
            water_moved = max_water_moved;
        }

        /*Version1.0
        if(main_bool==true){
            Main_Water_Power = water_power;
            Main_Water_Radial = water_radial;
            Main_Water_Moved = water_moved;
        }else{
            Sub_Water_Power = water_power;
            Sub_Water_Radial = water_radial;
            Sub_Water_Moved = water_moved;
        }
        Version1.0 */
        //Version2.0 名称を変更
        if(main_bool==true){
            Main_Water_Power = water_power;
            Main_Water_Radial = water_radial;
            Main_Water_Playing = water_moved;
        }else{
            Sub_Water_Power = water_power;
            Sub_Water_Radial = water_radial;
            Sub_Water_Playing = water_moved;
        }
        //Version2.0
    }

    //Rotaion値のチェック
    //待ちの直前で行う。他の値と一緒に処理した場合、稼働中に値の変更を行うとチェック済みのため入力値がスルーになってしまう
    private void check_watarrotation(float water_rotation,bool main_bool){
        if(water_rotation < min_water_rotation){
            water_rotation = min_water_rotation;
        }
        if(water_rotation > max_water_rotation){
            water_rotation = max_water_rotation;
        }
        /*Version1.0
        if(main_bool==true){
            Main_Water_Rotation = water_rotation;
        }else{
            Sub_Water_Rotation = water_rotation;
        }
        Version1.0 */
        //Version2.0 名称を変更
        if(main_bool==true){
            Main_Water_Interval = water_rotation;
        }else{
            Sub_Water_Interval = water_rotation;
        }
        //Version2.0
    }

    //値を設定に入力(共通)
    private void set_waterdata(){
        for(int no=0;no<waterjet_count;no++){
            switch(no){
                case 0:
                    if(Main_Water_Power > min_water_power){
                        main_startspeed_curve.constantMin = min_startspeed;
                        main_startspeed_curve.constantMax = (min_startspeed + 1) + Mathf.RoundToInt(Main_Water_Power / speed_calc);
                        waterjet_main[no].startSpeed = main_startspeed_curve;
                        waterjet_emission[no].rateOverTime = Main_Water_Power * power_plus;
                        waterjet_shape[no].angle = Main_Water_Radial;
                    }
                break;
                default:
                    if(Sub_Water_Power > min_water_power){
                        sub_startspeed_curve.constantMin = min_startspeed;
                        sub_startspeed_curve.constantMax = (min_startspeed + 1) + Mathf.RoundToInt(Sub_Water_Power / speed_calc);
                        waterjet_main[no].startSpeed = sub_startspeed_curve;
                        waterjet_emission[no].rateOverTime = Sub_Water_Power * power_plus;
                        waterjet_shape[no].angle = Sub_Water_Radial;
                    }
                break;
            }
        }
    }

    //パーティクルのRotation(待ち時間)(メイン)
    private IEnumerator main_water_wait(){
        float times = new float();
        /*Version1.0
        float wait_times = Main_Water_Rotation;
        while(times<Main_Water_Rotation){
            times += Time.deltaTime;
            yield return null;
        }
        Version1.0 */
        //Version2.0 名称を変更
        float wait_times = Main_Water_Interval;
        while(times<Main_Water_Interval){
            times += Time.deltaTime;
            yield return null;
        }
        //Version2.0

        StartCoroutine("main_water_check");
        yield break;
    }

    //パーティクルのRotation(待ち時間)(サブ)
    private IEnumerator sub_water_wait(){
        float times = new float();
        /*Version1.0
        float wait_times = Sub_Water_Rotation;
        while(times<Sub_Water_Rotation){
            times += Time.deltaTime;
            yield return null;
        }
        Version1.0 */
        //Version2.0 名称を変更
        float wait_times = Sub_Water_Interval;
        while(times<Sub_Water_Interval){
            times += Time.deltaTime;
            yield return null;
        }
        //Version2.0

        StartCoroutine("sub_water_check");
        yield break;
    }

    //パーティクルの再生/停止(共通)
    private void playstop_waterjet(int waterjet_no,bool playbool){
        if(playbool == true){
            waterjet_particles[waterjet_no].Play();
        }else{
            waterjet_particles[waterjet_no].Stop();
        }
    }

}
