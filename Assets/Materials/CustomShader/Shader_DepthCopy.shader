Shader "Custom/Shader_DepthCopy"
{
    SubShader
    {
        Pass
        {
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_TempDepthTexture);
            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_TempDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = GetFullScreenTriangleVertexPosition(v.vertexID);
                o.uv = GetFullScreenTriangleTexCoord(v.vertexID);
                return o;
            }

            float frag(Varyings i) : SV_Target
            {
                float transparentDepth = SAMPLE_TEXTURE2D(_TempDepthTexture, sampler_TempDepthTexture, i.uv);

                float cameraDepth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv);

                float depth = max(transparentDepth, cameraDepth);
                
                return depth;
            }

            ENDHLSL
        }
    }
}