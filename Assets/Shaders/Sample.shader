Shader "Custom/Sample" {
	Properties {
		_EmissionColor ("EmissionColor", Color) = (1,1,1,1)
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Emission ("Emission", 2D) = "white"{}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _EmissionTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _EmissionColor;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float t = ((2 * _SinTime.w * _CosTime.w) + 1.0) * 0.5;
			float e = tex2D(_EmissionTex, IN.uv_MainTex).a * t;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = _EmissionColor * e;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
