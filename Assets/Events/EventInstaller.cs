using UnityEngine;
using Zenject;

using System.Reflection;
using System;
using System.Linq;

public class EventInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject player;

    public override void InstallBindings()
    {
        //install signal bus into container

        SignalBusInstaller.Install(Container);
        //Grab all the IEvents
        var events = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(IEvent).IsAssignableFrom(type) && !type.IsInterface);

        foreach (Type type in events)
        {
            Container.DeclareSignal(type);
        }

        Container.Bind<PlayerMovement>().FromComponentInNewPrefab(player).AsSingle().NonLazy();
    }
}