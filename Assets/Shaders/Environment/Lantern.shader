Shader "Environment/Lantern"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _Noise ("Noise Texture", 2D) = "white" {}
        
        _FlickInt ("Flicker Intensity", Float) = 1
        _FlickSpd ("Flicker Speed", Float) = 1
        
        [HDR] _LightCol ("Candle Colour", Color) = (1, 1, 1, 1)
        _LightPos ("Candle Position", Vector) = (0, 0, 0, 0)
        _Exponent ("Exponent", Float) = 1
        
        _BobSpd ("Bob Speed", Float) = 1
        _BobAmt ("Bob Amount", Float) = 1
        
        _Offset ("Animation Cycle Offset", Range(0, 1)) = 0
        _Dissolve ("Dissolve Interpolation", Range(0, 1)) = 0
    }

    SubShader
    {
        // Surface pass
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        //Blend SrcAlpha OneMinusSrcAlpha
        //ZWrite Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow
        #pragma target 3.5

        #define PI 3.141592653589793238462

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Dissolve;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        sampler2D _Noise;
        float4 _LightCol;
        float4 _LightPos;
        float _Exponent;

        float _BobSpd;
        float _BobAmt;

        float _Offset;
        float _Dissolve;
        
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o)

            v.vertex.xyz += mul(unity_WorldToObject, float3(0, 1, 0)) * sin(_Time[1] * _BobSpd + PI * 2 * _Offset) * _BobAmt;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float dissolve = tex2D(_Noise, IN.uv_MainTex).r;
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;   
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;//saturate(_Dissolve * 2 - dissolve);
        }
        ENDCG
        
        
        // Unlit glow pass
        Pass
        {
            Tags { "Queue" = "Geometry" }
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5

            #include "UnityCG.cginc"
            
            #define PI 3.141592653589793238462

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float texcoord1 : TEXCOORD1;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD2;
                float3 worldLight : TEXCOORD3;
            };

            sampler2D _Noise;
            sampler2D _MainTex;

            float _FlickInt;
            float _FlickSpd;
            
            float4 _LightPos;
            float4 _LightCol;
            float _Exponent;

            float _BobAmt;
            float _BobSpd;

            float _Offset;
            
            v2f vert(appdata v)
            {
                v2f o;
                v.vertex.xyz += mul(unity_WorldToObject, float3(0, 1, 0)) * sin(_Time[1] * _BobSpd + PI * 2 * _Offset) * _BobAmt;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldLight = mul(unity_ObjectToWorld, float4(_LightPos.xyz, 1) + mul(unity_WorldToObject, float3(0, 1, 0)) * sin(_Time[1] * _BobSpd + PI * 2 * _Offset) * _BobAmt);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float mask = tex2D(_MainTex, i.uv).r;
                
                float intSamp = tex2D(_Noise, float2(_Time[1] * _FlickSpd, 0)).r * _FlickInt;
                intSamp = clamp(intSamp, -1, 1);

                float3 toVertex = normalize(i.worldPos - i.worldLight);
                float3 toCamera = normalize(_WorldSpaceCameraPos - i.worldPos);

                float dotProduct = dot(toVertex, toCamera);
                dotProduct = 0 + (dotProduct - intSamp) * (1 - 0) / (1 - intSamp);
                dotProduct = saturate(dotProduct);
                dotProduct = pow(dotProduct, _Exponent);

                return _LightCol * dotProduct * mask;
            }
            
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}