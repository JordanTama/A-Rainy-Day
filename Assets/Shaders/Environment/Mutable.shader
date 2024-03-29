﻿Shader "Environment/Mutable"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Mute ("Mute Value", Range(0, 1)) = 1
        _Tint ("Tint Value", Range(0, 1)) = 1
        _HighlightMul ("Highlight Multiplier", Range(0, 1)) = 1
        _HighlightColor ("Highlight Color", Color) = (0, 0, 0, 0)
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
        float4 _HighlightColor;
        float _HighlightMul;

        uniform float LocalTime;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        float OlehsCustomExpression(float x)
        {
            return -(cos(UNITY_PI * x) - 1) / 2;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 raw = tex2D (_MainTex, IN.uv_MainTex);
            float greyValue = 0.21 * raw.r + 0.71 * raw.g + 0.07 * raw.b;
            float4 greyScale = float4(raw.r * (1.0 - _Mute) + greyValue * _Mute, raw.g * (1.0 - _Mute) + greyValue * _Mute, raw.b * (1.0 - _Mute) + greyValue * _Mute, raw.a);
            
            fixed4 c = lerp(greyScale, _Color, _Tint);

            o.Albedo = c.rgb;
            o.Alpha = c.a;

            float x33 = ( 1.0 - abs( sin( LocalTime ) ) );
            float localMyCustomExpression = OlehsCustomExpression(x33);
            
            o.Emission = _HighlightColor.rgb * saturate( localMyCustomExpression ) * _HighlightMul;
            
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
