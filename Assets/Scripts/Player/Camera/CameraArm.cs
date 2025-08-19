using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject cameraOffset;
    Vector3 cameraDefault;
    RaycastHit hit;

    float camera_NormalHeight = 0.2f;
    float camera_StairHeight = 0.7f;
    float camera_RotationX_Offset = 29;

    float minClampValue = 0;
    float maxClampValue = 1;

    float smoothRotationResult = 25;
    float transitionSpeed = 25;

    
    //--------------------


    void Update()
    {
        cameraDefault = transform.position + transform.TransformDirection(cameraController.cameraOffset_originalPos);

        Physics.Raycast(transform.position, cameraDefault - transform.position, out hit, Vector3.Distance(cameraDefault, transform.position));

        //If hitting something - Calculated camera angles
        if (hit.collider != null && hit.collider.gameObject.GetComponent<BlockInfo>())
        {
            if (CameraController.Instance.cameraState == CameraState.CeilingCam || CameraController.Instance.isRotating || CameraController.Instance.isCeilingRotating)
            {

            }
            else
            {
                if (Movement.Instance.blockStandingOn != null && Movement.Instance.blockStandingOn.GetComponent<BlockInfo>() && (Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Stair || Movement.Instance.blockStandingOn.GetComponent<BlockInfo>().blockType == BlockType.Slope))
                {
                    cameraOffset.transform.position = Vector3.Lerp(cameraOffset.transform.position, hit.point + (Vector3.up * (1 - Mathf.Clamp(Vector3.Distance(transform.position, hit.point), minClampValue, maxClampValue)) * camera_StairHeight), smoothRotationResult * Time.deltaTime);
                    cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(camera_RotationX_Offset - Mathf.Pow(Vector3.Distance(cameraDefault, hit.point) / Vector3.Distance(cameraDefault, transform.position), 2) * 25, 0, 0), transitionSpeed * Time.deltaTime);
                }
                else
                {
                    cameraOffset.transform.position = Vector3.Lerp(cameraOffset.transform.position, hit.point + (Vector3.up * (1 - Mathf.Clamp(Vector3.Distance(transform.position, hit.point), minClampValue, maxClampValue)) * camera_NormalHeight), smoothRotationResult * Time.deltaTime);
                    cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(camera_RotationX_Offset - Mathf.Pow(Vector3.Distance(cameraDefault, hit.point) / Vector3.Distance(cameraDefault, transform.position), 2) * 25, 0, 0), transitionSpeed * Time.deltaTime);
                }
            }
        }

        //If not hitting - Default camera angles
        else
        {
            if (CameraController.Instance.cameraState == CameraState.CeilingCam || CameraController.Instance.isRotating || CameraController.Instance.isCeilingRotating)
            {
                
            }
            else
            {
                cameraOffset.transform.localPosition = Vector3.Lerp(cameraOffset.transform.localPosition, transform.InverseTransformPoint(cameraDefault), transitionSpeed * Time.deltaTime);
                cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(camera_RotationX_Offset, 0, 0), transitionSpeed * Time.deltaTime);
            }
        }
    }
}