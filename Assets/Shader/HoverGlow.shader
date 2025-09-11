Shader "Custom/HoverGlowSmooth"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineSize ("Outline Size", Range(0,10)) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

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
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv) * _Color;

                // sample nhiều hướng (16 hướng quanh pixel)
                float alpha = 0.0;
                const int samples = 16;
                for (int k = 0; k < samples; k++)
                {
                    float angle = (6.2831853 / samples) * k; // 2*PI / samples
                    float2 dir = float2(cos(angle), sin(angle));
                    float2 offset = dir * _MainTex_TexelSize.xy * _OutlineSize;
                    alpha = max(alpha, tex2D(_MainTex, i.uv + offset).a);
                }

                fixed4 outline = _OutlineColor;
                outline.a *= alpha;

                // Nếu pixel gốc có alpha thì dùng màu gốc, nếu không thì hiển outline
                return (c.a > 0.0) ? c : outline;
            }
            ENDCG
        }
    }
}
