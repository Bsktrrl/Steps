using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRendererFeature : ScriptableRendererFeature
{
    [SerializeField] bool visualize;

    static readonly int CustomDepthTexture = Shader.PropertyToID("_CustomDepthTexture");
    static readonly int CustomColorTexture = Shader.PropertyToID("_CustomColorTexture");
    static readonly int IsMainCamera = Shader.PropertyToID("_IsMainCamera");

    class CustomDepthPass : ScriptableRenderPass
    {
        RTHandle depthTexture;
        RTHandle tempDepthTexture;
        RTHandle tempDepthBuffer;
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

            if (depthTexture == null || baseDesc.width != cachedWidth || baseDesc.height != cachedHeight)
            {
                cachedWidth = baseDesc.width;
                cachedHeight = baseDesc.height;

                var desc = baseDesc;
                desc.colorFormat = RenderTextureFormat.RFloat;
                desc.depthBufferBits = 0;
                desc.msaaSamples = 1;
                desc.useMipMap = false;

                var depthDesc = baseDesc;
                depthDesc.colorFormat = RenderTextureFormat.Depth;
                depthDesc.depthBufferBits = 32;
                depthDesc.msaaSamples = 1;

                RenderingUtils.ReAllocateIfNeeded(ref depthTexture, desc, name: "_CustomDepthTexture");
                RenderingUtils.ReAllocateIfNeeded(ref tempDepthTexture, desc, name: "_TempDepthTexture");
                RenderingUtils.ReAllocateIfNeeded(ref tempDepthBuffer, depthDesc, name: "_TempDepthBuffer");
            }

            ConfigureTarget(tempDepthTexture, tempDepthBuffer);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Custom Depth Pass");

            cmd.SetRenderTarget(tempDepthTexture.nameID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, tempDepthBuffer.nameID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            cmd.ClearRenderTarget(true, true, Color.black);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            // The override writes only the nearest depth. Front-to-back sorting
            // lets nearer surfaces reject fragments from surfaces farther away.
            var transparentFiltering = new FilteringSettings(RenderQueueRange.transparent);
            const SortingCriteria sorting = SortingCriteria.QuantizedFrontToBack | SortingCriteria.OptimizeStateChanges;
            var transparentDrawing = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData, sorting);
            transparentDrawing.overrideMaterial = depthOverrideMaterial;

            var stateBlock = new RenderStateBlock(RenderStateMask.Depth)
            {
                depthState = new DepthState(writeEnabled: true, compareFunction: CompareFunction.LessEqual)
            };

            context.DrawRenderers(renderingData.cullResults, ref transparentDrawing, ref transparentFiltering, ref stateBlock);

            //Copy camera depth into render texture
            depthCopyMaterial.SetTexture("_TempDepthTexture", tempDepthTexture.rt);
            Blitter.BlitCameraTexture(cmd, renderingData.cameraData.renderer.cameraDepthTargetHandle, depthTexture, depthCopyMaterial, 0);

            //Expose render texture globally
            cmd.SetGlobalTexture(CustomDepthTexture, depthTexture.rt);

            //Debug visualization
            if (visualize)
            {
                cmd.Blit(depthTexture.rt, renderingData.cameraData.renderer.cameraColorTargetHandle);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Release()
        {
            depthTexture?.Release();
            tempDepthTexture?.Release();
            tempDepthBuffer?.Release();
            depthTexture = null;
            tempDepthTexture = null;
            tempDepthBuffer = null;
        }
    }

    class CustomColorPass : ScriptableRenderPass
    {
        RTHandle customColorTexture;

        int cachedFullWidth = -1;
        int cachedFullHeight = -1;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var baseDesc = renderingData.cameraData.cameraTargetDescriptor;

            int fullWidth = baseDesc.width;
            int fullHeight = baseDesc.height;

            if (customColorTexture == null || fullWidth != cachedFullWidth || fullHeight != cachedFullHeight)
            {
                cachedFullWidth = fullWidth;
                cachedFullHeight = fullHeight;

                var fullDesc = new RenderTextureDescriptor(fullWidth, fullHeight, RenderTextureFormat.DefaultHDR);
                fullDesc.depthBufferBits = 0;
                fullDesc.msaaSamples = 1;
                fullDesc.useMipMap = true;
                fullDesc.autoGenerateMips = false;
                RenderingUtils.ReAllocateIfNeeded(ref customColorTexture, fullDesc, name: "_CustomColorTexture");
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Copy Camera Color");

            SetMainCameraFlag(cmd, renderingData.cameraData.camera);
            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            cmd.Blit(source, customColorTexture);

            cmd.GenerateMips(customColorTexture);

            cmd.SetGlobalTexture(CustomColorTexture, customColorTexture.rt);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void RestoreGlobalTexture(CommandBuffer cmd)
        {
            if (customColorTexture != null)
                cmd.SetGlobalTexture(CustomColorTexture, customColorTexture.rt);
        }

        public void Release()
        {
            customColorTexture?.Release();
            customColorTexture = null;
        }
    }

    class SkippedTexturesPass : ScriptableRenderPass
    {
        readonly CustomColorPass colorPass;
        public bool SkipDepth { get; set; }
        public bool SkipColor { get; set; }

        public SkippedTexturesPass(CustomColorPass colorPass)
        {
            this.colorPass = colorPass;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Bind Custom Texture Fallbacks");
            SetMainCameraFlag(cmd, renderingData.cameraData.camera);
            if (SkipDepth)
                cmd.SetGlobalTexture(CustomDepthTexture, Texture2D.blackTexture);
            if (SkipColor)
                cmd.SetGlobalTexture(CustomColorTexture, Texture2D.blackTexture);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // Some gameplay particles sample the previous copy before this frame's color pass.
            if (SkipColor)
                colorPass.RestoreGlobalTexture(cmd);
        }
    }

    static void SetMainCameraFlag(CommandBuffer cmd, Camera camera)
    {
        cmd.SetGlobalFloat(IsMainCamera, camera == Camera.main ? 1f : 0f);
    }

    [SerializeField] Material depthMaterial;
    [SerializeField] Material depthCopyMaterial;
    CustomDepthPass depthPass;
    CustomColorPass colorPass;
    SkippedTexturesPass skippedTexturesPass;

    public override void Create()
    {
        depthPass?.Release();
        colorPass?.Release();

        depthPass = new CustomDepthPass();
        depthPass.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;

        colorPass = new CustomColorPass();
        colorPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

        skippedTexturesPass = new SkippedTexturesPass(colorPass);
        skippedTexturesPass.renderPassEvent = RenderPassEvent.BeforeRendering;
    }

    protected override void Dispose(bool disposing)
    {
        depthPass?.Release();
        colorPass?.Release();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var camera = renderingData.cameraData.camera;
        bool hasOverride = camera.TryGetComponent<CustomRendererPassUsage>(out var usage) && usage.isActiveAndEnabled;
        bool needsDepth = visualize || !hasOverride || usage.RequiresCustomDepth;
        bool needsColor = !hasOverride || usage.RequiresCustomColor;

        if (!needsDepth || !needsColor)
        {
            skippedTexturesPass.SkipDepth = !needsDepth;
            skippedTexturesPass.SkipColor = !needsColor;
            renderer.EnqueuePass(skippedTexturesPass);
        }

        if (needsDepth)
        {
            depthPass.SetMaterial(depthMaterial);
            depthPass.SetCopyMaterial(depthCopyMaterial);
            depthPass.SetVisualize(visualize);
            renderer.EnqueuePass(depthPass);
        }

        if (needsColor)
            renderer.EnqueuePass(colorPass);
    }
}
