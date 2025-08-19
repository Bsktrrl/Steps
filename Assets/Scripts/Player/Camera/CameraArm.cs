using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject cameraOffset;

    Vector3 cameraDefault_Normal;
    Vector3 cameraDefault_CeilingGrab;

    RaycastHit hit;

    float camera_NormalHeight = 0.2f;
    float camera_StairHeight = 0.7f;
    float camera_RotationX_Offset_Normal = 29;
    float camera_RotationX_Offset_NormalOffset = 5;
    float camera_RotationX_Offset_CeilingGrab = -17;

    float minClampValue = 0;
    float maxClampValue = 1;

    float smoothRotationResult = 25;
    float transitionSpeed = 13;


    //--------------------


    void Update()
    {
        // Calculate default camera position in world space
        cameraDefault_Normal = transform.position + transform.TransformDirection(cameraController.cameraOffset_originalPos);
        cameraDefault_CeilingGrab = transform.position + transform.TransformDirection(cameraController.cameraOffset_ceilingGrabPos);

        // Raycast from player to camera target
        Vector3 direction;
        if (CameraController.Instance.cameraState == CameraState.CeilingCam || CameraController.Instance.isCeilingRotating)
            direction = cameraDefault_CeilingGrab - transform.position;
        else
            direction = cameraDefault_Normal - transform.position;

        float distance = direction.magnitude;
        direction.Normalize();
        Physics.Raycast(transform.position, direction, out hit, distance);

        // If hitting something - calculate camera position
        if (hit.collider != null && hit.collider.gameObject.GetComponent<BlockInfo>())
        {
            if (CameraController.Instance.cameraState == CameraState.CeilingCam || CameraController.Instance.isCeilingRotating)
            {
                
            }
            else
            {
                if (Movement.Instance.blockStandingOn != null && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>()
                && (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    cameraOffset.transform.position = Vector3.Lerp(cameraOffset.transform.position, hit.point - direction * 0.1f + Vector3.up * (1 - Mathf.Clamp(Vector3.Distance(transform.position, hit.point), minClampValue, maxClampValue)) * camera_StairHeight, transitionSpeed * Time.deltaTime);
                    cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(camera_RotationX_Offset_Normal - camera_RotationX_Offset_NormalOffset - Mathf.Pow(Vector3.Distance(cameraDefault_Normal, hit.point) / distance, 2) * smoothRotationResult, 0, 0), transitionSpeed * Time.deltaTime);
                }
                else
                {
                    cameraOffset.transform.position = Vector3.Lerp(cameraOffset.transform.position, hit.point - direction * 0.1f + Vector3.up * (1 - Mathf.Clamp(Vector3.Distance(transform.position, hit.point), minClampValue, maxClampValue)) * camera_NormalHeight, transitionSpeed * Time.deltaTime);
                    cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(camera_RotationX_Offset_Normal - camera_RotationX_Offset_NormalOffset - Mathf.Pow(Vector3.Distance(cameraDefault_Normal, hit.point) / distance, 2) * smoothRotationResult, 0, 0), transitionSpeed * Time.deltaTime);
                }
            }
        }

        // If not hitting anything - move to default position
        else
        {
            if (CameraController.Instance.cameraState == CameraState.CeilingCam || CameraController.Instance.isCeilingRotating)
            {
                //cameraOffset.transform.localPosition = Vector3.Lerp(cameraOffset.transform.localPosition, transform.InverseTransformPoint(cameraDefault_CeilingGrab), transitionSpeed * Time.deltaTime);
                //cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(camera_RotationX_Offset_CeilingGrab, 0, 0), transitionSpeed * Time.deltaTime);
            }
            else
            {
                cameraOffset.transform.localPosition = Vector3.Lerp(cameraOffset.transform.localPosition, transform.InverseTransformPoint(cameraDefault_Normal), transitionSpeed * Time.deltaTime);
                cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(camera_RotationX_Offset_Normal, 0, 0), transitionSpeed * Time.deltaTime);
            }
        }
    }
}