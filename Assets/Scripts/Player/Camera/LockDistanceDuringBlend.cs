using UnityEngine;
using Unity.Cinemachine;

[ExecuteAlways]
public class LockDistanceDuringBlend : MonoBehaviour
{
    private CinemachineThirdPersonFollow tpf;
    private float lastDistance;
    private float lastArmLength;
    private CinemachineBrain brain;

    void Awake()
    {
        tpf = GetComponent<CinemachineThirdPersonFollow>();
        brain = FindObjectOfType<CinemachineBrain>();
    }

    void LateUpdate()
    {
        if (tpf == null || brain == null)
            return;

        if (brain.IsBlending && (Movement.Instance.isAscending || Movement.Instance.isDecending))
        {
            // Freeze values during blend
            tpf.CameraDistance = lastDistance;
            tpf.VerticalArmLength = lastArmLength;
        }
        else
        {
            // Cache values when not blending
            lastDistance = tpf.CameraDistance;
            lastArmLength = tpf.VerticalArmLength;
        }
    }
}
