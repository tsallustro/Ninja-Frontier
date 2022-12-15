Shader "Unlit/BurnableObject"
{
	Properties
	{
		_maxDissolve("Max Amount The Paper Will Disappear (0 to 1)", Float) = 0.6
		_DissolveDuration ("Dissolve Duration In Seconds", Float) = 20.0
		_MainTex ("Texture", 2D) = "white" {}
		_BurnTex("Burn Texture", 2D) = "white" {}
		_BurnMask("Burn Mask", 2D) = "white" {}
		_FireTex("Fire Texture", 2D) = "white" {}
		_FireCol("Fire Color", Color) = (0,0,0,0)
		_MaskThreshold("Mask Threshold", Range(0,1)) = 0
		_BurnSize("Burn Size", Range(0,1)) = 0
		_Intensity("Intensity", Range(0, 3)) = 1
		[MaterialToggle] _BurnTexActive("Activate Burn Texture", Float) = 1
		_Color ("Color (RGBA)", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull front
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float _maxDissolve;
			float _DissolveDuration;
			sampler2D _MainTex;
			sampler2D _BurnTex;
			sampler2D _BurnMask;
			sampler2D _FireTex;
			float4 _FireCol;
			float _MaskThreshold;
			float _BurnSize;
			float _Intensity;
			float _BurnTexActive;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 frag(v2f i) : SV_Target
			{
				_MaskThreshold = (_Time.y / _DissolveDuration <= _maxDissolve) ? _Time.y / _DissolveDuration : _maxDissolve;
				// sample the texture
				float4 mainCol = tex2D(_MainTex, i.uv);
				float maskCol = tex2D(_BurnMask, i.uv).r;
				float burnThreshold = _MaskThreshold + _BurnSize;

				if (maskCol <= _MaskThreshold)
				{
					if(_BurnTexActive == 1)
						mainCol = tex2D(_BurnTex, i.uv) * _Intensity;
					else
						mainCol.a = 0;
				}
					
				
				if (_MaskThreshold > 0 && maskCol > _MaskThreshold && maskCol <= burnThreshold)
					mainCol = tex2D(_FireTex, float2((maskCol - _MaskThreshold)/ _BurnSize,0));

				return mainCol;
			}
			ENDCG
		}
	}
}
