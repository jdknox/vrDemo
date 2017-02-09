// Unlit shader. Simplest possible textured shader. Render where Stencil = 1
// - SUPPORTS lightmap
// - no lighting
// - no per-material color

Shader "Mobile/Stencil Keep Unlit" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "black" {}
	}

	SubShader {
		Stencil{
			Ref 1
			Comp equal
			Pass keep
			Fail keep
		}

		Tags{ "RenderType" = "Opaque" }
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



