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
				   float4 pos : SV_POSITION;
				   float4 scrPos:TEXCOORD1;
				   float2 uv:TEXCOORD0;
				   float2 color:COLOR;
				   UNITY_VERTEX_INPUT_INSTANCE_ID
				   UNITY_VERTEX_OUTPUT_STEREO
				};

				v2f vert(appdata_full v) {
				   v2f o;
				   UNITY_SETUP_INSTANCE_ID(v);
				   UNITY_INITIALIZE_OUTPUT(v2f, o);
				   //UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(v);
				   TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
				   o.pos = UnityObjectToClipPos(v.vertex);
				//   o.uv = v.uv;
				   o.scrPos = ComputeScreenPos(o.pos); //UnityStereoScreenSpaceUVAdjust(ComputeScreenPos(o.pos), _CameraDepthTexture_ST); 
				   return o;
				}
				UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
				half4 frag(v2f i) : COLOR{
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
					//#if UNITY_SINGLE_PASS_STEREO
					//	i.uvgrab.xy = TransformStereoScreenSpaceTex(i.uvgrab.xy, i.uvgrab.w);
					//#endif
					// the value is between [0, 1] = [near plane, far plane]
					float d = Linear01Depth(UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthTexture, i.scrPos)); ;//Linear01Depth(tex2Dproj(_CameraDepthTexture, i.scrPos).r);
					return half4(d, d, d, 1);
				}
				ENDCG
			}
	}
		FallBack "Diffuse"
}