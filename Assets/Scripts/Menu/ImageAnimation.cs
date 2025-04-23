using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] bool onlyWhenSelected;
    [SerializeField] Vector2 waitTimerRange;
    float waitTimer;

    [Header("Image Source")]
    [SerializeField] Image waveImage;

    [Header("Sprites")]
    [SerializeField] List<Sprite> waveSpriteList;

    int imageNumber;


    //--------------------


    private void OnEnable()
    {
        waitTimer = Random.Range(waitTimerRange.x, waitTimerRange.y);

        Animation();
    }
    private void OnDisable()
    {
        StopCoroutine(changeImage(0));
    }


    //--------------------


    void Animation()
    {
        StartCoroutine(changeImage(waitTimer));
    }
    IEnumerator changeImage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (onlyWhenSelected)
        {
            if (gameObject == RememberCurrentlySelectedUIElement.Instance.currentSelectedUIElement)
            {
                PerformAnimation();

                Animation();
            }
            else
            {
                Animation();
            }
        }
        else
        {
            PerformAnimation();

            Animation();
        }
    }

    void PerformAnimation()
    {
        imageNumber += 1;

        if (imageNumber >= waveSpriteList.Count)
        {
            imageNumber = 0;
        }

        waveImage.sprite = waveSpriteList[imageNumber];

        waitTimer = Random.Range(waitTimerRange.x, waitTimerRange.y);
    }
}
