using System;
using UnityEngine;

public class HoleShaderOnOffScript : Singleton<HoleShaderOnOffScript>
{
    [SerializeField] private Camera cameraObject;

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private bool checkEveryFrame = true;

    public float PlayerBody_offset;

    private bool holeShaderIsOn;
    private FreeCam freeCam;
    private RaycastHit[] obstacleHits = new RaycastHit[16];
    bool transitionBool;


    //--------------------


    private void Awake()
    {
        if (cameraObject == null)
            cameraObject = Camera.main;

        // Shader globals can retain values from earlier camera/scene activity.
        // Start disabled; the normal raycast check will enable the hole if needed.
        HoleShader_Off();
    }

    private void Start()
    {
        freeCam = FindAnyObjectByType<FreeCam>();
    }

    private void Update()
    {
        if (checkEveryFrame && !GlueplantCamera.IsTraveling && !AncientTablet.ActiveTablet &&
            !(freeCam && freeCam._isActive))
        {
            CheckForBlockInfoBetweenPlayerAndCamera();
        }
    }


    //--------------------


    private void CheckForBlockInfoBetweenPlayerAndCamera()
    {
        if (PlayerManager.Instance == null ||
            PlayerManager.Instance.playerBody == null ||
            cameraObject == null)
        {
            HoleShader_Off();
            return;
        }

        //Vector3 playerPosition = PlayerManager.Instance.playerBody.transform.position + (Vector3.up * (Mathf.Abs(Player_BodyHeight.Instance.height_Normal) + Mathf.Abs(PlayerBody_offset)));
        Vector3 playerPosition = PlayerManager.Instance.playerBody.transform.position;

        Vector3 cameraPosition = cameraObject.transform.position;

        Vector3 direction = cameraPosition - playerPosition;
        float distance = direction.magnitude;

        if (distance <= 0.01f)
        {
            HoleShader_Off();
            return;
        }

        direction.Normalize();

        // A ray starting inside a water collider cannot hit that collider.
        SetHoleShaderState(HasBlockBetweenPlayerAndCamera(playerPosition, direction, distance) ||
            (!PlayerManager.Instance.npcInteraction && Movement.Instance.IsInWaterfall()));
    }

    private bool HasBlockBetweenPlayerAndCamera(Vector3 origin, Vector3 direction, float distance)
    {
        while (true)
        {
            int hitCount = Physics.RaycastNonAlloc(
                origin, direction, obstacleHits, distance, obstacleLayers, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hitCount; i++)
            {
                if (obstacleHits[i].collider.GetComponentInParent<BlockInfo>() != null)
                    return true;
            }

            if (hitCount < obstacleHits.Length)
                return false;

            // A full buffer can omit hits, including a block behind other colliders.
            Array.Resize(ref obstacleHits, obstacleHits.Length * 2);
        }
    }

    private void SetHoleShaderState(bool active)
    {
        if (holeShaderIsOn == active)
            return;

        if (active)
        {
            HoleShader_On();
        }
        else
        {
            HoleShader_Off();
        }
    }


    //--------------------


    public void HoleShader_On()
    {
        if (AncientTablet.ActiveTablet || (freeCam && freeCam._isActive)) return;

        holeShaderIsOn = true;
        transitionBool = true;
        Shader.SetGlobalFloat("_HoleShaderEnabled", 1);
        Shader.SetGlobalFloat("_CameraHoleEnabled", 1);
    }
    public void HoleShader_Off()
    {
        holeShaderIsOn = false;
        transitionBool = false;
        Shader.SetGlobalFloat("_HoleShaderEnabled", 0);
        Shader.SetGlobalFloat("_CameraHoleEnabled", 0);
    }

    public void SetHullShaderPosY(float y)
    {
        Shader.SetGlobalFloat("_PlayerOffset", y);
    }


    //--------------------


    private void OnDrawGizmosSelected()
    {
        if (cameraObject == null)
            return;

        if (PlayerManager.Instance == null || PlayerManager.Instance.playerBody == null)
            return;

        Gizmos.DrawLine(
            PlayerManager.Instance.playerBody.transform.position,
            cameraObject.transform.position
        );
    }
}
