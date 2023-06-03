Shader "Custom/FogShader"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _FogColor("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
        _MaxRenderDistance("Max Render Distance", Float) = 4000
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Lambert alpha

            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
                float3 worldPos;
            };

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
            uniform float4 _FogColor;
            uniform float _MaxRenderDistance;

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;

                float dist = distance(IN.worldPos, _WorldSpaceCameraPos) * 0.4;
                float fogAmount = saturate(dist / _MaxRenderDistance);

                // Adjust fog reduction rate
                float fogReduction = fogAmount * fogAmount;

                // Adjust transparency rate and apply transparency condition
                if (fogAmount > 0.01)
                    o.Alpha = 1 - ((fogReduction * 0.5) * fogAmount);
                else
                    o.Alpha = 1;

                o.Albedo.rgb = lerp(o.Albedo.rgb, _FogColor.rgb, fogReduction);
            }
            ENDCG
        }
            FallBack "Diffuse"
}
