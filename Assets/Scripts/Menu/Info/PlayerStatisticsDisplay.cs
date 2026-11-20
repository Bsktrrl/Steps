using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class PlayerStatisticsDisplay : MonoBehaviour
{
    public TextMeshProUGUI stats_LeftSide;
    public TextMeshProUGUI stats_RightSide;

    private void OnEnable()
    {
        DataManager.Action_dataHasLoaded += Refresh;
        SettingsManager.Action_SetNewLanguage += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        DataManager.Action_dataHasLoaded -= Refresh;
        SettingsManager.Action_SetNewLanguage -= Refresh;
    }

    private void Refresh()
    {
        SetupStatsDisplayLeft();
        SetupStatsDisplayRight();
    }

    public void SetupStatsDisplayLeft()
    {
        if (!stats_LeftSide || !TryGetDisplayData(out var stats, out var text, out var culture))
            return;

        // Values follow the order of the left column in GameTextDatabase.
        stats_LeftSide.text = BuildColumn(text.options_Info_StatsLeft_Text, new[]
        {
            stats.progress.ToString("F0", culture) + "%",
            FormatDuration(stats.playTime),
            stats.sessions.ToString(culture),
            FormatDuration(stats.longestSession),
            stats.stepsTaken.ToString(culture),
            stats.stepsReplenished.ToString(culture),
            stats.stepsEmptied.ToString(culture),
            stats.averageStepsPerLevel.ToString("F0", culture),
            stats.pickup_EssencePickedUp.ToString(culture),
            stats.pickup_SkinsFound.ToString(culture),
            stats.pickup_SkinsBought.ToString(culture),
            stats.pickup_TabletsRed.ToString(culture),
            stats.pickup_DialoguesInterracted.ToString(culture)
        });
    }

    public void SetupStatsDisplayRight()
    {
        if (!stats_RightSide || !TryGetDisplayData(out var stats, out var text, out var culture))
            return;

        // Values follow the order of the right column in GameTextDatabase.
        stats_RightSide.text = BuildColumn(text.options_Info_StatsRight_Text, new[]
        {
            stats.regionsCompleted.ToString(culture),
            stats.levelsCompleted.ToString(culture),
            stats.levelsReplayed.ToString(culture),
            FormatDuration(stats.fastestLevelCompletion_Time),
            FormatDuration(stats.slowestLevelCompletion_Time),
            FormatDuration(stats.averageLevelCompletionTime),
            stats.respawnsTaken.ToString(culture),
            FormatDuration(stats.timeStandingStill),
            FormatDuration(stats.TimeInFreeCam),
            stats.hintsUsed.ToString(culture),
            stats.abilitiesPerformed.ToString(culture),
            stats.ability_Jump_Performed.ToString(culture),
            stats.ability_GrapplingHook_Performed.ToString(culture),
            stats.ability_HandDrill_Performed.ToString(culture),
            stats.ability_DrillHelmet_Performed.ToString(culture),
            stats.ability_DrillBoots_Performed.ToString(culture),
            stats.ability_SwimDistance_Performed.ToString(culture),
            stats.ability_CeilingClimbDisatnace_Performed.ToString(culture)
        });
    }

    private static bool TryGetDisplayData(
        out PlayerStatistics stats, out Game_TextDatabase_Language text, out CultureInfo culture)
    {
        stats = null;
        text = null;
        culture = CultureInfo.InvariantCulture;

        var data = FindAnyObjectByType<DataManager>();
        if (!data || data.settingData_StoreList == null)
            return false;

        var localizedTexts = data.game_TextDatabase_Store?.gameText_LanguageList;
        int languageIndex = (int)data.settingData_StoreList.currentLanguage;
        if (localizedTexts == null || languageIndex < 0 || languageIndex >= localizedTexts.Count)
            return false;

        stats = data.playerStatistics_Store;
        text = localizedTexts[languageIndex];
        if (languageIndex == (int)Languages.Norwegian)
            culture = CultureInfo.GetCultureInfo("nb-NO");

        return stats != null && text != null;
    }

    private static string BuildColumn(string template, string[] values)
    {
        if (string.IsNullOrEmpty(template))
            return string.Empty;

        var lines = template.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        int valueIndex = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            var label = lines[i].TrimEnd();
            if (!label.EndsWith(":"))
                continue;

            if (valueIndex == values.Length)
                return template;

            lines[i] = label + " " + values[valueIndex++];
        }

        return valueIndex == values.Length ? string.Join("\n", lines) : template;
    }

    private static string FormatDuration(float seconds)
    {
        long totalMinutes = (long)(seconds / 60d);
        return FormattableString.Invariant($"{totalMinutes / 60}:{totalMinutes % 60:D0}");
    }
}
