using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

public class WaveManager : MonoBehaviour, MMEventListener<MMLifeCycleEvent>
{

    [SerializeField]
    private WaveDetail[] waveDetail;

    [SerializeField]
    private GameObject [] spawnPoints;
    
    [SerializeField]
    private List<GameObject> enemiesInGame;

    private int currentWave = 0;
    private int maxWaveNumbers;

    private bool currentWaveStart = false;

    // Contar o timer de fim de wave
    //

    private void OnEnable()
    {
        this.MMEventStartListening<MMLifeCycleEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<MMLifeCycleEvent>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        maxWaveNumbers = waveDetail.Length - 1;
        enemiesInGame = new List<GameObject>();
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
        Debug.Log("Starting Wave " + currentWave);
        for (int i = 0; i < waveDetail[currentWave].enemieQuantity; i++)
        {
            GameObject enemyToSpawn = SelectEnemyToSpawn();
            Vector3 spawnPosition = SelectSpawnPoint();
            GameObject enemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            enemiesInGame.Add(enemy);
        }

        currentWaveStart = true;
    }

    public void OnMMEvent(MMLifeCycleEvent lifeCycleEvent)
    {
        if (lifeCycleEvent.MMLifeCycleEventType == MMLifeCycleEventTypes.Death)
        {
            RemoveEnemy(lifeCycleEvent.AffectedHealth.gameObject);
            CheckWaveConclusion();
        }
    }

    private void RemoveEnemy(GameObject enemy)
    {
        if (enemiesInGame.Contains(enemy))
        {
            enemiesInGame.Remove(enemy);
        }
    }

    private void CheckWaveConclusion()
    {
        if (enemiesInGame.Count == 0)
        {
            currentWaveStart = false;
            currentWave++;
            if (currentWave <= maxWaveNumbers)
            {
                SpawnwWave();
            }
            else
            {
                Debug.Log("Fim de sala");
            }
        }
    }
}
