Shader "Custom/Cube" {

	Properties {
		_BumpAmt ("Distortion", Range(0, 128)) = 10
		_OutlineDistance ("Outline Distance", Range(0, 0.1)) = 0
		_OutlineSize ("Outline Size", Range(0, 2)) = 0.9
		_ScrollX("Scroll X", Range(0.0, 1.0)) = 0.5
		_MainColor("Main Color", Color) = (0, 0, 0)
		_OutlineColor("Outline Color", Color) = (0, 0, 0)
		_MainTex("Base", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
    }

    SubShader {
        Tags {
			"Queue"="Transparent"
            "RenderType"="Opaque"
        }
		GrabPass {
			Name "Scroll"
			Tags { "LightMode" = "Always" }
		}
		Pass {
            Name "Outline"
            
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0

			uniform float3 _OutlineColor;
			uniform float _OutlineDistance;
			uniform float _OutlineSize;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal * _OutlineDistance, _OutlineSize));
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_OutlineColor, 0);
            }
            ENDCG
		}
		Pass
		{
			Name "Scroll"
			Tags { "LightMode"="Always" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD0;
				float2 uvbump : TEXCOORD1;
				float2 uvmain : TEXCOORD2;
				UNITY_FOG_COORDS(3)
			};

			float _BumpAmt;
			sampler2D _GrabTexture;
			sampler2D _MainTex;
			sampler2D _BumpMap;
			float3 _MainColor;
			float4 _MainTex_ST;
			float4 _BumpMap_ST;
			float4 _GrabTexture_TexelSize;
			float _ScrollX;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
				#else
					float scale = 1.0;
				#endif
				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;
				o.uvbump = TRANSFORM_TEX(v.uv, _BumpMap);
				o.uvmain = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 scroll = float2(_ScrollX, 0) * _Time.y;
				half2 bump = UnpackNormal(tex2D(_BumpMap, i.uvbump + scroll)).rg;
				float2 offset = bump * _BumpAmt * _GrabTexture_TexelSize.xy;
				#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE
					i.uvgrab.xy = offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(i.uvgrab.z) + i.uvgrab.xy;
				#else
					i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
				#endif
				half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				fixed4 tint = tex2D(_MainTex, i.uvmain + scroll);
				tint.rgb += _MainColor;
				tint.rgb /= 2;
				col *= tint;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
    }

	SubShader {
		Blend DstColor Zero
		Pass {
			Name "Scroll"
			SetTexture [_MainTex] {	combine texture }
		}
	}
}
