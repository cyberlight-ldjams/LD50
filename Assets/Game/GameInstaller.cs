using UnityEngine;
using Zenject;

using System.Reflection;
using System;
using System.Linq;

public class GameInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        InstallEvents();

        Container.Bind<GameState>().AsSingle();
        Container.Bind<PlayerControls>().FromInstance(new PlayerControls()).AsSingle();

    }


    // Add all structs that implement IEvent, through reflection.
    public void InstallEvents()
    {
        //Add Signal
        SignalBusInstaller.Install(Container);
        //Grab all the IEvents
        var events = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(IEvent).IsAssignableFrom(type) && !type.IsInterface);

        foreach (Type type in events)
        {
            Container.DeclareSignal(type);
        }
    }
}