using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GraplingHook : Singleton<Player_GraplingHook>
{
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

        redDotSceneObject.SetActive(false);
    }
    private void OnEnable()
    {
        Movement.Action_StepTaken += ResetGrapplingHook;
        Movement.Action_RespawnPlayerEarly += ResetGrapplingHook;
    }
    private void OnDisable()
    {
        Movement.Action_StepTaken -= ResetGrapplingHook;
        Movement.Action_RespawnPlayerEarly -= ResetGrapplingHook;
    }


    //--------------------


    void SetupLine()
    {
        redDotSceneObject = Instantiate(redDot);
        redDotSceneObject.transform.parent = redDot_Parent.transform;

        lineRenderer.positionCount = 2;
        EndLineRenderer();
    }
    void RemoveLine()
    {
        if (redDotSceneObject)
            DestroyImmediate(redDotSceneObject);

        endPoint = Vector3.zero;
    }

    public void RunLineReader()
    {
        Vector3 lookDir = (transform.position - endPoint).normalized;

        //Beam Start
        lineRenderer.SetPosition(0, transform.position - (lookDir * 0.4f));

        //Beam End
        lineRenderer.SetPosition(1, redDotSceneObject.transform.position);
    }
    public void EndLineRenderer()
    {
        //Beam Start
        lineRenderer.SetPosition(0, transform.position);

        //Beam End
        lineRenderer.SetPosition(1, transform.position);

        if (redDotSceneObject)
            redDotSceneObject.SetActive(false);
    }
    public void ResetGrapplingHook()
    {
        RemoveLine();
        SetupLine();
    }
}
