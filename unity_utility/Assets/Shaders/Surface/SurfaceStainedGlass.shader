Shader "Custom/SurfaceStainedGlass"
{
    Properties
    {
        _MainTex ("MainTexture", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
        }

        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            // グレースケールを利用してステンドグラスの透過を判断
            o.Alpha = (c.r * 0.3f + c.g * 0.6f + c.b * 0.1f < 0.2f) ? 1.0f : 0.7f;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
