using Unity.Cinemachine;
using UnityEngine;

public class MotionSicknessToggle : Singleton<MotionSicknessToggle>
{
    public CinemachineBrain brain;

    private CinemachineBlendDefinition savedDefaultBlend;
    private CinemachineBlenderSettings savedCustomBlends;
    private bool saved = false;

    public void SetReduceMotion(bool enabled)
    {
        if (!brain) brain = Camera.main.GetComponent<CinemachineBrain>();

        if (enabled)
        {
            if (!saved)
            {
                // Save current blends so we can restore later
                savedDefaultBlend = brain.DefaultBlend;
                savedCustomBlends = brain.CustomBlends;
                saved = true;
            }

            // Replace with instant cut
            brain.DefaultBlend = new CinemachineBlendDefinition(
                CinemachineBlendDefinition.Styles.Cut,
                0f
            );

            // Null out custom blends so nothing overrides
            brain.CustomBlends = null;
        }
        else
        {
            if (saved)
            {
                // Restore what was there before
                brain.DefaultBlend = savedDefaultBlend;
                brain.CustomBlends = savedCustomBlends;
                saved = false;
            }
        }
    }
}
