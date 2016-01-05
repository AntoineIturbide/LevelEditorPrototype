Shader "Custom/V2" {Properties 
	{
		_GroundTex ("Ground Texture ", 2D) = "white" {}
		_WallTex ("Wall Texture", 2D) = "white" {}
		_TriplanarBlendSharpness ("Blend Sharpness",float) = 1
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert

		sampler2D _GroundTex;
		sampler2D _WallTex;

		float _TextureScale;
		float _TriplanarBlendSharpness;

		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_GroundTex;
			float2 uv_WallTex;
		}; 

		void surf (Input IN, inout SurfaceOutput o) 
		{
			// Find our UVs for each axis based on world position of the fragment.
			half2 yUV = IN.worldPos.xz / IN.uv_GroundTex;
			half2 xUV = IN.worldPos.zy / IN.uv_WallTex;
			half2 zUV = IN.worldPos.xy / IN.uv_WallTex;
			// Now do texture samples from our diffuse map with each of the 3 UV set's we've just made.
			half3 yTex = tex2D (_GroundTex, yUV);
			half3 xTex = tex2D (_WallTex, xUV);
			half3 zTex = tex2D (_WallTex, zUV);
			// Get the absolute value of the world normal.
			// Put the blend weights to the power of BlendSharpness, the higher the value, 
            // the sharper the transition between the planar maps will be.
			half3 blendWeights = pow (abs(IN.worldNormal), _TriplanarBlendSharpness);
			// Divide our blend mask by the sum of it's components, this will make x+y+z=1
			blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);
			// Finally, blend together all three samples based on the blend mask.
			o.Albedo = xTex * blendWeights.x + yTex * blendWeights.y + zTex * blendWeights.z;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
