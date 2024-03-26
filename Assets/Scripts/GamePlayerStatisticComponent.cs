using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamePlayerStatisticComponent : PlayerStatisticComponent
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text currentEXP_Text;
    [SerializeField] Image fillEXP_Area;
    private const int LOSE_COEFFICIENT = 1;
    private const int WIN_COEFFICIENT =3;
    private const int STANDARD_EXP_VALUE = 50;

    private void UpdateText()
    {
        levelText.text = $"Level: {playerLevel}";
        currentEXP_Text.text = $"{currentEXP_Text} / {EXPtoNextLevel}";
        fillEXP_Area.fillAmount = (float)currentEXP / (float)EXPtoNextLevel;
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("PlayerLevel",playerLevel);
        PlayerPrefs.SetInt("CurrentEXP",currentEXP);
    }

    public void ShowWinInfo()
    {
        ///emphasising which operation to do first saves time
        int value = (playerLevel * STANDARD_EXP_VALUE) * WIN_COEFFICIENT;
        if(currentEXP + value > EXPtoNextLevel)
        {
            int bufferValue = (currentEXP + value) - EXPtoNextLevel;
            playerLevel++;
            currentEXP = bufferValue;
            UpdateEXPToNextLevel();
        }
        else
        {
            currentEXP += value;
        }
        titleText.text = "You win!";
        UpdateText();
        SaveData();
    }

    public void ShowLoseInfo()
    {
        int value = (playerLevel * STANDARD_EXP_VALUE) * LOSE_COEFFICIENT;
        if(currentEXP + value > EXPtoNextLevel)
        {
            int bufferValue = (currentEXP + value) - EXPtoNextLevel;
            playerLevel++;
            currentEXP = bufferValue;
            UpdateEXPToNextLevel();
        }
        else
        {
            currentEXP += value;
        }
        titleText.text = "You lost.";
        UpdateText();
        SaveData();
    }
}
