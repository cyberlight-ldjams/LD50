using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class LevelEndScreen : MonoBehaviour
{
    [Inject]
    private readonly SignalBus _bus;

    [SerializeField]
    private TextMeshProUGUI title, body;

    [SerializeField]
    private GameObject StatsList, Prefab;

    private Statistic[] stats;
    private CanvasGroup faderGroup;

    // Start is called before the first frame update
    void Start()
    {
        _bus.Subscribe<DeathSignal>(OnDeath);
        _bus.Subscribe<WinSignal>(OnWin);
        faderGroup.GetComponent<CanvasGroup>();
    }

    private void OnDeath(DeathSignal data)
    {
        throw new NotImplementedException();
    }

    private void OnWin(WinSignal data)
    {
        throw new NotImplementedException();
    }

    private void ShowPanel()
    {
        //Todo: replace this with actual logic to grab relevant stats.
        foreach (Statistic statistic in stats)
        {
            StatPuller sp = (GameObject.Instantiate(Prefab, StatsList.transform)).GetComponent<StatPuller>();
        }
    }

    public struct Statistic
    {
        public string Name;
        public string Amount;
    }
}
