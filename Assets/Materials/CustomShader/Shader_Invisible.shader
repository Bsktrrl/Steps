Shader "Custom/Invisible"
{
    SubShader
    {
        Tags {"RenderType"="Opaque"}

        Pass
        {
            ZWrite Off
            ColorMask 0
        }
    }
}