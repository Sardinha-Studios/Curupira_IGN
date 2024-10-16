using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [SerializeField]
    private WaveDetail[] waveDetail;

    [SerializeField]
    private GameObject [] spawnPoints;

    private int currentWave = 0;
    private int maxWaveNumbers;
    
    // Start is called before the first frame update
    void Start()
    {
        maxWaveNumbers = waveDetail.Length - 1;
        SpawnwWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 SelectSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;
    }

    private GameObject SelectEnemyToSpawn()
    {
        GameObject[] waveEnemies = waveDetail[currentWave].waveEnemies;
        return waveEnemies[Random.Range(0, waveEnemies.Length - 1)];
    }

    private void SpawnwWave()
    {
        for (int i = 0; i < waveDetail[currentWave].enemieQuantity; i++)
        {
            GameObject enemyToSpawn = SelectEnemyToSpawn();
            Vector3 spawnPosition = SelectSpawnPoint();
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }

    }
}
