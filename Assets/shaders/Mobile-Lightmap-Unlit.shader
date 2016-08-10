// Unlit shader. Simplest possible textured shader.
// - SUPPORTS lightmap
// - no lighting
// - no per-material color

Shader "Mobile/Unlit ZTest (always on top)" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Tags {
			"RenderType"="Opaque"
			"Queue" = "Overlay" 
		 }
	LOD 100
	
	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		Lighting Off
		ZTest Always
		ZWrite On
		SetTexture [_MainTex] {
			constantColor (0,0,0,1)
			//combine texture, constant // UNITY_OPAQUE_ALPHA_FFP
		}  
	}
	
	
	

}
}



