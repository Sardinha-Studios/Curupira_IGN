using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralEvents
{
    public enum LevelControllerEvents
    {
        OnLevelEnd,
        OnWaveEnds,
        RequestNextRoom,
        NextRoomSorted
    }

    public enum PowerUpEvents
    {
        OnVelocityUp, OnDashUp
    }
}
