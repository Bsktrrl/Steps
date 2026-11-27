using UnityEngine;

public class Player_Quicksand : Singleton<Player_Quicksand>
{
    [Header("General")]
    public bool isInQuicksand;

    [Header("Quicksand")]
    public int quicksandCounter;

    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += UpdateQuicksandState;
        Movement.Action_RespawnPlayer += ResetQuicksandCounter;
    }

    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= UpdateQuicksandState;
        Movement.Action_RespawnPlayer -= ResetQuicksandCounter;
    }

    private void ResetQuicksandCounter()
    {
        quicksandCounter = 0;
    }

    private void UpdateQuicksandState()
    {
        Movement movement = Movement.Instance;
        GameObject standingBlock = movement.blockStandingOn;
        if (!standingBlock)
            return;

        isInQuicksand = standingBlock.TryGetComponent(out Block_Quicksand quicksand);
        if (!isInQuicksand)
            quicksandCounter = 0;
        else if (!movement.isRespawning && quicksand.isActiveAndEnabled)
            quicksandCounter++;
    }
}
