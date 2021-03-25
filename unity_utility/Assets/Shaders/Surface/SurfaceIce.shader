Shader "Custom/SurfaceIce"
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
            float3 worldNormal;
            float3 viewDir; 
        };
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = fixed4(1, 1, 1, 1);
            float alpha = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
            // 1.5fは見た目調整の固定値だそうです。
            o.Alpha = alpha * 1.5f;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
