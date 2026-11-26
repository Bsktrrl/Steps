using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public sealed class CustomRendererPassUsage : MonoBehaviour
{
    [SerializeField, Tooltip("Keep enabled if this camera renders effects that sample the custom depth texture.")]
    bool requiresCustomDepth = true;

    [SerializeField, Tooltip("Keep enabled if this camera renders effects that sample the custom color texture.")]
    bool requiresCustomColor = true;

    public bool RequiresCustomDepth => requiresCustomDepth;
    public bool RequiresCustomColor => requiresCustomColor;
}
