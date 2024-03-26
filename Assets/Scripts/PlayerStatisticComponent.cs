using UnityEngine;

public class PlayerStatisticComponent : MonoBehaviour
{
    protected int playerLevel;
    protected int currentEXP;
    protected int EXPtoNextLevel;
    protected const int START_EXP_VALUE = 500;

    public virtual void Awake() ///virtual makes override possible (if passing down classes)
    {
        playerLevel = PlayerPrefs.GetInt("PlayerLevel");
        if(playerLevel == 0) playerLevel = 1;
        currentEXP = PlayerPrefs.GetInt("CurrentEXP");
        UpdateEXPToNextLevel();
    }

    protected void UpdateEXPToNextLevel()
    {
        EXPtoNextLevel = START_EXP_VALUE * playerLevel;
    }
}
