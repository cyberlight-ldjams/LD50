using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameState
{

    // Start is called before the first frame update
    public GameState(SignalBus bus)
    {
        bus.Subscribe<DeathSignal>(onPlayerDeath);
        bus.Subscribe<WinSignal>(onLevelWon);
    }

    private void onLevelWon()
    {
        throw new NotImplementedException();
    }

    private void onPlayerDeath()
    {
        throw new NotImplementedException();
    }

    public void OnRestart()
    {
        //TODO: Implement checkpoint system
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
