using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sardinha.Events;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> roomSpawnPoints;
    private GameObject bossRoomSpawnPoint;

    // private void Awake()
    // {
    //     EventManager.Register<GameObject>(GeneralEvents.LevelControllerEvents.RequestNextRoom, SelectNextRoomListener);
    // }

    // private void Start()
    // {
    //     roomSpawnPoints = GameObject.FindGameObjectsWithTag("RoomSpawnPoint").ToList();
    //     GetBossRoomSpawnPoint();
    // }

    // private void GetBossRoomSpawnPoint()
    // {
    //     foreach (GameObject go in roomSpawnPoints)
    //     {
    //         if (go.transform.parent.tag == "BossRoom")
    //         {
    //             bossRoomSpawnPoint = go;
    //             roomSpawnPoints.Remove(go);
    //             break;
    //         }
    //     }
    // }

    // private GameObject SortNextRoom(GameObject curretRoomSpawnPoint)
    // {
    //     roomSpawnPoints.Remove(curretRoomSpawnPoint);
        
    //     if (roomSpawnPoints.Count == 0)
    //     {
    //         return bossRoomSpawnPoint;
    //     }

    //     GameObject selectedRoomSpawnPoint = roomSpawnPoints[Random.Range(0, roomSpawnPoints.Count)];
    //     roomSpawnPoints.Remove(selectedRoomSpawnPoint);

    //     return selectedRoomSpawnPoint;
    // }

    // private void SelectNextRoomListener(GameObject playerSpawnPoint)
    // {
    //     GameObject selectedSpawnPoint = SortNextRoom(playerSpawnPoint);
    //     EventManager.Trigger(GeneralEvents.LevelControllerEvents.NextRoomSorted, selectedSpawnPoint);
    // }

    // private void OnDestroy()
    // {
    //     EventManager.Unregister<GameObject>(GeneralEvents.LevelControllerEvents.RequestNextRoom, SelectNextRoomListener);
    // }
}
