Shader "UI/GlowRayPulse"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1, 0.9, 0.5, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 1
        _Radius ("Glow Radius", Range(0, 1)) = 0.5
        _EdgeWidth ("Edge Width", Range(0.01, 0.5)) = 0.15
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2
        _RayCount ("Ray Count", Range(2, 32)) = 8
        _RayStrength ("Ray Strength", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GlowColor;
            float _GlowIntensity;
            float _Radius;
            float _EdgeWidth;
            float _PulseSpeed;
            float _RayCount;
            float _RayStrength;

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
                // Màu gốc
                fixed4 mainCol = tex2D(_MainTex, i.uv) * i.color;
                float alphaMask = mainCol.a;

                float2 center = float2(0.5, 0.5);
                float2 uv = i.uv - center;
                float dist = length(uv) * 2.0;

                // Góc dùng để tạo tia
                float angle = atan2(uv.y, uv.x);

                // Sóng tia quay quanh sprite
                float ray = sin(angle * _RayCount + _Time.y * _PulseSpeed);
                ray = pow(abs(ray), 4.0) * _RayStrength;

                // Hiệu ứng nhịp sáng lan theo bán kính
                float pulse = sin(_Time.y * _PulseSpeed * 0.5) * 0.05 + 0.05;
                float radius = saturate(_Radius + pulse);

                // Biên sáng lan mềm
                float edge = smoothstep(radius, radius - _EdgeWidth, dist);

                // Tổng hợp glow (ánh sáng nền + tia)
                float glowMask = saturate(edge + ray);

                // Áp màu phát sáng đã fix (không bị dính màu sprite)
                fixed4 glow = _GlowColor * glowMask * _GlowIntensity;
                glow.rgb *= saturate((1 - alphaMask) + 0.5);

                fixed4 finalColor = mainCol + glow;
                finalColor.a = saturate(mainCol.a + glow.a * 0.5);

                return finalColor;
            }
            ENDCG
        }
    }
}
