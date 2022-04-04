using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPuller : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI statName, statValue;

    public void RefreshStats(LevelEndScreen.Statistic stat)
    {
        statName.text = stat.Name;
        statValue.text = stat.Amount;
    }
}
