using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_LevelDoor;
    public void OnRoomEndsListener()
    {
        m_LevelDoor.SetActive(true);
    }
}
