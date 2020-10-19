Shader "Environment/Mutable"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Mute ("Mute Value", Range(0, 1)) = 1
        _Tint ("Tint Value", Range(0, 1)) = 1
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        half _Mute;
        half _Tint;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 raw = tex2D (_MainTex, IN.uv_MainTex);
            float greyValue = 0.21 * raw.r + 0.71 * raw.g + 0.07 * raw.b;
            float4 greyScale = float4(raw.r * (1.0 - _Mute) + greyValue * _Mute, raw.g * (1.0 - _Mute) + greyValue * _Mute, raw.b * (1.0 - _Mute) + greyValue * _Mute, raw.a);
            
            fixed4 c = lerp(greyScale, _Color, _Tint);

            o.Albedo = c.rgb;
            o.Alpha = c.a;
            
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
