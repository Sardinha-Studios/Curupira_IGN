using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sardinha.Events;

public class TeleporterController : MonoBehaviour
{
    [SerializeField]
    private Teleporter levelDoor;

    [SerializeField]
    private Teleporter roomSpawnPoint;

    [SerializeField]
    private Room nextRoom;

    private bool isUnregister = false;

    private void Awake()
    {
        EventManager.Register<GameObject>(GeneralEvents.LevelControllerEvents.NextRoomSorted, SortedRoomListener);
    }

    public void OnRoomEndsListener()
    {
        levelDoor.gameObject.SetActive(true);
    }

    public void RequestNextLevel()
    {
        // EventManager.Trigger(GeneralEvents.LevelControllerEvents.RequestNextRoom, roomSpawnPoint.gameObject);
        SortedRoomListener(null);
    }

    private void SortedRoomListener(GameObject sortedSpawnPoint)
    {
        // Room targetRoom = sortedSpawnPoint.transform.parent.GetComponent<Room>();
        levelDoor.TargetRoom = nextRoom;
        // levelDoor.Destination = sortedSpawnPoint.GetComponent<Teleporter>();
    }

    private void OnDestroy()
    {
        if (!isUnregister)
            EventManager.Unregister<GameObject>(GeneralEvents.LevelControllerEvents.NextRoomSorted, SortedRoomListener);
    }

    public void UnregisterEvent()
    {
        if (!isUnregister)
        {
            EventManager.Unregister<GameObject>(GeneralEvents.LevelControllerEvents.NextRoomSorted, SortedRoomListener);
            isUnregister = true;
        }
    }
}
