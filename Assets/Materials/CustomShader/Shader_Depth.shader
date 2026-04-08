Shader "Custom/Shader_Depth"
{
    SubShader
    {
        Pass
        {
            ZWrite On
            ZTest LEqual
            ColorMask R
            Blend Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings 
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                return o;
            }

            float frag(Varyings i) : SV_Target
            {


                float depth = i.positionCS.z;
                
                return depth;
            }

            ENDHLSL
        }
    }
}