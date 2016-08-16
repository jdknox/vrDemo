// Unlit shader. Simplest possible textured shader.
// - SUPPORTS lightmap
// - no lighting
// - no per-material color

Shader "Mobile/Unlit ZTest (always on top)" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "black" {}
	}

	SubShader {
		
		Tags {
				"IgnoreProjector" = "True"
				"RenderType" = "Opaque"
				"Queue" = "Geometry+1" 

				//"Queue" = "Transparent+1"
				//"IgnoreProjector" = "True"
				//"RenderType" = "Transparent"
			 }
		
		Lighting Off
		ZTest Always
		ZWrite On
		//Cull Off
		//Blend SrcAlpha OneMinusSrcAlpha
		//BlendOp Add
		//Blend One One

		LOD 100
	
		// Non-lightmapped
		Pass {
			Tags {
					"LightMode" = "Vertex"
				 }
			SetTexture [_MainTex] {
				constantColor (1, 1, 1, 1)
				combine texture, constant // UNITY_OPAQUE_ALPHA_FFP
			}  
		}
	}
}



