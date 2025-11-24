Shader "Custom/TMP_PixelOutline"
{
    Properties
    {
        _MainTex("Font Atlas", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineSize("Outline Size (px)", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
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

            struct appdata_t
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
            float4 _OutlineColor;
            float _OutlineSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pixel = _OutlineSize / _ScreenParams.xy;

                fixed4 center = tex2D(_MainTex, i.uv);
                center *= i.color; // preserve TMP vertex color

                // Check 4 neighbors for outline
                float alpha = tex2D(_MainTex, i.uv + float2(pixel.x, 0)).a;
                alpha = max(alpha, tex2D(_MainTex, i.uv + float2(-pixel.x, 0)).a);
                alpha = max(alpha, tex2D(_MainTex, i.uv + float2(0, pixel.y)).a);
                alpha = max(alpha, tex2D(_MainTex, i.uv + float2(0, -pixel.y)).a);

                if (center.a == 0 && alpha > 0)
                    return _OutlineColor;

                return center;
            }
            ENDCG
        }
    }
}