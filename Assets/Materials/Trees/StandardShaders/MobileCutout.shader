// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mati36/Mobile/Cutout" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_CutoutThreshold("Cutout Threshold", Range(0,1)) = 0.5
	}
		SubShader{
		Tags{ "RenderType" = "TransparentCutout" "Queue" = "Transparent" }
		LOD 150

		CGPROGRAM
#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;
		fixed _CutoutThreshold;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;
		clip(c.a - _CutoutThreshold);
		o.Alpha = c.a;
	}
	ENDCG
	}

		Dependency "BillboardShader" = "Hidden/Mati36/LeavesBillboard"
		Fallback "Mobile/VertexLit"
}
