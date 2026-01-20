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

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.colorFormat = RenderTextureFormat.RFloat;
            desc.depthBufferBits = 32;

            RenderingUtils.ReAllocateIfNeeded(ref depthTexture, desc, name: "_CustomDepthTexture");

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
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

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

    [SerializeField] Material depthMaterial;
    CustomDepthPass depthPass;

    public override void Create()
    {
        depthPass = new CustomDepthPass();
        depthPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        depthPass.SetMaterial(depthMaterial);
        depthPass.SetVisualize(visualize);
        renderer.EnqueuePass(depthPass);
    }
}