Shader "Mati36/FX/BezierAnimation"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_FirstPoint("First Point", Vector) = (0,0,0,0)
		_SecondPoint("Second Point", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _FirstPoint;
			float4 _SecondPoint;

			float3 Bezier(float t)
			{
				float3 middlePoint = lerp(float3(0,0,0), _SecondPoint.xyz, 0.5);
				middlePoint.y += 5;
				float3 xy = lerp(float3(0, 0, 0), middlePoint.xyz, t);
				float3 yz = lerp(middlePoint.xyz, _SecondPoint.xyz, t);
				return lerp(xy, yz, t);
			}

			static const float timeScale = 0.4;

            v2f vert (appdata v)
            {
                v2f o;
				float3 worldPos;
				float loopedScaledTime = frac(_Time.y * timeScale);
				worldPos = mul(unity_ObjectToWorld, v.vertex.xyz * saturate(1 - (v.vertex.z + loopedScaledTime * 2))).xyz;
				worldPos += Bezier(saturate(loopedScaledTime + (v.vertex.z + loopedScaledTime * 2) - 0.5));
				v.vertex.xyz = mul(unity_WorldToObject, worldPos).xyz;
				//v.vertex.xyz += Bezier(saturate(frac(_Time.y * timeScale) + (v.vertex.z + frac(_Time.y * timeScale)*2) -0.5));
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.vertex.xyz += Bezier(frac(_Time.y));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

			
            ENDCG
        }
    }
}
