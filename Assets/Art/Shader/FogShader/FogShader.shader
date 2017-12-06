// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FogShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_FogIntensity("FogIntensity", Range( 0 , 1)) = 0
		_Color("Color", Color) = (0,0,0,0)
		_FoxMaxIntensity("FoxMaxIntensity", Range( 0 , 1)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard alpha:fade keepalpha noshadow dithercrossfade 
		struct Input
		{
			float4 screenPos;
		};

		uniform float4 _Color;
		uniform sampler2D _CameraDepthTexture;
		uniform float _FogIntensity;
		uniform float _FoxMaxIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color.rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos2 = ase_screenPos;
			float eyeDepth3 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos2))));
			float clampResult11 = clamp( ( abs( ( eyeDepth3 - ase_screenPos2.w ) ) * (0.01 + (_FogIntensity - 0.0) * (0.4 - 0.01) / (1.0 - 0.0)) ) , 0.0 , _FoxMaxIntensity );
			o.Alpha = clampResult11;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
248;138;1051;514;1512.435;273.6643;2.780018;True;True
Node;AmplifyShaderEditor.ScreenPosInputsNode;2;-985.9907,86.03132;Float;False;1;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;3;-730.0372,46.78963;Float;False;0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;7;-765.4368,346.4074;Float;False;Property;_FogIntensity;FogIntensity;1;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;4;-475.8198,115.7403;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;8;-343.6044,317.4818;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.01;False;4;FLOAT;0.4;False;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;6;-244.7749,119.823;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-85.68386,90.89738;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.1;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;10;-389.832,587.808;Float;False;Property;_FoxMaxIntensity;FoxMaxIntensity;2;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;9;-79.60016,-126.1242;Float;False;Property;_Color;Color;1;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;11;-55.20582,443.0753;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;142.2179,190.4273;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;FogShader;False;False;False;False;False;False;False;False;False;False;False;False;True;False;True;True;True;Back;0;0;False;0;0;Transparent;0.5;True;False;0;True;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;4;0;3;0
WireConnection;4;1;2;4
WireConnection;8;0;7;0
WireConnection;6;0;4;0
WireConnection;5;0;6;0
WireConnection;5;1;8;0
WireConnection;11;0;5;0
WireConnection;11;2;10;0
WireConnection;0;0;9;0
WireConnection;0;9;11;0
ASEEND*/
//CHKSM=4FE2842BD3AF2E2153EC9055D1EEA0BA5409204D