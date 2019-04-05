Shader "Mati36/VR/DiffuseOutline"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(0, 3)) = 1.0
	}
		SubShader{
			Tags{ "RenderType" = "TransparentCutout" "Queue" = "Transparent" }
			LOD 150
			
			Pass
			{
			    Cull Off
				ZTest Always
				ZWrite Off
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				fixed4 _OutlineColor;
				float _OutlineWidth;

				v2f vert (appdata v)
				{
					v2f o;
					v.vertex.xyz += v.normal * _OutlineWidth;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					
					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 col = _OutlineColor;
					col.a *= 1-step( _OutlineWidth,0);
					clip(col.a - 0.5);
					return col;
				}
				ENDCG
			}

			CGPROGRAM
			#pragma surface surf Lambert noforwardadd

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
			ENDCG
		}	
}
