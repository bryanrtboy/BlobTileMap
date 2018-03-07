Shader "Hidden/BWShader"
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "black" {}
		_threshold ("Cuttoff", Range(0,1))= .5
	}
	SubShader
	{
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always

		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _threshold;

			half4 frag(v2f_img i) : COLOR {
				half4 c = tex2D(_MainTex, i.uv);
				
				float lum = c.r*.3 + c.g*.59 + c.b*.11;
				lum = max(0,lum > _threshold);
				half4 bw = half4( lum, lum, lum ,lum);

				//clip(bw.rgb - _threshold);
				return bw;
			}
			ENDCG
		}
	}
}
