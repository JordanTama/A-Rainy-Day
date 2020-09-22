Shader "Custom/Refraction"
{
    Properties
    {
        _Col ("Tint Color", Color) = (1, 1, 1, .2)
        _DisplacementTex ("Texture", 2D) = "white" {}
        _DisplacementAmount ("Displacement Amount", Float) = 1
        _PanDirection ("Displacement Pan Direction", Vector) = (0, 1, 0, 0)
        _PanSpeed ("Displacement Pan Speed", Float) = 1
        _Offset ("Displacement Offset", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+200" }
        LOD 100

        GrabPass
        {
            "_GrabTexture"            
        }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
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
                float4 screenPos : TEXCOORD1;
            };

            float4 _Col;
            
            sampler2D _DisplacementTex;
            float4 _DisplacementTex_ST;

            float _DisplacementAmount;

            sampler2D _GrabTexture;

            float4 _PanDirection;
            float _PanSpeed;

            float _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _DisplacementTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float2 displacement = tex2D(_DisplacementTex, i.uv + normalize(_PanDirection) * _PanSpeed * _Time[1]).rg * 2 - 1;
                displacement -= _Offset;
                float4 col = tex2D(_GrabTexture, (i.screenPos / i.screenPos.w) + displacement * _DisplacementAmount);

                col.rgb = lerp(col.rgb, _Col.rgb, _Col.a);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
