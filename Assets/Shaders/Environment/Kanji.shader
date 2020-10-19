Shader "Environment/Kanji"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Col ("Color", Color) = (1, 1, 1, 1)
        _Fade ("Fade", Range(0, 1)) = 1
        
        _DispTex ("Displacement Texture", 2D) = "gray" {}
        _DispAmt ("Displacement Amount", Float) = .1
        _DispDir ("Displacement Pan Direction", Vector) = (0, 1, 0, 0)
        _DispSpd ("Displacement Speed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 disp_uv : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 disp_uv : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Col;
            float _Fade;

            sampler2D _DispTex;
            float4 _DispTex_ST;
            float _DispAmt;
            float2 _DispDir;
            float _DispSpd;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.disp_uv = TRANSFORM_TEX(v.disp_uv, _DispTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 displacement = tex2D(_DispTex, i.disp_uv + _DispDir * _DispSpd * _Time[1]).rg;
                displacement = displacement * 2 - 1;
                displacement *= _DispAmt;
                
                float mask = 1 - tex2D(_MainTex, i.uv + displacement).r;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return float4(_Col.rgb, mask * _Fade);
            }
            ENDCG
        }
    }
}
