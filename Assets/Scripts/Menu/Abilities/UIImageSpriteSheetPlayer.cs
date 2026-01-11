using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent]
public class UIImageSpriteSheetPlayer : MonoBehaviour
{
    [Header("Assign these")]
    [SerializeField] private Image targetImage;
    [Tooltip("Assign ANY sliced sprite from the sprite sheet")]
    [SerializeField] private Sprite spriteSheetReference;

    [Header("Playback")]
    [SerializeField] private float fps = 30f;
    [SerializeField] private bool loop = true;

    private Sprite[] frames;
    private Coroutine playRoutine;

    private void Reset()
    {
        targetImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (targetImage == null || spriteSheetReference == null)
            return;

        LoadFramesFromSpriteSheet();
        StartPlayback();
    }

    private void OnDisable()
    {
        StopPlayback();
    }

    // -------------------- Frame loading --------------------

    private void LoadFramesFromSpriteSheet()
    {
        if (frames != null && frames.Length > 0)
            return;

#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(spriteSheetReference.texture);
        var allAssets = AssetDatabase.LoadAllAssetsAtPath(path);

        var list = new System.Collections.Generic.List<Sprite>();
        foreach (var a in allAssets)
        {
            if (a is Sprite s)
                list.Add(s);
        }

        list.Sort(CompareSpriteNamesNaturally);
        frames = list.ToArray();
#else
        Debug.LogError(
            $"{name}: Sprite sheet auto-loading without Resources is Editor-only.\n" +
            "For runtime builds, either:\n" +
            "- Use Resources\n" +
            "- Assign frames manually\n" +
            "- Use Addressables / SpriteAtlas",
            this
        );
#endif
    }

    // -------------------- Playback --------------------

    private void StartPlayback()
    {
        StopPlayback();
        playRoutine = StartCoroutine(PlayLoop());
    }

    private void StopPlayback()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }
    }

    private IEnumerator PlayLoop()
    {
        if (frames == null || frames.Length == 0)
            yield break;

        int index = 0;
        float frameTime = 1f / Mathf.Max(1f, fps);

        while (true)
        {
            targetImage.sprite = frames[index];

            index++;
            if (index >= frames.Length)
            {
                if (!loop)
                {
                    targetImage.sprite = frames[frames.Length - 1];
                    yield break;
                }
                index = 0;
            }

            yield return new WaitForSeconds(frameTime);
        }
    }

    // -------------------- Sorting --------------------

    private static int CompareSpriteNamesNaturally(Sprite a, Sprite b)
    {
        int ai = ExtractTrailingNumber(a.name, out bool aHas);
        int bi = ExtractTrailingNumber(b.name, out bool bHas);

        if (aHas && bHas)
            return ai.CompareTo(bi);

        return string.Compare(a.name, b.name, StringComparison.Ordinal);
    }

    private static int ExtractTrailingNumber(string name, out bool found)
    {
        found = false;
        int i = name.Length - 1;
        while (i >= 0 && char.IsDigit(name[i])) i--;

        if (i == name.Length - 1)
            return 0;

        found = true;
        return int.TryParse(name.Substring(i + 1), out int v) ? v : 0;
    }
}
