using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_SoundManager : MonoBehaviour
{
    [Header("Audio Sources (Pool)")]
    public List<AudioSource> audioSource_Abilities_List = new List<AudioSource>();

    [Header("Menu Sounds")]
    public AudioClip ability_Ascend;
    public AudioClip ability_Descend;

    public AudioClip ability_Dash;
    public AudioClip ability_Jump;

    public AudioClip ability_CeilingGrab;

    public AudioClip ability_GrapplingHook_Start;
    public AudioClip ability_GrapplingHook_Hit;
    public AudioClip ability_GrapplingHook_SendPlayer;
    public AudioClip ability_GrapplingHook_RollBack;

    private int nextAudioSourceIndex = 0;


    //--------------------


    private void OnEnable()
    {
        Movement.Action_isAscending += PlaySound_Ascend;
        Movement.Action_isDescending += PlaySound_Descend;
        Player_CeilingGrab.Action_CeilingGrab_isPressed += PlaySound_CeilingGrab;

        Movement.Action_isDashing += PlaySound_Dash;
        Movement.Action_isJumping += PlaySound_Jump;

        Player_KeyInputs.Action_GrapplingHook_isPressed += PlaySound_GrapplingHook_Start;
        Player_GraplingHook.Action_GrapplingHook_Hit += PlaySound_GrapplingHook_Hit;
        Movement.Action_isGrapplingHooking_RollBack += PlaySound_GrapplingHook_RollBack;
        Movement.Action_isGrapplingHooking_SendPlayer += PlaySound_GrapplingHook_SendPlayer;

        Movement.Action_RespawnPlayer += StopSoundPlaying;
    }
    private void OnDisable()
    {
        Movement.Action_isAscending -= PlaySound_Ascend;
        Movement.Action_isDescending -= PlaySound_Descend;
        Player_CeilingGrab.Action_CeilingGrab_isPressed -= PlaySound_CeilingGrab;

        Movement.Action_isDashing -= PlaySound_Dash;
        Movement.Action_isJumping -= PlaySound_Jump;

        Player_KeyInputs.Action_GrapplingHook_isPressed -= PlaySound_GrapplingHook_Start;
        Player_GraplingHook.Action_GrapplingHook_Hit -= PlaySound_GrapplingHook_Hit;
        Movement.Action_isGrapplingHooking_RollBack -= PlaySound_GrapplingHook_RollBack;
        Movement.Action_isGrapplingHooking_SendPlayer -= PlaySound_GrapplingHook_SendPlayer;

        Movement.Action_RespawnPlayer -= StopSoundPlaying;
    }


    //--------------------


    void PlaySound(AudioClip clip, float delay)
    {
        if (delay > 0f)
            StartCoroutine(PlaySoundDelayed(clip, delay));
        else
            PlaySoundNow(clip);
    }

    IEnumerator PlaySoundDelayed(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySoundNow(clip);
    }

    void PlaySoundNow(AudioClip clip)
    {
        List<AudioSource> audioSourcePool = audioSource_Abilities_List;

        if (clip == null || audioSourcePool == null || audioSourcePool.Count == 0)
            return;

        AudioSource selectedSource = null;
        int poolCount = audioSourcePool.Count;

        for (int i = 0; i < poolCount; i++)
        {
            int index = (nextAudioSourceIndex + i) % poolCount;
            AudioSource source = audioSourcePool[index];

            if (source == null)
                continue;

            if (!source.isPlaying)
            {
                selectedSource = source;
                nextAudioSourceIndex = (index + 1) % poolCount;
                break;
            }
        }

        if (selectedSource == null)
        {
            for (int i = 0; i < poolCount; i++)
            {
                int index = (nextAudioSourceIndex + i) % poolCount;
                AudioSource source = audioSourcePool[index];

                if (source == null)
                    continue;

                selectedSource = source;
                nextAudioSourceIndex = (index + 1) % poolCount;
                break;
            }
        }

        if (selectedSource == null)
            return;

        selectedSource.PlayOneShot(clip);
    }


    //--------------------


    void StopSoundPlaying()
    {
        // Stop delayed sounds that have not played yet
        StopAllCoroutines();

        // Stop sounds that are currently playing
        if (audioSource_Abilities_List == null)
            return;

        foreach (AudioSource source in audioSource_Abilities_List)
        {
            if (source == null)
                continue;

            source.Stop();
        }
    }


    //--------------------


    void PlaySound_Ascend()
    {
        PlaySound(ability_Ascend, 0.3f);
    }
    void PlaySound_Descend()
    {
        PlaySound(ability_Descend, 0.3f);
    }

    void PlaySound_Dash()
    {
        PlaySound(ability_Dash, 0.45f);
    }
    void PlaySound_Jump()
    {
        PlaySound(ability_Jump, 0.4f);
    }

    void PlaySound_CeilingGrab()
    {
        PlaySound(ability_CeilingGrab, 0.45f);
    }

    void PlaySound_GrapplingHook_Start()
    {
        PlaySound(ability_GrapplingHook_Start, 0.0f);
    }
    void PlaySound_GrapplingHook_Hit()
    {
        PlaySound(ability_GrapplingHook_Hit, 0.0f);
    }
    void PlaySound_GrapplingHook_SendPlayer()
    {
        PlaySound(ability_GrapplingHook_SendPlayer, 0.0f);
    }
    void PlaySound_GrapplingHook_RollBack()
    {
        PlaySound(ability_GrapplingHook_RollBack, 0.0f);
    }
}
