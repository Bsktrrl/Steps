using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GraplingHook : Singleton<Player_GraplingHook>
{
    public static event Action Action_GrapplingHook_Hit;

    [Header("Grappling Distance")]
    //float grapplingDistance = 5.55f;
    //float movementSpeed = 15;

    [Header("Red Dot Object")]
    [SerializeField] GameObject redDot_Parent;
    [SerializeField] GameObject redDot;
    public GameObject hitEffect;
    public GameObject redDotSceneObject;

    //RaycastHit hit;

    public Vector3 endPoint;
    public LineRenderer lineRenderer;

    //[SerializeField] bool isSearchingGrappling;
    //[SerializeField] bool canGrapple;
    public bool isGrapplingHooking;


    //Vector3 endDestination;

    //GameObject blockUnderPlayer_Old;
    //GameObject blockUnderPlayer_New;


    //--------------------


    private void Start()
    {
        SetupLine();
    }
    private void OnEnable()
    {
        Movement.Action_RespawnPlayerEarly += ResetGrapplingHook;
    }
    private void OnDisable()
    {
        Movement.Action_RespawnPlayerEarly -= ResetGrapplingHook;
        ResetGrapplingHook();
    }


    //--------------------


    void SetupLine()
    {
        redDotSceneObject = Instantiate(redDot);
        redDotSceneObject.transform.parent = redDot_Parent.transform;

        lineRenderer.positionCount = 2;
        EndLineRenderer();
    }

    public void RunLineReader()
    {
        Vector3 lookDir = (transform.position - endPoint).normalized;

        //Beam Start
        lineRenderer.SetPosition(0, transform.position - (lookDir * 0.4f));

        //Beam End
        lineRenderer.SetPosition(1, redDotSceneObject.transform.position);

        // If a step happens while the aim is visible, clear it as before.
        Movement.Action_StepTaken -= ResetGrapplingHook;
        Movement.Action_StepTaken += ResetGrapplingHook;

        Action_GrapplingHook_Hit?.Invoke();
    }
    public void EndLineRenderer()
    {
        Movement.Action_StepTaken -= ResetGrapplingHook;

        if (lineRenderer && lineRenderer.positionCount >= 2)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        if (redDotSceneObject)
            redDotSceneObject.SetActive(false);
    }
    public void ResetGrapplingHook()
    {
        endPoint = Vector3.zero;
        EndLineRenderer();
    }
}
