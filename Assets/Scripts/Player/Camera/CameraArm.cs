using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{
    [SerializeField] GameObject cameraOffset;
    Vector3 cameraDefault;
    RaycastHit hit;

    void Update()
    {
        cameraDefault = transform.position + transform.TransformDirection(new Vector3(0, 2.5f, -4.2f));

        Physics.Raycast(transform.position, cameraDefault - transform.position, out hit, Vector3.Distance(cameraDefault, transform.position));
        Debug.DrawRay(transform.position, cameraDefault - transform.position, Color.green);

        if(hit.collider != null)
        {
            cameraOffset.transform.position = Vector3.Lerp(cameraOffset.transform.position, hit.point + (Vector3.up * (1 - Mathf.Clamp(Vector3.Distance(transform.position, hit.point), 0, 1)) * 0.2f), 25 * Time.deltaTime);
            cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(29 - Mathf.Pow(Vector3.Distance(cameraDefault, hit.point) / Vector3.Distance(cameraDefault, transform.position), 2) * 25, 0, 0), 25 * Time.deltaTime);
        }
        else
        {
            cameraOffset.transform.localPosition = Vector3.Lerp(cameraOffset.transform.localPosition, transform.InverseTransformPoint(cameraDefault), 25 * Time.deltaTime);
            cameraOffset.transform.localEulerAngles = Vector3.Lerp(cameraOffset.transform.localEulerAngles, new Vector3(29, 0, 0), 25 * Time.deltaTime);
        }
    }
}