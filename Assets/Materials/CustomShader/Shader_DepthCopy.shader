Shader "Custom/Shader_DepthCopy"
{
    SubShader
    {
        Tags 
        {
            "RenderType"="Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "LightMode" = "UniversalForward"
        }

        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                return o;
            }

            float frag(Varyings i) : SV_Target
            {
                float rawDepth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv);

                float linearDepth = LinearEyeDepth(rawDepth);

                return linearDepth;
            }

            ENDHLSL
        }
    }
}