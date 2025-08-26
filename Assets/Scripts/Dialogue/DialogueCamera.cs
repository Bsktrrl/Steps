using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraMover : MonoBehaviour
{
    [Header("Camera Setup")]
    public CinemachineCamera virtualCamera;

    [Header("Scene Targets")]
    public Transform playerTransform; // The object the camera should look at
    public Transform npcPoint;        // The position the camera should move to

    [Header("Movement Settings")]
    public float moveDuration = 2f;

    private Coroutine moveCoroutine;

    private void Start()
    {
        if (virtualCamera != null && playerTransform != null)
        {
            virtualCamera.LookAt = playerTransform;
        }
    }

    public void MoveCameraToNPC()
    {
        if (virtualCamera == null || npcPoint == null)
        {
            Debug.LogWarning("VirtualCamera or NPC Point is not assigned.");
            return;
        }

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(SmoothMoveCamera(virtualCamera.transform, npcPoint.position));
    }

    private IEnumerator SmoothMoveCamera(Transform cameraTransform, Vector3 targetPosition)
    {
        Vector3 startPos = cameraTransform.position;
        Quaternion startRot = cameraTransform.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / moveDuration);

            cameraTransform.position = Vector3.Lerp(startPos, targetPosition, t);

            // Optional: keep camera rotation towards player during move
            Vector3 lookDirection = playerTransform.position - cameraTransform.position;
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDirection.normalized);
                cameraTransform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            }

            yield return null;
        }

        cameraTransform.position = targetPosition;

        // Final rotation to face the player exactly
        Vector3 finalLookDirection = playerTransform.position - cameraTransform.position;
        if (finalLookDirection != Vector3.zero)
        {
            cameraTransform.rotation = Quaternion.LookRotation(finalLookDirection.normalized);
        }
    }
}