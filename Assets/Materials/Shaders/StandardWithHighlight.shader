// Upgrade NOTE: upgraded instancing buffer 'StandardWithHighlight' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "StandardWithHighlight"
{
	Properties
	{
		_Albedo("Albedo", Color) = (1,1,1,0)
		[HDR]_HighlightColor("HighlightColor", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
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
			half filler;
		};

		UNITY_INSTANCING_BUFFER_START(StandardWithHighlight)
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
			float4 _Albedo_Instance = UNITY_ACCESS_INSTANCED_PROP(_Albedo_arr, _Albedo);
			o.Albedo = _Albedo_Instance.rgb;
			float x33 = ( 1.0 - abs( sin( _Time.w ) ) );
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
2024;163;1577;694;1232.945;139.4062;1;True;True
Node;AmplifyShaderEditor.TimeNode;16;-1472.596,176.1216;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;15;-1210.084,38.92342;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;27;-1047.213,32.91812;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;34;-1021.945,364.5938;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;35;-876.9449,36.5938;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;33;-730.9449,34.5938;Inherit;False;return -(cos(PI * x) - 1) / 2@;1;False;2;True;x;FLOAT;0;In;;Inherit;False;True;PI;FLOAT;0;In;;Inherit;False;My Custom Expression;True;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;26;-341.2874,16.33269;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;-383.9446,187.5938;Inherit;False;InstancedProperty;_HighlightColor;HighlightColor;1;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;2.118547,1.763608,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;25;-610.2808,-228.6168;Inherit;False;InstancedProperty;_Albedo;Albedo;0;0;Create;True;0;0;False;0;False;1,1,1,0;0.6509804,0.6509804,0.6509804,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-130.9446,43.59381;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;28;-584.1974,224.432;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;130.9271,-81.14883;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;StandardWithHighlight;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;16;4
WireConnection;27;0;15;0
WireConnection;35;0;27;0
WireConnection;33;0;35;0
WireConnection;33;1;34;0
WireConnection;26;0;33;0
WireConnection;31;0;26;0
WireConnection;31;1;32;0
WireConnection;0;0;25;0
WireConnection;0;2;31;0
ASEEND*/
//CHKSM=821DA1BE8BE6515D4FA34BC5CC27927CAFBA952A