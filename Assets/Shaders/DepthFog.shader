Shader "Environment/DepthFog"
{
    Properties
    {
        _Col ("Fog Colour", Color) = (1, 1, 1, 1)
        _Threshold ("Threshold depth", Float) = 1
        _Exponent ("Exponent", Float) = 1
        _Disp ("Displacement Texture", 2D) = "black" {}
        _DispAmt ("Displacement Amount", Float) = 1
        _Pan ("Pan Speed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 screenPos : TEXCOORD2;
            };
            
			sampler2D _CameraDepthTexture;
            float4 _Col;
            float _Threshold;
            float _Exponent;
            sampler2D _Disp;
            float4 _Disp_ST;
            float _DispAmt;
            float _Pan;

            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Disp);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float displacement = tex2D(_Disp, float2(i.uv.x, i.uv.y + _Time[0] * _Pan)).r * _DispAmt;
                
                float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                float diff = saturate(_Threshold * ((depth - displacement) - i.screenPos.w));
                fixed4 col = lerp(float4(_Col.rgb, 0), _Col, pow(diff, _Exponent));
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
