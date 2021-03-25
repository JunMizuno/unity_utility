Shader "Unlit/UnlitForwardBaseShadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "LightMode"="ForwardBase" }

        Pass
        {
            CGPROGRAM
    
            ENDCG
        }
    }
}
