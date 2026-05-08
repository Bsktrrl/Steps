using UnityEngine;

public class Block_RandomAssetRotation : MonoBehaviour
{
    private readonly int[] angles = { -90, 0, 90, 180 };

    private void Start()
    {
        //RotateRandomX();
        RotateRandomY();
        //RotateRandomZ();
    }

    public void RotateRandomX()
    {
        Vector3 currentEuler = transform.localEulerAngles;
        currentEuler.x = angles[Random.Range(0, angles.Length)];
        transform.localEulerAngles = currentEuler;
    }

    public void RotateRandomY()
    {
        Vector3 currentEuler = transform.localEulerAngles;
        currentEuler.y = angles[Random.Range(0, angles.Length)];
        transform.localEulerAngles = currentEuler;
    }

    public void RotateRandomZ()
    {
        Vector3 currentEuler = transform.localEulerAngles;
        currentEuler.z = angles[Random.Range(0, angles.Length)];
        transform.localEulerAngles = currentEuler;
    }
}