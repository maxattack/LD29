Shader "Little Polygon/HUD Text" { 
	Properties { 
	   _MainTex ("Font Texture", 2D) = "white" {} 
	} 
	
	SubShader { 
	   Tags { "Queue"="Transparent+64" "IgnoreProjector"="True" "RenderType"="Transparent" } 
	   Lighting Off Cull Off ZWrite Off Fog { Mode Off }
	   Blend SrcAlpha OneMinusSrcAlpha 
	   Pass { 
	   	BindChannels {
	   		Bind "Vertex", vertex
	   		Bind "texcoord", texcoord
	   		Bind "Color", color
	   	}
	      SetTexture [_MainTex] { 
	         combine primary, texture * primary 
	      } 
	   } 
	} 
}