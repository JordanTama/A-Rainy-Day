    Shader "Environment/Dissolve"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Alt ("Alternate Color", Color) = (1, 1, 1, 1)
        [HDR] _High ("Highlight Color", Color) = (1, 1, 1, 1)
        
        _Displacement ("Displacement Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _Sharpness ("Sharpness", Range(0,1)) = 0.9
        _Edge ("Edge Sharpness", Range(0, 1)) = 0.1
        _Bot ("Lowest Height", Float) = 0
        _Top ("Heighest Height", Float) = 1
        _Interp ("Interpolation", Range(0, 1)) = 1
        _DispAmt ("Displacement Amount", Float) = 1
        _DispScale ("Displacement Scale", Float) = 1
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _Displacement;

        fixed4 _Color;
        fixed4 _Alt;
        fixed4 _High;
        
        half _Glossiness;
        half _Metallic;
        half _Sharpness;

        half _Bot;
        half _Top;

        half _Interp;

        half _DispAmt;
        half _DispScale;

        half _Edge;

        struct Input
        {
            float2 uv_Displacement;
            float4 vertex;
            float3 worldPos;
        };

        float inverse_lerp(float a, float b, float v)
        {
            return (v - a) / (b - a);
        }

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.vertex = v.vertex;
            o.worldPos = mul(unity_ObjectToWorld, v.vertex);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float displacement = tex2D(_Displacement, IN.worldPos.xz / _DispScale);
            displacement = displacement * 2 - 1;
            displacement *= _DispAmt;
            
            float t = inverse_lerp(_Bot, _Top, IN.vertex.y + displacement);
            t = saturate(t);

            t = smoothstep(_Interp - _Sharpness * 0.5, _Interp + _Sharpness * 0.5, t);

            float high = 1 - (abs(0.5 - t) * 2);
            high = smoothstep(0, _Edge, high);
            
            fixed4 baseCol = lerp(_Color, _Alt, t);
            
            fixed4 c = lerp(baseCol, _High, high); 
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
