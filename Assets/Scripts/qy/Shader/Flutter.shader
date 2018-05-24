Shader "Custom/Flutter"
{
	Properties
    {

        _MainTex ("Texture", 2D) = "white" {}
        [NoScaleOffset]
        _Mask("Mask", 2D) = "white" {}
        _Speed("Speed", Range(0,10)) = 1
        _Parameters("Parameters", vector) = (1,1,1,1)
        [KeywordEnum(Roll,Wave)] _Type("Type",float) = 0

    }
    SubShader
    {
        Tags { "Queue"="Transparent"  }
        LOD 100

        Pass
        {
			//ZTest Always
			//ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _TYPE_ROLL _TYPE_WAVE
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #define pi 3.14159265358979 
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Mask;
            float4 _Mask_ST;
            float4 _Parameters;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                fixed3 mask = tex2D(_Mask,i.uv).rgb;

				
                float2 phase = (i.uv+_Speed*float2(_Time.y,_Time.y));//原UV作为初相，为了便于描述数据和效果，再乘上2π 
                float2 offset;
                //使用不同参数控制不同方向上的振幅、频率。
                #if _TYPE_ROLL
                    offset.x = _Parameters.x*sin(_Parameters.y*phase.x);
                    offset.y = _Parameters.z*sin(_Parameters.w*phase.y);
                #elif _TYPE_WAVE
                    offset.y = _Parameters.x*sin(_Parameters.y*phase.x);
                    offset.x = _Parameters.z*sin(_Parameters.w*phase.y);
                #endif

				fixed4 col = tex2D(_MainTex, i.uv+0.001*offset*mask.b);

				//phase = i.uv+_Speed*float2(_Time.y,_Time.y);

				//offset.y = _Parameters.x * sin(phase.x * _Parameters.y);
				//offset.x = _Parameters.z * sin(phase.y * _Parameters.w);
				//col = tex2D(_MainTex,i.uv + offset * 0.001 * mask.b);
               

                return col;
            }
            ENDCG
        }
    }
}
