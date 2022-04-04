using UnityEngine;

public struct LadderInteractionSignal : IEvent
{
    public GameObject ladder;

    // Position relative to the ladder where the player starts to climb
    public Vector3 ladderRailBottom;

    // Position relative to the ladder where the player ends the climb
    public Vector3 ladderRailTop;

    // Position at the top of the ladder where the player gets off
    public Vector3 ladderFinish;

    // Position where the ladder climbing animation should end
    public Vector3 getOffLadder;
}
