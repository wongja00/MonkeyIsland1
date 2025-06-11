Shader "Unlit/FontOutline"
{
     Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 0.1)) = 0.05
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
        LOD 100

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
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 original = tex2D(_MainTex, i.uv);
                fixed4 outline = _OutlineColor;

                // Sample the neighboring pixels for the outline effect
                float2 offset = float2(_OutlineThickness, 0);
                outline.a = max(outline.a, tex2D(_MainTex, i.uv + offset).a);
                outline.a = max(outline.a, tex2D(_MainTex, i.uv - offset).a);
                offset = float2(0, _OutlineThickness);
                outline.a = max(outline.a, tex2D(_MainTex, i.uv + offset).a);
                outline.a = max(outline.a, tex2D(_MainTex, i.uv - offset).a);

                // Combine the outline and original
                return outline * (1.0 - original.a) + original;
            }
            ENDCG
        }
    }
    FallBack "GUI/Text Shader"
}
