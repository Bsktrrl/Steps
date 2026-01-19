Shader "Custom/Invisible"
{
    SubShader
    {
        Tags {"RenderType"="Opaque"}

        Pass
        {
            ZWrite On
            ZTest LEqual
            ColorMask 0
        }
    }
}