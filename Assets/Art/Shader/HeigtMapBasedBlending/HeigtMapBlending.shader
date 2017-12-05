// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HeigtMapBlending"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Floor_AO("Floor_AO", 2D) = "white" {}
		_Oclussion2("Oclussion 2", 2D) = "white" {}
		_Floor("Floor", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_FloorNom("FloorNom", 2D) = "white" {}
		_TextureSample5("Texture Sample 5", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.9176471
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _FloorNom;
		uniform float4 _FloorNom_ST;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform sampler2D _TextureSample5;
		uniform float4 _TextureSample5_ST;
		uniform sampler2D _Floor;
		uniform float4 _Floor_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _Smoothness;
		uniform sampler2D _Floor_AO;
		uniform float4 _Floor_AO_ST;
		uniform sampler2D _Oclussion2;
		uniform float4 _Oclussion2_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_FloorNom = i.uv_texcoord * _FloorNom_ST.xy + _FloorNom_ST.zw;
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float2 uv_TextureSample5 = i.uv_texcoord * _TextureSample5_ST.xy + _TextureSample5_ST.zw;
			float4 tex2DNode10 = tex2D( _TextureSample5, uv_TextureSample5 );
			float3 lerpResult8 = lerp( UnpackNormal( tex2D( _FloorNom, uv_FloorNom ) ) , UnpackNormal( tex2D( _TextureSample3, uv_TextureSample3 ) ) , tex2DNode10.r);
			o.Normal = lerpResult8;
			float2 uv_Floor = i.uv_texcoord * _Floor_ST.xy + _Floor_ST.zw;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 lerpResult7 = lerp( tex2D( _Floor, uv_Floor ) , tex2D( _TextureSample1, uv_TextureSample1 ) , tex2DNode10.r);
			o.Albedo = lerpResult7.xyz;
			o.Smoothness = _Smoothness;
			float2 uv_Floor_AO = i.uv_texcoord * _Floor_AO_ST.xy + _Floor_AO_ST.zw;
			float2 uv_Oclussion2 = i.uv_texcoord * _Oclussion2_ST.xy + _Oclussion2_ST.zw;
			float4 lerpResult9 = lerp( tex2D( _Floor_AO, uv_Floor_AO ) , tex2D( _Oclussion2, uv_Oclussion2 ) , tex2DNode10.r);
			o.Occlusion = lerpResult9.x;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
48;249;926;625;740.5933;158.6714;1;True;False
Node;AmplifyShaderEditor.SamplerNode;6;-902.9429,838.7502;Float;True;Property;_Oclussion2;Oclussion 2;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;5;-909.9182,597.4232;Float;True;Property;_Floor_AO;Floor_AO;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-982.9932,-102.8826;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-899.8076,-330.2856;Float;True;Property;_Floor;Floor;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;4;-970.2308,345.1348;Float;True;Property;_TextureSample3;Texture Sample 3;0;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-982.1725,111.1752;Float;True;Property;_FloorNom;FloorNom;0;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;10;-1607.324,132.9768;Float;True;Property;_TextureSample5;Texture Sample 5;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;9;-483.0038,695.585;Float;False;3;0;COLOR;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;13;-468.5933,7.32864;Float;False;Property;_Smoothness;Smoothness;7;0;0.9176471;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;8;-502.2353,268.0205;Float;False;3;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;7;-561.3369,-228.4449;Float;False;3;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;HeigtMapBlending;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;5;0
WireConnection;9;1;6;0
WireConnection;9;2;10;1
WireConnection;8;0;3;0
WireConnection;8;1;4;0
WireConnection;8;2;10;1
WireConnection;7;0;1;0
WireConnection;7;1;2;0
WireConnection;7;2;10;1
WireConnection;0;0;7;0
WireConnection;0;1;8;0
WireConnection;0;4;13;0
WireConnection;0;5;9;0
ASEEND*/
//CHKSM=4420830B2410062FD1D9C20970F7E2F529FC24B5