using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldMenu : MonoBehaviour
{
    public static event Action OverworldButton_isPressed;

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


    //--------------------


    public void Biome0_isPressed()
    {
        MainMenuManager.Instance.menuState = MenuState.Biome_0;
        OverworldButton_isPressed?.Invoke();
    }
    public void Biome1_isPressed()
    {
        MainMenuManager.Instance.menuState = MenuState.Biome_1;
        OverworldButton_isPressed?.Invoke();
    }
    public void Biome2_isPressed()
    {
        MainMenuManager.Instance.menuState = MenuState.Biome_2;
        OverworldButton_isPressed?.Invoke();
    }
    public void Biome3_isPressed()
    {
        MainMenuManager.Instance.menuState = MenuState.Biome_3;
        OverworldButton_isPressed?.Invoke();
    }
    public void Biome4_isPressed()
    {
        MainMenuManager.Instance.menuState = MenuState.Biome_4;
        OverworldButton_isPressed?.Invoke();
    }
    public void Biome5_isPressed()
    {
        MainMenuManager.Instance.menuState = MenuState.Biome_5;
        OverworldButton_isPressed?.Invoke();
    }
    public void Biome6_isPressed()
    {
        MainMenuManager.Instance.menuState = MenuState.Biome_6;
        OverworldButton_isPressed?.Invoke();
    }
    public void Biome7_isPressed()
    {
        MainMenuManager.Instance.menuState = MenuState.Biome_7;
        OverworldButton_isPressed?.Invoke();
    }
    public void Biome8_isPressed()
    {
        MainMenuManager.Instance.menuState = MenuState.Biome_8;
        OverworldButton_isPressed?.Invoke();
    }
}