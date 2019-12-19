Shader "Custom / Skill" {

	Properties {
		_MainTex("_MainTex", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}

	SubShader {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 100
		
		Cull Off
		Lighting Off
		ZWrite Off
		AlphaTest Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				sampler2D _MainTex;
				#include "UnityCG.cginc"

				fixed4 _Color;
				float4 frag(v2f_img i) : COLOR {
					float4 input_color = tex2D(_MainTex, i.uv);

					if (input_color.r * 255.0 == 212.0 && input_color.g * 255.0 == 192.0  && input_color.b * 255.0 == 212.0)
						input_color.a = 0;
					if (input_color.r == 0 && input_color.g == 0 && input_color.b == 0)
						input_color.a = 0;
					return input_color * _Color;
				}
			ENDCG
		}
	}

	FallBack "Unlit"
}