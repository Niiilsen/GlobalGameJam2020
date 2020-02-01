Shader "Tanknarok/Stripes" {
	Properties {
		_Color1("Color 1", Color) = (1,1,1,1)
		_Color2("Color 2", Color) = (0,0,0,1)
		_Thickness("Thickness", Range(0.01, 1)) = 1
		_TrackSpeed("TrackSpeed", Float) = 0
	}
	SubShader {
		Tags { 
			"RenderType"="Opaque"
		}
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _Color1;
		fixed4 _Color2;
		float _Thickness;
		float _TrackSpeed;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Metallic = 0;
			o.Smoothness = 0;

			float2 thickness = float2(1.0, 1.0) / float2(1, _Thickness*0.16);
			float2 xy = (IN.uv_MainTex + _TrackSpeed * float2(0, 1)) * thickness;
			float s = round(sin(xy.y) * 0.5 + 0.5);

			o.Albedo = lerp(_Color1.rgb, _Color2.rgb, s);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
