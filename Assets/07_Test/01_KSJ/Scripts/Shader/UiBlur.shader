Shader "Custom/UiBlur"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 10)) = 1.0
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurSize;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = 0;
                float2 offset = _BlurSize / _ScreenParams.xy;

                // Sample the texture 9 times for a basic blur
                col += tex2D(_MainTex, i.uv + offset * float2(-1, -1)) * 0.111;
                col += tex2D(_MainTex, i.uv + offset * float2(0, -1)) * 0.111;
                col += tex2D(_MainTex, i.uv + offset * float2(1, -1)) * 0.111;
                col += tex2D(_MainTex, i.uv + offset * float2(-1, 0)) * 0.111;
                col += tex2D(_MainTex, i.uv) * 0.111;
                col += tex2D(_MainTex, i.uv + offset * float2(1, 0)) * 0.111;
                col += tex2D(_MainTex, i.uv + offset * float2(-1, 1)) * 0.111;
                col += tex2D(_MainTex, i.uv + offset * float2(0, 1)) * 0.111;
                col += tex2D(_MainTex, i.uv + offset * float2(1, 1)) * 0.111;

                col *= _Color;
                
                return col;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}