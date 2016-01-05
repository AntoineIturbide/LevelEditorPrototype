Shader "Custom/V1" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_GroundTex ("Ground (RGB)", 2D) = "white" {}
		_WallTex("Walls (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _GroundTex;
		sampler2D _WallTex;

		struct Input {
			float3 worldPos;
			float2 uv_GroundTex;
			float2 uv_WallTex;
			float4 color: Color;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float2 worlduv = IN.worldPos.xz;
			half4 groundTex = tex2D(_GroundTex, worlduv / IN.uv_GroundTex);
			half4 wallTex = tex2D(_WallTex, IN.uv_WallTex);
			o.Albedo = lerp(groundTex.rgb, wallTex.rgb, IN.color.r);

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
