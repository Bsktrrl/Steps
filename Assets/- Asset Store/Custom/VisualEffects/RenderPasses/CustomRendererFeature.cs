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
        Material depthCopyMaterial;

        int cachedWidth = -1;
        int cachedHeight = -1;

        bool visualize;

        public void SetVisualize(bool value)
        {
            visualize = value;
        }
        public void SetMaterial(Material mat)
        {
            depthOverrideMaterial = mat;
        }
        public void SetCopyMaterial(Material mat)
        {
            depthCopyMaterial = mat;
        }

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
                desc.depthBufferBits = 0;
                desc.msaaSamples = 1;
                desc.useMipMap = false;

                RenderingUtils.ReAllocateIfNeeded(ref depthTexture, desc, name: "_CustomDepthTexture");
            }

            ConfigureTarget(depthTexture);
            ConfigureClear(ClearFlag.Color, Color.black);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Custom Depth Pass");

            //Input camera depth texture into conversion material
            Texture cameraDepthTexture = Shader.GetGlobalTexture("_CameraDepthTexture");
            if (cameraDepthTexture != null)
            {
                depthCopyMaterial.SetTexture("_CameraDepthTexture", cameraDepthTexture);
            }

            //Input conversion material into render texture
            Blitter.BlitCameraTexture(cmd, renderingData.cameraData.renderer.cameraDepthTargetHandle, depthTexture, depthCopyMaterial, 0);

            //Draw transparents
            var transparentFiltering = new FilteringSettings(RenderQueueRange.transparent);
            var transparentDrawing = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData, SortingCriteria.CommonTransparent);
            transparentDrawing.overrideMaterial = depthOverrideMaterial;
            context.DrawRenderers(renderingData.cullResults, ref transparentDrawing, ref transparentFiltering);

            //Expose render texture globally
            cmd.SetGlobalTexture("_CustomDepthTexture", depthTexture.rt);

            //Debug visualization
            if (visualize)
            {
                cmd.Blit(depthTexture.rt, renderingData.cameraData.renderer.cameraColorTargetHandle);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
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

        int cachedFullWidth = -1;
        int cachedFullHeight = -1;
        int cachedLowResWidth = -1;
        int cachedLowResHeight = -1;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var baseDesc = renderingData.cameraData.cameraTargetDescriptor;

            int fullWidth = baseDesc.width;
            int fullHeight = baseDesc.height;

            int lowResWidth = Mathf.Max(1, (int)(fullWidth * downscaleFactor));
            int lowResHeight = Mathf.Max(1, (int)(fullHeight * downscaleFactor));

            if (customColorTexture == null || lowResColorTexture == null || fullWidth != cachedFullWidth || fullHeight != cachedFullHeight || lowResWidth != cachedLowResWidth || lowResHeight != cachedLowResHeight)
            {
                cachedFullWidth = fullWidth;
                cachedFullHeight = fullHeight;
                cachedLowResWidth = lowResWidth;
                cachedLowResHeight = lowResHeight;

                var fullDesc = new RenderTextureDescriptor(fullWidth, fullHeight, RenderTextureFormat.DefaultHDR);
                fullDesc.depthBufferBits = 0;
                fullDesc.msaaSamples = 1;
                RenderingUtils.ReAllocateIfNeeded(ref customColorTexture, fullDesc, name: "_CustomColorTexture");

                var downDesc = new RenderTextureDescriptor(lowResWidth, lowResHeight, RenderTextureFormat.DefaultHDR);
                downDesc.depthBufferBits = 0;
                downDesc.msaaSamples = 1;
                RenderingUtils.ReAllocateIfNeeded(ref lowResColorTexture, downDesc, name: "_LowResColorTexture");
                lowResColorTexture.rt.filterMode = FilterMode.Bilinear;
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            bool isGameCamera = renderingData.cameraData.cameraType == CameraType.Game;
            Shader.SetGlobalFloat("_IsGameCamera", isGameCamera ? 1f : 0f);

            CommandBuffer cmd = CommandBufferPool.Get("Copy Camera Color");

            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            cmd.Blit(source, customColorTexture);
            cmd.Blit(customColorTexture, lowResColorTexture);

            cmd.SetGlobalTexture("_CustomColorTexture", customColorTexture.rt);
            cmd.SetGlobalTexture("_LowResColorTexture", lowResColorTexture.rt);
    
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField] Material depthMaterial;
    [SerializeField] Material depthCopyMaterial;
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
        depthPass.SetCopyMaterial(depthCopyMaterial);
        depthPass.SetVisualize(visualize);
        renderer.EnqueuePass(depthPass);

        colorPass.SetDownscaleFactor(downscaleFactor);
        renderer.EnqueuePass(colorPass);
    }
}