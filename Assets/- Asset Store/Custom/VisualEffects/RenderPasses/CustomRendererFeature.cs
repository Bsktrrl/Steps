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
        RTHandle lowResColorTexture;
        float downscaleFactor;

        public void SetDownscaleFactor(float factor)
        {
            downscaleFactor = factor;
        }

        int cachedWidth = -1;
        int cachedHeight = -1;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var baseDesc = renderingData.cameraData.cameraTargetDescriptor;

            int fullWidth = baseDesc.width;
            int fullHeight = baseDesc.height;

            int lowResWidth = Mathf.Max(1, (int)(fullWidth * downscaleFactor));
            int lowResHeight = Mathf.Max(1, (int)(fullHeight * downscaleFactor));

            var fullDesc = new RenderTextureDescriptor(fullWidth, fullHeight, RenderTextureFormat.DefaultHDR);
            fullDesc.depthBufferBits = 0;
            fullDesc.msaaSamples = 1;
            RenderingUtils.ReAllocateIfNeeded(ref customColorTexture, fullDesc, name: "_CustomColorTexture");

            var downDesc = new RenderTextureDescriptor(lowResWidth, lowResHeight, RenderTextureFormat.DefaultHDR);
            downDesc.depthBufferBits = 0;
            downDesc.msaaSamples = 1;
            RenderingUtils.ReAllocateIfNeeded(ref lowResColorTexture, downDesc, name: "_LowResColorTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Copy Camera Color");

            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            cmd.Blit(source, customColorTexture);
            cmd.Blit(source, lowResColorTexture);

            cmd.SetGlobalTexture("_CustomColorTexture", customColorTexture.rt);
            cmd.SetGlobalTexture("_LowResColorTexture", lowResColorTexture.rt);
    
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