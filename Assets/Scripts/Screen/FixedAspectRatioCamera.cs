using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatioCamera : MonoBehaviour
{
    public Vector2 targetAspectRatio = new Vector2(16, 9); // or 1920x1080

    void Update()
    {
        float targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            Rect rect = new Rect(0, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = new Rect((1.0f - scaleWidth) / 2.0f, 0, scaleWidth, 1.0f);
            camera.rect = rect;
        }
    }
}
