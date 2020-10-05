Shader "Crowd/VATPedestrian"
{
    Properties
    {
        _VertTex ("Vertex Animation Texture", 2D) = "white" {}
        _Col ("Silhouette Color", Color) = (0, 0, 0, 1)
        _ObsCol ("Obstruction Color", Color) = (0, 0, 0, 1)
        _Speed ("Animation Speed", Float) = 1
        _Offset ("Animation Cycle Offset", Range(0, 1)) = 0
    }
    
    SubShader
    {
        // Base pass
        Pass
        {
            Tags { "Queue"="Geometry" }
            Stencil
            {
                Ref 100
                Pass Replace
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                uint vid : SV_VertexID;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            sampler2D _VertTex;
            float4 _VertTex_TexelSize;
            
            float4 _Col;
            float _Speed;

            float _Offset;

            float4 vertex_sample(sampler2D tex, float4 texelSize, uint id)  
            {
                float vertexCoords = id;
                float animCoords = _Time[1] * _Speed / texelSize.y % texelSize.w;
                float4 texCoords = float4(vertexCoords + 0.5, animCoords + 0.5, 0, 0);
                texCoords.xy *= texelSize.xy;
                texCoords.y += _Offset;
                
                return tex2Dlod(tex, texCoords);
            }
            
            v2f vert (appdata v)
            {
                float4 position = vertex_sample(_VertTex, _VertTex_TexelSize, v.vid);

                v2f o;
                o.vertex = UnityObjectToClipPos(position);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Col;
            }
            ENDCG
        }
        
        // Shadow caster pass
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On ZTest LEqual

            CGPROGRAM
            #pragma target 2.0

            #pragma multi_compile_shadowcaster

            #pragma vertex vertShadowCaster
            #pragma fragment fragShadowCaster

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                uint vid : SV_VertexID;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            sampler2D _VertTex;
            uniform float4 _VertTex_TexelSize;
            
            float _Speed;
            float _Offset;

            float4 vertex_sample(sampler2D tex, float4 texelSize, uint id)  
            {
                float vertexCoords = id;
                float animCoords = _Time[1] * _Speed / texelSize.y % texelSize.w;
                float4 texCoords = float4(vertexCoords + 0.5, animCoords + 0.5, 0, 0);
                texCoords.xy *= texelSize.xy;
                texCoords.y += _Offset;
                
                return tex2Dlod(tex, texCoords);
            }
            
            v2f vertShadowCaster (appdata v)
            {
                float4 position = vertex_sample(_VertTex, _VertTex_TexelSize, v.vid);

                v2f o;
                o.vertex = UnityObjectToClipPos(position);
                return o;
            }

            fixed4 fragShadowCaster (v2f i) : SV_Target
            {
                return 0;
            }

            ENDCG
        }
        
        // Obstruction pass
        Pass
        {            
            Tags { "Queue"="Overlay" }
            Stencil
            {
                Ref 100
                Comp NotEqual
            }
            ZTest Greater
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                uint vid : SV_VertexID;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            sampler2D _VertTex;
            float4 _VertTex_TexelSize;
            
            float4 _ObsCol;
            float _Speed;

            float _Offset;

            float4 vertex_sample(sampler2D tex, float4 texelSize, uint id)  
            {
                float vertexCoords = id;
                float animCoords = _Time[1] * _Speed / texelSize.y % texelSize.w;
                float4 texCoords = float4(vertexCoords + 0.5, animCoords + 0.5, 0, 0);
                texCoords.xy *= texelSize.xy;
                texCoords.y += _Offset;
                
                return tex2Dlod(tex, texCoords);
            }
            
            v2f vert (appdata v)
            {
                float4 position = vertex_sample(_VertTex, _VertTex_TexelSize, v.vid);

                v2f o;
                o.vertex = UnityObjectToClipPos(position);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _ObsCol;
            }
            ENDCG
        }
    }
}
