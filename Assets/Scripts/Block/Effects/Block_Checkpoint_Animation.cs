using System.Collections;
using UnityEngine;

public class Block_Checkpoint_Animation : MonoBehaviour
{
    [Header("Wait Before Scaling")]
    [SerializeField] float scale_Wait = 0.07f;

    [Header("Scale Speed")]
    [SerializeField] float scale_Speed = 2.5f;

    [Header("Scale dimensions")]
    [SerializeField] bool scaleX;
    [SerializeField] bool scaleY;
    [SerializeField] bool scaleZ;

    [Header("Scale Size")]
    [SerializeField] float scale_Min = 1f;
    [SerializeField] float scale_Max = 1.4f;
    [SerializeField] float scale_Up_End = 1.2f;
    [SerializeField] float scale_ReturnSpeed = 1.5f;

    private Coroutine scaleCoroutine;
    private bool playerInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // 6 = Player
        {
            playerInside = true;
            ScaleUp();

            if (!Movement.Instance.isRespawning && GetComponent<AudioSource>())
                GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) // 6 = Player
        {
            playerInside = false;
            ScaleDown();
        }
    }

    void ScaleUp()
    {
        StartNewScaleCoroutine(ScaleUpRoutine());
    }

    void ScaleDown()
    {
        StartNewScaleCoroutine(ScaleDownRoutine());
    }

    void StartNewScaleCoroutine(IEnumerator routine)
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
            scaleCoroutine = null;
        }

        scaleCoroutine = StartCoroutine(routine);
    }

    IEnumerator ScaleUpRoutine()
    {
        float timer = 0f;

        while (timer < scale_Wait)
        {
            if (!playerInside)
            {
                scaleCoroutine = null;
                ScaleDown();
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!playerInside)
        {
            scaleCoroutine = null;
            ScaleDown();
            yield break;
        }

        Vector3 maxScale = GetTargetScale(scale_Max);

        while (Vector3.Distance(transform.localScale, maxScale) > 0.001f)
        {
            if (!playerInside)
            {
                scaleCoroutine = null;
                ScaleDown();
                yield break;
            }

            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                maxScale,
                scale_Speed * Time.deltaTime
            );

            yield return null;
        }

        transform.localScale = maxScale;

        if (!playerInside)
        {
            scaleCoroutine = null;
            ScaleDown();
            yield break;
        }

        Vector3 endScale = GetTargetScale(scale_Up_End);

        while (Vector3.Distance(transform.localScale, endScale) > 0.001f)
        {
            if (!playerInside)
            {
                scaleCoroutine = null;
                ScaleDown();
                yield break;
            }

            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                endScale,
                scale_ReturnSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.localScale = endScale;
        scaleCoroutine = null;
    }

    IEnumerator ScaleDownRoutine()
    {
        Vector3 minScale = GetTargetScale(scale_Min);

        while (Vector3.Distance(transform.localScale, minScale) > 0.001f)
        {
            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                minScale,
                scale_Speed * Time.deltaTime
            );

            yield return null;
        }

        transform.localScale = minScale;
        scaleCoroutine = null;
    }

    Vector3 GetTargetScale(float targetValue)
    {
        Vector3 targetScale = transform.localScale;

        if (scaleX) targetScale.x = targetValue;
        if (scaleY) targetScale.y = targetValue;
        if (scaleZ) targetScale.z = targetValue;

        return targetScale;
    }
}