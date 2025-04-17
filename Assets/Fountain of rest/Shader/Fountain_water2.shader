//スクリプト(シェーダープログラム、C#)はコピー、流用、改変、2次配布、再販禁止です(そのほか諸々は利用規約をご覧ください)
//憩いの噴水(Version2.0) 最終更新日:2024/01/04 著:ねじにんじん
//
//変更点(Version1.0 -> Version2.0)
//1.透明度をマテリアルのインスペクタ欄から調整できるように変更

Shader "Custom/Fountain_water2"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        //_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        //透明度を指定可能に(インスペクタより)
        _Alphadata ("Alphadata", Range(0,1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Tags { "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types

        //#pragma surface surf Standard fullforwardshadows
        #pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        //透明度を追加
        half _Alphadata;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            //o.Alpha = c.a;

            //テクスチャの移動
            fixed2 uv = IN.uv_MainTex;
            uv.x  += ( _Time / 10);
            uv.y  += ( _Time / 5);
		    o.Albedo = tex2D(_MainTex,uv);

            //Surface構造体の設定(透明(アルファ値)を追加)
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Alphadata;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
