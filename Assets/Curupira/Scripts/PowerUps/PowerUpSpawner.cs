using Sardinha.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PowerUpSpawner : MonoBehaviour
{

    [SerializeField]
    private WaveManager waveManager;
    [SerializeField]
    private GameObject[] powerUps;

    private GameObject spawnedPoweUp;

    public UnityEvent onLevelEnd;

    public void SpawnPowerUp()
    {
        spawnedPoweUp  = Instantiate(powerUps[Random.Range(0, powerUps.Length - 1)], transform.position, Quaternion.identity, transform);

        spawnedPoweUp.SetActive(true);
    }

    public void CallOnLevelEnd()
    {
        onLevelEnd?.Invoke();
    }
}
