// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "My Shaders/Dissolve w Ramp"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_DissolveNoise("Dissolve Noise", 2D) = "white" {}
		_Dissolved("Dissolved", Range( 0 , 1)) = 0
		_Albedo("Albedo ", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_RampTexture("Ramp Texture", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _Color0;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _RampTexture;
		uniform float4 _RampTexture_ST;
		uniform sampler2D _DissolveNoise;
		uniform float4 _DissolveNoise_ST;
		uniform float _Dissolved;
		uniform float _MaskClipValue = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = ( UnpackNormal( tex2D( _Normal, uv_Normal ) ) * 2.0 );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = ( _Color0 * tex2D( _Albedo, uv_Albedo ) ).rgb;
			float2 uv_RampTexture = i.uv_texcoord * _RampTexture_ST.xy + _RampTexture_ST.zw;
			float2 uv_DissolveNoise = i.uv_texcoord * _DissolveNoise_ST.xy + _DissolveNoise_ST.zw;
			float temp_output_10_0 = ( tex2D( _DissolveNoise, uv_DissolveNoise ).r + (-0.8 + (_Dissolved - 0.0) * (0.8 - -0.8) / (1.0 - 0.0)) );
			float clampResult14 = clamp( (-8.0 + (temp_output_10_0 - 0.0) * (8.0 - -8.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			o.Emission = ( tex2D( _RampTexture, uv_RampTexture ) * ( 1.0 - clampResult14 ) * 7.5 ).xyz;
			o.Alpha = 1;
			clip( temp_output_10_0 - _MaskClipValue );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
47;141;1032;788;2183.997;1427.543;2.430812;True;False
Node;AmplifyShaderEditor.CommentaryNode;13;-1227.2,885.4998;Float;False;917.9009;505.0993;Dissolve;4;10;2;9;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1177.199,1211.599;Float;False;Property;_Dissolved;Dissolved;2;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-964.7999,935.4998;Float;True;Property;_DissolveNoise;Dissolve Noise;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;9;-738.4989,1188.599;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-0.8;False;4;FLOAT;0.8;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-527.4993,1004.599;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;26;-1402.198,278.3004;Float;False;863.9988;534.2992;Ramp;5;12;14;18;17;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemap;12;-1165.6,610.5995;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-8.0;False;4;FLOAT;8.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;14;-950.0985,602.1;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;23;-1080.898,-527.5004;Float;False;747.8004;491.5003;Texture;3;16;15;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;25;-832.0973,-10.00011;Float;False;474.7993;169.2496;Aux - hechos para este caso en particular;2;21;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-565.298,39.99989;Float;False;Constant;_DarkenValue;Darken Value;6;0;7.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;18;-1352.198,328.3004;Float;True;Property;_RampTexture;Ramp Texture;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;28;-577.23,-768.7925;Float;False;Property;_Color0;Color 0;6;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;17;-765.1974,566.8005;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-782.0973,44.24948;Float;False;Constant;_Normalamplifier;Normal amplifier;6;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-1019.199,-477.5004;Float;True;Property;_Albedo;Albedo ;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;16;-1030.898,-266.0001;Float;True;Property;_Normal;Normal;4;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-619.6978,-224.2009;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-707.1982,396.4;Float;False;3;3;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-249.0701,-365.2783;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;FLOAT4;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1.900001,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;My Shaders/Dissolve w Ramp;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Off;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;2;0
WireConnection;10;0;1;1
WireConnection;10;1;9;0
WireConnection;12;0;10;0
WireConnection;14;0;12;0
WireConnection;17;0;14;0
WireConnection;22;0;16;0
WireConnection;22;1;24;0
WireConnection;19;0;18;0
WireConnection;19;1;17;0
WireConnection;19;2;21;0
WireConnection;27;0;28;0
WireConnection;27;1;15;0
WireConnection;0;0;27;0
WireConnection;0;1;22;0
WireConnection;0;2;19;0
WireConnection;0;10;10;0
ASEEND*/
//CHKSM=78B0F6591EA7CEFA7E394A688D49BA0BBFA87A44