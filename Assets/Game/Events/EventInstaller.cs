using UnityEngine;
using Zenject;

using System.Reflection;
using System;
using System.Linq;

public class EventInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //install signal bus into container
        /**
         * +SignalBusInstaller.Install(Container);
        Debug.Log("Here");
        //Grab all the IEvents
        var events = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(IEvent).IsAssignableFrom(type) && !type.IsInterface);


        foreach (Type type in events)
        {
            Container.DeclareSignal(type);
        }

        Debug.Log("Done");
        */
    }
}