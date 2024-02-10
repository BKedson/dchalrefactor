Shader "Custom/CelShader"
{
	Properties
	{
		_SurfaceColor ("Surface Color", Color) = (1, 1, 1, 1)
		_Smoothness("Smoothness Coefficient", Range(0.0, 1.0)) = 1.0
		//_RimCoe("Rim Coefficient", Range(0.0, 1.0)) = 1.0

		_DiffuseThreshold("Diffuse Threshold",  Range(0.0, 1.0)) = 0.5
		_SpecularThreshold("Specular Threshold",  Range(0.0, 1.0)) = 0.5
		//_RimThreshold("Rim Threshold",  Range(0.0, 1.0)) = 0.5
		_ShadowThreshold("Shadow Threshold",  Range(0.0, 1.0)) = 0.5

		_CustomShadowBias("Shadow Bias",  Range(0.0, 0.5)) = 0.01

		_OutlineWidth("Outline Width", Range(0.0, 0.1)) = 0.01
		_OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }

			// Cel shader
			Pass
			{
				Cull Back ZWrite On

				Tags
				{
					"LightMode" = "UniversalForward"
				}

				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
				#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

				struct a2v
				{
					float4 vertexOS : POSITION;
					float3 normalOS : NORMAL;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertexCS : SV_POSITION;
					float2 uv : TEXCOORD0;
					float4 vertexWS : TEXCOORD2;
					float3 normalWS : TEXCOORD3;
					//SHADOW_COORDS(2)
				};

				float4 _SurfaceColor;
				float _Smoothness;
				//float _RimCoe;

				float _DiffuseThreshold;
				float _SpecularThreshold;
				//float _RimThreshold;
				float _ShadowThreshold;

				float _CustomShadowBias;

				v2f vert(a2v v)
				{
					v2f o;
					o.uv = v.uv;
					o.vertexWS = mul(UNITY_MATRIX_M, v.vertexOS);
					o.vertexCS = mul(UNITY_MATRIX_VP, o.vertexWS);
					o.normalWS = normalize(mul((float3x3)UNITY_MATRIX_I_M, v.normalOS));

					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					// Get light
					Light l = GetMainLight(TransformWorldToShadowCoord(i.vertexWS.xyz + i.normalWS * _CustomShadowBias));

				// Calculate shadow
				float shadow = step(_ShadowThreshold, saturate(l.shadowAttenuation));

				// Calculate diffuse
				float diffuse = saturate(dot(i.normalWS, l.direction));
				diffuse *= shadow;

				// Get view direction
				float3 viewDirWS = normalize(_WorldSpaceCameraPos.xyz - i.vertexWS.xyz);
				float3 h = normalize(l.direction + viewDirWS);
				// Calculate specular
				float specular = pow(saturate(dot(i.normalWS, h)), exp2(_Smoothness + 1));
				specular *= diffuse * _Smoothness;
				specular = step(_SpecularThreshold, specular);

				// Calculate rim light
				//float rim = 1 - dot(viewDirWS, i.normalWS);
				//rim *= pow(diffuse, _RimCoe);
				//rim = step(_RimThreshold, rim);

				diffuse = step(_DiffuseThreshold, diffuse);

				// Get ambient
				//float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				float3 ambient = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);

				float4 outColor = float4(l.color.rgb * diffuse + specular + ambient, 1) * _SurfaceColor;
				//outColor = float4(specular, specular, specular, 1);

				//return float4(shadow, shadow, shadow, 1);
				return outColor;
			}

			ENDHLSL
		}

			// Outline
			Pass
			{
				Name "OutlinePass"

				Cull Front

				HLSLPROGRAM
				#pragma vertex vert_outline
				#pragma fragment frag_outline

				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

				struct a2v
				{
					float4 vertexOS : POSITION;
					float3 normalOS : NORMAL;
				};

				struct v2f
				{
					float4 vertexCS : SV_POSITION;
				};

				float _OutlineWidth;
				float4 _OutlineColor;

				v2f vert_outline(a2v v)
				{
					v2f o;
					float3 normalCS = mul(UNITY_MATRIX_MVP, v.normalOS);
					o.vertexCS = mul(UNITY_MATRIX_MVP, v.vertexOS);
					o.vertexCS += float4(normalize(normalCS.xy) * _OutlineWidth, 0, 0);
					//o.vertexCS.xy += normalize(TransformWorldToHClipDir(v.normalOS)).xy * _OutlineWidth * o.vertexCS.w;
					//o.vertexCS += float4(normalize(normalCS).xy * _OutlineWidth * o.vertexCS.w, 0, 0);

					return o;
				}

				float4 frag_outline(v2f i) : SV_Target
				{
					return _OutlineColor;
				}
				ENDHLSL
			}

			//// Shadow
			//UsePass "VertexLit/SHADOWCASTER"
			Pass
			{
				Name "ShadowCaster"
				Tags{ "LightMode" = "ShadowCaster" }

				HLSLPROGRAM

				#pragma vertex vert_shadow
				#pragma fragment frag_shadow
				#pragma target 3.0

				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


				float4 vert_shadow(float4 vertex:POSITION, uint id : SV_VertexID, float3 normal : NORMAL) : SV_POSITION
				{
					vertex = mul(UNITY_MATRIX_MVP, vertex);
					normal = mul(UNITY_MATRIX_MVP, normal);
					//vertex -= float4(normal * _ShadowBias, 0);
					return vertex;
				}

				float4 frag_shadow(void) : COLOR
				{
					return 0;
				}
				ENDHLSL
			}
		}
}
