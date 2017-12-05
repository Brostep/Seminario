// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Basic Toon Shading"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_ToonRamp("Toon Ramp", 2D) = "white" {}
		_Color("Color", Color) = (0.1617647,0.1208149,0.04757784,0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _Color;
		uniform sampler2D _ToonRamp;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#if DIRECTIONAL
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float dotResult3 = dot( i.worldNormal , ase_worldlightDir );
			float2 temp_cast_0 = (saturate( (dotResult3*0.5 + 0.5) )).xx;
			float3 ase_worldNormal = i.worldNormal;
			float3 indirectDiffuse11 = ShadeSH9( float4( ase_worldNormal, 1 ) );
			c.rgb = ( ( _Color * tex2D( _ToonRamp, temp_cast_0 ) ) * ( _LightColor0 * float4( ( indirectDiffuse11 + ase_lightAtten ) , 0.0 ) ) ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
48;249;926;625;502.8858;311.1557;1.701839;True;False
Node;AmplifyShaderEditor.CommentaryNode;48;-1317.034,88.54639;Float;False;540.401;320.6003;Comment;3;1;3;2;N . L;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-1205.034,136.5463;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;51;-737.7677,127.9524;Float;False;723.599;290;Also know as Lambert Wrap or Half Lambert;3;5;4;15;Diffuse Wrap;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-1253.034,296.5461;Float;False;1;0;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;5;-687.7677,302.9521;Float;False;Constant;_WrapperValue;Wrapper Value;0;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;3;-917.0375,200.5462;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;52;-175.7287,537.7321;Float;False;812;304;Comment;5;7;8;11;10;12;Attenuation and Ambient;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;4;-422.3666,177.9523;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LightAttenuation;7;-105.3287,721.7319;Float;False;0;1;FLOAT
Node;AmplifyShaderEditor.SaturateNode;15;-189.1685,184.7519;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.IndirectDiffuseLighting;11;80.27132,665.7321;Float;False;Tangent;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleAddOpNode;12;320.2713,697.7319;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;6;62.10143,155.0119;Float;True;Property;_ToonRamp;Toon Ramp;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LightColorNode;8;-63.72859,585.7321;Float;False;0;1;COLOR
Node;AmplifyShaderEditor.ColorNode;54;104.6707,-81.40741;Float;False;Property;_Color;Color;3;0;0.1617647,0.1208149,0.04757784,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;470.3012,136.4119;Float;False;2;2;0;COLOR;1,1,1,0;False;1;COLOR;1,1,1,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;480.2714,585.7321;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;53;802.0688,683.9769;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;771.7013,388.2116;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1157.733,343.8546;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Basic Toon Shading;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0.001;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;14;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;4;0;3;0
WireConnection;4;1;5;0
WireConnection;4;2;5;0
WireConnection;15;0;4;0
WireConnection;12;0;11;0
WireConnection;12;1;7;0
WireConnection;6;1;15;0
WireConnection;42;0;54;0
WireConnection;42;1;6;0
WireConnection;10;0;8;0
WireConnection;10;1;12;0
WireConnection;23;0;42;0
WireConnection;23;1;10;0
WireConnection;0;2;23;0
ASEEND*/
//CHKSM=FE7CA5213A7BB27E887766673A07D45B4CF6ACB2