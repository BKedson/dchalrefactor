Shader "Custom/BackfaceShader"
{
	Properties
	{
		_SurfaceColor("Surface Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
			Cull Front ZWrite On

			Tags
			{
				"LightMode" = "UniversalForward"
			}

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			struct a2v
			{
				float4 vertexOS : POSITION;
			};

			struct v2f
			{
				float4 vertexCS : SV_POSITION;
			};

			float4 _SurfaceColor;

			v2f vert(a2v v)
			{
				v2f o;
				o.vertexCS = mul(UNITY_MATRIX_MVP, v.vertexOS);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				return _SurfaceColor;
			}
			ENDHLSL
		}
	}
}
