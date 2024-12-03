using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldMenu : MonoBehaviour
{
    [SerializeField] Image waveImage;

    [SerializeField] List<Sprite> waveSpriteList;
    int imageNumber;

    float waitTimer = 1;

    //--------------------


    private void OnEnable()
    {
        ImageAnimation();
    }
    private void OnDisable()
    {
        StopCoroutine(changeImage(0));
    }


    //--------------------


    void ImageAnimation()
    {
        StartCoroutine(changeImage(waitTimer));
    }
    IEnumerator changeImage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        imageNumber += 1;

        if (imageNumber >= waveSpriteList.Count)
        {
            imageNumber = 0;
        }

        waveImage.sprite = waveSpriteList[imageNumber];

        waitTimer = UnityEngine.Random.Range(0.5f, 1f);

        ImageAnimation();
    }
}