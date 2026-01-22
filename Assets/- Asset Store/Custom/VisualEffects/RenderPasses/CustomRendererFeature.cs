using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRendererFeature : ScriptableRendererFeature
{
    [SerializeField] bool visualize;

    class CustomDepthPass : ScriptableRenderPass
    {
        RTHandle depthTexture;
        Material depthOverrideMaterial;

        int cachedWidth = -1;
        int cachedHeight = -1;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var baseDesc = renderingData.cameraData.cameraTargetDescriptor;

            int width = baseDesc.width;
            int height = baseDesc.height;

            if (depthTexture == null || width != cachedWidth || height != cachedHeight)
            {
                cachedWidth = width;
                cachedHeight = height;

                var desc = baseDesc;
                desc.colorFormat = RenderTextureFormat.RFloat;
                desc.depthBufferBits = 32;
                desc.msaaSamples = 1;
                desc.useMipMap = false;

                RenderingUtils.ReAllocateIfNeeded(ref depthTexture, desc, name: "_CustomDepthTexture");
            }

            ConfigureTarget(depthTexture);
            ConfigureClear(ClearFlag.Depth | ClearFlag.Color, Color.black);
        }

        public void SetMaterial(Material mat)
        {
            depthOverrideMaterial = mat;
        }
        
        bool visualize;
        public void SetVisualize(bool value)
        {
            visualize = value;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Custom Depth Pass");

            var transparentFiltering = new FilteringSettings(RenderQueueRange.all);
            var transparentDrawing = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData, SortingCriteria.CommonTransparent);
            transparentDrawing.overrideMaterial = depthOverrideMaterial;
            context.DrawRenderers(renderingData.cullResults, ref transparentDrawing, ref transparentFiltering);

            cmd.SetGlobalTexture("_CustomDepthTexture", depthTexture.rt);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            if (visualize)
            {
                cmd.Blit(depthTexture.rt, renderingData.cameraData.renderer.cameraColorTargetHandle);
            }
        }
    }

    class CustomColorPass : ScriptableRenderPass
    {
        RTHandle customColorTexture;
        float downscaleFactor;
        static readonly int CameraColorID = Shader.PropertyToID("_CustomColorTexture");

        public void SetDownscaleFactor(float factor)
        {
            downscaleFactor = factor;
        }

        int cachedWidth = -1;
        int cachedHeight = -1;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var baseDesc = renderingData.cameraData.cameraTargetDescriptor;

            int width = Mathf.Max(1, (int)(baseDesc.width * downscaleFactor));
            int height = Mathf.Max(1, (int)(baseDesc.height * downscaleFactor));

            if (customColorTexture == null || width != cachedWidth || height != cachedHeight)
            {
                cachedWidth = width;
                cachedHeight = height;

                var desc = new RenderTextureDescriptor(width, height, RenderTextureFormat.DefaultHDR);
                desc.depthBufferBits = 0;
                desc.msaaSamples = 1;
                desc.useMipMap = false;
                desc.autoGenerateMips = false;

                RenderingUtils.ReAllocateIfNeeded(ref customColorTexture, desc, name: "_CustomColorTexture");
                customColorTexture.rt.wrapMode = TextureWrapMode.Clamp;
                customColorTexture.rt.filterMode = FilterMode.Bilinear;
            }
        }
    
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Copy Camera Color");
    
            cmd.Blit(renderingData.cameraData.renderer.cameraColorTargetHandle, customColorTexture.rt);

            cmd.SetGlobalTexture(CameraColorID, customColorTexture.rt);
    
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField] Material depthMaterial;
    [SerializeField] float downscaleFactor;
    CustomDepthPass depthPass;
    CustomColorPass colorPass;

    public override void Create()
    {
        depthPass = new CustomDepthPass();
        depthPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        colorPass = new CustomColorPass();
        colorPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        depthPass.SetMaterial(depthMaterial);
        depthPass.SetVisualize(visualize);
        renderer.EnqueuePass(depthPass);

        colorPass.SetDownscaleFactor(downscaleFactor);
        renderer.EnqueuePass(colorPass);
    }
}