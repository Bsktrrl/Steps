using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_StepSound : Singleton<Player_StepSound>
{
    [Header("General")]
    [SerializeField] private int audioSourcePoolSize = 10;
    [SerializeField] private float stepSound_Volume = 1f;
    [SerializeField] private Vector2 randomVolumeRange = new Vector2(0.9f, 4.1f);
    [SerializeField] private Vector2 randomPitchRange = new Vector2(0.9f, 1.1f);

    [Header("Block Sounds")]
    [SerializeField] private List<BlockStepSoundSet> blockSounds = new List<BlockStepSoundSet>();

    private readonly List<AudioSource> stepSound_AudioSources = new List<AudioSource>();
    private readonly Dictionary<BlockElement, AudioClip[]> soundLookup = new Dictionary<BlockElement, AudioClip[]>();

    private int nextSourceIndex = 0;


    //--------------------


    private void Start()
    {
        BuildLookup();
        CreateAudioSourcePool();
    }


    //--------------------


    private void OnEnable()
    {
        Movement.Action_StepTaken += MakeStepSound;
    }

    private void OnDisable()
    {
        Movement.Action_StepTaken -= MakeStepSound;
    }


    //--------------------


    private void BuildLookup()
    {
        soundLookup.Clear();

        for (int i = 0; i < blockSounds.Count; i++)
        {
            BlockStepSoundSet set = blockSounds[i];

            if (set == null || set.blockElement == BlockElement.None || set.clips == null || set.clips.Length == 0)
                continue;

            soundLookup[set.blockElement] = set.clips;
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


    //--------------------


    public void MakeStepSound()
    {
        GameObject currentBlock = Movement.Instance.blockStandingOn;
        if (currentBlock == null)
            return;

        if (!currentBlock.TryGetComponent<BlockInfo>(out BlockInfo blockInfo))
            return;

        if (blockInfo.blockElement == BlockElement.None)
            return;

        AudioClip clipToPlay = GetRandomClip(blockInfo.blockElement);
        if (clipToPlay == null)
            return;

        AudioSource source = FindAudioSource();
        if (source == null)
            return;

        float randomVolume = UnityEngine.Random.Range(randomVolumeRange.x, randomVolumeRange.y);
        float randomPitch = UnityEngine.Random.Range(randomPitchRange.x, randomPitchRange.y);

        source.pitch = randomPitch;
        source.volume = stepSound_Volume * randomVolume;

        source.PlayOneShot(clipToPlay);
    }

    private AudioClip GetRandomClip(BlockElement element)
    {
        if (!soundLookup.TryGetValue(element, out AudioClip[] clips))
            return null;

        if (clips == null || clips.Length == 0)
            return null;

        int index = UnityEngine.Random.Range(0, clips.Length);
        return clips[index];
    }

    private AudioSource FindAudioSource()
    {
        int sourceCount = stepSound_AudioSources.Count;
        if (sourceCount == 0)
            return null;

        // First try to find a free source, starting from the last used index.
        for (int i = 0; i < sourceCount; i++)
        {
            int index = (nextSourceIndex + i) % sourceCount;

            if (!stepSound_AudioSources[index].isPlaying)
            {
                nextSourceIndex = (index + 1) % sourceCount;
                return stepSound_AudioSources[index];
            }
        }

        // Fallback: all sources are busy, reuse the next one in round-robin.
        AudioSource fallback = stepSound_AudioSources[nextSourceIndex];
        nextSourceIndex = (nextSourceIndex + 1) % sourceCount;
        return fallback;
    }
}


[Serializable]
public class BlockStepSoundSet
{
    public BlockElement blockElement;
    public AudioClip[] clips;
}
