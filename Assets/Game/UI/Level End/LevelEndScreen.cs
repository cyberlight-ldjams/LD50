using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.UI;

public class LevelEndScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title, body;

    [SerializeField]
    private GameObject StatsList, Prefab;

    [SerializeField]
    private Color winColor, loseColor;

    [SerializeField]
    private float transitionSpeed = 5f;

    [SerializeField]
    private Button retry, quit;

    [Inject]
    private GameState gameState;

    [Inject]
    private readonly SignalBus _bus;

    private Statistic[] _stats = null;

    private Image background;
    private CanvasGroup faderGroup;


    private AnimatorState state = AnimatorState.OFF;

    // Start is called before the first frame update
    void Start()
    {
        _bus.Subscribe<DeathSignal>(OnDeath);
        _bus.Subscribe<WinSignal>(OnWin);
        background = GetComponent<Image>();
        faderGroup = GetComponent<CanvasGroup>();

        retry.onClick.AddListener(() => gameState.OnRestart());
        quit.onClick.AddListener(() => gameState.OnQuit());

        if(Application.isEditor || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            quit.enabled = false;
        }
    }

    private void OnDeath(DeathSignal data)
    {
        title.text = "Uh Oh!";
        body.text = data.message;
        background.color = loseColor;

        _stats = new Statistic[] {
            new Statistic
            {
                Name = "Attempts:",
                Amount = PlayerPrefs.GetInt("attemptCount", 1) + "",
            },
            new Statistic
            {
                Amount = Time.realtimeSinceStartup + " seconds",
                Name = "Drown In:"
            }
        };

        ShowPanel(true);
    }

    private void GetStats()
    {

    }

    private void OnWin(WinSignal data)
    {
        title.text = "Congratulations";
        body.text = data.message;
        background.color = winColor;

        _stats = new Statistic[] {
            new Statistic
            {
                Name = "Attempts:",
                Amount = PlayerPrefs.GetInt("attemptCount", 1) +"",
            },
            new Statistic
            {
                Amount = Time.realtimeSinceStartup + " seconds",
                Name = "Completed In:"
            }
        };

        ShowPanel(true);
    }

  

    private void Update()
    {
        float movement = transitionSpeed * Time.deltaTime;
        switch (state){
            case AnimatorState.IN:
                faderGroup.alpha = Mathf.Clamp(faderGroup.alpha + movement, 0, 1);
                break;
            case AnimatorState.OUT:
                faderGroup.alpha = Mathf.Clamp(faderGroup.alpha - movement, 0, 1);
                break;
        }

        if(faderGroup.alpha >= 1f || faderGroup.alpha <= 0f)
        {
            state = AnimatorState.OFF;
        }


    }

    private void ShowPanel(bool show)
    {
        if (show)
        {
            //Todo: replace this with actual logic to grab relevant stats.
            //GetStats();

            foreach (Statistic statistic in _stats)
            {
                StatPuller sp = (GameObject.Instantiate(Prefab, StatsList.transform)).GetComponent<StatPuller>();
                sp.RefreshStats(statistic);
            }
        }
        else
        {
            HidePanel();
        }

        state = (show) ? AnimatorState.IN : AnimatorState.OUT;
    }

    private void HidePanel()
    {
        //Todo: replace this with actual logic to grab relevant stats.
        foreach (Transform statistic in GetComponentInChildren<Transform>())
        {
            Destroy(statistic.gameObject);
        }
    }

    public enum AnimatorState
    {
        IN, OFF, OUT
    }

    public struct Statistic
    {
        public string Name;
        public string Amount;
    }
}
