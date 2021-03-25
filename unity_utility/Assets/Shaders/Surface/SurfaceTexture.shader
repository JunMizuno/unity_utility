Shader "Custom/SurfaceTexture"
{
    Properties
    {
        _MainTex ("MainTexture", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c * float4(1.0f, 1.0f, 1.0f, 1.0f);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
