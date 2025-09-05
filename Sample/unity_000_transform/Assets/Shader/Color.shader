Shader "Common/Color"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" { }
		_MainColor ("Main Color", Color) = (1, 1, 1, 0.8)
	}

	SubShader
	{
		Tags 
		{ 
			"RenderType" = "Transparent" 
			"Queue" = "Transparent+20" 
		}

		Cull Back 
	    ZWrite On
	    ZTest LEqual
	   	Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma enable_d3d11_debug_symbols

			#include "UnityCG.cginc"	
			
			half4 _MainColor;
			
			struct appdata
			{
				float4 vertex 	: POSITION;
				float2 uv 		: TEXCOORD0;
				float4 color	: COLOR;
			};

			struct v2f
			{
				float4 vertex 	: SV_POSITION;
				float2 uv 		: TEXCOORD0;
				float4 color	: TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= i.color;
				col *= _MainColor;
				col.a = _MainColor.a;

				return col;
			}
		ENDCG
		}
	}
}
