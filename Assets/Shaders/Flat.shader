Shader "Little Polygon/Flat" {
	Properties {
		_Color ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Main Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Geometry" }
		Lighting Off Fog { Mode Off }
		Pass {
		
CGPROGRAM
#pragma exclude_renderers ps3 xbox360 flash
#pragma fragmentoption ARB_precision_hint_fastest
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

uniform fixed4 _Color;
uniform sampler2D _MainTex;
uniform float4 _MainTex_ST;

struct FragmentInput {
	float4 pos : SV_POSITION;
	half2 uv : TEXCOORD0;
};

FragmentInput vert(
	float4 vertex : POSITION,
	float4 texcoord : TEXCOORD0
) {						
	FragmentInput o;
	o.pos = mul(UNITY_MATRIX_MVP, vertex);
	o.uv = TRANSFORM_TEX( texcoord, _MainTex );
	return o;
}

half4 frag(FragmentInput i) : COLOR {
	return _Color * tex2D( _MainTex, i.uv );
}

ENDCG

		} 	
	}
	FallBack "Diffuse"
}
