using UnityEngine;

public class FixedAspectRatio : MonoBehaviour
{
    public Vector2 targetResolution = new Vector2(1920, 1080);

    [SerializeField]
    Camera camera;

    void Update()
    {
        if (camera == null)
        {
            camera = GetComponent<Camera>();
        }
        
        if (camera)
        {
            float targetAspect = targetResolution.x / targetResolution.y;
            float windowAspect = (float)Screen.width / Screen.height;

            float scaleHeight = windowAspect / targetAspect;

            if (scaleHeight < 1.0f)
            {
                Rect rect = camera.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;

                camera.rect = rect;
            }
            else
            {
                float scaleWidth = 1.0f / scaleHeight;

                Rect rect = camera.rect;

                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                camera.rect = rect;
            }
        }
    }
}
