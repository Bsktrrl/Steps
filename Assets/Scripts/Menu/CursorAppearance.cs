using UnityEngine;

[CreateAssetMenu(menuName = "Glueplant/Cursor Appearance")]
public sealed class CursorAppearance : ScriptableObject
{
    public Texture2D normal;
    public Texture2D pressed;
    [Tooltip("Click point in pixels from the top-left corner of the imported texture.")]
    public Vector2 hotspot;
    [Range(8, 128), Tooltip("Longest side in pixels at 1080p. Scales with the game window height.")]
    public int sizeAt1080p = 32;
    [Range(-45f, 45f), Tooltip("Rotation around the click point while pressed. Positive angles tilt clockwise.")]
    public float pressedAngle = 8f;
}
