Shader "Custom/SurfaceRimlighting"
{
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0
        
        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 viewDir; 
        };
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 baseColor = fixed4(1.0f, 0.0f, 0.0f, 1.0f);
            fixed4 rimColor = fixed4(0.9f, 0.9f, 0.9f, 1.0f);

            o.Albedo = baseColor;
            o.Alpha = 1.0f;
            float rim = 1.0f - saturate(dot(IN.viewDir, o.Normal));
            o.Emission = rimColor * pow(rim, 1.0f);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
