Shader "Custom/Shader_StencilWriter"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-10" } // Renders early

        Pass
        {
            //Stencil settings 
            Stencil
            {
                Ref 1           // The reference value written to the buffer
                Comp Always     // Always pass the stencil test
                Pass Replace    // Replace stencil value with Ref
            }

            //Disable normal rendering
            ColorMask 0       // Don’t write to color buffer
            ZWrite Off        // Don’t write to depth buffer
        }
    }
}
