using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameState
{
    public PlayerControls PlayerControls { get; }

    // Start is called before the first frame update
    public GameState(SignalBus bus, PlayerControls pc)
    {
        bus.Subscribe<DeathSignal>(onPlayerDeath);
        bus.Subscribe<WinSignal>(onLevelWon);
        PlayerControls = pc;
    }

    private void onLevelWon()
    {
        PlayerControls.Disable();
    }

    private void onPlayerDeath()
    {
        PlayerControls.Disable();
    }

    public void OnRestart()
    {
        //TODO: Implement checkpoint system
        PlayerPrefs.SetInt("attemptCount", PlayerPrefs.GetInt("attemptCount", 0) + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void OnAboutPage()
    {
        throw new NotImplementedException();
    }

    public void OnQuit()
    {
        Application.Quit(0);
    }
}
