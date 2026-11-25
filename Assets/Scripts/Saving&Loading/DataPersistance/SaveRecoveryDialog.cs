using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal sealed class SaveRecoveryDialog : MonoBehaviour
{
    private TextMeshProUGUI title;
    private TextMeshProUGUI message;
    private Button retryButton;
    private Button otherButton;
    private TextMeshProUGUI otherButtonLabel;
    private GameObject previousSelection;
    private float previousTimeScale;

    internal static SaveRecoveryDialog Create(Scene scene)
    {
        var root = new GameObject("Save Recovery Dialog", typeof(RectTransform));
        root.SetActive(false);
        if (scene != root.scene)
            SceneManager.MoveGameObjectToScene(root, scene);

        var dialog = root.AddComponent<SaveRecoveryDialog>();
        dialog.Build();
        return dialog;
    }

    internal void Show(string heading, string description, Action retry, string otherLabel = null, Action other = null)
    {
        title.text = heading;
        message.text = description;
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(() => retry());

        otherButton.onClick.RemoveAllListeners();
        otherButton.gameObject.SetActive(other != null);
        if (other != null)
        {
            otherButtonLabel.text = otherLabel;
            otherButton.onClick.AddListener(() => other());
        }

        retryButton.navigation = HorizontalNavigation(other != null ? otherButton : null);
        otherButton.navigation = HorizontalNavigation(retryButton);

        if (!gameObject.activeSelf)
        {
            EventSystem system = EventSystem.current;
            previousSelection = system ? system.currentSelectedGameObject : null;
            gameObject.SetActive(true);
        }

        FocusDialog();
    }

    internal void Dismiss(bool restoreSelection = false)
    {
        if (!gameObject.activeSelf)
            return;

        EventSystem system = EventSystem.current;
        if (system && OwnsSelection(system.currentSelectedGameObject))
        {
            GameObject selected = restoreSelection && previousSelection && previousSelection.activeInHierarchy
                ? previousSelection : null;
            system.SetSelectedGameObject(selected);
        }

        previousSelection = null;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    private void OnDisable() => Time.timeScale = previousTimeScale;

    // Startup UI can change the selection after this panel is first shown.
    private void LateUpdate() => FocusDialog();

    private void FocusDialog()
    {
        EventSystem system = EventSystem.current;
        if (system && system.isActiveAndEnabled && !OwnsSelection(system.currentSelectedGameObject))
            system.SetSelectedGameObject(retryButton.gameObject);
    }

    private bool OwnsSelection(GameObject selected) =>
        selected == retryButton.gameObject || (otherButton.gameObject.activeSelf && selected == otherButton.gameObject);

    private void Build()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;
        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        gameObject.AddComponent<GraphicRaycaster>();

        RectTransform backdrop = AddRect(transform, "Backdrop", Vector2.zero, Vector2.one);
        backdrop.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.8f);

        RectTransform panel = AddRect(backdrop, "Panel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        panel.sizeDelta = new Vector2(620f, 256f);
        panel.gameObject.AddComponent<Image>().color = new Color(0.12f, 0.09f, 0.17f);

        title = AddLabel(panel, "Title", new Vector2(0.06f, 0.71f), new Vector2(0.94f, 0.94f), 28f);
        title.fontStyle = FontStyles.Bold;
        message = AddLabel(panel, "Message", new Vector2(0.06f, 0.33f), new Vector2(0.94f, 0.72f), 22f);
        retryButton = AddButton(panel, "Retry", 0.06f, out TextMeshProUGUI retryLabel);
        retryLabel.text = "Retry";
        otherButton = AddButton(panel, "Alternative", 0.31f, out otherButtonLabel);
    }

    private static Button AddButton(Transform parent, string name, float left, out TextMeshProUGUI label)
    {
        RectTransform rect = AddRect(parent, name, new Vector2(left, 0.09f), new Vector2(left + 0.22f, 0.28f));
        Image image = rect.gameObject.AddComponent<Image>();
        var button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.29f, 0.20f, 0.41f);
        colors.highlightedColor = new Color(0.48f, 0.31f, 0.65f);
        colors.selectedColor = colors.highlightedColor;
        colors.pressedColor = new Color(0.38f, 0.24f, 0.52f);
        button.colors = colors;

        label = AddLabel(rect, "Label", Vector2.zero, Vector2.one, 22f);
        label.alignment = TextAlignmentOptions.Center;
        return button;
    }

    private static Navigation HorizontalNavigation(Button other) => new Navigation
    {
        mode = Navigation.Mode.Explicit,
        selectOnLeft = other,
        selectOnRight = other
    };

    private static TextMeshProUGUI AddLabel(Transform parent, string name, Vector2 min, Vector2 max, float size)
    {
        var label = AddRect(parent, name, min, max).gameObject.AddComponent<TextMeshProUGUI>();
        label.fontSize = size;
        label.color = Color.white;
        label.alignment = TextAlignmentOptions.MidlineLeft;
        label.raycastTarget = false;
        return label;
    }

    private static RectTransform AddRect(Transform parent, string name, Vector2 min, Vector2 max)
    {
        var rect = (RectTransform)new GameObject(name, typeof(RectTransform)).transform;
        rect.SetParent(parent, false);
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return rect;
    }
}
