using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRendererFeature : ScriptableRendererFeature
{
    class CustomDepthPass : ScriptableRenderPass
    {
        RTHandle depthTexture;
        Material depthOverrideMaterial;

        public void AllocateTexture(RenderTextureDescriptor desc)
        {
            if (depthTexture == null)
            {
                depthTexture = RTHandles.Alloc(desc, name: "_CustomDepthTexture");
            }
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            var desc = cameraTextureDescriptor;
            desc.colorFormat = RenderTextureFormat.RFloat;
            //desc.colorFormat = RenderTextureFormat.Depth;
            //desc.depthBufferBits = 32;
            //desc.msaaSamples = 1;

            ConfigureTarget(depthTexture.rt);
            ConfigureClear(ClearFlag.Color, Color.black);
            //ConfigureClear(ClearFlag.Depth, Color.black);
        }

        public void SetMaterial(Material mat)
        {
            depthOverrideMaterial = mat;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Custom Depth Pass");
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            var drawingSettings = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData, SortingCriteria.CommonOpaque);
            drawingSettings.overrideMaterial = depthOverrideMaterial;
            var filteringSettings = new FilteringSettings(RenderQueueRange.all);
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);

            cmd.SetGlobalTexture("_CustomDepthTexture", depthTexture);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField] Material depthMaterial;
    CustomDepthPass depthPass;

    public override void Create()
    {
        depthPass = new CustomDepthPass();
        depthPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        var desc = new RenderTextureDescriptor(Screen.width, Screen.height);
        desc.colorFormat = RenderTextureFormat.RFloat;
        desc.depthBufferBits = 0;
        depthPass.AllocateTexture(desc);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        depthPass.SetMaterial(depthMaterial);
        renderer.EnqueuePass(depthPass);
    }
}