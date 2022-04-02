using System;
using Zenject;

public struct ExampleSignal : IEvent //Implement IEvent to have the signal automatically bound to Zenject.
{
    //This custom data can vary by the signal.
    public string property1;
    public int property2;
    public float someOtherProperty;
}

public class Signaler
{
    private readonly SignalBus _signalBus;

    /**
     * In this example, a single class is doing the work of both the firing and receiving of the event.
     * This is useless outside of example and would normally be split into multiple functions
     */
    public Signaler(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<ExampleSignal>(handlerExample);
    }

    private void handlerExample(ExampleSignal example)
    {
        if(example.property1 == "Hello, Jason!")
        {
            //Do something based on the custom data provided in the struct.
        }
    }

    //Example of firing a signal
    public void FireExample()
    {
        _signalBus.Fire(new ExampleSignal
        {
            property1 = "Hello, world",
            property2 = 42,
            someOtherProperty = 1.0f
        });
    }
}
