// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GR/ExtractDepth" {

	Properties{
		_gray("Gray Scale", Float) = 0
	}

		SubShader{
			Cull Off ZWrite Off ZTest Always
			Tags { "RenderType" = "Opaque" }

			Pass{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"


				float _gray;
				UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
				//sampler2D _CameraDepthTexture;
				//sampler2D _MainTex;
				half4 _CameraDepthTexture_ST;
				struct v2f {
				   float4 vertex : SV_POSITION;
				   float4 screenPos :TEXCOORD1;
				   float2 uv:TEXCOORD0;
				   float2 color:COLOR;
				};

				v2f vert(appdata_full v) {
				   v2f o;
				   o.vertex = UnityObjectToClipPos(v.vertex);

				   // compute depth
				   o.screenPos = ComputeScreenPos(o.vertex);
				   COMPUTE_EYEDEPTH(o.screenPos.z);
				   return o;
				}
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
				float4 frag(v2f i) : COLOR{
					  float d=Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.screenPos)));
					  return float4(d, d, d, 1);
				}
				ENDCG
			}
	}
		FallBack "Diffuse"
}