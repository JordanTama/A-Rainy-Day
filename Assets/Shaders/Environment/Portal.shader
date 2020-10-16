Shader "Environment/Portal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        [HDR] _Col1 ("Colour 1", Color) = (1, 1, 1, 1)
        [HDR] _Col2 ("Colour 2", Color) = (1, 1, 1, 1)
        
        _Exposure ("Exposure", Range(0, 1)) = .5
        _Step ("Step Sharpness", Range(0, 1)) = .05
        
        _Disp ("Displacement Texture", 2D) = "grey" {}
        _DispAmt ("Displacement Amount", Float) = .2
        
        _RadialScale ("Stretch", Float) = 1
        _LengthScale ("Length", Float) = 1
        
        _Speed ("Speed", Vector) = (1, 1, 0, 0)
        
        
        _Threshold ("Threshold depth", Float) = 1
        _Exponent ("Exponent", Float) = 1
    }
    SubShader
    {        
        Pass
        {
            Cull Off
            
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
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Col1;
            float4 _Col2;

            float _Exposure;
            float _Step;

            sampler2D _Disp;
            float _DispAmt;

            float _RadialScale;
            float _LengthScale;

            float2 _Speed;

            sampler2D _CameraDepthTexture;
            float _Threshold;
            float _Exponent;

            static float2 getPolar(float2 uv, float2 centre, float radialScale, float lengthScale)
            {
                float2 delta = uv - centre;
                float radius = length(delta) * 2 * radialScale;
                float angle = atan2(delta.x, delta.y) * 1.0/6.28 * lengthScale;
                return float2(radius, angle);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                float diff = saturate(_Threshold * (depth - i.screenPos.w));
                float edge = pow(diff, _Exponent);
                
                float2 disp = tex2D(_Disp, i.uv).rg;
                disp = disp * 2 - 1;
                disp *= _DispAmt;
                
                float2 polar = getPolar(i.uv, float2(0.5, 0.5), _RadialScale, _LengthScale);
                polar += disp;
                polar.x += _Time[1] * _Speed.x;
                polar.y += _Time[1] * _Speed.y;
                float col = tex2Dlod(_MainTex, float4(polar, 0, 3)).r;
                
                float step = _Step * 0.5;
                float mid = lerp(step, 1 - step, _Exposure);
                col = smoothstep(col, mid - step , mid + step);
                
                fixed4 output = lerp(_Col1, _Col2, min(col, edge));
                UNITY_APPLY_FOG(i.fogCoord, output);
                return output;
            }
            ENDCG
        }
    }
}
