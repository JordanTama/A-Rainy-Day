Shader "Crowd/Pedestrian"
{
    Properties
    {
        _Col ("Silouette Color", Color) = (0, 0, 0, 1)
        _Line ("Outline Color", Color) = (1, 1, 1, 1)
        _Weight ("Outline Weight", Float) = 2 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
            Stencil
            {
                Ref 100
                Pass Replace    
                Comp Always
            }
            
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            float4 _Col;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = _Col;
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return col;
            }
            ENDCG
        }
        
        Pass
        {
            Stencil
            {
                Ref 100
                Comp NotEqual
                Pass Keep
            }
            
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            float4 _Line;
            float _Weight;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));

                float2 offset = normalize(clipNormal.xy) / _ScreenParams.xy * _Weight * o.vertex.w * 2;
                
                o.vertex.xy += offset;
                
                o.uv = v.uv;
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = _Line;
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return col;
            }
            ENDCG
        }
    }
}
