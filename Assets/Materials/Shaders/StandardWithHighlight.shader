// Upgrade NOTE: upgraded instancing buffer 'StandardWithHighlight' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "StandardWithHighlight"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_Albedo("Albedo", Color) = (1,1,1,0)
		[HDR]_HighlightColor("HighlightColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex.SampleLevel(samplerTex,coord, lod)
		#define SAMPLE_TEXTURE2D_BIAS(tex,samplerTex,coord,bias) tex.SampleBias(samplerTex,coord,bias)
		#define SAMPLE_TEXTURE2D_GRAD(tex,samplerTex,coord,ddx,ddy) tex.SampleGrad(samplerTex,coord,ddx,ddy)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex2Dlod(tex,float4(coord,0,lod))
		#define SAMPLE_TEXTURE2D_BIAS(tex,samplerTex,coord,bias) tex2Dbias(tex,float4(coord,0,bias))
		#define SAMPLE_TEXTURE2D_GRAD(tex,samplerTex,coord,ddx,ddy) tex2Dgrad(tex,coord,ddx,ddy)
		#endif//ASE Sampling Macros

		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		UNITY_DECLARE_TEX2D_NOSAMPLER(_MainTexture);
		SamplerState sampler_MainTexture;
		uniform float LocalTime;

		UNITY_INSTANCING_BUFFER_START(StandardWithHighlight)
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainTexture_ST)
#define _MainTexture_ST_arr StandardWithHighlight
			UNITY_DEFINE_INSTANCED_PROP(float4, _Albedo)
#define _Albedo_arr StandardWithHighlight
			UNITY_DEFINE_INSTANCED_PROP(float4, _HighlightColor)
#define _HighlightColor_arr StandardWithHighlight
		UNITY_INSTANCING_BUFFER_END(StandardWithHighlight)


		float MyCustomExpression33( float x, float PI )
		{
			return -(cos(PI * x) - 1) / 2;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _MainTexture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTexture_ST_arr, _MainTexture_ST);
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST_Instance.xy + _MainTexture_ST_Instance.zw;
			float4 _Albedo_Instance = UNITY_ACCESS_INSTANCED_PROP(_Albedo_arr, _Albedo);
			o.Albedo = ( SAMPLE_TEXTURE2D( _MainTexture, sampler_MainTexture, uv_MainTexture ) * _Albedo_Instance ).rgb;
			float x33 = ( 1.0 - abs( sin( LocalTime ) ) );
			float PI33 = UNITY_PI;
			float localMyCustomExpression33 = MyCustomExpression33( x33 , PI33 );
			float4 _HighlightColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_HighlightColor_arr, _HighlightColor);
			o.Emission = ( saturate( localMyCustomExpression33 ) * _HighlightColor_Instance ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18400
58;266;1577;694;1427.955;586.1447;1.6;True;True
Node;AmplifyShaderEditor.RangedFloatNode;36;-1491.551,58.28702;Inherit;False;Global;LocalTime;LocalTime;2;0;Create;True;0;0;False;0;False;0;1.708241;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;15;-1210.084,38.92342;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;27;-1047.213,32.91812;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;34;-1021.945,364.5938;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;35;-876.9449,36.5938;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;33;-730.9449,34.5938;Inherit;False;return -(cos(PI * x) - 1) / 2@;1;False;2;True;x;FLOAT;0;In;;Inherit;False;True;PI;FLOAT;0;In;;Inherit;False;My Custom Expression;True;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;39;-994.3553,-403.7447;Inherit;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SaturateNode;26;-341.2874,16.33269;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;-383.9446,187.5938;Inherit;False;InstancedProperty;_HighlightColor;HighlightColor;2;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;25;-621.4809,-180.6169;Inherit;False;InstancedProperty;_Albedo;Albedo;1;0;Create;True;0;0;False;0;False;1,1,1,0;0.6509804,0.6509804,0.6509804,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;-703.1553,-400.5446;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-130.9446,43.59381;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;28;-584.1974,224.432;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-290.3553,-234.1446;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;130.9271,-81.14883;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;StandardWithHighlight;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;36;0
WireConnection;27;0;15;0
WireConnection;35;0;27;0
WireConnection;33;0;35;0
WireConnection;33;1;34;0
WireConnection;26;0;33;0
WireConnection;37;0;39;0
WireConnection;31;0;26;0
WireConnection;31;1;32;0
WireConnection;38;0;37;0
WireConnection;38;1;25;0
WireConnection;0;0;38;0
WireConnection;0;2;31;0
ASEEND*/
//CHKSM=26563B397D9451BE575F862EE2D3645217BE645D