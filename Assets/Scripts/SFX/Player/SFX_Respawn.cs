using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SFX_Respawn : Singleton<SFX_Respawn>
{
    public static event Action Action_RespawnPlayer;

    [Header("KeyPresses Parameters")]
    float holdDuration = 0.5f;
    [SerializeField] float holdtimer = 0;
    [SerializeField] bool useUnscaledTime = true; // ignore timescale (pause)
    Coroutine holdRoutine;

    [Header("Audio")]
    AudioSource audioSource_Player;
    [SerializeField] AudioClip sound_RespawnHold;
    [SerializeField] AudioClip sound_RespawnCancel;
    [SerializeField] AudioClip sound_RespawnComplete;

    [Header("SFX")]
    [SerializeField] GameObject sfx_OnHoldStarted;
    [SerializeField] GameObject sfx_OnHoldCanceled;
    [SerializeField] GameObject sfx_OnHoldCompleted;

    [Header("Events")]
    [SerializeField] UnityEvent event_OnHoldStarted;
    [SerializeField] UnityEvent event_OnHoldCanceled;
    [SerializeField] UnityEvent event_OnHoldCompleted;


    //--------------------


    private void Start()
    {
        audioSource_Player = GetComponent<AudioSource>();
        SetEffectsOff();
    }

    private void OnEnable()
    {
        Player_KeyInputs.Action_RespawnHold += StartRespawnHold;
        Player_KeyInputs.Action_RespawnCanceled += CancelRespawnHold;
    }
    private void OnDisable()
    {
        Player_KeyInputs.Action_RespawnHold -= StartRespawnHold;
        Player_KeyInputs.Action_RespawnCanceled -= CancelRespawnHold;
    }

    //--------------------


    public void StartRespawnHold()
    {
        if (holdRoutine != null) StopCoroutine(holdRoutine);
        holdRoutine = StartCoroutine(HoldTimer());

        event_OnHoldStarted?.Invoke();

        if (audioSource_Player != null)
        {
            audioSource_Player.loop = true;
            audioSource_Player.time = 0f;
            audioSource_Player.Play();
        }
    }

    public void CancelRespawnHold()
    {
        if (holdRoutine != null)
        {
            StopCoroutine(holdRoutine);
            holdRoutine = null;
        }

        if (holdtimer < holdDuration)
        {
            if (audioSource_Player != null && audioSource_Player.isPlaying)
                audioSource_Player.Stop();

            event_OnHoldCanceled?.Invoke();
        }

        holdtimer = 0;
    }
    private IEnumerator HoldTimer()
    {
        float t = 0f;
        while (t < holdDuration)
        {
            t += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            holdtimer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            //onHoldProgress?.Invoke(Mathf.Clamp01(t / holdDuration));
            yield return null;
        }

        // finished
        holdRoutine = null;
        if (audioSource_Player != null && audioSource_Player.isPlaying) audioSource_Player.Stop();

        event_OnHoldCompleted?.Invoke();
    }


    //--------------------


    public void Event_OnHoldStarted()
    {
        SetEffectsOff();

        //Sound Effect
        if (audioSource_Player)
        {
            audioSource_Player.clip = sound_RespawnHold;
            audioSource_Player.loop = false;
            audioSource_Player.Play();
        }

        //SFX
        if (sfx_OnHoldStarted)
        {
            sfx_OnHoldStarted.SetActive(true);
        }
    }
    public void Event_OnHoldCanceled()
    {
        SetEffectsOff();

        //Sound Effect
        if (audioSource_Player)
        {
            audioSource_Player.clip = sound_RespawnCancel;
            audioSource_Player.loop = false;
            audioSource_Player.Play();
        }
        
        //SFX
        if (sfx_OnHoldCanceled)
        {
            sfx_OnHoldCanceled.SetActive(true);
            StartCoroutine(StopEffect(0.2f, sfx_OnHoldCanceled));
        }
    }
    public void Event_OnHoldCompleted()
    {
        SetEffectsOff();

        //Sound Effect
        if (audioSource_Player)
        {
            audioSource_Player.clip = sound_RespawnComplete;
            audioSource_Player.loop = false;
            audioSource_Player.Play();
        }
        
        //SFX
        if (sfx_OnHoldCompleted)
        {
            sfx_OnHoldCompleted.SetActive(true);
            StartCoroutine(StopEffect(0.25f, sfx_OnHoldCompleted));
        }
        
        //Perform Respawn
        Action_RespawnPlayer?.Invoke();
    }

    void SetEffectsOff()
    {
        sfx_OnHoldStarted.SetActive(false);
        sfx_OnHoldCanceled.SetActive(false);
        sfx_OnHoldCompleted.SetActive(false);
    }

    IEnumerator StopEffect(float waitTime, GameObject effectPrefab)
    {
        yield return new WaitForSeconds(waitTime);

        effectPrefab.SetActive(false);
    }
}
