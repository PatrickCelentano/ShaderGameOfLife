Shader "Unlit/CubemapShader"
{
	Properties
	{
		size ("Size", Int) = 256
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

         	uniform StructuredBuffer<int> buffer;
			uniform int size;
			
			v2f vert (const appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (const v2f i) : SV_Target
			{
				const float u = i.uv.x;
				const float v = i.uv.y;
				
				const uint x = floor(u * size);
				const uint y = floor(v * size);

				const float dist_to_center = distance(
					float2(u * size - 0.5, v * size - 0.5),
					float2(x, y)
					) * 2.0;

				const float alive_value = buffer[x + size * y];
				const float4 color_at_point = 5.0 * float4(u, v, v - u, 1.0);
				
				return color_at_point * alive_value * saturate(1.0 - dist_to_center);
			}
			
			ENDCG
		}
	}
}