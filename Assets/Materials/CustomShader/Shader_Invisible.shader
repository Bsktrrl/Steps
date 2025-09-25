Shader "Custom/Invisible"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }

        Pass
        {
            ZWrite Off       // Don't write to depth
            ColorMask 0      // Don't write to color
            Blend Off        // No blending
        }
    }
}