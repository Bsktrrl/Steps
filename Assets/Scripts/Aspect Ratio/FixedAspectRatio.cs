using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatio : MonoBehaviour
{
    public float targetAspect = 16f / 9f;

    Camera cam;
    int lastWidth;
    int lastHeight;

    void Awake()
    {
        cam = GetComponent<Camera>();
        Apply();
        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            Apply();
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }
    }

    void Apply()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1f)
        {
            // Letterbox (top/bottom)
            cam.rect = new Rect(
                0,
                (1f - scaleHeight) * 0.5f,
                1f,
                scaleHeight
            );
        }
        else
        {
            // Pillarbox (left/right)
            float scaleWidth = 1f / scaleHeight;

            cam.rect = new Rect(
                (1f - scaleWidth) * 0.5f,
                0,
                scaleWidth,
                1f
            );
        }
    }
}
