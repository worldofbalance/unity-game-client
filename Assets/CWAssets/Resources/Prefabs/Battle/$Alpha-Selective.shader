Shader "Transparent/Selective Color" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	
	_TransparentColor ("Trans Color", Color) = (1,1,1,1)
	
	_TransContrast ("Contrast", Range (.999, .001)) = 0.5
	
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha

sampler2D _MainTex;
fixed4 _Color;
fixed4 _TransparentColor;
fixed _TransContrast;


struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed3 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	
	fixed transparencyBasic = distance(c,_TransparentColor.rgb)/1.732; //normalized distance according to furthest distance in a cube (sqrt(3)).
	
	fixed transparencyContrasted = transparencyBasic/_TransContrast;
	
	o.Albedo = c;
	
	o.Alpha = transparencyContrasted;
}
ENDCG
}

Fallback "Transparent/VertexLit"
}
