Shader "UI/DropZone"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _Thickness ("Outline Thickness", Range(0.001,0.1)) = 0.02
        _Repeat ("Dash Repeat", Float) = 10
        _Speed ("Dash Speed", Float) = 1
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

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

            float4 _OutlineColor;
            float _Thickness;
            float _Repeat;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float distToEdge = min(min(uv.x, 1.0 - uv.x), min(uv.y, 1.0 - uv.y));
                float outlineMask = step(distToEdge, _Thickness);

                float dashPattern = frac((uv.x + uv.y) * _Repeat - _Time.y * _Speed);
                float dashMask = step(0.5, dashPattern);

                float finalMask = outlineMask * dashMask;

                return float4(_OutlineColor.rgb, _OutlineColor.a * finalMask);
            }
            ENDCG
        }
    }
}
