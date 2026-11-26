using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletMenuManager : MonoBehaviour
{
    public TextMeshProUGUI tabletDisplayText;
    public List<GameObject> tabletButtonList = new List<GameObject>();

    [SerializeField] TextAsset tabletTextDocument;
    [SerializeField] GridLayoutGroup tabletGrid;
    [SerializeField] Button tabletTab;
    [SerializeField, Range(0f, 1f)] float unreadAlpha = 0.5f;

    readonly HashSet<int> readEntries = new HashSet<int>();
    TabletButton[] tabletButtons;
    TabletData tabletData;
    DataManager data;
    TabletButton focusedTablet;
    TabletButton previewTablet;

    private void Awake()
    {
        tabletButtons = new TabletButton[tabletButtonList.Count];
        for (int i = 0; i < tabletButtonList.Count; i++)
        {
            GameObject slot = tabletButtonList[i];
            if (!slot)
                continue;

            TabletButton tablet = slot.GetComponentInChildren<TabletButton>(true);
            if (!tablet)
                continue;

            tablet.Bind(this, i, tabletTab);
            tabletButtons[i] = tablet;
        }

        SetGridNavigation();
    }

    private void OnEnable()
    {
        tabletData ??= TabletManager.ReadTabletData(tabletTextDocument);
        DataManager.Action_dataHasLoaded += RefreshReadButtons;
        SettingsManager.Action_SetNewLanguage += RefreshText;
        ActionButtonsManager.MouseHoverUpdated += UpdateMousePreview;
        RefreshReadButtons();
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= RefreshReadButtons;
        SettingsManager.Action_SetNewLanguage -= RefreshText;
        ActionButtonsManager.MouseHoverUpdated -= UpdateMousePreview;
        focusedTablet = null;
        previewTablet = null;
    }

    private void RefreshReadButtons()
    {
        data = FindAnyObjectByType<DataManager>();
        readEntries.Clear();
        var history = data ? data.playerStatistics_Store?.tabletsRead : null;
        if (history != null)
        {
            foreach (string key in history)
            {
                // The save key stores the tablet's text index between scene and position.
                int start = key?.IndexOf('/') ?? -1;
                if (start <= 0)
                    continue;

                int end = key.IndexOf('/', start + 1);
                if (end > start + 1 &&
                    int.TryParse(key.AsSpan(start + 1, end - start - 1), NumberStyles.None,
                        CultureInfo.InvariantCulture, out int entry))
                    readEntries.Add(entry);
            }
        }

        foreach (TabletButton tablet in tabletButtons)
        {
            if (tablet)
                tablet.SetRead(readEntries.Contains(tablet.TextEntry), unreadAlpha);
        }

        foreach (TabletButton tablet in tabletButtons)
            UpdateHighlight(tablet);

        RefreshText();
    }

    internal void SelectTablet(TabletButton tablet)
    {
        focusedTablet = tablet;
        UpdateHighlight(tablet);
        SetPreview(tablet);
    }

    internal void DeselectTablet(TabletButton tablet)
    {
        if (focusedTablet == tablet)
            focusedTablet = null;
        if (previewTablet == tablet)
            SetPreview(null);
        UpdateHighlight(tablet);
    }

    private void UpdateMousePreview(Button button)
    {
        SetPreview(button && button.TryGetComponent(out TabletButton tablet) &&
            tablet.transform.IsChildOf(transform) ? tablet : null);
    }

    private void SetPreview(TabletButton tablet)
    {
        if (previewTablet == tablet)
            return;

        previewTablet = tablet;
        RefreshText();
    }

    internal void PressTablet(TabletButton tablet)
    {
        if (focusedTablet == tablet)
            SetPreview(tablet);
    }

    internal void CancelTabletSelection()
    {
        SetPreview(null);
    }

    private void UpdateHighlight(TabletButton tablet)
    {
        if (tablet)
            tablet.SetHighlighted(tablet == focusedTablet);
    }

    private void RefreshText()
    {
        if (!tabletDisplayText)
            return;

        TabletButton tablet = previewTablet;
        int entry = tablet && tablet.IsRead ? tablet.TextEntry : -1;
        if (entry < 0 || entry >= tabletData.tabletDataSegment.Count)
        {
            tabletDisplayText.text = string.Empty;
            return;
        }

        var translations = tabletData.tabletDataSegment[entry].languageDialogueList;
        int english = (int)Languages.English;
        int language = data && data.settingData_StoreList != null
            ? (int)data.settingData_StoreList.currentLanguage
            : english;
        if (language < 0 || language >= translations.Count || string.IsNullOrEmpty(translations[language]))
            language = english;

        tabletDisplayText.text = translations[language];
    }

    private void SetGridNavigation()
    {
        if (!tabletGrid || tabletGrid.transform.childCount == 0)
            return;

        Transform gridParent = tabletGrid.transform;
        int columns = tabletGrid.constraintCount;
        int rows = (gridParent.childCount + columns - 1) / columns;
        var grid = new Button[rows, columns];

        // The list identifies tablet text. Sibling order includes the blank cells in the layout.
        for (int i = 0; i < tabletButtons.Length; i++)
        {
            if (!tabletButtons[i] || tabletButtonList[i].transform.parent != gridParent)
                continue;

            int cell = tabletButtonList[i].transform.GetSiblingIndex();
            grid[cell / columns, cell % columns] = tabletButtons[i].SelectionButton;
        }

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Button button = grid[row, column];
                if (!button)
                    continue;

                var navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnLeft = FindNeighbor(grid, row, column, 0, -1),
                    selectOnRight = FindNeighbor(grid, row, column, 0, 1),
                    selectOnUp = row == 0 ? tabletTab : FindNeighbor(grid, row, column, -1, 0),
                    selectOnDown = FindNeighbor(grid, row, column, 1, 0)
                };
                button.navigation = navigation;
            }
        }

        if (tabletTab)
        {
            Navigation navigation = tabletTab.navigation;
            navigation.selectOnDown = grid[0, 0];
            tabletTab.navigation = navigation;
        }
    }

    private static Button FindNeighbor(Button[,] grid, int row, int column, int rowDirection, int columnDirection)
    {
        int rows = grid.GetLength(0);
        int columns = grid.GetLength(1);
        int length = rowDirection == 0 ? columns : rows;
        for (int distance = 1; distance < length; distance++)
        {
            int nextRow = (row + rowDirection * distance + rows) % rows;
            int nextColumn = (column + columnDirection * distance + columns) % columns;
            Button neighbor = grid[nextRow, nextColumn];
            if (neighbor)
                return neighbor;
        }

        return null;
    }
}
