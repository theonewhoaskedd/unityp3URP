using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPlayerStatisticComponent : PlayerStatisticComponent
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentEXP_Text;
    [SerializeField] private Image fillEXP_Area;
    
    void Start()
    {
        levelText.text = $"Level: {playerLevel}";
        currentEXP_Text.text = $"{currentEXP_Text} / {EXPtoNextLevel}";
        fillEXP_Area.fillAmount = (float)currentEXP / (float)EXPtoNextLevel;
    }
}
