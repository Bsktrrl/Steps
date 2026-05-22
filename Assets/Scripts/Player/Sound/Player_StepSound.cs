using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Player_StepSound : Singleton<Player_StepSound>
{
    [Header("General")]
    [SerializeField] private int audioSourcePoolSize = 10;
    [SerializeField] private float stepSound_Volume = 0.4f;
    [SerializeField] private Vector2 randomVolumeRange = new Vector2(0.9f, 4.1f);
    [SerializeField] private Vector2 randomPitchRange = new Vector2(0.9f, 1.1f);

    [Header("Block Sounds")]
    [SerializeField] private List<BlockStepSoundSet> blockSounds = new List<BlockStepSoundSet>();

    [Header("Block Sounds With Same Element Variant")]
    [SerializeField]
    private List<BlockStepSoundSet_WithSameElementVariant> blockSounds_WithSameElementVariant =
        new List<BlockStepSoundSet_WithSameElementVariant>();

    private readonly List<AudioSource> stepSound_AudioSources = new List<AudioSource>();

    private readonly Dictionary<BlockElement, BlockStepSoundSet> soundLookup =
        new Dictionary<BlockElement, BlockStepSoundSet>();

    private readonly Dictionary<BlockElement, BlockStepSoundSet_WithSameElementVariant> sameElementVariantLookup =
        new Dictionary<BlockElement, BlockStepSoundSet_WithSameElementVariant>();

    private int nextSourceIndex = 0;

    private BlockElement stepStartedOnElement = BlockElement.None;
    private GameObject stepStartedOnBlock = null;

    private bool sameElementMoveSoundWasPlayedEarly = false;

  

    private void Start()
    {
        BuildLookup();
        CreateAudioSourcePool();
    }


    private void OnEnable()
    {
        Movement.Action_StepTaken_Early += CacheStepStartBlock;
        Movement.Action_StepTaken_Early += TryPlaySameElementMoveSoundEarly;

        Movement.Action_StepTaken += MakeStepSound;

        Player_CeilingGrab.Action_isCeilingGrabbing += MakeStepSound;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished += MakeStepSound;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken_Early -= CacheStepStartBlock;
        Movement.Action_StepTaken_Early -= TryPlaySameElementMoveSoundEarly;

        Movement.Action_StepTaken -= MakeStepSound;

        Player_CeilingGrab.Action_isCeilingGrabbing -= MakeStepSound;
        Player_CeilingGrab.Action_isCeilingGrabbing_Finished -= MakeStepSound;
    }


    private void CacheStepStartBlock()
    {
        sameElementMoveSoundWasPlayedEarly = false;

        stepStartedOnElement = BlockElement.None;
        stepStartedOnBlock = Movement.Instance.blockStandingOn;

        if (stepStartedOnBlock == null)
            return;

        if (!stepStartedOnBlock.TryGetComponent<BlockInfo>(out BlockInfo blockInfo))
            return;

        stepStartedOnElement = blockInfo.blockElement;
    }

    private void TryPlaySameElementMoveSoundEarly()
    {
        //if (Player_Animations.Instance.isWalkGliding_Delay)
        //    return;

        GameObject startBlock = Movement.Instance.blockStandingOn;
        GameObject targetBlock = Movement.Instance.currentMoveTargetBlock;

        if (startBlock == null || targetBlock == null)
            return;

        if (startBlock == targetBlock)
            return;

        if (!startBlock.TryGetComponent<BlockInfo>(out BlockInfo startBlockInfo))
            return;

        if (!targetBlock.TryGetComponent<BlockInfo>(out BlockInfo targetBlockInfo))
            return;

        BlockElement startElement = startBlockInfo.blockElement;
        BlockElement targetElement = targetBlockInfo.blockElement;

        if (startElement == BlockElement.None || targetElement == BlockElement.None)
            return;

        if (startElement != targetElement)
            return;

        if (!sameElementVariantLookup.TryGetValue(targetElement, out BlockStepSoundSet_WithSameElementVariant variantSoundSet))
            return;

        if (!HasClips(variantSoundSet.moveOn_SameElement_Clips))
            return;

        PlayStepSound(
            variantSoundSet.moveOn_SameElement_Clips,
            variantSoundSet.outputMixerGroup
        );

        sameElementMoveSoundWasPlayedEarly = true;
    }


    private void BuildLookup()
    {
        soundLookup.Clear();
        sameElementVariantLookup.Clear();

        for (int i = 0; i < blockSounds.Count; i++)
        {
            BlockStepSoundSet set = blockSounds[i];

            if (set == null)
                continue;

            if (set.blockElement == BlockElement.None)
                continue;

            if (set.stepOn_Clips == null || set.stepOn_Clips.Length == 0)
                continue;

            soundLookup[set.blockElement] = set;
        }

        for (int i = 0; i < blockSounds_WithSameElementVariant.Count; i++)
        {
            BlockStepSoundSet_WithSameElementVariant set = blockSounds_WithSameElementVariant[i];

            if (set == null)
                continue;

            if (set.blockElement == BlockElement.None)
                continue;

            if (!HasClips(set.stepOn_Clips) && !HasClips(set.moveOn_SameElement_Clips))
                continue;

            sameElementVariantLookup[set.blockElement] = set;
        }
    }

    private void CreateAudioSourcePool()
    {
        stepSound_AudioSources.Clear();

        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f; // 0 = 2D, 1 = 3D. Change if you want 3D footsteps.
            source.dopplerLevel = 0f;
            source.volume = 1f;

            stepSound_AudioSources.Add(source);
        }
    }


    public void MakeStepSound()
    {
        if (sameElementMoveSoundWasPlayedEarly)
        {
            ClearCachedStepStartBlock();
            sameElementMoveSoundWasPlayedEarly = false;
            return;
        }

        //if (Player_Animations.Instance.isWalkGliding_Delay)
        //    return;

        GameObject currentBlock = Movement.Instance.blockStandingOn;
        if (currentBlock == null)
            return;

        if (!currentBlock.TryGetComponent<BlockInfo>(out BlockInfo currentBlockInfo))
            return;

        BlockElement currentElement = currentBlockInfo.blockElement;

        if (currentElement == BlockElement.None)
            return;

        BlockElement previousElement = GetPreviousBlockElement();

        AudioClip[] clipsToPlay = null;
        AudioMixerGroup mixerGroupToUse = null;

        if (sameElementVariantLookup.TryGetValue(currentElement, out BlockStepSoundSet_WithSameElementVariant variantSoundSet))
        {
            bool movedFromSameElement =
                previousElement == currentElement &&
                stepStartedOnBlock != currentBlock;

            clipsToPlay = movedFromSameElement
                ? variantSoundSet.moveOn_SameElement_Clips
                : variantSoundSet.stepOn_Clips;

            if (!HasClips(clipsToPlay))
                clipsToPlay = variantSoundSet.stepOn_Clips;

            mixerGroupToUse = variantSoundSet.outputMixerGroup;
        }
        else
        {
            BlockStepSoundSet soundSet = GetSoundSet(currentElement);
            if (soundSet == null)
                return;

            clipsToPlay = soundSet.stepOn_Clips;
            mixerGroupToUse = soundSet.outputMixerGroup;
        }

        PlayStepSound(clipsToPlay, mixerGroupToUse);

        ClearCachedStepStartBlock();
    }

    private void PlayStepSound(AudioClip[] clips, AudioMixerGroup outputMixerGroup)
    {
        AudioClip clipToPlay = GetRandomClip(clips);
        if (clipToPlay == null)
            return;

        AudioSource source = FindAudioSource();
        if (source == null)
            return;

        float randomVolume = UnityEngine.Random.Range(randomVolumeRange.x, randomVolumeRange.y);
        float randomPitch = UnityEngine.Random.Range(randomPitchRange.x, randomPitchRange.y);

        source.pitch = randomPitch;
        source.volume = stepSound_Volume * randomVolume;
        source.outputAudioMixerGroup = outputMixerGroup;

        source.PlayOneShot(clipToPlay);
    }

    private void ClearCachedStepStartBlock()
    {
        stepStartedOnElement = BlockElement.None;
        stepStartedOnBlock = null;
    }

    private BlockElement GetPreviousBlockElement()
    {
        if (stepStartedOnElement != BlockElement.None)
            return stepStartedOnElement;

        GameObject previousBlock = Movement.Instance.blockStandingOn_Previous;

        if (previousBlock == null)
            return BlockElement.None;

        if (!previousBlock.TryGetComponent<BlockInfo>(out BlockInfo previousBlockInfo))
            return BlockElement.None;

        return previousBlockInfo.blockElement;
    }

    private BlockStepSoundSet GetSoundSet(BlockElement element)
    {
        if (!soundLookup.TryGetValue(element, out BlockStepSoundSet soundSet))
            return null;

        return soundSet;
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (!HasClips(clips))
            return null;

        int index = UnityEngine.Random.Range(0, clips.Length);
        return clips[index];
    }

    private bool HasClips(AudioClip[] clips)
    {
        return clips != null && clips.Length > 0;
    }

    private AudioSource FindAudioSource()
    {
        int sourceCount = stepSound_AudioSources.Count;
        if (sourceCount == 0)
            return null;

        for (int i = 0; i < sourceCount; i++)
        {
            int index = (nextSourceIndex + i) % sourceCount;

            if (!stepSound_AudioSources[index].isPlaying)
            {
                nextSourceIndex = (index + 1) % sourceCount;
                return stepSound_AudioSources[index];
            }
        }

        AudioSource fallback = stepSound_AudioSources[nextSourceIndex];
        nextSourceIndex = (nextSourceIndex + 1) % sourceCount;
        return fallback;
    }
}


[Serializable]
public class BlockStepSoundSet
{
    public BlockElement blockElement;

    [Header("Audio")]
    public AudioMixerGroup outputMixerGroup;
    public AudioClip[] stepOn_Clips;
}


[Serializable]
public class BlockStepSoundSet_WithSameElementVariant
{
    public BlockElement blockElement;

    [Header("Audio")]
    public AudioMixerGroup outputMixerGroup;

    [Header("Different Element Into This Element")]
    public AudioClip[] stepOn_Clips;

    [Header("Same Element Into Same Element")]
    public AudioClip[] moveOn_SameElement_Clips;
}