Shader "Mati36/FX/DissolveAnimation"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_DissolveTexture("DissolveTexture", 2D) = "white"{}
	}
		SubShader
	{
		Tags { "RenderType" = "Transparent"  "Queue" = "Transparent"}
		LOD 200

		ZWrite Off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DissolveTexture;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		uniform float _AnimTime;

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float loopedTime = frac(_AnimTime);
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1 - saturate((IN.worldPos.y + (loopedTime * 4) - 5) * 1);
		}
		ENDCG
	}
		FallBack "Diffuse"
}
