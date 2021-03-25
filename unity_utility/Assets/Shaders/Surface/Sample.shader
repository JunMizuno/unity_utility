Shader "Custom/Sample"
{
    /*
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 0, 0, 1)
    }
    */
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags 
        { 
          "Queue"="Transparent"
          "RendarType"="Transparent"
        }
        LOD 200

        CGPROGRAM
        //#pragma surface surf Standard fullforwardshadows
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0
        
        struct Input
        {
            float2 uv_MainTex;
        };
        
        fixed4 _BaseColor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _BaseColor;
            o.Alpha = _BaseColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
