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
    private bool startWaveTimer = false;
    private float currentWaveTime;
    
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
        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (startWaveTimer)
        {
            currentWaveTime -= Time.deltaTime;
            CheckWaveTimer();
        }
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

    private void SpawnWave()
    {
        Debug.Log("Starting Wave " + currentWave);
        
        if (currentWaveStart) return;
        
        for (int i = 0; i < waveDetail[currentWave].enemieQuantity; i++)
        {
            GameObject enemyToSpawn = SelectEnemyToSpawn();
            Vector3 spawnPosition = SelectSpawnPoint();
            GameObject enemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            enemiesInGame.Add(enemy);
        }
        currentWaveTime = waveDetail[currentWave].waveMaxTime;
        currentWaveStart = true;
        startWaveTimer = true;
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
            StartWave();
        } 
    }

    private void CheckWaveConclusionOnTime()
    {
        if (startWaveTimer)
        {
            startWaveTimer = false;
            currentWaveStart = false;
            currentWave++;
            if (currentWave <= maxWaveNumbers)
            {
                SpawnWave();
            }
        }
    }

    private void StartWave()
    {
        currentWaveStart = false;
        currentWave++;
        if (currentWave <= maxWaveNumbers)
        {
            SpawnWave();
        }
        else
        {
            Debug.Log("Fim de sala");
        }
    }

    private void CheckWaveTimer()
    {
        if (currentWaveTime <= 0 && startWaveTimer)
        {
            CheckWaveConclusionOnTime();
        }
    }
}
